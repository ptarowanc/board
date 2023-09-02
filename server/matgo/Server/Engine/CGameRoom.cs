#define EVENT_JACKPOT

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Network;
using Guid = System.Guid;
using ZNet;

namespace Server.Engine
{
    public class CGameRoom
    {
        #region Value
        public object Locker = new object();
        public ConcurrentQueue<MessagePacket> waiting_packets;
        public Thread Thread_sequential_packet_handler;
        public bool isRun = true;
        public RoomServer RoomSvr;
        public int ChanId = -1;
        public ChannelKind ChanKind = ChannelKind.None;
        public ChannelType ChanType = ChannelType.None;
        public bool ChanFree = false;

        public const int max_users = 2;
        ConcurrentQueue<byte> players_index;
        ConcurrentDictionary<string, PacketType> received_protocol;
        public ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer> PlayersConnect;
        public ConcurrentDictionary<int, CPlayer> PlayersGaming; // 게임중 플레이어
        //public ConcurrentDictionary<RemoteID, CPlayer> PlayersConnect; // 접속중 플레이어
        public CPlayer[] PlayersObserve; // 관전중 플레이어

        public int remote_svr;      // 서버구분용 remoteID번호 (입장할때 방이 어떤 서버에 존재하는지 정보)
        public int remote_lobby;    // 서버구분용 remoteID번호 (원래입장시점의 로비서버 - 방에서 나갈때 사용)
        public CGostopEngine engine;
        internal SS.Proxy Proxy;

        // 방 변수
        public Guid roomID;
        public int roomNumber = 0;
        public RoomStatus status;
        public int stake;
        public string Password = "";
        public int BaseMoney;
        public long MinMoney;
        public long MaxMoney;
        public DateTime roomWaitTime; // 대기중인 시간

        // 게임 변수
        MISSION currentMission;
        Random rand = new Random((int)DateTime.UtcNow.Ticks);
        bool isJackPot { get; set; }    // 이벤트 판 여부
        byte drow_count { get; set; }   // 무승부(나가리) 횟수

        CMissionManager missionsManager;
        CPlayerOrderManager order_manager;

        //long BetingLimite; // 배팅 한도
        #endregion

        public CGameRoom(RoomServer roomServer)
        {
            this.RoomSvr = roomServer;
            this.Proxy = RoomSvr.Proxy;
            this.engine = new CGostopEngine();
            this.waiting_packets = new ConcurrentQueue<MessagePacket>();
            Thread_sequential_packet_handler = new Thread(new ThreadStart(sequential_packet_handler));
            this.PlayersGaming = new ConcurrentDictionary<int, CPlayer>();
            this.PlayersConnect = new ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer>();
            this.PlayersObserve = new CPlayer[1];
            this.received_protocol = new ConcurrentDictionary<string, PacketType>();
            this.order_manager = new CPlayerOrderManager();
            this.missionsManager = new CMissionManager();

            this.status = RoomStatus.Stay;
            this.players_index = new ConcurrentQueue<byte>();
            this.isJackPot = false;
            this.drow_count = 0;

            for (int i = 0; i < CGameRoom.max_users; ++i)
            {
                this.players_index.Enqueue((byte)i);
            }

            Thread_sequential_packet_handler.Start();
            this.roomWaitTime = DateTime.Now.AddSeconds(5);

            // 한 게임당 배팅한도 제한
            /*
            if (roomServer.ChannelType == ChannelType.Charge)
            {
                BetingLimite = 50000000000;
            }
            else if (roomServer.ChannelType == ChannelType.Freedom)
            {
                BetingLimite = 50000;
            }
            else if (roomServer.ChannelType == ChannelType.Free)
            {
                BetingLimite = 50000;
            }
            else
            {
                BetingLimite = 50000;
            }
            */
        }

        #region Room Common
        public byte pop_players_index()
        {
            if (this.players_index.Count == 0) return byte.MaxValue;

            byte temp;
            if (this.players_index.TryDequeue(out temp))
                return temp;
            else
            {
                Log._log.Error("pop_players_index Dequeue failed");

                int i = 0;
                foreach (var player in PlayersConnect)
                {
                    if (player.Value.player_index != i)
                    {
                        temp = (byte)i;
                        break;
                    }
                    ++i;
                }

                return temp;
            }
        }
        public bool push_players_index(byte pi)
        {
            if (this.players_index.Contains(pi)) return false;

            this.players_index.Enqueue(pi);

            List<byte> temp = new List<byte>();

            int count = this.players_index.Count;
            for (int i = 0; i < count; i++)
            {
                byte p;
                if (this.players_index.TryDequeue(out p))
                    temp.Add(p);
                else
                {
                    Log._log.Error("push_players_index TryDequeue Failed");
                }
            }

            temp.Sort();

            foreach (byte b in temp)
                this.players_index.Enqueue(b);

            return true;
        }
        public void From(int CID, Guid id, int _num, int _stake, int _remote_svr, int _remote_lobby)
        {
            this.roomID = id;
            this.roomNumber = _num;
            this.remote_svr = _remote_svr;
            this.remote_lobby = _remote_lobby;
            this.stake = _stake;
            this.ChanId = CID;
            this.ChanKind = GameServer.GetChannelKind(CID);
            this.ChanType = GameServer.GetChannelType(CID);
            this.ChanFree = GameServer.GetChannelFree(CID);
            this.engine.baseMoney = this.BaseMoney = GameServer.GetStakeMoney(this.ChanKind, this.stake);
            this.MinMoney = GameServer.GetMinimumMoney(this.ChanKind, this.stake);
            this.MaxMoney = GameServer.GetMaximumMoney(this.ChanKind, this.stake);
        }
        private void ErrorLog(Exception e)
        {
            //var st = new System.Diagnostics.StackTrace(e, true);
            //var frames = st.GetFrames();
            //var traceString = new System.Text.StringBuilder();

            //foreach (var frame in frames)
            //{
            //    if (frame.GetFileLineNumber() < 1)
            //        continue;

            //    traceString.Append("File: " + frame.GetFileName());
            //    traceString.Append(", Method:" + frame.GetMethod().Name);
            //    traceString.Append(", LineNumber: " + frame.GetFileLineNumber());
            //    traceString.Append("  -->  ");
            //}
            string fileName = "log_" + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString() + ".txt";
            System.IO.File.WriteAllText(fileName, e.ToString());
        }
        public bool ActionExecute(CPlayer player, MessageTemp msg)
        {
            if (!player.agent.packet_handler.ContainsKey(msg.mRmiID))
            {
                // 리턴되지 않으려면 IsPlayablePacket에 패킷ID 추가.
                if (msg.mRmiID != SS.Common.GameNotifyStat && msg.mRmiID != SS.Common.GameUserInfo)
                {
#if debug
                    Log._log.WarnFormat("ActionExecute msg.mRmiID : {0}", msg.mRmiID);
#endif
                }
                return false;
            }
            else
            {
                player.agent.packet_handler[msg.mRmiID](msg.Msg);
            }

            return true;
        }
        #endregion

