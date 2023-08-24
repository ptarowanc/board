using System;

namespace CommonBadugi
{
    /// <summary>
    /// 서버 종류 (클라이언트 기준)
    /// </summary>
    public enum ServerType : int
    {
        None = -1,
        Login,
        Lobby,
        Room,
    }
    public enum StartType : int
    {
        None = 0,
        Free,
        Paid,
    }
}
