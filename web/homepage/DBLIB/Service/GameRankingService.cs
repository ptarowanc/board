using DBLIB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class GameRankingService
    {
        readonly GameEntities vongGameDb;

        public GameRankingService(GameEntities db)
        {
            this.vongGameDb = db;
        }

        /// <summary>
        /// 지정된 게임별 랭킹 목록을 반환한다
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        public GameRankEntry[] GetRankingList(GameTypeEnum gameType, int rows)
        {
            if(gameType == GameTypeEnum.Baduki)
            {
                return vongGameDb.V_WEB_RankingBadugiBetting.Take(10)
                            .Select(a => new GameRankEntry()
                            {
                                rank = (int)a.ID,
                                betting = (long)(a.TotalBetMoney.HasValue ? a.TotalBetMoney.Value : 0),
                                nickname = a.NickName
                            }).ToArray();
            }else if(gameType == GameTypeEnum.Matgo)
            {
                return vongGameDb.V_WEB_RankingMatgoBetting.Take(10)
                            .Select(a => new GameRankEntry()
                            {
                                rank = (int)a.ID,
                                betting = (long)(a.TotalBetMoney.HasValue ? a.TotalBetMoney.Value : 0),
                                nickname = a.NickName
                            }).ToArray();

            }
            return null;
        }
    }
}
