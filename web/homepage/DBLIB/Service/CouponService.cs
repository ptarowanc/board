using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{
    public class CouponService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CouponService));

        readonly GameEntities vongGameDb;
        public CouponService(GameEntities vongGameEntities)
        {
            this.vongGameDb = vongGameEntities;
        }

        public int GetAllCouponCount()
        {
            return vongGameDb.Coupon.Count();
        }
        public int GetAllCouponLogCount()
        {
            return vongGameDb.V_LogCoupon.Count();
        }

        public List<Coupon> GetCouponList(int start, int length, int sortField, OrderDirectionEnum sortOrder, string search)
        {
            IQueryable<Coupon> list = vongGameDb.Coupon;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(a => a.Serial.Contains(search));

            if (sortField == 0)
                if (sortOrder == OrderDirectionEnum.asc)
                    list = list.OrderBy(a => a.Id);
                else
                    list = list.OrderByDescending(a => a.Id);
            else
                if (sortOrder == OrderDirectionEnum.asc)
                list = list.OrderBy(a => a.Serial);
            else
                list = list.OrderByDescending(a => a.Serial);

            list = list.Skip(start).Take(length);
            return list.ToList();
        }
        public List<V_LogCoupon> GetCouponLogList(int start, int length, int sortField, OrderDirectionEnum sortOrder, string search)
        {
            IQueryable<V_LogCoupon> list = vongGameDb.V_LogCoupon;
            if (!string.IsNullOrEmpty(search))
                list = list.Where(a => a.Serial.Contains(search) || a.PlayerID.Contains(search) || a.NickName.Contains(search));

            if (sortField == 0)
                if (sortOrder == OrderDirectionEnum.asc)
                    list = list.OrderBy(a => a.Id);
                else
                    list = list.OrderByDescending(a => a.Id);
            else
                if (sortOrder == OrderDirectionEnum.asc)
                list = list.OrderBy(a => a.Serial);
            else
                list = list.OrderByDescending(a => a.Serial);

            list = list.Skip(start).Take(length);
            return list.ToList();
        }
    }
}
