// Auto created from IDLCompiler.exe
using System;


namespace SS 
{

public class Common
{
	public const ZNet.PacketType MasterAllShutdown = (ZNet.PacketType)(100+0);

	public const ZNet.PacketType MasterNotifyP2PServerInfo = (ZNet.PacketType)(100+1);

	public const ZNet.PacketType P2PMemberCheck = (ZNet.PacketType)(100+2);

	public const ZNet.PacketType RoomLobbyMakeRoom = (ZNet.PacketType)(100+3);

	public const ZNet.PacketType RoomLobbyJoinRoom = (ZNet.PacketType)(100+4);

	public const ZNet.PacketType RoomLobbyOutRoom = (ZNet.PacketType)(100+5);

	public const ZNet.PacketType RoomLobbyMessage = (ZNet.PacketType)(100+6);

	public const ZNet.PacketType RoomLobbyEventStart = (ZNet.PacketType)(100+7);

	public const ZNet.PacketType RoomLobbyEventEnd = (ZNet.PacketType)(100+8);

	public const ZNet.PacketType LobbyRoomJackpotInfo = (ZNet.PacketType)(100+9);

	public const ZNet.PacketType LobbyRoomNotifyMessage = (ZNet.PacketType)(100+10);

	public const ZNet.PacketType LobbyRoomNotifyServermaintenance = (ZNet.PacketType)(100+11);

	public const ZNet.PacketType LobbyRoomReloadServerData = (ZNet.PacketType)(100+12);

	public const ZNet.PacketType LobbyRoomKickUser = (ZNet.PacketType)(100+13);

	public const ZNet.PacketType LobbyLoginKickUser = (ZNet.PacketType)(100+14);

	public const ZNet.PacketType RoomLobbyRequestMoveRoom = (ZNet.PacketType)(100+15);

	public const ZNet.PacketType LobbyRoomResponseMoveRoom = (ZNet.PacketType)(100+16);

	public const ZNet.PacketType ServerRequestDataSync = (ZNet.PacketType)(100+17);

	public const ZNet.PacketType RoomLobbyResponseDataSync = (ZNet.PacketType)(100+18);

	public const ZNet.PacketType RelayLobbyResponseDataSync = (ZNet.PacketType)(100+19);

	public const ZNet.PacketType RelayClientJoin = (ZNet.PacketType)(100+20);

	public const ZNet.PacketType RelayClientLeave = (ZNet.PacketType)(100+21);

	public const ZNet.PacketType RelayCloseRemoteClient = (ZNet.PacketType)(100+22);

	public const ZNet.PacketType RelayServerMoveFailure = (ZNet.PacketType)(100+23);

	public const ZNet.PacketType RelayRequestLobbyKey = (ZNet.PacketType)(100+24);

	public const ZNet.PacketType RelayRequestJoinInfo = (ZNet.PacketType)(100+25);

	public const ZNet.PacketType RelayRequestChannelMove = (ZNet.PacketType)(100+26);

	public const ZNet.PacketType RelayRequestRoomMake = (ZNet.PacketType)(100+27);

	public const ZNet.PacketType RelayRequestRoomJoin = (ZNet.PacketType)(100+28);

	public const ZNet.PacketType RelayRequestRoomJoinSelect = (ZNet.PacketType)(100+29);

	public const ZNet.PacketType RelayRequestBank = (ZNet.PacketType)(100+30);

	public const ZNet.PacketType RelayRequestPurchaseList = (ZNet.PacketType)(100+31);

	public const ZNet.PacketType RelayRequestPurchaseAvailability = (ZNet.PacketType)(100+32);

	public const ZNet.PacketType RelayRequestPurchaseReceiptCheck = (ZNet.PacketType)(100+33);

	public const ZNet.PacketType RelayRequestPurchaseResult = (ZNet.PacketType)(100+34);

	public const ZNet.PacketType RelayRequestPurchaseCash = (ZNet.PacketType)(100+35);

	public const ZNet.PacketType RelayRequestMyroomList = (ZNet.PacketType)(100+36);

	public const ZNet.PacketType RelayRequestMyroomAction = (ZNet.PacketType)(100+37);

	public const ZNet.PacketType LobbyRelayResponsePurchaseList = (ZNet.PacketType)(100+38);

