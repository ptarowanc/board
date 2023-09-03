using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWeb.Models.DataTables
{
    [Serializable()]
    public class DataTableRequestParameter
    {
        public int draw { get; set; }
        public int length { get; set; }
        public int start { get; set; }
        public List<DataTableColumm> columns { get; set; }
        public List<OrderEntry> order { get; set; }
        public SearchValue search { get; set; }
    }
    
    [Serializable]
    public class OrderEntry
    {
        public int column { get; set; }
        public DBLIB.Service.OrderDirectionEnum dir { get; set; }
    }

    [Serializable()]
    public class DataTableColumm
    {
        public string data { get; set; }
        public string name { get; set; }
        public Boolean searchable { get; set; }
        public Boolean orderable { get; set; }
        public SearchValue Search { get; set; }
    }

    [Serializable()]
    public class SearchValue
    {
        public string value { get; set; }
        public Boolean regex { get; set; }
    }
}