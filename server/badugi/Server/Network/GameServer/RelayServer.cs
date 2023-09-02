using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Guid = System.Guid;
using ZNet;

namespace Server.Network
{
    public class RelayServer : GameServer
    {
        internal object RelayServerLocker = new object();

        // 접속세션 목록
        ConcurrentDictionary<ushort, ArrByte> AddrPortClients;
        // 클라이언트 목록
        public ConcurrentDictionary<RemoteID, CPlayer> RemoteClients;
        // 게임서버 목록
        public ConcurrentDictionary<int, RemoteID> RemoteRoomServers;
        // <유저, 룸> 포인트
        ConcurrentDictionary<RemoteID, RemoteID> RemoteRelays;

        // 릴레이 서버 ID
        public int RelayID;

        public RelayServer(BadugiService f, UnityCommon.Server t, ushort portnum, int relayID) : base(f, t, portnum, relayID)
        {
            // 릴레이 서버 초기화
            RelayID = relayID;
            AddrPortClients = new ConcurrentDictionary<ushort, ArrByte>();
            RemoteClients = new ConcurrentDictionary<RemoteID, CPlayer>();
            RemoteRoomServers = new ConcurrentDictionary<int, RemoteID>();
            RemoteRelays = new ConcurrentDictionary<RemoteID, RemoteID>();
        }

        ~RelayServer()
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

            // Client Stub
            {
                //Stub.ServerMoveFailure = ServerMoveFailure;

                // 게임
                Stub.GameRoomInUser = GameRoomInUser;
                Stub.GameRequestReady = GameRequestReady;
                Stub.GameDealCardsEnd = GameDealCardsEnd;
                Stub.GameActionBet = GameActionBet;
                Stub.GameActionChangeCard = GameActionChangeCard;
                Stub.GameRoomOutRsvn = GameRoomOutRsvn;
                Stub.GameRoomOut = GameRoomOut;
                Stub.GameRoomMove = GameRoomMove;
            }

            // Server Stub
            {
                // Lobby/Room to Relay
                Stub.RelayCloseRemoteClient = RelayCloseRemoteClient;

                // Lobby to Room
                Stub.LobbyRoomJackpotInfo = LobbyRoomJackpotInfo;
                Stub.LobbyRoomNotifyMessage = LobbyRoomNotifyMessage;

                ////////// Lobby //////////

                ////////// Room //////////
                // Room to Relay
                Stub.RoomRelayServerMoveStart = RoomRelayServerMoveStart;
                Stub.RelayResponseRoomOut = RelayResponseRoomOut;
                Stub.RelayResponseRoomMove = RelayResponseRoomMove;
                Stub.GameRelayResponseRoomOutRsvn = GameRelayResponseRoomOutRsvn;
                Stub.GameRelayResponseRoomOut = GameRelayResponseRoomOut;
                Stub.GameRelayResponseRoomMove = GameRelayResponseRoomMove;

                Stub.GameRelayRoomIn = GameRelayRoomIn;
                Stub.GameRelayRoomReady = GameRelayRoomReady;
                Stub.GameRelayStart = GameRelayStart;
                Stub.GameRelayDealCards = GameRelayDealCards;
                Stub.GameRelayUserIn = GameRelayUserIn;
                Stub.GameRelaySetBoss = GameRelaySetBoss;
                Stub.GameRelayNotifyStat = GameRelayNotifyStat;
                Stub.GameRelayRoundStart = GameRelayRoundStart;
                Stub.GameRelayChangeTurn = GameRelayChangeTurn;
                Stub.GameRelayRequestBet = GameRelayRequestBet;
                Stub.GameRelayResponseBet = GameRelayResponseBet;
                Stub.GameRelayChangeRound = GameRelayChangeRound;
                Stub.GameRelayRequestChangeCard = GameRelayRequestChangeCard;
                Stub.GameRelayResponseChangeCard = GameRelayResponseChangeCard;
                Stub.GameRelayCardOpen = GameRelayCardOpen;
                Stub.GameRelayOver = GameRelayOver;
                Stub.GameRelayRoomInfo = GameRelayRoomInfo;
                Stub.GameRelayKickUser = GameRelayKickUser;
                Stub.GameRelayEventInfo = GameRelayEventInfo;
                Stub.GameRelayUserInfo = GameRelayUserInfo;
                Stub.GameRelayUserOut = GameRelayUserOut;
                Stub.GameRelayEventStart = GameRelayEventStart;
                Stub.GameRelayEvent2Start = GameRelayEvent2Start;
                Stub.GameRelayEventRefresh = GameRelayEventRefresh;
                Stub.GameRelayEventEnd = GameRelayEventEnd;
                Stub.GameRelayMileageRefresh = GameRelayMileageRefresh;
                Stub.GameRelayEventNotify = GameRelayEventNotify;
                Stub.GameRelayCurrentInfo = GameRelayCurrentInfo;
                Stub.GameRelayEntrySpectator = GameRelayEntrySpectator;
                Stub.GameRelayNotifyMessage = GameRelayNotifyMessage;
                //Stub.GameRelayNotifyJackpotInfo = GameRelayNotifyJackpotInfo;
            }
        }

