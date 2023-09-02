using Server.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityCommon;
using ZNet;

namespace Server.User
{
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

        MATGO_JACKPOT = 6,
        MATGO_PUSH = 11,
        MATGO_PUSH_BASE = 12,
        MATGO_PUSH_USER = 13,
        MATGO_PUSH_GAME = 14
    }

    public class UserServer : Base.BaseServer
    {
        public int GameId = 2;  // 맞고
        public bool ServerMaintenance = false; // 서버 점검(모든 활동 중단)
        public string ServerMsg;
        public bool ShutDown = false;
        public DateTime CountDown;
        public bool Froce = false;

        //클라이언트 목록
        // public Dictionary<ZNet.RemoteID, CPlayer> RemoteClients = new Dictionary<ZNet.RemoteID, CPlayer>();
        public UserServer(FormServer f, UnityCommon.Server t, int portnum) : base(f, t, portnum)
        {
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            param = new StartOption();
            param.m_IpAddressListen = ListenAddr.m_ip;
            param.m_PortListen = ListenAddr.m_port;
            //접속을 받을 최대동접 수:
            //작게 잡을 경우 동적으로 동접 한계치에 가까워지면 최대 동접 수가 늘어남
            //동적접속수 증가를 비활성화 할 수도 있다. m_bExpandMaxConnect옵션을 false로 지정하면 된다.
            param.m_MaxConnectionCount = 50000;
            //주기적으로 서버에서 처리할 콜백 함수 시간을 설정
            param.m_RefreshServerTickMs = 10000;
            //서버와 클라이언트간의 프로토콜 버전, 서러 다른 경우 경고 메시지 이벤트가 발생
            param.m_ProtocolVersion = Join.protocol_ver;
            //내부 쓰레드 사용 중단
            param.m_LogicThreadCount = 0;
            //param.m_bAutoLimitConnection = false;


            //클라이언트의 반응이 없을 경우 내부적으로 접속을 해제시킬 시간 설정(초단위)
            m_Core.SetKeepAliveOption(10);

            //클라이언트의 연결이 이루어지는 경우 발생되는 이벤트 처리
            //m_Core.client_join_handler = ClientJoin;
            //클라이언트의 연결이 종료되는 경우 발생되는 이벤트 처리
            //m_Core.client_leave_handler = ClientLeave;

            //서버이동 도중에 이전서버에 퇴장은 되었으나 새로운 서버로 입장을 실패하는 경우(회선문제 등으로)
            // --> 이때 확실히 로그아웃 처리를 마무리해줄 필요가 있음
            //m_Core.move_server_failed_handler = MoveServerFailed;
            //어떤 유저가 서버 이동을 시작한 경우 발생하는 이벤트
            //m_Core.move_server_start_handler = MoveServerStart;

            //마스터 서버에 입장 성공한 이벤트
            //m_Core.server_master_join_handler = ServerMasterJoin;
            //마스터 서버에 퇴장된 이벤트
            //m_Core.server_master_leave_handler = ServerMasterLeave;
            //마스터 서버에 연결된 모든 서버들로부터 주기적으로 자동 받게되는 정보
            //m_Core.server_refresh_handler = ServerRefresh;

        }

        #region Handlers
        /*
        void ClientJoin(ZNet.RemoteID remote, ZNet.NetAddress addr, ZNet.ArrByte move_server, ZNet.ArrByte move_param)
        {
            if (move_server.Count > 0)
            {
                CPlayer user;
                Common.Common.ServerMoveComplete(move_server, out user);

                form.printf("Move Server Complete {0} {1} {2} {3}", user.data.userID, user.data.money_cash, user.data.money_game, user.data.temp);
                RemoteClients.Add(remote, user);
            }
            else
            {
                if (this.Type == UnityCommon.Server.Login)
                {
                    // 로그인 서버에서만 일반입장을 허용
                    CPlayer rc = new CPlayer();
                    rc.data.userID = Guid.Empty;
                    rc.data.temp = "최초입장_인증받기전";
                    RemoteClients.Add(remote, rc);
                }
                else
                {
                    form.printf("로그인 서버외의 서버에서 일반 입장은 허용하지 않습니다");
                }
            }

            form.printf("Client {0} is Join {1}:{2}. Current={3}\n", remote, addr.m_ip, addr.m_port, RemoteClients.Count);
        }
        void ClientLeave(ZNet.RemoteID remote, bool bMoveServer)
        {
            // 서버 이동중이 아닌상태에서 퇴장하는 경우 로그아웃에 대한 처리를 해줍니다
            if (bMoveServer == false)
            {
                CPlayer rc;
                if (RemoteClients.TryGetValue(remote, out rc))
                {
                    form.printf("[DB로그아웃] 처리, user {0} {1}\n", rc.data.userName, remote);
                    if (Var.Use_DB)
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                Simple.Data.Database.Open().PlayerInfo.UpdateByUserUUID(UserUUID: rc.data.userID, StateOnline: false);
                            }
                            catch (Exception e)
                            {
                                form.printf("[DB로그아웃] 예외발생 {0}\n", e.ToString());
                            }
                        });
                    }
                }
            }

            RemoteClients.Remove(remote);
            form.printf("Client {0} Leave\n", remote);
        }
        /*
        void MoveServerFailed(ZNet.ArrByte move_server)
        {
            NetCommon.CUser rc;
            NetCommon.Common.ServerMoveComplete(move_server, out rc);

            form.printf("[DB로그아웃] 서버이동을 실패한 경우에 대한 마무리 처리, user {0}\n", rc.data.userName);
            if (Var.Use_DB)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Simple.Data.Database.Open().PlayerInfo.UpdateByUserUUID(UserUUID: rc.data.userID, StateOnline: false);
                    }
                    catch (Exception e)
                    {
                        form.printf("[DB로그아웃] 예외발생 {0}\n", e.ToString());
                    }
                });
            }
        }
        void MoveServerStart(ZNet.RemoteID remote, out ZNet.ArrByte buffer)
        {
            // 해당 유저의 유효성 체크
            NetCommon.CUser rc;
            if (RemoteClients.TryGetValue(remote, out rc) == false)
            {
                buffer = null;
                return;
            }

            // 인증여부 확인
            if (rc.joined == false)
            {
                buffer = null;
                return;
            }

            // 데이터 이동 완료시 목표 서버에서 정상적인 데이터인지 확인을 위한 임시 데이터 구성
            rc.data.temp = this.Name;

            // 동기화 할 유저 데이터를 구성하여 buffer에 넣어둔다 -> 이동 목표 서버에서 해당 데이터를 그대로 받게된다
            NetCommon.Common.ServerMoveStart(rc, out buffer);

            form.printf("move server start  {0} {1} {2} {3}", rc.data.userID, rc.data.money_cash, rc.data.money_game, rc.data.temp);
        }
        void ServerMasterJoin(ZNet.RemoteID remote, ZNet.RemoteID myRemoteID)
        {
            form.printf(string.Format("마스터서버에 입장성공 remoteID {0}", myRemoteID));
        }
        void ServerMasterLeave()
        {
            form.printf(string.Format("마스터서버와 연결종료!!!"));
            form.Close();    // 마스터 서버를 종료하면 모든 서버 프로그램이 자동 종료처리 되게 하는 내용...
        }
        void ServerRefresh(ZNet.MasterInfo master_info)
        {
            //form.printf(string.Format("서버P2P remote:{0} type:{1}[{2}] current:{3} addr:{4}:{5}",

            //    // 정보를 보낸 서버의 remoteID
            //    master_info.m_remote,

            //    // 정보를 보낸 서버의 종류 : 정보를 보낸 서버가 MasterConnect시 입력한 4번째 파라미터를 의미합니다
            //    (UnityCommon.Server)master_info.m_ServerType,

            //    // 정보를 보낸 서버의 설명 : 정보를 보낸 서버가 MasterConnect시 입력한 3번째 파라미터를 의미합니다
            //    master_info.m_Description,

            //    // 정보를 보낸 서버의 현재 동접 숫자 : 이것을 근거로 나중에 서버이동시 로드벨런싱에 사용할것입니다
            //    master_info.m_Clients,

            //    // 정보를 보낸 서버의 주소
            //    master_info.m_Addr.m_ip,
            //    master_info.m_Addr.m_port
            //    ));

            
        }*/
        #endregion Handlers

        protected override void AfterServerStart()
        {
            base.AfterServerStart();

            m_Core.MasterConnect(Properties.Settings.Default.MasterIp,
                Properties.Settings.Default.MasterPort,
                this.Name,
                (int)this.Type);
        }
        public static bool IsExistChnnel(int chanID)
        {
            switch (chanID)
            {
                case 1: // 무료채널
                case 2: // 무료자유채널
                case 3: // 유료채널
                    return true;
            }
            return false;
        }
        public static string GetChnnelName(ChannelType chanType)
        {
            switch (chanType)
            {
                case ChannelType.Free:
                    return "무료채널";
                case ChannelType.Freedom:
                    return "자유채널";
                case ChannelType.Charge:
                    return "유료채널";
                default:
                    return "?채널";
            }
        }
        public static bool IsExistStakeType(ChannelType chanType, int stakeType)
        {
            if (chanType == ChannelType.Charge)
            {
                switch (stakeType)
                {
                    case 1:
                        return true;
                }
            }
            else
            {
                switch (stakeType)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return true;
                }
            }

            return false;
        }
        public static ChannelType GetChnnelType(int chanID)
        {
            switch (chanID)
            {
                case 1:
                    return ChannelType.Free;
                case 2:
                    return ChannelType.Freedom;
                case 3:
                    return ChannelType.Charge;
            }
            return ChannelType.None;
        }

        public static int GetStakeMoney(ChannelType chanType, int stakeType)
        {
            if (chanType == ChannelType.Charge)
            {
                switch (stakeType)
                {
                    case 1:
                        return 100000;
                }
            }
            else
            {
                switch (stakeType)
                {
                    case 1:
                        return 100;
                    case 2:
                        return 200;
                    case 3:
                        return 300;
                    case 4:
                        return 500;
                    case 5:
                        return 1000;
                }
            }

            return 0;
        }
        public static int GetMinimumMoney(ChannelType chanType, int stakeType)
        {
            if (chanType == ChannelType.Charge)
            {
                switch (stakeType)
                {
                    case 1:
                        return 100000 * 10;
                }
            }
            else if (chanType == ChannelType.Freedom)
            {
                return 1;
            }
            else if (chanType == ChannelType.Free)
            {
                switch (stakeType)
                {
                    case 1:
                        return 100 * 30;
                    case 2:
                        return 200 * 10;
                    case 3:
                        return 300 * 10;
                    case 4:
                        return 500 * 10;
                    case 5:
                        return 1000 * 10;
                }
            }
            else
            {
                return 1;
            }

            return 0;
        }
        public static string GetMinimumMoneyText(ChannelType chanType, int stakeType)
        {
            if (chanType == ChannelType.Charge)
            {
                switch (stakeType)
                {
                    case 1:
                        return "10만";
                }
            }
            else if (chanType == ChannelType.Free)
            {
                switch (stakeType)
                {
                    case 1:
                        return "3000";
                    case 2:
                        return "2000";
                    case 3:
                        return "3000";
                    case 4:
                        return "5000";
                    case 5:
                        return "1만";
                }
            }
            else
            {
                return "1";
            }

            return "0";
        }
        public static void NewMissionData(ref List<CPlayerAgent.MissionData> MissionData)
        {
            if (MissionData == null) MissionData = new List<CPlayerAgent.MissionData>();

            MissionData.Clear();
            // 탑쌓기 이벤트 데이터 생성
            Random rnd = new Random();
            List<int> templist = (new List<int>(new[] { 1, 2, 3, 4, 7, 8, 9, 10, 11, 12 })).OrderBy(o => rnd.Next()).ToList();
            for (int i = 0; i < 10; ++i)
            {
                CPlayerAgent.MissionData md = new CPlayerAgent.MissionData();
                md.isComplete = false;

                byte newMission = (byte)templist[i];
                md.type = newMission;

                MissionData.Add(md);
            }
        }
        public static void AddMissionData(ref List<CPlayerAgent.MissionData> MissionData, byte Mission, bool Compelete)
        {
            if (MissionData == null) MissionData = new List<CPlayerAgent.MissionData>();

            CPlayerAgent.MissionData md = new CPlayerAgent.MissionData();
            md.type = Mission;
            md.isComplete = Compelete;
            MissionData.Add(md);
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

        public void DisplayStatus(ZNet.CoreServerNet svr)
        {
            ZNet.ServerState status;
            svr.GetCurrentState(out status);


            // 기본 정보
            Log._log.InfoFormat(
                "[NetInfo]  Connect/Join {0}({1})/{2}  Connect(Server) {3}/{4}  Accpet/Max {5}/{6}",

                // 실제 연결된 client
                status.m_CurrentClient,

                // 연결복구 처리과정인 client
                status.m_RecoveryCount,

                // 서버에 입장완료상태의 client
                status.m_JoinedClient,

                // 서버간 direct p2p 연결된 server
                status.m_ServerP2PCount,

                // 서버간 direct p2p 연결 모니터링중인 server(서버간 연결 자동복구를 위한 모니터링)
                status.m_ServerP2PConCount,

                // 이 서버에 추가 연결 가능한 숫자
                status.m_nIoAccept,

                // 이 서버에 최대 연결 가능한 숫자
                status.m_MaxAccept
                );


            // 엔진 내부에서 작업중인 IO 관련 상태 정보
            Log._log.InfoFormat(
                "[IO Info]  Close {0}  Event {1}  Recv {2}  Send {3}",

                // current io close
                status.m_nIoClose,

                // current io event
                status.m_nIoEvent,

                // current io recv socket
                status.m_nIoRecv,

                // current io send socket
                status.m_nIoSend
            );


            // 엔진 메모리 관련 사용 정보
            Log._log.InfoFormat(
                "[MemInfo]  Alloc/Instant[{0}/{1}], test[{2}], EngineVersion[{3}.{4:0000}] ",

                // 미리 할당된 IO 메모리
                status.m_nAlloc,

                // 즉석 할당된 IO 메모리
                status.m_nAllocInstant,

                // test data
                status.m_test_data,

                // Core버전
                svr.GetCoreVersion() / 10000,
                svr.GetCoreVersion() % 10000
            );


            // 스레드 정보
            if (status.m_arrThread != null)
            {
                string strThr = "[ThreadInfo] (";
                int MaxDisplayThreadCount = status.m_arrThread.Count();
                if (MaxDisplayThreadCount > 8)   // 화면이 복잡하니까 그냥 최대 8개까지만 표시
                {
                    strThr += MaxDisplayThreadCount;
                    strThr += ") : ";
                    MaxDisplayThreadCount = 8;
                }
                else
                {
                    strThr += MaxDisplayThreadCount;
                    strThr += ") : ";
                }

                for (int i = 0; i < MaxDisplayThreadCount; i++)
                {
                    strThr += "[";
                    strThr += status.m_arrThread[i].m_ThreadID;     // 스레드ID
                    strThr += "/";
                    strThr += status.m_arrThread[i].m_CountQueue;   // 처리 대기중인 작업
                    strThr += "/";
                    strThr += status.m_arrThread[i].m_CountWorked;  // 처리된 작업(누적)
                    strThr += "] ";
                }
                Log._log.Info(strThr);
            }
        }
    }
}
