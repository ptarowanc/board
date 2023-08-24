// Auto created from IDLCompiler.exe
using System;
using System.Collections.Generic;
using System.Net;


namespace SS
{

    public class Stub : ZNet.PKStub
    {
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
        public delegate bool P2PMemberCheckDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption);
        public P2PMemberCheckDelegate P2PMemberCheck = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption)
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
        public delegate bool RoomLobbyRequestMoveRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, ZNet.RemoteID userRemote, int userID, long money, bool ipFree, bool shopFree, int shopId);
        public RoomLobbyRequestMoveRoomDelegate RoomLobbyRequestMoveRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, System.Guid roomID, ZNet.RemoteID userRemote, int userID, long money, bool ipFree, bool shopFree, int shopId)
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
        public delegate bool RelayLobbyResponseDataSyncDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public RelayLobbyResponseDataSyncDelegate RelayLobbyResponseDataSync = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
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
        public delegate bool RelayRequestLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string id, string key);
        public RelayRequestLobbyKeyDelegate RelayRequestLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string id, string key)
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
        public delegate bool LobbyRelayResponseLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string key);
        public LobbyRelayResponseLobbyKeyDelegate LobbyRelayResponseLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, string key)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, Rmi.Marshaler.LobbyUserInfo userInfo);
        public LobbyRelayNotifyUserInfoDelegate LobbyRelayNotifyUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyUserListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList);
        public LobbyRelayNotifyUserListDelegate LobbyRelayNotifyUserList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList)
        {
            return false;
        };
        public delegate bool LobbyRelayNotifyRoomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos);
        public LobbyRelayNotifyRoomListDelegate LobbyRelayNotifyRoomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos)
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
        public delegate bool RelayRequestOutRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayRequestOutRoomDelegate RelayRequestOutRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayResponseOutRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultOut);
        public RelayResponseOutRoomDelegate RelayResponseOutRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultOut)
        {
            return false;
        };
        public delegate bool RelayRequestMoveRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote);
        public RelayRequestMoveRoomDelegate RelayRequestMoveRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote)
        {
            return false;
        };
        public delegate bool RelayResponseMoveRoomDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage);
        public RelayResponseMoveRoomDelegate RelayResponseMoveRoom = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            return false;
        };
        public delegate bool RelayGameRoomInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameRoomInDelegate RelayGameRoomIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameReadyDelegate RelayGameReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameSelectOrderDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameSelectOrderDelegate RelayGameSelectOrder = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameDistributedEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameDistributedEndDelegate RelayGameDistributedEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameActionPutCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameActionPutCardDelegate RelayGameActionPutCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameActionFlipBombDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameActionFlipBombDelegate RelayGameActionFlipBomb = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameActionChooseCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameActionChooseCardDelegate RelayGameActionChooseCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameSelectKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameSelectKookjinDelegate RelayGameSelectKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameSelectGoStopDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameSelectGoStopDelegate RelayGameSelectGoStop = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGameSelectPushDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGameSelectPushDelegate RelayGameSelectPush = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool RelayGamePracticeDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayGamePracticeDelegate RelayGamePractice = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRoomInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result);
        public GameRelayRoomInDelegate GameRelayRoomIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool result)
        {
            return false;
        };
        public delegate bool GameRelayRequestReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRequestReadyDelegate GameRelayRequestReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayStartDelegate GameRelayStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRequestSelectOrderDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRequestSelectOrderDelegate GameRelayRequestSelectOrder = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayOrderEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayOrderEndDelegate GameRelayOrderEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayDistributedStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayDistributedStartDelegate GameRelayDistributedStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayFloorHasBonusDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayFloorHasBonusDelegate GameRelayFloorHasBonus = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayTurnStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayTurnStartDelegate GameRelayTurnStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelaySelectCardResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelaySelectCardResultDelegate GameRelaySelectCardResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayFlipDeckResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayFlipDeckResultDelegate GameRelayFlipDeckResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayTurnResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayTurnResultDelegate GameRelayTurnResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayUserInfoDelegate GameRelayUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayNotifyIndexDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayNotifyIndexDelegate GameRelayNotifyIndex = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayNotifyStatDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayNotifyStatDelegate GameRelayNotifyStat = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRequestKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRequestKookjinDelegate GameRelayRequestKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayNotifyKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayNotifyKookjinDelegate GameRelayNotifyKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRequestGoStopDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRequestGoStopDelegate GameRelayRequestGoStop = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayNotifyGoStopDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayNotifyGoStopDelegate GameRelayNotifyGoStop = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayMoveKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayMoveKookjinDelegate GameRelayMoveKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEventStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEventStartDelegate GameRelayEventStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayEventInfoDelegate GameRelayEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayOverDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayOverDelegate GameRelayOver = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRequestPushDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRequestPushDelegate GameRelayRequestPush = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayResponseRoomMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage);
        public GameRelayResponseRoomMoveDelegate GameRelayResponseRoomMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool resultMove, string errorMessage)
        {
            return false;
        };
        public delegate bool GameRelayPracticeEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayPracticeEndDelegate GameRelayPracticeEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayResponseRoomOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool permissionOut);
        public GameRelayResponseRoomOutDelegate GameRelayResponseRoomOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, bool permissionOut)
        {
            return false;
        };
        public delegate bool GameRelayKickUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayKickUserDelegate GameRelayKickUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayRoomInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayRoomInfoDelegate GameRelayRoomInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayUserOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayUserOutDelegate GameRelayUserOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameRelayObserveInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayObserveInfoDelegate GameRelayObserveInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
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
        public delegate bool RelayRequestLobbyEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public RelayRequestLobbyEventInfoDelegate RelayRequestLobbyEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
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
        public delegate bool GameRelayResponseRoomMissionInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data);
        public GameRelayResponseRoomMissionInfoDelegate GameRelayResponseRoomMissionInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.RemoteID userRemote, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool ServerMoveStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param);
        public ServerMoveStartDelegate ServerMoveStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string moveServerIP, ushort moveServerPort, ZNet.ArrByte param)
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
        public delegate bool ResponseLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string key);
        public ResponseLobbyKeyDelegate ResponseLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string key)
        {
            return false;
        };
        public delegate bool ResponseLoginDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage);
        public ResponseLoginDelegate ResponseLogin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result, string resultMessage)
        {
            return false;
        };
        public delegate bool NotifyLobbyListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<string> lobbyList);
        public NotifyLobbyListDelegate NotifyLobbyList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<string> lobbyList)
        {
            return false;
        };
        public delegate bool NotifyUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.LobbyUserInfo userInfo);
        public NotifyUserInfoDelegate NotifyUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, Rmi.Marshaler.LobbyUserInfo userInfo)
        {
            return false;
        };
        public delegate bool NotifyUserListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList);
        public NotifyUserListDelegate NotifyUserList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos, List<string> lobbyFriendList)
        {
            return false;
        };
        public delegate bool NotifyRoomListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos);
        public NotifyRoomListDelegate NotifyRoomList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, int channelID, List<Rmi.Marshaler.RoomInfo> roomInfos)
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
        public delegate bool GameRoomInDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result);
        public GameRoomInDelegate GameRoomIn = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result)
        {
            return false;
        };
        public delegate bool GameRequestReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameRequestReadyDelegate GameRequestReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameStartDelegate GameStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameRequestSelectOrderDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameRequestSelectOrderDelegate GameRequestSelectOrder = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameOrderEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameOrderEndDelegate GameOrderEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameDistributedStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameDistributedStartDelegate GameDistributedStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameFloorHasBonusDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameFloorHasBonusDelegate GameFloorHasBonus = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameTurnStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameTurnStartDelegate GameTurnStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameSelectCardResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameSelectCardResultDelegate GameSelectCardResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameFlipDeckResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameFlipDeckResultDelegate GameFlipDeckResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameTurnResultDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameTurnResultDelegate GameTurnResult = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameUserInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameUserInfoDelegate GameUserInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameNotifyIndexDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameNotifyIndexDelegate GameNotifyIndex = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameNotifyStatDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameNotifyStatDelegate GameNotifyStat = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameRequestKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameRequestKookjinDelegate GameRequestKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameNotifyKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameNotifyKookjinDelegate GameNotifyKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameRequestGoStopDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameRequestGoStopDelegate GameRequestGoStop = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameNotifyGoStopDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameNotifyGoStopDelegate GameNotifyGoStop = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameMoveKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameMoveKookjinDelegate GameMoveKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameEventStartDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameEventStartDelegate GameEventStart = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameEventInfoDelegate GameEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameOverDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameOverDelegate GameOver = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameRequestPushDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameRequestPushDelegate GameRequestPush = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameResponseRoomMoveDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool move, string errorMessage);
        public GameResponseRoomMoveDelegate GameResponseRoomMove = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool move, string errorMessage)
        {
            return false;
        };
        public delegate bool GamePracticeEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GamePracticeEndDelegate GamePracticeEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameResponseRoomOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result);
        public GameResponseRoomOutDelegate GameResponseRoomOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, bool result)
        {
            return false;
        };
        public delegate bool GameKickUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameKickUserDelegate GameKickUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameRoomInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameRoomInfoDelegate GameRoomInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameUserOutDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameUserOutDelegate GameUserOut = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameObserveInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameObserveInfoDelegate GameObserveInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameNotifyMessageDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameNotifyMessageDelegate GameNotifyMessage = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool ResponsePurchaseListDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge);
        public ResponsePurchaseListDelegate ResponsePurchaseList = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, List<string> Purchase_avatar, List<string> Purchase_card, List<string> Purchase_evt, List<string> Purchase_charge)
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
        public delegate bool ResponseGameOptionsDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public ResponseGameOptionsDelegate ResponseGameOptions = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool ResponseLobbyEventInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public ResponseLobbyEventInfoDelegate ResponseLobbyEventInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool ResponseLobbyEventParticipateDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public ResponseLobbyEventParticipateDelegate ResponseLobbyEventParticipate = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
        {
            return false;
        };
        public delegate bool GameResponseRoomMissionInfoDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data);
        public GameResponseRoomMissionInfoDelegate GameResponseRoomMissionInfo = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.ArrByte data)
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
        public delegate bool RequestLobbyKeyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key);
        public RequestLobbyKeyDelegate RequestLobbyKey = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, string id, string key)
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
        public delegate bool GameRoomInUserDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameRoomInUserDelegate GameRoomInUser = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameReadyDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameReadyDelegate GameReady = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameSelectOrderDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameSelectOrderDelegate GameSelectOrder = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameDistributedEndDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameDistributedEndDelegate GameDistributedEnd = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameActionPutCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameActionPutCardDelegate GameActionPutCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameActionFlipBombDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameActionFlipBombDelegate GameActionFlipBomb = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameActionChooseCardDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameActionChooseCardDelegate GameActionChooseCard = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameSelectKookjinDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameSelectKookjinDelegate GameSelectKookjin = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameSelectGoStopDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameSelectGoStopDelegate GameSelectGoStop = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GameSelectPushDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GameSelectPushDelegate GameSelectPush = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
        {
            return false;
        };
        public delegate bool GamePracticeDelegate(ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data);
        public GamePracticeDelegate GamePractice = delegate (ZNet.RemoteID remote, ZNet.CPackOption pkOption, ZNet.CMessage data)
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
            NetworkManager.Instance.ReceiveMsg(rm);

            {
                //switch (PkID)
                //{
                //    case Common.MasterAllShutdown:
                //        {
                //            string msg; Rmi.Marshaler.Read(__msg, out msg);

                //            bool bRet = MasterAllShutdown(remote, pkOption, msg);
                //            if (bRet == false)
                //                NeedImplement("MasterAllShutdown");
                //        }
                //        break;

                //    case Common.MasterNotifyP2PServerInfo:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = MasterNotifyP2PServerInfo(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("MasterNotifyP2PServerInfo");
                //        }
                //        break;

                //    case Common.P2PMemberCheck:
                //        {

                //            bool bRet = P2PMemberCheck(remote, pkOption);
                //            if (bRet == false)
                //                NeedImplement("P2PMemberCheck");
                //        }
                //        break;

                //    case Common.RoomLobbyMakeRoom:
                //        {
                //            Rmi.Marshaler.RoomInfo roomInfo; Rmi.Marshaler.Read(__msg, out roomInfo);
                //            Rmi.Marshaler.LobbyUserList userInfo; Rmi.Marshaler.Read(__msg, out userInfo);
                //            int userID; Rmi.Marshaler.Read(__msg, out userID);
                //            string IP; Rmi.Marshaler.Read(__msg, out IP);
                //            string Pass; Rmi.Marshaler.Read(__msg, out Pass);
                //            int shopId; Rmi.Marshaler.Read(__msg, out shopId);

                //            bool bRet = RoomLobbyMakeRoom(remote, pkOption, roomInfo, userInfo, userID, IP, Pass, shopId);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyMakeRoom");
                //        }
                //        break;

                //    case Common.RoomLobbyJoinRoom:
                //        {
                //            System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                //            Rmi.Marshaler.LobbyUserList userInfo; Rmi.Marshaler.Read(__msg, out userInfo);
                //            int userID; Rmi.Marshaler.Read(__msg, out userID);
                //            string IP; Rmi.Marshaler.Read(__msg, out IP);
                //            int shopId; Rmi.Marshaler.Read(__msg, out shopId);

                //            bool bRet = RoomLobbyJoinRoom(remote, pkOption, roomID, userInfo, userID, IP, shopId);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyJoinRoom");
                //        }
                //        break;

                //    case Common.RoomLobbyOutRoom:
                //        {
                //            System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                //            int userID; Rmi.Marshaler.Read(__msg, out userID);

                //            bool bRet = RoomLobbyOutRoom(remote, pkOption, roomID, userID);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyOutRoom");
                //        }
                //        break;

                //    case Common.RoomLobbyMessage:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string message; Rmi.Marshaler.Read(__msg, out message);

                //            bool bRet = RoomLobbyMessage(remote, pkOption, userRemote, message);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyMessage");
                //        }
                //        break;

                //    case Common.RoomLobbyEventStart:
                //        {
                //            System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                //            int type; Rmi.Marshaler.Read(__msg, out type);

                //            bool bRet = RoomLobbyEventStart(remote, pkOption, roomID, type);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyEventStart");
                //        }
                //        break;

                //    case Common.RoomLobbyEventEnd:
                //        {
                //            System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                //            int type; Rmi.Marshaler.Read(__msg, out type);
                //            string name; Rmi.Marshaler.Read(__msg, out name);
                //            long reward; Rmi.Marshaler.Read(__msg, out reward);

                //            bool bRet = RoomLobbyEventEnd(remote, pkOption, roomID, type, name, reward);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyEventEnd");
                //        }
                //        break;

                //    case Common.LobbyRoomJackpotInfo:
                //        {
                //            long jackpot; Rmi.Marshaler.Read(__msg, out jackpot);

                //            bool bRet = LobbyRoomJackpotInfo(remote, pkOption, jackpot);
                //            if (bRet == false)
                //                NeedImplement("LobbyRoomJackpotInfo");
                //        }
                //        break;

                //    case Common.LobbyRoomNotifyMessage:
                //        {
                //            int type; Rmi.Marshaler.Read(__msg, out type);
                //            string message; Rmi.Marshaler.Read(__msg, out message);
                //            int period; Rmi.Marshaler.Read(__msg, out period);

                //            bool bRet = LobbyRoomNotifyMessage(remote, pkOption, type, message, period);
                //            if (bRet == false)
                //                NeedImplement("LobbyRoomNotifyMessage");
                //        }
                //        break;

                //    case Common.LobbyRoomNotifyServermaintenance:
                //        {
                //            int type; Rmi.Marshaler.Read(__msg, out type);
                //            string message; Rmi.Marshaler.Read(__msg, out message);
                //            int release; Rmi.Marshaler.Read(__msg, out release);

                //            bool bRet = LobbyRoomNotifyServermaintenance(remote, pkOption, type, message, release);
                //            if (bRet == false)
                //                NeedImplement("LobbyRoomNotifyServermaintenance");
                //        }
                //        break;

                //    case Common.LobbyRoomReloadServerData:
                //        {
                //            int type; Rmi.Marshaler.Read(__msg, out type);

                //            bool bRet = LobbyRoomReloadServerData(remote, pkOption, type);
                //            if (bRet == false)
                //                NeedImplement("LobbyRoomReloadServerData");
                //        }
                //        break;

                //    case Common.LobbyRoomKickUser:
                //        {
                //            int userID; Rmi.Marshaler.Read(__msg, out userID);

                //            bool bRet = LobbyRoomKickUser(remote, pkOption, userID);
                //            if (bRet == false)
                //                NeedImplement("LobbyRoomKickUser");
                //        }
                //        break;

                //    case Common.LobbyLoginKickUser:
                //        {
                //            int userID; Rmi.Marshaler.Read(__msg, out userID);

                //            bool bRet = LobbyLoginKickUser(remote, pkOption, userID);
                //            if (bRet == false)
                //                NeedImplement("LobbyLoginKickUser");
                //        }
                //        break;

                //    case Common.RoomLobbyRequestMoveRoom:
                //        {
                //            System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int userID; Rmi.Marshaler.Read(__msg, out userID);
                //            long money; Rmi.Marshaler.Read(__msg, out money);
                //            bool ipFree; Rmi.Marshaler.Read(__msg, out ipFree);
                //            bool shopFree; Rmi.Marshaler.Read(__msg, out shopFree);
                //            int shopId; Rmi.Marshaler.Read(__msg, out shopId);

                //            bool bRet = RoomLobbyRequestMoveRoom(remote, pkOption, roomID, userRemote, userID, money, ipFree, shopFree, shopId);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyRequestMoveRoom");
                //        }
                //        break;

                //    case Common.LobbyRoomResponseMoveRoom:
                //        {
                //            bool makeRoom; Rmi.Marshaler.Read(__msg, out makeRoom);
                //            System.Guid roomID; Rmi.Marshaler.Read(__msg, out roomID);
                //            ZNet.NetAddress addr; Rmi.Marshaler.Read(__msg, out addr);
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                //            ZNet.RemoteID remoteS; Rmi.Marshaler.Read(__msg, out remoteS);
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string message; Rmi.Marshaler.Read(__msg, out message);

                //            bool bRet = LobbyRoomResponseMoveRoom(remote, pkOption, makeRoom, roomID, addr, chanID, remoteS, userRemote, message);
                //            if (bRet == false)
                //                NeedImplement("LobbyRoomResponseMoveRoom");
                //        }
                //        break;

                //    case Common.ServerRequestDataSync:
                //        {
                //            bool isLobby; Rmi.Marshaler.Read(__msg, out isLobby);

                //            bool bRet = ServerRequestDataSync(remote, pkOption, isLobby);
                //            if (bRet == false)
                //                NeedImplement("ServerRequestDataSync");
                //        }
                //        break;

                //    case Common.RoomLobbyResponseDataSync:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RoomLobbyResponseDataSync(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("RoomLobbyResponseDataSync");
                //        }
                //        break;

                //    case Common.RelayLobbyResponseDataSync:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayLobbyResponseDataSync(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("RelayLobbyResponseDataSync");
                //        }
                //        break;

                //    case Common.RelayClientJoin:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            ZNet.NetAddress addr; Rmi.Marshaler.Read(__msg, out addr);
                //            ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);

                //            bool bRet = RelayClientJoin(remote, pkOption, userRemote, addr, param);
                //            if (bRet == false)
                //                NeedImplement("RelayClientJoin");
                //        }
                //        break;

                //    case Common.RelayClientLeave:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool bMoveServer; Rmi.Marshaler.Read(__msg, out bMoveServer);

                //            bool bRet = RelayClientLeave(remote, pkOption, userRemote, bMoveServer);
                //            if (bRet == false)
                //                NeedImplement("RelayClientLeave");
                //        }
                //        break;

                //    case Common.RelayCloseRemoteClient:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                //            bool bRet = RelayCloseRemoteClient(remote, pkOption, userRemote);
                //            if (bRet == false)
                //                NeedImplement("RelayCloseRemoteClient");
                //        }
                //        break;

                //    case Common.RelayServerMoveFailure:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                //            bool bRet = RelayServerMoveFailure(remote, pkOption, userRemote);
                //            if (bRet == false)
                //                NeedImplement("RelayServerMoveFailure");
                //        }
                //        break;

                //    case Common.RelayRequestLobbyKey:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string id; Rmi.Marshaler.Read(__msg, out id);
                //            string key; Rmi.Marshaler.Read(__msg, out key);

                //            bool bRet = RelayRequestLobbyKey(remote, pkOption, userRemote, id, key);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestLobbyKey");
                //        }
                //        break;

                //    case Common.RelayRequestJoinInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                //            bool bRet = RelayRequestJoinInfo(remote, pkOption, userRemote);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestJoinInfo");
                //        }
                //        break;

                //    case Common.RelayRequestChannelMove:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                //            bool bRet = RelayRequestChannelMove(remote, pkOption, userRemote, chanID);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestChannelMove");
                //        }
                //        break;

                //    case Common.RelayRequestRoomMake:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int relayID; Rmi.Marshaler.Read(__msg, out relayID);
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                //            int betType; Rmi.Marshaler.Read(__msg, out betType);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RelayRequestRoomMake(remote, pkOption, userRemote, relayID, chanID, betType, pass);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestRoomMake");
                //        }
                //        break;

                //    case Common.RelayRequestRoomJoin:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int relayID; Rmi.Marshaler.Read(__msg, out relayID);
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                //            int betType; Rmi.Marshaler.Read(__msg, out betType);

                //            bool bRet = RelayRequestRoomJoin(remote, pkOption, userRemote, relayID, chanID, betType);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestRoomJoin");
                //        }
                //        break;

                //    case Common.RelayRequestRoomJoinSelect:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int relayID; Rmi.Marshaler.Read(__msg, out relayID);
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                //            int roomNumber; Rmi.Marshaler.Read(__msg, out roomNumber);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RelayRequestRoomJoinSelect(remote, pkOption, userRemote, relayID, chanID, roomNumber, pass);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestRoomJoinSelect");
                //        }
                //        break;

                //    case Common.RelayRequestBank:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int option; Rmi.Marshaler.Read(__msg, out option);
                //            long money; Rmi.Marshaler.Read(__msg, out money);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RelayRequestBank(remote, pkOption, userRemote, option, money, pass);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestBank");
                //        }
                //        break;

                //    case Common.RelayRequestPurchaseList:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                //            bool bRet = RelayRequestPurchaseList(remote, pkOption, userRemote);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestPurchaseList");
                //        }
                //        break;

                //    case Common.RelayRequestPurchaseAvailability:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);

                //            bool bRet = RelayRequestPurchaseAvailability(remote, pkOption, userRemote, pid);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestPurchaseAvailability");
                //        }
                //        break;

                //    case Common.RelayRequestPurchaseReceiptCheck:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string result; Rmi.Marshaler.Read(__msg, out result);

                //            bool bRet = RelayRequestPurchaseReceiptCheck(remote, pkOption, userRemote, result);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestPurchaseReceiptCheck");
                //        }
                //        break;

                //    case Common.RelayRequestPurchaseResult:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                //            bool bRet = RelayRequestPurchaseResult(remote, pkOption, userRemote, token);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestPurchaseResult");
                //        }
                //        break;

                //    case Common.RelayRequestPurchaseCash:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);

                //            bool bRet = RelayRequestPurchaseCash(remote, pkOption, userRemote, pid);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestPurchaseCash");
                //        }
                //        break;

                //    case Common.RelayRequestMyroomList:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                //            bool bRet = RelayRequestMyroomList(remote, pkOption, userRemote);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestMyroomList");
                //        }
                //        break;

                //    case Common.RelayRequestMyroomAction:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);

                //            bool bRet = RelayRequestMyroomAction(remote, pkOption, userRemote, pid);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestMyroomAction");
                //        }
                //        break;

                //    case Common.LobbyRelayResponsePurchaseList:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            List<string> Purchase_avatar; Rmi.Marshaler.Read(__msg, out Purchase_avatar);
                //            List<string> Purchase_card; Rmi.Marshaler.Read(__msg, out Purchase_card);
                //            List<string> Purchase_evt; Rmi.Marshaler.Read(__msg, out Purchase_evt);
                //            List<string> Purchase_charge; Rmi.Marshaler.Read(__msg, out Purchase_charge);

                //            bool bRet = LobbyRelayResponsePurchaseList(remote, pkOption, userRemote, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponsePurchaseList");
                //        }
                //        break;

                //    case Common.LobbyRelayResponsePurchaseAvailability:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool available; Rmi.Marshaler.Read(__msg, out available);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = LobbyRelayResponsePurchaseAvailability(remote, pkOption, userRemote, available, reason);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponsePurchaseAvailability");
                //        }
                //        break;

                //    case Common.LobbyRelayResponsePurchaseReceiptCheck:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                //            bool bRet = LobbyRelayResponsePurchaseReceiptCheck(remote, pkOption, userRemote, result, token);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponsePurchaseReceiptCheck");
                //        }
                //        break;

                //    case Common.LobbyRelayResponsePurchaseResult:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = LobbyRelayResponsePurchaseResult(remote, pkOption, userRemote, result, reason);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponsePurchaseResult");
                //        }
                //        break;

                //    case Common.LobbyRelayResponsePurchaseCash:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = LobbyRelayResponsePurchaseCash(remote, pkOption, userRemote, result, reason);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponsePurchaseCash");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseMyroomList:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string json; Rmi.Marshaler.Read(__msg, out json);

                //            bool bRet = LobbyRelayResponseMyroomList(remote, pkOption, userRemote, json);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseMyroomList");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseMyroomAction:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = LobbyRelayResponseMyroomAction(remote, pkOption, userRemote, pid, result, reason);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseMyroomAction");
                //        }
                //        break;

                //    case Common.LobbyRelayServerMoveStart:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string moveServerIP; Rmi.Marshaler.Read(__msg, out moveServerIP);
                //            ushort moveServerPort; Rmi.Marshaler.Read(__msg, out moveServerPort);
                //            ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);
                //            Guid guid; Rmi.Marshaler.Read(__msg, out guid);

                //            bool bRet = LobbyRelayServerMoveStart(remote, pkOption, userRemote, moveServerIP, moveServerPort, param, guid);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayServerMoveStart");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseLobbyKey:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string key; Rmi.Marshaler.Read(__msg, out key);

                //            bool bRet = LobbyRelayResponseLobbyKey(remote, pkOption, userRemote, key);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseLobbyKey");
                //        }
                //        break;

                //    case Common.LobbyRelayNotifyUserInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            Rmi.Marshaler.LobbyUserInfo userInfo; Rmi.Marshaler.Read(__msg, out userInfo);

                //            bool bRet = LobbyRelayNotifyUserInfo(remote, pkOption, userRemote, userInfo);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayNotifyUserInfo");
                //        }
                //        break;

                //    case Common.LobbyRelayNotifyUserList:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; Rmi.Marshaler.Read(__msg, out lobbyUserInfos);
                //            List<string> lobbyFriendList; Rmi.Marshaler.Read(__msg, out lobbyFriendList);

                //            bool bRet = LobbyRelayNotifyUserList(remote, pkOption, userRemote, lobbyUserInfos, lobbyFriendList);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayNotifyUserList");
                //        }
                //        break;

                //    case Common.LobbyRelayNotifyRoomList:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int channelID; Rmi.Marshaler.Read(__msg, out channelID);
                //            List<Rmi.Marshaler.RoomInfo> roomInfos; Rmi.Marshaler.Read(__msg, out roomInfos);

                //            bool bRet = LobbyRelayNotifyRoomList(remote, pkOption, userRemote, channelID, roomInfos);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayNotifyRoomList");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseChannelMove:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                //            bool bRet = LobbyRelayResponseChannelMove(remote, pkOption, userRemote, chanID);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseChannelMove");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseLobbyMessage:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            string message; Rmi.Marshaler.Read(__msg, out message);

                //            bool bRet = LobbyRelayResponseLobbyMessage(remote, pkOption, userRemote, message);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseLobbyMessage");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseBank:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            int resultType; Rmi.Marshaler.Read(__msg, out resultType);

                //            bool bRet = LobbyRelayResponseBank(remote, pkOption, userRemote, result, resultType);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseBank");
                //        }
                //        break;

                //    case Common.LobbyRelayNotifyJackpotInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            long jackpot; Rmi.Marshaler.Read(__msg, out jackpot);

                //            bool bRet = LobbyRelayNotifyJackpotInfo(remote, pkOption, userRemote, jackpot);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayNotifyJackpotInfo");
                //        }
                //        break;

                //    case Common.LobbyRelayNotifyLobbyMessage:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            int type; Rmi.Marshaler.Read(__msg, out type);
                //            string message; Rmi.Marshaler.Read(__msg, out message);
                //            int period; Rmi.Marshaler.Read(__msg, out period);

                //            bool bRet = LobbyRelayNotifyLobbyMessage(remote, pkOption, userRemote, type, message, period);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayNotifyLobbyMessage");
                //        }
                //        break;

                //    case Common.RoomRelayServerMoveStart:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            ZNet.NetAddress addr; Rmi.Marshaler.Read(__msg, out addr);
                //            ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);
                //            Guid idx; Rmi.Marshaler.Read(__msg, out idx);

                //            bool bRet = RoomRelayServerMoveStart(remote, pkOption, userRemote, addr, param, idx);
                //            if (bRet == false)
                //                NeedImplement("RoomRelayServerMoveStart");
                //        }
                //        break;

                //    case Common.RelayRequestOutRoom:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                //            bool bRet = RelayRequestOutRoom(remote, pkOption, userRemote);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestOutRoom");
                //        }
                //        break;

                //    case Common.RelayResponseOutRoom:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool resultOut; Rmi.Marshaler.Read(__msg, out resultOut);

                //            bool bRet = RelayResponseOutRoom(remote, pkOption, userRemote, resultOut);
                //            if (bRet == false)
                //                NeedImplement("RelayResponseOutRoom");
                //        }
                //        break;

                //    case Common.RelayRequestMoveRoom:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);

                //            bool bRet = RelayRequestMoveRoom(remote, pkOption, userRemote);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestMoveRoom");
                //        }
                //        break;

                //    case Common.RelayResponseMoveRoom:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool resultMove; Rmi.Marshaler.Read(__msg, out resultMove);
                //            string errorMessage; Rmi.Marshaler.Read(__msg, out errorMessage);

                //            bool bRet = RelayResponseMoveRoom(remote, pkOption, userRemote, resultMove, errorMessage);
                //            if (bRet == false)
                //                NeedImplement("RelayResponseMoveRoom");
                //        }
                //        break;

                //    case Common.RelayGameRoomIn:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameRoomIn(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameRoomIn");
                //        }
                //        break;

                //    case Common.RelayGameReady:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameReady(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameReady");
                //        }
                //        break;

                //    case Common.RelayGameSelectOrder:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameSelectOrder(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameSelectOrder");
                //        }
                //        break;

                //    case Common.RelayGameDistributedEnd:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameDistributedEnd(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameDistributedEnd");
                //        }
                //        break;

                //    case Common.RelayGameActionPutCard:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameActionPutCard(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameActionPutCard");
                //        }
                //        break;

                //    case Common.RelayGameActionFlipBomb:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameActionFlipBomb(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameActionFlipBomb");
                //        }
                //        break;

                //    case Common.RelayGameActionChooseCard:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameActionChooseCard(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameActionChooseCard");
                //        }
                //        break;

                //    case Common.RelayGameSelectKookjin:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameSelectKookjin(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameSelectKookjin");
                //        }
                //        break;

                //    case Common.RelayGameSelectGoStop:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameSelectGoStop(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameSelectGoStop");
                //        }
                //        break;

                //    case Common.RelayGameSelectPush:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGameSelectPush(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGameSelectPush");
                //        }
                //        break;

                //    case Common.RelayGamePractice:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayGamePractice(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayGamePractice");
                //        }
                //        break;

                //    case Common.GameRelayRoomIn:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool result; Rmi.Marshaler.Read(__msg, out result);

                //            bool bRet = GameRelayRoomIn(remote, pkOption, userRemote, result);
                //            if (bRet == false)
                //                NeedImplement("GameRelayRoomIn");
                //        }
                //        break;

                //    case Common.GameRelayRequestReady:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayRequestReady(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayRequestReady");
                //        }
                //        break;

                //    case Common.GameRelayStart:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayStart(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayStart");
                //        }
                //        break;

                //    case Common.GameRelayRequestSelectOrder:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayRequestSelectOrder(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayRequestSelectOrder");
                //        }
                //        break;

                //    case Common.GameRelayOrderEnd:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayOrderEnd(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayOrderEnd");
                //        }
                //        break;

                //    case Common.GameRelayDistributedStart:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayDistributedStart(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayDistributedStart");
                //        }
                //        break;

                //    case Common.GameRelayFloorHasBonus:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayFloorHasBonus(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayFloorHasBonus");
                //        }
                //        break;

                //    case Common.GameRelayTurnStart:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayTurnStart(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayTurnStart");
                //        }
                //        break;

                //    case Common.GameRelaySelectCardResult:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelaySelectCardResult(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelaySelectCardResult");
                //        }
                //        break;

                //    case Common.GameRelayFlipDeckResult:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayFlipDeckResult(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayFlipDeckResult");
                //        }
                //        break;

                //    case Common.GameRelayTurnResult:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayTurnResult(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayTurnResult");
                //        }
                //        break;

                //    case Common.GameRelayUserInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayUserInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayUserInfo");
                //        }
                //        break;

                //    case Common.GameRelayNotifyIndex:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayNotifyIndex(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayNotifyIndex");
                //        }
                //        break;

                //    case Common.GameRelayNotifyStat:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayNotifyStat(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayNotifyStat");
                //        }
                //        break;

                //    case Common.GameRelayRequestKookjin:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayRequestKookjin(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayRequestKookjin");
                //        }
                //        break;

                //    case Common.GameRelayNotifyKookjin:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayNotifyKookjin(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayNotifyKookjin");
                //        }
                //        break;

                //    case Common.GameRelayRequestGoStop:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayRequestGoStop(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayRequestGoStop");
                //        }
                //        break;

                //    case Common.GameRelayNotifyGoStop:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayNotifyGoStop(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayNotifyGoStop");
                //        }
                //        break;

                //    case Common.GameRelayMoveKookjin:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayMoveKookjin(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayMoveKookjin");
                //        }
                //        break;

                //    case Common.GameRelayEventStart:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayEventStart(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayEventStart");
                //        }
                //        break;

                //    case Common.GameRelayEventInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayEventInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayEventInfo");
                //        }
                //        break;

                //    case Common.GameRelayOver:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayOver(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayOver");
                //        }
                //        break;

                //    case Common.GameRelayRequestPush:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayRequestPush(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayRequestPush");
                //        }
                //        break;

                //    case Common.GameRelayResponseRoomMove:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool resultMove; Rmi.Marshaler.Read(__msg, out resultMove);
                //            string errorMessage; Rmi.Marshaler.Read(__msg, out errorMessage);

                //            bool bRet = GameRelayResponseRoomMove(remote, pkOption, userRemote, resultMove, errorMessage);
                //            if (bRet == false)
                //                NeedImplement("GameRelayResponseRoomMove");
                //        }
                //        break;

                //    case Common.GameRelayPracticeEnd:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayPracticeEnd(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayPracticeEnd");
                //        }
                //        break;

                //    case Common.GameRelayResponseRoomOut:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            bool permissionOut; Rmi.Marshaler.Read(__msg, out permissionOut);

                //            bool bRet = GameRelayResponseRoomOut(remote, pkOption, userRemote, permissionOut);
                //            if (bRet == false)
                //                NeedImplement("GameRelayResponseRoomOut");
                //        }
                //        break;

                //    case Common.GameRelayKickUser:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayKickUser(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayKickUser");
                //        }
                //        break;

                //    case Common.GameRelayRoomInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayRoomInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayRoomInfo");
                //        }
                //        break;

                //    case Common.GameRelayUserOut:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayUserOut(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayUserOut");
                //        }
                //        break;

                //    case Common.GameRelayObserveInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayObserveInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayObserveInfo");
                //        }
                //        break;

                //    case Common.GameRelayNotifyMessage:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayNotifyMessage(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayNotifyMessage");
                //        }
                //        break;

                //    case Common.GameRelayNotifyJackpotInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayNotifyJackpotInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayNotifyJackpotInfo");
                //        }
                //        break;

                //    case Common.RelayRequestLobbyEventInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayRequestLobbyEventInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestLobbyEventInfo");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseLobbyEventInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = LobbyRelayResponseLobbyEventInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseLobbyEventInfo");
                //        }
                //        break;

                //    case Common.RelayRequestLobbyEventParticipate:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RelayRequestLobbyEventParticipate(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("RelayRequestLobbyEventParticipate");
                //        }
                //        break;

                //    case Common.LobbyRelayResponseLobbyEventParticipate:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = LobbyRelayResponseLobbyEventParticipate(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("LobbyRelayResponseLobbyEventParticipate");
                //        }
                //        break;

                //    case Common.GameRelayResponseRoomMissionInfo:
                //        {
                //            ZNet.RemoteID userRemote; Rmi.Marshaler.Read(__msg, out userRemote);
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRelayResponseRoomMissionInfo(remote, pkOption, userRemote, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRelayResponseRoomMissionInfo");
                //        }
                //        break;

                //    case Common.ServerMoveStart:
                //        {
                //            string moveServerIP; Rmi.Marshaler.Read(__msg, out moveServerIP);
                //            ushort moveServerPort; Rmi.Marshaler.Read(__msg, out moveServerPort);
                //            ZNet.ArrByte param; Rmi.Marshaler.Read(__msg, out param);

                //            bool bRet = ServerMoveStart(remote, pkOption, moveServerIP, moveServerPort, param);
                //            if (bRet == false)
                //                NeedImplement("ServerMoveStart");
                //        }
                //        break;

                //    case Common.ServerMoveEnd:
                //        {
                //            bool Moved; Rmi.Marshaler.Read(__msg, out Moved);

                //            bool bRet = ServerMoveEnd(remote, pkOption, Moved);
                //            if (bRet == false)
                //                NeedImplement("ServerMoveEnd");
                //        }
                //        break;

                //    case Common.ResponseLauncherLogin:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string nickname; Rmi.Marshaler.Read(__msg, out nickname);
                //            string key; Rmi.Marshaler.Read(__msg, out key);
                //            byte resultType; Rmi.Marshaler.Read(__msg, out resultType);

                //            bool bRet = ResponseLauncherLogin(remote, pkOption, result, nickname, key, resultType);
                //            if (bRet == false)
                //                NeedImplement("ResponseLauncherLogin");
                //        }
                //        break;

                //    case Common.ResponseLauncherLogout:
                //        {

                //            bool bRet = ResponseLauncherLogout(remote, pkOption);
                //            if (bRet == false)
                //                NeedImplement("ResponseLauncherLogout");
                //        }
                //        break;

                //    case Common.ResponseLoginKey:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string resultMessage; Rmi.Marshaler.Read(__msg, out resultMessage);

                //            bool bRet = ResponseLoginKey(remote, pkOption, result, resultMessage);
                //            if (bRet == false)
                //                NeedImplement("ResponseLoginKey");
                //        }
                //        break;

                //    case Common.ResponseLobbyKey:
                //        {
                //            string key; Rmi.Marshaler.Read(__msg, out key);

                //            bool bRet = ResponseLobbyKey(remote, pkOption, key);
                //            if (bRet == false)
                //                NeedImplement("ResponseLobbyKey");
                //        }
                //        break;

                //    case Common.ResponseLogin:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string resultMessage; Rmi.Marshaler.Read(__msg, out resultMessage);

                //            bool bRet = ResponseLogin(remote, pkOption, result, resultMessage);
                //            if (bRet == false)
                //                NeedImplement("ResponseLogin");
                //        }
                //        break;

                //    case Common.NotifyLobbyList:
                //        {
                //            List<string> lobbyList; Rmi.Marshaler.Read(__msg, out lobbyList);

                //            bool bRet = NotifyLobbyList(remote, pkOption, lobbyList);
                //            if (bRet == false)
                //                NeedImplement("NotifyLobbyList");
                //        }
                //        break;

                //    case Common.NotifyUserInfo:
                //        {
                //            Rmi.Marshaler.LobbyUserInfo userInfo; Rmi.Marshaler.Read(__msg, out userInfo);

                //            bool bRet = NotifyUserInfo(remote, pkOption, userInfo);
                //            if (bRet == false)
                //                NeedImplement("NotifyUserInfo");
                //        }
                //        break;

                //    case Common.NotifyUserList:
                //        {
                //            List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; Rmi.Marshaler.Read(__msg, out lobbyUserInfos);
                //            List<string> lobbyFriendList; Rmi.Marshaler.Read(__msg, out lobbyFriendList);

                //            bool bRet = NotifyUserList(remote, pkOption, lobbyUserInfos, lobbyFriendList);
                //            if (bRet == false)
                //                NeedImplement("NotifyUserList");
                //        }
                //        break;

                //    case Common.NotifyRoomList:
                //        {
                //            int channelID; Rmi.Marshaler.Read(__msg, out channelID);
                //            List<Rmi.Marshaler.RoomInfo> roomInfos; Rmi.Marshaler.Read(__msg, out roomInfos);

                //            bool bRet = NotifyRoomList(remote, pkOption, channelID, roomInfos);
                //            if (bRet == false)
                //                NeedImplement("NotifyRoomList");
                //        }
                //        break;

                //    case Common.ResponseChannelMove:
                //        {
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                //            bool bRet = ResponseChannelMove(remote, pkOption, chanID);
                //            if (bRet == false)
                //                NeedImplement("ResponseChannelMove");
                //        }
                //        break;

                //    case Common.ResponseLobbyMessage:
                //        {
                //            string message; Rmi.Marshaler.Read(__msg, out message);

                //            bool bRet = ResponseLobbyMessage(remote, pkOption, message);
                //            if (bRet == false)
                //                NeedImplement("ResponseLobbyMessage");
                //        }
                //        break;

                //    case Common.ResponseBank:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            int resultType; Rmi.Marshaler.Read(__msg, out resultType);

                //            bool bRet = ResponseBank(remote, pkOption, result, resultType);
                //            if (bRet == false)
                //                NeedImplement("ResponseBank");
                //        }
                //        break;

                //    case Common.NotifyJackpotInfo:
                //        {
                //            long jackpot; Rmi.Marshaler.Read(__msg, out jackpot);

                //            bool bRet = NotifyJackpotInfo(remote, pkOption, jackpot);
                //            if (bRet == false)
                //                NeedImplement("NotifyJackpotInfo");
                //        }
                //        break;

                //    case Common.NotifyLobbyMessage:
                //        {
                //            int type; Rmi.Marshaler.Read(__msg, out type);
                //            string message; Rmi.Marshaler.Read(__msg, out message);
                //            int period; Rmi.Marshaler.Read(__msg, out period);

                //            bool bRet = NotifyLobbyMessage(remote, pkOption, type, message, period);
                //            if (bRet == false)
                //                NeedImplement("NotifyLobbyMessage");
                //        }
                //        break;

                //    case Common.GameRoomIn:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);

                //            bool bRet = GameRoomIn(remote, pkOption, result);
                //            if (bRet == false)
                //                NeedImplement("GameRoomIn");
                //        }
                //        break;

                //    case Common.GameRequestReady:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRequestReady(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameRequestReady");
                //        }
                //        break;

                //    case Common.GameStart:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameStart(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameStart");
                //        }
                //        break;

                //    case Common.GameRequestSelectOrder:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRequestSelectOrder(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameRequestSelectOrder");
                //        }
                //        break;

                //    case Common.GameOrderEnd:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameOrderEnd(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameOrderEnd");
                //        }
                //        break;

                //    case Common.GameDistributedStart:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameDistributedStart(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameDistributedStart");
                //        }
                //        break;

                //    case Common.GameFloorHasBonus:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameFloorHasBonus(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameFloorHasBonus");
                //        }
                //        break;

                //    case Common.GameTurnStart:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameTurnStart(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameTurnStart");
                //        }
                //        break;

                //    case Common.GameSelectCardResult:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameSelectCardResult(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameSelectCardResult");
                //        }
                //        break;

                //    case Common.GameFlipDeckResult:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameFlipDeckResult(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameFlipDeckResult");
                //        }
                //        break;

                //    case Common.GameTurnResult:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameTurnResult(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameTurnResult");
                //        }
                //        break;

                //    case Common.GameUserInfo:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameUserInfo(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameUserInfo");
                //        }
                //        break;

                //    case Common.GameNotifyIndex:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameNotifyIndex(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameNotifyIndex");
                //        }
                //        break;

                //    case Common.GameNotifyStat:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameNotifyStat(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameNotifyStat");
                //        }
                //        break;

                //    case Common.GameRequestKookjin:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRequestKookjin(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameRequestKookjin");
                //        }
                //        break;

                //    case Common.GameNotifyKookjin:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameNotifyKookjin(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameNotifyKookjin");
                //        }
                //        break;

                //    case Common.GameRequestGoStop:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRequestGoStop(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameRequestGoStop");
                //        }
                //        break;

                //    case Common.GameNotifyGoStop:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameNotifyGoStop(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameNotifyGoStop");
                //        }
                //        break;

                //    case Common.GameMoveKookjin:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameMoveKookjin(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameMoveKookjin");
                //        }
                //        break;

                //    case Common.GameEventStart:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameEventStart(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameEventStart");
                //        }
                //        break;

                //    case Common.GameEventInfo:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameEventInfo(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameEventInfo");
                //        }
                //        break;

                //    case Common.GameOver:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameOver(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameOver");
                //        }
                //        break;

                //    case Common.GameRequestPush:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRequestPush(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameRequestPush");
                //        }
                //        break;

                //    case Common.GameResponseRoomMove:
                //        {
                //            bool move; Rmi.Marshaler.Read(__msg, out move);
                //            string errorMessage; Rmi.Marshaler.Read(__msg, out errorMessage);

                //            bool bRet = GameResponseRoomMove(remote, pkOption, move, errorMessage);
                //            if (bRet == false)
                //                NeedImplement("GameResponseRoomMove");
                //        }
                //        break;

                //    case Common.GamePracticeEnd:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GamePracticeEnd(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GamePracticeEnd");
                //        }
                //        break;

                //    case Common.GameResponseRoomOut:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);

                //            bool bRet = GameResponseRoomOut(remote, pkOption, result);
                //            if (bRet == false)
                //                NeedImplement("GameResponseRoomOut");
                //        }
                //        break;

                //    case Common.GameKickUser:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameKickUser(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameKickUser");
                //        }
                //        break;

                //    case Common.GameRoomInfo:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRoomInfo(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameRoomInfo");
                //        }
                //        break;

                //    case Common.GameUserOut:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameUserOut(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameUserOut");
                //        }
                //        break;

                //    case Common.GameObserveInfo:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameObserveInfo(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameObserveInfo");
                //        }
                //        break;

                //    case Common.GameNotifyMessage:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameNotifyMessage(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameNotifyMessage");
                //        }
                //        break;

                //    case Common.ResponsePurchaseList:
                //        {
                //            List<string> Purchase_avatar; Rmi.Marshaler.Read(__msg, out Purchase_avatar);
                //            List<string> Purchase_card; Rmi.Marshaler.Read(__msg, out Purchase_card);
                //            List<string> Purchase_evt; Rmi.Marshaler.Read(__msg, out Purchase_evt);
                //            List<string> Purchase_charge; Rmi.Marshaler.Read(__msg, out Purchase_charge);

                //            bool bRet = ResponsePurchaseList(remote, pkOption, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
                //            if (bRet == false)
                //                NeedImplement("ResponsePurchaseList");
                //        }
                //        break;

                //    case Common.ResponsePurchaseAvailability:
                //        {
                //            bool available; Rmi.Marshaler.Read(__msg, out available);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = ResponsePurchaseAvailability(remote, pkOption, available, reason);
                //            if (bRet == false)
                //                NeedImplement("ResponsePurchaseAvailability");
                //        }
                //        break;

                //    case Common.ResponsePurchaseReceiptCheck:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                //            bool bRet = ResponsePurchaseReceiptCheck(remote, pkOption, result, token);
                //            if (bRet == false)
                //                NeedImplement("ResponsePurchaseReceiptCheck");
                //        }
                //        break;

                //    case Common.ResponsePurchaseResult:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = ResponsePurchaseResult(remote, pkOption, result, reason);
                //            if (bRet == false)
                //                NeedImplement("ResponsePurchaseResult");
                //        }
                //        break;

                //    case Common.ResponsePurchaseCash:
                //        {
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = ResponsePurchaseCash(remote, pkOption, result, reason);
                //            if (bRet == false)
                //                NeedImplement("ResponsePurchaseCash");
                //        }
                //        break;

                //    case Common.ResponseMyroomList:
                //        {
                //            string json; Rmi.Marshaler.Read(__msg, out json);

                //            bool bRet = ResponseMyroomList(remote, pkOption, json);
                //            if (bRet == false)
                //                NeedImplement("ResponseMyroomList");
                //        }
                //        break;

                //    case Common.ResponseMyroomAction:
                //        {
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);
                //            bool result; Rmi.Marshaler.Read(__msg, out result);
                //            string reason; Rmi.Marshaler.Read(__msg, out reason);

                //            bool bRet = ResponseMyroomAction(remote, pkOption, pid, result, reason);
                //            if (bRet == false)
                //                NeedImplement("ResponseMyroomAction");
                //        }
                //        break;

                //    case Common.ResponseGameOptions:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = ResponseGameOptions(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("ResponseGameOptions");
                //        }
                //        break;

                //    case Common.ResponseLobbyEventInfo:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = ResponseLobbyEventInfo(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("ResponseLobbyEventInfo");
                //        }
                //        break;

                //    case Common.ResponseLobbyEventParticipate:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = ResponseLobbyEventParticipate(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("ResponseLobbyEventParticipate");
                //        }
                //        break;

                //    case Common.GameResponseRoomMissionInfo:
                //        {
                //            ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameResponseRoomMissionInfo(remote, pkOption, data);
                //            if (bRet == false)
                //                NeedImplement("GameResponseRoomMissionInfo");
                //        }
                //        break;

                //    case Common.ServerMoveFailure:
                //        {

                //            bool bRet = ServerMoveFailure(remote, pkOption);
                //            if (bRet == false)
                //                NeedImplement("ServerMoveFailure");
                //        }
                //        break;

                //    case Common.RequestLauncherLogin:
                //        {
                //            string id; Rmi.Marshaler.Read(__msg, out id);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RequestLauncherLogin(remote, pkOption, id, pass);
                //            if (bRet == false)
                //                NeedImplement("RequestLauncherLogin");
                //        }
                //        break;

                //    case Common.RequestLauncherLogout:
                //        {
                //            string id; Rmi.Marshaler.Read(__msg, out id);
                //            string key; Rmi.Marshaler.Read(__msg, out key);

                //            bool bRet = RequestLauncherLogout(remote, pkOption, id, key);
                //            if (bRet == false)
                //                NeedImplement("RequestLauncherLogout");
                //        }
                //        break;

                //    case Common.RequestLoginKey:
                //        {
                //            string id; Rmi.Marshaler.Read(__msg, out id);
                //            string key; Rmi.Marshaler.Read(__msg, out key);

                //            bool bRet = RequestLoginKey(remote, pkOption, id, key);
                //            if (bRet == false)
                //                NeedImplement("RequestLoginKey");
                //        }
                //        break;

                //    case Common.RequestLobbyKey:
                //        {
                //            string id; Rmi.Marshaler.Read(__msg, out id);
                //            string key; Rmi.Marshaler.Read(__msg, out key);

                //            bool bRet = RequestLobbyKey(remote, pkOption, id, key);
                //            if (bRet == false)
                //                NeedImplement("RequestLobbyKey");
                //        }
                //        break;

                //    case Common.RequestLogin:
                //        {
                //            string name; Rmi.Marshaler.Read(__msg, out name);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RequestLogin(remote, pkOption, name, pass);
                //            if (bRet == false)
                //                NeedImplement("RequestLogin");
                //        }
                //        break;

                //    case Common.RequestLobbyList:
                //        {

                //            bool bRet = RequestLobbyList(remote, pkOption);
                //            if (bRet == false)
                //                NeedImplement("RequestLobbyList");
                //        }
                //        break;

                //    case Common.RequestGoLobby:
                //        {
                //            string lobbyName; Rmi.Marshaler.Read(__msg, out lobbyName);

                //            bool bRet = RequestGoLobby(remote, pkOption, lobbyName);
                //            if (bRet == false)
                //                NeedImplement("RequestGoLobby");
                //        }
                //        break;

                //    case Common.RequestJoinInfo:
                //        {

                //            bool bRet = RequestJoinInfo(remote, pkOption);
                //            if (bRet == false)
                //                NeedImplement("RequestJoinInfo");
                //        }
                //        break;

                //    case Common.RequestChannelMove:
                //        {
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);

                //            bool bRet = RequestChannelMove(remote, pkOption, chanID);
                //            if (bRet == false)
                //                NeedImplement("RequestChannelMove");
                //        }
                //        break;

                //    case Common.RequestRoomMake:
                //        {
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                //            int betType; Rmi.Marshaler.Read(__msg, out betType);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RequestRoomMake(remote, pkOption, chanID, betType, pass);
                //            if (bRet == false)
                //                NeedImplement("RequestRoomMake");
                //        }
                //        break;

                //    case Common.RequestRoomJoin:
                //        {
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                //            int betType; Rmi.Marshaler.Read(__msg, out betType);

                //            bool bRet = RequestRoomJoin(remote, pkOption, chanID, betType);
                //            if (bRet == false)
                //                NeedImplement("RequestRoomJoin");
                //        }
                //        break;

                //    case Common.RequestRoomJoinSelect:
                //        {
                //            int chanID; Rmi.Marshaler.Read(__msg, out chanID);
                //            int roomNumber; Rmi.Marshaler.Read(__msg, out roomNumber);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RequestRoomJoinSelect(remote, pkOption, chanID, roomNumber, pass);
                //            if (bRet == false)
                //                NeedImplement("RequestRoomJoinSelect");
                //        }
                //        break;

                //    case Common.RequestBank:
                //        {
                //            int option; Rmi.Marshaler.Read(__msg, out option);
                //            long money; Rmi.Marshaler.Read(__msg, out money);
                //            string pass; Rmi.Marshaler.Read(__msg, out pass);

                //            bool bRet = RequestBank(remote, pkOption, option, money, pass);
                //            if (bRet == false)
                //                NeedImplement("RequestBank");
                //        }
                //        break;

                //    case Common.GameRoomInUser:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRoomInUser(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRoomInUser");
                //        }
                //        break;

                //    case Common.GameReady:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameReady(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameReady");
                //        }
                //        break;

                //    case Common.GameSelectOrder:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameSelectOrder(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameSelectOrder");
                //        }
                //        break;

                //    case Common.GameDistributedEnd:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameDistributedEnd(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameDistributedEnd");
                //        }
                //        break;

                //    case Common.GameActionPutCard:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameActionPutCard(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameActionPutCard");
                //        }
                //        break;

                //    case Common.GameActionFlipBomb:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameActionFlipBomb(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameActionFlipBomb");
                //        }
                //        break;

                //    case Common.GameActionChooseCard:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameActionChooseCard(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameActionChooseCard");
                //        }
                //        break;

                //    case Common.GameSelectKookjin:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameSelectKookjin(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameSelectKookjin");
                //        }
                //        break;

                //    case Common.GameSelectGoStop:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameSelectGoStop(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameSelectGoStop");
                //        }
                //        break;

                //    case Common.GameSelectPush:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameSelectPush(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameSelectPush");
                //        }
                //        break;

                //    case Common.GamePractice:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GamePractice(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GamePractice");
                //        }
                //        break;

                //    case Common.GameRoomOut:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRoomOut(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRoomOut");
                //        }
                //        break;

                //    case Common.GameRoomMove:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = GameRoomMove(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("GameRoomMove");
                //        }
                //        break;

                //    case Common.RequestPurchaseList:
                //        {

                //            bool bRet = RequestPurchaseList(remote, pkOption);
                //            if (bRet == false)
                //                NeedImplement("RequestPurchaseList");
                //        }
                //        break;

                //    case Common.RequestPurchaseAvailability:
                //        {
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);

                //            bool bRet = RequestPurchaseAvailability(remote, pkOption, pid);
                //            if (bRet == false)
                //                NeedImplement("RequestPurchaseAvailability");
                //        }
                //        break;

                //    case Common.RequestPurchaseReceiptCheck:
                //        {
                //            string result; Rmi.Marshaler.Read(__msg, out result);

                //            bool bRet = RequestPurchaseReceiptCheck(remote, pkOption, result);
                //            if (bRet == false)
                //                NeedImplement("RequestPurchaseReceiptCheck");
                //        }
                //        break;

                //    case Common.RequestPurchaseResult:
                //        {
                //            System.Guid token; Rmi.Marshaler.Read(__msg, out token);

                //            bool bRet = RequestPurchaseResult(remote, pkOption, token);
                //            if (bRet == false)
                //                NeedImplement("RequestPurchaseResult");
                //        }
                //        break;

                //    case Common.RequestPurchaseCash:
                //        {
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);

                //            bool bRet = RequestPurchaseCash(remote, pkOption, pid);
                //            if (bRet == false)
                //                NeedImplement("RequestPurchaseCash");
                //        }
                //        break;

                //    case Common.RequestMyroomList:
                //        {

                //            bool bRet = RequestMyroomList(remote, pkOption);
                //            if (bRet == false)
                //                NeedImplement("RequestMyroomList");
                //        }
                //        break;

                //    case Common.RequestMyroomAction:
                //        {
                //            string pid; Rmi.Marshaler.Read(__msg, out pid);

                //            bool bRet = RequestMyroomAction(remote, pkOption, pid);
                //            if (bRet == false)
                //                NeedImplement("RequestMyroomAction");
                //        }
                //        break;

                //    case Common.RequestGameOptions:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RequestGameOptions(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("RequestGameOptions");
                //        }
                //        break;

                //    case Common.RequestLobbyEventInfo:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RequestLobbyEventInfo(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("RequestLobbyEventInfo");
                //        }
                //        break;

                //    case Common.RequestLobbyEventParticipate:
                //        {
                //            //ZNet.ArrByte data; Rmi.Marshaler.Read(__msg, out data);

                //            bool bRet = RequestLobbyEventParticipate(remote, pkOption, __msg);
                //            if (bRet == false)
                //                NeedImplement("RequestLobbyEventParticipate");
                //        }
                //        break;

                //    default: goto __fail;
                //}

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

