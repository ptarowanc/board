using System;
using System.Collections;

namespace Server.Engine
{
    public enum PAE_TYPE : byte
    {
        PEE,    //피
        KWANG,  //광
        TEE,    //띠
        YEOL    //열
    }
    public enum CARD_STATUS : byte
    {
        NONE,
        GODORI,         // 고도리
        TWO_PEE,        // 쌍피
        THREE_PEE,      // 쓰리피
        CHEONG_DAN,     // 청단
        HONG_DAN,       // 홍단
        CHO_DAN,        // 초단
        KOOKJIN         // 국진
    }
    public enum CARD_MADE : int
    {
        NONE            = 0,
        FIVE_KWANG      = 300,
        FOUR_KWANG      = 120,//오광깨졌으면 60
        THREE_KWANG     = 110,
        BI_THREE_KWANG  = 80,
        GODORI          = 150,
        HONGDAN         = 100,
        CHODAN          = 100,
        CHUNGDAN        = 100
    }
    public class CCard
    {
        // 특정 카드에 대한 디파인.
        public static readonly byte BEE_KWANG = 11;

        // 0 ~ 11 number.
        public byte number { get; private set; }

        // pae type.
        public PAE_TYPE pae_type { get; private set; }

        // 1,2,3,4 position.
        public byte position { get; private set; }

        public CARD_STATUS status { get; private set; }

        public byte slot_number { get; private set; }


        public CCard(byte number, PAE_TYPE pae_type, byte position)
        {
            this.number = number;
            this.pae_type = pae_type;
            this.position = position;
            this.status = CARD_STATUS.NONE;
            this.slot_number = number;
        }


        public void set_card_status(CARD_STATUS status)
        {
            this.status = status;
        }

        public void set_card_slot(byte slot)
        {
            this.slot_number = slot;
        }


        public void change_pae_type(PAE_TYPE pae_type_to_change)
        {
            this.pae_type = pae_type_to_change;
        }


        public bool is_bonus_card()
        {
            return this.number == 12;
        }

        public bool is_kookjin_card()
        {
            return this.number == 8 && this.position == 0;
        }


        public bool is_same(byte number, PAE_TYPE pae_type, byte position)
        {
            return this.number == number &&
                this.pae_type == pae_type &&
                this.position == position;
        }

        public bool is_same(PAE_TYPE pae_type)
        {
            return this.pae_type == pae_type;
        }

        public bool is_same(byte number, byte position)
        {
            return this.number == number &&
                this.position == position;
        }
        public bool is_same(byte number, PAE_TYPE pae_type)
        {
            return this.number == number &&
                this.pae_type == pae_type;
        }

        public bool is_same_number(byte number)
        {
            return this.number == number;
        }
        public bool is_same_slot(byte slot_number)
        {
            return this.slot_number == slot_number;
        }


        public bool is_same_status(CARD_STATUS status)
        {
            return this.status == status;
        }
    }

}
