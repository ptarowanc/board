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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Server.Engine
{
    public class CGameRoom
    {
        #region Value
        public object Locker = new object();
        Random rnd;
        public ConcurrentQueue<MessagePacket> waiting_packets;
        public Thread Thread_sequential_packet_handler;
        public bool isRun = true;
        public RoomServer RoomSvr;
        public int ChanId = -1;
        public ChannelKind ChanKind = ChannelKind.None;
        public ChannelType ChanType = ChannelType.None;
        public bool ChanFree = false;

        public int remote_svr;      // 서버구분용 remoteID번호 (입장할때 방이 어떤 서버에 존재하는지 정보)
        public int remote_lobby;    // 서버구분용 remoteID번호 (원래입장시점의 로비서버 - 방에서 나갈때 사용)
        public CBadugiEngine engine;
        internal SS.Proxy Proxy;

        public int max_users;
        ConcurrentQueue<byte> players_index;
        ConcurrentDictionary<string, PacketType> received_protocol;
        public ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer> PlayersConnect; // 접속중 플레이어
        public ConcurrentDictionary<int, CPlayer> PlayersGaming; // 게임중 플레이어

        // 방 변수
        public Guid roomID;
        public int roomNumber = 0;
        public RoomStatus status = RoomStatus.None;
        public int stake;
        public string Password = "";
        public int BaseMoney;
        public long MinMoney;
        public long MaxMoney;
        public bool FirstRun = true;
        public DateTime roomWaitTime; // 대기중인 시간
        public DateTime roomWaitTime2; // 대기중인 시간

        public GAME_RULE_TYPE GameRuleType;

        public long LeavePlayerBetMoney;
        public long LeavePlayerDealMoney;
        public long LeavePlayerJackpotMoney;

        // 게임 변수
        public CPlayer Operator { get; private set; }
        public void SetOperator(CPlayer player) { Operator = player; }
        public long DealerFeeMoney { get; private set; }
        public long JackPotFeeMoney { get; private set; }
        public EVENT_JACKPOT_TYPE jackPotType { get; private set; } // 이벤트 타입
        CPlayer JackpotSuddenEventWinner; // 돌발이벤트 당첨 플레이어
                                          //bool eventTriggerDouble; // 2배 이벤트 트리거

        public EVENT_JACKPOT_TYPE CallJackPotType; // 콜 이벤트 타입
        public int CallTarget; // 콜 이벤트 타겟 플레이어 아이디

        public CGameLog gameLog;

        delegate void delegateRoomGameReset();
        delegateRoomGameReset RoomGameReset;
        delegate void delegateRoomGameEventCheck();
        delegateRoomGameEventCheck RoomGameEventCheck;
        delegate void delegateRoomGameMadeCheck();
        delegateRoomGameMadeCheck RoomGameMadeCheck;
        delegate void delegateRoomSendDealCard();
        delegateRoomSendDealCard RoomSendDealCard;
        delegate void delegateRoomGameEnd();
        delegateRoomGameEnd RoomGameEnd;
        delegate bool delegateRoomGameChangeCard(CPlayer owner, CMessage msg);
        delegateRoomGameChangeCard RoomGameChangeCard;


        #endregion

        public CGameRoom(RoomServer roomServer)
        {
            this.RoomSvr = roomServer;
            this.Proxy = RoomSvr.Proxy;
            //if (RoomSvr.ChannelID == 5)
            //{
            //    max_users = 8;
            //    GameRuleType = GAME_RULE_TYPE.HOLDEM_BADUGI;
            //    RoomGameReset = new delegateRoomGameReset(ResetBadugi);
            //    RoomGameEventCheck = new delegateRoomGameEventCheck(EventCheckBadugiHoldem);
            //    RoomGameMadeCheck = new delegateRoomGameMadeCheck(MadeCheckBadugiHoldem);
            //    RoomSendDealCard = new delegateRoomSendDealCard(send_dealcardtoAllUserHoldem);
            //    RoomGameEnd = new delegateRoomGameEnd(gameEndHoldem);
            //    RoomGameChangeCard = new delegateRoomGameChangeCard(on_player_change_card_holdem);
            //    //max_users = 8;
            //}
            //else
            {
                max_users = 5;
                GameRuleType = GAME_RULE_TYPE.BADUGI;
                RoomGameReset = new delegateRoomGameReset(ResetBadugi);
                RoomGameEventCheck = new delegateRoomGameEventCheck(EventCheckBadugi);
                RoomGameMadeCheck = new delegateRoomGameMadeCheck(MadeCheckBadugi);
                RoomSendDealCard = new delegateRoomSendDealCard(send_dealcardtoAllUser);
                RoomGameEnd = new delegateRoomGameEnd(gameEnd);
                RoomGameChangeCard = new delegateRoomGameChangeCard(on_player_change_card);
            }
            this.engine = new CBadugiEngine(max_users);
            this.waiting_packets = new ConcurrentQueue<MessagePacket>();
            Thread_sequential_packet_handler = new Thread(new ThreadStart(sequential_packet_handler));
            this.PlayersConnect = new ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer>();
            this.PlayersGaming = new ConcurrentDictionary<int, CPlayer>();
            //this.players = new ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer>();
            this.received_protocol = new ConcurrentDictionary<string, PacketType>();
            this.status = RoomStatus.Stay;
            this.players_index = new ConcurrentQueue<byte>();

            this.jackPotType = EVENT_JACKPOT_TYPE.NONE;

            this.DealerFeeMoney = 0;
            this.JackPotFeeMoney = 0;

            LeavePlayerBetMoney = 0;
            LeavePlayerDealMoney = 0;
            LeavePlayerJackpotMoney = 0;

            this.gameLog = new CGameLog(max_users);

            for (int i = 0; i < max_users; ++i)
            {
                this.players_index.Enqueue((byte)i);
            }

            Thread_sequential_packet_handler.Start();
            this.roomWaitTime = DateTime.Now.AddSeconds(5);
            this.roomWaitTime2 = this.roomWaitTime;

            rnd = new Random((int)DateTime.UtcNow.Ticks);
        }

        #region Room Common
        public byte pop_players_index()
        {
            if (this.players_index.Count == 0) return byte.MaxValue;

            byte temp;
            if (this.players_index.TryDequeue(out temp))
            {
                return temp;
            }
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
            if (pi == byte.MaxValue) return true;
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
            //this.MaxMoney = GameServer.GetMaximumMoney(this.ChanKind, this.stake);
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
        public void ActionExecute(CPlayer player, MessageTemp msg)
        {
            if (!player.agent.packet_handler.ContainsKey(msg.mRmiID))
            {
                // 리턴되지 않으려면 IsPlayablePacket에 패킷ID 추가.
                return;
            }

            try
            {
                player.agent.packet_handler[msg.mRmiID](msg.Msg);
            }
            catch (Exception e)
            {
                Log._log.Error(e.ToString());
            }
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
                    else
                    {
                        Log._log.ErrorFormat("sequential_packet_handler cancel waiting_packets.TryDequeue.");
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
                    TaskTick = DateTime.Now.AddSeconds(1);

                    ++this.tick;

                    if (this.tick % 1 == 0)
                    {
                        ProcessAutoPlay();

                        // 응답없는 플레이어 종료
                        if (status == RoomStatus.GamePlay)
                        {
                            var result = Check_Player_Active();
                            if (result != null)
                            {
#if DEBUG2
                                Log._log.Warn("actionTimeLimit Out. Player:" + result.data.userID);
                                //Send(result, result.currentMsg.Msg, result.currentMsg.mRmiID);
                                //RoomSvr.RelayClientLeave(result.Remote.Key, RmiContext.UnreliableSend, result.Remote, false);
                                //RoomSvr.ClientDisconect(result.Remote.Key, result.Remote, "Task actionTimeLimit Over");
#else
                                Log._log.Warn("actionTimeLimit Out. Player:" + result.data.userID);
                                RoomSvr.DBLog(result.data.ID, RoomSvr.ChannelID, roomNumber, LOG_TYPE.연결끊김, "게임중 응답없음");
                                RoomSvr.RelayClientLeave(result.Remote.Key, CPackOption.Basic, result.Remote.Value, false);
                                RoomSvr.ClientDisconect(result.Remote.Key, result.Remote.Value, "Task actionTimeLimit Over");
#endif
                            }
                        }
                    }

                    if (this.tick % 1 == 0)
                    {
                        //lock (room.Value.Locker)
                        lock (Locker)
                        {
                            // 대기중인방 자동 시작

                            if (status == RoomStatus.Stay && PlayersConnect.Count > 1 && roomWaitTime < DateTime.Now && roomWaitTime2 < DateTime.Now && RoomSvr.ServerMaintenance == false)
                            {
                                AutoGameStart();
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
                case SS.Common.GameDealCards:
                    isPlayable = true;
                    if (jackPotType == EVENT_JACKPOT_TYPE.NONE)
                        player.actionTimeLimit = DateTime.Now.AddSeconds(10);
                    else
                        player.actionTimeLimit = DateTime.Now.AddSeconds(25);
                    break;
                case SS.Common.GameRequestBet:
                    isPlayable = true;
                    player.actionTimeLimit = DateTime.Now.AddSeconds(10);
                    break;
                case SS.Common.GameRequestChangeCard:
                    isPlayable = true;
                    player.actionTimeLimit = DateTime.Now.AddSeconds(10);
                    break;
            }

            if (isPlayable == true)
            {
                player.isActionExecute = false;

                return true;
            }

            return false;
        }
        public bool CommitPacket(CPlayer player, PacketType rmiID)
        {
            if (player.currentMsg == null) return false;

            switch (player.currentMsg.mRmiID)
            {
                case SS.Common.GameDealCards:
                    {
                        if (rmiID == SS.Common.GameDealCardsEnd)
                            return true;
                    }
                    break;
                case SS.Common.GameRequestBet:
                    {
                        if (rmiID == SS.Common.GameActionBet)
                            return true;
                    }
                    break;
                case SS.Common.GameRequestChangeCard:
                    {
                        if (rmiID == SS.Common.GameActionChangeCard)
                            return true;
                    }
                    break;
            }
            return false;
        }
        public void PacketProcess(CPlayer player)
        {

            if (player.QueueMsg == null || player.QueueMsg.Count == 0)
            {
                if (player.currentMsg != null)
                {
                    ActionExecute(player, player.currentMsg);
                    player.currentMsg = null;
                    return;
                }
                else
                {
                    return;
                }
            }

            MessageTemp temp;
            if (player.QueueMsg.TryDequeue(out temp))
            {
                ProcessMsg(player, temp.Msg, temp.mRmiID);
            }
        }
        bool is_received(string player_index, PacketType pkID)
        {
            if (!this.received_protocol.ContainsKey(player_index))
            {
                return false;
            }

            return this.received_protocol[player_index] == pkID;
        }
        void checked_protocol(string player_index, PacketType protocol)
        {
            if (this.received_protocol.ContainsKey(player_index))
            {
                //err
                return;
            }

            this.received_protocol.TryAdd(player_index, protocol);
        }
        bool all_received(PacketType protocol)
        {
            if (this.received_protocol.Count < this.PlayersConnect.Count)
            {
                return false;
            }
            foreach (KeyValuePair<string, PacketType> kvp in this.received_protocol)
            {
                if (kvp.Value != protocol)
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
            if (this.status != RoomStatus.GamePlay) return false;

            return true;
        }
        #endregion

        #region Room Game
        void ResetGame()
        {

        }
        void ResetBadugi()
        {
            this.engine.reset();
            this.DealerFeeMoney = 0;
            this.JackPotFeeMoney = 0;

            LeavePlayerBetMoney = 0;
            LeavePlayerDealMoney = 0;
            LeavePlayerJackpotMoney = 0;

            PlayersGaming.Clear();
            foreach (var player in PlayersConnect)
            {
                if (player.Value.player_index == byte.MaxValue) continue;

                PlayersGaming.TryAdd(player.Value.player_index, player.Value);

                if (PlayersGaming.Count >= max_users) break;
            }

            if (PlayersGaming.Count < max_users)
            {
                foreach (var player in PlayersConnect)
                {
                    if (player.Value.player_index != byte.MaxValue) continue;

                    // 관전자 입장처리
                    player.Value.player_index = pop_players_index();
                    send_players_infos(player.Value);

                    PlayersGaming.TryAdd(player.Value.player_index, player.Value);

                    if (PlayersGaming.Count >= max_users) break;
                }
            }

            // 플레이 상태로 변경
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                player.Value.status = UserStatus.RoomPlay;
            }

            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                player.Value.ChangeMoney = 0;
                player.Value.GameResult = 0;
                player.Value.GameDealMoney = 0;
                player.Value.JackPotDealMoney = 0;

                player.Value.agent.reset();
                player.Value.actionTimeLimit = DateTime.Now.AddSeconds(25);
                player.Value.agent.status = UserGameStatus.Play;
                player.Value.agent.initPossibleRaceCount(this.engine.currentRound, ChanFree, ChanKind);

                this.engine.addAgents(player.Value.agent);
                this.engine.distributed_players_cards.Add(new List<CardInfo.sCARD_INFO>());

                // 방장이 보스
                if (player.Value == Operator)
                {
                    engine.SetBoss(player.Value);
                }

                ++this.engine.startPlayerCount;
            }

            engine.SetCurrentPlayer(engine.BossPlayer);

            gameLog.Reset(this.engine.baseMoney);
        }
        void EventCheckBadugi()
        {
            if (CallJackPotType != EVENT_JACKPOT_TYPE.NONE)
            {
                // 이벤트 콜 확인
                this.jackPotType = CallJackPotType;
                CallJackPotType = EVENT_JACKPOT_TYPE.NONE;
            }
            else
            {
                this.jackPotType = RoomSvr.EventCheck(ChanId, PlayersGaming.Count);
            }

            if (this.jackPotType != EVENT_JACKPOT_TYPE.NONE)
            {
                if (this.jackPotType == EVENT_JACKPOT_TYPE.Z1) // 돌발 이벤트
                {
                    int r = rnd.Next(PlayersGaming.Count);
                    this.JackpotSuddenEventWinner = PlayersGaming.Values.ToList()[r];

                    if (CallTarget != 0)
                    {
                        // 콜 타겟 확인
                        foreach (var player in PlayersGaming)
                        {
                            if (player.Value.data.ID == CallTarget)
                            {
                                this.JackpotSuddenEventWinner = player.Value;
                                break;
                            }
                        }
                        CallTarget = 0;
                    }
                    //JackpotSuddenEventWinner.agent.addMoney(Math.Min(200000, this.engine.baseMoney * 100));
                    JackpotSuddenEventWinner.agent.addMoney(this.engine.baseMoney * 100);
                    send_event2_jackpot_start(JackpotSuddenEventWinner.player_index);
                }
                else // 장비 이벤트
                {
                    send_event_jackpot_start();
                }
                // 잭팟 이벤트 공지
                //Proxy.RoomLobbyEventStart((RemoteID)this.remote_lobby, CPackOption.Basic, this.roomID, (int)this.jackPotType);

                // 이벤트 진행중에는 자리있어도 입장불가
                //send.room_lobby_closeroom((RemoteID)this.remote_lobby, CPackOption.Basic, this.roomID);
            }
        }
        public bool isRestrict()
        {
            bool restrict = false;

            if (jackPotType != EVENT_JACKPOT_TYPE.NONE)
            {
                restrict = true;
            }
            if (PlayersConnect.Count == max_users)
            {
                restrict = true;

            }

            return restrict;
        }
        public bool isEventRestrict()
        {
            bool restrict = false;

            if (jackPotType != EVENT_JACKPOT_TYPE.NONE)
            {
                restrict = true;
            }

            return restrict;
        }
        public void player_room_out(CPlayer Player)
        {
            if (push_players_index(Player.player_index) == false) return;

            CPlayer temp;
            if (!PlayersConnect.TryRemove(Player.Remote, out temp))
            {
                // 에러
                //Log._log.Fatal("Player_room_out no player");
                //return;
                //System.Diagnostics.Debug.Assert(false,"Player_room_out Failed");
            }
            if (!PlayersGaming.TryRemove(Player.player_index, out temp))
            {
                // 에러
                //Log._log.Fatal("Player_room_out no player");
                //return;
                //System.Diagnostics.Debug.Assert(false,"Player_room_out Failed");
            }
            Player.PacketReady = false;
            removeAgent(Player);

            if (PlayersConnect.Count == 0) return;

            roomWaitTime = DateTime.Now.AddMilliseconds(500);

            // 방장이 나간거면 다음사람을 방장으로
            if (Player == Operator)
            {
                SetOperatorNext();
                if (Operator == null)
                {
                    SetOperator(PlayersConnect.First().Value);
                }
            }

            if (Operator.status == UserStatus.RoomReady)
            {
                //Operator.status = UserStatus.RoomStay;
            }

            send_room_out(Player.player_index);

            // 혼자일 경우 게임시작 불가
            if (this.status == RoomStatus.Stay)
            {
                if (PlayersConnect.Count == 1)
                {
                    send_can_start(Operator, false);
                    FirstRun = true;
                }
                else
                {
                    send_can_start(Operator, true);
                }
            }
        }
        public void KickPlayer(CPlayer player)
        {
            send_player_all_in(player);
            player.KickCount = true;
        }
        #endregion

        #region Room Game 2(Holdem)
        void ResetBadugiHoldem()
        {
        }
        void EventCheckBadugiHoldem()
        {

        }

        #endregion

        public void ProcessMsg(CPlayer owner, CMessage msg, PacketType rmiID)
        {
#if DEBUG
#else
            try
#endif
            {
                if (is_received(owner.data.userID, rmiID))
                {
                    Log._log.WarnFormat("ProcessMsg is_received. user:{0}, rmiID{1}", owner.data.userID, rmiID);
                    return;
                }
                checked_protocol(owner.data.userID, rmiID);
                bool packetAllowed = false;
                if (owner.status != UserStatus.RoomPlayOut)
                {
                    packetAllowed = CommitPacket(owner, rmiID);
                    if (packetAllowed == true)
                    {
                        owner.isActionExecute = true;
                        owner.currentMsg = null;
                    }
                }

                lock (Locker)
                {
                    switch (rmiID)
                    {
                        case SS.Common.GameRoomInUser:
                            {
                                on_room_in(owner, msg);
                            }
                            break;
                        case SS.Common.GameRequestReady:
                            {
                                if (RoomSvr.ServerMaintenance) break;

                                On_Ready_Req(owner, msg);
                            }
                            break;
                        case SS.Common.GameDealCardsEnd:
                            {
                                //카드를 모두 받음 턴시작
                                if (!CheckGameRun())
                                {
                                    Log._log.Warn(string.Format("ProcessMsg Break. pkID:{0} Player:{1}", rmiID, owner.data.userID));
                                    break;
                                }
                                on_start_betting(owner);
                            }
                            break;
                        case SS.Common.GameActionBet:
                            {
                                //배팅 버튼 이벤트
                                //게임종료인지 확인
                                //턴 종료인지 확인
                                //다음턴 사람에게 배팅 요청(가능한배팅알려주기)
                                if (!CheckGameRun())
                                {
                                    Log._log.Warn(string.Format("ProcessMsg Break. pkID:{0} Player:{1}", rmiID, owner.data.userID));
                                    break;
                                }
                                if (on_player_betting(owner, msg) == false)
                                    Log._log.Warn(string.Format("packetCommit False. pkID:{0} Player:{1}", rmiID, owner.data.userID));
                            }
                            break;
                        case SS.Common.GameActionChangeCard:
                            {
                                if (!CheckGameRun())
                                {
                                    Log._log.Warn(string.Format("ProcessMsg Break. pkID:{0} Player:{1}", rmiID, owner.data.userID));
                                    break;
                                }
                                if (RoomGameChangeCard(owner, msg) == false)
                                    Log._log.Warn(string.Format("packetCommit False. pkID:{0} Player:{1}", rmiID, owner.data.userID));
                            }
                            break;
                    }
                }
            }
#if DEBUG
#else
            catch (Exception e)
            {
                Log._log.FatalFormat("ProcessMsg Exception. owner:{0}, e:{1}", owner.data.userID, e.ToString());
                RoomSvr.ClientDisconect(owner.Remote.Key, owner.Remote.Value, "패킷 위/변조 감지");
            }
#endif
        }

        public void AutoGameStart()
        {
            // 돈없는 플레이어가 있으면 강퇴 후 준비
            foreach (var player in PlayersConnect)
            {
                if (player.Value.agent.haveMoney < BaseMoney)
                {
                    send_player_all_in(player.Value);
                    return;
                }
            }

            //게임 참여자 모두 준비 완료
            if (PlayersConnect.Count >= 2)
            {
                if (this.status != RoomStatus.Stay) return;

                foreach (var player in PlayersConnect)
                {
                    if (player.Value.status == UserStatus.None) return;
                }

                gameStart();
            }
        }

        bool gameStart() // 게임시작
        {
            FirstRun = false;

            this.status = RoomStatus.GamePlay;

            // 초기화
            RoomGameReset();

            foreach (var player in PlayersGaming)
            {
                if (ChanType == ChannelType.Charge)
                    player.Value.agent.setMoney(player.Value.data.money_pay);
                else
                    player.Value.agent.setMoney(player.Value.data.money_free);
            }

            // 게임시작
            send_game_start();

            // 이벤트 확인
            RoomGameEventCheck();

            //RoomSvr.PlayerReload(this);

            // 카드 분배
            if (GameRuleType == GAME_RULE_TYPE.HOLDEM_BADUGI)
                this.engine.cardOperateBadugiHoldem();
            else
                this.engine.cardOperateBadugi(RoomSvr.MadeLimit);

            // 게임로그 시작
            if (GameRuleType == GAME_RULE_TYPE.HOLDEM_BADUGI)
                gameLog.StartHoldem(this.engine.startPlayerCount, this.engine.flopCard.m_nShape, this.engine.flopCard.m_nCardNum);
            else
                gameLog.Start(this.engine.startPlayerCount);

            // 메이드 확인
            RoomGameMadeCheck();

            // 카드 로그 저장
            foreach (var player in PlayersGaming)
            {
                gameLog.PlayerLog[player.Value.player_index].UserId = player.Value.data.ID;
                gameLog.PlayerLog[player.Value.player_index].UserCard = player.Value.agent.MakePlayerCardLog();

                gameLog.StartGame(player.Value.player_index, player.Value.agent.MakePlayerCard(), player.Value.agent.haveMoney + engine.baseMoney);
            }

            //카드 정보 전송
            RoomSendDealCard();

            send_player_startbetting();
            return true;
        }

        void MadeCheckBadugi()
        {
            // 메이드 가중치 (메이드 확률 조절, 밀어주기)
            foreach (var player in PlayersGaming)
            {
                //for (int j = 0; j < this.engine.DeckCount(); ++j) // 일회성 확률이 아닌 절대 확률로 적용
                //{
                // 메이드일경우 확률 조건에 따라 메이드 카드를 교체시킴
                if (RoomSvr.IsPush(this.ChanId, player.Value.agent.IsMadeNumberStart(), 0, 0, player.Value.GameLevel, player.Value.UserLevel, player.Value.GameLevel2, engine.currentRound) == true)
                {
                    byte changeIndex = (byte)(rnd.Next() % 4);
                    //CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
                    //card.m_nCardNum = player.Value.agent.GetUserHands()[0].card.m_nCardNum;
                    //card.m_nShape = player.Value.agent.GetUserHands()[0].card.m_nShape;
                    this.engine.AddExcahngeCard(player.Value.agent.GetUserHands()[changeIndex].card);
                    
                    //this.engine.AddExcahngeCard(card);
                    player.Value.agent.remove_card_to_hand(changeIndex);

                    CardInfo.sCARD_INFO exchangeCard = this.engine.getExchangeCard();
                    player.Value.agent.add_card_to_hand(changeIndex, exchangeCard);
                    player.Value.agent.CalcHandMade();
                }
                //    else
                //    {
                //        break;
                //    }
                //}
            }
        }

        void MadeCheckBadugiHoldem()
        {
        }

        #region Packet Game
        void Send(CPlayer player, CMessage msg, PacketType rmiID)
        {
            if (player.status == Server.Engine.UserStatus.RoomPlayOut)
            {
                MessageTemp msg_ = new MessageTemp(msg, rmiID);
                ActionExecute(player, msg_);
                return;
            }
            else if (IsPlayablePacket(player, rmiID))
            {
                MessageTemp msg_ = new MessageTemp(msg, rmiID);
                player.currentMsg = msg_;
            }

#if PACKET
            Log._log.InfoFormat("SEND Packet. rmiID:{0}, user:{1}", rmiID, player.data.userID);
#endif

            //ArrByte array = new ArrByte();
            //ArrByte.Copy(array.);

            if (msg.m_array == null)
            {
                msg.m_array = new ArrByte();
            }

            // 릴레이 패킷으로 변환하여 전송
            bool sendResult;
            switch (rmiID)
            {
                case SS.Common.GameRoomReady: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRoomReady, msg); break;
                case SS.Common.GameStart: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayStart, msg); break;
                case SS.Common.GameDealCards: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayDealCards, msg); break;
                case SS.Common.GameUserIn: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayUserIn, msg); break;
                case SS.Common.GameSetBoss: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelaySetBoss, msg); break;
                case SS.Common.GameNotifyStat: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayNotifyStat, msg); break;
                case SS.Common.GameRoundStart: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRoundStart, msg); break;
                case SS.Common.GameChangeTurn: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayChangeTurn, msg); break;
                case SS.Common.GameRequestBet: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRequestBet, msg); break;
                case SS.Common.GameResponseBet: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayResponseBet, msg); break;
                case SS.Common.GameChangeRound: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayChangeRound, msg); break;
                case SS.Common.GameRequestChangeCard: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRequestChangeCard, msg); break;
                case SS.Common.GameResponseChangeCard: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayResponseChangeCard, msg); break;
                case SS.Common.GameCardOpen: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayCardOpen, msg); break;
                case SS.Common.GameOver: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayOver, msg); break;
                case SS.Common.GameRoomInfo: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayRoomInfo, msg); break;
                case SS.Common.GameKickUser: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayKickUser, msg); break;
                case SS.Common.GameEventInfo: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEventInfo, msg); break;
                case SS.Common.GameUserInfo: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayUserInfo, msg); break;
                case SS.Common.GameUserOut: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayUserOut, msg); break;
                case SS.Common.GameEventStart: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEventStart, msg); break;
                case SS.Common.GameEvent2Start: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEvent2Start, msg); break;
                case SS.Common.GameEventRefresh: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEventRefresh, msg); break;
                case SS.Common.GameEventEnd: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEventEnd, msg); break;
                case SS.Common.GameMileageRefresh: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayMileageRefresh, msg); break;
                case SS.Common.GameEventNotify: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEventNotify, msg); break;
                case SS.Common.GameCurrentInfo: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayCurrentInfo, msg); break;
                case SS.Common.GameEntrySpectator: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayEntrySpectator, msg); break;
                case SS.Common.GameNotifyMessage: sendResult = Send(player.Remote.Key, player.Remote.Value, SS.Common.GameRelayNotifyMessage, msg); break;
                default: Log._log.ErrorFormat("Send Relay Failure. player:{0}, rmiID:{1}", player.data.userID, rmiID); sendResult = false; break;
            }

            if (sendResult == false)
            {
                Log._log.ErrorFormat("Relay Send Failure. rmiID:{0}");
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
        // 방 입장
        public void on_room_in(CPlayer owner, CMessage rm)
        {
            if (owner.PacketReady == true) return;

            bool spectator = false;
            // 유저 정보 전송
            if ((this.status != RoomStatus.Stay && owner.status == UserStatus.None))
            {
                // 관전자 처리
                spectator = true;
                owner.status = UserStatus.None;
                owner.agent.status = UserGameStatus.None;
            }
            else
            {
                roomWaitTime = DateTime.Now.AddMilliseconds(500);
            }

            if (this.status == RoomStatus.Stay)
            {
                owner.status = UserStatus.RoomStay;
                // 2명 이상일 경우 게임 시작 가능
                if (PlayersConnect.Count >= max_users)
                {
                    send_can_start(Operator, false);
                    Operator.status = UserStatus.RoomReady;
                }
                else if (PlayersConnect.Count > 1)
                {
                    send_can_start(Operator, true);
                }
            }

            // 정보 전송
            send_room_in(owner, spectator);
            send_room_info(owner);
            if (owner.player_index != byte.MaxValue)
            {
                send_players_infos(owner);
            }
            send_players_info(owner);
            send_event_jackpot_info(owner);
            owner.PacketReady = true;
        }
        // 레디
        public void On_Ready_Req(CPlayer owner, CMessage rm)
        {
            clear_received_protocol();

            // 플레이어한테 판돈이 부족하면 레디 안받고 강퇴 처리
            if (owner.agent.haveMoney < BaseMoney)
            {
                send_player_all_in(owner);
                return;
            }

            lock (Locker)
            {
                if (this.status != RoomStatus.Stay) return;

                //if (players.Count < 2)
                //{
                //    owner.status = UserStatus.RoomStay;
                //    return;
                //}

                owner.status = UserStatus.RoomReady;

                if (PlayersConnect.Count >= 2 && FirstRun == false && roomWaitTime < DateTime.Now)
                {
                    foreach (var player in PlayersConnect)
                    {
                        if (player.Value.status != UserStatus.RoomReady) return;
                    }
                    gameStart();
                }
            }
        }
        // 패트 배팅
        private void on_start_betting(CPlayer owner)
        {
            //clear_received_protocol();
            owner.agent.status = UserGameStatus.Play;

            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                if (player.Value.agent.status == UserGameStatus.DealCard) return;
            }

            // 플레이어 모두 준비되면 처리
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                player.Value.agent.status = UserGameStatus.Play;
            }

            gameLog.StartBetting(this.engine.currentRound, GetLivePlayersCount(), this.engine.totalMoney);

            bool race = true;
            // 내가 올인
            if (engine.CurrentPlayer.agent.haveMoney <= 0
                || engine.CurrentPlayer.agent.haveMoney < engine.CurrentPlayer.agent.callMoney)
            {
                race = false;
            }
            // 나를 제외한 플레이어가 올인인지 확인
            else
            {
                if (ChanFree == false) // 자유채널이면 무시
                {
                    race = false;
                    foreach (var player in PlayersGaming)
                    {
                        if (player.Value.status == UserStatus.None) continue;
                        if (player.Value.agent.isDeadPlayer) continue;
                        if (player.Value.agent.haveMoney <= 0) continue;
                        if (player.Value.player_index == engine.CurrentPlayer.player_index) continue;

                        race = true;
                        break;
                    }
                }
                else
                {
                    race = true;
                }
            }

            //engine.CurrentPlayer.agent.Buttons = SetButtonActive(false, true, false, true, false, false, false);
            //if (ChanKind == ChannelKind.무료2채널)
            //{
            //    SetButtonActive(engine.CurrentPlayer.agent.Buttons, false, race, false, false, true, !race, false);
            //}
            //else
            {
                SetButtonActive(engine.CurrentPlayer.agent.Buttons, false, race, false, race, true, !race, false);
            }
            send_request_betting(current_player());
        }
        // 라운드시작 보스 배팅
        private void roundstart_boss_betting()
        {
            engine.SetCurrentPlayer(engine.BossPlayer);
            CPlayer currentPlayer = current_player();

            bool race;
            bool Bbing;

            // 내가 올인
            if (currentPlayer.agent.haveMoney <= 0
                || currentPlayer.agent.haveMoney < currentPlayer.agent.callMoney)
            {
                race = false;
            }
            // 나를 제외한 플레이어가 올인인지 확인
            else
            {
                if (ChanFree == false) // 자유채널이면 무시
                {
                    race = false;
                    foreach (var player in PlayersGaming)
                    {
                        if (player.Value.status == UserStatus.None) continue;
                        if (player.Value.agent.isDeadPlayer) continue;
                        if (player.Value.agent.haveMoney <= 0) continue;
                        if (player.Value.player_index == currentPlayer.player_index) continue;

                        race = true;
                        break;
                    }
                }
                else
                {
                    race = true;
                }
            }

            // 삥제한 룰 추가
            /* 1. 아침, 점심에
             * 2. 패스가 없으면 삥 전부 비활성화
             * 3. 패스가 있으면 패스한 플레이어 포함 패스플레이어 자리부터 뒷자리까지 삥버튼 비활성화  => 현재 보스가 패스했으면 삥 전부 비활성화
             */
            if (race == true)
            {
                Bbing = true;
                //if (engine.currentRound == GameRound.MORNING || engine.currentRound == GameRound.AFTERNOON)
                //{
                //    if (engine.PassCount == 0)
                //    {
                //        Bbing = false;
                //    }
                //    else if (engine.PassCount == 1)
                //    {
                //        if (engine.BossPlayer.agent.roundChange[(int)engine.currentRound - 1] == 5)
                //        {
                //            Bbing = false;
                //        }
                //    }
                //}
            }
            else
            {
                Bbing = false;
            }

            gameLog.StartBetting(this.engine.currentRound, GetLivePlayersCount(), this.engine.totalMoney);

            // 라운드시작 첫 배팅
            // 패트 제외 보스 플레이어 다이 가능

            bool canDie = true;
            if (currentPlayer.agent.haveMoney <= 0 && ChanFree == false) // 올인이면 다이 X
            {
                canDie = false;
            }
            else if (currentPlayer.agent.callMoney == 0)  // 콜을 받아야 되는데 나머지 올인이면 다이 X
            {
                int AlivePlayerCnt = 0;
                int AllinPlayerCnt = 0;
                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.None) continue;
                    if (player.Value.player_index == currentPlayer.player_index) continue;
                    if (player.Value.agent.isDeadPlayer == false)
                    {
                        ++AlivePlayerCnt;
                        if (player.Value.agent.haveMoney <= 0)
                            ++AllinPlayerCnt;
                    }

                }
                if (AlivePlayerCnt == AllinPlayerCnt && ChanFree == false)
                {
                    canDie = false;
                }
            }

            //currentPlayer.agent.Buttons = SetButtonActive(false, Bbing, false, race, canDie, true, false);
            SetButtonActive(currentPlayer.agent.Buttons, false, Bbing, false, race, canDie, true, false);

            send_request_betting(currentPlayer);
        }
        private bool on_player_betting(CPlayer owner, CMessage msg)
        {
            clear_received_protocol();
            if (owner.agent.status != UserGameStatus.Betting) return false;
            owner.agent.status = UserGameStatus.Play;

            // 현재 턴이 아닌사람이 배팅했으면 배팅턴 다시 보냄
            if (owner.player_index != current_player().player_index)
            {
                send_change_turn(current_player().player_index);
                return false;
            }

            byte playerIndex;
            Rmi.Marshaler.Read(msg, out playerIndex);
            byte button;
            Rmi.Marshaler.Read(msg, out button);
            BETTING _button = (BETTING)button;

            // 패킷 예외처리
            if (_button < BETTING.CALL || _button > BETTING.BASE)
            {
                _button = BETTING.DIE;
            }
            if (Convert.ToBoolean(owner.agent.Buttons[(int)_button]) == false)
            {
                // 누를 수 없는 버튼을 전송한 경우 최소 선택으로 변경해서 처리
                if (Convert.ToBoolean(owner.agent.Buttons[(int)(BETTING.CHECK)]) == true) // 체크할 수 있으면 체크
                {
                    _button = BETTING.CHECK;
                }
                else if (Convert.ToBoolean(owner.agent.Buttons[(int)(BETTING.CALL)]) == true) // 콜할 수 있으면 콜
                {
                    _button = BETTING.CALL;
                }
                else if (Convert.ToBoolean(owner.agent.Buttons[(int)(BETTING.BBING)]) == true) // 삥할 수 있으면 삥
                {
                    _button = BETTING.BBING;
                }
                else if (Convert.ToBoolean(owner.agent.Buttons[(int)(BETTING.HARF)]) == true) // 하프할 수 있으면 하프
                {
                    _button = BETTING.HARF;
                }
                else // 다이
                {
                    _button = BETTING.DIE;
                }
            }

            // 레이스할 돈이 없으면 콜로 바꿈
            switch (_button)
            {
                case BETTING.CALL:
                    on_player_betting_call(owner, msg);
                    break;
                case BETTING.BBING:
                    on_player_betting_bbing(owner, msg);
                    break;
                case BETTING.QUATER:
                    if (owner.agent.CheckPaidMoney(owner.agent.callMoney) == false)
                        on_player_betting_call(owner, msg);
                    else
                        //on_player_betting_quater(owner, msg);
                        on_player_betting_half(owner, msg);
                    break;
                case BETTING.HARF:
                    if (owner.agent.CheckPaidMoney(owner.agent.callMoney) == false)
                        on_player_betting_call(owner, msg);
                    else
                        on_player_betting_half(owner, msg);
                    break;
                case BETTING.DIE:
                    on_player_betting_die(owner, msg);
                    break;
                case BETTING.CHECK:
                    on_player_betting_check(owner, msg);
                    break;
                case BETTING.DDADDANG:
                    if (owner.agent.CheckPaidMoney(owner.agent.callMoney) == false)
                        on_player_betting_call(owner, msg);
                    else
                        on_player_betting_ddaddang(owner, msg);
                    break;
            }
            return true;
        }
        private void on_player_betting_call(CPlayer owner, CMessage msg)
        {
            long betting = this.engine.PlayerBettingCall(owner.agent);

            send_result_betting(owner, BETTING.CALL, betting);

            encrementCallDieCount();    //콜다이 카운트 증가
            next_betting_turn();
        }
        private void on_player_betting_bbing(CPlayer owner, CMessage msg)
        {
            long betting = this.engine.PlayerBettingBbing(owner.agent);

            send_result_betting(owner, BETTING.BBING, betting);

            clearCallDieCount();    //콜다이 카운트 초기화
            next_betting_turn();
        }
        private void on_player_betting_quater(CPlayer owner, CMessage msg)
        {
            long betting = this.engine.PlayerBettingQuater(owner.agent);

            send_result_betting(owner, BETTING.QUATER, betting);

            clearCallDieCount();
            next_betting_turn();
        }
        private void on_player_betting_half(CPlayer owner, CMessage msg)
        {
            long betting;
            //if (ChanKind == ChannelKind.무료자유2채널) // 자유2채널 하프 50배
            //    betting = this.engine.PlayerBettingHalffix(owner.agent);
            //else
                betting = this.engine.PlayerBettingHalf(owner.agent);

            send_result_betting(owner, BETTING.HARF, betting);

            clearCallDieCount();
            next_betting_turn();
        }
        private void on_player_betting_die(CPlayer owner, CMessage msg)
        {
            this.engine.PlayerBettingDie(owner.agent);

            send_result_betting(owner, BETTING.DIE, 0);

            if (owner.status == UserStatus.RoomPlayOut)
            {
                lock (this.Locker)
                {
                    RoomSvr.DummyPlayerDieLeave(this, owner);
                }
            }

            encrementCallDieCount();
            if (owner.player_index == engine.BossPlayer.player_index)
            {
                //if (this.engine.startPlayerCount > 2)
                //{
                //    this.engine.set_next_player_boss();
                //    send_bossindex();
                //}
                this.engine.set_next_player_boss(PlayersGaming);
                send_bossindex();
            }
            next_betting_turn();
        }
        private void on_player_betting_check(CPlayer owner, CMessage msg)
        {
            this.engine.prevBetType = BETTING.CHECK;

            owner.agent.setPossibleRaceCount(0);
            owner.agent.bCalled = true;

            send_result_betting(owner, BETTING.CHECK, 0);
            clearCallDieCount();
            next_betting_turn();
        }
        private void on_player_betting_ddaddang(CPlayer owner, CMessage msg)
        {
            long betting = this.engine.PlayerBettingDdaddang(owner.agent);

            send_result_betting(owner, BETTING.DDADDANG, betting);

            clearCallDieCount();
            next_betting_turn();
        }
        private void SetButtonActive(byte[] Buttons, bool call, bool bbing, bool quater, bool half, bool die, bool check, bool ddadang)
        {
            Buttons[(int)BETTING.CALL] = Convert.ToByte(call); //콜
            Buttons[(int)BETTING.BBING] = Convert.ToByte(bbing); //삥
            Buttons[(int)BETTING.QUATER] = Convert.ToByte(quater); //쿼터
            Buttons[(int)BETTING.HARF] = Convert.ToByte(half); //하프
            Buttons[(int)BETTING.DIE] = Convert.ToByte(die); //다이
            Buttons[(int)BETTING.CHECK] = Convert.ToByte(check); //체크
            Buttons[(int)BETTING.DDADDANG] = Convert.ToByte(ddadang); //따당
        }
        private void next_betting_turn()
        {
            //게임 상태 모든 유저에게 정보 전송
            send_player_statics();

            int callDieUserCount = this.engine.startPlayerCount - 1;

            if (this.engine.callDiePlayerCount == this.engine.deadPlayerCount && callDieUserCount == this.engine.deadPlayerCount)
            {
                // 모두 다이 했을 경우
                GameOver(true);
            }
            else if (this.engine.callDiePlayerCount == callDieUserCount)
            {
                NextRound();
                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.None) continue;
                    player.Value.agent.initPossibleRaceCount(this.engine.currentRound, ChanFree, this.ChanKind);
                }
            }
            else
            {
                NextBetTurn();
            }
        }
        private void NextBetTurn()
        {
            this.engine.set_next_player_current(PlayersGaming);
            CPlayer currentPlayer = current_player();
            bool Call = true;
            bool Race = false;
            bool Ddadang = false;
            bool Bbing = false;
            bool Check = false;

            //// 초반 보스다이 보스베팅 카피
            //if(currentPlayer == engine.BossPlayer &&
            //    currentPlayer.agent.callMoney == 0 &&
            //    engine.prevBetType == BETTING.DIE) // 보스가 죽어서 보스가된 플레이어
            //{
            //    roundstart_boss_betting();
            //    return;
            //}

            if (currentPlayer == engine.BossPlayer && currentPlayer.agent.callMoney == 0 && engine.prevBetType == BETTING.BASE)
            {
                Call = false;
                Check = true;
            }

            // 초반 보스다이 삥콜첵 확인
            if (currentPlayer.agent.callMoney == 0 && currentPlayer.agent.haveMoney >= this.BaseMoney)
            {
                Bbing = true;
            }

            // 삥제한 룰 추가
            /* 1. 아침, 점심에
             * 2. 패스가 없으면 삥 전부 비활성화
             * 3. 패스가 있으면 패스한 플레이어 포함 패스플레이어 자리부터 뒷자리까지 삥버튼 비활성화  => 현재 보스가 패스했으면 삥 전부 비활성화
             */
            if (Bbing == true)
            {
                if (engine.currentRound == GameRound.MORNING || engine.currentRound == GameRound.AFTERNOON)
                {
                    if (engine.PassCount == 0)
                    {
                        Bbing = false;
                    }
                    else
                    {
                        if (engine.BossPlayer.agent.roundChange[(int)engine.currentRound - 1] == 5)
                        {
                            Bbing = false;
                        }
                    }
                }
            }

            // 올인, 레이스 횟수 확인
            if ((currentPlayer.agent.haveMoney > 0 && currentPlayer.agent.getPossibleRaceCount() != 0))
            {
                if (ChanFree == false)
                {
                    foreach (var player in PlayersGaming)
                    {
                        if (player.Value.status == UserStatus.None) continue;
                        if (player.Value.agent.isDeadPlayer) continue;
                        if (player.Value.agent.haveMoney <= 0) continue;
                        if (player.Value.player_index == currentPlayer.player_index) continue;

                        Race = true;
                        break;
                    }
                }
                else
                {
                    Race = true;
                }

                if (currentPlayer.agent.haveMoney < currentPlayer.agent.callMoney) Race = false;

                if (Bbing == true && currentPlayer.agent.haveMoney < this.BaseMoney)
                {
                    // 삥을 할 수 있을때 돈이 없으면 삥 불가
                    Bbing = false;
                }

                if (Race &&
                    currentPlayer.agent.callMoney > 0 &&
                    (this.engine.prevBetType == BETTING.CALL ||
                    this.engine.prevBetType == BETTING.BBING ||
                    this.engine.prevBetType == BETTING.QUATER ||
                    this.engine.prevBetType == BETTING.HARF ||
                    this.engine.prevBetType == BETTING.DDADDANG))
                {
                    Ddadang = true;
                }
            }

            bool canDie = true;
            if (currentPlayer.agent.haveMoney <= 0 && ChanFree == false) // 올인이면 다이 X
            //if ((currentPlayer.agent.haveMoney <= 0 && ChanFree == false) // 올인이면 다이 X
            //    || (engine.currentRound == GameRound.START && engine.FirstRaise == false)) // 패트때 레이즈 없으면 다이 X
            {
                canDie = false;
            }
            else if (currentPlayer.agent.callMoney == 0)  // 콜을 받아야 되는데 나머지 올인이면 다이 X
            {
                int AlivePlayerCnt = 0;
                int AllinPlayerCnt = 0;
                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.None) continue;
                    if (player.Value.player_index == currentPlayer.player_index) continue;
                    if (player.Value.agent.isDeadPlayer == false)
                    {
                        ++AlivePlayerCnt;
                        if (player.Value.agent.haveMoney <= 0)
                            ++AllinPlayerCnt;
                    }

                }
                if (AlivePlayerCnt == AllinPlayerCnt && ChanFree == false)
                {
                    canDie = false;
                }
            }

            if (Race == false)
                Bbing = false;
            //currentPlayer.agent.Buttons = SetButtonActive(Call, Bbing, false, Race, canDie, Check, Ddadang);
            //if (ChanKind == ChannelKind.무료2채널 && engine.currentRound == GameRound.START && engine.prevBetType == BETTING.BBING)
            //{
            //    // 2번째 플레이어 첫레이스 하프 고정 (돈 없으면 콜)
            //    SetButtonActive(currentPlayer.agent.Buttons, !Race, false, false, Race, false, false, false);
            //}
            //else
            {
                SetButtonActive(currentPlayer.agent.Buttons, Call, Bbing, false, Race, canDie, Check, false);
            }

            send_request_betting(currentPlayer);
        }
        private void NextRound()
        {
            //다음 라운드
            //만일 현재라운드 저녘이라면 게임종료
            //BPROTOCOL.SERVER_CHANGE_ROUND
            if (this.engine.currentRound == GameRound.START)
            {
                this.engine.currentRound = GameRound.MORNING;
                RoundReset();
                send_change_round(this.engine.currentRound);
                StartChangeCard();
            }
            else if (this.engine.currentRound == GameRound.MORNING)
            {
                this.engine.currentRound = GameRound.AFTERNOON;
                RoundReset();
                send_change_round(this.engine.currentRound);
                StartChangeCard();
            }
            else if (this.engine.currentRound == GameRound.AFTERNOON)
            {
                this.engine.currentRound = GameRound.EVENING;
                RoundReset();
                send_change_round(this.engine.currentRound);
                StartChangeCard();
            }
            else if (this.engine.currentRound == GameRound.EVENING)
            {
                //게임 종료
                this.engine.currentRound = GameRound.START;

                GameOver();
            }
            clearCallDieCount();
            //this.engine.callDiePlayerCount = 0;

        }
        void RoundReset()
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;

                player.Value.agent.raiseMoney = 0;
                player.Value.agent.callMoney = 0;
            }
            this.engine.prevBetType = BETTING.BASE;

            engine.SetCurrentPlayer(engine.BossPlayer);
            CPlayer currentPlayer = current_player();
            foreach (var player in PlayersGaming)
            {
                if (currentPlayer.player_index == engine.BossPlayer.player_index && currentPlayer.agent.isDeadPlayer == true)
                {
                    this.engine.set_next_player_current(PlayersGaming);
                    currentPlayer = current_player();
                    engine.SetBoss(currentPlayer);
                }
            }

            // 패스 정보 초기화
            engine.PassCount = 0;

            engine.FirstRaise = false;
            // 만약 보스가 없으면 오퍼레이터가 보스함
            //if (GetBossIndex() == byte.MaxValue)
            //{
            //GetOperator().agent.SetBoss(true);
            //}

            //send_bossindex();
        }
        private void GameOver(bool alldie = false)
        {
            //this.engine.current_player_index = GetBossIndex();
            //CPlayer user = current_player();
            //if (user.agent.isBoss == true && user.agent.isDeadPlayer == true)
            //{
            //    this.engine.move_to_next_player();
            //    user = current_player();
            //    user.agent.SetBoss(true);
            //    send_bossindex();
            //}

            engine.SetCurrentPlayer(engine.BossPlayer);

            // 기권승은 패 오픈 안함
            if (alldie == false)
            {
                on_send_card_open_all();
            }

            RoomGameEnd();
        }
        private long event_jackpot_gameover()
        {
            long JackPotResultMoney = 0;
            int jackPotMag = 0;
            // 잭팟 머니 적용
            switch (this.jackPotType)
            {
                case EVENT_JACKPOT_TYPE.X1:
                    {
                        jackPotMag = 50;
                    }
                    break;
                case EVENT_JACKPOT_TYPE.X2:
                    {
                        jackPotMag = 100;
                    }
                    break;
                case EVENT_JACKPOT_TYPE.X3:
                    {
                        jackPotMag = 200;
                    }
                    break;
            }
            byte winners = 0;
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;

                if (player.Value.agent.isWin)
                {
                    ++winners;
                }
            }
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;

                if (player.Value.agent.isWin)
                {
                    //JackPotResultMoney = Math.Min(200000, this.engine.baseMoney * jackPotMag);
                    JackPotResultMoney = this.engine.baseMoney * jackPotMag;
                    player.Value.ChangeMoney += JackPotResultMoney / winners;
                    player.Value.agent.earnedMoney += JackPotResultMoney / winners;
                }
            }
            return JackPotResultMoney;
        }
        void send_event_jackpot_golf(long eventMoney, string nickName)
        {
            CMessage Msg = new CMessage();
            Rmi.Marshaler.Write(Msg, (int)eventMoney);
            Rmi.Marshaler.Write(Msg, nickName);

            foreach (var player in PlayersConnect)
            {
                Send(player.Value, Msg, SS.Common.GameEventNotify);
            }
        }
        /*
        private void send_event_mileage_refresh(CPlayer owner)
        {
            CMessage newmsg = new CMessage();
            PacketType msgID = (PacketType)Rmi.Common.SC_EVENT_MILEAGE_REFRESH;
            CPackOption CPackOption.Basic = CPackOption.Basic;
            newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);

            Rmi.Marshaler.Write(newmsg, (int)(((double)owner.data.mileage / (double)RoomSvr.CheerPointLimit) * 100)); // 0 ~ 100
            Rmi.Marshaler.Write(newmsg, (int)owner.data.charm);

            Proxy.PacketSend(owner.RelayRemote, CPackOption.Basic, newmsg.Data);
        }
        */
        public bool GameEnding;
        //int[] ranking = new int[6] { 5, 5, 5, 5, 5, 5 };
        //long[] earnedMoney = new long[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //long[] earnedMoney2 = new long[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //long[] arrMoney = new long[5] { 0, 0, 0, 0, 0 };
        bool CalcEarnedMoney(bool FoldWin, ref long BbingCutMoney, ref int BbingCutPlayer, ref bool isCardWin)
        {
            List<CPlayer> rankList = new List<CPlayer>();
            List<CPlayer> betmoneyList = new List<CPlayer>();
            int[] rankCnt = new int[PlayersGaming.Count];
            for (int rc = 0; rc < rankCnt.Length; ++rc) rankCnt[rc] = 0;
            int i = 0;

            rankList.Clear();
            betmoneyList.Clear();
            Array.Clear(rankCnt, 0, rankCnt.Length);
            //Array.Clear(ranking, 5, ranking.Length);
            //Array.Clear(earnedMoney, 0, earnedMoney.Length);
            //Array.Clear(earnedMoney2, 0, earnedMoney2.Length);
            //Array.Clear(arrMoney, 0, arrMoney.Length);

            double FeeRate;

            //if (ChanKind == ChannelKind.무료2채널)
            //    FeeRate = 0.5;
            //else
                FeeRate = 1;

            // 딜러피
            double dealerPee = RoomSvr.DealerFee / 100.0 * FeeRate;
            double jackpotPee = RoomSvr.JackPotRate / 100.0 * FeeRate;
            double totalPee = dealerPee + jackpotPee;

            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                player.Value.agent.totalpaideMoney -= player.Value.agent.paidMoney;
                player.Value.agent.betMoney = player.Value.agent.paidMoney;
            }

            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                rankList.Add(player.Value);
                betmoneyList.Add(player.Value);
                rankCnt[player.Value.agent.handRank]++;
            }

            if (rankList.Count <= 0)
            {
                Log._log.FatalFormat("CalcEarnedMoney Error. rankList.Count == 0.");
                return false;
            }

            rankList.Sort((e1, e2) => e1.agent.handRank.CompareTo(e2.agent.handRank));
            betmoneyList.Sort((e1, e2) => e2.agent.paidMoney.CompareTo(e1.agent.paidMoney));

            long[] returnMoney = new long[PlayersGaming.Count];
            for (int rc = 0; rc < returnMoney.Length; ++rc) returnMoney[rc] = 0;
            long returnSum = 0;

            long returnBet = 0;
            if (FoldWin) // 기권승
            {
                // 1등유저가 베팅한 돈이 가장 많은 돈을 베팅한 경우
                // 오버베팅이 발생했을수 있으므로 그 금액을 빼준다 ( 0일수도 있음 )

                if (rankList[0].agent.paidMoney == betmoneyList[0].agent.paidMoney && betmoneyList.Count > 1)
                {
                    returnBet = betmoneyList[0].agent.paidMoney - betmoneyList[1].agent.paidMoney;

                    betmoneyList[0].agent.paidMoney -= returnBet;
                    betmoneyList[0].agent.addMoney(returnBet);
                    betmoneyList[0].agent.totalpaideMoney += returnBet;
                }

                engine.totalMoney = 0;
                long totalBetMoney = 0;
                for (i = 0; i < rankList.Count; i++)
                {
                    engine.totalMoney += rankList[i].agent.betMoney;
                    totalBetMoney += rankList[i].agent.paidMoney;
                }

                engine.totalMoney += LeavePlayerBetMoney;
                totalBetMoney += LeavePlayerBetMoney;

                // 삥컷 확인
                if (RoomSvr.BbingCutEnable == true)
                {
                    if (engine.startPlayerCount >= 4 && RoomSvr.BbingCutTotalBetMoney * BaseMoney <= totalBetMoney && RoomSvr.BbingCutWinnerMoney * BaseMoney <= rankList[0].agent.paidMoney)
                    {
                        BbingCutPlayer = rankList[0].data.ID;

                        totalBetMoney -= BaseMoney;
                        rankList[0].agent.paidMoney -= BaseMoney;
                        BbingCutMoney = BaseMoney;
                    }
                }

                rankList[0].agent.isWin = true;
                rankList[0].agent.earnedMoney = (long)(totalBetMoney * (1.0 - totalPee));
                rankList[0].agent.totalpaideMoney += rankList[0].agent.earnedMoney;
            }
            else
            {
                isCardWin = true;
                engine.totalMoney = 0;
                for (i = 0; i < rankList.Count; i++)
                {
                    engine.totalMoney += rankList[i].agent.paidMoney;
                }

                // 오버베팅한 유저의 돈을 돌려준다.
                returnBet = 0;
                if (betmoneyList[0].agent.isDeadPlayer == false && betmoneyList.Count > 1)
                {
                    // 다이하지 않은 유저중 제일 많이 베팅한 유저의 머니와 그다음 많이 베팅한 유저의 머니를 비교한다.
                    returnBet = betmoneyList[0].agent.paidMoney - betmoneyList[1].agent.paidMoney;
                    // 베팅머니에서 오버베팅머니를 빼준다.
                    betmoneyList[0].agent.paidMoney -= returnBet;
                    // 보유머니에 오버베팅머니를 합산해준다.
                    betmoneyList[0].agent.addMoney(returnBet);
                    betmoneyList[0].agent.totalpaideMoney += returnBet;
                }

                long totalMoney = 0;

                engine.totalMoney += LeavePlayerBetMoney;
                totalMoney += LeavePlayerBetMoney;
                // 1등이 가져가야 할돈을 계산한다.		
                long getSum = 0;
                for (i = 0; i < rankCnt[0]; i++)
                {
                    // 가져갈돈( 비율대로 나눌때 분모) 합산.
                    getSum += rankList[i].agent.paidMoney;
                    rankList[i].agent.isWin = true;
                }

                totalMoney += getSum;
                for (i = rankCnt[0]; i < rankList.Count; i++)
                {
                    returnMoney[i] = rankList[i].agent.paidMoney - getSum;
                    if (returnMoney[i] < 0)
                        returnMoney[i] = 0;

                    // 돌려줘야할돈 총합 이게 0이면 
                    returnSum += returnMoney[i];
                    // 해당등수한테 갈 총액계산		
                    totalMoney += (rankList[i].agent.paidMoney - returnMoney[i]);
                }

                // 삥컷 확인
                if (RoomSvr.BbingCutEnable == true && engine.startPlayerCount >= 4 && rankCnt[0] == 1 && RoomSvr.BbingCutTotalBetMoney * BaseMoney <= getSum)
                {
                    if (RoomSvr.BbingCutWinnerMoney * BaseMoney <= rankList[0].agent.paidMoney)
                    {
                        getSum -= BaseMoney;
                        totalMoney -= BaseMoney;
                        rankList[0].agent.paidMoney -= BaseMoney;
                        BbingCutMoney += BaseMoney;
                        BbingCutPlayer = rankList[0].data.ID;
                    }
                }

                // 1등 들에게 베팅비율대로 머니를 나눠준다.
                for (i = 0; i < rankCnt[0]; i++)
                {
                    long getMoney = (long)((double)totalMoney * (double)((double)rankList[i].agent.paidMoney / (double)getSum));
                    rankList[i].agent.earnedMoney = (long)(getMoney * (1.0 - totalPee));
                    rankList[i].agent.totalpaideMoney += rankList[i].agent.earnedMoney;
                }

                // 돌려줄돈이 있을경우
                if (returnSum > 0)
                {
                    // 다음순위에게 금액을 배분한다.!!
                    int iRank = 1;
                    // 현재랭킹위치 2등
                    //int iRankPos = 1;	
                    // 해당등수유저들이 나눠받을 액수의 총액
                    getSum = 0;
                    // 해당등수 유저들이 개인별 돌려받을 기본금액의 총합( 나누줄때 분모가된다. )
                    returnSum = 0;
                    // 나눠주고남은금액 ( 다음등수에게 넘겨줄 금액 )
                    long remainSum = 0;
                    int iDieCnt = 0;
                    // 1등유저들을 뺀 그다음 유저부터 처리		
                    for (i = rankCnt[0]; i < rankList.Count(); ++i) // 1 ~ 5 // 4
                    {
                        getSum = 0;
                        returnSum = 0;
                        for (int j = 0; j < rankCnt[iRank]; ++j) // 0 ~ 1 // 1
                        {
                            // 다이한 유저는 처리하지 않는다.
                            if (rankList[i + j].agent.isDeadPlayer == true)
                            {
                                ++iDieCnt;
                                continue;
                            }

                            // 돌려받을 돈이 남아있지 않은 등수의 유저는 돌려주는 계산에서 뺀다.
                            if (returnMoney[i + j] <= 0)
                                continue;

                            getSum += returnMoney[i + j];
                            returnSum += returnMoney[i + j];
                        }

                        //	다음순위에게 나눠줘야하는 금액의 총액계산  
                        for (int k = i + rankCnt[iRank]; k < rankList.Count; k++)
                        {
                            // 돌려줄돈이 남은게 없을경우 다음유저로 이동
                            if (returnMoney[k] == 0)
                                continue;

                            // 남은 돈이 이번에 나눠줄돈보다 적거나 같으면 전부 준다.
                            if (returnSum >= returnMoney[k])
                            {
                                getSum += returnMoney[k];
                                returnMoney[k] = 0;
                            }
                            else
                            {
                                // 남은돈이 나눠줄돈보다 클경우
                                // 해당등수 유저의 돌려받을돈만큼만 합산한다.
                                getSum += returnSum;
                                returnMoney[k] -= returnSum;
                            }
                        }

                        // 이 등수의 유저들에게 나눠줄 금액이 있는 경우
                        if (getSum > 0)
                        {
                            for (int j = 0; j < rankCnt[iRank]; j++)
                            {
                                if (returnMoney[i + j] > 0)
                                {
                                    // 비율만큼 나눠준다.
                                    long getMoney = (long)((double)getSum * (double)(((double)returnMoney[i + j] / (double)returnSum)));
                                    rankList[i + j].agent.earnedMoney = (long)(getMoney * (1.0 - totalPee));
                                    rankList[i + j].agent.totalpaideMoney += (long)(getMoney * (1.0 - totalPee));
                                }
                                else
                                {
                                    // 돌려받을돈이 없는 유저는 0원세팅
                                    rankList[i + j].agent.earnedMoney = 0;
                                }
                            }
                        }

                        // 돌려줄돈이 남았는지 체크한다.
                        for (int k = i + rankCnt[iRank]; k < rankList.Count; k++)
                        {
                            remainSum += returnMoney[k];
                        }

                        // 다 나눠준 경우 
                        if (remainSum == 0)
                        {
                            // 나머지 유저들의 획득머니를 계산한다.
                            for (int k = i + rankCnt[iRank]; k < rankList.Count; k++)
                            {
                                //rankList[k].agent.earnedMoney = 0;
                            }
                            break;
                        }
                        else if (iDieCnt == rankCnt[iRank])
                        {
                            // 남은 돈이 있는데 해당순위 유저가 모두 다이했다!! 그럼 그밑도 모두 다이라 더이상 받을 유저가 없다!!
                            // 낙전발생 로그기록

                            int a = 0;

                            break;
                        }
                        iDieCnt = 0;
                        i += (rankCnt[iRank++] - 1);	// 해당등수까지 몇명인지 누적하고 다음등수로 이동한다.
                    }
                }
            }

            for (i = 0; i < rankList.Count; i++)
            {
                rankList[i].JackPotDealMoney = (long)((double)(rankList[i].agent.paidMoney) * jackpotPee);
                rankList[i].GameDealMoney = (long)((double)(rankList[i].agent.paidMoney) * dealerPee);

                DealerFeeMoney += rankList[i].GameDealMoney;
                JackPotFeeMoney += rankList[i].JackPotDealMoney;
            }

            // 오버배팅 딜비 처리
            if (returnBet > 0)
            {
                long overPee = (long)((double)(returnBet) * totalPee);
                long jackpotPeeAdd = (long)((double)(returnBet) * jackpotPee);
                long gameDealPeeAdd = overPee - jackpotPeeAdd;

                betmoneyList[0].agent.addMoney(-overPee);
                betmoneyList[0].agent.earnedMoney -= overPee;
                betmoneyList[0].agent.totalpaideMoney -= overPee;

                betmoneyList[0].JackPotDealMoney += jackpotPeeAdd;
                betmoneyList[0].GameDealMoney += gameDealPeeAdd;
                JackPotFeeMoney += jackpotPeeAdd;
                DealerFeeMoney += gameDealPeeAdd;
            }

            // 낙전을 계산해서 합산해준다.
            long totalDeal = 0;
            long totalEarn = 0;
            long totalBet = 0;
            for (i = 0; i < rankList.Count; i++)
            {
                totalDeal += (rankList[i].JackPotDealMoney + rankList[i].GameDealMoney);
                totalEarn += rankList[i].agent.earnedMoney;
                totalBet += rankList[i].agent.paidMoney;
            }

            // 다이퇴장 플레이어 딜비 적용
            totalBet += LeavePlayerBetMoney;
            totalDeal += LeavePlayerDealMoney + LeavePlayerJackpotMoney;
            JackPotFeeMoney += LeavePlayerJackpotMoney;
            DealerFeeMoney += LeavePlayerDealMoney;

            long diff = totalBet - totalDeal - totalEarn;
            rankList[0].agent.earnedMoney += diff;
            rankList[0].agent.totalpaideMoney += diff;

            //// 공컷
            //if (RoomSvr.WinCutEnable == true && engine.startPlayerCount >= 4 && rankCnt[0] == 1 && RoomSvr.WinCutTotalBetMoney * BaseMoney <= rankList[0].agent.earnedMoney)
            //{
            //    WinCutPlayer = rankList[0].data.ID;
            //    WinCutMoney = (long)(rankList[0].agent.earnedMoney * RoomSvr.WinCutRate);

            //    rankList[0].agent.earnedMoney -= WinCutMoney;
            //}

            return rankCnt[0] > 1;
        }

        void gameEnd()
        {
            GameEnding = true;

            // 기권승 여부
            bool FoldWin = (GetLivePlayersCount() == 1);

            // 핸드 계산
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                player.Value.agent.CalcHandScore();
            }

            // 족보 비교
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                foreach (var player2 in PlayersGaming)
                {
                    if (player2.Value.status == UserStatus.None) continue;

                    if ((player.Value.agent.isDeadPlayer == true && player2.Value.agent.isDeadPlayer == false)
                        || (player.Value.agent.isDeadPlayer == false && player2.Value.agent.isDeadPlayer == false && player.Value.agent.handScore < player2.Value.agent.handScore))
                    {
                        ++player.Value.agent.handRank;
                    }
                }
            }

            // 머니 처리 계산
            int BbingCutPlayer = 0, WinCutPlayer = 0;
            long BbingCutMoney = 0, WinCutMoney = 0;
            bool isCardWin = false;
            bool MultiWin = CalcEarnedMoney(FoldWin, ref BbingCutMoney, ref BbingCutPlayer, ref isCardWin);

            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                player.Value.ChangeMoney = player.Value.agent.totalpaideMoney;

                //player.Value.agent.money_var += player.Value.ChangeMoney;
                //player.Value.agent.addMoney(player.Value.agent.earnedMoney);
            }
            /*
            // 공컷
            if (RoomSvr.WinCutEnable == true && engine.startPlayerCount >= 4 && rankCnt[0] == 1 && RoomSvr.WinCutTotalBetMoney * BaseMoney <= rankList[0].agent.earnedMoney)
            {
                WinCutPlayer = rankList[0].data.ID;
                WinCutMoney = (long)(rankList[0].agent.earnedMoney * RoomSvr.WinCutRate);

                rankList[0].agent.earnedMoney -= WinCutMoney;
            }
             */

            // 골프 잭팟
            long EventGolfMoney = 0;
            int EventGolfPlayerId = 0;
            string EventGolfPlayerName = "";
            int EventGolfType = 0;
            if (RoomSvr.GolfRewardRatio > 0 && engine.startPlayerCount > 2)
            {
                EventGolfType = 2; // 골프 이벤트

                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.None) continue;
                    if (player.Value.agent.userCardInfo.GetResult() == 7)
                    {
                        EventGolfPlayerId = player.Value.data.ID;
                        long Money = BaseMoney * RoomSvr.GolfRewardRatio;
                        player.Value.ChangeMoney += Money;
                        player.Value.agent.earnedMoney += Money;
                        EventGolfMoney = Money;
                        EventGolfPlayerName = player.Value.data.nickName;
                        break;
                    }
                }
            }

            bool Z1Event = false;
            // 이벤트 잭팟
            long JackPotResultMoney = 0;
            string JackPotResultPlayerName = "";
            int JackPotResultPlayerId = 0;
            int JackPotResultType = 0;
            short JackPotRewardType = 0;
            if (jackPotType != EVENT_JACKPOT_TYPE.NONE)
            {
                if (jackPotType == EVENT_JACKPOT_TYPE.Z1) // 돌발 이벤트
                {
                    Z1Event = true;
                    JackPotResultType = 3;
                    //JackPotResultMoney = Math.Min(200000, this.engine.baseMoney * 100);
                    JackPotResultMoney = this.engine.baseMoney * 100;
                    JackPotResultPlayerId = JackpotSuddenEventWinner.data.ID;

                    JackpotSuddenEventWinner.ChangeMoney += JackPotResultMoney;
                    JackpotSuddenEventWinner.agent.earnedMoney += JackPotResultMoney;

                    JackpotSuddenEventWinner = null;
                }
                else // 장비 이벤트
                {
                    JackPotResultType = 1;
                    JackPotResultMoney = event_jackpot_gameover();
                    send_event_jackpot_end();
                    JackPotRewardType = (short)this.jackPotType;
                }
                jackPotType = EVENT_JACKPOT_TYPE.NONE;
            }

            if (MultiWin == false && isCardWin == true && Z1Event == false && /*engine.LastRaise == true &&*/ engine.startPlayerCount >= 4 && RoomSvr.WinCutEnable == true && RoomSvr.WinCutTotalBetMoney * BaseMoney <= engine.totalMoney)
            {
                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.None) continue;

                    if (player.Value.agent.isWin)
                    {
                        // 공컷
                        if (RoomSvr.WinCutWinnerBetMoney * BaseMoney <= player.Value.agent.totalpaideMoney && RoomSvr.WinCutWinnerMoney * BaseMoney <= player.Value.ChangeMoney)
                        {
                            WinCutPlayer = player.Value.data.ID;
                            WinCutMoney = (long)(player.Value.ChangeMoney * RoomSvr.WinCutRate);

                            player.Value.ChangeMoney -= WinCutMoney;
                            player.Value.agent.earnedMoney -= WinCutMoney;
                        }
                        break;
                    }
                }
            }

            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;

                player.Value.agent.money_var += player.Value.ChangeMoney;
                player.Value.agent.addMoney(player.Value.agent.earnedMoney);
            }
            // DB, 로그 정리
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                //CPlayerData Data = new CPlayerData();

                // 승패, 마일리지
                if (player.Value.agent.isWin == true)
                {
                    if (MultiWin)
                    {
                        player.Value.GameResult = 3;
                        JackPotResultPlayerName = player.Value.data.nickName;
                        JackPotResultPlayerId = player.Value.data.ID;
                    }
                    else
                    {
                        player.Value.GameResult = 1;
                        JackPotResultPlayerName = player.Value.data.nickName;
                        JackPotResultPlayerId = player.Value.data.ID;
                    }
                    ++player.Value.data.winCount;
                }
                else
                {
                    player.Value.GameResult = 2;
                    ++player.Value.data.loseCount;
                }

                gameLog.PlayerLog[player.Value.player_index].ChangeMoney = player.Value.ChangeMoney;
            }
            gameLog.Result();
            // 게임 로그 처리
            for (int i = 0; i < gameLog.PlayerLog.Length; ++i)
            {
                if (gameLog.PlayerLog[i].UserCard == "") continue;

                string result;
                CPlayer player = this_player((byte)i);
                if (player == null || player.status == UserStatus.None || player.agent.isDeadPlayer == true)
                {
                    result = "4"; // 다이
                }
                else if (player.GameResult == 3) //
                {
                    result = "0"; // 무승부
                }
                else if (player.GameResult == 1)
                {
                    if (FoldWin)
                    {
                        result = "2"; // 기권승(올다이)
                    }
                    else
                    {
                        result = "1"; // 승리
                    }
                }
                else
                {
                    result = "3"; // 패배
                }

                gameLog.PlayerLog[i].UserCard = string.Format("{0}_{1}", gameLog.PlayerLog[i].UserCard, result);
                gameLog.ResultPlayer(i, result, gameLog.PlayerLog[i].UserCard, gameLog.PlayerLog[i].ChangeMoney);
            }
            long PlayedGameMoney = this.engine.totalMoney;
            CGameLog gameLogDB = (CGameLog)gameLog.End();
            int dbResult = 1;
            Task.Run(() =>
            {
                try
                {
                    var db = RoomSvr.db;
                    // 게임 룸 로그 저장
                    db.LogGameBadugi.Insert(PlayId: gameLogDB.LogId, RoomId: roomID, ChannelId: ChanId, RoomNumber: roomNumber, BetMoney: gameLogDB.BetMoney, UserId1: gameLogDB.PlayerLog[0].UserId, UserCard1: gameLogDB.PlayerLog[0].UserCard, UserId2: gameLogDB.PlayerLog[1].UserId, UserCard2: gameLogDB.PlayerLog[1].UserCard, UserId3: gameLogDB.PlayerLog[2].UserId, UserCard3: gameLogDB.PlayerLog[2].UserCard, UserId4: gameLogDB.PlayerLog[3].UserId, UserCard4: gameLogDB.PlayerLog[3].UserCard, UserId5: gameLogDB.PlayerLog[4].UserId, UserCard5: gameLogDB.PlayerLog[4].UserCard, StartTime: gameLogDB.StratTime, EndTime: gameLogDB.EndTime, PlayLog: gameLog.PlayLog);

                    foreach (var player in PlayersGaming)
                    {
                        if (player.Value.status == UserStatus.None) continue;

                        bool isDummy = player.Value.status == UserStatus.RoomPlayOut;
                        double FeeRate;
                        //if (ChanKind == ChannelKind.무료2채널)
                        //    FeeRate = 0.5;
                        //else
                            FeeRate = 1;
                        foreach (var row in db.Room_BadugiResultPlayer(player.Value.data.ID, player.Value.ChangeMoney, player.Value.agent.betMoney, player.Value.GameDealMoney, player.Value.GameResult, (int)ChanType, RoomSvr.DealerFee * FeeRate, ChanId, isDummy))
                        {
                            player.Value.UserLevel = row.MemberLevel;
                            player.Value.GameLevel = row.GameLevel;
                            player.Value.GameLevel2 = row.GameLevel2;
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
                        db.LogGamePlayer.Insert(UserId: player.Value.data.ID, ShopID: player.Value.data.shopId, GameId: RoomSvr.GameId, ChannelId: ChanId, RoomId: roomNumber, PlayId: gameLogDB.LogId, Result: player.Value.GameResult, ChangeMoney: player.Value.ChangeMoney, AfterMoney: player.Value.agent.haveMoney, Time: gameLogDB.EndTime, GameDealMoney: player.Value.GameDealMoney, JackPotDealMoney: player.Value.JackPotDealMoney, BetMoney: player.Value.agent.betMoney, BaseMoney: BaseMoney);
                    }

                    // 게임 정보 처리
                    db.Room_BadugiResultGame(ChanId, PlayedGameMoney, DealerFeeMoney, JackPotFeeMoney, (int)ChanType, EventGolfMoney, JackPotResultMoney, BbingCutMoney + WinCutMoney);

                    if (EventGolfMoney != 0)
                    {
                        // 잭팟 로그 기록
                        db.LogJackPot.Insert(GameID: RoomSvr.GameId, ChannelId: ChanId, RoomNumber: roomNumber, PlayId: gameLogDB.LogId, JackPotType: EventGolfType, BetMoney: gameLogDB.BetMoney, JackPotMoney: EventGolfMoney, UserId: EventGolfPlayerId);
                    }
                    if (JackPotResultMoney != 0)
                    {
                        // 잭팟 로그 기록
                        db.LogJackPot.Insert(GameID: RoomSvr.GameId, ChannelId: ChanId, RoomNumber: roomNumber, PlayId: gameLogDB.LogId, JackPotType: JackPotResultType, BetMoney: gameLogDB.BetMoney, JackPotMoney: JackPotResultMoney, UserId: JackPotResultPlayerId);

                        // 잭팟 헌터 이벤트 프로시저
                        //db.Room_BadugiJackpotHunter(JackPotResultPlayerId, this.stake, JackPotRewardType);
                    }
                    if (BbingCutMoney > 0)
                    {
                        // 삥컷 로그
                        db.LogGameBadugiBbingCut.Insert(GameId: RoomSvr.GameId, ChannelId: ChanId, PlayId: gameLogDB.LogId, Player: BbingCutPlayer, CutMoney: BbingCutMoney);
                    }
                    if (WinCutMoney > 0)
                    {
                        // 공컷 로그
                        db.LogGameBadugiWinCut.Insert(GameId: RoomSvr.GameId, ChannelId: ChanId, PlayId: gameLogDB.LogId, Player: WinCutPlayer, CutMoney: WinCutMoney);
                    }
                }
                catch (Exception e)
                {
                    ErrorLog(e);
                    Log._log.ErrorFormat("{0} 게임 결과 저장 : 예외발생 {1}", gameLogDB.LogId, e.ToString());
                    dbResult = 0;
                }

                send_players_info();

                if (EventGolfMoney != 0)
                {
                    send_event_jackpot_golf(EventGolfMoney, EventGolfPlayerName);
                    // 골프잭팟 공지
                    //Proxy.RoomLobbyEventEnd((RemoteID)this.remote_lobby, CPackOption.Basic, this.roomID, EventGolfType, EventGolfPlayerName, EventGolfMoney);
                }
                if (JackPotResultMoney != 0)
                {
                    // 판수잭팟 공지
                    //Proxy.RoomLobbyEventEnd((RemoteID)this.remote_lobby, CPackOption.Basic, this.roomID, JackPotResultType, JackPotResultPlayerName, JackPotResultMoney);
                }
                send_game_result(DealerFeeMoney + JackPotFeeMoney, MultiWin, FoldWin);

                foreach (var player in PlayersGaming)
                {
                    player.Value.DB = false;
                    if (player.Value.status == UserStatus.RoomPlayOut) continue;
                    player.Value.status = UserStatus.RoomStay;
                }

                // 게임이 끝날때마다 방장(보스)를 시계방향으로 돌린다.
#if DEBUG
                SetOperatorNext();
#else
                SetOperatorNext();
#endif
                // 정상 판인지 확인
                if (PlayersGaming.Count >= 4) // 4명보다 많을때 판수 인정
                {
                    //알고리즘 필요
                    RoomSvr.EventCountUp();
                }

                this.roomWaitTime = DateTime.Now.AddSeconds(3);
                roomWaitTime2 = DateTime.Now.AddSeconds(7);


                this.status = RoomStatus.Stay;

                RoomSvr.DummyPlayerLeave(this);

                if (dbResult != 1)
                {
                    Log._log.ErrorFormat("{0}채널 {1}번방 게임결과 처리 실패", ChanId, roomNumber);
                    foreach (var player in PlayersGaming)
                    {
                        Log._log.FatalFormat("{0}채널 {1}번방 게임결과 처리 실패 유저 {2}: ", ChanId, roomNumber, player.Value.data.userID);
                    }
                }
                GameEnding = false;
            }
                );
        }
        private void on_send_card_open()
        {
            send_change_turn(this.engine.CurrentPlayer.player_index);
            CMessage newmsg = new CMessage();
            string handRes = current_player().agent.GetUserHandName();
            List<CPlayerHand> playerHand = current_player().agent.userHands;
            Rmi.Marshaler.Write(newmsg, (byte)this.engine.CurrentPlayer.player_index);
            Rmi.Marshaler.Write(newmsg, (string)handRes);
            Rmi.Marshaler.Write(newmsg, (byte)playerHand.Count);
            for (int i = 0; i < playerHand.Count; i++)
            {
                Rmi.Marshaler.Write(newmsg, (byte)playerHand[i].card.m_nCardNum);
                Rmi.Marshaler.Write(newmsg, (byte)playerHand[i].card.m_nShape);
                Rmi.Marshaler.Write(newmsg, (byte)playerHand[i].card.m_btIsState);
            }

            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;
                if (player.Value.agent.status != UserGameStatus.None)
                    player.Value.agent.status = UserGameStatus.OpenCard;
                Send(player.Value, newmsg, SS.Common.GameCardOpen);
            }
        }
        private void on_send_card_open_all()
        {
            // 현재 플레이어부터 차례대로 카드 오픈
            byte openFirstPlayerIndex = current_player().player_index;
            byte openPlayerIndex = openFirstPlayerIndex;

            foreach (var player in PlayersGaming)
            {
                CPlayer openPlayer = this_player(openPlayerIndex);
                if (openPlayer == null || openPlayer.status == UserStatus.None || openPlayer.agent.isDeadPlayer == true)
                {
                    openPlayerIndex = get_next_live_player_index(openPlayerIndex);
                    if (openPlayerIndex == openFirstPlayerIndex)
                        break;
                    else
                        continue;
                }

                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, (bool)true);
                //if (player.Value == players.Values.Last())
                //{
                //    Rmi.Marshaler.Write(newmsg, (bool)true);
                //}
                //else
                //{
                //    Rmi.Marshaler.Write(newmsg, (bool)false);
                //}

                Rmi.Marshaler.Write(newmsg, (byte)openPlayerIndex);
                Rmi.Marshaler.Write(newmsg, (string)openPlayer.agent.GetUserHandName());

                List<CPlayerHand> playerHand = openPlayer.agent.userHands;
                Rmi.Marshaler.Write(newmsg, (byte)playerHand.Count);
                for (int j = 0; j < playerHand.Count; ++j)
                {
                    Rmi.Marshaler.Write(newmsg, (byte)playerHand[j].card.m_nCardNum);
                    Rmi.Marshaler.Write(newmsg, (byte)playerHand[j].card.m_nShape);
                    Rmi.Marshaler.Write(newmsg, (byte)playerHand[j].card.m_btIsState);
                }

                foreach (var player2 in PlayersConnect)
                {
                    if (player2.Value.PacketReady == false) continue;
                    Send(player2.Value, newmsg, SS.Common.GameCardOpen);
                }

                openPlayerIndex = get_next_live_player_index(openPlayerIndex);
                if (openPlayerIndex == openFirstPlayerIndex)
                    break;
                else
                    continue;
            }

        }
        private bool on_player_change_card(CPlayer owner, CMessage msg)
        {
            clear_received_protocol();

            if (owner.agent.status != UserGameStatus.ChangeCard) return false;
            owner.agent.status = UserGameStatus.Play;

            if (owner.player_index != this.engine.CurrentPlayer.player_index) return false;

            // 로그
            gameLog.CardChange(owner.player_index);

            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index); // 사용안함
            byte button;
            Rmi.Marshaler.Read(msg, out button);
            // 패킷 예외처리
            if (button < (byte)CHANGECARD.CHANGE || button > (byte)CHANGECARD.PASS)
            {
                button = (byte)CHANGECARD.PASS;
            }

            switch ((CHANGECARD)button)
            {
                case CHANGECARD.CHANGE:

                    byte changecardCount;
                    List<CPlayerHand> hands = new List<CPlayerHand>();

                    Rmi.Marshaler.Read(msg, out changecardCount);
                    if (changecardCount <= 0 || changecardCount > 4) // 패킷 예외처리
                        changecardCount = 1;

                    bool isHaveMade = false;
                    // 판에 있는 메이드 확인 (특정 메이드이상 있으면 그보다 높은 메이드 없도록)
                    if (RoomSvr.MadeLimit > 0)
                    {
                        foreach (var player in PlayersGaming)
                        {
                            if (player.Value.status == UserStatus.None) continue;
                            if (owner.player_index == player.Value.player_index) continue;

                            if (player.Value.agent.userCardInfo.m_nResult > 4 // 골프, 세컨드, 써드
                                || (player.Value.agent.userCardInfo.m_nResult == 4 && RoomSvr.MadeLimit >= player.Value.agent.userCardInfo.m_nTopNumber)) // 메이드
                            {
                                isHaveMade = true;
                                break;
                            }
                        }
                    }
                    /*
                    bool advantagePlayer = false;
                    // 판에 메이드가 없을경우 플레이어 어드벤티지 확률에 따라 좋은 패로 교환
                    if (isHaveMade == false)
                    {
                        Random rngRoom = new Random();
                        if (owner.advantagePoint > rngRoom.Next(0,100))
                        {
                            advantagePlayer = true;
                        }
                    }
                    */

                    // 베이스 체인지 보정
                    int BaseTop = 0;
                    int TwoBaseTop = 0;

                    if (owner.agent.userCardInfo.m_nResult == 3)
                    {
                        BaseTop = owner.agent.userCardInfo.m_nTopNumber;
                    }
                    // 투베이스 체인지 보정

                    else if (owner.agent.userCardInfo.m_nResult == 2)
                    {
                        TwoBaseTop = owner.agent.userCardInfo.m_nTopNumber;
                    }

                    for (int i = 0; i < changecardCount; ++i)
                    {
                        byte changecardIndex;
                        Rmi.Marshaler.Read(msg, out changecardIndex);
                        if (changecardIndex < 0 || changecardIndex >= 4) // 패킷 예외처리
                        {
                            changecardIndex = (byte)i;
                        }

                        //CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
                        byte cardNum;
                        Rmi.Marshaler.Read(msg, out cardNum); // 미사용
                        byte cardShape;
                        Rmi.Marshaler.Read(msg, out cardShape); // 미사용

                        var handCard = owner.agent.GetUserHands()[changecardIndex];

                        if (handCard != null)
                        {
                            this.engine.AddExcahngeCard(owner.agent.GetUserHands()[changecardIndex].card);
                            //card.m_nCardNum = owner.agent.GetUserHands()[changecardIndex].card.m_nCardNum;
                            //card.m_nShape = owner.agent.GetUserHands()[changecardIndex].card.m_nShape;
                        }
                        else
                        {
                            continue;
                        }

                        owner.agent.remove_card_to_hand(changecardIndex);
                        //this.engine.AddExcahngeCard(card);
                    }

                    // 교체 카드 지급
                    for (int i = 0; i < changecardCount; ++i)
                    {
                        int changecardIndex;
                        changecardIndex = owner.agent.get_null_card_index();
                        if (changecardIndex == -1) break;

                        CardInfo.sCARD_INFO exchangeCard = this.engine.getExchangeCard();

                        // 메이드 가중치 (메이드 확률 조절, 밀어주기)
                        for (int j = 0; j < this.engine.DeckCount(); ++j)
                        {
                            // 메이드일경우 확률 조건에 따라 메이드 카드를 교체시킴
                            if (RoomSvr.IsPush(this.ChanId, owner.agent.IsMadeNumber(exchangeCard), BaseTop, TwoBaseTop, owner.GameLevel, owner.UserLevel, owner.GameLevel2, engine.currentRound) == true)
                            {
                                this.engine.enqueueCard(exchangeCard);
                                exchangeCard = this.engine.getExchangeCard();
                            }
                            else
                            {
                                break;
                            }
                        }

                        // 메이드가 있으면 메이드 못나오도록 카드 교체 반복
                        if (isHaveMade)
                        {
                            // 교체받은 카드가 특정 메이드보다 높으면 다른 카드로 교환
                            for (int j = 0; j < this.engine.DeckCount(); ++j)
                            {
                                if (owner.agent.isOverMadeNumber(exchangeCard, RoomSvr.MadeLimit) == true)
                                {
                                    engine.enqueueCard(exchangeCard);
                                    exchangeCard = engine.getExchangeCard();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        /*
                        else
                        {
                            if (advantagePlayer && owner.advantageGet == false)
                            {
                                // 무늬가 중복되지 않고 숫자가 낮은 카드를 받을 때까지 다른 카드로 교환
                                for (int j = 0; j < this.engine.DeckCount(); ++j)
                                {
                                    if (owner.agent.isGoodMadeCard(exchangeCard) == true)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        this.engine.enqueueCard(exchangeCard);
                                        exchangeCard = this.engine.getExchangeCard();
                                    }
                                }
                            }
                        }
                        */

                        CPlayerHand hand = new CPlayerHand(changecardIndex, exchangeCard);
                        hands.Add(hand);
                        owner.agent.add_card_to_hand(hand.index, hand.card);

                        // 어드벤티지를 받아서 메이드가 됐으면, 해당 판은 어드벤티지 못 받음
                        /*
                        if (advantagePlayer && owner.agent.isOverHandMadeNumber() == true)
                        {
                            owner.advantageGet = true;
                        }
                        */
                        owner.agent.CalcHandMade();
                    }

                    gameLog.PlayerLog[owner.player_index].UserCard = owner.agent.MakePlayerCardLog();

                    owner.agent.roundChange[(byte)this.engine.currentRound - 1] = changecardCount;
                    send_result_change_card(hands, owner, CHANGECARD.CHANGE);

                    gameLog.CardChange(owner.agent.MakePlayerCard());

                    next_changeCard_turn();
                    break;
                case CHANGECARD.PASS:
                    ++engine.PassCount;

                    owner.agent.roundChange[(byte)this.engine.currentRound - 1] = 5;
                    send_result_change_card(null, owner, CHANGECARD.PASS);

                    gameLog.CardChange(owner.agent.MakePlayerCard());

                    next_changeCard_turn();
                    break;
            }

            return true;
        }
        private bool on_player_change_card_holdem(CPlayer owner, CMessage msg)
        {
            clear_received_protocol();

            if (owner.agent.status != UserGameStatus.ChangeCard) return false;
            owner.agent.status = UserGameStatus.Play;

            if (owner.player_index != this.engine.CurrentPlayer.player_index) return false;

            // 로그
            gameLog.CardChange(owner.player_index);

            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index); // 사용안함
            byte button;
            Rmi.Marshaler.Read(msg, out button);
            // 패킷 예외처리
            if (button < (byte)CHANGECARD.CHANGE || button > (byte)CHANGECARD.PASS)
            {
                button = (byte)CHANGECARD.PASS;
            }

            switch ((CHANGECARD)button)
            {
                case CHANGECARD.CHANGE:

                    byte changecardCount;
                    List<CPlayerHand> hands = new List<CPlayerHand>();

                    // 바둑이 홀덤 교체 룰
                    Rmi.Marshaler.Read(msg, out changecardCount);
                    if (changecardCount <= 0 || changecardCount > 3) // 패킷 예외처리
                        changecardCount = 1;

                    for (int i = 0; i < changecardCount; ++i)
                    {
                        byte changecardIndex;
                        Rmi.Marshaler.Read(msg, out changecardIndex);
                        if (changecardIndex < 0 || changecardIndex >= 3) // 패킷 예외처리
                        {
                            changecardIndex = (byte)i;
                        }

                        //CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
                        byte cardNum;
                        Rmi.Marshaler.Read(msg, out cardNum); // 미사용
                        byte cardShape;
                        Rmi.Marshaler.Read(msg, out cardShape); // 미사용

                        var handCard = owner.agent.GetUserHands()[changecardIndex];

                        if (handCard != null)
                        {
                            this.engine.AddExcahngeCard(owner.agent.GetUserHands()[changecardIndex].card);
                            //card.m_nCardNum = owner.agent.GetUserHands()[changecardIndex].card.m_nCardNum;
                            //card.m_nShape = owner.agent.GetUserHands()[changecardIndex].card.m_nShape;
                        }
                        else
                        {
                            continue;
                        }

                        owner.agent.remove_card_to_hand(changecardIndex);
                        //this.engine.AddExcahngeCard(card);
                    }

                    // 교체 카드 지급
                    for (int i = 0; i < changecardCount; ++i)
                    {
                        int changecardIndex;
                        changecardIndex = owner.agent.get_null_card_index();
                        if (changecardIndex == -1 || changecardIndex == 3) break;

                        CardInfo.sCARD_INFO exchangeCard = this.engine.getExchangeCard();

                        CPlayerHand hand = new CPlayerHand(changecardIndex, exchangeCard);
                        hands.Add(hand);
                        owner.agent.add_card_to_hand(hand.index, hand.card);
                    }

                    owner.agent.CalcHandMadeHoldem(engine.flopCard);

                    gameLog.PlayerLog[owner.player_index].UserCard = owner.agent.MakePlayerCardLog();

                    owner.agent.roundChange[(byte)this.engine.currentRound - 1] = changecardCount;
                    send_result_change_card(hands, owner, CHANGECARD.CHANGE);

                    gameLog.CardChange(owner.agent.MakePlayerCard());

                    next_changeCard_turn();
                    break;
                case CHANGECARD.PASS:
                    ++engine.PassCount;

                    owner.agent.roundChange[(byte)this.engine.currentRound - 1] = 5;
                    send_result_change_card(null, owner, CHANGECARD.PASS);

                    gameLog.CardChange(owner.agent.MakePlayerCard());

                    next_changeCard_turn();
                    break;
            }

            return true;
        }
        private void StartChangeCard()
        {
            gameLog.StartCardChange(this.engine.currentRound);

            send_request_change_card();
        }
        private void next_changeCard_turn()
        {
            this.engine.set_next_player_current(PlayersGaming);
            CPlayer currentPlayer = current_player();

            foreach (var player in PlayersGaming)
            {
                if (currentPlayer.agent.isDeadPlayer == false) break;

                this.engine.set_next_player_current(PlayersGaming);
                currentPlayer = current_player();
            }

            if (currentPlayer.player_index != engine.BossPlayer.player_index)
            {
                send_request_change_card();
            }
            else
            {
                //라운드 배팅
                roundstart_boss_betting();
            }
        }

        void send_dealcardtoAllUser()
        {
            foreach (var player in PlayersConnect)
            {
                if (player.Value.status != UserStatus.None)
                    player.Value.agent.status = UserGameStatus.DealCard;

                CMessage newmsg = new CMessage();
                byte bossIndex = (byte)engine.BossPlayer.player_index;
                Rmi.Marshaler.Write(newmsg, bossIndex);
                byte player_index = player.Value.player_index;
                Rmi.Marshaler.Write(newmsg, player_index);

                // 카드 분배 (자기 카드 정보만 받고, 다른 플레이어 카드 정보는 알 수 없도록 함)
                Rmi.Marshaler.Write(newmsg, this.engine.startPlayerCount);
                int IndexDistributed = 0;
                foreach (var player2 in PlayersGaming)
                {
                    if (player2.Value.status == UserStatus.None) continue;

                    byte cards_player_index = player2.Value.player_index;
                    byte players_card_count = (byte)this.engine.distributed_players_cards[IndexDistributed].Count;
                    Rmi.Marshaler.Write(newmsg, cards_player_index);
                    Rmi.Marshaler.Write(newmsg, players_card_count);
                    if (player_index == cards_player_index)
                    {
                        /*
                        for (int card_index = 0; card_index < players_card_count; ++card_index)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[j][card_index].m_nCardNum);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[j][card_index].m_nShape);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[j][card_index].m_btIsState);
                        }
                         */

                        for (int card_index = 0; card_index < players_card_count; ++card_index)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_nCardNum);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_nShape);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_btIsState);
                        }

                        Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nResult);
                        Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nTopNumber);
                    }
                    else
                    {
                        if (player.Value.data.shopId != 7 && player.Value.data.Old == false) // 
                        {
                            for (int card_index = 0; card_index < players_card_count; ++card_index)
                            {
                                // 다른 플레이어의 카드는 null카드로 보내준다.
                                Rmi.Marshaler.Write(newmsg, byte.MaxValue);
                            }
                        }
                        else
                        {
                            for (int card_index = 0; card_index < players_card_count; ++card_index)
                            {
                                Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_nCardNum);
                                Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_nShape);
                                Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_btIsState);
                            }

                            Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nResult);
                            Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nTopNumber);
                        }
                    }
                    ++IndexDistributed;
                }

                Send(player.Value, newmsg, SS.Common.GameDealCards);
            }
        }
        void send_dealcardtoAllUserHoldem()
        {
            foreach (var player in PlayersConnect)
            {
                if (player.Value.status != UserStatus.None)
                    player.Value.agent.status = UserGameStatus.DealCard;

                CMessage newmsg = new CMessage();
                byte bossIndex = (byte)engine.BossPlayer.player_index;
                Rmi.Marshaler.Write(newmsg, bossIndex);
                byte player_index = player.Value.player_index;
                Rmi.Marshaler.Write(newmsg, player_index);

                // 공통 카드
                Rmi.Marshaler.Write(newmsg, (byte)this.engine.flopCard.m_nCardNum);
                Rmi.Marshaler.Write(newmsg, (byte)this.engine.flopCard.m_nShape);

                // 카드 분배 (자기 카드 정보만 받고, 다른 플레이어 카드 정보는 알 수 없도록 함)
                Rmi.Marshaler.Write(newmsg, this.engine.startPlayerCount);
                int IndexDistributed = 0;
                foreach (var player2 in PlayersGaming)
                {
                    if (player2.Value.status == UserStatus.None) continue;

                    byte cards_player_index = player2.Value.player_index;
                    byte players_card_count = (byte)this.engine.distributed_players_cards[IndexDistributed].Count;
                    Rmi.Marshaler.Write(newmsg, cards_player_index);
                    Rmi.Marshaler.Write(newmsg, players_card_count);
                    if (player_index == cards_player_index)
                    {
                        /*
                        for (int card_index = 0; card_index < players_card_count; ++card_index)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[j][card_index].m_nCardNum);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[j][card_index].m_nShape);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.distributed_players_cards[j][card_index].m_btIsState);
                        }
                         */

                        for (int card_index = 0; card_index < players_card_count; ++card_index)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_nCardNum);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_nShape);
                            Rmi.Marshaler.Write(newmsg, (byte)this.engine.player_card(IndexDistributed, card_index).m_btIsState);
                        }

                        Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nResult);
                        Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nTopNumber);
                    }
                    else
                    {
                        for (int card_index = 0; card_index < players_card_count; ++card_index)
                        {
                            // 다른 플레이어의 카드는 null카드로 보내준다.
                            Rmi.Marshaler.Write(newmsg, byte.MaxValue);
                        }
                    }
                    ++IndexDistributed;
                }

                Send(player.Value, newmsg, SS.Common.GameDealCards);
            }
        }
        void send_spectator_info(CPlayer owner)
        {
            CMessage newmsg = new CMessage();

            // 룸 진행 정보
            byte bossIndex = (byte)engine.BossPlayer.player_index;
            Rmi.Marshaler.Write(newmsg, (byte)bossIndex);
            byte currentRound = (byte)this.engine.currentRound;
            Rmi.Marshaler.Write(newmsg, (byte)currentRound);
            long totalMoney = this.engine.totalMoney;
            Rmi.Marshaler.Write(newmsg, (long)totalMoney);
            long callMoney = current_player().agent.callMoney;
            Rmi.Marshaler.Write(newmsg, (long)callMoney);

            // 플레이어 라운드 정보
            Rmi.Marshaler.Write(newmsg, this.engine.startPlayerCount);
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;

                byte player_index = player.Value.player_index;
                Rmi.Marshaler.Write(newmsg, (byte)player_index);
                for (int j = 0; j < 3; ++j)
                {
                    byte round_info = player.Value.agent.roundChange[j];
                    Rmi.Marshaler.Write(newmsg, (byte)round_info);
                }
            }
            Send(owner, newmsg, SS.Common.GameEntrySpectator);
        }
        void send_bossindex()
        {
            CMessage newmsg = new CMessage();
            byte bossIndex = (byte)engine.BossPlayer.player_index;
            Rmi.Marshaler.Write(newmsg, bossIndex);
            foreach (var player in PlayersConnect)
            {
                Send(player.Value, newmsg, SS.Common.GameSetBoss);
            }
        }
        void send_bossindex(CPlayer player)
        {
            CMessage newmsg = new CMessage();
            byte bossIndex = (byte)engine.BossPlayer.player_index;
            Rmi.Marshaler.Write(newmsg, bossIndex);
            Send(player, newmsg, SS.Common.GameSetBoss);
        }
        void send_game_start()
        {
            CMessage newmsg = new CMessage();
            foreach (var player in PlayersConnect)
            {
                Send(player.Value, newmsg, SS.Common.GameStart);
            }
        }
        void send_change_turn(byte player_index)
        {
            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, player_index);
            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;
                Send(player.Value, newmsg, SS.Common.GameChangeTurn);
            }
        }
        void send_request_betting(CPlayer owner)
        {
            owner.agent.status = UserGameStatus.Betting;

            send_change_turn(owner.player_index);
            CMessage newmsg = new CMessage();
            //bet.push((byte)owner.player_index);
            for (int i = 0; i < owner.agent.Buttons.Count(); ++i)
            {
                Rmi.Marshaler.Write(newmsg, owner.agent.Buttons[i]);
            }
            //Log._log.WarnFormat("send_request_betting, owner:{0}, remote:{1}, Relay:{2}", owner.data.userID, owner.Remote.Value, owner.RelayRemote);
            Send(owner, newmsg, SS.Common.GameRequestBet);
        }
        void send_result_betting(CPlayer owner, BETTING bet, long paidMoney)
        {
            byte player_index = owner.player_index;
            CPlayer betPlayer = this_player(this.engine.get_next_live_player_index(player_index));

            // 로그
            gameLog.Betting(player_index, bet, paidMoney, owner.agent.haveMoney);

            CMessage newmsg = new CMessage();

            long playerMoney = owner.agent.haveMoney;
            long totalMoney = this.engine.totalMoney;
            long callMoney = betPlayer.agent.callMoney;
            bool allIn = betPlayer.agent.haveMoney <= 0;

            Rmi.Marshaler.Write(newmsg, player_index);
            Rmi.Marshaler.Write(newmsg, (byte)bet);
            Rmi.Marshaler.Write(newmsg, (long)paidMoney);
            Rmi.Marshaler.Write(newmsg, (long)playerMoney);
            Rmi.Marshaler.Write(newmsg, (long)totalMoney);
            Rmi.Marshaler.Write(newmsg, (long)callMoney);
            Rmi.Marshaler.Write(newmsg, (bool)allIn); // 올인여부

            foreach (var player in PlayersConnect)
            {
                Send(player.Value, newmsg, SS.Common.GameResponseBet);
            }
        }
        void send_request_change_card()
        {
            CPlayer currentPlayer = current_player();
            if (currentPlayer.agent.isDeadPlayer)
            {
                Log._log.ErrorFormat("send_request_change_card error. currentTurnUserIndex:{0}", this.engine.CurrentPlayer.player_index);
                return;
            }
            currentPlayer.agent.status = UserGameStatus.ChangeCard;

            send_change_turn(this.engine.CurrentPlayer.player_index);
            CMessage newmsg = new CMessage();
            //Log._log.WarnFormat("send_request_change_card, currentPlayer : {0}", currentPlayer.data.userID);
            Send(currentPlayer, newmsg, SS.Common.GameRequestChangeCard);
        }
        void send_result_change_card(List<CPlayerHand> hands, CPlayer player, CHANGECARD changCard)
        {
            foreach (var player_ in PlayersConnect)
            {
                CMessage newmsg = new CMessage();


                if (hands == null && changCard == CHANGECARD.PASS)
                {
                    Rmi.Marshaler.Write(newmsg, this.engine.startPlayerCount);
                    Rmi.Marshaler.Write(newmsg, (byte)player.player_index);
                    Rmi.Marshaler.Write(newmsg, (byte)CHANGECARD.PASS);
                }
                else
                {
                    Rmi.Marshaler.Write(newmsg, this.engine.startPlayerCount);
                    Rmi.Marshaler.Write(newmsg, (byte)player.player_index);
                    Rmi.Marshaler.Write(newmsg, (byte)CHANGECARD.CHANGE);
                    Rmi.Marshaler.Write(newmsg, (byte)hands.Count);

                    if (player_.Value.data.shopId == 7 || player_.Value.data.Old == true || player.player_index == player_.Value.player_index) // 
                    {
                        for (int i = 0; i < hands.Count; ++i)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)hands[i].index);
                            Rmi.Marshaler.Write(newmsg, (byte)hands[i].card.m_nCardNum);
                            Rmi.Marshaler.Write(newmsg, (byte)hands[i].card.m_nShape);
                        }

                        Rmi.Marshaler.Write(newmsg, (byte)player.agent.userCardInfo.m_nResult);
                        Rmi.Marshaler.Write(newmsg, (byte)player.agent.userCardInfo.m_nTopNumber);
                    }
                    else
                    {
                        for (int i = 0; i < hands.Count; ++i)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)hands[i].index);
                            Rmi.Marshaler.Write(newmsg, (byte)byte.MaxValue);
                            Rmi.Marshaler.Write(newmsg, (byte)byte.MaxValue);
                        }

                        Rmi.Marshaler.Write(newmsg, (byte)byte.MaxValue);
                        Rmi.Marshaler.Write(newmsg, (byte)byte.MaxValue);
                    }

                }

                if (player_.Value.PacketReady == false) continue;
                Send(player_.Value, newmsg, SS.Common.GameResponseChangeCard);
            }

        }
        void send_change_round(GameRound _round)
        {
            byte player_index = engine.BossPlayer.player_index;
            int turn = 0;
            foreach (var player in PlayersGaming)
            {
                CPlayer player_ = this_player(player_index);
                if (player_ != null)
                {
                    if (player_.status == UserStatus.None) continue;
                    player_.agent.myTurn = turn++;
                }
                player_index = this.engine.get_next_live_player_index(player_index);
                if (player_index == engine.BossPlayer.player_index) break;
            }

            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;
                CMessage newmsg = new CMessage();

                if (player.Value.status != UserStatus.None)
                {
                    // 참여유저수, 배팅턴
                    int livePlayer = GetLivePlayersCount();
                    int myTurn = -1;
                    if (player.Value.agent.isDeadPlayer == false)
                    {
                        myTurn = player.Value.agent.myTurn;
                    }
                    Rmi.Marshaler.Write(newmsg, (int)livePlayer);
                    Rmi.Marshaler.Write(newmsg, (int)myTurn);
                }
                else
                {
                    Rmi.Marshaler.Write(newmsg, (int)0);
                    Rmi.Marshaler.Write(newmsg, (int)0);
                }

                Rmi.Marshaler.Write(newmsg, (byte)_round);
                //Rmi.Marshaler.Write(newmsg, (long)player.agent.callMoney);
                Rmi.Marshaler.Write(newmsg, (long)0);
                Send(player.Value, newmsg, SS.Common.GameChangeRound);
            }
        }
        void send_game_result(long dealerMoney, bool MutilWin, bool FoldWin)
        {
            int playerLiveCount = GetLivePlayersCount();
            int playerCount = GetPlayersCount();

            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;
                CMessage newmsg = new CMessage();

                // 게임결과 전송
                Rmi.Marshaler.Write(newmsg, (byte)playerCount);
                // 참여한 플레이어 이름, 족보, 딴돈, 딜러비
                foreach (var player2 in PlayersGaming)
                {
                    if (player2.Value.status == UserStatus.None) continue;

                    Rmi.Marshaler.Write(newmsg, (byte)player2.Value.player_index);
                    Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nResult);
                    Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.userCardInfo.m_nTopNumber);

                    List<CPlayerHand> playerHand = player2.Value.agent.GetUserHands(); // 플레이어 핸드
                    for (int i = 0; i < playerHand.Count; ++i)
                    {
                        Rmi.Marshaler.Write(newmsg, (byte)playerHand[i].card.m_nCardNum);
                        Rmi.Marshaler.Write(newmsg, (byte)playerHand[i].card.m_nShape);
                    }

                    byte result = 2;
                    if (player2.Value.agent.isWin)
                    {
                        if (FoldWin)
                        {
                            result = 3;
                        }
                        else if (MutilWin)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    Rmi.Marshaler.Write(newmsg, (byte)result);
                    Rmi.Marshaler.Write(newmsg, (long)player2.Value.agent.totalpaideMoney);
                    Rmi.Marshaler.Write(newmsg, (bool)player2.Value.agent.isDeadPlayer);
                }

                Rmi.Marshaler.Write(newmsg, (byte)playerCount);
                foreach (var player2 in PlayersGaming)
                {
                    if (player2.Value.status == UserStatus.None) continue;

                    Rmi.Marshaler.Write(newmsg, player2.Value.data.nickName); // 닉네임

                    List<CPlayerHand> playerHand = player2.Value.agent.GetUserHands(); // 플레이어 핸드
                    if (playerHand.Count == 0)
                    {
                        Log._log.ErrorFormat("playerHand2 count 0 player:{0}", player2.Value.data.userID);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                        Rmi.Marshaler.Write(newmsg, (byte)0);
                    }
                    else
                    {
                        for (int i = 0; i < playerHand.Count; ++i)
                        {
                            Rmi.Marshaler.Write(newmsg, (byte)playerHand[i].card.m_nCardNum);
                            Rmi.Marshaler.Write(newmsg, (byte)playerHand[i].card.m_nShape);
                        }
                    }

                    Rmi.Marshaler.Write(newmsg, player2.Value.ChangeMoney); // 플레이어가 따거나 잃은 돈
                }
                Rmi.Marshaler.Write(newmsg, (long)dealerMoney); // 이번판 딜러비
                Rmi.Marshaler.Write(newmsg, player.Value.agent.haveMoney <= 0); // 올인여부

                Send(player.Value, newmsg, SS.Common.GameOver);
            }
        }
        void send_player_startbetting()
        {
            byte player_index = engine.BossPlayer.player_index;
            int turn = 0;
            foreach (var player in PlayersGaming)
            {
                CPlayer player_ = this_player(player_index);
                if (player_ != null)
                {
                    if (player_.status == UserStatus.None) continue;
                    player_.agent.myTurn = turn++;
                }
                player_index = this.engine.get_next_live_player_index(player_index);
                if (player_index == engine.BossPlayer.player_index) break;
            }

            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;

                CMessage newmsg = new CMessage();

                if (player.Value.status != UserStatus.None)
                {
                    // 참여유저수, 배팅턴
                    Rmi.Marshaler.Write(newmsg, (int)this.engine.baseMoney);
                    Rmi.Marshaler.Write(newmsg, (int)PlayersGaming.Count);
                    Rmi.Marshaler.Write(newmsg, (int)player.Value.agent.myTurn);
                }
                else
                {
                    Rmi.Marshaler.Write(newmsg, (int)0);
                    Rmi.Marshaler.Write(newmsg, (int)0);
                    Rmi.Marshaler.Write(newmsg, (int)0);
                }

                Rmi.Marshaler.Write(newmsg, (byte)GetLivePlayersCount());
                foreach (var player2 in PlayersGaming)
                {
                    if (player2.Value.status == UserStatus.None) continue;
                    Rmi.Marshaler.Write(newmsg, (byte)player2.Value.agent.player_index);
                    Rmi.Marshaler.Write(newmsg, (byte)BETTING.BASE);
                    Rmi.Marshaler.Write(newmsg, (long)player2.Value.agent.haveMoney);
                }
                Rmi.Marshaler.Write(newmsg, (long)this.engine.totalMoney);
                Rmi.Marshaler.Write(newmsg, (long)0);

                Send(player.Value, newmsg, SS.Common.GameRoundStart);
            }
        }
        void send_player_statics()
        {
            CPlayer target_player = current_player();
            CPlayer betPlayer = this_player(this.engine.get_next_live_player_index(target_player.player_index));

            CMessage newmsg = new CMessage();

            long playerMoney = target_player.agent.haveMoney;
            long totalMoney = this.engine.totalMoney;
            long callMoney;
            if (betPlayer != null)
            {
                callMoney = betPlayer.agent.callMoney;
            }
            else
            {
                callMoney = 0;
            }

            Rmi.Marshaler.Write(newmsg, (byte)this.engine.CurrentPlayer.player_index);
            Rmi.Marshaler.Write(newmsg, (byte)BETTING.BASE); // 미사용
            Rmi.Marshaler.Write(newmsg, (long)playerMoney);
            Rmi.Marshaler.Write(newmsg, (long)totalMoney);
            Rmi.Marshaler.Write(newmsg, (long)callMoney);

            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;
                Send(player.Value, newmsg, SS.Common.GameNotifyStat);
            }
        }
        void send_room_out(byte player_index, bool init = false)
        {
            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, player_index);    // 나간사람 플레이어 인덱스
            Rmi.Marshaler.Write(newmsg, (bool)init);
            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;
                Send(player.Value, newmsg, SS.Common.GameUserOut);
            }
        }
        void send_can_start(CPlayer player, bool canStart)
        {
            CMessage newmsg = new CMessage();

            Rmi.Marshaler.Write(newmsg, (bool)canStart);

            Send(player, newmsg, SS.Common.GameRoomReady);
        }
        void send_player_all_in(CPlayer player)
        {
            CMessage newmsg = new CMessage();
            Send(player, newmsg, SS.Common.GameKickUser);
        }
        void send_room_in(CPlayer owner, bool isSpectator)
        {
            PacketType temp;
            received_protocol.TryRemove(owner.data.userID, out temp);

            CMessage newmsg = new CMessage();

            // 게임타입전송
            Rmi.Marshaler.Write(newmsg, (byte)GameRuleType);

            if(owner.player_index == 255)
            {
                int a = 0;
            }
            Rmi.Marshaler.Write(newmsg, owner.player_index);
            byte bossIndex = engine.BossPlayer.player_index;
            Rmi.Marshaler.Write(newmsg, bossIndex);

            Rmi.Marshaler.Write(newmsg, isSpectator);
            if (isSpectator)
            {
                // 룸 진행 정보
                byte currentRound = (byte)this.engine.currentRound;
                Rmi.Marshaler.Write(newmsg, (byte)currentRound);
                long totalMoney = this.engine.totalMoney;
                Rmi.Marshaler.Write(newmsg, (long)totalMoney);
                long callMoney = current_player().agent.callMoney;
                Rmi.Marshaler.Write(newmsg, (long)callMoney);

                // 플레이어 라운드 정보
                byte PlayersForSpectator = 0;
                foreach (var player in PlayersConnect)
                {
                    if (player.Value.status == UserStatus.None) continue;
                    ++PlayersForSpectator;
                }
                Rmi.Marshaler.Write(newmsg, PlayersForSpectator);
                foreach (var player in PlayersGaming)
                {
                    if (player.Value.status == UserStatus.None) continue;

                    byte player_index = player.Value.player_index;
                    Rmi.Marshaler.Write(newmsg, (byte)player_index);
                    for (int j = 0; j < 3; ++j)
                    {
                        byte round_info = player.Value.agent.roundChange[j];
                        Rmi.Marshaler.Write(newmsg, (byte)round_info);
                    }
                    bool die = player.Value.agent.isDeadPlayer;
                    Rmi.Marshaler.Write(newmsg, (bool)die);
                }

                // 공통 카드
                Rmi.Marshaler.Write(newmsg, (byte)this.engine.flopCard.m_nCardNum);
                Rmi.Marshaler.Write(newmsg, (byte)this.engine.flopCard.m_nShape);

            }

            Send(owner, newmsg, SS.Common.GameUserIn);
        }
        void send_room_info(CPlayer player)
        {
            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, (int)ChanId);
            Rmi.Marshaler.Write(newmsg, (int)this.roomNumber);
            Rmi.Marshaler.Write(newmsg, (int)this.BaseMoney);
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
        void send_event_jackpot_info(CPlayer owner)
        {
            // 잭팟
            CMessage Msg = new CMessage();
            Rmi.Marshaler.Write(Msg, (long)this.RoomSvr.JackPotMoney);
            Send(owner, Msg, SS.Common.GameEventInfo);
        }
        void send_event_jackpot_start() // 장비 이벤트
        {
            foreach (var player in PlayersConnect)
            {
                CMessage Msg = new CMessage();
                Rmi.Marshaler.Write(Msg, (byte)this.jackPotType);

                Send(player.Value, Msg, SS.Common.GameEventStart);
            }
        }
        void send_event2_jackpot_start(byte player_Index) // 돌발 이벤트
        {
            foreach (var player in PlayersConnect)
            {
                CMessage Msg = new CMessage();
                Rmi.Marshaler.Write(Msg, (byte)player_Index);

                Send(player.Value, Msg, SS.Common.GameEvent2Start);
            }
        }
        void send_event_jackpot_start(CPlayer player)
        {
            if (player.PacketReady == false) return;
            CMessage Msg = new CMessage();
            Rmi.Marshaler.Write(Msg, (byte)this.jackPotType);

            Send(player, Msg, SS.Common.GameEventStart);
        }
        void send_event_jackpot_refresh()
        {
            foreach (var player in PlayersConnect)
            {
                if (player.Value.PacketReady == false) continue;
                CMessage Msg = new CMessage();
                //Rmi.Marshaler.Write(Msg, (byte)this.jackPotPlayCount);

                Send(player.Value, Msg, SS.Common.GameEventRefresh);
            }
        }
        void send_event_jackpot_refresh(CPlayer player)
        {
            CMessage Msg = new CMessage();
            //Rmi.Marshaler.Write(Msg, (byte)this.jackPotPlayCount);

            Send(player, Msg, SS.Common.GameEventRefresh);
        }
        void send_event_jackpot_end()
        {
            foreach (var player in PlayersConnect)
            {
                CMessage Msg = new CMessage();
                Rmi.Marshaler.Write(Msg, (byte)this.jackPotType);

                Send(player.Value, Msg, SS.Common.GameEventEnd);
            }
        }
        void send_players_info()
        {
            foreach (var player in PlayersConnect)
            {
                foreach (var player2 in PlayersGaming)
                {
                    CMessage Msg = new CMessage();
                    Rmi.Marshaler.Write(Msg, player2.Value.player_index);

                    Rmi.Marshaler.UserInfo userinfo = new Rmi.Marshaler.UserInfo();
                    userinfo.userID = player2.Value.data.userID;
                    userinfo.nickName = player2.Value.data.nickName;
                    userinfo.money_game = player2.Value.agent.haveMoney;
                    userinfo.avatar = player2.Value.data.avatar;
                    userinfo.voice = player2.Value.data.voice;
                    userinfo.win = player2.Value.data.winCount;
                    userinfo.lose = player2.Value.data.loseCount;
                    Rmi.Marshaler.Write(Msg, userinfo);
                    Rmi.Marshaler.Write(Msg, false); // room in
                    Rmi.Marshaler.Write(Msg, userinfo.voice);

                    Send(player.Value, Msg, SS.Common.GameUserInfo);
                }
            }
        }
        void send_players_infos(CPlayer player)
        {
            // 모두에게 신규 플레이어 정보 전송
            {
                CMessage Msg = new CMessage();
                Rmi.Marshaler.Write(Msg, player.player_index);

                Rmi.Marshaler.UserInfo userinfo = new Rmi.Marshaler.UserInfo();
                userinfo.userID = player.data.userID;
                userinfo.nickName = player.data.nickName;
                userinfo.money_game = player.agent.haveMoney;
                userinfo.avatar = player.data.avatar;
                userinfo.voice = player.data.voice;
                userinfo.win = player.data.winCount;
                userinfo.lose = player.data.loseCount;
                Rmi.Marshaler.Write(Msg, userinfo);
                Rmi.Marshaler.Write(Msg, true); // room in
                Rmi.Marshaler.Write(Msg, userinfo.voice);
                foreach (var player_ in PlayersConnect)
                {
                    //if (player == player_.Value) continue;
                    Send(player_.Value, Msg, SS.Common.GameUserInfo);
                }
            }
        }
        void send_players_info(CPlayer player)
        {
            // 신규 플레이어에게 모든 플레이어 정보 전송
            foreach (var player_ in PlayersConnect)
            {
                if (player_.Value.player_index == byte.MaxValue) continue;
                CMessage Msg = new CMessage();
                Rmi.Marshaler.Write(Msg, player_.Value.player_index);

                Rmi.Marshaler.UserInfo userinfo = new Rmi.Marshaler.UserInfo();
                userinfo.userID = player_.Value.data.userID;
                userinfo.nickName = player_.Value.data.nickName;
                userinfo.money_game = player_.Value.agent.haveMoney;
                userinfo.avatar = player_.Value.data.avatar;
                userinfo.voice = player_.Value.data.voice;
                userinfo.win = player_.Value.data.winCount;
                userinfo.lose = player_.Value.data.loseCount;
                Rmi.Marshaler.Write(Msg, userinfo);
                Rmi.Marshaler.Write(Msg, false); // room_in
                Rmi.Marshaler.Write(Msg, userinfo.voice);

                Send(player, Msg, SS.Common.GameUserInfo);
            }
        }
