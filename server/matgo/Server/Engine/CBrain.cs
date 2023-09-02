using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 용어의 정의
 * ① 족보 : 오광, 고도리, 사광, 삼광, 홍단, 초단, 청단, 비삼광을 족보라 한다.
 * ② 손패 : 플레이어의 손에 들고 있는 패를 말한다.
 * ③ 먹은패 : 플레이어가 먹은(가져간) 패를 말한다.
 * ④ 바닥패 : 바닥에 깔려 있는 패 중에서 플레이어의 손패에 짝이 있어서 먹을 수 있는 패를 말한다.
 * ⑤ 깨진패 : 상대방이 1장 이상을 가져가서 내가 족보를 이룰 수 없는 패를 말한다.
 * ⑥ 기대패 : 상대방이 1장도 가져가지 않아서 내가 족보를 이룰 수 있는 가능성이 존재하는 패를 말한다.
 * ⑦ 노릴패 : 먹은패와 손패와 바닥패를 합쳐서 족보가 완성될 수 있을 때의 바닥패를 노릴패라 한다.
 * ⑧ 버릴패 : 바닥에 먹을 패(바닥패)가 없을 경우, 한 장 버려야 하는 패를 말한다.
 * ⑨ 막을패 : 상대가 바닥패를 1장 먹으면 족보가 완성될 수 있을 때의 그 바닥패를 말한다.
 * ⑩ 본패 : 족보에 해당하는 패를 말한다.(위 표에서 회색 바탕의 셀에 있는 패. 10월오는 청단의 일원이므로 본패이다.)
 * ⑪ 짝패 : 족보의 짝이되는 패를 말한다.(본패가 아닌 패를 짝패라 한다. 10월열 및 10월피는 본패의 짝이므로 짝패이다.)
 * ⑫ 굳은자 : 어떠한 경우에도 상대방이 먹을 수 없는 바닥패를 말한다.
 */

/* 굳은자 종류와 규칙
 * A. 먹은패 2장, 바닥패 1장, 손패 1장
 * B. 바닥패(피) 2장, 손패 1장
 * C. 먹은패 2장, 손패 2장
 * D. 바닥패 1장, 손패 3장 (3장 폭탄)
 * E. 바닥패 2장, 손패 2장 (2장 폭탄)
 * F. 바닥패 3장, 손패 1장 (뻑 먹기)
 * 
 * 1. 먹는 순서는 D→E→F→B→A→C (수정 필요)
 * 2. 뺏어올 피가 없을 경우 먹는 순서 B→A→C->D→E→F (수정 필요)
 * 3. 쌍피를 뺏어올 수 있는 경우 약탈점수 추가 
 * 4. D,E,F의 경우 패4장에 대한 족보점수, 방어점수를 중첩하고, 무조건 본패점수 추가
 * 5. 상대가 7점 이상이면 굳은자도 바닥패 규칙 적용 + 피뺏기 장당 약탈점수 추가
 */
namespace Server.Engine
{
    public class CBrain
    {
        public CBrain()
        {
        }

        // 손패 낼때 2장패 선택해야될 경우 처리
        int ChoiceCardNumber = -1;
        byte ChoiceCardPosition = 0;
        public List<CCard> choiceCardList = new List<CCard>();
        
        public void NoChoiceCard()
        {
            this.ChoiceCardNumber = -1;
            this.ChoiceCardPosition = 0;
        }
        void SetChoiceCard(CCard card)
        {
            this.ChoiceCardNumber = card.number;
            this.ChoiceCardPosition = card.position;
        }
        public bool IsChoiceCard(byte number, byte position)
        {
            return (this.ChoiceCardNumber == number) && (this.ChoiceCardPosition == position);
        }

        #region 바닥패 규칙
        /* 낼 패 규칙
         * 1. 상대한테 피 점수가 있을 경우, 또는 쌍피를 뺏을 수 있다면 뺏는다. (서비스카드, 폭탄, 뻑회수)
         * 2. 바닥패의 기본 점수를 계산하여 가장 높은 점수에 맞는 패를 낸다.
         * 3. 바닥패, 굳은자가 없다면, 서비스카드를 낸다.
         * 4. 바닥패, 굳은자, 서비스카드가 없다면, 폭탄패를 낸다.
         * 5. 바닥패, 굳은자, 서비스카드, 폭탄패가 없다면, 버릴패를 낸다.
         */
        public int PutOutHandCardAI(List<CCard> hand_cards, List<CCard> myfloor_cards, List<CCard> otherFloor_cards, CFloorCardManager floor_card_manager, int remain_bomb_card_count)
        {
            int PutOutCardIndex = -1;

            // 피뺏기 규칙
            if (CheckTakeCard(hand_cards, otherFloor_cards))
            {
                PutOutCardIndex = CalculateTakeCard(hand_cards, otherFloor_cards);
            }

            if(PutOutCardIndex == -1)
            {
                // 바닥패 규칙
                PutOutCardIndex = CalculateHandCard(floor_card_manager, hand_cards, otherFloor_cards);

                if (PutOutCardIndex == -1)
                {
                    if (hand_cards.Count(c => c.is_bonus_card() == true) > 0)
                    {
                        // 서비스카드
                        PutOutCardIndex = hand_cards.FindIndex(c => c.is_bonus_card() == true);
                    }
                    else if (remain_bomb_card_count > 0)
                    {
                        // 폭탄패
                        PutOutCardIndex = 13;
                    }
                    else
                    {
                        // 버릴패
                        PutOutCardIndex = CalculateDropCard(hand_cards, myfloor_cards, otherFloor_cards, floor_card_manager);
                    }
                }
            }

            return PutOutCardIndex;
        }
        
