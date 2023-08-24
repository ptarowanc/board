using System;

/// <summary>
/// 프로토콜 정의.
/// 서버에서 클라이언트로 가는 패킷 : S -> C
/// 클라이언트에서 서버로 가는 패킷 : C -> S
/// </summary>
public enum PROTOCOL : short
{
	BEGIN = 0,

	// 시스템 프로토콜.
    LOCAL_SERVER_STARTED = 1,	// S -> C

	// 게임 프로토콜.
	READY_TO_START = 10,		// C -> S 클라이언트 게임준비
	BEGIN_CARD_INFO = 11,		// S -> C 서버가 각 클라에게 시작 카드 정보 줌
	DISTRIBUTED_ALL_CARDS = 12,	// C -> S 클라가 모든 카드 분배됨을 알림

	SELECT_CARD_REQ = 13,		// C -> S 클라가 카드 선택 요청
	SELECT_CARD_ACK = 14,		// S -> C 서버가 클라에게 카드 선택 응답

	// 플레이어가 두개의 카드 중 하나를 선택해야 하는 경우.
    //CHOICE_ONE_CARD = 15,		// S -> C   // 사용하지 않음.
	CHOOSE_CARD = 16,			// C -> S  클라가 두개중 하나 카드 선택

    FLIP_BOMB_CARD_REQ = 17,	// C -> S  클라가 폭탄 카드 뒤집음 요청

	FLIP_DECK_CARD_REQ = 18,	// C -> S  클라가 덱카드 뒤집음 요청
	FLIP_DECK_CARD_ACK = 19,	// S -> C  서버가 덱카드 뒤집음 응답

	TURN_RESULT = 20,			// S -> C  서버가 턴 결과

	ASK_GO_OR_STOP = 21,		// S -> C  서버가 고/스톱 물음
	ANSWER_GO_OR_STOP = 22,		// C -> S  클라가 고/스톱 응답
	NOTIFY_GO_COUNT = 23,		// S -> C  서버가 고 횟수 알림

	UPDATE_PLAYER_STATISTICS = 24,	// S -> C 서버가 플레이어 통계 알림

	ASK_KOOKJIN_TO_PEE = 25,		// S -> C 서버가 국진을 피로 물음
	ANSWER_KOOKJIN_TO_PEE = 26,		// C -> S 클라가 국진을 피로 알림

	MOVE_KOOKJIN_TO_PEE = 27,		// S -> C 서버가 국진을 피로 이동

	GAME_RESULT = 28,				// S -> C 서버가 게임결과

	PLAYER_ORDER_RESULT = 29,		// S -> C 서버가 플레이어 선후 결과 알림

	START_TURN = 98,
	TURN_END = 100,

	END
}

public enum BettingType : byte
{
    CALL = 0,
    BBING = 1,
    QUATER = 2,
    HALF = 3,
    DIE = 4,
    CHECK = 5,
    DDADANG = 6
}

public enum GameRound
{
    START,
    MORNING,
    AFTERNOON,
    EVENING,
    END
}
public enum BETTING : byte
{
    CALL,
    BBING,
    QUATER,
    HARF,
    DIE,
    CHECK,
    DDADDANG,
    ERR,
}
public enum AI_BETTING : byte
{
    RAISE = 1,
    CALL = 2,
    BBING = 3,
    CHECK = 4,
    DIE = 5,
    ERR
}

public enum CHANGECARD:byte
{
    CHANGE,
    PASS
}

public enum BPROTOCOL : short
{
    SERVER_START = 1,
    SERVER_SET_BOSS,
    PLAYER_READY,
    SERVER_GAMESTART,
    SERVER_GAMESTARTBETTING,
    PLAYER_BASEBETT,
    SERVER_DEALCARDTOALLUSER,
    PLAYER_ALLDEALCARD,

    SERVER_CHANGE_TURN,//보스부터 차례로 턴을 돌린다
    SERVER_REQ_BETTING,//서버에서 베팅 순서에 따라 가능한 베팅을 알려준다
    PLAYER_BETTING,
    SERVER_RES_BETTING,//베팅의 결과를 갱신한다   
     
    SERVER_CHANGE_ROUND,//아침,점심,저녘 라운드 교체
    SERVER_REQ_CHANGE_CARD,//서버에서 턴순서에 따라 카드 교체알려줌
    PLAYER_CHANGE_CARD,
    SERVER_RES_CHANGE_CARD,//교체카드시 새카드 알려줌

    SERVER_REQ_CARD_OPEN,
    PLAYER_CARD_OPEN,

    SERVER_GAMESTATICS,

    SERVER_GAME_END,
    SERVER_GAME_RESULT,
}

public enum dPACKETS_BADUKI
{

    dPACKET_UPDATE_MEMBER = 10,
    dPACKET_ADD_MEMBER = 11,
    dPACKET_REMOVE_MEMBER = 12,
    dPACKET_ROOM_INFO = 13,
    dPACKET_QUICK_JOIN = 14,

    dPACKET_RESET_GAME = 15,
    dPACKET_CARD_INFO = 16,
    dPACKET_START_GAME = 17,
    dPACKET_UPDATE_CARD = 18,
    dPACKET_UPDATE_CARDSTATE = 19,
    dPACKET_CHANGE_TURN = 20,
    dPACKET_SET_BOSS = 21,

