using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class BadugiService : ServiceBase
    {
        Network.NetCore svr;
        public BadugiService(string[] args)
        {
            UnityCommon.Server launchMode = UnityCommon.Server.Login;
            ushort portnum = 0;

            // 서버 타입 설정
            switch (args[0])
            {
                case "Login":
                    launchMode = UnityCommon.Server.Login;
                    break;

                case "Lobby":
                    launchMode = UnityCommon.Server.Lobby;
                    break;

                case "Room":
                    launchMode = UnityCommon.Server.Room;
                    break;

                case "Master":
                    launchMode = UnityCommon.Server.Master;
                    break;

                case "Relay":
                    launchMode = UnityCommon.Server.Relay;
                    break;

                case "RelayLobby":
                    launchMode = UnityCommon.Server.RelayLobby;
                    break;
            }

            // 포트 설정
            portnum = ushort.Parse(args[1]);

            if (args.Count() > 2)
            {

            }
            int ID = 0;
            if (launchMode == UnityCommon.Server.Room
                || launchMode == UnityCommon.Server.Relay
                || launchMode == UnityCommon.Server.RelayLobby)
            {
                // 채널 설정
                if (args.Count() >= 3)
                {
                    int.TryParse(args[2], out ID);
                }
            }

            //Log._log.Info(args[0] + " Server On. Port:" + args[1]);

            Init(launchMode, portnum, ID);
        }

        string server_name = "Service";
        public void Init(UnityCommon.Server s, ushort portnum, int ID = 0)
        {
            server_name = string.Format("{0}-{1}", s, portnum);
            Log.Setup(server_name);
            Log._log.InfoFormat("Service Init. Server:{0}, portnum:{1}, ID:{2}", s, portnum, ID);

            InitializeComponent();

            switch (s)
            {
                case UnityCommon.Server.Login:
                    svr = new Network.LoginServer(this, s, portnum);
                    break;

                case UnityCommon.Server.Lobby:
                    svr = new Network.LobbyServer(this, s, portnum);
                    break;

                case UnityCommon.Server.Room:
                    svr = new Network.RoomServer(this, s, portnum, ID);
                    break;

                case UnityCommon.Server.Relay:
                    svr = new Network.RelayServer(this, s, portnum, ID);
                    break;

                case UnityCommon.Server.RelayLobby:
                    svr = new Network.RelayLobbyServer(this, s, portnum, ID);
                    break;

                case UnityCommon.Server.Master:
                    svr = new Network.MasterServer(this, s, portnum);
                    break;
            }

            this.ServiceName = string.Format("B Server : {0}", server_name);
        }

        internal void TestStartup(string[] args)
        {
            this.OnStart(args);
        }
        internal void TestStop()
        {
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                svr.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(server_name, "OnStart Exception : " + ex.ToString(), EventLogEntryType.Error);
                Stop();
            }

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            try
            {
                //EventLog.WriteEntry("MyService", "Service is going to stop because of ...", EventLogEntryType.Information);
                // Dispose all your objects here        
                svr.Stop();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(server_name, "OnStop Exception : " + ex.ToString(), EventLogEntryType.Error);
            }
            finally
            {
                GC.Collect();
                base.OnStop();
            }
        }
    }
}