        #region Task
        async void sequential_packet_handler()
        {
            await Task.Yield();

            while (isRun)
            {
                await Task.Delay(1);

                // 패킷
                if (this.waiting_packets.Count > 0)
                {
                    MessagePacket msg;
                    if (this.waiting_packets.TryDequeue(out msg))
                    {
                        ProcessMsg(msg.player, msg.Msg, msg.mRmiID);
                    }
                }

                // 타이머
                RoomTask();
            }

            while (this.waiting_packets.Count > 0)
            {
                await Task.Delay(1);

                MessagePacket msg;
                if (this.waiting_packets.TryDequeue(out msg))
                {
                    ProcessMsg(msg.player, msg.Msg, msg.mRmiID);
                }
                // 타이머
                RoomTask();
            }
        }
        DateTime TaskTick = new DateTime();
        int tick = 0;
        void RoomTask()
        {
#if DEBUG
#else
            try
#endif
            {
                if (TaskTick < DateTime.Now)
                {
                    //lock (Locker)
                    {
                        TaskTick = DateTime.Now.AddSeconds(1);

                        ++this.tick;

                        //if (this.tick % 1 == 0)
                        {
                            ProcessAutoPlay();
                        }

                        if (this.tick % 3 == 0)
                        {
                            if (status == RoomStatus.Stay && PlayersConnect.Count == 2 && roomWaitTime < DateTime.Now)
                            {
                                if (RoomSvr.ServerMaintenance == false)
                                {
                                    bool start = true;
                                    // 돈없는 플레이어가 있으면 강퇴 후 준비
                                    foreach (var player in PlayersConnect)
                                    {
                                        if (ChanType != ChannelType.Charge)
                                        {
                                            if (player.Value.data.money_free < BaseMoney * 7)
                                            {
                                                start = false;
                                                if (player.Value.KickCount)
                                                {
                                                    Log._log.Warn("No Money Out. Player:" + player.Value.data.userID);
                                                    RoomSvr.DBLog(player.Value.data.ID, RoomSvr.ChannelID, roomNumber, LOG_TYPE.연결끊김, "게임전 판돈부족 강퇴");
                                                    RoomSvr.ClientDisconect(player.Value.Remote.Key, player.Value.Remote.Value, "Task No Money");
                                                }
                                                else
                                                {
                                                    KickPlayer(player.Value);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (player.Value.data.money_pay < BaseMoney * 7)
                                            {
                                                start = false;
                                                if (player.Value.KickCount)
                                                {
                                                    Log._log.Warn("No Money Out. Player:" + player.Value.data.userID);
                                                    RoomSvr.DBLog(player.Value.data.ID, RoomSvr.ChannelID, roomNumber, LOG_TYPE.연결끊김, "게임전 판돈부족 강퇴");
                                                    RoomSvr.ClientDisconect(player.Value.Remote.Key, player.Value.Remote.Value, "Task No Money");
                                                }
                                                else
                                                {
                                                    KickPlayer(player.Value);
                                                }
                                            }
                                        }
                                    }
                                    if (start == true)
                                    {
                                        AutoGameStart();
                                    }
                                }
                            }

                            // 방에서 응답없는 플레이어 종료
                            if (status != RoomStatus.Stay) // 게임중 or 연습게임중
                            {
                                var result = Check_Player_Active();
                                if (result != null)
                                {
#if DEBUG
                                    Log._log.Warn("actionTimeLimit Out. Player:" + result.data.userID);
                                    //Send(result, result.currentMsg.Msg, result.currentMsg.mRmiID);
                                    //RoomSvr.RelayClientLeave(result.RelayRemote, RmiContext.UnreliableSend, result.remote, false);
                                    //RoomSvr.ClientDisconect(result.RelayRemote, result.remote, "Task actionTimeLimit Over");
#else
                                    //room.SaveCardLog();
                                    Log._log.Warn("actionTimeLimit Out. Player:" + result.data.userID);
                                    RoomSvr.DBLog(result.data.ID, RoomSvr.ChannelID, roomNumber, LOG_TYPE.연결끊김, "게임중 응답없음");
                                    RoomSvr.RelayClientLeave(result.Remote.Key, CPackOption.Basic, result.Remote.Value, false);
                                    RoomSvr.ClientDisconect(result.Remote.Key, result.Remote.Value, "Task actionTimeLimit Over");
#endif
                                }
                            }
                        }
                    }
                }
            }
#if DEBUG
#else
            catch (Exception e)
            {
                Log._log.Fatal("RoomTask 에러:" + e.ToString());
            }
#endif
        }
        #endregion

        #region Packet Common
        public bool IsPlayablePacket(CPlayer player, PacketType pkID)
        {
            bool isPlayable = false;
            // CAIPlayer packet_handler에서 처리하고 싶은 패킷ID를 추가하면 됨
            switch (pkID)
            {
                case SS.Common.GameStart:
                    isPlayable = true;
                    break;
                case SS.Common.GameDistributedStart:
                    isPlayable = true;
                    break;
                case SS.Common.GameTurnStart:
                    if (engine.current_player_index == player.player_index) isPlayable = true;
                    break;
                case SS.Common.GameSelectCardResult:
                    if (engine.current_player_index == player.player_index &&
                        (engine.expected_result_type == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)) isPlayable = true;
                    break;
                case SS.Common.GameFlipDeckResult:
                    if (engine.current_player_index == player.player_index &&
                        (engine.expected_result_type == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)) isPlayable = true;
                    break;
                case SS.Common.GameRequestGoStop:
                    if (engine.current_player_index == player.player_index) isPlayable = true;
                    break;
                case SS.Common.GameRequestKookjin:
                    if (engine.current_player_index == player.player_index) isPlayable = true;
                    break;
            }

            return isPlayable;
        }
        public bool CommitPacket(CPlayer player, PacketType rmiID, PLAYER_SELECT_CARD_RESULT expected_result_type)
        {
            if (player.agent.currentMsg == null) return false;

            switch (player.agent.currentMsg.mRmiID)
            {
                case SS.Common.GameStart:
                    {
                        if (rmiID == SS.Common.GameSelectOrder)
                            return true;
                    }
                    break;
                case SS.Common.GameDistributedStart:
                    {
                        if (rmiID == SS.Common.GameDistributedEnd)
                            return true;
                    }
                    break;
                case SS.Common.GameTurnStart:
                    {
                        if ((rmiID == SS.Common.GameActionPutCard || rmiID == SS.Common.GameActionFlipBomb))
                            return true;
                    }
                    break;
                case SS.Common.GameSelectCardResult:
                    {
                        if (rmiID == SS.Common.GameActionChooseCard &&
                            (expected_result_type == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER))
                            return true;
                    }
                    break;
                case SS.Common.GameFlipDeckResult:
                    {
                        if ((rmiID == SS.Common.GameActionChooseCard)
                            && (expected_result_type == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK))
                            return true;
                    }
                    break;
                case SS.Common.GameRequestGoStop:
                    {
                        if (rmiID == SS.Common.GameSelectGoStop)
                            return true;
                    }
                    break;
                case SS.Common.GameRequestKookjin:
                    {
                        if (rmiID == SS.Common.GameSelectKookjin)
                            return true;
                    }
                    break;
                default:
                    Log._log.ErrorFormat("CommitPacket Invaild PacketType. {0}", player.agent.currentMsg.mRmiID);
                    break;
            }
            return false;
        }
        public void PacketProcess(CPlayer player)
        {
            MessageTemp temp;
            if (player.agent.QueueMsg.TryDequeue(out temp))
            {
                ProcessMsg(player, temp.Msg, temp.mRmiID);
            }
            else
            {
                // 처리되지 않은 행동이 있으면 수행
                if (player.agent.currentMsg != null/* && player.agent.isActionExecute == false*/)
                {
                    ActionExecute(player, player.agent.currentMsg);
                    player.agent.LastMsg = player.agent.currentMsg;
                    player.agent.currentMsg = null;
                }
            }
        }
        bool is_received(string player_guid, PacketType rmiID)
        {
            if (!this.received_protocol.ContainsKey(player_guid))
            {
                return false;
            }

            return this.received_protocol[player_guid] == rmiID;
        }
        void checked_protocol(string player_guid, PacketType rmiID)
        {
            if (this.received_protocol.ContainsKey(player_guid))
            {
                //err
                return;
            }

            this.received_protocol.TryAdd(player_guid, rmiID);
        }
        bool all_received(PacketType rmiID)
        {
            if (this.received_protocol.Count < CGameRoom.max_users)
            {
                return false;
            }

            foreach (KeyValuePair<string, PacketType> kvp in this.received_protocol)
            {
                if (kvp.Value != rmiID)
                {
                    return false;
                }
            }
            clear_received_protocol();
            return true;
        }
        void clear_received_protocol()
        {
            this.received_protocol.Clear();
        }
        public bool CheckGameRun()
        {
            // 게임중이 아닌데 게임진행 패킷을 받은 경우
            if (this.status == RoomStatus.Stay) return false;
            //if (players.Count != max_users) return false;

            return true;
        }
        #endregion

        #region Room Game
        void reset(byte firstTurnIndex = 0)
        {
            this.engine.reset();
            this.order_manager.reset(this.engine, firstTurnIndex);
            this.missionsManager.reset();

            foreach (var player in PlayersGaming)
            {
                if (player.Value.isPracticeDummy == true)
                    player.Value.status = UserStatus.RoomPlayAuto;
                else
                    player.Value.status = UserStatus.RoomPlay;
                player.Value.GameResult = 0;
                player.Value.GameDealMoney = 0;
                player.Value.JackPotDealMoney = 0;

                player.Value.agent.reset();
                player.Value.agent.status = UserGameStatus.Play;
                player.Value.agent.actionTimeLimit = DateTime.Now.Ticks + 170000000;
                //player.Value.agent.isActionExecute = true;
                player.Value.agent.currentMsg = null;
                //player.Value.agent.LastMsg = null;
                player.Value.agent.QueueMsg = null;

                this.engine.addAgents(player.Value.agent);
            }

            gameLog.Reset(this.engine.baseMoney);
        }
        public bool isRestrict()
        {
            bool restrict = false;

            if (PlayersConnect.Count == max_users)
            {
                restrict = true;

            }

            return restrict;
        }
        public void player_room_out(CPlayer Player)
        {
            if (push_players_index(Player.player_index) == false) return;

            for (int i = 0; i < PlayersObserve.Count(); ++i)
            {
                if (PlayersObserve[i] == null) continue;

                if (PlayersObserve[i] == Player)
                {
                    PlayersObserve[i] = null;
                }
            }

            roomWaitTime = DateTime.Now.AddSeconds(3);

            CPlayer temp;
            PlayersConnect.TryRemove(Player.Remote, out temp);
            Player.PacketReady = false;

            if (PlayersConnect.Count > 0)
            {
                foreach (var player in PlayersConnect)
                {
                    if (player.Value.status == UserStatus.RoomReady)
                        player.Value.status = UserStatus.RoomStay;

                    player.Value.Operator = true;
                }
                Player.Operator = false;
            }

            send_room_out(Player);
        }
        public void player_room_out_gameinit(CPlayer Player)
        {
            if (push_players_index(Player.player_index) == false) return;

            status = RoomStatus.Stay;

            CPlayer temp;
            PlayersConnect.TryRemove(Player.Remote, out temp);

            foreach (var player in PlayersConnect)
            {
                if (player.Value.status == UserStatus.RoomReady)
                    player.Value.status = UserStatus.RoomStay;

                player.Value.Operator = true;
            }
            Player.Operator = false;

            send_room_out(Player, true);
        }
        public void KickPlayer(CPlayer player)
        {
            send_player_all_in(player);
            player.KickCount = true;
        }
        #endregion

        // 게임 데이터 저장, 로그
        CGameLog gameLog = new CGameLog(max_users);

        public void ProcessMsg(CPlayer owner, CMessage msg, PacketType rmiID)
        {
            lock (Locker)
            {
                if (is_received(owner.data.userID, rmiID))
                {
                    Log._log.WarnFormat("ProcessMsg is_received. user:{0}. rmiID:{1}", owner.data.userID, rmiID);
                    return;
                }
                checked_protocol(owner.data.userID, rmiID);
                bool packetAllowed = false;
                if (owner.status != UserStatus.RoomPlayAuto)
                {
                    packetAllowed = CommitPacket(owner, rmiID, engine.expected_result_type);
                    if (packetAllowed == true)
                    {
                        //owner.agent.isActionExecute = true;
                        owner.agent.currentMsg = null;
                    }
                }

                switch (rmiID)
                {
                    case SS.Common.GameRoomInUser:
                        {
                            CS_ROOM_IN(owner, msg);
                        }
                        break;
                    case SS.Common.GameReady:
                        {
                            if (CheckGameRun())
                            {
                                if (this.status == RoomStatus.PracticeGamePlay)
                                {

                                }
                                else
                                {
                                    Log._log.Warn(string.Format("ProcessMsg Break. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                                    break;
                                }
                            }

                            CS_READY_TO_START(owner, msg);
                        }
                        break;
                    case SS.Common.GameSelectOrder:
                        {
                            if (!CheckGameRun())
                            {
                                Log._log.Warn(string.Format("ProcessMsg Break. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                                break;
                            }
                            CS_PLAYER_ORDER_START(owner, msg);
                        }
                        break;
                    case SS.Common.GameDistributedEnd:
                        {
                            if (!CheckGameRun())
                            {
                                Log._log.Warn(string.Format("ProcessMsg Break. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                                break;
                            }
                            CS_DISTRIBUTED_ALL_CARDS(owner, msg);
                        }
                        break;
                    case SS.Common.GameSelectKookjin:
                        {
                            if (!CheckGameRun())
                            {
                                Log._log.Warn(string.Format("ProcessMsg Break. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                                break;
                            }
                            CS_ANSWER_KOOKJIN_TO_PEE(owner, msg);
                        }
                        break;
                    case SS.Common.GameSelectGoStop:
                        {
                            if (!CheckGameRun())
                            {
                                Log._log.Warn(string.Format("ProcessMsg Break. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                                break;
                            }
                            CS_ANSWER_GO_OR_STOP(owner, msg);
                        }
                        break;
                    case SS.Common.GameActionPutCard:
                    case SS.Common.GameActionFlipBomb:
                    case SS.Common.GameActionChooseCard:
                        {
                            if (!CheckGameRun())
                            {
                                Log._log.Warn(string.Format("ProcessMsg Break. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                                break;
                            }

                            if (CS_ACTION(owner, msg, rmiID) == false)
                            {
                                Log._log.Warn(string.Format("CS_ACTION Failure. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                            }

                        }
                        break;
                    case SS.Common.GamePractice:
                        {
                            if (CheckGameRun())
                            {
                                Log._log.Warn(string.Format("ProcessMsg Break. rmiID:{0} Player:{1}", rmiID, owner.data.userID));
                                break;
                            }
                            CS_PRACTICE_GAME(owner, msg);
                        }
                        break;
                }
            }
        }

        void GameStart()
        {
            this.status = RoomStatus.GamePlay;

            PlayersGaming.Clear();
            foreach (var player in PlayersConnect)
            {
                PlayersGaming.TryAdd(player.Value.player_index, player.Value);
            }

            // 선 미리 정해놓음
            byte FirstTurnPlayerIndex = RoomSvr.IsPushTurn(PlayersGaming[0].data.GameLevel, PlayersGaming[0].data.UserLevel, PlayersGaming[1].data.GameLevel, PlayersGaming[1].data.UserLevel);

            // 초기화
#if DEBUG
            FirstTurnPlayerIndex = 1;
#endif

            reset(FirstTurnPlayerIndex);

            send_pick_card();

            //RoomSvr.PlayerReload(this);

#if EVENT_JACKPOT
            //// 이벤트(잭팟)
            //if (isJackPot == false)
            //{
            //    isJackPot = RoomSvr.EventCheck(ChanId);

            //    // 잭팟 이벤트 공지 (거북이, 상어, 거북이 이벤트)
            //    if (isJackPot)
            //    {
            //        CMessage msg = new CMessage();
            //        foreach (var player in PlayersConnect)
            //        {
            //            Send(player.Value, msg, SS.Common.GameEventStart);
            //        }

            //        Proxy.RoomLobbyEventStart((RemoteID)this.remote_lobby, CPackOption.Basic, this.roomID, 1);
            //    }
            //}
#endif
        }

        #region Packet Game
        void Send(CPlayer player, CMessage msg, PacketType rmiID)
        {
            if (IsPlayablePacket(player, rmiID))
            {
                if (player.status == Server.Engine.UserStatus.RoomPlayAuto)
                {
                    MessageTemp msg_ = new MessageTemp(msg, rmiID);
                    ActionExecute(player, msg_);
                    return;
                }
                else
                {
                    player.agent.actionTimeLimit = DateTime.Now.Ticks + 170000000;
                    //player.agent.isActionExecute = false;
                    MessageTemp msg_ = new MessageTemp(msg, rmiID);
                    player.agent.currentMsg = msg_;
                }
            }
            else
            {
                if (player.status == Server.Engine.UserStatus.RoomPlayAuto)
                {
                    return;
                }
            }

#if PACKET
            Log._log.InfoFormat("SEND Packet. rmiID:{0}, user:{1}", rmiID, player.data.userID);
#endif
            if (msg.m_array == null)
            {
                msg.m_array = new ArrByte();
            }

            bool sendOK;
            // 릴레이 패킷으로 변환하여 전송
            switch (rmiID)
            {
                case SS.Common.GameRequestReady: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRequestReady, msg); break;
                case SS.Common.GameStart: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayStart, msg); break;
                case SS.Common.GameObserveInfo: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayObserveInfo, msg); break;
                case SS.Common.GameNotifyIndex: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayNotifyIndex, msg); break;
                case SS.Common.GameRoomInfo: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRoomInfo, msg); break;
                case SS.Common.GameUserOut: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayUserOut, msg); break;
                case SS.Common.GameEventStart: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEventStart, msg); break;
                case SS.Common.GameFloorHasBonus: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayFloorHasBonus, msg); break;
                case SS.Common.GameTurnStart: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayTurnStart, msg); break;
                case SS.Common.GameSelectCardResult: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelaySelectCardResult, msg); break;
                case SS.Common.GameTurnResult: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayTurnResult, msg); break;
                case SS.Common.GameFlipDeckResult: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayFlipDeckResult, msg); break;
                case SS.Common.GameMoveKookjin: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayMoveKookjin, msg); break;
                case SS.Common.GameNotifyGoStop: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayNotifyGoStop, msg); break;
                case SS.Common.GameRequestKookjin: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRequestKookjin, msg); break;
                case SS.Common.GameRequestGoStop: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRequestGoStop, msg); break;
                case SS.Common.GameOver: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayOver, msg); break;
                case SS.Common.GameKickUser: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayKickUser, msg); break;
                case SS.Common.GameNotifyStat: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayNotifyStat, msg); break;
                case SS.Common.GamePracticeEnd: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayPracticeEnd, msg); break;
                case SS.Common.GameRequestSelectOrder: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRequestSelectOrder, msg); break;
                case SS.Common.GameOrderEnd: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayOrderEnd, msg); break;
                case SS.Common.GameDistributedStart: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayDistributedStart, msg); break;
                case SS.Common.GameUserInfo: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayUserInfo, msg); break;
                case SS.Common.GameEventInfo: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEventInfo, msg); break;
                case SS.Common.GameResponseRoomMissionInfo: sendOK = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayResponseRoomMissionInfo, msg); break;

                default: sendOK = false; Log._log.ErrorFormat("Send Relay Failure. player:{0}, rmiID:{1}", player.data.userID, rmiID); break;
            }

            if (sendOK == false)
            {
                Log._log.ErrorFormat("Send Failure. rmiID:{0}, user:{1}", rmiID, player.data.userID);
            }

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
        void CS_ROOM_IN(CPlayer owner, CMessage rm)
        {
            if (owner.PacketReady == true) return;

            roomWaitTime = DateTime.Now.AddSeconds(3);
            clear_received_protocol();

            // 유저 정보 전송
            send_player_index(owner);
            send_room_info(owner);
            send_players_info_room_in();
            // 이벤트 정보 전송
            send_event_jackpot_info(owner);
            send_event_tapssahgi_info(owner);

            owner.status = UserStatus.RoomStay;

            // 연습중이면 관전모드
            if (this.status == RoomStatus.PracticeGamePlay)
            {
                send_game_observe(owner);
            }
            // 게임시작 준비
            send_game_ready();
            owner.PacketReady = true;
        }
        void send_game_ready()
        {
            bool canStart;
            if (PlayersConnect.Count == max_users)
                canStart = true;
            else
                canStart = false;

            foreach (var player in PlayersConnect)
            {
                if (player.Value == null) continue;

                CMessage newmsg = new CMessage();
                if (player.Value.Operator)
                {
                    //Rmi.Marshaler.Write(newmsg, (bool)true); // 준비요청
                    Rmi.Marshaler.Write(newmsg, (bool)false); // 준비요청 (Ready 안보냄)
                    if (canStart)
                    {
                        Rmi.Marshaler.Write(newmsg, (bool)false); // 연습게임
                        //Rmi.Marshaler.Write(newmsg, (bool)false); // 게임시작
                        Rmi.Marshaler.Write(newmsg, (bool)true); // 게임시작
                    }
                    else
                    {
                        if (this.status == RoomStatus.Stay)
                        {
                            Rmi.Marshaler.Write(newmsg, (bool)true); // 연습게임
                        }
                        else
                        {
                            Rmi.Marshaler.Write(newmsg, (bool)false); // 연습게임
                        }
                        Rmi.Marshaler.Write(newmsg, (bool)false); // 게임시작
                    }
                }
                else
                {
                    Rmi.Marshaler.Write(newmsg, (bool)true); // 준비요청
                    Rmi.Marshaler.Write(newmsg, (bool)false); // 연습게임
                    Rmi.Marshaler.Write(newmsg, (bool)false); // 게임시작
                }

                Send(player.Value, newmsg, SS.Common.GameRequestReady);
            }
        }
        void send_game_observe(CPlayer owner)
        {
            CMessage newmsg = new CMessage();

            //바닥카드
            var floorCards = this.engine.floor_manager.GetCardsList();
            Rmi.Marshaler.Write(newmsg, (byte)floorCards.Count);
            for (int i = 0; i < floorCards.Count; ++i)
            {
                Rmi.Marshaler.Write(newmsg, (byte)floorCards[i].number);
                Rmi.Marshaler.Write(newmsg, (byte)floorCards[i].pae_type);
                Rmi.Marshaler.Write(newmsg, (byte)floorCards[i].position);
            }

            //각 플레이어 바닥과 손패
            Rmi.Marshaler.Write(newmsg, (byte)2);
            foreach (var player_ in PlayersGaming)
            {
                // 플레이어 인덱스
                byte pi = player_.Value.player_index;
                Rmi.Marshaler.Write(newmsg, (byte)pi);

                // 손패 수
                var handCount = player_.Value.agent.GetPlayerHandsCards().Count;
                Rmi.Marshaler.Write(newmsg, (byte)handCount);

                // 바닥
                var playerFloor = player_.Value.agent.GetPlayerFloorCards();
                Rmi.Marshaler.Write(newmsg, (byte)playerFloor.Count);
                for (int i = 0; i < playerFloor.Count; ++i)
                {
                    Rmi.Marshaler.Write(newmsg, (byte)playerFloor[i].number);
                    Rmi.Marshaler.Write(newmsg, (byte)playerFloor[i].pae_type);
                    Rmi.Marshaler.Write(newmsg, (byte)playerFloor[i].position);
                }

                // 나머지 정보
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.score);               // 점수
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.go_count);            // 고 횟수
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.shaking_count);       // 흔들기 횟수
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.ppuk_count);          // 뻑 횟수
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.get_pee_count());     // 피 장수
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.pibak);               // 피박 여부
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.kwangbak);            // 광박 여부
                Rmi.Marshaler.Write(newmsg, player_.Value.agent.multiple); // 현재 배수
            }

            Send(owner, newmsg, SS.Common.GameObserveInfo);

            PlayersObserve[0] = owner;
        }
        void send_player_index(CPlayer player)
        {
            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, player.player_index);

            Send(player, newmsg, SS.Common.GameNotifyIndex);
        }
        void send_room_info(CPlayer player)
        {
            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, (int)this.ChanId);
            Rmi.Marshaler.Write(newmsg, (int)this.roomNumber);
            Rmi.Marshaler.Write(newmsg, (int)this.engine.baseMoney);
            if (ChanType == ChannelType.Charge) // 머니타입
            {
                Rmi.Marshaler.Write(newmsg, (int)2); // 골드
            }
            else
            {
                Rmi.Marshaler.Write(newmsg, (int)1); // 실버
            }
            Send(player, newmsg, SS.Common.GameRoomInfo);
        }
        void send_room_out(CPlayer playerOut, bool init = false)
        {
            foreach (var player in PlayersConnect)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, (bool)init);

                if (player.Value.Operator)
                {
                    if (this.status == RoomStatus.Stay)
                    {
                        Rmi.Marshaler.Write(newmsg, (bool)true); // 연습게임
                    }
                    else
                    {
                        Rmi.Marshaler.Write(newmsg, (bool)false); // 연습게임
                    }
                }
                else
                {
                    Rmi.Marshaler.Write(newmsg, (bool)false); // 연습게임
                }

                Send(player.Value, newmsg, SS.Common.GameUserOut);
            }
        }
        void CS_READY_TO_START(CPlayer owner, CMessage rm)
        {
            clear_received_protocol();

            if (this.ChanType == ChannelType.Charge)
                owner.agent.setMoney(owner.data.money_pay);
            else
                owner.agent.setMoney(owner.data.money_free);

            // 플레이어한테 판돈이 부족하면 레디 안받고 강퇴 처리
            if (owner.agent.haveMoney < BaseMoney * 7)
            {
                send_player_all_in(owner);
                return;
            }

            if (owner.status == UserStatus.None)
            {
                return;
            }

            if (this.status == RoomStatus.PracticeGamePlay && owner.status == UserStatus.RoomPlay)
            {
                PracticeGameEnd();
            }

            owner.status = UserStatus.RoomReady;

            if (this.status != RoomStatus.Stay) return;

            //게임 참여자 모두가 준비 완료 되었음
            if (PlayersConnect.Count == max_users)
            {
                foreach (var player in PlayersConnect)
                {
                    if (player.Value.status != UserStatus.RoomReady) return;

                    if (player.Value.agent.haveMoney < BaseMoney * 7)
                    {
                        send_player_all_in(player.Value);
                        return;
                    }
                }

                // 게임시작
                GameStart();
            }
            else
            {

            }
        }
        public void AutoGameStart()
        {
            if (PlayersConnect.Count < 2) return;

            foreach (var player in PlayersConnect)
            {
                if (ChanType == ChannelType.Charge)
                    player.Value.agent.setMoney(player.Value.data.money_pay);
                else
                    player.Value.agent.setMoney(player.Value.data.money_free);
            }

            foreach (var player in PlayersConnect)
            {
                if (player.Value.status == UserStatus.None) return;
            }

            if (this.status != RoomStatus.Stay) return;

            //게임 참여자 모두 준비 완료
            if (PlayersConnect.Count == 2)
            {
                foreach (var player in PlayersConnect)
                {
                    if (player.Value.status != UserStatus.RoomReady && player.Value.status != UserStatus.RoomStay) return;
                }
                GameStart();
            }
            //if (players.Count >= 2)
            //    gameStart();
        }
        private void send_pick_card()
        {
            CMessage newmsg = new CMessage();
            foreach (var player in PlayersGaming)
            {
                Send(player.Value, newmsg, SS.Common.GameStart);
            }
        }
        void CS_DISTRIBUTED_ALL_CARDS(CPlayer owner, CMessage rm)
        {
            clear_received_protocol();

            foreach (var playerO in PlayersObserve)
            {
                if (playerO == null) continue;
                if (playerO == owner) return;
            }

            if (owner.agent.status != UserGameStatus.DistributeCard) return;
            owner.agent.status = UserGameStatus.DealCard;
            //owner.agent.status = UserGameStatus.Play;

            /*
            if (!all_received(rm.pkID))
            {
                return;
            }
            */
            //clear_received_protocol();

            // 플레이어 모두 준비되면 처리
            foreach (var player in PlayersGaming)
                if (player.Value.agent.status != UserGameStatus.DealCard) return;
            foreach (var player in PlayersGaming)
                player.Value.agent.status = UserGameStatus.Play;

            CPlayer currentPlayer = current_player();

            bool FloorBonus;
            FloorBonus = this.engine.RefreshFloorCards(currentPlayer);

            // 바닥에 보너스카드가 있으면 선에게 주고 더미에서 카드를 꺼낸뒤 턴 시작
            if (FloorBonus)
            {
                // 로그
                for (int j = 0; j < this.engine.distributed_floor_bonus_cards.Count; j++)
                {
                    gameLog.ActionCard("d", this.engine.distributed_floor_bonus_cards[j]);
                }
                for (int j = 0; j < this.engine.distributed_bonus_floor_cards.Count; ++j)
                {
                    gameLog.ActionCard("e", this.engine.distributed_bonus_floor_cards[j]);
                }

                byte player_index = currentPlayer.player_index;

                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, player_index);
                int bonusCardCount = this.engine.distributed_floor_bonus_cards.Count;
                Rmi.Marshaler.Write(newmsg, bonusCardCount);
                for (int j = 0; j < bonusCardCount; j++)
                {
                    Rmi.Marshaler.Write(newmsg, this.engine.distributed_floor_bonus_cards[j].number);
                    Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_floor_bonus_cards[j].pae_type);
                    Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_floor_bonus_cards[j].position);
                }
                int floorcardcount = this.engine.distributed_bonus_floor_cards.Count;
                Rmi.Marshaler.Write(newmsg, floorcardcount);
                for (int j = 0; j < floorcardcount; ++j)
                {
                    Rmi.Marshaler.Write(newmsg, this.engine.distributed_bonus_floor_cards[j].number);
                    Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_bonus_floor_cards[j].pae_type);
                    Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_bonus_floor_cards[j].position);
                }

                foreach (var player in PlayersGaming)
                {
                    Send(player.Value, newmsg, SS.Common.GameFloorHasBonus);
                    if (player.Value.isPracticeDummy)
                    {
                        foreach (var playerO in PlayersObserve)
                        {
                            if (playerO == null) continue;
                            Send(playerO, newmsg, SS.Common.GameFloorHasBonus);
                        }
                    }
                }
            }

            // 바닥에 보너스카드가 없으면 총통 확인하고 턴 시작.
            // 총통으로 게임이 종료됐으면 멈춤
            if (CheckChongtong(currentPlayer) == false)
            {
                send_player_start_turn(currentPlayer);
            }
        }
        void send_player_start_turn(CPlayer currentPlayer)
        {
            byte player_index = currentPlayer.player_index;

            currentPlayer.agent.status = UserGameStatus.TurnStart;
            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, player_index);
                Rmi.Marshaler.Write(newmsg, (byte)0);
                Send(player.Value, newmsg, SS.Common.GameTurnStart);
            }
        }
        bool CS_ACTION(CPlayer owner, CMessage rm, PacketType rmiID)
        {
            foreach (var playerO in PlayersObserve)
            {
                if (playerO == null) continue;
                if (playerO == owner) return false;
            }
            clear_received_protocol();
            if (owner.player_index != this.engine.current_player_index) return false;

            bool result;
            switch (rmiID)
            {
                case SS.Common.GameActionPutCard:
                    {
                        if (owner.agent.status != UserGameStatus.TurnStart) return false;
                        owner.agent.status = UserGameStatus.Play;

                        byte number;
                        Rmi.Marshaler.Read(rm, out number);
                        byte paetype;
                        Rmi.Marshaler.Read(rm, out paetype);
                        PAE_TYPE pae_type = (PAE_TYPE)paetype;
                        byte position;
                        Rmi.Marshaler.Read(rm, out position);
                        byte slot_index;
                        Rmi.Marshaler.Read(rm, out slot_index);
                        byte is_shaking;
                        Rmi.Marshaler.Read(rm, out is_shaking);

                        result = player_put_card(owner, number, pae_type, position, slot_index, is_shaking);
                    }
                    break;
                case SS.Common.GameActionFlipBomb:
                    {
                        if (owner.agent.status != UserGameStatus.TurnStart) return false;
                        owner.agent.status = UserGameStatus.Play;

                        if (owner.agent.decrease_bomb_count() == false)
                        {
                            result = false;
                            break;
                        }
                        else
                        {
                            result = true;
                        }
                        owner.agent.flip_type = FLIP_TYPE.FLIP_BOOM;
                        PLAYER_SELECT_CARD_RESULT flipResult = player_filp_card(owner);

                        if (flipResult != PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
                        {
                            // 게임완료 체크
                            if (this.engine.check_kookjin(owner.agent))
                            {
                                send_ask_kookjin_to_pee(owner);
                            }
                            else if (check_game_finish(owner) == false)
                            {
                                foreach (var iplayer in PlayersGaming)
                                    iplayer.Value.agent.status = UserGameStatus.Play;
                                next_turn();
                            }
                        }
                    }
                    break;
                case SS.Common.GameActionChooseCard:
                    {
                        if (owner.agent.status != UserGameStatus.TurnSelect) return false;
                        owner.agent.status = UserGameStatus.Play;

                        byte choice_index;
                        Rmi.Marshaler.Read(rm, out choice_index);

                        result = player_choose_card(owner, choice_index);

                    }
                    break;
                default:
                    result = false;
                    return false;
            }


            return result;
        }
        public bool player_put_card(CPlayer player, byte card_number, PAE_TYPE pae_type, byte position, byte slot_index, byte is_shaking)
        {
            // 어드벤티지 확인
            if (player.agent.Advantage)
            {
                // 상대가 너무 유리함 (바닥패 차이 n장 이상)
                if (player.agent.get_cards_count() + 8 <= this_player_next(player.player_index).agent.get_cards_count())
                {
                    //player.agent.AdvantageType1 = true;
                    player.agent.AdvantageType2 = true;
                    //player.agent.AdvantageType3 = true;
                }
                else
                {
                    player.agent.AdvantageType2 = false;
                }
            }

            // 패널티 확인
            if (player.agent.Penalty)
            {
                // 내가 너무 유리함 (바닥패 차이 n장 이상)
                if (this_player_next(player.player_index).agent.get_cards_count() + 8 <= player.agent.get_cards_count())
                {
                    //player.agent.PenaltyType1 = true;
                    player.agent.PenaltyType2 = true;
                    player.agent.PenaltyType3 = true;
                }
                else
                {
                    player.agent.PenaltyType2 = false;
                    player.agent.PenaltyType3 = false;
                }
            }

            // 패 처리
            PLAYER_SELECT_CARD_RESULT result = this.engine.Player_Put_HandCard(player, card_number, pae_type, position, is_shaking);

            //clear_received_protocol();

            if (result == PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD)
            {
                // 잘못된 패킷 데이터 처리. 폭탄패 없으면 강제 종료 시킴
                Log._log.ErrorFormat("player_put_card. ERROR_INVALID_CARD player:{0}", player.data.userID);

                // 폭탄패 확인
                if (player.agent.decrease_bomb_count() == true)
                {
                    player.agent.flip_type = FLIP_TYPE.FLIP_BOOM;
                    PLAYER_SELECT_CARD_RESULT flipResult = player_filp_card(player);

                    if (flipResult != PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
                    {
                        // 게임완료 체크
                        if (this.engine.check_kookjin(player.agent))
                        {
                            send_ask_kookjin_to_pee(player);
                        }
                        else if (check_game_finish(player) == false)
                        {
                            foreach (var iplayer in PlayersGaming)
                                iplayer.Value.agent.status = UserGameStatus.Play;
                            next_turn();
                        }
                    }
                    return true;
                }
                else
                {
                    //비정상 플레이어 연결 종료
                    Log._log.Error("player_put_card Invalid. Player:" + player.data.userID);
                    RoomSvr.ClientDisconect(player.Remote.Key, player.Remote.Value, "Invalid Player(player_put_card)");
                }

                return false;
            }

            send_player_put_card_result(result, player, slot_index);
            if (result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                // 보너스 카드 처리
                this.engine.Get_Cards_from_Others(player);
                player.agent.put_card_bonus = true;
                send_turn_result(player, true);
            }
            else if (result == PLAYER_SELECT_CARD_RESULT.COMPLETED)
            {
                // 덱 뒤집기
                player.agent.flip_type = FLIP_TYPE.FLIP_NORMAL;
                result = player_filp_card(player);
            }

            if (result != PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER && result != PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
            {
                // 게임완료 체크
                if (player.agent.put_card_bonus)
                {
                    player.agent.put_card_bonus = false;
                    // 전 턴에 보너스 카드를 냈었다면 턴 계속 진행
                    foreach (var iplayer in PlayersGaming)
                        iplayer.Value.agent.status = UserGameStatus.Play;
                    again_myturn();
                }
                else if (this.engine.check_kookjin(player.agent))
                {
                    send_ask_kookjin_to_pee(player);
                }
                else if (check_game_finish(player) == false)
                {
                    foreach (var iplayer in PlayersGaming)
                        iplayer.Value.agent.status = UserGameStatus.Play;
                    next_turn();
                }
            }

            return true;
        }
        void send_player_put_card_result(PLAYER_SELECT_CARD_RESULT result, CPlayer player, byte slot_index)
        {
            // 로그
            switch (this.engine.card_event_type)
            {
                case CARD_EVENT_TYPE.BOMB:
                    {
                        for (byte i = 0; i < this.engine.bomb_cards_from_player.Count; ++i)
                        {
                            gameLog.ActionCard("b", this.engine.bomb_cards_from_player[i]);
                        }
                    }
                    break;
                default:
                    {
                        gameLog.ActionCard("b", this.engine.card_from_player);
                    }
                    break;
            }

            //카드를 두장 가져오는걸 예방하기 위해 먼저 실행
            CCard card = null;// = new CCard(20, PAE_TYPE.KWANG, 0);
            if (result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                // 보너스 카드를 냈을 경우
                card = this.engine.Player_Put_Bonus_Card(player);
                gameLog.ActionCard("a", card);
            }

            CMessage newmsg = new CMessage();

            Rmi.Marshaler.Write(newmsg, (byte)0); // delay 미사용
            // 플레이어 정보.
            Rmi.Marshaler.Write(newmsg, player.player_index);
            // 낸 카드 정보.
            // 플레이어가 낸 카드 정보.

            Rmi.Marshaler.Write(newmsg, this.engine.card_from_player.number);
            Rmi.Marshaler.Write(newmsg, (byte)this.engine.card_from_player.pae_type);
            Rmi.Marshaler.Write(newmsg, this.engine.card_from_player.position);
            Rmi.Marshaler.Write(newmsg, this.engine.same_card_count_with_player);
            Rmi.Marshaler.Write(newmsg, slot_index);

            // 카드 이벤트.
            Rmi.Marshaler.Write(newmsg, (byte)this.engine.card_event_type);

            // 폭탄 카드 정보.
            switch (this.engine.card_event_type)
            {
                case CARD_EVENT_TYPE.BOMB:
                    {
                        byte bomb_cards_count = (byte)this.engine.bomb_cards_from_player.Count;
                        Rmi.Marshaler.Write(newmsg, (byte)bomb_cards_count);
                        for (byte card_index = 0; card_index < bomb_cards_count; ++card_index)
                        {
                            Rmi.Marshaler.Write(newmsg, this.engine.bomb_cards_from_player[card_index].number);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.bomb_cards_from_player[card_index].pae_type);
                            Rmi.Marshaler.Write(newmsg, this.engine.bomb_cards_from_player[card_index].position);
                        }
                    }
                    break;

                case CARD_EVENT_TYPE.SHAKING:
                    {
                        byte shaking_cards_count = (byte)this.engine.shaking_cards.Count;
                        Rmi.Marshaler.Write(newmsg, (byte)shaking_cards_count);
                        for (byte card_index = 0; card_index < shaking_cards_count; ++card_index)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.shaking_cards[card_index].number);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.shaking_cards[card_index].pae_type);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.shaking_cards[card_index].position);
                        }
                    }
                    break;
            }

            // 둘중 하나를 선택하는 경우 대상이 되는 카드 정보를 담는다.
            Rmi.Marshaler.Write(newmsg, (byte)result);
            if (result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                //가운데 카드를 뒤집어서 내 패로 이동
                Rmi.Marshaler.Write(newmsg, card.number);
                Rmi.Marshaler.Write(newmsg, (byte)card.pae_type);
                Rmi.Marshaler.Write(newmsg, card.position);
                this.engine.floor_cards_to_player.Clear();
            }
            else if (result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)
            {
                //선택할 카드 정보를 담는다.
                add_choice_card_info_to(newmsg);

                player.agent.status = UserGameStatus.TurnSelect;
            }

            foreach (var iplayer in PlayersGaming)
            {
                Send(iplayer.Value, newmsg, SS.Common.GameSelectCardResult);
                if (iplayer.Value.isPracticeDummy)
                {
                    foreach (var playerO in PlayersObserve)
                    {
                        if (playerO == null) continue;
                        Send(playerO, newmsg, SS.Common.GameSelectCardResult);
                    }
                }
            }
        }
        public bool player_choose_card(CPlayer player, byte choice_index)
        {
            // 카드 선택
            PLAYER_SELECT_CARD_RESULT result = this.engine.on_choose_card(player, choice_index);

            if (result == PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD)
            {
                return false;
            }

            if (result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)
            {
                player.agent.flip_type = FLIP_TYPE.FLIP_NORMAL;
                result = player_filp_card(player);
            }
            else
            {
                this.engine.Get_Cards_from_Others(player);
                send_turn_result(player);
            }

            if (result != PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
            {
                // 게임완료 체크
                if (this.engine.check_kookjin(player.agent))
                {
                    send_ask_kookjin_to_pee(player);
                }
                else if (check_game_finish(player) == false)
                {
                    foreach (var iplayer in PlayersGaming)
                        iplayer.Value.agent.status = UserGameStatus.Play;
                    next_turn();
                }
            }

            return true;
        }
        void send_turn_result(CPlayer owner, bool bonus = false)
        {
            // 탑쌓기 이벤트 체크
            missionsManager.CheckMission(PlayersGaming);

            byte count_cards = (byte)this.engine.floor_cards_to_player.Count;
            for (byte card_index = 0; card_index < count_cards; ++card_index)
            {
                gameLog.ActionCard("d", this.engine.floor_cards_to_player[card_index]);
            }
            foreach (KeyValuePair<byte, List<CCard>> kvp in this.engine.other_cards_to_player)
            {
                byte count = (byte)this.engine.other_cards_to_player[kvp.Key].Count;
                for (byte card_index = 0; card_index < count; ++card_index)
                {
                    gameLog.ActionCard("d", this.engine.other_cards_to_player[kvp.Key][card_index]);
                }
            }

            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                // 플레이어 정보.
                Rmi.Marshaler.Write(newmsg, owner.player_index);
                Rmi.Marshaler.Write(newmsg, bonus);

                // 턴 정보
                player_turn_result(owner, newmsg);

                // 미션 정보
                Rmi.Marshaler.Write(newmsg, player.Value.agent.missionresult);
                Rmi.Marshaler.Write(newmsg, owner.agent.missionscore);
                player.Value.agent.missionresult = 0;

                Send(player.Value, newmsg, SS.Common.GameTurnResult);
                if (player.Value.isPracticeDummy)
                {
                    foreach (var playerO in PlayersObserve)
                    {
                        if (playerO == null) continue;
                        Send(playerO, newmsg, SS.Common.GameTurnResult);
                    }
                }
            }
        }
        PLAYER_SELECT_CARD_RESULT player_filp_card(CPlayer player)
        {
            bool whalbin = false;

            PLAYER_SELECT_CARD_RESULT result = this.engine.Player_Flip_DeckCard(player, player.agent.flip_type, ref whalbin);

            if (result == PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD)
            {
                Log._log.ErrorFormat("player_filp_card. ERROR_INVALID_CARD");
                return PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD;
            }

            // 활빈당 미션이면 (보너스카드는 붙어도 배수 증가 없음)
            if (whalbin && player.agent.front_card_match && result != PLAYER_SELECT_CARD_RESULT.BONUS_CARD && missionsManager.GetCurrentMission() == MISSION.WHALBIN)
            {
                player.agent.missionresult = 3;
                player.agent.missionscore++;
                current_player_next().agent.missionresult = 4;
            }
            player.agent.front_card_match = false;

            if (result != PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                this.engine.Get_Cards_from_Others(player);
            }

            send_flip_result(player, result);

            if (result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                player.agent.flip_type = FLIP_TYPE.FLIP_BONUS;
                result = player_filp_card(player);
            }

            return result;
        }
        void send_flip_result(CPlayer currentPlayer, PLAYER_SELECT_CARD_RESULT result, bool choose = false)
        {
            missionsManager.CheckMission(PlayersGaming);

            //currentPlayer.agent.status = UserGameStatus.TurnEnd;

            gameLog.ActionCard("c", this.engine.card_from_deck);
            byte count_cards = (byte)this.engine.floor_cards_to_player.Count;
            for (byte card_index = 0; card_index < count_cards; ++card_index)
            {
                gameLog.ActionCard("d", this.engine.floor_cards_to_player[card_index]);
            }
            foreach (KeyValuePair<byte, List<CCard>> kvp in this.engine.other_cards_to_player)
            {
                byte count = (byte)this.engine.other_cards_to_player[kvp.Key].Count;
                for (byte card_index = 0; card_index < count; ++card_index)
                {
                    gameLog.ActionCard("d", this.engine.other_cards_to_player[kvp.Key][card_index]);
                }
            }

            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();

                // 플레이어 정보.
                Rmi.Marshaler.Write(newmsg, currentPlayer.player_index);
                // 덱에서 뒤집은 카드 정보.
                Rmi.Marshaler.Write(newmsg, this.engine.card_from_deck.number);
                Rmi.Marshaler.Write(newmsg, (byte)this.engine.card_from_deck.pae_type);
                Rmi.Marshaler.Write(newmsg, this.engine.card_from_deck.position);
                Rmi.Marshaler.Write(newmsg, this.engine.same_card_count_with_deck);

                Rmi.Marshaler.Write(newmsg, (byte)result);
                Rmi.Marshaler.Write(newmsg, choose);
                Rmi.Marshaler.Write(newmsg, (byte)currentPlayer.agent.flip_type);
                if (result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
                {
                    // for AI
                    add_floor_cards_to_player(newmsg);
                    add_other_cards_to_player(newmsg);
                    add_choice_card_info_to(newmsg);

                    player.Value.agent.status = UserGameStatus.TurnSelect;
                }
                else
                {
                    // 턴 정보
                    player_turn_result(currentPlayer, newmsg);

                    // 미션 정보
                    Rmi.Marshaler.Write(newmsg, player.Value.agent.missionresult);
                    Rmi.Marshaler.Write(newmsg, currentPlayer.agent.missionscore);
                    player.Value.agent.missionresult = 0;
                }

                Send(player.Value, newmsg, SS.Common.GameFlipDeckResult);
                if (player.Value.isPracticeDummy)
                {
                    foreach (var playerO in PlayersObserve)
                    {
                        if (playerO == null) continue;
                        Send(playerO, newmsg, SS.Common.GameFlipDeckResult);
                    }
                }
            }
        }
        void add_choice_card_info_to(CMessage msg)
        {
            List<CCard> target_cards = this.engine.target_cards_to_choice;
            byte count = (byte)target_cards.Count;
            Rmi.Marshaler.Write(msg, count);
            for (int i = 0; i < count; ++i)
            {
                CCard card = target_cards[i];
                Rmi.Marshaler.Write(msg, card.number);
                Rmi.Marshaler.Write(msg, (byte)card.pae_type);
                Rmi.Marshaler.Write(msg, card.position);
            }
        }
        void player_turn_result(CPlayer currentPlayer, CMessage msg)
        {
            add_floor_cards_to_player(msg);
            add_other_cards_to_player(msg);
            add_turn_result_to(currentPlayer, msg);
        }
        void add_floor_cards_to_player(CMessage msg)
        {
            // 플레이어가 바닥에서 가져갈 카드
            byte count_cards = (byte)this.engine.floor_cards_to_player.Count;
            Rmi.Marshaler.Write(msg, count_cards);
            for (byte card_index = 0; card_index < count_cards; ++card_index)
            {
                CCard card = this.engine.floor_cards_to_player[card_index];
                Rmi.Marshaler.Write(msg, card.number);
                Rmi.Marshaler.Write(msg, (byte)card.pae_type);
                Rmi.Marshaler.Write(msg, card.position);
            }
        }
        void add_other_cards_to_player(CMessage msg)
        {
            Rmi.Marshaler.Write(msg, (byte)this.engine.other_cards_to_player.Count);
            foreach (KeyValuePair<byte, List<CCard>> kvp in this.engine.other_cards_to_player)
            {
                Rmi.Marshaler.Write(msg, kvp.Key);
                byte count = (byte)this.engine.other_cards_to_player[kvp.Key].Count;
                Rmi.Marshaler.Write(msg, count);
                for (byte card_index = 0; card_index < count; ++card_index)
                {
                    CCard card = this.engine.other_cards_to_player[kvp.Key][card_index];
                    Rmi.Marshaler.Write(msg, card.number);
                    Rmi.Marshaler.Write(msg, (byte)card.pae_type);
                    Rmi.Marshaler.Write(msg, card.position);
                }
            }
        }
        void add_turn_result_to(CPlayer currentPlayer, CMessage msg)
        {
            //플레이어 바닥 이벤트
            currentPlayer.agent.CheckPlayerFloor();
            byte playerflooreventcount = (byte)currentPlayer.agent.PlayerFloorCheck.Count;
            Rmi.Marshaler.Write(msg, playerflooreventcount);
            for (byte j = 0; j < playerflooreventcount; ++j)
            {
                Rmi.Marshaler.Write(msg, (byte)currentPlayer.agent.PlayerFloorCheck[j]);
            }

            // 카드 이벤트 정보.
            byte count = (byte)this.engine.flipped_card_event_type.Count;
            Rmi.Marshaler.Write(msg, count);
            for (byte i = 0; i < count; ++i)
            {
                Rmi.Marshaler.Write(msg, (byte)this.engine.flipped_card_event_type[i]);
            }
        }
        void CS_ANSWER_KOOKJIN_TO_PEE(CPlayer owner, CMessage rm)
        {
            foreach (var playerO in PlayersObserve)
            {
                if (playerO == null) continue;
                if (playerO == owner) return;
            }
            if (owner.player_index != this.current_player().player_index || owner.agent.status != UserGameStatus.KookJin) return;
            owner.agent.status = UserGameStatus.Play;

            //clear_received_protocol();

            owner.agent.kookjin_selected();
            byte answer;
            Rmi.Marshaler.Read(rm, out answer);
            gameLog.ActionKookJin(answer.ToString());
            if (answer == 1)
            {
                // 국진을 쌍피로 이동.
                owner.agent.move_kookjin_to_pee();
                send_move_kookjin_to_pee(owner.player_index);
            }

            if (check_game_finish(owner) == false)
            {
                // 판이 날때까지 다음 턴 진행
                next_turn();
            }
        }
        void send_move_kookjin_to_pee(byte owner_player_index)
        {
            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, owner_player_index);
                Send(player.Value, newmsg, SS.Common.GameMoveKookjin);
                if (player.Value.isPracticeDummy)
                {
                    foreach (var playerO in PlayersObserve)
                    {
                        if (playerO == null) continue;
                        Send(player.Value, newmsg, SS.Common.GameMoveKookjin);
                    }
                }
            }
        }
        void CS_ANSWER_GO_OR_STOP(CPlayer owner, CMessage rm)
        {
            foreach (var playerO in PlayersObserve)
            {
                if (playerO == null) continue;
                if (playerO == owner) return;
            }
            if (owner.player_index != this.current_player().player_index || owner.agent.status != UserGameStatus.GoStop) return;
            owner.agent.status = UserGameStatus.Play;

            //clear_received_protocol();

            // answer가 1이면 GO, 0이면 STOP.
            byte answer;
            Rmi.Marshaler.Read(rm, out answer);
            gameLog.ActionGoStop(answer.ToString());
            if (answer == 1)
            {
                owner.agent.plus_go_count();
                send_notify_go_count(owner);
                next_turn();

                // 고 추가 점수로만 고 or 스톱되지 않도록 prev_score 갱신
                owner.agent.update_prev_score();
            }
            else
            {
                gameEnd(this.engine.get_player_result(), owner);
            }

        }
        void send_notify_go_count(CPlayer player)
        {
            byte delay = 0;

            foreach (var player_ in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, player.player_index);
                Rmi.Marshaler.Write(newmsg, delay);
                Rmi.Marshaler.Write(newmsg, player.agent.go_count);
                Send(player_.Value, newmsg, SS.Common.GameNotifyGoStop);
                if (player_.Value.isPracticeDummy)
                {
                    foreach (var playerO in PlayersObserve)
                    {
                        if (playerO == null) continue;
                        Send(playerO, newmsg, SS.Common.GameNotifyGoStop);
                    }
                }
            }
        }
        void send_ask_kookjin_to_pee(CPlayer currentPlayer)
        {
            send_player_statistics();
            byte player_index = currentPlayer.player_index;
            currentPlayer.agent.status = UserGameStatus.KookJin;
            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, player_index);
                Send(player.Value, newmsg, SS.Common.GameRequestKookjin);
            }
        }
        bool check_game_finish(CPlayer currentPlayer)
        {
            if (currentPlayer.agent.ppuk_count >= 3)
            {
                // 삼뻑
                gameEnd(this.engine.get_player_result_ppuk(), currentPlayer);
            }
            else if (this.engine.is_time_to_ask_gostop(currentPlayer))
            {
                // 뒤집을 카드가 없음, 내손에 패가 없음, 7고 = 자동 스톱
                if ((currentPlayer.agent.GetPlayerHandsCards().Count == 0 && currentPlayer.agent.remain_bomb_count == 0) || currentPlayer.agent.go_count >= 7)
                {
                    gameEnd(this.engine.get_player_result(), currentPlayer);
                }
                else
                {
                    send_go_or_stop(currentPlayer);
                }
            }
            else
            {
                if (this.engine.is_finished())
                {
                    // 나가리(무승부)
                    gameEnd(GAME_RESULT_TYPE.DRAW, currentPlayer);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        void send_go_or_stop(CPlayer currentPlayer)
        {
            send_player_statistics();

            long potential_money = currentPlayer.agent.score;

            CPlayer nextPlayer = this_player_next(currentPlayer.player_index);

            // 스톱 했을때 받을 수 있는 머니 (상대방 머니보다 많이 받을 수 없음)
            potential_money = this.engine.baseMoney * currentPlayer.agent.multiple * currentPlayer.agent.score;

            // 배팅 한도
            /*
            if (potential_money < BetingLimite)
            {
                potential_money = BetingLimite;
            }
            */

            currentPlayer.agent.status = UserGameStatus.GoStop;

            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, currentPlayer.player_index);
                Rmi.Marshaler.Write(newmsg, currentPlayer.agent.go_count);
                Rmi.Marshaler.Write(newmsg, potential_money);
                //Rmi.Marshaler.Write(newmsg, Math.Min(potential_money, current_player_next().agent.haveMoney));
                Send(player.Value, newmsg, SS.Common.GameRequestGoStop);
            }
        }
        void next_turn()
        {
            gameLog.TurnNext();
            this.engine.clear_turn_data();
            this.engine.move_to_next_player();
            send_players_info();
            send_player_next_turn();

            // 선후 1턴씩 진행했으면 미션정보 전송 (연습게임일 경우 X)
            if (engine.IsPractice == false
                && current_player().agent.is_first_hand()
                && engine.missionSended == false)
            {
                engine.missionSended = true;
                CMessage newmsg = new CMessage();
                List<CCard> missioncards = new List<CCard>();
                if (this.currentMission >= MISSION.WOL1_2 && this.currentMission <= MISSION.WOL12_2)
                {
                    missioncards = missionsManager.missionCards;
                }
                Rmi.Marshaler.Write(newmsg, (byte)currentMission);
                Rmi.Marshaler.Write(newmsg, (byte)missioncards.Count);
                if (missioncards.Count > 0)
                {
                    for (int i = 0; i < missioncards.Count; ++i)
                    {
                        Rmi.Marshaler.Write(newmsg, missioncards[i].number);
                        Rmi.Marshaler.Write(newmsg, (byte)missioncards[i].pae_type);
                        Rmi.Marshaler.Write(newmsg, missioncards[i].position);
                    }
                }
                Rmi.Marshaler.Write(newmsg, (byte)missionsManager.missionscore);
                foreach (var player in PlayersGaming)
                {
                    Send(player.Value, newmsg, SS.Common.GameResponseRoomMissionInfo);
                }
            }
        }
        void again_myturn()
        {
            this.engine.clear_turn_data();
            send_player_next_turn();
        }
        void send_player_next_turn()
        {
            CPlayer currentPlayer = current_player();
            send_player_statistics();
            currentPlayer.agent.status = UserGameStatus.TurnStart;

            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, currentPlayer.player_index);
                Rmi.Marshaler.Write(newmsg, currentPlayer.agent.remain_bomb_count);

                //// 바닥 카드 갱신을 위한 데이터.
                //List<CFloorSlot> slots = this.engine.floor_manager.slots;

                //Rmi.Marshaler.Write(newmsg, (byte)slots.Count);
                //for (int j = 0; j < slots.Count; ++j)
                //{
                //    Rmi.Marshaler.Write(newmsg, (byte)slots[j].cards.Count);
                //    for (int card_index = 0; card_index < slots[j].cards.Count; ++card_index)
                //    {
                //        CCard card = slots[j].cards[card_index];
                //        Rmi.Marshaler.Write(newmsg, card.number);
                //        Rmi.Marshaler.Write(newmsg, (byte)card.pae_type);
                //        Rmi.Marshaler.Write(newmsg, card.position);
                //    }
                //}
                Send(player.Value, newmsg, SS.Common.GameTurnStart);
            }
        }
        void gameEnd(GAME_RESULT_TYPE result, CPlayer winPlayer)
        {
            // 마지막으로 플레이어 정보 전송

            int result_score = 0, result_finalscore = 0, result_gobak = 1, result_pibak = 1, result_kwangbak = 1, result_mungtta = 1, result_shaking = 1, result_go = 1, result_mission = 1, result_draw = 1, result_push = 1, result_chongtongnumber = 12;
            bool result_threePpuck = false, result_Loser_NoMoney = false;

            short score1 = 0, score2 = 0, score3 = 0, score4 = 0, score5 = 0; // 점수 디테일. 광 열 띠 피 고
            long JackPotResultMoney = 0;
            int JackPotResultType = 0;
            int JackPotResultPlayerId = 0;

            CPlayer losePlayer = this_player_next(winPlayer.player_index);

            bool isPractice;

            if (winPlayer.isPracticeDummy || losePlayer.isPracticeDummy)
                isPractice = true;
            else
                isPractice = false;

            winPlayer.ChangeMoney = 0;
            winPlayer.GameDealMoney = 0;
            winPlayer.JackPotDealMoney = 0;
            losePlayer.ChangeMoney = 0;
            losePlayer.GameDealMoney = 0;
            losePlayer.JackPotDealMoney = 0;

            // 패자가 피박인데 국진이 있다면 쌍피로 다시 계산하도록
            if (losePlayer.agent.is_used_kookjin == false && losePlayer.agent.pibak && losePlayer.agent.GetPibakCheckMoveKookjin())
            {
                send_move_kookjin_to_pee(losePlayer.player_index);
            }

            send_player_statistics();

            if (result == GAME_RESULT_TYPE.START_PLAYER_WIN || result == GAME_RESULT_TYPE.LAST_PLAYER_WIN)
            {
                winPlayer.agent.get_score_detail(out score1, out score2, out score3, out score4, out score5);

                if (score1 == 15)
                {
                    // 5광 잭팟
                    //JackPotResultType = 1;

                }

                winPlayer.GameResult = 1;
                winPlayer.agent.win_count += 1;
                losePlayer.GameResult = 2;
                losePlayer.agent.win_count = 0;
                result_score = winPlayer.agent.score; // 패 점수

                if (losePlayer.agent.gobak) // 고박
                {
                    winPlayer.agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.GO_BAK_WIN);
                    result_gobak = 2;
                }
                if (losePlayer.agent.pibak) // 피박
                    result_pibak = 2;
                if (losePlayer.agent.kwangbak) // 광박
                    result_kwangbak = 2;
                if (winPlayer.agent.mungtta) // 멍따
                    result_mungtta = 2;

                if (winPlayer.agent.shaking_count >= 1)
                    result_shaking = (int)Math.Pow(2, winPlayer.agent.shaking_count);  // 흔들기 배수
                if (winPlayer.agent.go_count >= 3)
                    result_go = (int)Math.Pow(2, winPlayer.agent.go_count - 2);  // 고 배수

                result_mission = Math.Max((int)winPlayer.agent.missionscore, 1);   // 미션 배수
                result_draw = Math.Max((int)Math.Pow(2, this.drow_count), 1);   // 무승부 배수
                this.drow_count = 0;
                result_push = 1;    // 밀기 배수 (미사용)

                result_finalscore = result_score * result_gobak * result_pibak * result_kwangbak * result_mungtta * result_shaking * result_go * result_mission * result_draw * result_push;

                if (isPractice == false)
                    game_result_players_money_change(winPlayer, losePlayer, this.engine.baseMoney * result_finalscore, ref result_Loser_NoMoney);
            }
            else if (result == GAME_RESULT_TYPE.START_PLAYER_CHONGTONG || result == GAME_RESULT_TYPE.LAST_PLAYER_CHONGTONG)
            {
                winPlayer.GameResult = 1;
                winPlayer.agent.win_count += 1;
                losePlayer.GameResult = 2;
                losePlayer.agent.win_count = 0;
                winPlayer.agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.CHONGTONG);

                // 총통 점수계산
                result_score = 7;

                result_mission = Math.Max((int)winPlayer.agent.missionscore, 1);
                result_draw = Math.Max((int)Math.Pow(2, this.drow_count), 1);
                this.drow_count = 0;
                result_push = 1;

                result_finalscore = result_score * result_mission * result_draw * result_push;

                result_chongtongnumber = winPlayer.agent.chong_tong_number;

                if (isPractice == false)
                    game_result_players_money_change(winPlayer, losePlayer, this.engine.baseMoney * result_finalscore, ref result_Loser_NoMoney);
            }
            else if (result == GAME_RESULT_TYPE.START_PLAYER_THREEPPUK || result == GAME_RESULT_TYPE.LAST_PLAYER_THREEPPUK)
            {
                winPlayer.GameResult = 1;
                winPlayer.agent.win_count += 1;
                losePlayer.GameResult = 2;
                losePlayer.agent.win_count = 0;
                winPlayer.agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.THREE_PPUK);

                // 삼뻑 점수계산
                result_score = 7;

                result_mission = Math.Max((int)winPlayer.agent.missionscore, 1);
                result_draw = Math.Max((int)Math.Pow(2, this.drow_count), 1);
                this.drow_count = 0;
                result_push = 1;

                result_finalscore = result_score * result_mission * result_draw * result_push;

                result_threePpuck = true;

                if (isPractice == false)
                    game_result_players_money_change(winPlayer, losePlayer, this.engine.baseMoney * result_finalscore, ref result_Loser_NoMoney);
            }
            else if (result == GAME_RESULT_TYPE.DRAW)
            {
                winPlayer.GameResult = 3;
                losePlayer.GameResult = 3;
                winPlayer.agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.NAGARI);
                losePlayer.agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.NAGARI);
                // 무승부 횟수 1=2배, 2=4배, 3=8배
                if (isPractice == false)
                    if (this.drow_count < 3) // 최대 8배 까지
                    {
                        ++this.drow_count;
                    }
            }
            else
            {
                Log._log.ErrorFormat("GAME_RESULT_TYPE = unknown 비정상접근 감지 {0}\n", result);
            }

