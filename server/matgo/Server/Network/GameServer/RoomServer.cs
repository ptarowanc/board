using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Guid = System.Guid;
using ZNet;

namespace Server.Network
{
    public class RoomServer : GameServer
    {
        internal object RoomServerLocker = new object();

        // 클라이언트 목록
        ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>> RemoteClients = new ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>>();
        // 방 목록
        public ConcurrentDictionary<Guid, CGameRoom> RemoteRooms;
        // 릴레이 로비 서버 목록
        RelayServerInfo LobbyInfo = new RelayServerInfo();
        ConcurrentDictionary<int, RelayServerInfo> RemoteRelays = new ConcurrentDictionary<int, RelayServerInfo>();
        ConcurrentDictionary<RemoteID, RelayServerInfo> RemoteRelaysh = new ConcurrentDictionary<RemoteID, RelayServerInfo>();
        // 방 번호
        public Stack<int> RoomNumbers;

        public int ChannelID; // 채널 ID
        public ChannelKind ChanKind; // 채널 유형
        public ChannelType ChanType; // 채널타입
        public bool ChanFree; // 자유채널 규칙여부

        // 설정값
        public long JackPotMoney;       // 잭팟 머니
        public double DealerFee;        // 딜러비 비율
        public double JackPotRate;      // 잭팟 적립 비율

        // 홍길동 이벤트 발동조건
        private long EventTermCount;     // 발동 카운트
        private long EventTermX200;       // 200배 이벤트 발동 판수

        // 밀어주기, 가중치
        public long PushType;
        public Dictionary<long, long> BonusBase; // 기본값
        public Dictionary<long, long> BonusPushUser; // 유저 레벨 가중치
        public Dictionary<long, long> BonusPushGame; // 게임 레벨 가중치
        public Dictionary<int, short> BonusPushFirst; // 게임 선잡기 밀어주기
        public Dictionary<int, short> BonusPushChance; // 게임 밀어주기 확률

        // 미션패
        public Dictionary<MISSION, sMission> MissionData;
        public Dictionary<MISSION, int> MissionRate;

        public RoomServer(MatgoService f, UnityCommon.Server t, ushort portnum, int channelID) : base(f, t, portnum, channelID)
        {
            ChannelID = channelID;
            RemoteClients = new ConcurrentDictionary<RemoteID, ConcurrentDictionary<RemoteID, CPlayer>>();
            RemoteRooms = new ConcurrentDictionary<Guid, CGameRoom>();
            RoomNumbers = new Stack<int>();
            BonusBase = new Dictionary<long, long>();
            BonusPushUser = new Dictionary<long, long>();
            BonusPushGame = new Dictionary<long, long>();
            BonusPushFirst = new Dictionary<int, short>();
            BonusPushChance = new Dictionary<int, short>();
            MissionData = new Dictionary<MISSION, sMission>();
            MissionRate = new Dictionary<MISSION, int>();

            ChanKind = GetChannelKind(ChannelID);
            ChanType = GetChannelType(ChannelID);
            ChanFree = GetChannelFree(ChannelID);

            for (int RoomNumber = 1000; RoomNumber > 0; --RoomNumber)
            {
                RoomNumbers.Push(RoomNumber);
            }

            DB_Server_GetRoomData();
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            base.BeforeServerStart(out param);
            param.m_UpdateTimeMs = 1000;

            // CoreHandler
            {
                NetServer.message_handler = message_handler;
                NetServer.master_server_join_handler = master_server_join_handler; // ☆
                NetServer.server_refresh_handler = server_refresh_handler; // ☆
                NetServer.server_master_leave_handler = server_master_leave_handler; // ☆
                NetServer.server_master_join_handler = server_master_join_handler; // ☆
                NetServer.server_leave_handler = server_leave_handler; // ☆
                NetServer.server_join_handler = server_join_handler; // ☆
                NetServer.group_destroy_handler = group_destroy_handler;
                NetServer.group_memberout_handler = group_memberout_handler;
                NetServer.update_event_handler = update_event_handler;
                NetServer.recovery_start_handler = recovery_start_handler;
                NetServer.recovery_end_handler = recovery_end_handler;
                NetServer.recovery_info_handler = recovery_info_handler;
                NetServer.move_server_failed_handler = move_server_failed_handler; // ★
                NetServer.move_server_param_handler = move_server_param_handler; // ★
                NetServer.move_server_start_handler = move_server_start_handler; // ★
                NetServer.limit_connection_handler = limit_connection_handler;
                NetServer.client_leave_handler = client_leave_handler; // ★
                NetServer.client_join_handler = client_join_handler; // ★
                NetServer.client_disconnect_handler = client_disconnect_handler;
                NetServer.client_connect_handler = client_connect_handler;
                NetServer.exception_handler = exception_handler;
            }

            // Master Stub
            {
                Stub.MasterAllShutdown = MasterAllShutdown;
                Stub.MasterNotifyP2PServerInfo = MasterNotifyP2PServerInfo;
            }

            // Server Stub
            {
                // 데이터 동기화 요청
                Stub.ServerRequestDataSync = ServerRequestDataSync;

                // 로비 서버
                Stub.LobbyRoomNotifyServermaintenance = LobbyRoomNotifyServermaintenance;
                Stub.LobbyRoomReloadServerData = LobbyRoomReloadServerData;
                Stub.LobbyRoomKickUser = LobbyRoomKickUser;
                Stub.LobbyRoomJackpotInfo = LobbyRoomJackpotInfo;

                // 릴레이서버 CoreHandle Relay
                Stub.RelayClientJoin = RelayClientJoin;
                Stub.RelayClientLeave = RelayClientLeave;
                Stub.RelayRequestOutRoom = RelayRequestOutRoom;
                Stub.RelayRequestMoveRoom = RelayRequestMoveRoom;
                Stub.LobbyRoomResponseMoveRoom = LobbyRoomResponseMoveRoom;
                Stub.RelayServerMoveFailure = RelayServerMoveFailure;

                // 클라이언트 Response Relay
                Stub.RelayGameRoomIn = RelayGameRoomIn;
                Stub.RelayGameReady = RelayGameReady;
                Stub.RelayGameSelectOrder = RelayGameSelectOrder;
                Stub.RelayGameDistributedEnd = RelayGameDistributedEnd;
                Stub.RelayGameActionPutCard = RelayGameActionPutCard;
                Stub.RelayGameActionFlipBomb = RelayGameActionFlipBomb;
                Stub.RelayGameActionChooseCard = RelayGameActionChooseCard;
                Stub.RelayGameSelectKookjin = RelayGameSelectKookjin;
                Stub.RelayGameSelectGoStop = RelayGameSelectGoStop;
                Stub.RelayGameSelectPush = RelayGameSelectPush;
                Stub.RelayGamePractice = RelayGamePractice;
            }
        }

