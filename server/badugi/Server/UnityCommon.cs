using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityCommon
{
    public class Join
    {
        public static readonly string ipaddr = "notuse";
        public static readonly UInt16 portnum = 0;
        public static readonly UInt32 protocol_ver = 0;
    }

    public enum Server : int
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
