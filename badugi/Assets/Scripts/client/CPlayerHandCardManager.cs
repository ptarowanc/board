using System;
using System.Collections;
using System.Collections.Generic;
using ClassLibraryCardInfo;
using UnityEngine;
using UnityEngine.UI;

public class CPlayerHandCardManager
{
    List<CCardPicture> cards;

    public CPlayerHandCardManager()
    {
        this.cards = new List<CCardPicture>();
    }


    public void reset()
    {
        this.cards.Clear();
    }


    public void add(CCardPicture card_picture)
    {
        this.cards.Add(card_picture);
    }


    public void remove(byte slot)
    {
        if (slot < cards.Count)
            this.cards[slot] = null;
    }

    //2019-05-17 sungwon  추가
    public void remove_playercard(byte slot)
    {
        if (slot >= cards.Count)
            return;
        if (this.cards[slot].server_slot == 255)
        {
            this.cards[slot] = null;
            return;
        }

        for (int i = 0; i < this.cards.Count; ++i)
        {
            if (this.cards[i] != null &&
                this.cards[i].server_slot == slot)
            {
                this.cards[i] = null;
                break;
            }
        }
    }

    public void set_card(byte slot, CCardPicture card_picture)
    {
        if (slot >= cards.Count)
            return;
        this.cards[slot] = card_picture;
    }

    public void set_cardinfo(byte slot, CardInfo.sCARD_INFO card)
    {
        if (slot >= cards.Count)
            return;
        this.cards[slot].update_card(card, PlayGameUI.Instance.get_card_sprite(card));
    }
    public int get_card_count()
    {
        return this.cards.Count;
    }


    public CCardPicture get_card(int index)
    {
        if (index >= cards.Count)
            return null;

        return this.cards[index];
    }

    public bool is_card_change()
    {
        return this.cards.Exists(c => c.select == true);
    }

    public void set_card_select(bool select)
    {
        cards.ForEach(c => c.SetSelect(select));
    }


    public int get_same_number_count(byte number)
    {
        List<CCardPicture> same_cards = this.cards.FindAll(obj => obj.is_same(number));
        return same_cards.Count;
    }

    public void enable_all_colliders(bool flag)
    {
        for (int i = 0; i < this.cards.Count; ++i)
        {
            this.cards[i].enable_collider(flag);
        }
    }

    public List<CardInfo.sCARD_INFO> getCards()
    {
        List<CardInfo.sCARD_INFO> cardInfos = new List<CardInfo.sCARD_INFO>();
        for (int i = 0; i < this.cards.Count; ++i)
        {
            cardInfos.Add(this.cards[i].card);
        }
        return cardInfos;
    }

    public CCardPicture get_card_from_slot(byte i)
    {
        if (i >= cards.Count)
            return null;

        return this.cards[i];
    }

    //2019-05-17 sungwon 추가
    public CCardPicture get_card_from_slot_playercard(byte slot)
    {
        for (int i = 0; i < this.cards.Count; ++i)
        {
            if (this.cards[i] != null &&
                this.cards[i].server_slot == slot)
            {
                return this.cards[i];
            }
        }

        if (slot >= cards.Count)
            return null;

        return this.cards[slot];
    }

    public List<CCardPicture> getCardPictures()
    {
        return this.cards;
    }

    public List<CCardPicture> getSelectCards()
    {
        List<CCardPicture> selectCards = new List<CCardPicture>();
        for (int i = 0; i < this.cards.Count; ++i)
        {
            if (this.cards[i].select == true)
            {
                selectCards.Add(this.cards[i]);
            }
        }
        return selectCards;
    }

    //--------------------
    //2019-05-14 sungwon 카드 교환할때 정렬 추가
    public void Sorting()
    {
        byte iSlotCount = 0;
        for (int i = 0; i < cards.Count; ++i)
        {
            if (cards[i] != null && !cards[i].select)
            {
                cards[i].set_slot_index(iSlotCount);
                cards[iSlotCount] = cards[i];
                iSlotCount++;
            }
        }
        for (int i = iSlotCount; i < cards.Count; ++i)
        {
            remove((byte)i);
        }
    }
    //--------------------
}
