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

		case Packet_P2PMemberCheck: 
			{

				bool bRet = P2PMemberCheck_Call( remote, pkOption );
				if( bRet==false )
					m_owner->NeedImplement("P2PMemberCheck");
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
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int userID; __msg >> userID;
				long money; __msg >> money;
				bool ipFree; __msg >> ipFree;
				bool shopFree; __msg >> shopFree;
				int shopId; __msg >> shopId;

				bool bRet = RoomLobbyRequestMoveRoom_Call( remote, pkOption, roomID, userRemote, userID, money, ipFree, shopFree, shopId );
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
				ZNet.RemoteID userRemote; __msg >> userRemote;
				string message; __msg >> message;

				bool bRet = LobbyRoomResponseMoveRoom_Call( remote, pkOption, makeRoom, roomID, addr, chanID, userRemote, message );
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
				List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; __msg >> lobbyUserInfos;
				List<string> lobbyFriendList; __msg >> lobbyFriendList;

				bool bRet = LobbyRelayNotifyUserList_Call( remote, pkOption, userRemote, lobbyUserInfos, lobbyFriendList );
				if( bRet==false )
					m_owner->NeedImplement("LobbyRelayNotifyUserList");
			} 
			break; 

		case Packet_LobbyRelayNotifyRoomList: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				int channelID; __msg >> channelID;
				List<Rmi.Marshaler.RoomInfo> roomInfos; __msg >> roomInfos;

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

		case Packet_RelayRequestOutRoom: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayRequestOutRoom_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestOutRoom");
			} 
			break; 

		case Packet_RelayResponseOutRoom: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool resultOut; __msg >> resultOut;

				bool bRet = RelayResponseOutRoom_Call( remote, pkOption, userRemote, resultOut );
				if( bRet==false )
					m_owner->NeedImplement("RelayResponseOutRoom");
			} 
			break; 

		case Packet_RelayRequestMoveRoom: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;

				bool bRet = RelayRequestMoveRoom_Call( remote, pkOption, userRemote );
				if( bRet==false )
					m_owner->NeedImplement("RelayRequestMoveRoom");
			} 
			break; 

		case Packet_RelayResponseMoveRoom: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				bool resultMove; __msg >> resultMove;
				string errorMessage; __msg >> errorMessage;

				bool bRet = RelayResponseMoveRoom_Call( remote, pkOption, userRemote, resultMove, errorMessage );
				if( bRet==false )
					m_owner->NeedImplement("RelayResponseMoveRoom");
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

		case Packet_RelayGameReady: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameReady_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameReady");
			} 
			break; 

		case Packet_RelayGameSelectOrder: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameSelectOrder_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameSelectOrder");
			} 
			break; 

		case Packet_RelayGameDistributedEnd: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameDistributedEnd_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameDistributedEnd");
			} 
			break; 

		case Packet_RelayGameActionPutCard: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameActionPutCard_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameActionPutCard");
			} 
			break; 

		case Packet_RelayGameActionFlipBomb: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameActionFlipBomb_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameActionFlipBomb");
			} 
			break; 

		case Packet_RelayGameActionChooseCard: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameActionChooseCard_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameActionChooseCard");
			} 
			break; 

		case Packet_RelayGameSelectKookjin: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameSelectKookjin_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameSelectKookjin");
			} 
			break; 

		case Packet_RelayGameSelectGoStop: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameSelectGoStop_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameSelectGoStop");
			} 
			break; 

		case Packet_RelayGameSelectPush: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGameSelectPush_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGameSelectPush");
			} 
			break; 

		case Packet_RelayGamePractice: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = RelayGamePractice_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("RelayGamePractice");
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

		case Packet_GameRelayRequestReady: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRequestReady_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRequestReady");
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

		case Packet_GameRelayRequestSelectOrder: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRequestSelectOrder_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRequestSelectOrder");
			} 
			break; 

		case Packet_GameRelayOrderEnd: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayOrderEnd_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayOrderEnd");
			} 
			break; 

		case Packet_GameRelayDistributedStart: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayDistributedStart_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayDistributedStart");
			} 
			break; 

		case Packet_GameRelayFloorHasBonus: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayFloorHasBonus_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayFloorHasBonus");
			} 
			break; 

		case Packet_GameRelayTurnStart: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayTurnStart_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayTurnStart");
			} 
			break; 

		case Packet_GameRelaySelectCardResult: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelaySelectCardResult_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelaySelectCardResult");
			} 
			break; 

		case Packet_GameRelayFlipDeckResult: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayFlipDeckResult_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayFlipDeckResult");
			} 
			break; 

		case Packet_GameRelayTurnResult: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayTurnResult_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayTurnResult");
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

		case Packet_GameRelayNotifyIndex: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayNotifyIndex_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayNotifyIndex");
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

		case Packet_GameRelayRequestKookjin: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRequestKookjin_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRequestKookjin");
			} 
			break; 

		case Packet_GameRelayNotifyKookjin: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayNotifyKookjin_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayNotifyKookjin");
			} 
			break; 

		case Packet_GameRelayRequestGoStop: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRequestGoStop_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRequestGoStop");
			} 
			break; 

		case Packet_GameRelayNotifyGoStop: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayNotifyGoStop_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayNotifyGoStop");
			} 
			break; 

		case Packet_GameRelayMoveKookjin: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayMoveKookjin_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayMoveKookjin");
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

		case Packet_GameRelayEventInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayEventInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayEventInfo");
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

		case Packet_GameRelayRequestPush: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayRequestPush_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayRequestPush");
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

		case Packet_GameRelayPracticeEnd: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayPracticeEnd_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayPracticeEnd");
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

		case Packet_GameRelayKickUser: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayKickUser_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayKickUser");
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

		case Packet_GameRelayUserOut: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayUserOut_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayUserOut");
			} 
			break; 

		case Packet_GameRelayObserveInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayObserveInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayObserveInfo");
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

		case Packet_GameRelayResponseRoomMissionInfo: 
			{
				ZNet.RemoteID userRemote; __msg >> userRemote;
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRelayResponseRoomMissionInfo_Call( remote, pkOption, userRemote, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRelayResponseRoomMissionInfo");
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
				List<string> lobbyList; __msg >> lobbyList;

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
				List<Rmi.Marshaler.LobbyUserList> lobbyUserInfos; __msg >> lobbyUserInfos;
				List<string> lobbyFriendList; __msg >> lobbyFriendList;

				bool bRet = NotifyUserList_Call( remote, pkOption, lobbyUserInfos, lobbyFriendList );
				if( bRet==false )
					m_owner->NeedImplement("NotifyUserList");
			} 
			break; 

		case Packet_NotifyRoomList: 
			{
				int channelID; __msg >> channelID;
				List<Rmi.Marshaler.RoomInfo> roomInfos; __msg >> roomInfos;

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

		case Packet_GameRoomIn: 
			{
				bool result; __msg >> result;

				bool bRet = GameRoomIn_Call( remote, pkOption, result );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomIn");
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

		case Packet_GameStart: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameStart_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameStart");
			} 
			break; 

		case Packet_GameRequestSelectOrder: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRequestSelectOrder_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRequestSelectOrder");
			} 
			break; 

		case Packet_GameOrderEnd: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameOrderEnd_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameOrderEnd");
			} 
			break; 

		case Packet_GameDistributedStart: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameDistributedStart_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameDistributedStart");
			} 
			break; 

		case Packet_GameFloorHasBonus: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameFloorHasBonus_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameFloorHasBonus");
			} 
			break; 

		case Packet_GameTurnStart: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameTurnStart_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameTurnStart");
			} 
			break; 

		case Packet_GameSelectCardResult: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameSelectCardResult_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameSelectCardResult");
			} 
			break; 

		case Packet_GameFlipDeckResult: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameFlipDeckResult_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameFlipDeckResult");
			} 
			break; 

		case Packet_GameTurnResult: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameTurnResult_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameTurnResult");
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

		case Packet_GameNotifyIndex: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameNotifyIndex_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameNotifyIndex");
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

		case Packet_GameRequestKookjin: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRequestKookjin_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRequestKookjin");
			} 
			break; 

		case Packet_GameNotifyKookjin: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameNotifyKookjin_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameNotifyKookjin");
			} 
			break; 

		case Packet_GameRequestGoStop: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRequestGoStop_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRequestGoStop");
			} 
			break; 

		case Packet_GameNotifyGoStop: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameNotifyGoStop_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameNotifyGoStop");
			} 
			break; 

		case Packet_GameMoveKookjin: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameMoveKookjin_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameMoveKookjin");
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

		case Packet_GameEventInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameEventInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameEventInfo");
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

		case Packet_GameRequestPush: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRequestPush_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRequestPush");
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

		case Packet_GamePracticeEnd: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GamePracticeEnd_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GamePracticeEnd");
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

		case Packet_GameKickUser: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameKickUser_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameKickUser");
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

		case Packet_GameUserOut: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameUserOut_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameUserOut");
			} 
			break; 

		case Packet_GameObserveInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameObserveInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameObserveInfo");
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
				List<string> Purchase_avatar; __msg >> Purchase_avatar;
				List<string> Purchase_card; __msg >> Purchase_card;
				List<string> Purchase_evt; __msg >> Purchase_evt;
				List<string> Purchase_charge; __msg >> Purchase_charge;

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

		case Packet_GameResponseRoomMissionInfo: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameResponseRoomMissionInfo_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameResponseRoomMissionInfo");
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

		case Packet_GameRoomIn: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameRoomIn_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameRoomIn");
			} 
			break; 

		case Packet_GameReady: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameReady_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameReady");
			} 
			break; 

		case Packet_GameSelectOrder: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameSelectOrder_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameSelectOrder");
			} 
			break; 

		case Packet_GameDistributedEnd: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameDistributedEnd_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameDistributedEnd");
			} 
			break; 

		case Packet_GameActionPutCard: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameActionPutCard_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameActionPutCard");
			} 
			break; 

		case Packet_GameActionFlipBomb: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameActionFlipBomb_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameActionFlipBomb");
			} 
			break; 

		case Packet_GameActionChooseCard: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameActionChooseCard_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameActionChooseCard");
			} 
			break; 

		case Packet_GameSelectKookjin: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameSelectKookjin_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameSelectKookjin");
			} 
			break; 

		case Packet_GameSelectGoStop: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameSelectGoStop_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameSelectGoStop");
			} 
			break; 

		case Packet_GameSelectPush: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GameSelectPush_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GameSelectPush");
			} 
			break; 

		case Packet_GamePractice: 
			{
				ZNet.ArrByte data; __msg >> data;

				bool bRet = GamePractice_Call( remote, pkOption, data );
				if( bRet==false )
					m_owner->NeedImplement("GamePractice");
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

