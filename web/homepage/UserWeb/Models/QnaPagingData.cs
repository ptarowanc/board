using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class QnaPagingData
    {
        public int PageNo;
        public int TotalPages;
        public int StartPage;
        public List<DBLIB.qna> List;
    }
}