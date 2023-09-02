using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engine
{
    public class CFloorSlot
    {
        public byte slot_position { get; private set; }
        public List<CCard> cards { get; private set; }
        public CFloorSlot(byte position)
        {
            this.cards = new List<CCard>();
            this.slot_position = position;

            reset();
        }
        public void reset()
        {
            this.cards.Clear();
        }
        public bool is_same(byte number, byte position)
        {
            if (this.cards.Count <= 0)
            {
                return false;
            }

            return this.cards.Exists(obj => obj.is_same(
                    number,
                    position));
        }
        public bool is_same_slot(byte slot_number)
        {
            if (this.cards.Count <= 0)
            {
                return false;
            }

            return this.cards.Exists(obj => obj.is_same_slot(slot_number));
        }
        public byte count(byte number)
        {
            if (this.cards.Count <= 0)
            {
                return 0;
            }

            if(this.cards[0].number != number)
            {
                return 0;
            }

            return (byte)cards.Count;
        }
        public void add_card(CCard card)
        {
            this.cards.Add(card);
        }
        public void remove_card(CCard card)
        {
            this.cards.Remove(card);
        }
        public bool is_empty()
        {
            return this.cards.Count <= 0;
            //return this.cards.FindAll(c=>c.is_bonus_card() == false).Count <= 0;
        }
    }
}
