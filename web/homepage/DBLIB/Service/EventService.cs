using DBLIB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class EventService
    {
        readonly GameEntities vongGameDb;

        public EventService(GameEntities db)
        {
            this.vongGameDb = db;
        }

        // 가지고 있는 부적 수
        public int GetUserEventLottoCount(string userId)
        {
            try
            {
                int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();
                return vongGameDb.EventLotto.Where(a => a.UserId == id).Count();
            }
            catch
            {
                return 0;
            }
        }

        // 응모한 부적 목록
        public List<EventLottoEnter> GetUserEventLottoEnterList(string userId, int pageNo, int pageSize)
        {
            try
            {
                int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();
                return vongGameDb.EventLottoEnter.Where(a => a.UserId == id).OrderByDescending(a => a.Date).Skip(Math.Max((pageNo - 1), 0) * pageSize).Take(pageSize).ToList();
            }
            catch
            {
                return null;
            }
        }

        // 응모한 부적 페이지 수
        public int GetTotalPageCountEventLottoEnter(string userId, int pageSize)
        {
            return (int)Math.Ceiling((double)vongGameDb.EventLottoEnter.Where(a => a.Player.UserID == userId).Count() / (double)pageSize);
        }

        // 진행중인 이벤트 
        public EventLottoBase GetEventLotto()
        {
            try
            {
                return vongGameDb.EventLottoBase.Where(a => a.EventEnd == false).OrderByDescending(a => a.EventNumber).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        
        // 이벤트 결과
        public EventLottoResult GetEventLottoResultList(int eventNumber = 0)
        {
            try
            {
                if (eventNumber == 0)
                {
                    return vongGameDb.EventLottoResult.OrderByDescending(a => a.EventNumber).FirstOrDefault();
                }
                else
                {
                    return vongGameDb.EventLottoResult.Where(a => a.EventNumber == eventNumber).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        // 역대 결과수
        public int GetEventLottoResultCount()
        {
            try
            {
                return vongGameDb.EventLottoResult.Count();
            }
            catch
            {
                return 0;
            }
        }

        public bool EventLottoEnter(string userId, int number0, int number1, int number2, int number3, int number4, int number5, out string message)
        {
            int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();

            var Out_Result = new ObjectParameter("Out_Result", typeof(int));

            try
            {
                var procResult = vongGameDb.SP_PlayerEventLottoEnter(id, number0, number1, number2, number3, number4, number5, Out_Result);
                procResult.ToList();
            }
            catch (Exception e)
            {
                int a = 0;
            }

            //var result = 
            //result.ToList();

            if (Out_Result.Value is DBNull)
                throw new Exception("서버 오류로 요청을 처리하지 못했습니다.");
            int? responseCode = (int?)Out_Result.Value;

            bool succeeded = responseCode.Value == 0;
            message = responseCode.Value == 0 ? "응모되었습니다." :
                        responseCode.Value == 1 ? "비정상 요청으로 오류가 발생했습니다." :
                        responseCode.Value == 2 ? "부적이 부족합니다." :
                        responseCode.Value == 3 ? "올바른 숫자가 아닙니다." :
                        responseCode.Value == 4 ? "중복된 숫자입니다." :
                        responseCode.Value == 5 ? "이미 응모한 숫자입니다." :
                        responseCode.Value == 6 ? "응모 기간이 지났습니다." : "서버 오류로 요청을 처리하지 못했습니다.";

            return succeeded;
        }
    }
}
