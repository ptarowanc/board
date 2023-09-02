using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Guid = System.Guid;
using ZNet;

namespace Server.Network
{
    class LobbyServer : GameServer
    {
        object LobbyLock = new object();
        // 클라이언트 목록
        ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>> RemoteClients = new ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>>();
        ConcurrentDictionary<int, CPlayer> RemoteClients2 = new ConcurrentDictionary<int, CPlayer>();
        // 릴레이 서버 목록
        ConcurrentDictionary<int, RelayServerInfo> RemoteRelays = new ConcurrentDictionary<int, RelayServerInfo>();
        ConcurrentDictionary<RemoteID, RelayServerInfo> RemoteRelaysh = new ConcurrentDictionary<RemoteID, RelayServerInfo>();
        // 방 목록
        ConcurrentDictionary<Guid, Rmi.Marshaler.RoomInfo> RemoteRoomInfos = new ConcurrentDictionary<Guid, Rmi.Marshaler.RoomInfo>();
        // 채널별 방 수
        ConcurrentDictionary<int, int> ChannelsRoomsCount = new ConcurrentDictionary<int, int>();
        // 전송할 로비 유저 목록
        ConcurrentDictionary<int, Rmi.Marshaler.LobbyUserList> LobbyUserList = new ConcurrentDictionary<int, Rmi.Marshaler.LobbyUserList>();

        // 방입장 제한 샵 목록
        ConcurrentDictionary<int, bool> RestrictionShop = new ConcurrentDictionary<int, bool>();

        // 채널당 방생성 한도
        int RoomLimit = 200;

        // 로비 데이터 갱신주기 관리
        DateTime BrodcastRoomListTime = DateTime.Now;
        bool RoomListUpdate;
        DateTime BrodcastUserListTime = DateTime.Now;
        bool UserListUpdate;

        // 잭팟 금액
        long JackPotMoney = 0;

        // 리필 머니, 횟수
        long RechargeFreeMoney = 0;
        int RechargeFreeCount = 0;
        long RechargePayMoney = 0;
        int RechargePayCount = 0;

        // 이벤트 : 행운의 복권
        int EventLuckyLottoCost;
        int EventLuckyLottoLimited;

        public LobbyServer(BadugiService f, UnityCommon.Server t, ushort portnum) : base(f, t, portnum, 0)
        {
            DB_Server_CurrentPlayerClear();
            DB_Server_GetLobbyData();
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
                // 잭팟
                Stub.RoomLobbyEventStart = RoomLobbyEventStart;
                Stub.RoomLobbyEventEnd = RoomLobbyEventEnd;

                // 로비 서버
                Stub.RoomLobbyMakeRoom = RoomLobbyMakeRoom;
                Stub.RoomLobbyJoinRoom = RoomLobbyJoinRoom;
                Stub.RoomLobbyOutRoom = RoomLobbyOutRoom;
                Stub.RoomLobbyRequestMoveRoom = RoomLobbyRequestMoveRoom;
                Stub.RoomLobbyResponseDataSync = RoomLobbyResponseDataSync;
                Stub.RoomLobbyMessage = RoomLobbyMessage;

                // 릴레이서버 CoreHandle Relay
                Stub.RelayClientJoin = RelayClientJoin; // ok
                Stub.RelayClientLeave = RelayClientLeave; // ok
                Stub.RelayServerMoveFailure = RelayServerMoveFailure;

                // 클라이언트 Response Relay
                Stub.RelayRequestLobbyKey = RelayRequestLobbyKey; // ok
                Stub.RelayRequestJoinInfo = RelayRequestJoinInfo; // ok
                Stub.RelayRequestChannelMove = RelayRequestChannelMove; // ok
                Stub.RelayRequestRoomMake = RelayRequestRoomMake; // ok
                Stub.RelayRequestRoomJoin = RelayRequestRoomJoin; // ok
                Stub.RelayRequestRoomJoinSelect = RelayRequestRoomJoinSelect; // ok
                Stub.RelayRequestBank = RelayRequestBank; // ok

                Stub.RelayRequestPurchaseList = RelayRequestPurchaseList;
                Stub.RelayRequestPurchaseAvailability = RelayRequestPurchaseAvailability;
                Stub.RelayRequestPurchaseReceiptCheck = RelayRequestPurchaseReceiptCheck;
                Stub.RelayRequestPurchaseResult = RelayRequestPurchaseResult;
                Stub.RelayRequestPurchaseCash = RelayRequestPurchaseCash;
                Stub.RelayRequestMyroomList = RelayRequestMyroomList;
                Stub.RelayRequestMyroomAction = RelayRequestMyroomAction;

                Stub.RelayRequestLobbyEventInfo = RelayRequestLobbyEventInfo;
                Stub.RelayRequestLobbyEventParticipate = RelayRequestLobbyEventParticipate;
            }
        }

