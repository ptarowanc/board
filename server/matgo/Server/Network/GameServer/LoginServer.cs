using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZNet;

namespace Server.Network
{
    class LoginServer : GameServer
    {
        internal object LoginServerLocker = new object();

        // 클라이언트 목록
        public ConcurrentDictionary<RemoteID, CPlayer> RemoteClients = new ConcurrentDictionary<RemoteID, CPlayer>();
        // 로비리스트 목록
        public ConcurrentDictionary<RemoteID, string> lobby_list = new ConcurrentDictionary<RemoteID, string>();
        // 릴레이리스트 목록
        public ConcurrentDictionary<int, RelayServerInfo> Relay_list = new ConcurrentDictionary<int, RelayServerInfo>();
        // 접속자 수 한도
        int UserLimit = 3000;

        //long ClientTag = 1;

        public LoginServer(MatgoService s, UnityCommon.Server t, ushort portnum) : base(s, t, portnum, 0)
        {
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            base.BeforeServerStart(out param);
            param.m_UpdateTimeMs = 1000;

            // CoreHandler
            {
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
            }

            // Master Stub
            {
                Stub.MasterAllShutdown = MasterAllShutdown;
                Stub.MasterNotifyP2PServerInfo = MasterNotifyP2PServerInfo;
            }

            // Server Stub
            {
                // 로비 서버
                Stub.LobbyLoginKickUser = LobbyLoginKickUser;
            }

            // Client Stub
            {
                Stub.ServerMoveFailure = ServerMoveFailure;

                Stub.RequestLogin = RequestLogin;
                Stub.RequestLoginKey = RequestLoginKey;
                Stub.RequestGoLobby = RequestGoLobby;
                Stub.RequestGameOptions = RequestGameOptions;
            }
        }

        #region Login Server
        public override void ServerTask(object sender, ElapsedEventArgs e_)
        {
            ++this.tick;

            if (this.tick % 13 == 0)
            {
                if (this.ShutDown)
                {
                    // 세션 없으면 프로그램 종료
                    if (RemoteClients.Count == 0 || this.CountDown.AddMinutes(1) < DateTime.Now)
                    {
                        Log._log.Info("서버 종료. ShutDown");
                        //System.Windows.Forms.Application.Exit();
                        System.Environment.Exit(0);
                        return;
                    }
                    else if (this.CountDown < DateTime.Now)
                    {
                        // 모든 세션 종료
                        foreach (var client in RemoteClients)
                        {
                            NetServer.CloseConnection(client.Key);
                        }
                    }
                    Log._log.Info("세션 종료중. 남은 세션 수:" + RemoteClients.Count);
                }
            }
        }
        void ClientLeave(RemoteID remote, bool bMoveServer)
        {
            lock (GameServerLocker)
            {
                CPlayer rc;
                if (RemoteClients.TryRemove(remote, out rc))
                {
                    if (bMoveServer == false)
                    {
                        DB_User_Logout(rc.data.ID);
                        DBLog(rc.data.ID, 0, 0, LOG_TYPE.로그아웃, "");
                        //Log._log.InfoFormat("ClientLeave Success. hostID:{0}, leaveType:{1}", remote, leaveType);
                    }
                }
                else
                {
                    Log._log.WarnFormat("ClientLeave Unknown RemoteClients. hostID:{0}", remote);
                }
            }
        }
        private void ClientDisconect(RemoteID remote, string reasone)
        {
            NetServer.CloseConnection(remote);
            //ClientLeave(remote, ErrorType.DisconnectFromRemote, null);
            Log._log.WarnFormat("ClientDisconect. remote:{0}, reasone:{1}", remote, reasone);
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
            // Master Server Info
            //ServerInfo serverNew = new ServerInfo();
            //serverNew.ServerHostID = remote;
            //serverNew.ServerName = description;
            //serverNew.ServerType = (ServerType)type;
            //serverNew.ServerAddrPort = addr;
            //serverNew.PublicIP = addr.m_ip;
            //serverNew.PublicPort = addr.m_port;
            //serverNew.ServerID = ServerID;

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
            MasterHostID = remote;
            Log._log.InfoFormat("server_master_join_handler. remote:{0}, myRemoteID:{1}", remote, myRemoteID);
        }
        void server_leave_handler(RemoteID remote, NetAddress addr)
        {
            //if (ShutDown) return; // 서버 종료중이면 서버퇴장 대응 없음

            lock (GameServerLocker)
            {
                var itemToRemove = ServerInfoList.Values.SingleOrDefault(r => r.ServerHostID == remote);
                if (itemToRemove != null)
                {
                    ServerInfo remove;
                    ServerInfoList.TryRemove(itemToRemove.ServerHostID, out remove);

                    if (itemToRemove.ServerType == ServerType.Lobby)
                    {
                        string serverLobbyRemove;
                        lobby_list.TryRemove(itemToRemove.ServerHostID, out serverLobbyRemove);
                    }
                    else if (itemToRemove.ServerType == ServerType.RelayLobby)
                    {
                        int delID = 0;
                        foreach (var del in Relay_list)
                        {
                            if (del.Value.remote == itemToRemove.ServerHostID)
                            {
                                delID = del.Key;
                                break;
                            }
                        }

                        if (delID != 0)
                        {
                            RelayServerInfo serverRelayRemove;
                            Relay_list.TryRemove(delID, out serverRelayRemove);
                        }
                    }
                }
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
            CPlayer rc;
            Common.Common.ServerMoveComplete(move_server, out rc);

            if (rc != null)
            {
                DB_User_Logout(rc.data.ID);
            }
            Log._log.InfoFormat("move_server_failed_handler.");
        }
        bool move_server_param_handler(ArrByte move_param, int count_idx)
        {
            //Log._log.InfoFormat("move_server_param_handler. count_idx:{0}", count_idx);
            return true;
        }
        void move_server_start_handler(RemoteID remote, out ArrByte userdata)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false)
            {
                userdata = null;
                Log._log.ErrorFormat("move_server_start_handler. Error. remote:{0}", remote);
                return;
            }