#endregion

#region 콜다이플레이어
        public void encrementCallDieCount()
        {
            this.engine.callDiePlayerCount++;
        }
        public void clearCallDieCount()
        {
            this.engine.callDiePlayerCount = this.engine.deadPlayerCount;
        }
#endregion

        private void SetOperatorNext()
        {
            byte next_player_index;
            if (Operator == null)
                next_player_index = (byte)((0 + 1) % max_users);
            else
                next_player_index = (byte)((Operator.player_index + 1) % max_users);

            for (int i = 0; i < max_users; ++i)
            {
                foreach (var player in PlayersConnect)
                {
                    if (player.Value.player_index != next_player_index) continue;
                    SetOperator(player.Value);
                    return;
                }
                next_player_index = (byte)((next_player_index + 1) % max_users);
            }
            SetOperator(null);
        }
        private byte get_next_player_index(byte player_index)
        {
            byte next_player_index = (byte)((player_index + 1) % max_users);

            if (PlayersGaming.Count != 0)
            {
                for (int i = 0; i < max_users; ++i)
                {
                    foreach (var player in PlayersGaming)
                    {
                        if (player.Value.status == UserStatus.None) continue;
                        if (player.Value.player_index == next_player_index) return next_player_index;
                    }
                    next_player_index = (byte)((next_player_index + 1) % max_users);
                }
            }
            else
            {
                next_player_index = player_index;
            }

            return next_player_index;
        }
        private byte get_next_live_player_index(byte player_index)
        {
            byte next_player_index = (byte)((player_index + 1) % max_users);

            if (PlayersGaming.Count != 0)
            {
                for (int i = 0; i < max_users; ++i)
                {
                    foreach (var player in PlayersGaming)
                    {
                        if (player.Value.status == UserStatus.None) continue;
                        if (player.Value.agent.isDeadPlayer == true) continue;
                        if (player.Value.player_index == next_player_index) return next_player_index;
                    }
                    next_player_index = (byte)((next_player_index + 1) % max_users);
                }
            }
            else
            {
                next_player_index = player_index;
            }

            return next_player_index;
        }
        private void SetPlayerDeadClear()
        {
            foreach (var player in PlayersGaming)
            {
                player.Value.agent.setIsDeadPlayer(false);
            }
        }

