using System;
using System.Collections;
using System.Collections.Generic;
using ClassLibraryCardInfo;
public class CCardManager
{
    public List<CardInfo.sCARD_INFO> cards { get; private set; }

	public CCardManager()
	{
		this.cards = new List<CardInfo.sCARD_INFO>();
	}


    public void make_all_cards()
    {
		this.cards.Clear();
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ClassLibraryCardInfo.CardInfo.sCARD_INFO card = new ClassLibraryCardInfo.CardInfo.sCARD_INFO();
                card.Init(i + 1, j + 1, 0);
                cards.Add(card);
            }
        }
    }


    public void shuffle()
    {
        CHelper.Shuffle<CardInfo.sCARD_INFO>(this.cards);

    }
}