        bool CheckTakeCard(List<CCard> hand_cards, List<CCard> otherFloor_cards)
        {
            int bonus_card_count = hand_cards.Count(c => c.is_bonus_card());

            int pee = otherFloor_cards.Count(c => c.is_same(PAE_TYPE.PEE));
            int two_three_pee = otherFloor_cards.Count(c => c.is_same_status(CARD_STATUS.TWO_PEE)) + otherFloor_cards.Count(c => c.is_same_status(CARD_STATUS.THREE_PEE));

            if ((bonus_card_count == 2 && pee <= two_three_pee + 1) ||
                (bonus_card_count == 1 && pee == two_three_pee))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int CalculateTakeCard(List<CCard> hand_cards, List<CCard> otherFloor_cards)
        {
            if (hand_cards.Count(c => c.is_bonus_card() == true) > 0)
            {
                return hand_cards.FindIndex(c => c.is_bonus_card());
            }
            else
            {
                return -1;
            }
        }
        int CalculateHandCard(CFloorCardManager Floor, List<CCard> MyHand, List<CCard> OtherFloor)
        {
            int bestCardPoint = 0;
            CCard bestCard = null;

            for (byte number = 0; number < 12; ++number)
            {
                List<CCard> floor_cards = Floor.get_cards(number);
                if (floor_cards == null || floor_cards.Count == 0) continue;

                List<CCard> match_cards = MyHand.Where(o => o.is_same_number(number)).ToList();
                if (match_cards == null || match_cards.Count == 0) continue;
                
                for(int i = 0;i < match_cards.Count; ++i)
                {
                    int cardPoint = GetBasePoint(OtherFloor, match_cards[i]);
                    if (cardPoint > bestCardPoint)
                    {
                        bestCardPoint = cardPoint;
                        bestCard = match_cards[i];
                    }
                }
            }

            if(bestCard != null)
            {
                return MyHand.FindIndex(o => o.is_same(bestCard.number, bestCard.position));
            }
            else
            {
                return -1;
            }
        }
        int CalculateDropCard(List<CCard> hand_cards, List<CCard> myfloor_cards, List<CCard> otherFloor_cards, CFloorCardManager floor_card_manager)
        {
            int bestCardIndex = -1;

            for (byte number = 0; number < 12; ++number)
            {
                List<CCard> floor_cards = floor_card_manager.get_cards(number);
                if (floor_cards != null && floor_cards.Count > 0) continue;

                bestCardIndex = GetDropCard(myfloor_cards, otherFloor_cards, hand_cards, number);

                if (bestCardIndex != -1) break;
            }

            if (bestCardIndex != -1)
                return bestCardIndex;
            else
                return 0;
        }
        int GetDropCard(List<CCard> myfloor, List<CCard> otherfloor, List<CCard> handCard, byte number)
        {
            int bestCardPoint = 10;
            int bestCardIndex = 0;
            for (int i = 0; i < handCard.Count; ++i)
            {
                if (handCard[i].number != number) continue;

                if (GetDropCardSafe(myfloor, otherfloor, handCard[i]))
                {
                    int cardPoint = GetBasePoint(otherfloor, handCard[i]);
                    if (cardPoint < bestCardPoint)
                    {
                        bestCardPoint = cardPoint;
                        bestCardIndex = handCard.FindIndex(o => o.is_same(handCard[i].number, handCard[i].position));
                    }

                    break;
                }
            }

            return bestCardIndex;
        }
        bool GetDropCardSafe(List<CCard> myfloor, List<CCard> otherfloor, CCard matchCard)
        {
            if (GetTargetFiveKwang(otherfloor, matchCard) != 0) return false;
            if (GetTargetFourKwang(otherfloor, myfloor, matchCard) != 0) return false;
            if (GetTargetThreeKwang(otherfloor, matchCard) != 0) return false;
            if (GetTargetGodori(otherfloor, matchCard) != 0) return false;
            if (GetTargetHongDan(otherfloor, matchCard) != 0) return false;
            if (GetTargetChoDan(otherfloor, matchCard) != 0) return false;
            if (GetTargetChungDan(otherfloor, matchCard) != 0) return false;

            return true;
        }
        #endregion

        #region 붙은패 규칙
        /* 1. 기본점수가 높은 패를 먹는다.
         */
        public int ChoiceFloorCard(
       List<CCard> otherFloor)
        {
            int bestCardPoint = 0;
            int bestCardChoice = 0;

            for (int i = 0; i < choiceCardList.Count; ++i)
            {
                int cardPoint = GetChoiceCardsPoint(otherFloor, choiceCardList[i]);
                if (cardPoint > bestCardPoint)
                {
                    bestCardPoint = cardPoint;
                    bestCardChoice = i;
                }
            }

            return bestCardChoice;
        }

        int GetChoiceCardsPoint(List<CCard> otherFloor, CCard choiceCard)
        {
            return GetBasePoint(otherFloor, choiceCard);
        }
        #endregion

        // 기본점수
        int GetBasePoint(List<CCard> otherfloor, CCard card)
        {
            bool brokenCard = false;

            if (card.pae_type == PAE_TYPE.KWANG && IsHaveKwangCard(otherfloor))
            {
                brokenCard = true;
            }
            else if (card.status == CARD_STATUS.GODORI && IsHaveGodoriCard(otherfloor))
            {
                brokenCard = true;
            }
            else if (card.status == CARD_STATUS.HONG_DAN && IsHaveHongDanCard(otherfloor))
            {
                brokenCard = true;
            }
            else if (card.status == CARD_STATUS.CHO_DAN && IsHaveChoDanCard(otherfloor))
            {
                brokenCard = true;
            }
            else if (card.status == CARD_STATUS.CHEONG_DAN && IsHaveChungDanCard(otherfloor))
            {
                brokenCard = true;
            }

            switch (card.pae_type)
            {
                case PAE_TYPE.KWANG:
                    {
                        if (brokenCard)
                        {
                            return 0;
                        }
                        else if (card.number == 11)
                        {
                            return 8;
                        }
                        else
                        {
                            return 9;
                        }
                    }
                case PAE_TYPE.YEOL:
                    {
                        if (brokenCard == false &&
                            (card.number == 1 ||
                            card.number == 3 ||
                            card.number == 7))
                        {
                            return 7;
                        }
                        else if (card.number == 8)
                        {
                            return 5;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                case PAE_TYPE.TEE:
                    {
                        if (brokenCard == true || card.number == 11)
                        {
                            return 2;
                        }
                        else
                        {
                            return 6;
                        }
                    }
                case PAE_TYPE.PEE:
                    {
                        if (card.number == 10 ||
                            card.number == 11)
                        {
                            return 4;
                        }
                        else
                        {
                            return 3;
                        }
                    }
            }

            // error 

            return 0;
        }
        bool IsHaveKwangCard(List<CCard> floor)
        {
            short count = 0;
            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].pae_type == PAE_TYPE.KWANG) ++count;
            }
            return count >= 3;
        }
        bool IsHaveGodoriCard(List<CCard> floor)
        {
            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.GODORI)) return true;
            }
            return false;
        }
        bool IsHaveHongDanCard(List<CCard> floor)
        {
            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.HONG_DAN)) return true;
            }
            return false;
        }
        bool IsHaveChoDanCard(List<CCard> floor)
        {
            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.CHO_DAN)) return true;
            }
            return false;
        }
        bool IsHaveChungDanCard(List<CCard> floor)
        {
            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.CHEONG_DAN)) return true;
            }
            return false;
        }

        // 족보점수 (바닥패가 본패일 경우 추가점수)
        int GetMadeFiveKwang(List<CCard> floor, CCard floorCard, CCard handCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].pae_type == PAE_TYPE.KWANG) ++count;
            }
            if (count == 5)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 5)
            {
                return 300 + 100;
            }

            // 손패가 본패
            if (handCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 5)
            {
                return 300;
            }

            return 0;
        }
        int GetMadeFourKwang(List<CCard> floor, List<CCard> otherfloor, CCard floorCard, CCard handCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].pae_type == PAE_TYPE.KWANG) ++count;
            }
            if (count != 3)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 4)
            {
                // 오광 깨졌으면 추가 점수 감소
                for (int i = 0; i < otherfloor.Count; ++i)
                {
                    if (otherfloor[i].pae_type == PAE_TYPE.KWANG) return 60 + 100;
                }

                return 120 + 100;
            }

            // 손패가 본패
            if (handCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 4)
            {
                // 오광 깨졌으면 추가 점수 감소
                for (int i = 0; i < otherfloor.Count; ++i)
                {
                    if (otherfloor[i].pae_type == PAE_TYPE.KWANG) return 60;
                }

                return 120;
            }

            return 0;
        }
        int GetMadeThreeKwang(List<CCard> floor, CCard floorCard, CCard handCard)
        {
            // 비광은 추가 점수 감소
            bool bi = false;
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].pae_type == PAE_TYPE.KWANG)
                {
                    ++count;
                    if (floor[i].number == CCard.BEE_KWANG)
                        bi = true;
                }
            }
            if (count != 3)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.pae_type == PAE_TYPE.KWANG)
            {
                ++count;
                if (floorCard.number == CCard.BEE_KWANG)
                    bi = true;
            }
            if (count == 3)
            {
                if (bi)
                    return 80 + 100;
                else
                    return 110 + 100;
            }

            // 손패가 본패
            if (handCard.pae_type == PAE_TYPE.KWANG)
            {
                ++count;
                if (handCard.number == CCard.BEE_KWANG)
                    bi = true;
            }
            if (count == 3)
            {
                if (bi)
                    return 80;
                else
                    return 110;
            }

            return 0;
        }
        int GetMadeGodori(List<CCard> floor, CCard floorCard, CCard handCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.GODORI)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.GODORI)) ++count;
            if (count == 3)
            {
                return 150 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.GODORI)) ++count;
            if (count == 3)
            {
                return 150;
            }

            return 0;
        }
        int GetMadeHongDan(List<CCard> floor, CCard floorCard, CCard handCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.HONG_DAN)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.HONG_DAN)) ++count;
            if (count == 3)
            {
                return 100 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.HONG_DAN)) ++count;
            if (count == 3)
            {
                return 100;
            }

            return 0;
        }
        int GetMadeChoDan(List<CCard> floor, CCard floorCard, CCard handCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.CHO_DAN)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.CHO_DAN)) ++count;
            if (count == 3)
            {
                return 100 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.CHO_DAN)) ++count;
            if (count == 3)
            {
                return 100;
            }

            return 0;
        }
        int GetMadeChungDan(List<CCard> floor, CCard floorCard, CCard handCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.CHEONG_DAN)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.CHEONG_DAN)) ++count;
            if (count == 3)
            {
                return 100 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.CHEONG_DAN)) ++count;
            if (count == 3)
            {
                return 100;
            }

            return 0;
        }

        // 방어점수 (바닥패가 본패일 경우 추가점수, 내 손패가 본패일 경우 점수 없음)
        int GetDefenceFiveKwang(List<CCard> floor, CCard floorCard, CCard handCard, ref bool madeCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].pae_type == PAE_TYPE.KWANG) ++count;
            }
            if (count != 4)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 5)
            {
                return 300 + 100;
            }

            // 손패가 본패
            if (handCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 5)
            {
                madeCard = true;
                return 0;
            }

            // 바닥패가 짝패
            if (IsPairKwang(floorCard)) ++count;
            if (count == 5)
            {
                return 300;
            }

            return 0;
        }
        int GetDefenceFourKwang(List<CCard> floor, List<CCard> myfloor, CCard floorCard, CCard handCard, ref bool madeCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].pae_type == PAE_TYPE.KWANG) ++count;
            }
            if (count != 3)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 4)
            {
                // 오광 깨졌으면 추가 점수 감소
                for (int i = 0; i < myfloor.Count; ++i)
                {
                    if (myfloor[i].pae_type == PAE_TYPE.KWANG) return 60 + 100;
                }

                return 120 + 100;
            }

            // 손패가 본패
            if (handCard.pae_type == PAE_TYPE.KWANG) ++count;
            if (count == 4)
            {
                madeCard = true;
                return 0;
            }

            // 바닥패가 짝패
            if (IsPairKwang(floorCard)) ++count;
            if (count == 4)
            {
                // 오광 깨졌으면 추가 점수 감소
                for (int i = 0; i < myfloor.Count; ++i)
                {
                    if (myfloor[i].pae_type == PAE_TYPE.KWANG) return 60;
                }

                return 120;
            }

            return 0;
        }
        int GetDefenceThreeKwang(List<CCard> floor, CCard floorCard, CCard handCard, ref bool madeCard)
        {
            bool bi = false;
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].pae_type == PAE_TYPE.KWANG)
                {
                    ++count;
                    if (floor[i].number == CCard.BEE_KWANG)
                        bi = true;
                }
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.pae_type == PAE_TYPE.KWANG)
            {
                ++count;
                if (floorCard.number == CCard.BEE_KWANG)
                    bi = true;
            }
            if (count == 3)
            {
                if (bi)
                    return 80 + 100;
                else
                    return 110 + 100;
            }

            // 손패가 본패
            if (handCard.pae_type == PAE_TYPE.KWANG)
            {
                ++count;
                if (handCard.number == CCard.BEE_KWANG)
                    bi = true;
            }
            if (count == 3)
            {
                madeCard = true;
                return 0;
            }

            // 바닥패가 짝패
            if (IsPairKwang(floorCard))
            {
                ++count;
                if (floorCard.number == CCard.BEE_KWANG)
                    bi = true;
            }
            if (count == 3)
            {
                if (bi)
                    return 80;
                else
                    return 110;
            }

            return 0;
        }
        int GetDefenceGodori(List<CCard> floor, CCard floorCard, CCard handCard, ref bool madeCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.GODORI)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.GODORI)) ++count;
            if (count == 3)
            {
                return 150 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.GODORI)) ++count;
            if (count == 3)
            {
                madeCard = true;
                return 0;
            }

            // 바닥패가 짝패
            if (IsPairGodori(floorCard)) ++count;
            if (count == 3)
            {
                return 150;
            }

            return 0;
        }
        int GetDefenceHongDan(List<CCard> floor, CCard floorCard, CCard handCard, ref bool madeCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.HONG_DAN)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.HONG_DAN)) ++count;
            if (count == 3)
            {
                return 100 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.HONG_DAN)) ++count;
            if (count == 3)
            {
                madeCard = true;
                return 0;
            }

            // 바닥패가 짝패
            if (IsPairHongdan(floorCard)) ++count;
            if (count == 3)
            {
                return 100;
            }

            return 0;
        }
        int GetDefenceChoDan(List<CCard> floor, CCard floorCard, CCard handCard, ref bool madeCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.CHO_DAN)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.CHO_DAN)) ++count;
            if (count == 3)
            {
                return 100 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.CHO_DAN)) ++count;
            if (count == 3)
            {
                madeCard = true;
                return 0;
            }

            // 바닥패가 짝패
            if (IsPairChodan(floorCard)) ++count;
            if (count == 3)
            {
                return 100;
            }

            return 0;
        }
        int GetDefenceChungDan(List<CCard> floor, CCard floorCard, CCard handCard, ref bool madeCard)
        {
            int count = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                if (floor[i].is_same_status(CARD_STATUS.CHEONG_DAN)) ++count;
            }
            if (count != 2)
            {
                return 0;
            }

            // 바닥패가 본패
            if (floorCard.is_same_status(CARD_STATUS.CHEONG_DAN)) ++count;
            if (count == 3)
            {
                return 100 + 100;
            }

            // 손패가 본패
            if (handCard.is_same_status(CARD_STATUS.CHEONG_DAN)) ++count;
            if (count == 3)
            {
                madeCard = true;
                return 0;
            }

            // 바닥패가 짝패
            if (IsPairChungdan(floorCard)) ++count;
            if (count == 3)
            {
                return 100;
            }

            return 0;
        }

        // 노림패 확인
        int GetTargetFiveKwang(List<CCard> floor, CCard handCard)
        {
            bool sameNumber = false;
            List<CCard> made = floor.Where(o => o.is_same(PAE_TYPE.KWANG)).ToList();

            if (made != null && made.Count == 4)
            {
                if (made.Find(o => o.number == 0) == null && handCard.number == 0) sameNumber = true;
                else if (made.Find(o => o.number == 2) == null && handCard.number == 2) sameNumber = true;
                else if (made.Find(o => o.number == 7) == null && handCard.number == 7) sameNumber = true;
                else if (made.Find(o => o.number == 10) == null && handCard.number == 10) sameNumber = true;
                else if (made.Find(o => o.number == 11) == null && handCard.number == 11) sameNumber = true;

                if (sameNumber)
                {
                    // 본패
                    if (handCard.pae_type == PAE_TYPE.KWANG)
                    {
                        return 1;
                    }
                    // 짝패
                    else
                    {
                        return 2;
                    }
                }
            }

            return 0;
        }
        int GetTargetFourKwang(List<CCard> floor, List<CCard> otherfloor, CCard handCard)
        {
            bool sameNumber = false;
            List<CCard> made = floor.Where(o => o.is_same(PAE_TYPE.KWANG)).ToList();

            if (made != null && made.Count == 3)
            {
                if (made.Find(o => o.number == 0) == null && handCard.number == 0) sameNumber = true;
                else if (made.Find(o => o.number == 2) == null && handCard.number == 2) sameNumber = true;
                else if (made.Find(o => o.number == 7) == null && handCard.number == 7) sameNumber = true;
                else if (made.Find(o => o.number == 10) == null && handCard.number == 10) sameNumber = true;
                else if (made.Find(o => o.number == 11) == null && handCard.number == 11) sameNumber = true;

                if (sameNumber)
                {
                    // 본패
                    if (handCard.pae_type == PAE_TYPE.KWANG)
                    {
                        for (int i = 0; i < otherfloor.Count; ++i)
                            if (otherfloor[i].pae_type == PAE_TYPE.KWANG) return 3;
                        return 1;
                    }
                    // 짝패
                    else
                    {
                        for (int i = 0; i < otherfloor.Count; ++i)
                            if (otherfloor[i].pae_type == PAE_TYPE.KWANG) return 4;
                        return 2;
                    }
                }
            }

            return 0;
        }
        int GetTargetThreeKwang(List<CCard> floor, CCard handCard)
        {
            bool sameNumber = false;
            List<CCard> made = floor.Where(o => o.is_same(PAE_TYPE.KWANG)).ToList();
            bool bi = false;

            if (made != null && made.Count == 2)
            {
                if (made.Find(o => o.number == 0) == null && handCard.number == 0) sameNumber = true;
                else if (made.Find(o => o.number == 2) == null && handCard.number == 2) sameNumber = true;
                else if (made.Find(o => o.number == 7) == null && handCard.number == 7) sameNumber = true;
                else if (made.Find(o => o.number == 10) == null && handCard.number == 10) sameNumber = true;
                else if (made.Find(o => o.number == 11) == null && handCard.number == 11) sameNumber = true;

                if (sameNumber)
                {
                    // 비삼광
                    if (made.Find(o => o.number == CCard.BEE_KWANG) != null)
                        bi = true;

                    // 본패
                    if (handCard.pae_type == PAE_TYPE.KWANG)
                    {
                        if (!bi)
                            if (handCard.number == CCard.BEE_KWANG)
                                bi = true;
                        if (bi)
                            return 3;
                        return 1;
                    }
                    // 짝패
                    else
                    {
                        if (bi)
                            return 4;
                        return 2;
                    }
                }
            }

            return 0;
        }
        int GetTargetGodori(List<CCard> floor, CCard handCard)
        {
            bool sameNumber = false;
            List<CCard> made = floor.Where(o => o.is_same_status(CARD_STATUS.GODORI)).ToList();

            if (made != null && made.Count == 2)
            {
                if (made.Find(o => o.number == 1) == null && handCard.number == 1) sameNumber = true;
                else if (made.Find(o => o.number == 3) == null && handCard.number == 3) sameNumber = true;
                else if (made.Find(o => o.number == 7) == null && handCard.number == 7) sameNumber = true;

                if (sameNumber)
                {
                    // 본패
                    if (handCard.status == CARD_STATUS.GODORI)
                    {
                        return 1;
                    }
                    // 짝패
                    else
                    {
                        return 2;
                    }
                }
            }

            return 0;
        }
        int GetTargetHongDan(List<CCard> floor, CCard handCard)
        {
            bool sameNumber = false;
            List<CCard> made = floor.Where(o => o.is_same_status(CARD_STATUS.HONG_DAN)).ToList();

            if (made != null && made.Count == 2)
            {
                if (made.Find(o => o.number == 0) == null && handCard.number == 0) sameNumber = true;
                else if (made.Find(o => o.number == 1) == null && handCard.number == 1) sameNumber = true;
                else if (made.Find(o => o.number == 2) == null && handCard.number == 2) sameNumber = true;

                if (sameNumber)
                {
                    // 본패
                    if (handCard.status == CARD_STATUS.HONG_DAN)
                    {
                        return 1;
                    }
                    // 짝패
                    else
                    {
                        return 2;
                    }
                }
            }

            return 0;
        }
        int GetTargetChoDan(List<CCard> floor, CCard handCard)
        {
            bool sameNumber = false;
            List<CCard> made = floor.Where(o => o.is_same_status(CARD_STATUS.CHO_DAN)).ToList();

            if (made != null && made.Count == 2)
            {
                if (made.Find(o => o.number == 3) == null && handCard.number == 3) sameNumber = true;
                else if (made.Find(o => o.number == 4) == null && handCard.number == 4) sameNumber = true;
                else if (made.Find(o => o.number == 6) == null && handCard.number == 6) sameNumber = true;

                if (sameNumber)
                {
                    // 본패
                    if (handCard.status == CARD_STATUS.CHO_DAN)
                    {
                        return 1;
                    }
                    // 짝패
                    else
                    {
                        return 2;
                    }
                }
            }

            return 0;
        }
        int GetTargetChungDan(List<CCard> floor, CCard handCard)
        {
            bool sameNumber = false;
            List<CCard> made = floor.Where(o => o.is_same_status(CARD_STATUS.CHEONG_DAN)).ToList();

            if (made != null && made.Count == 2)
            {
                if (made.Find(o => o.number == 5) == null && handCard.number == 5) sameNumber = true;
                else if (made.Find(o => o.number == 8) == null && handCard.number == 8) sameNumber = true;
                else if (made.Find(o => o.number == 9) == null && handCard.number == 9) sameNumber = true;

                if (sameNumber)
                {
                    // 본패
                    if (handCard.status == CARD_STATUS.CHEONG_DAN)
                    {
                        return 1;
                    }
                    // 짝패
                    else
                    {
                        return 2;
                    }
                }
            }

            return 0;
        }
        int GetTargetMatchCardType(List<CCard> myfloor, List<CCard> otherfloor, CCard matchCard)
        {
            /* 노림패 result
                0: 노림패 없음
                1: 오광 본패
                2: 오광 짝패
                3: 오광안깨진사광 본패
                4: 오광안깨진사광 짝패
                5: 오광깨진사광 본패
                6: 오광깨진사관 짝패
                7: 삼광 본패
                8: 삼광 짝패
                9: 비삼광 본패
                10:비삼광 짝패
                11:고도리 본패
                12:고도리 짝패
                13:홍단 본패
                14:홍단 짝패
                15:청단 본패
                16:청단 짝패
                17:초단 본패
                18:초단 짝패
            */
            int resultFiveKwang = 0, resultFourKwang = 0, resultThreeKwang = 0, resultGodori = 0, resultHongDan = 0, resultChoDan = 0, resultChunDan = 0;

            resultFiveKwang = GetTargetFiveKwang(otherfloor, matchCard);
            if (resultFiveKwang == 1) return 1;
            else if (resultFiveKwang == 2) return 2;
            resultFourKwang = GetTargetFourKwang(otherfloor, myfloor, matchCard);
            if (resultFourKwang == 1) return 3;
            else if (resultFourKwang == 2) return 4;
            else if (resultFourKwang == 3) return 5;
            else if (resultFourKwang == 4) return 6;
            resultThreeKwang = GetTargetThreeKwang(otherfloor, matchCard);
            if (resultThreeKwang == 1) return 7;
            else if (resultThreeKwang == 2) return 8;
            else if (resultThreeKwang == 3) return 9;
            else if (resultThreeKwang == 4) return 10;
            resultGodori = GetTargetGodori(otherfloor, matchCard);
            if (resultGodori == 1) return 11;
            else if (resultGodori == 2) return 12;
            resultHongDan = GetTargetHongDan(otherfloor, matchCard);
            if (resultHongDan == 1) return 13;
            else if (resultHongDan == 2) return 14;
            resultChoDan = GetTargetChoDan(otherfloor, matchCard);
            if (resultChoDan == 1) return 15;
            else if (resultChoDan == 2) return 16;
            resultChunDan = GetTargetChungDan(otherfloor, matchCard);
            if (resultChunDan == 1) return 17;
            else if (resultChunDan == 2) return 18;

            return 0;
        }

        // 족보 짝패 확인
        bool IsPairKwang(CCard card)
        {
            if (card.is_same_number(0)) return true;
            if (card.is_same_number(2)) return true;
            if (card.is_same_number(7)) return true;
            if (card.is_same_number(10)) return true;
            if (card.is_same_number(11)) return true;

            return false;
        }
        bool IsPairGodori(CCard card)
        {
            if (card.is_same_number(1)) return true;
            if (card.is_same_number(3)) return true;
            if (card.is_same_number(7)) return true;

            return false;
        }
        bool IsPairHongdan(CCard card)
        {
            if (card.is_same_number(0)) return true;
            if (card.is_same_number(1)) return true;
            if (card.is_same_number(2)) return true;

            return false;
        }
        bool IsPairChodan(CCard card)
        {
            if (card.is_same_number(3)) return true;
            if (card.is_same_number(4)) return true;
            if (card.is_same_number(6)) return true;

            return false;
        }
        bool IsPairChungdan(CCard card)
        {
            if (card.is_same_number(5)) return true;
            if (card.is_same_number(8)) return true;
            if (card.is_same_number(9)) return true;

            return false;
        }

        //피뺏기
        List<CCard> takenCard(List<CCard> floor, int takenCount)
        {
            if (floor.Count == 0) return null;

            // 가져올 패 선택
            List<CCard> taken_cards = takenCardFromFloor(floor, takenCount);

            return taken_cards;
        }
        List<CCard> takenCardFromFloor(List<CCard> floor, int takenCount)
        {
            if (floor == null) return null;

            List<CCard> pee_card = floor.FindAll(obj => obj.status != CARD_STATUS.TWO_PEE && obj.status != CARD_STATUS.THREE_PEE);
            List<CCard> twopee_card = floor.FindAll(obj => obj.status == CARD_STATUS.TWO_PEE);
            List<CCard> threepee_card = floor.FindAll(obj => obj.status == CARD_STATUS.THREE_PEE);
            
            List<CCard> result_cards = new List<CCard>();

            // 가져올 바닥의 피 선택. 피가 부족하면 쌍피나 쓰리피를 가져옴.
            for (int count = 0; count < takenCount;)
            {
                int takenLeft = takenCount - count;
                if (takenLeft >= 3)
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
                else if (takenLeft >= 2)
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
                else if (takenLeft >= 1)
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

        // 점수 계산
        public int GetCalculateScore(List<CCard> floor)
        {
            short score = 0, pae_pee = 0, pae_tee = 0, pae_yeol = 0, pae_kwang = 0, made_godiri = 0, made_hongdan = 0, made_chodan = 0, made_cheongdan = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                switch (floor[i].pae_type)
                {
                    case PAE_TYPE.PEE:
                        {
                            if (floor[i].status == CARD_STATUS.TWO_PEE)
                            {
                                pae_pee += 2;
                            }
                            else if (floor[i].status == CARD_STATUS.THREE_PEE)
                            {
                                pae_pee += 3;
                            }
                            else
                            {
                                pae_pee += 1;
                            }
                        }
                        break;

                    case PAE_TYPE.TEE:
                        {
                            if (floor[i].status == CARD_STATUS.HONG_DAN)
                            {
                                ++made_hongdan;
                            }
                            else if (floor[i].status == CARD_STATUS.CHO_DAN)
                            {
                                ++made_chodan;
                            }
                            else if (floor[i].status == CARD_STATUS.CHEONG_DAN)
                            {
                                ++made_cheongdan;
                            }

                            ++pae_tee;
                        }
                        break;

                    case PAE_TYPE.YEOL:
                        {
                            if (floor[i].status == CARD_STATUS.GODORI)
                            {
                                ++made_godiri;
                            }

                            ++pae_yeol;
                        }
                        break;

                    case PAE_TYPE.KWANG:
                        {
                            ++pae_kwang;
                        }
                        break;

                }
            }

            if (pae_pee >= 10)
            {
                score += (short)(pae_pee - 9);
            }
            if (pae_tee >= 10)
            {
                score += (short)(pae_tee - 4);
            }
            if (pae_yeol >= 10)
            {
                score += (short)(pae_yeol - 4);
            }

            if (pae_kwang == 3)
            {
                if (floor.Exists(obj => obj.is_same(CCard.BEE_KWANG, PAE_TYPE.KWANG)))
                {
                    score += 2;
                }
                else
                {
                    score += 3;
                }
            }
            else if (pae_kwang == 4)
            {
                score += 4;
            }
            else if (pae_kwang == 5)
            {
                score += 15;
            }

            if (made_godiri == 3)
            {
                score += 5;
            }
            if (made_hongdan == 3)
            {
                score += 3;
            }
            if (made_chodan == 3)
            {
                score += 3;
            }
            if (made_cheongdan == 3)
            {
                score += 3;
            }

            return score;
        }
        public void CalculateDouble(List<CCard> floor, List<CCard> ohterFloor, out bool DoublePee, out bool DoubleKwang, out bool DoubleMungtta)
        {
            short pae_pee = 0, pae_yeol = 0, pae_kwang = 0;
            short other_pae_pee = 0, other_pae_kwang = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                switch (floor[i].pae_type)
                {
                    case PAE_TYPE.PEE:
                        {
                            if (floor[i].status == CARD_STATUS.TWO_PEE)
                            {
                                pae_pee += 2;
                            }
                            else if (floor[i].status == CARD_STATUS.THREE_PEE)
                            {
                                pae_pee += 3;
                            }
                            else
                            {
                                pae_pee += 1;
                            }
                        }
                        break;
                    case PAE_TYPE.YEOL:
                        {
                            ++pae_yeol;
                        }
                        break;

                    case PAE_TYPE.KWANG:
                        {
                            ++pae_kwang;
                        }
                        break;

                }
            }

            for (int i = 0; i < ohterFloor.Count; ++i)
            {
                switch (ohterFloor[i].pae_type)
                {
                    case PAE_TYPE.PEE:
                        {
                            if (ohterFloor[i].status == CARD_STATUS.TWO_PEE)
                            {
                                other_pae_pee += 2;
                            }
                            else if (ohterFloor[i].status == CARD_STATUS.THREE_PEE)
                            {
                                other_pae_pee += 3;
                            }
                            else
                            {
                                other_pae_pee += 1;
                            }
                        }
                        break;

                    case PAE_TYPE.KWANG:
                        {
                            ++other_pae_kwang;
                        }
                        break;

                }
            }

            if (pae_pee >= 10 && other_pae_pee <= 7)
            {
                DoublePee = true;
            }
            else
            {
                DoublePee = false;
            }
            if (pae_yeol >= 7)
            {
                DoubleMungtta = true;
            }
            else
            {
                DoubleMungtta = false;
            }

            if (pae_kwang >= 3 && other_pae_kwang == 0)
            {
                DoubleKwang = true;
            }
            else
            {
                DoubleKwang = false;
            }
        }
        public bool CalculateDoubleExpect(List<CCard> floor, List<CCard> ohterFloor, bool DoublePee, bool DoubleKwang, bool DoubleMungtta)
        {
            short pae_pee = 0, pae_yeol = 0, pae_kwang = 0;
            short other_pae_pee = 0, other_pae_kwang = 0;

            for (int i = 0; i < floor.Count; ++i)
            {
                switch (floor[i].pae_type)
                {
                    case PAE_TYPE.PEE:
                        {
                            if (floor[i].status == CARD_STATUS.TWO_PEE)
                            {
                                pae_pee += 2;
                            }
                            else if (floor[i].status == CARD_STATUS.THREE_PEE)
                            {
                                pae_pee += 3;
                            }
                            else
                            {
                                pae_pee += 1;
                            }
                        }
                        break;
                    case PAE_TYPE.YEOL:
                        {
                            ++pae_yeol;
                        }
                        break;

                    case PAE_TYPE.KWANG:
                        {
                            ++pae_kwang;
                        }
                        break;

                }
            }

            for (int i = 0; i < ohterFloor.Count; ++i)
            {
                switch (ohterFloor[i].pae_type)
                {
                    case PAE_TYPE.PEE:
                        {
                            if (ohterFloor[i].status == CARD_STATUS.TWO_PEE)
                            {
                                other_pae_pee += 2;
                            }
                            else if (ohterFloor[i].status == CARD_STATUS.THREE_PEE)
                            {
                                other_pae_pee += 3;
                            }
                            else
                            {
                                other_pae_pee += 1;
                            }
                        }
                        break;

                    case PAE_TYPE.KWANG:
                        {
                            ++other_pae_kwang;
                        }
                        break;

                }
            }

            if (pae_pee >= 10 && other_pae_pee <= 7 && DoublePee == false)
            {
                return true;
            }
            if (pae_yeol >= 7 && DoubleMungtta == false)
            {
                return true;
            }
            if (pae_kwang >= 3 && other_pae_kwang == 0 && DoubleKwang == false)
            {
                return true;
            }

            return false;
        }

    }
}