	public const ZNet.PacketType LobbyRelayResponsePurchaseAvailability = (ZNet.PacketType)(100+39);

	public const ZNet.PacketType LobbyRelayResponsePurchaseReceiptCheck = (ZNet.PacketType)(100+40);

	public const ZNet.PacketType LobbyRelayResponsePurchaseResult = (ZNet.PacketType)(100+41);

	public const ZNet.PacketType LobbyRelayResponsePurchaseCash = (ZNet.PacketType)(100+42);

	public const ZNet.PacketType LobbyRelayResponseMyroomList = (ZNet.PacketType)(100+43);

	public const ZNet.PacketType LobbyRelayResponseMyroomAction = (ZNet.PacketType)(100+44);

	public const ZNet.PacketType LobbyRelayServerMoveStart = (ZNet.PacketType)(100+45);

	public const ZNet.PacketType LobbyRelayResponseLobbyKey = (ZNet.PacketType)(100+46);

	public const ZNet.PacketType LobbyRelayNotifyUserInfo = (ZNet.PacketType)(100+47);

	public const ZNet.PacketType LobbyRelayNotifyUserList = (ZNet.PacketType)(100+48);

	public const ZNet.PacketType LobbyRelayNotifyRoomList = (ZNet.PacketType)(100+49);

	public const ZNet.PacketType LobbyRelayResponseChannelMove = (ZNet.PacketType)(100+50);

	public const ZNet.PacketType LobbyRelayResponseLobbyMessage = (ZNet.PacketType)(100+51);

	public const ZNet.PacketType LobbyRelayResponseBank = (ZNet.PacketType)(100+52);

	public const ZNet.PacketType LobbyRelayNotifyJackpotInfo = (ZNet.PacketType)(100+53);

	public const ZNet.PacketType LobbyRelayNotifyLobbyMessage = (ZNet.PacketType)(100+54);

	public const ZNet.PacketType RoomRelayServerMoveStart = (ZNet.PacketType)(100+55);

	public const ZNet.PacketType RelayRequestOutRoom = (ZNet.PacketType)(100+56);

	public const ZNet.PacketType RelayResponseOutRoom = (ZNet.PacketType)(100+57);

	public const ZNet.PacketType RelayRequestMoveRoom = (ZNet.PacketType)(100+58);

	public const ZNet.PacketType RelayResponseMoveRoom = (ZNet.PacketType)(100+59);

	public const ZNet.PacketType RelayGameRoomIn = (ZNet.PacketType)(100+60);

	public const ZNet.PacketType RelayGameReady = (ZNet.PacketType)(100+61);

	public const ZNet.PacketType RelayGameSelectOrder = (ZNet.PacketType)(100+62);

	public const ZNet.PacketType RelayGameDistributedEnd = (ZNet.PacketType)(100+63);

	public const ZNet.PacketType RelayGameActionPutCard = (ZNet.PacketType)(100+64);

	public const ZNet.PacketType RelayGameActionFlipBomb = (ZNet.PacketType)(100+65);

	public const ZNet.PacketType RelayGameActionChooseCard = (ZNet.PacketType)(100+66);

	public const ZNet.PacketType RelayGameSelectKookjin = (ZNet.PacketType)(100+67);

	public const ZNet.PacketType RelayGameSelectGoStop = (ZNet.PacketType)(100+68);

	public const ZNet.PacketType RelayGameSelectPush = (ZNet.PacketType)(100+69);

	public const ZNet.PacketType RelayGamePractice = (ZNet.PacketType)(100+70);

	public const ZNet.PacketType GameRelayRoomIn = (ZNet.PacketType)(100+71);

	public const ZNet.PacketType GameRelayRequestReady = (ZNet.PacketType)(100+72);

	public const ZNet.PacketType GameRelayStart = (ZNet.PacketType)(100+73);

	public const ZNet.PacketType GameRelayRequestSelectOrder = (ZNet.PacketType)(100+74);

	public const ZNet.PacketType GameRelayOrderEnd = (ZNet.PacketType)(100+75);

	public const ZNet.PacketType GameRelayDistributedStart = (ZNet.PacketType)(100+76);

