using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class CouponViewData
    {
        /// <summary>
        /// 사용한 쿠폰내역
        /// </summary>
        public List<DBLIB.Coupon> items;

        public int pageNo;
        public int startPage;
        public int totalPages;
    }
}