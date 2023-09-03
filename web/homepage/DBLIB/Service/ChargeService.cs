using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;

namespace DBLIB.Service
{
    public class ChargeService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProductService));

        readonly GameEntities vongGameDb;

        public ChargeService(GameEntities vongGameEntities)
        {
            this.vongGameDb = vongGameEntities;
        }

        public int GetCurrentAvailableCash(string userId)
        {
            // TODO :: 보유중인 캐쉬
            return 0;
        }

        /// <summary>
        /// 잔여 적립금(Mileage)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public long GetAvailMileage(string userId)
        {
            var entry = vongGameDb.Player.Where(a => a.UserID == userId).FirstOrDefault();
            return entry != null ? entry.Point : 0;
        }

        /// <summary>
        /// 잔여 적립금(Mileage)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public long GetAvailMileage2(string userId)
        {
            var entry = vongGameDb.Player.Where(a => a.UserID == userId).FirstOrDefault();

            if(entry != null)
            {
                var entry2 = vongGameDb.AdminUser.Where(a => a.Id == entry.FriendId).FirstOrDefault();
                if(entry2 != null)
                {
                    if(entry2.Type == 6)
                    {
                        return entry2.PointSettlement;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 적립금 전환 기록 조회
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<DBLIB.LogExchangeMileage> GetMileageExchangeHistory(string userid, int pageNo, int pageSize)
        {
            return vongGameDb.LogExchangeMileage.Where(a => a.Player.UserID == userid).OrderByDescending(a => a.Date).Skip(Math.Max((pageNo-1),0)* pageSize).Take(pageSize).ToList();
        }

        public int GetMileageExchangeHistoryCount(string userid)
        {
            return vongGameDb.LogExchangeMileage.Where(a => a.Player.UserID == userid).Count();
        }

        public bool ExchangeMileage(string userId, long mileage, out string message)
        {
            int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();

            var param = new ObjectParameter("Out_Result", typeof(int));
            vongGameDb.SP_PlayerMileageExchange(id, mileage, param);

            int? responseCode = (byte?)param.Value;
            if (param.Value is DBNull)
                throw new Exception("서버 오류로 요청을 처리하지 못했습니다.");

            bool succeeded = responseCode.Value == 0;
            message = responseCode.Value == 0 ? "정상 처리되었습니다." :
                        responseCode.Value == 1 ? "비정상 요청으로 오류가 발생했습니다." :
                        responseCode.Value == 2 ? "100 마일리지 이하는 전환할 수 없습니다." :
                        responseCode.Value == 3 ? "마일리지가 부족합니다." :
                        responseCode.Value == 4 ? "게임 중에는 전환할 수 없습니다." : "(미지정 에러코드" + responseCode + ")";

            return succeeded;
        }


        /// <summary>
        /// 쿠폰 기록 조회
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<DBLIB.Coupon> GetCouponHistory(string userId, int pageNo, int pageSize)
        {
            int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();
            return vongGameDb.Coupon.Where(a => a.UsePlayer == id).OrderByDescending(a => a.UseDate).Skip(Math.Max((pageNo - 1), 0) * pageSize).Take(pageSize).ToList();
        }

        public int GetCouponHistoryCount(string userId)
        {
            int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();
            return vongGameDb.Coupon.Where(a => a.UsePlayer == id).Count();
        }

        public bool UseCoupon(string userId, string coupon, out string message)
        {
            int id = vongGameDb.Player.Where(a => a.UserID == userId).Select(a => a.Id).FirstOrDefault();

            var param = new ObjectParameter("Out_Result", typeof(int));

            vongGameDb.SP_PlayerUseCoupon(id, coupon, param);

            int? responseCode = (byte?)param.Value;
            if (param.Value is DBNull)
                throw new Exception("서버 오류로 요청을 처리하지 못했습니다.");

            bool succeeded = responseCode.Value == 0;
            message = responseCode.Value == 0 ? "쿠폰 사용 완료" :
                        responseCode.Value == 1 ? "비정상 요청으로 오류가 발생했습니다." :
                        responseCode.Value == 2 ? "유효하지 않은 쿠폰 일련번호입니다." :
                        responseCode.Value == 3 ? "이미 사용된 쿠폰입니다." :
                        responseCode.Value == 4 ? "잠시 후 다시 시도하시기 바랍니다." : "(미지정 에러코드" + responseCode + ")";

            return succeeded;
        }
    }
}