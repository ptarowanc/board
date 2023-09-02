using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZNet;

namespace Server.Network
{
    class MasterServer : NetCore
    {
        internal object MasterServerLocker = new object();

        bool ShutDown = false;
        bool ShutDownReady = false;
        int tick = 0;

        // 그룹
        RemoteID GroupHostID = RemoteID.Remote_None;
        List<ServerInfo> ServerInfos = new List<ServerInfo>();

        public MasterServer(BadugiService f, UnityCommon.Server t, ushort portnum) : base(f, t, portnum, 0)
        {
        }

        protected override void NewCore()
        {
            //인자값이 마스터 서버이므로 재정의
            NetServer = new ZNet.CoreServerNet(true);
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            param = new StartOption();

            param.m_IpAddressListen = this.IPPublic;
            ushort masterPort = ushort.Parse(Var.getRegistryValue("MasterPort"));
            param.m_PortListen = masterPort;
            param.m_MaxConnectionCount = 5000;

            NetServer.SetKeepAliveOption(60);

            NetServer.message_handler = message_handler;
            NetServer.master_server_join_handler = master_server_join_handler; // ☆
            NetServer.server_refresh_handler = server_refresh_handler; // ☆
            NetServer.server_master_leave_handler = server_master_leave_handler; // ☆
            NetServer.server_master_join_handler = server_master_join_handler; // ☆
            NetServer.server_leave_handler = server_leave_handler; // ☆
            NetServer.server_join_handler = server_join_handler; // ☆
            NetServer.group_destroy_handler = group_destroy_handler;
            NetServer.group_memberout_handler = group_memberout_handler;
            NetServer.update_event_handler = update_event_handler;
            NetServer.recovery_start_handler = recovery_start_handler;
            NetServer.recovery_end_handler = recovery_end_handler;
            NetServer.recovery_info_handler = recovery_info_handler;
            NetServer.move_server_failed_handler = move_server_failed_handler; // ★
            NetServer.move_server_param_handler = move_server_param_handler; // ★
            NetServer.move_server_start_handler = move_server_start_handler; // ★
            NetServer.limit_connection_handler = limit_connection_handler;
            NetServer.client_leave_handler = client_leave_handler; // ★
            NetServer.client_join_handler = client_join_handler; // ★
            NetServer.client_disconnect_handler = client_disconnect_handler;
            NetServer.client_connect_handler = client_connect_handler;
            NetServer.exception_handler = exception_handler;

            Log._log.InfoFormat("StartOption. m_IpAddressListen:{0}, masterPort:{1}", param.m_IpAddressListen, masterPort);

        }