        #region Relay Server
        public override void ServerTask(object sender, ElapsedEventArgs e_)
        {
            ++this.tick;

            if (this.tick % 17 == 0)
            {
                //DisplayStatus(m_Core);

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
        private void ClientDisconect(RemoteID remote, string reasone)
        {
            NetServer.CloseConnection(remote);
            //ClientLeave(remote, ErrorType.DisconnectFromRemote, null);
            Log._log.WarnFormat("ClientDisconect. remote:{0}, reasone:{1}", remote, reasone);
        }
        void ClientLeave(RemoteID remote, bool bMoveServer)
        {
            int userID = -1;

            lock (RelayServerLocker)
            {
                try
                {
                    CPlayer rc;
                    if (RemoteClients.TryGetValue(remote, out rc))
                    {
                        userID = rc.data.ID;

                        if (bMoveServer == true)
                        {
                        }
                        else
                        {
                            // 비정상 연결 종료
                            Log._log.WarnFormat("ClientLeave Exit. hostID:{0}, bMoveServer:{1}", remote, bMoveServer);
                        }
                    }
                    else
                    {
                        Log._log.WarnFormat("ClientLeave Cancel.(RemoteClients) hostID:{0}", remote);
                    }
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("ClientLeave Exception. hostID:{0}, bMoveServer:{1}, e:{2}", remote, bMoveServer, e.ToString());
                }

                RemoteID remoteRoomRemove;
                if (RemoteRelays.TryRemove(remote, out (remoteRoomRemove)))
                {
                    Proxy.RelayClientLeave(remoteRoomRemove, CPackOption.Basic, remote, bMoveServer);
                }
                CPlayer playerRemove;
                RemoteClients.TryRemove(remote, out playerRemove);
                if (bMoveServer == false && userID != -1)
                {
                    //DB_User_Logout(userID);
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
            ServerInfo serverNew = new ServerInfo();
            serverNew.ServerHostID = remote;
            serverNew.ServerName = description;
            serverNew.ServerType = (ServerType)type;
            serverNew.ServerAddrPort = addr;
            serverNew.PublicIP = addr.m_ip;
            serverNew.PublicPort = addr.m_port;
            serverNew.ServerID = ServerID;

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
            //    if (ShutDown) return; // 서버 종료중이면 서버퇴장 대응 없음

            lock (GameServerLocker)
            {
                var itemToRemove = ServerInfoList.Values.SingleOrDefault(r => r.ServerHostID == remote);
                if (itemToRemove != null)
                {
                    ServerInfo remove;
                    ServerInfoList.TryRemove(itemToRemove.ServerHostID, out remove);

                    if (itemToRemove.ServerType == ServerType.Room)
                    {
                        try
                        {
                            foreach (var roomSrv in RemoteRoomServers)
                            {
                                if (roomSrv.Value == itemToRemove.ServerHostID)
                                {
                                    RemoteID temp_;
                                    RemoteRoomServers.TryRemove(roomSrv.Key, out temp_);
                                }
                            }
                            foreach (var remoteClient in RemoteRelays)
                            {
                                if (remoteClient.Value == itemToRemove.ServerHostID)
                                {
                                    RemoteID temp_;
                                    RemoteRelays.TryRemove(remoteClient.Key, out temp_);
                                }
                            }
                            Log._log.InfoFormat("MasterP2PMemberLeaveHandler Room Clear. RemoteID:{0}", itemToRemove.ServerHostID);
                        }
                        catch (Exception e)
                        {
                            Log._log.ErrorFormat("MasterP2PMemberLeaveHandler Failure. RemoteID:{0}, e:{1}", itemToRemove.ServerHostID, e.ToString());
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
            Log._log.InfoFormat("move_server_failed_handler.");
        }
        bool move_server_param_handler(ArrByte move_param, int count_idx)
        {
            //Log._log.InfoFormat("move_server_param_handler. count_idx:{0}", count_idx);
            return true;
        }
        void move_server_start_handler(RemoteID remote, out ArrByte userdata)
        {
            CPlayer rc = null;
            userdata = null;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                // 여기서는 이동할 서버로 동기화 시킬 유저 데이터를 구성하여 buffer에 넣어둔다 -> 완료서버에서 해당 데이터를 그대로 받게된다
                Common.Common.ServerMoveStart(rc, out userdata);
            }

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
            lock (RelayServerLocker)
            {
                string clientIP = addr.m_ip;

                CPlayer rc = null;

                try
                {
                    if (move_server.Count > 0)
                    {
                        // 서버 이동 데이터 불러오기
                        Common.MoveParam param;
                        Common.Common.ServerMoveParamRead(move_param, out param, out rc);
                        rc.m_ip = clientIP;
                        rc.channelNumber = param.ChannelNumber;

                        if (RelayID == param.RelayID)
                        {
                            RemoteID remoteServer;
                            if (RemoteRoomServers.TryGetValue(param.ChannelNumber, out remoteServer))
                            {
                                // 접속하려는 채널 ID에 해당하는 서버로 연결
                                ServerInfo[] svr_array;
                                GetServerInfo(ServerType.Room, out svr_array);
                                if (svr_array != null)
                                {
                                    foreach (var svr in svr_array)
                                    {
                                        if (svr.ServerHostID == remoteServer)
                                        {
                                            RemoteClients.TryAdd(remote, rc);
                                            RemoteRelays.TryAdd(remote, remoteServer);

                                            //Proxy.ServerMoveEnd(remote, CPackOption.Basic, true);
                                            Proxy.RelayClientJoin(svr.ServerHostID, CPackOption.Basic, remote, addr, move_param);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log._log.ErrorFormat("ClientJoinHandler Exciption. IP:{0}, e:{1}", clientIP, e.ToString());
                }

                //Proxy.ServerMoveEnd(remote, CPackOption.Basic, false);
                ClientDisconect(remote, "client_join_handler");
                Log._log.ErrorFormat("ClientJoinHandler Error. IP:{0}, remote:{1}", clientIP, remote);

                //// 접속하려는 채널 없음.
                //if (rc != null)
                //{
                //    RemoteClients.TryAdd(remote, rc);
                //    Log._log.WarnFormat("ClientJoinHandler Cance. IP:{0}, UserID:{1}", clientIP, rc.data.userID);
                //}
                ////Proxy.ServerMoveEnd(remote, CPackOption.Basic, false);
                ////ClientDisconect(clientInfo.hostID, "No Channel ID");
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
                        // 룸일 경우 현재 정보 요청
                        if (serverInfo.ServerType == ServerType.Room)
                        {
                            RemoteRoomServers.TryAdd(serverInfo.ServerID, serverInfo.ServerHostID);
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Client Stub Handler
        bool ServerMoveFailure(RemoteID remote, CPackOption rmiContext)
        {
            RemoteID remoteRoom;
            if (RemoteRelays.TryGetValue(remote, out remoteRoom))
            {
                Proxy.RelayServerMoveFailure(remoteRoom, CPackOption.Basic, remote);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }

        bool GameRoomInUser(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom) && data != null)
                {
                    Send(remoteRoom, remote, SS.Common.RelayGameRoomIn, data);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool GameRequestReady(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom) && data != null)
                {
                    Send(remoteRoom, remote, SS.Common.RelayGameRequestReady, data);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool GameDealCardsEnd(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom) && data != null)
                {
                    Send(remoteRoom, remote, SS.Common.RelayGameDealCardsEnd, data);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool GameActionBet(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom) && data != null)
                {
                    Send(remoteRoom, remote, SS.Common.RelayGameActionBet, data);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool GameActionChangeCard(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom) && data != null)
                {
                    Send(remoteRoom, remote, SS.Common.RelayGameActionChangeCard, data);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool GameRoomOutRsvn(RemoteID remote, CPackOption rmiContext, bool IsRsvn)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom))
                {
                    Proxy.RelayRequestRoomOutRsvn(remoteRoom, CPackOption.Basic, remote, IsRsvn);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool GameRoomOut(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom))
                {
                    Proxy.RelayRequestRoomOut(remoteRoom, CPackOption.Basic, remote);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool GameRoomMove(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            try
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom))
                {
                    Proxy.RelayRequestRoomMove(remoteRoom, CPackOption.Basic, remote);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, e:{2}", MethodBase.GetCurrentMethod().Name, remote, e.ToString());
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        #endregion

        #region Server Stub Handler
        bool LobbyRoomJackpotInfo(RemoteID remote, CPackOption rmiContext, long jackpot)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameEventInfo;
            Msg.WriteStart(msgID, CPackOption.Basic, 0, true);

            Rmi.Marshaler.Write(Msg, jackpot);
            foreach (var obj in RemoteClients.Keys)
            {
                Proxy.PacketSend(obj, CPackOption.Basic, Msg);
            }

            return true;
        }
        bool LobbyRoomNotifyMessage(RemoteID remote, CPackOption rmiContext, int type, string message, int period)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameEventInfo;
            Msg.WriteStart(msgID, CPackOption.Basic, 0, true);

            Rmi.Marshaler.Write(Msg, type);
            Rmi.Marshaler.Write(Msg, message);
            Rmi.Marshaler.Write(Msg, period);
            foreach (var obj in RemoteClients.Keys)
            {
                Proxy.PacketSend(obj, CPackOption.Basic, Msg);
            }

            return true;
        }
        bool RelayCloseRemoteClient(RemoteID remote, CPackOption rmiContext, RemoteID userRemote)
        {
            try
            {
                CPlayer rc;
                if (RemoteClients.TryGetValue(userRemote, out rc))
                {
                    ClientDisconect(userRemote, "RelayCloseRemoteClient");
                }
                else
                {
                    ClientDisconect(userRemote, "RelayCloseRemoteClient");
                    Log._log.WarnFormat("RelayCloseRemoteClient rc none remote:{0}, userRemote{1}", remote, userRemote);
                }
                return true;
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("{0} Failure. remote:{1}, userRemote:{2}, e:{3}", MethodBase.GetCurrentMethod().Name, remote, userRemote, e.ToString());
            }
            ClientDisconect(userRemote, "Server Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return false;
        }

        bool RoomRelayServerMoveStart(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param, Guid idx)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.CMessage Msg2 = new ZNet.CMessage();
            Msg2.m_array = param;
            unsafe
            {
                byte[] Datas = Msg2.GetData();
                //Rmi.Marshaler.Write(Msg, Datas.Count());
                fixed (byte* pData = &Datas[4])
                {
                    Msg.Write(pData, Datas.Count());
                }
            }
            NetServer.ServerMoveStart(userRemote, addr, Msg.m_array, idx);
            return true;
            //return Proxy.ServerMoveStart(userRemote, CPackOption.Basic, moveServerIP, moveServerPort, param);
        }
        bool RelayResponseRoomOut(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool resultOut)
        {
            Proxy.GameResponseRoomOut(userRemote, CPackOption.Basic, resultOut);
            return true;
        }
        bool RelayResponseRoomMove(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool resultMove, string errorMessage)
        {
            if (resultMove)
            {
                Proxy.GameResponseRoomMove(userRemote, CPackOption.Basic, true, "");
            }
            else
            {
                Proxy.GameResponseRoomMove(userRemote, CPackOption.Basic, false, errorMessage);
            }
            return true;
        }
        bool GameRelayResponseRoomOutRsvn(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, byte player_index, bool IsRsvn)
        {
            Proxy.GameResponseRoomOutRsvp(userRemote, CPackOption.Basic, player_index, IsRsvn);
            return true;
        }
        bool GameRelayResponseRoomOut(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool permissionOut)
        {
            Proxy.GameResponseRoomOut(userRemote, CPackOption.Basic, permissionOut);
            return true;
        }
        bool GameRelayResponseRoomMove(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool resultMove, string errorMessage)
        {
            if (resultMove)
            {
                Proxy.GameResponseRoomMove(userRemote, CPackOption.Basic, true, "");
            }
            else
            {
                Proxy.GameResponseRoomMove(userRemote, CPackOption.Basic, false, errorMessage);
            }
            return true;
        }
        bool GameRelayRoomIn(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool result)
        {
            Proxy.GameRoomIn(userRemote, CPackOption.Basic, result);
            return true;
        }
        bool GameRelayRoomReady(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameRoomReady, data);
            return true;
        }
        bool GameRelayStart(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameStart, data);
            return true;
        }
        bool GameRelayDealCards(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameDealCards, data);
            return true;
        }
        bool GameRelayUserIn(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameUserIn, data);
            return true;
        }
        bool GameRelaySetBoss(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameSetBoss, data);
            return true;
        }
        bool GameRelayNotifyStat(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameNotifyStat, data);
            return true;
        }
        bool GameRelayRoundStart(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameRoundStart, data);
            return true;
        }
        bool GameRelayChangeTurn(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameChangeTurn, data);
            return true;
        }
        bool GameRelayRequestBet(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameRequestBet, data);
            return true;
        }
        bool GameRelayResponseBet(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameResponseBet, data);
            return true;
        }
        bool GameRelayChangeRound(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameChangeRound, data);
            return true;
        }
        bool GameRelayRequestChangeCard(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameRequestChangeCard, data);
            return true;
        }
        bool GameRelayResponseChangeCard(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameResponseChangeCard, data);
            return true;
        }
        bool GameRelayCardOpen(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameCardOpen, data);
            return true;
        }
        bool GameRelayOver(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameOver, data);
            return true;
        }
        bool GameRelayRoomInfo(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameRoomInfo, data);
            return true;
        }
        bool GameRelayKickUser(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameKickUser, data);
            return true;
        }
        bool GameRelayEventInfo(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameEventInfo, data);
            return true;
        }
        bool GameRelayUserInfo(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameUserInfo, data);
            return true;
        }
        bool GameRelayUserOut(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameUserOut, data);
            return true;
        }
        bool GameRelayEventStart(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameEventStart, data);
            return true;
        }
        bool GameRelayEvent2Start(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameEvent2Start, data);
            return true;
        }
        bool GameRelayEventRefresh(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameEventRefresh, data);
            return true;
        }
        bool GameRelayEventEnd(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameEventEnd, data);
            return true;
        }
        bool GameRelayMileageRefresh(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameMileageRefresh, data);
            return true;
        }
        bool GameRelayEventNotify(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameEventNotify, data);
            return true;
        }
        bool GameRelayCurrentInfo(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameCurrentInfo, data);
            return true;
        }
        bool GameRelayEntrySpectator(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameEntrySpectator, data);
            return true;
        }
        bool GameRelayNotifyMessage(RemoteID remote, CPackOption rmiContext, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            Send2(userRemote, SS.Common.GameNotifyMessage, data);
            return true;
        }
        bool Send(RemoteID remote, RemoteID userRemote, ZNet.PacketType msgID, ZNet.CMessage data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            Msg.WriteStart(msgID, CPackOption.Basic, 0, true);
            Rmi.Marshaler.Write(Msg, userRemote);

            ZNet.CMessage Msg2 = data;
            if (Msg2.m_array.Count > 0)
            {
                unsafe
                {
                    byte[] Datas = Msg2.GetData();
                    fixed (byte* pData = &Datas[Msg2.Position])
                    {
                        Msg.Write(pData, Datas.Count());
                    }
                }
            }
            return Proxy.PacketSend(remote, CPackOption.Basic, Msg);
        }
        bool Send2(RemoteID userRemote, ZNet.PacketType msgID, ZNet.CMessage data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            Msg.WriteStart(msgID, CPackOption.Basic, 0, true);

            ZNet.CMessage Msg2 = data;
            if (Msg2.m_array.Count > 0)
            {
                unsafe
                {
                    byte[] Datas = Msg2.GetData();
                    fixed (byte* pData = &Datas[Msg2.Position])
                    {
                        Msg.Write(pData, Datas.Count());
                    }
                }
            }
            return Proxy.PacketSend(userRemote, CPackOption.Basic, Msg);
        }
        #endregion
    }

    struct RelayServerInfo
    {
        public RemoteID remote;
        public int RelayID;
        public ZNet.NetAddress Addr;
    }
}
