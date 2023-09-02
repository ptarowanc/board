using Server.Engine;
using Simple.Data.Ado;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZNet;

namespace Server.Network
{
    public class GameServer : NetCore
    {
        const string MoneyName = "칩";

        internal object GameServerLocker = new object();
        internal dynamic db = Simple.Data.Database.OpenConnection(Server.Var.getRegistryValue("DBConnectString"));
        Thread DBWorker; // DB 처리 스레드
        ConcurrentQueue<DBLogData> DBQueue;
        bool isRunDB;
        //internal dynamic db = Simple.Data.Database.OpenConnection(Server.Properties.Settings.Default.DBConnectString);
        /// <summary>
        /// 게임 종류
        /// 1 = 바둑이
        /// 2 = 맞고
        /// 3 = 홀덤
        /// </summary>
        public int GameId = 1;
        internal bool ServerMaintenance = false; // 서버 점검(모든 활동 중단)
        internal string ServerMsg;
        internal bool ShutDown = false;
        internal DateTime CountDown;
        internal int tick = 0;
        internal RemoteID MasterHostID = RemoteID.Remote_Master;
        internal ConcurrentDictionary<RemoteID, ServerInfo> ServerInfoList = new ConcurrentDictionary<RemoteID, ServerInfo>();

        public GameServer(BadugiService s, UnityCommon.Server t, ushort portnum, int serverID) : base(s, t, portnum, serverID)
        {
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            param = new StartOption();

            param.m_IpAddressListen = IPPublic;
            param.m_PortListen = PortNum;
            param.m_MaxConnectionCount = 5000;
            param.m_RefreshServerTickMs = 10000;
            param.m_ProtocolVersion = uint.MaxValue;
            param.m_LogicThreadCount = 0;

            NetServer.SetKeepAliveOption(10);

            Log._log.InfoFormat("StartOption. m_IpAddressListen:{0}, m_PortListen:{1}", IPPublic, PortNum);
        }

        protected override void AfterServerStart()
        {
            base.AfterServerStart();

            // 마스터 서버에 연결
            {
                var MasterIP = Var.getRegistryValue("MasterIP");
                var MasterPort = ushort.Parse(Var.getRegistryValue("MasterPort"));
                bool isMaster = NetServer.MasterConnect(MasterIP,
                    MasterPort,
                    this.Name,
                    (int)this.type);

                if (isMaster == false)
                {
                    Environment.Exit(0);
                }

                DBQueue = new ConcurrentQueue<DBLogData>();
                DBWorker = new Thread(() =>
                {
                    DBLogData temp = null;
                    isRunDB = true;
                    while (isRunDB)
                    {
                        if (DBQueue.Count > 0 && DBQueue.TryDequeue(out temp))
                        {
                            try
                            {
                                dynamic PlayerMoney = db.V_PlayerMoney.FindAllById(temp.userID).FirstOrDefault();
                                if (PlayerMoney == null)
                                {
                                    db.LogDetail.Insert(UserId: temp.userID, GameId: GameId, ChanId: temp.chanId, RoomNumber: temp.roomNumber, LogType: (int)temp.logType, LogText: temp.logText);
                                }
                                else
                                {
                                    db.LogDetail.Insert(UserId: temp.userID, GameId: GameId, ChanId: temp.chanId, RoomNumber: temp.roomNumber, LogType: (int)temp.logType, LogText: temp.logText, HaveMoney: PlayerMoney.GameMoney, SafeMoney: PlayerMoney.SafeMoney, HavePoint: PlayerMoney.Point);
                                }
                            }
                            catch (Exception e)
                            {
                                Log._log.FatalFormat("DBLog Error. userID:{0}, chanId:{1}, roomNumber:{2}, logType:{3}, logText:{4}, e:{5}", temp.userID, temp.chanId, temp.roomNumber, temp.logType, temp.logText, e.ToString());
                            }
                        }
                        Thread.Sleep(10);
                    }

                    while (DBQueue.Count > 0)
                    {
                        if (DBQueue.TryDequeue(out temp))
                        {
                            try
                            {
                                dynamic PlayerMoney = db.V_PlayerMoney.FindAllById(temp.userID).FirstOrDefault();
                                if (PlayerMoney == null)
                                {
                                    db.LogDetail.Insert(UserId: temp.userID, GameId: GameId, ChanId: temp.chanId, RoomNumber: temp.roomNumber, LogType: (int)temp.logType, LogText: temp.logText);
                                }
                                else
                                {
                                    db.LogDetail.Insert(UserId: temp.userID, GameId: GameId, ChanId: temp.chanId, RoomNumber: temp.roomNumber, LogType: (int)temp.logType, LogText: temp.logText, HaveMoney: PlayerMoney.GameMoney, SafeMoney: PlayerMoney.SafeMoney, HavePoint: PlayerMoney.Point);
                                }
                            }
                            catch (Exception e)
                            {
                                Log._log.FatalFormat("DBLog Error. userID:{0}, chanId:{1}, roomNumber:{2}, logType:{3}, logText:{4}, e:{5}", temp.userID, temp.chanId, temp.roomNumber, temp.logType, temp.logText, e.ToString());
                            }
                        }
                    }
                });
                DBWorker.Start();
            }
        }

