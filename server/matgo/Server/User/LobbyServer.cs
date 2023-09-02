using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityCommon;
using ZNet;

namespace Server.User
{
    class LobbyServer : UserServer
    {
        //클라이언트 목록
        ConcurrentDictionary<ZNet.RemoteID, CPlayer> RemoteClients = new ConcurrentDictionary<ZNet.RemoteID, CPlayer>();
        ConcurrentDictionary<ZNet.RemoteID, int> RemoteRelays = new ConcurrentDictionary<ZNet.RemoteID, int>();
        // 방 목록
        ConcurrentDictionary<Guid, RemoteClass.Marshaler.RoomInfo> RemoteRoomInfos = new ConcurrentDictionary<Guid, RemoteClass.Marshaler.RoomInfo>();
        // 채널별 방 수
        ConcurrentDictionary<int, int> ChannelsRoomsCount = new ConcurrentDictionary<int, int>();
        // 채널당 방생성 한도
        int RoomLimit = 500;

        // 전송할 로비 유저 목록
        ConcurrentDictionary<int, RemoteClass.Marshaler.LobbyUserList> LobbyUserList = new ConcurrentDictionary<int, RemoteClass.Marshaler.LobbyUserList>();
        Random rngRoom = new Random(new System.DateTime().Millisecond);

        // 이 서버의 remoteID : 이 로비서버에서 만들어진 방에서 다시 나갈때 원래의 로비서버로 돌아갈때 구분하기 위해서
        int svrRemoteID;
        long JackPotMoney = 0;    // 잭팟 금액

        long RechargeFreeMoney = 0;
        int RechargeFreeCount = 0;
        long RechargePayMoney = 0;
        int RechargePayCount = 0;

        public LobbyServer(FormServer f, UnityCommon.Server t, int portnum) : base(f, t, portnum)
        {
            DB_Server_CurrentPlayerClear();
            DB_Server_GetLobbyData();

        }

        ~LobbyServer()
        {
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            base.BeforeServerStart(out param);
            param.m_UpdateTimeMs = 1000;
            // 주기적으로 업데이트할 필요가 있는 내용들...
            m_Core.update_event_handler = ScheduleTask;

            stub.master_all_shutdown = ShutDownServer;

            // 모바일 상점,결제,마이룸
            stub.request_purchase_list = RequestPurchaseList;
            stub.request_purchase_availability = RequestPurchaseAvailability;
            stub.request_purchase_receipt_check = RequestPurchaseReceiptCheck;
            stub.request_purchase_result = RequestPurchaseResult;
            stub.request_purchase_cash = RequestPurchaseCash;
            stub.request_myroom_list = RequestMyroomList;
            stub.request_myroom_action = RequestMyroomAction;

            // --- 클라에게 받는 패킷 ---
            stub.request_LobbyKey = RequestLobbyKey;
            stub.Chat = Chat;
            stub.request_join_info = RequestJoinInfo;
            stub.request_room_list = RequestChannelMove;
            stub.request_make_room = RequestMakeRoom;
            stub.request_join_room = RequestJoinRoom;
            stub.request_bank = RequestBank;
            stub.request_join_room_specific = RequestJoinRoomSpecific;

            // --- 서버간 통신 패킷 ---
            stub.room_lobby_makeroom = RoomLobbyMakeRoom;
            stub.room_lobby_joinroom = RoomLobbyJoinRoom;
            stub.room_lobby_outroom = RoomLobbyOutRoom;
            stub.room_lobby_moveroom_request = RoomLobbyMoveRoom;
            stub.room_lobby_current_response = RoomLobbyCurrent;
            stub.relay_lobby_current_response = RelayLobbyCurrent;

            stub.room_lobby_error = RoomLobbyError;
            stub.lobby_room_moveroom_response = LobbyRoomMoveRoomRedirect;

            stub.lobby_room_kick_player = KickPlayer;
            stub.KickSession = KickSession;

            // 잭팟 알림
            stub.room_lobby_jackpot_event_start = RoomLobbyEventStart;
            stub.room_lobby_jackpot_event_end = RoomLobbyEventEnd;

            //---Core Event -----
            m_Core.client_join_handler = ClientJoin;
            m_Core.client_leave_handler = ClientLeave;
            m_Core.move_server_start_handler = MoveServerStart;
            m_Core.move_server_param_handler = MoveServerParam;
            m_Core.move_server_failed_handler = MoveServerFailed;
            m_Core.message_handler = CoreMessage;
            m_Core.exception_handler = CoreException;
            m_Core.server_join_handler = ServerJoin;
            m_Core.server_leave_handler = ServerLeave;
            m_Core.server_master_join_handler = ServerMasterJoin;
            m_Core.server_master_leave_handler = ServerMasterLeave;
            m_Core.server_refresh_handler = ServerRefresh;

            //// 연결복구 관련 이벤트
            //m_Core.recovery_info_handler = (ZNet.RemoteID remoteNew, ZNet.RemoteID remoteTo) =>
            //{
            //    form.printf("Recovery try... new connection Client[{0}] to Client[{1}].\n", remoteNew, remoteTo);
            //};
            //m_Core.recovery_start_handler = (ZNet.RemoteID remote) =>
            //{
            //    form.printf("Recovery Start Client {0}.\n", remote);
            //    CPlayer player;
            //    if(RemoteClients.TryRemove(remote, out player))
            //    {
            //        RecoveryClients.TryAdd(remote, player);
            //        form.printf("Stay Client {0}.\n", player.data.userID);
            //    }

            //};
            //m_Core.recovery_end_handler = (ZNet.RemoteID remote, ZNet.NetAddress addrNew, bool bTimeOut) =>
            //{
            //    if (bTimeOut)
            //        form.printf("Recovery TimeOUT Client {0}.\n", remote);
            //    else
            //        form.printf("Recovery Complete Client {0}.  NewAddr[{1}:{2}]\n", remote, addrNew.m_ip, addrNew.m_port);
            //};

            //// 서버 접속제한시점의 이벤트
            //m_Core.limit_connection_handler = (ZNet.RemoteID remote, ZNet.NetAddress addr) =>
            //{
            //    form.printf("limit_connection {0}, {1} is Leave.\n", remote, addr.m_ip, addr.m_port);
            //};

        }
        #region CoreEvent
        private void ClientJoin(RemoteID remote, NetAddress addr, ArrByte move_server, ArrByte move_param)
        {
            if (move_server.Count > 0)
            {
                CPlayer rc;
                Common.Common.ServerMoveComplete(move_server, out rc);
                rc.m_ip = addr.m_ip;

                Common.MoveParam param;
                Common.Common.ServerMoveParamRead(move_param, out param);
                rc.channelNumber = param.ChannelNumber;

                // DB 플레이어 정보 불러오기
                Task.Run(async () =>
                {
                    var result = await Task.Run(() =>
                    {
                        try
                        {
                            dynamic Data_Player = Simple.Data.Database.Open().Player.FindAllById(rc.data.ID).FirstOrDefault();
                            rc.data.nickName = Data_Player.NickName;

                            bool AvatarUsing = false;
                            int DefaultAvatarId = 0;
                            string DefaultAvatar = "";
                            int DefaultAvatarVoice = 0;

                            bool CardUsing = false;
                            int DefaultCardId = 0;
                            string DefaultCard = "";

                            rc.DayChangeMoney = Data_Player.DayChangeMoney;
                            rc.IPFree = Data_Player.IPFree;

                            dynamic Data_Item = Simple.Data.Database.Open().V_PlayerItemList.FindAllByUserId(rc.data.ID);
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
                                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: row.Id, Using: false);
                                            }
                                            else
                                            {
                                                rc.data.avatar = row.string1;
                                                rc.data.voice = row.value1;
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
                                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: row.Id, Using: false);
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
                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: DefaultAvatarId, Using: true);
                                rc.data.avatar = DefaultAvatar;
                                rc.data.voice = DefaultAvatarVoice;
                            }
                            if (DefaultCardId != 0 && CardUsing == false)
                            {
                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: DefaultCardId, Using: true);
                                rc.data.avatar_card = DefaultCard;
                            }

                            dynamic Data_SafeBox = Simple.Data.Database.Open().PlayerSafeBox.FindAllByUserID(rc.data.ID).FirstOrDefault();
                            rc.data.bank_money_pay = (long)Data_SafeBox.SafeMoney2;
                            rc.data.bank_money_free = (long)Data_SafeBox.SafeMoney;

                            // 실버 자동 충전
                            if (rc.data.money_free + rc.data.bank_money_free < RechargeFreeMoney)
                            {
                                if (RechargeFreeCount > 0) // 충전 횟수 확인
                                {
                                    dynamic Data_RechargeMoney = Simple.Data.Database.Open().V_LogRechargeMoney.FindAllBy(PlayerId: rc.data.ID, MoneyType: 1);
                                    if (Data_RechargeMoney.ToList().Count < RechargeFreeCount || RechargeFreeCount >= 100)
                                    {
                                        rc.data.money_free += RechargeFreeMoney;
                                        Simple.Data.Database.Open().PlayerGameMoney.UpdateByUserId(UserId: rc.data.ID, GameMoney: rc.data.money_free);
                                        Simple.Data.Database.Open().LogRechargeMoney.Insert(PlayerId: rc.data.ID, MoneyType: 1, RechargeMoney: RechargeFreeMoney);

                                        if (RechargeFreeCount >= 100)
                                        {
                                            RoomLobbyError(rc.remote, ZNet.CPackOption.Basic, RechargeFreeMoney + "실버 충전됐습니다.");
                                        }
                                        else
                                        {
                                            RoomLobbyError(rc.remote, ZNet.CPackOption.Basic, RechargeFreeMoney + "실버 충전됐습니다.\n남은 충전 횟수 →" + (RechargeFreeCount - Data_RechargeMoney.ToList().Count) + " 회");
                                        }
                                    }
                                }
                            }
                            // 골드 자동 충전
                            if (rc.data.money_pay + rc.data.bank_money_pay < RechargePayMoney)
                            {
                                if (RechargePayCount > 0) // 충전 횟수 확인
                                {
                                    dynamic Data_RechargeMoney = Simple.Data.Database.Open().V_LogRechargeMoney.FindAllBy(PlayerId: rc.data.ID, MoneyType: 1);
                                    if (Data_RechargeMoney.ToList().Count < RechargePayCount || RechargePayCount >= 100)
                                    {
                                        rc.data.money_pay += RechargePayMoney;
                                        Simple.Data.Database.Open().PlayerGameMoney.UpdateByUserId(UserId: rc.data.ID, GameMoney: rc.data.money_pay);
                                        Simple.Data.Database.Open().LogRechargeMoney.Insert(PlayerId: rc.data.ID, MoneyType: 2, RechargeMoney: RechargePayMoney);

                                        if (RechargePayCount >= 100)
                                        {
                                            RoomLobbyError(rc.remote, ZNet.CPackOption.Basic, RechargePayMoney + "골드 충전됐습니다.");
                                        }
                                        else
                                        {
                                            RoomLobbyError(rc.remote, ZNet.CPackOption.Basic, RechargePayMoney + "골드 충전됐습니다.\n남은 충전 횟수 →" + (RechargePayCount - Data_RechargeMoney.ToList().Count) + " 회");
                                        }
                                    }
                                }
                            }

                            DB_User_CurrentUpdate(rc.data.ID);

                            RemoteClass.Marshaler.LobbyUserList UserInfo = new RemoteClass.Marshaler.LobbyUserList();
                            UserInfo.ID = rc.data.ID;
                            UserInfo.RemoteID = rc.remote;
                            UserInfo.nickName = rc.data.nickName;
                            UserInfo.FreeMoney = rc.data.money_free;
                            UserInfo.PayMoney = rc.data.money_pay;
                            UserInfo.chanID = rc.channelNumber;
                            UserInfo.roomNumber = 0;
                            LobbyUserInfoUpdate(rc.data.ID, UserInfo);
                            RemoteClients.TryAdd(remote, rc);

                            proxy.server_lobby_user_info(remote, ZNet.CPackOption.Basic, makeLobbyUserInfo(rc.data));
                            proxy.server_lobby_user_list(remote, ZNet.CPackOption.Basic, LobbyUserList.Values.ToList(), rc.friendList);
                            proxy.notify_room_list(remote, ZNet.CPackOption.Basic, rc.channelNumber, RemoteRoomInfos);
                            proxy.server_lobby_jackpot_info(remote, ZNet.CPackOption.Basic, this.JackPotMoney);
                        }
                        catch (Exception e)
                        {
                            form.printf("로비 유저정보 불러오기 : 예외발생 {0}\n", e.ToString());
                            Log._log.ErrorFormat("Client Join LoadData Failed. Player:{0}", rc.data.userID);
                            return 0;
                        }

                        return 1;
                    });