#if DEBUG
            if(losePlayer.agent.haveMoney - losePlayer.ChangeMoney != 0 && -losePlayer.ChangeMoney > this.engine.baseMoney * result_finalscore)
            {
                Log._log.Debug("A");
            }
#endif

            // 수수료 처리
            long DealerFeeMoney = 0;
            long JackPotFeeMoney = 0;
            long PlayedGameMoney = 0; // 베팅머니
            if (isPractice == false)
            {
                long ChangeMoney = winPlayer.ChangeMoney / 2;
                PlayedGameMoney = ChangeMoney;

                DealerFeeMoney = (long)(ChangeMoney * (RoomSvr.DealerFee / 100.0));
                JackPotFeeMoney = (long)(ChangeMoney * (RoomSvr.JackPotRate / 100.0));

                winPlayer.GameDealMoney = DealerFeeMoney;
                winPlayer.JackPotDealMoney = JackPotFeeMoney;
                losePlayer.GameDealMoney = DealerFeeMoney;
                losePlayer.JackPotDealMoney = JackPotFeeMoney;

                DealerFeeMoney *= 2;
                JackPotFeeMoney *= 2;

                winPlayer.ChangeMoney -= DealerFeeMoney + JackPotFeeMoney;

            }

            if (result != GAME_RESULT_TYPE.DRAW)
            {
                if (winPlayer.agent.win_count >= 3)
                {
                    winPlayer.agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.THREE_WIN);
                }
