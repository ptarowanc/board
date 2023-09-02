// Auto created from IDLCompiler.exe
#include "SS_stub.h"

namespace SS 
{
	bool Stub::ProcessMsg(Zero::CRecvedMessage &rm) 
	{
		Zero::CPackOption pkOption;
		Zero::CSerializer &__msg = rm.msg.m_Stream;
		Zero::RemoteID remote = rm.remote;
		Zero::PacketType nPktID = rm.msg.ReadStart(pkOption);

		switch( nPktID ) 
		{
		case Packet_MasterAllShutdown: 
			{
				string msg; __msg >> msg;

				bool bRet = MasterAllShutdown_Call( remote, pkOption, msg );
				if( bRet==false )
					m_owner->NeedImplement("MasterAllShutdown");
			} 
			break; 

		case Packet_MasterNotifyP2PServerInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = MasterNotifyP2PServerInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("MasterNotifyP2PServerInfo");
			} 
			break; 

		case Packet_RoomLobbyMakeRoom: 
			{
				Rmi.Marshaler.RoomInfo roomInfo; __msg >> roomInfo;
				Rmi.Marshaler.LobbyUserList userInfo; __msg >> userInfo;
				int userID; __msg >> userID;
				string IP; __msg >> IP;
				string Pass; __msg >> Pass;
				int shopId; __msg >> shopId;

				bool bRet = RoomLobbyMakeRoom_Call( remote, pkOption, roomInfo, userInfo, userID, IP, Pass, shopId );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyMakeRoom");
			} 
			break; 

		case Packet_RoomLobbyJoinRoom: 
			{
				System.Guid roomID; __msg >> roomID;
				Rmi.Marshaler.LobbyUserList userInfo; __msg >> userInfo;
				int userID; __msg >> userID;
				string IP; __msg >> IP;
				int shopId; __msg >> shopId;

				bool bRet = RoomLobbyJoinRoom_Call( remote, pkOption, roomID, userInfo, userID, IP, shopId );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyJoinRoom");
			} 
			break; 

		case Packet_RoomLobbyOutRoom: 
			{
				System.Guid roomID; __msg >> roomID;
				int userID; __msg >> userID;

				bool bRet = RoomLobbyOutRoom_Call( remote, pkOption, roomID, userID );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyOutRoom");
			} 
			break; 

		case Packet_RoomLobbyMessage: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string message; __msg >> message;

				bool bRet = RoomLobbyMessage_Call( remote, pkOption, userRemote, message );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyMessage");
			} 
			break; 

		case Packet_RoomLobbyEventStart: 
			{
				System.Guid roomID; __msg >> roomID;
				int type; __msg >> type;

				bool bRet = RoomLobbyEventStart_Call( remote, pkOption, roomID, type );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyEventStart");
			} 
			break; 

		case Packet_RoomLobbyEventEnd: 
			{
				System.Guid roomID; __msg >> roomID;
				int type; __msg >> type;
				string name; __msg >> name;
				long reward; __msg >> reward;

				bool bRet = RoomLobbyEventEnd_Call( remote, pkOption, roomID, type, name, reward );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyEventEnd");
			} 
			break; 

		case Packet_LobbyRoomJackpotInfo: 
			{
				long jackpot; __msg >> jackpot;

				bool bRet = LobbyRoomJackpotInfo_Call( remote, pkOption, jackpot );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRoomJackpotInfo");
			} 
			break; 

		case Packet_LobbyRoomNotifyMessage: 
			{
				int type; __msg >> type;
				string message; __msg >> message;
				int period; __msg >> period;

				bool bRet = LobbyRoomNotifyMessage_Call( remote, pkOption, type, message, period );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRoomNotifyMessage");
			} 
			break; 

		case Packet_LobbyRoomNotifyServermaintenance: 
			{
				int type; __msg >> type;
				string message; __msg >> message;
				int release; __msg >> release;

				bool bRet = LobbyRoomNotifyServermaintenance_Call( remote, pkOption, type, message, release );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRoomNotifyServermaintenance");
			} 
			break; 

		case Packet_LobbyRoomReloadServerData: 
			{
				int type; __msg >> type;

				bool bRet = LobbyRoomReloadServerData_Call( remote, pkOption, type );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRoomReloadServerData");
			} 
			break; 

		case Packet_LobbyRoomCalling: 
			{
				int type; __msg >> type;
				int chanId; __msg >> chanId;
				System.Guid roomId; __msg >> roomId;
				int playerId; __msg >> playerId;

				bool bRet = LobbyRoomCalling_Call( remote, pkOption, type, chanId, roomId, playerId );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRoomCalling");
			} 
			break; 

		case Packet_LobbyRoomKickUser: 
			{
				int userID; __msg >> userID;

				bool bRet = LobbyRoomKickUser_Call( remote, pkOption, userID );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRoomKickUser");
			} 
			break; 

		case Packet_LobbyLoginKickUser: 
			{
				int userID; __msg >> userID;

				bool bRet = LobbyLoginKickUser_Call( remote, pkOption, userID );
				if( bRet==false )
					m_owner->NeedImplement("LobbyLoginKickUser");
			} 
			break; 

		case Packet_RoomLobbyRequestMoveRoom: 
			{
				System.Guid roomID; __msg >> roomID;
				ZNet.RemoteID remoteS; __msg >> remoteS;
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int userID; __msg >> userID;
				long money; __msg >> money;
				bool ipFree; __msg >> ipFree;
				bool shopFree; __msg >> shopFree;
				int shopId; __msg >> shopId;
				bool restrict; __msg >> restrict;

				bool bRet = RoomLobbyRequestMoveRoom_Call( remote, pkOption, roomID, remoteS, userRemote, userID, money, ipFree, shopFree, shopId, restrict );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyRequestMoveRoom");
			} 
			break; 