        public override void Stop()
        {
            if (NetServer != null)
            {
                NetServer.MasterDisconnect();
                NetServer.Stop();
            }

            isRunDB = false;
            isNetRun = false;
            ServerTaskTimer.Stop();
            DBWorker.Join();

            if (NetServer != null)
            {
                NetServer.Dispose();
            }
        }

        internal void GetServerInfo(ServerType serverType, out ServerInfo[] serverInfoArray)
        {
            //lock (GameServerLocker) // Bad Lock
            //{
            serverInfoArray = ServerInfoList.Values.Where(x => x.ServerType == serverType).ToArray();
            //}
        }

        #region Static
        public static bool IsExistChnnel(int chanID)
        {
            switch (chanID)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    return true;
            }
            return false;
        }
        public static bool IsExistStakeType(ChannelKind chanKind, int stakeType)
        {
            switch (chanKind)
            {
                case ChannelKind.유료중수채널:
                case ChannelKind.유료고수채널:
                case ChannelKind.무료1채널:
                case ChannelKind.무료2채널:
                case ChannelKind.무료자유1채널:
                case ChannelKind.무료자유2채널:
                    switch (stakeType)
                    {
                        case 1:
                        case 2:
                        case 3:
                            return true;
                    }
                    break;

                case ChannelKind.유료초보채널:
                    switch (stakeType)
                    {
                        case 1:
                            return true;
                    }
                    break;
                case ChannelKind.유료자유채널:
                    switch (stakeType)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            return true;
                    }
                    break;
            }

