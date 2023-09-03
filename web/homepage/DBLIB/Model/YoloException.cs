using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Model
{
    public class YoloException : Exception
    {
        public YoloException(string message)
            : base(message)
        {
        }
    }
}
