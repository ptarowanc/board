using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Server.Base
{
    public abstract partial class BaseServer
    {
        UnityCommon.Server type;
        string name;
        ZNet.NetAddress listenAddr = new ZNet.NetAddress();
        public FormServer form;
        public dynamic db;
        public ZNet.CoreServerNet m_Core;

        public Rmi.Proxy proxy;
        public Rmi.Stub stub;

        public System.Timers.Timer NetLoopTimer;
        public System.Timers.Timer ServerTaskTimer;

        public string Name
        {
            get { return name; }
        }
        public UnityCommon.Server Type
        {
            get { return type; }
        }

        public ZNet.NetAddress ListenAddr
        {
            get { return listenAddr; }
        }

        public BaseServer(FormServer f, UnityCommon.Server t, int portnum)
        {
            this.form = f;
            this.type = t;
            this.name = string.Format("{0}-{1}", t, portnum);

            listenAddr.m_ip = Server.Properties.Settings.Default.ListenIp;
            listenAddr.m_port = (ushort)portnum;
        }
        ~BaseServer()
        {
        }

        protected virtual void NewCore()
        {
            m_Core = new ZNet.CoreServerNet();
        }

        protected abstract void BeforeServerStart(out ZNet.StartOption param);

        protected virtual void AfterServerStart()
        {

        }

        public virtual void NetLoop(object sender, ElapsedEventArgs e)
        {

        }
        public virtual void ServerTask(object sender, ElapsedEventArgs e)
        {

        }
        public virtual void PrintLog(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }

        public void OnServerStart()
        {
            NewCore();

            proxy = new Rmi.Proxy();
            stub = new Rmi.Stub();

            m_Core.Attach(proxy, stub);

            ZNet.StartOption param;
            BeforeServerStart(out param);

            ZNet.ResultInfo outResult = new ZNet.ResultInfo();
            if(m_Core.Start(param, outResult))
            {
                //Log.logger.InfoFormat("{0} Start ok.", this.name);
            }
            else
            {
                //Log.logger.ErrorFormat("{0} Start error : {1}", this.name, outResult.msg);
            }

            AfterServerStart();

            NetLoopTimer = new System.Timers.Timer();
            NetLoopTimer.Interval = 1; // 1/1000 sec
            NetLoopTimer.Elapsed += new System.Timers.ElapsedEventHandler(NetLoop);
            NetLoopTimer.Start();

            ServerTaskTimer = new System.Timers.Timer();
            ServerTaskTimer.Interval = 1000; // 1 sec
            ServerTaskTimer.Elapsed += new System.Timers.ElapsedEventHandler(ServerTask);
            ServerTaskTimer.Start();
        }
        public void OnServerStop()
        {
            if(m_Core != null)
            {
                m_Core.Stop();
            }
            NetLoopTimer.Stop();
            ServerTaskTimer.Stop();
        }
        public void Dispose()
        {
            if (m_Core != null)
            {
                m_Core.Dispose();
            }
        }
    }
}
