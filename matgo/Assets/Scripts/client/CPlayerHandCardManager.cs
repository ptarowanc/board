using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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


    public void remove(CCardPicture card_picture)
    {
        bool result = this.cards.Remove(card_picture);
        if (!result)
        {
            UnityEngine.Debug.LogError("Cannot remove the card!");
        }
    }


    public int get_card_count()
    {
        return this.cards.Count;
    }


    public CCardPicture get_card(int index)
    {
        return this.cards[index];
    }


    public CCardPicture find_card(byte number, PAE_TYPE pae_type, byte position)
    {
        return this.cards.Find(obj => obj.card.is_same(number, pae_type, position));
    }

    public CCardPicture find_card(byte number)
    {
        return this.cards.Find(obj => obj.card.is_same_number(number));
    }

    public CCardPicture find_bomb_card()
    {
        return this.cards.Find(obj => obj.isBomb == true);
    }

    public int get_card_index(byte number, byte position)
    {
        return this.cards.FindIndex(obj => obj.card.is_same(number, position));
    }
    public int get_card_index(byte number)
    {
        return this.cards.FindIndex(obj => obj.card.is_same_number(number));
    }

    public int get_same_number_count(byte number)
    {
        List<CCardPicture> same_cards = this.cards.FindAll(obj => obj.is_same(number));
        return same_cards.Count;
    }


    public void sort_by_number()
    {
        this.cards.Sort((CCardPicture lhs, CCardPicture rhs) =>
        {
            if (lhs.card.number == rhs.card.number)
            {
                if (lhs.card.position < rhs.card.position)
                {
                    return -1;
                }
                else if (lhs.card.position > rhs.card.position)
                {
                    return 1;
                }
            }
            else if (lhs.card.number < rhs.card.number)
            {
                return -1;
            }
            else if (lhs.card.number > rhs.card.number)
            {
                return 1;
            }

            return 0;
        });
    }

    public void sort_by_bomb()
    {
        this.cards = this.cards.OrderBy(x => x.isBomb ? 0 : 1).Reverse().ToList();
    }

    public void enable_all_colliders(bool flag)
    {
        for (int i = 0; i < this.cards.Count; ++i)
        {
            this.cards[i].enable_collider(flag);
        }
    }
}