	public const ZNet.PacketType GameRelayFloorHasBonus = (ZNet.PacketType)(100+77);

	public const ZNet.PacketType GameRelayTurnStart = (ZNet.PacketType)(100+78);

	public const ZNet.PacketType GameRelaySelectCardResult = (ZNet.PacketType)(100+79);

	public const ZNet.PacketType GameRelayFlipDeckResult = (ZNet.PacketType)(100+80);

	public const ZNet.PacketType GameRelayTurnResult = (ZNet.PacketType)(100+81);

	public const ZNet.PacketType GameRelayUserInfo = (ZNet.PacketType)(100+82);

	public const ZNet.PacketType GameRelayNotifyIndex = (ZNet.PacketType)(100+83);

	public const ZNet.PacketType GameRelayNotifyStat = (ZNet.PacketType)(100+84);

	public const ZNet.PacketType GameRelayRequestKookjin = (ZNet.PacketType)(100+85);

	public const ZNet.PacketType GameRelayNotifyKookjin = (ZNet.PacketType)(100+86);

	public const ZNet.PacketType GameRelayRequestGoStop = (ZNet.PacketType)(100+87);

	public const ZNet.PacketType GameRelayNotifyGoStop = (ZNet.PacketType)(100+88);

	public const ZNet.PacketType GameRelayMoveKookjin = (ZNet.PacketType)(100+89);

	public const ZNet.PacketType GameRelayEventStart = (ZNet.PacketType)(100+90);

	public const ZNet.PacketType GameRelayEventInfo = (ZNet.PacketType)(100+91);

	public const ZNet.PacketType GameRelayOver = (ZNet.PacketType)(100+92);

	public const ZNet.PacketType GameRelayRequestPush = (ZNet.PacketType)(100+93);

	public const ZNet.PacketType GameRelayResponseRoomMove = (ZNet.PacketType)(100+94);

	public const ZNet.PacketType GameRelayPracticeEnd = (ZNet.PacketType)(100+95);

	public const ZNet.PacketType GameRelayResponseRoomOut = (ZNet.PacketType)(100+96);

	public const ZNet.PacketType GameRelayKickUser = (ZNet.PacketType)(100+97);

	public const ZNet.PacketType GameRelayRoomInfo = (ZNet.PacketType)(100+98);

	public const ZNet.PacketType GameRelayUserOut = (ZNet.PacketType)(100+99);

	public const ZNet.PacketType GameRelayObserveInfo = (ZNet.PacketType)(100+100);

	public const ZNet.PacketType GameRelayNotifyMessage = (ZNet.PacketType)(100+101);

	public const ZNet.PacketType GameRelayNotifyJackpotInfo = (ZNet.PacketType)(100+102);

	public const ZNet.PacketType RelayRequestLobbyEventInfo = (ZNet.PacketType)(100+103);

	public const ZNet.PacketType LobbyRelayResponseLobbyEventInfo = (ZNet.PacketType)(100+104);

	public const ZNet.PacketType RelayRequestLobbyEventParticipate = (ZNet.PacketType)(100+105);

	public const ZNet.PacketType LobbyRelayResponseLobbyEventParticipate = (ZNet.PacketType)(100+106);

	public const ZNet.PacketType GameRelayResponseRoomMissionInfo = (ZNet.PacketType)(100+107);

	public const ZNet.PacketType ServerMoveStart = (ZNet.PacketType)(100+108);

	public const ZNet.PacketType ServerMoveEnd = (ZNet.PacketType)(100+109);

	public const ZNet.PacketType ResponseLauncherLogin = (ZNet.PacketType)(100+110);

	public const ZNet.PacketType ResponseLauncherLogout = (ZNet.PacketType)(100+111);

	public const ZNet.PacketType ResponseLoginKey = (ZNet.PacketType)(100+112);

	public const ZNet.PacketType ResponseLobbyKey = (ZNet.PacketType)(100+113);

	public const ZNet.PacketType ResponseLogin = (ZNet.PacketType)(100+114);

	public const ZNet.PacketType NotifyLobbyList = (ZNet.PacketType)(100+115);

	public const ZNet.PacketType NotifyUserInfo = (ZNet.PacketType)(100+116);