            // 여기서는 이동할 서버로 동기화 시킬 유저 데이터를 구성하여 buffer에 넣어둔다 -> 완료서버에서 해당 데이터를 그대로 받게된다
            Common.Common.ServerMoveStart(rc, out userdata);

            //Log._log.InfoFormat("move_server_start_handler. remote:{0}", remote);
        }
        void limit_connection_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("limit_connection_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void client_leave_handler(RemoteID remote, bool bMoveServer)
        {
            ClientLeave(remote, bMoveServer);
            //Log._log.InfoFormat("client_leave_handler. remote:{0}, bMoveServer:{1}", remote, bMoveServer);
        }
        void client_join_handler(RemoteID remote, NetAddress addr, ArrByte move_server, ArrByte move_param)
        {
            string clientIP = addr.m_ip;

            try
            {
                if (move_server.Count > 0)
                {
                    // 서버 이동으로 입장한 경우 데이터 불러오기
                    Common.MoveParam param;
                    CPlayer rc;
                    Common.Common.ServerMoveParamRead(move_param, out param, out rc);

                    rc.m_ip = clientIP;

                    RemoteClients.TryAdd(remote, rc);

                    //Log._log.InfoFormat("ClientJoinHandler Success. IP:{0}, Player{1}", rc.m_ip, rc.data.userID);
                }
                else
                {
                    CPlayer rc = new CPlayer();
                    rc.data.ID = 0;
                    rc.data.userID = string.Empty;
                    rc.m_ip = clientIP;

                    RemoteClients.TryAdd(remote, rc);
                    //Log._log.InfoFormat("ClientJoinHandler Success.(No moveData) IP:{0}", rc.m_ip);
                }

                //입장시 로비 리스트 보냄
                lock (GameServerLocker)
                {
                    //ZNet.MasterInfo[] svr_array;
                    //NetServer.GetServerList((int)ServerType.Lobby, out svr_array);
                    //if (svr_array == null)
                    //{
                    //    Log._log.ErrorFormat("ClientJoinHandler Cancel.(No lobbyList) IP:{0}", clientIP);
                    //    ClientDisconect(remote, "No Lobby List");
                    //    return;
                    //}

                    //foreach (var obj in svr_array)
                    //{
                    //    Proxy.NotifyLobbyList(remote, CPackOption.Basic, lobbyList);
                    //}
                    var lobbyList = this.lobby_list.Values.ToList();
                    if (lobbyList.Count > 0)
                    {
                        Proxy.NotifyLobbyList(remote, CPackOption.Basic, lobbyList);
                    }
                    else
                    {
                        Log._log.ErrorFormat("ClientJoinHandler Cancel.(No lobbyList) IP:{0}", clientIP);
                        ClientDisconect(remote, "No Lobby List");
                    }
                }
            }
            catch (Exception e)
            {
                ClientDisconect(remote, "Client Join Failure");
                Log._log.ErrorFormat("ClientJoinHandler Exciption. IP:{0}, e:{1}", clientIP, e.ToString());
            }
            //Log._log.InfoFormat("client_join_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void client_disconnect_handler(RemoteID remote)
        {
            //Log._log.InfoFormat("client_disconnect_handler. remote:{0}", remote);
        }
        void client_connect_handler(RemoteID remote, NetAddress addr)
        {
            //Log._log.InfoFormat("client_connect_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void exception_handler(Exception e)
        {
            Log._log.ErrorFormat("exception_handler. e:{0}", e.ToString());
        }
        #endregion

        #region Master Stub Handler
        bool MasterAllShutdown(RemoteID remote, CPackOption rmiContext, string msg)
        {
            bool result = true;

            ServerMaintenance = true;
            ServerMsg = "※안내※\n서버 점검중입니다.";
            ShutDown = true;
            CountDown = DateTime.Now.AddMinutes(1);

            Log._log.Info("서버종료 요청 받음.");

            return result;
        }
        bool MasterNotifyP2PServerInfo(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            lock (GameServerLocker)
            {
                CMessage msg = data;
                ServerInfo serverInfo = new ServerInfo();
                msg.Read(out serverInfo.ServerName);
                int serverType;
                msg.Read(out serverType);
                serverInfo.ServerType = (ServerType)serverType;
                msg.Read(out serverInfo.ServerHostID);
                string serverAddrIP;
                msg.Read(out serverAddrIP);
                ushort serverAddrPort;
                msg.Read(out serverAddrPort);
                msg.Read(out serverInfo.PublicIP);
                msg.Read(out serverInfo.PublicPort);
                msg.Read(out serverInfo.ServerID);

                serverInfo.ServerAddrPort = new NetAddress();
                serverInfo.ServerAddrPort.m_ip = serverAddrIP;
                serverInfo.ServerAddrPort.m_port = serverAddrPort;

                if (serverInfo.ServerName == this.Name)
                {
                    this.ServerHostID = serverInfo.ServerHostID;
                }
                else
                {
                    if (ServerInfoList.TryAdd(serverInfo.ServerHostID, serverInfo))
                    {
                        // 로비일 경우 로비 목록에 추가
                        if (serverInfo.ServerType == ServerType.Lobby)
                        {
                            lobby_list.TryAdd(serverInfo.ServerHostID, serverInfo.ServerName);
                        }
                        // 로비일 경우 로비 목록에 추가
                        else if (serverInfo.ServerType == ServerType.RelayLobby)
                        {
                            RelayServerInfo relayInfo;
                            relayInfo.remote = serverInfo.ServerHostID;
                            relayInfo.RelayID = serverInfo.ServerID;
                            relayInfo.Addr = serverInfo.ServerAddrPort;
                            Relay_list.TryAdd(serverInfo.ServerID, relayInfo);
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Server Stub Handler
        bool LobbyLoginKickUser(RemoteID remote, CPackOption rmiContext, int userID)
        {
            foreach (var user in RemoteClients)
            {
                if (user.Value.data.ID == userID)
                {
                    DBLog(userID, 0, 0, LOG_TYPE.연결끊김, "운영툴 강제퇴장");
                    ClientDisconect(user.Key, "LobbyLoginKickUser");
                    return true;
                }
            }
            return true;
        }
        #endregion

        #region Client Stub Handler
        bool ServerMoveFailure(RemoteID remote, CPackOption rmiContext)
        {
            // 재시도하도록 안내하거나 연결 끊음
            ClientDisconect(remote, "ServerMoveFailure");

            return true;
        }
        bool RequestLogin(RemoteID remote, CPackOption rmiContext, string id, string pass)
        {
            // 유효한 유저인지 확인
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false) return false;
            string ip = rc.m_ip;

            Task.Run(async () =>
            {
                UserData dummy = new UserData();

                int result = await Task.Run(() => TryLogin(ip, id, pass, ref dummy, false));

                // 상단의 DB Task가 완료되는 시점이므로 다시 유저가 유효한 상태인지 확인합니다
                if (RemoteClients.TryGetValue(remote, out rc) == false)
                {
                    DB_User_Logout(dummy.ID, 2);
                    Log._log.WarnFormat("RequestLogin Cancel. remote:{0}, ID:{1}, IP:{2}", remote, id, ip);
                    return;
                }

                // 인증 성공
                if (result == 1)
                {
                    rc.data = dummy;
                    CMessage Msg = new CMessage();
                    ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.ResponseGameOptions;
                    Msg.WriteStart(msgID, CPackOption.Basic, 0, true);
                    Msg.Write(dummy.Option1);
                    //msg.Write(dummy.Option2);
                    Proxy.PacketSend(remote, CPackOption.Basic, Msg);

                    Proxy.ResponseLogin(remote, CPackOption.Encrypt, true, "");
                    //Log._log.InfoFormat("RequestLogin Success. remote:{0}, ID:{1}, IP:{2}", remote, id, ip);
                }
                else
                {
                    string ResultType;
                    if (result == 2) // 중복 로그인
                        ResultType = "※안내※\n이미 접속중인 아이디입니다.";
                    else if (result == 5) // 자동치기
                        ResultType = "※안내※\n이전 게임에서 퇴장하여 자동치기가 진행중입니다.\n잠시후 다시 시도하세요.";
                    else if (result == 6) // 차단된 IP
                        ResultType = "※안내※\n관리자에게 문의하세요.\ncode: 01";
                    else if (result == 7) // 차단된 회원
                        ResultType = "※안내※\n관리자에게 문의하세요.\ncode: 02";
                    else if (result == 8) // 서버 점검중
                        ResultType = "※안내※\n서버 점검중 입니다.";
                    else if (result == 9) // 접속인원 수 제한
                        ResultType = "※안내※\n접속 인원이 초과했습니다.\n잠시후 다시 시도해 주시기 바랍니다.";
                    else if (result == 10) // 이용제한 계정
                        ResultType = "※안내※\n이용제한 자가설정된 아이디입니다.\n홈페이지에서 이용제한 기간을 확인하세요.";
                    else // 입력정보 오류, 접속 오류
                        ResultType = "※안내※\n아이디 또는 비밀번호를 다시 확인해주세요.";

                    // 인증 실패를 알려줌
                    Proxy.ResponseLogin(remote, CPackOption.Encrypt, false, ResultType);
                    Log._log.WarnFormat("RequestLogin Deny. remote:{0}, ID:{1}, IP:{2}", remote, id, ip);
                }
                return;
            });

            return true;
        }
        bool RequestLoginKey(RemoteID remote, CPackOption rmiContext, string id, string key)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false) return false;
            string ip = rc.m_ip;

            Task.Run(async () =>
            {
                UserData dummy = new UserData();

                int result = await Task.Run(() => TryLogin(ip, id, key, ref dummy, true));

                // 상단의 DB Task가 완료되는 시점이므로 다시 유저가 유효한 상태인지 확인합니다
                if (RemoteClients.TryGetValue(remote, out rc) == false)
                {
                    DB_User_Logout(dummy.ID, 2);
                    Log._log.WarnFormat("RequestLoginKey Cancel. remote:{0}, ID:{1}, IP:{2}", remote, id, ip);
                    return;
                }

                // 인증 성공
                if (result == 1)
                {
                    rc.data = dummy;
                    CMessage Msg = new CMessage();
                    ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.ResponseGameOptions;
                    Msg.WriteStart(msgID, CPackOption.Basic, 0, true);
                    Msg.Write(dummy.Option1);
                    //msg.Write(dummy.Option2);
                    Proxy.PacketSend(remote, CPackOption.Basic, Msg);

                    Proxy.ResponseLoginKey(remote, CPackOption.Encrypt, true, "");
                    Log._log.InfoFormat("RequestLoginKey Success. remote:{0}, ID:{1}, IP:{2}", remote, id, ip);
                }
                else
                {
                    string ResultType;
                    if (result == 2) // 중복 로그인
                        ResultType = "※안내※\n이미 접속중인 아이디입니다.";
                    else if (result == 5) // 자동치기
                        ResultType = "※안내※\n이전 게임에서 퇴장하여 자동치기가 진행중입니다.\n잠시후 다시 시도하세요.";
                    else if (result == 6) // 차단된 IP
                        ResultType = "※안내※\n관리자에게 문의하세요.\ncode: 01";
                    else if (result == 7) // 차단된 회원
                        ResultType = "※안내※\n관리자에게 문의하세요.\ncode: 02";
                    else if (result == 8) // 서버 점검중
                        ResultType = "※안내※\n서버 점검중 입니다.";
                    else if (result == 9) // 접속인원 수 제한
                        ResultType = "※안내※\n접속 인원이 초과했습니다.\n잠시후 다시 시도해 주시기 바랍니다.";
                    else if (result == 10) // 이용제한 계정
                        ResultType = "※안내※\n이용제한 자가설정된 아이디입니다.";
                    else // 입력정보 오류, 접속 오류
                        ResultType = "※안내※\n아이디 또는 비밀번호를 다시 확인해주세요.";

                    // 인증 실패를 알려줌
                    Proxy.ResponseLoginKey(remote, CPackOption.Encrypt, false, ResultType);
                    Log._log.WarnFormat("RequestLoginKey Deny. remote:{0}, ID:{1}, IP:{2}", remote, id, ip);
                }
                return;
            });

            return true;
        }
        int TryLogin(string ip, string id, string pass, ref UserData dummy, bool useKey)
        {
            if (ServerMaintenance == true)
            {
                return 8; // 서버 점검중
            }

            try
            {
                // 접속자 수 확인
                int Data_CurrentUser = db.GameCurrentUser.GetCount();
                if (Data_CurrentUser >= UserLimit)
                {
                    return 9; // 접속자 수 제한
                }

                dynamic Data_BlockIP = db.AdminBlockIP.FindAllByIP(ip).FirstOrDefault();
                if (Data_BlockIP != null && Data_BlockIP.Blocking > DateTime.Now)
                {
                    return 6; // 차단된 IP
                }

                dynamic Data_Player = db.Player.FindAllByUserID(id).FirstOrDefault();
                if (Data_Player == null)
                {
                    return 4; // 없는 계정
                }
                else if (Data_Player.Quit == true)
                {
                    return 4; // 탈퇴 계정
                }

                if (Data_Player.SelfRestrictedDate > DateTime.Now)
                {
                    return 10; // 이용제한 계정
                }

                if (useKey)
                {
                    dynamic Data_Password = db.PlayerPassword.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                    if (Data_Password != null && Data_Password.Password == pass)
                    {
                        // 인증에 사용된 키는 파기되어 재사용 불가능
                        db.PlayerPassword.UpdateByUserId(UserId: Data_Player.Id, Password: "");
                        db.LogUserLogin.Insert(UserId: Data_Player.Id, GameId: GameId, IP: ip, Pass: true);
                    }
                    else
                    {
                        db.LogUserLogin.Insert(UserId: Data_Player.Id, GameId: GameId, IP: ip, Pass: false);
                        return 3; // 비밀번호 틀림
                    }
                }
                else
                {
                    SHA1 sha = SHA1.Create();
                    string passSalt;
                    if (Data_Player.CreatedFrom.Contains("JUST") || Data_Player.CreatedFrom.Contains("BGC"))
                    {
                        passSalt = "";
                    }
                    else
                    {
                        passSalt = "YOLO";
                    }

                    if (Data_Player.Password != HexStringFromBytes(sha.ComputeHash(Encoding.UTF8.GetBytes(passSalt + pass))))
                    {
                        // 비밀번호 틀림
                        db.LogUserLogin.Insert(UserId: Data_Player.Id, GameId: GameId, IP: ip, Pass: false);
                        return 3;
                    }

                    db.LogUserLogin.Insert(UserId: Data_Player.Id, GameId: GameId, IP: ip, Pass: true);

                    // 로그인 키 생성
                    string key_ = HexStringFromBytes(sha.ComputeHash(Encoding.UTF8.GetBytes(id + DateTime.Now.ToString() + "vong")));

                    dynamic Data_Password = db.PlayerPassword.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                    if (Data_Password == null)
                    {
                        db.PlayerPassword.Insert(UserId: Data_Player.Id, Password: key_, CreatedOnUtc: DateTime.Now);
                    }
                    else
                    {
                        db.PlayerPassword.UpdateByUserId(UserId: Data_Player.Id, Password: key_, CreatedOnUtc: DateTime.Now);
                    }
                }

                dynamic Data_BlockUser = db.AdminBlockUser.FindAllByUserId(Data_Player.Id);
                if (Data_BlockUser != null && Data_BlockUser.ToList().Count != 0)
                {
                    foreach (var row in Data_BlockUser.ToList())
                    {
                        if (row.Blocking > DateTime.Now)
                        {
                            return 7; // 차단된 회원
                        }
                    }
                }

                dynamic Data_Current = db.GameCurrentUser.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                if (Data_Current != null)
                {
                    return 2; // 중복 접속
                }

                dynamic Data_CurrentDummy = db.GameCurrentDummy.FindAllByUserID(Data_Player.Id).FirstOrDefault();
                if (Data_CurrentDummy != null)
                {
                    return 5; // 자동치기 처리중
                }

                // 접속 정보 확인
                dummy.ID = Data_Player.Id;
                dummy.userID = Data_Player.UserID;
                dummy.nickName = Data_Player.NickName;
                dummy.member_point = Data_Player.Point;
                dummy.IPFree = Data_Player.IPFree;
                dummy.ShopFree = Data_Player.ShopFree;
                dummy.UserLevel = Data_Player.MemberLevel;

                // 매장 정보 확인
                if (Data_Player.ShopId != 0)
                {
                    dynamic Data_Shop = db.AdminUser.FindAllById(Data_Player.ShopId).FirstOrDefault();

                    dummy.shop_name = Data_Shop.Title;
                    if (Data_Shop.Type == 6 && Data_Shop.Id != 6 && Data_Player.FriendId == 0)
                    {
                        dummy.shopId = Data_Player.ShopId;
                    }
                    dummy.RelayID = Data_Shop.RelayID;
                }
                else
                {
                    dummy.shop_name = "";
                    dummy.shopId = 0;
                    dummy.RelayID = 1;
                }

                // 보유 아이템
                bool AvatarUsing = false;
                int DefaultAvatarId = 0;
                string DefaultAvatar = "";
                int DefaultAvatarVoice = 0;

                bool CardUsing = false;
                int DefaultCardId = 0;
                string DefaultCard = "";

                dynamic Data_Item = db.V_PlayerItemList.FindAllByUserId(dummy.ID);
                dummy.avatar = "";
                dummy.avatar_card = "";
                if (Data_Item == null || Data_Item.ToList().Count == 0)
                {
                    db.PlayerItemList.Insert(UserId: dummy.ID, ItemId: 10, Count: 1, Using: true);
                    db.PlayerItemList.Insert(UserId: dummy.ID, ItemId: 11, Count: 1, Using: false);
                    db.PlayerItemList.Insert(UserId: dummy.ID, ItemId: 22, Count: 1, Using: true);
                    db.PlayerItemList.Insert(UserId: dummy.ID, ItemId: 28, Count: 1, Using: true);
                    Data_Item = db.V_PlayerItemList.FindAllByUserId(dummy.ID);
                }
                foreach (var row in Data_Item.ToList())
                {
                    // 아이템 만료됐으면 기본으로 변경
                    switch (row.ptype)
                    {
                        case "avatar":
                            {
                                if (row.Using == false)
                                {
                                    if (AvatarUsing == false && row.value2 == 1)
                                    {
                                        DefaultAvatarId = row.Id;
                                        DefaultAvatar = row.string1;
                                        DefaultAvatarVoice = row.value1;
                                    }
                                    continue;
                                }

                                if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
                                {
                                    db.PlayerItemList.UpdateById(Id: row.Id, Using: false);
                                }
                                else
                                {
                                    dummy.avatar = row.string1;
                                    dummy.voice = row.value1;
                                    AvatarUsing = true;
                                }
                            }
                            break;
                        case "card":
                            {
                                if (row.Using == false)
                                {
                                    if (CardUsing == false && row.value2 == 1)
                                    {
                                        DefaultCardId = row.Id;
                                        DefaultCard = row.string1;
                                    }
                                    continue;
                                }

                                if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
                                {
                                    db.PlayerItemList.UpdateById(Id: row.Id, Using: false);
                                }
                                else
                                {
                                    dummy.avatar_card = row.string1;
                                    CardUsing = true;
                                }
                            }
                            break;
                    }
                }
                // 만료된 아이템은 기본 아이템으로 변경 착용
                if (DefaultAvatarId != 0 && AvatarUsing == false)
                {
                    db.PlayerItemList.UpdateById(Id: DefaultAvatarId, Using: true);
                    dummy.avatar = DefaultAvatar;
                    dummy.voice = DefaultAvatarVoice;
                }
                if (DefaultCardId != 0 && CardUsing == false)
                {
                    db.PlayerItemList.UpdateById(Id: DefaultCardId, Using: true);
                    dummy.avatar_card = DefaultCard;
                }

                dynamic Data_Money = db.PlayerGameMoney.FindAllByUserID(dummy.ID).FirstOrDefault();
                if (Data_Money == null)
                {
                    bool FullMember = false; // 정회원 여부
                    long GiveGameMoney = 0;
                    long GivePayMoney = 0;
                    dynamic Data_GiveMoney = db.GameGiveMoney.FindAllByMoneyType(1).FirstOrDefault(); // 무료머니
                    if (FullMember)
                        GiveGameMoney = Data_GiveMoney.FullMemberMoney;
                    else
                        GiveGameMoney = Data_GiveMoney.MemberMoney;
                    Data_GiveMoney = db.GameGiveMoney.FindAllByMoneyType(2).FirstOrDefault(); // 유료머니
                    if (FullMember)
                        GivePayMoney = Data_GiveMoney.FullMemberMoney;
                    else
                        GivePayMoney = Data_GiveMoney.MemberMoney;

                    db.PlayerGameMoney.Insert(UserId: dummy.ID, Cash: 0, GameMoney: GiveGameMoney, PayMoney: GivePayMoney);
                    Data_Money = db.PlayerGameMoney.FindAllByUserID(dummy.ID).FirstOrDefault();
                }
                dummy.cash = Data_Money.Cash;
                dummy.money_pay = (long)Data_Money.PayMoney;
                dummy.money_free = (long)Data_Money.GameMoney;

                //dynamic Data_Lotto = db.EventLotto.FindAllByUserID(dummy.ID);
                //dummy.charm = Data_Lotto.ToList().Count;
                dummy.charm = 0;

                dynamic Data_SafeBox = db.PlayerSafeBox.FindAllByUserID(dummy.ID).FirstOrDefault();
                if (Data_SafeBox == null)
                {
                    db.PlayerSafeBox.Insert(UserId: dummy.ID);
                    Data_SafeBox = db.PlayerSafeBox.FindAllByUserID(dummy.ID).FirstOrDefault();
                }
                dummy.bank_money_pay = (long)Data_SafeBox.SafeMoney2;
                dummy.bank_money_free = (long)Data_SafeBox.SafeMoney;

                dynamic Data_Matgo = db.PlayerMatgo.FindAllByUserID(dummy.ID).FirstOrDefault();
                if (Data_Matgo == null)
                {
                    db.PlayerMatgo.Insert(UserId: dummy.ID);
                    Data_Matgo = db.PlayerMatgo.FindAllByUserID(dummy.ID).FirstOrDefault();
                }
                dummy.winCount = Data_Matgo.Win;
                dummy.loseCount = Data_Matgo.Lose;

                if (Data_Matgo.OptionVoiceType != null)
                    dummy.Option1 = Data_Matgo.OptionVoiceType;
                dummy.GameLevel = Data_Matgo.GameLevel;

                dynamic Data_Badugi = db.PlayerBadugi.FindAllByUserID(dummy.ID).FirstOrDefault();
                if (Data_Badugi == null)
                {
                    db.PlayerBadugi.Insert(UserId: dummy.ID);
                }

                //dummy.topMission = new List<CPlayerAgent.MissionData>();
                //dynamic Data_Mission = db.PlayerMatgoMission.FindAllByUserID(dummy.ID).FirstOrDefault();
                //if (Data_Mission == null)
                //{
                //    NewMissionData(ref dummy.topMission);
                //    db.PlayerMatgoMission.Insert(UserId: dummy.ID, Mission1: dummy.topMission[0].type, Complete1: dummy.topMission[0].isComplete, Mission2: dummy.topMission[1].type, Complete2: dummy.topMission[1].isComplete, Mission3: dummy.topMission[2].type, Complete3: dummy.topMission[2].isComplete, Mission4: dummy.topMission[3].type, Complete4: dummy.topMission[3].isComplete, Mission5: dummy.topMission[4].type, Complete5: dummy.topMission[4].isComplete, Mission6: dummy.topMission[5].type, Complete6: dummy.topMission[5].isComplete, Mission7: dummy.topMission[6].type, Complete7: dummy.topMission[6].isComplete, Mission8: dummy.topMission[7].type, Complete8: dummy.topMission[7].isComplete, Mission9: dummy.topMission[8].type, Complete9: dummy.topMission[8].isComplete, Mission10: dummy.topMission[9].type, Complete10: dummy.topMission[9].isComplete);
                //    Data_Mission = db.PlayerMatgoMission.FindAllByUserID(dummy.ID).FirstOrDefault();
                //}
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission1, Data_Mission.Complete1);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission2, Data_Mission.Complete2);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission3, Data_Mission.Complete3);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission4, Data_Mission.Complete4);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission5, Data_Mission.Complete5);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission6, Data_Mission.Complete6);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission7, Data_Mission.Complete7);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission8, Data_Mission.Complete8);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission9, Data_Mission.Complete9);
                //AddMissionData(ref dummy.topMission, Data_Mission.Mission10, Data_Mission.Complete10);

                // 로그인처리
                DB_User_Login(dummy.ID, ip);
                DBLog(dummy.ID, 0, 0, LOG_TYPE.로그인, "");

                return 1;
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("TryLogin Exception. ip{0}, id:{1}, e:{2}", ip, id, e.ToString());
                DB_User_Logout(dummy.ID, 2);
            }

            return 0;
        }
        bool RequestGoLobby(RemoteID remote, CPackOption rmiContext, string lobbyName)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false)
            {
                ClientDisconect(remote, "ServerMoveDataRequest Failure.");
                return false;
            }

            // 플레이어에게 할당된 릴레이 서버 확인
            int GoRelayID = rc.data.RelayID;

            // 릴레이 서버 확인
            RelayServerInfo relayInfo;
            if (Relay_list.TryGetValue(GoRelayID, out relayInfo))
            {
                // 이동 파라미터 구성
                ArrByte param_buffer;
                Common.MoveParam param = new Common.MoveParam();
                param.moveTo = Common.MoveParam.ParamMove.MoveToLobby;
                param.ChannelNumber = 1; // 채널 기본값
                param.RelayID = GoRelayID;
                Common.Common.ServerMoveParamWrite(param, rc, out param_buffer);

                NetServer.ServerMoveStart(remote, relayInfo.Addr, param_buffer, new Guid());
                return true;
            }

            return false;
        }
        bool RequestGameOptions(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false) return false;

            try
            {
                CMessage msg = data;
                int OptionVoiceType;
                msg.Read(out OptionVoiceType);

                Task.Run(() =>
                {
                    try
                    {
                        db.PlayerMatgo.UpdateByUserId(UserId: rc.data.ID, OptionVoiceType: OptionVoiceType);

                    }
                    catch (Exception e)
                    {
                        Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                    }
                });
            }
            catch (Exception e)
            {
                Log._log.ErrorFormat("RequestGameOptions Failure. Id:{0}", rc.data.userID);
                return false;
            }

            return true;
        }
        #endregion

        #region DB
        void DB_User_Login(int userId, string ip)
        {
            //Task.Run(() =>
            //{
            try
            {
                db.GameCurrentUser.Insert(UserId: userId, Locate: 0, GameId: GameId, ChannelId: 0, RoomId: 0, IP: ip, AutoPlay: false);
                db.Player.UpdateById(Id: userId, LastLoginDateDate: DateTime.Now, LastIP: ip, Online: true);

                //Log._log.InfoFormat("DB_User_Login Success. userId:{0}, ip:{1}", userId, ip);
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("DB_User_Login Failure. userId:{0}, ip:{1}, e:{2}", userId, ip, e.ToString());
            }
            //});
        }
        void DB_User_Logout(int userId, int delaySec = 0)
        {
            //Task.Run(async () =>
            //{
                try
                {
                    //await Task.Delay(1000 * delaySec);
                    db.GameCurrentUser.DeleteByUserId(UserId: userId);
                    db.Player.UpdateById(Id: userId, LastActivityDate: DateTime.Now, Online: false);

                    //Log._log.InfoFormat("{0} Success.", MethodBase.GetCurrentMethod().Name);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            //});
        }
        #endregion
    }
}