        #region Room Server
        public override void ServerTask(object sender, ElapsedEventArgs e_)
        {
#if DEBUG
#else
            try
#endif
            {
                ++this.tick;

                if (this.tick % 5 == 0)
                {
                    // 입장시간 지났는데 멈춰있는 플레이어. 처리후, 응답없으면 강퇴
                    foreach (var realyClient in RemoteClients)
                        foreach (var player in realyClient.Value)
                        {
                            if (player.Value.status == UserStatus.None)
                            {
                                if (player.Value.roomID == Guid.Empty && player.Value.roomTime.AddSeconds(10) < DateTime.Now)
                                {
                                    Log._log.Warn("입장시간 Over. (Disconect) Player:" + player.Value.data.userID);
                                    DBLog(player.Value.data.ID, ChannelID, 0, LOG_TYPE.연결끊김, "입장실패 후 응답없음");
                                    ClientDisconect(player.Value.Remote.Key, player.Value.Remote.Value, "Task roomTime Over");
                                }
                            }
                        }
                }

                if (this.tick % 17 == 0)
                {
                    //DisplayStatus(m_Core);

                    if (this.ShutDown)
                    {
                        int players = 0;
                        foreach (var playercount in RemoteClients)
                        {
                            players += playercount.Value.Count();
                        }
                        int rooms = RemoteRooms.Count;
                        Log._log.Info("세션 종료중. 남은 플레이어 수:" + players + " 남은 방 수:" + RemoteRooms.Count);

                        // 모든 세션 종료
                        if (this.CountDown < DateTime.Now)
                        {
                            foreach (var relay in RemoteClients)
                                foreach (var client in relay.Value)
                                {
                                    ClientDisconect(client.Value.Remote.Key, client.Value.Remote.Value, "ShutDown");
                                    //NetServer.CloseConnection(client.Key);
                                }
                        }
                        // 더 이상 방이 없으면 프로그램 종료
                        if (players == 0 && rooms == 0)
                        {
                            Log._log.Info("서버 종료. ShutDown");
                            //System.Windows.Forms.Application.Exit();
                            System.Environment.Exit(0);
                        }
                    }
                }
            }
#if DEBUG
#else
            catch (Exception e)
            {
                Log._log.Fatal("TaskTimer 에러:" + e.ToString());
            }
#endif
        }
        public void ClientDisconect(RemoteID remoteS, RemoteID remoteC, string reasone)
        {
            Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
            Log._log.WarnFormat("ClientDisconect. remoteS:{0}, remoteC:{1}, reasone:{2}", remoteS, remoteC, reasone);
        }
        public void KickPlayer(CPlayer player, string reasone)
        {
            Proxy.GameRelayKickUser(player.Remote.Key, CPackOption.Basic, player.Remote.Value);

            Log._log.WarnFormat("KickPlayer. player:{0} reasone:{1}", player.data.userID, reasone);
        }
        void CheckRoomCount(CGameRoom room)
        {
            if (room.PlayersConnect.Count == 0)
            {
                CGameRoom temp;
                if (RemoteRooms.TryRemove(room.roomID, out temp))
                {
                    if (temp.isRun)
                    {
                        temp.isRun = false;
                        temp.Thread_sequential_packet_handler.Join();
                    }
                    DB_Room_Delete(room.roomID);
                    RoomNumbers.Push(room.roomNumber);
                }
            }
            else
            {
                DB_Room_Update(room.roomID, room.PlayersConnect);
            }
        }
        bool ProcessGame(RemoteID remote, RemoteID userRemote, CMessage data, PacketType rmiID)
        {
            CPlayer rc;
            CMessage msg = data;
            ConcurrentDictionary<RemoteID, CPlayer> relayClients;
            if (RemoteClients.TryGetValue(remote, out relayClients))
                if (relayClients.TryGetValue(userRemote, out rc))
                {
                    CGameRoom room_join;
                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                    {
#if PACKET
                    Log._log.InfoFormat("RECV Packet. remote:{0}, rmiID:{1}, user:{2}", remote, rmiID, rc.data.userID);
#endif
                        room_join.waiting_packets.Enqueue(new MessagePacket(rc, msg, rmiID));
                    }
                    else
                    {
                        Log._log.WarnFormat("ProcessGame no room. remote:{0}, rmiID:{1}, user:{2}", userRemote, rmiID, rc.data.userID);
                    }
                }

            return true;
        }
        public void DummyPlayerLeave(CGameRoom room)
        {
            List<CPlayer> DummyPlayer = null;

            foreach (var player in room.PlayersGaming)
            {
                if (player.Value.status == UserStatus.RoomPlayAuto)
                {
                    if (DummyPlayer == null)
                    {
                        DummyPlayer = new List<CPlayer>();
                    }
                    DummyPlayer.Add(player.Value);
                }
            }

            if (DummyPlayer != null)
            {
                for (int i = 0; i < DummyPlayer.Count; ++i)
                {
                    DB_User_Dummyout(DummyPlayer[i].data.ID);
                    room.player_room_out(DummyPlayer[i]);
                    Proxy.RoomLobbyOutRoom((RemoteID)room.remote_lobby, CPackOption.Basic, room.roomID, DummyPlayer[i].data.ID);
                    Log._log.InfoFormat("DummyClient {0} Leave.", DummyPlayer[i].data.userID);
                }
                CheckRoomCount(room);
            }
        }
        public bool EventCheck(int ChanId)
        {
            // 이벤트 값 없으면 진행 안함
            if (EventTermX200 == 0) return false;

            ++EventTermCount;
            //Log._log.Info("EventTermCount:" + EventTermCount + ", EventTermX200:" + EventTermX200);
            if (EventTermCount >= EventTermX200)
            {
                //Log._log.Info("GameCount Event On");
                EventTermCount = 0;
                return true;
            }

            return false;
        }
        public bool IsPush(int Game1Level, int User1Level, int Game2Level, int User2Level, ref bool FirstPlayerPick)
        {
            // 보정 확률
            long BasePush;
            if (BonusBase.TryGetValue(PushType, out BasePush) == false) return false;
            if (BasePush > 0)
            {
                // 플레이어 게임, 유저 레벨 확인 후 확률 검사 (레벨 높은 순으로 검사, 같을경우 랜덤 순으로 검사)
                long GameLevel1Push;
                if (BonusPushGame.TryGetValue(Game1Level, out GameLevel1Push) == false) return false;
                long UserLevel1Push;
                if (BonusPushUser.TryGetValue(User1Level, out UserLevel1Push) == false) return false;

                long GameLevel2Push;
                if (BonusPushGame.TryGetValue(Game2Level, out GameLevel2Push) == false) return false;
                long UserLevel2Push;
                if (BonusPushUser.TryGetValue(User2Level, out UserLevel2Push) == false) return false;

                if (Game1Level > Game2Level)
                {
                    short PushChance;
                    if (BonusPushChance.TryGetValue(Game1Level, out PushChance) == false) return false;
                    if (Rnd.Next() % 100 + 1 <= PushChance) return false;

                    long push = GameLevel1Push + UserLevel1Push;
                    if (push > 0)
                    {
                        if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                        {
                            FirstPlayerPick = true;
                            return true;
                        }
                    }
                    push = GameLevel2Push + UserLevel2Push;
                    if (push > 0)
                    {
                        if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                        {
                            FirstPlayerPick = false;
                            return true;
                        }
                    }
                }
                else if (Game1Level < Game2Level)
                {
                    short PushChance;
                    if (BonusPushChance.TryGetValue(Game2Level, out PushChance) == false) return false;
                    if (Rnd.Next() % 100 + 1 <= PushChance) return false;

                    long push = GameLevel2Push + UserLevel2Push;
                    if (push > 0)
                    {
                        if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                        {
                            FirstPlayerPick = false;
                            return true;
                        }
                    }
                    push = GameLevel1Push + UserLevel1Push;
                    if (push > 0)
                    {
                        if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                        {
                            FirstPlayerPick = true;
                            return true;
                        }
                    }
                }
                else
                {
                    int randFirst = Rnd.Next() % 2;
                    if (randFirst == 0)
                    {
                        short PushChance;
                        if (BonusPushChance.TryGetValue(Game1Level, out PushChance) == false) return false;
                        if (Rnd.Next() % 100 + 1 <= PushChance) return false;


                        long push = GameLevel1Push + UserLevel1Push;
                        if (push > 0)
                        {
                            if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                            {
                                FirstPlayerPick = true;
                                return true;
                            }
                        }
                        push = GameLevel2Push + UserLevel2Push;
                        if (push > 0)
                        {
                            if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                            {
                                FirstPlayerPick = false;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        short PushChance;
                        if (BonusPushChance.TryGetValue(Game2Level, out PushChance) == false) return false;
                        if (Rnd.Next() % 100 + 1 <= PushChance) return false;


                        long push = GameLevel2Push + UserLevel2Push;
                        if (push > 0)
                        {
                            if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                            {
                                FirstPlayerPick = false;
                                return true;
                            }
                        }
                        push = GameLevel1Push + UserLevel1Push;
                        if (push > 0)
                        {
                            if (Rnd.Next() % 100 + 1 <= BasePush * ((push) / 100.0))
                            {
                                FirstPlayerPick = true;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        public byte IsPushTurn(int Game1Level, int User1Level, int Game2Level, int User2Level)
        {
            // 보정 확률 테이블

            // 레벨 높은 쪽이 먼저 검사
            if (Game1Level > Game2Level)
            {
                short temp;
                if (BonusPushFirst.TryGetValue(Game1Level, out temp))
                {
                    if (Rnd.Next() % 100 < temp)
                    {
                        return 1;
                    }
                }

                if (BonusPushFirst.TryGetValue(Game2Level, out temp))
                {
                    if (Rnd.Next() % 100 < temp)
                    {
                        return 2;
                    }
                }
            }
            else if (Game1Level < Game2Level)
            {
                short temp;
                if (BonusPushFirst.TryGetValue(Game2Level, out temp))
                {
                    if (Rnd.Next() % 100 < temp)
                    {
                        return 2;
                    }
                }

                if (BonusPushFirst.TryGetValue(Game1Level, out temp))
                {
                    if (Rnd.Next() % 100 < temp)
                    {
                        return 1;
                    }
                }
            }
            else // 레벨이 같을경우 랜덤
            {
                int randFirst = Rnd.Next() % 2;

                if (randFirst == 0)
                {
                    short temp;
                    if (BonusPushFirst.TryGetValue(Game1Level, out temp))
                    {
                        if (Rnd.Next() % 100 < temp)
                        {
                            return 1;
                        }
                    }

                    if (BonusPushFirst.TryGetValue(Game2Level, out temp))
                    {
                        if (Rnd.Next() % 100 < temp)
                        {
                            return 2;
                        }
                    }
                }
                else
                {
                    short temp;
                    if (BonusPushFirst.TryGetValue(Game2Level, out temp))
                    {
                        if (Rnd.Next() % 100 < temp)
                        {
                            return 2;
                        }
                    }

                    if (BonusPushFirst.TryGetValue(Game1Level, out temp))
                    {
                        if (Rnd.Next() % 100 < temp)
                        {
                            return 1;
                        }
                    }
                }
            }

            return 0;
        }
        #endregion

        #region CoreHandler
        void message_handler(ResultInfo resultInfo)
        {
            switch (resultInfo.m_Level)
            {
                case IResultLevel.IMsg:
                    Log._log.InfoFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
                case IResultLevel.IWrn:
                    Log._log.WarnFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
                case IResultLevel.IErr:
                    Log._log.ErrorFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
                case IResultLevel.ICri:
                    Log._log.FatalFormat("message_handler. msg:{0}", resultInfo.msg);
                    break;
            }
        }
        void master_server_join_handler(RemoteID remote, string description, int type, NetAddress addr)
        {
            ServerInfo serverNew = new ServerInfo();
            serverNew.ServerHostID = remote;
            serverNew.ServerName = description;
            serverNew.ServerType = (ServerType)type;
            serverNew.ServerAddrPort = addr;
            serverNew.PublicIP = addr.m_ip;
            serverNew.PublicPort = addr.m_port;
            serverNew.ServerID = ServerID;

            Log._log.InfoFormat("master_server_join_handler remoteID({0}) {1} type({2})", remote, description, type);
        }
        void server_refresh_handler(MasterInfo master_info)
        {
            //Log._log.InfoFormat("server_refresh_handler m_ip1:{0}, m_port1:{1}, m_ip2:{2}, m_port2:{3}, m_Clients:{4}, m_Description:{5}, m_remote:{6}, m_ServerType:{7}",
            //    master_info.m_Addr.m_ip, master_info.m_Addr.m_port,
            //    master_info.m_AddrForClient.m_ip, master_info.m_AddrForClient.m_port,
            //    master_info.m_Clients, master_info.m_Description, master_info.m_remote, master_info.m_ServerType);
        }
        void server_master_leave_handler()
        {
            Log._log.InfoFormat("server_master_leave_handler");
        }
        void server_master_join_handler(RemoteID remote, RemoteID myRemoteID)
        {
            Log._log.InfoFormat("server_master_join_handler. remote:{0}, myRemoteID:{1}", remote, myRemoteID);
        }
        void server_leave_handler(RemoteID remote, NetAddress addr)
        {
            //    if (ShutDown) return; // 서버 종료중이면 서버퇴장 대응 없음

            lock (GameServerLocker)
            {
                var itemToRemove = ServerInfoList.Values.SingleOrDefault(r => r.ServerHostID == remote);
                if (itemToRemove != null)
                {
                    ServerInfo remove;
                    ServerInfoList.TryRemove(itemToRemove.ServerHostID, out remove);

                    if (itemToRemove.ServerType == ServerType.Relay)
                    {
                        lock (RoomServerLocker)
                        {
                            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                            if (RemoteClients.TryRemove(itemToRemove.ServerHostID, out relayTemp))
                            {
                                foreach (var rc in relayTemp.Values)
                                {
                                    CGameRoom room_join;
                                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                                    {
                                        lock (room_join.Locker)
                                        {
                                            if (room_join.status == RoomStatus.GamePlay && rc.status == UserStatus.RoomPlay) // 게임중 퇴장
                                            {
                                                rc.agent.setDummyPlayer();
                                                rc.status = UserStatus.RoomPlayAuto;
                                                DB_User_AutoPlay(rc.data.ID, room_join.roomNumber);

                                                DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.자동치기, "");
                                                Log._log.InfoFormat("DummyClient {0} Online2.\n", rc.data.userID);
                                            }
                                            else
                                            {
                                                DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.비정상종료, "");
                                            }
                                            room_join.player_room_out(rc);
                                            CheckRoomCount(room_join);
                                            Proxy.RoomLobbyOutRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, rc.data.ID);
                                        }
                                    }
                                    Log._log.WarnFormat("Relay Leave. hostID:{0}, player:{1}", itemToRemove.ServerHostID, rc.data.userID);
                                }
                            }
                        }
                    }
                    else if (itemToRemove.ServerType == ServerType.RelayLobby)
                    {
                        LobbyInfo.remote = RemoteID.Remote_None;
                        RelayServerInfo info;
                        if (RemoteRelaysh.TryRemove(itemToRemove.ServerHostID, out info))
                        {
                            RelayServerInfo temp_;
                            RemoteRelays.TryRemove(info.RelayID, out temp_);
                        }
                    }
                }
            }
            Log._log.InfoFormat("server_leave_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void server_join_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("server_join_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void group_destroy_handler(RemoteID groupID)
        {
            Log._log.InfoFormat("group_destroy_handler. groupID:{0}", groupID);
        }
        void group_memberout_handler(RemoteID groupID, RemoteID memberID, int Members)
        {
            Log._log.InfoFormat("group_memberout_handler. groupID:{0}, memberID:{1}, Members:{2}", groupID, memberID, Members);
        }
        void update_event_handler()
        {
            //Log._log.InfoFormat("update_event_handler.");
        }
        void recovery_start_handler(RemoteID remote)
        {
            Log._log.InfoFormat("recovery_start_handler. remote:{0}", remote);
        }
        void recovery_end_handler(RemoteID remote, NetAddress addrNew, bool bTimeOut)
        {
            Log._log.InfoFormat("recovery_end_handler. remote:{0}, m_ip:{1}, m_port:{2}, bTimeOut:{3}", remote, addrNew.m_ip, addrNew.m_port, bTimeOut);
        }
        void recovery_info_handler(RemoteID remoteNew, RemoteID remoteTo)
        {
            Log._log.InfoFormat("recovery_info_handler. remoteNew:{0}, remoteTo:{1}", remoteNew, remoteTo);
        }
        void move_server_failed_handler(ArrByte move_server)
        {
            Log._log.InfoFormat("move_server_failed_handler.");
        }
        bool move_server_param_handler(ArrByte move_param, int count_idx)
        {
            Log._log.InfoFormat("move_server_param_handler. count_idx:{0}", count_idx);
            return true;
        }
        void move_server_start_handler(RemoteID remote, out ArrByte userdata)
        {
            userdata = null;

            Log._log.InfoFormat("move_server_start_handler. remote:{0}", remote);
        }
        void limit_connection_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("limit_connection_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void client_leave_handler(RemoteID remote, bool bMoveServer)
        {
            Log._log.InfoFormat("client_leave_handler. remote:{0}, bMoveServer:{1}", remote, bMoveServer);
        }
        void client_join_handler(RemoteID remote, NetAddress addr, ArrByte move_server, ArrByte move_param)
        {
            Log._log.InfoFormat("client_join_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void client_disconnect_handler(RemoteID remote)
        {
            Log._log.InfoFormat("client_disconnect_handler. remote:{0}", remote);
        }
        void client_connect_handler(RemoteID remote, NetAddress addr)
        {
            Log._log.InfoFormat("client_connect_handler. remote:{0}, m_ip:{1}, m_port:{2}", remote, addr.m_ip, addr.m_port);
        }
        void exception_handler(Exception e)
        {
            Log._log.ErrorFormat("exception_handler. e:{0}", e.ToString());
        }
        #endregion

        #region Master Handler
        //void MasterJoinServerCompleteHandler(ErrorInfo info, ArrByte replyFromServer)
        //{
        //    if (info.errorType == ErrorType.Ok)
        //    {
        //        MasterHostID = info.remote;
        //        Log._log.InfoFormat("MasterJoinServerCompleteHandler Success. MasterHostID:{0}", MasterHostID);
        //    }
        //    else
        //    {
        //        Log._log.ErrorFormat("MasterJoinServerCompleteHandler Failed. info.errorType:{0}", info.errorType);
        //    }
        //}
        //void MasterLeaveServerHandler(ErrorInfo info)
        //{
        //    Log._log.WarnFormat("MasterLeaveServerHandler. remote:{0}, errorType:{1}, detailType:{2}", info.remote, info.errorType, info.detailType);
        //}
        //void MasterP2PMemberJoinHandler(RemoteID memberHostID, RemoteID groupHostID, int memberCount, ArrByte customField)
        //{
        //    Log._log.InfoFormat("MasterP2PMemberJoinHandler Success. memberHostID:{0}, groupHostID:{1}, memberCount:{2}", memberHostID, groupHostID, memberCount);

        //}
        //void MasterP2PMemberLeaveHandler(RemoteID memberHostID, RemoteID groupHostID, int memberCount)
        //{
        //    if (ShutDown) return; // 서버 종료중이면 서버퇴장 대응 없음

        //    lock (GameServerLocker)
        //    {
        //        var itemToRemove = ServerInfoList.Values.SingleOrDefault(r => r.ServerHostID == memberHostID);
        //        if (itemToRemove != null)
        //        {
        //            ServerInfo remove;
        //            ServerInfoList.TryRemove(itemToRemove.ServerHostID, out remove);

        //            if (itemToRemove.ServerType == ServerType.RelayLobby)
        //            {
        //                RelayServerInfo info;
        //                if (RemoteRelaysh.TryRemove(itemToRemove.ServerHostID, out info))
        //                {
        //                    RelayServerInfo temp_;
        //                    RemoteRelays.TryRemove(info.RelayID, out temp_);
        //                }
        //            }
        //        }
        //    }

        //    Log._log.InfoFormat("MasterP2PMemberLeaveHandler Success. memberHostID:{0}, groupHostID:{1}, memberCount:{2}", memberHostID, groupHostID, memberCount);
        //}
        #endregion

        #region Master Stub Handler
        bool MasterAllShutdown(RemoteID remote, CPackOption rmiContext, string msg)
        {
            bool result = true;

            ServerMaintenance = true;
            ServerMsg = "※안내※\n서버 점검중입니다.";
            ShutDown = true;
            CountDown = DateTime.Now.AddMinutes(1);

            Log._log.Info("서버종료 요청 받음.");

            return result;
        }
        bool MasterNotifyP2PServerInfo(RemoteID remote, CPackOption rmiContext, CMessage data)
        {
            lock (GameServerLocker)
            {
                CMessage msg = data;
                ServerInfo serverInfo = new ServerInfo();
                msg.Read(out serverInfo.ServerName);
                int serverType;
                msg.Read(out serverType);
                serverInfo.ServerType = (ServerType)serverType;
                msg.Read(out serverInfo.ServerHostID);
                string serverAddrIP;
                msg.Read(out serverAddrIP);
                ushort serverAddrPort;
                msg.Read(out serverAddrPort);
                msg.Read(out serverInfo.PublicIP);
                msg.Read(out serverInfo.PublicPort);
                msg.Read(out serverInfo.ServerID);
                serverInfo.ServerAddrPort = new NetAddress();
                serverInfo.ServerAddrPort.m_ip = serverAddrIP;
                serverInfo.ServerAddrPort.m_port = serverAddrPort;

                if (serverInfo.ServerName == this.Name)
                {
                    this.ServerHostID = serverInfo.ServerHostID;
                }
                else
                {
                    if (ServerInfoList.TryAdd(serverInfo.ServerHostID, serverInfo))
                    {
                        if (serverInfo.ServerType == ServerType.RelayLobby)
                        {
                            RelayServerInfo relayInfo;
                            relayInfo.remote = serverInfo.ServerHostID;
                            relayInfo.RelayID = serverInfo.ServerID;
                            relayInfo.Addr = serverInfo.ServerAddrPort;
                            RemoteRelays.TryAdd(relayInfo.RelayID, relayInfo);
                            RemoteRelaysh.TryAdd(relayInfo.remote, relayInfo);
                            LobbyInfo = relayInfo;
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Server Stub Handler
        bool ServerRequestDataSync(RemoteID remote, CPackOption rmiContext, bool isLobby)
        {
            CMessage msg = new CMessage();
            msg.Write((int)ChannelID);
            msg.Write((int)RemoteRooms.Count);

            foreach (var room in RemoteRooms)
            {
                if (isLobby)
                {
                    DB_Room_InsertAll((int)room.Value.ChanId, room.Value.roomID, room.Value.roomNumber, room.Value.BaseMoney, room.Value.PlayersConnect);
                }

                room.Value.remote_lobby = (int)remote;

                msg.Write(room.Value.roomID);
                msg.Write(ChannelID);
                msg.Write((int)room.Value.ChanType);
                msg.Write(room.Value.ChanFree);
                msg.Write(room.Value.roomNumber);
                msg.Write(room.Value.stake);
                msg.Write(room.Value.BaseMoney);
                msg.Write(room.Value.MinMoney);
                msg.Write(room.Value.MaxMoney);
                msg.Write(room.Value.PlayersConnect.Count);
                msg.Write(room.Value.isRestrict());
                msg.Write(room.Value.remote_svr);
                msg.Write(room.Value.remote_lobby);
                msg.Write(room.Value.Password.Length != 0);
                msg.Write(room.Value.Password);

                foreach (var player in room.Value.PlayersConnect)
                {
                    msg.Write(player.Value.data.ID);
                    msg.Write(player.Value.m_ip);
                    msg.Write(player.Value.data.shopId);
                    msg.Write(player.Value.Remote.Value);
                    msg.Write(player.Value.data.nickName);
                    msg.Write(player.Value.data.money_free);
                    msg.Write(player.Value.data.money_pay);
                }
            }

            Proxy.RoomLobbyResponseDataSync(remote, rmiContext, msg.m_array);

            return true;
        }
        bool LobbyRoomNotifyServermaintenance(RemoteID remote, CPackOption rmiContext, int type, string message, int release)
        {
            if (release == 1)
            {
                ServerMaintenance = false;
                ServerMsg = "";
            }
            else
            {
                ServerMaintenance = true;
                ServerMsg = "※안내※\n서버 점검중입니다.";
            }

            return true;
        }
        bool LobbyRoomReloadServerData(RemoteID remote, CPackOption rmiContext, int loadType)
        {
            // 초기화. 서버 설정값 확인
            Task.Run(() =>
            {
                try
                {
                    switch ((DB_COMMAND)loadType)
                    {
                        case DB_COMMAND.MATGO_JACKPOT:
                            {
                                dynamic Data_JackPot = db.GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();

                                this.EventTermX200 = Data_JackPot.Multiple200;
                            }
                            break;

                        case DB_COMMAND.MATGO_PUSH:
                            {
                                dynamic Data_Push = db.GameMatgoPush.FindAllBy(ChannelId: ChannelID).FirstOrDefault();

                                this.PushType = Data_Push.PushBaseType;
                            }
                            break;
                        case DB_COMMAND.MATGO_PUSH_BASE:
                            {
                                dynamic Data_PushBase = db.GameMatgoPushBase.All();

                                foreach (var row in Data_PushBase.ToList())
                                {
                                    long temp;
                                    if (this.BonusBase.TryGetValue(row.Type, out temp))
                                    {
                                        this.BonusBase[row.Type] = row.BonusCard;
                                    }
                                    else
                                    {
                                        this.BonusBase.Add(row.Type, row.BonusCard);
                                    }
                                }

                                dynamic Data_PushFirst = db.GameMatgoPushFirst.All();
                                foreach (var row in Data_PushFirst.ToList())
                                {
                                    short temp;
                                    if (BonusPushFirst.TryGetValue(row.Level, out temp))
                                    {
                                        BonusPushFirst[row.Level] = row.Bonus;
                                    }
                                    else
                                    {
                                        BonusPushFirst.Add(row.Level, row.Bonus);
                                    }
                                    if (BonusPushChance.TryGetValue(row.Level, out temp))
                                    {
                                        BonusPushChance[row.Level] = row.Chance;
                                    }
                                    else
                                    {
                                        BonusPushChance.Add(row.Level, row.Chance);
                                    }
                                }
                            }
                            break;
                        case DB_COMMAND.MATGO_PUSH_USER:
                            {
                                dynamic Data_PushUser = db.GameMatgoPushUser.All();

                                foreach (var row in Data_PushUser.ToList())
                                {
                                    long temp;
                                    if (this.BonusPushUser.TryGetValue(row.UserLevel, out temp))
                                    {
                                        this.BonusPushUser[row.UserLevel] = row.Push;
                                    }
                                    else
                                    {
                                        this.BonusPushUser.Add(row.UserLevel, row.Push);
                                    }
                                }
                            }
                            break;
                        case DB_COMMAND.MATGO_PUSH_GAME:
                            {
                                dynamic Data_PushGame = db.GameMatgoPushGame.All();

                                foreach (var row in Data_PushGame.ToList())
                                {
                                    long temp;
                                    if (this.BonusPushGame.TryGetValue(row.GameLevel, out temp))
                                    {
                                        this.BonusPushGame[row.GameLevel] = row.Push;
                                    }
                                    else
                                    {
                                        this.BonusPushGame.Add(row.GameLevel, row.Push);
                                    }
                                }
                            }
                            break;
                        case DB_COMMAND.MATGO_MISSION:
                            {
                                dynamic Data_Mission = db.GameMatgoMission.All();

                                foreach (var row in Data_Mission.ToList())
                                {
                                    sMission temp;
                                    if (MissionData.TryGetValue((MISSION)row.MissionId, out temp))
                                    {
                                        temp.Id = row.MissionId;
                                        temp.Name = row.MissionName;
                                        temp.Reward = row.MissionReward;
                                        temp.Rate = row.MissionRate;
                                        //MissionData[(MISSION)temp.Id] = temp;
                                    }
                                    else
                                    {
                                        temp.Id = row.MissionId;
                                        temp.Name = row.MissionName;
                                        temp.Reward = row.MissionReward;
                                        temp.Rate = row.MissionRate;
                                        MissionData.Add((MISSION)temp.Id, temp);
                                    }

                                    int temp2;
                                    if (MissionRate.TryGetValue((MISSION)row.MissionId, out temp2))
                                    {
                                        MissionRate[(MISSION)row.MissionId] = row.MissionRate;
                                    }
                                    else
                                    {
                                        MissionRate.Add((MISSION)row.MissionId, row.MissionRate);
                                    }
                                }
                            }
                            break;
                    }

                    Log._log.InfoFormat("ReloadServerData Success. loadType:{0}", loadType);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("ReloadServerData Failure. loadType:{0}, e:{1}", loadType, e.ToString());
                }

            });

            return true;
        }
        bool LobbyRoomKickUser(RemoteID remote, CPackOption rmiContext, int userID)
        {
            lock (RoomServerLocker)
            {
                foreach (var relay in RemoteClients)
                    foreach (var user in relay.Value)
                    {
                        if (user.Value.data.ID == userID)
                        {
                            // 자동치기 상태이면 버그이므로 현재 접속자에서 삭제
                            if (user.Value.status == UserStatus.RoomPlayAuto)
                            {
                                Task.Run(() =>
                                {
                                    try
                                    {
                                        db.GameCurrentUser.DeleteByUserId(UserId: user.Value.data.ID);
                                        db.GameCurrentDummy.DeleteByUserId(UserId: user.Value.data.ID);
                                    }
                                    catch
                                    {
                                    }
                                });
                            }
                            else
                            {
                                CGameRoom room;
                                if (RemoteRooms.TryGetValue(user.Value.roomID, out room))
                                {
                                    DBLog(userID, ChannelID, room.roomNumber, LOG_TYPE.연결끊김, "운영툴 강제퇴장");
                                }
                                else
                                {
                                    DBLog(userID, ChannelID, 0, LOG_TYPE.연결끊김, "운영툴 강제퇴장");
                                }
                                ClientDisconect(user.Value.Remote.Key, user.Value.Remote.Value, "LobbyRoomKickUser");
                            }
                        }
                    }
            }
            return true;
        }
        bool LobbyRoomJackpotInfo(RemoteID remote, CPackOption rmiContext, long jackpot)
        {
            this.JackPotMoney = jackpot;

            return true;
        }
        bool RelayClientJoin(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC, NetAddress addr, ArrByte moveData)
        {
            // 서버 이동 데이터 불러오기
            Common.MoveParam param;
            CPlayer rc;
            Common.Common.ServerMoveParamRead2(moveData, out param, out rc);

            rc.m_ip = addr.addr;
            rc.Remote = new KeyValuePair<RemoteID, RemoteID>(remoteS, remoteC);
            rc.RelayID = param.RelayID;

            int CID = param.ChannelNumber;
            lock (RoomServerLocker)
            {
                if (ChannelID != CID || param.room_id == Guid.Empty || param.roomJoin == Common.MoveParam.ParamRoom.RoomNull)
                {
                    rc.Reset();
                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                    if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                    {
                        relayTemp.TryAdd(rc.Remote.Value, rc);
                    }
                    else
                    {
                        relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                        relayTemp.TryAdd(rc.Remote.Value, rc);
                        RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                    }
                    Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                    KickPlayer(rc, "RelayClientJoin Failure. CID, room_id, roomJoin");
                    return false;
                }

                // DB 플레이어 정보 불러오기
                try
                {
                    //방생성
                    if (param.roomJoin == Common.MoveParam.ParamRoom.RoomMake)
                    {
                        rc.Reset();
                        rc.Operator = true;
                        if (RoomNumbers.Count == 0)
                        {
                            rc.Reset();
                            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                            {
                                relayTemp.TryAdd(rc.Remote.Value, rc);
                            }
                            else
                            {
                                relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                                relayTemp.TryAdd(rc.Remote.Value, rc);
                                RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                            }
                            Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                            KickPlayer(rc, "RelayClientJoin Failure. RoomNumber Count 0");
                            return false;
                        }

                        CGameRoom new_room = new CGameRoom(this);
                        lock (new_room.Locker)
                        {
                            rc.player_index = new_room.pop_players_index();
                            rc.agent = new CPlayerAgent(rc.player_index, new_room);
                            if (ChanType == ChannelType.Charge)
                                rc.agent.setMoney(rc.data.money_pay);
                            else
                                rc.agent.setMoney(rc.data.money_free);
                            rc.roomID = param.room_id;
                            //rc.agent.topMission = rc.data.topMission;

                            new_room.From(CID, param.room_id, RoomNumbers.Pop(), param.roomStake, (int)this.ServerHostID, param.lobby_remote);
                            RemoteRooms.TryAdd(param.room_id, new_room);

                            new_room.PlayersConnect.TryAdd(rc.Remote, rc);

                            Rmi.Marshaler.RoomInfo new_roominfo = new Rmi.Marshaler.RoomInfo();
                            new_roominfo.roomID = new_room.roomID;
                            new_roominfo.chanID = CID;
                            new_roominfo.chanType = (int)ChanType;
                            new_roominfo.chanFree = ChanFree;
                            new_roominfo.number = new_room.roomNumber;
                            new_roominfo.stakeType = param.roomStake;
                            new_roominfo.baseMoney = GetStakeMoney(ChanKind, param.roomStake);
                            new_roominfo.minMoney = GetMinimumMoney(ChanKind, param.roomStake);
                            new_roominfo.maxMoney = GetMaximumMoney(ChanKind, param.roomStake);
                            new_roominfo.userCount = 1;
                            new_roominfo.restrict = false;
                            new_roominfo.remote_svr = new_room.remote_svr;
                            new_roominfo.remote_lobby = new_room.remote_lobby;
                            if (param.roomPassword != null && param.roomPassword != "")
                            {
                                new_roominfo.needPassword = true;
                                new_roominfo.roomPassword = param.roomPassword;
                                new_room.Password = param.roomPassword;
                            }
                            else
                            {
                                new_roominfo.needPassword = false;
                                new_roominfo.roomPassword = "";
                            }

                            Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                            userInfo.nickName = rc.data.nickName;
                            userInfo.FreeMoney = rc.data.money_free;
                            userInfo.PayMoney = rc.data.money_pay;
                            userInfo.chanID = CID;
                            userInfo.roomNumber = new_room.roomNumber;

                            DB_Room_Insert(CID, new_room.roomID, new_room.roomNumber, new_room.engine.baseMoney, rc.data.ID);
                            // 로비서버에게 방생성을 알린다.
                            Proxy.RoomLobbyMakeRoom((RemoteID)new_room.remote_lobby, CPackOption.Basic, new_roominfo, userInfo, rc.data.ID, rc.m_ip, param.roomPassword, rc.data.shopId);
                            // 클라이언트 입장시킴
                            Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, true);
                        }
                    }
                    else if (param.roomJoin == Common.MoveParam.ParamRoom.RoomJoin)
                    {
                        //방입장
                        CGameRoom room_join;

                        if (RemoteRooms.TryGetValue(param.room_id, out room_join))
                        {
                            rc.Reset();
                            rc.Operator = false;
                            lock (room_join.Locker)
                            {
                                if (room_join.PlayersConnect.Count >= CGameRoom.max_users) // 인원수 초과시
                                {
                                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                                    if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                                    {
                                        relayTemp.TryAdd(rc.Remote.Value, rc);
                                    }
                                    else
                                    {
                                        relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                                        relayTemp.TryAdd(rc.Remote.Value, rc);
                                        RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                                    }
                                    Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                                    KickPlayer(rc, "RelayClientJoin Failure. max_users");
                                    return false;
                                }

                                rc.player_index = room_join.pop_players_index();
                                rc.agent = new CPlayerAgent(rc.player_index, room_join);
                                if (ChanType == ChannelType.Charge)
                                    rc.agent.setMoney(rc.data.money_pay);
                                else
                                    rc.agent.setMoney(rc.data.money_free);
                                //rc.agent.topMission = rc.data.topMission;
                                rc.roomID = room_join.roomID;

                                room_join.PlayersConnect.TryAdd(rc.Remote, rc);

                                Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                                userInfo.nickName = rc.data.nickName;
                                userInfo.FreeMoney = rc.data.money_free;
                                userInfo.PayMoney = rc.data.money_pay;
                                userInfo.chanID = CID;
                                userInfo.roomNumber = room_join.roomNumber;

                                DB_Room_Update(room_join.roomID, room_join.PlayersConnect, CID, rc.data.ID, room_join.roomNumber);

                                // 로비서버에게 방입장을 알린다.
                                Proxy.RoomLobbyJoinRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, userInfo, rc.data.ID, rc.m_ip, rc.data.shopId);
                                // 클라이언트 입장시킴
                                Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, true);
                            }
                        }
                    }
                    else
                    {
                        Log._log.ErrorFormat("Client Join RoomMake Error. Player:{0}", rc.data.userID);
                        rc.Reset();
                        rc.Operator = false;
                        ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                        if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                        {
                            relayTemp.TryAdd(rc.Remote.Value, rc);
                        }
                        else
                        {
                            relayTemp = new ConcurrentDictionary<RemoteID, CPlayer>();
                            relayTemp.TryAdd(rc.Remote.Value, rc);
                            RemoteClients.TryAdd(rc.Remote.Key, relayTemp);
                        }
                        Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, rc.Remote.Value, false);
                        KickPlayer(rc, "RelayClientJoin Failure. param.roomJoin Unknown");
                        return false;
                    }

                    ConcurrentDictionary<RemoteID, CPlayer> relayTemp_;
                    if (RemoteClients.TryGetValue(remoteS, out relayTemp_))
                    {
                        relayTemp_.TryAdd(rc.Remote.Value, rc);
                    }
                    else
                    {
                        relayTemp_ = new ConcurrentDictionary<RemoteID, CPlayer>();
                        relayTemp_.TryAdd(rc.Remote.Value, rc);
                        RemoteClients.TryAdd(rc.Remote.Key, relayTemp_);
                    }
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("RelayClientJoin. Failure. Player:{0}, e:{1}", rc.data.userID, e.ToString());
                    Proxy.RelayCloseRemoteClient(remoteS, CPackOption.Basic, remoteC);
                }
            }
            return true;
        }
        public bool RelayClientLeave(RemoteID remote, CPackOption rmiContext, RemoteID remoteC, bool bMoveServer)
        {
            lock (RoomServerLocker)
            {
                CPlayer rc;
                ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
                if (RemoteClients.TryGetValue(remote, out relayTemp))
                    if (relayTemp.TryGetValue(remoteC, out rc))
                    {
                        CGameRoom room_join;
                        if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                        {
                            lock (room_join.Locker)
                            {
                                // 서버 이동
                                if (bMoveServer == true)
                                {
                                    if (room_join.status == RoomStatus.Stay) // 대기중 퇴장
                                    {
                                    }
                                    else if (room_join.status == RoomStatus.PracticeGamePlay) // 연습게임중 퇴장
                                    {
                                        if (rc.status == UserStatus.RoomPlay) // 방장
                                        {
                                            // 연습게임 취소하고 퇴장
                                            room_join.PracticeGameEnd();
                                        }
                                        else // 관전자
                                        {
                                        }
                                    }
                                    else if (room_join.status == RoomStatus.GamePlay) // 게임중 퇴장
                                    {
                                        // 방 초기화
                                        room_join.player_room_out_gameinit(rc);
                                        Log._log.ErrorFormat("ClientLeave None1. ID:{0}, player:{1}, room:{2}-{3}\n", rc.data.userID, rc.status, room_join.status, room_join.roomNumber);
                                    }
                                    else // 비정상퇴장
                                    {
                                        DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.비정상종료, "");
                                        Log._log.ErrorFormat("ClientLeave None2. ID:{0}, player:{1}, room:{2}\n", rc.data.userID, rc.status, room_join.status);
                                    }
                                }
                                else // 강제 종료
                                {
                                    if (room_join.status == RoomStatus.Stay) // 아직 게임시작 안했으면 퇴장
                                    {
                                        room_join.player_room_out(rc);
                                    }
                                    else if (room_join.status == RoomStatus.PracticeGamePlay) // 연습게임
                                    {
                                        if (rc.status == UserStatus.RoomPlay) // 방장
                                        {
                                            // 연습게임 취소하고 퇴장
                                            room_join.PracticeGameEnd();
                                            room_join.player_room_out(rc);
                                        }
                                        else // 관전자
                                        {
                                            room_join.player_room_out(rc);
                                        }
                                    }
                                    else if (room_join.status == RoomStatus.GamePlay) // 게임중 퇴장 (자동치기)
                                    {
                                        rc.agent.setDummyPlayer();
                                        rc.status = UserStatus.RoomPlayAuto;
                                        DB_User_AutoPlay(rc.data.ID, room_join.roomNumber);

                                        DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.자동치기, "");
                                        Log._log.InfoFormat("DummyClient {0} Online2.\n", rc.data.userID);
                                        CPlayer dummy;
                                        relayTemp.TryRemove(remoteC, out dummy);
                                        return true;
                                    }
                                    else // 비정상퇴장
                                    {
                                        DBLog(rc.data.ID, ChannelID, room_join.roomNumber, LOG_TYPE.비정상종료, "");
                                        Log._log.ErrorFormat("[※]ClientLeave None3. ID:{0}, player:{1}, room:{2}\n", rc.data.userID, rc.status, room_join.status);
                                    }
                                }
                                room_join.player_room_out(rc);
                                CheckRoomCount(room_join);
                                Proxy.RoomLobbyOutRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, rc.data.ID);
                            }
                        }

                        // 서버 이동
                        if (bMoveServer == true)
                        {

                        }
                        else
                        {
                            DB_User_Logout(rc.data.ID);
                        }
                        CPlayer temp;
                        relayTemp.TryRemove(remoteC, out (temp));
                    }
                    else
                    {
                        Log._log.FatalFormat("RelayClientLeave2. {0},{1}", remoteC, bMoveServer);
                    }
                else
                {
                    Log._log.FatalFormat("RelayClientLeave1. {0},{1}", remoteC, bMoveServer);
                }
            }
            return true;
        }
        bool RelayRequestOutRoom(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                if (relayTemp.TryGetValue(remoteC, out rc))
                {
                    CGameRoom room_join;
                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                    {
                        lock (room_join.Locker)
                        {
                            if (room_join.status != RoomStatus.GamePlay) // 대기중일때 퇴장
                            {
                                RelayServerInfo relayInfo = LobbyInfo;
                                if (RemoteRelays.TryGetValue(rc.RelayID, out relayInfo))
                                {
                                    Proxy.GameRelayResponseRoomOut(remoteS, CPackOption.Basic, remoteC, true);

                                    // 이동 파라미터 구성
                                    ArrByte param_buffer;
                                    Common.MoveParam param = new Common.MoveParam();
                                    param.From(Common.MoveParam.ParamMove.MoveToLobby, Common.MoveParam.ParamRoom.RoomNull, Guid.Empty, 0, ChannelID, 0, "", rc.RelayID);
                                    Common.Common.ServerMoveParamWrite(param, rc, out param_buffer);

                                    Proxy.RoomRelayServerMoveStart(remoteS, CPackOption.Basic, remoteC, relayInfo.Addr, param_buffer, param.room_id);
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Proxy.RelayResponseOutRoom(remoteS, CPackOption.Basic, remoteC, true);
                        //Log._log.InfoFormat("RelayRequestOutRoom Success2. player:{0}", rc.data.userID);
                        return true;
                    }
                }

            Server.Engine.UserData userData;
            if (rc == null)
            {
                userData = new UserData();
            }
            else
            {
                userData = rc.data;
            }

            //Log._log.ErrorFormat("RelayRequestOutRoom Failure. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
            Proxy.RelayResponseOutRoom(remoteS, CPackOption.Basic, remoteC, false);
            return false;
        }
        bool RelayRequestMoveRoom(RemoteID remoteS, CPackOption rmiContext, RemoteID remoteC)
        {
            CPlayer rc = null;
            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                if (relayTemp.TryGetValue(remoteC, out rc))
                {
                    CGameRoom room_join;
                    if (RemoteRooms.TryGetValue(rc.roomID, out room_join))
                    {
                        lock (room_join.Locker)
                        {
                            if (room_join.status != RoomStatus.GamePlay) // 대기중일때 퇴장
                            {
                                Proxy.RoomLobbyRequestMoveRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, remoteS, remoteC, rc.data.ID, rc.agent.haveMoney, rc.data.IPFree, rc.data.ShopFree, rc.data.shopId);
                                //Log._log.InfoFormat("RelayRequestMoveRoom Success. player:{0}", rc.data.userID);
                                return true;
                            }
                            else
                            {
                                //Log._log.WarnFormat("RelayRequestMoveRoom Cancel. room_join.status:{0}, player:{1}", room_join.status, rc.data.userID);
                            }
                        }
                    }
                }

            //Log._log.ErrorFormat("RelayRequestMoveRoom Failure. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
            Proxy.RelayResponseMoveRoom(remoteS, CPackOption.Basic, remoteC, false, "잠시후 다시 시도하세요.");
            return false;
        }
        bool LobbyRoomResponseMoveRoom(RemoteID remoteLobby, CPackOption rmiContext, bool makeRoom, System.Guid roomID, NetAddress addr, int chanID, RemoteID remoteS, RemoteID remoteC, string message)
        {
            bool result = true;
            string errorMessage = message;
            CPlayer rc = null;

            ConcurrentDictionary<RemoteID, CPlayer> relayTemp;
            if (RemoteClients.TryGetValue(remoteS, out relayTemp))
                relayTemp.TryGetValue(remoteC, out rc);

            if (rc == null)
            {
                Log._log.ErrorFormat("LobbyRoomResponseMoveRoom rc Null. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
                errorMessage = "잠시후 다시 시도하세요.";
                Proxy.RelayResponseMoveRoom(remoteS, CPackOption.Basic, remoteC, false, errorMessage);
                return false;
            }

            if (ChannelID != chanID || roomID == Guid.Empty)
            {
                errorMessage = "잠시후 다시 시도하세요.";
                Proxy.RelayResponseMoveRoom(remoteS, CPackOption.Basic, remoteC, false, errorMessage);
                return false;
            }

            CGameRoom room_cur;
            if (RemoteRooms.TryGetValue(rc.roomID, out room_cur) == false)
            {
                errorMessage = "잠시후 다시 시도하세요.";
                Proxy.RelayResponseMoveRoom(remoteS, CPackOption.Basic, remoteC, false, errorMessage);
                return false;
            }

            // 방 이동하기 전에 퇴장
            lock (room_cur.Locker)
            {
                if (room_cur.status == RoomStatus.Stay) // 대기중 퇴장
                {
                    room_cur.player_room_out(rc);
                    CheckRoomCount(room_cur);
                    Proxy.RoomLobbyOutRoom((RemoteID)room_cur.remote_lobby, CPackOption.Basic, room_cur.roomID, rc.data.ID);
                }
                else if (room_cur.status == RoomStatus.PracticeGamePlay) // 연습게임중 퇴장
                {
                    if (rc.status == UserStatus.RoomPlay) // 방장
                    {
                        // 연습게임 취소하고 퇴장
                        room_cur.PracticeGameEnd();

                        room_cur.player_room_out(rc);
                        CheckRoomCount(room_cur);
                        Proxy.RoomLobbyOutRoom((RemoteID)room_cur.remote_lobby, CPackOption.Basic, room_cur.roomID, rc.data.ID);
                    }
                    else // 관전자
                    {
                        room_cur.player_room_out(rc);
                        CheckRoomCount(room_cur);
                        Proxy.RoomLobbyOutRoom((RemoteID)room_cur.remote_lobby, CPackOption.Basic, room_cur.roomID, rc.data.ID);
                    }
                }
                else // 비정상퇴장
                {
                    errorMessage = "잠시후 다시 시도하세요.";
                    result = false;
                }
            }

            int stake = room_cur.stake;
            int lobby_remote = room_cur.remote_lobby;

            if (result == true && errorMessage == "")
            {
                if (makeRoom)
                {
                    rc.Reset();
                    rc.Operator = true;
                    if (RoomNumbers.Count == 0)
                    {
                        Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, false);
                        KickPlayer(rc, "LobbyRoomResponseMoveRoom RoomNumbers Count 0"); // relay
                        return false;
                    }

                    CGameRoom new_room = new CGameRoom(this);
                    lock (new_room.Locker)
                    {
                        rc.player_index = new_room.pop_players_index();
                        rc.agent = new CPlayerAgent(rc.player_index, new_room);
                        if (ChanType == ChannelType.Charge)
                            rc.agent.setMoney(rc.data.money_pay);
                        else
                            rc.agent.setMoney(rc.data.money_free);
                        rc.roomID = roomID;
                        //rc.agent.topMission = rc.data.topMission;

                        new_room.From(chanID, roomID, RoomNumbers.Pop(), stake, (int)this.ServerHostID, lobby_remote);
                        RemoteRooms.TryAdd(roomID, new_room);

                        new_room.PlayersConnect.TryAdd(rc.Remote, rc);

                        Rmi.Marshaler.RoomInfo new_roominfo = new Rmi.Marshaler.RoomInfo();
                        new_roominfo.roomID = new_room.roomID;
                        new_roominfo.chanID = chanID;
                        new_roominfo.chanType = (int)ChanType;
                        new_roominfo.chanFree = ChanFree;
                        new_roominfo.number = new_room.roomNumber;
                        new_roominfo.stakeType = stake;
                        new_roominfo.baseMoney = GetStakeMoney(ChanKind, stake);
                        new_roominfo.minMoney = GetMinimumMoney(ChanKind, stake);
                        new_roominfo.maxMoney = GetMaximumMoney(ChanKind, stake);
                        new_roominfo.userCount = 1;
                        new_roominfo.restrict = false;
                        new_roominfo.remote_svr = new_room.remote_svr;
                        new_roominfo.remote_lobby = new_room.remote_lobby;
                        new_roominfo.needPassword = false;
                        new_roominfo.roomPassword = "";

                        Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                        userInfo.nickName = rc.data.nickName;
                        userInfo.FreeMoney = rc.data.money_free;
                        userInfo.PayMoney = rc.data.money_pay;
                        userInfo.chanID = chanID;
                        userInfo.roomNumber = new_room.roomNumber;

                        DB_Room_Insert(chanID, new_room.roomID, new_room.roomNumber, new_room.engine.baseMoney, rc.data.ID);
                        // 로비서버에게 방생성을 알린다.
                        Proxy.RoomLobbyMakeRoom((RemoteID)new_room.remote_lobby, CPackOption.Basic, new_roominfo, userInfo, rc.data.ID, rc.m_ip, "", rc.data.shopId);
                        // 클라이언트 입장시킴
                        Proxy.RelayResponseMoveRoom(remoteS, CPackOption.Basic, remoteC, true, "");
                        Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, true);
                    }
                    return true;
                }
                else
                {
                    //방입장
                    CGameRoom room_join;

                    if (RemoteRooms.TryGetValue(roomID, out room_join))
                    {
                        rc.Reset();
                        rc.Operator = false;
                        lock (room_join.Locker)
                        {
                            if (room_join.PlayersConnect.Count >= CGameRoom.max_users) // 인원수 초과시
                            {
                                Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, false);
                                KickPlayer(rc, "LobbyRoomResponseMoveRoom max_users"); // relay
                                return false;
                            }

                            rc.player_index = room_join.pop_players_index();
                            rc.agent = new CPlayerAgent(rc.player_index, room_join);
                            if (ChanType == ChannelType.Charge)
                                rc.agent.setMoney(rc.data.money_pay);
                            else
                                rc.agent.setMoney(rc.data.money_free);
                            rc.roomID = room_join.roomID;
                            //rc.agent.topMission = rc.data.topMission;

                            room_join.PlayersConnect.TryAdd(rc.Remote, rc);
                            Rmi.Marshaler.LobbyUserList userInfo = new Rmi.Marshaler.LobbyUserList();
                            userInfo.nickName = rc.data.nickName;
                            userInfo.FreeMoney = rc.data.money_free;
                            userInfo.PayMoney = rc.data.money_pay;
                            userInfo.chanID = chanID;
                            userInfo.roomNumber = room_join.roomNumber;

                            DB_Room_Update(room_join.roomID, room_join.PlayersConnect, chanID, rc.data.ID, room_join.roomNumber);
                            // 로비서버에게 방입장을 알린다.
                            Proxy.RoomLobbyJoinRoom((RemoteID)room_join.remote_lobby, CPackOption.Basic, room_join.roomID, userInfo, rc.data.ID, rc.m_ip, rc.data.shopId);
                            // 클라이언트 입장시킴
                            Proxy.RelayResponseMoveRoom(remoteS, CPackOption.Basic, remoteC, true, "");
                            Proxy.GameRelayRoomIn(remoteS, CPackOption.Basic, remoteC, true);
                        }
                        return true;
                    }
                    else
                    {
                        errorMessage = "잠시후 다시 시도하세요.";
                        result = false;
                    }
                }

            }
            else
            {
                Proxy.RelayResponseMoveRoom(remoteS, CPackOption.Basic, remoteC, result, errorMessage);
            }

            return false;
        }
        bool RelayServerMoveFailure(RemoteID remote, CPackOption rmiContext, RemoteID remoteC)
        {
            Log._log.InfoFormat("RelayServerMoveFailure. remote:{0}, remoteC:{1}", remote, remoteC);
            return true;
        }
        bool RelayGameRoomIn(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameRoomInUser);
        }
        bool RelayGameReady(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameReady);
        }
        bool RelayGameSelectOrder(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameSelectOrder);
        }
        bool RelayGameDistributedEnd(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameDistributedEnd);
        }
        bool RelayGameActionPutCard(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameActionPutCard);
        }
        bool RelayGameActionFlipBomb(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameActionFlipBomb);
        }
        bool RelayGameActionChooseCard(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameActionChooseCard);
        }
        bool RelayGameSelectKookjin(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameSelectKookjin);
        }
        bool RelayGameSelectGoStop(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameSelectGoStop);
        }
        bool RelayGameSelectPush(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GameSelectPush);
        }
        bool RelayGamePractice(RemoteID remote, CPackOption rmiContext, RemoteID userRemote, CMessage data)
        {
            return ProcessGame(remote, userRemote, data, SS.Common.GamePractice);
        }
        #endregion

        #region DB
        void DB_User_CurrentUpdate(int CID, int userId, int RoomId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.UpdateByUserId(UserId: userId, RoomId: RoomId, ChannelId: CID);

                    //Log._log.InfoFormat("{0} Success.", MethodBase.GetCurrentMethod().Name);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_User_Logout(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.DeleteByUserId(UserId: userId);

                    //Log._log.InfoFormat("DB_User_Logout Success. userId:{0}", userId);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_User_Logout Failure. userId:{0}, e:{1}", userId, e.ToString());
                }
            });
        }
        void DB_User_Dummyout(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentDummy.DeleteByUserId(UserId: userId);

                    //Log._log.InfoFormat("DB_User_Dummyout Success. userId:{0}", userId);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("DB_User_Dummyout Failure. userId:{0}, e:{1}", userId, e.ToString());
                }
            });
        }
        void DB_User_AutoPlay(int userId, int roomId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.DeleteByUserId(UserId: userId);
                    db.GameCurrentDummy.Insert(UserId: userId, Locate: 0, GameId: GameId, ChannelId: ChannelID, RoomId: roomId);

                    //Log._log.InfoFormat("{0} Success.", MethodBase.GetCurrentMethod().Name);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Room_Insert(int CID, Guid roomID, int roomNumber, int baseMoney, int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameCurrentUser.UpdateByUserId(UserId: userId, RoomId: roomNumber, ChannelId: CID);
                    db.GameRoomList.Insert(Id: roomID, GameId: GameId, ChannelId: CID, RoomNumber: roomNumber, BetMoney: baseMoney, UserId1: userId, UserValue1: 0, UserDate1: DateTime.Now);

#if debug
                    Log._log.InfoFormat("{0} Success.", MethodBase.GetCurrentMethod().Name);
#endif
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Room_InsertAll(int CID, Guid roomID, int roomNumber, int baseMoney, ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer> players)
        {
            int userId1 = 0;
            long userValue1 = 0;
            DateTime? userDate1 = null;
            int userId2 = 0;
            long userValue2 = 0;
            DateTime? userDate2 = null;

            foreach (var player in players)
            {
                switch (player.Value.player_index)
                {
                    case 0:
                        userId1 = player.Value.data.ID;
                        userValue1 = player.Value.agent.money_var;
                        userDate1 = player.Value.roomTime;
                        break;
                    case 1:
                        userId2 = player.Value.data.ID;
                        userValue2 = player.Value.agent.money_var;
                        userDate2 = player.Value.roomTime;
                        break;
                }
            }

            Task.Run(() =>
            {
                try
                {
                    foreach (var player in players)
                    {
                        if (player.Value.isPracticeDummy == true) continue;
                        db.GameCurrentUser.Insert(UserId: player.Value.data.ID, Locate: 0, GameId: GameId, ChannelId: CID, RoomId: roomNumber, IP: player.Value.m_ip, AutoPlay: player.Value.status == UserStatus.RoomPlayAuto);
                    }

                    db.GameRoomList.Insert(Id: roomID, GameId: GameId, ChannelId: CID, RoomNumber: roomNumber, BetMoney: baseMoney, UserId1: userId1, UserValue1: userValue1, UserId2: userId2, UserValue2: userValue2);

#if debug
                    Log._log.InfoFormat("{0} Success.", MethodBase.GetCurrentMethod().Name);
#endif
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        public void DB_Room_Update(Guid roomID, ConcurrentDictionary<KeyValuePair<RemoteID, RemoteID>, CPlayer> players, int CID = 0, int userId = 0, int RoomId = 0)
        {
            int userId1 = 0;
            long userValue1 = 0;
            DateTime? userDate1 = null;
            int userId2 = 0;
            long userValue2 = 0;
            DateTime? userDate2 = null;

            foreach (var player in players)
            {
                switch (player.Value.player_index)
                {
                    case 0:
                        userId1 = player.Value.data.ID;
                        userValue1 = player.Value.agent.money_var;
                        userDate1 = player.Value.roomTime;
                        break;
                    case 1:
                        userId2 = player.Value.data.ID;
                        userValue2 = player.Value.agent.money_var;
                        userDate2 = player.Value.roomTime;
                        break;
                }
            }

            Task.Run(() =>
            {
                try
                {
                    if (CID != 0)
                    {
                        db.GameCurrentUser.UpdateByUserId(UserId: userId, RoomId: RoomId, ChannelId: CID);
                    }
                    db.GameRoomList.UpdateById(Id: roomID, UserId1: userId1, UserValue1: userValue1, UserDate1: userDate1, UserId2: userId2, UserValue2: userValue2, UserDate2: userDate2);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Room_Delete(Guid roomID)
        {
            Task.Run(() =>
            {
                try
                {
                    db.GameRoomList.DeleteById(Id: roomID);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }
            });
        }
        void DB_Server_GetRoomData()
        {
            // 서버 설정값 확인
            Task.Run(() =>
            {
                try
                {
                    dynamic Data_DealFee = db.GameDealFee.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();

                    DealerFee = (double)Data_DealFee.DelerFee;
                    JackPotRate = (double)Data_DealFee.JackPotRate;

                    dynamic Data_JackPot = db.GameJackPotSet.FindAllBy(GameId: this.GameId, ChannelId: ChannelID).FirstOrDefault();
                    EventTermX200 = Data_JackPot.Multiple200;

                    dynamic Data_Push = db.GameMatgoPush.FindAllBy(ChannelId: ChannelID).FirstOrDefault();
                    PushType = Data_Push.PushBaseType;

                    dynamic Data_PushFirst = db.GameMatgoPushFirst.All();
                    foreach (var row in Data_PushFirst.ToList())
                    {

                        short temp;
                        if (BonusPushFirst.TryGetValue(row.Level, out temp))
                        {
                            BonusPushFirst[row.Level] = row.Bonus;
                        }
                        else
                        {
                            BonusPushFirst.Add(row.Level, row.Bonus);
                        }
                        if (BonusPushChance.TryGetValue(row.Level, out temp))
                        {
                            BonusPushChance[row.Level] = row.Chance;
                        }
                        else
                        {
                            BonusPushChance.Add(row.Level, row.Chance);
                        }
                    }

                    dynamic Data_PushBase = db.GameMatgoPushBase.All();
                    foreach (var row in Data_PushBase.ToList())
                    {
                        long temp;
                        if (BonusBase.TryGetValue(row.Type, out temp))
                        {
                            BonusBase[row.Type] = row.BonusCard;
                        }
                        else
                        {
                            BonusBase.Add(row.Type, row.BonusCard);
                        }
                    }

                    dynamic Data_PushUser = db.GameMatgoPushUser.All();
                    foreach (var row in Data_PushUser.ToList())
                    {
                        long temp;
                        if (BonusPushUser.TryGetValue(row.UserLevel, out temp))
                        {
                            BonusPushUser[row.UserLevel] = row.Push;
                        }
                        else
                        {
                            BonusPushUser.Add(row.UserLevel, row.Push);
                        }
                    }

                    dynamic Data_PushGame = db.GameMatgoPushGame.All();
                    foreach (var row in Data_PushGame.ToList())
                    {
                        long temp;
                        if (BonusPushGame.TryGetValue(row.GameLevel, out temp))
                        {
                            BonusPushGame[row.GameLevel] = row.Push;
                        }
                        else
                        {
                            BonusPushGame.Add(row.GameLevel, row.Push);
                        }
                    }

                    dynamic Data_Mission = db.GameMatgoMission.All();
                    foreach (var row in Data_Mission.ToList())
                    {
                        sMission temp;
                        if (MissionData.TryGetValue((MISSION)row.MissionId, out temp))
                        {
                            temp.Id = row.MissionId;
                            temp.Name = row.MissionName;
                            temp.Reward = row.MissionReward;
                            temp.Rate = row.MissionRate;
                            //MissionData[(MISSION)temp.Id] = temp;
                        }
                        else
                        {
                            temp.Id = row.MissionId;
                            temp.Name = row.MissionName;
                            temp.Reward = row.MissionReward;
                            temp.Rate = row.MissionRate;
                            MissionData.Add((MISSION)temp.Id, temp);
                        }

                        int temp2;
                        if (MissionRate.TryGetValue((MISSION)row.MissionId, out temp2))
                        {
                            MissionRate[(MISSION)row.MissionId] = row.MissionRate;
                        }
                        else
                        {
                            MissionRate.Add((MISSION)row.MissionId, row.MissionRate);
                        }
                    }

                    Log._log.InfoFormat("{0} Success.", MethodBase.GetCurrentMethod().Name);
                }
                catch (Exception e)
                {
                    Log._log.FatalFormat("{0} Failure. e:{1}", MethodBase.GetCurrentMethod().Name, e.ToString());
                }

            });
        }
        #endregion
    }
}