#if EVENT_JACKPOT
                if (JackPotResultType != 0)
                {
                    switch(JackPotResultType)
                    {
                        case 1: // 5광 잭팟
                            {
                                // 잭팟머니 지급 100배
                                JackPotResultMoney = this.engine.baseMoney * 100;
                            }
                            break;
                    }
                    JackPotResultPlayerId = winPlayer.data.ID;

                    winPlayer.ChangeMoney += JackPotResultMoney;

                    //this.isJackPot = false;
                }
#endif
            }

            // DB, 로그 정리
            if (isPractice == false)
            {
                foreach (var player in PlayersGaming)
                {
                    // 돈
                    player.Value.agent.money_var += player.Value.ChangeMoney;
                    player.Value.agent.addMoney(player.Value.ChangeMoney);

                    // 탑쌓기 이벤트
                    //if (player.data.topMission.Exists(o => o.isComplete == false) == false) // 모든 탑쌓기 이벤트 완료 했으면 새 탑쌓기 이벤트로 교체
                    //{
                    //    ++player.data.charm;
                    //    Data.isCompleteTopMission = true;
                    //    player.data.topMission.Clear();
                    //    Random rng = new Random(new System.DateTime().Millisecond);
                    //    List<int> templist = (new List<int>(new[] { 1, 2, 3, 4, 7, 8, 9, 10, 11, 12 })).OrderBy(o => rng.Next()).ToList();
                    //    for (int j = 0; j < 10; ++j)
                    //    {
                    //        CPlayerAgent.MissionData md = new CPlayerAgent.MissionData();
                    //        md.isComplete = false;
                    //        byte newMission = (byte)templist[j];
                    //        md.type = newMission;

                    //        player.data.topMission.Add(md);
                    //    }
                    //}
                    //else
                    //{
                    //    Data.isCompleteTopMission = false;
                    //}

                    // 유저 로그
                    gameLog.PlayerLog[player.Value.player_index].UserID = player.Value.data.ID;
                    gameLog.PlayerLog[player.Value.player_index].UserMoney = player.Value.ChangeMoney;
                    gameLog.PlayerLog[player.Value.player_index].UserCard = player.Value.agent.MakePlayerFloorLog();
                    gameLog.PlayerLog[player.Value.player_index].MissionResult = player.Value.agent.missionscore;
                }
                gameLog.Result(winPlayer.player_index, result_score);
                // 게임 로그 처리
                gameLog.Mission = (int)missionsManager.GetCurrentMission();
                if ((MISSION)gameLog.Mission == MISSION.WHALBIN)
                {
                    gameLog.MissionResult = winPlayer.agent.missionscore; // 미션이 특수미션(활빈당 미션)일 경우 승리플레이어 미션배수 기록
                }
                else
                {
                    for (int i = 0; i < gameLog.PlayerLog.Length; ++i)
                    {
                        if (gameLog.PlayerLog[i].MissionResult != 1)
                        {
                            gameLog.MissionResult = i;
                        }
                    }
                }

                string SummaryLog = "";
                // 점수 결과 요약
                {// = 1,  = 1,  = 1,  = 1,  = 1,  = 1, result_mission = 1, result_draw = 1, result_push = 1, result_chongtongnumber
                 // 로그버전
                    SummaryLog += "01_";
                    // 점수
                    // 족보점수+패점수 등 합산. 배수 곱하기 전 점수
                    SummaryLog += result_score + "_";
                    // 추가 배수
                    // 고박, 피박, 광박, 멍박, 흔들기, 고 추가배수, 무승부 밀기
                    SummaryLog += result_gobak + "_" + result_pibak + "_" + result_kwangbak + "_" + result_mungtta + "_" + result_shaking + "_" + result_go + "_" + result_draw + "_";
                    // 미션
                    // 미션 번호, 미션 성공여부, 미션 배수
                    SummaryLog += result_mission + "_";
                    // 총점
                    SummaryLog += result_finalscore + "_";
                }

                // 재현용 패순서
                CGameLog gameLogDB = (CGameLog)gameLog.End();

                int dbResult = 1;

                Task.Run(() =>
                {
                    try
                    {
                        var db = RoomSvr.db;
                        // 게임 룸 로그 저장
                        db.LogGameMatgo.Insert(PlayId: gameLogDB.LogId, RoomId: roomID, ChannelId: ChanId, RoomNumber: roomNumber, BetMoney: gameLogDB.BetMoney, UserId1: gameLogDB.PlayerLog[0].UserID, UserCard1: gameLogDB.PlayerLog[0].UserCard, UserMoney1: gameLogDB.PlayerLog[0].UserMoney, UserId2: gameLogDB.PlayerLog[1].UserID, UserCard2: gameLogDB.PlayerLog[1].UserCard, UserMoney2: gameLogDB.PlayerLog[1].UserMoney, PlayLog: gameLogDB.PlayLog, Mission: gameLogDB.Mission, MissionResult: gameLogDB.MissionResult, StartTime: gameLogDB.StratTime, EndTime: gameLogDB.EndTime, SummaryLog: SummaryLog, FirstPlayer: gameLog.FirstPlayer);

                        foreach (var player in PlayersGaming)
                        {
                            bool isDummy = player.Value.status == UserStatus.RoomPlayAuto;
                            foreach (var row in db.Room_MatgoResultPlayer(player.Value.data.ID, player.Value.ChangeMoney + player.Value.agent.inGameChangeMoney, PlayedGameMoney, player.Value.GameDealMoney, player.Value.GameResult, (int)ChanType, RoomSvr.DealerFee, ChanId, isDummy))
                            {
                                player.Value.data.UserLevel = row.MemberLevel;
                                player.Value.data.GameLevel = row.GameLevel;
                                if (ChanType == ChannelType.Free)
                                {
                                    player.Value.data.money_free = (long)row.GameMoney;
                                    player.Value.agent.setMoney(player.Value.data.money_free);
                                }
                                else
                                {
                                    player.Value.data.money_pay = (long)row.PayMoney;
                                    player.Value.agent.setMoney(player.Value.data.money_pay);
                                }
                            }
                            db.LogGamePlayer.Insert(UserId: player.Value.data.ID, ShopID: player.Value.data.shopId, GameId: RoomSvr.GameId, ChannelId: ChanId, RoomId: roomNumber, PlayId: gameLogDB.LogId, Result: player.Value.GameResult, ChangeMoney: player.Value.ChangeMoney + player.Value.agent.inGameChangeMoney, AfterMoney: player.Value.agent.haveMoney, Time: gameLogDB.EndTime, GameDealMoney: player.Value.GameDealMoney, JackPotDealMoney: player.Value.JackPotDealMoney, BetMoney: PlayedGameMoney, BaseMoney: BaseMoney);
                        }

                        // 게임 정보 처리
                        db.Room_MatgoResultGame(ChanId, PlayedGameMoney, DealerFeeMoney, JackPotFeeMoney, (int)ChanType, JackPotResultMoney);

                        if (JackPotResultMoney != 0)
                        {
                            // 잭팟 로그 기록
                            db.LogJackPot.Insert(GameID: RoomSvr.GameId, ChannelId: ChanId, RoomNumber: roomNumber, PlayId: gameLogDB.LogId, JackPotType: JackPotResultType, BetMoney: gameLogDB.BetMoney, JackPotMoney: JackPotResultMoney, UserId: JackPotResultPlayerId);
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorLog(e);
                        Log._log.ErrorFormat("{0} 게임 결과 저장 : 예외발생 {1}", gameLogDB.LogId, e.ToString());
                        dbResult = 0;
                    }

                    try
                    {
                        this.roomWaitTime = DateTime.Now.AddSeconds(10);
                        this.status = RoomStatus.Stay;
                        foreach (var player in PlayersGaming)
                        {
                            if (player.Value.status == UserStatus.RoomPlayAuto) continue;
                            player.Value.status = UserStatus.RoomStay;
                        }

                        // 플레이어 정보 갱신
                        send_players_info();
                        // 게임 결과 전송
                        int jackPotMoney = 0;
                        int jackPotReward = 0;
                        if (JackPotResultType == 1)
                        {
                            jackPotMoney = this.engine.baseMoney;
                            jackPotReward = 100;
                        }
                        foreach (var player in PlayersGaming)
                        {
                            CMessage newmsg = new CMessage();

                            byte is_win = 0;
                            bool result_noMoney = false;

                            if (result == GAME_RESULT_TYPE.DRAW)
                            {
                                is_win = 0;
                            }
                            else if (winPlayer.player_index == player.Value.player_index)
                            {
                                is_win = 1;
                            }
                            else if (losePlayer.player_index == player.Value.player_index)
                            {
                                is_win = 2;
                                if (result_Loser_NoMoney) result_noMoney = true;
                            }

                            // 게임 결과 전송
                            Rmi.Marshaler.Write(newmsg, (byte)is_win);                  // 승패 여부. 0:무승부(나가리), 1:승, 2:패
                            Rmi.Marshaler.Write(newmsg, (int)this.engine.baseMoney);    // 판돈
                            Rmi.Marshaler.Write(newmsg, (int)result_finalscore);        // 최종 점수
                            Rmi.Marshaler.Write(newmsg, (int)result_score);             // 패 점수
                            Rmi.Marshaler.Write(newmsg, (int)result_gobak);             // 고박 배수
                            Rmi.Marshaler.Write(newmsg, (int)result_pibak);             // 피박 배수
                            Rmi.Marshaler.Write(newmsg, (int)result_kwangbak);          // 광박 배수
                            Rmi.Marshaler.Write(newmsg, (int)result_mungtta);           // 멍따 배수
                            Rmi.Marshaler.Write(newmsg, (int)result_shaking);           // 흔들기 배수(흔들기 + 폭탄)
                            Rmi.Marshaler.Write(newmsg, (int)result_go);                // 고 배수
                            Rmi.Marshaler.Write(newmsg, (int)result_mission);           // 미션 배수
                            Rmi.Marshaler.Write(newmsg, (int)result_draw);              // 무승부 배수
                            Rmi.Marshaler.Write(newmsg, (int)result_push);              // 밀기 배수
                            Rmi.Marshaler.Write(newmsg, (bool)false);                   // 밀기 가능여부. 미사용.
                            Rmi.Marshaler.Write(newmsg, (int)result_chongtongnumber);   // 총통 패 번호. 12:없음, 0~11
                            Rmi.Marshaler.Write(newmsg, (bool)result_threePpuck);       // 3뻑 여부

                            Rmi.Marshaler.Write(newmsg, (long)winPlayer.ChangeMoney);      // 승리플레이어 머니
                            Rmi.Marshaler.Write(newmsg, (long)losePlayer.ChangeMoney);       // 패배플레이어 머니
                            Rmi.Marshaler.Write(newmsg, (long)DealerFeeMoney + JackPotFeeMoney);      // 딜러비

                            Rmi.Marshaler.Write(newmsg, (bool)result_noMoney);          // 올인, 또는 판돈 부족이므로 퇴장시킴

                            Rmi.Marshaler.Write(newmsg, (int)jackPotMoney);             // 잭팟 머니(판돈)
                            Rmi.Marshaler.Write(newmsg, (int)jackPotReward);            // 잭팟 배율(100배)

                            Rmi.Marshaler.Write(newmsg, (short)score1);            // 잭팟 배율(200배)
                            Rmi.Marshaler.Write(newmsg, (short)score2);            // 잭팟 배율(200배)
                            Rmi.Marshaler.Write(newmsg, (short)score3);            // 잭팟 배율(200배)
                            Rmi.Marshaler.Write(newmsg, (short)score4);            // 잭팟 배율(200배)
                            Rmi.Marshaler.Write(newmsg, (short)score5);            // 잭팟 배율(200배)

                            Rmi.Marshaler.Write(newmsg, (bool)true); // 게임시작 가능
                            Rmi.Marshaler.Write(newmsg, (int)JackPotResultType); // 잭팟 타입. 1: 5광 잭팟
                            
                            Send(player.Value, newmsg, SS.Common.GameOver);
                            if (player.Value.isPracticeDummy)
                            {
                                foreach (var playerO in PlayersObserve)
                                {
                                    if (playerO == null) continue;
                                    Send(playerO, newmsg, SS.Common.GameOver);
                                }
                            }
                        }

                        if (JackPotResultMoney != 0)
                        {
                            // 잭팟 공지
                            //Proxy.RoomLobbyEventEnd((RemoteID)this.remote_lobby, CPackOption.Basic, this.roomID, JackPotResultType, winPlayer.data.nickName, JackPotResultMoney);
                        }

                        RoomSvr.DummyPlayerLeave(this);
                        if (dbResult != 1)
                        {
                            Log._log.FatalFormat("{0}채널 {1}번방 게임결과 처리 실패", ChanId, roomNumber);
                            foreach (var player in PlayersGaming)
                            {
                                Log._log.FatalFormat("{0}채널 {1}번방 게임결과 처리 실패 유저 {2}: ", ChanId, roomNumber, player.Value.data.userID);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log._log.FatalFormat("GameEnd Error : {0}", e.ToString());
                    }
                    PlayersGaming.Clear();
                    for (int i = 0; i < PlayersObserve.Count(); ++i)
                    {
                        PlayersObserve[i] = null;
                    }
                });
            }
            else
            {
                // 연습게임 초기화
                this.status = RoomStatus.Stay;
                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.RoomPlayAuto) continue;
                    player.Value.status = UserStatus.RoomStay;
                }

                //CPlayer Dummy = null;
                CPlayer PracticePlayer = null;
                if (winPlayer.isPracticeDummy)
                {
                    PracticePlayer = losePlayer;
                }
                else if (losePlayer.isPracticeDummy)
                {
                    PracticePlayer = winPlayer;
                }

                {
                    CMessage newmsg = new CMessage();

                    byte is_win = 0;
                    bool result_noMoney = false;

                    if (result == GAME_RESULT_TYPE.DRAW)
                    {
                        is_win = 0;
                    }
                    else if (winPlayer.player_index == PracticePlayer.player_index)
                    {
                        is_win = 1;
                    }
                    else if (losePlayer.player_index == PracticePlayer.player_index)
                    {
                        is_win = 2;
                        if (result_Loser_NoMoney) result_noMoney = true;
                    }

                    // 게임 결과 전송
                    Rmi.Marshaler.Write(newmsg, (byte)is_win);                  // 승패 여부. 0:무승부(나가리), 1:승, 2:패
                    Rmi.Marshaler.Write(newmsg, (int)this.engine.baseMoney);    // 판돈
                    Rmi.Marshaler.Write(newmsg, (int)result_finalscore);        // 최종 점수
                    Rmi.Marshaler.Write(newmsg, (int)result_score);             // 패 점수
                    Rmi.Marshaler.Write(newmsg, (int)result_gobak);             // 고박 배수
                    Rmi.Marshaler.Write(newmsg, (int)result_pibak);             // 피박 배수
                    Rmi.Marshaler.Write(newmsg, (int)result_kwangbak);          // 광박 배수
                    Rmi.Marshaler.Write(newmsg, (int)result_mungtta);           // 멍따 배수
                    Rmi.Marshaler.Write(newmsg, (int)result_shaking);           // 흔들기 배수(흔들기 + 폭탄)
                    Rmi.Marshaler.Write(newmsg, (int)result_go);                // 고 배수
                    Rmi.Marshaler.Write(newmsg, (int)result_mission);           // 미션 배수
                    Rmi.Marshaler.Write(newmsg, (int)result_draw);              // 무승부 배수
                    Rmi.Marshaler.Write(newmsg, (int)result_push);              // 밀기 배수
                    Rmi.Marshaler.Write(newmsg, (bool)false);                   // 밀기 가능여부. 미사용.
                    Rmi.Marshaler.Write(newmsg, (int)result_chongtongnumber);   // 총통 패 번호. 12:없음, 0~11
                    Rmi.Marshaler.Write(newmsg, (bool)result_threePpuck);       // 3뻑 여부

                    Rmi.Marshaler.Write(newmsg, (long)winPlayer.ChangeMoney);      // 승리플레이어 머니
                    Rmi.Marshaler.Write(newmsg, (long)losePlayer.ChangeMoney);       // 패배플레이어 머니
                    Rmi.Marshaler.Write(newmsg, (long)DealerFeeMoney + JackPotFeeMoney);      // 딜러비

                    Rmi.Marshaler.Write(newmsg, (bool)result_noMoney);          // 올인, 또는 판돈 부족이므로 퇴장시킴

                    Rmi.Marshaler.Write(newmsg, (int)0);             // 잭팟 머니(판돈)
                    Rmi.Marshaler.Write(newmsg, (int)0);            // 잭팟 배율(200배)

                    Rmi.Marshaler.Write(newmsg, (short)score1);            // 잭팟 배율(200배)
                    Rmi.Marshaler.Write(newmsg, (short)score2);            // 잭팟 배율(200배)
                    Rmi.Marshaler.Write(newmsg, (short)score3);            // 잭팟 배율(200배)
                    Rmi.Marshaler.Write(newmsg, (short)score4);            // 잭팟 배율(200배)
                    Rmi.Marshaler.Write(newmsg, (short)score5);            // 잭팟 배율(200배)

                    if (PlayersConnect.Count == max_users)
                    {
                        Rmi.Marshaler.Write(newmsg, (bool)true); // 게임시작 준비
                    }
                    else
                    {
                        Rmi.Marshaler.Write(newmsg, (bool)false); // 연습게임 안내
                    }

                    Send(PracticePlayer, newmsg, SS.Common.GameOver);
                    foreach (var playerO in PlayersObserve)
                    {
                        if (playerO == null) continue;
                        Send(playerO, newmsg, SS.Common.GameOver);
                    }
                }
                PlayersGaming.Clear();
                for (int i = 0; i < PlayersObserve.Count(); ++i)
                {
                    PlayersObserve[i] = null;
                }
            }
        }
        void send_player_all_in(CPlayer player)
        {
            CMessage newmsg = new CMessage();
            Send(player, newmsg, SS.Common.GameKickUser);
        }
        void game_result_players_money_change(CPlayer winPlayer, CPlayer losePlayer, long money_result, ref bool is_all_in)
        {
            // 자유 채널은 결과머니 제한규칙 없음
            if (this.ChanFree == false)
            {
                // 승자는 자신이 보유했던 게임머니 이상 못가져감
                if (money_result > winPlayer.agent.haveMoney)
                {
                    money_result = winPlayer.agent.haveMoney;
                }
            }

            // 배팅 한도
            /*
            if (money_result < BetingLimite)
            {
                money_result = BetingLimite;
            }
            */

            if (money_result > losePlayer.agent.haveMoney)
            {
                money_result = losePlayer.agent.haveMoney;
                is_all_in = true;
            }
            else if (losePlayer.agent.haveMoney < BaseMoney * 7)
            {
                is_all_in = true;
            }

            winPlayer.ChangeMoney += money_result;
            winPlayer.data.winCount += 1;

            losePlayer.ChangeMoney -= money_result;
            losePlayer.data.loseCount += 1;
        }
        void send_player_statistics()
        {
            // 점수 계산 후 정보 전송
            this.engine.Calculate_Players_Score(drow_count);

            // 모든 플레이어에게 각자 정보 전송
            foreach (var player in PlayersGaming)
            {
                foreach (var player_ in PlayersGaming)
                {
                    CPlayerAgent target_player = player_.Value.agent;

                    CMessage newmsg = new CMessage();
                    Rmi.Marshaler.Write(newmsg, target_player.player_index);                                 // 플레이어 인덱스
                    Rmi.Marshaler.Write(newmsg, target_player.score);               // 점수
                    Rmi.Marshaler.Write(newmsg, target_player.go_count);            // 고 횟수
                    Rmi.Marshaler.Write(newmsg, target_player.shaking_count);       // 흔들기 횟수
                    Rmi.Marshaler.Write(newmsg, target_player.ppuk_count);          // 뻑 횟수
                    Rmi.Marshaler.Write(newmsg, target_player.get_pee_count());     // 피 장수
                    Rmi.Marshaler.Write(newmsg, target_player.pibak);               // 피박 여부
                    Rmi.Marshaler.Write(newmsg, target_player.kwangbak);            // 광박 여부
                    Rmi.Marshaler.Write(newmsg, target_player.multiple); // 현재 배수

                    Send(player.Value, newmsg, SS.Common.GameNotifyStat);
                    if (player.Value.isPracticeDummy)
                    {
                        foreach (var playerO in PlayersObserve)
                        {
                            if (playerO == null) continue;
                            Send(playerO, newmsg, SS.Common.GameNotifyStat);
                        }
                    }
                }

                //if (player.Value.agent.mission_update == true)
                //{
                //    player.Value.agent.mission_update = false;
                //    CMessage Msg = new CMessage();
                //    PacketType msgID_ = (PacketType)Rmi.Common.SC_EVENT_TAPSSAHGI_INFO;
                //    CPackOption pkOption_ = CPackOption.Basic;
                //    Msg.WriteStart(msgID_, pkOption_, 0, true);

                //    //byte count = 1;
                //    //for (int j = 0; j < player.agent.topMission.Count; ++j)
                //    //{
                //    //    if (player.agent.topMission[j].isComplete == false)
                //    //        break;

                //    //    ++count;
                //    //}
                //    //if (count > 10) count = 10;
                //    //Rmi.Marshaler.Write(Msg, (byte)count);          // 탑쌓기 이벤트 수
                //    Rmi.Marshaler.Write(Msg, (byte)0);          // 탑쌓기 이벤트 수

                //    //for (int j = 0; j < count; ++j)
                //    //{
                //    //    Rmi.Marshaler.Write(Msg, (byte)player.agent.topMission[j].type);       // 탑쌓기 이벤트 타입
                //    //}
                //    Rmi.Marshaler.Write(Msg, (int)player.Value.data.charm);          // 부적 수
                //    send.PacketSend(player.Value, pkOption_, Msg, msgID_, this);
                //}
            }
        }
        void CS_PUSH(CPlayer owner, CMessage rm)
        {
            // do notting.
        }
        void CS_PRACTICE_GAME(CPlayer owner, CMessage rm)
        {
            if (this.status != RoomStatus.Stay) return;

            if (PlayersConnect.Count > 1) return;

            GameStartPractice(owner);
        }
        public void PracticeGameEnd()
        {
            if (status == RoomStatus.PracticeGamePlay)
            {
                status = RoomStatus.Stay;
                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.RoomPlayAuto) continue;
                    player.Value.status = UserStatus.RoomStay;
                }
                PlayersGaming.Clear();
                PlayersObserve[0] = null;
            }

            CMessage newmsg = new CMessage();
            foreach (var player in PlayersConnect)
            {
                Send(player.Value, newmsg, SS.Common.GamePracticeEnd);
            }
        }
        void CS_PLAYER_ORDER_START(CPlayer owner, CMessage rm)
        {
            clear_received_protocol();

            foreach (var playerO in PlayersObserve)
            {
                if (playerO == null) continue;
                if (playerO == owner) return;
            }
            //clear_received_protocol();
            if (PlayersGaming.Count != max_users)
            {
                Log._log.Error("CS_PLAYER_ORDER_START.");
            }

            // 선잡기 패 선택
            byte position;
            Rmi.Marshaler.Read(rm, out position);
            if (position < 0 || position > 7) // 패킷 예외처리
            {
                position = 0;
            }

            // 플레이어 모두 준비되면 처리
            if (owner.agent.status != UserGameStatus.Play) return;
            owner.agent.status = UserGameStatus.OrderCard;

            if (position == this_player_next(owner.player_index).agent.order_position)
            {
                if (position == 0)
                    position = 5;
                else
                    --position;
            }
            owner.agent.order_position = position;
            send_order_start(owner.player_index, owner.agent.order_position);

            foreach (var player in PlayersGaming)
            {
                if (player.Value.agent.status != UserGameStatus.OrderCard) return;
            }
            foreach (var player in PlayersGaming)
            {
                player.Value.agent.status = UserGameStatus.Play;
            }

            // 선 결정
            byte head_player_index = 0;

            //head_player_index = this_player(0).player_index;

            byte best_number = 0;
            for (byte i = 0; i < this.order_manager.order_cards.Count; ++i)
            {
                CCard card = this.order_manager.order_cards[i];
                if (best_number < card.number)
                {
                    head_player_index = i;
                    best_number = card.number;
                }
            }

            // 선플레이어 기록. 0번 플레이어가 선이면 true
            if (0 == head_player_index)
                gameLog.FirstPlayer = true;
            else
                gameLog.FirstPlayer = false;

            // 선 결과 정보 전송
            send_orderCard_to_player(head_player_index);

            // 선이 정해지면 선부터 카드를 분배한다.
            this.engine.current_player_index = head_player_index;
            this.engine.start_player = head_player_index;

            bool FirstPlayerPick = false;
            CPlayer FirstPlayer = this_player(head_player_index);
            CPlayer NextPlayer = this_player_next(head_player_index);
            // 보너스 카드 보정 (밀기)
            if (RoomSvr.IsPush(FirstPlayer.data.GameLevel, FirstPlayer.data.UserLevel, NextPlayer.data.GameLevel, NextPlayer.data.UserLevel, ref FirstPlayerPick))
            {
                if (FirstPlayerPick)
                {
                    FirstPlayer.agent.PushBonus = true;
                    //NextPlayer.agent.PenaltyType1 = true;

                    //FirstPlayer.agent.Advantage = true;
                    //FirstPlayer.agent.Penalty = false;

                    //NextPlayer.agent.Penalty = true;
                    //NextPlayer.agent.Advantage = false;
                }
                else
                {
                    NextPlayer.agent.PushBonus = true;
                    //FirstPlayer.agent.PenaltyType1 = true;

                    //NextPlayer.agent.Advantage = true;
                    //NextPlayer.agent.Penalty = false;

                    //FirstPlayer.agent.Penalty = true;
                    //FirstPlayer.agent.Advantage = false;
                }
            }

            this.engine.MatgoStart();

            // 미션 부여
            this.missionsManager.NewMission(RoomSvr.MissionRate);
            this.missionsManager.SetMissionCard();

            // 미션 안전장치
            // 강매 VS 유저. 유저가 미션패를 전부 들고 있으면 미션을 다른 걸로 교체 (50%)
            if (rand.NextDouble() <= 1.0)
            {
                CPlayer CardCheckPlayer = null;
                // 플레이어 확인
#if DEBUG
                if (FirstPlayer.data.shopId == 11 && NextPlayer.data.shopId != 11)
                {
                    CardCheckPlayer = NextPlayer;
                }
                else if (NextPlayer.data.shopId == 11 && FirstPlayer.data.shopId != 11)
                {
                    CardCheckPlayer = FirstPlayer;
                }
#else
                if (FirstPlayer.data.shopId == 432 && NextPlayer.data.shopId != 432)
                {
                    CardCheckPlayer = NextPlayer;
                }
                else if (NextPlayer.data.shopId == 432 && FirstPlayer.data.shopId != 432)
                {
                    CardCheckPlayer = FirstPlayer;
                }
#endif

                if (CardCheckPlayer != null && this.missionsManager.missionCards.Count > 0)
                {
                    // 카드 확인
                    bool allMatching = true;
                    var CardCheckCards = CardCheckPlayer.agent.GetPlayerHandsCards();
                    foreach (var card in this.missionsManager.missionCards)
                    {
                        // 카드가 없으면 무효
                        if(CardCheckCards.Exists(x => x.is_same(card.number, card.position)) == false)
                        {
                            allMatching = false;
                        }
                    }

                    if(allMatching == true)
                    {
                        this.missionsManager.NewMission(RoomSvr.MissionRate, this.missionsManager.currentMission);
                        this.missionsManager.SetMissionCard();
                    }
                }
            }

            gameLog.Start(FirstPlayer.agent.GetPlayerHandsCards(), NextPlayer.agent.GetPlayerHandsCards(), engine.floor_manager.get_begin_cards());

            // 미션 확인 적용
            this.missionsManager.CheckMission(this.PlayersGaming);

            // 카드 분배하기
            foreach (var player in PlayersGaming)
            {
                player.Value.agent.status = UserGameStatus.DistributeCard;
                send_distributeCard_to_player(player.Value);
            }
        }
        void GameStartPractice(CPlayer owner)
        {
            // 더미 플레이어 생성
            byte playerIndex;
            if (owner.player_index == 1)
                playerIndex = 0;
            else
                playerIndex = 1;

            CPlayer Dummy = new CPlayer();
            Dummy.agent = new CPlayerAgent(playerIndex, this);
            {
                Dummy.isPracticeDummy = true;
                Dummy.player_index = playerIndex;
                Dummy.data.userID = "PracticeBot";
                Dummy.data.nickName = "연습중";
                Dummy.agent.setMoney(0);
                Dummy.agent.money_var = 0;
                Dummy.data.avatar = "avatar_0001";
                Dummy.data.winCount = 0;
                Dummy.data.loseCount = 0;
                Dummy.data.voice = 1;
            }

            PlayersGaming.Clear();
            PlayersGaming.TryAdd(owner.player_index, owner);
            PlayersGaming.TryAdd(playerIndex, Dummy);

            this.status = RoomStatus.PracticeGamePlay;

            // 초기화
            reset();

            Dummy.agent.setDummyPlayer();

            foreach (var player in PlayersGaming)
            {
                player.Value.agent.status = UserGameStatus.Play;
            }

            // 연습게임은 방장이 선
            byte head_player_index = owner.player_index;
            // 선플레이어 기록player.player_index
            if (0 == head_player_index)
                gameLog.FirstPlayer = true;
            else
                gameLog.FirstPlayer = false;

            CPlayer FirstPlayer = this_player(head_player_index);
            CPlayer NextPlayer = this_player_next(head_player_index);
            {
                FirstPlayer.agent.PushBonus = false;
                NextPlayer.agent.PushBonus = false;
            }

            // 선이 정해지면 선부터 카드를 분배한다.
            this.engine.current_player_index = head_player_index;
            this.engine.start_player = head_player_index;
            this.engine.IsPractice = true;
            this.engine.MatgoStart();

            gameLog.Start(FirstPlayer.agent.GetPlayerHandsCards(), NextPlayer.agent.GetPlayerHandsCards(), engine.floor_manager.get_begin_cards());

            // 미션 없음
            this.missionsManager.resetNone();

            //this.missionsManager.CheckMission(this.PlayersGaming);

            //카드 분배하기
            foreach (var player in PlayersGaming)
            {
                player.Value.agent.status = UserGameStatus.DistributeCard;
                send_distributeCard_to_player(player.Value);
            }
        }
        void send_order_start(byte player_index, byte number)
        {
            foreach (var player in PlayersGaming)
            {
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, player_index);       // 선잡기한 플레이어의 인덱스
                Rmi.Marshaler.Write(newmsg, number);     // 선잡기한 패의 위치번호
                Send(player.Value, newmsg, SS.Common.GameRequestSelectOrder);
            }
        }
        void send_orderCard_to_player(byte head)
        {
            byte tail = this_player_next(head).player_index;

            foreach (var player in PlayersGaming)
            {
                CMessage Msg = new CMessage();
                Rmi.Marshaler.Write(Msg, (byte)player.Value.player_index);
                Rmi.Marshaler.Write(Msg, (byte)head);

                Rmi.Marshaler.Write(Msg, this.order_manager.order_cards[head].number);
                Rmi.Marshaler.Write(Msg, (byte)this.order_manager.order_cards[head].pae_type);
                Rmi.Marshaler.Write(Msg, this.order_manager.order_cards[head].position);
                Rmi.Marshaler.Write(Msg, this_player(head).agent.order_position);

                Rmi.Marshaler.Write(Msg, this.order_manager.order_cards[tail].number);
                Rmi.Marshaler.Write(Msg, (byte)this.order_manager.order_cards[tail].pae_type);
                Rmi.Marshaler.Write(Msg, this.order_manager.order_cards[tail].position);
                Rmi.Marshaler.Write(Msg, this_player(tail).agent.order_position);

                if (isJackPot)
                {
                    Rmi.Marshaler.Write(Msg, "event_card"); // 잭팟이면 잭팟 카드 전송
                }
                else
                {
                    Rmi.Marshaler.Write(Msg, this_player(head).data.avatar_card); // 선 플레이어의 카드 스킨으로 게임진행
                }

                Send(player.Value, Msg, SS.Common.GameOrderEnd);
            }
        }
        void send_distributeCard_to_player(CPlayer player)
        {
            CMessage newmsg = new CMessage();

            //바닥카드
            byte floor_count = (byte)this.engine.distributed_floor_cards.Count;
            Rmi.Marshaler.Write(newmsg, floor_count);
            for (int i = 0; i < floor_count; ++i)
            {
                Rmi.Marshaler.Write(newmsg, this.engine.distributed_floor_cards[i].number);
                Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_floor_cards[i].pae_type);
                Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_floor_cards[i].position);
            }
            //각 플레이어 카드
            Rmi.Marshaler.Write(newmsg, (byte)2);
            foreach (var player_ in PlayersGaming)
            {
                byte pi = player_.Value.player_index;
                byte players_card_count = (byte)this.engine.distributed_players_cards[pi].Count;
                Rmi.Marshaler.Write(newmsg, pi);
                Rmi.Marshaler.Write(newmsg, players_card_count);

                // 플레이어 본인의 카드정보만 실제 카드로 보내주고,
                // 다른 플레이어의 카드는 null카드로 보내줘서 클라이언트딴에서는 알지 못하게 한다.
                if (player.player_index == pi)
                {
                    for (int card_index = 0; card_index < players_card_count; ++card_index)
                    {
                        Rmi.Marshaler.Write(newmsg, this.engine.distributed_players_cards[pi][card_index].number);
                        Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[pi][card_index].pae_type);
                        Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[pi][card_index].position);
                    }
                }
                else
                {
                    for (int card_index = 0; card_index < players_card_count; ++card_index)
                    {
                        // 다른 플레이어의 카드는 null카드로 보내준다.
                        Rmi.Marshaler.Write(newmsg, byte.MaxValue);
                    }
                }
            }

            this.currentMission = missionsManager.GetCurrentMission();
            List<CCard> missioncards = new List<CCard>();
            if (this.currentMission >= MISSION.WOL1_2 && this.currentMission <= MISSION.WOL12_2)
            {
                missioncards = missionsManager.missionCards;
            }
            Rmi.Marshaler.Write(newmsg, (byte)currentMission);
            Rmi.Marshaler.Write(newmsg, (byte)missioncards.Count);
            if (missioncards.Count > 0)
            {
                for (int i = 0; i < missioncards.Count; ++i)
                {
                    Rmi.Marshaler.Write(newmsg, missioncards[i].number);
                    Rmi.Marshaler.Write(newmsg, (byte)missioncards[i].pae_type);
                    Rmi.Marshaler.Write(newmsg, missioncards[i].position);
                }
            }

            Send(player, newmsg, SS.Common.GameDistributedStart);
        }
        bool CheckChongtong(CPlayer currentPlayer)
        {
            // 바닥 확인
            byte number_floor;
            if (this.engine.is_floor_chongtong(out number_floor))
            {
                GAME_RESULT_TYPE result;
                if (this.engine.start_player == currentPlayer.player_index)
                {
                    result = GAME_RESULT_TYPE.START_PLAYER_CHONGTONG;
                }
                else
                {
                    result = GAME_RESULT_TYPE.LAST_PLAYER_CHONGTONG;
                }
                currentPlayer.agent.set_chongtong(number_floor);
                gameEnd(result, currentPlayer);
                return true;
            }
            else
            {
                // 플레이어 손에서 확인
                foreach (var player in PlayersGaming)
                {
                    byte number;
                    if (this.engine.is_player_chongtong(player.Value.player_index, out number))
                    {
                        GAME_RESULT_TYPE result;
                        if (this.engine.start_player == player.Value.player_index)
                        {
                            result = GAME_RESULT_TYPE.START_PLAYER_CHONGTONG;
                        }
                        else
                        {
                            result = GAME_RESULT_TYPE.LAST_PLAYER_CHONGTONG;
                        }

                        player.Value.agent.set_chongtong(number);
                        gameEnd(result, player.Value);
                        return true;
                    }
                }
            }

            return false;
        }
        void send_event_jackpot_info(CPlayer owner)
        {
            // 잭팟
            CMessage Msg = new CMessage();
            Rmi.Marshaler.Write(Msg, (long)this.RoomSvr.JackPotMoney);
            Send(owner, Msg, SS.Common.GameEventInfo);
        }
        void send_event_tapssahgi_info(CPlayer owner)
        {
            //CMessage Msg = new CMessage();
            //PacketType msgID = (PacketType)Rmi.Common.SC_EVENT_TAPSSAHGI_INFO;
            //ZNet.CPackOption pkOption = ZNet.CPackOption.Basic;
            //Msg.WriteStart(msgID, pkOption, 0, true);

            ////byte count = 1;
            ////for (int i = 0; i < owner.agent.topMission.Count; ++i)
            ////{
            ////    if (owner.agent.topMission[i].isComplete == false)
            ////        break;

            ////    ++count;
            ////}
            ////if (count > 10) count = 10;
            ////Rmi.Marshaler.Write(Msg, (byte)count);          // 탑쌓기 이벤트 수
            //Rmi.Marshaler.Write(Msg, (byte)0);          // 탑쌓기 이벤트 수

            ////for (int i = 0; i < count; ++i)
            ////{
            ////    Rmi.Marshaler.Write(Msg, (byte)owner.agent.topMission[i].type);       // 탑쌓기 이벤트 타입
            ////}
            //Rmi.Marshaler.Write(Msg, (int)owner.data.charm);          // 부적 수
            //send.PacketSend(owner, pkOption, Msg, msgID, this);
        }
        void send_players_info_room_in()
        {
            int playerCount = PlayersConnect.Count;
            if (playerCount == 1)
            {
                CPlayer player = PlayersConnect.First().Value;

                CMessage Msg = new CMessage();

                Rmi.Marshaler.Write(Msg, player.player_index);

                Rmi.Marshaler.UserInfo userinfo = new Rmi.Marshaler.UserInfo();
                userinfo.userID = player.data.userID;
                userinfo.nickName = player.data.nickName;
                userinfo.money_game = player.agent.haveMoney;
                userinfo.money_var = player.agent.money_var;
                userinfo.avatar = player.data.avatar;
                userinfo.win = player.data.winCount;
                userinfo.lose = player.data.loseCount;
                userinfo.voice = player.data.voice;
                Rmi.Marshaler.Write(Msg, userinfo);

                Rmi.Marshaler.Write(Msg, false);

                Send(player, Msg, SS.Common.GameUserInfo);
            }
            else if (playerCount == 2)
            {
                bool bell = false;
                foreach (var player in PlayersConnect)
                {
                    foreach (var player_ in PlayersConnect)
                    {
                        CMessage Msg = new CMessage();
                        Rmi.Marshaler.Write(Msg, player_.Value.player_index);

                        Rmi.Marshaler.UserInfo userinfo = new Rmi.Marshaler.UserInfo();
                        userinfo.userID = player_.Value.data.userID;
                        userinfo.nickName = player_.Value.data.nickName;
                        userinfo.money_game = player_.Value.agent.haveMoney;
                        userinfo.money_var = player_.Value.agent.money_var;
                        userinfo.avatar = player_.Value.data.avatar;
                        userinfo.win = player_.Value.data.winCount;
                        userinfo.lose = player_.Value.data.loseCount;
                        userinfo.voice = player_.Value.data.voice;
                        Rmi.Marshaler.Write(Msg, userinfo);

                        // 방장은 띵동 소리나도록 처리
                        if (player.Value.Operator == true && bell == false)
                        {
                            Rmi.Marshaler.Write(Msg, true);
                            bell = true;
                        }
                        else
                        {
                            Rmi.Marshaler.Write(Msg, false);
                        }

                        Send(player.Value, Msg, SS.Common.GameUserInfo);
                    }
                }
            }
        }
        void send_players_info()
        {
            foreach (var player in PlayersConnect)
            {
                foreach (var player_ in PlayersConnect)
                {
                    CMessage Msg = new CMessage();

                    Rmi.Marshaler.Write(Msg, player_.Value.player_index);

                    Rmi.Marshaler.UserInfo userinfo = new Rmi.Marshaler.UserInfo();
                    userinfo.userID = player_.Value.data.userID;
                    userinfo.nickName = player_.Value.data.nickName;
                    userinfo.money_game = player_.Value.agent.haveMoney;
                    userinfo.money_var = player_.Value.agent.money_var;
                    userinfo.avatar = player_.Value.data.avatar;
                    userinfo.win = player_.Value.data.winCount;
                    userinfo.lose = player_.Value.data.loseCount;
                    userinfo.voice = player_.Value.data.voice;
                    Rmi.Marshaler.Write(Msg, userinfo);
                    Rmi.Marshaler.Write(Msg, false);

                    Send(player.Value, Msg, SS.Common.GameUserInfo);
                }
            }
        }
        #endregion

        #region BROADCAST
        public CPlayer Check_Player_Active()
        {
            // 플레이어의 마지막 행동이 제한시간을 넘겼을 경우 더미 플레이어로 전환시킴
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status != UserStatus.RoomPlay || player.Value.status == UserStatus.None) continue;
                if (player.Value.agent.currentMsg == null) continue;

                if (player.Value.agent.actionTimeLimit < DateTime.Now.Ticks)
                {
                    Log._log.ErrorFormat("player.Value.currentMsg == {0}", player.Value.agent.currentMsg.mRmiID);
                    return player.Value;
                }
            }

            return null;
        }
        public void ProcessAutoPlay()
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.RoomPlayAuto)
                {
                    PacketProcess(player.Value);
                }
            }
        }
        #endregion BROADCAST
        /// <summary>
        /// ai플레이일 경우 딜레이 값을 넣어줘서 너무 빨리 진행되지 않도록 한다.
        /// </summary>
        /// <param name="current_player"></param>
        /// <returns></returns>
        byte get_aiplayer_delay(CPlayer current_player)
        {
            byte delay = 0;
            //if (current_player.is_autoplayer())
            {
                delay = 1;
            }

            return delay;
        }
        CPlayer this_player(byte player_index)
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.player_index == player_index)
                {
                    return player.Value;
                }
            }

            return null;
        }
        CPlayer this_player_next(byte player_index)
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.player_index != player_index)
                {
                    return player.Value;
                }
            }

            return null;
        }
        CPlayer current_player()
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.player_index == this.engine.current_player_index)
                {
                    return player.Value;
                }
            }

            return null;
        }
        CPlayer current_player_next()
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.player_index != this.engine.current_player_index)
                {
                    return player.Value;
                }
            }

            return null;
        }

        public void SaveCardLog()
        {
            Log._log.Info(engine.CardLog);
        }

    }
    public class MessageTemp
    {
        public CMessage Msg;
        public PacketType mRmiID;

        public MessageTemp(CMessage Msg_, PacketType mRmiID_)
        {
            Msg = Msg_;
            Msg.ResetPosition();
            mRmiID = mRmiID_;
        }
    }
    public class MessagePacket
    {
        public CPlayer player;
        public CMessage Msg;
        public PacketType mRmiID;

        public MessagePacket(CPlayer player_, CMessage Msg_, PacketType mRmiID_)
        {
            player = player_;
            Msg = Msg_;
            mRmiID = mRmiID_;
        }
    }
    public class MessagePacketRelay
    {
        public RemoteID remote;
        public CMessage Msg;
        public PacketType mRmiID;

        public MessagePacketRelay(RemoteID remote_, CMessage Msg_, PacketType mRmiID_)
        {
            remote = remote_;
            Msg = Msg_;
            mRmiID = mRmiID_;
        }
    }
    public enum RoomStatus : byte
    {
        None,
        Stay,       // 대기중
        GamePlay,   // 진행중
        PracticeGamePlay,   // 연습게임 진행중
    }
}
