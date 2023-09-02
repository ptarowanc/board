using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Engine
{
    public enum ChannelKind : int
    {
        None = 0,
        유료초보채널,
        유료중수채널,
        유료고수채널,
        유료자유채널,
        무료1채널,
        무료2채널,
        무료자유1채널,
        무료자유2채널,
        Max
    }
    public enum ChannelType : byte
    {
        None,
        Free,
        Charge
    }
    public enum StakeType : int
    {
        None,
        Stake1,
        Stake2,
        Stake3,
        Stake4,
        Stake5,
        Stake6
    }
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

        //총통
        CHONGTONG,

        // 첫 뻑
        FIRST_PPUK,
        // 2연 뻑
        SECOND_PPUK,
        // 첫 따닥
        FIRST_DDADAK,
        // 2연 뻑
        THIRD_PPUK,

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

    // 게임 결과.
    public enum GAME_RESULT_TYPE : byte
    {
        NONE,

        DRAW,                       // 무승부(나가리)
        START_PLAYER_WIN,           // 선 승
        LAST_PLAYER_WIN,            // 후 승
        START_PLAYER_CHONGTONG,     // 선 총통
        LAST_PLAYER_CHONGTONG,      // 후 총통
        START_PLAYER_THREEPPUK,     // 선 3뻑
        LAST_PLAYER_THREEPPUK,      // 후 3뻑

        END
    }

    // 미션 결과
    public enum MISSION_RESULT_TYPE : byte
    {
        NONE,

        SUCCESS,                // 미션 성공
        FAIL,                   // 미션 실패
        WHALBIN_MYSUCCESS,      // 활빈당. 해당 플레이어 성공
        WHALBIN_ENEMYSUCCESS,   // 활빈당. 상대 플레이어 성공

        END
    }

    // 탑쌓기 이벤트
    public enum MISSION_TAPSSAHGI_TYPE : byte
    {
        NONE,

        // Easy
        HONGDAN,        // 홍단!
        CHEONGDAN,      // 청단!
        CHODAN,         // 초단!
        GODORI,         // 고도리!
        FIRST_PPUK,     // 첫뻑!
        FIVE_GO,        // 5고!
        SAM_KWANG,      // 삼광!
        BI_SAM_KWANG,   // 비삼광!
        SA_KWANG,       // 사광!
        JA_PPUK,        // 자뻑!
        KISS,           // 쪽!
        DDADAK,         // 따닥!

        // Hard
        MUNGTTA,        // 멍따!
        THREE_WIN,      // 3연승!
        GO_BAK_WIN,     // 고박승(역전승)!
        O_KWANG,        // 오광!
        THREE_PPUK,     // 3뻑!
        THREE_SHAKE,    // 3흔들기 !
        CHONGTONG,      // 총통!
        NAGARI,         // 나가리(무승부)!

        END
    }

    // 규칙 머니
    public enum RULE_MONEY_TYPE : byte
    {
        NONE,
        
        FIRST_PPUK,     // 첫뻑
        CHAIN_PPUK,     // 2연뻑
        THREE_PPUK,     // 3연뻑

        END
    }
}
