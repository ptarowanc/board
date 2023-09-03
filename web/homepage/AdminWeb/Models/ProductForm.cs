using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Models
{
    public class ProductForm
    {
        public int? id { get; set; }
        public string ptype { get; set; }
        public string pname { get; set; }
        public HttpPostedFileBase img { get; set; }
        public int value1 { get; set; }
        public string string1 { get; set; }
    }
}