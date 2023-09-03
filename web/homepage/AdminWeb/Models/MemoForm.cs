using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Models
{
    public class MemoForm
    {
        public int? id { get; set; }
        public string userId { get; set; }
        public string message { get; set; }
    }
}