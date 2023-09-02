using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZNet;

namespace Server.Engine
{
    public class CBadugiEngine
    {
        private List<CPlayerAgent> player_agents;
        private Queue<CardInfo.sCARD_INFO> deck;
        private Dictionary<CardInfo.sCARD_INFO, bool> deckPop;
        private CDealer dealer;//카드 메니져
        //public byte current_player_index { get; set; }
        public List<List<CardInfo.sCARD_INFO>> distributed_players_cards { get; private set; }
        public List<CardInfo.sCARD_INFO> changeCardDeck { get; private set; }

        public CardInfo.sCARD_INFO flopCard;
        Random rand = new Random((int)DateTime.UtcNow.Ticks);

        public int max_users;

        public GameRound currentRound = GameRound.START;
        public int baseMoney { get; set; }             // 판돈
        public long totalMoney { get; set; }            // 총 배팅 금액
        public long prevBetMoney { get; set; }          // 최근 배팅 머니
        public byte startPlayerCount { get; set; }      // 게임 플레이어 수
        public byte callDiePlayerCount { get; set; }    // 콜다이 플레이어 수
        public byte deadPlayerCount { get; set; }       // 기권한 플레이어 수
        public BETTING prevBetType { get; set; }        // 최근 배팅 타입
        public byte PassCount { get; set; }              // 최근 패스여부
        public bool FirstRaise { get; set; }              // 레이즈 여부
        public bool LastRaise { get; set; }              // 저녁레이즈 여부

        long BetingLimite; // 배팅 한도
        public CPlayer CurrentPlayer { get; private set; } // 현재 플레이어
        public void SetCurrentPlayer(CPlayer player) { CurrentPlayer = player; }
        public CPlayer BossPlayer { get; private set; } // 보스 플레이어
        public void SetBoss(CPlayer player) { BossPlayer = player; }

