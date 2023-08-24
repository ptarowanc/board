// Auto created from IDLCompiler.exe
using System;
using System.Collections.Generic;
using System.Net;


namespace SS
{

    public class Proxy : ZNet.PKProxy
    {
        public bool MasterAllShutdown(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.MasterAllShutdown;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, msg);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool MasterAllShutdown(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string msg)
        {
            foreach (var obj in remotes)
                MasterAllShutdown(obj, pkOption, msg);
            return true;
        }

        public bool MasterNotifyP2PServerInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.MasterNotifyP2PServerInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool MasterNotifyP2PServerInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                MasterNotifyP2PServerInfo(obj, pkOption, data);
            return true;
        }

        public bool P2PMemberCheck(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.P2PMemberCheck;

            Msg.WriteStart(msgID, pkOption, 0, true);


            return PacketSend(remote, pkOption, Msg);
        }

        public bool P2PMemberCheck(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption)
        {
            foreach (var obj in remotes)
                P2PMemberCheck(obj, pkOption);
            return true;
        }

        public bool RoomLobbyMakeRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.RoomInfo roomInfo, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, string Pass, int shopId)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyMakeRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, roomInfo);
            Rmi.Marshaler.Write(Msg, userInfo);
            Rmi.Marshaler.Write(Msg, userID);
            Rmi.Marshaler.Write(Msg, IP);
            Rmi.Marshaler.Write(Msg, Pass);
            Rmi.Marshaler.Write(Msg, shopId);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyMakeRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, Rmi.Marshaler.RoomInfo roomInfo, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, string Pass, int shopId)
        {
            foreach (var obj in remotes)
                RoomLobbyMakeRoom(obj, pkOption, roomInfo, userInfo, userID, IP, Pass, shopId);
            return true;
        }

        public bool RoomLobbyJoinRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, int shopId)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyJoinRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, roomID);
            Rmi.Marshaler.Write(Msg, userInfo);
            Rmi.Marshaler.Write(Msg, userID);
            Rmi.Marshaler.Write(Msg, IP);
            Rmi.Marshaler.Write(Msg, shopId);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyJoinRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, System.Guid roomID, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, int shopId)
        {
            foreach (var obj in remotes)
                RoomLobbyJoinRoom(obj, pkOption, roomID, userInfo, userID, IP, shopId);
            return true;
        }

        public bool RoomLobbyOutRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int userID)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyOutRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, roomID);
            Rmi.Marshaler.Write(Msg, userID);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyOutRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, System.Guid roomID, int userID)
        {
            foreach (var obj in remotes)
                RoomLobbyOutRoom(obj, pkOption, roomID, userID);
            return true;
        }

        public bool RoomLobbyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, message);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message)
        {
            foreach (var obj in remotes)
                RoomLobbyMessage(obj, pkOption, userRemote, message);
            return true;
        }

        public bool RoomLobbyEventStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int type)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyEventStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, roomID);
            Rmi.Marshaler.Write(Msg, type);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyEventStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, System.Guid roomID, int type)
        {
            foreach (var obj in remotes)
                RoomLobbyEventStart(obj, pkOption, roomID, type);
            return true;
        }

        public bool RoomLobbyEventEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int type, string name, long reward)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyEventEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, roomID);
            Rmi.Marshaler.Write(Msg, type);
            Rmi.Marshaler.Write(Msg, name);
            Rmi.Marshaler.Write(Msg, reward);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyEventEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, System.Guid roomID, int type, string name, long reward)
        {
            foreach (var obj in remotes)
                RoomLobbyEventEnd(obj, pkOption, roomID, type, name, reward);
            return true;
        }

        public bool LobbyRoomJackpotInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, long jackpot)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRoomJackpotInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, jackpot);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRoomJackpotInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, long jackpot)
        {
            foreach (var obj in remotes)
                LobbyRoomJackpotInfo(obj, pkOption, jackpot);
            return true;
        }

        public bool LobbyRoomNotifyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int period)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRoomNotifyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, type);
            Rmi.Marshaler.Write(Msg, message);
            Rmi.Marshaler.Write(Msg, period);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRoomNotifyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int type, string message, int period)
        {
            foreach (var obj in remotes)
                LobbyRoomNotifyMessage(obj, pkOption, type, message, period);
            return true;
        }

        public bool LobbyRoomNotifyServermaintenance(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int release)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRoomNotifyServermaintenance;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, type);
            Rmi.Marshaler.Write(Msg, message);
            Rmi.Marshaler.Write(Msg, release);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRoomNotifyServermaintenance(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int type, string message, int release)
        {
            foreach (var obj in remotes)
                LobbyRoomNotifyServermaintenance(obj, pkOption, type, message, release);
            return true;
        }

        public bool LobbyRoomReloadServerData(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRoomReloadServerData;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, type);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRoomReloadServerData(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int type)
        {
            foreach (var obj in remotes)
                LobbyRoomReloadServerData(obj, pkOption, type);
            return true;
        }

        public bool LobbyRoomKickUser(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int userID)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRoomKickUser;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userID);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRoomKickUser(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int userID)
        {
            foreach (var obj in remotes)
                LobbyRoomKickUser(obj, pkOption, userID);
            return true;
        }

        public bool LobbyLoginKickUser(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int userID)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyLoginKickUser;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userID);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyLoginKickUser(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int userID)
        {
            foreach (var obj in remotes)
                LobbyLoginKickUser(obj, pkOption, userID);
            return true;
        }

        public bool RoomLobbyRequestMoveRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, ZNet.RemoteID userRemote, int userID, long money, bool ipFree, bool shopFree, int shopId)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyRequestMoveRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, roomID);
            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, userID);
            Rmi.Marshaler.Write(Msg, money);
            Rmi.Marshaler.Write(Msg, ipFree);
            Rmi.Marshaler.Write(Msg, shopFree);
            Rmi.Marshaler.Write(Msg, shopId);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyRequestMoveRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, System.Guid roomID, ZNet.RemoteID userRemote, int userID, long money, bool ipFree, bool shopFree, int shopId)
        {
            foreach (var obj in remotes)
                RoomLobbyRequestMoveRoom(obj, pkOption, roomID, userRemote, userID, money, ipFree, shopFree, shopId);
            return true;
        }

        public bool LobbyRoomResponseMoveRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool makeRoom, System.Guid roomID, ZNet.NetAddress addr, int chanID, ZNet.RemoteID userRemote, string message)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRoomResponseMoveRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, makeRoom);
            Rmi.Marshaler.Write(Msg, roomID);
            Rmi.Marshaler.Write(Msg, addr);
            Rmi.Marshaler.Write(Msg, chanID);
            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, message);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRoomResponseMoveRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool makeRoom, System.Guid roomID, ZNet.NetAddress addr, int chanID, ZNet.RemoteID userRemote, string message)
        {
            foreach (var obj in remotes)
                LobbyRoomResponseMoveRoom(obj, pkOption, makeRoom, roomID, addr, chanID, userRemote, message);
            return true;
        }

        public bool ServerRequestDataSync(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool isLobby)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ServerRequestDataSync;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, isLobby);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ServerRequestDataSync(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool isLobby)
        {
            foreach (var obj in remotes)
                ServerRequestDataSync(obj, pkOption, isLobby);
            return true;
        }

        public bool RoomLobbyResponseDataSync(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomLobbyResponseDataSync;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomLobbyResponseDataSync(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RoomLobbyResponseDataSync(obj, pkOption, data);
            return true;
        }

        public bool RelayLobbyResponseDataSync(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayLobbyResponseDataSync;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayLobbyResponseDataSync(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayLobbyResponseDataSync(obj, pkOption, data);
            return true;
        }

        public bool RelayClientJoin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayClientJoin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, addr);
            Rmi.Marshaler.Write(Msg, param);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayClientJoin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param)
        {
            foreach (var obj in remotes)
                RelayClientJoin(obj, pkOption, userRemote, addr, param);
            return true;
        }

        public bool RelayClientLeave(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool bMoveServer)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayClientLeave;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, bMoveServer);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayClientLeave(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool bMoveServer)
        {
            foreach (var obj in remotes)
                RelayClientLeave(obj, pkOption, userRemote, bMoveServer);
            return true;
        }

        public bool RelayCloseRemoteClient(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayCloseRemoteClient;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayCloseRemoteClient(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                RelayCloseRemoteClient(obj, pkOption, userRemote);
            return true;
        }

        public bool RelayServerMoveFailure(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayServerMoveFailure;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayServerMoveFailure(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                RelayServerMoveFailure(obj, pkOption, userRemote);
            return true;
        }

        public bool RelayRequestLobbyKey(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string id, string key)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestLobbyKey;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, id);
            Rmi.Marshaler.Write(Msg, key);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestLobbyKey(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string id, string key)
        {
            foreach (var obj in remotes)
                RelayRequestLobbyKey(obj, pkOption, userRemote, id, key);
            return true;
        }

        public bool RelayRequestJoinInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestJoinInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestJoinInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                RelayRequestJoinInfo(obj, pkOption, userRemote);
            return true;
        }

        public bool RelayRequestChannelMove(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestChannelMove;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, chanID);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestChannelMove(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID)
        {
            foreach (var obj in remotes)
                RelayRequestChannelMove(obj, pkOption, userRemote, chanID);
            return true;
        }

        public bool RelayRequestRoomMake(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestRoomMake;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, relayID);
            Rmi.Marshaler.Write(Msg, chanID);
            Rmi.Marshaler.Write(Msg, betType);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestRoomMake(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType, string pass)
        {
            foreach (var obj in remotes)
                RelayRequestRoomMake(obj, pkOption, userRemote, relayID, chanID, betType, pass);
            return true;
        }

        public bool RelayRequestRoomJoin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestRoomJoin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, relayID);
            Rmi.Marshaler.Write(Msg, chanID);
            Rmi.Marshaler.Write(Msg, betType);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestRoomJoin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType)
        {
            foreach (var obj in remotes)
                RelayRequestRoomJoin(obj, pkOption, userRemote, relayID, chanID, betType);
            return true;
        }

        public bool RelayRequestRoomJoinSelect(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int roomNumber, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestRoomJoinSelect;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, relayID);
            Rmi.Marshaler.Write(Msg, chanID);
            Rmi.Marshaler.Write(Msg, roomNumber);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestRoomJoinSelect(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int roomNumber, string pass)
        {
            foreach (var obj in remotes)
                RelayRequestRoomJoinSelect(obj, pkOption, userRemote, relayID, chanID, roomNumber, pass);
            return true;
        }

        public bool RelayRequestBank(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int option, long money, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestBank;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, option);
            Rmi.Marshaler.Write(Msg, money);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestBank(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int option, long money, string pass)
        {
            foreach (var obj in remotes)
                RelayRequestBank(obj, pkOption, userRemote, option, money, pass);
            return true;
        }

        public bool RelayRequestPurchaseList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestPurchaseList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestPurchaseList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                RelayRequestPurchaseList(obj, pkOption, userRemote);
            return true;
        }

        public bool RelayRequestPurchaseAvailability(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestPurchaseAvailability;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, pid);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestPurchaseAvailability(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            foreach (var obj in remotes)
                RelayRequestPurchaseAvailability(obj, pkOption, userRemote, pid);
            return true;
        }

        public bool RelayRequestPurchaseReceiptCheck(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string result)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestPurchaseReceiptCheck;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, result);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestPurchaseReceiptCheck(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string result)
        {
            foreach (var obj in remotes)
                RelayRequestPurchaseReceiptCheck(obj, pkOption, userRemote, result);
            return true;
        }

        public bool RelayRequestPurchaseResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, System.Guid token)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestPurchaseResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, token);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestPurchaseResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, System.Guid token)
        {
            foreach (var obj in remotes)
                RelayRequestPurchaseResult(obj, pkOption, userRemote, token);
            return true;
        }

        public bool RelayRequestPurchaseCash(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestPurchaseCash;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, pid);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestPurchaseCash(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            foreach (var obj in remotes)
                RelayRequestPurchaseCash(obj, pkOption, userRemote, pid);
            return true;
        }

        public bool RelayRequestMyroomList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestMyroomList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestMyroomList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                RelayRequestMyroomList(obj, pkOption, userRemote);
            return true;
        }

        public bool RelayRequestMyroomAction(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestMyroomAction;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, pid);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestMyroomAction(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            foreach (var obj in remotes)
                RelayRequestMyroomAction(obj, pkOption, userRemote, pid);
            return true;
        }

        public bool LobbyRelayResponsePurchaseList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponsePurchaseList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, Purchase_avatar);
            Rmi.Marshaler.Write(Msg, Purchase_card);
            Rmi.Marshaler.Write(Msg, Purchase_evt);
            Rmi.Marshaler.Write(Msg, Purchase_charge);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponsePurchaseList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge)
        {
            foreach (var obj in remotes)
                LobbyRelayResponsePurchaseList(obj, pkOption, userRemote, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
            return true;
        }

        public bool LobbyRelayResponsePurchaseAvailability(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool available, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponsePurchaseAvailability;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, available);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponsePurchaseAvailability(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool available, string reason)
        {
            foreach (var obj in remotes)
                LobbyRelayResponsePurchaseAvailability(obj, pkOption, userRemote, available, reason);
            return true;
        }

        public bool LobbyRelayResponsePurchaseReceiptCheck(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, System.Guid token)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponsePurchaseReceiptCheck;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, token);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponsePurchaseReceiptCheck(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, System.Guid token)
        {
            foreach (var obj in remotes)
                LobbyRelayResponsePurchaseReceiptCheck(obj, pkOption, userRemote, result, token);
            return true;
        }

        public bool LobbyRelayResponsePurchaseResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponsePurchaseResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponsePurchaseResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason)
        {
            foreach (var obj in remotes)
                LobbyRelayResponsePurchaseResult(obj, pkOption, userRemote, result, reason);
            return true;
        }

        public bool LobbyRelayResponsePurchaseCash(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponsePurchaseCash;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponsePurchaseCash(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason)
        {
            foreach (var obj in remotes)
                LobbyRelayResponsePurchaseCash(obj, pkOption, userRemote, result, reason);
            return true;
        }

        public bool LobbyRelayResponseMyroomList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string json)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseMyroomList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, json);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseMyroomList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string json)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseMyroomList(obj, pkOption, userRemote, json);
            return true;
        }

        public bool LobbyRelayResponseMyroomAction(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid, bool result, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseMyroomAction;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, pid);
            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseMyroomAction(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid, bool result, string reason)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseMyroomAction(obj, pkOption, userRemote, pid, result, reason);
            return true;
        }

        public bool LobbyRelayServerMoveStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayServerMoveStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, moveServerIP);
            Rmi.Marshaler.Write(Msg, moveServerPort);
            Rmi.Marshaler.Write(Msg, param);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayServerMoveStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param)
        {
            foreach (var obj in remotes)
                LobbyRelayServerMoveStart(obj, pkOption, userRemote, moveServerIP, moveServerPort, param);
            return true;
        }

        public bool LobbyRelayResponseLobbyKey(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string key)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseLobbyKey;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, key);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseLobbyKey(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string key)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseLobbyKey(obj, pkOption, userRemote, key);
            return true;
        }

        public bool LobbyRelayNotifyUserInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayNotifyUserInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, userInfo);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayNotifyUserInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            foreach (var obj in remotes)
                LobbyRelayNotifyUserInfo(obj, pkOption, userRemote, userInfo);
            return true;
        }

        public bool LobbyRelayNotifyUserList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayNotifyUserList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, lobbyUserInfos);
            Rmi.Marshaler.Write(Msg, lobbyFriendList);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayNotifyUserList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList)
        {
            foreach (var obj in remotes)
                LobbyRelayNotifyUserList(obj, pkOption, userRemote, lobbyUserInfos, lobbyFriendList);
            return true;
        }

        public bool LobbyRelayNotifyRoomList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayNotifyRoomList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, channelID);
            Rmi.Marshaler.Write(Msg, roomInfos);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayNotifyRoomList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos)
        {
            foreach (var obj in remotes)
                LobbyRelayNotifyRoomList(obj, pkOption, userRemote, channelID, roomInfos);
            return true;
        }

        public bool LobbyRelayResponseChannelMove(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseChannelMove;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, chanID);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseChannelMove(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseChannelMove(obj, pkOption, userRemote, chanID);
            return true;
        }

        public bool LobbyRelayResponseLobbyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseLobbyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, message);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseLobbyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseLobbyMessage(obj, pkOption, userRemote, message);
            return true;
        }

        public bool LobbyRelayResponseBank(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, int resultType)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseBank;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, resultType);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseBank(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, int resultType)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseBank(obj, pkOption, userRemote, result, resultType);
            return true;
        }

        public bool LobbyRelayNotifyJackpotInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, long jackpot)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayNotifyJackpotInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, jackpot);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayNotifyJackpotInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, long jackpot)
        {
            foreach (var obj in remotes)
                LobbyRelayNotifyJackpotInfo(obj, pkOption, userRemote, jackpot);
            return true;
        }

        public bool LobbyRelayNotifyLobbyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int type, string message, int period)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayNotifyLobbyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, type);
            Rmi.Marshaler.Write(Msg, message);
            Rmi.Marshaler.Write(Msg, period);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayNotifyLobbyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int type, string message, int period)
        {
            foreach (var obj in remotes)
                LobbyRelayNotifyLobbyMessage(obj, pkOption, userRemote, type, message, period);
            return true;
        }

        public bool RoomRelayServerMoveStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param, Guid idx)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RoomRelayServerMoveStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, addr);
            Rmi.Marshaler.Write(Msg, param);
            Rmi.Marshaler.Write(Msg, idx);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RoomRelayServerMoveStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param, Guid idx)
        {
            foreach (var obj in remotes)
                RoomRelayServerMoveStart(obj, pkOption, userRemote, addr, param, idx);
            return true;
        }

        public bool RelayRequestOutRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestOutRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestOutRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                RelayRequestOutRoom(obj, pkOption, userRemote);
            return true;
        }

        public bool RelayResponseOutRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultOut)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayResponseOutRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, resultOut);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayResponseOutRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultOut)
        {
            foreach (var obj in remotes)
                RelayResponseOutRoom(obj, pkOption, userRemote, resultOut);
            return true;
        }

        public bool RelayRequestMoveRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestMoveRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestMoveRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                RelayRequestMoveRoom(obj, pkOption, userRemote);
            return true;
        }

        public bool RelayResponseMoveRoom(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayResponseMoveRoom;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, resultMove);
            Rmi.Marshaler.Write(Msg, errorMessage);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayResponseMoveRoom(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            foreach (var obj in remotes)
                RelayResponseMoveRoom(obj, pkOption, userRemote, resultMove, errorMessage);
            return true;
        }

        public bool RelayGameRoomIn(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameRoomIn;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameRoomIn(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameRoomIn(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameReady(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameReady;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameReady(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameReady(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameSelectOrder(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameSelectOrder;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameSelectOrder(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameSelectOrder(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameDistributedEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameDistributedEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameDistributedEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameDistributedEnd(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameActionPutCard(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameActionPutCard;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameActionPutCard(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameActionPutCard(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameActionFlipBomb(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameActionFlipBomb;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameActionFlipBomb(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameActionFlipBomb(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameActionChooseCard(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameActionChooseCard;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameActionChooseCard(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameActionChooseCard(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameSelectKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameSelectKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameSelectKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameSelectKookjin(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameSelectGoStop(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameSelectGoStop;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameSelectGoStop(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameSelectGoStop(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGameSelectPush(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGameSelectPush;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGameSelectPush(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGameSelectPush(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayGamePractice(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayGamePractice;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayGamePractice(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayGamePractice(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayRoomIn(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayRoomIn;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, result);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayRoomIn(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result)
        {
            foreach (var obj in remotes)
                GameRelayRoomIn(obj, pkOption, userRemote, result);
            return true;
        }

        public bool GameRelayRequestReady(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayRequestReady;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayRequestReady(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayRequestReady(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayStart(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayRequestSelectOrder(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayRequestSelectOrder;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayRequestSelectOrder(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayRequestSelectOrder(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayOrderEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayOrderEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayOrderEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayOrderEnd(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayDistributedStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayDistributedStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayDistributedStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayDistributedStart(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayFloorHasBonus(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayFloorHasBonus;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayFloorHasBonus(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayFloorHasBonus(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayTurnStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayTurnStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayTurnStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayTurnStart(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelaySelectCardResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelaySelectCardResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelaySelectCardResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelaySelectCardResult(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayFlipDeckResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayFlipDeckResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayFlipDeckResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayFlipDeckResult(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayTurnResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayTurnResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayTurnResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayTurnResult(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayUserInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayUserInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayUserInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayUserInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayNotifyIndex(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayNotifyIndex;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayNotifyIndex(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayNotifyIndex(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayNotifyStat(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayNotifyStat;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayNotifyStat(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayNotifyStat(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayRequestKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayRequestKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayRequestKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayRequestKookjin(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayNotifyKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayNotifyKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayNotifyKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayNotifyKookjin(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayRequestGoStop(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayRequestGoStop;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayRequestGoStop(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayRequestGoStop(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayNotifyGoStop(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayNotifyGoStop;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayNotifyGoStop(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayNotifyGoStop(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayMoveKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayMoveKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayMoveKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayMoveKookjin(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayEventStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayEventStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayEventStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayEventStart(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayEventInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayEventInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayEventInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayEventInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayOver(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayOver;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayOver(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayOver(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayRequestPush(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayRequestPush;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayRequestPush(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayRequestPush(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayResponseRoomMove(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayResponseRoomMove;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, resultMove);
            Rmi.Marshaler.Write(Msg, errorMessage);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayResponseRoomMove(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            foreach (var obj in remotes)
                GameRelayResponseRoomMove(obj, pkOption, userRemote, resultMove, errorMessage);
            return true;
        }

        public bool GameRelayPracticeEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayPracticeEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayPracticeEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayPracticeEnd(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayResponseRoomOut(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool permissionOut)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayResponseRoomOut;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, permissionOut);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayResponseRoomOut(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool permissionOut)
        {
            foreach (var obj in remotes)
                GameRelayResponseRoomOut(obj, pkOption, userRemote, permissionOut);
            return true;
        }

        public bool GameRelayKickUser(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayKickUser;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayKickUser(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            foreach (var obj in remotes)
                GameRelayKickUser(obj, pkOption, userRemote);
            return true;
        }

        public bool GameRelayRoomInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayRoomInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayRoomInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayRoomInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayUserOut(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayUserOut;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayUserOut(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayUserOut(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayObserveInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayObserveInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayObserveInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayObserveInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayNotifyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayNotifyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayNotifyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayNotifyMessage(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayNotifyJackpotInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayNotifyJackpotInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayNotifyJackpotInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayNotifyJackpotInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayRequestLobbyEventInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestLobbyEventInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestLobbyEventInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayRequestLobbyEventInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool LobbyRelayResponseLobbyEventInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseLobbyEventInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseLobbyEventInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseLobbyEventInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool RelayRequestLobbyEventParticipate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RelayRequestLobbyEventParticipate;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RelayRequestLobbyEventParticipate(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RelayRequestLobbyEventParticipate(obj, pkOption, userRemote, data);
            return true;
        }

        public bool LobbyRelayResponseLobbyEventParticipate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.LobbyRelayResponseLobbyEventParticipate;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool LobbyRelayResponseLobbyEventParticipate(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                LobbyRelayResponseLobbyEventParticipate(obj, pkOption, userRemote, data);
            return true;
        }

        public bool GameRelayResponseRoomMissionInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRelayResponseRoomMissionInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userRemote);
            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRelayResponseRoomMissionInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRelayResponseRoomMissionInfo(obj, pkOption, userRemote, data);
            return true;
        }

        public bool ServerMoveStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ServerMoveStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, moveServerIP);
            Rmi.Marshaler.Write(Msg, moveServerPort);
            Rmi.Marshaler.Write(Msg, param);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ServerMoveStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param)
        {
            foreach (var obj in remotes)
                ServerMoveStart(obj, pkOption, moveServerIP, moveServerPort, param);
            return true;
        }

        public bool ServerMoveEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool Moved)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ServerMoveEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, Moved);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ServerMoveEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool Moved)
        {
            foreach (var obj in remotes)
                ServerMoveEnd(obj, pkOption, Moved);
            return true;
        }

        public bool ResponseLauncherLogin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string nickname, string key, byte resultType)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLauncherLogin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, nickname);
            Rmi.Marshaler.Write(Msg, key);
            Rmi.Marshaler.Write(Msg, resultType);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLauncherLogin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result, string nickname, string key, byte resultType)
        {
            foreach (var obj in remotes)
                ResponseLauncherLogin(obj, pkOption, result, nickname, key, resultType);
            return true;
        }

        public bool ResponseLauncherLogout(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLauncherLogout;

            Msg.WriteStart(msgID, pkOption, 0, true);


            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLauncherLogout(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption)
        {
            foreach (var obj in remotes)
                ResponseLauncherLogout(obj, pkOption);
            return true;
        }

        public bool ResponseLoginKey(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLoginKey;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, resultMessage);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLoginKey(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result, string resultMessage)
        {
            foreach (var obj in remotes)
                ResponseLoginKey(obj, pkOption, result, resultMessage);
            return true;
        }

        public bool ResponseLobbyKey(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string key)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLobbyKey;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, key);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLobbyKey(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string key)
        {
            foreach (var obj in remotes)
                ResponseLobbyKey(obj, pkOption, key);
            return true;
        }

        public bool ResponseLogin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLogin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, resultMessage);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLogin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result, string resultMessage)
        {
            foreach (var obj in remotes)
                ResponseLogin(obj, pkOption, result, resultMessage);
            return true;
        }

        public bool NotifyLobbyList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<string> lobbyList)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.NotifyLobbyList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, lobbyList);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool NotifyLobbyList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, List<string> lobbyList)
        {
            foreach (var obj in remotes)
                NotifyLobbyList(obj, pkOption, lobbyList);
            return true;
        }

        public bool NotifyUserInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.NotifyUserInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, userInfo);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool NotifyUserInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            foreach (var obj in remotes)
                NotifyUserInfo(obj, pkOption, userInfo);
            return true;
        }

        public bool NotifyUserList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.NotifyUserList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, lobbyUserInfos);
            Rmi.Marshaler.Write(Msg, lobbyFriendList);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool NotifyUserList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList)
        {
            foreach (var obj in remotes)
                NotifyUserList(obj, pkOption, lobbyUserInfos, lobbyFriendList);
            return true;
        }

        public bool NotifyRoomList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.NotifyRoomList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, channelID);
            Rmi.Marshaler.Write(Msg, roomInfos);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool NotifyRoomList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos)
        {
            foreach (var obj in remotes)
                NotifyRoomList(obj, pkOption, channelID, roomInfos);
            return true;
        }

        public bool ResponseChannelMove(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseChannelMove;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, chanID);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseChannelMove(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int chanID)
        {
            foreach (var obj in remotes)
                ResponseChannelMove(obj, pkOption, chanID);
            return true;
        }

        public bool ResponseLobbyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string message)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLobbyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, message);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLobbyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string message)
        {
            foreach (var obj in remotes)
                ResponseLobbyMessage(obj, pkOption, message);
            return true;
        }

        public bool ResponseBank(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, int resultType)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseBank;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, resultType);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseBank(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result, int resultType)
        {
            foreach (var obj in remotes)
                ResponseBank(obj, pkOption, result, resultType);
            return true;
        }

        public bool NotifyJackpotInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, long jackpot)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.NotifyJackpotInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, jackpot);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool NotifyJackpotInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, long jackpot)
        {
            foreach (var obj in remotes)
                NotifyJackpotInfo(obj, pkOption, jackpot);
            return true;
        }

        public bool NotifyLobbyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int period)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.NotifyLobbyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, type);
            Rmi.Marshaler.Write(Msg, message);
            Rmi.Marshaler.Write(Msg, period);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool NotifyLobbyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int type, string message, int period)
        {
            foreach (var obj in remotes)
                NotifyLobbyMessage(obj, pkOption, type, message, period);
            return true;
        }

        public bool GameRoomIn(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRoomIn;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRoomIn(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result)
        {
            foreach (var obj in remotes)
                GameRoomIn(obj, pkOption, result);
            return true;
        }

        public bool GameRequestReady(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRequestReady;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRequestReady(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRequestReady(obj, pkOption, data);
            return true;
        }

        public bool GameStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameStart(obj, pkOption, data);
            return true;
        }

        public bool GameRequestSelectOrder(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRequestSelectOrder;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRequestSelectOrder(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRequestSelectOrder(obj, pkOption, data);
            return true;
        }

        public bool GameOrderEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameOrderEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameOrderEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameOrderEnd(obj, pkOption, data);
            return true;
        }

        public bool GameDistributedStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameDistributedStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameDistributedStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameDistributedStart(obj, pkOption, data);
            return true;
        }

        public bool GameFloorHasBonus(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameFloorHasBonus;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameFloorHasBonus(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameFloorHasBonus(obj, pkOption, data);
            return true;
        }

        public bool GameTurnStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameTurnStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameTurnStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameTurnStart(obj, pkOption, data);
            return true;
        }

        public bool GameSelectCardResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameSelectCardResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameSelectCardResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameSelectCardResult(obj, pkOption, data);
            return true;
        }

        public bool GameFlipDeckResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameFlipDeckResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameFlipDeckResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameFlipDeckResult(obj, pkOption, data);
            return true;
        }

        public bool GameTurnResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameTurnResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameTurnResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameTurnResult(obj, pkOption, data);
            return true;
        }

        public bool GameUserInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameUserInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameUserInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameUserInfo(obj, pkOption, data);
            return true;
        }

        public bool GameNotifyIndex(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameNotifyIndex;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameNotifyIndex(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameNotifyIndex(obj, pkOption, data);
            return true;
        }

        public bool GameNotifyStat(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameNotifyStat;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameNotifyStat(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameNotifyStat(obj, pkOption, data);
            return true;
        }

        public bool GameRequestKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRequestKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRequestKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRequestKookjin(obj, pkOption, data);
            return true;
        }

        public bool GameNotifyKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameNotifyKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameNotifyKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameNotifyKookjin(obj, pkOption, data);
            return true;
        }

        public bool GameRequestGoStop(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRequestGoStop;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRequestGoStop(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRequestGoStop(obj, pkOption, data);
            return true;
        }

        public bool GameNotifyGoStop(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameNotifyGoStop;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameNotifyGoStop(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameNotifyGoStop(obj, pkOption, data);
            return true;
        }

        public bool GameMoveKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameMoveKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameMoveKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameMoveKookjin(obj, pkOption, data);
            return true;
        }

        public bool GameEventStart(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameEventStart;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameEventStart(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameEventStart(obj, pkOption, data);
            return true;
        }

        public bool GameEventInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameEventInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameEventInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameEventInfo(obj, pkOption, data);
            return true;
        }

        public bool GameOver(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameOver;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameOver(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameOver(obj, pkOption, data);
            return true;
        }

        public bool GameRequestPush(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRequestPush;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRequestPush(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRequestPush(obj, pkOption, data);
            return true;
        }

        public bool GameResponseRoomMove(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool move, string errorMessage)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameResponseRoomMove;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, move);
            Rmi.Marshaler.Write(Msg, errorMessage);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameResponseRoomMove(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool move, string errorMessage)
        {
            foreach (var obj in remotes)
                GameResponseRoomMove(obj, pkOption, move, errorMessage);
            return true;
        }

        public bool GamePracticeEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GamePracticeEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GamePracticeEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GamePracticeEnd(obj, pkOption, data);
            return true;
        }

        public bool GameResponseRoomOut(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameResponseRoomOut;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameResponseRoomOut(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result)
        {
            foreach (var obj in remotes)
                GameResponseRoomOut(obj, pkOption, result);
            return true;
        }

        public bool GameKickUser(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameKickUser;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameKickUser(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameKickUser(obj, pkOption, data);
            return true;
        }

        public bool GameRoomInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRoomInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRoomInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRoomInfo(obj, pkOption, data);
            return true;
        }

        public bool GameUserOut(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameUserOut;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameUserOut(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameUserOut(obj, pkOption, data);
            return true;
        }

        public bool GameObserveInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameObserveInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameObserveInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameObserveInfo(obj, pkOption, data);
            return true;
        }

        public bool GameNotifyMessage(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameNotifyMessage;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameNotifyMessage(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameNotifyMessage(obj, pkOption, data);
            return true;
        }

        public bool ResponsePurchaseList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponsePurchaseList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, Purchase_avatar);
            Rmi.Marshaler.Write(Msg, Purchase_card);
            Rmi.Marshaler.Write(Msg, Purchase_evt);
            Rmi.Marshaler.Write(Msg, Purchase_charge);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponsePurchaseList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge)
        {
            foreach (var obj in remotes)
                ResponsePurchaseList(obj, pkOption, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
            return true;
        }

        public bool ResponsePurchaseAvailability(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool available, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponsePurchaseAvailability;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, available);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponsePurchaseAvailability(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool available, string reason)
        {
            foreach (var obj in remotes)
                ResponsePurchaseAvailability(obj, pkOption, available, reason);
            return true;
        }

        public bool ResponsePurchaseReceiptCheck(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, System.Guid token)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponsePurchaseReceiptCheck;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, token);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponsePurchaseReceiptCheck(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result, System.Guid token)
        {
            foreach (var obj in remotes)
                ResponsePurchaseReceiptCheck(obj, pkOption, result, token);
            return true;
        }

        public bool ResponsePurchaseResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponsePurchaseResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponsePurchaseResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result, string reason)
        {
            foreach (var obj in remotes)
                ResponsePurchaseResult(obj, pkOption, result, reason);
            return true;
        }

        public bool ResponsePurchaseCash(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponsePurchaseCash;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponsePurchaseCash(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, bool result, string reason)
        {
            foreach (var obj in remotes)
                ResponsePurchaseCash(obj, pkOption, result, reason);
            return true;
        }

        public bool ResponseMyroomList(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string json)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseMyroomList;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, json);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseMyroomList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string json)
        {
            foreach (var obj in remotes)
                ResponseMyroomList(obj, pkOption, json);
            return true;
        }

        public bool ResponseMyroomAction(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid, bool result, string reason)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseMyroomAction;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, pid);
            Rmi.Marshaler.Write(Msg, result);
            Rmi.Marshaler.Write(Msg, reason);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseMyroomAction(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string pid, bool result, string reason)
        {
            foreach (var obj in remotes)
                ResponseMyroomAction(obj, pkOption, pid, result, reason);
            return true;
        }

        public bool ResponseGameOptions(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseGameOptions;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseGameOptions(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                ResponseGameOptions(obj, pkOption, data);
            return true;
        }

        public bool ResponseLobbyEventInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLobbyEventInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLobbyEventInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                ResponseLobbyEventInfo(obj, pkOption, data);
            return true;
        }

        public bool ResponseLobbyEventParticipate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ResponseLobbyEventParticipate;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool ResponseLobbyEventParticipate(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                ResponseLobbyEventParticipate(obj, pkOption, data);
            return true;
        }

        public bool GameResponseRoomMissionInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameResponseRoomMissionInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameResponseRoomMissionInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameResponseRoomMissionInfo(obj, pkOption, data);
            return true;
        }

        public bool ServerMoveFailure(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.ServerMoveFailure;

            Msg.WriteStart(msgID, pkOption, 0, true);


            return PacketSend(remote, pkOption, Msg);
        }

        public bool ServerMoveFailure(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption)
        {
            foreach (var obj in remotes)
                ServerMoveFailure(obj, pkOption);
            return true;
        }

        public bool RequestLauncherLogin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLauncherLogin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, id);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLauncherLogin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string id, string pass)
        {
            foreach (var obj in remotes)
                RequestLauncherLogin(obj, pkOption, id, pass);
            return true;
        }

        public bool RequestLauncherLogout(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLauncherLogout;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, id);
            Rmi.Marshaler.Write(Msg, key);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLauncherLogout(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string id, string key)
        {
            foreach (var obj in remotes)
                RequestLauncherLogout(obj, pkOption, id, key);
            return true;
        }

        public bool RequestLoginKey(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLoginKey;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, id);
            Rmi.Marshaler.Write(Msg, key);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLoginKey(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string id, string key)
        {
            foreach (var obj in remotes)
                RequestLoginKey(obj, pkOption, id, key);
            return true;
        }

        public bool RequestLobbyKey(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLobbyKey;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, id);
            Rmi.Marshaler.Write(Msg, key);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLobbyKey(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string id, string key)
        {
            foreach (var obj in remotes)
                RequestLobbyKey(obj, pkOption, id, key);
            return true;
        }

        public bool RequestLogin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string name, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLogin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, name);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLogin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string name, string pass)
        {
            foreach (var obj in remotes)
                RequestLogin(obj, pkOption, name, pass);
            return true;
        }

        public bool RequestLobbyList(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLobbyList;

            Msg.WriteStart(msgID, pkOption, 0, true);


            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLobbyList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption)
        {
            foreach (var obj in remotes)
                RequestLobbyList(obj, pkOption);
            return true;
        }

        public bool RequestGoLobby(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string lobbyName)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestGoLobby;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, lobbyName);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestGoLobby(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string lobbyName)
        {
            foreach (var obj in remotes)
                RequestGoLobby(obj, pkOption, lobbyName);
            return true;
        }

        public bool RequestJoinInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestJoinInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);


            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestJoinInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption)
        {
            foreach (var obj in remotes)
                RequestJoinInfo(obj, pkOption);
            return true;
        }

        public bool RequestChannelMove(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestChannelMove;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, chanID);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestChannelMove(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int chanID)
        {
            foreach (var obj in remotes)
                RequestChannelMove(obj, pkOption, chanID);
            return true;
        }

        public bool RequestRoomMake(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int betType, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestRoomMake;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, chanID);
            Rmi.Marshaler.Write(Msg, betType);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestRoomMake(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int chanID, int betType, string pass)
        {
            foreach (var obj in remotes)
                RequestRoomMake(obj, pkOption, chanID, betType, pass);
            return true;
        }

        public bool RequestRoomJoin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int betType)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestRoomJoin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, chanID);
            Rmi.Marshaler.Write(Msg, betType);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestRoomJoin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int chanID, int betType)
        {
            foreach (var obj in remotes)
                RequestRoomJoin(obj, pkOption, chanID, betType);
            return true;
        }

        public bool RequestRoomJoinSelect(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int roomNumber, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestRoomJoinSelect;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, chanID);
            Rmi.Marshaler.Write(Msg, roomNumber);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestRoomJoinSelect(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int chanID, int roomNumber, string pass)
        {
            foreach (var obj in remotes)
                RequestRoomJoinSelect(obj, pkOption, chanID, roomNumber, pass);
            return true;
        }

        public bool RequestBank(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int option, long money, string pass)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestBank;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, option);
            Rmi.Marshaler.Write(Msg, money);
            Rmi.Marshaler.Write(Msg, pass);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestBank(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, int option, long money, string pass)
        {
            foreach (var obj in remotes)
                RequestBank(obj, pkOption, option, money, pass);
            return true;
        }

        public bool GameRoomInUser(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRoomInUser;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRoomInUser(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRoomInUser(obj, pkOption, data);
            return true;
        }

        public bool GameReady(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameReady;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameReady(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameReady(obj, pkOption, data);
            return true;
        }

        public bool GameSelectOrder(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameSelectOrder;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameSelectOrder(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameSelectOrder(obj, pkOption, data);
            return true;
        }

        public bool GameDistributedEnd(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameDistributedEnd;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameDistributedEnd(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameDistributedEnd(obj, pkOption, data);
            return true;
        }

        public bool GameActionPutCard(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameActionPutCard;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameActionPutCard(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameActionPutCard(obj, pkOption, data);
            return true;
        }

        public bool GameActionFlipBomb(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameActionFlipBomb;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameActionFlipBomb(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameActionFlipBomb(obj, pkOption, data);
            return true;
        }

        public bool GameActionChooseCard(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameActionChooseCard;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameActionChooseCard(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameActionChooseCard(obj, pkOption, data);
            return true;
        }

        public bool GameSelectKookjin(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameSelectKookjin;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameSelectKookjin(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameSelectKookjin(obj, pkOption, data);
            return true;
        }

        public bool GameSelectGoStop(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameSelectGoStop;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameSelectGoStop(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameSelectGoStop(obj, pkOption, data);
            return true;
        }

        public bool GameSelectPush(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameSelectPush;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameSelectPush(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameSelectPush(obj, pkOption, data);
            return true;
        }

        public bool GamePractice(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GamePractice;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GamePractice(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GamePractice(obj, pkOption, data);
            return true;
        }

        public bool GameRoomOut(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRoomOut;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRoomOut(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRoomOut(obj, pkOption, data);
            return true;
        }

        public bool GameRoomMove(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.GameRoomMove;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool GameRoomMove(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                GameRoomMove(obj, pkOption, data);
            return true;
        }

        public bool RequestPurchaseList(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestPurchaseList;

            Msg.WriteStart(msgID, pkOption, 0, true);


            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestPurchaseList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption)
        {
            foreach (var obj in remotes)
                RequestPurchaseList(obj, pkOption);
            return true;
        }

        public bool RequestPurchaseAvailability(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestPurchaseAvailability;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, pid);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestPurchaseAvailability(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string pid)
        {
            foreach (var obj in remotes)
                RequestPurchaseAvailability(obj, pkOption, pid);
            return true;
        }

        public bool RequestPurchaseReceiptCheck(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string result)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestPurchaseReceiptCheck;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, result);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestPurchaseReceiptCheck(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string result)
        {
            foreach (var obj in remotes)
                RequestPurchaseReceiptCheck(obj, pkOption, result);
            return true;
        }

        public bool RequestPurchaseResult(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid token)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestPurchaseResult;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, token);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestPurchaseResult(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, System.Guid token)
        {
            foreach (var obj in remotes)
                RequestPurchaseResult(obj, pkOption, token);
            return true;
        }

        public bool RequestPurchaseCash(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestPurchaseCash;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, pid);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestPurchaseCash(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string pid)
        {
            foreach (var obj in remotes)
                RequestPurchaseCash(obj, pkOption, pid);
            return true;
        }

        public bool RequestMyroomList(ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestMyroomList;

            Msg.WriteStart(msgID, pkOption, 0, true);


            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestMyroomList(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption)
        {
            foreach (var obj in remotes)
                RequestMyroomList(obj, pkOption);
            return true;
        }

        public bool RequestMyroomAction(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestMyroomAction;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, pid);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestMyroomAction(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, string pid)
        {
            foreach (var obj in remotes)
                RequestMyroomAction(obj, pkOption, pid);
            return true;
        }

        public bool RequestGameOptions(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestGameOptions;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestGameOptions(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RequestGameOptions(obj, pkOption, data);
            return true;
        }

        public bool RequestLobbyEventInfo(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLobbyEventInfo;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLobbyEventInfo(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RequestLobbyEventInfo(obj, pkOption, data);
            return true;
        }

        public bool RequestLobbyEventParticipate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            ZNet.CMessage Msg = new ZNet.CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)Common.RequestLobbyEventParticipate;

            Msg.WriteStart(msgID, pkOption, 0, true);

            Rmi.Marshaler.Write(Msg, data);

            return PacketSend(remote, pkOption, Msg);
        }

        public bool RequestLobbyEventParticipate(ZNet.RemoteID[] remotes, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            foreach (var obj in remotes)
                RequestLobbyEventParticipate(obj, pkOption, data);
            return true;
        }

    }

}

