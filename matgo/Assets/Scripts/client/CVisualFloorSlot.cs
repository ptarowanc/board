﻿using System;
using System.Collections;
using System.Collections.Generic;

public class CVisualFloorSlot
{
	public byte ui_slot_position { get; private set; }
	public byte card_number { get; private set; }
	List<CCardPicture> card_pictures;


	public CVisualFloorSlot(byte ui_slot_position, byte card_number)
	{
		this.ui_slot_position = ui_slot_position;
		this.card_number = card_number;
		this.card_pictures = new List<CCardPicture>();
	}


	public void reset()
	{
		this.card_number = byte.MaxValue;
		this.card_pictures.Clear();
	}


	public void add_card(CCardPicture card_pic)
	{
		this.card_number = card_pic.card.number;
		this.card_pictures.Add(card_pic);
	}

    public void add_bonusCard(CCardPicture card_pic, byte num)
    {
        this.card_number = num;
        this.card_pictures.Add(card_pic);
    }

    public void setcard_number(byte number)
    {
        this.card_number = number;
    }
	public void remove_card(CCardPicture card_pic)
	{
		this.card_pictures.Remove(card_pic);
		if (this.card_pictures.Count <= 0)
		{
			this.card_number = byte.MaxValue;
		}else
        {
            this.card_number = card_pictures[card_pictures.Count - 1].card.number;
        }
	}


	public int get_card_count()
	{
		return this.card_pictures.Count;
	}
    public bool is_same_card(CCard card, bool isBug = false)
    {
        CCardPicture cardpic = card_pictures.Find(obj => obj.card.is_same_number(card, isBug));

        if(cardpic != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public bool is_same_card(byte number)
	{
		return this.card_number == number;
	}


	public CCardPicture find_card(CCard card)
	{
		return this.card_pictures.Find(obj =>
			obj.card.is_same(card.number, card.pae_type, card.position));
	}


	public CCardPicture get_first_card()
	{
		if (get_card_count() <= 0)
		{
			return null;
		}

		return this.card_pictures[0];
	}


	public List<CCardPicture> get_cards()
	{
		return this.card_pictures;
	}
}
