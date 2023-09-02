// Auto created from IDLCompiler.exe
#pragma once

#include "SS_common.h"

namespace SS
{
	class Proxy : public Zero::IProxy {
	public:
		virtual bool MasterAllShutdown(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &msg) sealed;

		virtual bool MasterNotifyP2PServerInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool P2PMemberCheck(Zero::RemoteID remote, Zero::CPackOption pkOption) sealed;

		virtual bool RoomLobbyMakeRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.RoomInfo &roomInfo, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const string &Pass, const int &shopId) sealed;

		virtual bool RoomLobbyJoinRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const int &shopId) sealed;

		virtual bool RoomLobbyOutRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &userID) sealed;

		virtual bool RoomLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message) sealed;

		virtual bool RoomLobbyEventStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type) sealed;

		virtual bool RoomLobbyEventEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type, const string &name, const long &reward) sealed;

		virtual bool LobbyRoomJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot) sealed;

		virtual bool LobbyRoomNotifyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period) sealed;

		virtual bool LobbyRoomNotifyServermaintenance(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &release) sealed;

		virtual bool LobbyRoomReloadServerData(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type) sealed;

		virtual bool LobbyRoomKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID) sealed;

		virtual bool LobbyLoginKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID) sealed;

		virtual bool RoomLobbyRequestMoveRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const ZNet.RemoteID &userRemote, const int &userID, const long &money, const bool &ipFree, const bool &shopFree, const int &shopId) sealed;

		virtual bool LobbyRoomResponseMoveRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &makeRoom, const System.Guid &roomID, const ZNet.NetAddress &addr, const int &chanID, const ZNet.RemoteID &userRemote, const string &message) sealed;

		virtual bool ServerRequestDataSync(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &isLobby) sealed;

		virtual bool RoomLobbyResponseDataSync(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool RelayLobbyResponseDataSync(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool RelayClientJoin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.NetAddress &addr, const ZNet.ArrByte &param) sealed;

		virtual bool RelayClientLeave(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &bMoveServer) sealed;

		virtual bool RelayCloseRemoteClient(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) sealed;

		virtual bool RelayServerMoveFailure(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) sealed;

		virtual bool RelayRequestLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &id, const string &key) sealed;

		virtual bool RelayRequestJoinInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) sealed;

		virtual bool RelayRequestChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID) sealed;

		virtual bool RelayRequestRoomMake(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType, const string &pass) sealed;

		virtual bool RelayRequestRoomJoin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType) sealed;

		virtual bool RelayRequestRoomJoinSelect(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &roomNumber, const string &pass) sealed;

		virtual bool RelayRequestBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &option, const long &money, const string &pass) sealed;

		virtual bool RelayRequestPurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) sealed;

		virtual bool RelayRequestPurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid) sealed;

		virtual bool RelayRequestPurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &result) sealed;

		virtual bool RelayRequestPurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Guid &token) sealed;

		virtual bool RelayRequestPurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid) sealed;

		virtual bool RelayRequestMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) sealed;

		virtual bool RelayRequestMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid) sealed;

		virtual bool LobbyRelayResponsePurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge) sealed;

		virtual bool LobbyRelayResponsePurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &available, const string &reason) sealed;

		virtual bool LobbyRelayResponsePurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const System.Guid &token) sealed;

		virtual bool LobbyRelayResponsePurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason) sealed;

		virtual bool LobbyRelayResponsePurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason) sealed;

		virtual bool LobbyRelayResponseMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &json) sealed;

		virtual bool LobbyRelayResponseMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid, const bool &result, const string &reason) sealed;

		virtual bool LobbyRelayServerMoveStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param) sealed;

		virtual bool LobbyRelayResponseLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &key) sealed;

		virtual bool LobbyRelayNotifyUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const Rmi.Marshaler.LobbyUserInfo &userInfo) sealed;

		virtual bool LobbyRelayNotifyUserList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList) sealed;

		virtual bool LobbyRelayNotifyRoomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos) sealed;

		virtual bool LobbyRelayResponseChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID) sealed;

		virtual bool LobbyRelayResponseLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message) sealed;

		virtual bool LobbyRelayResponseBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const int &resultType) sealed;

		virtual bool LobbyRelayNotifyJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const long &jackpot) sealed;

		virtual bool LobbyRelayNotifyLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &type, const string &message, const int &period) sealed;

		virtual bool RoomRelayServerMoveStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param) sealed;

		virtual bool RelayRequestOutRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) sealed;

		virtual bool RelayResponseOutRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultOut) sealed;

		virtual bool RelayRequestMoveRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) sealed;

		virtual bool RelayResponseMoveRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage) sealed;

		virtual bool RelayGameRoomIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameSelectOrder(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameDistributedEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameActionPutCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameActionFlipBomb(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameActionChooseCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameSelectKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameSelectGoStop(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGameSelectPush(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayGamePractice(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayRoomIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result) sealed;

		virtual bool GameRelayRequestReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayRequestSelectOrder(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayOrderEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayDistributedStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayFloorHasBonus(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayTurnStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelaySelectCardResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayFlipDeckResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayTurnResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayNotifyIndex(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayNotifyStat(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayRequestKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayNotifyKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayRequestGoStop(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayNotifyGoStop(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayMoveKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayEventStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayOver(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayRequestPush(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayResponseRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage) sealed;

		virtual bool GameRelayPracticeEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayResponseRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &permissionOut) sealed;

		virtual bool GameRelayKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayRoomInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayUserOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayObserveInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayNotifyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayNotifyJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayRequestLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool LobbyRelayResponseLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool RelayRequestLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool LobbyRelayResponseLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool GameRelayResponseRoomMissionInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) sealed;

		virtual bool ServerMoveStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param) sealed;

		virtual bool ServerMoveEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &Moved) sealed;

		virtual bool ResponseLauncherLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &nickname, const string &key, const byte &resultType) sealed;

		virtual bool ResponseLauncherLogout(Zero::RemoteID remote, Zero::CPackOption pkOption) sealed;

		virtual bool ResponseLoginKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage) sealed;

		virtual bool ResponseLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &key) sealed;

		virtual bool ResponseLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage) sealed;

		virtual bool NotifyLobbyList(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &lobbyList) sealed;

		virtual bool NotifyUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.LobbyUserInfo &userInfo) sealed;

		virtual bool NotifyUserList(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList) sealed;

		virtual bool NotifyRoomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos) sealed;

		virtual bool ResponseChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID) sealed;

		virtual bool ResponseLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &message) sealed;

		virtual bool ResponseBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const int &resultType) sealed;

		virtual bool NotifyJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot) sealed;

		virtual bool NotifyLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period) sealed;

		virtual bool GameRoomIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result) sealed;

		virtual bool GameRequestReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameRequestSelectOrder(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameOrderEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameDistributedStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameFloorHasBonus(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameTurnStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameSelectCardResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameFlipDeckResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameTurnResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameNotifyIndex(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameNotifyStat(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameRequestKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameNotifyKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameRequestGoStop(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameNotifyGoStop(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameMoveKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameEventStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameOver(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameRequestPush(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameResponseRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &move, const string &errorMessage) sealed;

		virtual bool GamePracticeEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameResponseRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result) sealed;

		virtual bool GameKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameRoomInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameUserOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameObserveInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameNotifyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool ResponsePurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge) sealed;

		virtual bool ResponsePurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &available, const string &reason) sealed;

		virtual bool ResponsePurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const System.Guid &token) sealed;

		virtual bool ResponsePurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason) sealed;

		virtual bool ResponsePurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason) sealed;

		virtual bool ResponseMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &json) sealed;

		virtual bool ResponseMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid, const bool &result, const string &reason) sealed;

		virtual bool ResponseGameOptions(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool ResponseLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool ResponseLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameResponseRoomMissionInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool ServerMoveFailure(Zero::RemoteID remote, Zero::CPackOption pkOption) sealed;

		virtual bool RequestLauncherLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &pass) sealed;

		virtual bool RequestLauncherLogout(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key) sealed;

		virtual bool RequestLoginKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key) sealed;

		virtual bool RequestLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key) sealed;

		virtual bool RequestLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &name, const string &pass) sealed;

		virtual bool RequestLobbyList(Zero::RemoteID remote, Zero::CPackOption pkOption) sealed;

		virtual bool RequestGoLobby(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &lobbyName) sealed;

		virtual bool RequestJoinInfo(Zero::RemoteID remote, Zero::CPackOption pkOption) sealed;

		virtual bool RequestChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID) sealed;

		virtual bool RequestRoomMake(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType, const string &pass) sealed;

		virtual bool RequestRoomJoin(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType) sealed;

		virtual bool RequestRoomJoinSelect(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &roomNumber, const string &pass) sealed;

		virtual bool RequestBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &option, const long &money, const string &pass) sealed;

		virtual bool GameRoomIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameSelectOrder(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameDistributedEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameActionPutCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameActionFlipBomb(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameActionChooseCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameSelectKookjin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameSelectGoStop(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameSelectPush(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GamePractice(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool GameRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool RequestPurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption) sealed;

		virtual bool RequestPurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid) sealed;

		virtual bool RequestPurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &result) sealed;

		virtual bool RequestPurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &token) sealed;

		virtual bool RequestPurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid) sealed;

		virtual bool RequestMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption) sealed;

		virtual bool RequestMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid) sealed;

		virtual bool RequestGameOptions(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool RequestLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

		virtual bool RequestLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) sealed;

	};
}