        #region Lobby Server
        public override void ServerTask(object sender, ElapsedEventArgs e_)
        {
            ++this.tick;

            if (this.tick % 10 == 0)
            {
                if (RoomListUpdate)
                {
                    Brodcast_Room_List();
                    RoomListUpdate = false;
                }
                if (UserListUpdate)
                {
                    Brodcast_User_List();
                    UserListUpdate = false;
                }
            }

            if (this.tick % 7 == 0)
            {
                DB_Server_GetServerMessage();
            }

            if (this.tick % 13 == 0)
            {
                DB_Server_SendPlayerInfo();
            }

            if (this.tick % 17 == 0)
            {
                //DisplayStatus(m_Core);

                if (this.ShutDown)
                {
                    // 세션 없으면 프로그램 종료
                    if (RemoteClients.Count == 0 || this.CountDown.AddMinutes(1) < DateTime.Now)
                    {
                        Log._log.Info("서버 종료. ShutDown");
                        DB_Server_CurrentPlayerClear();
                        //System.Windows.Forms.Application.Exit();
                        System.Environment.Exit(0);
                        return;
                    }
                    else if (this.CountDown < DateTime.Now)
                    {
                        // 모든 세션 종료
                        foreach (var relay in RemoteClients)
                        {
                            foreach (var client in relay.Value)
                            {
                                ClientDisconect(relay.Key, client.Key, "ShutDown");
                                //NetServer.CloseConnection(client.Key);
                            }
                        }
                    }
                    Log._log.Info("세션 종료중. 남은 세션 수:" + RemoteClients.Count);
                }
            }

            if (this.tick % 61 == 0)
            {
                DB_Server_SendJackPotMoney();

                ServerNoticeMaintenance();
            }
        }
        public void ClientDisconect(RemoteID remoteS, RemoteID remoteC, string reasone)
        {
            Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
            Log._log.WarnFormat("ClientDisconect. remoteS:{0}, remoteC:{1}, reasone:{2}", remoteS, remoteC, reasone);
        }
        void Brodcast_Room_List()
        {
            if (BrodcastRoomListTime > DateTime.Now)
            {
                RoomListUpdate = true;
                return;
            }
            BrodcastRoomListTime = DateTime.Now.AddMilliseconds(10000);

            var temp = RemoteRoomInfos.Values.ToList();
            foreach (var relay in RemoteClients)
            {
                foreach (var client in relay.Value)
                {
                    // 클라이언트에서 채널번호 처리하도록 작업 필요!
                    //Proxy.NotifyRoomList(client.Key, CPackOption.Basic, client.Value.channelNumber, RemoteRoomInfos.Values.ToList());
                    if (client.Value.channelNumber == 0) continue;
                    Proxy.LobbyRelayNotifyRoomList(client.Value.Remote.Key, CPackOption.Basic, client.Value.Remote.Value, client.Value.channelNumber, temp);
                }
            }
            RoomListUpdate = false;
        }
        void Brodcast_User_List()
        {
            if (BrodcastUserListTime > DateTime.Now)
            {
                UserListUpdate = true;
                return;
            }
            BrodcastUserListTime = DateTime.Now.AddMilliseconds(333);

            var UserList = LobbyUserList.Values.ToList();

            foreach (var relay in RemoteClients)
            {
                foreach (var client in relay.Value)
                {
                    Proxy.LobbyRelayNotifyUserList(client.Value.Remote.Key, CPackOption.Basic, client.Value.Remote.Value, UserList, client.Value.friendList);
                }
            }
            //List<string> temp = new List<string>();
            //Proxy.NotifyUserList(RemoteClients.Keys.ToArray(), CPackOption.Basic, UserList, temp);

            UserListUpdate = false;
        }
        void Brodcast_User_Info()
        {
            foreach (var relay in RemoteClients)
            {
                foreach (var client in relay.Value)
                {
                    Proxy.LobbyRelayNotifyUserInfo(client.Value.Remote.Key, CPackOption.Basic, client.Value.Remote.Value, makeLobbyUserInfo(client.Value.data));
                }
            }
        }
        void Brodcast_Notify_Message(int type, string message, int showTime)
        {
            foreach (var relay in RemoteClients)
            {
                foreach (var client in relay.Value)
                {
                    // 공지 타입 : 전체 공지, 로비 공지, 룸 공지 등
                    Proxy.LobbyRelayNotifyLobbyMessage(client.Value.Remote.Key, CPackOption.Basic, client.Value.Remote.Value, type, message, showTime);
                }
            }
            //Proxy.NotifyLobbyMessage(RemoteClients.Keys.ToArray(), CPackOption.Basic, type, message, showTime);

            // 릴레이 서버에 공지 전달
            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Relay, out svr_array);
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    Proxy.LobbyRoomNotifyMessage(svr.ServerHostID, CPackOption.Basic, type, message, showTime);
                }
            }
            else
            {
                Log._log.WarnFormat("Brodcast_Notify_Message Cancel. type:{0}, message:{1}, showTime{2}", type, message, showTime);
            }
        }
        void Brodcast_Notify_ServerMaintenance(int type, string message, int Release)
        {
            if (Release == 1)
            {
                this.ServerMaintenance = false;
            }
            else
            {
                this.ServerMaintenance = true;
            }

            foreach (var relay in RemoteClients)
            {
                foreach (var client in relay.Value)
                {
                    // 공지 타입 : 전체 공지, 로비 공지, 룸 공지 등
                    Proxy.LobbyRelayNotifyLobbyMessage(client.Value.Remote.Key, CPackOption.Basic, client.Value.Remote.Value, type, message, 30);
                }
            }
            //Proxy.NotifyLobbyMessage(RemoteClients.Keys.ToArray(), CPackOption.Basic, type, message, 30);

            // 룸 서버에 점검 전달
            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Room, out svr_array);
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    Proxy.LobbyRoomNotifyServermaintenance(svr.ServerHostID, CPackOption.Basic, type, message, Release);
                }
            }
            else
            {
                Log._log.WarnFormat("Brodcast_Notify_ServerMaintenance Room Cancel. type:{0}, message:{1}, showTime{2}", type, message, Release);
            }

            // 릴레이 서버에 공지 전달
            GetServerInfo(ServerType.Relay, out svr_array);
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    Proxy.LobbyRoomNotifyMessage(svr.ServerHostID, CPackOption.Basic, type, message, 30);
                }
            }
            else
            {
                Log._log.WarnFormat("Brodcast_Notify_ServerMaintenance Relay Cancel. type:{0}, message:{1}", type, message);
            }
        }
        void Brodcast_Reload_ServerData(int type)
        {
            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Room, out svr_array);
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    Proxy.LobbyRoomReloadServerData(svr.ServerHostID, CPackOption.Basic, type);
                }
            }
            else
            {
                Log._log.WarnFormat("Brodcast_Reload_ServerData Room Cancel. type:{0}", type);
            }
        }
        void Reload_ServerData_GiveMoney()
        {
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_GiveMoney = db.GameGiveMoney.FindAllByMoneyType(1).FirstOrDefault(); // 무료머니
                    RechargeFreeMoney = Data_GiveMoney.RechargeMoney;
                    RechargeFreeCount = Data_GiveMoney.RechargeCount;
                    Data_GiveMoney = db.GameGiveMoney.FindAllByMoneyType(2).FirstOrDefault(); // 유료머니
                    RechargePayMoney = Data_GiveMoney.RechargeMoney;
                    RechargePayCount = Data_GiveMoney.RechargeCount;
                    Log._log.InfoFormat("Reload_ServerData_GiveMoney Success.");

                    // 행운의 복권 이벤트 설정값
                    dynamic Data_EventLuckyLotto = db.EventLuckyLottery.FindAll().FirstOrDefault();
                    EventLuckyLottoCost = Data_EventLuckyLotto.Cost;
                    EventLuckyLottoLimited = Data_EventLuckyLotto.Limited;
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("Reload_ServerData_GiveMoney Failure. e:{0}", e.ToString());
                }
            });
        }
        void Brodcast_Kick_Player(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    Log._log.InfoFormat("Brodcast_Kick_Player. userId:{0}", userId);
                    // 프라우드넷
                    //ServerInfo[] svr_array;
                    //GetServerInfo(ServerType.Login, out svr_array);
                    //if (svr_array != null)
                    //{
                    //    foreach (var svr in svr_array)
                    //    {
                    //        Proxy.LobbyLoginKickUser(svr.ServerHostID, CPackOption.Basic, userId);
                    //    }
                    //}

                    // ZNET
                    bool UserInLobby = false;

                    dynamic Data_Current = db.GameCurrentUser.FindAllBy(UserID: userId, GameId: GameId).FirstOrDefault();
                    if (Data_Current != null)
                    {
                        // 회원이 로비에 있으면 강제 접속종료
                        // 모든 세션 종료
                        foreach (var relay in RemoteClients)
                        {
                            foreach (var client in relay.Value)
                            {
                                if (client.Value.data.ID != userId) continue;
                                UserInLobby = true;
                                ClientDisconect(relay.Key, client.Key, "kick from lobby");
                                Log._log.InfoFormat("Brodcast_Kick_Player success in Lobby. userId:{0}", userId);
                                break;
                            }
                        }

                        Rmi.Marshaler.LobbyUserList temp;
                        if (LobbyUserList.TryRemove(userId, out temp))
                        {
                            Brodcast_User_List();
                        }

                        //db.GameCurrentUser.DeleteByUserId(UserId: userId);
                    }

                    // 회원이 로비에 없으면 룸서버에서 찾아서 강제 접속종료
                    if (UserInLobby == false)
                    {
                        ServerInfo[] svr_array;
                        GetServerInfo(ServerType.Room, out svr_array);
                        if (svr_array != null)
                        {
                            foreach (var svr in svr_array)
                            {
                                Proxy.LobbyRoomKickUser(svr.ServerHostID, CPackOption.Basic, userId);
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("Brodcast_Kick_Player Failure. userId:{0}, e:{1}", userId, e.ToString());
                }
            });
        }
        void LobbyUserInfoUpdate(int userID, Rmi.Marshaler.LobbyUserList userInfo)
        {
            if (LobbyUserList.ContainsKey(userID) == false)
            {
                LobbyUserList.TryAdd(userID, userInfo);
            }
            else
            {
                userInfo.roomNumber = 0;
                LobbyUserList[userID] = userInfo;
            }
            Brodcast_User_List();
        }
        void LobbyUserInfoUpdate(int userID, long money_free, long money_pay)
        {
            if (LobbyUserList.ContainsKey(userID))
            {
                LobbyUserList[userID].FreeMoney = money_free;
                LobbyUserList[userID].PayMoney = money_pay;
            }
            Brodcast_User_List();
        }
        Rmi.Marshaler.LobbyUserInfo makeLobbyUserInfo(UserData data)
        {
            Rmi.Marshaler.LobbyUserInfo lobbyuserinfo = new Rmi.Marshaler.LobbyUserInfo();

            lobbyuserinfo.nickName = data.nickName;
            lobbyuserinfo.avatar = data.avatar;
            lobbyuserinfo.money_free = data.money_free;
            lobbyuserinfo.money_pay = data.money_pay;
            lobbyuserinfo.bank_money_free = data.bank_money_free;
            lobbyuserinfo.bank_money_pay = data.bank_money_pay;
            lobbyuserinfo.cash = data.cash;
            lobbyuserinfo.win = data.winCount;
            lobbyuserinfo.lose = data.loseCount;

            // 추가
            lobbyuserinfo.member_point = data.member_point;
            lobbyuserinfo.shop_name = data.shop_name;

            return lobbyuserinfo;
        }
        void Brodcast_Calling(int type, int chanId, string roomId, int playerId)
        {
            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Room, out svr_array);
            if (svr_array != null)
            {
                Guid roomGuid = Guid.Parse(roomId);
                foreach (var svr in svr_array)
                {
                    Proxy.LobbyRoomCalling(svr.ServerHostID, CPackOption.Basic, type, chanId, roomGuid, playerId);
                }
            }
            else
            {
                Log._log.WarnFormat("Brodcast_Calling Room Cancel. type:{0}, chanId:{1}, roomId:{2}, playerId:{3}", type, chanId, roomId, playerId);
            }
        }
        private void RestrictionShopRoom(string roomId, int restrict)
        {
            Guid gRoomID;

            if (Guid.TryParse(roomId, out gRoomID))
            {
                Rmi.Marshaler.RoomInfo roomInfo;
                if (RemoteRoomInfos.TryGetValue(gRoomID, out roomInfo))
                {
                    if (restrict == 0)
                    {
                        roomInfo.RestrictionShop = false;
                        Log._log.InfoFormat("RestrictionShopRoom. roomId:{0}, restrict:{1}", gRoomID, false);
                    }
                    else
                    {
                        roomInfo.RestrictionShop = true;
                        Log._log.InfoFormat("RestrictionShopRoom. roomId:{0}, restrict:{1}", gRoomID, true);
                    }
                }
                else
                {
                    Log._log.WarnFormat("RestrictionShopRoom Cancel (RemoteRoomInfos.TryGetValue failure). roomId:{0}", gRoomID);
                }
            }
            else
            {
                Log._log.WarnFormat("RestrictionShopRoom Cancel (Guid.TryParse failure). roomId:{0}", roomId);
            }
        }
        private void RestrictionUserRoom(string roomId, int restrict)
        {
            Guid gRoomID;

            if (Guid.TryParse(roomId, out gRoomID))
            {
                Rmi.Marshaler.RoomInfo roomInfo;
                if (RemoteRoomInfos.TryGetValue(gRoomID, out roomInfo))
                {
                    if (restrict == 0)
                    {
                        roomInfo.RestrictionUser = false;
                        Log._log.InfoFormat("RestrictionUserRoom. roomId:{0}, restrict:{1}", gRoomID, false);
                    }
                    else
                    {
                        roomInfo.RestrictionUser = true;
                        Log._log.InfoFormat("RestrictionUserRoom. roomId:{0}, restrict:{1}", gRoomID, true);
                    }
                }
                else
                {
                    Log._log.WarnFormat("RestrictionUserRoom Cancel (RemoteRoomInfos.TryGetValue failure). roomId:{0}", gRoomID);
                }
            }
            else
            {
                Log._log.WarnFormat("RestrictionUserRoom Cancel (Guid.TryParse failure). roomId:{0}", roomId);
            }
        }
        private void RestrictionRunRoom(string roomId, int restrict)
        {
            Guid gRoomID;

            if (Guid.TryParse(roomId, out gRoomID))
            {
                Rmi.Marshaler.RoomInfo roomInfo;
                if (RemoteRoomInfos.TryGetValue(gRoomID, out roomInfo))
                {
                    if (restrict == 0)
                    {
                        roomInfo.RestrictionRun = false;
                        Log._log.InfoFormat("RestrictionRunRoom. roomId:{0}, restrict:{1}", gRoomID, false);
                    }
                    else
                    {
                        roomInfo.RestrictionRun = true;
                        Log._log.InfoFormat("RestrictionRunRoom. roomId:{0}, restrict:{1}", gRoomID, true);
                    }
                }
                else
                {
                    Log._log.WarnFormat("RestrictionRunRoom Cancel (RemoteRoomInfos.TryGetValue failure). roomId:{0}", gRoomID);
                }
            }
            else
            {
                Log._log.WarnFormat("RestrictionRunRoom Cancel (Guid.TryParse failure). roomId:{0}", roomId);
            }
        }

        private void ServerNoticeMaintenance()
        {
            if (ServerMaintenance == false) return;

            string message = "[점검중] 점검중에는 서비스이용이 원활하지 않을 수 있으니 접속을 종료하시기 바랍니다.";
            foreach (var relay in RemoteClients)
            {
                foreach (var client in relay.Value)
                {
                    // 공지 타입 : 전체 공지, 로비 공지, 룸 공지 등
                    Proxy.LobbyRelayNotifyLobbyMessage(client.Value.Remote.Key, CPackOption.Basic, client.Value.Remote.Value, 1, message, 30);
                }
            }
            // 릴레이 서버에 공지 전달
            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Relay, out svr_array);
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    Proxy.LobbyRoomNotifyMessage(svr.ServerHostID, CPackOption.Basic, 1, message, 30);
                }
            }
            else
            {
                Log._log.WarnFormat("Brodcast_Notify_ServerMaintenance Relay Cancel. type:{0}, message:{1}", type, message);
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

                    //if (itemToRemove.ServerType == ServerType.Lobby)
                    if (itemToRemove.ServerType == ServerType.RelayLobby)
                    {
                        lock (LobbyLock)
                        {
                            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                            if (RemoteClients.TryRemove(itemToRemove.ServerHostID, out relayTemp))
                            {
                                foreach (var player in relayTemp.Values)
                                {
                                    CPlayer playerRemove;
                                    Rmi.Marshaler.LobbyUserList temp;
                                    LobbyUserList.TryRemove(player.data.ID, out temp);
                                    Log._log.WarnFormat("RelayLobby Leave. hostID:{0}, player:{1}", itemToRemove.ServerHostID, player.data.userID);
                                    RemoteClients2.TryRemove(player.data.ID, out playerRemove);
                                    DB_User_Logout(player.data.ID);
                                }
                            }
                        }
                    }
                    else if (itemToRemove.ServerType == ServerType.Room)
                    {
                        try
                        {
                            foreach (var room in RemoteRoomInfos)
                            {
                                if (room.Value.remote_svr == (int)remote)
                                {
                                    var Users = LobbyUserList.Where(o => o.Value.chanID == room.Value.chanID && o.Value.roomNumber != 0).Select(o => o.Key).ToList();
                                    for (int j = 0; j < Users.Count; ++j)
                                    {
                                        db.GameCurrentUser.DeleteByUserId(UserId: Users[j]);
                                        Rmi.Marshaler.LobbyUserList temp;
                                        LobbyUserList.TryRemove(Users[j], out temp);
                                    }
                                    db.GameRoomList.DeleteById(Id: room.Value.roomID);
                                    Rmi.Marshaler.RoomInfo temp_;
                                    RemoteRoomInfos.TryRemove(room.Value.roomID, out temp_);
                                }
                            }
                            Brodcast_Room_List();
                            Log._log.InfoFormat("MasterP2PMemberLeaveHandler Room Clear. RemoteID:{0}", itemToRemove.ServerHostID);
                        }
                        catch (Exception e)
                        {
                            Log._log.FatalFormat("MasterP2PMemberLeaveHandler Failure. RemoteID:{0}, e:{1}", itemToRemove.ServerHostID, e.ToString());
                        }
                    }
                    else if (itemToRemove.ServerType == ServerType.Relay)
                    {
                        RelayServerInfo info;
                        if (RemoteRelaysh.TryRemove(itemToRemove.ServerHostID, out info))
                        {
                            RelayServerInfo temp_;
                            RemoteRelays.TryRemove(info.RelayID, out temp_);
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
            Common.MoveParam param;
            CPlayer rc;
            Common.Common.ServerMoveParamRead(move_param, out param, out rc);
            //이 서버가 로비서버이므로 파라이터가 로비서버 일때만 승인해 준다.
            if (param.moveTo == Common.MoveParam.ParamMove.MoveToLobby)
                return true;
            return false;
        }
        void move_server_start_handler(RemoteID remote, out ArrByte userdata)
        {
            //CPlayer rc;
            //if (RemoteClients.TryGetValue(remote, out rc) == false)
            //{
            //    Log._log.WarnFormat("MoveServerStart UnkownRemote. remote:{0}", remote);
            userdata = null;
            //    CPlayer temp;
            //    RemoteClients.TryRemove(remote, out temp);
            //    //ClientDisconect(remote);
            //    return;
            //}
            //// 여기서는 이동할 서버로 동기화 시킬 유저 데이터를 구성하여 buffer에 넣어둔다->완료서버에서 해당 데이터를 그대로 받게된다
            //Common.Common.ServerMoveStart(rc, out userdata);
            ////Log._log.InfoFormat("move_server_start_handler. remote:{0}", remote);
        }
        void limit_connection_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("limit_connection_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void client_leave_handler(RemoteID remote, bool bMoveServer)
        {
            //ClientLeave(remote, bMoveServer);
            //Log._log.InfoFormat("client_leave_handler. remote:{0}, bMoveServer:{1}", remote, bMoveServer);
        }
        void client_join_handler(RemoteID remote, NetAddress addr, ArrByte move_server, ArrByte move_param)
        {
            //    lock (LobbyLock)
            //    {
            //        try
            //        {
            //            if (move_server.Count > 0)
            //            {
            //                Common.MoveParam param;
            //                CPlayer rc;
            //                Common.Common.ServerMoveParamRead(move_param, out param, out rc);

            //                // 이동정보 검사 필요
            //                rc.m_ip = addr.m_ip;
            //                rc.channelNumber = param.ChannelNumber;
            //                rc.Remote = remote;
            //                rc.RelayID = param.RelayID;

            //                // DB 플레이어 정보 불러오기
            //                int result = 1;
            //                try
            //                {
            //                    dynamic Data_Player = db.V_CurrentUserMoney.FindAllByUserId(rc.data.ID).FirstOrDefault();
            //                    rc.data.nickName = Data_Player.NickName;

            //                    bool AvatarUsing = false;
            //                    int DefaultAvatarId = 0;
            //                    string DefaultAvatar = "";
            //                    int DefaultAvatarVoice = 0;

            //                    bool CardUsing = false;
            //                    int DefaultCardId = 0;
            //                    string DefaultCard = "";

            //                    //rc.DayChangeMoney = Data_Player.DayChangeMoney;
            //                    rc.data.IPFree = Data_Player.IPFree;
            //                    rc.data.ShopFree = Data_Player.ShopFree;
            //                    rc.data.money_free = (long)Data_Player.FreeMoney;
            //                    //rc.data.money_pay = (long)Data_Player.PayMoney;
            //                    rc.data.member_point = (long)Data_Player.Point;
            //                    rc.data.bank_money_free = (long)Data_Player.BankFreeMoney;
            //                    //rc.data.bank_money_pay = (long)Data_Player.BankPayMoney;
            //                    if (Data_Player.Memo != null && Data_Player.Memo != "" && Data_Player.Memo.Contains("먹튀"))
            //                    {
            //                        rc.data.Restrict = true;
            //                    }

            //                    rc.EventLuckyLottoCount = Data_Player.EventLuckyLottoCount;

            //                    dynamic Data_Item = db.V_PlayerItemList.FindAllByUserId(rc.data.ID);
            //                    foreach (var row in Data_Item.ToList())
            //                    {
            //                        // 아이템 만료됐으면 기본으로 변경
            //                        switch (row.ptype)
            //                        {
            //                            case "avatar":
            //                                {
            //                                    if (row.Using == false)
            //                                    {
            //                                        if (AvatarUsing == false && row.value2 == 1)
            //                                        {
            //                                            DefaultAvatarId = row.Id;
            //                                            DefaultAvatar = row.string1;
            //                                            DefaultAvatarVoice = row.value1;
            //                                        }
            //                                        continue;
            //                                    }

            //                                    if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
            //                                    {
            //                                        db.PlayerItemList.UpdateById(Id: row.Id, Using: false);
            //                                    }
            //                                    else
            //                                    {
            //                                        rc.data.avatar = row.string1;
            //                                        rc.data.voice = row.value1;
            //                                        AvatarUsing = true;
            //                                    }
            //                                }
            //                                break;
            //                            case "card2":
            //                                {
            //                                    if (row.Using == false)
            //                                    {
            //                                        if (CardUsing == false && row.value2 == 1)
            //                                        {
            //                                            DefaultCardId = row.Id;
            //                                            DefaultCard = row.string1;
            //                                        }
            //                                        continue;
            //                                    }

            //                                    if (row.ExpireDate != null && row.ExpireDate < DateTime.Now)
            //                                    {
            //                                        db.PlayerItemList.UpdateById(Id: row.Id, Using: false);
            //                                    }
            //                                    else
            //                                    {
            //                                        rc.data.avatar_card = row.string1;
            //                                        CardUsing = true;
            //                                    }
            //                                }
            //                                break;
            //                        }
            //                    }
            //                    // 만료된 아이템은 기본 아이템으로 변경 착용
            //                    if (DefaultAvatarId != 0 && AvatarUsing == false)
            //                    {
            //                        db.PlayerItemList.UpdateById(Id: DefaultAvatarId, Using: true);
            //                        rc.data.avatar = DefaultAvatar;
            //                        rc.data.voice = DefaultAvatarVoice;
            //                    }
            //                    if (DefaultCardId != 0 && CardUsing == false)
            //                    {
            //                        db.PlayerItemList.UpdateById(Id: DefaultCardId, Using: true);
            //                        rc.data.avatar_card = DefaultCard;
            //                    }

            //                    DB_User_CurrentUpdate(rc.data.ID);

            //                    Rmi.Marshaler.LobbyUserList UserInfo = new Rmi.Marshaler.LobbyUserList();
            //                    UserInfo.ID = rc.data.ID;
            //                    UserInfo.RemoteID = rc.Remote.Value;
            //                    UserInfo.nickName = rc.data.nickName;
            //                    UserInfo.FreeMoney = rc.data.money_free;
            //                    UserInfo.PayMoney = rc.data.money_pay;
            //                    UserInfo.chanID = rc.channelNumber;
            //                    UserInfo.roomNumber = 0;
            //                    LobbyUserInfoUpdate(UserInfo.ID, UserInfo);

            //                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            //                    if (RemoteClients.TryGetValue(rc.Remote.Key, out relayTemp))
            //                    {
            //                        relayTemp.TryAdd(rc.Remote.Value, rc);
            //                    }
            //                    else
            //                    {
            //                        relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
            //                        relayTemp.TryAdd(rc.Remote.Value, rc);
            //                        RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
            //                    }
            //                    RemoteClients2.TryAdd(rc.data.ID, rc);

            //                    //Proxy.NotifyUserInfo(rc.remote, CPackOption.Basic, makeLobbyUserInfo(rc.data));
            //                    //Proxy.NotifyUserList(rc.remote, CPackOption.Basic, LobbyUserList.Values.ToList(), rc.friendList);
            //                    //Proxy.NotifyRoomList(rc.remote, CPackOption.Basic, rc.channelNumber, RemoteRoomInfos.Values.ToList());
            //                    //Proxy.NotifyJackpotInfo(rc.remote, CPackOption.Basic, this.JackPotMoney);
            //                }
            //                catch (Exception e)
            //                {
            //                    Log._log.ErrorFormat("Client Join LoadData Failed. Player:{0}, error:{1}", rc.data.userID, e.ToString());
            //                    result = 0;
            //                }

            //                if (result == 1)
            //                {
            //                    //Log._log.InfoFormat("ClientJoinHandler Success. IP:{0}, UserID:{1}, remoteC:{2}", clientIP, rc.data.userID, remoteC);
            //                }
            //                else
            //                {
            //                    //Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
            //                    Log._log.ErrorFormat("ClientJoinHandler Failure. IP:{0}, UserID:{1}", addr.m_ip, rc.data.userID);
            //                    ClientDisconect(remote, "Client Join Failure");
            //                }
            //            }
            //            else
            //            {
            //                Log._log.ErrorFormat("ClientJoinHandler None. IP:{0}", addr.m_ip);
            //                ClientDisconect(remote, "Client Join None");
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            //Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
            //            ClientDisconect(remote, "Client Join Exciption");
            //            Log._log.ErrorFormat("ClientJoinHandler Exciption. IP:{0}, e:{1}", addr.m_ip, e.ToString());
            //        }
            //    }
        }
        void client_disconnect_handler(RemoteID remote)
        {
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
                            Proxy.ServerRequestDataSync(serverInfo.ServerHostID, CPackOption.Basic, true);
                        }
                        else if (serverInfo.ServerType == ServerType.Relay)
                        {
                            RelayServerInfo relayInfo;
                            relayInfo.remote = serverInfo.ServerHostID;
                            relayInfo.RelayID = serverInfo.ServerID;
                            relayInfo.Addr = serverInfo.ServerAddrPort;
                            RemoteRelays.TryAdd(serverInfo.ServerID, relayInfo);
                            RemoteRelaysh.TryAdd(serverInfo.ServerHostID, relayInfo);
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Server Stub Handler

        bool RoomLobbyEventStart(RemoteID remote, CPackOption rmiContext, System.Guid roomID, int eventType)
        {
            try
            {
                Rmi.Marshaler.RoomInfo roomInfo;
                if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
                {
                    string NoticeMessage = "";

                    switch (eventType)
                    {
                        case 1: // 판수 이벤트
                            {
                                NoticeMessage = "[잭팟] ";

                                if (roomInfo.chanID == 5)
                                    NoticeMessage += "하급채널 ";
                                else if (roomInfo.chanID == 6)
                                    NoticeMessage += "고급채널 ";
                                else if (roomInfo.chanID == 7)
                                    NoticeMessage += "자유채널 ";
                                else if (roomInfo.chanID == 8)
                                    NoticeMessage += "자유2채널 ";
                                else
                                    NoticeMessage += " ";

                                NoticeMessage += roomInfo.number.ToString() + "번방에 잭팟이 나타났습니다!";
                            }
                            break;
                    }

                    Brodcast_Notify_Message(0, NoticeMessage, 5);

                    return true;
                }

            }
            catch (Exception e)
            {
                //form.printf("RoomLobbyEventStart 예외발생. {0}\n", e.ToString());
                return false;
            }

            return false;
        }
        bool RoomLobbyEventEnd(RemoteID remote, CPackOption rmiContext, System.Guid roomID, int eventType, string name, long rewards)
        {
            try
            {
                Rmi.Marshaler.RoomInfo roomInfo;
                if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
                {
                    string NoticeMessage = "";

                    switch (eventType)
                    {
                        case 1: // 판수 잭팟 이벤트
                            {
                                NoticeMessage = "[잭팟] ";

                                if (roomInfo.chanID == 5)
                                    NoticeMessage += "하급채널 ";
                                else if (roomInfo.chanID == 6)
                                    NoticeMessage += "고급채널 ";
                                else if (roomInfo.chanID == 7)
                                    NoticeMessage += "자유채널 ";
                                else if (roomInfo.chanID == 8)
                                    NoticeMessage += "자유2채널 ";
                                else
                                    NoticeMessage += " ";

                                NoticeMessage += roomInfo.number.ToString() + "번방에서 " + name + " 님이 ";

                                NoticeMessage += " ";

                                NoticeMessage += String.Format("{0:0,0}", rewards) + " 당첨되셨습니다.";
                            }
                            break;
                        case 2: // 골프 잭팟 이벤트
                            {
                                NoticeMessage = "[골프 잭팟] ";

                                if (roomInfo.chanID == 5)
                                    NoticeMessage += "하급채널 ";
                                else if (roomInfo.chanID == 6)
                                    NoticeMessage += "고급채널 ";
                                else if (roomInfo.chanID == 7)
                                    NoticeMessage += "자유채널 ";
                                else if (roomInfo.chanID == 8)
                                    NoticeMessage += "자유2채널 ";
                                else
                                    NoticeMessage += " ";

                                NoticeMessage += roomInfo.number.ToString() + "번방에서 " + name + " 님이 ";

                                NoticeMessage += " ";

                                NoticeMessage += String.Format("{0:0,0}", rewards) + " 당첨되셨습니다.";
                            }
                            break;
                    }

                    Brodcast_Notify_Message(0, NoticeMessage, 5);

                    return true;
                }

            }
            catch (Exception e)
            {
                //form.printf("RoomLobbyEventEnd 예외발생. {0}\n", e.ToString());
                return false;
            }

            return false; ;
        }
        bool RoomLobbyMakeRoom(RemoteID remote, CPackOption rmiContext, Rmi.Marshaler.RoomInfo roomInfo, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, string Pass, int shopId)
        {
            if (RemoteRoomInfos.TryAdd(roomInfo.roomID, roomInfo))
            {
                var chanKind = GetChannelKind(roomInfo.chanID);
                roomInfo.chanFree = GetChannelFree(roomInfo.chanID);
                roomInfo.baseMoney = GetStakeMoney(chanKind, roomInfo.stakeType);
                roomInfo.minMoney = GetMinimumMoney(chanKind, roomInfo.stakeType);
                //roomInfo.maxMoney = GetMaximumMoney(chanKind, roomInfo.stakeType);

                if (Pass == "")
                {
                    roomInfo.needPassword = false;
                }
                else
                {
                    roomInfo.needPassword = true;
                    roomInfo.roomPassword = Pass;
                }
                roomInfo.userList.TryAdd(userID, IP);
                roomInfo.userListShop.TryAdd(userID, shopId);
                LobbyUserInfoUpdate(userID, userInfo);

                int rooms;
                if (ChannelsRoomsCount.TryGetValue(roomInfo.chanID, out rooms))
                {
                    ++ChannelsRoomsCount[roomInfo.chanID];
                }

                Brodcast_Room_List();
                DBLog(userID, roomInfo.chanID, roomInfo.number, LOG_TYPE.방만들기, "판돈:" + roomInfo.baseMoney);
            }

            // 릴레이 서버에 알림
            //Proxy.RoomLobbyMakeRoom(RemoteRelaysh.Keys.ToArray(), rmiContext, roomInfo, userInfo, userID, IP, Pass);

            return true;
        }
        bool RoomLobbyJoinRoom(RemoteID remote, CPackOption rmiContext, System.Guid roomID, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, int shopId)
        {
            Rmi.Marshaler.RoomInfo roomInfo;
            if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo) == false) return false;

            roomInfo.userList.TryAdd(userID, IP);
            roomInfo.userListShop.TryAdd(userID, shopId);

            ++roomInfo.userCount;
            int max_users = 5;
            //if (roomInfo.chanID == 5)
            //    max_users = 8;
            if (roomInfo.userCount >= max_users)
            {
                roomInfo.restrict = true;
                Brodcast_Room_List();
            }

            LobbyUserInfoUpdate(userID, userInfo);
            DBLog(userID, roomInfo.chanID, roomInfo.number, LOG_TYPE.방입장, " 판돈:" + roomInfo.baseMoney);

            return true;
        }
        bool RoomLobbyOutRoom(RemoteID remote, CPackOption rmiContext, System.Guid roomID, int userID)
        {
            Rmi.Marshaler.RoomInfo roomInfo;
            if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo) == false) return false;

            string temp_;
            roomInfo.userList.TryRemove(userID, out temp_);
            int temp__;
            roomInfo.userListShop.TryRemove(userID, out temp__);

            --roomInfo.userCount;
            if (roomInfo.restrict == true)
            {
                roomInfo.restrict = false;
                Brodcast_Room_List();
            }

            if (roomInfo.userCount <= 0)
            {
                Rmi.Marshaler.RoomInfo temp;
                RemoteRoomInfos.TryRemove(roomID, out temp);

                int rooms;
                if (ChannelsRoomsCount.TryGetValue(roomInfo.chanID, out rooms))
                {
                    --ChannelsRoomsCount[roomInfo.chanID];
                }
                Brodcast_Room_List();
            }

            // 릴레이 서버에 알림
            //Proxy.RoomLobbyOutRoom(RemoteRelaysh.Keys.ToArray(), rmiContext, roomID, userID);

            if (LobbyUserList.ContainsKey(userID))
            {
                Rmi.Marshaler.LobbyUserList lobbyUserList;
                if (LobbyUserList.TryGetValue(userID, out lobbyUserList) && lobbyUserList.roomNumber != 0)
                {
                    Rmi.Marshaler.LobbyUserList temp;
                    LobbyUserList.TryRemove(userID, out temp);
                    Brodcast_User_List();
                }
            }
            DBLog(userID, roomInfo.chanID, roomInfo.number, LOG_TYPE.방퇴장, "");

            return true;
        }
        bool RoomLobbyRequestMoveRoom(RemoteID remote, CPackOption rmiContext, System.Guid roomIDNow, RemoteID remoteS, RemoteID userRemote, int userID, long money, bool ipFree, bool shopFree, int shopId, bool restrict)
        {
            string errorMsg = "잠시후 다시 시도하세요.";
            NetAddress addrTemp = new NetAddress();
            if (ServerMaintenance)
            {
                Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, false, Guid.Empty, addrTemp, 0, remoteS, userRemote, "※안내※\n서버 점검중입니다.");
                return false;
            }

            Rmi.Marshaler.RoomInfo CurrentRoom;
            if (RemoteRoomInfos.TryGetValue(roomIDNow, out CurrentRoom) == false)
            {
                Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, false, Guid.Empty, addrTemp, 0, remoteS, userRemote, errorMsg);
                return false;
            }

            int chanID = CurrentRoom.chanID;
            int stake = CurrentRoom.stakeType;
            string playerIP = "";
            CurrentRoom.userList.TryGetValue(userID, out playerIP);

            //if (CurrentRoom.userList.TryGetValue(userID, out playerIP) == false)
            //{
            //    // 에러
            //    //Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, false, Guid.Empty, addrTemp, 0, remoteS, userRemote, errorMsg);
            //    //return false;
            //}

            ChannelKind chanKind = GetChannelKind(chanID);

            // 바로입장 옵션값 검사
            if (IsExistChnnel(chanID) == false || IsExistStakeType(chanKind, stake) == false) return false;

            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Room, out svr_array);
            if (svr_array == null)
            {
                Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, false, Guid.Empty, addrTemp, 0, remoteS, userRemote, errorMsg);
                return false;
            }

            ServerInfo find_server = null;

            // 방 검색
            var rooms = RemoteRoomInfos.Values.ToList().OrderBy(o => Rnd.Next());
            Rmi.Marshaler.RoomInfo moveRoom = null;
            foreach (var room in rooms)
            //foreach (var room in RemoteRoomInfos)
            {
                if (room.chanID == chanID && room.stakeType == stake && room.restrict == false && room.needPassword == false && room.roomID != roomIDNow)
                {
                    if (money < room.baseMoney)
                    {
                        errorMsg = "방이동할 수 있는 최소 금액은 " + room.baseMoney + " 입니다.";
                        Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, false, Guid.Empty, addrTemp, 0, remoteS, userRemote, errorMsg);
                        return false;
                    }
                    //else if (money > room.maxMoney)
                    //{
                    //    errorMsg = "방이동할 수 있는 최대 금액은 " + room.maxMoney + " 입니다.";
                    //    Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, false, Guid.Empty, addrTemp, 0, userRemote, errorMsg);
                    //    return false;
                    //}
                    // 룸 서버 검색
                    foreach (var svr in svr_array)
                    {
                        // 해당 방이 존재하는지 확인
                        if ((RemoteID)room.remote_svr == svr.ServerHostID)
                        {
                            if (chanKind == ChannelKind.무료1채널 || chanKind == ChannelKind.무료2채널)
                            {
                                // 입장 가능한 IP, 샵인지 확인
                                if (ipFree == false)
                                {
                                    bool IPOverLap = false;
                                    foreach (var roomUser in room.userList)
                                    {
                                        if (roomUser.Value == playerIP)
                                        {
                                            IPOverLap = true;
                                            break;
                                        }
                                    }

                                    if (IPOverLap)
                                    {
                                        continue;
                                    }

                                }
                                if (shopFree == false)
                                {
                                    if (shopId != 0)
                                    {
                                        bool ShopOverLap = false;
                                        foreach (var roomUser in room.userListShop)
                                        {
                                            if (roomUser.Value == shopId)
                                            {
                                                ShopOverLap = true;
                                                break;
                                            }
                                        }

                                        if (ShopOverLap)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (room.RestrictionShop == true)
                                {
                                    bool restrictShop;
                                    if (RestrictionShop.TryGetValue(shopId, out restrictShop))
                                    {
                                        if (restrictShop == true)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (room.RestrictionUser == true)
                                {
                                    bool restrictUser;
                                    if (RestrictionShop.TryGetValue(shopId, out restrictUser))
                                    {
                                        if (restrict == false)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                if (room.RestrictionRun == true)
                                {
                                    if (restrict == true)
                                    {
                                        continue;
                                    }
                                }
                            }

                            moveRoom = room;
                            find_server = svr;
                            break;
                        }
                    }
                }
                if (find_server != null)
                    break;
            }

            if (find_server == null)
            {
                // 해당 서버 없음 : 방만들기
                foreach (var svr in svr_array)
                {
                    Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, true, Guid.NewGuid(), svr.ServerAddrPort, chanID, remoteS, userRemote, "");
                    return true;
                }
                return false;
            }

            Proxy.LobbyRoomResponseMoveRoom(remote, rmiContext, false, moveRoom.roomID, find_server.ServerAddrPort, chanID, remoteS, userRemote, "");
            DBLog(userID, moveRoom.chanID, moveRoom.number, LOG_TYPE.방이동, "");

            return true;
        }
        bool RoomLobbyResponseDataSync(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            CMessage msg = data;
            int chanID; Rmi.Marshaler.Read(msg, out chanID);
            if (ChannelsRoomsCount.TryAdd(chanID, 0) == false)
            {
                ChannelsRoomsCount.TryUpdate(chanID, 0, 0);
            }

            int RoomCount; Rmi.Marshaler.Read(msg, out RoomCount);

            for (int i = 0; i < RoomCount; ++i)
            {
                Rmi.Marshaler.RoomInfo roominfo = new Rmi.Marshaler.RoomInfo();

                Rmi.Marshaler.Read(msg, out roominfo.roomID);
                Rmi.Marshaler.Read(msg, out roominfo.chanID);
                Rmi.Marshaler.Read(msg, out roominfo.chanType);
                Rmi.Marshaler.Read(msg, out roominfo.chanFree);
                Rmi.Marshaler.Read(msg, out roominfo.number);
                Rmi.Marshaler.Read(msg, out roominfo.stakeType);
                Rmi.Marshaler.Read(msg, out roominfo.baseMoney);
                Rmi.Marshaler.Read(msg, out roominfo.minMoney);
                Rmi.Marshaler.Read(msg, out roominfo.maxMoney);
                Rmi.Marshaler.Read(msg, out roominfo.userCount);
                Rmi.Marshaler.Read(msg, out roominfo.restrict);
                Rmi.Marshaler.Read(msg, out roominfo.remote_svr);
                Rmi.Marshaler.Read(msg, out roominfo.remote_lobby);
                Rmi.Marshaler.Read(msg, out roominfo.needPassword);
                Rmi.Marshaler.Read(msg, out roominfo.roomPassword);

                for (int j = 0; j < roominfo.userCount; ++j)
                {
                    string IP;
                    Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                    //
                    Rmi.Marshaler.Read(msg, out userInfo.ID);
                    Rmi.Marshaler.Read(msg, out IP);
                    roominfo.userList.TryAdd(userInfo.ID, IP);
                    int ShopID;
                    Rmi.Marshaler.Read(msg, out ShopID);
                    roominfo.userListShop.TryAdd(userInfo.ID, ShopID);

                    Rmi.Marshaler.Read(msg, out userInfo.RemoteID);
                    Rmi.Marshaler.Read(msg, out userInfo.nickName);
                    Rmi.Marshaler.Read(msg, out userInfo.FreeMoney);
                    Rmi.Marshaler.Read(msg, out userInfo.PayMoney);
                    userInfo.chanID = roominfo.chanID;
                    userInfo.roomNumber = roominfo.number;
                    LobbyUserList.TryAdd(userInfo.ID, userInfo);
                }

                RemoteRoomInfos.TryAdd(roominfo.roomID, roominfo);
                ChannelsRoomsCount[roominfo.chanID]++;
            }

            Log._log.InfoFormat("RoomLobbyResponseDataSync Success. remote:{0}, chanID:{1}", remote, chanID);

            return true;
        }
        bool RoomLobbyMessage(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string message)
        {
            Proxy.ResponseLobbyMessage(userRemote, rmiContext, message);

            return true;
        }

        bool RelayClientJoin(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC, ZNet.NetAddress addr, ZNet.ArrByte moveData)
        {
            string clientIP = addr.addr.ToString();

            lock (LobbyLock)
            {
                try
                {
                    // 서버 이동 데이터 불러오기
                    Common.MoveParam param;
                    CPlayer rc;
                    Common.Common.ServerMoveParamRead2(moveData, out param, out rc);

                    // 이동정보 검사 필요

                    rc.m_ip = clientIP;
                    rc.channelNumber = param.ChannelNumber;
                    rc.Remote = new KeyValuePair<RemoteID, RemoteID>(remoteS, remoteC);
                    rc.RelayID = param.RelayID;

                    // DB 플레이어 정보 불러오기
                    int result = 1;
                    try
                    {
                        // View
                        dynamic Data_IsView = db.PlayerBadugiViewer.FindAllByUserID(rc.data.ID).FirstOrDefault();
                        if (Data_IsView != null)
                        {
                            rc.data.Old = true;
                        }
                        else
                        {
                            rc.data.Old = false;
                        }

                        dynamic Data_Player = db.V_CurrentUserMoney.FindAllByUserId(rc.data.ID).FirstOrDefault();
                        rc.data.nickName = Data_Player.NickName;

                        bool AvatarUsing = false;
                        int DefaultAvatarId = 0;
                        string DefaultAvatar = "";
                        int DefaultAvatarVoice = 0;

                        bool CardUsing = false;
                        int DefaultCardId = 0;
                        string DefaultCard = "";

                        //rc.DayChangeMoney = Data_Player.DayChangeMoney;
                        rc.data.IPFree = Data_Player.IPFree;
                        rc.data.ShopFree = Data_Player.ShopFree;
                        rc.data.money_free = (long)Data_Player.FreeMoney;
                        //rc.data.money_pay = (long)Data_Player.PayMoney;
                        rc.data.member_point = (long)Data_Player.Point;
                        rc.data.bank_money_free = (long)Data_Player.BankFreeMoney;
                        //rc.data.bank_money_pay = (long)Data_Player.BankPayMoney;
                        if (Data_Player.Memo != null && Data_Player.Memo != "" && Data_Player.Memo.Contains("먹튀"))
                        {
                            rc.data.Restrict = true;
                        }

                        rc.EventLuckyLottoCount = Data_Player.EventLuckyLottoCount;

                        dynamic Data_Item = db.V_PlayerItemList.FindAllByUserId(rc.data.ID);
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
                                            rc.data.avatar = row.string1;
                                            rc.data.voice = row.value1;
                                            AvatarUsing = true;
                                        }
                                    }
                                    break;
                                case "card2":
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
                                            rc.data.avatar_card = row.string1;
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
                            rc.data.avatar = DefaultAvatar;
                            rc.data.voice = DefaultAvatarVoice;
                        }
                        if (DefaultCardId != 0 && CardUsing == false)
                        {
                            db.PlayerItemList.UpdateById(Id: DefaultCardId, Using: true);
                            rc.data.avatar_card = DefaultCard;
                        }

                        DB_User_CurrentUpdate(rc.data.ID);

                        Rmi.Marshaler.LobbyUserList UserInfo = new Rmi.Marshaler.LobbyUserList();
                        UserInfo.ID = rc.data.ID;
                        UserInfo.RemoteID = rc.Remote.Value;
                        UserInfo.nickName = rc.data.nickName;
                        UserInfo.FreeMoney = rc.data.money_free;
                        UserInfo.PayMoney = rc.data.money_pay;
                        UserInfo.chanID = rc.channelNumber;
                        UserInfo.roomNumber = 0;
                        LobbyUserInfoUpdate(UserInfo.ID, UserInfo);

                        ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                        if (RemoteClients.TryGetValue(rc.Remote.Key, out relayTemp))
                        {
                            relayTemp.TryAdd(rc.Remote.Value, rc);
                        }
                        else
                        {
                            relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                            relayTemp.TryAdd(rc.Remote.Value, rc);
                            RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                        }
                        RemoteClients2.TryAdd(rc.data.ID, rc);

                        //Proxy.NotifyUserInfo(rc.remote, CPackOption.Basic, makeLobbyUserInfo(rc.data));
                        //Proxy.NotifyUserList(rc.remote, CPackOption.Basic, LobbyUserList.Values.ToList(), rc.friendList);
                        //Proxy.NotifyRoomList(rc.remote, CPackOption.Basic, rc.channelNumber, RemoteRoomInfos.Values.ToList());
                        //Proxy.NotifyJackpotInfo(rc.remote, CPackOption.Basic, this.JackPotMoney);
                    }
                    catch (Exception e)
                    {
                        Log._log.ErrorFormat("Client Join LoadData Failed. Player:{0}", rc.data.userID);
                        result = 0;
                    }

                    if (result == 1)
                    {
                        //Log._log.InfoFormat("ClientJoinHandler Success. IP:{0}, UserID:{1}, remoteC:{2}", clientIP, rc.data.userID, remoteC);
                    }
                    else
                    {
                        Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
                        Log._log.ErrorFormat("ClientJoinHandler Failure. IP:{0}, UserID:{1}", clientIP, rc.data.userID);
                        ClientDisconect(remoteS, remoteC, "Client Join Failure");
                        DB_User_Logout(rc.data.ID);
                    }
                }
                catch (Exception e)
                {
                    Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
                    Log._log.ErrorFormat("ClientJoinHandler Exciption. IP:{0}, e:{1}", clientIP, e.ToString());
                    return false;
                }
            }
            return true;
        }
        bool RelayClientLeave(RemoteID remote, CPackOption rmiContext, RemoteID remoteC, bool bMoveServer)
        {
            int userID = -1;

            lock (LobbyLock)
            {
                try
                {
                    CPlayer rc;

                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                    if (RemoteClients.TryGetValue(remote, out relayTemp))
                    {
                        if (relayTemp.TryGetValue(remoteC, out rc))
                        {
                            userID = rc.data.ID;
                            Rmi.Marshaler.LobbyUserList temp;
                            LobbyUserList.TryRemove(userID, out temp);
                            //Log._log.InfoFormat("ClientLeave Success. hostID:{0}, player:{1}", remoteC, rc.data.userID);

                        }
                        else
                        {
                            Log._log.WarnFormat("ClientLeave Cancel.(relayTemp) hostID:{0}", remoteC);
                        }
                    }
                    else
                    {
                        Log._log.WarnFormat("ClientLeave Cancel.(RemoteClients) hostID:{0}", remoteC);
                    }

                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("ClientLeave Exception. hostID:{0}, e:{1}", remoteC, e.ToString());
                }

                ConcurrentDictionary<RemoteID, CPlayer> relayTemp2;
                if (RemoteClients.TryGetValue(remote, out relayTemp2))
                {
                    CPlayer playerRemove;
                    if (relayTemp2.TryRemove(remoteC, out playerRemove))
                    {
                        RemoteClients2.TryRemove(playerRemove.data.ID, out playerRemove);
                    }
                }

                if (bMoveServer == false && userID != -1)
                {
                    Rmi.Marshaler.LobbyUserList temp;
                    LobbyUserList.TryRemove(userID, out temp);
                    DB_User_Logout(userID);
                }
            }
            return true;
        }


        bool RelayServerMoveFailure(RemoteID remote, CPackOption rmiContext, RemoteID remoteC)
        {
            Log._log.InfoFormat("RelayServerMoveFailure. remote:{0}, remoteC:{1}", remote, remoteC);
            return true;
        }
        bool RelayRequestLobbyKey(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string id, string key, int gameid)
        {
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "※안내※\n서버 점검중입니다.");
                return false;
            }

            Task.Run(() =>
            {
                CPlayer rc = null;
                ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                if (RemoteClients.TryGetValue(remote, out relayTemp))
                    if (relayTemp.TryGetValue(userRemote, out rc) == false) return;

                if (rc == null) return;
                try
                {
                    string newkey = "";
                    //dynamic Data_Password = db.PlayerPassword.FindAllByUserID(rc.data.ID).FirstOrDefault();
                    //if (Data_Password.Password == key)
                    {
                        System.Security.Cryptography.SHA1 sha = System.Security.Cryptography.SHA1.Create();
                        newkey = HexStringFromBytes(sha.ComputeHash(Encoding.UTF8.GetBytes(id + DateTime.Now.ToString() + "vong")));

                        db.PlayerPassword.UpdateByUserId(UserId: rc.data.ID, Password: newkey, CreatedOnUtc: DateTime.Now);
                    }
                    Proxy.LobbyRelayResponseLobbyKey(remote, CPackOption.Basic, userRemote, newkey, gameid);
                }
                catch (Exception e)
                {
                    Log._log.ErrorFormat("RequestLobbyKey Failure. e:{0}", e.ToString());
                }
            });

            return true;
        }
        bool RelayRequestJoinInfo(RemoteID remote, CPackOption rmiContext, RemoteID userRemote)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (relayTemp.TryGetValue(userRemote, out rc))
                {
                    Proxy.LobbyRelayNotifyRoomList(remote, CPackOption.Basic, userRemote, rc.channelNumber, RemoteRoomInfos.Values.ToList());
                    Proxy.LobbyRelayNotifyUserInfo(remote, CPackOption.Basic, userRemote, makeLobbyUserInfo(rc.data));
                    Proxy.LobbyRelayNotifyUserList(remote, CPackOption.Basic, userRemote, LobbyUserList.Values.ToList(), rc.friendList);
                    Proxy.LobbyRelayNotifyJackpotInfo(remote, CPackOption.Basic, userRemote, this.JackPotMoney);
                }

            return true;
        }
        bool RelayRequestChannelMove(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, int chanID)
        {
            ChannelType chanType = GetChannelType(chanID);
            if (chanType == ChannelType.None) return false;

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (relayTemp.TryGetValue(userRemote, out rc))
                {
                    switch (chanType)
                    {
                        case ChannelType.Free:
                            {
                                // 무료머니 자동 충전
                                if (rc.data.money_free + rc.data.bank_money_free < RechargeFreeMoney)
                                {
                                    if (RechargeFreeCount > 0) // 충전 횟수 확인
                                    {
                                        foreach (var row in db.Lobby_RechargeMoney(rc.data.ID, true))
                                        {
                                            if (row.ok == 1)
                                            {
                                                rc.data.money_free += row.reMoney;

                                                if (RechargeFreeCount >= 100)
                                                {
                                                    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "무료충전\n" + row.reMoney + " 충전됐습니다.");
                                                }
                                                else
                                                {
                                                    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "무료충전\n" + row.reMoney + " 충전됐습니다.\n남은 충전 횟수 :" + (RechargeFreeCount - row.reCount) + " 회");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case ChannelType.Charge:
                            {
                                // 유료머니 자동 충전
                                if (rc.data.money_pay + rc.data.bank_money_pay < RechargePayMoney)
                                {
                                    if (RechargePayCount > 0) // 충전 횟수 확인
                                    {
                                        foreach (var row in db.Lobby_RechargeMoney(rc.data.ID, false))
                                        {
                                            if (row.ok == 1)
                                            {
                                                rc.data.money_pay += row.reMoney;

                                                if (RechargePayCount >= 100)
                                                {
                                                    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "무료충전\n" + row.reMoney + " 충전됐습니다.");
                                                }
                                                else
                                                {
                                                    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "무료충전\n" + row.reMoney + " 충전됐습니다.\n남은 충전 횟수 :" + (RechargePayCount - row.reCount) + " 회");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }

                    rc.channelNumber = chanID;
                    Rmi.Marshaler.LobbyUserList temp;
                    if (LobbyUserList.TryGetValue(rc.data.ID, out temp))
                    {
                        temp.chanID = chanID;
                        Brodcast_User_List();
                    }
                }

            return true;
        }
        bool RelayRequestRoomMake(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, int relayID, int chanID, int betType, string pass)
        {
            string errorMsg = "방만들기 실패\n 잠시후 다시 시도하세요.";
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "※안내※\n서버 점검중입니다.");
                return false;
            }

            if (pass.Length > 20) // 비밀번호가 너무 김
            {
                errorMsg = "방만들기 실패\n 비밀번호가 너무 깁니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            ChannelKind chanKind = GetChannelKind(chanID);
            ChannelType chanType = GetChannelType(chanID);

            // 방만들기 옵션값 검사
            if (IsExistChnnel(chanID) == false || IsExistStakeType(chanKind, betType) == false)
            {
                errorMsg = "방만들기 실패\n 만들수 없는 방 옵션입니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            int limit;
            if (ChannelsRoomsCount.TryGetValue(chanID, out limit) && limit >= RoomLimit)
            {
                errorMsg = "방 만들기 실패\n 현재 채널은 더 이상 방을 만들 수 없습니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            long playerMoney;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (relayTemp.TryGetValue(userRemote, out rc))
                {
                    // 손실한도 제한
                    if (rc.DayChangeMoney < -100000000000) // 일일 손실한도 1000억
                    {
                        int timeHour = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours;
                        string limitTime = "";
                        if (timeHour > 0)
                            limitTime = timeHour.ToString() + " 시간";
                        else
                            limitTime = (DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes.ToString() + " 분";
                        errorMsg = "일일 손실한도를 초과했습니다. " + limitTime + " 후 다시 이용할 수 있습니다.";
                        Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                        return false;
                    }

                    if (chanType == ChannelType.Charge)
                        playerMoney = rc.data.money_pay;
                    else
                        playerMoney = rc.data.money_free;

                    long minMoney = GetMinimumMoney(chanKind, betType);
                    //long maxMoney = GetMaximumMoney(chanKind, betType);
                    if (playerMoney < minMoney)
                    {
                        errorMsg = "방을 만들 수 있는 최소 금액은 " + minMoney + " 입니다.";
                        Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                        return false;
                    }
                    //else if (playerMoney > maxMoney)
                    //{
                    //    errorMsg = "방을 만들 수 있는 최대 금액은 " + maxMoney + " 입니다.";
                    //    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                    //    return false;
                    //}
                }
                else
                {
                    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                    return false;
                }

            // 릴레이 서버 확인
            RelayServerInfo relayInfo;
            if (RemoteRelays.TryGetValue(relayID, out relayInfo))
            {
            }
            else // 해당하는 릴레이가 없으면 입장 불가
            {
                // 릴레이 서버 없음 : 이동 실패
                errorMsg = "방입장 실패!\n입장할 수 있는 방이 없습니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            // 이동 파라미터 구성
            ArrByte param_buffer;
            Common.MoveParam param = new Common.MoveParam();
            param.From(Common.MoveParam.ParamMove.MoveToRoom, Common.MoveParam.ParamRoom.RoomMake, Guid.NewGuid(), (int)this.ServerHostID, chanID, betType, pass, relayInfo.RelayID);
            Common.Common.ServerMoveParamWrite(param, rc, out param_buffer);

            Proxy.LobbyRelayServerMoveStart(remote, CPackOption.Basic, userRemote, relayInfo.Addr.m_ip, relayInfo.Addr.m_port, param_buffer);
            return true;
        }
        bool RelayRequestRoomJoin(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, int relayID, int chanID, int betType)
        {
            string errorMsg = "방입장 실패\n 잠시후 다시 시도하세요.";
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "※안내※\n서버 점검중입니다.");
                return false;
            }

            ChannelKind chanKind = GetChannelKind(chanID);
            ChannelType chanType = GetChannelType(chanID);

            // 바로입장 옵션값 검사
            if (IsExistChnnel(chanID) == false || IsExistStakeType(chanKind, betType) == false) return false;

            // 룸 서버에 요청
            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Room, out svr_array);
            if (svr_array == null)
            {
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            ServerInfo find_server = null;
            Guid roomID = Guid.Empty;

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            long playerMoney = 0;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (relayTemp.TryGetValue(userRemote, out rc))
                {
                    // 손실한도 제한
                    if (rc.DayChangeMoney < -100000000000) // 일일 손실한도 1000억
                    {
                        int timeHour = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours;
                        string limitTime = "";
                        if (timeHour > 0)
                            limitTime = timeHour.ToString() + " 시간";
                        else
                            limitTime = (DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes.ToString() + " 분";
                        errorMsg = "일일 손실한도를 초과했습니다. " + limitTime + " 후 다시 이용할 수 있습니다.";
                        Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                        return false;
                    }
                }
                else
                {
                    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                    return false;
                }

            // 방 검색

            var rooms = RemoteRoomInfos.Values.ToList().OrderBy(o => Rnd.Next());
            foreach (var room in rooms)
            //foreach (var room in RemoteRoomInfos)
            {
                if (room.chanType == (int)ChannelType.Charge)
                    playerMoney = rc.data.money_pay;
                else
                    playerMoney = rc.data.money_free;

                if (room.chanID == chanID && room.stakeType == betType && room.restrict == false && room.needPassword == false)
                {
                    if (playerMoney < room.minMoney)
                    {
                        errorMsg = "입장할 수 있는 최소 금액은 " + room.minMoney + " 입니다.";
                        Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                        return false;
                    }
                    //else if (playerMoney > room.maxMoney)
                    //{
                    //    errorMsg = "입장할 수 있는 최대 금액은 " + room.maxMoney + " 입니다.";
                    //    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                    //    return false;
                    //}
                    // 서버 검색
                    foreach (var svr in svr_array)
                    {
                        // 해당 방이 존재하는지 확인
                        if ((RemoteID)room.remote_svr == svr.ServerHostID)
                        {
                            if (chanKind == ChannelKind.무료1채널 || chanKind == ChannelKind.무료2채널)
                            {
                                // 입장 가능한 IP인지 확인
                                if (rc.data.IPFree == false)
                                {
                                    bool IPOverLap = false;
                                    foreach (var roomUser in room.userList)
                                    {
                                        if (roomUser.Value == rc.m_ip)
                                        {
                                            IPOverLap = true;
                                            break;
                                        }
                                    }

                                    if (IPOverLap)
                                    {
                                        continue;
                                    }

                                }
                                if (rc.data.ShopFree == false)
                                {
                                    if (rc.data.shopId != 0 && rc.data.shopId != 6)
                                    {
                                        bool ShopOverLap = false;
                                        foreach (var roomUser in room.userListShop)
                                        {
                                            if (roomUser.Value == rc.data.shopId)
                                            {
                                                ShopOverLap = true;
                                                break;
                                            }
                                        }

                                        if (ShopOverLap)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (room.RestrictionShop == true)
                                {
                                    bool restrict;
                                    if (RestrictionShop.TryGetValue(rc.data.shopId, out restrict))
                                    {
                                        if (restrict == true)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (room.RestrictionUser == true)
                                {
                                    bool restrict;
                                    if (RestrictionShop.TryGetValue(rc.data.shopId, out restrict))
                                    {
                                        if (restrict == false)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                if (room.RestrictionRun == true)
                                {
                                    if (rc.data.Restrict == true)
                                    {
                                        continue;
                                    }
                                }
                            }

                            roomID = room.roomID;
                            find_server = svr;
                            break;
                        }
                    }
                }
                if (find_server != null)
                    break;
            }

            if (find_server == null)
            {
                // 해당 서버 없음 : 이동 실패
                errorMsg = "방입장 실패\n입장할 수 있는 방이 없습니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            // 릴레이 서버 확인
            RelayServerInfo relayInfo;
            if (RemoteRelays.TryGetValue(relayID, out relayInfo))
            {

            }
            else // 해당하는 릴레이가 없으면 입장 불가
            {
                // 릴레이 서버 없음 : 이동 실패
                errorMsg = "방입장 실패!\n입장할 수 있는 방이 없습니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            // 이동 파라미터 구성
            ArrByte param_buffer;
            Common.MoveParam param = new Common.MoveParam();
            param.From(Common.MoveParam.ParamMove.MoveToRoom, Common.MoveParam.ParamRoom.RoomJoin, roomID, (int)this.ServerHostID, chanID, betType, "", relayInfo.RelayID);
            Common.Common.ServerMoveParamWrite(param, rc, out param_buffer);

            Proxy.LobbyRelayServerMoveStart(remote, CPackOption.Basic, userRemote, relayInfo.Addr.m_ip, relayInfo.Addr.m_port, param_buffer);
            return true;
        }
        bool RelayRequestRoomJoinSelect(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, int relayID, int chanID, int roomNumber, string pass)
        {
            string errorMsg = "방입장 실패\n 잠시후 다시 시도하세요.";
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "※안내※\n서버 점검중입니다.");
                return false;
            }

            // 특정 방을 입장할 수 있는 채널인지 검사
            if (chanID != 4 && chanID != 7 && chanID != 8)
            {
                //errorMsg = "방입장 실패\n 입장할 수 없는 채널입니다.";
                //Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                //return false;
                Log._log.WarnFormat("클릭입장 확인 userRemote:{0}", userRemote);
            }

            if (pass.Length > 20) // 비밀번호가 너무 김
            {
                errorMsg = "방입장 실패\n 비밀번호가 너무 깁니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            int betType = 0;
            // 룸 서버에 요청
            ServerInfo[] svr_array;
            GetServerInfo(ServerType.Room, out svr_array);
            if (svr_array == null)
            {
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            ServerInfo find_server = null;
            Guid roomID = Guid.Empty;

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            long playerMoney = 0;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (relayTemp.TryGetValue(userRemote, out rc))
                {
                    // 손실한도 제한
                    if (rc.DayChangeMoney < -100000000000) // 일일 손실한도 1000억
                    {
                        int timeHour = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours;
                        string limitTime = "";
                        if (timeHour > 0)
                            limitTime = timeHour.ToString() + " 시간";
                        else
                            limitTime = (DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes.ToString() + " 분";
                        errorMsg = "일일 손실한도를 초과했습니다. " + limitTime + " 후 다시 이용할 수 있습니다.";
                        Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                        return false;
                    }
                }
                else
                {
                    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                    return false;
                }

            ChannelKind chanKind = GetChannelKind(chanID);
            ChannelType chanType = GetChannelType(chanID);

            if (chanType == ChannelType.Charge)
                playerMoney = rc.data.money_pay;
            else
                playerMoney = rc.data.money_free;

            // 방 검색
            var rooms = RemoteRoomInfos.Values.ToList().OrderBy(o => Rnd.Next());
            foreach (var room in rooms)
            //foreach (var room in RemoteRoomInfos)
            {
                if (room.number == roomNumber && room.chanID == chanID && room.restrict == false)
                {
                    if (playerMoney < room.minMoney)
                    {
                        errorMsg = "입장할 수 있는 최소 금액은 " + room.minMoney + " 입니다.";
                        Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                        return false;
                    }
                    //else if (playerMoney > room.maxMoney)
                    //{
                    //    errorMsg = "입장할 수 있는 최대 금액은 " + room.maxMoney + " 입니다.";
                    //    Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                    //    return false;
                    //}
                    // 룸 서버 검색
                    foreach (var svr in svr_array)
                    {
                        // 해당 방이 존재하는지 확인
                        if ((RemoteID)room.remote_svr == svr.ServerHostID)
                        {
                            if (room.roomPassword != pass)
                            {
                                // 방은 있지만 비밀번호가 다름
                                errorMsg = "방입장 실패\n비밀번호가 다릅니다.";
                                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                                return false;
                            }

                            roomID = room.roomID;
                            betType = room.stakeType;
                            find_server = svr;
                            break;
                        }
                    }
                }
                if (find_server != null)
                    break;
            }

            if (find_server == null)
            {
                // 해당 서버 없음 : 이동 실패
                errorMsg = "방입장 실패\n입장할 수 있는 방이 없습니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            // 릴레이 서버 확인
            RelayServerInfo relayInfo;
            if (RemoteRelays.TryGetValue(relayID, out relayInfo))
            {
            }
            else // 해당하는 릴레이가 없으면 입장 불가
            {
                // 릴레이 서버 없음 : 이동 실패
                errorMsg = "방입장 실패!\n입장할 수 있는 방이 없습니다.";
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, errorMsg);
                return false;
            }

            // 이동 파라미터 구성
            ArrByte param_buffer;
            Common.MoveParam param = new Common.MoveParam();
            param.From(Common.MoveParam.ParamMove.MoveToRoom, Common.MoveParam.ParamRoom.RoomJoin, roomID, (int)this.ServerHostID, chanID, betType, "", relayInfo.RelayID);
            Common.Common.ServerMoveParamWrite(param, rc, out param_buffer);

            Proxy.LobbyRelayServerMoveStart(remote, CPackOption.Basic, userRemote, relayInfo.Addr.m_ip, relayInfo.Addr.m_port, param_buffer);
            return true;
        }
        bool RelayRequestBank(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, int option, long money, string pass)
        {
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponseLobbyMessage(remote, CPackOption.Basic, userRemote, "※안내※\n서버 점검중입니다.");
                return false;
            }

            if (money <= 0)
            {
                Proxy.LobbyRelayResponseBank(remote, CPackOption.Basic, userRemote, false, 2);
            }

            CPlayer rc;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (relayTemp.TryGetValue(userRemote, out rc))
                {
                    DB_User_UpdateBank(remote, userRemote, rc, option, money, pass);
                    return true;
                }

            return false;
        }
        bool RelayRequestPurchaseList(RemoteID remote, CPackOption rmiContext, RemoteID userRemote)
        {
            // 웹에서 처리

            return true;
        }
        bool RelayRequestPurchaseAvailability(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string pid)
        {
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponsePurchaseAvailability(remote, CPackOption.Basic, userRemote, false, "※안내※\n서버 점검중입니다.");
                return false;
            }

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (!relayTemp.TryGetValue(userRemote, out rc))
                {
                    Proxy.LobbyRelayResponsePurchaseAvailability(remote, CPackOption.Basic, userRemote, false, "안내\n잠시후 다시 시도해주세요.");
                    return false;
                }

            Task.Run(() =>
            {
                try
                {
                    bool available = true;
                    string reason = "";

                    dynamic Data_Product = db.V_MobileShop.FindAllBypid(pid: pid).FirstOrDefault();

                    if (Data_Product == null || Data_Product.sale == false)
                    {
                        available = false;
                        reason = "판매중인 상품이 아닙니다. \n(pid:" + pid.ToString() + ")";
                    }

                    // 인앱 결제만 허용
                    if (Data_Product.purchase_kind != "inapp")
                    {
                        available = false;
                        reason = "인앱결제 상품이 아닙니다.";
                    }

                    // 모바일 월 결제한도
                    dynamic Data_Payment = db.V_PurchaseAndroidMonth.FindAllByPlayerId(rc.data.ID).FirstOrDefault();
                    //if (true)
                    if (Data_Payment != null && Data_Payment.PurchaseMoney + Data_Product.price >= 550000)
                    {
                        available = false;
                        reason = "월 결제한도를 초과합니다.\n(현재 결제금액 : " + Data_Payment.PurchaseMoney.ToString() + ")";
                    }

                    /*
                    if (available == false)
                    {
                        db._error.Insert(Type: GameId, CMessage: rc.data.ID.ToString() + ") 모바일 : inapp구매가능확인 실패 : " + reason);
                    }
                    */

                    Proxy.LobbyRelayResponsePurchaseAvailability(remote, CPackOption.Basic, userRemote, available, reason);
                }
                catch (Exception e)
                {
                    db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : inapp구매가능확인 오류 : " + e.ToString());
                    Log._log.ErrorFormat("RequestPurchaseAvailability Error. userID:{0}, e:{1}", rc.data.userID, e.ToString());
                }
            });

            return true;
        }
        bool RelayRequestPurchaseReceiptCheck(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string result)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (!relayTemp.TryGetValue(userRemote, out rc))
                {
                    Proxy.LobbyRelayResponsePurchaseReceiptCheck(remote, CPackOption.Basic, userRemote, false, Guid.Empty);
                    return false;
                }

            Task.Run(() =>
            {
                try
                {
                    bool check = false;
                    Guid token = Guid.Empty;

                    var result_json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                    var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result_json["json"]);
                    string orderId = json["orderId"];

                    dynamic Data_Check = db.PurchaseAndroid.FindAllByorderId(orderId: orderId).FirstOrDefault();
                    if (Data_Check == null)
                    {
                        string packageName = json["packageName"];
                        string productId = json["productId"];
                        string purchaseTime = json["purchaseTime"];
                        string purchaseState = json["purchaseState"];
                        string purchaseToken = json["purchaseToken"];

                        string signature = result_json["signature"];

                        //string publicKey = "?";
                        //string license = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApyK4yE1097Bo+l4OO4pFE92KccqlMeXXbyG/wr64Es+i/2nwlZXV7lfxV+djqaOO4LygGrPgpw2JMTaLaphxgoKCka6fxwsEx4W1yqSHHLpDBOpLX3I+GWYSIe8Rx0zWNhXjE+60+e/f6FvY/LcLXiVolY8pzBsyJLnSEH9xh0oCmlWaVRJmzSSkG+g93O3ZRxBRvcT7eTru7h0vioqv3fC9zTzkr7eDn2K82itEfnFLDp70EApxFO/SuB86Mlxy+SPJgkp6iXMDlSS3klXmLdueSTqAI5jf9yKy57zkvWygcMfI9bMjL42dVTYe8H6pPLI5XWdQqtW25Jl+Ld6j0QIDAQAB";

                        //check = Server.Common.AndroidInAppPurchase.Varify_android(purchaseToken, signature, publicKey);
                        check = true; // 테스트

                        if (check)
                        {
                            // 검증 완료
                            token = Guid.NewGuid();

                            db.PurchaseAndroid.Insert(guid: token, PlayerId: rc.data.ID, orderId: orderId, packageName: packageName, productId: productId, purchaseTime: purchaseTime, purchaseState: purchaseState, purchaseToken: purchaseToken, signature: signature, Success: false);

                            // 캐쉬 지급
                            dynamic Data_Product = db.V_MobileShop.FindAllBypid(pid: productId).FirstOrDefault();
                            dynamic Data_Money = db.PlayerGameMoney.FindAllByUserID(rc.data.ID).FirstOrDefault();
                            int cash = rc.data.cash = Data_Money.cash;
                            cash += Data_Product.paidvalue1;
                            rc.data.cash = cash;
                            db.PlayerGameMoney.UpdateByUserID(UserID: rc.data.ID, Cash: cash);

                            // 확인
                            db.PurchaseAndroid.UpdateBytoken(guid: token, Success: true);
                            db.LogPurchase.Insert(UserId: rc.data.ID, pid: Data_Product.pid, pname: Data_Product.pname, purchase_kind: Data_Product.purchase_kind, price: Data_Product.price, Location: "mobile");
                        }
                    }

                    if (check == false)
                    {
                        db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : 영수증 검증 실패 : ");
                    }

                    Proxy.LobbyRelayResponsePurchaseReceiptCheck(remote, CPackOption.Basic, userRemote, check, token);
                }
                catch (Exception e)
                {
                    db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : 영수증 검증 오류 : " + e.ToString());
                    Log._log.ErrorFormat("RequestPurchaseReceiptCheck Error. userID:{0}, e:{1}\n", rc.data.userID, e.ToString());
                }
            });

            return true;
        }
        bool RelayRequestPurchaseResult(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, System.Guid token)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (!relayTemp.TryGetValue(userRemote, out rc))
                {
                    Proxy.LobbyRelayResponsePurchaseResult(remote, CPackOption.Basic, userRemote, false, "오류! 고객지원에 문의해주세요.");
                    return false;
                }

            Task.Run(() =>
            {
                try
                {
                    bool check = false;
                    string reason = "";

                    if (token == Guid.Empty)
                    {
                        reason = "비정상적인 요청입니다.";
                    }
                    else
                    {
                        dynamic Data_Purchase = db.PurchaseAndroid.FindAllByguid(guid: token).FirstOrDefault();

                        if (Data_Purchase == null || Data_Purchase.Success != false)
                        {
                            reason = "승인되지 않은 결제입니다. 고객지원에 문의해주세요.";
                        }
                        else
                        {
                            // 결제 종료
                            check = true;
                        }
                    }

                    if (check == false)
                    {
                        db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : 최종 처리 실패 : " + reason);
                    }

                    Proxy.LobbyRelayResponsePurchaseResult(remote, CPackOption.Basic, userRemote, check, reason);
                }
                catch (Exception e)
                {
                    db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : 최종 처리 오류 : " + e.ToString());
                    Log._log.ErrorFormat("RequestPurchaseReceiptCheck Error. userID:{0}, e:{1}\n", rc.data.userID, e.ToString());
                }
            });

            return true;
        }
        bool RelayRequestPurchaseCash(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string pid)
        {
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponsePurchaseCash(remote, CPackOption.Basic, userRemote, false, "※안내※\n서버 점검중입니다.");
                return false;
            }

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (!relayTemp.TryGetValue(userRemote, out rc))
                {
                    Proxy.LobbyRelayResponsePurchaseCash(remote, CPackOption.Basic, userRemote, false, "잠시후 다시 시도하세요.");
                    return false;
                }

            Task.Run(() =>
            {
                try
                {
                    bool result = false;
                    string reason = "";

                    dynamic Data_Product = db.V_MobileShop.FindAllBypid(pid: pid).FirstOrDefault();

                    if (Data_Product == null || Data_Product.sale == false)
                        reason = "판매중인 상품이 아닙니다.";
                    else
                    {
                        // 게임머니 상품인지 확인
                        if (Data_Product.purchase_kind == "gamemoney")
                        {
                            if (Data_Product.ptype == "avatar" ||
                            Data_Product.ptype == "card2" ||
                            Data_Product.ptype == "evt")
                            {
                                // 상품 구매
                                dynamic Data_Money = db.PlayerGameMoney.FindAllByUserID(rc.data.ID).FirstOrDefault();
                                long GameMoney = (long)Data_Money.GameMoney;

                                if (GameMoney >= Data_Product.price)
                                {
                                    dynamic Data_Item = db.PlayerItemList.FindAllBy(UserID: rc.data.ID, ItemId: Data_Product.productid).FirstOrDefault();
                                    DateTime updatetime;
                                    if (Data_Item != null)
                                    {
                                        if (Data_Item.ExpireDate > DateTime.Now)
                                        {
                                            updatetime = Data_Item.ExpireDate.AddDays(Data_Product.paidvalue3);
                                        }
                                        else
                                        {
                                            updatetime = DateTime.Now.AddDays(Data_Product.paidvalue3);
                                        }
                                        db.PlayerItemList.UpdateById(Id: Data_Item.Id, ItemId: Data_Product.productid, Count: 1, ExpireDate: updatetime);
                                    }
                                    else
                                    {
                                        updatetime = DateTime.Now.AddDays(Data_Product.paidvalue3);
                                        db.PlayerItemList.Insert(UserId: rc.data.ID, ItemId: Data_Product.productid, Count: 1, ExpireDate: updatetime);
                                    }
                                    GameMoney -= Data_Product.price;

                                    rc.data.money_free = GameMoney;

                                    db.PlayerGameMoney.UpdateByUserId(UserId: rc.data.ID, GameMoney: GameMoney);
                                    db.LogPurchase.Insert(UserId: rc.data.ID, pid: Data_Product.pid, pname: Data_Product.pname, purchase_kind: Data_Product.purchase_kind, price: Data_Product.price, Location: "mobile");

                                    result = true;
                                }
                                else
                                    reason = "게임머니가 부족합니다.";
                            }
                            else
                                reason = "판매중인 상품이 아닙니다.";
                        }
                        else
                            reason = "판매중인 상품이 아닙니다.";
                    }

                    if (result == false)
                    {
                        db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : 아이템 구매 실패 : " + reason);
                    }

                    Proxy.LobbyRelayResponsePurchaseCash(remote, CPackOption.Basic, userRemote, result, reason);
                }
                catch (Exception e)
                {
                    db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : 아이템 구매 오류 : " + e.ToString());
                    Log._log.ErrorFormat("RequestPurchaseCash Error. userID:{0}, e:{1}", rc.data.userID, e.ToString());
                }
            });

            return true;
        }
        bool RelayRequestMyroomList(RemoteID remote, CPackOption rmiContext, RemoteID userRemote)
        {
            // 웹에서 처리

            return true;
        }
        bool RelayRequestMyroomAction(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, string pid)
        {
            if (ServerMaintenance)
            {
                Proxy.LobbyRelayResponseMyroomAction(remote, CPackOption.Basic, userRemote, pid, false, "※안내※\n서버 점검중입니다.");
                return false;
            }

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (!relayTemp.TryGetValue(userRemote, out rc))
                {
                    Proxy.LobbyRelayResponseMyroomAction(remote, CPackOption.Basic, userRemote, pid, false, "잠시후 다시 시도하세요.");
                    return false;
                }

            Task.Run(() =>
            {
                try
                {
                    bool result = false;
                    string reason = "잠시후 다시 시도하세요.";

                    dynamic Data_Item = db.V_PlayerItemList.FindAllBy(Id: pid, UserId: rc.data.ID).FirstOrDefault();

                    if (Data_Item == null)
                    {
                        reason = "보유중인 아이템이 아닙니다.";
                    }
                    else
                    {
                        if (Data_Item.ptype == "avatar" ||
                        Data_Item.ptype == "card2" ||
                        Data_Item.ptype == "evt")
                        {
                            // 만료일 확인
                            if (Data_Item.ExpireDate == null || Data_Item.ExpireDate >= DateTime.Now)
                            {
                                // 개수 확인
                                if (Data_Item.Count >= 1)
                                {
                                    // 아이템 사용, 이전 아이템 사용 중지
                                    if (Data_Item.ptype == "avatar")
                                    {
                                        dynamic Data_ItemUsing = db.V_PlayerItemList.FindAllBy(UserId: rc.data.ID, ptype: Data_Item.ptype, Using: true).FirstOrDefault();
                                        if (Data_ItemUsing != null)
                                        {
                                            if (Data_Item.Id != Data_ItemUsing.Id)
                                            {
                                                db.PlayerItemList.UpdateById(Id: Data_ItemUsing.Id, Using: false);
                                                db.PlayerItemList.UpdateById(Id: Data_Item.Id, Using: true);
                                                rc.data.avatar = Data_Item.string1;
                                                rc.data.voice = Data_Item.value1;
                                                result = true;
                                                reason = "";
                                            }
                                            else
                                            {
                                                reason = "이미 사용중인 아이템입니다.";
                                            }
                                        }
                                    }
                                    else if (Data_Item.ptype == "card2")
                                    {
                                        dynamic Data_ItemUsing = db.V_PlayerItemList.FindAllBy(UserId: rc.data.ID, ptype: Data_Item.ptype, Using: true).FirstOrDefault();
                                        if (Data_ItemUsing != null)
                                        {
                                            if (Data_Item.Id != Data_ItemUsing.Id)
                                            {
                                                db.PlayerItemList.UpdateById(Id: Data_ItemUsing.Id, Using: false);
                                                db.PlayerItemList.UpdateById(Id: Data_Item.Id, Using: true);
                                                rc.data.avatar_card = Data_Item.string1;
                                                result = true;
                                                reason = "";
                                            }
                                            else
                                            {
                                                reason = "이미 사용중인 아이템입니다.";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        reason = "사용할 수 없는 아이템 입니다.";
                                    }
                                }
                                else
                                    reason = "수량이 부족합니다.";
                            }
                            else
                                reason = "기간이 만료된 아이템입니다.";
                        }
                        else
                            reason = "사용할 수 없는 아이템입니다.";
                    }

                    Proxy.LobbyRelayResponseMyroomAction(remote, CPackOption.Basic, userRemote, pid, result, reason);
                }
                catch (Exception e)
                {
                    db._error.Insert(Type: GameId, CMessage: rc.data.userID + ") 모바일 : 마이룸 액션 오류 : " + e.ToString());
                    Log._log.ErrorFormat("RequestMyroomAction Error. userID:{0}, e:{1}", rc.data.userID, e.ToString());
                }
            });

            return true;
        }
        bool RelayRequestLobbyEventInfo(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, ArrByte data)
        {
            if (ServerMaintenance)
            {
                CMessage msg = new CMessage();
                msg.Write(false);
                msg.Write("※안내※\n서버 점검중입니다.");
                Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventInfo, msg);
                return false;
            }

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (!relayTemp.TryGetValue(userRemote, out rc))
                {
                    CMessage msg = new CMessage();
                    msg.Write(false);
                    msg.Write("잠시후 다시 시도하세요.");
                    Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventInfo, msg);
                    return false;
                }

            CMessage msg_ = new CMessage();
            msg_.Write(true);
            msg_.Write(EventLuckyLottoCost); // 이벤트 참여 비용
            msg_.Write(EventLuckyLottoLimited); // 이벤트 최대 참여횟수
            msg_.Write(EventLuckyLottoLimited - rc.EventLuckyLottoCount); // 남은 참여 횟수
            msg_.Write(rc.data.money_free); // 현재머니
            Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventInfo, msg_);

            return true;
        }
        bool RelayRequestLobbyEventParticipate(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            if (ServerMaintenance)
            {
                CMessage msg = new CMessage();
                msg.Write(false);
                msg.Write("※안내※\n서버 점검중입니다.");
                Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventParticipate, msg);
                return false;
            }

            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remote, out relayTemp))
                if (!relayTemp.TryGetValue(userRemote, out rc))
                {
                    CMessage msg = new CMessage();
                    msg.Write(false);
                    msg.Write("잠시후 다시 시도하세요.");
                    Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventParticipate, msg);
                    return false;
                }

            if (rc.data.money_free < EventLuckyLottoCost)
            {
                CMessage msg = new CMessage();
                msg.Write(false);
                msg.Write("게임머니가 부족합니다.");
                Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventParticipate, msg);
                return false;
            }

            if (rc.EventLuckyLottoCount >= EventLuckyLottoLimited)
            {
                CMessage msg = new CMessage();
                msg.Write(false);
                msg.Write("남아있는 이용횟수가 없습니다.");
                Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventParticipate, msg);
                return false;
            }

            // DB 프로시저
            foreach (var row in db.Lobby_EventLuckyLotto(rc.data.ID))
            {
                if (row.ok == 1)
                {
                    rc.EventLuckyLottoCount = row.LeftLimited;
                    rc.data.money_free = row.AfterMoney;

                    CMessage msg_ = new CMessage();
                    msg_.Write(true);
                    msg_.Write((long)row.RewardMoney);
                    msg_.Write(EventLuckyLottoLimited - rc.EventLuckyLottoCount);
                    msg_.Write((long)rc.data.money_free);
                    Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventParticipate, msg_);
                    DBLog(rc.data.ID, 0, 0, LOG_TYPE.행운의복권, row.RewardMoney.ToString());
                }
                else
                {
                    CMessage msg_ = new CMessage();
                    msg_.Write(false);
                    switch (row.result)
                    {
                        case 1:
                            {
                                msg_.Write("게임머니가 부족합니다.");
                            }
                            break;
                        case 2:
                            {
                                msg_.Write("남아있는 이용횟수가 없습니다.");
                            }
                            break;
                        default:
                            {
                                msg_.Write("※이벤트 참여실패※\n고객 지원센터에 문의하시기 바랍니다.");
                            }
                            break;
                    }
                    Send(remote, userRemote, SS.Common.LobbyRelayResponseLobbyEventParticipate, msg_);
                }
            }

            return true;
        }

        #endregion

        #region DB
        void DB_Server_CurrentPlayerClear()
        {
            try
            {
                db.GameCurrentUser.DeleteByGameId(GameId: GameId);
                db.GameCurrentDummy.DeleteByGameId(GameId: GameId);
                db.GameRoomList.DeleteByGameId(GameId: GameId);
                Log._log.InfoFormat("DB_Server_CurrentPlayerClear Success.");
            }
            catch (Exception e)
            {
                Log._log.FatalFormat("DB_Server_CurrentPlayerClear Failure. e:{0}", e.ToString());
            }
        }
        void DB_Server_GetLobbyData()
        {
            // 서버 설정값 확인
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_JackPot = db.GameJackPotMoney.All().FirstOrDefault();

                    JackPotMoney = (long)Data_JackPot.JackPotMoney;

                    dynamic Data_GiveMoney = db.GameGiveMoney.FindAllByMoneyType(1).FirstOrDefault(); // 무료머니
                    RechargeFreeMoney = Data_GiveMoney.RechargeMoney;
                    RechargeFreeCount = Data_GiveMoney.RechargeCount;
                    Data_GiveMoney = db.GameGiveMoney.FindAllByMoneyType(2).FirstOrDefault(); // 유료머니
                    RechargePayMoney = Data_GiveMoney.RechargeMoney;
                    RechargePayCount = Data_GiveMoney.RechargeCount;

                    // 행운의 복권 이벤트 설정값
                    dynamic Data_EventLuckyLotto = db.EventLuckyLottery.FindAllByid(1).FirstOrDefault();
                    EventLuckyLottoCost = Data_EventLuckyLotto.Cost;
                    EventLuckyLottoLimited = Data_EventLuckyLotto.Limited;

                    dynamic Data_RestriectStop = db.GameRoomRestrictShop.FindAllByGameId(GameId);
                    if (Data_RestriectStop != null)
                    {
                        foreach (var row in Data_RestriectStop.ToList())
                        {
                            if (RestrictionShop.TryAdd(row.ShopId, true))
                            {
                                Log._log.InfoFormat("RestrictionShop Add. ShopId:{0}", row.ShopId);
                            }
                            else
                            {
                                Log._log.WarnFormat("RestrictionShop Cancel. ShopId:{0}", row.ShopId);
                            }
                        }
                    }

                    Log._log.InfoFormat("DB_Server_GetLobbyData Success. JackPotMoney:{0}", JackPotMoney);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_Server_GetLobbyData Failure. e:{0}", e.ToString());
                }
            });
        }
        void DB_Server_GetServerMessage()
        {
            // 서버 메시지 처리
            Task.Run(async () =>
            {
                List<ServerMessage> MessagePool = new List<ServerMessage>();

                var dbResult = await Task.Run(() =>
                {
                    try
                    {
                        dynamic Data_ServerMessage = db.GameServerMessage.FindAllByGameId(GameId).ToList();

                        if (Data_ServerMessage == null || Data_ServerMessage.Count == 0) return 0;

                        foreach (var CMessage in Data_ServerMessage)
                        {
                            if (CMessage.Date >= DateTime.Now) continue;
                            ServerMessage data;
                            int Id = CMessage.Id;
                            data.type = CMessage.Type;
                            data.value1 = CMessage.Value1;
                            data.value2 = CMessage.Value2;
                            data.value3 = CMessage.Value3;
                            data.value4 = CMessage.Value4;
                            data.value5 = CMessage.Value5;
                            data.string1 = CMessage.String1;
                            data.string2 = CMessage.String2;
                            MessagePool.Add(data);
                            db.GameServerMessage.DeleteById(Id);
                        }

                        if (MessagePool.Count == 0) return 0;

                        return 1;
                    }
                    catch (Exception e)
                    {
                        Log._log.FatalFormat("DB_Server_GetServerMessage Failure. e:{0}", e.ToString());
                    }

                    return 0;
                });

                if (dbResult == 1)
                {
                    // DBCall 처리
                    foreach (var message in MessagePool)
                    {
                        switch (message.type)
                        {
                            case 0:
                                {
                                    Log._log.InfoFormat("로비서버 데이터 다시 불러오기");
                                    DB_Server_GetLobbyData();
                                }
                                break;
                            case 1:
                                {
                                    Log._log.InfoFormat("공지사항 : {0}\n", message.string1);
                                    Brodcast_Notify_Message(message.value1, message.string1, message.value2);
                                }
                                break;
                            case 2:
                                {
                                    Log._log.InfoFormat("서버점검 : {0}\n", message.string1);
                                    Brodcast_Notify_ServerMaintenance(message.value1, message.string1, message.value2);
                                }
                                break;
                            case 3:
                                {
                                    Log._log.InfoFormat("서버데이터 다시 불러오기 : {0}\n", message.value1);
                                    Brodcast_Reload_ServerData(message.value1);
                                }
                                break;
                            case 4:
                                {
                                    Log._log.InfoFormat("회원 강제로 접속종료 : {0}\n", message.value1);
                                    Brodcast_Kick_Player(message.value1);
                                }
                                break;
                            case 5:
                                {
                                    Log._log.InfoFormat("지급머니 다시 불러오기");
                                    Reload_ServerData_GiveMoney();
                                }
                                break;
                            case 6:
                                {
                                    Log._log.InfoFormat("콜 기능");
                                    Brodcast_Calling(message.value1, message.value2, message.string1, message.value3);
                                }
                                break;
                            case 7:
                                {
                                    Log._log.InfoFormat("방 샵 입장 제한");
                                    RestrictionShopRoom(message.string1, message.value1);
                                }
                                break;
                            case 8:
                                {
                                    Log._log.InfoFormat("방 유저 입장 제한");
                                    RestrictionUserRoom(message.string1, message.value1);
                                }
                                break;
                            case 9:
                                {
                                    Log._log.InfoFormat("방 유저 입장 제한");
                                    RestrictionRunRoom(message.string1, message.value1);
                                }
                                break;

                        }
                        Log._log.InfoFormat("DB_Server_GetServerMessage Success. type:{0}, message:{1}", message.type, message.string1);
                    }
                }
                else
                {
                    //Log._log.WarnFormat("DB_Server_GetServerMessage Cancel.");
                }
            });
        }
        void DB_Server_SendPlayerInfo()
        {
            // 로비에 있는 플레이어 정보 전송
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_UserMoney = db.V_CurrentUserMoney.FindAllByGameId(GameId);
                    foreach (var row in Data_UserMoney.ToList())
                    {
                        Rmi.Marshaler.LobbyUserList User;
                        if (LobbyUserList.TryGetValue(row.UserId, out User))
                        {
                            CPlayer rc;
                            if (RemoteClients2.TryGetValue(row.UserId, out rc))
                            {
                                rc.data.money_free = (long)row.FreeMoney;
                                rc.data.money_pay = (long)row.PayMoney;
                                rc.data.member_point = (long)row.Point;
                                rc.data.bank_money_free = (long)row.BankFreeMoney;
                                rc.data.bank_money_pay = (long)row.BankPayMoney;
                                rc.data.IPFree = row.IPFree;
                                rc.data.ShopFree = row.ShopFree;
                                if (row.Memo != null && row.Memo != "" && row.Memo.Contains("먹튀"))
                                {
                                    rc.data.Restrict = true;
                                }
                            }

                            User.FreeMoney = (long)row.FreeMoney;
                            User.PayMoney = (long)row.PayMoney;
                        }
                    }
                    Brodcast_User_Info();
                    Brodcast_User_List();

                    //Log._log.InfoFormat("DB_Server_SendPlayerInfo Success.");
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_Server_SendPlayerInfo Failure. e:{0}", e.ToString());
                }
            });
        }
        void DB_Server_SendJackPotMoney()
        {
            // 잭팟금액 전송
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_JackPot = db.GameJackPotMoney.All().FirstOrDefault();

                    JackPotMoney = (long)Data_JackPot.JackPotMoney;

                    // 로비에 있는 유저에게 일괄 전송
                    foreach (var relay in RemoteClients)
                    {
                        foreach (var user in relay.Value)
                        {
                            Proxy.LobbyRelayNotifyJackpotInfo(user.Value.Remote.Key, CPackOption.Basic, user.Value.Remote.Value, this.JackPotMoney);
                        }
                    }
                    //Proxy.NotifyJackpotInfo(RemoteClients.Keys.ToArray(), CPackOption.Encrypt, this.JackPotMoney);

                    // 룸 서버에 일괄 전송
                    ServerInfo[] svr_array;
                    GetServerInfo(ServerType.Room, out svr_array);
                    if (svr_array != null)
                    {
                        foreach (var svr in svr_array)
                        {
                            Proxy.LobbyRoomJackpotInfo(svr.ServerHostID, CPackOption.Basic, this.JackPotMoney);
                        }
                    }

                    // 릴레이 서버에 일괄 전송
                    GetServerInfo(ServerType.Relay, out svr_array);
                    if (svr_array != null)
                    {
                        foreach (var svr in svr_array)
                        {
                            Proxy.LobbyRoomJackpotInfo(svr.ServerHostID, CPackOption.Basic, this.JackPotMoney);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_Server_SendJackPotMoney Failure. e:{0}", e.ToString());
                }

                return;
            });
        }
        void DB_User_Login(int userId, string ip)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.Insert(UserId: userId, Locate: 0, GameId: GameId, ChannelId: 0, RoomId: 0, IP: ip, AutoPlay: false);
                    Log._log.InfoFormat("DB_User_Login Success. userId:{0}, ip:{1}", userId, ip);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_User_Login Failure. userId:{0}, ip:{1}, e:{2}", userId, ip, e.ToString());
                }
            });
        }
        void DB_User_CurrentUpdate(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.UpdateByUserId(UserId: userId, ChannelId: 0, RoomId: 0);
                    //Log._log.InfoFormat("DB_User_CurrentUpdate Success. userId:{0}", userId);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_User_CurrentUpdate Failure. userId:{0}, e:{1}", userId, e.ToString());
                }
            });
        }
        void DB_User_Logout(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.DeleteByUserId(UserId: userId);
                    Log._log.InfoFormat("DB_User_Logout Success. userId:{0}", userId);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_User_Logout Failure. userId:{0}, e:{1}", userId, e.ToString());
                }
            });
        }
        void DB_User_UpdateBank(RemoteID remote, RemoteID userRemote, CPlayer user, int option, long money, string password)
        {
            if (money <= 0) return;

            // 금고 금액, 비밀번호 확인
            Task.Run(async () =>
            {
                var dbResult = await Task.Run(() =>
                {
                    try
                    {
                        foreach (var row in db.Lobby_PlayerBank(user.data.ID, option, money, password))
                        {
                            user.data.money_free = (long)row.FreeMoney;
                            user.data.money_pay = (long)row.PayMoney;
                            user.data.bank_money_free = (long)row.SafeMoney;
                            user.data.member_point = (long)row.MemberPoint;
                        }
                    }
                    catch (Exception e)
                    {
                        Log._log.FatalFormat("DB_User_UpdateBank Failure1. userId:{0}, e:{1}", user.data.ID, e.ToString());

                        return 1;
                    }

                    return 0;
                });

                // 인증 성공
                if (dbResult == 0)
                {
                    Proxy.LobbyRelayResponseBank(remote, CPackOption.Basic, userRemote, true, 0);
                    Proxy.LobbyRelayNotifyUserInfo(remote, CPackOption.Basic, userRemote, makeLobbyUserInfo(user.data));
                    LobbyUserInfoUpdate(user.data.ID, user.data.money_free, user.data.money_pay);

                    //Log._log.InfoFormat("DB_User_UpdateBank Success. userId:{0}, option:{1}, money:{2}", user.data.ID, option, money);
                }
                else
                {
                    Proxy.LobbyRelayResponseBank(remote, CPackOption.Basic, userRemote, false, dbResult); // 1:에러, 2: 비밀번호 틀림

                    Log._log.ErrorFormat("DB_User_UpdateBank Cancel. userId:{0}, dbResult:{1}", user.data.ID, dbResult);
                }

                return;
            });
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
                    //Rmi.Marshaler.Write(Msg, Datas.Count());
                    fixed (byte* pData = &Datas[0])
                    {
                        Msg.Write(pData, Datas.Count());
                    }
                }
            }
            return Proxy.PacketSend(remote, CPackOption.Basic, Msg);
        }
        #endregion
    }
}