	public const ZNet.PacketType NotifyUserList = (ZNet.PacketType)(100+117);

	public const ZNet.PacketType NotifyRoomList = (ZNet.PacketType)(100+118);

	public const ZNet.PacketType ResponseChannelMove = (ZNet.PacketType)(100+119);

	public const ZNet.PacketType ResponseLobbyMessage = (ZNet.PacketType)(100+120);

	public const ZNet.PacketType ResponseBank = (ZNet.PacketType)(100+121);

	public const ZNet.PacketType NotifyJackpotInfo = (ZNet.PacketType)(100+122);

	public const ZNet.PacketType NotifyLobbyMessage = (ZNet.PacketType)(100+123);

	public const ZNet.PacketType GameRoomInUser = (ZNet.PacketType)(100+124);

	public const ZNet.PacketType GameRequestReady = (ZNet.PacketType)(100+125);

	public const ZNet.PacketType GameStart = (ZNet.PacketType)(100+126);

	public const ZNet.PacketType GameRequestSelectOrder = (ZNet.PacketType)(100+127);

	public const ZNet.PacketType GameOrderEnd = (ZNet.PacketType)(100+128);

	public const ZNet.PacketType GameDistributedStart = (ZNet.PacketType)(100+129);

	public const ZNet.PacketType GameFloorHasBonus = (ZNet.PacketType)(100+130);

	public const ZNet.PacketType GameTurnStart = (ZNet.PacketType)(100+131);

	public const ZNet.PacketType GameSelectCardResult = (ZNet.PacketType)(100+132);

	public const ZNet.PacketType GameFlipDeckResult = (ZNet.PacketType)(100+133);

	public const ZNet.PacketType GameTurnResult = (ZNet.PacketType)(100+134);

	public const ZNet.PacketType GameUserInfo = (ZNet.PacketType)(100+135);

	public const ZNet.PacketType GameNotifyIndex = (ZNet.PacketType)(100+136);

	public const ZNet.PacketType GameNotifyStat = (ZNet.PacketType)(100+137);

	public const ZNet.PacketType GameRequestKookjin = (ZNet.PacketType)(100+138);

	public const ZNet.PacketType GameNotifyKookjin = (ZNet.PacketType)(100+139);

	public const ZNet.PacketType GameRequestGoStop = (ZNet.PacketType)(100+140);

	public const ZNet.PacketType GameNotifyGoStop = (ZNet.PacketType)(100+141);

	public const ZNet.PacketType GameMoveKookjin = (ZNet.PacketType)(100+142);

	public const ZNet.PacketType GameEventStart = (ZNet.PacketType)(100+143);

	public const ZNet.PacketType GameEventInfo = (ZNet.PacketType)(100+144);

	public const ZNet.PacketType GameOver = (ZNet.PacketType)(100+145);

	public const ZNet.PacketType GameRequestPush = (ZNet.PacketType)(100+146);

	public const ZNet.PacketType GameResponseRoomMove = (ZNet.PacketType)(100+147);

	public const ZNet.PacketType GamePracticeEnd = (ZNet.PacketType)(100+148);

	public const ZNet.PacketType GameResponseRoomOut = (ZNet.PacketType)(100+149);

	public const ZNet.PacketType GameKickUser = (ZNet.PacketType)(100+150);

	public const ZNet.PacketType GameRoomInfo = (ZNet.PacketType)(100+151);

	public const ZNet.PacketType GameUserOut = (ZNet.PacketType)(100+152);

	public const ZNet.PacketType GameObserveInfo = (ZNet.PacketType)(100+153);

	public const ZNet.PacketType GameNotifyMessage = (ZNet.PacketType)(100+154);

	public const ZNet.PacketType ResponsePurchaseList = (ZNet.PacketType)(100+155);

	public const ZNet.PacketType ResponsePurchaseAvailability = (ZNet.PacketType)(100+156);

	public const ZNet.PacketType ResponsePurchaseReceiptCheck = (ZNet.PacketType)(100+157);

	public const ZNet.PacketType ResponsePurchaseResult = (ZNet.PacketType)(100+158);

	public const ZNet.PacketType ResponsePurchaseCash = (ZNet.PacketType)(100+159);

