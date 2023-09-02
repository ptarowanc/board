// Auto created from IDLCompiler.exe
#include "SS_proxy.h"

namespace SS 
{
	bool Proxy::MasterAllShutdown(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &msg )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_MasterAllShutdown; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << msg;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::MasterNotifyP2PServerInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_MasterNotifyP2PServerInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyMakeRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.RoomInfo &roomInfo, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const string &Pass, const int &shopId )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyMakeRoom; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << roomInfo;
		Msg.m_Stream << userInfo;
		Msg.m_Stream << userID;
		Msg.m_Stream << IP;
		Msg.m_Stream << Pass;
		Msg.m_Stream << shopId;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyJoinRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const Rmi.Marshaler.LobbyUserList &userInfo, const int &userID, const string &IP, const int &shopId )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyJoinRoom; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << roomID;
		Msg.m_Stream << userInfo;
		Msg.m_Stream << userID;
		Msg.m_Stream << IP;
		Msg.m_Stream << shopId;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyOutRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &userID )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyOutRoom; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << roomID;
		Msg.m_Stream << userID;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << message;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyEventStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyEventStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << roomID;
		Msg.m_Stream << type;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyEventEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const int &type, const string &name, const long &reward )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyEventEnd; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << roomID;
		Msg.m_Stream << type;
		Msg.m_Stream << name;
		Msg.m_Stream << reward;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRoomJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRoomJackpotInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << jackpot;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRoomNotifyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRoomNotifyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << type;
		Msg.m_Stream << message;
		Msg.m_Stream << period;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRoomNotifyServermaintenance(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &release )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRoomNotifyServermaintenance; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << type;
		Msg.m_Stream << message;
		Msg.m_Stream << release;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRoomReloadServerData(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRoomReloadServerData; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << type;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRoomCalling(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const int &chanId, const System.Guid &roomId, const int &playerId )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRoomCalling; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << type;
		Msg.m_Stream << chanId;
		Msg.m_Stream << roomId;
		Msg.m_Stream << playerId;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRoomKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRoomKickUser; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userID;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyLoginKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &userID )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyLoginKickUser; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userID;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyRequestMoveRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &roomID, const ZNet.RemoteID &remoteS, const ZNet.RemoteID &userRemote, const int &userID, const long &money, const bool &ipFree, const bool &shopFree, const int &shopId, const bool &restrict )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyRequestMoveRoom; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << roomID;
		Msg.m_Stream << remoteS;
		Msg.m_Stream << userRemote;
		Msg.m_Stream << userID;
		Msg.m_Stream << money;
		Msg.m_Stream << ipFree;
		Msg.m_Stream << shopFree;
		Msg.m_Stream << shopId;
		Msg.m_Stream << restrict;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRoomResponseMoveRoom(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &makeRoom, const System.Guid &roomID, const ZNet.NetAddress &addr, const int &chanID, const ZNet.RemoteID &remoteS, const ZNet.RemoteID &userRemote, const string &message )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRoomResponseMoveRoom; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << makeRoom;
		Msg.m_Stream << roomID;
		Msg.m_Stream << addr;
		Msg.m_Stream << chanID;
		Msg.m_Stream << remoteS;
		Msg.m_Stream << userRemote;
		Msg.m_Stream << message;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ServerRequestDataSync(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &isLobby )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ServerRequestDataSync; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << isLobby;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomLobbyResponseDataSync(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomLobbyResponseDataSync; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayLobbyResponseDataSync(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayLobbyResponseDataSync; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayClientJoin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.NetAddress &addr, const ZNet.ArrByte &param )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayClientJoin; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << addr;
		Msg.m_Stream << param;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayClientLeave(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &bMoveServer )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayClientLeave; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << bMoveServer;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayCloseRemoteClient(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayCloseRemoteClient; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayServerMoveFailure(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayServerMoveFailure; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &id, const string &key )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestLobbyKey; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << id;
		Msg.m_Stream << key;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestJoinInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestJoinInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestChannelMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << chanID;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestRoomMake(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestRoomMake; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << relayID;
		Msg.m_Stream << chanID;
		Msg.m_Stream << betType;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestRoomJoin(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &betType )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestRoomJoin; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << relayID;
		Msg.m_Stream << chanID;
		Msg.m_Stream << betType;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestRoomJoinSelect(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &relayID, const int &chanID, const int &roomNumber, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestRoomJoinSelect; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << relayID;
		Msg.m_Stream << chanID;
		Msg.m_Stream << roomNumber;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &option, const long &money, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestBank; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << option;
		Msg.m_Stream << money;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestPurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestPurchaseList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestPurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestPurchaseAvailability; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << pid;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestPurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &result )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestPurchaseReceiptCheck; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << result;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestPurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Guid &token )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestPurchaseResult; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << token;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestPurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestPurchaseCash; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << pid;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestMyroomList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestMyroomAction; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << pid;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponsePurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const List<string> &Purchase_avatar, const List<string> &Purchase_card, const List<string> &Purchase_evt, const List<string> &Purchase_charge )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponsePurchaseList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << Purchase_avatar;
		Msg.m_Stream << Purchase_card;
		Msg.m_Stream << Purchase_evt;
		Msg.m_Stream << Purchase_charge;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponsePurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &available, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponsePurchaseAvailability; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << available;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponsePurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const System.Guid &token )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponsePurchaseReceiptCheck; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << result;
		Msg.m_Stream << token;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponsePurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponsePurchaseResult; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << result;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponsePurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponsePurchaseCash; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << result;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &json )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseMyroomList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << json;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &pid, const bool &result, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseMyroomAction; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << pid;
		Msg.m_Stream << result;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayServerMoveStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayServerMoveStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << moveServerIP;
		Msg.m_Stream << moveServerPort;
		Msg.m_Stream << param;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &key )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseLobbyKey; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << key;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayNotifyUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const Rmi.Marshaler.LobbyUserInfo &userInfo )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayNotifyUserInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << userInfo;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayNotifyUserList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const System.Collections.Generic.List<string> &lobbyFriendList )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayNotifyUserList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << lobbyUserInfos;
		Msg.m_Stream << lobbyFriendList;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayNotifyRoomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &channelID, const System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> &roomInfos )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayNotifyRoomList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << channelID;
		Msg.m_Stream << roomInfos;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &chanID )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseChannelMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << chanID;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &message )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseLobbyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << message;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result, const int &resultType )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseBank; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << result;
		Msg.m_Stream << resultType;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayNotifyJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const long &jackpot )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayNotifyJackpotInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << jackpot;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayNotifyLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const int &type, const string &message, const int &period )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayNotifyLobbyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << type;
		Msg.m_Stream << message;
		Msg.m_Stream << period;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RoomRelayServerMoveStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RoomRelayServerMoveStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << moveServerIP;
		Msg.m_Stream << moveServerPort;
		Msg.m_Stream << param;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestRoomOutRsvn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &IsRsvn )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestRoomOutRsvn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << IsRsvn;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestRoomOut; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayResponseRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultOut )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayResponseRoomOut; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << resultOut;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestRoomMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayResponseRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayResponseRoomMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << resultMove;
		Msg.m_Stream << errorMessage;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayGameRoomIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayGameRoomIn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayGameRequestReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayGameRequestReady; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayGameDealCardsEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayGameDealCardsEnd; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayGameActionBet(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayGameActionBet; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayGameActionChangeCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayGameActionChangeCard; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayResponseRoomOutRsvn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const byte &player_index, const bool &Rsvn )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayResponseRoomOutRsvn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << player_index;
		Msg.m_Stream << Rsvn;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayResponseRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &permissionOut )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayResponseRoomOut; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << permissionOut;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayResponseRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &resultMove, const string &errorMessage )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayResponseRoomMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << resultMove;
		Msg.m_Stream << errorMessage;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayRoomIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const bool &result )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayRoomIn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << result;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayRoomReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayRoomReady; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayDealCards(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayDealCards; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayUserIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayUserIn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelaySetBoss(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelaySetBoss; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayNotifyStat(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayNotifyStat; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayRoundStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayRoundStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayChangeTurn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayChangeTurn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayRequestBet(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayRequestBet; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayResponseBet(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayResponseBet; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayChangeRound(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayChangeRound; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayRequestChangeCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayRequestChangeCard; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayResponseChangeCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayResponseChangeCard; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayCardOpen(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayCardOpen; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayOver(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayOver; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayRoomInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayRoomInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayKickUser; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayEventInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayUserInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayUserOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayUserOut; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayEventStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayEventStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayEvent2Start(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayEvent2Start; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayEventRefresh(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayEventRefresh; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayEventEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayEventEnd; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayMileageRefresh(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayMileageRefresh; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayEventNotify(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayEventNotify; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayCurrentInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayCurrentInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayEntrySpectator(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayEntrySpectator; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayNotifyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayNotifyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRelayNotifyJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRelayNotifyJackpotInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestLobbyEventInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseLobbyEventInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RelayRequestLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RelayRequestLobbyEventParticipate; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::LobbyRelayResponseLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.RemoteID &userRemote, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_LobbyRelayResponseLobbyEventParticipate; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userRemote;
		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ServerMoveStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &moveServerIP, const ushort &moveServerPort, const ZNet.ArrByte &param )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ServerMoveStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << moveServerIP;
		Msg.m_Stream << moveServerPort;
		Msg.m_Stream << param;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ServerMoveEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &Moved )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ServerMoveEnd; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << Moved;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLauncherLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &nickname, const string &key, const byte &resultType )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLauncherLogin; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;
		Msg.m_Stream << nickname;
		Msg.m_Stream << key;
		Msg.m_Stream << resultType;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLauncherLogout(Zero::RemoteID remote, Zero::CPackOption pkOption )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLauncherLogout; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );


		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLoginKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLoginKey; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;
		Msg.m_Stream << resultMessage;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &key )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLobbyKey; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << key;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &resultMessage )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLogin; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;
		Msg.m_Stream << resultMessage;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::NotifyLobbyList(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Collections.Generic.List<string> &lobbyList )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_NotifyLobbyList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << lobbyList;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::NotifyUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const Rmi.Marshaler.LobbyUserInfo &userInfo )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_NotifyUserInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << userInfo;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::NotifyUserList(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> &lobbyUserInfos, const System.Collections.Generic.List<string> &lobbyFriendList )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_NotifyUserList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << lobbyUserInfos;
		Msg.m_Stream << lobbyFriendList;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::NotifyRoomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &channelID, const System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> &roomInfos )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_NotifyRoomList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << channelID;
		Msg.m_Stream << roomInfos;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseChannelMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << chanID;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &message )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLobbyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << message;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const int &resultType )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseBank; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;
		Msg.m_Stream << resultType;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::NotifyJackpotInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const long &jackpot )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_NotifyJackpotInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << jackpot;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::NotifyLobbyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &type, const string &message, const int &period )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_NotifyLobbyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << type;
		Msg.m_Stream << message;
		Msg.m_Stream << period;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameResponseRoomOutRsvp(Zero::RemoteID remote, Zero::CPackOption pkOption, const byte &player_index, const bool &IsRsvn )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameResponseRoomOutRsvp; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << player_index;
		Msg.m_Stream << IsRsvn;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameResponseRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameResponseRoomOut; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameResponseRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &move, const string &errorMessage )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameResponseRoomMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << move;
		Msg.m_Stream << errorMessage;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoomIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoomIn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoomReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoomReady; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameDealCards(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameDealCards; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameUserIn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameUserIn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameSetBoss(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameSetBoss; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameNotifyStat(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameNotifyStat; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoundStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoundStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameChangeTurn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameChangeTurn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRequestBet(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRequestBet; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameResponseBet(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameResponseBet; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameChangeRound(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameChangeRound; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRequestChangeCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRequestChangeCard; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameResponseChangeCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameResponseChangeCard; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameCardOpen(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameCardOpen; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameOver(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameOver; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoomInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoomInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameKickUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameKickUser; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameEventInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameUserInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameUserInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameUserOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameUserOut; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameUserOutRsvn(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameUserOutRsvn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameEventStart(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameEventStart; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameEvent2Start(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameEvent2Start; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameEventRefresh(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameEventRefresh; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameEventEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameEventEnd; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameMileageRefresh(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameMileageRefresh; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameEventNotify(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameEventNotify; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameCurrentInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameCurrentInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameEntrySpectator(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameEntrySpectator; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameNotifyMessage(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameNotifyMessage; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponsePurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Collections.Generic.List<string> &Purchase_avatar, const System.Collections.Generic.List<string> &Purchase_card, const System.Collections.Generic.List<string> &Purchase_evt, const System.Collections.Generic.List<string> &Purchase_charge )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponsePurchaseList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << Purchase_avatar;
		Msg.m_Stream << Purchase_card;
		Msg.m_Stream << Purchase_evt;
		Msg.m_Stream << Purchase_charge;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponsePurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &available, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponsePurchaseAvailability; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << available;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponsePurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const System.Guid &token )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponsePurchaseReceiptCheck; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;
		Msg.m_Stream << token;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponsePurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponsePurchaseResult; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponsePurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &result, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponsePurchaseCash; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &json )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseMyroomList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << json;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid, const bool &result, const string &reason )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseMyroomAction; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << pid;
		Msg.m_Stream << result;
		Msg.m_Stream << reason;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseGameOptions(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseGameOptions; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLobbyEventInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ResponseLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ResponseLobbyEventParticipate; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::ServerMoveFailure(Zero::RemoteID remote, Zero::CPackOption pkOption )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_ServerMoveFailure; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );


		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLauncherLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLauncherLogin; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << id;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLauncherLogout(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLauncherLogout; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << id;
		Msg.m_Stream << key;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLoginKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLoginKey; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << id;
		Msg.m_Stream << key;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLobbyKey(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &id, const string &key )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLobbyKey; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << id;
		Msg.m_Stream << key;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLogin(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &name, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLogin; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << name;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLobbyList(Zero::RemoteID remote, Zero::CPackOption pkOption )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLobbyList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );


		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestGoLobby(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &lobbyName )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestGoLobby; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << lobbyName;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestJoinInfo(Zero::RemoteID remote, Zero::CPackOption pkOption )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestJoinInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );


		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestChannelMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestChannelMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << chanID;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestRoomMake(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestRoomMake; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << chanID;
		Msg.m_Stream << betType;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestRoomJoin(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &betType )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestRoomJoin; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << chanID;
		Msg.m_Stream << betType;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestRoomJoinSelect(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &chanID, const int &roomNumber, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestRoomJoinSelect; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << chanID;
		Msg.m_Stream << roomNumber;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestBank(Zero::RemoteID remote, Zero::CPackOption pkOption, const int &option, const long &money, const string &pass )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestBank; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << option;
		Msg.m_Stream << money;
		Msg.m_Stream << pass;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoomOutRsvn(Zero::RemoteID remote, Zero::CPackOption pkOption, const bool &IsRsvn )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoomOutRsvn; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << IsRsvn;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoomOut(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoomOut; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoomMove(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoomMove; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRoomInUser(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRoomInUser; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameRequestReady(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameRequestReady; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameDealCardsEnd(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameDealCardsEnd; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameActionBet(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameActionBet; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::GameActionChangeCard(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_GameActionChangeCard; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestPurchaseList(Zero::RemoteID remote, Zero::CPackOption pkOption )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestPurchaseList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );


		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestPurchaseAvailability(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestPurchaseAvailability; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << pid;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestPurchaseReceiptCheck(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &result )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestPurchaseReceiptCheck; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << result;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestPurchaseResult(Zero::RemoteID remote, Zero::CPackOption pkOption, const System.Guid &token )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestPurchaseResult; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << token;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestPurchaseCash(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestPurchaseCash; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << pid;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestMyroomList(Zero::RemoteID remote, Zero::CPackOption pkOption )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestMyroomList; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );


		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestMyroomAction(Zero::RemoteID remote, Zero::CPackOption pkOption, const string &pid )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestMyroomAction; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << pid;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestGameOptions(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestGameOptions; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLobbyEventInfo(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLobbyEventInfo; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

	bool Proxy::RequestLobbyEventParticipate(Zero::RemoteID remote, Zero::CPackOption pkOption, const ZNet.ArrByte &data )
	{
		Zero::CMessage Msg;
		Zero::PacketType msgID = (Zero::PacketType)Packet_RequestLobbyEventParticipate; 
		
		Msg.WriteStart( msgID, pkOption, 0, true );

		Msg.m_Stream << data;

		return PacketSend( remote, pkOption, Msg );
	} 

}

