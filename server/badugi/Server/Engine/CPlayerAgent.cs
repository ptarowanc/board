using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNet;

namespace Server.Engine
{
    public enum UserGameStatus : byte
    {
        None,   // 대기, 관전자
        Play,
        DealCard,
        Betting,
        ChangeCard,
        OpenCard
    }
    public class CPlayerHand
    {
        public int index;
        public CardInfo.sCARD_INFO card;
        public CPlayerHand(int _index, CardInfo.sCARD_INFO _card)
        {
            this.index = _index;
            this.card = _card;
        }
    }
    public class CPlayerAgent
    {
        private CGameRoom joinRoom;
        public UserGameStatus status;

        public byte player_index { get; private set; }
        public long haveMoney { get; private set; }
        public byte possibleRaceCount { get; private set; }

        public long raiseMoney;             // 해당 라운드에 레이즈한 금액
        public long callMoney;              // 현재 배팅에 콜해야 되는 금액
        public long paidMoney;              // 게임에서 배팅한 총 금액 
        public long betMoney;              // 게임에서 배팅한 총 금액 오버배팅 포함
        public long totalpaideMoney;        // 계산전 총 머니적용 금액
        public long earnedMoney;            // 계산후 머니적용 금액

        public long money_var = 0; // 변동머니

        public bool isDeadPlayer { get; private set; }

        public bool bCalled = false;
        public bool isWin = false;

        public byte handRank;
        public int handScore;

        public List<CPlayerHand> userHands;
        public List<CardInfo.sCARD_INFO> userHandsInfo;
        public CardInfo userCardInfo;
        public byte[] roundChange;
        public byte[] Buttons;             // 베팅때 누를 수 있는 버튼

        public CPlayerAgent(byte player_index, CGameRoom room)
        {
            this.status = UserGameStatus.None;
            this.player_index = player_index;
            this.joinRoom = room;

            this.haveMoney = -1;
            this.possibleRaceCount = 1;

            this.raiseMoney = 0;
            this.callMoney = 0;
            this.paidMoney = 0;
            betMoney = 0;
            this.totalpaideMoney = 0;
            this.earnedMoney = 0;

            this.money_var = 0;

            this.isDeadPlayer = false;
            this.bCalled = false;
            this.isWin = false;

            this.handRank = 0;
            this.handScore = 0;

            userHands = new List<CPlayerHand>(4);
            userHandsInfo = new List<CardInfo.sCARD_INFO>(4);
            userCardInfo = new CardInfo();
            roundChange = new byte[3];
            Buttons = new byte[7];
        }

        public void reset()
        {
            this.raiseMoney = 0;
            this.callMoney = 0;
            this.paidMoney = 0;
            betMoney = 0;
            this.totalpaideMoney = 0;
            earnedMoney = 0;

            this.isDeadPlayer = false;
            this.bCalled = false;
            this.isWin = false;

            this.handRank = 0;
            this.handScore = 0;

            userHands.Clear();
            userHandsInfo.Clear();
            userCardInfo.Clear();
            Array.Clear(roundChange, 0, roundChange.Count());
            Array.Clear(Buttons, 0, Buttons.Count());
        }

