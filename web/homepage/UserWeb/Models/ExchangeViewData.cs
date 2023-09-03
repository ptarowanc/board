using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class ExchangeViewData
    {
        /// <summary>
        /// 전환할 적립금
        /// </summary>
        public long availPoint;
        public long availPoint2;

        public List<DBLIB.LogExchangeMileage> items;

        public int pageNo;
        public int startPage;
        public int totalPages;
    }
}