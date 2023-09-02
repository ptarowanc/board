using Server.Common;
using Server.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZNet;

namespace Server.User
{
    public class RelayServer : UserServer
    {
        // 릴레이 서버 ID
        public int RelayID;

        // 클라이언트 목록
        public ConcurrentDictionary<ZNet.RemoteID, CPlayer> RemoteClients;
        // 게임방 목록
        ConcurrentDictionary<Guid, RemoteClass.Marshaler.RoomInfo> RemoteRoomInfos;
        // 게임서버 목록
        public ConcurrentDictionary<int, ZNet.RemoteID> RemoteRoomServers;
        // <유저, 게임서버> 포인트
        ConcurrentDictionary<ZNet.RemoteID, ZNet.RemoteID> RemoteRelays;

        Random rnd = new Random((int)DateTime.UtcNow.Ticks);

        public RelayServer(FormServer f, UnityCommon.Server t, int portnum, int relayID) : base(f, t, portnum)
        {
            // 중계 번호
            RelayID = relayID;
            RemoteClients = new ConcurrentDictionary<RemoteID, CPlayer>();
            RemoteRoomInfos = new ConcurrentDictionary<Guid, RemoteClass.Marshaler.RoomInfo>();
            RemoteRoomServers = new ConcurrentDictionary<int, RemoteID>();
            RemoteRelays = new ConcurrentDictionary<RemoteID, RemoteID>();
        }

        protected override void BeforeServerStart(out StartOption param)
        {
            base.BeforeServerStart(out param);

            param.m_UpdateTimeMs = 1000;

            // 마스터서버
            stub.master_all_shutdown = ShutDownServer;

            stub.relayServer = this;
            // 로비 <=> 룸
            //stub.lobby_room_jackpot_info = RefreshJackpotInfo;
            //stub.lobby_room_notify_message = NotifyMessage;
            //stub.lobby_room_notify_servermaintenance = NotifyServerMaintenance;
            //stub.lobby_room_reload_serverdata = ReloadServerData;
            //stub.lobby_room_kick_player = KickPlayer;
            //stub.KickSession = KickSession;
            //stub.lobby_room_moveroom_response = LobbyRoomMoveRoom;
            //stub.lobby_room_current_request = CurrentRooms;

            //----서버 메시지----
            m_Core.move_server_start_handler = MoveServerStart;
            m_Core.move_server_param_handler = MoveServerParam;
            m_Core.move_server_failed_handler = MoveServerFailed;
            m_Core.client_join_handler = ClientJoin; // 클라 -> 릴레이 -> 룸
            m_Core.client_leave_handler = ClientLeave; // 클라 -> 릴레이 -> 룸
            m_Core.message_handler = CoreMessage;
            m_Core.exception_handler = CoreException;
            m_Core.server_join_handler = ServerJoin;
            m_Core.server_leave_handler = ServerLeave;
            m_Core.server_master_join_handler = ServerMasterJoin;
            m_Core.server_master_leave_handler = ServerMasterLeave;
            m_Core.server_refresh_handler = ServerRefresh;

            // 클라이언트
            stub.request_out_room = RequestOutRoom;
            stub.request_move_room = RequestMoveRoom;
            stub.RelayResponseOutRoom = RelayResponseOutRoom;
            stub.RelayResponseMoveRoom = RelayResponseMoveRoom;

            // 릴레이 서버 메시지
            stub.room_lobby_makeroom = RoomLobbyMakeRoom;
            stub.room_lobby_joinroom = RoomLobbyJoinRoom;
            stub.room_lobby_outroom = RoomLobbyOutRoom;
            stub.RelayCloseRemoteClient = RelayCloseRemoteClient; // -> 룸 -> 릴레이 -> 클라
            stub.lobby_room_jackpot_info = RefreshJackpotInfo; // 로비 -> 릴레이 -> 클라
            stub.lobby_room_notify_message = NotifyMessage; // 로비 -> 릴레이 -> 클라

            stub.room_lobby_current_response = RoomRelayCurrent;
            stub.lobby_room_current_request = CurrentRelays;

            // 서버 접속제한시점의 이벤트
            m_Core.limit_connection_handler = (ZNet.RemoteID remote, ZNet.NetAddress addr) =>
            {
                form.printf("limit_connection_handler {0}, {1} is Leave.\n", remote, addr.m_ip, addr.m_port);
            };
        }

