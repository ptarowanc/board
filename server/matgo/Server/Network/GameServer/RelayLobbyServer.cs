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
    public class RelayLobbyServer : GameServer
    {
        internal object RelayServerLocker = new object();

        // 접속세션 목록
        ConcurrentDictionary<ushort, ArrByte> AddrPortClients;
        // 클라이언트 목록
        public ConcurrentDictionary<RemoteID, CPlayer> RemoteClients;
        // 로비서버 목록
        public RemoteID RemoteLobbyServers;
        // <유저, 로비> 포인트
        ConcurrentDictionary<RemoteID, RemoteID> RemoteRelays;

        // 릴레이 서버 ID
        public int RelayID;

        public RelayLobbyServer(MatgoService f, UnityCommon.Server t, ushort portnum, int relayID) : base(f, t, portnum, relayID)
        {
            // 릴레이 서버 초기화
            RelayID = relayID;
            AddrPortClients = new ConcurrentDictionary<ushort, ArrByte>();
            RemoteClients = new ConcurrentDictionary<RemoteID, CPlayer>();
            RemoteRelays = new ConcurrentDictionary<RemoteID, RemoteID>();
        }

        ~RelayLobbyServer()
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

                // 로비
                Stub.RequestLobbyKey = RequestLobbyKey;

                Stub.RequestJoinInfo = RequestJoinInfo;
                Stub.RequestChannelMove = RequestChannelMove; // ok
                Stub.RequestRoomMake = RequestRoomMake; // ok
                Stub.RequestRoomJoin = RequestRoomJoin;
                Stub.RequestRoomJoinSelect = RequestRoomJoinSelect;
                Stub.RequestBank = RequestBank;

                Stub.RequestPurchaseList = RequestPurchaseList;
                Stub.RequestPurchaseAvailability = RequestPurchaseAvailability;
                Stub.RequestPurchaseReceiptCheck = RequestPurchaseReceiptCheck;
                Stub.RequestPurchaseResult = RequestPurchaseResult;
                Stub.RequestPurchaseCash = RequestPurchaseCash;
                Stub.RequestMyroomList = RequestMyroomList;
                Stub.RequestMyroomAction = RequestMyroomAction;

                Stub.RequestLobbyEventInfo = RequestLobbyEventInfo;
                Stub.RequestLobbyEventParticipate = RequestLobbyEventParticipate;
            }

            // Server Stub
            {
                // Lobby/Room to Relay
                Stub.RelayCloseRemoteClient = RelayCloseRemoteClient;

                ////////// Lobby //////////
                // Lobby to Relay

                Stub.LobbyRelayServerMoveStart = LobbyRelayServerMoveStart;
                Stub.LobbyRelayResponseLobbyKey = LobbyRelayResponseLobbyKey;

                Stub.LobbyRelayNotifyUserInfo = LobbyRelayNotifyUserInfo;
                Stub.LobbyRelayNotifyUserList = LobbyRelayNotifyUserList;
                Stub.LobbyRelayNotifyRoomList = LobbyRelayNotifyRoomList;
                Stub.LobbyRelayResponseLobbyMessage = LobbyRelayResponseLobbyMessage;
                Stub.LobbyRelayResponseBank = LobbyRelayResponseBank;
                Stub.LobbyRelayNotifyJackpotInfo = LobbyRelayNotifyJackpotInfo;
                Stub.LobbyRelayNotifyLobbyMessage = LobbyRelayNotifyLobbyMessage;

                Stub.LobbyRelayResponsePurchaseList = LobbyRelayResponsePurchaseList;
                Stub.LobbyRelayResponsePurchaseAvailability = LobbyRelayResponsePurchaseAvailability;
                Stub.LobbyRelayResponsePurchaseReceiptCheck = LobbyRelayResponsePurchaseReceiptCheck;
                Stub.LobbyRelayResponsePurchaseResult = LobbyRelayResponsePurchaseResult;
                Stub.LobbyRelayResponsePurchaseCash = LobbyRelayResponsePurchaseCash;
                Stub.LobbyRelayResponseMyroomList = LobbyRelayResponseMyroomList;
                Stub.LobbyRelayResponseMyroomAction = LobbyRelayResponseMyroomAction;

                Stub.LobbyRelayResponseLobbyEventInfo = LobbyRelayResponseLobbyEventInfo;
                Stub.LobbyRelayResponseLobbyEventParticipate = LobbyRelayResponseLobbyEventParticipate;
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
                            Log._log.WarnFormat("ClientLeave Cancel. hostID:{0}, bMoveServer:{1}", remote, bMoveServer);
                        }
                    }
                    else
                    {
                        Log._log.WarnFormat("ClientLeave Cancel.(RemoteClients) RemoteID:{0}", remote);
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

                    if (itemToRemove.ServerType == ServerType.Lobby)
                    {
                        try
                        {
                            RemoteLobbyServers = RemoteID.Remote_None;

                            foreach (var remoteClient in RemoteRelays)
                            {
                                if (remoteClient.Value == itemToRemove.ServerHostID)
                                {
                                    RemoteID temp_;
                                    RemoteRelays.TryRemove(remoteClient.Key, out temp_);
                                    CPlayer temp__;
                                    RemoteClients.TryRemove(remoteClient.Key, out temp__);
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
                        //rc.channelNumber = param.ChannelNumber;
                        rc.channelNumber = 0; // 초기화 필요

                        if (RelayID == param.RelayID)
                        {
                            if (RemoteLobbyServers != RemoteID.Remote_None)
                            {
                                // 릴레이 서버에 할당된 로비서버로 안내
                                RemoteClients.TryAdd(remote, rc);
                                RemoteRelays.TryAdd(remote, RemoteLobbyServers);

                                Proxy.RelayClientJoin(RemoteLobbyServers, CPackOption.Basic, remote, addr, move_param);
                                //Proxy.ServerMoveEnd(remote, CPackOption.Basic, true);
                                return;
                            }
                            else
                            {
                                // 릴레이 서버에 할당된 로비서버가 없으면, 활성화된 로비서버를 찾아서 안내
                                ServerInfo[] svr_array;
                                GetServerInfo(ServerType.Lobby, out svr_array);
                                if (svr_array != null)
                                {
                                    foreach (var svr in svr_array)
                                    {
                                        RemoteClients.TryAdd(remote, rc);
                                        //RemoteRelays.TryAdd(remote, RemoteLobbyServers);
                                        RemoteRelays.TryAdd(remote, svr.ServerHostID);

                                        Proxy.RelayClientJoin(svr.ServerHostID, CPackOption.Basic, remote, addr, move_param);
                                        //Proxy.ServerMoveEnd(remote, CPackOption.Basic, true);
                                        return;
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
                NetServer.CloseRemoteClientForce(remote);
                Log._log.ErrorFormat("ClientJoinHandler Error. IP:{0}, remote:{1}", clientIP, remote);

                //// 접속하려는 채널 없음.
                //if (rc != null)
                //{
                //    RemoteClients.TryAdd(clientInfo.hostID, rc);
                //    Log._log.WarnFormat("ClientJoinHandler Cance. IP:{0}, UserID:{1}", clientIP, rc.data.userID);
                //}
                //else
                //{
                //    ArrByte moveData;
                //    AddrPortClients.TryRemove(clientInfo.tcpAddrFromServer.port, out moveData);
                //    Log._log.WarnFormat("ClientJoinHandler Cance. IP:{0}", clientIP);
                //}
                //Proxy.ServerMoveEnd(clientInfo.hostID, CPackOption.Basic, false);
                //ClientDisconect(clientInfo.hostID, "No Channel ID");
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
                        if (serverInfo.ServerType == ServerType.Lobby)
                        {
                            RemoteLobbyServers = serverInfo.ServerHostID;
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
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.ServerMoveFailure);
#endif
                Proxy.RelayServerMoveFailure(remoteLobby, CPackOption.Basic, remote);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        
        bool RequestLobbyKey(RemoteID remote, CPackOption rmiContext, string id, string key, int gameid)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestLobbyKey);
#endif
                Proxy.RelayRequestLobbyKey(remoteLobby, CPackOption.Basic, remote, id, key, gameid);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }

        bool RequestJoinInfo(RemoteID remote, CPackOption rmiContext)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestJoinInfo);
#endif
                Proxy.RelayRequestJoinInfo(remoteLobby, CPackOption.Basic, remote);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestChannelMove(RemoteID remote, CPackOption rmiContext, int chanID)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestChannelMove);
#endif
                Proxy.RelayRequestChannelMove(remoteLobby, CPackOption.Basic, remote, chanID);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestRoomMake(RemoteID remote, CPackOption rmiContext, int chanID, int betType, string pass)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestRoomMake);
#endif
                Proxy.RelayRequestRoomMake(remoteLobby, CPackOption.Basic, remote, RelayID, chanID, betType, pass);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestRoomJoin(RemoteID remote, CPackOption rmiContext, int chanID, int betType)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestRoomJoin);
#endif
                Proxy.RelayRequestRoomJoin(remoteLobby, CPackOption.Basic, remote, RelayID, chanID, betType);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestRoomJoinSelect(RemoteID remote, CPackOption rmiContext, int chanID, int roomNumber, string pass)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestRoomJoinSelect);
#endif
                Proxy.RelayRequestRoomJoinSelect(remoteLobby, CPackOption.Basic, remote, RelayID, chanID, roomNumber, pass);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestBank(RemoteID remote, CPackOption rmiContext, int option, long money, string pass)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestBank);
#endif
                Proxy.RelayRequestBank(remoteLobby, CPackOption.Basic, remote, option, money, pass);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestPurchaseList(RemoteID remote, CPackOption rmiContext)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestPurchaseList);
