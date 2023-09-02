// Auto created from IDLCompiler.exe
#pragma once

#include "SS_common.h"

namespace SS
{
	class Stub : public Zero::IStub {
	public:
		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &msg)> MasterAllShutdown;
		#define Func_SS_MasterAllShutdown (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &msg)

		virtual bool MasterAllShutdown_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &msg)
		{
			if (!MasterAllShutdown) return false;
			return MasterAllShutdown(remote, pkOption, msg);
		}
		#define Stub_SS_MasterAllShutdown_override bool MasterAllShutdown_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &msg) override 
		#define Stub_SS_MasterAllShutdown(ClassName) bool ClassName::MasterAllShutdown_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &msg)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> MasterNotifyP2PServerInfo;
		#define Func_SS_MasterNotifyP2PServerInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool MasterNotifyP2PServerInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!MasterNotifyP2PServerInfo) return false;
			return MasterNotifyP2PServerInfo(remote, pkOption, data);
		}
		#define Stub_SS_MasterNotifyP2PServerInfo_override bool MasterNotifyP2PServerInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_MasterNotifyP2PServerInfo(ClassName) bool ClassName::MasterNotifyP2PServerInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption)> P2PMemberCheck;
		#define Func_SS_P2PMemberCheck (Zero::RemoteID remote, Zero::CPackOption pkOption)

		virtual bool P2PMemberCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)
		{
			if (!P2PMemberCheck) return false;
			return P2PMemberCheck(remote, pkOption);
		}
		#define Stub_SS_P2PMemberCheck_override bool P2PMemberCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption) override 
		#define Stub_SS_P2PMemberCheck(ClassName) bool ClassName::P2PMemberCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.RoomInfo &roomInfo, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const string &Pass, const int &shopId)> RoomLobbyMakeRoom;
		#define Func_SS_RoomLobbyMakeRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.RoomInfo &roomInfo, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const string &Pass, const int &shopId)

		virtual bool RoomLobbyMakeRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.RoomInfo &roomInfo, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const string &Pass, const int &shopId)
		{
			if (!RoomLobbyMakeRoom) return false;
			return RoomLobbyMakeRoom(remote, pkOption, roomInfo, userInfo, userID, IP, Pass, shopId);
		}
		#define Stub_SS_RoomLobbyMakeRoom_override bool RoomLobbyMakeRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.RoomInfo &roomInfo, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const string &Pass, const int &shopId) override 
		#define Stub_SS_RoomLobbyMakeRoom(ClassName) bool ClassName::RoomLobbyMakeRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.RoomInfo &roomInfo, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const string &Pass, const int &shopId)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const int &shopId)> RoomLobbyJoinRoom;
		#define Func_SS_RoomLobbyJoinRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const int &shopId)

		virtual bool RoomLobbyJoinRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const int &shopId)
		{
			if (!RoomLobbyJoinRoom) return false;
			return RoomLobbyJoinRoom(remote, pkOption, roomID, userInfo, userID, IP, shopId);
		}
		#define Stub_SS_RoomLobbyJoinRoom_override bool RoomLobbyJoinRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const int &shopId) override 
		#define Stub_SS_RoomLobbyJoinRoom(ClassName) bool ClassName::RoomLobbyJoinRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const int &shopId)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &userID)> RoomLobbyOutRoom;
		#define Func_SS_RoomLobbyOutRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &userID)

		virtual bool RoomLobbyOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &userID)
		{
			if (!RoomLobbyOutRoom) return false;
			return RoomLobbyOutRoom(remote, pkOption, roomID, userID);
		}
		#define Stub_SS_RoomLobbyOutRoom_override bool RoomLobbyOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &userID) override 
		#define Stub_SS_RoomLobbyOutRoom(ClassName) bool ClassName::RoomLobbyOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &userID)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)> RoomLobbyMessage;
		#define Func_SS_RoomLobbyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)

		virtual bool RoomLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)
		{
			if (!RoomLobbyMessage) return false;
			return RoomLobbyMessage(remote, pkOption, userRemote, message);
		}
		#define Stub_SS_RoomLobbyMessage_override bool RoomLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message) override 
		#define Stub_SS_RoomLobbyMessage(ClassName) bool ClassName::RoomLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type)> RoomLobbyEventStart;
		#define Func_SS_RoomLobbyEventStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type)

		virtual bool RoomLobbyEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type)
		{
			if (!RoomLobbyEventStart) return false;
			return RoomLobbyEventStart(remote, pkOption, roomID, type);
		}
		#define Stub_SS_RoomLobbyEventStart_override bool RoomLobbyEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type) override 
		#define Stub_SS_RoomLobbyEventStart(ClassName) bool ClassName::RoomLobbyEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type, const string &name, const long &reward)> RoomLobbyEventEnd;
		#define Func_SS_RoomLobbyEventEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type, const string &name, const long &reward)

		virtual bool RoomLobbyEventEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type, const string &name, const long &reward)
		{
			if (!RoomLobbyEventEnd) return false;
			return RoomLobbyEventEnd(remote, pkOption, roomID, type, name, reward);
		}
		#define Stub_SS_RoomLobbyEventEnd_override bool RoomLobbyEventEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type, const string &name, const long &reward) override 
		#define Stub_SS_RoomLobbyEventEnd(ClassName) bool ClassName::RoomLobbyEventEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type, const string &name, const long &reward)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)> LobbyRoomJackpotInfo;
		#define Func_SS_LobbyRoomJackpotInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)

		virtual bool LobbyRoomJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)
		{
			if (!LobbyRoomJackpotInfo) return false;
			return LobbyRoomJackpotInfo(remote, pkOption, jackpot);
		}
		#define Stub_SS_LobbyRoomJackpotInfo_override bool LobbyRoomJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot) override 
		#define Stub_SS_LobbyRoomJackpotInfo(ClassName) bool ClassName::LobbyRoomJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)> LobbyRoomNotifyMessage;
		#define Func_SS_LobbyRoomNotifyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)

		virtual bool LobbyRoomNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)
		{
			if (!LobbyRoomNotifyMessage) return false;
			return LobbyRoomNotifyMessage(remote, pkOption, type, message, period);
		}
		#define Stub_SS_LobbyRoomNotifyMessage_override bool LobbyRoomNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period) override 
		#define Stub_SS_LobbyRoomNotifyMessage(ClassName) bool ClassName::LobbyRoomNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &release)> LobbyRoomNotifyServermaintenance;
		#define Func_SS_LobbyRoomNotifyServermaintenance (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &release)

		virtual bool LobbyRoomNotifyServermaintenance_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &release)
		{
			if (!LobbyRoomNotifyServermaintenance) return false;
			return LobbyRoomNotifyServermaintenance(remote, pkOption, type, message, release);
		}
		#define Stub_SS_LobbyRoomNotifyServermaintenance_override bool LobbyRoomNotifyServermaintenance_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &release) override 
		#define Stub_SS_LobbyRoomNotifyServermaintenance(ClassName) bool ClassName::LobbyRoomNotifyServermaintenance_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &release)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type)> LobbyRoomReloadServerData;
		#define Func_SS_LobbyRoomReloadServerData (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type)

		virtual bool LobbyRoomReloadServerData_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type)
		{
			if (!LobbyRoomReloadServerData) return false;
			return LobbyRoomReloadServerData(remote, pkOption, type);
		}
		#define Stub_SS_LobbyRoomReloadServerData_override bool LobbyRoomReloadServerData_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type) override 
		#define Stub_SS_LobbyRoomReloadServerData(ClassName) bool ClassName::LobbyRoomReloadServerData_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)> LobbyRoomKickUser;
		#define Func_SS_LobbyRoomKickUser (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)

		virtual bool LobbyRoomKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)
		{
			if (!LobbyRoomKickUser) return false;
			return LobbyRoomKickUser(remote, pkOption, userID);
		}
		#define Stub_SS_LobbyRoomKickUser_override bool LobbyRoomKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID) override 
		#define Stub_SS_LobbyRoomKickUser(ClassName) bool ClassName::LobbyRoomKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)> LobbyLoginKickUser;
		#define Func_SS_LobbyLoginKickUser (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)

		virtual bool LobbyLoginKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)
		{
			if (!LobbyLoginKickUser) return false;
			return LobbyLoginKickUser(remote, pkOption, userID);
		}
		#define Stub_SS_LobbyLoginKickUser_override bool LobbyLoginKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID) override 
		#define Stub_SS_LobbyLoginKickUser(ClassName) bool ClassName::LobbyLoginKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const ZNet.RemoteID &userRemote, const int &userID, const long &money, const bool &ipFree, const bool &shopFree, const int &shopId)> RoomLobbyRequestMoveRoom;
		#define Func_SS_RoomLobbyRequestMoveRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const ZNet.RemoteID &userRemote, const int &userID, const long &money, const bool &ipFree, const bool &shopFree, const int &shopId)

		virtual bool RoomLobbyRequestMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const ZNet.RemoteID &userRemote, const int &userID, const long &money, const bool &ipFree, const bool &shopFree, const int &shopId)
		{
			if (!RoomLobbyRequestMoveRoom) return false;
			return RoomLobbyRequestMoveRoom(remote, pkOption, roomID, userRemote, userID, money, ipFree, shopFree, shopId);
		}
		#define Stub_SS_RoomLobbyRequestMoveRoom_override bool RoomLobbyRequestMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const ZNet.RemoteID &userRemote, const int &userID, const long &money, const bool &ipFree, const bool &shopFree, const int &shopId) override 
		#define Stub_SS_RoomLobbyRequestMoveRoom(ClassName) bool ClassName::RoomLobbyRequestMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const ZNet.RemoteID &userRemote, const int &userID, const long &money, const bool &ipFree, const bool &shopFree, const int &shopId)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &makeRoom, const System.Guid &roomID, const ZNet.NetAddress &addr, const int &chanID, const ZNet.RemoteID &userRemote, const string &message)> LobbyRoomResponseMoveRoom;
		#define Func_SS_LobbyRoomResponseMoveRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &makeRoom, const System.Guid &roomID, const ZNet.NetAddress &addr, const int &chanID, const ZNet.RemoteID &userRemote, const string &message)

		virtual bool LobbyRoomResponseMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &makeRoom, const System.Guid &roomID, const ZNet.NetAddress &addr, const int &chanID, const ZNet.RemoteID &userRemote, const string &message)
		{
			if (!LobbyRoomResponseMoveRoom) return false;
			return LobbyRoomResponseMoveRoom(remote, pkOption, makeRoom, roomID, addr, chanID, userRemote, message);
		}
		#define Stub_SS_LobbyRoomResponseMoveRoom_override bool LobbyRoomResponseMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &makeRoom, const System.Guid &roomID, const ZNet.NetAddress &addr, const int &chanID, const ZNet.RemoteID &userRemote, const string &message) override 
		#define Stub_SS_LobbyRoomResponseMoveRoom(ClassName) bool ClassName::LobbyRoomResponseMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &makeRoom, const System.Guid &roomID, const ZNet.NetAddress &addr, const int &chanID, const ZNet.RemoteID &userRemote, const string &message)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &isLobby)> ServerRequestDataSync;
		#define Func_SS_ServerRequestDataSync (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &isLobby)

		virtual bool ServerRequestDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &isLobby)
		{
			if (!ServerRequestDataSync) return false;
			return ServerRequestDataSync(remote, pkOption, isLobby);
		}
		#define Stub_SS_ServerRequestDataSync_override bool ServerRequestDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &isLobby) override 
		#define Stub_SS_ServerRequestDataSync(ClassName) bool ClassName::ServerRequestDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &isLobby)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> RoomLobbyResponseDataSync;
		#define Func_SS_RoomLobbyResponseDataSync (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool RoomLobbyResponseDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!RoomLobbyResponseDataSync) return false;
			return RoomLobbyResponseDataSync(remote, pkOption, data);
		}
		#define Stub_SS_RoomLobbyResponseDataSync_override bool RoomLobbyResponseDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_RoomLobbyResponseDataSync(ClassName) bool ClassName::RoomLobbyResponseDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> RelayLobbyResponseDataSync;
		#define Func_SS_RelayLobbyResponseDataSync (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool RelayLobbyResponseDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!RelayLobbyResponseDataSync) return false;
			return RelayLobbyResponseDataSync(remote, pkOption, data);
		}
		#define Stub_SS_RelayLobbyResponseDataSync_override bool RelayLobbyResponseDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayLobbyResponseDataSync(ClassName) bool ClassName::RelayLobbyResponseDataSync_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.NetAddress &addr, const ZNet.ArrByte &param)> RelayClientJoin;
		#define Func_SS_RelayClientJoin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.NetAddress &addr, const ZNet.ArrByte &param)

		virtual bool RelayClientJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.NetAddress &addr, const ZNet.ArrByte &param)
		{
			if (!RelayClientJoin) return false;
			return RelayClientJoin(remote, pkOption, userRemote, addr, param);
		}
		#define Stub_SS_RelayClientJoin_override bool RelayClientJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.NetAddress &addr, const ZNet.ArrByte &param) override 
		#define Stub_SS_RelayClientJoin(ClassName) bool ClassName::RelayClientJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.NetAddress &addr, const ZNet.ArrByte &param)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &bMoveServer)> RelayClientLeave;
		#define Func_SS_RelayClientLeave (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &bMoveServer)

		virtual bool RelayClientLeave_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &bMoveServer)
		{
			if (!RelayClientLeave) return false;
			return RelayClientLeave(remote, pkOption, userRemote, bMoveServer);
		}
		#define Stub_SS_RelayClientLeave_override bool RelayClientLeave_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &bMoveServer) override 
		#define Stub_SS_RelayClientLeave(ClassName) bool ClassName::RelayClientLeave_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &bMoveServer)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)> RelayCloseRemoteClient;
		#define Func_SS_RelayCloseRemoteClient (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		virtual bool RelayCloseRemoteClient_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)
		{
			if (!RelayCloseRemoteClient) return false;
			return RelayCloseRemoteClient(remote, pkOption, userRemote);
		}
		#define Stub_SS_RelayCloseRemoteClient_override bool RelayCloseRemoteClient_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) override 
		#define Stub_SS_RelayCloseRemoteClient(ClassName) bool ClassName::RelayCloseRemoteClient_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)> RelayServerMoveFailure;
		#define Func_SS_RelayServerMoveFailure (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		virtual bool RelayServerMoveFailure_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)
		{
			if (!RelayServerMoveFailure) return false;
			return RelayServerMoveFailure(remote, pkOption, userRemote);
		}
		#define Stub_SS_RelayServerMoveFailure_override bool RelayServerMoveFailure_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) override 
		#define Stub_SS_RelayServerMoveFailure(ClassName) bool ClassName::RelayServerMoveFailure_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &id, const string &key)> RelayRequestLobbyKey;
		#define Func_SS_RelayRequestLobbyKey (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &id, const string &key)

		virtual bool RelayRequestLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &id, const string &key)
		{
			if (!RelayRequestLobbyKey) return false;
			return RelayRequestLobbyKey(remote, pkOption, userRemote, id, key);
		}
		#define Stub_SS_RelayRequestLobbyKey_override bool RelayRequestLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &id, const string &key) override 
		#define Stub_SS_RelayRequestLobbyKey(ClassName) bool ClassName::RelayRequestLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &id, const string &key)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)> RelayRequestJoinInfo;
		#define Func_SS_RelayRequestJoinInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		virtual bool RelayRequestJoinInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)
		{
			if (!RelayRequestJoinInfo) return false;
			return RelayRequestJoinInfo(remote, pkOption, userRemote);
		}
		#define Stub_SS_RelayRequestJoinInfo_override bool RelayRequestJoinInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) override 
		#define Stub_SS_RelayRequestJoinInfo(ClassName) bool ClassName::RelayRequestJoinInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)> RelayRequestChannelMove;
		#define Func_SS_RelayRequestChannelMove (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)

		virtual bool RelayRequestChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)
		{
			if (!RelayRequestChannelMove) return false;
			return RelayRequestChannelMove(remote, pkOption, userRemote, chanID);
		}
		#define Stub_SS_RelayRequestChannelMove_override bool RelayRequestChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID) override 
		#define Stub_SS_RelayRequestChannelMove(ClassName) bool ClassName::RelayRequestChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType, const string &pass)> RelayRequestRoomMake;
		#define Func_SS_RelayRequestRoomMake (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType, const string &pass)

		virtual bool RelayRequestRoomMake_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType, const string &pass)
		{
			if (!RelayRequestRoomMake) return false;
			return RelayRequestRoomMake(remote, pkOption, userRemote, relayID, chanID, betType, pass);
		}
		#define Stub_SS_RelayRequestRoomMake_override bool RelayRequestRoomMake_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType, const string &pass) override 
		#define Stub_SS_RelayRequestRoomMake(ClassName) bool ClassName::RelayRequestRoomMake_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType)> RelayRequestRoomJoin;
		#define Func_SS_RelayRequestRoomJoin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType)

		virtual bool RelayRequestRoomJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType)
		{
			if (!RelayRequestRoomJoin) return false;
			return RelayRequestRoomJoin(remote, pkOption, userRemote, relayID, chanID, betType);
		}
		#define Stub_SS_RelayRequestRoomJoin_override bool RelayRequestRoomJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType) override 
		#define Stub_SS_RelayRequestRoomJoin(ClassName) bool ClassName::RelayRequestRoomJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &roomNumber, const string &pass)> RelayRequestRoomJoinSelect;
		#define Func_SS_RelayRequestRoomJoinSelect (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &roomNumber, const string &pass)

		virtual bool RelayRequestRoomJoinSelect_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &roomNumber, const string &pass)
		{
			if (!RelayRequestRoomJoinSelect) return false;
			return RelayRequestRoomJoinSelect(remote, pkOption, userRemote, relayID, chanID, roomNumber, pass);
		}
		#define Stub_SS_RelayRequestRoomJoinSelect_override bool RelayRequestRoomJoinSelect_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &roomNumber, const string &pass) override 
		#define Stub_SS_RelayRequestRoomJoinSelect(ClassName) bool ClassName::RelayRequestRoomJoinSelect_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &roomNumber, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &option, const long &money, const string &pass)> RelayRequestBank;
		#define Func_SS_RelayRequestBank (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &option, const long &money, const string &pass)

		virtual bool RelayRequestBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &option, const long &money, const string &pass)
		{
			if (!RelayRequestBank) return false;
			return RelayRequestBank(remote, pkOption, userRemote, option, money, pass);
		}
		#define Stub_SS_RelayRequestBank_override bool RelayRequestBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &option, const long &money, const string &pass) override 
		#define Stub_SS_RelayRequestBank(ClassName) bool ClassName::RelayRequestBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &option, const long &money, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)> RelayRequestPurchaseList;
		#define Func_SS_RelayRequestPurchaseList (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		virtual bool RelayRequestPurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)
		{
			if (!RelayRequestPurchaseList) return false;
			return RelayRequestPurchaseList(remote, pkOption, userRemote);
		}
		#define Stub_SS_RelayRequestPurchaseList_override bool RelayRequestPurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) override 
		#define Stub_SS_RelayRequestPurchaseList(ClassName) bool ClassName::RelayRequestPurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)> RelayRequestPurchaseAvailability;
		#define Func_SS_RelayRequestPurchaseAvailability (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)

		virtual bool RelayRequestPurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)
		{
			if (!RelayRequestPurchaseAvailability) return false;
			return RelayRequestPurchaseAvailability(remote, pkOption, userRemote, pid);
		}
		#define Stub_SS_RelayRequestPurchaseAvailability_override bool RelayRequestPurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid) override 
		#define Stub_SS_RelayRequestPurchaseAvailability(ClassName) bool ClassName::RelayRequestPurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &result)> RelayRequestPurchaseReceiptCheck;
		#define Func_SS_RelayRequestPurchaseReceiptCheck (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &result)

		virtual bool RelayRequestPurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &result)
		{
			if (!RelayRequestPurchaseReceiptCheck) return false;
			return RelayRequestPurchaseReceiptCheck(remote, pkOption, userRemote, result);
		}
		#define Stub_SS_RelayRequestPurchaseReceiptCheck_override bool RelayRequestPurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &result) override 
		#define Stub_SS_RelayRequestPurchaseReceiptCheck(ClassName) bool ClassName::RelayRequestPurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &result)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Guid &token)> RelayRequestPurchaseResult;
		#define Func_SS_RelayRequestPurchaseResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Guid &token)

		virtual bool RelayRequestPurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Guid &token)
		{
			if (!RelayRequestPurchaseResult) return false;
			return RelayRequestPurchaseResult(remote, pkOption, userRemote, token);
		}
		#define Stub_SS_RelayRequestPurchaseResult_override bool RelayRequestPurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Guid &token) override 
		#define Stub_SS_RelayRequestPurchaseResult(ClassName) bool ClassName::RelayRequestPurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Guid &token)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)> RelayRequestPurchaseCash;
		#define Func_SS_RelayRequestPurchaseCash (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)

		virtual bool RelayRequestPurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)
		{
			if (!RelayRequestPurchaseCash) return false;
			return RelayRequestPurchaseCash(remote, pkOption, userRemote, pid);
		}
		#define Stub_SS_RelayRequestPurchaseCash_override bool RelayRequestPurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid) override 
		#define Stub_SS_RelayRequestPurchaseCash(ClassName) bool ClassName::RelayRequestPurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)> RelayRequestMyroomList;
		#define Func_SS_RelayRequestMyroomList (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		virtual bool RelayRequestMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)
		{
			if (!RelayRequestMyroomList) return false;
			return RelayRequestMyroomList(remote, pkOption, userRemote);
		}
		#define Stub_SS_RelayRequestMyroomList_override bool RelayRequestMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) override 
		#define Stub_SS_RelayRequestMyroomList(ClassName) bool ClassName::RelayRequestMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)> RelayRequestMyroomAction;
		#define Func_SS_RelayRequestMyroomAction (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)

		virtual bool RelayRequestMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)
		{
			if (!RelayRequestMyroomAction) return false;
			return RelayRequestMyroomAction(remote, pkOption, userRemote, pid);
		}
		#define Stub_SS_RelayRequestMyroomAction_override bool RelayRequestMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid) override 
		#define Stub_SS_RelayRequestMyroomAction(ClassName) bool ClassName::RelayRequestMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)> LobbyRelayResponsePurchaseList;
		#define Func_SS_LobbyRelayResponsePurchaseList (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)

		virtual bool LobbyRelayResponsePurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)
		{
			if (!LobbyRelayResponsePurchaseList) return false;
			return LobbyRelayResponsePurchaseList(remote, pkOption, userRemote, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
		}
		#define Stub_SS_LobbyRelayResponsePurchaseList_override bool LobbyRelayResponsePurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge) override 
		#define Stub_SS_LobbyRelayResponsePurchaseList(ClassName) bool ClassName::LobbyRelayResponsePurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &available, const string &reason)> LobbyRelayResponsePurchaseAvailability;
		#define Func_SS_LobbyRelayResponsePurchaseAvailability (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &available, const string &reason)

		virtual bool LobbyRelayResponsePurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &available, const string &reason)
		{
			if (!LobbyRelayResponsePurchaseAvailability) return false;
			return LobbyRelayResponsePurchaseAvailability(remote, pkOption, userRemote, available, reason);
		}
		#define Stub_SS_LobbyRelayResponsePurchaseAvailability_override bool LobbyRelayResponsePurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &available, const string &reason) override 
		#define Stub_SS_LobbyRelayResponsePurchaseAvailability(ClassName) bool ClassName::LobbyRelayResponsePurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &available, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const System.Guid &token)> LobbyRelayResponsePurchaseReceiptCheck;
		#define Func_SS_LobbyRelayResponsePurchaseReceiptCheck (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const System.Guid &token)

		virtual bool LobbyRelayResponsePurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const System.Guid &token)
		{
			if (!LobbyRelayResponsePurchaseReceiptCheck) return false;
			return LobbyRelayResponsePurchaseReceiptCheck(remote, pkOption, userRemote, result, token);
		}
		#define Stub_SS_LobbyRelayResponsePurchaseReceiptCheck_override bool LobbyRelayResponsePurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const System.Guid &token) override 
		#define Stub_SS_LobbyRelayResponsePurchaseReceiptCheck(ClassName) bool ClassName::LobbyRelayResponsePurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const System.Guid &token)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)> LobbyRelayResponsePurchaseResult;
		#define Func_SS_LobbyRelayResponsePurchaseResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)

		virtual bool LobbyRelayResponsePurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)
		{
			if (!LobbyRelayResponsePurchaseResult) return false;
			return LobbyRelayResponsePurchaseResult(remote, pkOption, userRemote, result, reason);
		}
		#define Stub_SS_LobbyRelayResponsePurchaseResult_override bool LobbyRelayResponsePurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason) override 
		#define Stub_SS_LobbyRelayResponsePurchaseResult(ClassName) bool ClassName::LobbyRelayResponsePurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)> LobbyRelayResponsePurchaseCash;
		#define Func_SS_LobbyRelayResponsePurchaseCash (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)

		virtual bool LobbyRelayResponsePurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)
		{
			if (!LobbyRelayResponsePurchaseCash) return false;
			return LobbyRelayResponsePurchaseCash(remote, pkOption, userRemote, result, reason);
		}
		#define Stub_SS_LobbyRelayResponsePurchaseCash_override bool LobbyRelayResponsePurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason) override 
		#define Stub_SS_LobbyRelayResponsePurchaseCash(ClassName) bool ClassName::LobbyRelayResponsePurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &json)> LobbyRelayResponseMyroomList;
		#define Func_SS_LobbyRelayResponseMyroomList (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &json)

		virtual bool LobbyRelayResponseMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &json)
		{
			if (!LobbyRelayResponseMyroomList) return false;
			return LobbyRelayResponseMyroomList(remote, pkOption, userRemote, json);
		}
		#define Stub_SS_LobbyRelayResponseMyroomList_override bool LobbyRelayResponseMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &json) override 
		#define Stub_SS_LobbyRelayResponseMyroomList(ClassName) bool ClassName::LobbyRelayResponseMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &json)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid, const bool &result, const string &reason)> LobbyRelayResponseMyroomAction;
		#define Func_SS_LobbyRelayResponseMyroomAction (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid, const bool &result, const string &reason)

		virtual bool LobbyRelayResponseMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid, const bool &result, const string &reason)
		{
			if (!LobbyRelayResponseMyroomAction) return false;
			return LobbyRelayResponseMyroomAction(remote, pkOption, userRemote, pid, result, reason);
		}
		#define Stub_SS_LobbyRelayResponseMyroomAction_override bool LobbyRelayResponseMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid, const bool &result, const string &reason) override 
		#define Stub_SS_LobbyRelayResponseMyroomAction(ClassName) bool ClassName::LobbyRelayResponseMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid, const bool &result, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)> LobbyRelayServerMoveStart;
		#define Func_SS_LobbyRelayServerMoveStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)

		virtual bool LobbyRelayServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)
		{
			if (!LobbyRelayServerMoveStart) return false;
			return LobbyRelayServerMoveStart(remote, pkOption, userRemote, moveServerIP, moveServerPort, param);
		}
		#define Stub_SS_LobbyRelayServerMoveStart_override bool LobbyRelayServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param) override 
		#define Stub_SS_LobbyRelayServerMoveStart(ClassName) bool ClassName::LobbyRelayServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &key)> LobbyRelayResponseLobbyKey;
		#define Func_SS_LobbyRelayResponseLobbyKey (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &key)

		virtual bool LobbyRelayResponseLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &key)
		{
			if (!LobbyRelayResponseLobbyKey) return false;
			return LobbyRelayResponseLobbyKey(remote, pkOption, userRemote, key);
		}
		#define Stub_SS_LobbyRelayResponseLobbyKey_override bool LobbyRelayResponseLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &key) override 
		#define Stub_SS_LobbyRelayResponseLobbyKey(ClassName) bool ClassName::LobbyRelayResponseLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &key)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const Rmi.Marshaler.LobbyUserInfo &userInfo)> LobbyRelayNotifyUserInfo;
		#define Func_SS_LobbyRelayNotifyUserInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const Rmi.Marshaler.LobbyUserInfo &userInfo)

		virtual bool LobbyRelayNotifyUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const Rmi.Marshaler.LobbyUserInfo &userInfo)
		{
			if (!LobbyRelayNotifyUserInfo) return false;
			return LobbyRelayNotifyUserInfo(remote, pkOption, userRemote, userInfo);
		}
		#define Stub_SS_LobbyRelayNotifyUserInfo_override bool LobbyRelayNotifyUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const Rmi.Marshaler.LobbyUserInfo &userInfo) override 
		#define Stub_SS_LobbyRelayNotifyUserInfo(ClassName) bool ClassName::LobbyRelayNotifyUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const Rmi.Marshaler.LobbyUserInfo &userInfo)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)> LobbyRelayNotifyUserList;
		#define Func_SS_LobbyRelayNotifyUserList (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)

		virtual bool LobbyRelayNotifyUserList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)
		{
			if (!LobbyRelayNotifyUserList) return false;
			return LobbyRelayNotifyUserList(remote, pkOption, userRemote, lobbyUserInfos, lobbyFriendList);
		}
		#define Stub_SS_LobbyRelayNotifyUserList_override bool LobbyRelayNotifyUserList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList) override 
		#define Stub_SS_LobbyRelayNotifyUserList(ClassName) bool ClassName::LobbyRelayNotifyUserList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)> LobbyRelayNotifyRoomList;
		#define Func_SS_LobbyRelayNotifyRoomList (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)

		virtual bool LobbyRelayNotifyRoomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)
		{
			if (!LobbyRelayNotifyRoomList) return false;
			return LobbyRelayNotifyRoomList(remote, pkOption, userRemote, channelID, roomInfos);
		}
		#define Stub_SS_LobbyRelayNotifyRoomList_override bool LobbyRelayNotifyRoomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos) override 
		#define Stub_SS_LobbyRelayNotifyRoomList(ClassName) bool ClassName::LobbyRelayNotifyRoomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)> LobbyRelayResponseChannelMove;
		#define Func_SS_LobbyRelayResponseChannelMove (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)

		virtual bool LobbyRelayResponseChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)
		{
			if (!LobbyRelayResponseChannelMove) return false;
			return LobbyRelayResponseChannelMove(remote, pkOption, userRemote, chanID);
		}
		#define Stub_SS_LobbyRelayResponseChannelMove_override bool LobbyRelayResponseChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID) override 
		#define Stub_SS_LobbyRelayResponseChannelMove(ClassName) bool ClassName::LobbyRelayResponseChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)> LobbyRelayResponseLobbyMessage;
		#define Func_SS_LobbyRelayResponseLobbyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)

		virtual bool LobbyRelayResponseLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)
		{
			if (!LobbyRelayResponseLobbyMessage) return false;
			return LobbyRelayResponseLobbyMessage(remote, pkOption, userRemote, message);
		}
		#define Stub_SS_LobbyRelayResponseLobbyMessage_override bool LobbyRelayResponseLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message) override 
		#define Stub_SS_LobbyRelayResponseLobbyMessage(ClassName) bool ClassName::LobbyRelayResponseLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const int &resultType)> LobbyRelayResponseBank;
		#define Func_SS_LobbyRelayResponseBank (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const int &resultType)

		virtual bool LobbyRelayResponseBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const int &resultType)
		{
			if (!LobbyRelayResponseBank) return false;
			return LobbyRelayResponseBank(remote, pkOption, userRemote, result, resultType);
		}
		#define Stub_SS_LobbyRelayResponseBank_override bool LobbyRelayResponseBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const int &resultType) override 
		#define Stub_SS_LobbyRelayResponseBank(ClassName) bool ClassName::LobbyRelayResponseBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const int &resultType)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const long &jackpot)> LobbyRelayNotifyJackpotInfo;
		#define Func_SS_LobbyRelayNotifyJackpotInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const long &jackpot)

		virtual bool LobbyRelayNotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const long &jackpot)
		{
			if (!LobbyRelayNotifyJackpotInfo) return false;
			return LobbyRelayNotifyJackpotInfo(remote, pkOption, userRemote, jackpot);
		}
		#define Stub_SS_LobbyRelayNotifyJackpotInfo_override bool LobbyRelayNotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const long &jackpot) override 
		#define Stub_SS_LobbyRelayNotifyJackpotInfo(ClassName) bool ClassName::LobbyRelayNotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const long &jackpot)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &type, const string &message, const int &period)> LobbyRelayNotifyLobbyMessage;
		#define Func_SS_LobbyRelayNotifyLobbyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &type, const string &message, const int &period)

		virtual bool LobbyRelayNotifyLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &type, const string &message, const int &period)
		{
			if (!LobbyRelayNotifyLobbyMessage) return false;
			return LobbyRelayNotifyLobbyMessage(remote, pkOption, userRemote, type, message, period);
		}
		#define Stub_SS_LobbyRelayNotifyLobbyMessage_override bool LobbyRelayNotifyLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &type, const string &message, const int &period) override 
		#define Stub_SS_LobbyRelayNotifyLobbyMessage(ClassName) bool ClassName::LobbyRelayNotifyLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &type, const string &message, const int &period)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)> RoomRelayServerMoveStart;
		#define Func_SS_RoomRelayServerMoveStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)

		virtual bool RoomRelayServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)
		{
			if (!RoomRelayServerMoveStart) return false;
			return RoomRelayServerMoveStart(remote, pkOption, userRemote, moveServerIP, moveServerPort, param);
		}
		#define Stub_SS_RoomRelayServerMoveStart_override bool RoomRelayServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param) override 
		#define Stub_SS_RoomRelayServerMoveStart(ClassName) bool ClassName::RoomRelayServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)> RelayRequestOutRoom;
		#define Func_SS_RelayRequestOutRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		virtual bool RelayRequestOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)
		{
			if (!RelayRequestOutRoom) return false;
			return RelayRequestOutRoom(remote, pkOption, userRemote);
		}
		#define Stub_SS_RelayRequestOutRoom_override bool RelayRequestOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) override 
		#define Stub_SS_RelayRequestOutRoom(ClassName) bool ClassName::RelayRequestOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultOut)> RelayResponseOutRoom;
		#define Func_SS_RelayResponseOutRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultOut)

		virtual bool RelayResponseOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultOut)
		{
			if (!RelayResponseOutRoom) return false;
			return RelayResponseOutRoom(remote, pkOption, userRemote, resultOut);
		}
		#define Stub_SS_RelayResponseOutRoom_override bool RelayResponseOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultOut) override 
		#define Stub_SS_RelayResponseOutRoom(ClassName) bool ClassName::RelayResponseOutRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultOut)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)> RelayRequestMoveRoom;
		#define Func_SS_RelayRequestMoveRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		virtual bool RelayRequestMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)
		{
			if (!RelayRequestMoveRoom) return false;
			return RelayRequestMoveRoom(remote, pkOption, userRemote);
		}
		#define Stub_SS_RelayRequestMoveRoom_override bool RelayRequestMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote) override 
		#define Stub_SS_RelayRequestMoveRoom(ClassName) bool ClassName::RelayRequestMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)> RelayResponseMoveRoom;
		#define Func_SS_RelayResponseMoveRoom (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)

		virtual bool RelayResponseMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)
		{
			if (!RelayResponseMoveRoom) return false;
			return RelayResponseMoveRoom(remote, pkOption, userRemote, resultMove, errorMessage);
		}
		#define Stub_SS_RelayResponseMoveRoom_override bool RelayResponseMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage) override 
		#define Stub_SS_RelayResponseMoveRoom(ClassName) bool ClassName::RelayResponseMoveRoom_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameRoomIn;
		#define Func_SS_RelayGameRoomIn (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameRoomIn) return false;
			return RelayGameRoomIn(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameRoomIn_override bool RelayGameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameRoomIn(ClassName) bool ClassName::RelayGameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameReady;
		#define Func_SS_RelayGameReady (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameReady) return false;
			return RelayGameReady(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameReady_override bool RelayGameReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameReady(ClassName) bool ClassName::RelayGameReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameSelectOrder;
		#define Func_SS_RelayGameSelectOrder (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameSelectOrder) return false;
			return RelayGameSelectOrder(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameSelectOrder_override bool RelayGameSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameSelectOrder(ClassName) bool ClassName::RelayGameSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameDistributedEnd;
		#define Func_SS_RelayGameDistributedEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameDistributedEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameDistributedEnd) return false;
			return RelayGameDistributedEnd(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameDistributedEnd_override bool RelayGameDistributedEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameDistributedEnd(ClassName) bool ClassName::RelayGameDistributedEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameActionPutCard;
		#define Func_SS_RelayGameActionPutCard (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameActionPutCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameActionPutCard) return false;
			return RelayGameActionPutCard(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameActionPutCard_override bool RelayGameActionPutCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameActionPutCard(ClassName) bool ClassName::RelayGameActionPutCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameActionFlipBomb;
		#define Func_SS_RelayGameActionFlipBomb (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameActionFlipBomb_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameActionFlipBomb) return false;
			return RelayGameActionFlipBomb(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameActionFlipBomb_override bool RelayGameActionFlipBomb_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameActionFlipBomb(ClassName) bool ClassName::RelayGameActionFlipBomb_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameActionChooseCard;
		#define Func_SS_RelayGameActionChooseCard (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameActionChooseCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameActionChooseCard) return false;
			return RelayGameActionChooseCard(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameActionChooseCard_override bool RelayGameActionChooseCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameActionChooseCard(ClassName) bool ClassName::RelayGameActionChooseCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameSelectKookjin;
		#define Func_SS_RelayGameSelectKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameSelectKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameSelectKookjin) return false;
			return RelayGameSelectKookjin(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameSelectKookjin_override bool RelayGameSelectKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameSelectKookjin(ClassName) bool ClassName::RelayGameSelectKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameSelectGoStop;
		#define Func_SS_RelayGameSelectGoStop (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameSelectGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameSelectGoStop) return false;
			return RelayGameSelectGoStop(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameSelectGoStop_override bool RelayGameSelectGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameSelectGoStop(ClassName) bool ClassName::RelayGameSelectGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGameSelectPush;
		#define Func_SS_RelayGameSelectPush (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGameSelectPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGameSelectPush) return false;
			return RelayGameSelectPush(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGameSelectPush_override bool RelayGameSelectPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGameSelectPush(ClassName) bool ClassName::RelayGameSelectPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayGamePractice;
		#define Func_SS_RelayGamePractice (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayGamePractice_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayGamePractice) return false;
			return RelayGamePractice(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayGamePractice_override bool RelayGamePractice_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayGamePractice(ClassName) bool ClassName::RelayGamePractice_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result)> GameRelayRoomIn;
		#define Func_SS_GameRelayRoomIn (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result)

		virtual bool GameRelayRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result)
		{
			if (!GameRelayRoomIn) return false;
			return GameRelayRoomIn(remote, pkOption, userRemote, result);
		}
		#define Stub_SS_GameRelayRoomIn_override bool GameRelayRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result) override 
		#define Stub_SS_GameRelayRoomIn(ClassName) bool ClassName::GameRelayRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayRequestReady;
		#define Func_SS_GameRelayRequestReady (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayRequestReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayRequestReady) return false;
			return GameRelayRequestReady(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayRequestReady_override bool GameRelayRequestReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayRequestReady(ClassName) bool ClassName::GameRelayRequestReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayStart;
		#define Func_SS_GameRelayStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayStart) return false;
			return GameRelayStart(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayStart_override bool GameRelayStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayStart(ClassName) bool ClassName::GameRelayStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayRequestSelectOrder;
		#define Func_SS_GameRelayRequestSelectOrder (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayRequestSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayRequestSelectOrder) return false;
			return GameRelayRequestSelectOrder(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayRequestSelectOrder_override bool GameRelayRequestSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayRequestSelectOrder(ClassName) bool ClassName::GameRelayRequestSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayOrderEnd;
		#define Func_SS_GameRelayOrderEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayOrderEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayOrderEnd) return false;
			return GameRelayOrderEnd(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayOrderEnd_override bool GameRelayOrderEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayOrderEnd(ClassName) bool ClassName::GameRelayOrderEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayDistributedStart;
		#define Func_SS_GameRelayDistributedStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayDistributedStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayDistributedStart) return false;
			return GameRelayDistributedStart(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayDistributedStart_override bool GameRelayDistributedStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayDistributedStart(ClassName) bool ClassName::GameRelayDistributedStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayFloorHasBonus;
		#define Func_SS_GameRelayFloorHasBonus (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayFloorHasBonus_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayFloorHasBonus) return false;
			return GameRelayFloorHasBonus(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayFloorHasBonus_override bool GameRelayFloorHasBonus_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayFloorHasBonus(ClassName) bool ClassName::GameRelayFloorHasBonus_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayTurnStart;
		#define Func_SS_GameRelayTurnStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayTurnStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayTurnStart) return false;
			return GameRelayTurnStart(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayTurnStart_override bool GameRelayTurnStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayTurnStart(ClassName) bool ClassName::GameRelayTurnStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelaySelectCardResult;
		#define Func_SS_GameRelaySelectCardResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelaySelectCardResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelaySelectCardResult) return false;
			return GameRelaySelectCardResult(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelaySelectCardResult_override bool GameRelaySelectCardResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelaySelectCardResult(ClassName) bool ClassName::GameRelaySelectCardResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayFlipDeckResult;
		#define Func_SS_GameRelayFlipDeckResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayFlipDeckResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayFlipDeckResult) return false;
			return GameRelayFlipDeckResult(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayFlipDeckResult_override bool GameRelayFlipDeckResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayFlipDeckResult(ClassName) bool ClassName::GameRelayFlipDeckResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayTurnResult;
		#define Func_SS_GameRelayTurnResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayTurnResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayTurnResult) return false;
			return GameRelayTurnResult(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayTurnResult_override bool GameRelayTurnResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayTurnResult(ClassName) bool ClassName::GameRelayTurnResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayUserInfo;
		#define Func_SS_GameRelayUserInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayUserInfo) return false;
			return GameRelayUserInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayUserInfo_override bool GameRelayUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayUserInfo(ClassName) bool ClassName::GameRelayUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayNotifyIndex;
		#define Func_SS_GameRelayNotifyIndex (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayNotifyIndex_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayNotifyIndex) return false;
			return GameRelayNotifyIndex(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayNotifyIndex_override bool GameRelayNotifyIndex_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayNotifyIndex(ClassName) bool ClassName::GameRelayNotifyIndex_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayNotifyStat;
		#define Func_SS_GameRelayNotifyStat (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayNotifyStat_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayNotifyStat) return false;
			return GameRelayNotifyStat(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayNotifyStat_override bool GameRelayNotifyStat_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayNotifyStat(ClassName) bool ClassName::GameRelayNotifyStat_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayRequestKookjin;
		#define Func_SS_GameRelayRequestKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayRequestKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayRequestKookjin) return false;
			return GameRelayRequestKookjin(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayRequestKookjin_override bool GameRelayRequestKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayRequestKookjin(ClassName) bool ClassName::GameRelayRequestKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayNotifyKookjin;
		#define Func_SS_GameRelayNotifyKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayNotifyKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayNotifyKookjin) return false;
			return GameRelayNotifyKookjin(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayNotifyKookjin_override bool GameRelayNotifyKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayNotifyKookjin(ClassName) bool ClassName::GameRelayNotifyKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayRequestGoStop;
		#define Func_SS_GameRelayRequestGoStop (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayRequestGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayRequestGoStop) return false;
			return GameRelayRequestGoStop(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayRequestGoStop_override bool GameRelayRequestGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayRequestGoStop(ClassName) bool ClassName::GameRelayRequestGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayNotifyGoStop;
		#define Func_SS_GameRelayNotifyGoStop (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayNotifyGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayNotifyGoStop) return false;
			return GameRelayNotifyGoStop(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayNotifyGoStop_override bool GameRelayNotifyGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayNotifyGoStop(ClassName) bool ClassName::GameRelayNotifyGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayMoveKookjin;
		#define Func_SS_GameRelayMoveKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayMoveKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayMoveKookjin) return false;
			return GameRelayMoveKookjin(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayMoveKookjin_override bool GameRelayMoveKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayMoveKookjin(ClassName) bool ClassName::GameRelayMoveKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayEventStart;
		#define Func_SS_GameRelayEventStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayEventStart) return false;
			return GameRelayEventStart(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayEventStart_override bool GameRelayEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayEventStart(ClassName) bool ClassName::GameRelayEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayEventInfo;
		#define Func_SS_GameRelayEventInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayEventInfo) return false;
			return GameRelayEventInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayEventInfo_override bool GameRelayEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayEventInfo(ClassName) bool ClassName::GameRelayEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayOver;
		#define Func_SS_GameRelayOver (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayOver_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayOver) return false;
			return GameRelayOver(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayOver_override bool GameRelayOver_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayOver(ClassName) bool ClassName::GameRelayOver_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayRequestPush;
		#define Func_SS_GameRelayRequestPush (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayRequestPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayRequestPush) return false;
			return GameRelayRequestPush(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayRequestPush_override bool GameRelayRequestPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayRequestPush(ClassName) bool ClassName::GameRelayRequestPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)> GameRelayResponseRoomMove;
		#define Func_SS_GameRelayResponseRoomMove (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)

		virtual bool GameRelayResponseRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)
		{
			if (!GameRelayResponseRoomMove) return false;
			return GameRelayResponseRoomMove(remote, pkOption, userRemote, resultMove, errorMessage);
		}
		#define Stub_SS_GameRelayResponseRoomMove_override bool GameRelayResponseRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage) override 
		#define Stub_SS_GameRelayResponseRoomMove(ClassName) bool ClassName::GameRelayResponseRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayPracticeEnd;
		#define Func_SS_GameRelayPracticeEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayPracticeEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayPracticeEnd) return false;
			return GameRelayPracticeEnd(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayPracticeEnd_override bool GameRelayPracticeEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayPracticeEnd(ClassName) bool ClassName::GameRelayPracticeEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &permissionOut)> GameRelayResponseRoomOut;
		#define Func_SS_GameRelayResponseRoomOut (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &permissionOut)

		virtual bool GameRelayResponseRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &permissionOut)
		{
			if (!GameRelayResponseRoomOut) return false;
			return GameRelayResponseRoomOut(remote, pkOption, userRemote, permissionOut);
		}
		#define Stub_SS_GameRelayResponseRoomOut_override bool GameRelayResponseRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &permissionOut) override 
		#define Stub_SS_GameRelayResponseRoomOut(ClassName) bool ClassName::GameRelayResponseRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &permissionOut)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayKickUser;
		#define Func_SS_GameRelayKickUser (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayKickUser) return false;
			return GameRelayKickUser(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayKickUser_override bool GameRelayKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayKickUser(ClassName) bool ClassName::GameRelayKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayRoomInfo;
		#define Func_SS_GameRelayRoomInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayRoomInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayRoomInfo) return false;
			return GameRelayRoomInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayRoomInfo_override bool GameRelayRoomInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayRoomInfo(ClassName) bool ClassName::GameRelayRoomInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayUserOut;
		#define Func_SS_GameRelayUserOut (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayUserOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayUserOut) return false;
			return GameRelayUserOut(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayUserOut_override bool GameRelayUserOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayUserOut(ClassName) bool ClassName::GameRelayUserOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayObserveInfo;
		#define Func_SS_GameRelayObserveInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayObserveInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayObserveInfo) return false;
			return GameRelayObserveInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayObserveInfo_override bool GameRelayObserveInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayObserveInfo(ClassName) bool ClassName::GameRelayObserveInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayNotifyMessage;
		#define Func_SS_GameRelayNotifyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayNotifyMessage) return false;
			return GameRelayNotifyMessage(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayNotifyMessage_override bool GameRelayNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayNotifyMessage(ClassName) bool ClassName::GameRelayNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayNotifyJackpotInfo;
		#define Func_SS_GameRelayNotifyJackpotInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayNotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayNotifyJackpotInfo) return false;
			return GameRelayNotifyJackpotInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayNotifyJackpotInfo_override bool GameRelayNotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayNotifyJackpotInfo(ClassName) bool ClassName::GameRelayNotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayRequestLobbyEventInfo;
		#define Func_SS_RelayRequestLobbyEventInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayRequestLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayRequestLobbyEventInfo) return false;
			return RelayRequestLobbyEventInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayRequestLobbyEventInfo_override bool RelayRequestLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayRequestLobbyEventInfo(ClassName) bool ClassName::RelayRequestLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> LobbyRelayResponseLobbyEventInfo;
		#define Func_SS_LobbyRelayResponseLobbyEventInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool LobbyRelayResponseLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!LobbyRelayResponseLobbyEventInfo) return false;
			return LobbyRelayResponseLobbyEventInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_LobbyRelayResponseLobbyEventInfo_override bool LobbyRelayResponseLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_LobbyRelayResponseLobbyEventInfo(ClassName) bool ClassName::LobbyRelayResponseLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> RelayRequestLobbyEventParticipate;
		#define Func_SS_RelayRequestLobbyEventParticipate (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool RelayRequestLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!RelayRequestLobbyEventParticipate) return false;
			return RelayRequestLobbyEventParticipate(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_RelayRequestLobbyEventParticipate_override bool RelayRequestLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_RelayRequestLobbyEventParticipate(ClassName) bool ClassName::RelayRequestLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> LobbyRelayResponseLobbyEventParticipate;
		#define Func_SS_LobbyRelayResponseLobbyEventParticipate (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool LobbyRelayResponseLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!LobbyRelayResponseLobbyEventParticipate) return false;
			return LobbyRelayResponseLobbyEventParticipate(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_LobbyRelayResponseLobbyEventParticipate_override bool LobbyRelayResponseLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_LobbyRelayResponseLobbyEventParticipate(ClassName) bool ClassName::LobbyRelayResponseLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)> GameRelayResponseRoomMissionInfo;
		#define Func_SS_GameRelayResponseRoomMissionInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		virtual bool GameRelayResponseRoomMissionInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)
		{
			if (!GameRelayResponseRoomMissionInfo) return false;
			return GameRelayResponseRoomMissionInfo(remote, pkOption, userRemote, data);
		}
		#define Stub_SS_GameRelayResponseRoomMissionInfo_override bool GameRelayResponseRoomMissionInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRelayResponseRoomMissionInfo(ClassName) bool ClassName::GameRelayResponseRoomMissionInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)> ServerMoveStart;
		#define Func_SS_ServerMoveStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)

		virtual bool ServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)
		{
			if (!ServerMoveStart) return false;
			return ServerMoveStart(remote, pkOption, moveServerIP, moveServerPort, param);
		}
		#define Stub_SS_ServerMoveStart_override bool ServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param) override 
		#define Stub_SS_ServerMoveStart(ClassName) bool ClassName::ServerMoveStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &Moved)> ServerMoveEnd;
		#define Func_SS_ServerMoveEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &Moved)

		virtual bool ServerMoveEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &Moved)
		{
			if (!ServerMoveEnd) return false;
			return ServerMoveEnd(remote, pkOption, Moved);
		}
		#define Stub_SS_ServerMoveEnd_override bool ServerMoveEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &Moved) override 
		#define Stub_SS_ServerMoveEnd(ClassName) bool ClassName::ServerMoveEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &Moved)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &nickname, const string &key, const byte &resultType)> ResponseLauncherLogin;
		#define Func_SS_ResponseLauncherLogin (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &nickname, const string &key, const byte &resultType)

		virtual bool ResponseLauncherLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &nickname, const string &key, const byte &resultType)
		{
			if (!ResponseLauncherLogin) return false;
			return ResponseLauncherLogin(remote, pkOption, result, nickname, key, resultType);
		}
		#define Stub_SS_ResponseLauncherLogin_override bool ResponseLauncherLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &nickname, const string &key, const byte &resultType) override 
		#define Stub_SS_ResponseLauncherLogin(ClassName) bool ClassName::ResponseLauncherLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &nickname, const string &key, const byte &resultType)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption)> ResponseLauncherLogout;
		#define Func_SS_ResponseLauncherLogout (Zero::RemoteID remote, Zero::CPackOption pkOption)

		virtual bool ResponseLauncherLogout_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)
		{
			if (!ResponseLauncherLogout) return false;
			return ResponseLauncherLogout(remote, pkOption);
		}
		#define Stub_SS_ResponseLauncherLogout_override bool ResponseLauncherLogout_Call(Zero::RemoteID remote, Zero::CPackOption pkOption) override 
		#define Stub_SS_ResponseLauncherLogout(ClassName) bool ClassName::ResponseLauncherLogout_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)> ResponseLoginKey;
		#define Func_SS_ResponseLoginKey (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)

		virtual bool ResponseLoginKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)
		{
			if (!ResponseLoginKey) return false;
			return ResponseLoginKey(remote, pkOption, result, resultMessage);
		}
		#define Stub_SS_ResponseLoginKey_override bool ResponseLoginKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage) override 
		#define Stub_SS_ResponseLoginKey(ClassName) bool ClassName::ResponseLoginKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &key)> ResponseLobbyKey;
		#define Func_SS_ResponseLobbyKey (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &key)

		virtual bool ResponseLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &key)
		{
			if (!ResponseLobbyKey) return false;
			return ResponseLobbyKey(remote, pkOption, key);
		}
		#define Stub_SS_ResponseLobbyKey_override bool ResponseLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &key) override 
		#define Stub_SS_ResponseLobbyKey(ClassName) bool ClassName::ResponseLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &key)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)> ResponseLogin;
		#define Func_SS_ResponseLogin (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)

		virtual bool ResponseLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)
		{
			if (!ResponseLogin) return false;
			return ResponseLogin(remote, pkOption, result, resultMessage);
		}
		#define Stub_SS_ResponseLogin_override bool ResponseLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage) override 
		#define Stub_SS_ResponseLogin(ClassName) bool ClassName::ResponseLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &lobbyList)> NotifyLobbyList;
		#define Func_SS_NotifyLobbyList (Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &lobbyList)

		virtual bool NotifyLobbyList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &lobbyList)
		{
			if (!NotifyLobbyList) return false;
			return NotifyLobbyList(remote, pkOption, lobbyList);
		}
		#define Stub_SS_NotifyLobbyList_override bool NotifyLobbyList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &lobbyList) override 
		#define Stub_SS_NotifyLobbyList(ClassName) bool ClassName::NotifyLobbyList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &lobbyList)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.LobbyUserInfo &userInfo)> NotifyUserInfo;
		#define Func_SS_NotifyUserInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.LobbyUserInfo &userInfo)

		virtual bool NotifyUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.LobbyUserInfo &userInfo)
		{
			if (!NotifyUserInfo) return false;
			return NotifyUserInfo(remote, pkOption, userInfo);
		}
		#define Stub_SS_NotifyUserInfo_override bool NotifyUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.LobbyUserInfo &userInfo) override 
		#define Stub_SS_NotifyUserInfo(ClassName) bool ClassName::NotifyUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.LobbyUserInfo &userInfo)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)> NotifyUserList;
		#define Func_SS_NotifyUserList (Zero::RemoteID remote, Zero::CPackOption pkOption, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)

		virtual bool NotifyUserList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)
		{
			if (!NotifyUserList) return false;
			return NotifyUserList(remote, pkOption, lobbyUserInfos, lobbyFriendList);
		}
		#define Stub_SS_NotifyUserList_override bool NotifyUserList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList) override 
		#define Stub_SS_NotifyUserList(ClassName) bool ClassName::NotifyUserList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const List<string> &lobbyFriendList)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)> NotifyRoomList;
		#define Func_SS_NotifyRoomList (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)

		virtual bool NotifyRoomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)
		{
			if (!NotifyRoomList) return false;
			return NotifyRoomList(remote, pkOption, channelID, roomInfos);
		}
		#define Stub_SS_NotifyRoomList_override bool NotifyRoomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos) override 
		#define Stub_SS_NotifyRoomList(ClassName) bool ClassName::NotifyRoomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &channelID, const List<Rmi.Marshaler.RoomInfo> &roomInfos)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)> ResponseChannelMove;
		#define Func_SS_ResponseChannelMove (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)

		virtual bool ResponseChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)
		{
			if (!ResponseChannelMove) return false;
			return ResponseChannelMove(remote, pkOption, chanID);
		}
		#define Stub_SS_ResponseChannelMove_override bool ResponseChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID) override 
		#define Stub_SS_ResponseChannelMove(ClassName) bool ClassName::ResponseChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &message)> ResponseLobbyMessage;
		#define Func_SS_ResponseLobbyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &message)

		virtual bool ResponseLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &message)
		{
			if (!ResponseLobbyMessage) return false;
			return ResponseLobbyMessage(remote, pkOption, message);
		}
		#define Stub_SS_ResponseLobbyMessage_override bool ResponseLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &message) override 
		#define Stub_SS_ResponseLobbyMessage(ClassName) bool ClassName::ResponseLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &message)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const int &resultType)> ResponseBank;
		#define Func_SS_ResponseBank (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const int &resultType)

		virtual bool ResponseBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const int &resultType)
		{
			if (!ResponseBank) return false;
			return ResponseBank(remote, pkOption, result, resultType);
		}
		#define Stub_SS_ResponseBank_override bool ResponseBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const int &resultType) override 
		#define Stub_SS_ResponseBank(ClassName) bool ClassName::ResponseBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const int &resultType)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)> NotifyJackpotInfo;
		#define Func_SS_NotifyJackpotInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)

		virtual bool NotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)
		{
			if (!NotifyJackpotInfo) return false;
			return NotifyJackpotInfo(remote, pkOption, jackpot);
		}
		#define Stub_SS_NotifyJackpotInfo_override bool NotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot) override 
		#define Stub_SS_NotifyJackpotInfo(ClassName) bool ClassName::NotifyJackpotInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)> NotifyLobbyMessage;
		#define Func_SS_NotifyLobbyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)

		virtual bool NotifyLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)
		{
			if (!NotifyLobbyMessage) return false;
			return NotifyLobbyMessage(remote, pkOption, type, message, period);
		}
		#define Stub_SS_NotifyLobbyMessage_override bool NotifyLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period) override 
		#define Stub_SS_NotifyLobbyMessage(ClassName) bool ClassName::NotifyLobbyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)> GameRoomIn;
		#define Func_SS_GameRoomIn (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)

		virtual bool GameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)
		{
			if (!GameRoomIn) return false;
			return GameRoomIn(remote, pkOption, result);
		}
		#define Stub_SS_GameRoomIn_override bool GameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result) override 
		#define Stub_SS_GameRoomIn(ClassName) bool ClassName::GameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRequestReady;
		#define Func_SS_GameRequestReady (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRequestReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRequestReady) return false;
			return GameRequestReady(remote, pkOption, data);
		}
		#define Stub_SS_GameRequestReady_override bool GameRequestReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRequestReady(ClassName) bool ClassName::GameRequestReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameStart;
		#define Func_SS_GameStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameStart) return false;
			return GameStart(remote, pkOption, data);
		}
		#define Stub_SS_GameStart_override bool GameStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameStart(ClassName) bool ClassName::GameStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRequestSelectOrder;
		#define Func_SS_GameRequestSelectOrder (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRequestSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRequestSelectOrder) return false;
			return GameRequestSelectOrder(remote, pkOption, data);
		}
		#define Stub_SS_GameRequestSelectOrder_override bool GameRequestSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRequestSelectOrder(ClassName) bool ClassName::GameRequestSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameOrderEnd;
		#define Func_SS_GameOrderEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameOrderEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameOrderEnd) return false;
			return GameOrderEnd(remote, pkOption, data);
		}
		#define Stub_SS_GameOrderEnd_override bool GameOrderEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameOrderEnd(ClassName) bool ClassName::GameOrderEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameDistributedStart;
		#define Func_SS_GameDistributedStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameDistributedStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameDistributedStart) return false;
			return GameDistributedStart(remote, pkOption, data);
		}
		#define Stub_SS_GameDistributedStart_override bool GameDistributedStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameDistributedStart(ClassName) bool ClassName::GameDistributedStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameFloorHasBonus;
		#define Func_SS_GameFloorHasBonus (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameFloorHasBonus_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameFloorHasBonus) return false;
			return GameFloorHasBonus(remote, pkOption, data);
		}
		#define Stub_SS_GameFloorHasBonus_override bool GameFloorHasBonus_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameFloorHasBonus(ClassName) bool ClassName::GameFloorHasBonus_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameTurnStart;
		#define Func_SS_GameTurnStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameTurnStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameTurnStart) return false;
			return GameTurnStart(remote, pkOption, data);
		}
		#define Stub_SS_GameTurnStart_override bool GameTurnStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameTurnStart(ClassName) bool ClassName::GameTurnStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameSelectCardResult;
		#define Func_SS_GameSelectCardResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameSelectCardResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameSelectCardResult) return false;
			return GameSelectCardResult(remote, pkOption, data);
		}
		#define Stub_SS_GameSelectCardResult_override bool GameSelectCardResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameSelectCardResult(ClassName) bool ClassName::GameSelectCardResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameFlipDeckResult;
		#define Func_SS_GameFlipDeckResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameFlipDeckResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameFlipDeckResult) return false;
			return GameFlipDeckResult(remote, pkOption, data);
		}
		#define Stub_SS_GameFlipDeckResult_override bool GameFlipDeckResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameFlipDeckResult(ClassName) bool ClassName::GameFlipDeckResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameTurnResult;
		#define Func_SS_GameTurnResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameTurnResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameTurnResult) return false;
			return GameTurnResult(remote, pkOption, data);
		}
		#define Stub_SS_GameTurnResult_override bool GameTurnResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameTurnResult(ClassName) bool ClassName::GameTurnResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameUserInfo;
		#define Func_SS_GameUserInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameUserInfo) return false;
			return GameUserInfo(remote, pkOption, data);
		}
		#define Stub_SS_GameUserInfo_override bool GameUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameUserInfo(ClassName) bool ClassName::GameUserInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameNotifyIndex;
		#define Func_SS_GameNotifyIndex (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameNotifyIndex_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameNotifyIndex) return false;
			return GameNotifyIndex(remote, pkOption, data);
		}
		#define Stub_SS_GameNotifyIndex_override bool GameNotifyIndex_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameNotifyIndex(ClassName) bool ClassName::GameNotifyIndex_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameNotifyStat;
		#define Func_SS_GameNotifyStat (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameNotifyStat_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameNotifyStat) return false;
			return GameNotifyStat(remote, pkOption, data);
		}
		#define Stub_SS_GameNotifyStat_override bool GameNotifyStat_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameNotifyStat(ClassName) bool ClassName::GameNotifyStat_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRequestKookjin;
		#define Func_SS_GameRequestKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRequestKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRequestKookjin) return false;
			return GameRequestKookjin(remote, pkOption, data);
		}
		#define Stub_SS_GameRequestKookjin_override bool GameRequestKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRequestKookjin(ClassName) bool ClassName::GameRequestKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameNotifyKookjin;
		#define Func_SS_GameNotifyKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameNotifyKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameNotifyKookjin) return false;
			return GameNotifyKookjin(remote, pkOption, data);
		}
		#define Stub_SS_GameNotifyKookjin_override bool GameNotifyKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameNotifyKookjin(ClassName) bool ClassName::GameNotifyKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRequestGoStop;
		#define Func_SS_GameRequestGoStop (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRequestGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRequestGoStop) return false;
			return GameRequestGoStop(remote, pkOption, data);
		}
		#define Stub_SS_GameRequestGoStop_override bool GameRequestGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRequestGoStop(ClassName) bool ClassName::GameRequestGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameNotifyGoStop;
		#define Func_SS_GameNotifyGoStop (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameNotifyGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameNotifyGoStop) return false;
			return GameNotifyGoStop(remote, pkOption, data);
		}
		#define Stub_SS_GameNotifyGoStop_override bool GameNotifyGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameNotifyGoStop(ClassName) bool ClassName::GameNotifyGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameMoveKookjin;
		#define Func_SS_GameMoveKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameMoveKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameMoveKookjin) return false;
			return GameMoveKookjin(remote, pkOption, data);
		}
		#define Stub_SS_GameMoveKookjin_override bool GameMoveKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameMoveKookjin(ClassName) bool ClassName::GameMoveKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameEventStart;
		#define Func_SS_GameEventStart (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameEventStart) return false;
			return GameEventStart(remote, pkOption, data);
		}
		#define Stub_SS_GameEventStart_override bool GameEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameEventStart(ClassName) bool ClassName::GameEventStart_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameEventInfo;
		#define Func_SS_GameEventInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameEventInfo) return false;
			return GameEventInfo(remote, pkOption, data);
		}
		#define Stub_SS_GameEventInfo_override bool GameEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameEventInfo(ClassName) bool ClassName::GameEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameOver;
		#define Func_SS_GameOver (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameOver_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameOver) return false;
			return GameOver(remote, pkOption, data);
		}
		#define Stub_SS_GameOver_override bool GameOver_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameOver(ClassName) bool ClassName::GameOver_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRequestPush;
		#define Func_SS_GameRequestPush (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRequestPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRequestPush) return false;
			return GameRequestPush(remote, pkOption, data);
		}
		#define Stub_SS_GameRequestPush_override bool GameRequestPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRequestPush(ClassName) bool ClassName::GameRequestPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &move, const string &errorMessage)> GameResponseRoomMove;
		#define Func_SS_GameResponseRoomMove (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &move, const string &errorMessage)

		virtual bool GameResponseRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &move, const string &errorMessage)
		{
			if (!GameResponseRoomMove) return false;
			return GameResponseRoomMove(remote, pkOption, move, errorMessage);
		}
		#define Stub_SS_GameResponseRoomMove_override bool GameResponseRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &move, const string &errorMessage) override 
		#define Stub_SS_GameResponseRoomMove(ClassName) bool ClassName::GameResponseRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &move, const string &errorMessage)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GamePracticeEnd;
		#define Func_SS_GamePracticeEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GamePracticeEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GamePracticeEnd) return false;
			return GamePracticeEnd(remote, pkOption, data);
		}
		#define Stub_SS_GamePracticeEnd_override bool GamePracticeEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GamePracticeEnd(ClassName) bool ClassName::GamePracticeEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)> GameResponseRoomOut;
		#define Func_SS_GameResponseRoomOut (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)

		virtual bool GameResponseRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)
		{
			if (!GameResponseRoomOut) return false;
			return GameResponseRoomOut(remote, pkOption, result);
		}
		#define Stub_SS_GameResponseRoomOut_override bool GameResponseRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result) override 
		#define Stub_SS_GameResponseRoomOut(ClassName) bool ClassName::GameResponseRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameKickUser;
		#define Func_SS_GameKickUser (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameKickUser) return false;
			return GameKickUser(remote, pkOption, data);
		}
		#define Stub_SS_GameKickUser_override bool GameKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameKickUser(ClassName) bool ClassName::GameKickUser_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRoomInfo;
		#define Func_SS_GameRoomInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRoomInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRoomInfo) return false;
			return GameRoomInfo(remote, pkOption, data);
		}
		#define Stub_SS_GameRoomInfo_override bool GameRoomInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRoomInfo(ClassName) bool ClassName::GameRoomInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameUserOut;
		#define Func_SS_GameUserOut (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameUserOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameUserOut) return false;
			return GameUserOut(remote, pkOption, data);
		}
		#define Stub_SS_GameUserOut_override bool GameUserOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameUserOut(ClassName) bool ClassName::GameUserOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameObserveInfo;
		#define Func_SS_GameObserveInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameObserveInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameObserveInfo) return false;
			return GameObserveInfo(remote, pkOption, data);
		}
		#define Stub_SS_GameObserveInfo_override bool GameObserveInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameObserveInfo(ClassName) bool ClassName::GameObserveInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameNotifyMessage;
		#define Func_SS_GameNotifyMessage (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameNotifyMessage) return false;
			return GameNotifyMessage(remote, pkOption, data);
		}
		#define Stub_SS_GameNotifyMessage_override bool GameNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameNotifyMessage(ClassName) bool ClassName::GameNotifyMessage_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)> ResponsePurchaseList;
		#define Func_SS_ResponsePurchaseList (Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)

		virtual bool ResponsePurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)
		{
			if (!ResponsePurchaseList) return false;
			return ResponsePurchaseList(remote, pkOption, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge);
		}
		#define Stub_SS_ResponsePurchaseList_override bool ResponsePurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge) override 
		#define Stub_SS_ResponsePurchaseList(ClassName) bool ClassName::ResponsePurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &available, const string &reason)> ResponsePurchaseAvailability;
		#define Func_SS_ResponsePurchaseAvailability (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &available, const string &reason)

		virtual bool ResponsePurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &available, const string &reason)
		{
			if (!ResponsePurchaseAvailability) return false;
			return ResponsePurchaseAvailability(remote, pkOption, available, reason);
		}
		#define Stub_SS_ResponsePurchaseAvailability_override bool ResponsePurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &available, const string &reason) override 
		#define Stub_SS_ResponsePurchaseAvailability(ClassName) bool ClassName::ResponsePurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &available, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const System.Guid &token)> ResponsePurchaseReceiptCheck;
		#define Func_SS_ResponsePurchaseReceiptCheck (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const System.Guid &token)

		virtual bool ResponsePurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const System.Guid &token)
		{
			if (!ResponsePurchaseReceiptCheck) return false;
			return ResponsePurchaseReceiptCheck(remote, pkOption, result, token);
		}
		#define Stub_SS_ResponsePurchaseReceiptCheck_override bool ResponsePurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const System.Guid &token) override 
		#define Stub_SS_ResponsePurchaseReceiptCheck(ClassName) bool ClassName::ResponsePurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const System.Guid &token)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)> ResponsePurchaseResult;
		#define Func_SS_ResponsePurchaseResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)

		virtual bool ResponsePurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)
		{
			if (!ResponsePurchaseResult) return false;
			return ResponsePurchaseResult(remote, pkOption, result, reason);
		}
		#define Stub_SS_ResponsePurchaseResult_override bool ResponsePurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason) override 
		#define Stub_SS_ResponsePurchaseResult(ClassName) bool ClassName::ResponsePurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)> ResponsePurchaseCash;
		#define Func_SS_ResponsePurchaseCash (Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)

		virtual bool ResponsePurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)
		{
			if (!ResponsePurchaseCash) return false;
			return ResponsePurchaseCash(remote, pkOption, result, reason);
		}
		#define Stub_SS_ResponsePurchaseCash_override bool ResponsePurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason) override 
		#define Stub_SS_ResponsePurchaseCash(ClassName) bool ClassName::ResponsePurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &json)> ResponseMyroomList;
		#define Func_SS_ResponseMyroomList (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &json)

		virtual bool ResponseMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &json)
		{
			if (!ResponseMyroomList) return false;
			return ResponseMyroomList(remote, pkOption, json);
		}
		#define Stub_SS_ResponseMyroomList_override bool ResponseMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &json) override 
		#define Stub_SS_ResponseMyroomList(ClassName) bool ClassName::ResponseMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &json)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid, const bool &result, const string &reason)> ResponseMyroomAction;
		#define Func_SS_ResponseMyroomAction (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid, const bool &result, const string &reason)

		virtual bool ResponseMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid, const bool &result, const string &reason)
		{
			if (!ResponseMyroomAction) return false;
			return ResponseMyroomAction(remote, pkOption, pid, result, reason);
		}
		#define Stub_SS_ResponseMyroomAction_override bool ResponseMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid, const bool &result, const string &reason) override 
		#define Stub_SS_ResponseMyroomAction(ClassName) bool ClassName::ResponseMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid, const bool &result, const string &reason)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> ResponseGameOptions;
		#define Func_SS_ResponseGameOptions (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool ResponseGameOptions_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!ResponseGameOptions) return false;
			return ResponseGameOptions(remote, pkOption, data);
		}
		#define Stub_SS_ResponseGameOptions_override bool ResponseGameOptions_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_ResponseGameOptions(ClassName) bool ClassName::ResponseGameOptions_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> ResponseLobbyEventInfo;
		#define Func_SS_ResponseLobbyEventInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool ResponseLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!ResponseLobbyEventInfo) return false;
			return ResponseLobbyEventInfo(remote, pkOption, data);
		}
		#define Stub_SS_ResponseLobbyEventInfo_override bool ResponseLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_ResponseLobbyEventInfo(ClassName) bool ClassName::ResponseLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> ResponseLobbyEventParticipate;
		#define Func_SS_ResponseLobbyEventParticipate (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool ResponseLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!ResponseLobbyEventParticipate) return false;
			return ResponseLobbyEventParticipate(remote, pkOption, data);
		}
		#define Stub_SS_ResponseLobbyEventParticipate_override bool ResponseLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_ResponseLobbyEventParticipate(ClassName) bool ClassName::ResponseLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameResponseRoomMissionInfo;
		#define Func_SS_GameResponseRoomMissionInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameResponseRoomMissionInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameResponseRoomMissionInfo) return false;
			return GameResponseRoomMissionInfo(remote, pkOption, data);
		}
		#define Stub_SS_GameResponseRoomMissionInfo_override bool GameResponseRoomMissionInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameResponseRoomMissionInfo(ClassName) bool ClassName::GameResponseRoomMissionInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption)> ServerMoveFailure;
		#define Func_SS_ServerMoveFailure (Zero::RemoteID remote, Zero::CPackOption pkOption)

		virtual bool ServerMoveFailure_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)
		{
			if (!ServerMoveFailure) return false;
			return ServerMoveFailure(remote, pkOption);
		}
		#define Stub_SS_ServerMoveFailure_override bool ServerMoveFailure_Call(Zero::RemoteID remote, Zero::CPackOption pkOption) override 
		#define Stub_SS_ServerMoveFailure(ClassName) bool ClassName::ServerMoveFailure_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &pass)> RequestLauncherLogin;
		#define Func_SS_RequestLauncherLogin (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &pass)

		virtual bool RequestLauncherLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &pass)
		{
			if (!RequestLauncherLogin) return false;
			return RequestLauncherLogin(remote, pkOption, id, pass);
		}
		#define Stub_SS_RequestLauncherLogin_override bool RequestLauncherLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &pass) override 
		#define Stub_SS_RequestLauncherLogin(ClassName) bool ClassName::RequestLauncherLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)> RequestLauncherLogout;
		#define Func_SS_RequestLauncherLogout (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)

		virtual bool RequestLauncherLogout_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)
		{
			if (!RequestLauncherLogout) return false;
			return RequestLauncherLogout(remote, pkOption, id, key);
		}
		#define Stub_SS_RequestLauncherLogout_override bool RequestLauncherLogout_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key) override 
		#define Stub_SS_RequestLauncherLogout(ClassName) bool ClassName::RequestLauncherLogout_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)> RequestLoginKey;
		#define Func_SS_RequestLoginKey (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)

		virtual bool RequestLoginKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)
		{
			if (!RequestLoginKey) return false;
			return RequestLoginKey(remote, pkOption, id, key);
		}
		#define Stub_SS_RequestLoginKey_override bool RequestLoginKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key) override 
		#define Stub_SS_RequestLoginKey(ClassName) bool ClassName::RequestLoginKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)> RequestLobbyKey;
		#define Func_SS_RequestLobbyKey (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)

		virtual bool RequestLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)
		{
			if (!RequestLobbyKey) return false;
			return RequestLobbyKey(remote, pkOption, id, key);
		}
		#define Stub_SS_RequestLobbyKey_override bool RequestLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key) override 
		#define Stub_SS_RequestLobbyKey(ClassName) bool ClassName::RequestLobbyKey_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &name, const string &pass)> RequestLogin;
		#define Func_SS_RequestLogin (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &name, const string &pass)

		virtual bool RequestLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &name, const string &pass)
		{
			if (!RequestLogin) return false;
			return RequestLogin(remote, pkOption, name, pass);
		}
		#define Stub_SS_RequestLogin_override bool RequestLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &name, const string &pass) override 
		#define Stub_SS_RequestLogin(ClassName) bool ClassName::RequestLogin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &name, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption)> RequestLobbyList;
		#define Func_SS_RequestLobbyList (Zero::RemoteID remote, Zero::CPackOption pkOption)

		virtual bool RequestLobbyList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)
		{
			if (!RequestLobbyList) return false;
			return RequestLobbyList(remote, pkOption);
		}
		#define Stub_SS_RequestLobbyList_override bool RequestLobbyList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption) override 
		#define Stub_SS_RequestLobbyList(ClassName) bool ClassName::RequestLobbyList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &lobbyName)> RequestGoLobby;
		#define Func_SS_RequestGoLobby (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &lobbyName)

		virtual bool RequestGoLobby_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &lobbyName)
		{
			if (!RequestGoLobby) return false;
			return RequestGoLobby(remote, pkOption, lobbyName);
		}
		#define Stub_SS_RequestGoLobby_override bool RequestGoLobby_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &lobbyName) override 
		#define Stub_SS_RequestGoLobby(ClassName) bool ClassName::RequestGoLobby_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &lobbyName)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption)> RequestJoinInfo;
		#define Func_SS_RequestJoinInfo (Zero::RemoteID remote, Zero::CPackOption pkOption)

		virtual bool RequestJoinInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)
		{
			if (!RequestJoinInfo) return false;
			return RequestJoinInfo(remote, pkOption);
		}
		#define Stub_SS_RequestJoinInfo_override bool RequestJoinInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption) override 
		#define Stub_SS_RequestJoinInfo(ClassName) bool ClassName::RequestJoinInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)> RequestChannelMove;
		#define Func_SS_RequestChannelMove (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)

		virtual bool RequestChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)
		{
			if (!RequestChannelMove) return false;
			return RequestChannelMove(remote, pkOption, chanID);
		}
		#define Stub_SS_RequestChannelMove_override bool RequestChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID) override 
		#define Stub_SS_RequestChannelMove(ClassName) bool ClassName::RequestChannelMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType, const string &pass)> RequestRoomMake;
		#define Func_SS_RequestRoomMake (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType, const string &pass)

		virtual bool RequestRoomMake_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType, const string &pass)
		{
			if (!RequestRoomMake) return false;
			return RequestRoomMake(remote, pkOption, chanID, betType, pass);
		}
		#define Stub_SS_RequestRoomMake_override bool RequestRoomMake_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType, const string &pass) override 
		#define Stub_SS_RequestRoomMake(ClassName) bool ClassName::RequestRoomMake_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType)> RequestRoomJoin;
		#define Func_SS_RequestRoomJoin (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType)

		virtual bool RequestRoomJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType)
		{
			if (!RequestRoomJoin) return false;
			return RequestRoomJoin(remote, pkOption, chanID, betType);
		}
		#define Stub_SS_RequestRoomJoin_override bool RequestRoomJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType) override 
		#define Stub_SS_RequestRoomJoin(ClassName) bool ClassName::RequestRoomJoin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &roomNumber, const string &pass)> RequestRoomJoinSelect;
		#define Func_SS_RequestRoomJoinSelect (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &roomNumber, const string &pass)

		virtual bool RequestRoomJoinSelect_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &roomNumber, const string &pass)
		{
			if (!RequestRoomJoinSelect) return false;
			return RequestRoomJoinSelect(remote, pkOption, chanID, roomNumber, pass);
		}
		#define Stub_SS_RequestRoomJoinSelect_override bool RequestRoomJoinSelect_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &roomNumber, const string &pass) override 
		#define Stub_SS_RequestRoomJoinSelect(ClassName) bool ClassName::RequestRoomJoinSelect_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &roomNumber, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &option, const long &money, const string &pass)> RequestBank;
		#define Func_SS_RequestBank (Zero::RemoteID remote, Zero::CPackOption pkOption, const int &option, const long &money, const string &pass)

		virtual bool RequestBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &option, const long &money, const string &pass)
		{
			if (!RequestBank) return false;
			return RequestBank(remote, pkOption, option, money, pass);
		}
		#define Stub_SS_RequestBank_override bool RequestBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &option, const long &money, const string &pass) override 
		#define Stub_SS_RequestBank(ClassName) bool ClassName::RequestBank_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &option, const long &money, const string &pass)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRoomIn;
		#define Func_SS_GameRoomIn (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRoomIn) return false;
			return GameRoomIn(remote, pkOption, data);
		}
		#define Stub_SS_GameRoomIn_override bool GameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRoomIn(ClassName) bool ClassName::GameRoomIn_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameReady;
		#define Func_SS_GameReady (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameReady) return false;
			return GameReady(remote, pkOption, data);
		}
		#define Stub_SS_GameReady_override bool GameReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameReady(ClassName) bool ClassName::GameReady_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameSelectOrder;
		#define Func_SS_GameSelectOrder (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameSelectOrder) return false;
			return GameSelectOrder(remote, pkOption, data);
		}
		#define Stub_SS_GameSelectOrder_override bool GameSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameSelectOrder(ClassName) bool ClassName::GameSelectOrder_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameDistributedEnd;
		#define Func_SS_GameDistributedEnd (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameDistributedEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameDistributedEnd) return false;
			return GameDistributedEnd(remote, pkOption, data);
		}
		#define Stub_SS_GameDistributedEnd_override bool GameDistributedEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameDistributedEnd(ClassName) bool ClassName::GameDistributedEnd_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameActionPutCard;
		#define Func_SS_GameActionPutCard (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameActionPutCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameActionPutCard) return false;
			return GameActionPutCard(remote, pkOption, data);
		}
		#define Stub_SS_GameActionPutCard_override bool GameActionPutCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameActionPutCard(ClassName) bool ClassName::GameActionPutCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameActionFlipBomb;
		#define Func_SS_GameActionFlipBomb (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameActionFlipBomb_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameActionFlipBomb) return false;
			return GameActionFlipBomb(remote, pkOption, data);
		}
		#define Stub_SS_GameActionFlipBomb_override bool GameActionFlipBomb_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameActionFlipBomb(ClassName) bool ClassName::GameActionFlipBomb_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameActionChooseCard;
		#define Func_SS_GameActionChooseCard (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameActionChooseCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameActionChooseCard) return false;
			return GameActionChooseCard(remote, pkOption, data);
		}
		#define Stub_SS_GameActionChooseCard_override bool GameActionChooseCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameActionChooseCard(ClassName) bool ClassName::GameActionChooseCard_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameSelectKookjin;
		#define Func_SS_GameSelectKookjin (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameSelectKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameSelectKookjin) return false;
			return GameSelectKookjin(remote, pkOption, data);
		}
		#define Stub_SS_GameSelectKookjin_override bool GameSelectKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameSelectKookjin(ClassName) bool ClassName::GameSelectKookjin_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameSelectGoStop;
		#define Func_SS_GameSelectGoStop (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameSelectGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameSelectGoStop) return false;
			return GameSelectGoStop(remote, pkOption, data);
		}
		#define Stub_SS_GameSelectGoStop_override bool GameSelectGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameSelectGoStop(ClassName) bool ClassName::GameSelectGoStop_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameSelectPush;
		#define Func_SS_GameSelectPush (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameSelectPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameSelectPush) return false;
			return GameSelectPush(remote, pkOption, data);
		}
		#define Stub_SS_GameSelectPush_override bool GameSelectPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameSelectPush(ClassName) bool ClassName::GameSelectPush_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GamePractice;
		#define Func_SS_GamePractice (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GamePractice_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GamePractice) return false;
			return GamePractice(remote, pkOption, data);
		}
		#define Stub_SS_GamePractice_override bool GamePractice_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GamePractice(ClassName) bool ClassName::GamePractice_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRoomOut;
		#define Func_SS_GameRoomOut (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRoomOut) return false;
			return GameRoomOut(remote, pkOption, data);
		}
		#define Stub_SS_GameRoomOut_override bool GameRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRoomOut(ClassName) bool ClassName::GameRoomOut_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> GameRoomMove;
		#define Func_SS_GameRoomMove (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool GameRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!GameRoomMove) return false;
			return GameRoomMove(remote, pkOption, data);
		}
		#define Stub_SS_GameRoomMove_override bool GameRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_GameRoomMove(ClassName) bool ClassName::GameRoomMove_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption)> RequestPurchaseList;
		#define Func_SS_RequestPurchaseList (Zero::RemoteID remote, Zero::CPackOption pkOption)

		virtual bool RequestPurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)
		{
			if (!RequestPurchaseList) return false;
			return RequestPurchaseList(remote, pkOption);
		}
		#define Stub_SS_RequestPurchaseList_override bool RequestPurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption) override 
		#define Stub_SS_RequestPurchaseList(ClassName) bool ClassName::RequestPurchaseList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)> RequestPurchaseAvailability;
		#define Func_SS_RequestPurchaseAvailability (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)

		virtual bool RequestPurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)
		{
			if (!RequestPurchaseAvailability) return false;
			return RequestPurchaseAvailability(remote, pkOption, pid);
		}
		#define Stub_SS_RequestPurchaseAvailability_override bool RequestPurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid) override 
		#define Stub_SS_RequestPurchaseAvailability(ClassName) bool ClassName::RequestPurchaseAvailability_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &result)> RequestPurchaseReceiptCheck;
		#define Func_SS_RequestPurchaseReceiptCheck (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &result)

		virtual bool RequestPurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &result)
		{
			if (!RequestPurchaseReceiptCheck) return false;
			return RequestPurchaseReceiptCheck(remote, pkOption, result);
		}
		#define Stub_SS_RequestPurchaseReceiptCheck_override bool RequestPurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &result) override 
		#define Stub_SS_RequestPurchaseReceiptCheck(ClassName) bool ClassName::RequestPurchaseReceiptCheck_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &result)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &token)> RequestPurchaseResult;
		#define Func_SS_RequestPurchaseResult (Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &token)

		virtual bool RequestPurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &token)
		{
			if (!RequestPurchaseResult) return false;
			return RequestPurchaseResult(remote, pkOption, token);
		}
		#define Stub_SS_RequestPurchaseResult_override bool RequestPurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &token) override 
		#define Stub_SS_RequestPurchaseResult(ClassName) bool ClassName::RequestPurchaseResult_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &token)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)> RequestPurchaseCash;
		#define Func_SS_RequestPurchaseCash (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)

		virtual bool RequestPurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)
		{
			if (!RequestPurchaseCash) return false;
			return RequestPurchaseCash(remote, pkOption, pid);
		}
		#define Stub_SS_RequestPurchaseCash_override bool RequestPurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid) override 
		#define Stub_SS_RequestPurchaseCash(ClassName) bool ClassName::RequestPurchaseCash_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption)> RequestMyroomList;
		#define Func_SS_RequestMyroomList (Zero::RemoteID remote, Zero::CPackOption pkOption)

		virtual bool RequestMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)
		{
			if (!RequestMyroomList) return false;
			return RequestMyroomList(remote, pkOption);
		}
		#define Stub_SS_RequestMyroomList_override bool RequestMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption) override 
		#define Stub_SS_RequestMyroomList(ClassName) bool ClassName::RequestMyroomList_Call(Zero::RemoteID remote, Zero::CPackOption pkOption)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)> RequestMyroomAction;
		#define Func_SS_RequestMyroomAction (Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)

		virtual bool RequestMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)
		{
			if (!RequestMyroomAction) return false;
			return RequestMyroomAction(remote, pkOption, pid);
		}
		#define Stub_SS_RequestMyroomAction_override bool RequestMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid) override 
		#define Stub_SS_RequestMyroomAction(ClassName) bool ClassName::RequestMyroomAction_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> RequestGameOptions;
		#define Func_SS_RequestGameOptions (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool RequestGameOptions_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!RequestGameOptions) return false;
			return RequestGameOptions(remote, pkOption, data);
		}
		#define Stub_SS_RequestGameOptions_override bool RequestGameOptions_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_RequestGameOptions(ClassName) bool ClassName::RequestGameOptions_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> RequestLobbyEventInfo;
		#define Func_SS_RequestLobbyEventInfo (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool RequestLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!RequestLobbyEventInfo) return false;
			return RequestLobbyEventInfo(remote, pkOption, data);
		}
		#define Stub_SS_RequestLobbyEventInfo_override bool RequestLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_RequestLobbyEventInfo(ClassName) bool ClassName::RequestLobbyEventInfo_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		std::function<bool(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)> RequestLobbyEventParticipate;
		#define Func_SS_RequestLobbyEventParticipate (Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool RequestLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)
		{
			if (!RequestLobbyEventParticipate) return false;
			return RequestLobbyEventParticipate(remote, pkOption, data);
		}
		#define Stub_SS_RequestLobbyEventParticipate_override bool RequestLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data) override 
		#define Stub_SS_RequestLobbyEventParticipate(ClassName) bool ClassName::RequestLobbyEventParticipate_Call(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data)

		virtual bool ProcessMsg(Zero::CRecvedMessage &rm) override; 

	};
}