    dPACKET_UPDATE_TABLEMONEY = 22,
    dPACKET_UPDATE_CALLMONEY = 23,
    dPACKET_UPDATE_TOTALBETMONEY = 24,
    dPACKET_UPDATE_BBINGMONEY = 25,
    dPACKET_UPDATE_MONEY = 26,
    dPACKET_UPDATE_ACHIEVE = 27,

    dPACKET_BTN_DIE = 28,
    dPACKET_BTN_CHECK = 29,
    dPACKET_BTN_CALL = 30,
    dPACKET_BTN_BBING = 31,
    dPACKET_BTN_DDADANG = 32,
    dPACKET_BTN_QUARTER = 33,
    dPACKET_BTN_HALF = 34,
    dPACKET_BTN_FULL = 35,
    dPACKET_BTN_ENABLE = 36,

    dPACKET_GAME_END = 37,
    dPACKET_GIVEUP_END = 38,
    dPACKET_OUTROOM_RESERVED = 39,
    dPACKET_UPDATE_JACKPOT = 40,
    dPACKET_SET_ALLIN = 41,
    dPACKET_OPEN_PLAYERCARD = 42,
    dPACKET_EVENT_MSG = 43,
    dPACKET_GAMEPLAY_STATE = 44,

    dPACKET_CHANGE_SELECTTURN = 45,
    dPACKET_CARDCHANGE_OK = 46,
    dPACKET_CARDCHANGE_PASS = 47,
    dPACKET_CHANGECARD_CNT = 48,
    dPACKET_CHANGECARD_DIE = 49,

    dPACKET_ONLASTBET = 50,
    dPACKET_SHOWDOWN_OPEN = 53,
    dPACKET_NOTICE_STRING = 55,
    dPACKET_CHANNEL_INFO = 56,
    dPACKET_SELECT_CHANNEL = 57,
    dPACKET_GOTOCHANNEL = 58,
    dPACKET_RETURN_CHANNEL = 59,
    dPACKET_REFRESH_CHANNEL = 60,
    dPACKET_USER_LIST = 61,
    dPACKET_ADD_USERLIST = 62,
    dPACKET_DEL_USERLIST = 63,
    dPACKET_LOBBY_USERLIST = 64,
    dPACKET_GET_USERLIST = 65,
    dPACKET_INVITE = 66,
    dPACKET_INVITE_ACCEPT = 67,
    dPACKET_INVITE_IGNORE = 68,
    dPACKET_GET_USERINFO = 69,
    dPACKET_ADD_ROOM = 70,
    dPACKET_REMOVE_ROOM = 71,
    dPACKET_INVALID_ROOMPWD = 72,
    dPACKET_CHAT = 73,
    dPACKET_P2P_CHAT = 74,
    dPACKET_SET_OPTION = 75,
    dPACKET_UPDATE_ROOM = 76,
    dPACKET_SHOWDOWN_END = 77,
    dPACKET_BET_ON = 78,
    dPACKET_SET_AUTOBET = 79,
    dPACKET_REFILL_FREE = 80,
    dPACKET_SET_WAITPLAYER = 81,
    dPACKET_JOIN_PLAY = 82,
    dPACKET_JOIN_WAIT = 83,
    dPACKET_CREATE_POT = 84,
    dPACKET_DESTROY_POT = 85,
    dPACKET_UPDATE_POT_MONEY = 86,
    dPACKET_ADD_MAIN_POT_MONEY = 87,
    dPACKET_DISTURIBUTE_POT = 88,
    dPACKET_DISTURIBUTE_POT_END = 89,
    dPACKET_DISTURIBUTE_POT_RECEIVE = 90,
    dPACKET_OVERBET_POT_DELETE = 91,
    dPACKET_SENDRANKINFO = 92,
    dPACKET_BTN_PASS = 93,
    dPACKET_BTN_ALLIN = 94,

    dPACKET_MODIFYSAFEMONEY = 103,
    dPACKET_EXCHANGESAVEMONEY = 104,

   
}



// 플레이어가 낸 카드에 대한 결과. 먹는 카드중 선택해야할때
public enum PLAYER_SELECT_CARD_RESULT : byte
{
	// 완료.
	COMPLETED,

	// 카드 한장 선택해야 함(플레이어가 낸 경우).
	CHOICE_ONE_CARD_FROM_PLAYER,

	// 카드 한장 선택해야 함(덱에서 뒤집은 경우).
	CHOICE_ONE_CARD_FROM_DECK,

	// 에러.
	ERROR_INVALID_CARD
}


// 카드 이벤트 정의.
public enum CARD_EVENT_TYPE : byte
{
	NONE,

	// 쪽.
	KISS,

	// 뻑.
	PPUK,

	// 따닥.
	DDADAK,

	// 폭탄.
	BOMB,

	// 싹쓸이.
	CLEAN,

	// 뻑 먹기.
	EAT_PPUK,

	// 자뻑.
	SELF_EAT_PPUK,

	// 흔들기.
	SHAKING
}


public enum TURN_RESULT_TYPE : byte
{
    // 일반 카드를 낸 후의 결과.
    RESULT_OF_NORMAL_CARD,

    // 폭탄 카드를 낸 후의 결과.
    RESULT_OF_BOMB_CARD
}
