using System;

// 플레이어가 낸 카드에 대한 결과.
public enum PLAYER_SELECT_CARD_RESULT : byte
{
    // 완료.
    COMPLETED,
    //보너스 카드를 꺼냄
    BONUS_CARD,

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
    SHAKING,

    CHONGTONG
}

public enum PLAYER_FLOOR_CHECK : byte
{
    NONE,
    FIVE_KWANG,
    FORE_KWANG,
    THREE_KWANG,
    BI_THREE_KWANG,
    GODORI,
    HONGDAN,
    CHODAN,
    CHUNGDAN,
    MUNGTTA,
}
public enum BAKCHECK : byte
{
    NONE,
    PIBAK,
    GOBAK,
    KWANGBAK
}

public enum MISSION : byte
{
    NONE,
    FIVEKWANG,
    KWANGTTANG,
    GODORI,
    HONGDAN,
    CHODAN,
    CHUNGDAN,
    WOL1_4,
    WOL2_4,
    WOL3_4,
    WOL4_4,
    WOL5_4,
    WOL6_4,
    WOL7_4,
    WOL8_4,
    WOL9_4,
    WOL10_4,
    WOL11_4,
    WOL12_4,
    WOL1_2,
    WOL2_2,
    WOL3_2,
    WOL4_2,
    WOL5_2,
    WOL6_2,
    WOL7_2,
    WOL8_2,
    WOL9_2,
    WOL10_2,
    WOL11_2,
    WOL12_2,
    BAE2,
    BAE3,
    BAE4,
    WHALBIN
}
public enum TURN_RESULT_TYPE : byte
{
    // 일반 카드를 낸 후의 결과.
    RESULT_OF_NORMAL_CARD,

    // 폭탄 카드를 낸 후의 결과.
    RESULT_OF_BOMB_CARD
}
public enum FLIP_TYPE : byte
{
    FLIP_NORMAL,
    FLIP_BOOM,
    FLIP_BONUS
}