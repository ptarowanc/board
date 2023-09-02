// Auto created from IDLCompiler.exe
using System;
using System.Collections.Generic;
using System.Net;


namespace SS
{
    public class Stub : ZNet.PKStub
    {
        public new void NeedImplement(string val)
        {
            Server.Log._log.InfoFormat("NeedImplement:{0}", val);
        }

        public delegate bool MasterAllShutdownDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg);
        public MasterAllShutdownDelegate MasterAllShutdown = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string msg)
        {
            return false;
        };
        public delegate bool MasterNotifyP2PServerInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public MasterNotifyP2PServerInfoDelegate MasterNotifyP2PServerInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RoomLobbyMakeRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.RoomInfo roomInfo, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, string Pass, int shopId);
        public RoomLobbyMakeRoomDelegate RoomLobbyMakeRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.RoomInfo roomInfo, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, string Pass, int shopId)
        {
            return false;
        };
        public delegate bool RoomLobbyJoinRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, int shopId);
        public RoomLobbyJoinRoomDelegate RoomLobbyJoinRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, Rmi.Marshaler.LobbyUserList userInfo, int userID, string IP, int shopId)
        {
            return false;
        };
        public delegate bool RoomLobbyOutRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int userID);
        public RoomLobbyOutRoomDelegate RoomLobbyOutRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int userID)
        {
            return false;
        };
        public delegate bool RoomLobbyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message);
        public RoomLobbyMessageDelegate RoomLobbyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message)
        {
            return false;
        };
        public delegate bool RoomLobbyEventStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int type);
        public RoomLobbyEventStartDelegate RoomLobbyEventStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int type)
        {
            return false;
        };
        public delegate bool RoomLobbyEventEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int type, string name, long reward);
        public RoomLobbyEventEndDelegate RoomLobbyEventEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, int type, string name, long reward)
        {
            return false;
        };
        public delegate bool LobbyRoomJackpotInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, long jackpot);
        public LobbyRoomJackpotInfoDelegate LobbyRoomJackpotInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, long jackpot)
        {
            return false;
        };
        public delegate bool LobbyRoomNotifyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int period);
        public LobbyRoomNotifyMessageDelegate LobbyRoomNotifyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int period)
        {
            return false;
        };
        public delegate bool LobbyRoomNotifyServermaintenanceDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int release);
        public LobbyRoomNotifyServermaintenanceDelegate LobbyRoomNotifyServermaintenance = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int release)
        {
            return false;
        };
        public delegate bool LobbyRoomReloadServerDataDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type);
        public LobbyRoomReloadServerDataDelegate LobbyRoomReloadServerData = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type)
        {
            return false;
        };
        public delegate bool LobbyRoomCallingDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, int chanId, System.Guid roomId, int playerId);
        public LobbyRoomCallingDelegate LobbyRoomCalling = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, int chanId, System.Guid roomId, int playerId)
        {
            return false;
        };
        public delegate bool LobbyRoomKickUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int userID);
        public LobbyRoomKickUserDelegate LobbyRoomKickUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int userID)
        {
            return false;
        };
        public delegate bool LobbyLoginKickUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int userID);
        public LobbyLoginKickUserDelegate LobbyLoginKickUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int userID)
        {
            return false;
        };
        public delegate bool RoomLobbyRequestMoveRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, ZNet.RemoteID remoteS, ZNet.RemoteID userRemote, int userID, long money, bool ipFree, bool shopFree, int shopId, bool restrict);
        public RoomLobbyRequestMoveRoomDelegate RoomLobbyRequestMoveRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, ZNet.RemoteID remoteS, ZNet.RemoteID userRemote, int userID, long money, bool ipFree, bool shopFree, int shopId, bool restrict)
        {
            return false;
        };
        public delegate bool LobbyRoomResponseMoveRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool makeRoom, System.Guid roomID, ZNet.NetAddress addr, int chanID, ZNet.RemoteID remoteS, ZNet.RemoteID userRemote, string message);
        public LobbyRoomResponseMoveRoomDelegate LobbyRoomResponseMoveRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool makeRoom, System.Guid roomID, ZNet.NetAddress addr, int chanID, ZNet.RemoteID remoteS, ZNet.RemoteID userRemote, string message)
        {
            return false;
        };
        public delegate bool ServerRequestDataSyncDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool isLobby);
        public ServerRequestDataSyncDelegate ServerRequestDataSync = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool isLobby)
        {
            return false;
        };
        public delegate bool RoomLobbyResponseDataSyncDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public RoomLobbyResponseDataSyncDelegate RoomLobbyResponseDataSync = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayLobbyResponseDataSyncDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public RelayLobbyResponseDataSyncDelegate RelayLobbyResponseDataSync = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayClientJoinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param);
        public RelayClientJoinDelegate RelayClientJoin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param)
        {
            return false;
        };
        public delegate bool RelayClientLeaveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool bMoveServer);
        public RelayClientLeaveDelegate RelayClientLeave = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool bMoveServer)
        {
            return false;
        };
        public delegate bool RelayCloseRemoteClientDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayCloseRemoteClientDelegate RelayCloseRemoteClient = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayServerMoveFailureDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayServerMoveFailureDelegate RelayServerMoveFailure = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayRequestLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string id, string key, int gameid);
        public RelayRequestLobbyKeyDelegate RelayRequestLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string id, string key, int gameid)
        {
            return false;
        };
        public delegate bool RelayRequestJoinInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayRequestJoinInfoDelegate RelayRequestJoinInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayRequestChannelMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID);
        public RelayRequestChannelMoveDelegate RelayRequestChannelMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID)
        {
            return false;
        };
        public delegate bool RelayRequestRoomMakeDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType, string pass);
        public RelayRequestRoomMakeDelegate RelayRequestRoomMake = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType, string pass)
        {
            return false;
        };
        public delegate bool RelayRequestRoomJoinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType);
        public RelayRequestRoomJoinDelegate RelayRequestRoomJoin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int betType)
        {
            return false;
        };
        public delegate bool RelayRequestRoomJoinSelectDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int roomNumber, string pass);
        public RelayRequestRoomJoinSelectDelegate RelayRequestRoomJoinSelect = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int relayID, int chanID, int roomNumber, string pass)
        {
            return false;
        };
        public delegate bool RelayRequestBankDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int option, long money, string pass);
        public RelayRequestBankDelegate RelayRequestBank = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int option, long money, string pass)
        {
            return false;
        };
        public delegate bool RelayRequestPurchaseListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayRequestPurchaseListDelegate RelayRequestPurchaseList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayRequestPurchaseAvailabilityDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid);
        public RelayRequestPurchaseAvailabilityDelegate RelayRequestPurchaseAvailability = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            return false;
        };
        public delegate bool RelayRequestPurchaseReceiptCheckDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string result);
        public RelayRequestPurchaseReceiptCheckDelegate RelayRequestPurchaseReceiptCheck = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string result)
        {
            return false;
        };
        public delegate bool RelayRequestPurchaseResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, System.Guid token);
        public RelayRequestPurchaseResultDelegate RelayRequestPurchaseResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, System.Guid token)
        {
            return false;
        };
        public delegate bool RelayRequestPurchaseCashDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid);
        public RelayRequestPurchaseCashDelegate RelayRequestPurchaseCash = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            return false;
        };
        public delegate bool RelayRequestMyroomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayRequestMyroomListDelegate RelayRequestMyroomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayRequestMyroomActionDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid);
        public RelayRequestMyroomActionDelegate RelayRequestMyroomAction = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid)
        {
            return false;
        };
        public delegate bool LobbyRelayResponsePurchaseListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge);
        public LobbyRelayResponsePurchaseListDelegate LobbyRelayResponsePurchaseList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge)
        {
            return false;
        };
        public delegate bool LobbyRelayResponsePurchaseAvailabilityDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool available, string reason);
        public LobbyRelayResponsePurchaseAvailabilityDelegate LobbyRelayResponsePurchaseAvailability = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool available, string reason)
        {
            return false;
        };
        public delegate bool LobbyRelayResponsePurchaseReceiptCheckDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, System.Guid token);
        public LobbyRelayResponsePurchaseReceiptCheckDelegate LobbyRelayResponsePurchaseReceiptCheck = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, System.Guid token)
        {
            return false;
        };
        public delegate bool LobbyRelayResponsePurchaseResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason);
        public LobbyRelayResponsePurchaseResultDelegate LobbyRelayResponsePurchaseResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason)
        {
            return false;
        };
        public delegate bool LobbyRelayResponsePurchaseCashDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason);
        public LobbyRelayResponsePurchaseCashDelegate LobbyRelayResponsePurchaseCash = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, string reason)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseMyroomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string json);
        public LobbyRelayResponseMyroomListDelegate LobbyRelayResponseMyroomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string json)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseMyroomActionDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid, bool result, string reason);
        public LobbyRelayResponseMyroomActionDelegate LobbyRelayResponseMyroomAction = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string pid, bool result, string reason)
        {
            return false;
        };
        public delegate bool LobbyRelayServerMoveStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param, Guid guid);
        public LobbyRelayServerMoveStartDelegate LobbyRelayServerMoveStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param, Guid guid)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string key, int gameid);
        public LobbyRelayResponseLobbyKeyDelegate LobbyRelayResponseLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string key, int gameid)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, Rmi.Marshaler.LobbyUserInfo userInfo);
        public LobbyRelayNotifyUserInfoDelegate LobbyRelayNotifyUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyUserListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, System.Collections.Generic.List<string> lobbyFriendList);
        public LobbyRelayNotifyUserListDelegate LobbyRelayNotifyUserList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, System.Collections.Generic.List<string> lobbyFriendList)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyRoomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int channelID, System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos);
        public LobbyRelayNotifyRoomListDelegate LobbyRelayNotifyRoomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int channelID, System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseChannelMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID);
        public LobbyRelayResponseChannelMoveDelegate LobbyRelayResponseChannelMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int chanID)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseLobbyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message);
        public LobbyRelayResponseLobbyMessageDelegate LobbyRelayResponseLobbyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string message)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseBankDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, int resultType);
        public LobbyRelayResponseBankDelegate LobbyRelayResponseBank = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result, int resultType)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyJackpotInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, long jackpot);
        public LobbyRelayNotifyJackpotInfoDelegate LobbyRelayNotifyJackpotInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, long jackpot)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyLobbyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int type, string message, int period);
        public LobbyRelayNotifyLobbyMessageDelegate LobbyRelayNotifyLobbyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int type, string message, int period)
        {
            return false;
        };
        public delegate bool RoomRelayServerMoveStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param, Guid idx);
        public RoomRelayServerMoveStartDelegate RoomRelayServerMoveStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.NetAddress addr, ZNet.ArrByte param, Guid idx)
        {
            return false;
        };
        public delegate bool RelayRequestRoomOutRsvnDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool IsRsvn);
        public RelayRequestRoomOutRsvnDelegate RelayRequestRoomOutRsvn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool IsRsvn)
        {
            return false;
        };
        public delegate bool RelayRequestRoomOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayRequestRoomOutDelegate RelayRequestRoomOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayResponseRoomOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultOut);
        public RelayResponseRoomOutDelegate RelayResponseRoomOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultOut)
        {
            return false;
        };
        public delegate bool RelayRequestRoomMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayRequestRoomMoveDelegate RelayRequestRoomMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayResponseRoomMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage);
        public RelayResponseRoomMoveDelegate RelayResponseRoomMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            return false;
        };
        public delegate bool RelayGameRoomInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameRoomInDelegate RelayGameRoomIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameRequestReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameRequestReadyDelegate RelayGameRequestReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameDealCardsEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameDealCardsEndDelegate RelayGameDealCardsEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameActionBetDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameActionBetDelegate RelayGameActionBet = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameActionChangeCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameActionChangeCardDelegate RelayGameActionChangeCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayResponseRoomOutRsvnDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, byte player_index, bool Rsvn);
        public GameRelayResponseRoomOutRsvnDelegate GameRelayResponseRoomOutRsvn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, byte player_index, bool Rsvn)
        {
            return false;
        };
        public delegate bool GameRelayResponseRoomOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool permissionOut);
        public GameRelayResponseRoomOutDelegate GameRelayResponseRoomOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool permissionOut)
        {
            return false;
        };
        public delegate bool GameRelayResponseRoomMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage);
        public GameRelayResponseRoomMoveDelegate GameRelayResponseRoomMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            return false;
        };
        public delegate bool GameRelayRoomInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result);
        public GameRelayRoomInDelegate GameRelayRoomIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result)
        {
            return false;
        };
        public delegate bool GameRelayRoomReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRoomReadyDelegate GameRelayRoomReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayStartDelegate GameRelayStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayDealCardsDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayDealCardsDelegate GameRelayDealCards = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayUserInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayUserInDelegate GameRelayUserIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelaySetBossDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelaySetBossDelegate GameRelaySetBoss = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayNotifyStatDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayNotifyStatDelegate GameRelayNotifyStat = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRoundStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRoundStartDelegate GameRelayRoundStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayChangeTurnDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayChangeTurnDelegate GameRelayChangeTurn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRequestBetDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRequestBetDelegate GameRelayRequestBet = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayResponseBetDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayResponseBetDelegate GameRelayResponseBet = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayChangeRoundDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayChangeRoundDelegate GameRelayChangeRound = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRequestChangeCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRequestChangeCardDelegate GameRelayRequestChangeCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayResponseChangeCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayResponseChangeCardDelegate GameRelayResponseChangeCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayCardOpenDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayCardOpenDelegate GameRelayCardOpen = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayOverDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayOverDelegate GameRelayOver = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRoomInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRoomInfoDelegate GameRelayRoomInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayKickUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayKickUserDelegate GameRelayKickUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEventInfoDelegate GameRelayEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayUserInfoDelegate GameRelayUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayUserOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayUserOutDelegate GameRelayUserOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEventStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEventStartDelegate GameRelayEventStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEvent2StartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEvent2StartDelegate GameRelayEvent2Start = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEventRefreshDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEventRefreshDelegate GameRelayEventRefresh = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEventEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEventEndDelegate GameRelayEventEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayMileageRefreshDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayMileageRefreshDelegate GameRelayMileageRefresh = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEventNotifyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEventNotifyDelegate GameRelayEventNotify = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayCurrentInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayCurrentInfoDelegate GameRelayCurrentInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEntrySpectatorDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEntrySpectatorDelegate GameRelayEntrySpectator = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayNotifyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayNotifyMessageDelegate GameRelayNotifyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayNotifyJackpotInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayNotifyJackpotInfoDelegate GameRelayNotifyJackpotInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayRequestLobbyEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data);
        public RelayRequestLobbyEventInfoDelegate RelayRequestLobbyEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseLobbyEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public LobbyRelayResponseLobbyEventInfoDelegate LobbyRelayResponseLobbyEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayRequestLobbyEventParticipateDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayRequestLobbyEventParticipateDelegate RelayRequestLobbyEventParticipate = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool LobbyRelayResponseLobbyEventParticipateDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public LobbyRelayResponseLobbyEventParticipateDelegate LobbyRelayResponseLobbyEventParticipate = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool ServerMoveStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.NetAddress addr, ZNet.ArrByte param, Guid idx);
        public ServerMoveStartDelegate ServerMoveStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.NetAddress addr, ZNet.ArrByte param, Guid idx)
        {
            return false;
        };
        public delegate bool ServerMoveEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool Moved);
        public ServerMoveEndDelegate ServerMoveEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool Moved)
        {
            return false;
        };
        public delegate bool ResponseLauncherLoginDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string nickname, string key, byte resultType);
        public ResponseLauncherLoginDelegate ResponseLauncherLogin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string nickname, string key, byte resultType)
        {
            return false;
        };
        public delegate bool ResponseLauncherLogoutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption);
        public ResponseLauncherLogoutDelegate ResponseLauncherLogout = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            return false;
        };
        public delegate bool ResponseLoginKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage);
        public ResponseLoginKeyDelegate ResponseLoginKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage)
        {
            return false;
        };
        public delegate bool ResponseLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string key, int gameid);
        public ResponseLobbyKeyDelegate ResponseLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string key, int gameid)
        {
            return false;
        };
        public delegate bool ResponseLoginDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage);
        public ResponseLoginDelegate ResponseLogin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage)
        {
            return false;
        };
        public delegate bool NotifyLobbyListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Collections.Generic.List<string> lobbyList);
        public NotifyLobbyListDelegate NotifyLobbyList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Collections.Generic.List<string> lobbyList)
        {
            return false;
        };
        public delegate bool NotifyUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.LobbyUserInfo userInfo);
        public NotifyUserInfoDelegate NotifyUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            return false;
        };
        public delegate bool NotifyUserListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, System.Collections.Generic.List<string> lobbyFriendList);
        public NotifyUserListDelegate NotifyUserList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, System.Collections.Generic.List<string> lobbyFriendList)
        {
            return false;
        };
        public delegate bool NotifyRoomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int channelID, System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos);
        public NotifyRoomListDelegate NotifyRoomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int channelID, System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos)
        {
            return false;
        };
        public delegate bool ResponseChannelMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID);
        public ResponseChannelMoveDelegate ResponseChannelMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID)
        {
            return false;
        };
        public delegate bool ResponseLobbyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string message);
        public ResponseLobbyMessageDelegate ResponseLobbyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string message)
        {
            return false;
        };
        public delegate bool ResponseBankDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, int resultType);
        public ResponseBankDelegate ResponseBank = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, int resultType)
        {
            return false;
        };
        public delegate bool NotifyJackpotInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, long jackpot);
        public NotifyJackpotInfoDelegate NotifyJackpotInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, long jackpot)
        {
            return false;
        };
        public delegate bool NotifyLobbyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int period);
        public NotifyLobbyMessageDelegate NotifyLobbyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int type, string message, int period)
        {
            return false;
        };
        public delegate bool GameResponseRoomOutRsvpDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, byte player_index, bool IsRsvn);
        public GameResponseRoomOutRsvpDelegate GameResponseRoomOutRsvp = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, byte player_index, bool IsRsvn)
        {
            return false;
        };
        public delegate bool GameResponseRoomOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result);
        public GameResponseRoomOutDelegate GameResponseRoomOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result)
        {
            return false;
        };
        public delegate bool GameResponseRoomMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool move, string errorMessage);
        public GameResponseRoomMoveDelegate GameResponseRoomMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool move, string errorMessage)
        {
            return false;
        };
        public delegate bool GameRoomInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result);
        public GameRoomInDelegate GameRoomIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result)
        {
            return false;
        };
        public delegate bool GameRoomReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRoomReadyDelegate GameRoomReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameStartDelegate GameStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameDealCardsDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameDealCardsDelegate GameDealCards = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameUserInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameUserInDelegate GameUserIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameSetBossDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameSetBossDelegate GameSetBoss = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameNotifyStatDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameNotifyStatDelegate GameNotifyStat = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRoundStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRoundStartDelegate GameRoundStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameChangeTurnDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameChangeTurnDelegate GameChangeTurn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRequestBetDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRequestBetDelegate GameRequestBet = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameResponseBetDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameResponseBetDelegate GameResponseBet = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameChangeRoundDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameChangeRoundDelegate GameChangeRound = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRequestChangeCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRequestChangeCardDelegate GameRequestChangeCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameResponseChangeCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameResponseChangeCardDelegate GameResponseChangeCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameCardOpenDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameCardOpenDelegate GameCardOpen = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameOverDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameOverDelegate GameOver = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRoomInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRoomInfoDelegate GameRoomInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameKickUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameKickUserDelegate GameKickUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameEventInfoDelegate GameEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameUserInfoDelegate GameUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameUserOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameUserOutDelegate GameUserOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameUserOutRsvnDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameUserOutRsvnDelegate GameUserOutRsvn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameEventStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameEventStartDelegate GameEventStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameEvent2StartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameEvent2StartDelegate GameEvent2Start = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameEventRefreshDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameEventRefreshDelegate GameEventRefresh = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameEventEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameEventEndDelegate GameEventEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameMileageRefreshDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameMileageRefreshDelegate GameMileageRefresh = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameEventNotifyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameEventNotifyDelegate GameEventNotify = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameCurrentInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameCurrentInfoDelegate GameCurrentInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameEntrySpectatorDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameEntrySpectatorDelegate GameEntrySpectator = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameNotifyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameNotifyMessageDelegate GameNotifyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool ResponsePurchaseListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Collections.Generic.List<string> Purchase_avatar, System.Collections.Generic.List<string> Purchase_card, System.Collections.Generic.List<string> Purchase_evt, System.Collections.Generic.List<string> Purchase_charge);
        public ResponsePurchaseListDelegate ResponsePurchaseList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Collections.Generic.List<string> Purchase_avatar, System.Collections.Generic.List<string> Purchase_card, System.Collections.Generic.List<string> Purchase_evt, System.Collections.Generic.List<string> Purchase_charge)
        {
            return false;
        };
        public delegate bool ResponsePurchaseAvailabilityDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool available, string reason);
        public ResponsePurchaseAvailabilityDelegate ResponsePurchaseAvailability = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool available, string reason)
        {
            return false;
        };
        public delegate bool ResponsePurchaseReceiptCheckDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, System.Guid token);
        public ResponsePurchaseReceiptCheckDelegate ResponsePurchaseReceiptCheck = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, System.Guid token)
        {
            return false;
        };
        public delegate bool ResponsePurchaseResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string reason);
        public ResponsePurchaseResultDelegate ResponsePurchaseResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string reason)
        {
            return false;
        };
        public delegate bool ResponsePurchaseCashDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string reason);
        public ResponsePurchaseCashDelegate ResponsePurchaseCash = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string reason)
        {
            return false;
        };
        public delegate bool ResponseMyroomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string json);
        public ResponseMyroomListDelegate ResponseMyroomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string json)
        {
            return false;
        };
        public delegate bool ResponseMyroomActionDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid, bool result, string reason);
        public ResponseMyroomActionDelegate ResponseMyroomAction = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid, bool result, string reason)
        {
            return false;
        };
        public delegate bool ResponseGameOptionsDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public ResponseGameOptionsDelegate ResponseGameOptions = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool ResponseLobbyEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public ResponseLobbyEventInfoDelegate ResponseLobbyEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool ResponseLobbyEventParticipateDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public ResponseLobbyEventParticipateDelegate ResponseLobbyEventParticipate = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool ServerMoveFailureDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption);
        public ServerMoveFailureDelegate ServerMoveFailure = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            return false;
        };
        public delegate bool RequestLauncherLoginDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string pass);
        public RequestLauncherLoginDelegate RequestLauncherLogin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string pass)
        {
            return false;
        };
        public delegate bool RequestLauncherLogoutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key);
        public RequestLauncherLogoutDelegate RequestLauncherLogout = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key)
        {
            return false;
        };
        public delegate bool RequestLoginKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key);
        public RequestLoginKeyDelegate RequestLoginKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key)
        {
            return false;
        };
        public delegate bool RequestLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key, int gameid);
        public RequestLobbyKeyDelegate RequestLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key, int gameid)
        {
            return false;
        };
        public delegate bool RequestLoginDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string name, string pass);
        public RequestLoginDelegate RequestLogin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string name, string pass)
        {
            return false;
        };
        public delegate bool RequestLobbyListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption);
        public RequestLobbyListDelegate RequestLobbyList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            return false;
        };
        public delegate bool RequestGoLobbyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string lobbyName);
        public RequestGoLobbyDelegate RequestGoLobby = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string lobbyName)
        {
            return false;
        };
        public delegate bool RequestJoinInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption);
        public RequestJoinInfoDelegate RequestJoinInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            return false;
        };
        public delegate bool RequestChannelMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID);
        public RequestChannelMoveDelegate RequestChannelMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID)
        {
            return false;
        };
        public delegate bool RequestRoomMakeDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int betType, string pass);
        public RequestRoomMakeDelegate RequestRoomMake = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int betType, string pass)
        {
            return false;
        };
        public delegate bool RequestRoomJoinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int betType);
        public RequestRoomJoinDelegate RequestRoomJoin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int betType)
        {
            return false;
        };
        public delegate bool RequestRoomJoinSelectDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int roomNumber, string pass);
        public RequestRoomJoinSelectDelegate RequestRoomJoinSelect = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int chanID, int roomNumber, string pass)
        {
            return false;
        };
        public delegate bool RequestBankDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int option, long money, string pass);
        public RequestBankDelegate RequestBank = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int option, long money, string pass)
        {
            return false;
        };
        public delegate bool GameRoomOutRsvnDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool IsRsvn);
        public GameRoomOutRsvnDelegate GameRoomOutRsvn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool IsRsvn)
        {
            return false;
        };
        public delegate bool GameRoomOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRoomOutDelegate GameRoomOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRoomMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRoomMoveDelegate GameRoomMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRoomInUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRoomInUserDelegate GameRoomInUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRequestReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRequestReadyDelegate GameRequestReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameDealCardsEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameDealCardsEndDelegate GameDealCardsEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameActionBetDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameActionBetDelegate GameActionBet = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameActionChangeCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameActionChangeCardDelegate GameActionChangeCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RequestPurchaseListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption);
        public RequestPurchaseListDelegate RequestPurchaseList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            return false;
        };
        public delegate bool RequestPurchaseAvailabilityDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid);
        public RequestPurchaseAvailabilityDelegate RequestPurchaseAvailability = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid)
        {
            return false;
        };
        public delegate bool RequestPurchaseReceiptCheckDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string result);
        public RequestPurchaseReceiptCheckDelegate RequestPurchaseReceiptCheck = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string result)
        {
            return false;
        };
        public delegate bool RequestPurchaseResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid token);
        public RequestPurchaseResultDelegate RequestPurchaseResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid token)
        {
            return false;
        };
        public delegate bool RequestPurchaseCashDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid);
        public RequestPurchaseCashDelegate RequestPurchaseCash = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid)
        {
            return false;
        };
        public delegate bool RequestMyroomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption);
        public RequestMyroomListDelegate RequestMyroomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption)
        {
            return false;
        };
        public delegate bool RequestMyroomActionDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid);
        public RequestMyroomActionDelegate RequestMyroomAction = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string pid)
        {
            return false;
        };
        public delegate bool RequestGameOptionsDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public RequestGameOptionsDelegate RequestGameOptions = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RequestLobbyEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public RequestLobbyEventInfoDelegate RequestLobbyEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RequestLobbyEventParticipateDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public RequestLobbyEventParticipateDelegate RequestLobbyEventParticipate = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };

        public override bool ProcessMsg(ZNet.CRecvedMsg rm)
        {
            ZNet.RemoteID remote = rm.remote;
            if (remote == ZNet.RemoteID.Remote_None)
            {
                //err
            }

            ZNet.CPackOption pkOption = rm.pkop;
            ZNet.CMessage __msg = rm.msg;
            ZNet.PacketType PkID = rm.pkID;
            if (PkID < ZNet.PacketType.PacketType_User)
                return true;

            switch (PkID)
            {
                case Common.MasterAllShutdown:
                    {
                        string msg; Rmi.Marshaler.Read(__msg, out msg);

                        bool bRet = MasterAllShutdown(remote, pkOption, msg);
                        if (bRet == false)
                            NeedImplement("MasterAllShutdown");
                    }
                    break;

                case Common.MasterNotifyP2PServerInfo:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = MasterNotifyP2PServerInfo(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("MasterNotifyP2PServerInfo");
                    }
                    break;

                case Common.RoomLobbyMakeRoom:
                    {
                        Rmi.Marshaler.RoomInfo roomInfo; Rmi.Marshaler.Read(__msg, out roomInfo);
                        Rmi.Marshaler.LobbyUserList userInfo; Rmi.Marshaler.Read(__msg, out userInfo);
                        int userID; Rmi.Marshaler.Read(__msg, out userID);
                        string IP; Rmi.Marshaler.Read(__msg, out IP);
                        string Pass; Rmi.Marshaler.Read(__msg, out Pass);
                        int shopId; Rmi.Marshaler.Read(__msg, out shopId);

                        bool bRet = RoomLobbyMakeRoom(remote, pkOption, roomInfo, userInfo, userID, IP, Pass, shopId);
                        if (bRet == false)
                            NeedImplement("RoomLobbyMakeRoom");
                    }
                    break;

                case Common.RoomLobbyJoinRoom:
                    {
                        System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                        Rmi.Marshaler.LobbyUserList userInfo; Rmi.Marshaler.Read(__msg, out userInfo);
                        int userID; Rmi.Marshaler.Read(__msg, out userID);
                        string IP; Rmi.Marshaler.Read(__msg, out IP);
                        int shopId; Rmi.Marshaler.Read(__msg, out shopId);

                        bool bRet = RoomLobbyJoinRoom(remote, pkOption, roomID, userInfo, userID, IP, shopId);
                        if (bRet == false)
                            NeedImplement("RoomLobbyJoinRoom");
                    }
                    break;

                case Common.RoomLobbyOutRoom:
                    {
                        System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                        int userID; Rmi.Marshaler.Read(__msg, out userID);

                        bool bRet = RoomLobbyOutRoom(remote, pkOption, roomID, userID);
                        if (bRet == false)
                            NeedImplement("RoomLobbyOutRoom");
                    }
                    break;

                case Common.RoomLobbyMessage:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string message; Rmi.Marshaler.Read(__msg, out message);

                        bool bRet = RoomLobbyMessage(remote, pkOption, userRemote, message);
                        if (bRet == false)
                            NeedImplement("RoomLobbyMessage");
                    }
                    break;

                case Common.RoomLobbyEventStart:
                    {
                        System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                        int type; Rmi.Marshaler.Read(__msg, out type);

                        bool bRet = RoomLobbyEventStart(remote, pkOption, roomID, type);
                        if (bRet == false)
                            NeedImplement("RoomLobbyEventStart");
                    }
                    break;

                case Common.RoomLobbyEventEnd:
                    {
                        System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                        int type; Rmi.Marshaler.Read(__msg, out type);
                        string name; Rmi.Marshaler.Read(__msg, out name);
                        long reward; Rmi.Marshaler.Read(__msg, out reward);

                        bool bRet = RoomLobbyEventEnd(remote, pkOption, roomID, type, name, reward);
                        if (bRet == false)
                            NeedImplement("RoomLobbyEventEnd");
                    }
                    break;

                case Common.LobbyRoomJackpotInfo:
                    {
                        long jackpot; Rmi.Marshaler.Read(__msg, out jackpot);

                        bool bRet = LobbyRoomJackpotInfo(remote, pkOption, jackpot);
                        if (bRet == false)
                            NeedImplement("LobbyRoomJackpotInfo");
                    }
                    break;

                case Common.LobbyRoomNotifyMessage:
                    {
                        int type; Rmi.Marshaler.Read(__msg, out type);
                        string message; Rmi.Marshaler.Read(__msg, out message);
                        int period; Rmi.Marshaler.Read(__msg, out period);

                        bool bRet = LobbyRoomNotifyMessage(remote, pkOption, type, message, period);
                        if (bRet == false)
                            NeedImplement("LobbyRoomNotifyMessage");
                    }
                    break;

                case Common.LobbyRoomNotifyServermaintenance:
                    {
                        int type; Rmi.Marshaler.Read(__msg, out type);
                        string message; Rmi.Marshaler.Read(__msg, out message);
                        int release; Rmi.Marshaler.Read(__msg, out release);

                        bool bRet = LobbyRoomNotifyServermaintenance(remote, pkOption, type, message, release);
                        if (bRet == false)
                            NeedImplement("LobbyRoomNotifyServermaintenance");
                    }
                    break;

                case Common.LobbyRoomReloadServerData:
                    {
                        int type; Rmi.Marshaler.Read(__msg, out type);

                        bool bRet = LobbyRoomReloadServerData(remote, pkOption, type);
                        if (bRet == false)
                            NeedImplement("LobbyRoomReloadServerData");
                    }
                    break;

                case Common.LobbyRoomCalling:
                    {
                        int type; Rmi.Marshaler.Read(__msg, out type);
                        int chanId; Rmi.Marshaler.Read(__msg, out chanId);
                        System.Guid roomId; Rmi.Marshaler.Read(__msg, out roomId);
                        int playerId; Rmi.Marshaler.Read(__msg, out playerId);

                        bool bRet = LobbyRoomCalling(remote, pkOption, type, chanId, roomId, playerId);
                        if (bRet == false)
                            NeedImplement("LobbyRoomCalling");
                    }
                    break;

                case Common.LobbyRoomKickUser:
                    {
                        int userID; Rmi.Marshaler.Read(__msg, out userID);

                        bool bRet = LobbyRoomKickUser(remote, pkOption, userID);
                        if (bRet == false)
                            NeedImplement("LobbyRoomKickUser");
                    }
                    break;

                case Common.LobbyLoginKickUser:
                    {
                        int userID; Rmi.Marshaler.Read(__msg, out userID);

                        bool bRet = LobbyLoginKickUser(remote, pkOption, userID);
                        if (bRet == false)
                            NeedImplement("LobbyLoginKickUser");
                    }
                    break;

                case Common.RoomLobbyRequestMoveRoom:
                    {
                        System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                        ZNet.RemoteID remoteS; Rmi.Marshaler.Read(__msg, out remoteS);
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int userID; Rmi.Marshaler.Read(__msg, out userID);
                        long money; Rmi.Marshaler.Read(__msg, out money);
                        bool ipFree; Rmi.Marshaler.Read(__msg, out ipFree);
                        bool shopFree; Rmi.Marshaler.Read(__msg, out shopFree);
                        int shopId; Rmi.Marshaler.Read(__msg, out shopId);
                        bool restrict; Rmi.Marshaler.Read(__msg, out restrict);

                        bool bRet = RoomLobbyRequestMoveRoom(remote, pkOption, roomID, remoteS, userRemote, userID, money, ipFree, shopFree, shopId, restrict);
                        if (bRet == false)
                            NeedImplement("RoomLobbyRequestMoveRoom");
                    }
                    break;

                case Common.LobbyRoomResponseMoveRoom:
                    {
                        bool makeRoom; Rmi.Marshaler.Read(__msg, out makeRoom);
                        System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                        ZNet.NetAddress addr; Rmi.Marshaler.Read(__msg, out addr);
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                        ZNet.RemoteID remoteS; Rmi.Marshaler.Read(__msg, out remoteS);
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string message; Rmi.Marshaler.Read(__msg, out message);

                        bool bRet = LobbyRoomResponseMoveRoom(remote, pkOption, makeRoom, roomID, addr, chanID, remoteS, userRemote, message);
                        if (bRet == false)
                            NeedImplement("LobbyRoomResponseMoveRoom");
                    }
                    break;

                case Common.ServerRequestDataSync:
                    {
                        bool isLobby; Rmi.Marshaler.Read(__msg, out isLobby);

                        bool bRet = ServerRequestDataSync(remote, pkOption, isLobby);
                        if (bRet == false)
                            NeedImplement("ServerRequestDataSync");
                    }
                    break;

                case Common.RoomLobbyResponseDataSync:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RoomLobbyResponseDataSync(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("RoomLobbyResponseDataSync");
                    }
                    break;

                case Common.RelayLobbyResponseDataSync:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayLobbyResponseDataSync(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("RelayLobbyResponseDataSync");
                    }
                    break;

                case Common.RelayClientJoin:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        ZNet.NetAddress addr; Rmi.Marshaler.Read(__msg, out addr);
                        ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);

                        bool bRet = RelayClientJoin(remote, pkOption, userRemote, addr, param);
                        if (bRet == false)
                            NeedImplement("RelayClientJoin");
                    }
                    break;

                case Common.RelayClientLeave:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool bMoveServer; Rmi.Marshaler.Read(__msg, out bMoveServer);

                        bool bRet = RelayClientLeave(remote, pkOption, userRemote, bMoveServer);
                        if (bRet == false)
                            NeedImplement("RelayClientLeave");
                    }
                    break;

                case Common.RelayCloseRemoteClient:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                        bool bRet = RelayCloseRemoteClient(remote, pkOption, userRemote);
                        if (bRet == false)
                            NeedImplement("RelayCloseRemoteClient");
                    }
                    break;

                case Common.RelayServerMoveFailure:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                        bool bRet = RelayServerMoveFailure(remote, pkOption, userRemote);
                        if (bRet == false)
                            NeedImplement("RelayServerMoveFailure");
                    }
                    break;

                case Common.RelayRequestLobbyKey:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string id; Rmi.Marshaler.Read(__msg, out id);
                        string key; Rmi.Marshaler.Read(__msg, out key);
                        int gameid; Rmi.Marshaler.Read(__msg, out gameid);

                        bool bRet = RelayRequestLobbyKey(remote, pkOption, userRemote, id, key, gameid);
                        if (bRet == false)
                            NeedImplement("RelayRequestLobbyKey");
                    }
                    break;

                case Common.RelayRequestJoinInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                        bool bRet = RelayRequestJoinInfo(remote, pkOption, userRemote);
                        if (bRet == false)
                            NeedImplement("RelayRequestJoinInfo");
                    }
                    break;

                case Common.RelayRequestChannelMove:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                        bool bRet = RelayRequestChannelMove(remote, pkOption, userRemote, chanID);
                        if (bRet == false)
                            NeedImplement("RelayRequestChannelMove");
                    }
                    break;

                case Common.RelayRequestRoomMake:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int relayID; Rmi.Marshaler.Read(__msg, out relayID);
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                        int betType; Rmi.Marshaler.Read(__msg, out betType);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RelayRequestRoomMake(remote, pkOption, userRemote, relayID, chanID, betType, pass);
                        if (bRet == false)
                            NeedImplement("RelayRequestRoomMake");
                    }
                    break;

                case Common.RelayRequestRoomJoin:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int relayID; Rmi.Marshaler.Read(__msg, out relayID);
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                        int betType; Rmi.Marshaler.Read(__msg, out betType);

                        bool bRet = RelayRequestRoomJoin(remote, pkOption, userRemote, relayID, chanID, betType);
                        //if (bRet == false)
                        //    NeedImplement("RelayRequestRoomJoin");
                    }
                    break;

                case Common.RelayRequestRoomJoinSelect:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int relayID; Rmi.Marshaler.Read(__msg, out relayID);
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                        int roomNumber; Rmi.Marshaler.Read(__msg, out roomNumber);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RelayRequestRoomJoinSelect(remote, pkOption, userRemote, relayID, chanID, roomNumber, pass);
                        if (bRet == false)
                            NeedImplement("RelayRequestRoomJoinSelect");
                    }
                    break;

                case Common.RelayRequestBank:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int option; Rmi.Marshaler.Read(__msg, out option);
                        long money; Rmi.Marshaler.Read(__msg, out money);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RelayRequestBank(remote, pkOption, userRemote, option, money, pass);
                        if (bRet == false)
                            NeedImplement("RelayRequestBank");
                    }
                    break;

                case Common.RelayRequestPurchaseList:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                        bool bRet = RelayRequestPurchaseList(remote, pkOption, userRemote);
                        if (bRet == false)
                            NeedImplement("RelayRequestPurchaseList");
                    }
                    break;

                case Common.RelayRequestPurchaseAvailability:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string pid; Rmi.Marshaler.Read(__msg, out pid);

                        bool bRet = RelayRequestPurchaseAvailability(remote, pkOption, userRemote, pid);
                        if (bRet == false)
                            NeedImplement("RelayRequestPurchaseAvailability");
                    }
                    break;

                case Common.RelayRequestPurchaseReceiptCheck:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string result; Rmi.Marshaler.Read(__msg, out result);

                        bool bRet = RelayRequestPurchaseReceiptCheck(remote, pkOption, userRemote, result);
                        if (bRet == false)
                            NeedImplement("RelayRequestPurchaseReceiptCheck");
                    }
                    break;

                case Common.RelayRequestPurchaseResult:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                        bool bRet = RelayRequestPurchaseResult(remote, pkOption, userRemote, token);
                        if (bRet == false)
                            NeedImplement("RelayRequestPurchaseResult");
                    }
                    break;

                case Common.RelayRequestPurchaseCash:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string pid; Rmi.Marshaler.Read(__msg, out pid);

                        bool bRet = RelayRequestPurchaseCash(remote, pkOption, userRemote, pid);
                        if (bRet == false)
                            NeedImplement("RelayRequestPurchaseCash");
                    }
                    break;

                case Common.RelayRequestMyroomList:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                        bool bRet = RelayRequestMyroomList(remote, pkOption, userRemote);
                        if (bRet == false)
                            NeedImplement("RelayRequestMyroomList");
                    }
                    break;

                case Common.RelayRequestMyroomAction:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string pid; Rmi.Marshaler.Read(__msg, out pid);

                        bool bRet = RelayRequestMyroomAction(remote, pkOption, userRemote, pid);
                        if (bRet == false)
                            NeedImplement("RelayRequestMyroomAction");
                    }
                    break;

                case Common.LobbyRelayResponsePurchaseList:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        List<string> Purchase_avatar; Rmi.Marshaler.Read(__msg, out Purchase_avatar);
                        List<string> Purchase_card; Rmi.Marshaler.Read(__msg, out Purchase_card);
                        List<string> Purchase_evt; Rmi.Marshaler.Read(__msg, out Purchase_evt);
                        List<string> Purchase_charge; Rmi.Marshaler.Read(__msg, out Purchase_charge);

                        bool bRet = LobbyRelayResponsePurchaseList(remote, pkOption, userRemote, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponsePurchaseList");
                    }
                    break;

                case Common.LobbyRelayResponsePurchaseAvailability:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool available; Rmi.Marshaler.Read(__msg, out available);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = LobbyRelayResponsePurchaseAvailability(remote, pkOption, userRemote, available, reason);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponsePurchaseAvailability");
                    }
                    break;

                case Common.LobbyRelayResponsePurchaseReceiptCheck:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                        bool bRet = LobbyRelayResponsePurchaseReceiptCheck(remote, pkOption, userRemote, result, token);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponsePurchaseReceiptCheck");
                    }
                    break;

                case Common.LobbyRelayResponsePurchaseResult:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = LobbyRelayResponsePurchaseResult(remote, pkOption, userRemote, result, reason);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponsePurchaseResult");
                    }
                    break;

                case Common.LobbyRelayResponsePurchaseCash:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = LobbyRelayResponsePurchaseCash(remote, pkOption, userRemote, result, reason);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponsePurchaseCash");
                    }
                    break;

                case Common.LobbyRelayResponseMyroomList:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string json; Rmi.Marshaler.Read(__msg, out json);

                        bool bRet = LobbyRelayResponseMyroomList(remote, pkOption, userRemote, json);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseMyroomList");
                    }
                    break;

                case Common.LobbyRelayResponseMyroomAction:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string pid; Rmi.Marshaler.Read(__msg, out pid);
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = LobbyRelayResponseMyroomAction(remote, pkOption, userRemote, pid, result, reason);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseMyroomAction");
                    }
                    break;

                case Common.LobbyRelayServerMoveStart:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string moveServerIP; Rmi.Marshaler.Read(__msg, out moveServerIP);
                        ushort moveServerPort; Rmi.Marshaler.Read(__msg, out moveServerPort);
                        ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);
                        Guid guid; Rmi.Marshaler.Read(__msg, out guid);

                        bool bRet = LobbyRelayServerMoveStart(remote, pkOption, userRemote, moveServerIP, moveServerPort, param, guid);
                        if (bRet == false)
                            NeedImplement("LobbyRelayServerMoveStart");
                    }
                    break;

                case Common.LobbyRelayResponseLobbyKey:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string key; Rmi.Marshaler.Read(__msg, out key);
                        int gameid; Rmi.Marshaler.Read(__msg, out gameid);

                        bool bRet = LobbyRelayResponseLobbyKey(remote, pkOption, userRemote, key, gameid);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseLobbyKey");
                    }
                    break;

                case Common.LobbyRelayNotifyUserInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        Rmi.Marshaler.LobbyUserInfo userInfo; Rmi.Marshaler.Read(__msg, out userInfo);

                        bool bRet = LobbyRelayNotifyUserInfo(remote, pkOption, userRemote, userInfo);
                        if (bRet == false)
                            NeedImplement("LobbyRelayNotifyUserInfo");
                    }
                    break;

                case Common.LobbyRelayNotifyUserList:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; Rmi.Marshaler.Read(__msg, out lobbyUserInfos);
                        System.Collections.Generic.List<string> lobbyFriendList; Rmi.Marshaler.Read(__msg, out lobbyFriendList);

                        bool bRet = LobbyRelayNotifyUserList(remote, pkOption, userRemote, lobbyUserInfos, lobbyFriendList);
                        //if (bRet == false)
                        //    NeedImplement("LobbyRelayNotifyUserList");
                    }
                    break;

                case Common.LobbyRelayNotifyRoomList:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int channelID; Rmi.Marshaler.Read(__msg, out channelID);
                        System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos; Rmi.Marshaler.Read(__msg, out roomInfos);

                        bool bRet = LobbyRelayNotifyRoomList(remote, pkOption, userRemote, channelID, roomInfos);
                        if (bRet == false)
                            NeedImplement("LobbyRelayNotifyRoomList");
                    }
                    break;

                case Common.LobbyRelayResponseChannelMove:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                        bool bRet = LobbyRelayResponseChannelMove(remote, pkOption, userRemote, chanID);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseChannelMove");
                    }
                    break;

                case Common.LobbyRelayResponseLobbyMessage:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        string message; Rmi.Marshaler.Read(__msg, out message);

                        bool bRet = LobbyRelayResponseLobbyMessage(remote, pkOption, userRemote, message);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseLobbyMessage");
                    }
                    break;

                case Common.LobbyRelayResponseBank:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        int resultType; Rmi.Marshaler.Read(__msg, out resultType);

                        bool bRet = LobbyRelayResponseBank(remote, pkOption, userRemote, result, resultType);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseBank");
                    }
                    break;

                case Common.LobbyRelayNotifyJackpotInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        long jackpot; Rmi.Marshaler.Read(__msg, out jackpot);

                        bool bRet = LobbyRelayNotifyJackpotInfo(remote, pkOption, userRemote, jackpot);
                        if (bRet == false)
                            NeedImplement("LobbyRelayNotifyJackpotInfo");
                    }
                    break;

                case Common.LobbyRelayNotifyLobbyMessage:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        int type; Rmi.Marshaler.Read(__msg, out type);
                        string message; Rmi.Marshaler.Read(__msg, out message);
                        int period; Rmi.Marshaler.Read(__msg, out period);

                        bool bRet = LobbyRelayNotifyLobbyMessage(remote, pkOption, userRemote, type, message, period);
                        if (bRet == false)
                            NeedImplement("LobbyRelayNotifyLobbyMessage");
                    }
                    break;

                case Common.RoomRelayServerMoveStart:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        ZNet.NetAddress addr; Rmi.Marshaler.Read(__msg, out addr);
                        ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);
                        Guid idx; Rmi.Marshaler.Read(__msg, out idx);

                        bool bRet = RoomRelayServerMoveStart(remote, pkOption, userRemote, addr, param, idx);
                        if (bRet == false)
                            NeedImplement("RoomRelayServerMoveStart");
                    }
                    break;

                case Common.RelayRequestRoomOutRsvn:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool IsRsvn; Rmi.Marshaler.Read(__msg, out IsRsvn);

                        bool bRet = RelayRequestRoomOutRsvn(remote, pkOption, userRemote, IsRsvn);
                        if (bRet == false)
                            NeedImplement("RelayRequestRoomOutRsvn");
                    }
                    break;

                case Common.RelayRequestRoomOut:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                        bool bRet = RelayRequestRoomOut(remote, pkOption, userRemote);
                        if (bRet == false)
                            NeedImplement("RelayRequestRoomOut");
                    }
                    break;

                case Common.RelayResponseRoomOut:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool resultOut; Rmi.Marshaler.Read(__msg, out resultOut);

                        bool bRet = RelayResponseRoomOut(remote, pkOption, userRemote, resultOut);
                        if (bRet == false)
                            NeedImplement("RelayResponseRoomOut");
                    }
                    break;

                case Common.RelayRequestRoomMove:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                        bool bRet = RelayRequestRoomMove(remote, pkOption, userRemote);
                        if (bRet == false)
                            NeedImplement("RelayRequestRoomMove");
                    }
                    break;

                case Common.RelayResponseRoomMove:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool resultMove; Rmi.Marshaler.Read(__msg, out resultMove);
                        string errorMessage; Rmi.Marshaler.Read(__msg, out errorMessage);

                        bool bRet = RelayResponseRoomMove(remote, pkOption, userRemote, resultMove, errorMessage);
                        if (bRet == false)
                            NeedImplement("RelayResponseRoomMove");
                    }
                    break;

                case Common.RelayGameRoomIn:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayGameRoomIn(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("RelayGameRoomIn");
                    }
                    break;

                case Common.RelayGameRequestReady:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayGameRequestReady(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("RelayGameRequestReady");
                    }
                    break;

                case Common.RelayGameDealCardsEnd:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayGameDealCardsEnd(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("RelayGameDealCardsEnd");
                    }
                    break;

                case Common.RelayGameActionBet:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayGameActionBet(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("RelayGameActionBet");
                    }
                    break;

                case Common.RelayGameActionChangeCard:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayGameActionChangeCard(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("RelayGameActionChangeCard");
                    }
                    break;

                case Common.GameRelayResponseRoomOutRsvn:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        byte player_index; Rmi.Marshaler.Read(__msg, out player_index);
                        bool Rsvn; Rmi.Marshaler.Read(__msg, out Rsvn);

                        bool bRet = GameRelayResponseRoomOutRsvn(remote, pkOption, userRemote, player_index, Rsvn);
                        if (bRet == false)
                            NeedImplement("GameRelayResponseRoomOutRsvn");
                    }
                    break;

                case Common.GameRelayResponseRoomOut:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool permissionOut; Rmi.Marshaler.Read(__msg, out permissionOut);

                        bool bRet = GameRelayResponseRoomOut(remote, pkOption, userRemote, permissionOut);
                        if (bRet == false)
                            NeedImplement("GameRelayResponseRoomOut");
                    }
                    break;

                case Common.GameRelayResponseRoomMove:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool resultMove; Rmi.Marshaler.Read(__msg, out resultMove);
                        string errorMessage; Rmi.Marshaler.Read(__msg, out errorMessage);

                        bool bRet = GameRelayResponseRoomMove(remote, pkOption, userRemote, resultMove, errorMessage);
                        if (bRet == false)
                            NeedImplement("GameRelayResponseRoomMove");
                    }
                    break;

                case Common.GameRelayRoomIn:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        bool result; Rmi.Marshaler.Read(__msg, out result);

                        bool bRet = GameRelayRoomIn(remote, pkOption, userRemote, result);
                        if (bRet == false)
                            NeedImplement("GameRelayRoomIn");
                    }
                    break;

                case Common.GameRelayRoomReady:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayRoomReady(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayRoomReady");
                    }
                    break;

                case Common.GameRelayStart:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayStart(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayStart");
                    }
                    break;

                case Common.GameRelayDealCards:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayDealCards(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayDealCards");
                    }
                    break;

                case Common.GameRelayUserIn:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayUserIn(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayUserIn");
                    }
                    break;

                case Common.GameRelaySetBoss:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelaySetBoss(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelaySetBoss");
                    }
                    break;

                case Common.GameRelayNotifyStat:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayNotifyStat(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayNotifyStat");
                    }
                    break;

                case Common.GameRelayRoundStart:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayRoundStart(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayRoundStart");
                    }
                    break;

                case Common.GameRelayChangeTurn:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayChangeTurn(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayChangeTurn");
                    }
                    break;

                case Common.GameRelayRequestBet:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayRequestBet(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayRequestBet");
                    }
                    break;

                case Common.GameRelayResponseBet:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayResponseBet(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayResponseBet");
                    }
                    break;

                case Common.GameRelayChangeRound:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayChangeRound(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayChangeRound");
                    }
                    break;

                case Common.GameRelayRequestChangeCard:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayRequestChangeCard(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayRequestChangeCard");
                    }
                    break;

                case Common.GameRelayResponseChangeCard:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayResponseChangeCard(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayResponseChangeCard");
                    }
                    break;

                case Common.GameRelayCardOpen:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayCardOpen(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayCardOpen");
                    }
                    break;

                case Common.GameRelayOver:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayOver(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayOver");
                    }
                    break;

                case Common.GameRelayRoomInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayRoomInfo(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayRoomInfo");
                    }
                    break;

                case Common.GameRelayKickUser:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayKickUser(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayKickUser");
                    }
                    break;

                case Common.GameRelayEventInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayEventInfo(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayEventInfo");
                    }
                    break;

                case Common.GameRelayUserInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayUserInfo(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayUserInfo");
                    }
                    break;

                case Common.GameRelayUserOut:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayUserOut(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayUserOut");
                    }
                    break;

                case Common.GameRelayEventStart:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayEventStart(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayEventStart");
                    }
                    break;

                case Common.GameRelayEvent2Start:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayEvent2Start(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayEvent2Start");
                    }
                    break;

                case Common.GameRelayEventRefresh:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayEventRefresh(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayEventRefresh");
                    }
                    break;

                case Common.GameRelayEventEnd:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayEventEnd(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayEventEnd");
                    }
                    break;

                case Common.GameRelayMileageRefresh:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayMileageRefresh(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayMileageRefresh");
                    }
                    break;

                case Common.GameRelayEventNotify:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayEventNotify(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayEventNotify");
                    }
                    break;

                case Common.GameRelayCurrentInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayCurrentInfo(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayCurrentInfo");
                    }
                    break;

                case Common.GameRelayEntrySpectator:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayEntrySpectator(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayEntrySpectator");
                    }
                    break;

                case Common.GameRelayNotifyMessage:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayNotifyMessage(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayNotifyMessage");
                    }
                    break;

                case Common.GameRelayNotifyJackpotInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRelayNotifyJackpotInfo(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("GameRelayNotifyJackpotInfo");
                    }
                    break;

                case Common.RelayRequestLobbyEventInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayRequestLobbyEventInfo(remote, pkOption, userRemote, null);
                        if (bRet == false)
                            NeedImplement("RelayRequestLobbyEventInfo");
                    }
                    break;

                case Common.LobbyRelayResponseLobbyEventInfo:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = LobbyRelayResponseLobbyEventInfo(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseLobbyEventInfo");
                    }
                    break;

                case Common.RelayRequestLobbyEventParticipate:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RelayRequestLobbyEventParticipate(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("RelayRequestLobbyEventParticipate");
                    }
                    break;

                case Common.LobbyRelayResponseLobbyEventParticipate:
                    {
                        ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = LobbyRelayResponseLobbyEventParticipate(remote, pkOption, userRemote, __msg);
                        if (bRet == false)
                            NeedImplement("LobbyRelayResponseLobbyEventParticipate");
                    }
                    break;

                case Common.ServerMoveStart:
                    {
                        ZNet.NetAddress addr; Rmi.Marshaler.Read(__msg, out addr);
                        ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);
                        Guid idx; Rmi.Marshaler.Read(__msg, out idx);

                        bool bRet = ServerMoveStart(remote, pkOption, addr, param, idx);
                        if (bRet == false)
                            NeedImplement("ServerMoveStart");
                    }
                    break;

                case Common.ServerMoveEnd:
                    {
                        bool Moved; Rmi.Marshaler.Read(__msg, out Moved);

                        bool bRet = ServerMoveEnd(remote, pkOption, Moved);
                        if (bRet == false)
                            NeedImplement("ServerMoveEnd");
                    }
                    break;

                case Common.ResponseLauncherLogin:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string nickname; Rmi.Marshaler.Read(__msg, out nickname);
                        string key; Rmi.Marshaler.Read(__msg, out key);
                        byte resultType; Rmi.Marshaler.Read(__msg, out resultType);

                        bool bRet = ResponseLauncherLogin(remote, pkOption, result, nickname, key, resultType);
                        if (bRet == false)
                            NeedImplement("ResponseLauncherLogin");
                    }
                    break;

                case Common.ResponseLauncherLogout:
                    {

                        bool bRet = ResponseLauncherLogout(remote, pkOption);
                        if (bRet == false)
                            NeedImplement("ResponseLauncherLogout");
                    }
                    break;

                case Common.ResponseLoginKey:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string resultMessage; Rmi.Marshaler.Read(__msg, out resultMessage);

                        bool bRet = ResponseLoginKey(remote, pkOption, result, resultMessage);
                        if (bRet == false)
                            NeedImplement("ResponseLoginKey");
                    }
                    break;

                case Common.ResponseLobbyKey:
                    {
                        string key; Rmi.Marshaler.Read(__msg, out key);
                        int gameid; Rmi.Marshaler.Read(__msg, out gameid);

                        bool bRet = ResponseLobbyKey(remote, pkOption, key, gameid);
                        if (bRet == false)
                            NeedImplement("ResponseLobbyKey");
                    }
                    break;

                case Common.ResponseLogin:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string resultMessage; Rmi.Marshaler.Read(__msg, out resultMessage);

                        bool bRet = ResponseLogin(remote, pkOption, result, resultMessage);
                        if (bRet == false)
                            NeedImplement("ResponseLogin");
                    }
                    break;

                case Common.NotifyLobbyList:
                    {
                        System.Collections.Generic.List<string> lobbyList; Rmi.Marshaler.Read(__msg, out lobbyList);

                        bool bRet = NotifyLobbyList(remote, pkOption, lobbyList);
                        if (bRet == false)
                            NeedImplement("NotifyLobbyList");
                    }
                    break;

                case Common.NotifyUserInfo:
                    {
                        Rmi.Marshaler.LobbyUserInfo userInfo; Rmi.Marshaler.Read(__msg, out userInfo);

                        bool bRet = NotifyUserInfo(remote, pkOption, userInfo);
                        if (bRet == false)
                            NeedImplement("NotifyUserInfo");
                    }
                    break;

                case Common.NotifyUserList:
                    {
                        System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; Rmi.Marshaler.Read(__msg, out lobbyUserInfos);
                        System.Collections.Generic.List<string> lobbyFriendList; Rmi.Marshaler.Read(__msg, out lobbyFriendList);

                        bool bRet = NotifyUserList(remote, pkOption, lobbyUserInfos, lobbyFriendList);
                        if (bRet == false)
                            NeedImplement("NotifyUserList");
                    }
                    break;

                case Common.NotifyRoomList:
                    {
                        int channelID; Rmi.Marshaler.Read(__msg, out channelID);
                        System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos; Rmi.Marshaler.Read(__msg, out roomInfos);

                        bool bRet = NotifyRoomList(remote, pkOption, channelID, roomInfos);
                        if (bRet == false)
                            NeedImplement("NotifyRoomList");
                    }
                    break;

                case Common.ResponseChannelMove:
                    {
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                        bool bRet = ResponseChannelMove(remote, pkOption, chanID);
                        if (bRet == false)
                            NeedImplement("ResponseChannelMove");
                    }
                    break;

                case Common.ResponseLobbyMessage:
                    {
                        string message; Rmi.Marshaler.Read(__msg, out message);

                        bool bRet = ResponseLobbyMessage(remote, pkOption, message);
                        if (bRet == false)
                            NeedImplement("ResponseLobbyMessage");
                    }
                    break;

                case Common.ResponseBank:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        int resultType; Rmi.Marshaler.Read(__msg, out resultType);

                        bool bRet = ResponseBank(remote, pkOption, result, resultType);
                        if (bRet == false)
                            NeedImplement("ResponseBank");
                    }
                    break;

                case Common.NotifyJackpotInfo:
                    {
                        long jackpot; Rmi.Marshaler.Read(__msg, out jackpot);

                        bool bRet = NotifyJackpotInfo(remote, pkOption, jackpot);
                        if (bRet == false)
                            NeedImplement("NotifyJackpotInfo");
                    }
                    break;

                case Common.NotifyLobbyMessage:
                    {
                        int type; Rmi.Marshaler.Read(__msg, out type);
                        string message; Rmi.Marshaler.Read(__msg, out message);
                        int period; Rmi.Marshaler.Read(__msg, out period);

                        bool bRet = NotifyLobbyMessage(remote, pkOption, type, message, period);
                        if (bRet == false)
                            NeedImplement("NotifyLobbyMessage");
                    }
                    break;

                case Common.GameResponseRoomOutRsvp:
                    {
                        byte player_index; Rmi.Marshaler.Read(__msg, out player_index);
                        bool IsRsvn; Rmi.Marshaler.Read(__msg, out IsRsvn);

                        bool bRet = GameResponseRoomOutRsvp(remote, pkOption, player_index, IsRsvn);
                        if (bRet == false)
                            NeedImplement("GameResponseRoomOutRsvp");
                    }
                    break;

                case Common.GameResponseRoomOut:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);

                        bool bRet = GameResponseRoomOut(remote, pkOption, result);
                        if (bRet == false)
                            NeedImplement("GameResponseRoomOut");
                    }
                    break;

                case Common.GameResponseRoomMove:
                    {
                        bool move; Rmi.Marshaler.Read(__msg, out move);
                        string errorMessage; Rmi.Marshaler.Read(__msg, out errorMessage);

                        bool bRet = GameResponseRoomMove(remote, pkOption, move, errorMessage);
                        if (bRet == false)
                            NeedImplement("GameResponseRoomMove");
                    }
                    break;

                case Common.GameRoomIn:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);

                        bool bRet = GameRoomIn(remote, pkOption, result);
                        if (bRet == false)
                            NeedImplement("GameRoomIn");
                    }
                    break;

                case Common.GameRoomReady:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRoomReady(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRoomReady");
                    }
                    break;

                case Common.GameStart:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameStart(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameStart");
                    }
                    break;

                case Common.GameDealCards:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameDealCards(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameDealCards");
                    }
                    break;

                case Common.GameUserIn:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameUserIn(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameUserIn");
                    }
                    break;

                case Common.GameSetBoss:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameSetBoss(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameSetBoss");
                    }
                    break;

                case Common.GameNotifyStat:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameNotifyStat(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameNotifyStat");
                    }
                    break;

                case Common.GameRoundStart:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRoundStart(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRoundStart");
                    }
                    break;

                case Common.GameChangeTurn:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameChangeTurn(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameChangeTurn");
                    }
                    break;

                case Common.GameRequestBet:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRequestBet(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRequestBet");
                    }
                    break;

                case Common.GameResponseBet:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameResponseBet(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameResponseBet");
                    }
                    break;

                case Common.GameChangeRound:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameChangeRound(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameChangeRound");
                    }
                    break;

                case Common.GameRequestChangeCard:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRequestChangeCard(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRequestChangeCard");
                    }
                    break;

                case Common.GameResponseChangeCard:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameResponseChangeCard(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameResponseChangeCard");
                    }
                    break;

                case Common.GameCardOpen:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameCardOpen(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameCardOpen");
                    }
                    break;

                case Common.GameOver:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameOver(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameOver");
                    }
                    break;

                case Common.GameRoomInfo:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRoomInfo(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRoomInfo");
                    }
                    break;

                case Common.GameKickUser:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameKickUser(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameKickUser");
                    }
                    break;

                case Common.GameEventInfo:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameEventInfo(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameEventInfo");
                    }
                    break;

                case Common.GameUserInfo:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameUserInfo(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameUserInfo");
                    }
                    break;

                case Common.GameUserOut:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameUserOut(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameUserOut");
                    }
                    break;

                case Common.GameUserOutRsvn:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameUserOutRsvn(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameUserOutRsvn");
                    }
                    break;

                case Common.GameEventStart:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameEventStart(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameEventStart");
                    }
                    break;

                case Common.GameEvent2Start:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameEvent2Start(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameEvent2Start");
                    }
                    break;

                case Common.GameEventRefresh:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameEventRefresh(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameEventRefresh");
                    }
                    break;

                case Common.GameEventEnd:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameEventEnd(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameEventEnd");
                    }
                    break;

                case Common.GameMileageRefresh:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameMileageRefresh(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameMileageRefresh");
                    }
                    break;

                case Common.GameEventNotify:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameEventNotify(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameEventNotify");
                    }
                    break;

                case Common.GameCurrentInfo:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameCurrentInfo(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameCurrentInfo");
                    }
                    break;

                case Common.GameEntrySpectator:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameEntrySpectator(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameEntrySpectator");
                    }
                    break;

                case Common.GameNotifyMessage:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameNotifyMessage(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameNotifyMessage");
                    }
                    break;

                case Common.ResponsePurchaseList:
                    {
                        System.Collections.Generic.List<string> Purchase_avatar; Rmi.Marshaler.Read(__msg, out Purchase_avatar);
                        System.Collections.Generic.List<string> Purchase_card; Rmi.Marshaler.Read(__msg, out Purchase_card);
                        System.Collections.Generic.List<string> Purchase_evt; Rmi.Marshaler.Read(__msg, out Purchase_evt);
                        System.Collections.Generic.List<string> Purchase_charge; Rmi.Marshaler.Read(__msg, out Purchase_charge);

                        bool bRet = ResponsePurchaseList(remote, pkOption, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
                        if (bRet == false)
                            NeedImplement("ResponsePurchaseList");
                    }
                    break;

                case Common.ResponsePurchaseAvailability:
                    {
                        bool available; Rmi.Marshaler.Read(__msg, out available);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = ResponsePurchaseAvailability(remote, pkOption, available, reason);
                        if (bRet == false)
                            NeedImplement("ResponsePurchaseAvailability");
                    }
                    break;

                case Common.ResponsePurchaseReceiptCheck:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                        bool bRet = ResponsePurchaseReceiptCheck(remote, pkOption, result, token);
                        if (bRet == false)
                            NeedImplement("ResponsePurchaseReceiptCheck");
                    }
                    break;

                case Common.ResponsePurchaseResult:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = ResponsePurchaseResult(remote, pkOption, result, reason);
                        if (bRet == false)
                            NeedImplement("ResponsePurchaseResult");
                    }
                    break;

                case Common.ResponsePurchaseCash:
                    {
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = ResponsePurchaseCash(remote, pkOption, result, reason);
                        if (bRet == false)
                            NeedImplement("ResponsePurchaseCash");
                    }
                    break;

                case Common.ResponseMyroomList:
                    {
                        string json; Rmi.Marshaler.Read(__msg, out json);

                        bool bRet = ResponseMyroomList(remote, pkOption, json);
                        if (bRet == false)
                            NeedImplement("ResponseMyroomList");
                    }
                    break;

                case Common.ResponseMyroomAction:
                    {
                        string pid; Rmi.Marshaler.Read(__msg, out pid);
                        bool result; Rmi.Marshaler.Read(__msg, out result);
                        string reason; Rmi.Marshaler.Read(__msg, out reason);

                        bool bRet = ResponseMyroomAction(remote, pkOption, pid, result, reason);
                        if (bRet == false)
                            NeedImplement("ResponseMyroomAction");
                    }
                    break;

                case Common.ResponseGameOptions:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = ResponseGameOptions(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("ResponseGameOptions");
                    }
                    break;

                case Common.ResponseLobbyEventInfo:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = ResponseLobbyEventInfo(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("ResponseLobbyEventInfo");
                    }
                    break;

                case Common.ResponseLobbyEventParticipate:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = ResponseLobbyEventParticipate(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("ResponseLobbyEventParticipate");
                    }
                    break;

                case Common.ServerMoveFailure:
                    {

                        bool bRet = ServerMoveFailure(remote, pkOption);
                        if (bRet == false)
                            NeedImplement("ServerMoveFailure");
                    }
                    break;

                case Common.RequestLauncherLogin:
                    {
                        string id; Rmi.Marshaler.Read(__msg, out id);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RequestLauncherLogin(remote, pkOption, id, pass);
                        if (bRet == false)
                            NeedImplement("RequestLauncherLogin");
                    }
                    break;

                case Common.RequestLauncherLogout:
                    {
                        string id; Rmi.Marshaler.Read(__msg, out id);
                        string key; Rmi.Marshaler.Read(__msg, out key);

                        bool bRet = RequestLauncherLogout(remote, pkOption, id, key);
                        if (bRet == false)
                            NeedImplement("RequestLauncherLogout");
                    }
                    break;

                case Common.RequestLoginKey:
                    {
                        string id; Rmi.Marshaler.Read(__msg, out id);
                        string key; Rmi.Marshaler.Read(__msg, out key);

                        bool bRet = RequestLoginKey(remote, pkOption, id, key);
                        if (bRet == false)
                            NeedImplement("RequestLoginKey");
                    }
                    break;

                case Common.RequestLobbyKey:
                    {
                        string id; Rmi.Marshaler.Read(__msg, out id);
                        string key; Rmi.Marshaler.Read(__msg, out key);
                        int gameid; Rmi.Marshaler.Read(__msg, out gameid);

                        bool bRet = RequestLobbyKey(remote, pkOption, id, key, gameid);
                        if (bRet == false)
                            NeedImplement("RequestLobbyKey");
                    }
                    break;

                case Common.RequestLogin:
                    {
                        string name; Rmi.Marshaler.Read(__msg, out name);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RequestLogin(remote, pkOption, name, pass);
                        if (bRet == false)
                            NeedImplement("RequestLogin");
                    }
                    break;

                case Common.RequestLobbyList:
                    {

                        bool bRet = RequestLobbyList(remote, pkOption);
                        if (bRet == false)
                            NeedImplement("RequestLobbyList");
                    }
                    break;

                case Common.RequestGoLobby:
                    {
                        string lobbyName; Rmi.Marshaler.Read(__msg, out lobbyName);

                        bool bRet = RequestGoLobby(remote, pkOption, lobbyName);
                        if (bRet == false)
                            NeedImplement("RequestGoLobby");
                    }
                    break;

                case Common.RequestJoinInfo:
                    {

                        bool bRet = RequestJoinInfo(remote, pkOption);
                        if (bRet == false)
                            NeedImplement("RequestJoinInfo");
                    }
                    break;

                case Common.RequestChannelMove:
                    {
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                        bool bRet = RequestChannelMove(remote, pkOption, chanID);
                        if (bRet == false)
                            NeedImplement("RequestChannelMove");
                    }
                    break;

                case Common.RequestRoomMake:
                    {
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                        int betType; Rmi.Marshaler.Read(__msg, out betType);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RequestRoomMake(remote, pkOption, chanID, betType, pass);
                        if (bRet == false)
                            NeedImplement("RequestRoomMake");
                    }
                    break;

                case Common.RequestRoomJoin:
                    {
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                        int betType; Rmi.Marshaler.Read(__msg, out betType);

                        bool bRet = RequestRoomJoin(remote, pkOption, chanID, betType);
                        if (bRet == false)
                            NeedImplement("RequestRoomJoin");
                    }
                    break;

                case Common.RequestRoomJoinSelect:
                    {
                        int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                        int roomNumber; Rmi.Marshaler.Read(__msg, out roomNumber);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RequestRoomJoinSelect(remote, pkOption, chanID, roomNumber, pass);
                        if (bRet == false)
                            NeedImplement("RequestRoomJoinSelect");
                    }
                    break;

                case Common.RequestBank:
                    {
                        int option; Rmi.Marshaler.Read(__msg, out option);
                        long money; Rmi.Marshaler.Read(__msg, out money);
                        string pass; Rmi.Marshaler.Read(__msg, out pass);

                        bool bRet = RequestBank(remote, pkOption, option, money, pass);
                        if (bRet == false)
                            NeedImplement("RequestBank");
                    }
                    break;

                case Common.GameRoomOutRsvn: // 중계서버 
                    {
                        bool IsRsvn; Rmi.Marshaler.Read(__msg, out IsRsvn);

                        bool bRet = GameRoomOutRsvn(remote, pkOption, IsRsvn);
                        if (bRet == false)
                            NeedImplement("GameRoomOutRsvn");
                    }
                    break;

                case Common.GameRoomOut: // 중계서버 
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRoomOut(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRoomOut");
                    }
                    break;

                case Common.GameRoomMove: // 중계서버 
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRoomMove(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRoomMove");
                    }
                    break;

                case Common.GameRoomInUser: // 중계서버 
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRoomInUser(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameRoomInUser");
                    }
                    break;

                case Common.GameRequestReady: // 중계서버 
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameRequestReady(remote, pkOption, __msg);
                        //if (bRet == false)
                        //    NeedImplement("GameRequestReady");
                    }
                    break;

                case Common.GameDealCardsEnd: // 중계서버 
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameDealCardsEnd(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameDealCardsEnd");
                    }
                    break;

                case Common.GameActionBet: // 중계서버 
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameActionBet(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameActionBet");
                    }
                    break;

                case Common.GameActionChangeCard: // 중계서버 
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = GameActionChangeCard(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("GameActionChangeCard");
                    }
                    break;

                case Common.RequestPurchaseList:
                    {

                        bool bRet = RequestPurchaseList(remote, pkOption);
                        if (bRet == false)
                            NeedImplement("RequestPurchaseList");
                    }
                    break;

                case Common.RequestPurchaseAvailability:
                    {
                        string pid; Rmi.Marshaler.Read(__msg, out pid);

                        bool bRet = RequestPurchaseAvailability(remote, pkOption, pid);
                        if (bRet == false)
                            NeedImplement("RequestPurchaseAvailability");
                    }
                    break;

                case Common.RequestPurchaseReceiptCheck:
                    {
                        string result; Rmi.Marshaler.Read(__msg, out result);

                        bool bRet = RequestPurchaseReceiptCheck(remote, pkOption, result);
                        if (bRet == false)
                            NeedImplement("RequestPurchaseReceiptCheck");
                    }
                    break;

                case Common.RequestPurchaseResult:
                    {
                        System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                        bool bRet = RequestPurchaseResult(remote, pkOption, token);
                        if (bRet == false)
                            NeedImplement("RequestPurchaseResult");
                    }
                    break;

                case Common.RequestPurchaseCash:
                    {
                        string pid; Rmi.Marshaler.Read(__msg, out pid);

                        bool bRet = RequestPurchaseCash(remote, pkOption, pid);
                        if (bRet == false)
                            NeedImplement("RequestPurchaseCash");
                    }
                    break;

                case Common.RequestMyroomList:
                    {

                        bool bRet = RequestMyroomList(remote, pkOption);
                        if (bRet == false)
                            NeedImplement("RequestMyroomList");
                    }
                    break;

                case Common.RequestMyroomAction:
                    {
                        string pid; Rmi.Marshaler.Read(__msg, out pid);

                        bool bRet = RequestMyroomAction(remote, pkOption, pid);
                        if (bRet == false)
                            NeedImplement("RequestMyroomAction");
                    }
                    break;

                case Common.RequestGameOptions:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RequestGameOptions(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("RequestGameOptions");
                    }
                    break;

                case Common.RequestLobbyEventInfo:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RequestLobbyEventInfo(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("RequestLobbyEventInfo");
                    }
                    break;

                case Common.RequestLobbyEventParticipate:
                    {
                        //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                        bool bRet = RequestLobbyEventParticipate(remote, pkOption, __msg);
                        if (bRet == false)
                            NeedImplement("RequestLobbyEventParticipate");
                    }
                    break;

                default: goto __fail;
            }

            return true;

        __fail:
            {
                //err
                return false;
            }
        }

    }

}

