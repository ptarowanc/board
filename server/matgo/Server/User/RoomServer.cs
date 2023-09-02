using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ZNet;

namespace Server.User
{
    public class RoomServer : UserServer
    {
        public object RoomServerLocker = new object();

        public int ChannelID; // 채널 ID
        public ChannelType ChanType; // 채널타입
        public int svrRemoteID = 0; // 서버 RemoteID

        // 클라이언트 목록
        public ConcurrentDictionary<ZNet.RemoteID, CPlayer> RemoteClients;
        // 방 목록
        public ConcurrentDictionary<Guid, CGameRoom> RemoteRooms;

        public Stack<int> RoomNumbers;

        // 게임비
        public long JackPotMoney;       // 잭팟 머니
        public double DealerFee;        // 딜러비 비율
        public double JackPotRate;      // 잭팟 적립 비율

        // 홍길동 이벤트 발동조건
        private long EventTermCount;     // 발동 카운트
        private long EventTermX200;       // 200배 이벤트 발동 판수

        // 밀어주기, 가중치
        public long PushType;
        public Dictionary<long, long> BonusBase; // 기본값
        public Dictionary<long, long> BonusPushUser; // 유저 레벨 가중치
        public Dictionary<long, long> BonusPushGame; // 게임 레벨 가중치

        Random rng = new Random(new System.DateTime().Millisecond);

        public RoomServer(FormServer f, UnityCommon.Server t, int portnum, int channelID) : base(f, t, portnum)
        {
            ChannelID = channelID;
            ChanType = new ChannelType();
            RemoteClients = new ConcurrentDictionary<RemoteID, CPlayer>();
            //            RecoveryClients = new ConcurrentDictionary<int, ConcurrentDictionary<RemoteID, CPlayer>>();
            RemoteRooms = new ConcurrentDictionary<Guid, CGameRoom>();
            RoomNumbers = new Stack<int>();
            DealerFee = new double();
            JackPotRate = new double();
            EventTermCount = new long();
            EventTermX200 = new long();
            PushType = new long();
            BonusBase = new Dictionary<long, long>();
            BonusPushUser = new Dictionary<long, long>();
            BonusPushGame = new Dictionary<long, long>();

            ChanType = GetChnnelType(ChannelID);
            for (int RoomNumber = 200; RoomNumber > 0; --RoomNumber)
            {
                RoomNumbers.Push(RoomNumber);
            }

            DB_Server_GetRoomData();
        }

        ~RoomServer()
        {
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            base.BeforeServerStart(out param);

            //param.m_UdpPorts = new int[1];
            //param.m_UdpPorts[0] = ListenAddr.m_port + 100;
            //param.m_RecoveryTimeMs = 50000;

            //업데이트 콜백 이벤트 시간을 설정
            param.m_UpdateTimeMs = 1000;
            // 주기적으로 업데이트할 필요가 있는 내용들...
            //m_Core.update_event_handler = ScheduleTask;
            stub.roomServer = this;

            stub.master_all_shutdown = ShutDownServer;

            // 채팅 메세지 처리
            stub.Chat = Chatting;
            // 방 처리
            //stub.request_out_room = RequestOutRoom;
            //stub.request_move_room = RequestMoveRoom;

            // 로비 <=> 룸
            //stub.lobby_room_jackpot_info = RefreshJackpotInfo; // 로비 -> 릴레이 -> 클라
            //stub.lobby_room_notify_message = NotifyMessage; // 로비 -> 릴레이 -> 클라
            stub.lobby_room_notify_servermaintenance = NotifyServerMaintenance; // 로비 -> 룸
            stub.lobby_room_reload_serverdata = ReloadServerData; // 로비 -> 룸
            stub.lobby_room_kick_player = KickPlayer; // 로비 -> 룸 -> 릴레이 -> 클라
            stub.KickSession = KickSession; // 로비 -> 룸 -> 릴레이 -> 클라
            stub.lobby_room_moveroom_response = RelayLobbyRoomMoveRoom; // 릴레이 -> 룸 -> 릴레이 -> 클라
            stub.lobby_room_current_request = CurrentRooms;

            //----서버 메시지----
            // 파라미터 검사후 서버이동 승인 여부 결정하기
            //m_Core.move_server_start_handler = MoveServerStart;
            //m_Core.move_server_param_handler = MoveServerParam;
            //m_Core.move_server_failed_handler = MoveServerFailed;
            //m_Core.client_join_handler = ClientJoin;
            //m_Core.client_leave_handler = ClientLeave;
            m_Core.message_handler = CoreMessage;
            m_Core.exception_handler = CoreException;
            m_Core.server_join_handler = ServerJoin;
            m_Core.server_leave_handler = ServerLeave;
            m_Core.server_master_join_handler = ServerMasterJoin;
            m_Core.server_master_leave_handler = ServerMasterLeave;
            m_Core.server_refresh_handler = ServerRefresh;

            // 릴레이 서버 메시지
            stub.RelayClientJoin = RelayClientJoin;
            stub.RelayClientLeave = RelayClientLeave;
            stub.RelayRequestOutRoom = RelayRequestOutRoom;
            stub.RelayRequestMoveRoom = RelayRequestMoveRoom;

            // 서버 접속제한시점의 이벤트
            m_Core.limit_connection_handler = (ZNet.RemoteID remote, ZNet.NetAddress addr) =>
            {
                form.printf("limit_connection_handler {0}, {1} is Leave.\n", remote, addr.m_ip, addr.m_port);
            };

            // 중계 서버 이벤트
            //stub.R_request_out_room = R_RequestOutRoom;
        }
        private bool Chatting(RemoteID remote, CPackOption pkOption, string msg)
        {
            form.printf("Remote[{0}] msg : {1}", remote, msg);
            proxy.Chat(remote, ZNet.CPackOption.Basic, msg);
            return true;
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
#if DEBUG
#else
            try
#endif
            {
                ++this.tick;

                if (this.tick % 2 == 0)
                {
                    var Rooms = RemoteRooms;
                    foreach (var room in Rooms)
                    {
                        //lock (room.Value.Locker)
                        {
                            room.Value.ProcessAutoPlay();
                        }
                    }
                }

                if (this.tick % 5 == 0)
                {
                    // 입장시간 지났는데 멈춰있는 플레이어. 처리후, 응답없으면 강퇴
                    foreach (var player in RemoteClients)
                    {
                        if (player.Value.status == UserStatus.None)
                        {
                            if (player.Value.roomID == Guid.Empty && player.Value.roomTime.AddSeconds(10) < DateTime.Now)
                            {
                                form.printf("입장시간 Over. (Disconect) Player:" + player.Value.data.userID);
                                Log._log.Warn("입장시간 Over. (Disconect) Player:" + player.Value.data.userID);
                                ClientDisconect(player.Value.RelayRemote, player.Value.remote);
                            }
                        }
                    }
                }

                if (this.tick % 7 == 0)
                {
                    // 방에서 응답없는 플레이어 종료
                    var Rooms = RemoteRooms;
                    foreach (var room in Rooms)
                    {
                        lock (room.Value.Locker)
                        {
                            if (room.Value.status != RoomStatus.Stay) // 게임중 or 연습게임중
                            {
                                var result = room.Value.Check_Player_Active();
                                if (result != null)
                                {
#if DEBUG
                                        //room.Value.SaveCardLog();
                                        form.printf("actionTimeLimit Out. Player:" + result.data.userID);
                                        Log._log.Warn("actionTimeLimit Out. Player:" + result.data.userID);
                                    ClientDisconect(result.RelayRemote, result.remote);
#else
                                    //room.SaveCardLog();
                                    form.printf("actionTimeLimit Out. Player:" + result.data.userID);
                                    Log._log.Warn("actionTimeLimit Out. Player:" + result.data.userID);
                                    ClientDisconect(result.RelayRemote, result.remote);
#endif
                                }
                            }
                        }
                    }
                }

                if (this.tick % 17 == 0)
                {
                    //DisplayStatus(m_Core);

                    if (this.ShutDown)
                    {
                        int rooms = 0;
                        rooms += RemoteClients.Count;
                        form.printf("세션 종료중. 남은 세션 수:" + RemoteClients.Count + " 남은 방 수:" + RemoteRooms.Count);
                        Log._log.Info("세션 종료중. 남은 세션 수:" + RemoteClients.Count + " 남은 방 수:" + RemoteRooms.Count);

                        // 모든 세션 종료
                        if (this.CountDown < DateTime.Now)
                        {
                            if (Froce)
                                m_Core.CloseAllClient();
                            else
                                m_Core.CloseAllClientForce();
                            Froce = true;
                        }
                        // 더 이상 방이 없으면 프로그램 종료
                        if (rooms == 0)
                        {
                            Log._log.Info("서버 종료. ShutDown");
                            System.Windows.Forms.Application.Exit();
                        }
                    }
                }
            }
#if DEBUG
#else
            catch (Exception e)
            {
                form.printf("TaskTimer 에러:" + e.ToString());
                Log._log.Fatal("TaskTimer 에러:" + e.ToString());
            }
#endif
        }