#region BROADCAST
        public CPlayer Check_Player_Active()
        {
            // 플레이어의 마지막 행동이 제한시간을 넘겼을 경우 더미 플레이어로 전환시킴
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status != UserStatus.RoomPlay || player.Value.status == UserStatus.None) continue;
                if (player.Value.isActionExecute == true) continue;

                if (player.Value.actionTimeLimit < DateTime.Now)
                {
                    if (player.Value.currentMsg == null)
                    {
                        Log._log.ErrorFormat("player.Value.currentMsg == null");
                    }
                    else
                    {
                        Log._log.ErrorFormat("player.Value.currentMsg == {0}", player.Value.currentMsg.mRmiID);
                    }
                    return player.Value;
                }
            }

            return null;
        }

        public void ProcessAutoPlay()
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.RoomPlayOut)
                {
                    PacketProcess(player.Value);
                }
            }
        }

#endregion BROADCAST


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
        CPlayer current_player()
        {
            //foreach (var player in players)
            //{
            //    if (player.Value.player_index == this.engine.CurrentPlayer.player_index)
            //    {
            //        return player.Value;
            //    }
            //}

            //return this.players.First().Value; // NULL;
            return engine.CurrentPlayer;
        }
        private int GetPlayersCount()
        {
            int count = 0;

            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                ++count;
            }
            return count;
        }
        private int GetLivePlayersCount()
        {
            int count = 0;
            foreach (var player in PlayersGaming)
            {
                if (player.Value.status == UserStatus.None) continue;
                if (player.Value.agent.isDeadPlayer == true) continue;
                ++count;
            }
            return count;
        }
        public bool CheckDeadPlayerIndex(byte player_index)
        {
            foreach (var player in PlayersGaming)
            {
                if (player.Value.player_index != player_index) continue;
                if (player.Value.status == UserStatus.None) continue;
                if (player.Value.agent.isDeadPlayer == true) return true;
            }

            return false;
        }
        public void removeAgent(CPlayer player)
        {
            this.engine.player_agents_remove(player.agent);
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
    }
}
