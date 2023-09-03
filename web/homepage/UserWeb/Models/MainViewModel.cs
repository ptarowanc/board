using DBLIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class MainViewModel
    {
        public string popupNotice;
        public WebNoticeList[] noticeList;
        public bool IsLogon;
    }
}