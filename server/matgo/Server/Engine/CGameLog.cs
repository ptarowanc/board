using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Engine
{
    public class CGameLog : ICloneable
    {
        public CGameLog(int max_users)
        {
            PlayerLog = new CPlayerLog[max_users];
        }

        // 게임 로그
        public Guid LogId = Guid.Empty;//NewGuid()

        public int BetMoney;

        public DateTime StratTime;
        public DateTime EndTime;

        public CPlayerLog[] PlayerLog;
        public string PlayLog = "";

        public int Mission;
        public int MissionResult;

        public bool FirstPlayer = false; // Id1이 선플레이어

        public void Reset(int betMoney)
        {
            LogId = Guid.NewGuid();
            BetMoney = betMoney;
            StratTime = DateTime.Now;
            PlayLog = "";
            for (int i = 0; i < PlayerLog.Length; ++i)
                PlayerLog[i] = new CPlayerLog();
            Mission = 0;
            MissionResult = 0;
        }
        public void Start(List<CCard> cardsFirst, List<CCard> cardsAfter, List<CCard> cardsFloor)
        {
            for (int i = 0; i < cardsFirst.Count; ++i)
                PlayLog += MakeCard(cardsFirst[i].number, cardsFirst[i].position);
            PlayLog += ",";
            for (int i = 0; i < cardsAfter.Count; ++i)
                PlayLog += MakeCard(cardsAfter[i].number, cardsAfter[i].position);
            PlayLog += ",";
            for (int i = 0; i < cardsFloor.Count; ++i)
                PlayLog += MakeCard(cardsFloor[i].number, cardsFloor[i].position);
            PlayLog += ";";
        }

        /*
            action
            a   플레이어 손에 패 추가 (보너스패 냈을때)
            b	플레이어가 낸 손패
            c	더미에서 나온 패
            d	플레이어의 바닥패로 이동
            e	바닥에 패 놓기 (맨 처음에 바닥에 보너스패 있을때)
            f	
            g	국진 이동
            h	고스톱
         */
        public void ActionCard (string action, CCard card)
        {
            PlayLog += action + MakeCard(card.number, card.position) + ",";
        }

        public void TurnNext()
        {
            PlayLog += "_";
        }

        public void ActionKookJin(string action)
        {
            PlayLog += "g" + action + ",";
        }

        public void ActionGoStop(string action)
        {
            PlayLog += "h" + action + ",";
        }

        public void Result(int winner_index, int winner_score)
        {
            PlayLog += ";" + winner_index.ToString() + "," + winner_score.ToString();
        }

        public object End()
        {
            EndTime = DateTime.Now;
            return Clone();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        string MakeCard(int number, int position)
        {
            string card = "";
            switch (number)
            {
                case 0:
                    card = "a";
                    break;
                case 1:
                    card = "b";
                    break;
                case 2:
                    card = "c";
                    break;
                case 3:
                    card = "d";
                    break;
                case 4:
                    card = "e";
                    break;
                case 5:
                    card = "f";
                    break;
                case 6:
                    card = "g";
                    break;
                case 7:
                    card = "h";
                    break;
                case 8:
                    card = "i";
                    break;
                case 9:
                    card = "j";
                    break;
                case 10:
                    card = "k";
                    break;
                case 11:
                    card = "l";
                    break;
                case 12:
                    card = "m";
                    break;
            }
            return card + (position + 1).ToString();
        }
    }
    public class CPlayerData
    {
        public CPlayer Instance;
        public UserStatus status = UserStatus.None;
        public UserData data = default(UserData);

        //public bool isCompleteTopMission = false; // 탑쌓기 이벤트 완료 여부
        public long ChangeMoney = 0;
        public int GameResult = 0;

        public long GameDealMoney = 0;
        public long JackPotDealMoney = 0;
    }
    public class CPlayerLog
    {
        public int UserID = 0;
        public string UserCard = "";
        public long UserMoney = 0;
        public int MissionResult = 0;
    }
}