        public override void NetLoop(object sender, ElapsedEventArgs e)
        {
            m_Core.NetLoop();
        }

        private void ClientDisconect(RemoteID remote)
        {
            ClientLeave(remote, false);
            m_Core.CloseRemoteClient(remote);
            Log._log.WarnFormat("Player Disconect. remote:{0}", remote);
        }

        private void MoveServerStart(RemoteID remote, out ArrByte userdata)
        {
            // 이동하기 전에 게임서버에서 플레이어 정보 갱신한다. 갱신 안됐으면 실패하기
            CPlayer rc = null;
            userdata = null;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                Common.Common.ServerMoveStart(rc, out userdata);
            }
        }
        private bool MoveServerParam(ZNet.ArrByte move_param, int count_idx)
        {
            Common.MoveParam param;
            Common.Common.ServerMoveParamRead(move_param, out param);

            if (param.moveTo == Common.MoveParam.ParamMove.MoveToRoom)
            {
                if (param.roomJoin == Common.MoveParam.ParamRoom.RoomMake)
                {
                    return true;
                }
                else if (param.roomJoin == Common.MoveParam.ParamRoom.RoomJoin)
                {
                    RemoteClass.Marshaler.RoomInfo room_join;
                    if (RemoteRoomInfos.TryGetValue(param.room_id, out room_join))
                    {
                        if (room_join.restrict == false)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private void MoveServerFailed(ZNet.ArrByte move_param)
        {
            CPlayer rc;
            Common.Common.ServerMoveComplete(move_param, out rc);

            if (rc != null)
            {
                form.printf("MoveServerFailed. {0}", rc.data.userID);
                //DB_User_Logout(rc.data.ID);
            }
        }
        private void ClientJoin(ZNet.RemoteID remote, ZNet.NetAddress addr, ZNet.ArrByte move_server, ZNet.ArrByte move_param)
        {
            // 릴레이 서버에 접속 -> 룸서버에 접속정보 전송 후 결과를 클라이언트에게 전송
            if (move_server.Count > 0)
            {
                CPlayer rc;
                Common.Common.ServerMoveComplete(move_server, out rc);
                rc.m_ip = addr.m_ip;
                rc.remote = remote;

                Common.MoveParam param;
                Common.Common.ServerMoveParamRead(move_param, out param);

                if (RelayID != param.RelayID)
                {
                    RemoteClients.TryAdd(remote, rc);
                    ClientDisconect(remote);
                    return;
                }

                RemoteID remoteServer;
                if (RemoteRoomServers.TryGetValue(param.ChannelNumber, out remoteServer))
                {
                    // 접속하려는 채널 ID에 해당하는 서버로 연결
                    ZNet.MasterInfo[] svr_array;
                    m_Core.GetServerList((int)ServerType.Room, out svr_array);

                    if (svr_array == null)
                    {
                        RemoteClients.TryAdd(remote, rc);
                        ClientDisconect(remote);
                        return;
                    }
                    foreach (var svr in svr_array)
                    {
                        if (svr.m_remote == remoteServer)
                        {
                            RemoteClients.TryAdd(remote, rc);
                            RemoteRelays.TryAdd(remote, remoteServer);

                            proxy.RelayClientJoin(svr.m_remote, remote, addr, rc.data, param);
                            break;
                        }
                    }
                }
                else
                {
                    // 접속하려는 채널 없음.
                    Log._log.WarnFormat("Client Join Request Failed ChannelID Unknown. remote:{0}, ChannelID{1}", remote, param.ChannelNumber);
                    RemoteClients.TryAdd(remote, rc);
                    ClientDisconect(remote);
                }
            }
            else
            {
                // 서버이동 결과없이 접속은 연결 종료
                Log._log.WarnFormat("Client Join Request Failed. remote:{0}", remote);
                ClientDisconect(remote);
            }
        }
        private void ClientLeave(RemoteID remote, bool bMoveServer = false)
        {
            CPlayer rc;

            if (RemoteClients.TryGetValue(remote, out rc))
            {
                RemoteID remoteRoom;
                if (RemoteRelays.TryGetValue(remote, out remoteRoom))
                {
                    // 서버 이동
                    proxy.RelayClientLeave(remoteRoom, remote, bMoveServer);

                    RemoteRelays.TryRemove(remote, out (remoteRoom));
                }
                else
                {
                    DB_User_Logout(rc.data.ID);
                }

                RemoteClients.TryRemove(remote, out (rc));
            }
        }
        private void CoreMessage(ResultInfo resultInfo)
        {
            switch (resultInfo.m_Level)
            {
                case IResultLevel.IMsg:
                    Log._log.Info("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.IWrn:
                    Log._log.Warn("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.IErr:
                    Log._log.Error("[CoreMsg]" + resultInfo.msg);
                    break;
                case IResultLevel.ICri:
                    Log._log.Fatal("[CoreMsg]" + resultInfo.msg);
                    break;
                default:
                    Log._log.Fatal("[CoreMsg]" + resultInfo.msg);
                    break;
            }
        }
        private void CoreException(Exception e)
        {
            Log._log.Fatal("[Exception]" + e.ToString());
        }
        private void ServerJoin(RemoteID remote, NetAddress addr)
        {
            form.printf(string.Format("서버P2P맴버 입장 remoteID {0}", remote));

            // 연결된 룸, 릴레이서버로부터 정보 받기
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                ZNet.MasterInfo[] svr_array;
                m_Core.GetServerList((int)ServerType.Room, out svr_array);

                if (svr_array == null) return;

                foreach (var svr in svr_array)
                {
                    if (svr.m_remote == remote)
                    {
                        proxy.lobby_room_current_request(remote, ZNet.CPackOption.Basic, false);
                    }
                }
            });
        }
        private void ServerLeave(RemoteID remote, NetAddress addr)
        {
            form.printf(string.Format("서버P2P맴버 퇴장 remoteID {0}", remote));

            // 나간 룸서버의 룸 목록 삭제 후 목록 일괄전송
            ZNet.MasterInfo[] svr_array;
            m_Core.GetServerList((int)ServerType.Room, out svr_array);
            if ((svr_array == null) == false)
            {
                foreach (var svr in svr_array)
                {
                    if (svr.m_remote == remote)
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                foreach (var room in RemoteRoomInfos)
                                {
                                    if (room.Value.remote_svr == (int)remote)
                                    {
                                        RemoteClass.Marshaler.RoomInfo temp_;
                                        RemoteRoomInfos.TryRemove(room.Value.roomID, out temp_);
                                    }
                                }
                                foreach (var roomSrv in RemoteRoomServers)
                                {
                                    if (roomSrv.Value == remote)
                                    {
                                        RemoteID temp_;
                                        RemoteRoomServers.TryRemove(roomSrv.Key, out temp_);
                                    }
                                }
                                foreach (var remoteClient in RemoteRelays)
                                {
                                    if (remoteClient.Value == remote)
                                    {
                                        RemoteID temp_;
                                        RemoteRelays.TryRemove(remoteClient.Key, out temp_);
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                form.printf("ServerLeave {0}\n", e.ToString());
                            }
                        });
                    }
                }
            }
        }
        private void ServerMasterJoin(RemoteID remote, RemoteID myRemoteID)
        {
            //this.svrRemoteID = (int)myRemoteID;
            form.printf(string.Format("마스터서버에 입장성공 remoteID {0}", myRemoteID));
        }
        private void ServerMasterLeave()
        {
            form.printf(string.Format("마스터서버와 연결종료!!!"));
            //run_program = false;    // 자동 종료처리를 위해
        }
        private void ServerRefresh(MasterInfo master_info)
        {
            //form.printf(string.Format("서버P2P remote:{0} type:{1}[{2}] current:{3} addr:{4}:{5}",
            //master_info.m_remote,
            //(UnityCommon.Server)master_info.m_ServerType,
            //master_info.m_Description,
            //master_info.m_Clients,
            //master_info.m_Addr.m_ip,
            //master_info.m_Addr.m_port
            //));
        }

        public bool ProcessRelayToClient(ZNet.CRecvedMsg rm)
        {
            ZNet.RemoteID remoteS = rm.remote;

            ZNet.RemoteID remoteC; int remoteC_;
            RemoteClass.Marshaler.Read(rm.msg, out remoteC_);
            remoteC = (ZNet.RemoteID)remoteC_;

            ZNet.RemoteID remoteRoom;
            if (RemoteRelays.TryGetValue(remoteC, out remoteRoom))
            {
                if (remoteS != remoteRoom)
                {
                    // remote 다름
                    return false;
                }
            }

            // 클라이언트로 보낼 릴레이 패킷 변환
            ZNet.PacketType relayPID;
            switch (rm.pkID)
            {
                case Rmi.Common.RELAY_response_out_room: relayPID = Rmi.Common.response_out_room; break;
                case Rmi.Common.RELAY_SC_ROOM_GAME_STARTED: relayPID = Rmi.Common.SC_ROOM_GAME_STARTED; break;
                case Rmi.Common.RELAY_SC_PLAYER_ORDER_RESULT: relayPID = Rmi.Common.SC_PLAYER_ORDER_RESULT; break;
                case Rmi.Common.RELAY_SC_DISTRIBUTE_ALL_CARDS: relayPID = Rmi.Common.SC_DISTRIBUTE_ALL_CARDS; break;
                case Rmi.Common.RELAY_SC_START_TURN: relayPID = Rmi.Common.SC_START_TURN; break;
                case Rmi.Common.RELAY_SC_FLOOR_HAS_BONUS: relayPID = Rmi.Common.SC_FLOOR_HAS_BONUS; break;
                case Rmi.Common.RELAY_SC_SELECT_CARD_ACK: relayPID = Rmi.Common.SC_SELECT_CARD_ACK; break;
                case Rmi.Common.RELAY_SC_FLIP_DECK_CARD_ACK: relayPID = Rmi.Common.SC_FLIP_DECK_CARD_ACK; break;
                case Rmi.Common.RELAY_SC_TURN_RESULT: relayPID = Rmi.Common.SC_TURN_RESULT; break;
                case Rmi.Common.RELAY_SC_ASK_GO_OR_STOP: relayPID = Rmi.Common.SC_ASK_GO_OR_STOP; break;
                case Rmi.Common.RELAY_SC_NOTIFY_GO_COUNT: relayPID = Rmi.Common.SC_NOTIFY_GO_COUNT; break;
                case Rmi.Common.RELAY_SC_UPDATE_PLAYER_STATISTICS: relayPID = Rmi.Common.SC_UPDATE_PLAYER_STATISTICS; break;
                case Rmi.Common.RELAY_SC_ASK_KOOKJIN_TO_PEE: relayPID = Rmi.Common.SC_ASK_KOOKJIN_TO_PEE; break;
                case Rmi.Common.RELAY_SC_MOVE_KOOKJIN_TO_PEE: relayPID = Rmi.Common.SC_MOVE_KOOKJIN_TO_PEE; break;
                case Rmi.Common.RELAY_SC_GAME_RESULT: relayPID = Rmi.Common.SC_GAME_RESULT; break;

                case Rmi.Common.RELAY_SC_PUSH: relayPID = Rmi.Common.SC_PUSH; break;
                case Rmi.Common.RELAY_SC_PLAYER_ORDER_START: relayPID = Rmi.Common.SC_PLAYER_ORDER_START; break;
                case Rmi.Common.RELAY_SC_READY_TO_START: relayPID = Rmi.Common.SC_READY_TO_START; break;
                case Rmi.Common.RELAY_SC_EVENT_START: relayPID = Rmi.Common.SC_EVENT_START; break;
                case Rmi.Common.RELAY_SC_EVENT_JACKPOT_INFO: relayPID = Rmi.Common.SC_EVENT_JACKPOT_INFO; break;
                case Rmi.Common.RELAY_SC_EVENT_TAPSSAHGI_INFO: relayPID = Rmi.Common.SC_EVENT_TAPSSAHGI_INFO; break;
                case Rmi.Common.RELAY_SC_USER_INFO: relayPID = Rmi.Common.SC_USER_INFO; break;
                case Rmi.Common.RELAY_SC_PLAYER_ALLIN: relayPID = Rmi.Common.SC_PLAYER_ALLIN; break;
                case Rmi.Common.RELAY_SC_PLAYER_INDEX: relayPID = Rmi.Common.SC_PLAYER_INDEX; break;
                case Rmi.Common.RELAY_SC_ROOM_INFO: relayPID = Rmi.Common.SC_ROOM_INFO; break;
                case Rmi.Common.RELAY_SC_ROOM_OUT: relayPID = Rmi.Common.SC_ROOM_OUT; break;
                case Rmi.Common.RELAY_SC_RULEMONEY_INFO: relayPID = Rmi.Common.SC_RULEMONEY_INFO; break;
                case Rmi.Common.RELAY_SC_EVENT_JACKPOT_UPDATE: relayPID = Rmi.Common.SC_EVENT_JACKPOT_UPDATE; break;
                case Rmi.Common.RELAY_response_room_in: relayPID = Rmi.Common.response_room_in; break;
                case Rmi.Common.RELAY_room_notify_message: relayPID = Rmi.Common.room_notify_message; break;
                case Rmi.Common.RELAY_SC_OBSERVE_INFO: relayPID = Rmi.Common.SC_OBSERVE_INFO; break;
                case Rmi.Common.RELAY_SC_PRACTICE_GAME_END: relayPID = Rmi.Common.SC_PRACTICE_GAME_END; break;
                default:
                    {
                        relayPID = ZNet.PacketType.PacketType_User;
                        Log._log.ErrorFormat("ProcessRelayToClient Unknown rm.pkID:{0}", rm.pkID);
                        return false;
                    }
            }

            ZNet.CMessage relayMsg = new ZNet.CMessage();
            relayMsg.WriteStart(relayPID, rm.pkop, 0, true);
            int count = rm.msg.Count - (relayMsg.Count + 4);
            if (count > 0)
            {
                var getData = rm.msg.GetData();
                unsafe
                {
                    fixed (byte* data = getData)
                    {
                        byte* t = data + 20;
                        relayMsg.Write(t, count);
                    }
                }
            }

            proxy.PacketSend(remoteC, rm.pkop, relayMsg);

            return true;
        }

        public bool ProcessRelayToServer(ZNet.CRecvedMsg rm)
        {
            CMessage relayMsg = new CMessage();
            PacketType msgID;
            CPackOption pkOption = CPackOption.Basic;

            switch (rm.pkID)
            {
                case Rmi.Common.CS_ROOM_IN: msgID = Rmi.Common.RELAY_CS_ROOM_IN; break;
                case Rmi.Common.CS_READY_TO_START: msgID = Rmi.Common.RELAY_CS_READY_TO_START; break;
                case Rmi.Common.CS_PLAYER_ORDER_START: msgID = Rmi.Common.RELAY_CS_PLAYER_ORDER_START; break;
                case Rmi.Common.CS_DISTRIBUTED_ALL_CARDS: msgID = Rmi.Common.RELAY_CS_DISTRIBUTED_ALL_CARDS; break;
                case Rmi.Common.CS_ANSWER_KOOKJIN_TO_PEE: msgID = Rmi.Common.RELAY_CS_ANSWER_KOOKJIN_TO_PEE; break;
                case Rmi.Common.CS_ANSWER_GO_OR_STOP: msgID = Rmi.Common.RELAY_CS_ANSWER_GO_OR_STOP; break;
                case Rmi.Common.CS_PRACTICE_GAME: msgID = Rmi.Common.RELAY_CS_PRACTICE_GAME; break;
                case Rmi.Common.CS_ACTION_PUT_CARD: msgID = Rmi.Common.RELAY_CS_ACTION_PUT_CARD; break;
                case Rmi.Common.CS_ACTION_FLIP_BOMB: msgID = Rmi.Common.RELAY_CS_ACTION_FLIP_BOMB; break;
                case Rmi.Common.CS_ACTION_CHOOSE_CARD: msgID = Rmi.Common.RELAY_CS_ACTION_CHOOSE_CARD; break;
                default:
                    {
                        msgID = Rmi.Common.RELAY_CS_ERROR;
                        Log._log.ErrorFormat("ProcessRelayToServer Unknown rm.pkID:{0}", rm.pkID);
                        return false;
                    }
            }

            relayMsg.WriteStart(msgID, pkOption, 0, true);
            int count = rm.msg.Count - relayMsg.Count;
            relayMsg.Write(rm.remote);
            if (count > 0)
            {
                var getData = rm.msg.GetData();
                unsafe
                {
                    fixed (byte* data = getData)
                    {
                        byte* t = data + 16;
                        relayMsg.Write(t, count);
                    }
                }
            }

            ZNet.RemoteID remoteRoom;
            if (RemoteRelays.TryGetValue(rm.remote, out remoteRoom))
            {
                proxy.PacketSend(remoteRoom, pkOption, relayMsg);
            }
            else
            {
                // log
            }

            return true;
        }

        bool ShutDownServer(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            ServerMaintenance = true;
            ServerMsg = "서버 점검중입니다.";
            ShutDown = true;
            CountDown = DateTime.Now.AddMinutes(3);

            form.printf("서버종료 요청 받음.");
            Log._log.Warn("서버종료 요청 받음.");

            return true;
        }

        private void RequestOutRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool roomMove = false)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                ZNet.RemoteID room_join;
                if (RemoteRelays.TryGetValue(rc.remote, out room_join))
                {
                    if (rc.RoomRequestTime.AddSeconds(1) < DateTime.Now)
                    {
                        rc.RoomRequestTime = DateTime.Now;
                        proxy.RelayRequestOutRoom(room_join, remote);
                        return;
                    }
                }
            }

