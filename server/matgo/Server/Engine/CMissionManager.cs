using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Engine
{
    public class CMissionManager
    {
        public byte missionscore;      // 미션 배수
        bool missionfail;       // 미션 실패
        bool missionsuccess;    // 미션 성공
        public MISSION currentMission { get; private set; }
        public List<CCard> missionCards { get; private set; }
        public CMissionManager()
        {
            this.missionCards = new List<CCard>();
        }

        public MISSION GetCurrentMission()
        {
            return currentMission;
        }

        public void reset()
        {
            this.missionfail = false;
            this.missionsuccess = false;
            this.missionscore = 1;

            this.currentMission = MISSION.NONE;

            this.missionCards.Clear();
        }

        public void resetNone()
        {
            this.missionfail = false;
            this.missionsuccess = false;
            this.missionscore = 0;

            this.currentMission = MISSION.NONE;

            this.missionCards.Clear();
        }
        // 새 미션 가져오기
        Random rnd = new Random((int)DateTime.UtcNow.Ticks);
        public MISSION NewMission(Dictionary<MISSION, int> MissionRate, MISSION ignore = MISSION.NONE)
        {
            MISSION new_mission = MISSION.NONE;

            if (MissionRate.Count == 0)
            {
                new_mission = MISSION.BAE2;
            }
            else
            {
                if (ignore != MISSION.NONE)
                {
                    while ((new_mission = WeightedRandomizer.From(MissionRate).TakeOne()) == ignore)
                    {

                    }
                }
                else
                {
                    new_mission = WeightedRandomizer.From(MissionRate).TakeOne();
                }
            }
#if DEBUG
            new_mission = MISSION.WOL11_4;
#endif
            return currentMission = new_mission;
        }
        List<CCard> get_random_cards(CCardManager cardmanager, byte number)
        {
            List<CCard> clone_cards = new List<CCard>();
            for (int i = 0; i < cardmanager.cards.Count; ++i)
            {
                clone_cards.Add(cardmanager.cards[i]);
            }
            List<CCard> numberList = clone_cards.FindAll(obj => obj.number == number);

            List<CCard> result = new List<CCard>();
            for (int i = 0; i < 2; ++i)
            {
                int index = rnd.Next(0, numberList.Count);
                result.Add(numberList[index]);

                numberList.RemoveAt(index);
            }

            return result;
        }

        // 미션 카드 설정
        public void SetMissionCard()
        {
            this.missionCards.Clear();
            switch (currentMission)
            {
                case MISSION.FIVEKWANG:
                    {
                        AddMissionCard(0, PAE_TYPE.KWANG, 0);
                        AddMissionCard(2, PAE_TYPE.KWANG, 0);
                        AddMissionCard(7, PAE_TYPE.KWANG, 0);
                        AddMissionCard(10, PAE_TYPE.KWANG, 0);
                        AddMissionCard(11, PAE_TYPE.KWANG, 0);
                        this.missionscore = 5;
                    }
                    break;
                case MISSION.KWANGTTANG:
                    {
                        AddMissionCard(0, PAE_TYPE.KWANG, 0);
                        AddMissionCard(2, PAE_TYPE.KWANG, 0);
                        AddMissionCard(7, PAE_TYPE.KWANG, 0);
                        this.missionscore = 4;
                    }
                    break;
                case MISSION.GODORI:
                    {
                        AddMissionCard(1, PAE_TYPE.YEOL, 0);
                        AddMissionCard(3, PAE_TYPE.YEOL, 0);
                        AddMissionCard(7, PAE_TYPE.YEOL, 1);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.HONGDAN:
                    {
                        AddMissionCard(0, PAE_TYPE.TEE, 1);
                        AddMissionCard(1, PAE_TYPE.TEE, 1);
                        AddMissionCard(2, PAE_TYPE.TEE, 1);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.CHODAN:
                    {
                        AddMissionCard(3, PAE_TYPE.TEE, 1);
                        AddMissionCard(4, PAE_TYPE.TEE, 1);
                        AddMissionCard(6, PAE_TYPE.TEE, 1);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.CHUNGDAN:
                    {
                        AddMissionCard(5, PAE_TYPE.TEE, 1);
                        AddMissionCard(8, PAE_TYPE.TEE, 1);
                        AddMissionCard(9, PAE_TYPE.TEE, 1);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL1_4:
                    {
                        AddMissionCard(0, PAE_TYPE.KWANG, 0);
                        AddMissionCard(0, PAE_TYPE.TEE, 1);
                        AddMissionCard(0, PAE_TYPE.PEE, 2);
                        AddMissionCard(0, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL2_4:
                    {
                        AddMissionCard(1, PAE_TYPE.YEOL, 0);
                        AddMissionCard(1, PAE_TYPE.TEE, 1);
                        AddMissionCard(1, PAE_TYPE.PEE, 2);
                        AddMissionCard(1, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL3_4:
                    {
                        AddMissionCard(2, PAE_TYPE.KWANG, 0);
                        AddMissionCard(2, PAE_TYPE.TEE, 1);
                        AddMissionCard(2, PAE_TYPE.PEE, 2);
                        AddMissionCard(2, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL4_4:
                    {
                        AddMissionCard(3, PAE_TYPE.YEOL, 0);
                        AddMissionCard(3, PAE_TYPE.TEE, 1);
                        AddMissionCard(3, PAE_TYPE.PEE, 2);
                        AddMissionCard(3, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL5_4:
                    {
                        AddMissionCard(4, PAE_TYPE.YEOL, 0);
                        AddMissionCard(4, PAE_TYPE.TEE, 1);
                        AddMissionCard(4, PAE_TYPE.PEE, 2);
                        AddMissionCard(4, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL6_4:
                    {
                        AddMissionCard(5, PAE_TYPE.YEOL, 0);
                        AddMissionCard(5, PAE_TYPE.TEE, 1);
                        AddMissionCard(5, PAE_TYPE.PEE, 2);
                        AddMissionCard(5, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL7_4:
                    {
                        AddMissionCard(6, PAE_TYPE.YEOL, 0);
                        AddMissionCard(6, PAE_TYPE.TEE, 1);
                        AddMissionCard(6, PAE_TYPE.PEE, 2);
                        AddMissionCard(6, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL8_4:
                    {
                        AddMissionCard(7, PAE_TYPE.KWANG, 0);
                        AddMissionCard(7, PAE_TYPE.YEOL, 1);
                        AddMissionCard(7, PAE_TYPE.PEE, 2);
                        AddMissionCard(7, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL9_4:
                    {
                        AddMissionCard(8, PAE_TYPE.YEOL, 0);
                        AddMissionCard(8, PAE_TYPE.TEE, 1);
                        AddMissionCard(8, PAE_TYPE.PEE, 2);
                        AddMissionCard(8, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL10_4:
                    {
                        AddMissionCard(9, PAE_TYPE.YEOL, 0);
                        AddMissionCard(9, PAE_TYPE.TEE, 1);
                        AddMissionCard(9, PAE_TYPE.PEE, 2);
                        AddMissionCard(9, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL11_4:
                    {
                        AddMissionCard(10, PAE_TYPE.KWANG, 0);
                        AddMissionCard(10, PAE_TYPE.PEE, 1);
                        AddMissionCard(10, PAE_TYPE.PEE, 2);
                        AddMissionCard(10, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL12_4:
                    {
                        AddMissionCard(11, PAE_TYPE.KWANG, 0);
                        AddMissionCard(11, PAE_TYPE.YEOL, 1);
                        AddMissionCard(11, PAE_TYPE.TEE, 2);
                        AddMissionCard(11, PAE_TYPE.PEE, 3);
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.WOL1_2:
                    {
                        AddMissionCard(0, PAE_TYPE.KWANG, 0);
                        AddMissionCard(0, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL2_2:
                    {
                        AddMissionCard(1, PAE_TYPE.YEOL, 0);
                        AddMissionCard(1, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL3_2:
                    {
                        AddMissionCard(2, PAE_TYPE.KWANG, 0);
                        AddMissionCard(2, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL4_2:
                    {
                        AddMissionCard(3, PAE_TYPE.YEOL, 0);
                        AddMissionCard(3, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL5_2:
                    {
                        AddMissionCard(4, PAE_TYPE.YEOL, 0);
                        AddMissionCard(4, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL6_2:
                    {
                        AddMissionCard(5, PAE_TYPE.YEOL, 0);
                        AddMissionCard(5, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL7_2:
                    {
                        AddMissionCard(6, PAE_TYPE.YEOL, 0);
                        AddMissionCard(6, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL8_2:
                    {
                        AddMissionCard(7, PAE_TYPE.KWANG, 0);
                        AddMissionCard(7, PAE_TYPE.YEOL, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL9_2:
                    {
                        AddMissionCard(8, PAE_TYPE.YEOL, 0);
                        AddMissionCard(8, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL10_2:
                    {
                        AddMissionCard(9, PAE_TYPE.YEOL, 0);
                        AddMissionCard(9, PAE_TYPE.TEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL11_2:
                    {
                        AddMissionCard(10, PAE_TYPE.KWANG, 0);
                        AddMissionCard(10, PAE_TYPE.PEE, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.WOL12_2:
                    {
                        AddMissionCard(11, PAE_TYPE.KWANG, 0);
                        AddMissionCard(11, PAE_TYPE.YEOL, 1);
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.BAE2:
                    {
                        this.missionscore = 2;
                    }
                    break;
                case MISSION.BAE3:
                    {
                        this.missionscore = 3;
                    }
                    break;
                case MISSION.BAE4:
                    {
                        this.missionscore = 4;
                    }
                    break;
                case MISSION.WHALBIN:
                    {
                        this.missionscore = 1;
                    }
                    break;
            }
        }
        // 미션 카드 추가
        void AddMissionCard(byte number, PAE_TYPE pae_type, byte position)
        {
            missionCards.Add(new CCard(number, pae_type, position));
        }

        public bool GetMissionFail()
        {
            return missionfail;
        }
        public bool GetMissionSuccess()
        {
            return missionsuccess;
        }

        // 미션 성공 실패 여부 확인
        public void CheckMission(System.Collections.Concurrent.ConcurrentDictionary<int, CPlayer> players)
        {
            // 미션 카드가 있을 때만 확인 
            CPlayer Player0 = players.First().Value;
            CPlayer Player1 = players.Last().Value;
            if (this.missionCards.Count > 0)
            {
                // 플레이어간 보유중인 미션카드 비교해서 성공 실패 여부 결정

                byte player0count = Player0.agent.get_same_cards_count_from_floor(this.missionCards);
                byte player1count = Player1.agent.get_same_cards_count_from_floor(this.missionCards);

                // 두 플레이어가 미션카드를 각자 가지고 있으면 미션 실패
                if (player0count > 0 && player1count > 0 && this.missionfail == false)
                {
                    Player0.agent.missionscore = 1;
                    Player0.agent.missionresult = 2;
                    Player1.agent.missionscore = 1;
                    Player1.agent.missionresult = 2;
                    this.missionsuccess = false;
                    this.missionfail = true;
                }
                // 한 플레이어가 미션카드를 모두 가지고 있으면 미션 성공
                else if (player0count == missionCards.Count && this.missionsuccess == false)
                {
                    Player0.agent.missionscore = this.missionscore;
                    Player0.agent.missionresult = 1;
                    Player1.agent.missionscore = 1;
                    Player1.agent.missionresult = 1;// 2; // 모두 미션 성공하도록 수정
                    this.missionsuccess = true;
                    this.missionfail = false;
                }
                else if (player1count == missionCards.Count && this.missionsuccess == false)
                {
                    Player1.agent.missionscore = this.missionscore;
                    Player1.agent.missionresult = 1;
                    Player0.agent.missionscore = 1;
                    Player0.agent.missionresult = 1;// 2; // 모두 미션 성공하도록 수정
                    this.missionsuccess = true;
                    this.missionfail = false;
                }
            }
            else
            {
                // 무조건 x배 일경우 적용
                switch (currentMission)
                {
                    case MISSION.BAE2:
                    case MISSION.BAE3:
                    case MISSION.BAE4:
                        {
                            Player0.agent.missionscore = this.missionscore;
                            Player1.agent.missionscore = this.missionscore;
                        }
                        break;
                }
            }
        }
    }

    public struct sMission
    {
        public int Id;
        public string Name;
        public int Reward;
        public int Rate;
    }
}
