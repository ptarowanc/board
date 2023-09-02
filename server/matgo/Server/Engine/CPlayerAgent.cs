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
        None,
        Play,
        OrderCard,
        DistributeCard,
        DealCard,
        TurnStart,
        TurnSelect,
        KookJin,
        GoStop,
    }
    public class CPlayerAgent
    {
        private CGameRoom joinRoom;
        public UserGameStatus status;
        public FLIP_TYPE flip_type;
        public byte player_index;
        public long haveMoney { get; private set; }
        public long inGameChangeMoney;

        private Dictionary<PAE_TYPE, List<CCard>> floor_pae;
        private List<CCard> hand_pae;

        public short score { get; private set; }
        public short prev_score { get; private set; }
        public byte missionscore { get; set; }
        public byte missionresult { get; set; }
        public short chong_tong_number { get; private set; }
        public short multiple { get; private set; } // 배수

        // 조건 확인용
        public byte go_count { get; private set; }
        public byte shaking_count { get; private set; }
        public byte ppuk_count { get; private set; }
        public byte ppuk_chain { get; private set; }
        public byte remain_bomb_count { get; private set; }
        public bool is_used_kookjin { get; private set; }
        public List<byte> ppuk_card_numbers { get; set; }
        public List<PLAYER_FLOOR_CHECK> PlayerFloorCheck { get; private set; }
        public bool put_card_bonus { get; set; }
        public bool front_card_match { get; set; }

        // 족보
        private bool bisamkwang;
        private bool samkwang;
        private bool forekwang;
        private bool fivekwang;
        private bool godori;
        private bool hongdan;
        private bool chungdan;
        private bool chodan;

        // 박
        public bool mungtta { get; private set; }
        public bool pibak { get; private set; }
        public bool kwangbak { get; private set; }
        public bool gobak { get; private set; }

        public byte order_position = byte.MaxValue; // 선잡기 번호
        public long money_var = 0; // 변동머니

        public int win_count;   // 현재 방에서 연속으로 이긴 횟수 (탑쌓기 이벤트 확인)

        public bool PushBonus;
        public bool IsBonusCard;
        public int BonusCardChance;

        public bool Advantage;
        public bool AdvantageType1; // 보너스 카드 먹음
        public bool AdvantageType2; // 쪽, 따닥, 뻑 다 먹음
        public bool AdvantageType3; // 뒷패 않맞을때 맞음
        public bool Penalty;
        public bool PenaltyType1; // 보너스 카드 못 먹음
        public bool PenaltyType2; // 자뻑 방지
        public bool PenaltyType3; // 뒷패 안 맞음 (쪽 따닥 뻑 다 안맞음)

        public MessageTemp currentMsg;   // 자동처리할때 넘길 현재 패킷
        public MessageTemp LastMsg;   // 처리한 마지막 패킷
        //public bool isActionExecute;   // 게임 행동의 처리 여부
        public long actionTimeLimit; // 네트워크 지연 시 AI로 전환시킬 제한시간
        public ConcurrentQueue<MessageTemp> QueueMsg; // 처리할 목록

        public struct MissionData
        {
            public byte type;
            public bool isComplete;
        }

        //public List<MissionData> topMission;
        public bool mission_update;

        void SetMission(int index, bool Success)
        {
            //MissionData Mission = topMission[index];
            //Mission.isComplete = Success;
            //topMission[index] = Mission;
        }
        public short CalcMultiple(CPlayerAgent defPlayer, byte drow_count)
        {
            multiple = 1;

            // 스톱 했을때 받을 수 있는 머니 계산, 전송
            if (defPlayer.gobak) // 고박
                multiple *= 2;
            if (defPlayer.pibak) // 피박
                multiple *= 2;
            if (defPlayer.kwangbak) // 광박
                multiple *= 2;
            if (mungtta) // 멍따
                multiple *= 2;
            if (shaking_count > 0) // 흔들기
                multiple *= (short)Math.Pow(2, shaking_count);
            if (go_count >= 3) // 3고 이상
                multiple *= (short)Math.Pow(2, go_count - 2);
            if (drow_count >= 1) // 나가리
                multiple *= drow_count;
            multiple *= missionscore; // 미션

            return multiple;
        }
        public CPlayerAgent(byte player_index, CGameRoom room)
        {
            this.status = UserGameStatus.None;
            this.player_index = player_index;
            this.joinRoom = room;

            haveMoney = -1;
            inGameChangeMoney = 0;

            floor_pae = new Dictionary<PAE_TYPE, List<CCard>>();
            hand_pae = new List<CCard>();

            score = 0;
            prev_score = 0;
            missionscore = 1;
            missionresult = 0;
            chong_tong_number = 12;
            multiple = 0;

            go_count = 0;
            shaking_count = 0;
            ppuk_count = 0;
            ppuk_chain = 0;
            remain_bomb_count = 0;
            is_used_kookjin = false;
            put_card_bonus = false;
            front_card_match = false;

            PlayerFloorCheck = new List<PLAYER_FLOOR_CHECK>();
            ppuk_card_numbers = new List<byte>();

            bisamkwang = samkwang = forekwang = fivekwang = godori = hongdan = chungdan = chodan = false;
            mungtta = pibak = kwangbak = gobak = false;

            order_position = byte.MaxValue;
            money_var = 0;

            //topMission = new List<MissionData>();

            win_count = 0;

        }
        public void reset()
        {
            inGameChangeMoney = 0;
            floor_pae.Clear();
            hand_pae.Clear();
            status = UserGameStatus.Play;
            score = 0;
            prev_score = 0;
            missionscore = 1;
            missionresult = 0;
            chong_tong_number = 12;
            multiple = 0;

            go_count = 0;
            shaking_count = 0;
            ppuk_count = 0;
            ppuk_chain = 0;
            remain_bomb_count = 0;
            is_used_kookjin = false;
            put_card_bonus = false;
            front_card_match = false;

            PlayerFloorCheck.Clear();
            ppuk_card_numbers.Clear();

            bisamkwang = samkwang = forekwang = fivekwang = godori = hongdan = chungdan = chodan = false;
            mungtta = pibak = kwangbak = gobak = false;

            order_position = byte.MaxValue;

            PushBonus = false;
            IsBonusCard = false;
            BonusCardChance = 0;
            Advantage = false;
            AdvantageType1 = false;
            AdvantageType2 = false;
            AdvantageType3 = false;
            Penalty = false;
            PenaltyType1 = false;
            PenaltyType2 = false;
            PenaltyType3 = false;
        }
        private bool is_me(byte player_index)
        {
            return this.player_index == player_index;
        }
        public void setMoney(long money)
        {
            this.haveMoney = money;
        }
        public long subtractMoney(long money)
        {
            if (this.haveMoney < money)
            {
                money = this.haveMoney;
                this.haveMoney = 0;
            }
            else
            {
                this.haveMoney -= money;
            }

            return money;
        }
        public void addMoney(long money)
        {
            this.haveMoney += money;
        }

        #region 게임플레이
        public void turnreset()
        {
            this.PlayerFloorCheck.Clear();
            front_card_match = false;
        }

        public List<CCard> GetPlayerHandsCards()
        {
            return hand_pae;
        }

        public List<CCard> GetPlayerFloorCards()
        {
            List<List<CCard>> FloorCards = new List<List<CCard>>();
            FloorCards = floor_pae.Values.ToList();
            List<CCard> playerFloorCards = new List<CCard>();
            for (int i = 0; i < FloorCards.Count; ++i)
            {
                for (int j = 0; j < FloorCards[i].Count; ++j)
                {
                    playerFloorCards.Add(FloorCards[i][j]);
                }
            }
            return playerFloorCards;
        }

        public void add_card_to_Playerhand(CCard card)
        {
            //UnityEngine.Debug.Log(string.Format("add to hand. player {0},   {1}, {2}, {3}",
            //	this.player_index, card.number, card.pae_type, card.position));
            this.hand_pae.Add(card);
        }

        public CCard pop_card_from_Playerhand(
            byte card_number,
            PAE_TYPE pae_type,
            byte position)
        {
            CCard card = this.hand_pae.Find(obj =>
            {
                return obj.number == card_number &&
                    obj.pae_type == pae_type &&
                    obj.position == position;
            });

            if (card == null)
            {
                return null;
            }

            this.hand_pae.Remove(card);
            return card;
        }
        public CCard pop_card_from_Playerhand_Any()
        {
            CCard card = this.hand_pae.FirstOrDefault();

            if (card == null)
            {
                return null;
            }

            this.hand_pae.Remove(card);
            return card;
        }

        public bool IsPlayerHandBonusCard()
        {
            CCard card = this.hand_pae.Find(obj =>
            {
                return obj.number == 12;
            });

            if (card == null)
            {
                return false;
            }

            return true;
        }

        public List<CCard> pop_all_cards_from_Playerhand(byte card_number)
        {
            List<CCard> cards = this.hand_pae.FindAll(obj => obj.is_same_number(card_number));
            List<CCard> result = new List<CCard>();
            for (int i = 0; i < cards.Count; ++i)
            {
                //UnityEngine.Debug.Log(string.Format("pop bomb card {0}, {1}, {2}", cards[i].number,
                //	cards[i].pae_type, cards[i].position));

                result.Add(cards[i]);
                this.hand_pae.Remove(cards[i]);
            }

            return result;
        }

        public void add_card_to_Playerfloor(CCard card)
        {
            if (!this.floor_pae.ContainsKey(card.pae_type))
            {
                this.floor_pae.Add(card.pae_type, new List<CCard>());
            }
            this.floor_pae[card.pae_type].Add(card);
        }

        public List<CCard> pop_card_from_Playerfloor(byte pee_count_to_want)
        {
            // 피가 한장도 없다면 줄 수 있는게 없다.
            if (!this.floor_pae.ContainsKey(PAE_TYPE.PEE))
            {
                return null;
            }

            List<CCard> player_pees = this.floor_pae[PAE_TYPE.PEE];
            if (player_pees.Count <= 0)
            {
                return null;
            }

            // 가져올 패 선택
            List<CCard> taken_cards = taken_cards_from_player(player_pees, pee_count_to_want);

            // 플레이어의 바닥패에서 제거.
            for (int i = 0; i < taken_cards.Count; ++i)
            {
                if (!player_pees.Remove(taken_cards[i]))
                {
                    Log._log.ErrorFormat("player_pees.Remove 비정상접근 감지 {0}\n", taken_cards[i].number);
                }
            }
            if (player_pees.Count <= 0)
            {
                this.floor_pae.Remove(PAE_TYPE.PEE);
            }

            return taken_cards;
        }

        List<CCard> taken_cards_from_player(List<CCard> player_pees, byte taken_count)
        {
            if (player_pees == null) return null;

            List<CCard> pee_card = player_pees.FindAll(obj => obj.status != CARD_STATUS.TWO_PEE && obj.status != CARD_STATUS.THREE_PEE);
            List<CCard> twopee_card = player_pees.FindAll(obj => obj.status == CARD_STATUS.TWO_PEE);
            List<CCard> threepee_card = player_pees.FindAll(obj => obj.status == CARD_STATUS.THREE_PEE);


            List<CCard> result_cards = new List<CCard>();

            // 가져올 바닥의 피 선택. 피가 부족하면 쌍피나 쓰리피를 가져옴.
            for (int count = 0; count < taken_count;)
            {
                if (taken_count - count >= 3)
                {
                    if (threepee_card.Count >= 1)
                    {
                        result_cards.Add(threepee_card[threepee_card.Count - 1]);
                        threepee_card.RemoveAt(threepee_card.Count - 1);
                        count += 3;
                    }
                    else if (twopee_card.Count >= 1)
                    {
                        result_cards.Add(twopee_card[twopee_card.Count - 1]);
                        twopee_card.RemoveAt(twopee_card.Count - 1);
                        count += 2;
                    }
                    else if (pee_card.Count >= 1)
                    {
                        result_cards.Add(pee_card[pee_card.Count - 1]);
                        pee_card.RemoveAt(pee_card.Count - 1);
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (taken_count - count >= 2)
                {
                    if (twopee_card.Count >= 1)
                    {
                        result_cards.Add(twopee_card[twopee_card.Count - 1]);
                        twopee_card.RemoveAt(twopee_card.Count - 1);
                        count += 2;
                    }
                    else if (threepee_card.Count >= 1 && pee_card.Count == 1)
                    {
                        result_cards.Add(threepee_card[threepee_card.Count - 1]);
                        threepee_card.RemoveAt(threepee_card.Count - 1);
                        count += 3;
                    }
                    else if (pee_card.Count >= 1)
                    {
                        result_cards.Add(pee_card[pee_card.Count - 1]);
                        pee_card.RemoveAt(pee_card.Count - 1);
                        count += 1;
                    }
                    else if (threepee_card.Count >= 1)
                    {
                        result_cards.Add(threepee_card[threepee_card.Count - 1]);
                        threepee_card.RemoveAt(threepee_card.Count - 1);
                        count += 3;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (taken_count - count >= 1)
                {
                    if (pee_card.Count >= 1)
                    {
                        result_cards.Add(pee_card[pee_card.Count - 1]);
                        pee_card.RemoveAt(pee_card.Count - 1);
                        count += 1;
                    }
                    else if (twopee_card.Count >= 1)
                    {
                        result_cards.Add(twopee_card[twopee_card.Count - 1]);
                        twopee_card.RemoveAt(twopee_card.Count - 1);
                        count += 2;
                    }
                    else if (threepee_card.Count >= 1)
                    {
                        result_cards.Add(threepee_card[threepee_card.Count - 1]);
                        threepee_card.RemoveAt(threepee_card.Count - 1);
                        count += 3;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return result_cards;
        }

        public bool is_same_cards(CCard card)
        {
            if (!this.floor_pae.ContainsKey(card.pae_type))
            {
                return false;
            }

            CCard samecard = this.floor_pae[card.pae_type].Find(obj => obj.is_same(card.number, card.position));

            if (samecard == null)
            {
                // 국진이면 피 타입에서도 확인
                if (card.is_kookjin_card())
                {
                    samecard = this.floor_pae[PAE_TYPE.PEE].Find(obj => obj.is_same(card.number, card.position));
                    if (samecard == null) return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        CCard pop_specific_card_from_Playerfloor(PAE_TYPE pae_type, CARD_STATUS status)
        {
            if (!this.floor_pae.ContainsKey(pae_type))
            {
                return null;
            }

            CCard card = this.floor_pae[pae_type].Find(obj => obj.status == status);
            this.floor_pae[pae_type].Remove(card);
            return card;
        }

        CCard pop_specific_card_from_Playerfloor(byte number, PAE_TYPE pae_type, byte position)
        {
            if (!this.floor_pae.ContainsKey(pae_type))
            {
                return null;
            }

            CCard card = this.floor_pae[pae_type].Find(obj => obj.is_same(number, pae_type, position));
            this.floor_pae[pae_type].Remove(card);
            return card;
        }

        List<CCard> find_cards(PAE_TYPE pae_type)
        {
            if (this.floor_pae.ContainsKey(pae_type))
            {
                return this.floor_pae[pae_type];
            }

            return null;
        }

        public byte get_same_cards_count_from_floor(List<CCard> cards)
        {
            byte same_card_count = 0;
            foreach (var card in cards)
            {
                PAE_TYPE pae_type = card.pae_type;

                if (!this.floor_pae.ContainsKey(pae_type))
                {
                    // 국진이면 피 타입에서도 확인
                    if (card.is_kookjin_card())
                    {
                        pae_type = PAE_TYPE.PEE;
                        if (!this.floor_pae.ContainsKey(pae_type))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (this.floor_pae[pae_type].Find(obj => obj.is_same(card.number, pae_type, card.position)) != null)
                {
                    ++same_card_count;
                }
                else if (card.is_kookjin_card())
                {
                    // 국진이면 피 타입에서도 확인
                    pae_type = PAE_TYPE.PEE;
                    if (this.floor_pae.ContainsKey(pae_type))
                    {
                        if (this.floor_pae[pae_type].Find(obj => obj.is_same(card.number, pae_type, card.position)) != null)
                        {
                            ++same_card_count;
                        }
                    }
                }
            }

            return same_card_count;
        }

        List<CCard> find_cards(Dictionary<PAE_TYPE, List<CCard>> pae, PAE_TYPE pae_type)
        {
            if (pae.ContainsKey(pae_type))
            {
                return pae[pae_type];
            }

            return null;
        }

        public bool find_card(byte number, PAE_TYPE pae_type, byte position)
        {
            List<CCard> cards = find_cards(pae_type);
            if (cards == null) return false;

            return cards.Exists(obj => obj.is_same(number, pae_type, position));
        }

        public byte get_kwang_count()
        {
            List<CCard> cards = find_cards(PAE_TYPE.KWANG);
            if (cards == null) return 0;

            return (byte)cards.Count;
        }
        public bool existKwang3()
        {
            if (find_card(2, PAE_TYPE.KWANG, 0))
            {
                return true;
            }
            return false;
        }
        public bool existKwang8()
        {
            if (find_card(7, PAE_TYPE.KWANG, 0))
            {
                return true;
            }
            return false;
        }

        public bool existKookjin(bool pee = false)
        {
            if (find_card(8, PAE_TYPE.YEOL, 0) || (pee && find_card(8, PAE_TYPE.PEE, 0)))
            {
                return true;
            }
            return false;
        }

        public byte get_card_count(PAE_TYPE pae_type, CARD_STATUS status)
        {
            if (!this.floor_pae.ContainsKey(pae_type))
            {
                return 0;
            }

            List<CCard> targets = this.floor_pae[pae_type].FindAll(obj => obj.is_same_status(status));
            if (targets == null)
            {
                return 0;
            }

            return (byte)targets.Count;
        }
        public int get_cards_count()
        {
            int count = 0;
            foreach (var pae in floor_pae.Values)
            {
                count += pae.Count;
            }

            return count;
        }

        public byte get_same_card_count_from_hand(byte number)
        {
            List<CCard> same_cards = find_same_cards_from_hand(number);
            if (same_cards == null)
            {
                return 0;
            }

            return (byte)same_cards.Count;
        }

        public List<CCard> find_same_cards_from_hand(byte number)
        {
            return this.hand_pae.FindAll(obj => obj.is_same_number(number));
        }

        public byte get_pee_count()
        {
            List<CCard> cards = find_cards(PAE_TYPE.PEE);
            if (cards == null)
            {
                return 0;
            }

            byte twopee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.TWO_PEE);
            byte threepee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.THREE_PEE);
            byte pee_count = (byte)(cards.Count - twopee_count - threepee_count);

            byte total_pee_count = (byte)(pee_count + (twopee_count * 2) + (threepee_count * 3));
            return total_pee_count;
        }

        short get_score_by_type(PAE_TYPE pae_type)
        {
            short pae_score = 0;

            List<CCard> cards = find_cards(pae_type);
            if (cards == null)
            {
                return 0;
            }

            switch (pae_type)
            {
                case PAE_TYPE.PEE:
                    {

                        byte twopee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.TWO_PEE);
                        byte threepee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.THREE_PEE);
                        byte pee_count = (byte)(cards.Count - twopee_count - threepee_count);

                        byte total_pee_count = (byte)(pee_count + (twopee_count * 2) + (threepee_count * 3));
                        //UnityEngine.Debug.Log(string.Format("[SCORE] Player {0}, total pee {1}, twopee {2}",
                        //	this.player_index, total_pee_count, twopee_count));
                        if (total_pee_count >= 10)
                        {
                            pae_score = (short)(total_pee_count - 9);
                        }
                    }
                    break;

                case PAE_TYPE.TEE:
                    if (cards.Count >= 5)
                    {
                        pae_score = (short)(cards.Count - 4);
                    }
                    break;

                case PAE_TYPE.YEOL:
                    if (cards.Count >= 5)
                    {
                        pae_score = (short)(cards.Count - 4);
                    }
                    break;

                case PAE_TYPE.KWANG:
                    if (cards.Count == 5)
                    {
                        pae_score = 15;
                    }
                    else if (cards.Count == 4)
                    {
                        pae_score = 4;
                    }
                    else if (cards.Count == 3)
                    {
                        // 비광이 포함되어 있으면 2점. 아니면 3점.
                        bool is_exist_beekwang = cards.Exists(obj => obj.is_same_number(CCard.BEE_KWANG));
                        if (is_exist_beekwang)
                        {
                            pae_score = 2;
                        }
                        else
                        {
                            pae_score = 3;
                        }
                    }
                    break;
            }

            //UnityEngine.Debug.Log(string.Format("{0} {1} score", pae_type, pae_score));
            return pae_score;
        }

        public void set_chongtong(short number)
        {
            this.chong_tong_number = number;
        }

        bool GetBiKwangCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.KWANG);
            if (cards == null)
            {
                return false;
            }
            if (cards.Count == 3)
            {
                bool is_exist_beekwang = cards.Exists(obj => obj.is_same_number(CCard.BEE_KWANG));
                if (is_exist_beekwang)
                {
                    if (!bisamkwang)
                    {
                        bisamkwang = true;
                        return true;
                    }
                }
            }
            return false;
        }
        bool GetSamKwangCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.KWANG);
            if (cards == null)
            {
                return false;
            }
            if (cards.Count == 3)
            {
                bool is_exist_beekwang = cards.Exists(obj => obj.is_same_number(CCard.BEE_KWANG));
                if (is_exist_beekwang)
                {
                    return false;
                }
                else
                {
                    if (!samkwang)
                    {
                        samkwang = true;
                        return true;
                    }
                }
            }
            return false;
        }
        bool GetForeKwangCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.KWANG);
            if (cards == null)
            {
                return false;
            }
            if (cards.Count == 4)
            {
                if (!forekwang)
                {
                    forekwang = true;
                    return true;
                }
            }
            return false;
        }
        bool GetFiveKwnagCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.KWANG);
            if (cards == null)
            {
                return false;
            }
            if (cards.Count == 5)
            {
                if (!fivekwang)
                {
                    fivekwang = true;
                    return true;
                }
            }
            return false;
        }

        bool GetGodoriCheck()
        {
            byte godori_count = get_card_count(PAE_TYPE.YEOL, CARD_STATUS.GODORI);
            if (godori_count == 3)
            {
                if (!godori)
                {
                    godori = true;
                    return true;
                }
            }
            return false;
        }
        bool GetMungttaCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.YEOL);
            if (cards == null)
            {
                return false;
            }
            if (cards.Count >= 7)
            {
                if (!mungtta)
                {
                    mungtta = true;
                    return true;
                }
            }
            return false;
        }
        bool MungttaCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.YEOL);
            if (cards == null)
            {
                return false;
            }
            if (cards.Count >= 7)
            {
                return true;
            }
            return false;
        }
        bool GetHongDanCheck()
        {
            byte hongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.HONG_DAN);
            if (hongdan_count == 3)
            {
                if (!hongdan)
                {
                    hongdan = true;
                    return true;
                }
            }
            return false;
        }
        bool GetChungDanCheck()
        {
            byte cheongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHEONG_DAN);
            if (cheongdan_count == 3)
            {
                if (!chungdan)
                {
                    chungdan = true;
                    return true;
                }
            }
            return false;
        }
        bool GetChoDanCheck()
        {
            byte chodan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHO_DAN);
            if (chodan_count == 3)
            {
                if (!chodan)
                {
                    chodan = true;
                    return true;
                }
            }
            return false;
        }

        public bool GetPibakCheckMoveKookjin()
        {
            // 국진 옮기고 피박 확인
            CCard card = pop_specific_card_from_Playerfloor(PAE_TYPE.YEOL, CARD_STATUS.KOOKJIN);

            if (card == null) return false;

            card.change_pae_type(PAE_TYPE.PEE);
            card.set_card_status(CARD_STATUS.TWO_PEE);
            add_card_to_Playerfloor(card);

            GetPibakCheck();

            // 그래도 피박이면 되돌림
            if (this.pibak == true)
            {
                CCard card_bak = pop_specific_card_from_Playerfloor(8, PAE_TYPE.PEE, 0);
                card_bak.change_pae_type(PAE_TYPE.YEOL);
                card_bak.set_card_status(CARD_STATUS.KOOKJIN);
                add_card_to_Playerfloor(card_bak);
                return false;
            }

            return true;
        }

        public bool GetPibakCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.PEE);
            if (cards == null)
            {
                return this.pibak = true;
            }
            byte twopee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.TWO_PEE);
            byte threepee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.THREE_PEE);
            byte pee_count = (byte)(cards.Count - twopee_count - threepee_count);

            byte total_pee_count = (byte)(pee_count + (twopee_count * 2) + (threepee_count * 3));
            if (total_pee_count <= 7)
            {
                this.pibak = true;
            }
            else
            {
                this.pibak = false;
            }

            return this.pibak;
        }
        public void SetPiBak(bool set)
        {
            this.pibak = set;
        }

        public bool GetKwangBakCheck()
        {
            List<CCard> cards = find_cards(PAE_TYPE.KWANG);
            if (cards == null)
            {
                return this.kwangbak = true;
            }

            if (cards.Count == 0)
            {
                this.kwangbak = true;
            }
            else
            {
                this.kwangbak = false;
            }

            return this.kwangbak;
        }
        public void SetKwangBak(bool set)
        {
            this.kwangbak = set;
        }

        public bool GetGoBakCheck()
        {
            if (this.go_count > 0)
            {
                this.gobak = true;
            }
            else
            {
                this.gobak = false;
            }

            return this.gobak;
        }

        public void CheckPlayerFloor()
        {
            if (GetBiKwangCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.BI_SAM_KWANG);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.BI_THREE_KWANG);
            }
            if (GetSamKwangCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.SAM_KWANG);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.THREE_KWANG);
            }
            if (GetForeKwangCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.SA_KWANG);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.FORE_KWANG);
            }
            if (GetFiveKwnagCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.O_KWANG);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.FIVE_KWANG);
            }

            if (GetGodoriCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.GODORI);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.GODORI);
            }
            if (GetMungttaCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.MUNGTTA);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.MUNGTTA);
            }
            if (GetHongDanCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.HONGDAN);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.HONGDAN);
            }
            if (GetChungDanCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.CHEONGDAN);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.CHUNGDAN);
            }
            if (GetChoDanCheck())
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.CHODAN);
                PlayerFloorCheck.Add(PLAYER_FLOOR_CHECK.CHODAN);
            }
        }

        public void calculate_score()
        {
            this.score = 0;

            // 10피 이상 피당 1점
            this.score += get_score_by_type(PAE_TYPE.PEE);
            // 5띠 이상 장당 1점
            this.score += get_score_by_type(PAE_TYPE.TEE);
            // 5열 이상 장당 1점
            this.score += get_score_by_type(PAE_TYPE.YEOL);
            // 광 점수
            this.score += get_score_by_type(PAE_TYPE.KWANG);

            // 고도리 5점
            byte godori_count = get_card_count(PAE_TYPE.YEOL, CARD_STATUS.GODORI);
            if (godori_count == 3)
                this.score += 5;

            // 홍단, 초단, 청단 각 3점
            byte hongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.HONG_DAN);
            if (hongdan_count == 3)
                this.score += 3;
            byte chodan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHO_DAN);
            if (chodan_count == 3)
                this.score += 3;
            byte cheongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHEONG_DAN);
            if (cheongdan_count == 3)
                this.score += 3;

            // 1고당 추가점수 1점
            this.score += this.go_count;
        }

        public void get_score_detail(out short score1, out short score2, out short score3, out short score4, out short score5)
        {
            // 광 열 띠 피 고
            score1 = score2 = score3 = score4 = score5 = 0;

            // 10피 이상 피당 1점
            score4 += get_score_by_type(PAE_TYPE.PEE);
            // 5띠 이상 장당 1점
            score3 += get_score_by_type(PAE_TYPE.TEE);
            // 5열 이상 장당 1점
            score2 += get_score_by_type(PAE_TYPE.YEOL);
            // 광 점수
            score1 += get_score_by_type(PAE_TYPE.KWANG);

            // 고도리 5점
            byte godori_count = get_card_count(PAE_TYPE.YEOL, CARD_STATUS.GODORI);
            if (godori_count == 3)
                score2 += 5;

            // 홍단, 초단, 청단 각 3점
            byte hongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.HONG_DAN);
            if (hongdan_count == 3)
                score3 += 3;
            byte chodan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHO_DAN);
            if (chodan_count == 3)
                score3 += 3;
            byte cheongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHEONG_DAN);
            if (cheongdan_count == 3)
                score3 += 3;

            // 1고당 추가점수 1점
            score5 += this.go_count;
        }

        // 플레이어의 패를 번호 순서에 따라 오름차순 정렬 한다.
        public void sort_player_hand_slots()
        {
            this.hand_pae.Sort((CCard lhs, CCard rhs) =>
            {
                if (lhs.number < rhs.number)
                {
                    return -1;
                }
                else if (lhs.number > rhs.number)
                {
                    return 1;
                }

                return 0;
            });

            string debug = string.Format("player [{0}] ", this.player_index);
            for (int i = 0; i < this.hand_pae.Count; ++i)
            {
                debug += string.Format("{0}, ",
                    this.hand_pae[i].number);
            }
            //UnityEngine.Debug.Log(debug);
        }

        public void add_bomb_count(byte count)
        {
            this.remain_bomb_count += count;
        }

        public bool decrease_bomb_count()
        {
            if (this.remain_bomb_count > 0)
            {
                --this.remain_bomb_count;
                return true;
            }

            return false;
        }

        public bool can_finish()
        {
            calculate_score();

            // 7점 이상
            if (this.score < 7)
            {
                return false;
            }

            // 이전 점수보다 높아야 됨
            if (this.prev_score >= this.score)
            {
                return false;
            }

            this.prev_score = this.score;
            return true;
        }

        public void update_prev_score()
        {
            this.prev_score = this.score;
        }

        public void plus_go_count()
        {
            ++this.go_count;
            if (this.go_count == 5)
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.FIVE_GO);
            }
        }

        public void plus_shaking_count()
        {
            ++this.shaking_count;
            if (this.shaking_count == 3)
            {
                trigger_mission_check(MISSION_TAPSSAHGI_TYPE.THREE_SHAKE);
            }
        }

        public void plus_ppuk_count()
        {
            ++this.ppuk_count;
        }

        public void kookjin_selected()
        {
            this.is_used_kookjin = true;
        }

        public void move_kookjin_to_pee()
        {
            CCard card = pop_specific_card_from_Playerfloor(PAE_TYPE.YEOL, CARD_STATUS.KOOKJIN);
            if (card == null)
            {
                return;
            }

            card.change_pae_type(PAE_TYPE.PEE);
            card.set_card_status(CARD_STATUS.TWO_PEE);

            // 멍따 확인
            mungtta = MungttaCheck();

            add_card_to_Playerfloor(card);
        }

        public bool can_move_kookjin()
        {
            bool result = false;
            CCard card = pop_specific_card_from_Playerfloor(PAE_TYPE.YEOL, CARD_STATUS.KOOKJIN);

            if (card == null) return false;

            card.change_pae_type(PAE_TYPE.PEE);
            card.set_card_status(CARD_STATUS.TWO_PEE);
            add_card_to_Playerfloor(card);
            calculate_score();

            if (score >= 7)
            {
                result = true;
            }

            CCard card_bak = pop_specific_card_from_Playerfloor(8, PAE_TYPE.PEE, 0);
            card_bak.change_pae_type(PAE_TYPE.YEOL);
            card_bak.set_card_status(CARD_STATUS.KOOKJIN);
            add_card_to_Playerfloor(card_bak);
            calculate_score();

            return result;
        }

        public bool is_empty_on_hand()
        {
            return this.hand_pae.Count + remain_bomb_count <= 0;
        }

        public bool is_first_hand()
        {
            return this.hand_pae.Count + remain_bomb_count == 9;
        }
        public bool is_second_hand()
        {
            return this.hand_pae.Count + remain_bomb_count == 8;
        }
        public bool is_third_hand()
        {
            return this.hand_pae.Count + remain_bomb_count == 7;
        }

        public void trigger_chain_ppuk()
        {
            ++this.ppuk_chain;
        }
        public void trigger_chain_ppuk_reset()
        {
            this.ppuk_chain = 0;
        }
        public void trigger_mission_check(MISSION_TAPSSAHGI_TYPE mission_type)
        {
            // 무료머니 채널만 탑쌓기 이벤트 진행 가능
            //if (joinRoom.RoomSvr.ChannelType != ChannelType.Free) return;

            //for (int i = 0; i < this.topMission.Count; ++i)
            //{
            //    if (topMission[i].isComplete == false)
            //    {
            //        if(topMission[i].type == (byte)mission_type)
            //        {
            //            SetMission(i, true);
            //            mission_update = true;
            //        }
            //        break;
            //    }
            //}
        }

        #endregion 게임플레이

        #region 자동치기
        public delegate void PacketFn(CMessage rm);
        public ConcurrentDictionary<PacketType, PacketFn> packet_handler;
        private CBrain brain;

        public void setDummyPlayer()
        {
            this.QueueMsg = new ConcurrentQueue<MessageTemp>();

            this.brain = new CBrain();
            // ☆ 여기서 AI 처리를 추가하면 CPlayer -> IsPlayablePacket에도 추가해줘야 처리됨.
            this.packet_handler = new ConcurrentDictionary<PacketType, PacketFn>();
            //this.packet_handler.Add((PacketType)Rmi.Common.SC_ROOM_GAME_STARTED, SC_ROOM_GAME_STARTED);         // 게임시작
            this.packet_handler.TryAdd(SS.Common.GameStart, SC_READY_TO_START);               // 선잡기 선택
            this.packet_handler.TryAdd(SS.Common.GameDistributedStart, SC_DISTRIBUTE_ALL_CARDS);   // 카드 나눠주기
            this.packet_handler.TryAdd(SS.Common.GameTurnStart, SC_START_TURN);                       // 턴 시작
            this.packet_handler.TryAdd(SS.Common.GameSelectCardResult, SC_SELECT_CARD_ACK);             // 내 차례에 대한 응답
            this.packet_handler.TryAdd(SS.Common.GameFlipDeckResult, SC_FLIP_DECK_CARD_ACK);       // 더미에서 뒤집기 응답
            this.packet_handler.TryAdd(SS.Common.GameRequestGoStop, SC_ASK_GO_OR_STOP);               // 고, 스톱 선택
            this.packet_handler.TryAdd(SS.Common.GameRequestKookjin, SC_ASK_KOOKJIN_TO_PEE);       // 국진 선택
            this.packet_handler.TryAdd(SS.Common.GameOver, SC_GAME_RESULT);                     // 게임 결과

        }

        MessageTemp SendMsgMake(CMessage msg, PacketType rmiID)
        {
            return new MessageTemp(msg, rmiID);
        }

        void Send(CMessage msg, PacketType protocol)
        {
            switch (protocol)
            {
                case SS.Common.GameSelectOrder:
                case SS.Common.GameDistributedEnd:
                case SS.Common.GameSelectKookjin:
                case SS.Common.GameSelectGoStop:
                case SS.Common.GameActionPutCard:
                case SS.Common.GameActionFlipBomb:
                case SS.Common.GameActionChooseCard:
                    {
                        QueueMsg.Enqueue(SendMsgMake(msg, protocol));
                    }
                    break;
                default:
                    {
                        Log._log.Error("Send Error protocol:" + protocol);
                    }
                    break;
            }
        }
        #region AI Handler
        Random rand = new Random((int)DateTime.UtcNow.Ticks);
        void SC_READY_TO_START(CMessage msg)
        {
            byte nRan = (byte)rand.Next(0, 8);
            CMessage newmsg = new CMessage();
            Rmi.Marshaler.Write(newmsg, nRan);
            Send(newmsg, SS.Common.GameSelectOrder);
        }
        // 카드 분배 받기
        void SC_DISTRIBUTE_ALL_CARDS(CMessage msg)
        {
            CMessage newmsg = new CMessage();
            Send(newmsg, SS.Common.GameDistributedEnd);
        }
        // 내 턴 처리
        void SC_START_TURN(CMessage msg)
        {
            byte currentTurn;
            Rmi.Marshaler.Read(msg, out currentTurn);
            if (!is_me(currentTurn))
                return;
            byte remain_bomb_card_count;
            Rmi.Marshaler.Read(msg, out remain_bomb_card_count);

            List<CCard> my_hand = GetPlayerHandsCards();
            List<CCard> my_floor = GetPlayerFloorCards();
            List<CCard> other_floor = this.joinRoom.engine.this_player_agent_next(this.player_index).GetPlayerFloorCards();

            if (my_hand == null || (my_hand.Count == 0 && remain_bomb_card_count == 0))
            {
                Log._log.Error("my_hand error");
            }

            // 손패 규칙에 따라 패를 낸다.
            int slot_index = this.brain.PutOutHandCardAI(my_hand, my_floor, other_floor, this.joinRoom.engine.floor_manager, remain_bomb_card_count);

            if (slot_index <= -1)
            {
                Log._log.Error("slot_index :" + slot_index);
                slot_index = 0;
            }

            if (my_hand != null && (my_hand.Count <= slot_index && slot_index != 13))
            {
                Log._log.Error("my_hand.Count :" + my_hand.Count + " slot_index :" + slot_index);
                return;
            }

            // 폭탄패 
            if (slot_index == 13)
            {
                CMessage newmsg = new CMessage();
                Send(newmsg, SS.Common.GameActionFlipBomb);
            }
            else
            {
                // 항상 흔들기
                byte is_shaking = 1;

                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, (byte)my_hand[slot_index].number);
                Rmi.Marshaler.Write(newmsg, (byte)my_hand[slot_index].pae_type);
                Rmi.Marshaler.Write(newmsg, (byte)my_hand[slot_index].position);
                Rmi.Marshaler.Write(newmsg, (byte)slot_index);
                Rmi.Marshaler.Write(newmsg, (byte)is_shaking);
                Send(newmsg, SS.Common.GameActionPutCard);
            }
        }
        // 내 턴 응답
        void SC_SELECT_CARD_ACK(CMessage msg)
        {
            byte delay;
            Rmi.Marshaler.Read(msg, out delay);
            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index);

            //System.Threading.Thread.Sleep(delay);

            // 플레이어가 낸 카드 정보.
            byte player_card_number;
            Rmi.Marshaler.Read(msg, out player_card_number);
            byte paetype;
            Rmi.Marshaler.Read(msg, out paetype);
            PAE_TYPE player_card_pae_type = PAE_TYPE.PEE;
            player_card_pae_type = (PAE_TYPE)paetype;
            byte player_card_position;
            Rmi.Marshaler.Read(msg, out player_card_position);
            byte same_count_with_player;
            Rmi.Marshaler.Read(msg, out same_count_with_player);
            byte slot_index;
            Rmi.Marshaler.Read(msg, out slot_index);
            byte cardevent;
            Rmi.Marshaler.Read(msg, out cardevent);
            CARD_EVENT_TYPE card_event = CARD_EVENT_TYPE.NONE;
            card_event = (CARD_EVENT_TYPE)cardevent;

            switch (card_event)
            {
                case CARD_EVENT_TYPE.BOMB:
                    {

                        byte bomb_card_count;
                        Rmi.Marshaler.Read(msg, out bomb_card_count);
                        for (byte i = 0; i < bomb_card_count; ++i)
                        {
                            byte number;
                            Rmi.Marshaler.Read(msg, out number);

                            byte cardtype;
                            Rmi.Marshaler.Read(msg, out cardtype);
                            PAE_TYPE pae_type = (PAE_TYPE)cardtype;
                            byte position;
                            Rmi.Marshaler.Read(msg, out position);
                            //CCard card = this.my_hand_cards.Find(obj => obj.is_same(number, pae_type, position));
                            if (is_me(player_index))
                            {
                                //this.my_hand_cards.Remove(card);
                                // 폭탄 수 ++;
                            }
                        }
                    }
                    break;

                case CARD_EVENT_TYPE.SHAKING:
                    {
                        byte shaking_card_count;
                        Rmi.Marshaler.Read(msg, out shaking_card_count);
                        for (byte i = 0; i < shaking_card_count; ++i)
                        {
                            byte number;
                            Rmi.Marshaler.Read(msg, out number);
                            byte paetype1;
                            Rmi.Marshaler.Read(msg, out paetype1);
                            PAE_TYPE pae_type = (PAE_TYPE)paetype1;
                            byte position;
                            Rmi.Marshaler.Read(msg, out position);
                        }
                        if (is_me(player_index))
                        {
                            //this.my_hand_cards.RemoveAt(slot_index);
                        }

                    }
                    break;

                    //  default:
                    //       this.player_hand_cards.RemoveAt(slot_index);
                    //       break;
            }
            byte result;
            Rmi.Marshaler.Read(msg, out result);
            PLAYER_SELECT_CARD_RESULT select_result = (PLAYER_SELECT_CARD_RESULT)result;
            //플레이어가 한장의 카드를 선택했을때
            if (select_result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)
            {
                if (is_me(player_index))
                {
                    byte count;
                    Rmi.Marshaler.Read(msg, out count);
                    byte choiceSlotIndex = 0;
                    for (byte i = 0; i < count; ++i)
                    {
                        byte number;
                        Rmi.Marshaler.Read(msg, out number);
                        byte paetype1;
                        Rmi.Marshaler.Read(msg, out paetype1);
                        PAE_TYPE pae_type = (PAE_TYPE)paetype1;
                        byte position;
                        Rmi.Marshaler.Read(msg, out position);

                        if (this.brain.IsChoiceCard(number, position))
                        {
                            choiceSlotIndex = i;
                        }
                    }
                    this.brain.NoChoiceCard();

                    CMessage newmsg = new CMessage();
                    Rmi.Marshaler.Write(newmsg, (byte)choiceSlotIndex);
                    Send(newmsg, SS.Common.GameActionChooseCard);
                    return;
                }
            }

            //선택 카드가 보너스일때
            if (select_result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                List<CCard> bonusCards = new List<CCard>();
                CCard card = new CCard(player_card_number, player_card_pae_type, player_card_position);

                byte number;
                Rmi.Marshaler.Read(msg, out number);
                byte paetype1;
                Rmi.Marshaler.Read(msg, out paetype1);
                PAE_TYPE pae_type = (PAE_TYPE)paetype1;
                byte position;
                Rmi.Marshaler.Read(msg, out position);
                CCard deckcard = new CCard(number, pae_type, position);

                if (is_me(player_index))
                {
                    //this.my_hand_cards.Add(deckcard);
                }
                return;

            }

            if (is_me(player_index))
            {
            }
        }
        // 더미에서 카드 뽑기 요청
        void SC_FLIP_DECK_CARD_ACK(CMessage msg)
        {
            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index);
            // 덱에서 뒤집은 카드 정보.
            byte deck_card_number;
            Rmi.Marshaler.Read(msg, out deck_card_number);
            byte paetype;
            Rmi.Marshaler.Read(msg, out paetype);
            PAE_TYPE deck_card_pae_type = (PAE_TYPE)paetype;
            byte deck_card_position;
            Rmi.Marshaler.Read(msg, out deck_card_position);
            byte same_count_with_deck;
            Rmi.Marshaler.Read(msg, out same_count_with_deck);

            byte result1;
            Rmi.Marshaler.Read(msg, out result1);
            bool choose;
            Rmi.Marshaler.Read(msg, out choose);
            byte flip_type_;
            Rmi.Marshaler.Read(msg, out flip_type_);
            FLIP_TYPE flip_type = (FLIP_TYPE)flip_type_;

            PLAYER_SELECT_CARD_RESULT result = (PLAYER_SELECT_CARD_RESULT)result1;
            if (result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
            {
                // floor_cards_to_player
                byte count_to_give;
                Rmi.Marshaler.Read(msg, out count_to_give);

                for (int i = 0; i < count_to_give; ++i)
                {
                    byte card_number;
                    Rmi.Marshaler.Read(msg, out card_number);
                    //byte paetype;
                    Rmi.Marshaler.Read(msg, out paetype);
                    byte position;
                    Rmi.Marshaler.Read(msg, out position);
                }

                // other_cards_to_player
                byte victim_count;
                Rmi.Marshaler.Read(msg, out victim_count);
                for (byte victim = 0; victim < victim_count; ++victim)
                {
                    byte victim_index;
                    Rmi.Marshaler.Read(msg, out victim_index);
                    byte count_to_take;
                    Rmi.Marshaler.Read(msg, out count_to_take);
                    for (byte i = 0; i < count_to_take; ++i)
                    {
                        byte card_number;
                        Rmi.Marshaler.Read(msg, out card_number);
                        //byte paetype;
                        Rmi.Marshaler.Read(msg, out paetype);
                        byte position;
                        Rmi.Marshaler.Read(msg, out position);
                    }
                }

                this.brain.choiceCardList.Clear();

                byte count;
                Rmi.Marshaler.Read(msg, out count);
                if (count > 2) Log._log.Warn("count1 :" + count);
                for (byte i = 0; i < count; ++i)
                {
                    byte number;
                    Rmi.Marshaler.Read(msg, out number);
                    byte paetype1;
                    Rmi.Marshaler.Read(msg, out paetype1);
                    PAE_TYPE pae_type = (PAE_TYPE)paetype1;
                    byte position;
                    Rmi.Marshaler.Read(msg, out position);

                    CCard card = new CCard(number, pae_type, position);

                    this.brain.choiceCardList.Add(card);
                }

                if (is_me(player_index))
                {
                    // 뒤집어서 나온 붙은패 선택
                    byte ChooseSlotIndex;
                    List<CCard> other_floor = this.joinRoom.engine.this_player_agent_next(this.player_index).GetPlayerFloorCards();

                    int resultChoice = this.brain.ChoiceFloorCard(other_floor);
                    if (resultChoice != -1)
                    {
                        ChooseSlotIndex = (byte)resultChoice;
                    }
                    else
                    {
                        ChooseSlotIndex = 0;
                    }

                    CMessage newmsg = new CMessage();
                    Rmi.Marshaler.Write(newmsg, ChooseSlotIndex);
                    Send(newmsg, SS.Common.GameActionChooseCard);
                }
            }
            else
            {
                // nothing
            }
        }
        // 고/스톱 처리
        void SC_ASK_GO_OR_STOP(CMessage msg)
        {
            byte playerIndex;
            Rmi.Marshaler.Read(msg, out playerIndex);

            byte gocout;
            Rmi.Marshaler.Read(msg, out gocout);
            gocout = (byte)(gocout + 1);

            long stopMoney;
            Rmi.Marshaler.Read(msg, out stopMoney);

            if (is_me(playerIndex))
            {
                byte is_go = 0;// 0:스톱, 1:고.
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, is_go);
                Send(newmsg, SS.Common.GameSelectGoStop);
            }
        }
        // 국진 이동 처리
        void SC_ASK_KOOKJIN_TO_PEE(CMessage msg)
        {
            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index);

            if (is_me(player_index))
            {
                byte use_kookjin = 1; // 0:사용하지 않음, 1:쌍피로 사용.
                CMessage newmsg = new CMessage();
                Rmi.Marshaler.Write(newmsg, use_kookjin);
                Send(newmsg, SS.Common.GameSelectKookjin);
            }

        }
        // 게임 결과
        void SC_GAME_RESULT(CMessage msg)
        {
        }
        #endregion AI Handler
        #endregion 자동치기


        public string MakePlayerFloorLog()
        {
            string FloorLog = "";

            // [고 횟수]_[흔들기 횟수]_[뻑 횟수]_[광]_[열]_[띠]_[피]
            FloorLog += go_count.ToString() + "_" + shaking_count.ToString() + "_" + ppuk_count.ToString() + "_";

            // 패(월) : a=1월, b=2월, c=3월 . 패(그림) : a1:1월광, c1:3월광, m:보너스패(m1:쌍피, m2:쓰리피)
            List<CCard> Cards;
            if (floor_pae.TryGetValue(PAE_TYPE.KWANG, out Cards))
                for (int i = 0; i < Cards.Count; ++i)
                    FloorLog += GetNumberToString(Cards[i].number) + (Cards[i].position + 1).ToString();
            FloorLog += "_";

            if (floor_pae.TryGetValue(PAE_TYPE.YEOL, out Cards))
                for (int i = 0; i < Cards.Count; ++i)
                    FloorLog += GetNumberToString(Cards[i].number) + (Cards[i].position + 1).ToString();
            FloorLog += "_";

            if (floor_pae.TryGetValue(PAE_TYPE.TEE, out Cards))
                for (int i = 0; i < Cards.Count; ++i)
                    FloorLog += GetNumberToString(Cards[i].number) + (Cards[i].position + 1).ToString();
            FloorLog += "_";

            if (floor_pae.TryGetValue(PAE_TYPE.PEE, out Cards))
                for (int i = 0; i < Cards.Count; ++i)
                    FloorLog += GetNumberToString(Cards[i].number) + (Cards[i].position + 1).ToString();

            return FloorLog;
        }

        string GetNumberToString(byte number)
        {
            switch (number)
            {
                case 0:
                    return "a";
                case 1:
                    return "b";
                case 2:
                    return "c";
                case 3:
                    return "d";
                case 4:
                    return "e";
                case 5:
                    return "f";
                case 6:
                    return "g";
                case 7:
                    return "h";
                case 8:
                    return "i";
                case 9:
                    return "j";
                case 10:
                    return "k";
                case 11:
                    return "l";
                case 12:
                    return "m";
            }
            return "?";
        }
    }
}