	public const ZNet.PacketType ResponseMyroomList = (ZNet.PacketType)(100+160);

	public const ZNet.PacketType ResponseMyroomAction = (ZNet.PacketType)(100+161);

	public const ZNet.PacketType ResponseGameOptions = (ZNet.PacketType)(100+162);

	public const ZNet.PacketType ResponseLobbyEventInfo = (ZNet.PacketType)(100+163);

	public const ZNet.PacketType ResponseLobbyEventParticipate = (ZNet.PacketType)(100+164);

	public const ZNet.PacketType GameResponseRoomMissionInfo = (ZNet.PacketType)(100+165);

	public const ZNet.PacketType ServerMoveFailure = (ZNet.PacketType)(100+166);

	public const ZNet.PacketType RequestLauncherLogin = (ZNet.PacketType)(100+167);

	public const ZNet.PacketType RequestLauncherLogout = (ZNet.PacketType)(100+168);

	public const ZNet.PacketType RequestLoginKey = (ZNet.PacketType)(100+169);

	public const ZNet.PacketType RequestLobbyKey = (ZNet.PacketType)(100+170);

	public const ZNet.PacketType RequestLogin = (ZNet.PacketType)(100+171);

	public const ZNet.PacketType RequestLobbyList = (ZNet.PacketType)(100+172);

	public const ZNet.PacketType RequestGoLobby = (ZNet.PacketType)(100+173);

	public const ZNet.PacketType RequestJoinInfo = (ZNet.PacketType)(100+174);

	public const ZNet.PacketType RequestChannelMove = (ZNet.PacketType)(100+175);

	public const ZNet.PacketType RequestRoomMake = (ZNet.PacketType)(100+176);

	public const ZNet.PacketType RequestRoomJoin = (ZNet.PacketType)(100+177);

	public const ZNet.PacketType RequestRoomJoinSelect = (ZNet.PacketType)(100+178);

	public const ZNet.PacketType RequestBank = (ZNet.PacketType)(100+179);

	public const ZNet.PacketType GameRoomIn = (ZNet.PacketType)(100+180);

	public const ZNet.PacketType GameReady = (ZNet.PacketType)(100+181);

	public const ZNet.PacketType GameSelectOrder = (ZNet.PacketType)(100+182);

	public const ZNet.PacketType GameDistributedEnd = (ZNet.PacketType)(100+183);

	public const ZNet.PacketType GameActionPutCard = (ZNet.PacketType)(100+184);

	public const ZNet.PacketType GameActionFlipBomb = (ZNet.PacketType)(100+185);

	public const ZNet.PacketType GameActionChooseCard = (ZNet.PacketType)(100+186);

	public const ZNet.PacketType GameSelectKookjin = (ZNet.PacketType)(100+187);

	public const ZNet.PacketType GameSelectGoStop = (ZNet.PacketType)(100+188);

	public const ZNet.PacketType GameSelectPush = (ZNet.PacketType)(100+189);

	public const ZNet.PacketType GamePractice = (ZNet.PacketType)(100+190);

	public const ZNet.PacketType GameRoomOut = (ZNet.PacketType)(100+191);

	public const ZNet.PacketType GameRoomMove = (ZNet.PacketType)(100+192);

	public const ZNet.PacketType RequestPurchaseList = (ZNet.PacketType)(100+193);

	public const ZNet.PacketType RequestPurchaseAvailability = (ZNet.PacketType)(100+194);

	public const ZNet.PacketType RequestPurchaseReceiptCheck = (ZNet.PacketType)(100+195);

	public const ZNet.PacketType RequestPurchaseResult = (ZNet.PacketType)(100+196);

	public const ZNet.PacketType RequestPurchaseCash = (ZNet.PacketType)(100+197);

	public const ZNet.PacketType RequestMyroomList = (ZNet.PacketType)(100+198);

	public const ZNet.PacketType RequestMyroomAction = (ZNet.PacketType)(100+199);

	public const ZNet.PacketType RequestGameOptions = (ZNet.PacketType)(100+200);

	public const ZNet.PacketType RequestLobbyEventInfo = (ZNet.PacketType)(100+201);

	public const ZNet.PacketType RequestLobbyEventParticipate = (ZNet.PacketType)(100+202);

}

}

