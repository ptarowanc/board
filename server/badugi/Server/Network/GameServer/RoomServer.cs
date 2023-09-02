using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Guid = System.Guid;
using ZNet;

namespace Server.Network
{
    public class RoomServer : GameServer
    {
        internal object RoomServerLocker = new object();

        // 클라이언트 목록
        ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>> RemoteClients = new ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>>();
        // 방 목록
        public ConcurrentDictionary<Guid, CGameRoom> RemoteRooms;
        // 릴레이 로비 서버 목록
        RelayServerInfo LobbyInfo = new RelayServerInfo();
        ConcurrentDictionary<int, RelayServerInfo> RemoteRelays = new ConcurrentDictionary<int, RelayServerInfo>();
        ConcurrentDictionary<RemoteID, RelayServerInfo> RemoteRelaysh = new ConcurrentDictionary<RemoteID, RelayServerInfo>();
        // 방 번호
        public Stack<int> RoomNumbers;

        public int ChannelID; // 채널 ID
        public ChannelKind ChanKind; // 채널 유형
        public ChannelType ChanType; // 채널타입
        public bool ChanFree; // 자유채널 규칙여부

        // 설정값
        public long JackPotMoney;       // 잭팟 머니
        public double DealerFee;        // 딜러비 비율
        public double JackPotRate;      // 잭팟 적립 비율

        // 판수 이벤트 발동조건
        private object EventTermLock = new object();
        private int EventTermCount = 1;    // 발동 카운트
        private int EventTermX1;       // 50배 발동 판수
        private int EventTermX2;      // 100배 발동 판수
        private int EventTermX3;       // 200배 발동 판수
        private int EventTermZ1;        // 돌발100배 발동 판수

        // 메이드 보너스
        public int GolfRewardRatio;     // 골프일경우 지급할 보너스 판돈 배율

        //public int CheerPointLimit;     // 플레이어 응원포인트 한도
        //public double CheerPointRate;   // 플레이어 응원포인트 적립 비율(잭팟 적립비 기준)

        // 밀어주기, 가중치
        public long PushType;
        public Dictionary<long, Dictionary<long, long>> MadePushBase; // 기본값
        public Dictionary<long, long> MadePushUser; // 유저 레벨 가중치
        public Dictionary<long, long> MadePushGame; // 게임 레벨 가중치
        public Dictionary<long, long> MadePushGame2; // 게임2 레벨 가중치

        // 메이드 제한
        public int MadeLimit; // 1~3 = 골프,세컨드,써드. 4~13 : 메이드4 ~ 메이드J

        // 삥컷
        public bool BbingCutEnable; // 삥컷 적용 여부 (채널별)
        public long BbingCutTotalBetMoney; // 삥컷 최소 총 배팅머니
        public long BbingCutWinnerMoney; // 삥컷 최소 승자머니
        public bool BbingCutTrigger1;   // 삥컷트리거 (true = 판정승만)
        public bool BbingCutTrigger2;   // 삥컷트리거 (true = 저녁 레이스만)

        // 공컷
        public bool WinCutEnable; // 공컷 적용 여부 (채널별)
        public long WinCutTotalBetMoney; // 공컷 최소 총 배팅머니
        public long WinCutWinnerBetMoney; // 공컷 승자 배팅머니
        public long WinCutWinnerMoney; // 공컷 최소 승자머니
        public double WinCutRate; // 공컷 비율
        public bool WinCutTrigger1;   // 공컷트리거 (true = 판정승만)
        public bool WinCutTrigger2;   // 공컷트리거 (true = 저녁 레이스만)
        //public long 


        Random rng = new Random(new System.DateTime().Millisecond);

        public RoomServer(BadugiService f, UnityCommon.Server t, ushort portnum, int channelID) : base(f, t, portnum, channelID)
        {
            ChannelID = channelID;
            RemoteClients = new ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>>();
            RemoteRooms = new ConcurrentDictionary<Guid, CGameRoom>();
            RoomNumbers = new Stack<int>();
            MadePushBase = new Dictionary<long, Dictionary<long, long>>();
            MadePushGame = new Dictionary<long, long>();
            MadePushUser = new Dictionary<long, long>();
            MadePushGame2 = new Dictionary<long, long>();

            ChanKind = GetChannelKind(ChannelID);
            ChanType = GetChannelType(ChannelID);
            ChanFree = GetChannelFree(ChannelID);

            for (int RoomNumber = 200; RoomNumber > 0; --RoomNumber)
            {
                RoomNumbers.Push(RoomNumber);
            }

            DB_Server_GetRoomData();
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
                // 데이터 동기화 요청
                Stub.ServerRequestDataSync = ServerRequestDataSync;

                // 로비 서버
                Stub.LobbyRoomNotifyServermaintenance = LobbyRoomNotifyServermaintenance;
                Stub.LobbyRoomReloadServerData = LobbyRoomReloadServerData;
                Stub.LobbyRoomKickUser = LobbyRoomKickUser;
                Stub.LobbyRoomJackpotInfo = LobbyRoomJackpotInfo;
                Stub.LobbyRoomCalling = LobbyRoomCalling;


                // 릴레이서버 CoreHandle Relay
                Stub.RelayClientJoin = RelayClientJoin;
                Stub.RelayClientLeave = RelayClientLeave;
                Stub.RelayRequestRoomOutRsvn = RelayRequestRoomOutRsvn;
                Stub.RelayRequestRoomOut = RelayRequestRoomOut;
                Stub.RelayRequestRoomMove = RelayRequestRoomMove;
                Stub.LobbyRoomResponseMoveRoom = LobbyRoomResponseMoveRoom;
                Stub.RelayServerMoveFailure = RelayServerMoveFailure;

                // 클라이언트 Response Relay
                Stub.RelayGameRoomIn = RelayGameRoomIn;
                Stub.RelayGameRequestReady = RelayGameRequestReady;
                Stub.RelayGameDealCardsEnd = RelayGameDealCardsEnd;
                Stub.RelayGameActionBet = RelayGameActionBet;
                Stub.RelayGameActionChangeCard = RelayGameActionChangeCard;
            }
        }

