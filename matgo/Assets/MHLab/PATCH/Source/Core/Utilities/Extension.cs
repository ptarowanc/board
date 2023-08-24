﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MHLab.PATCH.Utilities
{
    static class Extension
    {
        public static void CopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024]; 
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }
}
