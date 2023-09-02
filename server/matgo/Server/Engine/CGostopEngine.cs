using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engine
{
    public class CGostopEngine
    {
        // 판돈(점당 머니)
        public int baseMoney;

        // 전체 카드를 보관할 컨테이너.
        public CCardManager card_manager;
        Queue<CCard> card_queue;
        Queue<CCard> card_queue_bonus;

        public byte first_player_index;
        List<CPlayerAgent> player_agents;
        public CFloorCardManager floor_manager { get; private set; }

        public bool IsPractice;
        public bool missionSended;

        // 게임 진행시 카드 정보들을 저장해놓을 임시 변수들.
        // 한턴이 끝나면 모두 초기화 시켜줘야 한다.

        public CCard card_from_player { get; private set; } //플레이어가 낸 카드 정보
        public CCard card_from_deck { get; private set; } //덱에서 뒤집은 카드 정보
        public List<CCard> bomb_cards_from_player { get; private set; } //플레이어가 낸 폭탄 카드 정보
        public List<CCard> target_cards_to_choice { get; private set; } //바닥에 선택한 카드 정보
        public byte same_card_count_with_player { get; private set; } //플레이어가 낸 카드와 같은 숫자의 바닥의 카드 수
        public byte same_card_count_with_deck { get; private set; } //덱에서 뒤집은 카드 중 같은 숫자의 바닥의 카드수
        public CARD_EVENT_TYPE card_event_type { get; private set; } //카드 이벤트(쪽, 뻑, 따닥, 폭탄 등등)
        public List<CARD_EVENT_TYPE> flipped_card_event_type { get; private set; } //뒤집은 카드 이벤트 리스트
        public List<CCard> floor_cards_to_player { get; private set; } //플레이어가 바닥에서 가져갈 카드들

        public List<CCard> deck_flip_bonus_cards { get; private set; } // 더미에서 보너스카드가 나왔을때 플레이어가 가져갈 보너스 카드 임시 저장

        public List<CCard> cards_to_floor { get; private set; } //바닥에 낸 카드
        public byte cards_from_others_count { get; private set; } //다른 유저에게 가져올 카드 수
        public Dictionary<byte, List<CCard>> other_cards_to_player { get; private set; } //다른 유저에게 가져온 카드
        public List<CCard> shaking_cards { get; private set; } //플레이어가 흔든 카드

        // 두개의 카드중 하나를 선택하는 경우는 두가지가 있는데(플레이어가 낸 경우, 덱에서 뒤집은 경우),
        // 서버에서 해당 상황에 맞는 타입을 들고 있다가
        // 클라이언트로부터 온 타입과 맞는지 비교하는데 사용한다.
        // 틀리다면 오류 또는 어뷰징이다.
        public PLAYER_SELECT_CARD_RESULT expected_result_type;
        public byte start_player;
        public byte current_player_index;

        // history.
        public List<CCard> distributed_floor_cards { get; private set; }
        public List<List<CCard>> distributed_players_cards { get; private set; }
        public bool HasFloorStartCardsBonus { get; private set; }
        public List<CCard> distributed_floor_bonus_cards { get; private set; }
        public List<CCard> distributed_bonus_floor_cards { get; private set; }

        public string CardLog;

        public CGostopEngine()
        {
            this.IsPractice = false;
            missionSended = false;
            this.first_player_index = 0;

            this.floor_manager = new CFloorCardManager();
            this.card_manager = new CCardManager();
            this.player_agents = new List<CPlayerAgent>();

            this.distributed_floor_cards = new List<CCard>();
            this.distributed_players_cards = new List<List<CCard>>();
            this.distributed_floor_bonus_cards = new List<CCard>();
            this.distributed_bonus_floor_cards = new List<CCard>();

            this.distributed_players_cards.Add(new List<CCard>());
            this.distributed_players_cards.Add(new List<CCard>());

            this.floor_cards_to_player = new List<CCard>();

            this.deck_flip_bonus_cards = new List<CCard>();

            this.cards_to_floor = new List<CCard>();
            this.other_cards_to_player = new Dictionary<byte, List<CCard>>();
            this.other_cards_to_player.Add(0, new List<CCard>());
            this.other_cards_to_player.Add(1, new List<CCard>());

            this.current_player_index = 0;
            this.flipped_card_event_type = new List<CARD_EVENT_TYPE>();
            this.bomb_cards_from_player = new List<CCard>();
            this.target_cards_to_choice = new List<CCard>();
            this.shaking_cards = new List<CCard>();
            this.card_queue = new Queue<CCard>();
            this.card_queue_bonus = new Queue<CCard>();


            this.card_manager.make_all_cards(); //전체 카드 생성
            this.card_manager.SetOderCard(clone_cards);

            clear_turn_data();
        }

        /// <summary>
        /// 게임 한판 시작 전에 초기화 해야할 데이터.
        /// </summary>
        public void reset()
        {
            this.player_agents.Clear();

            this.distributed_players_cards[0].Clear();
            this.distributed_players_cards[1].Clear();

            this.IsPractice = false;
            missionSended = false;
            this.first_player_index = 0;
            this.current_player_index = 0;
            this.distributed_floor_cards.Clear();
            this.floor_manager.reset();

            clear_turn_data();
        }

        /// <summary>
        /// 매 턴 진행하기 전에 초기화 해야할 데이터.
        /// 이 때는 지난 턴에서 사용했던 데이터들을 모두 초기화 해 줍니다. 
        /// 플레이어가 낸 카드, 뒤집은 카드, 가져가야 할 카드등에 대한 변수들을 모두 초기화 
        /// 한 뒤 다음 플레이어의 게임 진행을 준비 합니다.
        /// </summary>
        public void clear_turn_data()
        {
            this.card_from_player = null;
            this.card_from_deck = null;
            this.target_cards_to_choice.Clear();
            this.same_card_count_with_player = 0;
            this.same_card_count_with_deck = 0;
            this.card_event_type = CARD_EVENT_TYPE.NONE;
            this.flipped_card_event_type.Clear();

            this.floor_cards_to_player.Clear();

            this.deck_flip_bonus_cards.Clear();

            this.cards_to_floor.Clear();
            this.cards_from_others_count = 0;
            this.other_cards_to_player[0].Clear();
            this.other_cards_to_player[1].Clear();
            this.bomb_cards_from_player.Clear();
            this.expected_result_type = PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD;
            this.shaking_cards.Clear();

            this.expected_result_type = PLAYER_SELECT_CARD_RESULT.COMPLETED;

            for (int i = 0; i < player_agents.Count; ++i)
            {
                player_agents[i].turnreset();
            }
        }
        public void addAgents(CPlayerAgent agent)
        {
            this.player_agents.Add(agent);
        }

        #region MatgoStart
        // 게임 시작.
        public void MatgoStart()
        {
            //카드 섞기
            CardLog = this.card_manager.ShuffleCard();

            this.card_queue.Clear();
            this.card_queue_bonus.Clear();

            bool pushBonus = false;
            // 보너스카드 밀어주기 (밀기)
            if (IsPractice == false)
            {
                for (int i = 0; i < this.player_agents.Count; ++i)
                {
                    if (this.player_agents[i].PushBonus == true)
                    {
                        if (start_player == this.player_agents[i].player_index)
                        {
                            pushBonus = this.player_agents[i].IsBonusCard = this.card_manager.pushBonus1(true);
                        }
                        else
                        {
                            pushBonus = this.player_agents[i].IsBonusCard = this.card_manager.pushBonus1(false);
                        }
                        if(pushBonus)
                        {
                            this.player_agents[i].BonusCardChance = rnd.Next() % 9 + 1;
                            //Log._log.InfoFormat("▣덱보너스턴:{0}", 10 - this.player_agents[i].BonusCardChance);
                        }
                        break;
                    }
                }
            }

            // 큐에 카드 적용
            this.card_manager.fill_to(card_queue, card_queue_bonus, pushBonus);

            DistributeCards(start_player); //카드 배분

            for (int i = 0; i < this.player_agents.Count; ++i)
            {
                this.player_agents[i].sort_player_hand_slots();
            }
        }
        // 카드 분배.
        // 카드를 섞은 뒤에는 바닥과 플레이어들에게 카드를 분배 합니다. 
        // 고스톱 게임에서 카드 분배는 바닥에 8장, 플레이어들에게 각각 10장씩 이루어집니다.
        // 바닥 -> 1P -> 2P -> 바닥 -> 1P -> 2P 이런 순서로 카드를 분배 합니다.
        void DistributeCards(byte start_player)
        {
            first_player_index = start_player;
            CPlayerAgent currentPlayerAgent = this_player_agent(this.first_player_index);
            CPlayerAgent nextPlayerAgent = this_player_agent_next(this.first_player_index);
            byte floor_index = 0;
            // 2번 반복하여 바닥에는 8장, 플레이어에게는 10장씩 돌아가도록 한다.
            for (int count = 0; count < 2; ++count)
            {
                // 바닥에 4장.
                for (byte i = 0; i < 4; ++i)
                {
                    CCard card = Pop_Front_Card();
                    this.distributed_floor_cards.Add(card);

                    this.floor_manager.put_to_begin_card(card);
                    ++floor_index;
                }

                // 1p에게 5장.
                for (int i = 0; i < 5; ++i)
                {
                    CCard card = Pop_Front_Card();
                    this.distributed_players_cards[currentPlayerAgent.player_index].Add(card);

                    currentPlayerAgent.add_card_to_Playerhand(card);
                }

                // 2p에게 5장.
                for (int i = 0; i < 5; ++i)
                {
                    CCard card = Pop_Front_Card();
                    this.distributed_players_cards[nextPlayerAgent.player_index].Add(card);

                    nextPlayerAgent.add_card_to_Playerhand(card);
                }
            }


        }
        public bool RefreshFloorCards(CPlayer currentPlayer)
        {
            distributed_floor_bonus_cards.Clear();
            distributed_bonus_floor_cards.Clear();
            bool floorBonus = this.hasStartFloorBonus(currentPlayer);
            this.floor_manager.refresh_floor_cards();

            return floorBonus;
        }
        #endregion MatgoStart

        #region TURN
        // 다음 플레이어로 이동
        public void move_to_next_player()
        {
            this.current_player_index = get_next_player_index();
        }
        // 다음 플레이어 인덱스
        byte MAX_PLAYER_COUNT = 2;
        public byte get_next_player_index()
        {
            if (this.current_player_index < MAX_PLAYER_COUNT - 1)
            {
                return (byte)(this.current_player_index + 1);
            }

            return 0;
        }
        // 다음 플레이어 인덱스 찾기
        public byte find_next_player_index_of(byte prev_player_index)
        {
            if (prev_player_index < MAX_PLAYER_COUNT - 1)
            {
                return (byte)(prev_player_index + 1);
            }

            return 0;
        }

        CPlayerAgent current_player_agent_next()
        {
            for (int i = 0; i < this.player_agents.Count; ++i)
            {
                if (this.player_agents[i].player_index != this.current_player_index)
                    return this.player_agents[i];
            }

            return null;
        }

        CPlayerAgent current_player_agent()
        {
            for (int i = 0; i < this.player_agents.Count; ++i)
            {
                if (this.player_agents[i].player_index == this.current_player_index)
                    return this.player_agents[i];
            }

            return null;
        }

        CPlayerAgent this_player_agent(byte player_index)
        {
            for (int i = 0; i < this.player_agents.Count; ++i)
            {
                if (this.player_agents[i].player_index == player_index)
                    return this.player_agents[i];

            }

            return null;
        }
        public CPlayerAgent this_player_agent_next(byte player_index)
        {
            for (int i = 0; i < this.player_agents.Count; ++i)
            {
                if (this.player_agents[i].player_index != player_index)
                    return this.player_agents[i];
            }

            return null;
        }
        #endregion TURN

        //처음 바닥에 보너스 카드가 있는가?
        List<CCard> bonusCards = new List<CCard>();
        public bool hasStartFloorBonus(CPlayer currentPlayer)
        {
            bonusCards.Clear();
            bonusCards = this.floor_manager.has_begincards_bonuscards();
            if (bonusCards.Count > 0)
            {
                distributed_floor_bonus_cards.AddRange(bonusCards);
                for (int i = 0; i < bonusCards.Count; ++i)
                {
                    // 선에게 지급.
                    currentPlayer.agent.add_card_to_Playerfloor(bonusCards[i]);
                    // 더미에서 다시 카드를 꺼낸다.
                    CCard card = Pop_Front_Card();
                    distributed_bonus_floor_cards.Add(card);
                    this.floor_manager.put_to_begin_card(card);
                }
                hasStartFloorBonus(currentPlayer);
                return true;
            }
            return false;
        }

        //덱에서 첫 카드 빼기
        bool Is_Deck_Card_Empty()
        {
            return this.card_queue.Count == 0;
        }
        CCard Pop_Front_Card()
        {
            CCard card = this.card_queue.Dequeue();
            return card;
        }
        CCard Pop_Bonus_Card()
        {
            CCard card = this.card_queue_bonus.Dequeue();
            return card;
        }
        void Put_Back_Card(CCard card)
        {
            this.card_queue.Enqueue(card);
        }
        int Count_Card()
        {
            return this.card_queue.Count;
        }

        #region PLAYER PUT HAND CARD
        // 플레이어가 카드를 낼 때 호출된다.
        public PLAYER_SELECT_CARD_RESULT Player_Put_HandCard(CPlayer player,
            byte card_number, PAE_TYPE pae_type, byte position,
            byte is_shaking)
        {
            //1. 플레이어가 낸 카드
            // 클라이언트가 보내온 카드 정보가 실제로 플레이어가 들고 있는 카드인지 확인한다.
            CCard card = player.agent.pop_card_from_Playerhand(card_number, pae_type, position);
            if (card == null)
            {
                // 비정상 데이터면 손패의 첫패 내고, 손패가 없으면 에러처리
                var cards = player.agent.GetPlayerHandsCards();
                if (cards == null || cards.Count == 0)
                {
                    Log._log.ErrorFormat("Player_Put_HandCard. 비정상 패 확인 : {0},{1},{2}", card_number, pae_type, position);
                    return PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD;
                }
                else
                {
                    Log._log.WarnFormat("Player_Put_HandCard. 비정상 패 확인 : {0},{1},{2}", card_number, pae_type, position);
                    card = player.agent.pop_card_from_Playerhand_Any();
                }
            }
            this.card_from_player = card;

            //보너스 카드는 바닥으로 가져오고 턴 다시 시작
            if (card.is_bonus_card())
            {
                return PLAYER_SELECT_CARD_RESULT.BONUS_CARD; // Player_Put_HandCard
            }

            // 2. 낸 카드와 같은 숫자의 카드가 바닥에 있나?
            // 바닥 카드중에서 플레이어가 낸 카드와 같은 숫자의 카드를 구한다.
            List<CCard> same_cards = this.floor_manager.get_cards(card.number);

            this.same_card_count_with_player = 0;
            if (same_cards != null)
            {
                foreach (var same_card in same_cards)
                {
                    if (same_card.is_same_number(card.number))
                    {
                        ++this.same_card_count_with_player;
                    }
                }
            }

            // 3. 낸 카드와 같은 숫자의 카드가 바닥에 몇 장 있나?
            switch (this.same_card_count_with_player)
            {
                case 0:
                    {
                        //흔든경우
                        if (is_shaking == 1)
                        {
                            byte count_from_hand =
                                player.agent.get_same_card_count_from_hand(this.card_from_player.number);
                            if (count_from_hand == 2)
                            {
                                this.card_event_type = CARD_EVENT_TYPE.SHAKING;//흔듬
                                player.agent.plus_shaking_count();

                                // 플레이어에게 흔든 카드 정보를 보내줄 때 사용하기 위해서 리스트에 보관해 놓는다.
                                this.shaking_cards =
                                    player.agent.find_same_cards_from_hand(
                                    this.card_from_player.number);
                                this.shaking_cards.Add(this.card_from_player);

                            }
                        }
                    }
                    break;

                case 1:
                    {
                        player.agent.front_card_match = true;

                        // 폭탄인 경우와 아닌 경우를 구분해서 처리 해 준다.
                        byte count_from_hand =
                            player.agent.get_same_card_count_from_hand(this.card_from_player.number);
                        //폭탄인경우
                        if (count_from_hand == 2)
                        {
                            this.card_event_type = CARD_EVENT_TYPE.BOMB;//폭탄

                            player.agent.plus_shaking_count();

                            // 플레이어가 선택한 카드와, 바닥 카드, 폭탄 카드를 모두 가져 간다.
                            this.floor_cards_to_player.Add(this.card_from_player);
                            this.floor_cards_to_player.Add(same_cards[0]);
                            this.bomb_cards_from_player.Add(this.card_from_player);
                            List<CCard> bomb_cards =
                                player.agent.pop_all_cards_from_Playerhand(this.card_from_player.number);
                            for (int i = 0; i < bomb_cards.Count; ++i)
                            {
                                this.floor_cards_to_player.Add(bomb_cards[i]);
                                this.bomb_cards_from_player.Add(bomb_cards[i]);
                            }

                            Add_Cards_from_Others(1);
                            player.agent.add_bomb_count(2);
                        }
                        else
                        {
                            this.floor_cards_to_player.Add(this.card_from_player);
                            this.floor_cards_to_player.Add(same_cards[0]);
                        }
                    }
                    break;

                case 2:
                    {
                        player.agent.front_card_match = true;

                        // 폭탄인 경우와 아닌 경우를 구분해서 처리 해 준다.
                        byte count_from_hand =
                            player.agent.get_same_card_count_from_hand(this.card_from_player.number);
                        // 2장 폭탄인 경우
                        if (count_from_hand == 1)
                        {
                            this.card_event_type = CARD_EVENT_TYPE.BOMB;//폭탄

                            // 2장 폭탄은 흔들기 X 
                            //2019-07-26 : 다시 추가
                            current_player_agent().plus_shaking_count();

                            // 플레이어가 선택한 카드와, 바닥 카드, 폭탄 카드를 모두 가져 간다.
                            this.floor_cards_to_player.Add(this.card_from_player);
                            this.bomb_cards_from_player.Add(this.card_from_player);
                            for (int i = 0; i < same_cards.Count; ++i)
                            {
                                this.floor_cards_to_player.Add(same_cards[i]);
                            }
                            List<CCard> bomb_cards =
                                player.agent.pop_all_cards_from_Playerhand(this.card_from_player.number);
                            for (int i = 0; i < bomb_cards.Count; ++i)
                            {
                                this.floor_cards_to_player.Add(bomb_cards[i]);
                                this.bomb_cards_from_player.Add(bomb_cards[i]);
                            }

                            Add_Cards_from_Others(1);
                            player.agent.add_bomb_count(1);
                        }
                        else
                        {
                            // 똥 쌍피 일경우도 포함
                            if (same_cards[0].pae_type != same_cards[1].pae_type ||
                               (same_cards[0].pae_type == same_cards[1].pae_type && same_cards[0].number == 10 && (same_cards[0].status == CARD_STATUS.TWO_PEE || same_cards[1].status == CARD_STATUS.TWO_PEE)))
                            {
                                // 카드 종류가 다르다면 플레이어가 한장을 선택할 수 있도록 해준다. 
                                this.target_cards_to_choice.Clear();
                                for (int i = 0; i < same_cards.Count; ++i)
                                {
                                    this.target_cards_to_choice.Add(same_cards[i]);
                                }

                                this.expected_result_type = PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER;
                                return PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER;
                            }
                            // 같은 종류의 카드라면 플레이어가 선택할 필요가 없으므로 첫번째 카드를 선택해 준다.
                            this.floor_cards_to_player.Add(this.card_from_player);

                            // 가능한 쌍피를 먹는다
                            if (same_cards[0].status == CARD_STATUS.TWO_PEE)
                            {
                                this.floor_cards_to_player.Add(same_cards[0]);
                            }
                            else
                            {
                                this.floor_cards_to_player.Add(same_cards[1]);
                            }
                        }
                    }
                    break;

                case 3:
                    {
                        player.agent.front_card_match = true;

                        if (player.agent.ppuk_card_numbers.Contains(card_number))
                        {
                            // 자뻑. 피 2장 가져옴. 뻑에 보너스카드가 껴있으면 피를 더 가져옴
                            this.card_event_type = CARD_EVENT_TYPE.SELF_EAT_PPUK;

                            this.floor_cards_to_player.Add(card);
                            for (int i = 0; i < same_cards.Count; ++i)
                            {
                                this.floor_cards_to_player.Add(same_cards[i]);

                                if (same_cards[i].is_bonus_card() && !deck_flip_bonus_cards.Contains(same_cards[i]))
                                {
                                    Add_Cards_from_Others(1);
                                }
                            }

                            Add_Cards_from_Others(2);
                        }
                        else
                        {
                            // 뻑. 피 1장 가져옴. 뻑에 보너스카드가 껴있으면 피를 더 가져옴
                            this.card_event_type = CARD_EVENT_TYPE.EAT_PPUK;

                            this.floor_cards_to_player.Add(card);
                            for (int i = 0; i < same_cards.Count; ++i)
                            {
                                this.floor_cards_to_player.Add(same_cards[i]);
                                if (same_cards[i].is_bonus_card() && !deck_flip_bonus_cards.Contains(same_cards[i]))
                                {
                                    Add_Cards_from_Others(1);
                                }
                            }

                            Add_Cards_from_Others(1);
                        }
                    }
                    break;
                default:
                    {
                        Log._log.ErrorFormat("same_card_count_with_player 비정상접근 감지 {0}\n", this.same_card_count_with_player);
                    }
                    break;
            }

            return PLAYER_SELECT_CARD_RESULT.COMPLETED;
        }
        public CCard Player_Put_Bonus_Card(CPlayer player)
        {
            //바닥에서 내논 보너스카드를 다시 가져온다
            this.floor_cards_to_player.Clear();
            this.floor_cards_to_player.Add(this.card_from_player);
            Give_Floorcards_to_Player(player);
            //덱에서 카드를 뽑아 내 핸드로 가져온다
            CCard card = Pop_Front_Card();
            player.agent.add_card_to_Playerhand(card);

            Add_Cards_from_Others(1);
            return card;
        }
        #endregion PLAYER PUT HAND CARD

        #region PLAYER FLIP DECK CARD
        // 덱에서 카드를 뒤집어 그에 맞는 처리를 진행한다.
        public PLAYER_SELECT_CARD_RESULT Player_Flip_DeckCard(CPlayer player, FLIP_TYPE flip_type, ref bool whalbin)
        {
            byte card_number_from_player = byte.MaxValue;
            if (flip_type == FLIP_TYPE.FLIP_BOOM || this.card_from_player == null)
            {
                set_card_from_player_null();
            }
            else
            {
                card_number_from_player = this.card_from_player.number;
            }

            PLAYER_SELECT_CARD_RESULT result = Flip_DeckCard(player.agent, card_number_from_player, ref whalbin);

            if (result == PLAYER_SELECT_CARD_RESULT.COMPLETED || result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                After_Flip_DeckCard(player);
                return result;
            }

            return result;
        }
        //더미에서 카드를 뽑음
        PLAYER_SELECT_CARD_RESULT Flip_DeckCard(CPlayerAgent player_agent, byte card_number_from_player, ref bool whalbin)
        {
            if (Is_Deck_Card_Empty() == true)
            {
                Log._log.WarnFormat("Flip_DeckCard. Is_Deck_Card_Empty. card_number_from_player:{0}", card_number_from_player);
            }

            if(player_agent.IsBonusCard) // 보너스 뒷패 밀어주기
            {
                if(player_agent.GetPlayerHandsCards().Count <= player_agent.BonusCardChance)
                {
                    var cardBonus = Pop_Bonus_Card();
                    if (cardBonus != null)
                    {
                        this.card_from_deck = cardBonus;
                    }
                    else
                    {
                        this.card_from_deck = Pop_Front_Card();
                    }
                    player_agent.IsBonusCard = false;
                }
                else
                {
                    this.card_from_deck = Pop_Front_Card();
                }
            }
            else
            {
                this.card_from_deck = Pop_Front_Card();
            }

            // 보너스 못먹음
            if (player_agent.PenaltyType1)
            {
                int cardCount = Count_Card();
                for (int i = 0; i < cardCount; ++i)
                {
                    // 맞는 패 있는 지 확인, 보너스 패면 교체, 뻑일경우 패스
                    if (this.card_from_deck.is_bonus_card())
                    {
                        Put_Back_Card(this.card_from_deck);
                        this.card_from_deck = Pop_Front_Card();
                        continue;
                    }

                    break;
                }
            }

            // 뻑 확률 반토막으로
            {
                if (this.floor_manager.get_cards_count(this.card_from_deck.number) == 1)
                {
                    if (card_number_from_player == this.card_from_deck.number)
                    {
                        if (rnd.NextDouble() <= 0.5)
                        {
                            Put_Back_Card(this.card_from_deck);
                            this.card_from_deck = Pop_Front_Card();
                            //Log._log.Info("뻑교체 O");
                        }
                        else
                        {
                            //Log._log.Info("뻑교체 X");
                        }
                    }
                }
            }

            // 밀어주기 검사
            if (player_agent.AdvantageType2 || player_agent.AdvantageType3)
            {
                // 발동 조건 : 무조건
                if (true)
                {
                    bool Advantaged2 = false;
                    bool Advantaged3 = false;
                    int cardCount = Count_Card();
                    for (int i = 0; i < cardCount; ++i)
                    {
                        // 맞는 패 있는 지 확인, 보너스 패면 패스, 뻑일경우 교체
                        if (this.card_from_deck.is_bonus_card()) break;

                        // 시뮬레이션
                        switch (this.floor_manager.get_cards_count(this.card_from_deck.number))
                        {
                            case 0:
                                {
                                    if (card_number_from_player == this.card_from_deck.number)
                                    {
                                        // 쪽
                                        Advantaged2 = true;
                                    }
                                }
                                break;

                            case 1:
                                {
                                    if (card_number_from_player == this.card_from_deck.number)
                                    {
                                        // 뻑
                                    }
                                    else
                                    {
                                        // 뒷장 붙음
                                        Advantaged3 = true;
                                    }
                                }
                                break;

                            case 2:
                                {
                                    player_agent.trigger_chain_ppuk_reset();

                                    if (card_number_from_player == this.card_from_deck.number)
                                    {
                                        // 따닥
                                        Advantaged2 = true;
                                    }
                                    else
                                    {
                                        // 둘중 하나
                                        Advantaged3 = true;
                                    }
                                }
                                break;

                            case 3:
                                {
                                    if (player_agent.ppuk_card_numbers.Contains(this.card_from_deck.number))
                                    {
                                        // 자뻑 먹음
                                        Advantaged2 = true;
                                    }
                                    else
                                    {
                                        // 뻑 먹음
                                        Advantaged2 = true;
                                    }
                                }
                                break;
                        }

                        if ((player_agent.AdvantageType2 && Advantaged2) || (player_agent.AdvantageType3 && Advantaged3)) break;

                        Put_Back_Card(this.card_from_deck);
                        this.card_from_deck = Pop_Front_Card();
                    }
                }
            }
            // 패널티 검사
            else if (player_agent.PenaltyType2 || player_agent.PenaltyType3)
            {
                // 발동 조건 : 무조건
                if (true)
                {
                    bool Penalty2 = true;
                    bool Penalty3 = true;

                    int cardCount = Count_Card();
                    for (int i = 0; i < cardCount; ++i)
                    {
                        // 맞는 패 있는 지 확인, 보너스 패면 교체, 뻑일경우 패스
                        if (this.card_from_deck.is_bonus_card())
                        {
                            Put_Back_Card(this.card_from_deck);
                            this.card_from_deck = Pop_Front_Card();
                            continue;
                        }

                        // 시뮬레이션
                        switch (this.floor_manager.get_cards_count(this.card_from_deck.number))
                        {
                            case 0:
                                {
                                    if (card_number_from_player == this.card_from_deck.number)
                                    {
                                        // 쪽
                                        Penalty2 = false;
                                    }
                                }
                                break;

                            case 1:
                                {
                                    if (card_number_from_player == this.card_from_deck.number)
                                    {
                                        // 뻑

                                        // 내손에 없는 패일 경우 (자뻑 방지)
                                        if (player_agent.GetPlayerHandsCards().Exists(x => x.is_same_number(this.card_from_deck.number)) == true)
                                        {
                                            Penalty2 = false;
                                        }
                                    }
                                    else
                                    {
                                        // 뒷장 붙음
                                        Penalty3 = false;
                                    }
                                }
                                break;

                            case 2:
                                {
                                    player_agent.trigger_chain_ppuk_reset();

                                    if (card_number_from_player == this.card_from_deck.number)
                                    {
                                        // 따닥
                                        Penalty2 = false;
                                    }
                                    else
                                    {
                                        // 둘중 하나
                                        Penalty2 = false;
                                    }
                                }
                                break;

                            case 3:
                                {
                                    if (player_agent.ppuk_card_numbers.Contains(this.card_from_deck.number))
                                    {
                                        // 자뻑 먹음
                                        Penalty2 = false;
                                    }
                                    else
                                    {
                                        // 뻑 먹음
                                        Penalty2 = false;
                                    }
                                }
                                break;
                        }

                        if ((player_agent.PenaltyType2 && Penalty2) || (player_agent.PenaltyType3 && Penalty3)) break;

                        Put_Back_Card(this.card_from_deck);
                        this.card_from_deck = Pop_Front_Card();
                    }
                }
            }

            if (this.card_from_deck.is_bonus_card())
            {
                // 덱에서 보너스패를 뽑음
                return PLAYER_SELECT_CARD_RESULT.BONUS_CARD; // Player_Flip_DeckCard
            }

            // 더미 카드랑 같은 숫자 카드 있는지 확인
            List<CCard> same_cards = this.floor_manager.get_cards(this.card_from_deck.number);

            this.same_card_count_with_deck = 0;
            if (same_cards != null)
            {
                foreach (var same_card in same_cards)
                {
                    if (card_from_player == same_card) continue;

                    if (same_card.is_same_number(this.card_from_deck.number))
                    {
                        ++this.same_card_count_with_deck;
                    }
                }
            }

            switch (same_card_count_with_deck)
            {
                case 0:
                    {
                        player_agent.trigger_chain_ppuk_reset();

                        if (card_number_from_player == this.card_from_deck.number)
                        {
                            // 쪽 (플레이어한테 남는 패가 없을때는 규칙 미적용)
                            if (player_agent.is_empty_on_hand() == false)
                            {
                                player_agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.KISS);
                                this.flipped_card_event_type.Add(CARD_EVENT_TYPE.KISS);
                                Add_Cards_from_Others(1);
                            }

                            this.floor_cards_to_player.Clear();
                            this.floor_cards_to_player.Add(this.card_from_player);
                            this.floor_cards_to_player.Add(this.card_from_deck);

                            whalbin = true;
                        }
                    }
                    break;

                case 1:
                    {
                        if (card_number_from_player == this.card_from_deck.number)
                        {
                            // 뻑 (플레이어한테 남는 패가 없을때는 규칙 미적용)
                            if (player_agent.is_empty_on_hand() == false)
                            {
                                // 첫뻑, 연뻑 트리거
                                CheckPpukTrigger(player_agent);

                                player_agent.plus_ppuk_count();
                                player_agent.ppuk_card_numbers.Add(card_number_from_player);
                                // 플레이어에게 주려던 카드를 모두 취소한다.
                                this.floor_cards_to_player.Clear();
                                this.deck_flip_bonus_cards.Clear();
                                this.Clear_Cards_from_Others();
                            }
                            else
                            {
                                player_agent.trigger_chain_ppuk_reset();
                                if (same_cards.Count == 2 && same_cards[0].pae_type != same_cards[1].pae_type/* && flip_type != FLIP_TYPE.FLIP_BONUS*/)
                                {
                                    this.target_cards_to_choice.Clear();
                                    for (int i = 0; i < same_cards.Count; ++i)
                                    {
                                        this.target_cards_to_choice.Add(same_cards[i]);
                                    }
                                    whalbin = true;
                                    this.expected_result_type = PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK;
                                    return PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK;
                                }
                                else
                                {
                                    // 각 플레이어한테 1장, 바닥에 1장, 덱에 1장 있을 때 발생(뻑 처리하는 대신 손에서 낸패만 먹는다.)
                                    //this.floor_cards_to_player.Add(this.card_from_deck);
                                    //this.floor_cards_to_player.Add(same_cards[0]);
                                    //whalbin = true;
                                }
                            }
                        }
                        else
                        {
                            this.floor_cards_to_player.Add(this.card_from_deck);
                            this.floor_cards_to_player.Add(same_cards[0]);
                            whalbin = true;
                            player_agent.trigger_chain_ppuk_reset();
                        }
                    }
                    break;

                case 2:
                    {
                        player_agent.trigger_chain_ppuk_reset();

                        if (card_number_from_player == this.card_from_deck.number)
                        {
                            // 따닥.
                            if (player_agent.is_first_hand() == true)
                            {
                                // 첫 따닥 7점
                                this.flipped_card_event_type.Add(CARD_EVENT_TYPE.FIRST_DDADAK);
                                if (IsPractice == false)
                                    game_players_money_change(player_agent, baseMoney * 7);

                            }
                            else
                            {
                                // 일반 따닥
                                this.flipped_card_event_type.Add(CARD_EVENT_TYPE.DDADAK);
                            }

                            // 따닥 트리거
                            //player_agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.DDADAK);

                            // 플레이어가 4장 모두 가져간다.
                            this.floor_cards_to_player.Clear();
                            for (int i = 0; i < same_cards.Count; ++i)
                            {
                                if (!same_cards[i].is_bonus_card())
                                {
                                    this.floor_cards_to_player.Add(same_cards[i]);
                                }
                            }
                            this.floor_cards_to_player.Add(this.card_from_deck);
                            this.floor_cards_to_player.Add(this.card_from_player);

                            Add_Cards_from_Others(1);
                            whalbin = true;
                        }
                        else
                        {
                            // 똥 쌍피 일경우도 포함
                            if (same_cards[0].pae_type != same_cards[1].pae_type ||
                               (same_cards[0].pae_type == same_cards[1].pae_type && same_cards[0].number == 10 && (same_cards[0].status == CARD_STATUS.TWO_PEE || same_cards[1].status == CARD_STATUS.TWO_PEE)))
                            {
                                // 뒤집었는데 타입이 다른 카드 두장과 같다면 한장을 선택하도록 한다.
                                this.target_cards_to_choice.Clear();
                                for (int i = 0; i < same_cards.Count; ++i)
                                {
                                    this.target_cards_to_choice.Add(same_cards[i]);
                                }

                                whalbin = true;
                                this.expected_result_type = PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK;
                                return PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK;
                            }
                            else
                            {
                                // 카드 타입이 같다면 첫번째 카드를 선택해 준다.
                                this.floor_cards_to_player.Add(this.card_from_deck);

                                // 가능한 쌍피를 먹는다
                                if (same_cards[0].status == CARD_STATUS.TWO_PEE)
                                {
                                    this.floor_cards_to_player.Add(same_cards[0]);
                                }
                                else
                                {
                                    this.floor_cards_to_player.Add(same_cards[1]);
                                }

                                whalbin = true;
                            }
                        }
                    }
                    break;

                case 3:
                    {
                        player_agent.trigger_chain_ppuk_reset();

                        if (player_agent.ppuk_card_numbers.Contains(this.card_from_deck.number))
                        {
                            // 자뻑. 피 2장 가져옴. 뻑에 보너스카드가 껴있으면 피를 더 가져옴
                            player_agent.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.JA_PPUK);
                            this.flipped_card_event_type.Add(CARD_EVENT_TYPE.SELF_EAT_PPUK);

                            for (int i = 0; i < same_cards.Count; ++i)
                            {
                                this.floor_cards_to_player.Add(same_cards[i]);
                                if (same_cards[i].is_bonus_card() && !deck_flip_bonus_cards.Contains(same_cards[i]))
                                {
                                    Add_Cards_from_Others(1);
                                }
                            }
                            this.floor_cards_to_player.Add(this.card_from_deck);

                            Add_Cards_from_Others(2);
                            whalbin = true;
                        }
                        else
                        {
                            // 뻑. 피 1장 가져옴. 뻑에 보너스카드가 껴있으면 피를 더 가져옴
                            this.flipped_card_event_type.Add(CARD_EVENT_TYPE.EAT_PPUK);

                            for (int i = 0; i < same_cards.Count; ++i)
                            {
                                this.floor_cards_to_player.Add(same_cards[i]);
                                if (same_cards[i].is_bonus_card() && !deck_flip_bonus_cards.Contains(same_cards[i]))
                                {
                                    Add_Cards_from_Others(1);
                                }
                            }
                            this.floor_cards_to_player.Add(this.card_from_deck);

                            Add_Cards_from_Others(1);
                            whalbin = true;
                        }
                    }
                    break;
                default:
                    {
                        Log._log.ErrorFormat("Flip_DeckCard. same_card_count_with_deck:{0}", same_card_count_with_deck);
                    }
                    break;
            }

            return PLAYER_SELECT_CARD_RESULT.COMPLETED;
        }
        // 카드를 뒤집은 후 처리해야할 내용들을 진행한다.
        void After_Flip_DeckCard(CPlayer player)
        {
            // 뒤집은 카드가 보너스 카드일 경우
            if (this.card_from_deck.is_bonus_card())
            {
                // 또 뒤집을거니깐 카드를 정리하지 않는다.
                deck_flip_bonus_cards.Add(this.card_from_deck);
            }
            else
            {
                // 뒤집어서 나온 보너스 카드 처리
                for (int i = 0; i < this.deck_flip_bonus_cards.Count; ++i)
                {
                    floor_cards_to_player.Add(this.deck_flip_bonus_cards[i]);
                    Add_Cards_from_Others(1);
                }
                deck_flip_bonus_cards.Clear();

                // 플레이어가 가져갈 카드와 바닥에 내려놓을 카드를 정리한다.
                Give_Floorcards_to_Player(player);
            }

            sort_player_pae();

            // 플레이어가 낸 패가 붙지 않았으면 바닥에 놓는다.
            if (this.card_from_player != null/* && flip_type == FLIP_TYPE.FLIP_NORMAL*/)
            {
                // 플레이어가 가져갈 카드 중에 냈던 카드가 포함되어 있지 않으면 바닥에 내려 놓는다.
                bool is_get_card_from_player = this.floor_cards_to_player.Exists(obj => obj.is_same(
                    this.card_from_player.number,
                    this.card_from_player.position));
                if (!is_get_card_from_player)
                {
                    this.floor_manager.puton_card(this.card_from_player);
                    this.cards_to_floor.Add(this.card_from_player);
                }
            }

            // 덱에서 뒤집어서 나온 패가 붙지 않았으면 바닥에 놓는다.
            bool is_get_card_from_deck = this.floor_cards_to_player.Exists(obj => obj.is_same(
                this.card_from_deck.number,
                this.card_from_deck.position));
            if (!is_get_card_from_deck)
            {
                // 보너스 카드는 플레이어가 냈었던 패에 붙인다.
                if (card_from_deck.is_bonus_card() && card_from_player != null)
                {
                    this.floor_manager.puton_card(this.card_from_deck, card_from_player.number);
                }
                else
                {
                    this.floor_manager.puton_card(this.card_from_deck);
                }
                this.cards_to_floor.Add(this.card_from_deck);
            }

            // 싹쓸이 (플레이어한테 남는 패가 없을때는 규칙 미적용)
            if (this.floor_manager.is_empty())
            {
                if (player.agent.is_empty_on_hand() == false)
                {
                    this.flipped_card_event_type.Add(CARD_EVENT_TYPE.CLEAN);
                    Add_Cards_from_Others(1);
                }
            }
        }
        #endregion PLAYER FLIP DECK CARD

        #region Player Choose Card
        //카드 선택
        public PLAYER_SELECT_CARD_RESULT on_choose_card(CPlayer player, byte choice_index)
        {
            // 클라이언트에서 엉뚱한 값을 보내올 수 있으므로 검증 후 이상이 있으면 첫번째 카드를 선택한다.
            CCard player_choose_card = null;
            if (choice_index < 0 || choice_index > 1)
            {
                Log._log.WarnFormat("on_choose_card. target_cards_to_choice.Count:{1} choice_index:{2} player:{0}", player.data.userID, target_cards_to_choice.Count, choice_index);
                player_choose_card = this.target_cards_to_choice[0];
            }
            else
            {
                player_choose_card = this.target_cards_to_choice[choice_index];
            }

            switch (this.expected_result_type)
            {
                case PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER:
                    this.floor_cards_to_player.Add(this.card_from_player);
                    this.floor_cards_to_player.Add(player_choose_card);
                    return this.expected_result_type;

                case PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK:
                    this.floor_cards_to_player.Add(this.card_from_deck);
                    this.floor_cards_to_player.Add(player_choose_card);
                    After_Flip_DeckCard(player);
                    break;
            }

            return PLAYER_SELECT_CARD_RESULT.COMPLETED;
        }
        #endregion Player Choose Card

        #region Give Floorcards to Player
        void Give_Floorcards_to_Player(CPlayer player)
        {
            for (int i = 0; i < this.floor_cards_to_player.Count; ++i)
            {
                //UnityEngine.Debug.Log("give player " + this.cards_to_give_player[i].number);
                player.agent.add_card_to_Playerfloor(this.floor_cards_to_player[i]);
                this.floor_manager.remove_card(this.floor_cards_to_player[i]);
            }
        }
        #endregion Give Floorcards to Player

        void sort_player_pae()
        {
            //??
        }

        //점수계산
        public void Calculate_Players_Score(byte drow_count)
        {
            // 족보 계산
            for (int i = 0; i < this.player_agents.Count; ++i)
            {
                this.player_agents[i].calculate_score();
            }

            // 피박 광박 고박 배수
            for (byte i = 0; i < this.player_agents.Count; ++i)
            {
                CPlayerAgent atkPlayer = this_player_agent(i);
                CPlayerAgent defPlayer = this_player_agent_next(i);

                if (atkPlayer.get_pee_count() >= 10)
                {
                    defPlayer.GetPibakCheck();
                }
                else
                {
                    defPlayer.SetPiBak(false);
                }

                if (atkPlayer.get_kwang_count() >= 3)
                {
                    defPlayer.GetKwangBakCheck();
                }
                else
                {
                    defPlayer.SetKwangBak(false);
                }

                defPlayer.GetGoBakCheck();

                atkPlayer.CalcMultiple(defPlayer, drow_count);
            }
        }

        void Add_Cards_from_Others(byte pee_count)
        {
            this.cards_from_others_count += pee_count;
        }
        void Clear_Cards_from_Others()
        {
            this.cards_from_others_count = 0;
        }

        //다른 플레이어에게서 카드 가져와
        public void Get_Cards_from_Others(CPlayer currentPlayer)
        {
            if (this.cards_from_others_count == 0) return;

            CPlayerAgent otherPlayerAgent = this_player_agent_next(currentPlayer.player_index);

            if (!this.other_cards_to_player.ContainsKey(otherPlayerAgent.player_index))
            {
                this.other_cards_to_player[otherPlayerAgent.player_index].Clear();
            }

            List<CCard> cards = otherPlayerAgent.pop_card_from_Playerfloor(this.cards_from_others_count);
            this.cards_from_others_count = 0;
            if (cards == null)
            {
                return;
            }

            for (int i = 0; i < cards.Count; ++i)
            {
                currentPlayer.agent.add_card_to_Playerfloor(cards[i]);
                this.other_cards_to_player[otherPlayerAgent.player_index].Add(cards[i]);
            }
        }

        #region Player Turn End
        public bool is_time_to_ask_gostop(CPlayer currentPlayer)
        {
            return currentPlayer.agent.can_finish();
        }
        public bool is_finished()
        {
            if (this.card_queue.Count <= 0)
            {
                return true;
            }
            return false;
        }
        public bool check_kookjin(CPlayerAgent player_agent)
        {
            if (player_agent.is_used_kookjin)
            {
                return false;
            }

            if (!player_agent.existKookjin())
            {
                return false;
            }

            // 국진 선택조건 확인
            if (!player_agent.can_move_kookjin())
            {
                return false;
            }

            return true;
        }

        public bool is_floor_chongtong(out byte number)
        {
            for (byte i = 0; i < 12; ++i)
            {
                byte count = this.floor_manager.get_same_card_count_form_floor(i);
                if (count == 4)
                {
                    number = i;
                    return true;
                }
            }
            number = 12;
            return false;
        }
        public bool is_player_chongtong(byte player_index, out byte number)
        {
            for (byte i = 0; i < 12; ++i)
            {
                byte count = this_player_agent(player_index).get_same_card_count_from_hand(i);
                if (count == 4)
                {
                    number = i;
                    return true;
                }
            }
            number = 12;
            return false;
        }
        #endregion Player Turn End

        Random rnd = new Random((int)DateTime.UtcNow.Ticks);
        List<CCard> clone_cards = new List<CCard>();
        public void get_random_order_cards(List<CCard> order_cards, byte firstTurnIndex)
        {
            order_cards.Clear();

            if (firstTurnIndex == 1)
            {
                int index1 = rnd.Next(1, 12);
                int index2 = rnd.Next(0, index1);
                order_cards.Add(clone_cards[index1*4 + rnd.Next(0,4)]);
                order_cards.Add(clone_cards[index2 * 4 + rnd.Next(0, 4)]);
            }
            else if (firstTurnIndex == 2)
            {
                int index2 = rnd.Next(1, 12);
                int index1 = rnd.Next(0, index2);
                order_cards.Add(clone_cards[index1 * 4 + rnd.Next(0, 4)]);
                order_cards.Add(clone_cards[index2 * 4 + rnd.Next(0, 4)]);
            }
            else
            {
                int index1 = rnd.Next(0, clone_cards.Count);
                order_cards.Add(clone_cards[index1]);

                int index2 = rnd.Next(0, clone_cards.Count);
                order_cards.Add(clone_cards[index2]);

                while (order_cards[0].is_same_number(order_cards[1].number))
                {
                    order_cards.RemoveAt(1);
                    order_cards.Add(clone_cards[rnd.Next(0, clone_cards.Count)]);
                }
            }
        }

        public GAME_RESULT_TYPE get_player_result()
        {
            if (start_player == current_player_index)
            {
                return GAME_RESULT_TYPE.START_PLAYER_WIN;
            }
            else if (start_player == this.get_next_player_index())
            {
                return GAME_RESULT_TYPE.LAST_PLAYER_WIN;
            }

            return GAME_RESULT_TYPE.NONE;
        }

        public GAME_RESULT_TYPE get_player_result_ppuk()
        {
            if (start_player == current_player_index)
            {
                return GAME_RESULT_TYPE.START_PLAYER_THREEPPUK;
            }
            else if (start_player == this.get_next_player_index())
            {
                return GAME_RESULT_TYPE.LAST_PLAYER_THREEPPUK;
            }

            return GAME_RESULT_TYPE.NONE;
        }

        public void set_card_from_player_null()
        {
            this.card_from_player = null;
        }

        #region CHECK_FUNC
        void CheckPpukTrigger(CPlayerAgent pa)
        {
            if (pa.is_first_hand())
            {
                // 첫뻑 7점
                this.flipped_card_event_type.Add(CARD_EVENT_TYPE.FIRST_PPUK);
                if (IsPractice == false)
                    game_players_money_change(pa, baseMoney * 7);

                pa.trigger_mission_check(MISSION_TAPSSAHGI_TYPE.FIRST_PPUK);
                pa.trigger_chain_ppuk();
            }
            else if (pa.is_second_hand() && pa.ppuk_chain == 1)
            {
                // 2연뻑 14점
                this.flipped_card_event_type.Add(CARD_EVENT_TYPE.SECOND_PPUK);
                if (IsPractice == false)
                    game_players_money_change(pa, baseMoney * 14);

                pa.trigger_chain_ppuk();
            }
            else if (pa.is_third_hand() && pa.ppuk_chain == 2)
            {
                // 3연뻑 21점
                this.flipped_card_event_type.Add(CARD_EVENT_TYPE.THIRD_PPUK);
                if (IsPractice == false)
                    game_players_money_change(pa, baseMoney * 21);

                // 3연뻑
                pa.trigger_chain_ppuk();
            }
            else
            {
                // 일반 뻑 
                this.flipped_card_event_type.Add(CARD_EVENT_TYPE.PPUK);
                pa.trigger_chain_ppuk_reset();
            }
        }
        #endregion CHECK_FUNC

        void game_players_money_change(CPlayerAgent pa, long money_change)
        {
            CPlayerAgent other_pa = null;
            foreach (var p in player_agents)
            {
                if (p.player_index != pa.player_index)
                {
                    other_pa = p;
                    break;
                }
            }

            if (other_pa == null) return;
            long changeMoney = other_pa.subtractMoney(money_change);
            pa.addMoney(changeMoney);

            other_pa.inGameChangeMoney -= changeMoney;
            other_pa.money_var -= changeMoney;
            pa.inGameChangeMoney += changeMoney;
            pa.money_var += changeMoney;
        }

        public int GetCurrentDeckCardCount()
        {
            return this.card_queue.Count;
        }
        public List<CFloorSlot> GetCurrentFloorSlots()
        {
            return this.floor_manager.slots;
        }
    }
}