		case Packet_LobbyRoomResponseMoveRoom: 
			{
				bool makeRoom; __msg >> makeRoom;
				System.Guid roomID; __msg >> roomID;
				ZNet.NetAddress addr; __msg >> addr;
				int chanID; __msg >> chanID;
				ZNet.RemoteID remoteS; __msg >> remoteS;
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string message; __msg >> message;

				bool bRet = LobbyRoomResponseMoveRoom_Call( remote, pkOption, makeRoom, roomID, addr, chanID, remoteS, userRemote, message );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRoomResponseMoveRoom");
			} 
			break; 

		case Packet_ServerRequestDataSync: 
			{
				bool isLobby; __msg >> isLobby;

				bool bRet = ServerRequestDataSync_Call( remote, pkOption, isLobby );
				if( bRet==false )
					m_owner->NeedImplement("ServerRequestDataSync");
			} 
			break; 

		case Packet_RoomLobbyResponseDataSync: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RoomLobbyResponseDataSync_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("RoomLobbyResponseDataSync");
			} 
			break; 

		case Packet_RelayLobbyResponseDataSync: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayLobbyResponseDataSync_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayLobbyResponseDataSync");
			} 
			break; 

		case Packet_RelayClientJoin: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.NetAddress addr; __msg >> addr;
				ZNet.ArrByte param; __msg >> param;

				bool bRet = RelayClientJoin_Call( remote, pkOption, userRemote, addr, param );
				if( bRet==false )
					m_owner->NeedImplement("RelayClientJoin");
			} 
			break; 

		case Packet_RelayClientLeave: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool bMoveServer; __msg >> bMoveServer;

				bool bRet = RelayClientLeave_Call( remote, pkOption, userRemote, bMoveServer );
				if( bRet==false )
					m_owner->NeedImplement("RelayClientLeave");
			} 
			break; 

		case Packet_RelayCloseRemoteClient: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayCloseRemoteClient_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayCloseRemoteClient");
			} 
			break; 

		case Packet_RelayServerMoveFailure: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayServerMoveFailure_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayServerMoveFailure");
			} 
			break; 

		case Packet_RelayRequestLobbyKey: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string id; __msg >> id;
				string key; __msg >> key;

				bool bRet = RelayRequestLobbyKey_Call( remote, pkOption, userRemote, id, key );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestLobbyKey");
			} 
			break; 

		case Packet_RelayRequestJoinInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayRequestJoinInfo_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestJoinInfo");
			} 
			break; 

		case Packet_RelayRequestChannelMove: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int chanID; __msg >> chanID;

				bool bRet = RelayRequestChannelMove_Call( remote, pkOption, userRemote, chanID );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestChannelMove");
			} 
			break; 

		case Packet_RelayRequestRoomMake: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int relayID; __msg >> relayID;
				int chanID; __msg >> chanID;
				int betType; __msg >> betType;
				string pass; __msg >> pass;

				bool bRet = RelayRequestRoomMake_Call( remote, pkOption, userRemote, relayID, chanID, betType, pass );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestRoomMake");
			} 
			break; 

		case Packet_RelayRequestRoomJoin: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int relayID; __msg >> relayID;
				int chanID; __msg >> chanID;
				int betType; __msg >> betType;

				bool bRet = RelayRequestRoomJoin_Call( remote, pkOption, userRemote, relayID, chanID, betType );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestRoomJoin");
			} 
			break; 

		case Packet_RelayRequestRoomJoinSelect: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int relayID; __msg >> relayID;
				int chanID; __msg >> chanID;
				int roomNumber; __msg >> roomNumber;
				string pass; __msg >> pass;

				bool bRet = RelayRequestRoomJoinSelect_Call( remote, pkOption, userRemote, relayID, chanID, roomNumber, pass );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestRoomJoinSelect");
			} 
			break; 

		case Packet_RelayRequestBank: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int option; __msg >> option;
				long money; __msg >> money;
				string pass; __msg >> pass;

				bool bRet = RelayRequestBank_Call( remote, pkOption, userRemote, option, money, pass );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestBank");
			} 
			break; 

		case Packet_RelayRequestPurchaseList: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayRequestPurchaseList_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestPurchaseList");
			} 
			break; 

		case Packet_RelayRequestPurchaseAvailability: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string pid; __msg >> pid;

				bool bRet = RelayRequestPurchaseAvailability_Call( remote, pkOption, userRemote, pid );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestPurchaseAvailability");
			} 
			break; 

		case Packet_RelayRequestPurchaseReceiptCheck: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string result; __msg >> result;

				bool bRet = RelayRequestPurchaseReceiptCheck_Call( remote, pkOption, userRemote, result );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestPurchaseReceiptCheck");
			} 
			break; 

		case Packet_RelayRequestPurchaseResult: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				System.Guid token; __msg >> token;

				bool bRet = RelayRequestPurchaseResult_Call( remote, pkOption, userRemote, token );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestPurchaseResult");
			} 
			break; 

		case Packet_RelayRequestPurchaseCash: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string pid; __msg >> pid;

				bool bRet = RelayRequestPurchaseCash_Call( remote, pkOption, userRemote, pid );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestPurchaseCash");
			} 
			break; 

		case Packet_RelayRequestMyroomList: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayRequestMyroomList_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestMyroomList");
			} 
			break; 

		case Packet_RelayRequestMyroomAction: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string pid; __msg >> pid;

				bool bRet = RelayRequestMyroomAction_Call( remote, pkOption, userRemote, pid );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestMyroomAction");
			} 
			break; 

		case Packet_LobbyRelayResponsePurchaseList: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				List<string> Purchase_avatar; __msg >> Purchase_avatar;
				List<string> Purchase_card; __msg >> Purchase_card;
				List<string> Purchase_evt; __msg >> Purchase_evt;
				List<string> Purchase_charge; __msg >> Purchase_charge;

				bool bRet = LobbyRelayResponsePurchaseList_Call( remote, pkOption, userRemote, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponsePurchaseList");
			} 
			break; 

		case Packet_LobbyRelayResponsePurchaseAvailability: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool available; __msg >> available;
				string reason; __msg >> reason;

				bool bRet = LobbyRelayResponsePurchaseAvailability_Call( remote, pkOption, userRemote, available, reason );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponsePurchaseAvailability");
			} 
			break; 

		case Packet_LobbyRelayResponsePurchaseReceiptCheck: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool result; __msg >> result;
				System.Guid token; __msg >> token;

				bool bRet = LobbyRelayResponsePurchaseReceiptCheck_Call( remote, pkOption, userRemote, result, token );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponsePurchaseReceiptCheck");
			} 
			break; 

		case Packet_LobbyRelayResponsePurchaseResult: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool result; __msg >> result;
				string reason; __msg >> reason;

				bool bRet = LobbyRelayResponsePurchaseResult_Call( remote, pkOption, userRemote, result, reason );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponsePurchaseResult");
			} 
			break; 

		case Packet_LobbyRelayResponsePurchaseCash: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool result; __msg >> result;
				string reason; __msg >> reason;

				bool bRet = LobbyRelayResponsePurchaseCash_Call( remote, pkOption, userRemote, result, reason );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponsePurchaseCash");
			} 
			break; 

		case Packet_LobbyRelayResponseMyroomList: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string json; __msg >> json;

				bool bRet = LobbyRelayResponseMyroomList_Call( remote, pkOption, userRemote, json );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseMyroomList");
			} 
			break; 

		case Packet_LobbyRelayResponseMyroomAction: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string pid; __msg >> pid;
				bool result; __msg >> result;
				string reason; __msg >> reason;

				bool bRet = LobbyRelayResponseMyroomAction_Call( remote, pkOption, userRemote, pid, result, reason );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseMyroomAction");
			} 
			break; 

		case Packet_LobbyRelayServerMoveStart: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string moveServerIP; __msg >> moveServerIP;
				ushort moveServerPort; __msg >> moveServerPort;
				ZNet.ArrByte param; __msg >> param;

				bool bRet = LobbyRelayServerMoveStart_Call( remote, pkOption, userRemote, moveServerIP, moveServerPort, param );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayServerMoveStart");
			} 
			break; 

		case Packet_LobbyRelayResponseLobbyKey: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string key; __msg >> key;

				bool bRet = LobbyRelayResponseLobbyKey_Call( remote, pkOption, userRemote, key );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseLobbyKey");
			} 
			break; 

		case Packet_LobbyRelayNotifyUserInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				Rmi.Marshaler.LobbyUserInfo userInfo; __msg >> userInfo;

				bool bRet = LobbyRelayNotifyUserInfo_Call( remote, pkOption, userRemote, userInfo );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayNotifyUserInfo");
			} 
			break; 

		case Packet_LobbyRelayNotifyUserList: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; __msg >> lobbyUserInfos;
				System.Collections.Generic.List<string> lobbyFriendList; __msg >> lobbyFriendList;

				bool bRet = LobbyRelayNotifyUserList_Call( remote, pkOption, userRemote, lobbyUserInfos, lobbyFriendList );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayNotifyUserList");
			} 
			break; 

		case Packet_LobbyRelayNotifyRoomList: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int channelID; __msg >> channelID;
				System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos; __msg >> roomInfos;

				bool bRet = LobbyRelayNotifyRoomList_Call( remote, pkOption, userRemote, channelID, roomInfos );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayNotifyRoomList");
			} 
			break; 

		case Packet_LobbyRelayResponseChannelMove: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int chanID; __msg >> chanID;

				bool bRet = LobbyRelayResponseChannelMove_Call( remote, pkOption, userRemote, chanID );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseChannelMove");
			} 
			break; 

		case Packet_LobbyRelayResponseLobbyMessage: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string message; __msg >> message;

				bool bRet = LobbyRelayResponseLobbyMessage_Call( remote, pkOption, userRemote, message );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseLobbyMessage");
			} 
			break; 

		case Packet_LobbyRelayResponseBank: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool result; __msg >> result;
				int resultType; __msg >> resultType;

				bool bRet = LobbyRelayResponseBank_Call( remote, pkOption, userRemote, result, resultType );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseBank");
			} 
			break; 

		case Packet_LobbyRelayNotifyJackpotInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				long jackpot; __msg >> jackpot;

				bool bRet = LobbyRelayNotifyJackpotInfo_Call( remote, pkOption, userRemote, jackpot );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayNotifyJackpotInfo");
			} 
			break; 

		case Packet_LobbyRelayNotifyLobbyMessage: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int type; __msg >> type;
				string message; __msg >> message;
				int period; __msg >> period;

				bool bRet = LobbyRelayNotifyLobbyMessage_Call( remote, pkOption, userRemote, type, message, period );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayNotifyLobbyMessage");
			} 
			break; 

		case Packet_RoomRelayServerMoveStart: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string moveServerIP; __msg >> moveServerIP;
				ushort moveServerPort; __msg >> moveServerPort;
				ZNet.ArrByte param; __msg >> param;

				bool bRet = RoomRelayServerMoveStart_Call( remote, pkOption, userRemote, moveServerIP, moveServerPort, param );
				if( bRet==false )
					m_owner->NeedImplement("RoomRelayServerMoveStart");
			} 
			break; 

		case Packet_RelayRequestRoomOutRsvn: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool IsRsvn; __msg >> IsRsvn;

				bool bRet = RelayRequestRoomOutRsvn_Call( remote, pkOption, userRemote, IsRsvn );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestRoomOutRsvn");
			} 
			break; 

		case Packet_RelayRequestRoomOut: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayRequestRoomOut_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestRoomOut");
			} 
			break; 

		case Packet_RelayResponseRoomOut: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool resultOut; __msg >> resultOut;

				bool bRet = RelayResponseRoomOut_Call( remote, pkOption, userRemote, resultOut );
				if( bRet==false )
					m_owner->NeedImplement("RelayResponseRoomOut");
			} 
			break; 

		case Packet_RelayRequestRoomMove: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayRequestRoomMove_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestRoomMove");
			} 
			break; 

		case Packet_RelayResponseRoomMove: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool resultMove; __msg >> resultMove;
				string errorMessage; __msg >> errorMessage;

				bool bRet = RelayResponseRoomMove_Call( remote, pkOption, userRemote, resultMove, errorMessage );
				if( bRet==false )
					m_owner->NeedImplement("RelayResponseRoomMove");
			} 
			break; 

		case Packet_RelayGameRoomIn: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameRoomIn_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameRoomIn");
			} 
			break; 

		case Packet_RelayGameRequestReady: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameRequestReady_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameRequestReady");
			} 
			break; 

		case Packet_RelayGameDealCardsEnd: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameDealCardsEnd_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameDealCardsEnd");
			} 
			break; 

		case Packet_RelayGameActionBet: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameActionBet_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameActionBet");
			} 
			break; 

		case Packet_RelayGameActionChangeCard: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameActionChangeCard_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameActionChangeCard");
			} 
			break; 

		case Packet_GameRelayResponseRoomOutRsvn: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				byte player_index; __msg >> player_index;
				bool Rsvn; __msg >> Rsvn;

				bool bRet = GameRelayResponseRoomOutRsvn_Call( remote, pkOption, userRemote, player_index, Rsvn );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayResponseRoomOutRsvn");
			} 
			break; 

		case Packet_GameRelayResponseRoomOut: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool permissionOut; __msg >> permissionOut;

				bool bRet = GameRelayResponseRoomOut_Call( remote, pkOption, userRemote, permissionOut );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayResponseRoomOut");
			} 
			break; 

		case Packet_GameRelayResponseRoomMove: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool resultMove; __msg >> resultMove;
				string errorMessage; __msg >> errorMessage;

				bool bRet = GameRelayResponseRoomMove_Call( remote, pkOption, userRemote, resultMove, errorMessage );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayResponseRoomMove");
			} 
			break; 

		case Packet_GameRelayRoomIn: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool result; __msg >> result;

				bool bRet = GameRelayRoomIn_Call( remote, pkOption, userRemote, result );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRoomIn");
			} 
			break; 

		case Packet_GameRelayRoomReady: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRoomReady_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRoomReady");
			} 
			break; 

		case Packet_GameRelayStart: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayStart_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayStart");
			} 
			break; 

		case Packet_GameRelayDealCards: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayDealCards_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayDealCards");
			} 
			break; 

		case Packet_GameRelayUserIn: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayUserIn_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayUserIn");
			} 
			break; 

		case Packet_GameRelaySetBoss: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelaySetBoss_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelaySetBoss");
			} 
			break; 

		case Packet_GameRelayNotifyStat: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayNotifyStat_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayNotifyStat");
			} 
			break; 

		case Packet_GameRelayRoundStart: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRoundStart_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRoundStart");
			} 
			break; 

		case Packet_GameRelayChangeTurn: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayChangeTurn_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayChangeTurn");
			} 
			break; 

		case Packet_GameRelayRequestBet: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRequestBet_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRequestBet");
			} 
			break; 

		case Packet_GameRelayResponseBet: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayResponseBet_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayResponseBet");
			} 
			break; 

		case Packet_GameRelayChangeRound: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayChangeRound_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayChangeRound");
			} 
			break; 

		case Packet_GameRelayRequestChangeCard: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRequestChangeCard_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRequestChangeCard");
			} 
			break; 

		case Packet_GameRelayResponseChangeCard: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayResponseChangeCard_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayResponseChangeCard");
			} 
			break; 

		case Packet_GameRelayCardOpen: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayCardOpen_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayCardOpen");
			} 
			break; 

		case Packet_GameRelayOver: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayOver_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayOver");
			} 
			break; 

		case Packet_GameRelayRoomInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRoomInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRoomInfo");
			} 
			break; 

		case Packet_GameRelayKickUser: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayKickUser_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayKickUser");
			} 
			break; 

		case Packet_GameRelayEventInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEventInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEventInfo");
			} 
			break; 

		case Packet_GameRelayUserInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayUserInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayUserInfo");
			} 
			break; 

		case Packet_GameRelayUserOut: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayUserOut_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayUserOut");
			} 
			break; 

		case Packet_GameRelayEventStart: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEventStart_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEventStart");
			} 
			break; 

		case Packet_GameRelayEvent2Start: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEvent2Start_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEvent2Start");
			} 
			break; 

		case Packet_GameRelayEventRefresh: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEventRefresh_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEventRefresh");
			} 
			break; 

		case Packet_GameRelayEventEnd: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEventEnd_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEventEnd");
			} 
			break; 

		case Packet_GameRelayMileageRefresh: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayMileageRefresh_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayMileageRefresh");
			} 
			break; 

		case Packet_GameRelayEventNotify: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEventNotify_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEventNotify");
			} 
			break; 

		case Packet_GameRelayCurrentInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayCurrentInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayCurrentInfo");
			} 
			break; 

		case Packet_GameRelayEntrySpectator: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEntrySpectator_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEntrySpectator");
			} 
			break; 

		case Packet_GameRelayNotifyMessage: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayNotifyMessage_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayNotifyMessage");
			} 
			break; 

		case Packet_GameRelayNotifyJackpotInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayNotifyJackpotInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayNotifyJackpotInfo");
			} 
			break; 

		case Packet_RelayRequestLobbyEventInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayRequestLobbyEventInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestLobbyEventInfo");
			} 
			break; 

		case Packet_LobbyRelayResponseLobbyEventInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = LobbyRelayResponseLobbyEventInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseLobbyEventInfo");
			} 
			break; 

		case Packet_RelayRequestLobbyEventParticipate: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayRequestLobbyEventParticipate_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestLobbyEventParticipate");
			} 
			break; 

		case Packet_LobbyRelayResponseLobbyEventParticipate: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = LobbyRelayResponseLobbyEventParticipate_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayResponseLobbyEventParticipate");
			} 
			break; 

		case Packet_ServerMoveStart: 
			{
				string moveServerIP; __msg >> moveServerIP;
				ushort moveServerPort; __msg >> moveServerPort;
				ZNet.ArrByte param; __msg >> param;

				bool bRet = ServerMoveStart_Call( remote, pkOption, moveServerIP, moveServerPort, param );
				if( bRet==false )
					m_owner->NeedImplement("ServerMoveStart");
			} 
			break; 

		case Packet_ServerMoveEnd: 
			{
				bool Moved; __msg >> Moved;

				bool bRet = ServerMoveEnd_Call( remote, pkOption, Moved );
				if( bRet==false )
					m_owner->NeedImplement("ServerMoveEnd");
			} 
			break; 

		case Packet_ResponseLauncherLogin: 
			{
				bool result; __msg >> result;
				string nickname; __msg >> nickname;
				string key; __msg >> key;
				byte resultType; __msg >> resultType;

				bool bRet = ResponseLauncherLogin_Call( remote, pkOption, result, nickname, key, resultType );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLauncherLogin");
			} 
			break; 

		case Packet_ResponseLauncherLogout: 
			{

				bool bRet = ResponseLauncherLogout_Call( remote, pkOption );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLauncherLogout");
			} 
			break; 

		case Packet_ResponseLoginKey: 
			{
				bool result; __msg >> result;
				string resultMessage; __msg >> resultMessage;

				bool bRet = ResponseLoginKey_Call( remote, pkOption, result, resultMessage );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLoginKey");
			} 
			break; 

		case Packet_ResponseLobbyKey: 
			{
				string key; __msg >> key;

				bool bRet = ResponseLobbyKey_Call( remote, pkOption, key );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLobbyKey");
			} 
			break; 

		case Packet_ResponseLogin: 
			{
				bool result; __msg >> result;
				string resultMessage; __msg >> resultMessage;

				bool bRet = ResponseLogin_Call( remote, pkOption, result, resultMessage );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLogin");
			} 
			break; 

		case Packet_NotifyLobbyList: 
			{
				System.Collections.Generic.List<string> lobbyList; __msg >> lobbyList;

				bool bRet = NotifyLobbyList_Call( remote, pkOption, lobbyList );
				if( bRet==false )
					m_owner->NeedImplement("NotifyLobbyList");
			} 
			break; 

		case Packet_NotifyUserInfo: 
			{
				Rmi.Marshaler.LobbyUserInfo userInfo; __msg >> userInfo;

				bool bRet = NotifyUserInfo_Call( remote, pkOption, userInfo );
				if( bRet==false )
					m_owner->NeedImplement("NotifyUserInfo");
			} 
			break; 

		case Packet_NotifyUserList: 
			{
				System.Collections.Generic.List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; __msg >> lobbyUserInfos;
				System.Collections.Generic.List<string> lobbyFriendList; __msg >> lobbyFriendList;

				bool bRet = NotifyUserList_Call( remote, pkOption, lobbyUserInfos, lobbyFriendList );
				if( bRet==false )
					m_owner->NeedImplement("NotifyUserList");
			} 
			break; 

		case Packet_NotifyRoomList: 
			{
				int channelID; __msg >> channelID;
				System.Collections.Generic.List<Rmi.Marshaler.RoomInfo> roomInfos; __msg >> roomInfos;

				bool bRet = NotifyRoomList_Call( remote, pkOption, channelID, roomInfos );
				if( bRet==false )
					m_owner->NeedImplement("NotifyRoomList");
			} 
			break; 

		case Packet_ResponseChannelMove: 
			{
				int chanID; __msg >> chanID;

				bool bRet = ResponseChannelMove_Call( remote, pkOption, chanID );
				if( bRet==false )
					m_owner->NeedImplement("ResponseChannelMove");
			} 
			break; 

		case Packet_ResponseLobbyMessage: 
			{
				string message; __msg >> message;

				bool bRet = ResponseLobbyMessage_Call( remote, pkOption, message );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLobbyMessage");
			} 
			break; 

		case Packet_ResponseBank: 
			{
				bool result; __msg >> result;
				int resultType; __msg >> resultType;

				bool bRet = ResponseBank_Call( remote, pkOption, result, resultType );
				if( bRet==false )
					m_owner->NeedImplement("ResponseBank");
			} 
			break; 

		case Packet_NotifyJackpotInfo: 
			{
				long jackpot; __msg >> jackpot;

				bool bRet = NotifyJackpotInfo_Call( remote, pkOption, jackpot );
				if( bRet==false )
					m_owner->NeedImplement("NotifyJackpotInfo");
			} 
			break; 

		case Packet_NotifyLobbyMessage: 
			{
				int type; __msg >> type;
				string message; __msg >> message;
				int period; __msg >> period;

				bool bRet = NotifyLobbyMessage_Call( remote, pkOption, type, message, period );
				if( bRet==false )
					m_owner->NeedImplement("NotifyLobbyMessage");
			} 
			break; 

		case Packet_GameResponseRoomOutRsvp: 
			{
				byte player_index; __msg >> player_index;
				bool IsRsvn; __msg >> IsRsvn;

				bool bRet = GameResponseRoomOutRsvp_Call( remote, pkOption, player_index, IsRsvn );
				if( bRet==false )
					m_owner->NeedImplement("GameResponseRoomOutRsvp");
			} 
			break; 

		case Packet_GameResponseRoomOut: 
			{
				bool result; __msg >> result;

				bool bRet = GameResponseRoomOut_Call( remote, pkOption, result );
				if( bRet==false )
					m_owner->NeedImplement("GameResponseRoomOut");
			} 
			break; 

		case Packet_GameResponseRoomMove: 
			{
				bool move; __msg >> move;
				string errorMessage; __msg >> errorMessage;

				bool bRet = GameResponseRoomMove_Call( remote, pkOption, move, errorMessage );
				if( bRet==false )
					m_owner->NeedImplement("GameResponseRoomMove");
			} 
			break; 

		case Packet_GameRoomIn: 
			{
				bool result; __msg >> result;

				bool bRet = GameRoomIn_Call( remote, pkOption, result );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomIn");
			} 
			break; 

		case Packet_GameRoomReady: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRoomReady_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomReady");
			} 
			break; 

		case Packet_GameStart: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameStart_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameStart");
			} 
			break; 

		case Packet_GameDealCards: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameDealCards_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameDealCards");
			} 
			break; 

		case Packet_GameUserIn: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameUserIn_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameUserIn");
			} 
			break; 

		case Packet_GameSetBoss: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameSetBoss_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameSetBoss");
			} 
			break; 

		case Packet_GameNotifyStat: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameNotifyStat_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameNotifyStat");
			} 
			break; 

		case Packet_GameRoundStart: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRoundStart_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRoundStart");
			} 
			break; 

		case Packet_GameChangeTurn: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameChangeTurn_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameChangeTurn");
			} 
			break; 

		case Packet_GameRequestBet: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRequestBet_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRequestBet");
			} 
			break; 

		case Packet_GameResponseBet: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameResponseBet_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameResponseBet");
			} 
			break; 

		case Packet_GameChangeRound: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameChangeRound_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameChangeRound");
			} 
			break; 

		case Packet_GameRequestChangeCard: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRequestChangeCard_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRequestChangeCard");
			} 
			break; 

		case Packet_GameResponseChangeCard: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameResponseChangeCard_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameResponseChangeCard");
			} 
			break; 

		case Packet_GameCardOpen: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameCardOpen_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameCardOpen");
			} 
			break; 

		case Packet_GameOver: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameOver_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameOver");
			} 
			break; 

		case Packet_GameRoomInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRoomInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomInfo");
			} 
			break; 

		case Packet_GameKickUser: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameKickUser_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameKickUser");
			} 
			break; 

		case Packet_GameEventInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEventInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEventInfo");
			} 
			break; 

		case Packet_GameUserInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameUserInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameUserInfo");
			} 
			break; 

		case Packet_GameUserOut: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameUserOut_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameUserOut");
			} 
			break; 

		case Packet_GameUserOutRsvn: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameUserOutRsvn_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameUserOutRsvn");
			} 
			break; 

		case Packet_GameEventStart: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEventStart_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEventStart");
			} 
			break; 

		case Packet_GameEvent2Start: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEvent2Start_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEvent2Start");
			} 
			break; 

		case Packet_GameEventRefresh: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEventRefresh_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEventRefresh");
			} 
			break; 

		case Packet_GameEventEnd: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEventEnd_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEventEnd");
			} 
			break; 

		case Packet_GameMileageRefresh: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameMileageRefresh_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameMileageRefresh");
			} 
			break; 

		case Packet_GameEventNotify: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEventNotify_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEventNotify");
			} 
			break; 

		case Packet_GameCurrentInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameCurrentInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameCurrentInfo");
			} 
			break; 

		case Packet_GameEntrySpectator: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEntrySpectator_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEntrySpectator");
			} 
			break; 

		case Packet_GameNotifyMessage: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameNotifyMessage_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameNotifyMessage");
			} 
			break; 

		case Packet_ResponsePurchaseList: 
			{
				System.Collections.Generic.List<string> Purchase_avatar; __msg >> Purchase_avatar;
				System.Collections.Generic.List<string> Purchase_card; __msg >> Purchase_card;
				System.Collections.Generic.List<string> Purchase_evt; __msg >> Purchase_evt;
				System.Collections.Generic.List<string> Purchase_charge; __msg >> Purchase_charge;

				bool bRet = ResponsePurchaseList_Call( remote, pkOption, Purchase_avatar, Purchase_card, Purchase_evt, Purchase_charge );
				if( bRet==false )
					m_owner->NeedImplement("ResponsePurchaseList");
			} 
			break; 

		case Packet_ResponsePurchaseAvailability: 
			{
				bool available; __msg >> available;
				string reason; __msg >> reason;

				bool bRet = ResponsePurchaseAvailability_Call( remote, pkOption, available, reason );
				if( bRet==false )
					m_owner->NeedImplement("ResponsePurchaseAvailability");
			} 
			break; 

		case Packet_ResponsePurchaseReceiptCheck: 
			{
				bool result; __msg >> result;
				System.Guid token; __msg >> token;

				bool bRet = ResponsePurchaseReceiptCheck_Call( remote, pkOption, result, token );
				if( bRet==false )
					m_owner->NeedImplement("ResponsePurchaseReceiptCheck");
			} 
			break; 

		case Packet_ResponsePurchaseResult: 
			{
				bool result; __msg >> result;
				string reason; __msg >> reason;

				bool bRet = ResponsePurchaseResult_Call( remote, pkOption, result, reason );
				if( bRet==false )
					m_owner->NeedImplement("ResponsePurchaseResult");
			} 
			break; 

		case Packet_ResponsePurchaseCash: 
			{
				bool result; __msg >> result;
				string reason; __msg >> reason;

				bool bRet = ResponsePurchaseCash_Call( remote, pkOption, result, reason );
				if( bRet==false )
					m_owner->NeedImplement("ResponsePurchaseCash");
			} 
			break; 

		case Packet_ResponseMyroomList: 
			{
				string json; __msg >> json;

				bool bRet = ResponseMyroomList_Call( remote, pkOption, json );
				if( bRet==false )
					m_owner->NeedImplement("ResponseMyroomList");
			} 
			break; 

		case Packet_ResponseMyroomAction: 
			{
				string pid; __msg >> pid;
				bool result; __msg >> result;
				string reason; __msg >> reason;

				bool bRet = ResponseMyroomAction_Call( remote, pkOption, pid, result, reason );
				if( bRet==false )
					m_owner->NeedImplement("ResponseMyroomAction");
			} 
			break; 

		case Packet_ResponseGameOptions: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = ResponseGameOptions_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("ResponseGameOptions");
			} 
			break; 

		case Packet_ResponseLobbyEventInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = ResponseLobbyEventInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLobbyEventInfo");
			} 
			break; 

		case Packet_ResponseLobbyEventParticipate: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = ResponseLobbyEventParticipate_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("ResponseLobbyEventParticipate");
			} 
			break; 

		case Packet_ServerMoveFailure: 
			{

				bool bRet = ServerMoveFailure_Call( remote, pkOption );
				if( bRet==false )
					m_owner->NeedImplement("ServerMoveFailure");
			} 
			break; 

		case Packet_RequestLauncherLogin: 
			{
				string id; __msg >> id;
				string pass; __msg >> pass;

				bool bRet = RequestLauncherLogin_Call( remote, pkOption, id, pass );
				if( bRet==false )
					m_owner->NeedImplement("RequestLauncherLogin");
			} 
			break; 

		case Packet_RequestLauncherLogout: 
			{
				string id; __msg >> id;
				string key; __msg >> key;

				bool bRet = RequestLauncherLogout_Call( remote, pkOption, id, key );
				if( bRet==false )
					m_owner->NeedImplement("RequestLauncherLogout");
			} 
			break; 

		case Packet_RequestLoginKey: 
			{
				string id; __msg >> id;
				string key; __msg >> key;

				bool bRet = RequestLoginKey_Call( remote, pkOption, id, key );
				if( bRet==false )
					m_owner->NeedImplement("RequestLoginKey");
			} 
			break; 

		case Packet_RequestLobbyKey: 
			{
				string id; __msg >> id;
				string key; __msg >> key;

				bool bRet = RequestLobbyKey_Call( remote, pkOption, id, key );
				if( bRet==false )
					m_owner->NeedImplement("RequestLobbyKey");
			} 
			break; 

		case Packet_RequestLogin: 
			{
				string name; __msg >> name;
				string pass; __msg >> pass;

				bool bRet = RequestLogin_Call( remote, pkOption, name, pass );
				if( bRet==false )
					m_owner->NeedImplement("RequestLogin");
			} 
			break; 

		case Packet_RequestLobbyList: 
			{

				bool bRet = RequestLobbyList_Call( remote, pkOption );
				if( bRet==false )
					m_owner->NeedImplement("RequestLobbyList");
			} 
			break; 

		case Packet_RequestGoLobby: 
			{
				string lobbyName; __msg >> lobbyName;

				bool bRet = RequestGoLobby_Call( remote, pkOption, lobbyName );
				if( bRet==false )
					m_owner->NeedImplement("RequestGoLobby");
			} 
			break; 

		case Packet_RequestJoinInfo: 
			{

				bool bRet = RequestJoinInfo_Call( remote, pkOption );
				if( bRet==false )
					m_owner->NeedImplement("RequestJoinInfo");
			} 
			break; 

		case Packet_RequestChannelMove: 
			{
				int chanID; __msg >> chanID;

				bool bRet = RequestChannelMove_Call( remote, pkOption, chanID );
				if( bRet==false )
					m_owner->NeedImplement("RequestChannelMove");
			} 
			break; 

		case Packet_RequestRoomMake: 
			{
				int chanID; __msg >> chanID;
				int betType; __msg >> betType;
				string pass; __msg >> pass;

				bool bRet = RequestRoomMake_Call( remote, pkOption, chanID, betType, pass );
				if( bRet==false )
					m_owner->NeedImplement("RequestRoomMake");
			} 
			break; 

		case Packet_RequestRoomJoin: 
			{
				int chanID; __msg >> chanID;
				int betType; __msg >> betType;

				bool bRet = RequestRoomJoin_Call( remote, pkOption, chanID, betType );
				if( bRet==false )
					m_owner->NeedImplement("RequestRoomJoin");
			} 
			break; 

		case Packet_RequestRoomJoinSelect: 
			{
				int chanID; __msg >> chanID;
				int roomNumber; __msg >> roomNumber;
				string pass; __msg >> pass;

				bool bRet = RequestRoomJoinSelect_Call( remote, pkOption, chanID, roomNumber, pass );
				if( bRet==false )
					m_owner->NeedImplement("RequestRoomJoinSelect");
			} 
			break; 

		case Packet_RequestBank: 
			{
				int option; __msg >> option;
				long money; __msg >> money;
				string pass; __msg >> pass;

				bool bRet = RequestBank_Call( remote, pkOption, option, money, pass );
				if( bRet==false )
					m_owner->NeedImplement("RequestBank");
			} 
			break; 

		case Packet_GameRoomOutRsvn: 
			{
				bool IsRsvn; __msg >> IsRsvn;

				bool bRet = GameRoomOutRsvn_Call( remote, pkOption, IsRsvn );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomOutRsvn");
			} 
			break; 

		case Packet_GameRoomOut: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRoomOut_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomOut");
			} 
			break; 

		case Packet_GameRoomMove: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRoomMove_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomMove");
			} 
			break; 

		case Packet_GameRoomInUser: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRoomInUser_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomInUser");
			} 
			break; 

		case Packet_GameRequestReady: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRequestReady_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRequestReady");
			} 
			break; 

		case Packet_GameDealCardsEnd: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameDealCardsEnd_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameDealCardsEnd");
			} 
			break; 

		case Packet_GameActionBet: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameActionBet_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameActionBet");
			} 
			break; 

		case Packet_GameActionChangeCard: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameActionChangeCard_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameActionChangeCard");
			} 
			break; 

		case Packet_RequestPurchaseList: 
			{

				bool bRet = RequestPurchaseList_Call( remote, pkOption );
				if( bRet==false )
					m_owner->NeedImplement("RequestPurchaseList");
			} 
			break; 

		case Packet_RequestPurchaseAvailability: 
			{
				string pid; __msg >> pid;

				bool bRet = RequestPurchaseAvailability_Call( remote, pkOption, pid );
				if( bRet==false )
					m_owner->NeedImplement("RequestPurchaseAvailability");
			} 
			break; 

		case Packet_RequestPurchaseReceiptCheck: 
			{
				string result; __msg >> result;

				bool bRet = RequestPurchaseReceiptCheck_Call( remote, pkOption, result );
				if( bRet==false )
					m_owner->NeedImplement("RequestPurchaseReceiptCheck");
			} 
			break; 

		case Packet_RequestPurchaseResult: 
			{
				System.Guid token; __msg >> token;

				bool bRet = RequestPurchaseResult_Call( remote, pkOption, token );
				if( bRet==false )
					m_owner->NeedImplement("RequestPurchaseResult");
			} 
			break; 

		case Packet_RequestPurchaseCash: 
			{
				string pid; __msg >> pid;

				bool bRet = RequestPurchaseCash_Call( remote, pkOption, pid );
				if( bRet==false )
					m_owner->NeedImplement("RequestPurchaseCash");
			} 
			break; 

		case Packet_RequestMyroomList: 
			{

				bool bRet = RequestMyroomList_Call( remote, pkOption );
				if( bRet==false )
					m_owner->NeedImplement("RequestMyroomList");
			} 
			break; 

		case Packet_RequestMyroomAction: 
			{
				string pid; __msg >> pid;

				bool bRet = RequestMyroomAction_Call( remote, pkOption, pid );
				if( bRet==false )
					m_owner->NeedImplement("RequestMyroomAction");
			} 
			break; 

		case Packet_RequestGameOptions: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RequestGameOptions_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("RequestGameOptions");
			} 
			break; 

		case Packet_RequestLobbyEventInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RequestLobbyEventInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("RequestLobbyEventInfo");
			} 
			break; 

		case Packet_RequestLobbyEventParticipate: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RequestLobbyEventParticipate_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("RequestLobbyEventParticipate");
			} 
			break; 

			default: return false;
		}
		return true;
	}
}