#endif
                Proxy.RelayRequestPurchaseList(remoteLobby, CPackOption.Basic, remote);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestPurchaseAvailability(RemoteID remote, CPackOption rmiContext, string pid)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestPurchaseAvailability);
#endif
                Proxy.RelayRequestPurchaseAvailability(remoteLobby, CPackOption.Basic, remote, pid);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestPurchaseReceiptCheck(RemoteID remote, CPackOption rmiContext, string result)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestPurchaseReceiptCheck);
#endif
                Proxy.RelayRequestPurchaseReceiptCheck(remoteLobby, CPackOption.Basic, remote, result);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestPurchaseResult(RemoteID remote, CPackOption rmiContext, System.Guid token)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestPurchaseResult);
#endif
                Proxy.RelayRequestPurchaseResult(remoteLobby, CPackOption.Basic, remote, token);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestPurchaseCash(RemoteID remote, CPackOption rmiContext, string pid)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestPurchaseCash);
#endif
                Proxy.RelayRequestPurchaseCash(remoteLobby, CPackOption.Basic, remote, pid);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestMyroomList(RemoteID remote, CPackOption rmiContext)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestMyroomList);
#endif
                Proxy.RelayRequestMyroomList(remoteLobby, CPackOption.Basic, remote);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestMyroomAction(RemoteID remote, CPackOption rmiContext, string pid)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby))
            {
#if PACKET
                    Log._log.InfoFormat("SEND Packet. remote:{0}, rmiID:{1})", remote, CS.Common.RequestMyroomAction);
#endif
                Proxy.RelayRequestMyroomAction(remoteLobby, CPackOption.Basic, remote, pid);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestLobbyEventInfo(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby) && data != null)
            {
                Send(remoteLobby, remote, SS.Common.RelayRequestLobbyEventInfo, data);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool RequestLobbyEventParticipate(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            RemoteID remoteLobby;
            if (RemoteRelays.TryGetValue(remote, out remoteLobby) && data != null)
            {
                Send(remoteLobby, remote, SS.Common.RelayRequestLobbyEventParticipate, data);
                return true;
            }
            ClientDisconect(remote, "Client Stub Cancel");
            Log._log.WarnFormat("{0} Cancel. remote:{1}", MethodBase.GetCurrentMethod().Name, remote);
            return false;
        }
        bool LobbyRelayResponseLobbyEventInfo(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            bool result_ = Send2(userRemote, SS.Common.ResponseLobbyEventInfo, data);
            if (result_ == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result_;
        }
        bool LobbyRelayResponseLobbyEventParticipate(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            bool result_ = Send2(userRemote, SS.Common.ResponseLobbyEventParticipate, data);
            if (result_ == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result_;
        }
        #endregion

        #region Server Stub Handler
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

        bool LobbyRelayServerMoveStart(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string moveServerIP, ushort moveServerPort, ArrByte param, Guid guid)
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
            NetAddress addr = new NetAddress();
            addr.m_ip = moveServerIP;
            addr.m_port = moveServerPort;
            NetServer.ServerMoveStart(userRemote, addr, Msg.m_array, guid);
            return true;
        }
        bool LobbyRelayResponseLobbyKey(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string key, int gameid)
        {
            bool result = Proxy.ResponseLobbyKey(userRemote, CPackOption.Encrypt, key, gameid);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }

        bool LobbyRelayNotifyUserInfo(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            bool result = Proxy.NotifyUserInfo(userRemote, CPackOption.Basic, userInfo);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayNotifyUserList(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, System.Collections.Generic.List<string> lobbyFriendList)
        {
            bool result = Proxy.NotifyUserList(userRemote, CPackOption.Basic, lobbyUserInfos, lobbyFriendList);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayNotifyRoomList(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, int channelID, System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos)
        {
            bool result = Proxy.NotifyRoomList(userRemote, CPackOption.Basic, channelID, roomInfos);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        //bool LobbyRelayResponseChannelMove(RemoteID remote,CPackOption rmiContext,  RemoteID userRemote,  int chanID); // not use
        bool LobbyRelayResponseLobbyMessage(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string message)
        {
            bool result = Proxy.ResponseLobbyMessage(userRemote, CPackOption.Basic, message);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayResponseBank(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool result, int resultType)
        {
            bool result_ = Proxy.ResponseBank(userRemote, CPackOption.Basic, result, resultType);
            if (result_ == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result_;
        }
        bool LobbyRelayNotifyJackpotInfo(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, long jackpot)
        {
            bool result = Proxy.NotifyJackpotInfo(userRemote, CPackOption.Basic, jackpot);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayNotifyLobbyMessage(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, int type, string message, int period)
        {
            bool result = Proxy.NotifyLobbyMessage(userRemote, CPackOption.Basic, type, message, period);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayResponsePurchaseList(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, System.Collections.Generic.List<string> Purchase_avatar, System.Collections.Generic.List<string> Purchase_card, System.Collections.Generic.List<string> Purchase_evt, System.Collections.Generic.List<string> Purchase_charge)
        {
            bool result = Proxy.ResponsePurchaseList(userRemote, CPackOption.Basic, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayResponsePurchaseAvailability(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool available, string reason)
        {
            bool result = Proxy.ResponsePurchaseAvailability(userRemote, CPackOption.Basic, available, reason);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayResponsePurchaseReceiptCheck(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool result, System.Guid token)
        {
            bool result_ = Proxy.ResponsePurchaseReceiptCheck(userRemote, CPackOption.Basic, result, token);
            if (result_ == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result_;
        }
        bool LobbyRelayResponsePurchaseResult(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool result, string reason)
        {
            bool result_ = Proxy.ResponsePurchaseResult(userRemote, CPackOption.Basic, result, reason);
            if (result_ == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result_;
        }
        bool LobbyRelayResponsePurchaseCash(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, bool result, string reason)
        {
            bool result_ = Proxy.ResponsePurchaseCash(userRemote, CPackOption.Basic, result, reason);
            if (result_ == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result_;
        }
        bool LobbyRelayResponseMyroomList(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string json)
        {
            bool result = Proxy.ResponseMyroomList(userRemote, CPackOption.Basic, json);
            if (result == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result;
        }
        bool LobbyRelayResponseMyroomAction(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string pid, bool result, string reason)
        {
            bool result_ = Proxy.ResponseMyroomAction(userRemote, CPackOption.Basic, pid, result, reason);
            if (result_ == false) Log._log.WarnFormat("{0} Cancel. remote:{1}, userRemote{2}", MethodBase.GetCurrentMethod().Name, remote, userRemote);
            return result_;
        }
        #endregion

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
    }
}