        private void ClientDisconect(RemoteID remoteS, RemoteID remoteC)
        {
            RelayClientLeave(remoteS, remoteC, false);
            proxy.RelayCloseRemoteClient(remoteS, remoteC);
            Log._log.WarnFormat("Player Disconect. remoteS:{0}, remoteC:{0}", remoteS, remoteC);
        }

        private void RealyClientDisconect(RemoteID remoteS, RemoteID remoteC)
        {
            proxy.RelayCloseRemoteClient(remoteS, remoteC);
            Log._log.WarnFormat("Realy Client Disconect. remoteS:{0}, remoteC:{0}", remoteS, remoteC);
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
        }
        private void ServerLeave(RemoteID remote, NetAddress addr)
        {
            form.printf(string.Format("서버P2P맴버 퇴장 remoteID {0}", remote));
        }
        private void ServerMasterJoin(RemoteID remote, RemoteID myRemoteID)
        {
            this.svrRemoteID = (int)myRemoteID;
            form.printf(string.Format("마스터서버에 입장성공 remoteID {0}", myRemoteID));
        }
        private void ServerMasterLeave()
        {
            form.printf(string.Format("마스터서버와 연결종료!!!"));
        }
        private void ServerRefresh(MasterInfo master_info)
        {
        }

        // 릴레이 서버 메시지 처리
        private void RelayClientJoin(ZNet.RemoteID remoteS, ZNet.RemoteID remoteC, ZNet.NetAddress addr, Server.Engine.UserData userData, Server.Common.MoveParam move_param)
        {
            CPlayer rc;
            Common.Common.ServerMoveComplete(userData, out rc);
            rc.m_ip = addr.m_ip;
            rc.remote = remoteC;
            rc.RelayRemote = remoteS;

            Common.MoveParam param = move_param;

            int CID = param.ChannelNumber;

            if (ChannelID != CID || param.room_id == Guid.Empty || param.roomJoin == Common.MoveParam.ParamRoom.RoomNull)
            {
                rc.Reset();
                RemoteClients.TryAdd(remoteC, rc);
                proxy.RELAY_response_room_in(remoteS, rc.remote, false);
                KickPlayer(rc, 1);
                return;
            }

            // DB 플레이어 정보 불러오기
            //Task.Run(() =>
            //{
            try
            {
                dynamic Data_Player = Simple.Data.Database.Open().Player.FindAllById(rc.data.ID).FirstOrDefault();
                rc.data.nickName = Data_Player.NickName;
                rc.UserLevel = Data_Player.MemberLevel;

                dynamic Data_Money = Simple.Data.Database.Open().PlayerGameMoney.FindAllByUserID(rc.data.ID).FirstOrDefault();
                rc.data.money_pay = (long)Data_Money.PayMoney;
                rc.data.money_free = (long)Data_Money.GameMoney;

                //dynamic Data_Lotto = Simple.Data.Database.Open().EventLotto.FindAllByUserID(rc.data.ID);
                //rc.data.charm = Data_Lotto.ToList().Count;
                rc.data.charm = 0;

                dynamic Data_Matgo = Simple.Data.Database.Open().PlayerMatgo.FindAllByUserID(rc.data.ID).FirstOrDefault();
                rc.GameLevel = Data_Matgo.GameLevel;

                //방생성
                if (param.roomJoin == Common.MoveParam.ParamRoom.RoomMake)
                {
                    rc.Reset();
                    rc.Operator = true;
                    if (RoomNumbers.Count == 0)
                    {
                        RemoteClients.TryAdd(remoteC, rc);
                        proxy.RELAY_response_room_in(remoteS, rc.remote, false);
                        KickPlayer(rc, 2);
                        return;
                    }

                    CGameRoom new_room = new CGameRoom(this);
                    lock (new_room.Locker)
                    {
                        rc.player_index = new_room.pop_players_index();
                        rc.agent = new CPlayerAgent(rc.player_index, new_room);
                        if (ChanType == ChannelType.Charge)
                            rc.agent.setMoney(rc.data.money_pay);
                        else
                            rc.agent.setMoney(rc.data.money_free);
                        rc.roomID = param.room_id;
                        //rc.agent.topMission = rc.data.topMission;

                        new_room.From(CID, param.room_id, RoomNumbers.Pop(), param.roomStake, this.svrRemoteID, param.lobby_remote);
                        RemoteRooms.TryAdd(param.room_id, new_room);

                        new_room.PlayersConnect[rc.player_index] = rc;

                        RemoteClass.Marshaler.RoomInfo new_roominfo = new RemoteClass.Marshaler.RoomInfo();
                        new_roominfo.roomID = new_room.roomID;
                        new_roominfo.chanID = CID;
                        new_roominfo.chanType = (int)ChanType;
                        new_roominfo.number = new_room.roomNumber;
                        new_roominfo.stakeType = param.roomStake;
                        new_roominfo.userCount = 1;
                        new_roominfo.restrict = false;
                        new_roominfo.remote_svr = new_room.remote_svr;
                        new_roominfo.remote_lobby = new_room.remote_lobby;
                        if (param.roomPassword != null && param.roomPassword != "")
                        {
                            new_roominfo.needPassword = true;
                            new_roominfo.roomPassword = param.roomPassword;
                            new_room.Password = param.roomPassword;
                        }
                        else
                        {
                            new_roominfo.needPassword = false;
                            new_roominfo.roomPassword = "";
                        }

                        RemoteClass.Marshaler.LobbyUserList userInfo = new RemoteClass.Marshaler.LobbyUserList();
                        userInfo.nickName = rc.data.nickName;
                        userInfo.FreeMoney = rc.data.money_free;
                        userInfo.PayMoney = rc.data.money_pay;
                        userInfo.chanID = CID;
                        userInfo.roomNumber = new_room.roomNumber;

                        DB_User_CurrentUpdate(CID, rc.data.ID, new_room.roomNumber);
                        DB_Room_Insert(CID, new_room.roomID, new_room.roomNumber, new_room.engine.baseMoney, rc.data.ID);
                        // 로비서버에게 방생성을 알린다.
                        proxy.room_lobby_makeroom((ZNet.RemoteID)new_room.remote_lobby, ZNet.CPackOption.Basic, new_roominfo, userInfo, rc.data.ID, rc.m_ip);
                        // 클라이언트 입장시킴
                        proxy.RELAY_response_room_in(remoteS, rc.remote, true);
                    }
                }
                else if (param.roomJoin == Common.MoveParam.ParamRoom.RoomJoin)
                {
                    //방입장
                    CGameRoom room_join;

                    if (RemoteRooms.TryGetValue(param.room_id, out room_join))
                    {
                        rc.Reset();
                        rc.Operator = false;
                        lock (room_join.Locker)
                        {
                            if (room_join.PlayersConnect.Count(x => x != null) >= CGameRoom.max_users) // 인원수 초과시
                            {
                                RemoteClients.TryAdd(remoteC, rc);
                                proxy.RELAY_response_room_in(remoteS, rc.remote, false);
                                KickPlayer(rc, 3);
                                return;
                            }

                            rc.player_index = room_join.pop_players_index();
                            rc.agent = new CPlayerAgent(rc.player_index, room_join);
                            if (ChanType == ChannelType.Charge)
                                rc.agent.setMoney(rc.data.money_pay);
                            else
                                rc.agent.setMoney(rc.data.money_free);
                            //rc.agent.topMission = rc.data.topMission;
                            rc.roomID = room_join.roomID;

                            room_join.PlayersConnect[rc.player_index] = rc;

                            RemoteClass.Marshaler.LobbyUserList userInfo = new RemoteClass.Marshaler.LobbyUserList();
                            userInfo.nickName = rc.data.nickName;
                            userInfo.FreeMoney = rc.data.money_free;
                            userInfo.PayMoney = rc.data.money_pay;
                            userInfo.chanID = CID;
                            userInfo.roomNumber = room_join.roomNumber;

                            DB_User_CurrentUpdate(CID, rc.data.ID, room_join.roomNumber);
                            DB_Room_Update(room_join.roomID, room_join.PlayersConnect);

                            // 로비서버에게 방입장을 알린다.
                            proxy.room_lobby_joinroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, userInfo, rc.data.ID, rc.m_ip);
                            // 클라이언트 입장시킴
                            proxy.RELAY_response_room_in(remoteS, rc.remote, true);
                        }
                    }
                }
                else
                {
                    form.printf("Client Join RoomMake Error. player:{0}\n", rc.data.userID);
                    Log._log.ErrorFormat("Client Join RoomMake Error. Player:{0}", rc.data.userID);
                    rc.Reset();
                    rc.Operator = false;
                    RemoteClients.TryAdd(remoteC, rc);
                    proxy.RELAY_response_room_in(remoteS, rc.remote, false);
                    KickPlayer(rc, 99);
                    return;
                }

                //Log._log.InfoFormat("Client Joined. Player:{0}, Room:{1}",rc.data.userID, rc.roomID);
                RemoteClients.TryAdd(remoteC, rc);
            }
            catch (Exception e)
            {
                form.printf("Client Join Error. player:{0}\n", rc.data.userID);
                Log._log.ErrorFormat("Client Join Error. Player:{0}, Error:{1}", rc.data.userID, e.ToString());
                RealyClientDisconect(remoteS, remoteC);
            }
        }
        private void RelayClientLeave(RemoteID remoteS,ZNet.RemoteID remoteC, bool bMoveServer = false)
        {
            CPlayer rc;
            RemoteID remote = remoteC;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                CGameRoom room_join;
                if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                {
                    // 서버 이동
                    lock (room_join.Locker)
                    {
                        if (bMoveServer == true)
                        {
                            if (room_join.status == RoomStatus.Stay) // 대기중 퇴장
                            {
                                room_join.player_room_out(rc);
                                CheckRoomCount(room_join);
                                proxy.room_lobby_outroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, rc.data.ID);
                                CPlayer temp;
                                RemoteClients.TryRemove(remote, out (temp));
                            }
                            else if (room_join.status == RoomStatus.PracticeGamePlay) // 연습게임중 퇴장
                            {
                                if (rc.status == UserStatus.RoomPlay) // 방장
                                {
                                    // 연습게임 취소하고 퇴장
                                    room_join.PracticeGameEnd();

                                    room_join.player_room_out(rc);
                                    CheckRoomCount(room_join);
                                    proxy.room_lobby_outroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, rc.data.ID);
                                    CPlayer temp;
                                    RemoteClients.TryRemove(remote, out (temp));
                                }
                                else // 관전자
                                {
                                    room_join.player_room_out(rc);
                                    CheckRoomCount(room_join);
                                    proxy.room_lobby_outroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, rc.data.ID);
                                    CPlayer temp;
                                    RemoteClients.TryRemove(remote, out (temp));
                                }
                            }
                            else if (room_join.status == RoomStatus.GamePlay) // 게임중 퇴장
                            {
                                // 방 초기화
                                form.printf("[※]ClientLeave None1. ID:{0}, player:{1}, room:{2}-{3}\n", rc.data.userID, rc.status, room_join.status, room_join.roomNumber);
                                Log._log.ErrorFormat("[※]ClientLeave None1. ID:{0}, player:{1}, room:{2}-{3}\n", rc.data.userID, rc.status, room_join.status, room_join.roomNumber);
                                room_join.player_room_out_gameinit(rc);
                                CheckRoomCount(room_join);
                                proxy.room_lobby_outroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, rc.data.ID);
                                CPlayer temp;
                                RemoteClients.TryRemove(remote, out (temp));
                            }
                            else // 비정상퇴장
                            {
                                form.printf("[※]ClientLeave None2. ID:{0}, player:{1}, room:{2}\n", rc.data.userID, rc.status, room_join.status);
                                Log._log.ErrorFormat("[※]ClientLeave None2. ID:{0}, player:{1}, room:{2}\n", rc.data.userID, rc.status, room_join.status);
                                return;
                            }
                        }
                        else // 강제 종료
                        {
                            if (room_join.status == RoomStatus.Stay) // 아직 게임시작 안했으면 퇴장
                            {
                                DB_User_Logout(rc.data.ID);
                                room_join.player_room_out(rc);
                            }
                            else if (room_join.status == RoomStatus.PracticeGamePlay) // 연습게임
                            {
                                if (rc.status == UserStatus.RoomPlay) // 방장
                                {
                                    // 연습게임 취소하고 퇴장
                                    room_join.PracticeGameEnd();
                                    DB_User_Logout(rc.data.ID);
                                    room_join.player_room_out(rc);
                                }
                                else // 관전자
                                {
                                    DB_User_Logout(rc.data.ID);
                                    room_join.player_room_out(rc);
                                }
                            }
                            else if (room_join.status == RoomStatus.GamePlay) // 게임중 퇴장 (자동치기)
                            {
                                rc.status = UserStatus.RoomPlayAuto;
                                DB_User_AutoPlay(rc.data.ID);
                                rc.agent.setDummyPlayer(rc);

                                form.printf("DummyClient {0} Online2.\n", rc.data.userID);
                                Log._log.InfoFormat("DummyClient {0} Online2.\n", rc.data.userID);
                                // 처리되지 않은 행동이 있으면 수행
                                if (rc.currentMsg != null && rc.isActionExecute == false)
                                {
                                    room_join.ActionExecute(rc, rc.currentMsg);
                                }
                                CPlayer dummy;
                                RemoteClients.TryRemove(remote, out dummy);
                                return;
                            }
                            else // 비정상퇴장
                            {
                                form.printf("[※]ClientLeave None3. ID:{0}, player:{1}, room:{2}\n", rc.data.userID, rc.status, room_join.status);
                                Log._log.ErrorFormat("[※]ClientLeave None3. ID:{0}, player:{1}, room:{2}\n", rc.data.userID, rc.status, room_join.status);
                                return;
                            }

                            CheckRoomCount(room_join);
                            proxy.room_lobby_outroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, rc.data.ID);
                            CPlayer temp;
                            RemoteClients.TryRemove(remote, out temp);
                        }
                    }
                }
                else
                {
                    // 서버 이동
                    if (bMoveServer == true)
                    {

                    }
                    else
                    {
                        DB_User_Logout(rc.data.ID);
                    }
                    CPlayer temp;
                    RemoteClients.TryRemove(remote, out (temp));
                }
            }
        }
        private void RelayRequestOutRoom(ZNet.RemoteID remoteS, ZNet.RemoteID remoteC)
        {
            CPlayer rc;
            RemoteID remote = remoteC;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                CGameRoom room_join;
                if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                {
                    lock (room_join.Locker)
                    {
                        if (room_join.status != RoomStatus.GamePlay) // 대기중일때 퇴장
                        {
                            proxy.RelayResponseOutRoom(remoteS, remote, true, ChannelID, rc.data);
                            RelayClientLeave(remoteS, remote, true);
                            return;
                        }
                        else
                        {
                            Log._log.Warn("RequestOutRoom Warn. room_join.status:" + room_join.status);
                        }
                    }
                }
            }

            Server.Engine.UserData userData;
            if (rc == null)
            {
                userData = new UserData();
            }
            else
            {
                userData = rc.data;
            }

            proxy.RelayResponseOutRoom(remoteS, remote, false, ChannelID, userData);
        }
        private void RelayRequestMoveRoom(ZNet.RemoteID remoteS, ZNet.RemoteID remoteC)
        {
            CPlayer rc;
            RemoteID remote = remoteC;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                CGameRoom room_join;
                if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                {
                    lock (room_join.Locker)
                    {
                        if (room_join.status != RoomStatus.GamePlay) // 대기중일때 퇴장
                        {
                            proxy.room_lobby_moveroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, rc.remote, rc.data.ID, rc.agent.haveMoney, rc.IPFree);
                            return;
                        }
                        else
                        {
                            Log._log.Warn("RequestOutRoom Warn. room_join.status:" + room_join.status);
                        }
                    }
                }
            }

            proxy.RelayResponseMoveRoom(remoteS, remote, false, "잠시후 다시 시도하세요.");
        }
        // 릴레이 서버 메시지 처리 끝

        public void DummyPlayerLeave(CGameRoom room)
        {
            bool haveDummy = false;

            foreach (var player in room.PlayersGaming)
            {
                if (player.Value.status == UserStatus.RoomPlayAuto)
                {
                    DB_User_Logout(player.Value.data.ID);
                    room.player_room_out(player.Value);

                    //로비서버에게 방퇴장을 알린다
                    proxy.room_lobby_outroom((RemoteID)room.remote_lobby, ZNet.CPackOption.Basic, room.roomID, player.Value.data.ID);
                    form.printf("DummyClient {0} Leave\n", player.Value.data.userID);

                    haveDummy = true;
                }
            }

            if (haveDummy)
            {
                CheckRoomCount(room);
            }

            //List<CPlayer> DummyPlayer = null;

            //foreach (var player in room.players)
            //{
            //    if (player.Value.status == UserStatus.RoomPlayOut)
            //    {
            //        if (DummyPlayer == null)
            //        {
            //            DummyPlayer = new List<CPlayer>();
            //        }
            //        DummyPlayer.Add(player.Value);
            //    }
            //}

            //if (DummyPlayer != null)
            //{
            //    for (int i = 0; i < DummyPlayer.Count; ++i)
            //    {
            //        DB_User_Logout(DummyPlayer[i].data.ID);
            //        room.player_room_out(DummyPlayer[i]);

            //        //로비서버에게 방퇴장을 알린다
            //        proxy.room_lobby_outroom((RemoteID)room.remote_lobby, ZNet.CPackOption.Basic, room.roomID, DummyPlayer[i].data.ID);
            //        CPlayer temp;
            //        RemoteClients[room.ChanId].TryRemove(DummyPlayer[i].remote, out temp);
            //        form.printf("DummyClient {0} Leave. Current={1}\n", DummyPlayer[i].data.userID, RemoteClients[room.ChanId].Count);
            //    }
            //    CheckRoomCount(room);
            //}
        }
        public bool EventCheck(int ChanId)
        {
            // 이벤트 값 없으면 진행 안함
            if (EventTermX200 == 0) return false;

            ++EventTermCount;
            if (EventTermCount == EventTermX200)
            {
                EventTermCount = 0;
                return true;
            }

            return false;
        }
        public bool ProcessGame(ZNet.CRecvedMsg rm)
        {
            ZNet.RemoteID remoteS = rm.remote;
            ZNet.RemoteID remoteC;
            rm.msg.Read(out remoteC);

            if (rm.pkID < ZNet.PacketType.PacketType_User)
                return true;

            switch(rm.pkID)
            {
                case Rmi.Common.RELAY_CS_ROOM_IN: rm.pkID = Rmi.Common.CS_ROOM_IN; break;
                case Rmi.Common.RELAY_CS_READY_TO_START: rm.pkID = Rmi.Common.CS_READY_TO_START; break;
                case Rmi.Common.RELAY_CS_PLAYER_ORDER_START: rm.pkID = Rmi.Common.CS_PLAYER_ORDER_START; break;
                case Rmi.Common.RELAY_CS_DISTRIBUTED_ALL_CARDS: rm.pkID = Rmi.Common.CS_DISTRIBUTED_ALL_CARDS; break;
                case Rmi.Common.RELAY_CS_ANSWER_KOOKJIN_TO_PEE: rm.pkID = Rmi.Common.CS_ANSWER_KOOKJIN_TO_PEE; break;
                case Rmi.Common.RELAY_CS_ANSWER_GO_OR_STOP: rm.pkID = Rmi.Common.CS_ANSWER_GO_OR_STOP; break;
                case Rmi.Common.RELAY_CS_PRACTICE_GAME: rm.pkID = Rmi.Common.CS_PRACTICE_GAME; break;
                case Rmi.Common.RELAY_CS_ACTION_PUT_CARD: rm.pkID = Rmi.Common.CS_ACTION_PUT_CARD; break;
                case Rmi.Common.RELAY_CS_ACTION_FLIP_BOMB: rm.pkID = Rmi.Common.CS_ACTION_FLIP_BOMB; break;
                case Rmi.Common.RELAY_CS_ACTION_CHOOSE_CARD: rm.pkID = Rmi.Common.CS_ACTION_CHOOSE_CARD; break;
                default:
                    {
                        Log._log.ErrorFormat("ProcessGame Unknown rm.pkID:{0}", rm.pkID);
                        return false;
                    }
            }

            CPlayer rc;
            bool recived = false;
            if (RemoteClients.TryGetValue(remoteC, out rc))
            {
                CGameRoom room_join;
                if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                {
                    rc.RelayRemote = remoteS;
                    room_join.send = proxy;
                    //lock (room_join.Locker)
                    {
                        room_join.ProcessMsg(rc, rm);
                    }
                    recived = true;
                }
                //else
                //{
                //    Log._log.WarnFormat("RemoteRooms Unknown. player:{0}, room{1}", rc.data.userID, rc.roomID);
                //}
            }

            if (recived == false)
            {
                //m_Core.CloseRemoteClient(remote);
                Log._log.WarnFormat("remote Unknown. remoteC:{0}", remoteC);
            }

            return true;
        }

        #region DB
        void DB_User_CurrentUpdate(int CID, int userId, int RoomId)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.UpdateByUserId(UserId: userId, RoomId: RoomId, ChannelId: CID);
                    Simple.Data.Database.Open().Player.UpdateById(Id: userId, LastActivityDate: DateTime.Now);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_CurrentUpdate 예외발생 {0}\n", e.ToString());
                }
            });
        }
        void DB_User_Logout(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: userId);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_Logout 예외발생 {0}\n", e.ToString());
                }
            });
        }
        void DB_User_AutoPlay(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.UpdateByUserId(UserId: userId, AutoPlay: true);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_AutoPlay 예외발생 {0}\n", e.ToString());
                }
            });
        }
        void DB_Room_Insert(int CID, Guid roomID, int roomNumber, int baseMoney, int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameRoomList.Insert(Id: roomID, GameId: GameId, ChannelId: CID, RoomNumber: roomNumber, BetMoney: baseMoney, UserId1: userId, UserValue1: 0, UserDate1: DateTime.Now);
                }
                catch (Exception e)
                {
                    form.printf("DB_Room_Insert 예외발생 {0}\n", e.ToString());
                }
            });
        }
        void DB_Room_InsertAll(int CID, Guid roomID, int roomNumber, int baseMoney, CPlayer[] players)
        {
            int userId1 = 0;
            long userValue1 = 0;
            DateTime? userDate1 = null;
            int userId2 = 0;
            long userValue2 = 0;
            DateTime? userDate2 = null;

            CPlayer temp;
            if ((temp = players[0]) != null)
            {
                userId1 = temp.data.ID;
                userValue1 = temp.agent.money_var;
                userDate1 = temp.roomTime;
            }
            if ((temp = players[1]) != null)
            {
                userId2 = temp.data.ID;
                userValue2 = temp.agent.money_var;
                userDate2 = temp.roomTime;
            }
            Task.Run(() =>
            {
                try
                {
                    foreach (var player in players)
                    {
                        if (player.isPracticeDummy == true) continue;
                        Simple.Data.Database.Open().GameCurrentUser.Insert(UserId: player.data.ID, Locate: 0, GameId: GameId, ChannelId: CID, RoomId: roomNumber, IP: player.m_ip, AutoPlay: player.status == UserStatus.RoomPlayAuto);
                    }

                    Simple.Data.Database.Open().GameRoomList.Insert(Id: roomID, GameId: GameId, ChannelId: CID, RoomNumber: roomNumber, BetMoney: baseMoney, UserId1: userId1, UserValue1: userValue1, UserId2: userId2, UserValue2: userValue2);
                }
                catch (Exception e)
                {
                    form.printf("DB_Room_Update 예외발생 {0}\n", e.ToString());
                }
            });
        }
        public void DB_Room_Update(Guid roomID, CPlayer[] players)
        {
            int userId1 = 0;
            long userValue1 = 0;
            DateTime? userDate1 = null;
            int userId2 = 0;
            long userValue2 = 0;
            DateTime? userDate2 = null;

            CPlayer temp;
            if ((temp = players[0]) != null)
            {
                userId1 = temp.data.ID;
                userValue1 = temp.agent.money_var;
                userDate1 = temp.roomTime;
            }
            if ((temp = players[1]) != null)
            {
                userId2 = temp.data.ID;
                userValue2 = temp.agent.money_var;
                userDate2 = temp.roomTime;
            }

            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameRoomList.UpdateById(Id: roomID, UserId1: userId1, UserValue1: userValue1, UserDate1: userDate1, UserId2: userId2, UserValue2: userValue2, UserDate2: userDate2);
                }
                catch (Exception e)
                {
                    form.printf("DB_Room_Update 예외발생 {0}\n", e.ToString());
                }
            });
        }
        void DB_Room_Delete(Guid roomID)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameRoomList.DeleteById(Id: roomID);
                }
                catch (Exception e)
                {
                    form.printf("DB_Room_Delete 예외발생 {0}\n", e.ToString());
                }
            });
        }
        bool NotifyServerMaintenance(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int messageType, string message, int Release)
        {
            if (Release == 1)
            {
                ServerMaintenance = false;
            }
            else
            {
                ServerMaintenance = true;
                ServerMsg = "서버 점검중입니다.";
            }

            return true;
        }
        bool ReloadServerData(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int loadType)
        {

            // 서버 설정값 확인
            Task.Run(() =>
            {
                form.printf("[DB] ServerData : 확인중...");
                try
                {
                    switch ((DB_COMMAND)loadType)
                    {
                        case DB_COMMAND.MATGO_JACKPOT:
                            {
                                dynamic Data_JackPot = Simple.Data.Database.Open().GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();

                                this.EventTermX200 = Data_JackPot.Multiple200;
                            }
                            break;

                        case DB_COMMAND.MATGO_PUSH:
                            {
                                dynamic Data_Push = Simple.Data.Database.Open().GameMatgoPush.FindAllBy(ChannelId: ChannelID).FirstOrDefault();

                                this.PushType = Data_Push.PushBaseType;
                            }
                            break;
                        case DB_COMMAND.MATGO_PUSH_BASE:
                            {
                                dynamic Data_PushBase = Simple.Data.Database.Open().GameMatgoPushBase.All();

                                foreach (var row in Data_PushBase.ToList())
                                {
                                    long temp;
                                    if (this.BonusBase.TryGetValue(row.Type, out temp))
                                    {
                                        this.BonusBase[row.Type] = row.BonusCard;
                                    }
                                    else
                                    {
                                        this.BonusBase.Add(row.Type, row.BonusCard);
                                    }
                                }
                            }
                            break;
                        case DB_COMMAND.MATGO_PUSH_USER:
                            {
                                dynamic Data_PushUser = Simple.Data.Database.Open().GameMatgoPushUser.All();

                                foreach (var row in Data_PushUser.ToList())
                                {
                                    long temp;
                                    if (this.BonusPushUser.TryGetValue(row.UserLevel, out temp))
                                    {
                                        this.BonusPushUser[row.UserLevel] = row.Push;
                                    }
                                    else
                                    {
                                        this.BonusPushUser.Add(row.UserLevel, row.Push);
                                    }
                                }
                            }
                            break;
                        case DB_COMMAND.MATGO_PUSH_GAME:
                            {
                                dynamic Data_PushGame = Simple.Data.Database.Open().GameMatgoPushGame.All();

                                foreach (var row in Data_PushGame.ToList())
                                {
                                    long temp;
                                    if (this.BonusPushGame.TryGetValue(row.GameLevel, out temp))
                                    {
                                        this.BonusPushGame[row.GameLevel] = row.Push;
                                    }
                                    else
                                    {
                                        this.BonusPushGame.Add(row.GameLevel, row.Push);
                                    }
                                }
                            }
                            break;
                    }

                    form.printf("[DB] ServerData: 서버 설정 불러오기 성공({0}).\n", loadType);
                }
                catch (Exception e)
                {
                    form.printf("[DB] ServerData: 예외발생. {0}\n", e.ToString());
                }

            });

            return true;
        }
        bool KickSession(ZNet.RemoteID remote, string StackTrace, string Message)
        {
            CPlayer Player;
            if (RemoteClients.TryGetValue(remote, out Player))
            {
                form.printf("에러 확인", Message);
                CGameRoom room_cur;
                if (RemoteRooms.TryGetValue(Player.roomID, out room_cur))
                {
                    room_cur.status = RoomStatus.Stay;
                    foreach (var player in room_cur.PlayersConnect)
                    {
                        if (player == null) continue;
                        ClientDisconect(player.RelayRemote, player.remote);
                    }
                }
                else
                {
                    ClientDisconect(Player.RelayRemote, Player.remote);
                }
            }

            return true;
        }
        bool KickPlayer(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int UserId)
        {
            foreach (var user in RemoteClients)
            {
                if (user.Value.data.ID == UserId)
                {
                    // 자동치기 상태이면 버그이므로 현재 접속자에서 삭제
                    if (user.Value.status == UserStatus.RoomPlayAuto)
                    {
                        form.printf("강제 접속자 삭제 {0}", user.Value.data.userID);

                        Task.Run(() =>
                        {
                            try
                            {
                                Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: user.Value.data.ID);
                            }
                            catch
                            {
                            }

                        });
                    }
                    else
                    {
                        form.printf("강제 접속종료 {0}", user.Value.data.userID);
                        ClientDisconect(user.Value.RelayRemote, user.Key);
                    }
                }
            }
            return true;
        }
        bool RelayLobbyRoomMoveRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool makeRoom, Guid roomID, ZNet.NetAddress roomAddr, int chanID, ZNet.RemoteID userRemoteID, string errorMessage)
        {
            bool result = true;
            CPlayer rc;

            if (RemoteClients.TryGetValue(userRemoteID, out rc) == false)
            {
                return false;
            }

            ZNet.RemoteID remoteS = rc.RelayRemote;

            if (ChannelID != chanID || roomID == Guid.Empty)
            {
                errorMessage = "잠시후 다시 시도하세요.";
                proxy.RelayResponseMoveRoom(remoteS, userRemoteID, false, errorMessage);
                return false;
            }

            CGameRoom room_cur;
            if (RemoteRooms.TryGetValue(rc.roomID, out room_cur) == false)
            {
                errorMessage = "잠시후 다시 시도하세요.";
                proxy.RelayResponseMoveRoom(remoteS, userRemoteID, false, errorMessage);
                return false;
            }

            // 방 이동하기 전에 퇴장
            lock (room_cur.Locker)
            {
                if (room_cur.status == RoomStatus.Stay) // 대기중 퇴장
                {
                    room_cur.player_room_out(rc);
                    CheckRoomCount(room_cur);
                    proxy.room_lobby_outroom((RemoteID)room_cur.remote_lobby, ZNet.CPackOption.Basic, room_cur.roomID, rc.data.ID);
                }
                else if (room_cur.status == RoomStatus.PracticeGamePlay) // 연습게임중 퇴장
                {
                    if (rc.status == UserStatus.RoomPlay) // 방장
                    {
                        // 연습게임 취소하고 퇴장
                        room_cur.PracticeGameEnd();

                        room_cur.player_room_out(rc);
                        CheckRoomCount(room_cur);
                        proxy.room_lobby_outroom((RemoteID)room_cur.remote_lobby, ZNet.CPackOption.Basic, room_cur.roomID, rc.data.ID);
                    }
                    else // 관전자
                    {
                        room_cur.player_room_out(rc);
                        CheckRoomCount(room_cur);
                        proxy.room_lobby_outroom((RemoteID)room_cur.remote_lobby, ZNet.CPackOption.Basic, room_cur.roomID, rc.data.ID);
                    }
                }
                else // 비정상퇴장
                {
                    errorMessage = "잠시후 다시 시도하세요.";
                    result = false;
                }
            }

            int stake = room_cur.stake;
            int lobby_remote = room_cur.remote_lobby;

            if (result == true && errorMessage == "")
            {
                if (makeRoom)
                {
                    rc.Reset();
                    rc.Operator = true;
                    if (RoomNumbers.Count == 0)
                    {
                        proxy.RELAY_response_room_in(rc.RelayRemote, rc.remote, false);
                        KickPlayer(rc, 4); // relay
                        return false;
                    }

                    CGameRoom new_room = new CGameRoom(this);
                    lock (new_room.Locker)
                    {
                        rc.player_index = new_room.pop_players_index();
                        rc.agent = new CPlayerAgent(rc.player_index, new_room);
                        if (ChanType == ChannelType.Charge)
                            rc.agent.setMoney(rc.data.money_pay);
                        else
                            rc.agent.setMoney(rc.data.money_free);
                        rc.roomID = roomID;
                        //rc.agent.topMission = rc.data.topMission;

                        new_room.From(chanID, roomID, RoomNumbers.Pop(), stake, this.svrRemoteID, lobby_remote);
                        RemoteRooms.TryAdd(roomID, new_room);

                        new_room.PlayersConnect[rc.player_index] = rc;

                        RemoteClass.Marshaler.RoomInfo new_roominfo = new RemoteClass.Marshaler.RoomInfo();
                        new_roominfo.roomID = new_room.roomID;
                        new_roominfo.chanID = chanID;
                        new_roominfo.chanType = (int)ChanType;
                        new_roominfo.number = new_room.roomNumber;
                        new_roominfo.stakeType = stake;
                        new_roominfo.userCount = 1;
                        new_roominfo.restrict = false;
                        new_roominfo.remote_svr = new_room.remote_svr;
                        new_roominfo.remote_lobby = new_room.remote_lobby;
                        new_roominfo.needPassword = false;
                        new_roominfo.roomPassword = "";

                        RemoteClass.Marshaler.LobbyUserList userInfo = new RemoteClass.Marshaler.LobbyUserList();
                        userInfo.nickName = rc.data.nickName;
                        userInfo.FreeMoney = rc.data.money_free;
                        userInfo.PayMoney = rc.data.money_pay;
                        userInfo.chanID = chanID;
                        userInfo.roomNumber = new_room.roomNumber;

                        DB_User_CurrentUpdate(chanID, rc.data.ID, new_roominfo.number);
                        DB_Room_Insert(chanID, new_room.roomID, new_room.roomNumber, new_room.engine.baseMoney, rc.data.ID);
                        // 로비서버에게 방생성을 알린다.
                        proxy.room_lobby_makeroom((ZNet.RemoteID)new_room.remote_lobby, ZNet.CPackOption.Basic, new_roominfo, userInfo, rc.data.ID, rc.m_ip);
                        // 클라이언트 입장시킴
                        proxy.RelayResponseMoveRoom(remoteS, userRemoteID, true, "");
                        proxy.RELAY_response_room_in(rc.RelayRemote, rc.remote, true);
                    }
                    return true;
                }
                else
                {
                    //방입장
                    CGameRoom room_join;

                    if (RemoteRooms.TryGetValue(roomID, out room_join))
                    {
                        rc.Reset();
                        rc.Operator = false;
                        lock (room_join.Locker)
                        {
                            if (room_join.PlayersConnect.Count(x => x != null) >= CGameRoom.max_users) // 인원수 초과시
                            {
                                proxy.RELAY_response_room_in(rc.RelayRemote, rc.remote, false);
                                KickPlayer(rc, 5);
                                return false;
                            }

                            rc.player_index = room_join.pop_players_index();
                            rc.agent = new CPlayerAgent(rc.player_index, room_join);
                            if (ChanType == ChannelType.Charge)
                                rc.agent.setMoney(rc.data.money_pay);
                            else
                                rc.agent.setMoney(rc.data.money_free);
                            rc.roomID = room_join.roomID;
                            //rc.agent.topMission = rc.data.topMission;

                            room_join.PlayersConnect[rc.player_index] = rc;
                            RemoteClass.Marshaler.LobbyUserList userInfo = new RemoteClass.Marshaler.LobbyUserList();
                            userInfo.nickName = rc.data.nickName;
                            userInfo.FreeMoney = rc.data.money_free;
                            userInfo.PayMoney = rc.data.money_pay;
                            userInfo.chanID = chanID;
                            userInfo.roomNumber = room_join.roomNumber;

                            DB_User_CurrentUpdate(chanID, rc.data.ID, room_join.roomNumber);
                            DB_Room_Update(room_join.roomID, room_join.PlayersConnect);
                            // 로비서버에게 방입장을 알린다.
                            proxy.room_lobby_joinroom((RemoteID)room_join.remote_lobby, ZNet.CPackOption.Basic, room_join.roomID, userInfo, rc.data.ID, rc.m_ip);
                            // 클라이언트 입장시킴
                            proxy.RelayResponseMoveRoom(remoteS, userRemoteID, true, "");
                            proxy.RELAY_response_room_in(rc.RelayRemote, rc.remote, true);
                        }
                        return true;
                    }
                    else
                    {
                        errorMessage = "잠시후 다시 시도하세요.";
                        result = false;
                    }
                }

            }
            else
            {
                proxy.RelayResponseMoveRoom(remoteS, userRemoteID, result, errorMessage);
            }

            return false;
        }
        bool CurrentRooms(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool IsLobby)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Rmi.Common.room_lobby_current_response;
            Msg.WriteStart(msgID, pkOption, 0, true);

            RemoteClass.Marshaler.Write(Msg, (int)ChannelID);
            RemoteClass.Marshaler.Write(Msg, (int)RemoteRooms.Count);

            foreach (var room in RemoteRooms)
            {
                if(IsLobby)
                {
                    DB_Room_InsertAll((int)room.Value.ChanId, room.Value.roomID, room.Value.roomNumber, room.Value.BaseMoney, room.Value.PlayersConnect);
                }

                room.Value.remote_lobby = (int)remote;

                //roominfo.roomID = room.roomID;
                //roominfo.chanID = this.ChannelId;
                //roominfo.chanType = (int)this.ChannelType;
                //roominfo.number = room.roomNumber;
                //roominfo.stakeType = room.stake;
                //roominfo.userCount = room.players.Count;
                //roominfo.restrict = room.isRestrict();
                //roominfo.eventRestrict = room.isEventRestrict();
                //roominfo.remote_svr = room.remote_svr;
                //roominfo.remote_lobby = room.remote_lobby;
                //roominfo.needPassword = room.Password.Length != 0;
                //roominfo.roomPassword = room.Password;

                RemoteClass.Marshaler.Write(Msg, room.Value.roomID);
                RemoteClass.Marshaler.Write(Msg, ChannelID);
                RemoteClass.Marshaler.Write(Msg, (int)room.Value.ChanType);
                RemoteClass.Marshaler.Write(Msg, room.Value.roomNumber);
                RemoteClass.Marshaler.Write(Msg, room.Value.stake);
                RemoteClass.Marshaler.Write(Msg, room.Value.PlayersConnect.Count(x => x != null));
                RemoteClass.Marshaler.Write(Msg, room.Value.isRestrict());
                RemoteClass.Marshaler.Write(Msg, room.Value.remote_svr);
                RemoteClass.Marshaler.Write(Msg, room.Value.remote_lobby);
                RemoteClass.Marshaler.Write(Msg, room.Value.Password.Length != 0);
                RemoteClass.Marshaler.Write(Msg, room.Value.Password);

                foreach (var player in room.Value.PlayersConnect)
                {
                    if (player == null) continue;
                    RemoteClass.Marshaler.Write(Msg, player.data.ID);
                    RemoteClass.Marshaler.Write(Msg, player.m_ip);
                    RemoteClass.Marshaler.Write(Msg, player.remote);
                    RemoteClass.Marshaler.Write(Msg, player.data.nickName);
                    if (ChanType == ChannelType.Charge)
                        RemoteClass.Marshaler.Write(Msg, player.data.money_pay);
                    else
                        RemoteClass.Marshaler.Write(Msg, player.data.money_free);

                    //userinfo.ID = player.data.ID;
                    //userinfo.RemoteID = player.remote;
                    //userinfo.nickName = player.data.nickName;
                    //if (ChanType[CID] == ChannelType.Charge)
                    //    userinfo.haveMoney = player.data.money_pay;
                    //else
                    //    userinfo.haveMoney = player.data.money_free;
                    //userinfo.chanID = this.ChannelId;
                    //userinfo.roomNumber = room.roomNumber;

                }
            }

            proxy.PacketSend(remote, pkOption, Msg);

            return true;
        }
        void DB_Server_GetRoomData()
        {

            //form.printf("[DB] ServerData : 확인중...");

            // 서버 설정값 확인
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_DealFee = Simple.Data.Database.Open().GameDealFee.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();

                    DealerFee = (double)Data_DealFee.DelerFee;
                    JackPotRate = (double)Data_DealFee.JackPotRate;

                    dynamic Data_JackPot = Simple.Data.Database.Open().GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();
                    EventTermX200 = Data_JackPot.Multiple200;

                    dynamic Data_Push = Simple.Data.Database.Open().GameMatgoPush.FindAllBy(ChannelId: ChannelID).FirstOrDefault();
                    PushType = Data_Push.PushBaseType;

                    dynamic Data_PushBase = Simple.Data.Database.Open().GameMatgoPushBase.All();
                    foreach (var row in Data_PushBase.ToList())
                    {
                        long temp;
                        if (BonusBase.TryGetValue(row.Type, out temp))
                        {
                            BonusBase[row.Type] = row.BonusCard;
                        }
                        else
                        {
                            BonusBase.Add(row.Type, row.BonusCard);
                        }
                    }

                    dynamic Data_PushUser = Simple.Data.Database.Open().GameMatgoPushUser.All();
                    foreach (var row in Data_PushUser.ToList())
                    {
                        long temp;
                        if (BonusPushUser.TryGetValue(row.UserLevel, out temp))
                        {
                            BonusPushUser[row.UserLevel] = row.Push;
                        }
                        else
                        {
                            BonusPushUser.Add(row.UserLevel, row.Push);
                        }
                    }

                    dynamic Data_PushGame = Simple.Data.Database.Open().GameMatgoPushGame.All();
                    foreach (var row in Data_PushGame.ToList())
                    {
                        long temp;
                        if (BonusPushGame.TryGetValue(row.GameLevel, out temp))
                        {
                            BonusPushGame[row.GameLevel] = row.Push;
                        }
                        else
                        {
                            BonusPushGame.Add(row.GameLevel, row.Push);
                        }
                    }

                    //form.printf("[DB] ServerData: 서버 설정 불러오기 성공. {0},{1},{2}\n", DealerFee[CID], JackPotRate[CID], EventTermX200[CID]);
                }
                catch (Exception e)
                {
                    form.printf("[DB] ServerData: 예외발생. {0}\n", e.ToString());
                }

            });
        }
        #endregion

        private void CheckRoomCount(CGameRoom room)
        {
            if (room.PlayersConnect.Count(x => x != null) == 0)
            {
                CGameRoom temp;
                if (RemoteRooms.TryRemove(room.roomID, out temp))
                {
                    DB_Room_Delete(room.roomID);
                    RoomNumbers.Push(room.roomNumber);
                }
            }
            else
            {
                DB_Room_Update(room.roomID, room.PlayersConnect);
            }
        }

        /*
         * 선플레이어 손패에 보너스
         * 후플레이어 손패에 보너스
         * 바닥에 보너스
         */
        public bool IsPush(int Game1Level, int User1Level, int Game2Level, int User2Level, ref bool FirstPlayerPick)
        {
            // 보정 확률
            long BasePush;
            if (BonusBase.TryGetValue(PushType, out BasePush) == false) return false;
            if (BasePush > 0)
            {
                // 플레이어 게임, 유저 레벨 확인 후 확률 검사 (선플레이어 먼저)

                long GameLevel1Push;
                if (BonusPushGame.TryGetValue(Game1Level, out GameLevel1Push) == false) return false;
                long UserLevel1Push;
                if (BonusPushUser.TryGetValue(User1Level, out UserLevel1Push) == false) return false;
                long push = GameLevel1Push + UserLevel1Push;
                if (push > 0)
                {
                    if (rng.Next() % 100 + 1 <= BasePush * ((push) / 100))
                    {
                        FirstPlayerPick = true;
                        return true;
                    }
                }

                long GameLevel2Push;
                if (BonusPushGame.TryGetValue(Game2Level, out GameLevel2Push) == false) return false;
                long UserLevel2Push;
                if (BonusPushUser.TryGetValue(User2Level, out UserLevel2Push) == false) return false;
                push = GameLevel2Push + UserLevel2Push;
                if (push > 0)
                {
                    if (rng.Next() % 100 + 1 <= BasePush * ((push) / 100))
                    {
                        FirstPlayerPick = false;
                        return true;
                    }
                }
            }

            return false;
        }
        bool ShutDownServer(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            ServerMaintenance = true;
            ServerMsg = "서버 점검중입니다.";
            ShutDown = true;
            CountDown = DateTime.Now.AddMinutes(3);

            form.printf("서버종료 요청 받음.");
            Log._log.Warn("서버종료 요청 받음.");

            return true;
        }
        public void KickPlayer(CPlayer player, int from)
        {
            Log._log.WarnFormat("KickPlayer. player:{0} from:{1}", player.data.userID, from);
            //var st = new System.Diagnostics.StackTrace();
            //foreach (var frame in st.GetFrames())
            //{
            //    //Log._log.Error(frame.GetFileLineNumber());
            //    //Log._log.Error(frame.GetFileName());
            //    Log._log.Error(frame.GetMethod());
            //}

            CMessage newmsg = new CMessage();
            PacketType msgID = (PacketType)Rmi.Common.RELAY_SC_PLAYER_ALLIN;
            CPackOption pkOption = CPackOption.Basic;
            newmsg.WriteStart(msgID, pkOption, 0, true);
            RemoteClass.Marshaler.Write(newmsg, player.remote);
            proxy.PacketSend(player, pkOption, newmsg, msgID);
        }

    }
}