        protected override void AfterServerStart()
        {
            base.AfterServerStart();
        }
        public override void Stop()
        {
            Log._log.Warn("서버 일괄 종료.");

            ShutDownServers();

            for (int t = 0; t < 100; ++t)
            {
                if (ShutDownReady == false)
                {
                    int memberCount = NetServer.GetCountClient();
                    string log = "서버 종료중. 남은 서버 수:" + memberCount;
                    Log._log.InfoFormat(log);

                    if (memberCount == 0)
                    {
                        ShutDownReady = true;
                    }
                    else
                    {
                        foreach (var servers in ServerInfos)
                        {
                            Log._log.InfoFormat("서버 종료중. member:{0} ", servers.ServerName);
                        }
                    }
                }
                else
                {
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }

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

            Log._log.InfoFormat("서버 종료");
        }

        #region Master Server
        public void ShutDownServers()
        {
            lock (MasterServerLocker)
            {
                foreach (var servers in ServerInfos)
                {
                    Proxy.MasterAllShutdown(servers.ServerHostID, CPackOption.Basic, "shutdown");
                    Log._log.InfoFormat("서버 종료 요청. member:{0} ", servers.ServerName);
                }
            }
        }
        #endregion

        #region CoreHandler
        void message_handler(ResultInfo resultInfo)
        {
            switch (resultInfo.m_Level)
            {
                case IResultLevel.IMsg:
                    Log._log.InfoFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
                case IResultLevel.IWrn:
                    Log._log.WarnFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
                case IResultLevel.IErr:
                    Log._log.ErrorFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
                case IResultLevel.ICri:
                    Log._log.FatalFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
            }
        }
        void master_server_join_handler(RemoteID remote, string description, int type, NetAddress addr)
        {
            lock (MasterServerLocker)
            {
                // 서버 이름 분리
                var tDesc = description.Split('_');
                int tServerID = 0;
                if (tDesc.Count() >= 2)
                {
                    //description = tDesc[0];
                    if (int.TryParse(tDesc[1], out tServerID) == false)
                    {
                        tServerID = 0;
                    }
                }

                ServerInfo serverNew = new ServerInfo();
                serverNew.ServerHostID = remote;
                serverNew.ServerType = (ServerType)type;
                serverNew.ServerName = description;
                serverNew.ServerAddrPort = addr;
                serverNew.PublicIP = addr.m_ip;
                serverNew.PublicPort = addr.m_port;
                serverNew.ServerID = tServerID;

                ServerInfos.Add(serverNew);

                ServerInfo serverJoin = serverNew;

                if (serverJoin != null)
                {
                    // 그룹이 없으면 생성
                    if (GroupHostID == RemoteID.Remote_None)
                    {
                        //NetServer.CreateGroup(out GroupHostID, serverJoin.ServerHostID);
                    }
                    else
                    {
                        // 그룹에 추가
                        int members;
                        //NetServer.JoinGroup(GroupHostID, serverJoin.ServerHostID, out members);
                    }

                    // 기존 서버에 입장한 서버 정보 전달
                    {
                        foreach (var server in ServerInfos)
                        {
                            CMessage msg = new CMessage();
                            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.MasterNotifyP2PServerInfo;
                            msg.WriteStart(msgID, CPackOption.Basic, 0, true);

                            msg.Write(serverJoin.ServerName);
                            msg.Write((int)serverJoin.ServerType);
                            msg.Write(serverJoin.ServerHostID);
                            msg.Write(serverJoin.ServerAddrPort.addr);
                            msg.Write(serverJoin.ServerAddrPort.port);
                            msg.Write(serverJoin.PublicIP);
                            msg.Write(serverJoin.PublicPort);
                            msg.Write(serverJoin.ServerID);

                            Proxy.PacketSend(server.ServerHostID, CPackOption.Basic, msg);
                            //Proxy.MasterNotifyP2PServerInfo(server.ServerHostID, CPackOption.Basic, msg.m_array);
                        }
                    }
                    // 입장한 서버에 기존 서버 정보 전달
                    {
                        foreach (var server in ServerInfos)
                        {
                            CMessage msg = new CMessage();
                            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.MasterNotifyP2PServerInfo;
                            msg.WriteStart(msgID, CPackOption.Basic, 0, true);

                            msg.Write(server.ServerName);
                            msg.Write((int)server.ServerType);
                            msg.Write(server.ServerHostID);
                            msg.Write(server.ServerAddrPort.addr);
                            msg.Write(server.ServerAddrPort.port);
                            msg.Write(server.PublicIP);
                            msg.Write(server.PublicPort);
                            msg.Write(server.ServerID);

                            Proxy.PacketSend(serverJoin.ServerHostID, CPackOption.Basic, msg);
                        }
                    }
                }
            }

            Log._log.InfoFormat("master_server_join_handler remoteID({0}) {1} type({2})", remote, description, type);
        }
        void server_refresh_handler(MasterInfo master_info)
        {
            //Log._log.InfoFormat("server_refresh_handler m_ip1:{0}, m_port1:{1}, m_ip2:{2}, m_port2:{3}, m_Clients:{4}, m_Description:{5}, m_remote:{6}, m_ServerType:{7}",
            //    master_info.m_Addr.m_ip, master_info.m_Addr.m_port,
            //    master_info.m_AddrForClient.m_ip, master_info.m_AddrForClient.m_port,
            //    master_info.m_Clients, master_info.m_Description, master_info.m_remote, master_info.m_ServerType);
        }
        void server_master_leave_handler()
        {
            Log._log.InfoFormat("server_master_leave_handler");
        }
        void server_master_join_handler(RemoteID remote, RemoteID myRemoteID)
        {
            Log._log.InfoFormat("server_master_join_handler. remote:{0}, myRemoteID:{1}", remote, myRemoteID);
        }
        void server_leave_handler(RemoteID remote, NetAddress addr)
        {
            lock (MasterServerLocker)
            {
                var itemToRemove = ServerInfos.SingleOrDefault(r => r.ServerHostID == remote);
                if (itemToRemove != null)
                    ServerInfos.Remove(itemToRemove);
            }

            Log._log.InfoFormat("server_leave_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void server_join_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("server_join_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void group_destroy_handler(RemoteID groupID)
        {
            Log._log.InfoFormat("group_destroy_handler. groupID:{0}", groupID);
        }
        void group_memberout_handler(RemoteID groupID, RemoteID memberID, int Members)
        {
            Log._log.InfoFormat("group_memberout_handler. groupID:{0}, memberID:{1}, Members:{2}", groupID, memberID, Members);
        }
        void update_event_handler()
        {
            //Log._log.InfoFormat("update_event_handler.");
        }
        void recovery_start_handler(RemoteID remote)
        {
            Log._log.InfoFormat("recovery_start_handler. remote:{0}", remote);
        }
        void recovery_end_handler(RemoteID remote, NetAddress addrNew, bool bTimeOut)
        {
            Log._log.InfoFormat("recovery_end_handler. remote:{0}, m_ip:{1}, m_port:{2}, bTimeOut:{3}", remote, addrNew.m_ip, addrNew.m_port, bTimeOut);
        }
        void recovery_info_handler(RemoteID remoteNew, RemoteID remoteTo)
        {
            Log._log.InfoFormat("recovery_info_handler. remoteNew:{0}, remoteTo:{1}", remoteNew, remoteTo);
        }
        void move_server_failed_handler(ArrByte move_server)
        {
            Log._log.InfoFormat("move_server_failed_handler.");
        }
        bool move_server_param_handler(ArrByte move_param, int count_idx)
        {
            Log._log.InfoFormat("move_server_param_handler. count_idx:{0}", count_idx);
            return true;
        }
        void move_server_start_handler(RemoteID remote, out ArrByte userdata)
        {
            userdata = null;

            Log._log.InfoFormat("move_server_start_handler. remote:{0}", remote);
        }
        void limit_connection_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("limit_connection_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void client_leave_handler(RemoteID remote, bool bMoveServer)
        {
            Log._log.InfoFormat("client_leave_handler. remote:{0}, bMoveServer:{1}", remote, bMoveServer);
        }
        void client_join_handler(RemoteID remote, NetAddress addr, ArrByte move_server, ArrByte move_param)
        {
            Log._log.InfoFormat("client_join_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void client_leave_handler()
        {
            Log._log.InfoFormat("client_leave_handler.");
        }
        void client_disconnect_handler(RemoteID remote)
        {
            var itemToRemove = ServerInfos.SingleOrDefault(r => r.ServerHostID == remote);
            if (itemToRemove != null)
                ServerInfos.Remove(itemToRemove);

            Log._log.InfoFormat("client_disconnect_handler. remote:{0}", remote);
        }
        void client_connect_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("client_connect_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void exception_handler(Exception e)
        {
            Log._log.ErrorFormat("exception_handler. e:{0}", e.ToString());
        }
        #endregion
    }
}