            proxy.room_player_out(remote, ZNet.CPackOption.Basic, false);
        }

        private void RequestMoveRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            CPlayer rc;
            if (RemoteClients.TryGetValue(remote, out rc))
            {
                ZNet.RemoteID room_join;
                if (RemoteRelays.TryGetValue(rc.remote, out room_join))
                {
                    if (rc.RoomRequestTime.AddSeconds(1) < DateTime.Now)
                    {
                        rc.RoomRequestTime = DateTime.Now;
                        proxy.RelayRequestMoveRoom(room_join, remote);
                        return;
                    }
                }
            }

            proxy.response_player_move(rc.remote, pkOption, false, "");
        }

        // 릴레이 서버 메시지
        bool RoomLobbyMakeRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, RemoteClass.Marshaler.RoomInfo roomInfo, RemoteClass.Marshaler.LobbyUserList UserInfo, int userID, string IP)
        {
            if (RemoteRoomInfos.TryAdd(roomInfo.roomID, roomInfo))
            {
                roomInfo.userList.TryAdd(userID, IP);
            }

            return true;
        }
        bool RoomLobbyJoinRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Guid roomID, RemoteClass.Marshaler.LobbyUserList UserInfo, int userID, string IP)
        {
            RemoteClass.Marshaler.RoomInfo roomInfo;
            if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
            {
                roomInfo.userList.TryAdd(userID, IP);

                ++roomInfo.userCount;
                if (roomInfo.userCount >= CGameRoom.max_users)
                {
                    roomInfo.restrict = true;
                }

                return true;
            }

            return false;
        }
        bool RoomLobbyOutRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Guid roomID, int userID)
        {
            RemoteClass.Marshaler.RoomInfo roomInfo;
            int chanID = 0;
            if (RemoteRoomInfos.TryGetValue(roomID, out roomInfo))
            {
                string temp_;
                roomInfo.userList.TryRemove(userID, out temp_);

                --roomInfo.userCount;
                roomInfo.restrict = false;
                chanID = roomInfo.chanID;

                if (roomInfo.userCount <= 0)
                {
                    RemoteClass.Marshaler.RoomInfo temp;
                    RemoteRoomInfos.TryRemove(roomID, out temp);
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        private void RelayCloseRemoteClient(RemoteID remoteS, RemoteID remoteC)
        {
            m_Core.CloseRemoteClientForce(remoteC);
            Log._log.WarnFormat("Relay Close Remote Client. remoteS:{0}, remoteC:{1}", remoteS, remoteC);
        }
        bool RefreshJackpotInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, long jackpotmoney)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Rmi.Common.SC_EVENT_JACKPOT_INFO;
            Msg.WriteStart(msgID, pkOption, 0, true);
            RemoteClass.Marshaler.Write(Msg, (long)jackpotmoney);

            foreach (var player in RemoteRelays)
            {
                proxy.PacketSend(player.Key, pkOption, Msg);
            }

            return true;
        }
        bool NotifyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int messageType, string message, int showTime)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Rmi.Common.room_notify_message;
            Msg.WriteStart(msgID, pkOption, 0, true);
            RemoteClass.Marshaler.Write(Msg, messageType);
            RemoteClass.Marshaler.Write(Msg, message);
            RemoteClass.Marshaler.Write(Msg, showTime);

            foreach (var player in RemoteRelays)
            {
                proxy.PacketSend(player.Key, pkOption, Msg);
            }
            return true;
        }
        private void RelayResponseOutRoom(RemoteID remoteS, RemoteID remoteC, bool resultOut, int channelID, Server.Engine.UserData userData)
        {
            if (resultOut)
            {
                CPlayer player;

                if (RemoteClients.TryGetValue(remoteC, out player))
                {
                    lock (player.PlayerLock)
                    {
                            player.data = userData;

                            ZNet.MasterInfo[] svr_array;
                            m_Core.GetServerList((int)ServerType.Lobby, out svr_array);
                            if (svr_array != null)
                            {
                                foreach (var obj in svr_array)
                                {
                                    ZNet.ArrByte param_buffer;
                                    Common.MoveParam param = new Common.MoveParam();
                                    param.From(Common.MoveParam.ParamMove.MoveToLobby, Common.MoveParam.ParamRoom.RoomNull, Guid.Empty, 0, channelID, 0, "", 0);
                                    Common.Common.ServerMoveParamWrite(param, out param_buffer);

                                    proxy.room_player_out(remoteC, ZNet.CPackOption.Basic, true);
                                    m_Core.ServerMoveStart(remoteC, obj.m_Addr, param_buffer, param.room_id);
                                    return;
                                }
                            }
                    }
                }
            }

            proxy.room_player_out(remoteC, ZNet.CPackOption.Basic, false);
        }
        private void RelayResponseMoveRoom(RemoteID remoteS, RemoteID remoteC, bool resultMove, string errorMessage)
        {
            if (resultMove)
            {
                proxy.response_player_move(remoteC, ZNet.CPackOption.Basic, true, "");
            }
            else
            {
                proxy.response_player_move(remoteC, ZNet.CPackOption.Basic, false, errorMessage);
            }
        }


        bool RoomRelayCurrent(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage Msg)
        {
            int chanID; RemoteClass.Marshaler.Read(Msg, out chanID);
            if (RemoteRoomServers.TryAdd(chanID, remote) == false)
            {
                //...
            }

            form.printf("룸서버 서버 입장 remote:{0}, chanID:{1}", remote, chanID);

            int RoomCount; RemoteClass.Marshaler.Read(Msg, out RoomCount);

            for (int i = 0; i < RoomCount; ++i)
            {
                RemoteClass.Marshaler.RoomInfo roominfo = new RemoteClass.Marshaler.RoomInfo();

                RemoteClass.Marshaler.Read(Msg, out roominfo.roomID);
                RemoteClass.Marshaler.Read(Msg, out roominfo.chanID);
                RemoteClass.Marshaler.Read(Msg, out roominfo.chanType);
                RemoteClass.Marshaler.Read(Msg, out roominfo.number);
                RemoteClass.Marshaler.Read(Msg, out roominfo.stakeType);
                RemoteClass.Marshaler.Read(Msg, out roominfo.userCount);
                RemoteClass.Marshaler.Read(Msg, out roominfo.restrict);
                RemoteClass.Marshaler.Read(Msg, out roominfo.remote_svr);
                RemoteClass.Marshaler.Read(Msg, out roominfo.remote_lobby);
                RemoteClass.Marshaler.Read(Msg, out roominfo.needPassword);
                RemoteClass.Marshaler.Read(Msg, out roominfo.roomPassword);

                for (int j = 0; j < roominfo.userCount; ++j)
                {
                    string IP;
                    RemoteClass.Marshaler.LobbyUserList userInfo = new RemoteClass.Marshaler.LobbyUserList();
                    //
                    RemoteClass.Marshaler.Read(Msg, out userInfo.ID);
                    RemoteClass.Marshaler.Read(Msg, out IP);
                    roominfo.userList.TryAdd(userInfo.ID, IP);

                    RemoteClass.Marshaler.Read(Msg, out userInfo.RemoteID);
                    RemoteClass.Marshaler.Read(Msg, out userInfo.nickName);
                    RemoteClass.Marshaler.Read(Msg, out userInfo.FreeMoney);
                    RemoteClass.Marshaler.Read(Msg, out userInfo.PayMoney);
                    userInfo.chanID = roominfo.chanID;
                    userInfo.roomNumber = roominfo.number;
                }

                RemoteRoomInfos.TryAdd(roominfo.roomID, roominfo);
            }

            return true;
        }
        bool CurrentRelays(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool IsLobby)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Rmi.Common.relay_lobby_current_response;
            Msg.WriteStart(msgID, pkOption, 0, true);

            RemoteClass.Marshaler.Write(Msg, (int)RelayID); // 릴레이 서버 ID

            proxy.PacketSend(remote, pkOption, Msg);

            return true;
        }

        void DB_User_Logout(int userId)
        {
            Task.Run(() =>
            {
                try
                {
                    Simple.Data.Database.Open().GameCurrentUser.DeleteByUserId(UserId: userId);
                }
                catch (Exception e)
                {
                    form.printf("DB_User_Logout 예외발생 {0}\n", e.ToString());
                }
            });
        }

        public override void PrintLog(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.F1:
                    {
                        form.printf(string.Format("접속세션: {0}, 방:{1}, 룸서버:{2}, 릴레이:{3}", RemoteClients.Count, RemoteRoomInfos.Count, RemoteRoomServers.Count, RemoteRelays.Count));
                    }
                    break;
                case System.Windows.Forms.Keys.F2:
                    {
                        form.printf(string.Format("접속 세션 디테일: {0}", RemoteClients.Count));
                        string sLog = "";
                        foreach (var Value in RemoteClients.Values)
                        {
                            sLog += string.Format("접속 플레이어: {0} \r\n", Value.data.userID);
                        }
                        form.printf(sLog);
                    }
                    break;
                case System.Windows.Forms.Keys.F3:
                    {
                        form.printf(string.Format("방 디테일: {0}", RemoteRoomInfos.Count));
                        string sLog = "";
                        foreach (var Value in RemoteRoomInfos.Values)
                        {
                            sLog += string.Format("방 번호: {0} \r\n", Value.number);
                            sLog += string.Format("유저 수: {0} \r\n", Value.userCount);
                        }
                        form.printf(sLog);
                    }
                    break;
            }
        }
    }
}
