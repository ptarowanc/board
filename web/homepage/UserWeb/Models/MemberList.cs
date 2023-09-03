using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserWeb.Models
{
    public class MemberListData
    {
        public string date;
    }
    
    public class MemberUserListData
    {
        //public long availPoint;

        public List<DBLIB.V_MemberUserList> items;

        public int pageNo;
        public int startPage;
        public int totalPages;
    }

    public class MemberBonusListData
    {
        //public long availPoint;

        public List<DBLIB.V_MemberFriendList> items;

        public int pageNo;
        public int startPage;
        public int totalPages;
    }

}