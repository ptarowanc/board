using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBLIB.Model
{
    public class StandardResult
    {
        public string result;
        public string reason;
        public object data;

        public static StandardResult createError(string result, string reason)
        {
            StandardResult res = new StandardResult();
            res.result = result;
            res.reason = reason;
            return res;
        }
        public static StandardResult createError(string reason)
        {
            StandardResult res = new StandardResult();
            res.result = "ERROR";
            res.reason = reason;
            return res;
        }

        public static StandardResult createSucceeded(string reason = "")
        {
            StandardResult res = new StandardResult();
            res.result = "OK";
            res.reason = reason;
            return res;
        }
    }
}