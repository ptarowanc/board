using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engine
{
    public class CCardManager
    {
        public List<CCard> cards { get; private set; }

        public CCardManager()
        {
            this.cards = new List<CCard>();
        }

        public void make_all_cards()
        {
            // Generate cards.
            Queue<PAE_TYPE> total_pae_type = new Queue<PAE_TYPE>();
            // 1
            total_pae_type.Enqueue(PAE_TYPE.KWANG);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 2
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 3
            total_pae_type.Enqueue(PAE_TYPE.KWANG);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 4
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 5
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 6
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 7
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 8
            total_pae_type.Enqueue(PAE_TYPE.KWANG);
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 9
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 10
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 11
            total_pae_type.Enqueue(PAE_TYPE.KWANG);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            // 12
            total_pae_type.Enqueue(PAE_TYPE.KWANG);
            total_pae_type.Enqueue(PAE_TYPE.YEOL);
            total_pae_type.Enqueue(PAE_TYPE.TEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            this.cards.Clear();
            for (byte number = 0; number < 12; ++number)
            {
                for (byte pos = 0; pos < 4; ++pos)
                {
                    this.cards.Add(new CCard(number, total_pae_type.Dequeue(), pos));
                }
            }

            total_pae_type.Enqueue(PAE_TYPE.PEE);
            total_pae_type.Enqueue(PAE_TYPE.PEE);

            this.cards.Add(new CCard(12, total_pae_type.Dequeue(), 0));
            this.cards.Add(new CCard(12, total_pae_type.Dequeue(), 1));
        }

        public void SetOderCard(List<CCard> clone_cards)
        {
            // 선잡기용 랜덤 패
            for (int i = 0; i < this.cards.Count; ++i)
            {
                if (this.cards[i].is_bonus_card() == true) continue;

                clone_cards.Add(this.cards[i]);
            }
        }

        void set_allcard_status()
        {
            // 카드 속성 설정.
            apply_card_status(1, PAE_TYPE.YEOL, 0, CARD_STATUS.GODORI);
            apply_card_status(3, PAE_TYPE.YEOL, 0, CARD_STATUS.GODORI);
            apply_card_status(7, PAE_TYPE.YEOL, 1, CARD_STATUS.GODORI);

            apply_card_status(5, PAE_TYPE.TEE, 1, CARD_STATUS.CHEONG_DAN);
            apply_card_status(8, PAE_TYPE.TEE, 1, CARD_STATUS.CHEONG_DAN);
            apply_card_status(9, PAE_TYPE.TEE, 1, CARD_STATUS.CHEONG_DAN);

            apply_card_status(0, PAE_TYPE.TEE, 1, CARD_STATUS.HONG_DAN);
            apply_card_status(1, PAE_TYPE.TEE, 1, CARD_STATUS.HONG_DAN);
            apply_card_status(2, PAE_TYPE.TEE, 1, CARD_STATUS.HONG_DAN);

            apply_card_status(3, PAE_TYPE.TEE, 1, CARD_STATUS.CHO_DAN);
            apply_card_status(4, PAE_TYPE.TEE, 1, CARD_STATUS.CHO_DAN);
            apply_card_status(6, PAE_TYPE.TEE, 1, CARD_STATUS.CHO_DAN);

            apply_card_status(10, PAE_TYPE.PEE, 1, CARD_STATUS.TWO_PEE);
            apply_card_status(11, PAE_TYPE.PEE, 3, CARD_STATUS.TWO_PEE);

            apply_card_status(8, PAE_TYPE.YEOL, 0, CARD_STATUS.KOOKJIN);

            apply_card_status(12, PAE_TYPE.PEE, 0, CARD_STATUS.TWO_PEE);
            apply_card_status(12, PAE_TYPE.PEE, 1, CARD_STATUS.THREE_PEE);
        }

        void apply_card_status(byte number, PAE_TYPE pae_type, byte position, CARD_STATUS status)
        {
            CCard card = find_card(number, position);
            if (card == null)
            {
                Log._log.ErrorFormat("apply_card_status 비정상접근 감지 {0},{1}\n", number, position);
                return;
            }
            card.set_card_status(status);
            card.change_pae_type(pae_type);
        }

        public string ShuffleCard()
        {
            string CardLog = "";

            Shuffle<CCard>(this.cards);

            // 보너스패가 더미 맨 밑에 있으면 바닥패로 이동 (버그 회피)
            if (this.cards[this.cards.Count - 1].is_bonus_card())
            {
                if (this.cards[0].is_bonus_card())
                {
                    var tmp = this.cards[this.cards.Count - 1];
                    this.cards[this.cards.Count - 1] = this.cards[1];
                    this.cards[1] = tmp;
                }
                else
                {
                    var tmp = this.cards[this.cards.Count - 1];
                    this.cards[this.cards.Count - 1] = this.cards[0];
                    this.cards[0] = tmp;
                }
            }

#if DEBUG
            //simulationtest똥쌍피선택테스트();
            //피뺏기미션테스트();
            오광();
#endif
            //CardLog = "Error Game\r\n";
            //foreach (var card in cards)
            //{
            //    var t = card.pae_type;
            //    switch (t)
            //    {
            //        case PAE_TYPE.PEE:
            //            {
            //                CardLog += string.Format("this.cards.Add(new CCard({0}, PAE_TYPE.PEE, {1}));\r\n", card.number, card.position);

            //            }
            //            break;
            //        case PAE_TYPE.KWANG:
            //            {
            //                CardLog += string.Format("this.cards.Add(new CCard({0}, PAE_TYPE.KWANG, {1}));\r\n", card.number, card.position);

            //            }
            //            break;
            //        case PAE_TYPE.TEE:
            //            {
            //                CardLog += string.Format("this.cards.Add(new CCard({0}, PAE_TYPE.TEE, {1}));\r\n", card.number, card.position);
            //            }
            //            break;
            //        case PAE_TYPE.YEOL:
            //            {
            //                CardLog += string.Format("this.cards.Add(new CCard({0}, PAE_TYPE.YEOL, {1}));\r\n", card.number, card.position);
            //            }
            //            break;
            //    }
            //}

#if DEBUG
            Log._log.InfoFormat("New Game!");
            foreach (var card in cards)
            {
                var t = card.pae_type;
                switch (t)
                {
                    case PAE_TYPE.PEE:
                        {
                            Log._log.InfoFormat("this.cards.Add(new CCard({0}, PAE_TYPE.PEE, {1}));", card.number, card.position);
                        }
                        break;
                    case PAE_TYPE.KWANG:
                        {
                            Log._log.InfoFormat("this.cards.Add(new CCard({0}, PAE_TYPE.KWANG, {1}));", card.number, card.position);
                        }
                        break;
                    case PAE_TYPE.TEE:
                        {
                            Log._log.InfoFormat("this.cards.Add(new CCard({0}, PAE_TYPE.TEE, {1}));", card.number, card.position);
                        }
                        break;
                    case PAE_TYPE.YEOL:
                        {
                            Log._log.InfoFormat("this.cards.Add(new CCard({0}, PAE_TYPE.YEOL, {1}));", card.number, card.position);
                        }
                        break;
                }
            }
#endif

            set_allcard_status();

            //string log = "";
            //for (int i = 0; i < this.cards.Count; ++i)
            //{
            //	log += string.Format("this.cards.Add(new CCard({0}, PAE_TYPE.{1}, {2}));\n",
            //		this.cards[i].number,
            //		this.cards[i].pae_type,
            //		this.cards[i].position);
            //}
            //UnityEngine.Debug.Log(log);
            return CardLog;
        }

        Random rng = new Random((int)DateTime.UtcNow.Ticks);
        public void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // 보너스패1 가져가기
        public bool pushBonus1(bool FirstPlayer)
        {
            int SwapIndex1 = 0;
            var rand = rng.NextDouble();
            if (FirstPlayer)
            {
                if(rand <= 0.2) // 바닥에
                {
                    SwapIndex1 = 0;
                    //Log._log.Info("◈선바닥");
                }
                else if (rand <= 0.4) // 손에
                {
                    SwapIndex1 = 4;
                    //Log._log.Info("◈선손");
                }
                else
                {
                    //Log._log.Info("◈선덱");
                    return true;
                }
            }
            else
            {
                if (rand <= 0.3)
                {
                    SwapIndex1 = 9; // 손에
                    //Log._log.Info("◈후손");
                }
                else // 덱에
                {
                    //Log._log.Info("◈후덱");
                    return true;
                }
            }

            for (int i = 0; i < cards.Count; ++i)
            {
                if (this.cards[i].is_bonus_card())
                {
                    var tmp = this.cards[i];
                    this.cards[i] = this.cards[SwapIndex1];
                    this.cards[SwapIndex1] = tmp;
                    break;
                }
            }

            return false;
        }

        // 보너스패2 가져가기
        public void pushBonus2(bool FirstPlayer)
        {
            int SwapIndex2;
            int SwapIndex1;

            if (FirstPlayer)
            {
                SwapIndex1 = 4;
                SwapIndex2 = 5;
            }
            else
            {
                SwapIndex1 = 9;
                SwapIndex2 = 10;
            }

            for (int i = 0; i < cards.Count; ++i)
            {
                if (this.cards[i].is_bonus_card() && SwapIndex1 != i)
                {
                    var tmp = this.cards[i];
                    this.cards[i] = this.cards[SwapIndex2];
                    this.cards[SwapIndex2] = tmp;
                    break;
                }
            }
        }

        public CCard find_card(byte number, PAE_TYPE pae_type, byte position)
        {
            return this.cards.Find(obj => obj.is_same(number, pae_type, position));
        }
        public CCard find_card(byte number, byte position)
        {
            return this.cards.Find(obj => obj.is_same(number, position));
        }

        public void fill_to(Queue<CCard> target, Queue<CCard> target2, bool pushBonus)
        {
            if(pushBonus)
            {
                var bonuCard = cards.Find(x => x.is_bonus_card());
                target2.Enqueue(bonuCard);
                cards.Remove(bonuCard);
                this.cards.ForEach(obj => target.Enqueue(obj));
                cards.Add(bonuCard);
            }
            else
            {
                this.cards.ForEach(obj => target.Enqueue(obj));
            }
        }

        void 피뺏기미션테스트()
        {
            this.cards.Clear();
            // 바닥
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 1));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(0, PAE_TYPE.TEE, 1));
            // 1P
            this.cards.Add(new CCard(0, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(0, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(1, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(12, PAE_TYPE.PEE, 0));
            this.cards.Add(new CCard(12, PAE_TYPE.PEE, 1));
            // 2P
            this.cards.Add(new CCard(10, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(11, PAE_TYPE.YEOL, 1));
            this.cards.Add(new CCard(11, PAE_TYPE.TEE, 2));
            this.cards.Add(new CCard(11, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(9, PAE_TYPE.TEE, 1));
            // 바닥
            this.cards.Add(new CCard(1, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(1, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(1, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(0, PAE_TYPE.PEE, 2));
            // 1P
            this.cards.Add(new CCard(2, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(2, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(2, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(7, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(7, PAE_TYPE.PEE, 3));
            // 2P
            this.cards.Add(new CCard(8, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(8, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(8, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(9, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(9, PAE_TYPE.PEE, 3));
            // 더미
            this.cards.Add(new CCard(3, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(3, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(2, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(3, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(3, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(11, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(4, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(4, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(4, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(4, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(5, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(5, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(5, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(5, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(6, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(6, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(6, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(6, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(7, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(7, PAE_TYPE.YEOL, 1));
            this.cards.Add(new CCard(8, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(9, PAE_TYPE.YEOL, 0));
        }

        void 오광()
        {
            this.cards.Clear();
            // 바닥
            this.cards.Add(new CCard(0, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(2, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(7, PAE_TYPE.YEOL, 1));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 1));
            // 1P
            this.cards.Add(new CCard(0, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(2, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(7, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(10, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(11, PAE_TYPE.KWANG, 0));
            // 2P
            this.cards.Add(new CCard(9, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(9, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(9, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(8, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(8, PAE_TYPE.PEE, 2));
            // 바닥
            this.cards.Add(new CCard(11, PAE_TYPE.TEE, 2));
            this.cards.Add(new CCard(11, PAE_TYPE.YEOL, 1));
            this.cards.Add(new CCard(11, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(0, PAE_TYPE.PEE, 3));
            // 1P
            this.cards.Add(new CCard(5, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(5, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(3, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(3, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(0, PAE_TYPE.TEE, 1));
            // 2P
            this.cards.Add(new CCard(8, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(6, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(6, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(6, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(4, PAE_TYPE.PEE, 3));
            // 더미
            this.cards.Add(new CCard(1, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(1, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(1, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(1, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(2, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(2, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(3, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(3, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(4, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(4, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(4, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(5, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(5, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(6, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(7, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(7, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(8, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(9, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(12, PAE_TYPE.PEE, 0));
            this.cards.Add(new CCard(12, PAE_TYPE.PEE, 1));
        }

        void make_order_cards()
        {
            this.cards.Clear();
            // 바닥
            // 1P
            // 2P
            // 바닥
            // 1P
            // 2P
            // 더미
            this.cards.Add(new CCard(0, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(0, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(0, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(0, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(1, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(1, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(1, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(1, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(2, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(2, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(2, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(2, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(3, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(3, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(3, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(3, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(4, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(4, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(4, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(4, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(5, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(5, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(5, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(5, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(6, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(6, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(6, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(6, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(7, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(7, PAE_TYPE.YEOL, 1));
            this.cards.Add(new CCard(7, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(7, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(8, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(8, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(8, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(8, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(9, PAE_TYPE.YEOL, 0));
            this.cards.Add(new CCard(9, PAE_TYPE.TEE, 1));
            this.cards.Add(new CCard(9, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(9, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(10, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 1));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 2));
            this.cards.Add(new CCard(10, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(11, PAE_TYPE.KWANG, 0));
            this.cards.Add(new CCard(11, PAE_TYPE.YEOL, 1));
            this.cards.Add(new CCard(11, PAE_TYPE.TEE, 2));
            this.cards.Add(new CCard(11, PAE_TYPE.PEE, 3));
            this.cards.Add(new CCard(12, PAE_TYPE.PEE, 0));
            this.cards.Add(new CCard(12, PAE_TYPE.PEE, 1));
        }
    }
}