                    if (result == 0)
                    {
                        Log._log.WarnFormat("Client Join Failed. Player:{0}", rc.data.userID);
                        ClientDisconect(remote);
                        return;
                    }

                    //Log._log.InfoFormat("Client Join. Player:{0}", rc.data.userID);
                    //form.printf("Client {0} is Join {1}:{2}. Current={3}\n", remote, addr.m_ip, addr.m_port, RemoteClients.Count);
                });
            }
            else
            {
                //일반입장은 허용하지 않음.
                Log._log.WarnFormat("Client Join Request Failed. remote:{0}", remote);
                ClientDisconect(remote);
            }
        }
        private void ClientLeave(RemoteID remote, bool bMoveServer = false)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                RemoteClass.Marshaler.LobbyUserList temp;
                if (LobbyUserList.TryRemove(rc.data.ID, out temp))
                {
                    if (bMoveServer == false)
                        DB_User_Logout(rc.data.ID);
                }
                else
                {
                    Log._log.WarnFormat("ClientLeave Can't Leave. player:{0}", rc.data.userID);
                }

                CPlayer temp_;
                RemoteClients.TryRemove(remote, out temp_);
            }

            //form.printf("Client {0} Leave. Current={1}\n", remote, RemoteClients.Count);
        }

        ConcurrentDictionary<Guid, RemoteClass.Marshaler.RoomInfo> makeRoomList(int chanID)
        {
            ConcurrentDictionary<Guid, RemoteClass.Marshaler.RoomInfo> roomList = new ConcurrentDictionary<Guid, RemoteClass.Marshaler.RoomInfo>();
            foreach (KeyValuePair<Guid, RemoteClass.Marshaler.RoomInfo> obj in RemoteRoomInfos)
            {
                //if (obj.Value.chanID == chanID)
                {
                    roomList.TryAdd(obj.Key, obj.Value);
                }
            }

            return roomList;
        }
        RemoteClass.Marshaler.LobbyUserInfo makeLobbyUserInfo(UserData data)
        {
            RemoteClass.Marshaler.LobbyUserInfo lobbyuserinfo = new RemoteClass.Marshaler.LobbyUserInfo();

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

        // 사용자 접속해제
        private void ClientDisconect(RemoteID remote)
        {
            //string playerId;
            //CPlayer rc;
            //if (RemoteClients.TryGetValue(remote, out rc))
            //    playerId = rc.data.userID;
            //else
            //    playerId = remote.ToString();

            ClientLeave(remote);
            m_Core.CloseRemoteClient(remote);
            Log._log.WarnFormat("Player Disconect. remote:{0}", remote);
        }
        //서버이동시작
        private void MoveServerStart(RemoteID remote, out ArrByte userdata)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false)
            {
                Log._log.WarnFormat("MoveServerStart UnkownRemote. remote:{0}", remote);
                userdata = null;
                CPlayer temp;
                RemoteClients.TryRemove(remote, out temp);
                ClientDisconect(remote);
                return;
            }
            // 여기서는 이동할 서버로 동기화 시킬 유저 데이터를 구성하여 buffer에 넣어둔다->완료서버에서 해당 데이터를 그대로 받게된다
            Common.Common.ServerMoveStart(rc, out userdata);
            //form.printf("move server start  {0}", rc.data.userID);
        }
        // 파라미터 검사후 서버이동 승인 여부 결정하기
        private bool MoveServerParam(ArrByte move_param, int count_idx)
        {
            Common.MoveParam param;
            Common.Common.ServerMoveParamRead(move_param, out param);
            //form.printf("MoveParam_2 {0} {1} {2}", param.moveTo, param.roomJoin, param.room_id);
            //이 서버가 로비서버이므로 파라이터가 로비서버 일때만 승인해 준다.
            if (param.moveTo == Common.MoveParam.ParamMove.MoveToLobby)
                return true;
            return false;
        }
        private void MoveServerFailed(ZNet.ArrByte move_param)
        {
            CPlayer rc;
            Common.Common.ServerMoveComplete(move_param, out rc);

            if (rc != null)
            {
                form.printf("MoveServerFailed. {0}", rc.data.userID);
                DB_User_Logout(rc.data.ID);
            }
        }
        private void CoreMessage(ResultInfo resultInfo)
        {
            switch (resultInfo.m_Level)
            {
                case IResultLevel.IMsg:
                    Log._log.Info("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.IWrn:
                    Log._log.Warn("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.IErr:
                    Log._log.Error("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.ICri:
                    Log._log.Fatal("[CoreMsg]" + resultInfo.msg);
                    break;
                default:
                    Log._log.Fatal("[CoreMsg]" + resultInfo.msg);
                    break;
            }
        }
        private void CoreException(Exception e)
        {
            Log._log.Fatal("[Exception]" + e.ToString());
        }
        private void ServerJoin(RemoteID remote, NetAddress addr)
        {
            form.printf(string.Format("서버P2P맴버 입장 remoteID {0}", remote));

            // 연결된 룸, 릴레이서버로부터 정보 받기
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                ZNet.MasterInfo[] svr_array;
                m_Core.GetServerList((int)ServerType.Room, out svr_array);

                if (svr_array == null) return;

                foreach (var svr in svr_array)
                {
                    if (svr.m_remote == remote)
                    {
                        proxy.lobby_room_current_request(remote, ZNet.CPackOption.Basic, true);
                    }
                }
            });
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                ZNet.MasterInfo[] svr_array;
                m_Core.GetServerList((int)ServerType.Relay, out svr_array);

                if (svr_array == null) return;

                foreach (var svr in svr_array)
                {
                    if (svr.m_remote == remote)
                    {
                        proxy.lobby_room_current_request(remote, ZNet.CPackOption.Basic, true);
                    }
                }
            });
        }
        private void ServerLeave(RemoteID remote, NetAddress addr)
        {
            form.printf(string.Format("서버P2P맴버 퇴장 remoteID {0}", remote));

            // 나간 룸서버의 룸 목록 삭제 후 목록 일괄전송
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList((int)ServerType.Room, out svr_array);
            if ((svr_array == null) == false)
            {
                foreach (var svr in svr_array)
                {
                    if (svr.m_remote == remote)
                    {
                        Task.Run(() =>
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
                                            Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: Users[j]);
                                            RemoteClass.Marshaler.LobbyUserList temp;
                                            LobbyUserList.TryRemove(Users[j], out temp);
                                        }
                                        Simple.Data.Database.Open().GameRoomList.DeleteById(Id: room.Value.roomID);
                                        RemoteClass.Marshaler.RoomInfo temp_;
                                        RemoteRoomInfos.TryRemove(room.Value.roomID, out temp_);
                                    }
                                }
                                Brodcast_Room_List();
                            }
                            catch (Exception e)
                            {
                                form.printf("ServerLeave {0}\n", e.ToString());
                            }
                        });
                    }
                }
            }
            else
            {
                m_Core.GetServerList((int)ServerType.Relay, out svr_array);
                if ((svr_array == null) == false)
                {
                    foreach (var svr in svr_array)
                    {
                        if (svr.m_remote == remote)
                        {
                            int temp;
                            RemoteRelays.TryRemove(remote, out temp);
                        }
                    }
                }
            }
        }
        private void ServerMasterJoin(RemoteID remote, RemoteID myRemoteID)
        {
            this.svrRemoteID = (int)myRemoteID;
            form.printf(string.Format("마스터서버에 입장성공 remoteID {0}", myRemoteID));
        }
        private void ServerMasterLeave()
        {
            form.printf(string.Format("마스터서버와 연결종료!!!"));
            //run_program = false;    // 자동 종료처리를 위해
        }
        private void ServerRefresh(MasterInfo master_info)
        {
            //Log._log.InfoFormat("서버P2P remote:{0} type:{1}[{2}] current:{3} addr:{4}:{5}",
            //master_info.m_remote,
            //(UnityCommon.Server)master_info.m_ServerType,
            //master_info.m_Description,
            //master_info.m_Clients,
            //master_info.m_Addr.m_ip,
            //master_info.m_Addr.m_port
            //);
        }

        public override void NetLoop(object sender, ElapsedEventArgs e)
        {
            m_Core.NetLoop();
        }
        int tick = 0;
        private void ScheduleTask()
        {
        }
        public override void ServerTask(object sender, ElapsedEventArgs e_)
        {
            ++this.tick;

            if (this.tick % 5 == 0)
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

                // 서버 정보 출력
                //{
                //    form.printf("로비 플레이어 : " + RemoteClients.Count + " 방 : " + RemoteRoomInfos.Count);
                //}
            }

            if (this.tick % 17 == 0)
            {
                //DisplayStatus(m_Core);

                if (this.ShutDown)
                {
                    // 세션 없으면 프로그램 종료
                    if (RemoteClients.Count == 0)
                    {
                        ServerClose();
                    }
                    else
                    // 모든 세션 종료
                    if (this.CountDown < DateTime.Now)
                    {
                        if (Froce)
                            m_Core.CloseAllClient();
                        else
                            m_Core.CloseAllClientForce();
                        Froce = true;
                    }
                    form.printf("세션 종료중. 남은 세션 수:" + RemoteClients.Count);
                    Log._log.Info("세션 종료중. 남은 세션 수:" + RemoteClients.Count);
                }
            }

            if (this.tick % 61 == 0)
            {
                DB_Server_SendJackPotMoney();
            }
        }

        public override void PrintLog(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.F1:
                    {
                        form.printf(string.Format("접속 세션 수: {0}, 방 수:{1}", RemoteClients.Count, RemoteRoomInfos.Count));
                    }
                    break;
                case System.Windows.Forms.Keys.F2:
                    {
                        form.printf(string.Format("접속 세션 디테일: {0}", RemoteClients.Count));
                        string sLog = "";
                        foreach (var Value in RemoteClients.Values)
                        {
                            sLog += string.Format("접속 플레이어: {0} \r\n", Value.data.userID);
                        }
                        form.printf(sLog);
                    }
                    break;
                case System.Windows.Forms.Keys.F3:
                    {
                        form.printf(string.Format("방 디테일: {0}", RemoteRoomInfos.Count));
                        string sLog = "";
                        foreach (var Value in RemoteRoomInfos.Values)
                        {
                            sLog += string.Format("방 번호: {0} \r\n", Value.number);
                        }
                        form.printf(sLog);
                    }
                    break;
            }
        }
        void ServerClose()
        {
            Log._log.Info("서버 종료. ShutDown");
            DB_Server_CurrentPlayerClear();

            System.Windows.Forms.Application.Exit();
        }
        #endregion CoreEvent

        #region Mobile
        bool RequestPurchaseList(RemoteID remote, CPackOption pkOption)
        {
            if (ServerMaintenance) return false;

            // 웹에서 처리

            return true;
        }
        bool RequestPurchaseAvailability(RemoteID remote, CPackOption pkOption, string pid)
        {
            if (ServerMaintenance)
            {
                proxy.response_purchase_availability(remote, pkOption, false, ServerMsg);
                return false;
            }

            CPlayer rc;
            if (!RemoteClients.TryGetValue(remote, out rc))
            {
                proxy.response_purchase_availability(remote, pkOption, false, "오류! 고객지원에 문의해주세요.");
                return false;
            }

            Task.Run(() =>
            {
                try
                {
                    bool available = true;
                    string reason = "";

                    dynamic Data_Product = Simple.Data.Database.Open().V_MobileShop.FindAllBypid(pid: pid).FirstOrDefault();

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
                    dynamic Data_Payment = Simple.Data.Database.Open().V_PurchaseAndroidMonth.FindAllByPlayerId(rc.data.ID).FirstOrDefault();
                    //if (true)
                    if (Data_Payment != null && Data_Payment.PurchaseMoney + Data_Product.price >= 550000)
                    {
                        available = false;
                        reason = "월 결제한도를 초과합니다.\n(현재 결제금액 : " + Data_Payment.PurchaseMoney.ToString() + ")";
                    }

                    /*
                    if (available == false)
                    {
                        Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : inapp구매가능확인 실패 : " + reason);
                    }
                    */

                    proxy.response_purchase_availability(remote, pkOption, available, reason);
                }
                catch (Exception e)
                {
                    Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : inapp구매가능확인 오류 : " + e.ToString());
                    form.printf("RequestPurchaseAvailability {0}\n", e.ToString());
                }
            });

            return true;
        }
        bool RequestPurchaseReceiptCheck(RemoteID remote, CPackOption pkOption, string result)
        {
            CPlayer rc;
            if (!RemoteClients.TryGetValue(remote, out rc))
            {
                proxy.response_purchase_receipt_check(remote, pkOption, false, Guid.Empty);
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

                    dynamic Data_Check = Simple.Data.Database.Open().PurchaseAndroid.FindAllByorderId(orderId: orderId).FirstOrDefault();
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

                            Simple.Data.Database.Open().PurchaseAndroid.Insert(guid: token, PlayerId: rc.data.ID, orderId: orderId, packageName: packageName, productId: productId, purchaseTime: purchaseTime, purchaseState: purchaseState, purchaseToken: purchaseToken, signature: signature, Success: false);

                            // 캐쉬 지급
                            dynamic Data_Product = Simple.Data.Database.Open().V_MobileShop.FindAllBypid(pid: productId).FirstOrDefault();
                            dynamic Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(rc.data.ID).FirstOrDefault();
                            int cash = rc.data.cash = Data_Money.cash;
                            cash += Data_Product.paidvalue1;
                            rc.data.cash = cash;
                            Simple.Data.Database.Open().PlayerGameMoney.UpdateByUserID(UserID: rc.data.ID, Cash: cash);

                            // 확인
                            Simple.Data.Database.Open().PurchaseAndroid.UpdateBytoken(guid: token, Success: true);
                            Simple.Data.Database.Open().LogPurchase.Insert(UserId: rc.data.ID, pid: Data_Product.pid, pname: Data_Product.pname, purchase_kind: Data_Product.purchase_kind, price: Data_Product.price, Location: "mobile");
                        }
                    }

                    if (check == false)
                    {
                        Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 영수증 검증 실패 : ");
                    }

                    proxy.response_purchase_receipt_check(remote, pkOption, check, token);
                }
                catch (Exception e)
                {
                    Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 영수증 검증 오류 : " + e.ToString());
                    form.printf("RequestPurchaseReceiptCheck remote:{0}, error:{1}\n", remote, e.ToString());
                }
            });

            return true;
        }
        bool RequestPurchaseResult(RemoteID remote, CPackOption pkOption, Guid token)
        {
            CPlayer rc;
            if (!RemoteClients.TryGetValue(remote, out rc))
            {
                proxy.response_purchase_result(remote, pkOption, false, "오류! 고객지원에 문의해주세요.");
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
                        dynamic Data_Purchase = Simple.Data.Database.Open().PurchaseAndroid.FindAllByguid(guid: token).FirstOrDefault();

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
                        Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 최종 처리 실패 : " + reason);
                    }

                    proxy.response_purchase_result(remote, pkOption, check, reason);
                }
                catch (Exception e)
                {
                    Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 최종 처리 오류 : " + e.ToString());
                    form.printf("RequestPurchaseReceiptCheck remote:{0}, error:{1}\n", remote, e.ToString());
                }
            });

            return true;
        }
        bool RequestPurchaseCash(RemoteID remote, CPackOption pkOption, string pid)
        {
            if (ServerMaintenance)
            {
                proxy.response_purchase_cash(remote, pkOption, false, ServerMsg);
                return false;
            }

            CPlayer rc;
            if (!RemoteClients.TryGetValue(remote, out rc))
            {
                proxy.response_purchase_cash(remote, pkOption, false, "잠시후 다시 시도하세요.");
                return false;
            }

            Task.Run(() =>
            {
                try
                {
                    bool result = false;
                    string reason = "";

                    dynamic Data_Product = Simple.Data.Database.Open().V_MobileShop.FindAllBypid(pid: pid).FirstOrDefault();

                    if (Data_Product == null || Data_Product.sale == false)
                        reason = "판매중인 상품이 아닙니다.";
                    else
                    {
                        // 게임머니 상품인지 확인
                        if (Data_Product.purchase_kind == "gamemoney")
                        {
                            if (Data_Product.ptype == "avatar" ||
                            Data_Product.ptype == "card" ||
                            Data_Product.ptype == "evt")
                            {
                                // 상품 구매
                                dynamic Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(rc.data.ID).FirstOrDefault();
                                long GameMoney = (long)Data_Money.GameMoney;

                                if (GameMoney >= Data_Product.price)
                                {
                                    dynamic Data_Item = Simple.Data.Database.Open().PlayerItemList.FindAllBy(UserID: rc.data.ID, ItemId: Data_Product.productid).FirstOrDefault();
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
                                        Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: Data_Item.Id, ItemId: Data_Product.productid, Count: 1, ExpireDate: updatetime);
                                    }
                                    else
                                    {
                                        updatetime = DateTime.Now.AddDays(Data_Product.paidvalue3);
                                        Simple.Data.Database.Open().PlayerItemList.Insert(UserId: rc.data.ID, ItemId: Data_Product.productid, Count: 1, ExpireDate: updatetime);
                                    }
                                    GameMoney -= Data_Product.price;

                                    rc.data.money_free = GameMoney;

                                    Simple.Data.Database.Open().PlayerGameMoney.UpdateByUserId(UserId: rc.data.ID, GameMoney: GameMoney);
                                    Simple.Data.Database.Open().LogPurchase.Insert(UserId: rc.data.ID, pid: Data_Product.pid, pname: Data_Product.pname, purchase_kind: Data_Product.purchase_kind, price: Data_Product.price, Location: "mobile");

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
                        Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 아이템 구매 실패 : " + reason);
                    }

                    proxy.response_purchase_cash(remote, pkOption, result, reason);
                }
                catch (Exception e)
                {
                    Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 아이템 구매 오류 : " + e.ToString());
                    form.printf("RequestPurchaseCash {0}\n", e.ToString());
                }
            });

            return true;
        }

        bool RequestPurchaseCash_gold(RemoteID remote, CPackOption pkOption, string pid)
        {
            if (ServerMaintenance)
            {
                proxy.response_purchase_cash(remote, pkOption, false, ServerMsg);
                return false;
            }

            CPlayer rc;
            if (!RemoteClients.TryGetValue(remote, out rc))
            {
                proxy.response_purchase_cash(remote, pkOption, false, "잠시후 다시 시도하세요.");
                return false;
            }

            Task.Run(() =>
            {
                try
                {
                    bool result = false;
                    string reason = "";

                    dynamic Data_Product = Simple.Data.Database.Open().V_MobileShop.FindAllBypid(pid: pid).FirstOrDefault();

                    if (Data_Product == null || Data_Product.sale == false)
                        reason = "판매중인 상품이 아닙니다.";
                    else
                    {
                        // 금괴 상품인지 확인
                        if (Data_Product.purchase_kind == "cash")
                        {
                            if (Data_Product.ptype == "avatar" ||
                            Data_Product.ptype == "card" ||
                            Data_Product.ptype == "evt")
                            {
                                // 상품 구매
                                dynamic Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(rc.data.ID).FirstOrDefault();
                                int Cash = rc.data.cash = Data_Money.cash;
                                long GameMoney = (long)Data_Money.GameMoney;
                                long PayMoney = (long)Data_Money.PayMoney;

                                if (Cash >= Data_Product.price)
                                {
                                    dynamic Data_Item = Simple.Data.Database.Open().PlayerItemList.FindAllBy(UserID: rc.data.ID, ItemId: Data_Product.productid).FirstOrDefault();
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
                                        Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: Data_Item.Id, ItemId: Data_Product.productid, Count: 1, ExpireDate: updatetime);
                                    }
                                    else
                                    {
                                        updatetime = DateTime.Now.AddDays(Data_Product.paidvalue3);
                                        Simple.Data.Database.Open().PlayerItemList.Insert(UserId: rc.data.ID, ItemId: Data_Product.productid, Count: 1, ExpireDate: updatetime);
                                    }
                                    Cash -= Data_Product.price;
                                    GameMoney += Data_Product.paidvalue1;
                                    PayMoney += Data_Product.paidvalue2;

                                    rc.data.cash = Cash;
                                    rc.data.money_free = GameMoney;
                                    rc.data.money_pay = PayMoney;

                                    Simple.Data.Database.Open().PlayerGameMoney.UpdateByUserId(UserId: rc.data.ID, GameMoney: GameMoney, PayMoney: PayMoney, Cash: Cash);
                                    Simple.Data.Database.Open().LogPurchase.Insert(UserId: rc.data.ID, pid: Data_Product.pid, pname: Data_Product.pname, purchase_kind: Data_Product.purchase_kind, price: Data_Product.price, Location: "mobile");

                                    result = true;
                                }
                                else
                                    reason = "금괴가 부족합니다.";
                            }
                            else
                                reason = "판매중인 상품이 아닙니다.";
                        }
                        else
                            reason = "캐쉬결제 상품이 아닙니다.";
                    }

                    if (result == false)
                    {
                        Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 아이템 구매 실패 : " + reason);
                    }

                    proxy.response_purchase_cash(remote, pkOption, result, reason);
                }
                catch (Exception e)
                {
                    Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 아이템 구매 오류 : " + e.ToString());
                    form.printf("RequestPurchaseCash {0}\n", e.ToString());
                }
            });

            return true;
        }
        bool RequestMyroomList(RemoteID remote, CPackOption pkOption)
        {
            if (ServerMaintenance) return false;

            // 웹에서 처리

            return true;
        }
        bool RequestMyroomAction(RemoteID remote, CPackOption pkOption, string pid)
        {
            if (ServerMaintenance)
            {
                proxy.response_myroom_action(remote, pkOption, pid, false, ServerMsg);
                return false;
            }

            CPlayer rc;
            if (!RemoteClients.TryGetValue(remote, out rc))
            {
                proxy.response_myroom_action(remote, pkOption, pid, false, "잠시후 다시 시도하세요.");
                return false;
            }

            Task.Run(() =>
            {
                try
                {
                    bool result = false;
                    string reason = "잠시후 다시 시도하세요.";

                    dynamic Data_Item = Simple.Data.Database.Open().V_PlayerItemList.FindAllBy(Id: pid, UserId: rc.data.ID).FirstOrDefault();

                    if (Data_Item == null)
                    {
                        reason = "보유중인 아이템이 아닙니다.";
                    }
                    else
                    {
                        if (Data_Item.ptype == "avatar" ||
                        Data_Item.ptype == "card" ||
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
                                        dynamic Data_ItemUsing = Simple.Data.Database.Open().V_PlayerItemList.FindAllBy(UserId: rc.data.ID, ptype: Data_Item.ptype, Using: true).FirstOrDefault();
                                        if (Data_ItemUsing != null)
                                        {
                                            if (Data_Item.Id != Data_ItemUsing.Id)
                                            {
                                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: Data_ItemUsing.Id, Using: false);
                                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: Data_Item.Id, Using: true);
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
                                    else if (Data_Item.ptype == "card")
                                    {
                                        dynamic Data_ItemUsing = Simple.Data.Database.Open().V_PlayerItemList.FindAllBy(UserId: rc.data.ID, ptype: Data_Item.ptype, Using: true).FirstOrDefault();
                                        if (Data_ItemUsing != null)
                                        {
                                            if (Data_Item.Id != Data_ItemUsing.Id)
                                            {
                                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: Data_ItemUsing.Id, Using: false);
                                                Simple.Data.Database.Open().PlayerItemList.UpdateById(Id: Data_Item.Id, Using: true);
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

                    proxy.response_myroom_action(remote, pkOption, pid, result, reason);
                }
                catch (Exception e)
                {
                    Simple.Data.Database.Open()._error.Insert(Type: GameId, Message: rc.data.ID.ToString() + ") 모바일 : 마이룸 액션 오류 : " + e.ToString());
                    form.printf("RequestMyroomAction {0}\n", e.ToString());
                }
            });

            return true;
        }
        #endregion Mobile

        #region Client Handler
        // --- 클라에게 받는 패킷 ---
        // 로그인키 재발급
        bool RequestLobbyKey(RemoteID remote, CPackOption pkOption, string id, string key)
        {
            if (ServerMaintenance) return false;

            Task.Run(() =>
            {
                CPlayer rc;
                if (RemoteClients.TryGetValue(remote, out rc) == false) return;

                try
                {
                    string newkey = "";
                    //dynamic Data_Password = Simple.Data.Database.Open().PlayerPassword.FindAllByUserID(rc.data.ID).FirstOrDefault();
                    //if (Data_Password.Password == key)
                    {
                        System.Security.Cryptography.SHA1 sha = System.Security.Cryptography.SHA1.Create();
                        newkey = HexStringFromBytes(sha.ComputeHash(Encoding.UTF8.GetBytes(id + DateTime.Now.ToString() + "vong")));

                        Simple.Data.Database.Open().PlayerPassword.UpdateByUserId(UserId: rc.data.ID, Password: newkey, CreatedOnUtc: DateTime.Now);
                    }
                    proxy.response_LobbyKey(remote, pkOption, newkey);
                }
                catch (Exception e)
                {
                    form.printf("RequestLobbyKey {0}\n", e.ToString());
                }
            });

            return true;
        }

        // 채팅 메세지 처리
        bool Chat(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            form.printf("Remote[{0}] msg : {1}", remote, msg);
            proxy.Chat(remote, ZNet.CPackOption.Basic, msg);
            return true;
        }

        // 방 목록 요청 처리
        private void RequestJoinInfo(RemoteID remote, CPackOption pkOption)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                proxy.notify_room_list(remote, ZNet.CPackOption.Basic, rc.channelNumber, RemoteRoomInfos);
                proxy.server_lobby_user_info(remote, ZNet.CPackOption.Basic, makeLobbyUserInfo(rc.data));
                proxy.server_lobby_user_list(remote, ZNet.CPackOption.Basic, LobbyUserList.Values.ToList(), rc.friendList);
                proxy.server_lobby_jackpot_info(remote, ZNet.CPackOption.Basic, this.JackPotMoney);
            }
        }
        // 채널 이동
        private bool RequestChannelMove(RemoteID remote, CPackOption pkOption, int chanID)
        {
            if (GetChnnelType(chanID) == ChannelType.None) return false;

            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                rc.channelNumber = chanID;
                RemoteClass.Marshaler.LobbyUserList temp;
                if (LobbyUserList.TryGetValue(rc.data.ID, out temp))
                {
                    temp.chanID = chanID;
                    Brodcast_User_List();
                }
            }

            return true;
        }
        // 방 만들기 : 룸 서버로 이동하기
        bool RequestMakeRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int stake, string roomPassword, ref string errorID)
        {
            if (ServerMaintenance)
            {
                errorID = ServerMsg;
                return false;
            }

            if (roomPassword.Length > 20) // 비밀번호가 너무 김
            {
                return false;
            }

            // 방만들기 옵션값 검사
            if (IsExistChnnel(chanID) == false || IsExistStakeType(UserServer.GetChnnelType(chanID), stake) == false) return false;

            int limit;
            if (ChannelsRoomsCount.TryGetValue(chanID, out limit) && limit >= RoomLimit)
            {
                errorID = "방 만들기 실패\n 현재 채널은 더 이상 방을 만들 수 없습니다.";
                return false;
            }

            CPlayer rc;
            long playerMoney;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                // 손실한도 제한
                /*
                if (rc.DayChangeMoney < -100000000000) // 일일 손실한도 1000억
                {
                    int timeHour = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours;
                    string limitTime = "";
                    if (timeHour > 0)
                        limitTime = timeHour.ToString() + " 시간";
                    else
                        limitTime = (DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes.ToString() + " 분";
                    errorID = "일일 손실한도를 초과했습니다. " + limitTime + " 후 다시 이용할 수 있습니다.";
                    return false;
                }
                */

                if (GetChnnelType(chanID) == ChannelType.Charge)
                    playerMoney = rc.data.money_pay;
                else
                    playerMoney = rc.data.money_free;

                if (playerMoney < GetMinimumMoney(UserServer.GetChnnelType(chanID), stake))
                {
                    errorID = GetChnnelName(GetChnnelType(chanID)) + "에 입장할 수 있는 최소 금액은 " + GetMinimumMoneyText(UserServer.GetChnnelType(chanID), stake) + " 냥 입니다.";
                    return false;
                }
            }

            int server_type = (int)ServerType.Relay; // 릴레이 서버에 요청
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList(server_type, out svr_array);
            if (svr_array == null) return false;
            foreach (var svr in svr_array)
            {
                int relayID;
                if(RemoteRelays.TryGetValue(svr.m_remote, out relayID) == false)
                {
                    continue;
                }

                if (relayID != 1) continue;

                // 이동 파라미터 구성
                ZNet.ArrByte param_buffer;
                Common.MoveParam param = new Common.MoveParam();
                param.From(Common.MoveParam.ParamMove.MoveToRoom, Common.MoveParam.ParamRoom.RoomMake, Guid.NewGuid(), this.svrRemoteID, chanID, stake, roomPassword, relayID);
                Common.Common.ServerMoveParamWrite(param, out param_buffer);

                m_Core.ServerMoveStart(remote, svr.m_Addr, param_buffer, param.room_id);
                return true;
            }

            return false;
        }

        // 방 입장하기 : 룸서버 이동
        bool RequestJoinRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int stake, ref string errorID)
        {
            if (ServerMaintenance)
            {
                errorID = ServerMsg;
                return false;
            }

            // 바로입장 옵션값 검사
            if (IsExistChnnel(chanID) == false || IsExistStakeType(UserServer.GetChnnelType(chanID), stake) == false) return false;

            int server_type = (int)ServerType.Room; // 룸 서버에 요청
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList(server_type, out svr_array);
            if (svr_array == null) return false;

            ZNet.MasterInfo find_server = null;
            Guid roomID = Guid.Empty;

            CPlayer rc;
            long playerMoney = 0;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                // 손실한도 제한
                /*
                if (rc.DayChangeMoney < -100000000000) // 일일 손실한도 1000억
                {
                    int timeHour = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours;
                    string limitTime = "";
                    if (timeHour > 0)
                        limitTime = timeHour.ToString() + " 시간";
                    else
                        limitTime = (DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes.ToString() + " 분";
                    errorID = "일일 손실한도를 초과했습니다. " + limitTime + " 후 다시 이용할 수 있습니다.";
                    return false;
                }
                */
            }
            else
            {
                errorID = "잠시후 다시 시도하세요.";
                return false;
            }

            // 방 검색

            var rooms = RemoteRoomInfos.Values.ToList().OrderBy(o => rngRoom.Next());
            foreach (var room in rooms)
            //foreach (var room in RemoteRoomInfos)
            {
                if (room.chanType == (int)ChannelType.Charge)
                    playerMoney = rc.data.money_pay;
                else
                    playerMoney = rc.data.money_free;

                if (room.chanID == chanID && room.stakeType == stake && room.restrict == false && room.needPassword == false)
                {
                    if (playerMoney < GetMinimumMoney(UserServer.GetChnnelType(chanID), room.stakeType))
                    {
                        errorID = GetChnnelName(GetChnnelType(chanID)) + "에 입장할 수 있는 최소 금액은 " + GetMinimumMoneyText(UserServer.GetChnnelType(chanID), stake) + " 냥 입니다.";
                        return false;
                    }
                    // 서버 검색
                    foreach (var svr in svr_array)
                    {
                        // 해당 방이 존재하는지 확인
                        if ((ZNet.RemoteID)room.remote_svr == svr.m_remote)
                        {
                            // 입장 가능한 IP인지 확인
                            if (rc.IPFree == false)
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
                return false;
            }

            // 릴레이 서버 확인
            ZNet.MasterInfo[] svr_arrayRelay;
            m_Core.GetServerList((int)ServerType.Relay, out svr_arrayRelay);
            if (svr_arrayRelay == null)
            {
                // 릴레이 서버 없음 : 이동 실패
                return false;
            }
            foreach (var svr in svr_arrayRelay)
            {
                int relayID;
                if (RemoteRelays.TryGetValue(svr.m_remote, out relayID) == false)
                {
                    continue;
                }

                if (relayID != 2) continue;

                // 이동 파라미터 구성
                ZNet.ArrByte param_buffer;
                Common.MoveParam param = new Common.MoveParam();
                param.From(Common.MoveParam.ParamMove.MoveToRoom, Common.MoveParam.ParamRoom.RoomJoin, roomID, this.svrRemoteID, chanID, stake, "", relayID);
                Common.Common.ServerMoveParamWrite(param, out param_buffer);
                m_Core.ServerMoveStart(remote, svr.m_Addr, param_buffer, roomID);

                return true;
            }

            return false;
        }
        bool RequestBank(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int requestOption, long requestMoney, string password)
        {
            if (ServerMaintenance) return false;

            if (requestMoney <= 0)
            {
                proxy.response_bank(remote, ZNet.CPackOption.Basic, false, 2);
            }

            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                DB_User_UpdateBank(remote, rc, requestOption, requestMoney, password);
                return true;
            }

            return false;
        }
        // 특정 방 입장하기 : 룸서버 이동
        bool RequestJoinRoomSpecific(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int roomNumber, string roomPassword, ref string errorID)
        {
            if (ServerMaintenance)
            {
                errorID = ServerMsg;
                return false;
            }

            // 특정 방을 입장할 수 있는 채널인지 검사
            if (chanID != 2 /*무료자유채널*/) return false;

            if (roomPassword.Length > 20) // 비밀번호가 너무 김
            {
                return false;
            }

            int stake = 0;
            int server_type = (int)ServerType.Room; // 룸 서버에 요청
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList(server_type, out svr_array);
            if (svr_array == null) return false;

            ZNet.MasterInfo find_server = null;
            Guid roomID = Guid.Empty;

            CPlayer rc;
            long playerMoney = 0;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                // 손실한도 제한
                /*
                if (rc.DayChangeMoney < -100000000000) // 일일 손실한도 1000억
                {
                    int timeHour = (int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours;
                    string limitTime = "";
                    if (timeHour > 0)
                        limitTime = timeHour.ToString() + " 시간";
                    else
                        limitTime = (DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes.ToString() + " 분";
                    errorID = "일일 손실한도를 초과했습니다. " + limitTime + " 후 다시 이용할 수 있습니다.";
                    return false;
                }
                */
            }
            else
            {
                errorID = "잠시후 다시 시도하세요.";
                return false;
            }

            // 방 검색
            var rooms = RemoteRoomInfos.Values.ToList().OrderBy(o => rngRoom.Next());
            foreach (var room in rooms)
            //foreach (var room in RemoteRoomInfos)
            {
                if (room.chanType == (int)ChannelType.Charge)
                    playerMoney = rc.data.money_pay;
                else
                    playerMoney = rc.data.money_free;

                if (room.chanID == chanID && room.restrict == false && room.number == roomNumber)
                {
                    if (playerMoney < GetMinimumMoney(UserServer.GetChnnelType(chanID), room.stakeType))
                    {
                        errorID = GetChnnelName(GetChnnelType(chanID)) + "에 입장할 수 있는 최소 금액은 " + GetMinimumMoneyText(UserServer.GetChnnelType(chanID), stake) + " 냥 입니다.";
                        return false;
                    }
                    // 룸 서버 검색
                    foreach (var svr in svr_array)
                    {
                        // 해당 방이 존재하는지 확인
                        if ((ZNet.RemoteID)room.remote_svr == svr.m_remote)
                        {
                            if (room.roomPassword != roomPassword)
                            {
                                // 방은 있지만 비밀번호가 다름
                                errorID = "비밀번호가 다릅니다.";
                                return false;
                            }

                            roomID = room.roomID;
                            stake = room.stakeType;
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
                return false;
            }

            // 릴레이 서버 확인
            ZNet.MasterInfo[] svr_arrayRelay;
            m_Core.GetServerList((int)ServerType.Relay, out svr_arrayRelay);
            if (svr_arrayRelay == null)
            {
                // 릴레이 서버 없음 : 이동 실패
                return false;
            }
            foreach (var svr in svr_arrayRelay)
            {
                int relayID;
                if (RemoteRelays.TryGetValue(svr.m_remote, out relayID) == false)
                {
                    continue;
                }

                // 이동 파라미터 구성
                ZNet.ArrByte param_buffer;
                Common.MoveParam param = new Common.MoveParam();
                param.From(Common.MoveParam.ParamMove.MoveToRoom, Common.MoveParam.ParamRoom.RoomJoin, roomID, this.svrRemoteID, chanID, stake, "", relayID);
                Common.Common.ServerMoveParamWrite(param, out param_buffer);
                m_Core.ServerMoveStart(remote, svr.m_Addr, param_buffer, roomID);

                return true;
            }

            return true;
        }

        bool RoomLobbyMoveRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Guid roomIDNow, ZNet.RemoteID userRemote, int userID, long playerMoney, bool IPFree, ref string errorID)
        {
            if (ServerMaintenance)
            {
                errorID = ServerMsg;
                return false;
            }

            RemoteClass.Marshaler.RoomInfo CurrentRoom;

            if (RemoteRoomInfos.TryGetValue(roomIDNow, out CurrentRoom) == false)
            {
                errorID = "잠시후 다시 시도하세요.";
                return false;
            }

            int chanID = CurrentRoom.chanID;
            int stake = CurrentRoom.stakeType;
            string playerIP = "";

            if (CurrentRoom.userList.TryGetValue(userID, out playerIP) == false)
            {
                errorID = "잠시후 다시 시도하세요.";
                return false;
            }

            // 바로입장 옵션값 검사
            if (IsExistChnnel(chanID) == false || IsExistStakeType(UserServer.GetChnnelType(chanID), stake) == false) return false;

            int server_type = (int)ServerType.Room;
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList(server_type, out svr_array);
            if (svr_array == null) return false;

            ZNet.MasterInfo find_server = null;
            Guid roomID = Guid.Empty;

            // 방 검색
            var rooms = RemoteRoomInfos.Values.ToList().OrderBy(o => rngRoom.Next());
            foreach (var room in rooms)
            //foreach (var room in RemoteRoomInfos)
            {
                if (room.chanID == chanID && room.stakeType == stake && room.restrict == false && room.needPassword == false && room.roomID != roomIDNow)
                {
                    if (playerMoney < GetMinimumMoney(UserServer.GetChnnelType(chanID), room.stakeType))
                    {
                        errorID = GetChnnelName(GetChnnelType(chanID)) + "에 입장할 수 있는 최소 금액은 " + GetMinimumMoneyText(UserServer.GetChnnelType(chanID), stake) + " 냥 입니다.";
                        return false;
                    }
                    // 룸 서버 검색
                    foreach (var svr in svr_array)
                    {
                        // 해당 방이 존재하는지 확인
                        if ((ZNet.RemoteID)room.remote_svr == svr.m_remote)
                        {
                            // 입장 가능한 IP인지 확인
                            if (IPFree == false && false)
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
                // 해당 서버 없음 : 방만들기
                foreach (var svr in svr_array)
                {
                    proxy.lobby_room_moveroom_response(remote, ZNet.CPackOption.Basic, true, Guid.NewGuid(), svr.m_Addr, chanID, userRemote, "");
                    return true;
                }
                return false;
            }

            proxy.lobby_room_moveroom_response(remote, ZNet.CPackOption.Basic, false, roomID, find_server.m_Addr, chanID, userRemote, "");

            return true;
        }

        bool RoomLobbyCurrent(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage Msg)
        {
            int chanID; RemoteClass.Marshaler.Read(Msg, out chanID);
            if (ChannelsRoomsCount.TryAdd(chanID, 0) == false)
            {
                ChannelsRoomsCount.TryUpdate(chanID, 0, 0);
            }
            form.printf("룸서버 서버 입장 remote:{0}, chanID:{1}", remote, chanID);

            int RoomCount; RemoteClass.Marshaler.Read(Msg, out RoomCount);

            for (int i = 0; i < RoomCount; ++i)
            {
                RemoteClass.Marshaler.RoomInfo roominfo = new RemoteClass.Marshaler.RoomInfo();

                RemoteClass.Marshaler.Read(Msg, out roominfo.roomID);
                RemoteClass.Marshaler.Read(Msg, out roominfo.chanID);
                RemoteClass.Marshaler.Read(Msg, out roominfo.chanType);
                RemoteClass.Marshaler.Read(Msg, out roominfo.number);
                RemoteClass.Marshaler.Read(Msg, out roominfo.stakeType);
                RemoteClass.Marshaler.Read(Msg, out roominfo.userCount);
                RemoteClass.Marshaler.Read(Msg, out roominfo.restrict);
                RemoteClass.Marshaler.Read(Msg, out roominfo.remote_svr);
                RemoteClass.Marshaler.Read(Msg, out roominfo.remote_lobby);
                RemoteClass.Marshaler.Read(Msg, out roominfo.needPassword);
                RemoteClass.Marshaler.Read(Msg, out roominfo.roomPassword);

                for (int j = 0; j < roominfo.userCount; ++j)
                {
                    string IP;
                    RemoteClass.Marshaler.LobbyUserList userInfo = new RemoteClass.Marshaler.LobbyUserList();
                    //
                    RemoteClass.Marshaler.Read(Msg, out userInfo.ID);
                    RemoteClass.Marshaler.Read(Msg, out IP);
                    roominfo.userList.TryAdd(userInfo.ID, IP);

                    RemoteClass.Marshaler.Read(Msg, out userInfo.RemoteID);
                    RemoteClass.Marshaler.Read(Msg, out userInfo.nickName);
                    RemoteClass.Marshaler.Read(Msg, out userInfo.FreeMoney);
                    RemoteClass.Marshaler.Read(Msg, out userInfo.PayMoney);
                    userInfo.chanID = roominfo.chanID;
                    userInfo.roomNumber = roominfo.number;
                    LobbyUserList.TryAdd(userInfo.ID, userInfo);
                }

                RemoteRoomInfos.TryAdd(roominfo.roomID, roominfo);
                ChannelsRoomsCount[roominfo.chanID]++;
            }

            return true;
        }
        bool RelayLobbyCurrent(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage Msg)
        {
            int relayID; RemoteClass.Marshaler.Read(Msg, out relayID);
            if (RemoteRelays.TryAdd(remote, relayID))
            {
                form.printf("릴레이 서버 입장 remote:{0}, relayID:{1}", remote, relayID);
                return true;
            }

            return false;
        }

        bool LobbyRoomMoveRoomRedirect(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool makeRoom, Guid roomID, ZNet.NetAddress roomAddr, int chanID, ZNet.RemoteID userRemoteID, string errorMessage)
        {
            proxy.lobby_room_moveroom_response(remote, pkOption, makeRoom, roomID, roomAddr, chanID, userRemoteID, errorMessage);

            return true;
        }

        bool KickPlayer(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int UserId)
        {
            bool kicked = false;
            foreach (var user in RemoteClients)
            {
                if (user.Value.data.ID == UserId)
                {
                    form.printf("강제 접속종료2 {0}", UserId);
                    ClientDisconect(user.Key);
                    kicked = true;
                    break;
                }
            }

            if (kicked == false)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: UserId);
                    }
                    catch
                    {
                    }

                });
            }

            return true;
        }

        bool KickSession(ZNet.RemoteID remote, string StackTrace, string Message)
        {
            CPlayer Player;
            if (RemoteClients.TryGetValue(remote, out Player))
            {
                ClientDisconect(Player.remote);
                form.printf("패킷에러 유저 강제 접속종료 {0}", Player.data.userID);
                form.printf("에러 StackTrace: {0}", StackTrace);
                form.printf("에러 Message: {0}", Message);
            }

            return true;
        }

        // 방 목록 일괄 전송
        DateTime BrodcastRoomListTime = DateTime.Now;
        bool RoomListUpdate;
        void Brodcast_Room_List()
        {
            if (BrodcastRoomListTime > DateTime.Now)
            {
                RoomListUpdate = true;
                return;
            }
            BrodcastRoomListTime = DateTime.Now.AddSeconds(1);

            foreach (var client in RemoteClients)
            {
                proxy.notify_room_list(client.Key, ZNet.CPackOption.Basic, client.Value.channelNumber, RemoteRoomInfos);
            }
            RoomListUpdate = false;
        }
        void Brodcast_User_Info()
        {
            foreach (var client in RemoteClients)
            {
                proxy.server_lobby_user_info(client.Key, ZNet.CPackOption.Basic, makeLobbyUserInfo(client.Value.data));
            }
        }
        DateTime BrodcastUserListTime = DateTime.Now;
        bool UserListUpdate;
        void Brodcast_User_List()
        {
            if (BrodcastUserListTime > DateTime.Now)
            {
                UserListUpdate = true;
                return;
            }
            BrodcastUserListTime = DateTime.Now.AddSeconds(3);

            var UserList = LobbyUserList.Values.ToList();

            foreach (var client in RemoteClients)
            {
                proxy.server_lobby_user_list(client.Key, ZNet.CPackOption.Basic, UserList, client.Value.friendList);
            }
            UserListUpdate = false;
        }
        void Brodcast_Notify_Message(int type, string message, int showTime)
        {
            foreach (var client in RemoteClients)
            {
                // 공지 타입 : 전체 공지, 로비 공지, 룸 공지 등
                proxy.lobby_notify_message(client.Key, ZNet.CPackOption.Basic, type, message, showTime);
            }

            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList((int)ServerType.Relay, out svr_array); // 릴레이 서버에 공지 전달
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    proxy.lobby_room_notify_message(svr.m_remote, ZNet.CPackOption.Basic, type, message, showTime);
                }
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

            foreach (var client in RemoteClients)
            {
                // 공지 타입 : 전체 공지, 로비 공지, 룸 공지 등
                proxy.lobby_notify_message(client.Key, ZNet.CPackOption.Basic, type, message, 30);
            }

            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList((int)ServerType.Room, out svr_array);
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    proxy.lobby_room_notify_servermaintenance(svr.m_remote, ZNet.CPackOption.Basic, type, message, Release);
                }
            }

            m_Core.GetServerList((int)ServerType.Relay, out svr_array); // 릴레이 서버에 공지 전달
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    proxy.lobby_room_notify_message(svr.m_remote, ZNet.CPackOption.Basic, type, message, 30);
                }
            }
        }
        void Brodcast_Reload_ServerData(int type)
        {
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList((int)ServerType.Room, out svr_array);
            if (svr_array != null)
            {
                foreach (var svr in svr_array)
                {
                    proxy.lobby_room_reload_serverdata(svr.m_remote, ZNet.CPackOption.Basic, type);
                }
            }
        }
        void Reload_ServerData_GiveMoney()
        {
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(1).FirstOrDefault(); // 무료머니
                    RechargeFreeMoney = Data_GiveMoney.RechargeMoney;
                    RechargeFreeCount = Data_GiveMoney.RechargeCount;
                    Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(2).FirstOrDefault(); // 유료머니
                    RechargePayMoney = Data_GiveMoney.RechargeMoney;
                    RechargePayCount = Data_GiveMoney.RechargeCount;
                }
                catch
                {
                    form.printf("지급머니 다시 불러오기 실패");
                    Log._log.Info("지급머니 다시 불러오기 실패");
                }
            });
        }
        void Brodcast_Kick_Player(int UserId)
        {
            Task.Run(() =>
            {
                try
                {
                    bool UserInLobby = false;

                    dynamic Data_Current = Simple.Data.Database.Open().GameCurrentUser.FindAllByUserID(UserId).FirstOrDefault();
                    if (Data_Current != null)
                    {
                        // 회원이 로비에 있으면 강제 접속종료
                        ZNet.RemoteID userRemoteID = RemoteID.Remote_None;
                        foreach (var user in RemoteClients)
                        {
                            if (user.Value.data.ID == UserId)
                            {
                                form.printf("강제 접속종료. player:{0}", user.Value.data.userID);
                                Log._log.WarnFormat("강제 접속종료. player:{0}", user.Value.data.userID);
                                UserInLobby = true;
                                ClientDisconect(user.Key);
                                userRemoteID = user.Value.remote;
                                break;
                            }
                        }

                        if (Data_Current.RoomId == 0 && UserInLobby == false)
                        {
                            if (LobbyUserList.ContainsKey(UserId))
                            {
                                RemoteClass.Marshaler.LobbyUserList temp;
                                LobbyUserList.TryRemove(UserId, out temp);
                                Brodcast_User_List();
                            }
                        }

                        Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: UserId);
                    }

                    // 회원이 로비에 없으면 룸서버에서 찾아서 강제 접속종료
                    if (UserInLobby == false)
                    {
                        ZNet.MasterInfo[] svr_array;
                        m_Core.GetServerList((int)ServerType.Room, out svr_array);
                        if (svr_array != null)
                        {
                            foreach (var svr in svr_array)
                            {
                                proxy.lobby_room_kick_player(svr.m_remote, ZNet.CPackOption.Basic, UserId);
                            }
                        }
                    }
                }
                catch
                {
                    form.printf("강제 접속종료 실패 {0}", UserId);
                    Log._log.ErrorFormat("강제 접속종료 실패 {0}", UserId);
                }
            });
        }
        #endregion

        #region ServerToServer
        // --- 서버간 통신 패킷 ---
        bool RoomLobbyMakeRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, RemoteClass.Marshaler.RoomInfo roomInfo, RemoteClass.Marshaler.LobbyUserList UserInfo, int userID, string IP)
        {
            if (RemoteRoomInfos.TryAdd(roomInfo.roomID, roomInfo))
            {
                roomInfo.userList.TryAdd(userID, IP);
                LobbyUserInfoUpdate(userID, UserInfo);

                int rooms;
                if (ChannelsRoomsCount.TryGetValue(roomInfo.chanID, out rooms))
                {
                    ++ChannelsRoomsCount[roomInfo.chanID];
                }

                Brodcast_Room_List();
            }

            // 릴레이 서버에 알림
            foreach(var relay in RemoteRelays)
            {
                proxy.room_lobby_makeroom(relay.Key, ZNet.CPackOption.Basic, roomInfo, UserInfo, userID, IP);
            }

            return true;
        }

        bool RoomLobbyJoinRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Guid roomID, RemoteClass.Marshaler.LobbyUserList UserInfo, int userID, string IP)
        {
            RemoteClass.Marshaler.RoomInfo roomInfo;
            if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
            {
                roomInfo.userList.TryAdd(userID, IP);

                ++roomInfo.userCount;
                if (roomInfo.userCount >= CGameRoom.max_users)
                {
                    roomInfo.restrict = true;
                }

                LobbyUserInfoUpdate(userID, UserInfo);

                Brodcast_Room_List();

                // 릴레이 서버에 알림
                foreach (var relay in RemoteRelays)
                {
                    proxy.room_lobby_joinroom(relay.Key, ZNet.CPackOption.Basic, roomID, UserInfo, userID, IP);
                }

                return true;
            }

            return false;
        }

        bool RoomLobbyOutRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Guid roomID, int userID)
        {
            RemoteClass.Marshaler.RoomInfo roomInfo;
            int chanID = 0;
            if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
            {
                string temp_;
                roomInfo.userList.TryRemove(userID, out temp_);

                --roomInfo.userCount;
                roomInfo.restrict = false;
                chanID = roomInfo.chanID;

                if (roomInfo.userCount <= 0)
                {
                    RemoteClass.Marshaler.RoomInfo temp;
                    RemoteRoomInfos.TryRemove(roomID, out temp);

                    int rooms;
                    if (ChannelsRoomsCount.TryGetValue(roomInfo.chanID, out rooms))
                    {
                        --ChannelsRoomsCount[roomInfo.chanID];
                    }
                }

                if (IsExistChnnel(chanID))
                {
                    Brodcast_Room_List();
                }
            }
            else
            {
                return false;
            }

            // 릴레이 서버에 알림
            foreach (var relay in RemoteRelays)
            {
                proxy.room_lobby_outroom(relay.Key, ZNet.CPackOption.Basic, roomID, userID);
            }

            if (LobbyUserList.ContainsKey(userID))
            {
                RemoteClass.Marshaler.LobbyUserList temp_;
                if (LobbyUserList.TryGetValue(userID, out temp_) && temp_.roomNumber != 0)
                {
                    RemoteClass.Marshaler.LobbyUserList temp;
                    LobbyUserList.TryRemove(userID, out temp);
                    Brodcast_User_List();
                }
            }

            return true;
        }

        bool RoomLobbyEventStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Guid roomID, int eventType)
        {
            try
            {
                RemoteClass.Marshaler.RoomInfo roomInfo;
                if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
                {
                    string NoticeMessage = "";

                    switch (eventType)
                    {
                        case 1: // 판수 이벤트
                            {
                                NoticeMessage = "[전우치 잭팟] ";

                                if (roomInfo.chanID == 1)
                                    NoticeMessage += "실버채널 ";
                                else if (roomInfo.chanID == 2)
                                    NoticeMessage += "실버자유채널 ";
                                else if (roomInfo.chanID == 3)
                                    NoticeMessage += "골드채널 ";
                                else
                                    NoticeMessage += "채널 ";

                                NoticeMessage += roomInfo.number.ToString() + "번방에 전우치가 나타났습니다!";
                            }
                            break;
                    }

                    Brodcast_Notify_Message(0, NoticeMessage, 5);

                    return true;
                }

            }
            catch (Exception e)
            {
                form.printf("RoomLobbyEventStart 예외발생. {0}\n", e.ToString());
                return false;
            }

            return false;
        }
        bool RoomLobbyEventEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Guid roomID, int eventType, string name, long rewards)
        {
            try
            {
                RemoteClass.Marshaler.RoomInfo roomInfo;
                if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
                {
                    string NoticeMessage = "";

                    switch (eventType)
                    {
                        case 1: // 판수 잭팟 이벤트
                            {
                                NoticeMessage = "[전우치 잭팟] ";

                                if (roomInfo.chanID == 1)
                                    NoticeMessage += "실버채널 ";
                                else if (roomInfo.chanID == 2)
                                    NoticeMessage += "실버자유채널 ";
                                else if (roomInfo.chanID == 3)
                                    NoticeMessage += "골드채널 ";
                                else
                                    NoticeMessage += "채널 ";

                                NoticeMessage += roomInfo.number.ToString() + "번방에서 " + name + " 님이 ";

                                if (roomInfo.chanID == 1 || roomInfo.chanID == 2)
                                    NoticeMessage += "실버 ";
                                else if (roomInfo.chanID == 3)
                                    NoticeMessage += "골드 ";
                                else
                                    NoticeMessage += "머니 ";

                                NoticeMessage += String.Format("{0:0,0}", rewards) + " 당첨되었습니다.";
                            }
                            break;
                        case 2: // 골프 잭팟 이벤트
                            {
                                NoticeMessage = "[골프 잭팟] ";

                                if (roomInfo.chanID == 1)
                                    NoticeMessage += "실버채널 ";
                                else if (roomInfo.chanID == 2)
                                    NoticeMessage += "실버자유채널 ";
                                else if (roomInfo.chanID == 3)
                                    NoticeMessage += "골드채널 ";
                                else
                                    NoticeMessage += "채널 ";

                                NoticeMessage += roomInfo.number.ToString() + "번방에서 " + name + " 님이 ";

                                if (roomInfo.chanID == 1 || roomInfo.chanID == 2)
                                    NoticeMessage += "실버 ";
                                else if (roomInfo.chanID == 3)
                                    NoticeMessage += "골드 ";
                                else
                                    NoticeMessage += "머니 ";

                                NoticeMessage += String.Format("{0:0,0}", rewards) + " 당첨되었습니다.";
                            }
                            break;
                    }

                    Brodcast_Notify_Message(0, NoticeMessage, 5);

                    return true;
                }

            }
            catch (Exception e)
            {
                form.printf("RoomLobbyEventEnd 예외발생. {0}\n", e.ToString());
                return false;
            }

            return false; ;
        }

        bool RoomLobbyError(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string errorID)
        {
            proxy.response_lobby_error(remote, ZNet.CPackOption.Basic, errorID);

            return true;
        }
        void LobbyUserInfoUpdate(int userID, RemoteClass.Marshaler.LobbyUserList userInfo)
        {
            if (!LobbyUserList.ContainsKey(userID))
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
        #endregion

        #region DB
        void DB_Server_CurrentPlayerClear()
        {
            try
            {
                Simple.Data.Database.Open().GameCurrentUser.DeleteByGameId(GameId: GameId);
                Simple.Data.Database.Open().GameRoomList.DeleteByGameId(GameId: GameId);
            }
            catch (Exception e)
            {
                form.printf("DB_Server_CurrentPlayerClear {0}\n", e.ToString());
            }
        }
        void DB_Server_GetLobbyData()
        {
            // 서버 설정값 확인
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_JackPot = Simple.Data.Database.Open().GameJackPotMoney.All().FirstOrDefault();

                    JackPotMoney = (long)Data_JackPot.JackPotMoney;
                    form.printf("DB_Server_GetLobbyData. JP:{0}", JackPotMoney);
                    Log._log.InfoFormat("DB_Server_GetLobbyData. JP:{0}", JackPotMoney);

                    Simple.Data.Database.Open().GameServerMessage.DeleteByGameId(GameId);

                    dynamic Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(1).FirstOrDefault(); // 무료머니
                    RechargeFreeMoney = Data_GiveMoney.RechargeMoney;
                    RechargeFreeCount = Data_GiveMoney.RechargeCount;
                    Data_GiveMoney = Simple.Data.Database.Open().GameGiveMoney.FindAllByMoneyType(2).FirstOrDefault(); // 유료머니
                    RechargePayMoney = Data_GiveMoney.RechargeMoney;
                    RechargePayCount = Data_GiveMoney.RechargeCount;
                }
                catch (Exception e)
                {
                    form.printf("DB_Server_GetLobbyData 예외발생. {0}\n", e.ToString());
                    Log._log.ErrorFormat("DB_Server_GetLobbyData 예외발생. {0}\n", e.ToString());
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
                        dynamic Data_ServerMessage = Simple.Data.Database.Open().GameServerMessage.FindAllByGameId(GameId).ToList();

                        if (Data_ServerMessage == null || Data_ServerMessage.Count == 0) return 0;

                        foreach (var Message in Data_ServerMessage)
                        {
                            if (Message.Date >= DateTime.Now) continue;
                            ServerMessage data;
                            int Id = Message.Id;
                            data.type = Message.Type;
                            data.value1 = Message.Value1;
                            data.value2 = Message.Value2;
                            data.value3 = Message.Value3;
                            data.value4 = Message.Value4;
                            data.value5 = Message.Value5;
                            data.string1 = Message.String1;
                            data.string2 = Message.String2;
                            MessagePool.Add(data);
                            Simple.Data.Database.Open().GameServerMessage.DeleteById(Id);
                        }

                        if (MessagePool.Count == 0) return 0;

                        return 1;
                    }
                    catch (Exception e)
                    {
                        form.printf("DB_Server_GetServerMessage 예외발생. {0}\n", e.ToString());
                    }

                    return 0;
                });

                if (dbResult == 1)
                {
                    // 공지사항 처리
                    foreach (var message in MessagePool)
                    {
                        switch (message.type)
                        {
                            case 0:
                                {

                                }
                                break;
                            case 1:
                                {
                                    form.printf("공지사항 : {0}\n", message.string1);
                                    Brodcast_Notify_Message(message.value1, message.string1, message.value2);
                                }
                                break;
                            case 2:
                                {
                                    form.printf("서버점검 : {0}\n", message.string1);
                                    this.ServerMaintenance = true;
                                    Brodcast_Notify_ServerMaintenance(message.value1, message.string1, message.value2);
                                }
                                break;
                            case 3:
                                {
                                    form.printf("서버데이터 다시 불러오기 : {0}\n", message.value1);
                                    Brodcast_Reload_ServerData(message.value1);
                                }
                                break;
                            case 4:
                                {
                                    form.printf("회원 강제로 접속종료 : {0}\n", message.value1);
                                    Brodcast_Kick_Player(message.value1);
                                }
                                break;
                            case 5:
                                {
                                    form.printf("지급머니 다시 불러오기");
                                    Reload_ServerData_GiveMoney();
                                }
                                break;
                        }
                    }
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
                    dynamic Data_UserMoney = Simple.Data.Database.Open().V_CurrentUserMoney.FindAllByGameId(GameId);
                    foreach (var row in Data_UserMoney.ToList())
                    {
                        RemoteClass.Marshaler.LobbyUserList User;
                        if (LobbyUserList.TryGetValue(row.UserId, out User))
                        {
                            CPlayer rc;
                            if (RemoteClients.TryGetValue(User.RemoteID, out rc))
                            {
                                rc.data.money_free = (long)row.FreeMoney;
                                rc.data.money_pay = (long)row.PayMoney;
                                rc.data.member_point = (long)row.Point;
                                rc.data.bank_money_free = (long)row.BankFreeMoney;
                                rc.data.bank_money_pay = (long)row.BankPayMoney;
                            }

                            User.FreeMoney = (long)row.FreeMoney;
                            User.PayMoney = (long)row.PayMoney;
                        }
                    }
                    Brodcast_User_Info();
                    Brodcast_User_List();
                }
                catch (Exception e)
                {
                    form.printf("DB_Server_SendPlayerInfo {0}\n", e.ToString());
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
                    dynamic Data_JackPot = Simple.Data.Database.Open().GameJackPotMoney.All().FirstOrDefault();

                    if (Data_JackPot == null) return;

                    JackPotMoney = (long)Data_JackPot.JackPotMoney;

                    // 로비에 있는 유저에게 일괄 전송
                    foreach (var user in RemoteClients)
                    {
                        proxy.server_lobby_jackpot_info(user.Key, ZNet.CPackOption.Basic, this.JackPotMoney);
                    }

                    // 릴레이 서버에 일괄 전송
                    ZNet.MasterInfo[] svr_array;
                    m_Core.GetServerList((int)ServerType.Relay, out svr_array);
                    if (svr_array != null)
                    {
                        foreach (var svr in svr_array)
                        {
                            proxy.lobby_room_jackpot_info(svr.m_remote, ZNet.CPackOption.Basic, this.JackPotMoney);
                        }
                    }
                }
                catch (Exception e)
                {
                    form.printf("DB_Server_SendJackPotMoney {0}\n", e.ToString());
                }

                return;
            });
        }
        void DB_User_Login(int UserId, string ip)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.Insert(UserId: UserId, Locate: 0, GameId: GameId, ChannelId: 0, RoomId: 0, IP: ip, AutoPlay: false);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_Login 예외발생 {0}\n", e.ToString());
                }
            });
        }
        void DB_User_CurrentUpdate(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.UpdateByUserId(UserId: userId, ChannelId: 0, RoomId: 0);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_CurrentUpdate 예외발생 {0}\n", e.ToString());
                }
            });
        }
        bool DB_User_Logout(int UserId)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: UserId);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_Logout 예외발생 {0}\n", e.ToString());
                }
            });

            return true;
        }
        void DB_User_UpdateBank(ZNet.RemoteID remote, CPlayer user, int option, long money, string password)
        {
            // 금고 금액, 비밀번호 확인
            Task.Run(async () =>
            {
                long GameMoney = 0;
                long GameMoney2 = 0;
                long SafeMoney = 0;
                long SafeMoney2 = 0;
                long MemberPoint = 0;

                var dbResult = await Task.Run(() =>
                {
                    if (money <= 0) return 1;

                    //form.printf("[DB] 금고 이용중 userID:{0}\n", user.data.userID);
                    try
                    {
                        dynamic Data_Player = Simple.Data.Database.Open().Player.FindAllById(user.data.ID).FirstOrDefault();
                        dynamic Data_GameMoney = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserId(user.data.ID).FirstOrDefault();
                        dynamic Data_SafeBox = Simple.Data.Database.Open().PlayerSafeBox.FindAllByUserId(user.data.ID).FirstOrDefault();

                        if (Data_Player == null || Data_GameMoney == null || Data_SafeBox == null) return 0;
                        //if (Data_Player.MyBoxPassword != password) return 2;

                        GameMoney = (long)Data_GameMoney.GameMoney;
                        GameMoney2 = (long)Data_GameMoney.PayMoney;
                        SafeMoney = (long)Data_SafeBox.SafeMoney;
                        SafeMoney2 = (long)Data_SafeBox.SafeMoney2;
                        MemberPoint = (long)Data_Player.Point;
                    }
                    catch (Exception e)
                    {
                        form.printf("[DB] 금고 열람 예외발생 {0}\n", e.ToString());
                        return 1;
                    }

                    return 0;
                });

                // 인증 성공
                if (dbResult == 0)
                {
                    switch (option)
                    {
                        // 적립금 전환
                        case 1:
                            {
                                if (MemberPoint >= money)
                                {
                                    MemberPoint -= money;
                                    GameMoney += money;
                                }
                                else
                                {
                                    GameMoney += MemberPoint;
                                    MemberPoint = 0;
                                }
                            }
                            break;
                        // 무료머니 입금
                        case 2:
                            {
                                if (GameMoney >= money)
                                {
                                    GameMoney -= money;
                                    SafeMoney += money;
                                }
                                else
                                {
                                    SafeMoney += GameMoney;
                                    GameMoney = 0;
                                }
                            }
                            break;
                        // 무료머니 출금
                        case 3:
                            {
                                if (SafeMoney >= money)
                                {
                                    SafeMoney -= money;
                                    GameMoney += money;
                                }
                                else
                                {
                                    GameMoney += SafeMoney;
                                    SafeMoney = 0;
                                }
                            }
                            break;
                        // 유료머니 입금
                        case 4:
                            {
                                if (GameMoney2 >= money)
                                {
                                    GameMoney2 -= money;
                                    SafeMoney2 += money;
                                }
                                else
                                {
                                    SafeMoney2 += GameMoney2;
                                    GameMoney2 = 0;
                                }
                            }
                            break;
                        // 유료머니 출금
                        case 5:
                            {
                                if (SafeMoney2 >= money)
                                {
                                    SafeMoney2 -= money;
                                    GameMoney2 += money;
                                }
                                else
                                {
                                    GameMoney2 += SafeMoney2;
                                    SafeMoney2 = 0;
                                }
                            }
                            break;
                    }

                    await Task.Run(() =>
                    {
                        try
                        {
                            switch (option)
                            {
                                case 1:
                                    Simple.Data.Database.Open().PlayerGameMoney.UpdateByUserId(UserId: user.data.ID, GameMoney: GameMoney, PayMoney: GameMoney2);
                                    Simple.Data.Database.Open().Player.UpdateById(Id: user.data.ID, Point: MemberPoint);
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                    Simple.Data.Database.Open().PlayerGameMoney.UpdateByUserId(UserId: user.data.ID, GameMoney: GameMoney, PayMoney: GameMoney2);
                                    Simple.Data.Database.Open().PlayerSafeBox.UpdateByUserId(UserId: user.data.ID, SafeMoney: SafeMoney, SafeMoney2: SafeMoney2);
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            form.printf("[DB] 금고 저장 예외발생 {0}\n", e.ToString());
                            return;
                        }
                    });

                    user.data.money_free = GameMoney;
                    user.data.money_pay = GameMoney2;
                    user.data.bank_money_free = SafeMoney;
                    user.data.bank_money_pay = SafeMoney2;
                    user.data.member_point = MemberPoint;

                    proxy.response_bank(remote, ZNet.CPackOption.Basic, true, 0);
                    proxy.server_lobby_user_info(remote, ZNet.CPackOption.Basic, makeLobbyUserInfo(user.data));
                    LobbyUserInfoUpdate(user.data.ID, user.data.money_free, user.data.money_pay);
                }
                else
                {
                    proxy.response_bank(remote, ZNet.CPackOption.Basic, false, dbResult); // 1:에러, 2: 비밀번호 틀림
                }

                return;
            });
        }
        #endregion DB

        bool ShutDownServer(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            ServerMaintenance = true;
            ServerMsg = "서버 점검중입니다.";
            ShutDown = true;
            CountDown = DateTime.Now.AddMinutes(1);

            form.printf("서버종료 요청 받음.");
            Log._log.Warn("서버종료 요청 받음.");

            return true;
        }
    }
}

