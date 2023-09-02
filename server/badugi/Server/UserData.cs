
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static public class Var
    {
        static readonly RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);

        static public RegistryKey getlocalKey()
        {
            return localKey;
        }

        static string RegeditAddress = "SOFTWARE\\WOW6432Node\\1jpot";

        static public void addRegistry(string name, string value)
        {
            var rkey = localKey.OpenSubKey(RegeditAddress);
            if (rkey.GetValue(name) == null)
            {
                Microsoft.Win32.RegistryKey key;
                key = localKey.CreateSubKey(RegeditAddress);
                key.SetValue(name, value);
                key.Close();
            }
        }

        static public void removeRegistry(string keyName)
        {
            using (Microsoft.Win32.RegistryKey key = localKey.OpenSubKey(@"" + keyName, true))
            {
                if (key != null)
                    key.DeleteValue("MyApp");
            }
        }

        static public bool existsRegistry(string name)
        {
            if (localKey == null)
            {

            }
            var rkey = localKey.OpenSubKey(RegeditAddress);
            return rkey.GetValue(name) != null ? true : false;
        }

        static public string getRegistryValue(string name)
        {
            var rkey = localKey.OpenSubKey(RegeditAddress);
            return rkey.GetValue(name).ToString();
        }
    }
    public enum ServerType : int
    {
        None = -1,
        Login,
        Lobby,
        Room,
        Relay,
        RelayLobby,
        Master
    }
}
