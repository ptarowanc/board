using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Models
{
    public class CounterResponse : DBLIB.Model.StandardResult
    {
        public int wait;
        public int total;
        public int unread;
    }
}