        public void add_card_to_hand(int index, CardInfo.sCARD_INFO card)
        {
            CPlayerHand handcard = new CPlayerHand(index, card);
            userHands[index] = handcard;
        }
        public int get_null_card_index()
        {
            //if (userHands.Count < 4)
            //{
            //    return userHands.Count;
            //}

            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i] == null) return i;
            }

            return -1;
        }
        public int get_not_null_card_index()
        {
            if (userHands.Count < 4)
            {
                return userHands.Count;
            }

            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i] != null) return i;
            }

            return -1;
        }

        public void remove_card_to_hand(byte _index)
        {
            userHands[_index] = null;
        }
        public List<CPlayerHand> GetUserHands()
        {
            return userHands;
        }
        public bool isOverMadeNumber(int MadeLimit)
        {
            int myHandMadeNumber = -1;
            // 플레이어 핸드가 메이드고, 메이드 숫자가 높거나 같을 경우 false
            List<CardInfo.sCARD_INFO> _userHandsInfo = new List<CardInfo.sCARD_INFO>();
            CardInfo _userCardInfo = new CardInfo();
            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i] == null) continue;
                _userHandsInfo.Add(userHands[i].card);
            }
            if (_userHandsInfo.Count != 4) return false;
            _userCardInfo.SetCard(_userHandsInfo.ToArray());
            _userCardInfo.MakeResult();

            if (_userCardInfo.m_nResult == 7)
            {
                // 골프
                myHandMadeNumber = 0;
            }
            else if (_userCardInfo.m_nResult == 6)
            {
                // 세컨드
                myHandMadeNumber = 0;
            }
            else if (_userCardInfo.m_nResult == 5)
            {
                // 써드
                myHandMadeNumber = 0;
            }
            else if (_userCardInfo.m_nResult == 4)
            {
                // 메이드
                myHandMadeNumber = _userCardInfo.GetTopNumber();
            }

            if (myHandMadeNumber == 0 ||
                (myHandMadeNumber > 0 && myHandMadeNumber <= MadeLimit))
            {
                return true;
            }

            return false;
        }
        public bool isOverMadeNumber(CardInfo.sCARD_INFO card, int MadeLimit)
        {
            int myHandMadeNumber = -1;
            // 플레이어 핸드가 메이드고, 메이드 숫자가 높거나 같을 경우 false
            List<CardInfo.sCARD_INFO> _userHandsInfo = new List<CardInfo.sCARD_INFO>();
            CardInfo _userCardInfo = new CardInfo();
            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i] == null) continue;
                _userHandsInfo.Add(userHands[i].card);
            }
            _userHandsInfo.Add(card);
            if (_userHandsInfo.Count != 4) return false;
            _userCardInfo.SetCard(_userHandsInfo.ToArray());
            _userCardInfo.MakeResult();

            if (_userCardInfo.m_nResult == 7)
            {
                // 골프
                myHandMadeNumber = 4;
            }
            else if (_userCardInfo.m_nResult == 6)
            {
                // 세컨드
                myHandMadeNumber = 5;
            }
            else if (_userCardInfo.m_nResult == 5)
            {
                // 써드
                myHandMadeNumber = 5;
            }
            else if (_userCardInfo.m_nResult == 4)
            {
                // 메이드
                myHandMadeNumber = _userCardInfo.m_nTopNumber;
            }

            if (myHandMadeNumber > 0 && myHandMadeNumber <= MadeLimit)
            {
                return true;
            }

            return false;
        }

        public int IsMadeNumber(CardInfo.sCARD_INFO card)
        {
            int myHandMadeNumber = -1;
            // 플레이어 핸드가 메이드고, 메이드 숫자가 높거나 같을 경우 false
            List<CardInfo.sCARD_INFO> _userHandsInfo = new List<CardInfo.sCARD_INFO>();
            CardInfo _userCardInfo = new CardInfo();
            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i] == null) continue;
                _userHandsInfo.Add(userHands[i].card);
            }
            _userHandsInfo.Add(card);
            if (_userHandsInfo.Count != 4) return -1;
            _userCardInfo.SetCard(_userHandsInfo.ToArray());
            _userCardInfo.MakeResult();

            if (_userCardInfo.m_nResult == 7)
            {
                // 골프
                myHandMadeNumber = 4;
            }
            else if (_userCardInfo.m_nResult == 6)
            {
                // 세컨드
                myHandMadeNumber = 5;
            }
            else if (_userCardInfo.m_nResult == 5)
            {
                // 써드
                myHandMadeNumber = 5;
            }
            else if (_userCardInfo.m_nResult == 4)
            {
                // 메이드
                myHandMadeNumber = _userCardInfo.m_nTopNumber + 1;
            }

            return myHandMadeNumber;
        }
        public int IsMadeNumberStart()
        {
            int myHandMadeNumber = -1;
            // 플레이어 핸드가 메이드고, 메이드 숫자가 높거나 같을 경우 false
            List<CardInfo.sCARD_INFO> _userHandsInfo = new List<CardInfo.sCARD_INFO>();
            CardInfo _userCardInfo = new CardInfo();
            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i] == null) continue;
                _userHandsInfo.Add(userHands[i].card);
            }
            if (_userHandsInfo.Count != 4) return -1;
            _userCardInfo.SetCard(_userHandsInfo.ToArray());
            _userCardInfo.MakeResult();

            if (_userCardInfo.m_nResult == 7)
                return 4;
            else if (_userCardInfo.m_nResult == 6)
                return 5;
            else if (_userCardInfo.m_nResult == 5)
                return 5;
            else if (_userCardInfo.m_nResult == 4)
                return _userCardInfo.m_nTopNumber + 1;

            return myHandMadeNumber;
        }

        public bool isGoodMadeCard(CardInfo.sCARD_INFO card)
        {
            if (card.m_nCardNum > 6) return false;

            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i].card.m_nShape == card.m_nShape) return false;
            }

            return true;
        }
        public void CalcHandMade()
        {
            userHandsInfo.Clear();
            userCardInfo.Clear();
            for (int i = 0; i < userHands.Count; ++i)
            {
                if (userHands[i] == null) break;
                userHandsInfo.Add(userHands[i].card);
            }
            if (userHandsInfo.Count != 4) return;
            userCardInfo.SetCard(userHandsInfo.ToArray());
            userCardInfo.MakeResult();
        }
        public void CalcHandMadeHoldem(CardInfo.sCARD_INFO flopCard)
        {
            userHandsInfo.Clear();
            userCardInfo.Clear();
            for (int i = 0; i < userHands.Count; ++i)
            {
                userHandsInfo.Add(userHands[i].card);
            }
            userHandsInfo.Add(flopCard);
            if (userHandsInfo.Count != 4) return;
            userCardInfo.SetCard(userHandsInfo.ToArray());
            userCardInfo.MakeResult();
        }

        public void CalcHandScore()
        {
            userHandsInfo.Clear();
            userCardInfo.Clear();
            for (int i = 0; i < userHands.Count; ++i)
            {
                userHandsInfo.Add(userHands[i].card);
            }
            if (userHandsInfo.Count != 4) return;
            userCardInfo.SetCard(userHandsInfo.ToArray());
            userCardInfo.MakeResult();
            this.handScore = userCardInfo.GetTotalHandScore();
        }
        public void CalcHandScoreHoldem(CardInfo.sCARD_INFO flopCard)
        {
            userHandsInfo.Clear();
            userCardInfo.Clear();
            for (int i = 0; i < userHands.Count; ++i)
            {
                userHandsInfo.Add(userHands[i].card);
            }
            userHandsInfo.Add(flopCard);
            if (userHandsInfo.Count != 4) return;
            userCardInfo.SetCard(userHandsInfo.ToArray());
            userCardInfo.MakeResult();
            this.handScore = userCardInfo.GetTotalHandScore();
        }

        public string GetUserHandName()
        {
            string cardName = userCardInfo.GetCardName();
            string cardRes = cardName + " " + userCardInfo.GetVoiceFileName();
            return cardRes;
        }
        public void setMoney(long money)
        {
            this.haveMoney = money;
        }
        public long getMoney()
        {
            return this.haveMoney;
        }
        public void addMoney(long money)
        {
            this.haveMoney += money;
        }
        public long addPaidMoney(long money, long bettingLimite)
        {
            // 배팅한도
            if (this.paidMoney + money > bettingLimite)
            {
                money = bettingLimite - this.paidMoney;
            }

            if (this.haveMoney <= money)
            {
                money = this.haveMoney;
                this.haveMoney = 0;
            }
            else
            {
                this.haveMoney -= money;
            }

            this.paidMoney += money;

            return money;
        }

        public bool CheckPaidMoney(long money)
        {
            if (this.haveMoney < money)
            {
                return false;
            }

            return true;
        }

        public byte getPossibleRaceCount()
        {
            return possibleRaceCount;
        }
        public void setPossibleRaceCount(byte possibleRaceCount)
        {
            this.possibleRaceCount = possibleRaceCount;
        }
        public void cutPossibleRaceCount()
        {
            --this.possibleRaceCount;
        }
        public void initPossibleRaceCount(GameRound round, bool chanFree, ChannelKind ChanKind)
        {
            switch (round)
            {
                case GameRound.START:
                    if (chanFree) this.possibleRaceCount = 10;
                    //else if (ChanKind == ChannelKind.무료2채널) this.possibleRaceCount = 2;
                    else this.possibleRaceCount = 1;
                    break;
                case GameRound.MORNING:
                    if (chanFree) this.possibleRaceCount = 10;
                    else this.possibleRaceCount = 2;
                    break;
                case GameRound.AFTERNOON:
                    if (chanFree) this.possibleRaceCount = 10;
                    else this.possibleRaceCount = 3;
                    break;
                case GameRound.EVENING:
                    if (chanFree) this.possibleRaceCount = 10;
                    else this.possibleRaceCount = 3;
                    break;
            }
        }

        public void setIsDeadPlayer(bool isDeadPlayer)
        {
            this.isDeadPlayer = isDeadPlayer;
        }

        #region AI플레이어
        public int myTurn = -1;
        #endregion AI플레이어

        #region 자동처리
        public delegate void PacketFn(CMessage rm);
        public ConcurrentDictionary<PacketType, PacketFn> packet_handler;
        private CPlayer dummyPlayer;

        public void setDummyPlayer(CPlayer player)
        {
            this.dummyPlayer = player;
            this.dummyPlayer.QueueMsg = new ConcurrentQueue<MessageTemp>();

            this.packet_handler = new ConcurrentDictionary<PacketType, PacketFn>();
            this.packet_handler.TryAdd(SS.Common.GameDealCards, ResponseDealCardToAllUser);
            this.packet_handler.TryAdd(SS.Common.GameRequestBet, ResponseBetting);
            this.packet_handler.TryAdd(SS.Common.GameRequestChangeCard, ResponseChangeCard);
        }

        MessageTemp SendMsgMake(CMessage msg, PacketType rmiID)
        {
            return new MessageTemp(msg, rmiID);
        }

        void Send(CMessage msg, PacketType protocol)
        {
            switch (protocol)
            {
                case SS.Common.GameDealCardsEnd:
                case SS.Common.GameActionBet:
                case SS.Common.GameActionChangeCard:
                    {
                        dummyPlayer.QueueMsg.Enqueue(SendMsgMake(msg, protocol));
                    }
                    break;
                default:
                    {
                        Log._log.Error("Send Error protocol:" + protocol);
                    }
                    break;
            }
        }
        void ResponseDealCardToAllUser(CMessage msg)
        {
            CMessage newmsg = new CMessage();
            Send(newmsg, SS.Common.GameDealCardsEnd);

        }
        void ResponseBetting(CMessage msg)
        {
            // 체크할 수 있는 경우가 아니면 무조건 다이
            byte call;
            Rmi.Marshaler.Read(msg, out call);
            byte bbing;
            Rmi.Marshaler.Read(msg, out bbing);
            byte quater;
            Rmi.Marshaler.Read(msg, out quater);
            byte half;
            Rmi.Marshaler.Read(msg, out half);
            byte die;
            Rmi.Marshaler.Read(msg, out die);
            byte check;
            Rmi.Marshaler.Read(msg, out check);
            byte ddaddang;
            Rmi.Marshaler.Read(msg, out ddaddang);

            BETTING myBet = BETTING.DIE;

            if (Convert.ToBoolean(check) == true)
            {
                myBet = BETTING.CHECK;
            }

            if (this.callMoney == 0)
            {
                if (Convert.ToBoolean(call) == true)
                {
                    myBet = BETTING.CALL;
                }
                else if (Convert.ToBoolean(check) == true)
                {
                    myBet = BETTING.CHECK;
                }
                else if (Convert.ToBoolean(bbing) == true)
                {
                    myBet = BETTING.BBING;
                }
                else
                {
                    myBet = BETTING.DIE;
                }
            }
            else
            {
                if (Convert.ToBoolean(die) == true)
                {
                    myBet = BETTING.DIE;
                }
                else if (Convert.ToBoolean(call) == true)
                {
                    myBet = BETTING.CALL;
                }
                else if (Convert.ToBoolean(check) == true)
                {
                    myBet = BETTING.CHECK;
                }
                else if (Convert.ToBoolean(bbing) == true)
                {
                    myBet = BETTING.BBING;
                }
                else if (Convert.ToBoolean(half) == true)
                {
                    myBet = BETTING.HARF;
                }
                else
                {
                    myBet = BETTING.DIE;
                }
            }

            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, (byte)dummyPlayer.player_index);
            Rmi.Marshaler.Write(newmsg, (byte)myBet);
            Send(newmsg, SS.Common.GameActionBet);
        }
        void ResponseChangeCard(CMessage msg)
        {
            // 무조건 패스
            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, (byte)dummyPlayer.player_index);
            Rmi.Marshaler.Write(newmsg, (byte)CHANGECARD.PASS);
            Send(newmsg, SS.Common.GameActionChangeCard);
        }
        void ResponseCardOpen(CMessage msg)
        {
            /*
            CMessage newmsg = new CMessage();
            PacketType msgID = (PacketType)Rmi.Common.PLAYER_CARD_OPEN;
            CPackOption pkOption = CPackOption.Basic;
            newmsg.WriteStart(msgID, pkOption, 0, true);
            Send(newmsg, msgID, msg.remote);
            */
        }
        #endregion 자동처리

        public string MakePlayerCardLog()
        {
            string FloorLog = "";

            // [족보][탑넘버]_
            FloorLog += GetMadeToString(userCardInfo.m_nResult) + GetNumberToNumber(userCardInfo.m_nTopNumber + 1) + "_";

            // 무늬 : a,b,c,d . 번호 : 1~13
            // [족보][탑넘버]_[카드1][카드2][카드3][카드4]
            for (int i = 0; i < userHands.Count; ++i)
            {
                FloorLog += GetShapeToString(userHands[i].card.m_nShape) + GetNumberToNumber(userHands[i].card.m_nCardNum + 1);
            }
            return FloorLog;
        }

        public string MakePlayerCard()
        {
            string Card = "";

            for (int i = 0; i < userHands.Count; ++i)
            {
                Card += GetShapeToSymbol(userHands[i].card.m_nShape) + GetNumberToNumber(userHands[i].card.m_nCardNum + 1);
            }
            return Card;
        }

        public string MakeChangeCard(byte CardIndex)
        {
            return GetShapeToSymbol(userHands[CardIndex].card.m_nShape) + GetNumberToNumber(userHands[CardIndex].card.m_nCardNum + 1); ;
        }

        string GetMadeToString(int Made)
        {
            switch (Made)
            {
                case 1:
                    return "d";
                case 2:
                    return "c";
                case 3:
                    return "b";
                case 4:
                    return "a";
                case 5:
                    return "a";
                case 6:
                    return "a";
                case 7:
                    return "a";
            }
            return "?";
        }

        string GetNumberToNumber(int Number)
        {
            switch (Number)
            {
                case 0:
                    return "0"; // 노베이스
                case 1:
                    return "1";
                case 2:
                    return "2";
                case 3:
                    return "3";
                case 4:
                    return "4";
                case 5:
                    return "5";
                case 6:
                    return "6";
                case 7:
                    return "7";
                case 8:
                    return "8";
                case 9:
                    return "9";
                case 10:
                    return "t";
                case 11:
                    return "j";
                case 12:
                    return "q";
                case 13:
                    return "k";
            }
            return "?";
        }

        string GetShapeToString(int Shape)
        {
            switch (Shape)
            {
                case 0:
                    return "a";
                case 1:
                    return "b";
                case 2:
                    return "c";
                case 3:
                    return "d";
            }
            return "?";
        }
        string GetShapeToSymbol(int Shape)
        {
            switch (Shape)
            {
                case 0:
                    return "♠";
                case 1:
                    return "◆";
                case 2:
                    return "♥";
                case 3:
                    return "♣";
            }
            return "?";
        }
    }
}
