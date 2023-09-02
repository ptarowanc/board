using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Guid = System.Guid;
using ZNet;
using System.Threading;

namespace Server.Network
{
    public abstract partial class NetCore
    {
        internal object NetCoreLocker = new object();

        //internal Guid ProtocolVersion = new Guid("F637FD3E-D2BC-42DC-82CD-C8E4DFD710BD");
        //internal Guid ProtocolVersionMaster = new Guid("904DD35B-B8E4-40EA-AF36-15669DC5D0C3");
        internal string IPPublic;
        internal string IPLocal;
        internal ushort PortNum;
        internal int ServerID;

        public RemoteID ServerHostID;
        public UnityCommon.Server type { get; private set; }
        public string Name { get; private set; }
        public MatgoService serv;

        public bool isNetRun = true;
        Thread NetWorker; // Net 처리 스레드
        public System.Timers.Timer ServerTaskTimer;
        public ZNet.CoreServerNet NetServer;

        public SS.Proxy Proxy;
        public SS.Stub Stub;

        internal Random Rnd = new Random((int)DateTime.UtcNow.Ticks);

        public NetCore(MatgoService s, UnityCommon.Server t, ushort portnum, int serverID)
        {
            serv = s;
            type = t;
            Name = string.Format("{0}-{1}_{2}", t, portnum, serverID);
            IPPublic = Var.getRegistryValue("ListenIP");// Server.Properties.Settings.Default.ListenIp;
            IPLocal = Var.getRegistryValue("LocalIP");
            PortNum = portnum;
            ServerID = serverID;

            ServerTaskTimer = new System.Timers.Timer();
        }
        public void Start()
        {
            NewCore();

            Proxy = new SS.Proxy();
            Stub = new SS.Stub();

            NetServer.Attach(Proxy, Stub);

            StartOption sp;
            BeforeServerStart(out sp);

            ZNet.ResultInfo outResult = new ZNet.ResultInfo();
            if (NetServer.Start(sp, outResult))
            {
                //Log.logger.InfoFormat("{0} Start ok.", this.name);
            }
            else
            {
                //Log.logger.ErrorFormat("{0} Start error : {1}", this.name, outResult.msg);
            }

            AfterServerStart();


            NetWorker = new Thread(() =>
            {
                while (isNetRun)
                {
                    NetServer.NetLoop();
                    Thread.Sleep(1);
                }
            });
            NetWorker.Start();

            //using (var pool = new Pool(1))
            //{
            //    Action<int> loop = (index =>
            //    {
            //        while (true)
            //        {
            //            NetServer.NetLoop();
            //        }
            //    });

            //    pool.QueueTask(() => loop(1));
            //}

            ServerTaskTimer = new System.Timers.Timer();
            ServerTaskTimer.Interval = 1000; // 1 sec
            ServerTaskTimer.Elapsed += new System.Timers.ElapsedEventHandler(ServerTask);
            ServerTaskTimer.Start();
        }

        public virtual void Stop()
        {
            if (NetServer != null)
            {
                NetServer.Stop();
            }
            isNetRun = false;
            ServerTaskTimer.Stop();

            if (NetServer != null)
            {
                NetServer.Dispose();
            }
            //Dispose();
        }
        private void Dispose()
        {
            if (NetServer != null)
            {
                NetServer.Dispose();
            }
        }

        protected virtual void NewCore()
        {
            NetServer = new ZNet.CoreServerNet();
        }

        protected abstract void BeforeServerStart(out StartOption param);
        protected virtual void AfterServerStart()
        {

        }
        public virtual void ServerTask(object sender, ElapsedEventArgs e)
        {

        }
        public virtual void PrintLog(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }
    }
    public class CMessageT : ZNet.CMessage
    {
        public CMessageT(ZNet.ArrByte ab)
        {
            int temp;
            this.Write(ab);
            this.ResetPosition();
            Marshaler.Read(this, out temp);
            Marshaler.Read(this, out temp);
            Marshaler.Read(this, out temp);
        }
    }
    public class ServerInfo
    {
        public string ServerName;
        public ServerType ServerType;
        public RemoteID ServerHostID;
        public NetAddress ServerAddrPort; // 서버간 통신주소
        public string PublicIP; // 클라이언트 통신 IP
        public ushort PublicPort; // 클라이언트 통신 Port
        public int ServerID; // 채널 ID, 릴레이 ID
    }

    public sealed class Pool : IDisposable
    {
        public Pool(int size)
        {
            this._workers = new LinkedList<Thread>();

            for (var i = 0; i < size; ++i)
            {
                var worker = new Thread(this.Worker)
                {
                    Name = string.Concat("Worker ", i)
                };

                worker.Start();
                this._workers.AddLast(worker);
            }
        }

        public void Dispose()
        {
            var waitForThreads = false;
            lock (this._tasks)
            {
                if (!this._disposed)
                {
                    GC.SuppressFinalize(this);

                    this._disallowAdd = true; // wait for all tasks to finish processing while not allowing any more new tasks

                    while (this._tasks.Count > 0)
                    {
                        Monitor.Wait(this._tasks);
                    }

                    this._disposed = true;
                    Monitor.PulseAll(this._tasks); // wake all workers (none of them will be active at this point; disposed flag will cause then to finish so that we can join them)
                    waitForThreads = true;
                }
            }
            if (waitForThreads)
            {
                foreach (var worker in this._workers)
                {
                    worker.Join();
                }
            }
        }

        public void QueueTask(Action task)
        {
            lock (this._tasks)
            {
                if (this._disallowAdd)
                {
                    throw new InvalidOperationException("This Pool instance is in the process of being disposed, can't add anymore");
                }

                if (this._disposed)
                {
                    throw new ObjectDisposedException("This Pool instance has already been disposed");
                }

                this._tasks.AddLast(task);
                Monitor.PulseAll(this._tasks); // pulse because tasks count changed
            }
        }

        private void Worker()
        {
            Action task = null;

            while (true) // loop until threadpool is disposed
            {
                lock (this._tasks) // finding a task needs to be atomic
                {
                    while (true) // wait for our turn in _workers queue and an available task
                    {
                        if (this._disposed)
                        {
                            return;
                        }
                        if (null != this._workers.First && object.ReferenceEquals(Thread.CurrentThread, this._workers.First.Value) && this._tasks.Count > 0) // we can only claim a task if its our turn (this worker thread is the first entry in _worker queue) and there is a task available
                        {
                            task = this._tasks.First.Value;
                            this._tasks.RemoveFirst();
                            this._workers.RemoveFirst();
                            Monitor.PulseAll(this._tasks); // pulse because current (First) worker changed (so that next available sleeping worker will pick up its task)
                            break; // we found a task to process, break out from the above 'while (true)' loop
                        }
                        Monitor.Wait(this._tasks); // go to sleep, either not our turn or no task to process
                    }
                }

                task(); // process the found task

                lock (this._tasks)
                {
                    this._workers.AddLast(Thread.CurrentThread);
                }
                task = null;
            }
        }

        private readonly LinkedList<Thread> _workers; // queue of worker threads ready to process actions
        private readonly LinkedList<Action> _tasks = new LinkedList<Action>(); // actions to be processed by worker threads
        private bool _disallowAdd; // set to true when disposing queue but there are still tasks pending
        private bool _disposed; // set to true when disposing queue and no more tasks are pending
    }
}
