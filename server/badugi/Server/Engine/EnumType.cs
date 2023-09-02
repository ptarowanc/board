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
    public enum ChannelType : int
    {
        None,
        Free,
        Charge
    }
    public enum GameRound
    {
        START,
        MORNING,
        AFTERNOON,
        EVENING,
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
        BASE
    }
    public enum HAND_RANKING : byte
    {
        NONE,
        NOBASE,
        TWOBASE,
        BASE,
        MADE,
        THIRD,
        SECOND,
        GOLF
    }

    public enum CARD_SUIT : byte
    {
        SPADE,
        DIAMOND,
        HEART,
        CLOVER
    }

    public enum CHANGECARD : byte
    {
        CHANGE,
        PASS
    }
    public enum EVENT_JACKPOT_TYPE : byte
    {
        NONE,

        X1,    // 50배
        X2,    // 100배
        X3,    // 200배

        Z1,    // 돌발 100배

        END
    }
    public enum GAME_RULE_TYPE : byte
    {
        NONE,
        BADUGI,
        HOLDEM_BADUGI,
        END
    }
}
