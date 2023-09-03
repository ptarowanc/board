using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class NoticeViewListData
    {
        public DBLIB.WebNoticeList[] webNoticeList;
  //      public int? pageSize;
        public int startPage;
        public int pageNo;
//        public int? pageSet;
//        public int totalRecords;
        public int totalPages;
    }
}