// Auto created from IDLCompiler.exe
#pragma once

namespace SS 
{
static const Zero::PacketType Packet_MasterAllShutdown = (Zero::PacketType)(100+0);

static const Zero::PacketType Packet_MasterNotifyP2PServerInfo = (Zero::PacketType)(100+1);

static const Zero::PacketType Packet_P2PMemberCheck = (Zero::PacketType)(100+2);

static const Zero::PacketType Packet_RoomLobbyMakeRoom = (Zero::PacketType)(100+3);

static const Zero::PacketType Packet_RoomLobbyJoinRoom = (Zero::PacketType)(100+4);

static const Zero::PacketType Packet_RoomLobbyOutRoom = (Zero::PacketType)(100+5);

static const Zero::PacketType Packet_RoomLobbyMessage = (Zero::PacketType)(100+6);

static const Zero::PacketType Packet_RoomLobbyEventStart = (Zero::PacketType)(100+7);

static const Zero::PacketType Packet_RoomLobbyEventEnd = (Zero::PacketType)(100+8);

static const Zero::PacketType Packet_LobbyRoomJackpotInfo = (Zero::PacketType)(100+9);

static const Zero::PacketType Packet_LobbyRoomNotifyMessage = (Zero::PacketType)(100+10);

static const Zero::PacketType Packet_LobbyRoomNotifyServermaintenance = (Zero::PacketType)(100+11);

static const Zero::PacketType Packet_LobbyRoomReloadServerData = (Zero::PacketType)(100+12);

static const Zero::PacketType Packet_LobbyRoomKickUser = (Zero::PacketType)(100+13);

static const Zero::PacketType Packet_LobbyLoginKickUser = (Zero::PacketType)(100+14);

static const Zero::PacketType Packet_RoomLobbyRequestMoveRoom = (Zero::PacketType)(100+15);

static const Zero::PacketType Packet_LobbyRoomResponseMoveRoom = (Zero::PacketType)(100+16);

static const Zero::PacketType Packet_ServerRequestDataSync = (Zero::PacketType)(100+17);

static const Zero::PacketType Packet_RoomLobbyResponseDataSync = (Zero::PacketType)(100+18);

static const Zero::PacketType Packet_RelayLobbyResponseDataSync = (Zero::PacketType)(100+19);

static const Zero::PacketType Packet_RelayClientJoin = (Zero::PacketType)(100+20);

static const Zero::PacketType Packet_RelayClientLeave = (Zero::PacketType)(100+21);

static const Zero::PacketType Packet_RelayCloseRemoteClient = (Zero::PacketType)(100+22);

static const Zero::PacketType Packet_RelayServerMoveFailure = (Zero::PacketType)(100+23);

static const Zero::PacketType Packet_RelayRequestLobbyKey = (Zero::PacketType)(100+24);

static const Zero::PacketType Packet_RelayRequestJoinInfo = (Zero::PacketType)(100+25);

static const Zero::PacketType Packet_RelayRequestChannelMove = (Zero::PacketType)(100+26);

static const Zero::PacketType Packet_RelayRequestRoomMake = (Zero::PacketType)(100+27);

static const Zero::PacketType Packet_RelayRequestRoomJoin = (Zero::PacketType)(100+28);

static const Zero::PacketType Packet_RelayRequestRoomJoinSelect = (Zero::PacketType)(100+29);

static const Zero::PacketType Packet_RelayRequestBank = (Zero::PacketType)(100+30);

static const Zero::PacketType Packet_RelayRequestPurchaseList = (Zero::PacketType)(100+31);

static const Zero::PacketType Packet_RelayRequestPurchaseAvailability = (Zero::PacketType)(100+32);

static const Zero::PacketType Packet_RelayRequestPurchaseReceiptCheck = (Zero::PacketType)(100+33);

static const Zero::PacketType Packet_RelayRequestPurchaseResult = (Zero::PacketType)(100+34);

static const Zero::PacketType Packet_RelayRequestPurchaseCash = (Zero::PacketType)(100+35);

static const Zero::PacketType Packet_RelayRequestMyroomList = (Zero::PacketType)(100+36);

static const Zero::PacketType Packet_RelayRequestMyroomAction = (Zero::PacketType)(100+37);

static const Zero::PacketType Packet_LobbyRelayResponsePurchaseList = (Zero::PacketType)(100+38);

static const Zero::PacketType Packet_LobbyRelayResponsePurchaseAvailability = (Zero::PacketType)(100+39);

static const Zero::PacketType Packet_LobbyRelayResponsePurchaseReceiptCheck = (Zero::PacketType)(100+40);

static const Zero::PacketType Packet_LobbyRelayResponsePurchaseResult = (Zero::PacketType)(100+41);

static const Zero::PacketType Packet_LobbyRelayResponsePurchaseCash = (Zero::PacketType)(100+42);

static const Zero::PacketType Packet_LobbyRelayResponseMyroomList = (Zero::PacketType)(100+43);

static const Zero::PacketType Packet_LobbyRelayResponseMyroomAction = (Zero::PacketType)(100+44);

static const Zero::PacketType Packet_LobbyRelayServerMoveStart = (Zero::PacketType)(100+45);

static const Zero::PacketType Packet_LobbyRelayResponseLobbyKey = (Zero::PacketType)(100+46);

static const Zero::PacketType Packet_LobbyRelayNotifyUserInfo = (Zero::PacketType)(100+47);

static const Zero::PacketType Packet_LobbyRelayNotifyUserList = (Zero::PacketType)(100+48);

static const Zero::PacketType Packet_LobbyRelayNotifyRoomList = (Zero::PacketType)(100+49);

static const Zero::PacketType Packet_LobbyRelayResponseChannelMove = (Zero::PacketType)(100+50);

static const Zero::PacketType Packet_LobbyRelayResponseLobbyMessage = (Zero::PacketType)(100+51);

static const Zero::PacketType Packet_LobbyRelayResponseBank = (Zero::PacketType)(100+52);

static const Zero::PacketType Packet_LobbyRelayNotifyJackpotInfo = (Zero::PacketType)(100+53);

static const Zero::PacketType Packet_LobbyRelayNotifyLobbyMessage = (Zero::PacketType)(100+54);

static const Zero::PacketType Packet_RoomRelayServerMoveStart = (Zero::PacketType)(100+55);

static const Zero::PacketType Packet_RelayRequestOutRoom = (Zero::PacketType)(100+56);

static const Zero::PacketType Packet_RelayResponseOutRoom = (Zero::PacketType)(100+57);

static const Zero::PacketType Packet_RelayRequestMoveRoom = (Zero::PacketType)(100+58);

static const Zero::PacketType Packet_RelayResponseMoveRoom = (Zero::PacketType)(100+59);

static const Zero::PacketType Packet_RelayGameRoomIn = (Zero::PacketType)(100+60);

static const Zero::PacketType Packet_RelayGameReady = (Zero::PacketType)(100+61);

static const Zero::PacketType Packet_RelayGameSelectOrder = (Zero::PacketType)(100+62);

static const Zero::PacketType Packet_RelayGameDistributedEnd = (Zero::PacketType)(100+63);

static const Zero::PacketType Packet_RelayGameActionPutCard = (Zero::PacketType)(100+64);

static const Zero::PacketType Packet_RelayGameActionFlipBomb = (Zero::PacketType)(100+65);

static const Zero::PacketType Packet_RelayGameActionChooseCard = (Zero::PacketType)(100+66);

static const Zero::PacketType Packet_RelayGameSelectKookjin = (Zero::PacketType)(100+67);

static const Zero::PacketType Packet_RelayGameSelectGoStop = (Zero::PacketType)(100+68);

static const Zero::PacketType Packet_RelayGameSelectPush = (Zero::PacketType)(100+69);

static const Zero::PacketType Packet_RelayGamePractice = (Zero::PacketType)(100+70);

static const Zero::PacketType Packet_GameRelayRoomIn = (Zero::PacketType)(100+71);

static const Zero::PacketType Packet_GameRelayRequestReady = (Zero::PacketType)(100+72);

static const Zero::PacketType Packet_GameRelayStart = (Zero::PacketType)(100+73);

static const Zero::PacketType Packet_GameRelayRequestSelectOrder = (Zero::PacketType)(100+74);

static const Zero::PacketType Packet_GameRelayOrderEnd = (Zero::PacketType)(100+75);

static const Zero::PacketType Packet_GameRelayDistributedStart = (Zero::PacketType)(100+76);

static const Zero::PacketType Packet_GameRelayFloorHasBonus = (Zero::PacketType)(100+77);

static const Zero::PacketType Packet_GameRelayTurnStart = (Zero::PacketType)(100+78);

static const Zero::PacketType Packet_GameRelaySelectCardResult = (Zero::PacketType)(100+79);

static const Zero::PacketType Packet_GameRelayFlipDeckResult = (Zero::PacketType)(100+80);

static const Zero::PacketType Packet_GameRelayTurnResult = (Zero::PacketType)(100+81);

static const Zero::PacketType Packet_GameRelayUserInfo = (Zero::PacketType)(100+82);

static const Zero::PacketType Packet_GameRelayNotifyIndex = (Zero::PacketType)(100+83);

static const Zero::PacketType Packet_GameRelayNotifyStat = (Zero::PacketType)(100+84);

static const Zero::PacketType Packet_GameRelayRequestKookjin = (Zero::PacketType)(100+85);

static const Zero::PacketType Packet_GameRelayNotifyKookjin = (Zero::PacketType)(100+86);

static const Zero::PacketType Packet_GameRelayRequestGoStop = (Zero::PacketType)(100+87);

static const Zero::PacketType Packet_GameRelayNotifyGoStop = (Zero::PacketType)(100+88);

static const Zero::PacketType Packet_GameRelayMoveKookjin = (Zero::PacketType)(100+89);

static const Zero::PacketType Packet_GameRelayEventStart = (Zero::PacketType)(100+90);

static const Zero::PacketType Packet_GameRelayEventInfo = (Zero::PacketType)(100+91);

static const Zero::PacketType Packet_GameRelayOver = (Zero::PacketType)(100+92);

static const Zero::PacketType Packet_GameRelayRequestPush = (Zero::PacketType)(100+93);

static const Zero::PacketType Packet_GameRelayResponseRoomMove = (Zero::PacketType)(100+94);

static const Zero::PacketType Packet_GameRelayPracticeEnd = (Zero::PacketType)(100+95);

static const Zero::PacketType Packet_GameRelayResponseRoomOut = (Zero::PacketType)(100+96);

static const Zero::PacketType Packet_GameRelayKickUser = (Zero::PacketType)(100+97);

static const Zero::PacketType Packet_GameRelayRoomInfo = (Zero::PacketType)(100+98);

static const Zero::PacketType Packet_GameRelayUserOut = (Zero::PacketType)(100+99);

static const Zero::PacketType Packet_GameRelayObserveInfo = (Zero::PacketType)(100+100);

static const Zero::PacketType Packet_GameRelayNotifyMessage = (Zero::PacketType)(100+101);

static const Zero::PacketType Packet_GameRelayNotifyJackpotInfo = (Zero::PacketType)(100+102);

static const Zero::PacketType Packet_RelayRequestLobbyEventInfo = (Zero::PacketType)(100+103);

static const Zero::PacketType Packet_LobbyRelayResponseLobbyEventInfo = (Zero::PacketType)(100+104);

static const Zero::PacketType Packet_RelayRequestLobbyEventParticipate = (Zero::PacketType)(100+105);

static const Zero::PacketType Packet_LobbyRelayResponseLobbyEventParticipate = (Zero::PacketType)(100+106);

static const Zero::PacketType Packet_GameRelayResponseRoomMissionInfo = (Zero::PacketType)(100+107);

static const Zero::PacketType Packet_ServerMoveStart = (Zero::PacketType)(100+108);

static const Zero::PacketType Packet_ServerMoveEnd = (Zero::PacketType)(100+109);

static const Zero::PacketType Packet_ResponseLauncherLogin = (Zero::PacketType)(100+110);

static const Zero::PacketType Packet_ResponseLauncherLogout = (Zero::PacketType)(100+111);

static const Zero::PacketType Packet_ResponseLoginKey = (Zero::PacketType)(100+112);

static const Zero::PacketType Packet_ResponseLobbyKey = (Zero::PacketType)(100+113);

static const Zero::PacketType Packet_ResponseLogin = (Zero::PacketType)(100+114);

static const Zero::PacketType Packet_NotifyLobbyList = (Zero::PacketType)(100+115);

static const Zero::PacketType Packet_NotifyUserInfo = (Zero::PacketType)(100+116);

static const Zero::PacketType Packet_NotifyUserList = (Zero::PacketType)(100+117);

static const Zero::PacketType Packet_NotifyRoomList = (Zero::PacketType)(100+118);

static const Zero::PacketType Packet_ResponseChannelMove = (Zero::PacketType)(100+119);

static const Zero::PacketType Packet_ResponseLobbyMessage = (Zero::PacketType)(100+120);

static const Zero::PacketType Packet_ResponseBank = (Zero::PacketType)(100+121);

static const Zero::PacketType Packet_NotifyJackpotInfo = (Zero::PacketType)(100+122);

static const Zero::PacketType Packet_NotifyLobbyMessage = (Zero::PacketType)(100+123);

static const Zero::PacketType Packet_GameRoomIn = (Zero::PacketType)(100+124);

static const Zero::PacketType Packet_GameRequestReady = (Zero::PacketType)(100+125);

static const Zero::PacketType Packet_GameStart = (Zero::PacketType)(100+126);

static const Zero::PacketType Packet_GameRequestSelectOrder = (Zero::PacketType)(100+127);

static const Zero::PacketType Packet_GameOrderEnd = (Zero::PacketType)(100+128);

static const Zero::PacketType Packet_GameDistributedStart = (Zero::PacketType)(100+129);

static const Zero::PacketType Packet_GameFloorHasBonus = (Zero::PacketType)(100+130);

static const Zero::PacketType Packet_GameTurnStart = (Zero::PacketType)(100+131);

static const Zero::PacketType Packet_GameSelectCardResult = (Zero::PacketType)(100+132);

static const Zero::PacketType Packet_GameFlipDeckResult = (Zero::PacketType)(100+133);

static const Zero::PacketType Packet_GameTurnResult = (Zero::PacketType)(100+134);

static const Zero::PacketType Packet_GameUserInfo = (Zero::PacketType)(100+135);

static const Zero::PacketType Packet_GameNotifyIndex = (Zero::PacketType)(100+136);

static const Zero::PacketType Packet_GameNotifyStat = (Zero::PacketType)(100+137);

static const Zero::PacketType Packet_GameRequestKookjin = (Zero::PacketType)(100+138);

static const Zero::PacketType Packet_GameNotifyKookjin = (Zero::PacketType)(100+139);

static const Zero::PacketType Packet_GameRequestGoStop = (Zero::PacketType)(100+140);

static const Zero::PacketType Packet_GameNotifyGoStop = (Zero::PacketType)(100+141);

static const Zero::PacketType Packet_GameMoveKookjin = (Zero::PacketType)(100+142);

static const Zero::PacketType Packet_GameEventStart = (Zero::PacketType)(100+143);

static const Zero::PacketType Packet_GameEventInfo = (Zero::PacketType)(100+144);

static const Zero::PacketType Packet_GameOver = (Zero::PacketType)(100+145);

static const Zero::PacketType Packet_GameRequestPush = (Zero::PacketType)(100+146);

static const Zero::PacketType Packet_GameResponseRoomMove = (Zero::PacketType)(100+147);

static const Zero::PacketType Packet_GamePracticeEnd = (Zero::PacketType)(100+148);

static const Zero::PacketType Packet_GameResponseRoomOut = (Zero::PacketType)(100+149);

static const Zero::PacketType Packet_GameKickUser = (Zero::PacketType)(100+150);

static const Zero::PacketType Packet_GameRoomInfo = (Zero::PacketType)(100+151);

static const Zero::PacketType Packet_GameUserOut = (Zero::PacketType)(100+152);

static const Zero::PacketType Packet_GameObserveInfo = (Zero::PacketType)(100+153);

static const Zero::PacketType Packet_GameNotifyMessage = (Zero::PacketType)(100+154);

static const Zero::PacketType Packet_ResponsePurchaseList = (Zero::PacketType)(100+155);

static const Zero::PacketType Packet_ResponsePurchaseAvailability = (Zero::PacketType)(100+156);

static const Zero::PacketType Packet_ResponsePurchaseReceiptCheck = (Zero::PacketType)(100+157);

static const Zero::PacketType Packet_ResponsePurchaseResult = (Zero::PacketType)(100+158);

static const Zero::PacketType Packet_ResponsePurchaseCash = (Zero::PacketType)(100+159);

static const Zero::PacketType Packet_ResponseMyroomList = (Zero::PacketType)(100+160);

static const Zero::PacketType Packet_ResponseMyroomAction = (Zero::PacketType)(100+161);

static const Zero::PacketType Packet_ResponseGameOptions = (Zero::PacketType)(100+162);

static const Zero::PacketType Packet_ResponseLobbyEventInfo = (Zero::PacketType)(100+163);

static const Zero::PacketType Packet_ResponseLobbyEventParticipate = (Zero::PacketType)(100+164);

static const Zero::PacketType Packet_GameResponseRoomMissionInfo = (Zero::PacketType)(100+165);

static const Zero::PacketType Packet_ServerMoveFailure = (Zero::PacketType)(100+166);

static const Zero::PacketType Packet_RequestLauncherLogin = (Zero::PacketType)(100+167);

static const Zero::PacketType Packet_RequestLauncherLogout = (Zero::PacketType)(100+168);

static const Zero::PacketType Packet_RequestLoginKey = (Zero::PacketType)(100+169);

static const Zero::PacketType Packet_RequestLobbyKey = (Zero::PacketType)(100+170);

static const Zero::PacketType Packet_RequestLogin = (Zero::PacketType)(100+171);

static const Zero::PacketType Packet_RequestLobbyList = (Zero::PacketType)(100+172);

static const Zero::PacketType Packet_RequestGoLobby = (Zero::PacketType)(100+173);

static const Zero::PacketType Packet_RequestJoinInfo = (Zero::PacketType)(100+174);

static const Zero::PacketType Packet_RequestChannelMove = (Zero::PacketType)(100+175);

static const Zero::PacketType Packet_RequestRoomMake = (Zero::PacketType)(100+176);

static const Zero::PacketType Packet_RequestRoomJoin = (Zero::PacketType)(100+177);

static const Zero::PacketType Packet_RequestRoomJoinSelect = (Zero::PacketType)(100+178);

static const Zero::PacketType Packet_RequestBank = (Zero::PacketType)(100+179);

static const Zero::PacketType Packet_GameRoomIn = (Zero::PacketType)(100+180);

static const Zero::PacketType Packet_GameReady = (Zero::PacketType)(100+181);

static const Zero::PacketType Packet_GameSelectOrder = (Zero::PacketType)(100+182);

static const Zero::PacketType Packet_GameDistributedEnd = (Zero::PacketType)(100+183);

static const Zero::PacketType Packet_GameActionPutCard = (Zero::PacketType)(100+184);

static const Zero::PacketType Packet_GameActionFlipBomb = (Zero::PacketType)(100+185);

static const Zero::PacketType Packet_GameActionChooseCard = (Zero::PacketType)(100+186);

static const Zero::PacketType Packet_GameSelectKookjin = (Zero::PacketType)(100+187);

static const Zero::PacketType Packet_GameSelectGoStop = (Zero::PacketType)(100+188);

static const Zero::PacketType Packet_GameSelectPush = (Zero::PacketType)(100+189);

static const Zero::PacketType Packet_GamePractice = (Zero::PacketType)(100+190);

static const Zero::PacketType Packet_GameRoomOut = (Zero::PacketType)(100+191);

static const Zero::PacketType Packet_GameRoomMove = (Zero::PacketType)(100+192);

static const Zero::PacketType Packet_RequestPurchaseList = (Zero::PacketType)(100+193);

static const Zero::PacketType Packet_RequestPurchaseAvailability = (Zero::PacketType)(100+194);

static const Zero::PacketType Packet_RequestPurchaseReceiptCheck = (Zero::PacketType)(100+195);

static const Zero::PacketType Packet_RequestPurchaseResult = (Zero::PacketType)(100+196);

static const Zero::PacketType Packet_RequestPurchaseCash = (Zero::PacketType)(100+197);

static const Zero::PacketType Packet_RequestMyroomList = (Zero::PacketType)(100+198);

static const Zero::PacketType Packet_RequestMyroomAction = (Zero::PacketType)(100+199);

static const Zero::PacketType Packet_RequestGameOptions = (Zero::PacketType)(100+200);

static const Zero::PacketType Packet_RequestLobbyEventInfo = (Zero::PacketType)(100+201);

static const Zero::PacketType Packet_RequestLobbyEventParticipate = (Zero::PacketType)(100+202);

}

