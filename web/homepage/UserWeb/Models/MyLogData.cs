using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class MyLogData
    {
        public List<DBLIB.V_LogDetail> items;

        public int pageNo;
        public int startPage;
        public int totalPages;
    }
}