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
        public long BetMoney;
        public DateTime StratTime;
        public DateTime EndTime;
        public CPlayerLog[] PlayerLog;
        public string PlayLog = "";

        public void Reset(long betMoney)
        {
            LogId = Guid.NewGuid();
            BetMoney = betMoney;
            StratTime = DateTime.Now;
            PlayLog = "";
            for (int i = 0; i < PlayerLog.Length; ++i)
                PlayerLog[i] = new CPlayerLog();
        }
        #region PlayLog
        public void Start(int PlayerCount)
        {
            PlayLog += "1," + BetMoney.ToString() + "," + PlayerCount.ToString();
        }
        public void StartHoldem(int PlayerCount, int Shape, int Number)
        {
            string sShape;
            switch (Shape)
            {
                case 0: sShape = "♠"; break;
                case 1: sShape = "◆"; break;
                case 2: sShape = "♥"; break;
                case 3: sShape = "♣"; break;
                default: sShape = "?"; break;
            }

            string sNumber;
            switch (Number)
            {
                case 0: sNumber = "0"; break;
                case 1: sNumber = "1"; break;
                case 2: sNumber = "2"; break;
                case 3: sNumber = "3"; break;
                case 4: sNumber = "4"; break;
                case 5: sNumber = "5"; break;
                case 6: sNumber = "6"; break;
                case 7: sNumber = "7"; break;
                case 8: sNumber = "8"; break;
                case 9: sNumber = "9"; break;
                case 10: sNumber = "t"; break;
                case 11: sNumber = "j"; break;
                case 12: sNumber = "q"; break;
                case 13: sNumber = "k"; break;
                default: sNumber = "?"; break;
            }
            
            PlayLog += "1," + BetMoney.ToString() + "," + PlayerCount.ToString() + "," + sShape + sNumber;
        }
        public void StartGame(int Player, string card, long StartMoney)
        {
            PlayLog += "," + Player.ToString() + "_" + card + "_" + StartMoney.ToString();
        }
        public void StartBetting(GameRound Round, int livePlayerCount, long TotalBetMoney)
        {
            switch (Round)
            {
                case GameRound.START:
                    PlayLog += ";2";
                    break;
                case GameRound.MORNING:
                    PlayLog += ";4";
                    break;
                case GameRound.AFTERNOON:
                    PlayLog += ";6";
                    break;
                case GameRound.EVENING:
                    PlayLog += ";8";
                    break;
                default:
                    PlayLog += ";?";
                    break;
            }

            PlayLog += "," + livePlayerCount.ToString() + "," + TotalBetMoney.ToString();
        }
        public void Betting(int Player, BETTING Bet, long paidMoney, long haveMoney)
        {
            string BetString = "";
            switch (Bet)
            {
                case BETTING.CALL:
                    BetString = "C";
                    break;
                case BETTING.BBING:
                    BetString = "B";
                    break;
                case BETTING.QUATER:
                    BetString = "Q";
                    break;
                case BETTING.HARF:
                    BetString = "H";
                    break;
                case BETTING.DIE:
                    BetString = "D";
                    break;
                case BETTING.CHECK:
                    BetString = "K";
                    break;
                case BETTING.DDADDANG:
                    BetString = "G";
                    break;
                default:
                    BetString = "?";
                    break;
            }
            PlayLog += "," + Player.ToString() + "_" + BetString + "_" + paidMoney + "_" + haveMoney;
        }
        public void StartCardChange(GameRound Round)
        {
            switch (Round)
            {
                case GameRound.MORNING:
                    PlayLog += ";3";
                    break;
                case GameRound.AFTERNOON:
                    PlayLog += ";5";
                    break;
                case GameRound.EVENING:
                    PlayLog += ";7";
                    break;
                default:
                    PlayLog += ";?";
                    break;
            }
        }
        public void CardChange(int player)
        {
            PlayLog += "," + player.ToString() + "_";
        }
        public void CardChange(string card)
        {
            PlayLog += card;
        }
        public void Result()
        {
            PlayLog += ";9";
        }
        public void ResultPlayer(int player, string result, string resultCard, long ChangeMoney)
        {
            string Result_ = "";
            switch (result)
            {
                case "0":
                    Result_ = "M";
                    break;
                case "1":
                    Result_ = "W";
                    break;
                case "2":
                    Result_ = "W";
                    break;
                case "3":
                    Result_ = "L";
                    break;
                case "4":
                    Result_ = "D";
                    break;
                default:
                    Result_ = "?";
                    break;
            }
            PlayLog += "," + player.ToString() + "_" + resultCard + "_" + Result_ + "_" + ChangeMoney.ToString();
        }
        #endregion

        public object End()
        {
            EndTime = DateTime.Now;
            return Clone();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class CPlayerData
    {
        public UserStatus status = UserStatus.None;
        public UserData data = default(UserData);

        public long paidMoney = 0;

        public long ChangeMoney = 0;
        public int GameResult = 0;

        public long GameDealMoney = 0;
        public long JackPotDealMoney = 0;
    }

    public class CPlayerLog
    {
        public int UserId = 0;
        public string UserCard = "";
        public long ChangeMoney = 0;
    }
}