        public CBadugiEngine(int _max_users)
        {
            this.max_users = _max_users;
            this.player_agents = new List<CPlayerAgent>();
            this.distributed_players_cards = new List<List<CardInfo.sCARD_INFO>>();
            this.changeCardDeck = new List<CardInfo.sCARD_INFO>();
            this.dealer = new CDealer();
            this.deck = new Queue<CardInfo.sCARD_INFO>();
            this.deckPop = new Dictionary<CardInfo.sCARD_INFO, bool>();

            this.baseMoney = 0;
            this.totalMoney = 0;
            this.prevBetMoney = 0;
            this.startPlayerCount = 0;
            this.deadPlayerCount = 0;
            this.callDiePlayerCount = 0;
            this.prevBetType = BETTING.BASE;

            this.PassCount = 0;
            this.FirstRaise = false;
            this.LastRaise = false;
            // 카드 생성
            dealer.createCards();
            // 한 게임당 배팅한도 제한
            BetingLimite = 500000000000;
            /*
            if(chanType == ChannelType.Charge)
            {
                BetingLimite = 50000000000;
            }
            else if (chanType == ChannelType.Free)
            {
                BetingLimite = 50000;
            }
            else if (chanType == ChannelType.Freedom)
            {
                BetingLimite = 50000;
            }
            else
            {
                BetingLimite = 50000;
            }
            */
        }
        public void reset()
        {
            //this.player_agents.ForEach(obj => obj.reset());
            this.player_agents.Clear();
            this.distributed_players_cards.Clear();
            this.changeCardDeck.Clear();

            this.startPlayerCount = 0;
            this.deadPlayerCount = 0;
            this.callDiePlayerCount = 0;
            this.totalMoney = 0;
            this.prevBetMoney = 0;
            this.currentRound = GameRound.START;
            this.prevBetType = BETTING.BASE;

            this.PassCount = 0;
            this.FirstRaise = false;
            this.LastRaise = false;

            this.CurrentPlayer = null;
            this.BossPlayer = null;
        }
        public void addAgents(CPlayerAgent agent)
        {
            this.player_agents.Add(agent);
        }
        public void cardOperateBadugi(int MadeLimit = 0)
        {
            shuffle();

            distribute_cards(MadeLimit);

            DivideUp_BaseMoney();
        }
        public void cardOperateBadugiHoldem()
        {
            shuffle();

            distribute_cards_holdem();

            DivideUp_BaseMoney();
        }
        private void shuffle()
        {
            this.dealer.reset();
            this.deck.Clear();
            this.deckPop.Clear();

#if DEBUG
            this.dealer.suffleCards();
#else
            this.dealer.suffleCards();
#endif

            this.dealer.fill_to(deck);
        }
        private void shuffleExcahgne()
        {
            this.dealer.shuffleExchangeCards();
            this.deck.Clear();
            ////this.deckPop.Clear();
            this.dealer.fill_from_exchangeDeck(this.deck);
        }
        private void distribute_cards(int MadeLimit = 0)
        {
            //카드 네장을 분배한다.
#if DEBUG2
            CPlayerAgent pa = BossPlayer.agent;
            for (int i = 0; i < player_agents.Count; ++i)
            {
                pa.userHands = new List<CPlayerHand>();
                for (int j = 0; j < 4; ++j)
                {
                    CardInfo.sCARD_INFO card = pop_deck_card();
                    this.distributed_players_cards[i].Add(card);
                    CPlayerHand handcard = new CPlayerHand(j, card);
                    pa.userHands.Add(handcard);
                }
                // 분배받은 패에서 메이드 확인 (특정 메이드이상 있으면 그보다 높은 메이드 없도록)
                if (MadeLimit > 0 && i > 0)
                {
                    bool haveMade = false;

                    if (pa.isOverMadeNumber(MadeLimit) == true)
                    {
                        haveMade = true;
                    }

                    if (haveMade == true)
                    {
                        for (int j = 0; j <= i; ++j)
                        {
                            if ((pa.userCardInfo.m_nResult > 4)
                                || (pa.userCardInfo.m_nResult == 4 && MadeLimit >= pa.userCardInfo.m_nTopNumber))
                            {
                                CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
                                card.m_nCardNum = pa.GetUserHands()[0].card.m_nCardNum;
                                card.m_nShape = pa.GetUserHands()[0].card.m_nShape;

                                pa.remove_card_to_hand(0);
                                AddExcahngeCard(card);
                                CardInfo.sCARD_INFO exchangeCard = getExchangeCard();
                                // 교체받은 카드가 특정 메이드보다 높으면 다른 카드로 교환
                                for (int k = 0; k < DeckCount(); ++k)
                                {
                                    if (pa.isOverMadeNumber(exchangeCard, MadeLimit) == true)
                                    {
                                        enqueueCard(exchangeCard);
                                        exchangeCard = getExchangeCard();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                CPlayerHand hand = new CPlayerHand(0, exchangeCard);
                                pa.add_card_to_hand(hand.index, hand.card);
                            }
                        }

                    }
                }
                pa.CalcHandMade();

                pa = GetPlayerAgent(get_next_live_player_index(pa.player_index));
            }
#else
            //카드 네장을 분배한다.
            for (int i = 0; i < player_agents.Count; ++i)
            {
                player_agents[i].userHands = new List<CPlayerHand>();
                for (int j = 0; j < 4; ++j)
                {
                    CardInfo.sCARD_INFO card = pop_deck_card();
                    this.distributed_players_cards[i].Add(card);
                    CPlayerHand handcard = new CPlayerHand(j, card);
                    player_agents[i].userHands.Add(handcard);
                }
                // 분배받은 패에서 메이드 확인 (특정 메이드이상 있으면 그보다 높은 메이드 없도록)
                if (MadeLimit > 0 && i > 0)
                {
                    bool haveMade = false;

                    if (player_agents[i].isOverMadeNumber(MadeLimit) == true)
                    {
                        haveMade = true;
                    }

                    if (haveMade == true)
                    {
                        for (int j = 0; j <= i; ++j)
                        {
                            if ((player_agents[j].userCardInfo.m_nResult > 4)
                                || (player_agents[j].userCardInfo.m_nResult == 4 && MadeLimit >= player_agents[j].userCardInfo.m_nTopNumber))
                            {
                                //CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
                                //card.m_nCardNum = player_agents[i].GetUserHands()[0].card.m_nCardNum;
                                //card.m_nShape = player_agents[i].GetUserHands()[0].card.m_nShape;
                                byte changeIndex = (byte)(rand.Next() % 4);


                                AddExcahngeCard(player_agents[i].GetUserHands()[changeIndex].card);

                                player_agents[i].remove_card_to_hand(changeIndex);
                                //addExcahngeCard(card);
                                CardInfo.sCARD_INFO exchangeCard = getExchangeCard();
                                // 교체받은 카드가 특정 메이드보다 높으면 다른 카드로 교환
                                for (int k = 0; k < DeckCount(); ++k)
                                {
                                    if (player_agents[i].isOverMadeNumber(exchangeCard, MadeLimit) == true)
                                    {
                                        AddExcahngeCard(exchangeCard);
                                        //AddExcahngeCard(player_agents[i].GetUserHands()[0].card);
                                        //enqueueCard(exchangeCard);
                                        exchangeCard = getExchangeCard();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                CPlayerHand hand = new CPlayerHand(changeIndex, exchangeCard);
                                player_agents[i].add_card_to_hand(hand.index, hand.card);
                            }
                        }
                    }
                }
                player_agents[i].CalcHandMade();
            }
#endif
        }
        private void distribute_cards_holdem()
        {
            //공통 카드 한장
            flopCard = pop_deck_card();

            //카드 세장을 분배한다.
            for (int i = 0; i < player_agents.Count; ++i)
            {
                player_agents[i].userHands = new List<CPlayerHand>();
                for (int j = 0; j < 3; ++j)
                {
                    CardInfo.sCARD_INFO card = pop_deck_card();
                    this.distributed_players_cards[i].Add(card);
                    CPlayerHand handcard = new CPlayerHand(j, card);
                    player_agents[i].userHands.Add(handcard);
                }
                player_agents[i].CalcHandMadeHoldem(flopCard);
            }
        }
        private void DivideUp_BaseMoney()
        {
            // 기본 판돈 
            for (int i = 0; i < this.player_agents.Count; ++i)
            {
                player_agents[i].addPaidMoney(this.baseMoney, this.BetingLimite);
                this.totalMoney += this.baseMoney;
            }
        }
        private CardInfo.sCARD_INFO pop_deck_card()
        {
            if (deck.Count == 0)
            {
                shuffleExcahgne();
            }

            // 이미 나온 카드 제외
            //bool poped = false;

            CardInfo.sCARD_INFO card = this.deck.Dequeue();
            //deckPop.TryGetValue(card, out poped);
            //int i = 0;
            //for (; i < 100; ++i)
            //{
            //    if (poped == false) break;
            //    if (deck.Count <= 0)
            //    {
            //        shuffleExcahgne();
            //    }
            //    card = this.deck.Dequeue();
            //    //deckPop.TryGetValue(card, out poped);
            //}

            //if (i >= 100)
            //{
            //    Log._log.Fatal($"pop_deck_card : {i}, {deck.Count}, {deckPop.Count}");
            //    Log._log.Fatal($"deck : {deck.Count}");
            //    Log._log.Fatal($"deckPop : {deckPop.Count}");
            //    Log._log.Fatal($"dealer.deck : {dealer.deck.Count}");
            //    Log._log.Fatal($"dealer.exchangedDeck : {dealer.exchangedDeck.Count}");
            //}
            //else
            //{
            //    deckPop.Add(card, true);
            //}

            return card;
        }
        public int DeckCount()
        {
            return this.deck.Count;
        }

        public void AddExcahngeCard(CardInfo.sCARD_INFO card)
        {
            this.dealer.addExchangedDeck(card);
        }
        public CardInfo.sCARD_INFO getExchangeCard()
        {
            return pop_deck_card();
        }
        public void enqueueCard(CardInfo.sCARD_INFO card)
        {
            // 이미 나온 카드 다시 덱으로
            //deckPop.Remove(card);

            this.deck.Enqueue(card);
        }

#region 턴

        public void set_next_player_boss(ConcurrentDictionary<int, CPlayer> players)
        {
            byte next_player_index = (byte)((BossPlayer.player_index + 1) % max_users);
            for (int i = 0; i < max_users; ++i)
            {
                foreach (var player in players)
                {
                    if (player.Value.player_index != next_player_index) continue;
                    if (player.Value.status == UserStatus.None) continue;
                    if (player.Value.agent.isDeadPlayer == true) continue;
                    SetBoss(player.Value);
                    return;
                }
                next_player_index = (byte)((next_player_index + 1) % max_users);
            }

            Log._log.Fatal("set_next_player_boss no return"); 
        }

        public void set_next_player_current(ConcurrentDictionary<int, CPlayer> players)
        {
            byte next_player_index = (byte)((CurrentPlayer.player_index + 1) % max_users);
            for (int i = 0; i < max_users; ++i)
            {
                foreach(var player in players)
                {
                    if (player.Value.player_index != next_player_index) continue;
                    if (player.Value.status == UserStatus.None) continue;
                    if (player.Value.agent.isDeadPlayer == true) continue;
                    SetCurrentPlayer(player.Value);
                    return;
                }
                next_player_index = (byte)((next_player_index + 1) % max_users);
            }
        }

        public byte get_next_live_player_index(byte player_index)
        {
            byte next_player_index = (byte)((player_index + 1) % max_users);

            for (int i = 0; i < max_users; ++i)
            {
                for (int j = 0; j < player_agents.Count; ++j)
                {
                    if (player_agents[j].player_index != next_player_index) continue;
                    if (player_agents[j].status == UserGameStatus.None) continue;
                    if (player_agents[j].isDeadPlayer == true) continue;
                    return next_player_index;
                }

                next_player_index = (byte)((next_player_index + 1) % max_users);
                if (next_player_index == player_index) break;
            }

            //Log._log.Error("get_next_live_player_index error.");

            //var st = new System.Diagnostics.StackTrace();
            //foreach (var frame in st.GetFrames())
            //{
            //    //Log._log.Error(frame.GetFileLineNumber());
            //    //Log._log.Error(frame.GetFileName());
            //    Log._log.Error(frame.GetMethod());
            //}
            return next_player_index;
        }
        public CPlayerAgent GetPlayerAgent(byte player_index)
        {
            for (int i = 0; i < max_users; ++i)
            {
                for (int j = 0; j < player_agents.Count; ++j)
                {
                    if (player_agents[j].player_index != player_index) continue;
                    return player_agents[j];
                }
            }
            return null;
        }

        public long PlayerBettingCall(CPlayerAgent pa)
        {
            long betting = pa.callMoney;
            long noBetting = betting;
            pa.raiseMoney = 0;
            pa.callMoney = 0;
            betting = pa.addPaidMoney(betting, this.BetingLimite);
            pa.setPossibleRaceCount(0);
            pa.bCalled = true;

            this.totalMoney += betting;
            this.prevBetMoney = betting;

            this.prevBetType = BETTING.CALL;

            return betting;
        }

        public long PlayerBettingBbing(CPlayerAgent pa)
        {
            long betting = this.baseMoney;
            betting = pa.addPaidMoney(betting, this.BetingLimite);
            pa.raiseMoney = betting;
            pa.cutPossibleRaceCount();

            this.totalMoney += betting;
            setCallMoney(pa.player_index, betting);
            this.prevBetMoney = betting;

            this.prevBetType = BETTING.BBING;

            return betting;
        }

        public long PlayerBettingQuater(CPlayerAgent pa)
        {
            this.totalMoney += pa.callMoney;
            long betting = Math.Max(1, this.totalMoney / 4) + pa.callMoney;
            betting = pa.addPaidMoney(betting, this.BetingLimite);
            pa.raiseMoney = betting - pa.callMoney;
            pa.cutPossibleRaceCount();

            this.totalMoney += pa.raiseMoney;
            setCallMoney(pa.player_index, pa.raiseMoney);
            this.prevBetMoney = betting;

            this.prevBetType = BETTING.QUATER;

            FirstRaise = true;

            if(currentRound == GameRound.EVENING)
            {
                LastRaise = true;
            }

            return betting;
        }

        public long PlayerBettingHalf(CPlayerAgent pa)
        {
            this.totalMoney += pa.callMoney;
            long betting = Math.Max(1, this.totalMoney / 2) + pa.callMoney;
            betting = pa.addPaidMoney(betting, this.BetingLimite);
            pa.raiseMoney = betting - pa.callMoney;
            pa.cutPossibleRaceCount();

            this.totalMoney += pa.raiseMoney;
            setCallMoney(pa.player_index, pa.raiseMoney);
            this.prevBetMoney = betting;

            this.prevBetType = BETTING.HARF;

            FirstRaise = true;

            if (currentRound == GameRound.EVENING)
            {
                LastRaise = true;
            }

            return betting;
        }

        public long PlayerBettingHalffix(CPlayerAgent pa)
        {
            this.totalMoney += pa.callMoney;
            long betting = Math.Max(1, this.totalMoney * 50) + pa.callMoney;
            betting = pa.addPaidMoney(betting, this.BetingLimite);
            pa.raiseMoney = betting - pa.callMoney;
            pa.cutPossibleRaceCount();

            this.totalMoney += pa.raiseMoney;
            setCallMoney(pa.player_index, pa.raiseMoney);
            this.prevBetMoney = betting;

            this.prevBetType = BETTING.HARF;

            FirstRaise = true;

            if (currentRound == GameRound.EVENING)
            {
                LastRaise = true;
            }

            return betting;
        }

        public void PlayerBettingDie(CPlayerAgent pa)
        {
            pa.setIsDeadPlayer(true);
            this.deadPlayerCount++;

            //this.prevBetType = BETTING.DIE;
        }

        public long PlayerBettingDdaddang(CPlayerAgent pa)
        {
            this.totalMoney += pa.callMoney;
            long betting = this.prevBetMoney * 2 + pa.callMoney;
            betting = pa.addPaidMoney(betting, this.BetingLimite);
            pa.raiseMoney = betting - pa.callMoney;
            pa.cutPossibleRaceCount();

            this.totalMoney += pa.raiseMoney;
            setCallMoney(pa.player_index, pa.raiseMoney);
            this.prevBetMoney = betting;

            this.prevBetType = BETTING.DDADDANG;

            FirstRaise = true;

            if (currentRound == GameRound.EVENING)
            {
                LastRaise = true;
            }

            return betting;
        }

        private void setCallMoney(byte player_index, long callMoney)
        {
            for (int i = 0; i < player_agents.Count; ++i)
            {
                if (player_agents[i].player_index == player_index)
                {
                    player_agents[i].callMoney = 0;
                }
                else
                {
                    player_agents[i].callMoney = player_agents[i].callMoney + callMoney;
                }
            }
        }
#endregion

        public void player_agents_remove(CPlayerAgent player_agent)
        {
            this.player_agents.Remove(player_agent);
        }
        public CardInfo.sCARD_INFO player_card(int player_index, int hand_index)
        {
            return this.player_agents[player_index].userHands[hand_index].card;
        }
    }
}