            return false;
        }
        public static ChannelType GetChannelType(int chanID)
        {
            switch (chanID)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return ChannelType.Charge;
                case 5:
                case 6:
                case 7:
                case 8:
                    return ChannelType.Free;
            }
            return ChannelType.None;
        }
        public static bool GetChannelFree(int chanID)
        {
            switch (chanID)
            {
                case 4:
                case 7:
                case 8:
                    return true;
            }
            return false;
        }
        public static ChannelKind GetChannelKind(int chanID)
        {
            switch (chanID)
            {
                case 1:
                    return ChannelKind.유료초보채널;
                case 2:
                    return ChannelKind.유료중수채널;
                case 3:
                    return ChannelKind.유료고수채널;
                case 4:
                    return ChannelKind.유료자유채널;
                case 5:
                    return ChannelKind.무료1채널;
                case 6:
                    return ChannelKind.무료2채널;
                case 7:
                    return ChannelKind.무료자유1채널;
                case 8:
                    return ChannelKind.무료자유2채널;
            }
            return ChannelKind.None;
        }
        public static int GetStakeMoney(ChannelKind chanKind, int stakeType)
        {
            switch (chanKind)
            {
                case ChannelKind.유료초보채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100;
                        }
                    }
                    break;
                case ChannelKind.유료중수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100000;
                            case 2:
                                return 1000000;
                            case 3:
                                return 10000000;
                            case 4:
                                return 30000000;
                            case 5:
                                return 100000000;
                        }
                    }
                    break;
                case ChannelKind.유료고수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 30000000;
                            case 2:
                                return 100000000;
                            case 3:
                                return 300000000;
                            case 4:
                                return 500000000;
                            case 5:
                                return 1000000000;
                        }
                    }
                    break;
                case ChannelKind.유료자유채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100;
                            case 2:
                                return 100000;
                            case 3:
                                return 1000000;
                            case 4:
                                return 10000000;
                            case 5:
                                return 30000000;
                            case 6:
                                return 100000000;
                            case 7:
                                return 300000000;
                            case 8:
                                return 500000000;
                            case 9:
                                return 1000000000;
                        }
                    }
                    break;
                case ChannelKind.무료1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 200;
                            case 2:
                                return 300;
                            case 3:
                                return 500;
                            case 4:
                                return 500;
                            case 5:
                                return 500;
                        }
                    }
                    break;
                case ChannelKind.무료2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 1000;
                            case 2:
                                return 2000;
                            case 3:
                                return 5000;
                            case 4:
                                return 5000;
                            case 5:
                                return 5000;
                        }
                    }
                    break;
                case ChannelKind.무료자유1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 1;
                            case 2:
                                return 5;
                            case 3:
                                return 10;
                            case 4:
                                return 10;
                            case 5:
                                return 10;
                        }
                    }
                    break;
                case ChannelKind.무료자유2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 1;
                            case 2:
                                return 5;
                            case 3:
                                return 10;
                            case 4:
                                return 10;
                            case 5:
                                return 10;
                        }
                    }
                    break;
            }

            return 0;
        }
        public static long GetMinimumMoneyLeave(ChannelKind chanKind, int stakeType)
        {
            long multiple = 10;
            switch (chanKind)
            {
                case ChannelKind.유료초보채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100 * multiple;
                        }
                    }
                    break;
                case ChannelKind.유료중수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100000 * multiple;
                            case 2:
                                return 1000000 * multiple;
                            case 3:
                                return 10000000 * multiple;
                            case 4:
                                return 30000000 * multiple;
                            case 5:
                                return 100000000 * multiple;
                        }
                    }
                    break;
                case ChannelKind.유료고수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 30000000 * multiple;
                            case 2:
                                return 100000000 * multiple;
                            case 3:
                                return 300000000 * multiple;
                            case 4:
                                return 500000000 * multiple;
                            case 5:
                                return 1000000000 * multiple;
                        }
                    }
                    break;
                case ChannelKind.유료자유채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100 * multiple;
                            case 2:
                                return 100000 * multiple;
                            case 3:
                                return 1000000 * multiple;
                            case 4:
                                return 10000000 * multiple;
                            case 5:
                                return 30000000 * multiple;
                            case 6:
                                return 100000000 * multiple;
                            case 7:
                                return 300000000 * multiple;
                            case 8:
                                return 500000000 * multiple;
                            case 9:
                                return 1000000000 * multiple;
                        }
                    }
                    break;
                case ChannelKind.무료1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 200 * multiple;
                            case 2:
                                return 300 * multiple;
                            case 3:
                                return 500 * multiple;
                            case 4:
                                return 500 * multiple;
                            case 5:
                                return 500 * multiple;
                        }
                    }
                    break;
                case ChannelKind.무료2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 1000 * multiple;
                            case 2:
                                return 2000 * multiple;
                            case 3:
                                return 5000 * multiple;
                            case 4:
                                return 5000 * multiple;
                            case 5:
                                return 5000 * multiple;
                        }
                    }
                    break;
                case ChannelKind.무료자유1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 1 * multiple;
                            case 2:
                                return 5 * multiple;
                            case 3:
                                return 10 * multiple;
                            case 4:
                                return 10 * multiple;
                            case 5:
                                return 10 * multiple;
                        }
                    }
                    break;
                case ChannelKind.무료자유2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 1 * multiple;
                            case 2:
                                return 5 * multiple;
                            case 3:
                                return 10 * multiple;
                            case 4:
                                return 10 * multiple;
                            case 5:
                                return 10 * multiple;
                        }
                    }
                    break;
            }

            return 0;
        }
        public static long GetMinimumMoney(ChannelKind chanKind, int stakeType)
        {
            long multiple = 30;
            switch (chanKind)
            {
                case ChannelKind.유료초보채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100 * multiple;
                        }
                    }
                    break;
                case ChannelKind.유료중수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100000 * multiple;
                            case 2:
                                return 1000000 * multiple;
                            case 3:
                                return 10000000 * multiple;
                            case 4:
                                return 30000000 * multiple;
                            case 5:
                                return 100000000 * multiple;
                        }
                    }
                    break;
                case ChannelKind.유료고수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 30000000 * multiple;
                            case 2:
                                return 100000000 * multiple;
                            case 3:
                                return 300000000 * multiple;
                            case 4:
                                return 500000000 * multiple;
                            case 5:
                                return 1000000000 * multiple;
                        }
                    }
                    break;
                case ChannelKind.유료자유채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100 * multiple;
                            case 2:
                                return 100000 * multiple;
                            case 3:
                                return 1000000 * multiple;
                            case 4:
                                return 10000000 * multiple;
                            case 5:
                                return 30000000 * multiple;
                            case 6:
                                return 100000000 * multiple;
                            case 7:
                                return 300000000 * multiple;
                            case 8:
                                return 500000000 * multiple;
                            case 9:
                                return 1000000000 * multiple;
                        }
                    }
                    break;
                case ChannelKind.무료1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 20000;
                            case 2:
                                return 30000;
                            case 3:
                                return 50000;
                            case 4:
                                return 50000;
                            case 5:
                                return 50000;
                        }
                    }
                    break;
                case ChannelKind.무료2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100000;
                            case 2:
                                return 200000;
                            case 3:
                                return 500000;
                            case 4:
                                return 500000;
                            case 5:
                                return 500000;
                        }
                    }
                    break;
                case ChannelKind.무료자유1채널:
                case ChannelKind.무료자유2채널:
                    {
                        return 1;
                    }
            }

            return 0;
        }
        public static long GetMaximumMoney(ChannelKind chanKind, int stakeType)
        {
            switch (chanKind)
            {
                case ChannelKind.유료초보채널:
                case ChannelKind.유료중수채널:
                case ChannelKind.유료고수채널:
                case ChannelKind.유료자유채널:
                    {
                        return long.MaxValue;
                    }
                case ChannelKind.무료1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 100000;
                            case 2:
                                return 150000;
                            case 3:
                                return 250000;
                            case 4:
                                return long.MaxValue;
                            case 5:
                                return long.MaxValue;
                        }
                    }
                    break;
                case ChannelKind.무료2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return 500000;
                            case 2:
                                return 1000000;
                            case 3:
                                return 2500000;
                            case 4:
                                return long.MaxValue;
                            case 5:
                                return long.MaxValue;
                        }
                    }
                    break;
                case ChannelKind.무료자유1채널:
                case ChannelKind.무료자유2채널:
                    {
                        return long.MaxValue;
                    }
            }

            return 0;
        }
        public static string GetMinimumMoneyText(ChannelKind chanKind, int stakeType)
        {
            switch (chanKind)
            {
                case ChannelKind.유료초보채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "3000" + MoneyName;
                        }
                    }
                    break;
                case ChannelKind.유료중수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "300만" + MoneyName;
                            case 2:
                                return "3천만" + MoneyName;
                            case 3:
                                return "3억" + MoneyName;
                            case 4:
                                return "9억" + MoneyName;
                            case 5:
                                return "30억" + MoneyName;
                        }
                    }
                    break;
                case ChannelKind.유료고수채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "9억" + MoneyName;
                            case 2:
                                return "30억" + MoneyName;
                            case 3:
                                return "90억" + MoneyName;
                            case 4:
                                return "150억" + MoneyName;
                            case 5:
                                return "300억" + MoneyName;
                        }
                    }
                    break;
                case ChannelKind.유료자유채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "3천" + MoneyName;
                            case 2:
                                return "3백만" + MoneyName;
                            case 3:
                                return "3천만" + MoneyName;
                            case 4:
                                return "3억" + MoneyName;
                            case 5:
                                return "9억" + MoneyName;
                            case 6:
                                return "30억" + MoneyName;
                            case 7:
                                return "90억" + MoneyName;
                            case 8:
                                return "150억" + MoneyName;
                            case 9:
                                return "300억" + MoneyName;
                        }
                    }
                    break;
                case ChannelKind.무료1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "2만" + MoneyName;
                            case 2:
                                return "3만" + MoneyName;
                            case 3:
                                return "5만" + MoneyName;
                            case 4:
                                return "6만" + MoneyName;
                            case 5:
                                return "15만" + MoneyName;
                        }
                    }
                    break;
                case ChannelKind.무료2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "10만" + MoneyName;
                            case 2:
                                return "20만" + MoneyName;
                            case 3:
                                return "50만" + MoneyName;
                            case 4:
                                return "15만" + MoneyName;
                            case 5:
                                return "30만" + MoneyName;
                        }
                    }
                    break;
                case ChannelKind.무료자유1채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "1" + MoneyName;
                            case 2:
                                return "1" + MoneyName;
                            case 3:
                                return "1" + MoneyName;
                            case 4:
                                return "1만5천" + MoneyName;
                            case 5:
                                return "3만" + MoneyName;
                        }
                    }
                    break;
                case ChannelKind.무료자유2채널:
                    {
                        switch (stakeType)
                        {
                            case 1:
                                return "30" + MoneyName;
                            case 2:
                                return "3천" + MoneyName;
                            case 3:
                                return "9천" + MoneyName;
                            case 4:
                                return "1만5천" + MoneyName;
                            case 5:
                                return "3만" + MoneyName;
                        }
                    }
                    break;
            }

            return "0";
        }
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString().ToUpper();
        }

        public struct ServerMessage
        {
            public int type;
            public int value1;
            public int value2;
            public int value3;
            public int value4;
            public int value5;
            public string string1;
            public string string2;
        }
        #endregion

        public void DBLog(int userID, int chanId, int roomNumber, LOG_TYPE logType, string logText)
        {
            DBQueue.Enqueue(new DBLogData(userID, chanId, roomNumber, logType, logText));
        }
    }
    public class DBLogData
    {
        public int userID;
        public int chanId;
        public int roomNumber;
        public LOG_TYPE logType;
        public string logText;

        public DBLogData(int userID_, int chanId_, int roomNumber_, LOG_TYPE logType_, string logText_)
        {
            userID = userID_;
            chanId = chanId_;
            roomNumber = roomNumber_;
            logType = logType_;
            logText = logText_;
        }
    }

    enum DB_COMMAND
    {
        BADUGI_PUSH = 1,
        BADUGI_PUSH_BASE = 2,
        BADUGI_PUSH_USER = 3,
        BADUGI_PUSH_GAME = 4,
        BADUGI_JJACKPOT = 5,
        BADUGI_MADE_BONUS = 7,
        BADUGI_MILEAGE = 8,
        BADUGI_MADE_LIMIT = 9,
        BADUGI_PUSH_GAME2 = 15,
        BADUGI_BBINGCUT = 16,

        MATGO_JACKPOT = 6,
        MATGO_PUSH = 11,
        MATGO_PUSH_BASE = 12,
        MATGO_PUSH_USER = 13,
        MATGO_PUSH_GAME = 14,
        MATGO_MISSION = 16
    }

    public enum LOG_TYPE
    {
        로그인,
        로그아웃,
        채널이동,
        정상종료,
        강제종료,
        연결끊김,
        방만들기,
        방입장,
        방퇴장,
        방이동,
        금고적립금전환,
        금고입금,
        금고출금,
        비정상종료,
        자동치기,
        행운의복권,
    }
}
