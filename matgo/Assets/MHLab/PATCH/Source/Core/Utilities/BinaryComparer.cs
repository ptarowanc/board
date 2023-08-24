using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MHLab.PATCH.Utilities
{
    internal class BinaryComparer
    {
        public static bool CompareArray(byte[] strA, byte[] strB)
        {
            int length = strA.Length;
            if (length != strB.Length)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (strA[i] != strB[i]) return false;
            }
            return true;
        }
    }
}
