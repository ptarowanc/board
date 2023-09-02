using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engine
{
    public class CPlayerOrderManager
    {
        public List<CCard> order_cards{ get; private set; }

        public CPlayerOrderManager()
        {
            order_cards = new List<CCard>();
        }


        public void reset(CGostopEngine engine, byte firstTurnIndex)
        {
            engine.get_random_order_cards(this.order_cards, firstTurnIndex);
        }
    }
}
