using System;
using System.Collections;

// 카드 타입.
public enum PAE_TYPE : byte
{
    PEE,    //피
    KWANG,  //광
    TEE,    //띠
    YEOL,    //열
    BOMB
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

    public CCard(byte number, byte position)
    {
        this.number = number;
        this.position = position;
        this.status = CARD_STATUS.NONE;
        this.slot_number = number;
        PAE_TYPE pae_type_temp = PAE_TYPE.PEE;
        switch (this.number)
        {
            case 0:
                if(this.position == 0)
                    pae_type_temp = PAE_TYPE.KWANG;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 1:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 2:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.KWANG;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 3:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 4:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 5:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 6:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 7:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.KWANG;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 8:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 9:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 10:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.KWANG;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.PEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 11:
                if (this.position == 0)
                    pae_type_temp = PAE_TYPE.KWANG;
                else if (this.position == 1)
                    pae_type_temp = PAE_TYPE.YEOL;
                else if (this.position == 2)
                    pae_type_temp = PAE_TYPE.TEE;
                else if (this.position == 3)
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 12:
                    pae_type_temp = PAE_TYPE.PEE;
                break;
            case 13:
                    pae_type_temp = PAE_TYPE.BOMB;
                break;
        }
        this.pae_type = pae_type_temp;
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


    public static bool is_bonus_card(byte number)
    {
        return number == 12;
    }

    public bool is_bonus_card()
    {
        return this.number == 12;
    }
    public bool is_bomb_card()
    {
        return this.number == 13;
    }

    public bool is_same(byte number, PAE_TYPE pae_type, byte position)
	{
		return this.number == number &&
			this.pae_type == pae_type &&
			this.position == position;
    }
    public bool is_same(byte number, PAE_TYPE pae_type)
    {
        return this.number == number &&
            this.pae_type == pae_type;
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

    public bool is_same_number(CCard card, bool isBug = false)
	{
        if (isBug)
        {
            return card.number == number &&
            card.pae_type == pae_type &&
            card.position == position;
        }
        else return card.number == number;
    }

    public bool is_same_number(byte number)
    {
        return this.number == number;
    }

    public bool is_same_status(CARD_STATUS status)
	{
		return this.status == status;
	}

    public void setnumber(byte number)
    {
        this.number = number;
    }
}