        #region Room Server
        public override void ServerTask(object sender, ElapsedEventArgs e_)
        {
#if DEBUG
#else
            try
#endif
            {
                ++this.tick;

                if (this.tick % 5 == 0)
                {
                    // 입장시간 지났는데 멈춰있는 플레이어. 처리후, 응답없으면 강퇴
                    foreach (var realyClient in RemoteClients)
                        foreach (var player in realyClient.Value)
                        {
                            if (player.Value.status == UserStatus.None)
                            {
                                if (player.Value.roomID == Guid.Empty && player.Value.roomTime.AddSeconds(10) < DateTime.Now)
                                {
                                    Log._log.Warn("입장시간 Over. (Disconect) Player:" + player.Value.data.userID);
                                    DBLog(player.Value.data.ID, ChannelID, 0, LOG_TYPE.연결끊김, "입장실패 후 응답없음");
                                    ClientDisconect(player.Value.Remote.Key, player.Value.Remote.Value, "Task roomTime Over");
                                }
                            }
                        }
                }

                if (this.tick % 17 == 0)
                {
                    //DisplayStatus(m_Core);

                    if (this.ShutDown)
                    {
                        int players = 0;
                        foreach (var playercount in RemoteClients)
                        {
                            players += playercount.Value.Count();
                        }
                        int rooms = RemoteRooms.Count;
                        Log._log.Info("세션 종료중. 남은 플레이어 수:" + players + " 남은 방 수:" + RemoteRooms.Count);

                        // 모든 세션 종료
                        if (this.CountDown < DateTime.Now)
                        {
                            foreach (var relay in RemoteClients)
                                foreach (var client in relay.Value)
                                {
                                    ClientDisconect(client.Value.Remote.Key, client.Value.Remote.Value, "ShutDown");
                                    //NetServer.CloseConnection(client.Key);
                                }
                        }
                        // 더 이상 방이 없으면 프로그램 종료
                        if (players == 0 && rooms == 0)
                        {
                            Log._log.Info("서버 종료. ShutDown");
                            //System.Windows.Forms.Application.Exit();
                            System.Environment.Exit(0);
                        }
                    }
                }
            }
#if DEBUG
#else
            catch (Exception e)
            {
                Log._log.Fatal("TaskTimer 에러:" + e.ToString());
            }
#endif
        }
        public void ClientDisconect(RemoteID remoteS, RemoteID remoteC, string reasone)
        {
            Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
            Log._log.WarnFormat("ClientDisconect. remoteS:{0}, remoteC:{1}, reasone:{2}", remoteS, remoteC, reasone);
        }
        public void KickPlayer(CPlayer player, string reasone)
        {
            Proxy.GameRelayKickUser(player.Remote.Key, CPackOption.Basic, player.Remote.Value);

            Log._log.WarnFormat("KickPlayer. player:{0} reasone:{1}", player.data.userID, reasone);
        }
        void CheckRoomCount(CGameRoom room)
        {
            if (room.PlayersConnect.Count == 0)
            {
                CGameRoom temp;
                if (RemoteRooms.TryRemove(room.roomID, out temp))
                {
                    if (temp.isRun)
                    {
                        temp.isRun = false;
                        temp.Thread_sequential_packet_handler.Join();
                    }
                    DB_Room_Delete(room.roomID);
                    RoomNumbers.Push(room.roomNumber);
                }
            }
            else
            {
                DB_Room_Update(room.roomID, room.PlayersConnect);
            }
        }
        bool ProcessGame(RemoteID remote, RemoteID userRemote, CMessage data, PacketType rmiID)
        {
            CPlayer rc;
            CMessage msg = data;
            ConcurrentDictionary<RemoteID, CPlayer> relayClients;
            if (RemoteClients.TryGetValue(remote, out relayClients))
                if (relayClients.TryGetValue(userRemote, out rc))
                {
                    CGameRoom room_join;
                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                    {
#if PACKET
                    Log._log.InfoFormat("RECV Packet. remote:{0}, rmiID:{1}, user:{2}", remote, rmiID, rc.data.userID);
#endif
                        room_join.waiting_packets.Enqueue(new MessagePacket(rc, msg, rmiID));
                    }
                    else
                    {
                        Log._log.WarnFormat("ProcessGame no room. remote:{0}, rmiID:{1}, user:{2}", userRemote, rmiID, rc.data.userID);
                    }
                }

            return true;
        }
        public void DummyPlayerDieLeave(CGameRoom room, CPlayer player)
        {
            PlayerDieOut(room, player);
            DB_User_Dummyout(player.data.ID);
            room.player_room_out(player);
            CheckRoomCount(room);
            Proxy.RoomLobbyOutRoom((RemoteID)room.remote_lobby, CPackOption.Basic, room.roomID, player.data.ID);
            CPlayer temp;
            ConcurrentDictionary<RemoteID, CPlayer> relayClients;
            if (RemoteClients.TryGetValue(player.Remote.Value, out relayClients))
                relayClients.TryRemove(player.Remote.Key, out (temp));
        }
        public void DummyPlayerLeave(CGameRoom room)
        {
            List<CPlayer> DummyPlayer = null;

            foreach (var player in room.PlayersGaming)
            {
                if (player.Value.status == UserStatus.RoomPlayOut)
                {
                    if (DummyPlayer == null)
                    {
                        DummyPlayer = new List<CPlayer>();
                    }
                    DummyPlayer.Add(player.Value);
                }
            }

            if (DummyPlayer != null)
            {
                for (int i = 0; i < DummyPlayer.Count; ++i)
                {
                    DB_User_Dummyout(DummyPlayer[i].data.ID);
                    room.player_room_out(DummyPlayer[i]);
                    Proxy.RoomLobbyOutRoom((RemoteID)room.remote_lobby, CPackOption.Basic, room.roomID, DummyPlayer[i].data.ID);
                    Log._log.InfoFormat("DummyClient {0} Leave.", DummyPlayer[i].data.userID);
                }
                CheckRoomCount(room);
            }
        }
        public EVENT_JACKPOT_TYPE EventCheck(int ChanId, int playerCount)
        {
#if DEBUG
            
#else
            if (playerCount <= 4) return EVENT_JACKPOT_TYPE.NONE; // 4명 이상 이벤트 등장
#endif
            if (ChanId <= 4) return EVENT_JACKPOT_TYPE.NONE;
            lock (EventTermLock)
            {
                if (EventTermX1 > 0 && EventTermCount % EventTermX1 == 0)
                {
                    ++EventTermCount;
                    return EVENT_JACKPOT_TYPE.X1;
                }
                else if (EventTermX2 > 0 && EventTermCount % EventTermX2 == 0)
                {
                    ++EventTermCount;
                    return EVENT_JACKPOT_TYPE.X2;
                }
                else if (EventTermX3 > 0 && EventTermCount % EventTermX3 == 0)
                {
                    ++EventTermCount;
                    return EVENT_JACKPOT_TYPE.X3;
                }
                else if (EventTermZ1 > 0 && EventTermCount % EventTermZ1 == 0)
                {
                    ++EventTermCount;
                    return EVENT_JACKPOT_TYPE.Z1;
                }
            }

            return EVENT_JACKPOT_TYPE.NONE;
        }
        public void EventCountUp()
        {
            lock (EventTermLock)
            {
                ++EventTermCount;
            }
        }
        public bool IsPush(int CID, int Card, int BaseTop, int TwoBaseTop, int GameLevel, int UserLevel, int GameLevel2, GameRound CurrentRound)
        {
            // 기본확률 * 게임레벨 가중치 * 유저레벨 가중치
            if (Card == -1) return false;

            // 기본 확률
            Dictionary<long, long> BaseTable;
            if (MadePushBase.TryGetValue(PushType, out BaseTable) == false) return false;

            long BasePush;
            BaseTable.TryGetValue(Card, out BasePush);
            // 게임 레벨
            long GameLevelPush;
            MadePushGame.TryGetValue(GameLevel, out GameLevelPush);
            //// 유저 레벨
            //long UserLevelPush;
            //MadePushUser.TryGetValue(UserLevel, out UserLevelPush);

            double PushProbability = ((double)(BasePush / 100.0) * (100 + GameLevelPush)) / 100.0;
            //double PushProbability = ((double)(BasePush / 100.0) * (100 + GameLevelPush + UserLevelPush)) / 100.0;
            //double PushProbability = (double)BasePush / 100.0;

            // 베이스일경우 카드 교체를 보정해줌
            if (BaseTop > 0)
            {
                switch (BaseTop) // 3~7
                {
                    case 2: // 베이스3 : 메이드4~7, 메이드8~10, 메이드J~K
                        {
                            // 메이드4~7
                            if (Card >= 4 && Card <= 7)
                            {
                                double chance = 1.5;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            // 메이드8~J
                            else if (Card >= 8 && Card <= 11)
                            {
                                double chance = 100.0;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            // 메이드 Q~K
                            else
                            {
                                PushProbability = PushProbability * 0.5;
                            }
                        }
                        break;
                    case 3: // 베이스4 : 메이드4~7, 메이드8~10, 메이드J~K
                        {
                            // 메이드4~7
                            if (Card >= 4 && Card <= 7)
                            {
                                double chance = 1.5;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            // 메이드8~J
                            else if (Card >= 8 && Card <= 11)
                            {
                                double chance = 100.0;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            else
                            // 메이드 Q~K
                            {
                                PushProbability = PushProbability * 0.5;
                            }
                        }
                        break;
                    case 4: // 베이스5 : 메이드5~7, 메이드8~10, 메이드J~K
                        {
                            // 메이드5~7
                            if (Card >= 5 && Card <= 7)
                            {
                                double chance = 1.0;
                                //double chance = 1.5;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        //chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            // 메이드8~J
                            else if (Card >= 8 && Card <= 11)
                            {
                                double chance = 1.0;
                                //double chance = 1.2;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        //chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            else
                            // 메이드 Q~K
                            {
                                //PushProbability = PushProbability * 0.5;
                            }
                        }
                        break;
                    case 5: // 베이스6 : 메이드6~7, 메이드8~10, 메이드J~K
                        {
                            // 메이드6~7
                            if (Card >= 6 && Card <= 7)
                            {
                                double chance = 1.0;
                                //double chance = 1.5;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        //chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            // 메이드8~10
                            else if (Card >= 8 && Card <= 10)
                            {
                                double chance = 1.0;
                                //double chance = 1.2;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        //chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            else
                            // 메이드 J~K
                            {
                                //PushProbability = PushProbability * 0.7;
                            }
                        }
                        break;
                    case 6: // 베이스7 : 메이드7, 메이드8~10, 메이드J~K
                        {
                            // 메이드7~7
                            if (Card >= 7 && Card <= 7)
                            {
                                double chance = 1.0;
                                //double chance = 1.5;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        //chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            // 메이드8~10
                            else if (Card >= 8 && Card <= 10)
                            {
                                double chance = 1.0;
                                //double chance = 1.2;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        //chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            // 메이드 J~K
                            else
                            {
                                //PushProbability = PushProbability * 0.85;
                            }
                        }
                        break;
                    case 7: // 베이스8 : 메이드8~10, 메이드J~K
                        {
                            // 메이드8~10
                            if (Card >= 8 && Card <= 10)
                            {
                                double chance = 1.0;
                                //double chance = 1.2;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        //chance = chance * 1.1;
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 1.2;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            else
                            // 메이드 J~K
                            {
                                //PushProbability = PushProbability * 0.85;
                            }
                        }
                        break;
                    case 8: // 베이스9 : 메이드9~10, 메이드J~K
                        {
                            // 메이드9~10
                            if (Card >= 9 && Card <= 10)
                            {
                                double chance = 1.0;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 0.8;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            else
                            // 메이드 J~K
                            {
                                double chance = 1.0;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 0.7;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                        }
                        break;
                    case 9: // 베이스10 : 메이드10, 메이드J~K
                        {
                            // 메이드10
                            if (Card >= 10 && Card <= 10)
                            {
                                double chance = 1.0;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 0.8;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                            else
                            // 메이드 J~K
                            {
                                double chance = 1.0;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 0.6;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                        }
                        break;
                    case 10: // 베이스J~K
                    case 11:
                    case 12:
                        {
                            // 메이드 J~K
                            if (Card >= 11 && Card <= 13)
                            {
                                double chance = 1.0;
                                switch (CurrentRound)
                                {
                                    case GameRound.START:
                                        break;
                                    case GameRound.MORNING:
                                        break;
                                    case GameRound.AFTERNOON:
                                        break;
                                    case GameRound.EVENING:
                                        //chance = chance * 0.5;
                                        break;
                                }
                                PushProbability = PushProbability * chance;
                            }
                        }
                        break;
                }
            }
            else if (TwoBaseTop > 0)
            {
                // 투베이스 일경우 확률을 낮춘다.
                PushProbability *= 0.7;
            }

            //long GameLevelPush2;
            //MadePushGame2.TryGetValue(GameLevel2, out GameLevelPush2);
            //double PushProbability2 = (double)(GameLevelPush2 / 100.0);

            PushProbability = Math.Min(1.0, PushProbability);

            // 확률이 밀어주기 가중치를 초과할 경우 카드 교체.
            var rngdouble = rng.NextDouble();
            //Log._log.InfoFormat("{0}\t{1}\t{2}\t{3}\t{4})", Card, rngdouble, PushProbability, BaseTop, TwoBaseTop);
            if (rng.NextDouble() > PushProbability/* && rng.NextDouble() > PushProbability2*/)
            {
                return true; // 교체함
            }
            else
            {
                return false; // 교체안함
            }
        }
        void PlayerDieOut(CGameRoom room_cur, CPlayer rc)
        {
            if (room_cur.GameEnding == true)
            {
                Log._log.WarnFormat("PlayerDieOut GameEnding. room_cur:{0}, CPlayer:{1}", room_cur.roomID, rc.data.userID);
                return;
            }

            int CID = room_cur.ChanId;
            RemoteID remoteC = rc.Remote.Value;

            // 게임머니, Lose 적용
            rc.agent.totalpaideMoney -= rc.agent.paidMoney;
            rc.agent.betMoney = rc.agent.paidMoney;

            room_cur.LeavePlayerBetMoney += rc.agent.betMoney;

            rc.ChangeMoney = rc.agent.totalpaideMoney;

            rc.agent.money_var += rc.ChangeMoney;
            rc.agent.addMoney(rc.agent.earnedMoney);
            ++rc.data.loseCount;

            // 멤버 포인트(적립금) 계산
            double FeeRate;
            //if (ChanKind == ChannelKind.무료2채널)
            //    FeeRate = 0.5;
            //else
                FeeRate = 1;
            rc.GameDealMoney = (long)(rc.agent.paidMoney * (DealerFee / 100.0 * FeeRate));
            rc.JackPotDealMoney = (long)(rc.agent.paidMoney * (JackPotRate / 100.0 * FeeRate));
            room_cur.LeavePlayerDealMoney += rc.GameDealMoney;
            room_cur.LeavePlayerJackpotMoney += rc.JackPotDealMoney;

            if (room_cur.ChanType == ChannelType.Charge)
                rc.data.money_pay += rc.ChangeMoney;
            else
                rc.data.money_free += rc.ChangeMoney;
            rc.GameResult = 2;

            // DB 처리
            CPlayer player = rc;
            bool isDummy = player.status == UserStatus.RoomPlayOut;

            //Task.Run(async () =>
            //{
            //var dbResult = await Task.Run(() =>
            //{
            try
            {
                foreach (var row in db.Room_BadugiResultPlayer(player.data.ID, player.ChangeMoney, player.agent.betMoney, player.GameDealMoney, 2, (int)ChanType, DealerFee * FeeRate, CID, isDummy))
                {
                    player.UserLevel = row.MemberLevel;
                    player.GameLevel = row.GameLevel;
                    player.GameLevel2 = row.GameLevel2;
                    if (ChanType == ChannelType.Free)
                    {
                        player.data.money_free = (long)row.GameMoney;
                        player.agent.setMoney(player.data.money_free);
                    }
                    else
                    {
                        player.data.money_pay = (long)row.PayMoney;
                        player.agent.setMoney(player.data.money_pay);
                    }

                    // 게임 플레이어 로그 저장
                    db.LogGamePlayer.Insert(UserId: player.data.ID, ShopID: player.data.shopId, GameId: GameId, ChannelId: CID, RoomId: room_cur.roomNumber, PlayId: room_cur.gameLog.LogId, Result: player.GameResult, ChangeMoney: player.ChangeMoney, AfterMoney: player.agent.haveMoney, GameDealMoney: player.GameDealMoney, JackPotDealMoney: player.JackPotDealMoney, BetMoney: player.agent.betMoney, BaseMoney: room_cur.BaseMoney);
                }
            }
            catch (Exception e)
            {
                Log._log.ErrorFormat("PlayerDieOut Exception. player:{0}, e:{1}", rc.data.userID, e.ToString());
                //return 0;
            }
            //    return 1;
            //});
            //});
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

                    if (itemToRemove.ServerType == ServerType.Relay)
                    {
                        lock (RoomServerLocker)
                        {
                            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                            if (RemoteClients.TryRemove(itemToRemove.ServerHostID, out relayTemp))
                            {
                                foreach (var rc in relayTemp.Values)
                                {
                                    CGameRoom room_join;
                                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                                    {
                                        lock (room_join.Locker)
                                        {
                                            if (room_join.status == RoomStatus.GamePlay && rc.status == UserStatus.RoomPlay) // 게임중 퇴장
                                            {
                                                rc.agent.setDummyPlayer(rc);
                                                rc.status = UserStatus.RoomPlayOut;
                                                DB_User_AutoPlay(rc.data.ID, room_join.roomNumber);

                                                Log._log.InfoFormat("DummyClient {0} Online3.\n", rc.data.userID);
                                                DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.자동치기, "");
                                            }
                                            else
                                            {
                                                DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.비정상종료, "");
                                            }
                                            room_join.player_room_out(rc);
                                            CheckRoomCount(room_join);
                                            Proxy.RoomLobbyOutRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, rc.data.ID);
                                        }
                                    }
                                    Log._log.WarnFormat("Relay Leave. hostID:{0}, player:{1}", itemToRemove.ServerHostID, rc.data.userID);
                                }
                            }
                        }
                    }
                    else if (itemToRemove.ServerType == ServerType.RelayLobby)
                    {
                        LobbyInfo.remote = RemoteID.Remote_None;
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

#region Master Handler
        //void MasterJoinServerCompleteHandler(ErrorInfo info, ArrByte replyFromServer)
        //{
        //    if (info.errorType == ErrorType.Ok)
        //    {
        //        MasterHostID = info.remote;
        //        Log._log.InfoFormat("MasterJoinServerCompleteHandler Success. MasterHostID:{0}", MasterHostID);
        //    }
        //    else
        //    {
        //        Log._log.ErrorFormat("MasterJoinServerCompleteHandler Failed. info.errorType:{0}", info.errorType);
        //    }
        //}
        //void MasterLeaveServerHandler(ErrorInfo info)
        //{
        //    Log._log.WarnFormat("MasterLeaveServerHandler. remote:{0}, errorType:{1}, detailType:{2}", info.remote, info.errorType, info.detailType);
        //}
        //void MasterP2PMemberJoinHandler(RemoteID memberHostID, RemoteID groupHostID, int memberCount, ArrByte customField)
        //{
        //    Log._log.InfoFormat("MasterP2PMemberJoinHandler Success. memberHostID:{0}, groupHostID:{1}, memberCount:{2}", memberHostID, groupHostID, memberCount);

        //}
        //void MasterP2PMemberLeaveHandler(RemoteID memberHostID, RemoteID groupHostID, int memberCount)
        //{
        //    if (ShutDown) return; // 서버 종료중이면 서버퇴장 대응 없음

        //    lock (GameServerLocker)
        //    {
        //        var itemToRemove = ServerInfoList.Values.SingleOrDefault(r => r.ServerHostID == memberHostID);
        //        if (itemToRemove != null)
        //        {
        //            ServerInfo remove;
        //            ServerInfoList.TryRemove(itemToRemove.ServerHostID, out remove);

        //            if (itemToRemove.ServerType == ServerType.Relay)
        //            {
        //                lock (RoomServerLocker)
        //                {
        //                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
        //                    if (RemoteClients.TryRemove(itemToRemove.ServerHostID, out relayTemp))
        //                    {
        //                        foreach (var rc in relayTemp.Values)
        //                        {
        //                            CGameRoom room_join;
        //                            if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
        //                            {
        //                                lock (room_join.Locker)
        //                                {
        //                                    if (room_join.status == RoomStatus.GamePlay && rc.status == UserStatus.RoomPlay) // 게임중 퇴장
        //                                    {
        //                                        rc.agent.setDummyPlayer(rc);
        //                                        rc.status = UserStatus.RoomPlayOut;
        //                                        DB_User_AutoPlay(rc.data.ID, room_join.roomNumber);

        //                                        Log._log.InfoFormat("DummyClient {0} Online3.\n", rc.data.userID);
        //                                        DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.자동치기, "");
        //                                    }
        //                                    else
        //                                    {
        //                                        DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.비정상종료, "");
        //                                    }
        //                                    room_join.player_room_out(rc);
        //                                    CheckRoomCount(room_join);
        //                                    Proxy.RoomLobbyOutRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, rc.data.ID);
        //                                    //CPlayer temp;
        //                                    //relayTemp.TryRemove(remoteC, out (temp));
        //                                }
        //                            }
        //                            Log._log.WarnFormat("Relay Leave. hostID:{0}, player:{1}", itemToRemove.ServerHostID, rc.data.userID);
        //                        }
        //                    }
        //                }
        //            }
        //            else if (itemToRemove.ServerType == ServerType.RelayLobby)
        //            {
        //                RelayServerInfo info;
        //                if (RemoteRelaysh.TryRemove(itemToRemove.ServerHostID, out info))
        //                {
        //                    RelayServerInfo temp_;
        //                    RemoteRelays.TryRemove(info.RelayID, out temp_);
        //                }
        //            }
        //        }
        //    }

        //    Log._log.InfoFormat("MasterP2PMemberLeaveHandler Success. memberHostID:{0}, groupHostID:{1}, memberCount:{2}", memberHostID, groupHostID, memberCount);
        //}
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
                        if (serverInfo.ServerType == ServerType.RelayLobby)
                        {
                            RelayServerInfo relayInfo;
                            relayInfo.remote = serverInfo.ServerHostID;
                            relayInfo.RelayID = serverInfo.ServerID;
                            relayInfo.Addr = serverInfo.ServerAddrPort;
                            RemoteRelays.TryAdd(relayInfo.RelayID, relayInfo);
                            RemoteRelaysh.TryAdd(relayInfo.remote, relayInfo);
                            LobbyInfo = relayInfo;
                        }
                    }
                }
            }

            return true;
        }
#endregion

#region Server Stub Handler
        bool ServerRequestDataSync(RemoteID remote, CPackOption rmiContext, bool isLobby)
        {
            CMessage msg = new CMessage();
            msg.Write((int)ChannelID);
            msg.Write((int)RemoteRooms.Count);

            foreach (var room in RemoteRooms)
            {
                if (isLobby)
                {
                    DB_Room_InsertAll((int)room.Value.ChanId, room.Value.roomID, room.Value.roomNumber, room.Value.BaseMoney, room.Value.PlayersConnect);
                }

                room.Value.remote_lobby = (int)remote;

                msg.Write(room.Value.roomID);
                msg.Write(ChannelID);
                msg.Write((int)room.Value.ChanType);
                msg.Write(room.Value.ChanFree);
                msg.Write(room.Value.roomNumber);
                msg.Write(room.Value.stake);
                msg.Write(room.Value.BaseMoney);
                msg.Write(room.Value.MinMoney);
                msg.Write(room.Value.MaxMoney);
                msg.Write(room.Value.PlayersConnect.Count);
                msg.Write(room.Value.isRestrict());
                msg.Write(room.Value.remote_svr);
                msg.Write(room.Value.remote_lobby);
                msg.Write(room.Value.Password.Length != 0);
                msg.Write(room.Value.Password);

                foreach (var player in room.Value.PlayersConnect)
                {
                    msg.Write(player.Value.data.ID);
                    msg.Write(player.Value.m_ip);
                    msg.Write(player.Value.data.shopId);
                    msg.Write(player.Value.Remote.Value);
                    msg.Write(player.Value.data.nickName);
                    msg.Write(player.Value.data.money_free);
                    msg.Write(player.Value.data.money_pay);
                }
            }

            Proxy.RoomLobbyResponseDataSync(remote, rmiContext, msg.m_array);

            return true;
        }
        bool LobbyRoomNotifyServermaintenance(RemoteID remote, CPackOption rmiContext, int type, string message, int release)
        {
            if (release == 1)
            {
                ServerMaintenance = false;
                ServerMsg = "";
            }
            else
            {
                ServerMaintenance = true;
                ServerMsg = "※안내※\n서버 점검중입니다.";
            }

            return true;
        }
        bool LobbyRoomReloadServerData(RemoteID remote, CPackOption rmiContext, int loadType)
        {
            // 초기화. 서버 설정값 확인
            Task.Run(() =>
            {
                try
                {
                    switch ((DB_COMMAND)loadType)
                    {
                        case DB_COMMAND.BADUGI_PUSH:
                            {
                                dynamic Data_Push = db.GameBadugiPush.FindAllBy(ChannelId: ChannelID).FirstOrDefault();

                                PushType = Data_Push.PushBaseType;
                            }
                            break;
                        case DB_COMMAND.BADUGI_PUSH_BASE:
                            {
                                dynamic Data_PushBase = db.GameBadugiPushBase.All();

                                foreach (var row in Data_PushBase.ToList())
                                {
                                    Dictionary<long, long> temp;
                                    if (this.MadePushBase.TryGetValue(row.Type, out temp))
                                    {
                                        temp[4] = row.Card4;
                                        temp[5] = row.Card5;
                                        temp[6] = row.Card6;
                                        temp[7] = row.Card7;
                                        temp[8] = row.Card8;
                                        temp[9] = row.Card9;
                                        temp[10] = row.Card10;
                                        temp[11] = row.Card11;
                                        temp[12] = row.Card12;
                                        temp[13] = row.Card13;
                                    }
                                }
                            }
                            break;
                        case DB_COMMAND.BADUGI_PUSH_USER:
                            {
                                dynamic Data_PushUser = db.GameBadugiPushUser.All();

                                foreach (var row in Data_PushUser.ToList())
                                {
                                    this.MadePushUser[row.UserLevel] = row.Push;
                                }
                            }
                            break;
                        case DB_COMMAND.BADUGI_PUSH_GAME:
                            {
                                dynamic Data_PushGame = db.GameBadugiPushGame.All();

                                foreach (var row in Data_PushGame.ToList())
                                {
                                    this.MadePushGame[row.GameLevel] = row.Push;
                                }
                            }
                            break;
                        case DB_COMMAND.BADUGI_PUSH_GAME2:
                            {
                                dynamic Data_PushGame2 = db.GameBadugiPushGame2.All();

                                foreach (var row in Data_PushGame2.ToList())
                                {
                                    this.MadePushGame2[row.GameLevel] = row.Push;
                                }
                            }
                            break;
                        case DB_COMMAND.BADUGI_JJACKPOT:
                            {
                                dynamic Data_JackPot = db.GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();

                                this.EventTermX1 = Data_JackPot.Multiple0;
                                this.EventTermX2 = Data_JackPot.Multiple25;
                                this.EventTermX3 = Data_JackPot.Multiple50;
                                this.EventTermZ1 = Data_JackPot.Multiple100;
                            }
                            break;
                        case DB_COMMAND.BADUGI_MADE_BONUS:
                            {
                                dynamic Data_JackPot = db.GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();

                                this.GolfRewardRatio = Data_JackPot.MadeBonus;
                            }
                            break;
                        case DB_COMMAND.BADUGI_MILEAGE:
                            {
                                dynamic Data_JackPot = db.GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();

                                // 응원 포인트
                                //this.CheerPointLimit = Data_JackPot.MileageMax;
                                //this.CheerPointRate = (double)Data_JackPot.MileageRate / 100.0;
                            }
                            break;
                        case DB_COMMAND.BADUGI_MADE_LIMIT:
                            {
                                dynamic Data_Limit = db.GameBadugiMadeLimit.FindAllBy(ChannelId: ChannelID).FirstOrDefault();

                                this.MadeLimit = Data_Limit.MadeLimit;
                            }
                            break;
                        case DB_COMMAND.BADUGI_BBINGCUT:
                            {
                                dynamic Data_BbingCut = db.GameBadugiBbingCut.FindAllBy(ChannelId: ChannelID).FirstOrDefault();
                                BbingCutEnable = Data_BbingCut.Enable;
                                BbingCutTotalBetMoney = Data_BbingCut.TotalMoneyCut;
                                BbingCutWinnerMoney = Data_BbingCut.WinnerMoneyCut;

                                dynamic Data_WinCut = db.GameBadugiWinCut.FindAllBy(ChannelId: ChannelID).FirstOrDefault();
                                WinCutEnable = Data_WinCut.Enable;
                                WinCutTotalBetMoney = Data_WinCut.TotalMoneyCut;
                                WinCutWinnerMoney = Data_WinCut.WinnerMoneyCut;
                                WinCutWinnerBetMoney = Data_WinCut.WinnerBetMoney;
                                WinCutRate = Data_WinCut.Rate / 100.0;
                                WinCutTrigger1 = Data_WinCut.Trigger1;
                                WinCutTrigger2 = Data_WinCut.Trigger2;
                            }
                            break;

                    }

                    Log._log.InfoFormat("ReloadServerData Success. loadType:{0}", loadType);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("ReloadServerData Failure. loadType:{0}, e:{1}", loadType, e.ToString());
                }

            });

            return true;
        }
        bool LobbyRoomKickUser(RemoteID remote, CPackOption rmiContext, int userID)
        {
            lock (RoomServerLocker)
            {
                foreach (var relay in RemoteClients)
                    foreach (var user in relay.Value)
                    {
                        if (user.Value.data.ID == userID)
                        {
                            // 자동치기 상태이면 버그이므로 현재 접속자에서 삭제
                            if (user.Value.status == UserStatus.RoomPlayOut)
                            {
                                Task.Run(() =>
                                {
                                    try
                                    {
                                        db.GameCurrentUser.DeleteByUserId(UserId: user.Value.data.ID);
                                        db.GameCurrentDummy.DeleteByUserId(UserId: user.Value.data.ID);
                                    }
                                    catch
                                    {
                                    }
                                });
                            }
                            else
                            {
                                CGameRoom room;
                                if (RemoteRooms.TryGetValue(user.Value.roomID, out room))
                                {
                                    DBLog(userID, ChannelID, room.roomNumber, LOG_TYPE.연결끊김, "운영툴 강제퇴장");
                                }
                                else
                                {
                                    DBLog(userID, ChannelID, 0, LOG_TYPE.연결끊김, "운영툴 강제퇴장");
                                }
                                ClientDisconect(user.Value.Remote.Key, user.Value.Remote.Value, "LobbyRoomKickUser");
                            }
                        }
                    }
            }
            return true;
        }
        bool LobbyRoomJackpotInfo(RemoteID remote, CPackOption rmiContext, long jackpot)
        {
            this.JackPotMoney = jackpot;

            return true;
        }
        bool LobbyRoomCalling(RemoteID remote, CPackOption rmiContext, int type, int chanId, Guid roomId, int playerId)
        {
            if (ChannelID != chanId) return true;

            // 방 검색
            CGameRoom room;
            if (RemoteRooms.TryGetValue(roomId, out room))
            {
                switch (type)
                {
                    case 1: // 돌발
                        {
                            room.CallJackPotType = EVENT_JACKPOT_TYPE.Z1;
                            room.CallTarget = playerId;
                        }
                        break;
                    case 2: // 50
                        {
                            room.CallJackPotType = EVENT_JACKPOT_TYPE.X1;
                            room.CallTarget = 0;
                        }
                        break;
                    case 3: // 100
                        {
                            room.CallJackPotType = EVENT_JACKPOT_TYPE.X2;
                            room.CallTarget = 0;
                        }
                        break;
                    case 4: // 200
                        {
                            room.CallJackPotType = EVENT_JACKPOT_TYPE.X3;
                            room.CallTarget = 0;
                        }
                        break;
                }
            }

            return true;
        }

        bool RelayClientJoin(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC, ZNet.NetAddress addr, ZNet.ArrByte moveData)
        {
            // 서버 이동 데이터 불러오기
            Common.MoveParam param;
            CPlayer rc;
            Common.Common.ServerMoveParamRead2(moveData, out param, out rc);

            rc.m_ip = addr.addr;
            rc.Remote = new KeyValuePair<RemoteID, RemoteID>(remoteS, remoteC);
            rc.RelayID = param.RelayID;

            int CID = param.ChannelNumber;
            lock (RoomServerLocker)
            {
                if (ChannelID != CID || param.room_id == Guid.Empty || param.roomJoin == Common.MoveParam.ParamRoom.RoomNull)
                {
                    rc.Reset();
                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                    if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                    {
                        relayTemp.TryAdd(rc.Remote.Value, rc);
                    }
                    else
                    {
                        relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                        relayTemp.TryAdd(rc.Remote.Value, rc);
                        RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                    }
                    Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                    KickPlayer(rc, "RelayClientJoin Failure. CID, room_id, roomJoin");
                    return false;
                }

                // DB 플레이어 정보 불러오기
                try
                {
                    //방생성
                    if (param.roomJoin == Common.MoveParam.ParamRoom.RoomMake)
                    {
                        rc.Reset();
                        rc.Operator = true;
                        if (RoomNumbers.Count == 0)
                        {
                            rc.Reset();
                            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                            {
                                relayTemp.TryAdd(rc.Remote.Value, rc);
                            }
                            else
                            {
                                relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                                relayTemp.TryAdd(rc.Remote.Value, rc);
                                RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                            }
                            Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                            KickPlayer(rc, "RelayClientJoin Failure. RoomNumber Count 0");
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
                            new_room.engine.SetBoss(rc);
                            new_room.SetOperator(rc);
                            rc.roomID = param.room_id;

                            new_room.From(CID, param.room_id, RoomNumbers.Pop(), param.roomStake, (int)this.ServerHostID, param.lobby_remote);
                            RemoteRooms.TryAdd(param.room_id, new_room);

                            new_room.PlayersConnect.TryAdd(rc.Remote, rc);

                            Rmi.Marshaler.RoomInfo new_roominfo = new Rmi.Marshaler.RoomInfo();
                            new_roominfo.roomID = new_room.roomID;
                            new_roominfo.chanID = CID;
                            new_roominfo.chanType = (int)ChanType;
                            new_roominfo.chanFree = ChanFree;
                            new_roominfo.number = new_room.roomNumber;
                            new_roominfo.stakeType = param.roomStake;
                            new_roominfo.baseMoney = GetStakeMoney(ChanKind, param.roomStake);
                            new_roominfo.minMoney = GetMinimumMoney(ChanKind, param.roomStake);
                            //new_roominfo.maxMoney = GetMaximumMoney(ChanKind, param.roomStake);
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

                            Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                            userInfo.nickName = rc.data.nickName;
                            userInfo.FreeMoney = rc.data.money_free;
                            userInfo.PayMoney = rc.data.money_pay;
                            userInfo.chanID = CID;
                            userInfo.roomNumber = new_room.roomNumber;

                            DB_Room_Insert(CID, new_room.roomID, new_room.roomNumber, new_room.engine.baseMoney, rc.data.ID);
                            // 로비서버에게 방생성을 알린다.
                            Proxy.RoomLobbyMakeRoom((RemoteID)new_room.remote_lobby, CPackOption.Basic, new_roominfo, userInfo, rc.data.ID, rc.m_ip, param.roomPassword, rc.data.shopId);
                            // 클라이언트 입장시킴
                            Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, true);
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
                                //if (room_join.PlayersConnect.Count >= room_join.max_users) // 인원수 초과시 관전
                                //{
                                //    ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                                //    if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                                //    {
                                //        relayTemp.TryAdd(rc.Remote.Value, rc);
                                //    }
                                //    else
                                //    {
                                //        relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                                //        relayTemp.TryAdd(rc.Remote.Value, rc);
                                //        RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                                //    }
                                //    Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                                //    KickPlayer(rc, "RelayClientJoin Failure. max_users");
                                //    return false;
                                //}

                                rc.player_index = room_join.pop_players_index();
                                rc.agent = new CPlayerAgent(rc.player_index, room_join);
                                if (ChanType == ChannelType.Charge)
                                    rc.agent.setMoney(rc.data.money_pay);
                                else
                                    rc.agent.setMoney(rc.data.money_free);
                                rc.roomID = room_join.roomID;

                                room_join.PlayersConnect.TryAdd(rc.Remote, rc);
                                room_join.roomWaitTime = DateTime.Now.AddMilliseconds(500);

                                Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                                userInfo.nickName = rc.data.nickName;
                                userInfo.FreeMoney = rc.data.money_free;
                                userInfo.PayMoney = rc.data.money_pay;
                                userInfo.chanID = CID;
                                userInfo.roomNumber = room_join.roomNumber;

                                DB_Room_Update(room_join.roomID, room_join.PlayersConnect, CID, rc.data.ID, room_join.roomNumber);
                                // 로비서버에게 방입장을 알린다.
                                Proxy.RoomLobbyJoinRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, userInfo, rc.data.ID, rc.m_ip, rc.data.shopId);
                                // 클라이언트 입장시킴
                                Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, true);
                            }
                        }
                    }
                    else
                    {
                        Log._log.ErrorFormat("Client Join RoomMake Error. Player:{0}", rc.data.userID);
                        rc.Reset();
                        rc.Operator = false;
                        ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                        if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                        {
                            relayTemp.TryAdd(rc.Remote.Value, rc);
                        }
                        else
                        {
                            relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                            relayTemp.TryAdd(rc.Remote.Value, rc);
                            RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                        }
                        Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                        KickPlayer(rc, "RelayClientJoin Failure. param.roomJoin Unknown");
                        return false;
                    }

                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp_;
                    if (RemoteClients.TryGetValue(remoteS, out relayTemp_))
                    {
                        relayTemp_.TryAdd(rc.Remote.Value, rc);
                    }
                    else
                    {
                        relayTemp_ = new ConcurrentDictionary<RemoteID, CPlayer>();
                        relayTemp_.TryAdd(rc.Remote.Value, rc);
                        RemoteClients.TryAdd(rc.Remote.Key, relayTemp_);
                    }
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("RelayClientJoin. Failure. Player:{0}, e:{1}", rc.data.userID, e.ToString());
                    Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
                }
            }
            return true;
        }
        public bool RelayClientLeave(RemoteID remote, CPackOption rmiContext, RemoteID remoteC, bool bMoveServer)
        {
            lock (RoomServerLocker)
            {
                CPlayer rc;
                ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                if (RemoteClients.TryGetValue(remote, out relayTemp))
                    if (relayTemp.TryGetValue(remoteC, out rc))
                    {
                        CGameRoom room_join;
                        if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                        {
                            lock (room_join.Locker)
                            {
                                // 서버 이동
                                if (bMoveServer == true)
                                {
                                    if (room_join.status == RoomStatus.GamePlay && rc.status == UserStatus.RoomPlay) // 게임중 서버이동
                                    {
                                        if (rc.agent.isDeadPlayer == true)
                                        {
                                            PlayerDieOut(room_join, rc);
                                        }
                                        else
                                        {
                                            rc.agent.setDummyPlayer(rc);
                                            rc.status = UserStatus.RoomPlayOut;
                                            DB_User_AutoPlay(rc.data.ID, room_join.roomNumber);

                                            DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.자동치기, "");
                                            Log._log.InfoFormat("DummyClient {0} Online2.\n", rc.data.userID);
                                            CPlayer dummy;
                                            relayTemp.TryRemove(remoteC, out dummy);
                                            return false;
                                        }
                                    }
                                }
                                else // 강제 종료
                                {
                                    if (room_join.status == RoomStatus.GamePlay && rc.status == UserStatus.RoomPlay) // 게임중 퇴장
                                    {
                                        Proxy.GameRelayResponseRoomOutRsvn(rc.Remote.Key, CPackOption.Basic, rc.Remote.Value, rc.player_index, true);
                                        rc.agent.setDummyPlayer(rc);
                                        rc.status = UserStatus.RoomPlayOut;
                                        DB_User_AutoPlay(rc.data.ID, room_join.roomNumber);

                                        DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.자동치기, "");
                                        Log._log.InfoFormat("DummyClient {0} Online1.\n", rc.data.userID);
                                        CPlayer dummy;
                                        relayTemp.TryRemove(remoteC, out dummy);
                                        return false;
                                    }
                                    else
                                    {
                                        DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.비정상종료, "");
                                    }
                                }
                                room_join.player_room_out(rc);
                                CheckRoomCount(room_join);
                                Proxy.RoomLobbyOutRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, rc.data.ID);
                            }
                        }
                        // 서버 이동
                        if (bMoveServer == true)
                        {

                        }
                        else
                        {
                            DB_User_Logout(rc.data.ID);
                        }
                        CPlayer temp;
                        relayTemp.TryRemove(remoteC, out (temp));
                    }
                    else
                    {
                        Log._log.FatalFormat("RelayClientLeave2. {0},{1}", remoteC, bMoveServer);
                    }
                else
                {
                    Log._log.FatalFormat("RelayClientLeave1. {0},{1}", remoteC, bMoveServer);
                }
            }
            return true;
        }
        bool RelayRequestRoomOutRsvn(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC, bool IsRsvn)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                if (relayTemp.TryGetValue(remoteC, out rc))
                {
                    CGameRoom room_join;
                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                    {
                        lock (room_join.Locker)
                        {
                            if ((room_join.status == RoomStatus.Stay)  // 대기중일때 퇴장
                                || (room_join.status != RoomStatus.Stay && rc.status == UserStatus.None) // 게임중 퇴장 (관전자는 정상 퇴장 처리)
                                || (room_join.status != RoomStatus.Stay && rc.status == UserStatus.RoomPlay && rc.agent.isDeadPlayer == true && room_join.jackPotType != EVENT_JACKPOT_TYPE.Z1)) // 게임중 퇴장 요청 (다이한 경우 정상 퇴장 처리)
                            {
                                foreach (var player in room_join.PlayersConnect)
                                {
                                    Proxy.GameRelayResponseRoomOutRsvn(player.Value.Remote.Key, CPackOption.Basic, player.Value.Remote.Value, rc.player_index, false);
                                }
                                return true;
                            }
                            else
                            {
                                foreach (var player in room_join.PlayersConnect)
                                {
                                    Proxy.GameRelayResponseRoomOutRsvn(player.Value.Remote.Key, CPackOption.Basic, player.Value.Remote.Value, rc.player_index, IsRsvn);
                                }
                                return true;
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

            Log._log.ErrorFormat("RelayRequestRoomOutRsvn Failure. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
            return false;
        }
        bool RelayRequestRoomOut(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                if (relayTemp.TryGetValue(remoteC, out rc))
                {
                    CGameRoom room_join;
                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                    {
                        lock (room_join.Locker)
                        {
                            if (((room_join.status == RoomStatus.Stay)  // 대기중일때 퇴장
                                || (room_join.status != RoomStatus.Stay && rc.status == UserStatus.None) // 게임중 퇴장 (관전자는 정상 퇴장 처리)
                                || (room_join.status != RoomStatus.Stay && rc.status == UserStatus.RoomPlay && rc.agent.isDeadPlayer == true && room_join.jackPotType != EVENT_JACKPOT_TYPE.Z1)) // 게임중 퇴장 요청 (다이한 경우 정상 퇴장 처리)
                                )
                            {
                                RelayServerInfo relayInfo = LobbyInfo;
                                if (RemoteRelays.TryGetValue(rc.RelayID, out relayInfo))
                                {
                                    room_join.roomWaitTime = DateTime.Now.AddMilliseconds(500);
                                    Proxy.GameRelayResponseRoomOut(remoteS, CPackOption.Basic, remoteC, true);

                                    // 이동 파라미터 구성
                                    ArrByte param_buffer;
                                    Common.MoveParam param = new Common.MoveParam();
                                    param.From(Common.MoveParam.ParamMove.MoveToLobby, Common.MoveParam.ParamRoom.RoomNull, Guid.Empty, 0, ChannelID, 0, "", rc.RelayID);
                                    Common.Common.ServerMoveParamWrite(param, rc, out param_buffer);

                                    Proxy.RoomRelayServerMoveStart(remoteS, CPackOption.Basic, remoteC, relayInfo.Addr, param_buffer, param.room_id);
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Proxy.RelayResponseRoomOut(remoteS, CPackOption.Basic, remoteC, true);
                        //Log._log.InfoFormat("RelayRequestRoomOut Success2. player:{0}", rc.data.userID);
                        return true;
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

            //Log._log.ErrorFormat("RelayRequestRoomOut Failure. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
            Proxy.RelayResponseRoomOut(remoteS, CPackOption.Basic, remoteC, false);
            return false;
        }
        bool RelayRequestRoomMove(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                if (relayTemp.TryGetValue(remoteC, out rc))
                {
                    CGameRoom room_join;
                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                    {
                        lock (room_join.Locker)
                        {
                            if (((room_join.status == RoomStatus.Stay)  // 대기중일때 퇴장
                                || (room_join.status != RoomStatus.Stay && rc.status == UserStatus.None) // 게임중 퇴장 (관전자는 정상 퇴장 처리)
                                || (room_join.status != RoomStatus.Stay && rc.status == UserStatus.RoomPlay && rc.agent.isDeadPlayer == true && room_join.jackPotType != EVENT_JACKPOT_TYPE.Z1)) // 게임중 퇴장 요청 (다이한 경우 정상 퇴장 처리)
                                                                                                         )
                            {
                                room_join.roomWaitTime = DateTime.Now.AddMilliseconds(500);
                                Proxy.RoomLobbyRequestMoveRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, remoteS, remoteC, rc.data.ID, rc.agent.haveMoney, rc.data.IPFree, rc.data.ShopFree, rc.data.shopId, rc.data.Restrict);
                                //Log._log.InfoFormat("RelayRequestRoomMove Success. player:{0}", rc.data.userID);
                                return true;
                            }
                            else
                            {
                                //Log._log.WarnFormat("RelayRequestRoomMove Cancel. room_join.status:{0}, player:{1}", room_join.status, rc.data.userID);
                            }
                        }
                    }
                }

            //Log._log.ErrorFormat("RelayRequestRoomMove Failure. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
            Proxy.RelayResponseRoomMove(remoteS, CPackOption.Basic, remoteC, false, "잠시후 다시 시도하세요.");
            return false;
        }
        bool LobbyRoomResponseMoveRoom(RemoteID remoteLobby, CPackOption rmiContext, bool makeRoom, System.Guid roomID, NetAddress addr, int chanID, RemoteID remoteS, RemoteID remoteC, string message)
        {
            bool result = true;
            string errorMessage = message;
            CPlayer rc = null;

            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                relayTemp.TryGetValue(remoteC, out rc);

            if (rc == null)
            {
                Log._log.ErrorFormat("LobbyRoomResponseMoveRoom rc Null. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
                errorMessage = "잠시후 다시 시도하세요.";
                Proxy.RelayResponseRoomMove(remoteS, CPackOption.Basic, remoteC, false, errorMessage);
                return false;
            }

            if (ChannelID != chanID || roomID == Guid.Empty)
            {
                errorMessage = "잠시후 다시 시도하세요.";
                Proxy.RelayResponseRoomMove(remoteS, CPackOption.Basic, remoteC, false, errorMessage);
                return false;
            }

            CGameRoom room_cur;
            if (RemoteRooms.TryGetValue(rc.roomID, out room_cur) == false)
            {
                errorMessage = "잠시후 다시 시도하세요.";
                Proxy.RelayResponseRoomMove(remoteS, CPackOption.Basic, remoteC, false, errorMessage);
                return false;
            }

            // 방 이동하기 전에 퇴장
            lock (room_cur.Locker)
            {
                if (room_cur.status != RoomStatus.Stay && rc.status == UserStatus.RoomPlay && rc.agent.isDeadPlayer == true && room_cur.jackPotType != EVENT_JACKPOT_TYPE.Z1) // 게임중 퇴장 요청 (다이한 경우 정상 퇴장 처리))
                {
                    PlayerDieOut(room_cur, rc);
                    room_cur.player_room_out(rc);
                    CheckRoomCount(room_cur);
                    Proxy.RoomLobbyOutRoom((RemoteID)room_cur.remote_lobby, CPackOption.Basic, room_cur.roomID, rc.data.ID);
                }
                else
                if ((room_cur.status == RoomStatus.Stay) || // 대기중일때 퇴장
                (room_cur.status != RoomStatus.Stay && rc.status == UserStatus.None)) // 게임중 퇴장 (관전자는 정상 퇴장 처리)
                {
                    room_cur.player_room_out(rc);
                    CheckRoomCount(room_cur);
                    Proxy.RoomLobbyOutRoom((RemoteID)room_cur.remote_lobby, CPackOption.Basic, room_cur.roomID, rc.data.ID);
                }
                else
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
                    //CPlayer newrc = new CPlayer();
                    //newrc.data = rc.data;
                    //newrc.m_ip = rc.m_ip;
                    //newrc.Remote = new KeyValuePair<HostID, HostID>(rc.Remote.Key, rc.Remote.Value);
                    //newrc.RelayID = rc.RelayID;

                    //rc = newrc;
                    rc.Reset();
                    rc.Operator = true;
                    if (RoomNumbers.Count == 0)
                    {
                        Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, false);
                        KickPlayer(rc, "LobbyRoomResponseMoveRoom RoomNumbers Count 0"); // relay
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
                        new_room.engine.SetBoss(rc);
                        new_room.SetOperator(rc);
                        rc.roomID = roomID;

                        new_room.From(chanID, roomID, RoomNumbers.Pop(), stake, (int)this.ServerHostID, lobby_remote);
                        RemoteRooms.TryAdd(roomID, new_room);

                        new_room.PlayersConnect.TryAdd(rc.Remote, rc);

                        Rmi.Marshaler.RoomInfo new_roominfo = new Rmi.Marshaler.RoomInfo();
                        new_roominfo.roomID = new_room.roomID;
                        new_roominfo.chanID = chanID;
                        new_roominfo.chanType = (int)ChanType;
                        new_roominfo.chanFree = ChanFree;
                        new_roominfo.number = new_room.roomNumber;
                        new_roominfo.stakeType = stake;
                        new_roominfo.baseMoney = GetStakeMoney(ChanKind, stake);
                        new_roominfo.minMoney = GetMinimumMoney(ChanKind, stake);
                        //new_roominfo.maxMoney = GetMaximumMoney(ChanKind, stake);
                        new_roominfo.userCount = 1;
                        new_roominfo.restrict = false;
                        new_roominfo.remote_svr = new_room.remote_svr;
                        new_roominfo.remote_lobby = new_room.remote_lobby;
                        new_roominfo.needPassword = false;
                        new_roominfo.roomPassword = "";

                        Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                        userInfo.nickName = rc.data.nickName;
                        userInfo.FreeMoney = rc.data.money_free;
                        userInfo.PayMoney = rc.data.money_pay;
                        userInfo.chanID = chanID;
                        userInfo.roomNumber = new_room.roomNumber;

                        DB_Room_Insert(chanID, new_room.roomID, new_room.roomNumber, new_room.engine.baseMoney, rc.data.ID);
                        // 로비서버에게 방생성을 알린다.
                        Proxy.RoomLobbyMakeRoom((RemoteID)new_room.remote_lobby, CPackOption.Basic, new_roominfo, userInfo, rc.data.ID, rc.m_ip, "", rc.data.shopId);
                        // 클라이언트 입장시킴
                        Proxy.RelayResponseRoomMove(remoteS, CPackOption.Basic, remoteC, true, "");
                        Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, true);
                    }
                    return true;
                }
                else
                {
                    //방입장
                    CGameRoom room_join;

                    if (RemoteRooms.TryGetValue(roomID, out room_join))
                    {
                        //CPlayer newrc = new CPlayer();
                        //newrc.data = new UserData(rc.data);
                        //newrc.m_ip = rc.m_ip;
                        //newrc.Remote = new KeyValuePair<HostID, HostID>(rc.Remote.Key, rc.Remote.Value);
                        //newrc.RelayID = rc.RelayID;

                        rc.Reset();
                        rc.Operator = false;
                        lock (room_join.Locker)
                        {
                            //if (room_join.PlayersConnect.Count >= room_join.max_users) // 인원수 초과시 관전
                            //{
                            //    Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, false);
                            //    KickPlayer(rc, "LobbyRoomResponseMoveRoom max_users"); // relay
                            //    return false;
                            //}

                            rc.player_index = room_join.pop_players_index();
                            rc.agent = new CPlayerAgent(rc.player_index, room_join);
                            if (ChanType == ChannelType.Charge)
                                rc.agent.setMoney(rc.data.money_pay);
                            else
                                rc.agent.setMoney(rc.data.money_free);
                            rc.roomID = room_join.roomID;

                            room_join.PlayersConnect.TryAdd(rc.Remote, rc);
                            room_join.roomWaitTime = DateTime.Now.AddMilliseconds(500);

                            Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                            userInfo.nickName = rc.data.nickName;
                            userInfo.FreeMoney = rc.data.money_free;
                            userInfo.PayMoney = rc.data.money_pay;
                            userInfo.chanID = chanID;
                            userInfo.roomNumber = room_join.roomNumber;

                            DB_Room_Update(room_join.roomID, room_join.PlayersConnect, chanID, rc.data.ID, room_join.roomNumber);
                            // 로비서버에게 방입장을 알린다.
                            Proxy.RoomLobbyJoinRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, userInfo, rc.data.ID, rc.m_ip, rc.data.shopId);
                            // 클라이언트 입장시킴
                            Proxy.RelayResponseRoomMove(remoteS, CPackOption.Basic, remoteC, true, "");
                            Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, true);
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
                Proxy.RelayResponseRoomMove(remoteS, CPackOption.Basic, remoteC, result, errorMessage);
            }

            return false;
        }
        bool RelayServerMoveFailure(RemoteID remote, CPackOption rmiContext, RemoteID remoteC)
        {
            Log._log.InfoFormat("RelayServerMoveFailure. remote:{0}, remoteC:{1}", remote, remoteC);
            return true;
        }
        bool RelayGameRoomIn(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameRoomInUser);
        }
        bool RelayGameRequestReady(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameRequestReady);
        }
        bool RelayGameDealCardsEnd(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameDealCardsEnd);
        }
        bool RelayGameActionBet(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameActionBet);
        }
        bool RelayGameActionChangeCard(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameActionChangeCard);
        }
#endregion

#region DB
        void DB_User_CurrentUpdate(int CID, int userId, int RoomId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.UpdateByUserId(UserId: userId, RoomId: RoomId, ChannelId: CID);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_User_Dummyout(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentDummy.DeleteByUserId(UserId: userId);

                    //Log._log.InfoFormat("DB_User_Dummyout Success. userId:{0}", userId);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_User_Dummyout Failure. userId:{0}, e:{1}", userId, e.ToString());
                }
            });
        }
        void DB_User_AutoPlay(int userId, int roomId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.DeleteByUserId(UserId: userId);
                    db.GameCurrentDummy.Insert(UserId: userId, Locate: 0, GameId: GameId, ChannelId: ChannelID, RoomId: roomId);

                    //Log._log.InfoFormat("{0} Success.", MethodBase.GetCurrentMethod().Name);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Room_Insert(int CID, Guid roomID, int roomNumber, int baseMoney, int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.UpdateByUserId(UserId: userId, RoomId: roomNumber, ChannelId: CID);
                    db.GameRoomList.Insert(Id: roomID, GameId: GameId, ChannelId: CID, RoomNumber: roomNumber, BetMoney: baseMoney, UserId1: userId, UserValue1: 0, UserDate1: DateTime.Now);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Room_InsertAll(int CID, Guid roomID, int roomNumber, int baseMoney, ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer> players)
        {
            int userId1 = 0;
            long userValue1 = 0;
            DateTime? userDate1 = null;
            int userId2 = 0;
            long userValue2 = 0;
            DateTime? userDate2 = null;
            int userId3 = 0;
            long userValue3 = 0;
            DateTime? userDate3 = null;
            int userId4 = 0;
            long userValue4 = 0;
            DateTime? userDate4 = null;
            int userId5 = 0;
            long userValue5 = 0;
            DateTime? userDate5 = null;

            foreach (var player in players)
            {
                switch (player.Value.player_index)
                {
                    case 0:
                        userId1 = player.Value.data.ID;
                        userValue1 = player.Value.agent.money_var;
                        userDate1 = player.Value.roomTime;
                        break;
                    case 1:
                        userId2 = player.Value.data.ID;
                        userValue2 = player.Value.agent.money_var;
                        userDate2 = player.Value.roomTime;
                        break;
                    case 2:
                        userId3 = player.Value.data.ID;
                        userValue3 = player.Value.agent.money_var;
                        userDate3 = player.Value.roomTime;
                        break;
                    case 3:
                        userId4 = player.Value.data.ID;
                        userValue4 = player.Value.agent.money_var;
                        userDate4 = player.Value.roomTime;
                        break;
                    case 4:
                        userId5 = player.Value.data.ID;
                        userValue5 = player.Value.agent.money_var;
                        userDate5 = player.Value.roomTime;
                        break;
                }
            }
            Task.Run(() =>
            {
                try
                {
                    foreach (var player in players)
                    {
                        db.GameCurrentUser.Insert(UserId: player.Value.data.ID, Locate: 0, GameId: GameId, ChannelId: CID, RoomId: roomNumber, IP: player.Value.m_ip, AutoPlay: player.Value.status == UserStatus.RoomPlayOut);
                    }

                    db.GameRoomList.Insert(Id: roomID, GameId: GameId, ChannelId: CID, RoomNumber: roomNumber, BetMoney: baseMoney, UserId1: userId1, UserValue1: userValue1, userDate1: userDate1, UserId2: userId2, UserValue2: userValue2, userDate2: userDate2, UserId3: userId3, UserValue3: userValue3, userDate3: userDate3, UserId4: userId4, UserValue4: userValue4, userDate4: userDate4, UserId5: userId5, UserValue5: userValue5, userDate5: userDate5);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        public void DB_Room_Update(Guid roomID, ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer> players, int CID = 0, int userId = 0, int RoomId = 0)
        {
            Task.Run(() =>
            {
                int userId1 = 0;
                long userValue1 = 0;
                DateTime? userDate1 = null;
                int userId2 = 0;
                long userValue2 = 0;
                DateTime? userDate2 = null;
                int userId3 = 0;
                long userValue3 = 0;
                DateTime? userDate3 = null;
                int userId4 = 0;
                long userValue4 = 0;
                DateTime? userDate4 = null;
                int userId5 = 0;
                long userValue5 = 0;
                DateTime? userDate5 = null;

                foreach (var player in players)
                {
                    switch (player.Value.player_index)
                    {
                        case 0:
                            userId1 = player.Value.data.ID;
                            userValue1 = player.Value.agent.money_var;
                            userDate1 = player.Value.roomTime;
                            break;
                        case 1:
                            userId2 = player.Value.data.ID;
                            userValue2 = player.Value.agent.money_var;
                            userDate2 = player.Value.roomTime;
                            break;
                        case 2:
                            userId3 = player.Value.data.ID;
                            userValue3 = player.Value.agent.money_var;
                            userDate3 = player.Value.roomTime;
                            break;
                        case 3:
                            userId4 = player.Value.data.ID;
                            userValue4 = player.Value.agent.money_var;
                            userDate4 = player.Value.roomTime;
                            break;
                        case 4:
                            userId5 = player.Value.data.ID;
                            userValue5 = player.Value.agent.money_var;
                            userDate5 = player.Value.roomTime;
                            break;
                    }
                }

                try
                {
                    if (CID != 0)
                    {
                        db.GameCurrentUser.UpdateByUserId(UserId: userId, RoomId: RoomId, ChannelId: CID);
                    }
                    db.GameRoomList.UpdateById(Id: roomID, UserId1: userId1, UserValue1: userValue1, userDate1: userDate1, UserId2: userId2, UserValue2: userValue2, userDate2: userDate2, UserId3: userId3, UserValue3: userValue3, userDate3: userDate3, UserId4: userId4, UserValue4: userValue4, userDate4: userDate4, UserId5: userId5, UserValue5: userValue5, userDate5: userDate5);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Room_Delete(Guid roomID)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameRoomList.DeleteById(Id: roomID);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Server_GetRoomData()
        {
            int CID = this.ChannelID;

            // 서버 설정값 확인
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_DealFee = db.GameDealFee.FindAllBy(GameId: this.GameId, ChannelId: CID).FirstOrDefault();

                    DealerFee = (double)Data_DealFee.DelerFee;
                    JackPotRate = (double)Data_DealFee.JackPotRate;

                    dynamic Data_JackPot = db.GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: CID).FirstOrDefault();
                    EventTermX1 = Data_JackPot.Multiple0;
                    EventTermX2 = Data_JackPot.Multiple25;
                    EventTermX3 = Data_JackPot.Multiple50;
                    EventTermZ1 = Data_JackPot.Multiple100;

                    GolfRewardRatio = Data_JackPot.MadeBonus;

                    //CheerPointLimit = Data_JackPot.MileageMax;
                    //CheerPointRate = (double)Data_JackPot.MileageRate / 100.0;

                    dynamic Data_Limit = db.GameBadugiMadeLimit.FindAllBy(ChannelId: CID).FirstOrDefault();
                    MadeLimit = Data_Limit.MadeLimit;

                    dynamic Data_PushBase = db.GameBadugiPushBase.All();
                    foreach (var row in Data_PushBase.ToList())
                    {
                        Dictionary<long, long> Cards = new Dictionary<long, long>();
                        {
                            Cards.Add(4, row.Card4);
                            Cards.Add(5, row.Card5);
                            Cards.Add(6, row.Card6);
                            Cards.Add(7, row.Card7);
                            Cards.Add(8, row.Card8);
                            Cards.Add(9, row.Card9);
                            Cards.Add(10, row.Card10);
                            Cards.Add(11, row.Card11);
                            Cards.Add(12, row.Card12);
                            Cards.Add(13, row.Card13);
                        }
                        this.MadePushBase.Add(row.Type, Cards);
                    }

                    dynamic Data_PushGame = db.GameBadugiPushGame.All();
                    foreach (var row in Data_PushGame.ToList())
                    {
                        MadePushGame.Add(row.GameLevel, row.Push);
                    }

                    dynamic Data_PushGame2 = db.GameBadugiPushGame2.All();
                    foreach (var row in Data_PushGame2.ToList())
                    {
                        MadePushGame2.Add(row.GameLevel, row.Push);
                    }

                    dynamic Data_PushUser = db.GameBadugiPushUser.All();
                    foreach (var row in Data_PushUser.ToList())
                    {
                        MadePushUser.Add(row.UserLevel, row.Push);
                    }

                    dynamic Data_BbingCut = db.GameBadugiBbingCut.FindAllBy(ChannelId: CID).FirstOrDefault();
                    BbingCutEnable = Data_BbingCut.Enable;
                    BbingCutTotalBetMoney = Data_BbingCut.TotalMoneyCut;
                    BbingCutWinnerMoney = Data_BbingCut.WinnerMoneyCut;

                    dynamic Data_WinCut = db.GameBadugiWinCut.FindAllBy(ChannelId: ChannelID).FirstOrDefault();
                    WinCutEnable = Data_WinCut.Enable;
                    WinCutTotalBetMoney = Data_WinCut.TotalMoneyCut;
                    WinCutWinnerMoney = Data_WinCut.WinnerMoneyCut;
                    WinCutWinnerBetMoney = Data_WinCut.WinnerBetMoney;
                    WinCutRate = Data_WinCut.Rate / 100.0;
                    WinCutTrigger1 = Data_WinCut.Trigger1;
                    WinCutTrigger2 = Data_WinCut.Trigger2;

                    dynamic Data_Push = db.GameBadugiPush.FindAllBy(ChannelId: CID).FirstOrDefault();
                    PushType = Data_Push.PushBaseType;

                    Log._log.InfoFormat("{0} Success. MadeLimit:{1}", MethodBase.GetCurrentMethod().Name, MadeLimit);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
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
                    //Log._log.InfoFormat("DB_User_Logout Success. userId:{0}", userId);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_User_Logout Failure. userId:{0}, e:{1}", userId, e.ToString());
                }
            });
        }
#endregion
    }
}
