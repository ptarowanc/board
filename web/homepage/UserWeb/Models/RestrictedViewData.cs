using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class RestrictedViewData
    {
        /// <summary>
        /// 이용제한 자가 신청
        /// </summary>
        public DateTime restrictedTime;
        public bool isRestricted;
    }
}