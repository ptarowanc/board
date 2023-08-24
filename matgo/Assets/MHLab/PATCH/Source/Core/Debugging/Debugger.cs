using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MHLab.PATCH.Settings;
using System.IO;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace MHLab.PATCH.Debugging
{
    public sealed class Debugger
    {
        public static void Log(string message)
        {
            string log = "[P.A.T.C.H. - " + DateTime.Now.ToString() + "] " + message;
            
#if UNITY_EDITOR
            Debug.Log(log);
#else
            CheckLogsDirectory();
            using (StreamWriter w = new StreamWriter(SettingsManager.LOGS_ERROR_PATH, true))
            {
                w.Write(log + "\r\n");
            }
#endif
        }
#if !UNITY_EDITOR
        private static bool CheckLogsDirectory()
        {
            try
            {
                if (!FileManager.DirectoryExists(SettingsManager.LOGS_ERROR_PATH))
                {
                    FileManager.CreateDirectory(Path.GetDirectoryName(SettingsManager.LOGS_ERROR_PATH));
                }
                return true;
            }
            catch (Exception e)
            {
                //Debugging.Log(e.Message);
                return false;
            }
        }
#endif
    }
}
