using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
class CardCompare implements Comparator<Card> {

public int compare(Card card1, Card card2)
{
if (card1.getNumber() < card2.getNumber()) return 1;
if (card1.getNumber() > card2.getNumber()) return -1;
return 0;
}
}*/

public class UserDeck
{

    private List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cards;
    public UserDeck()
    {
        cards = new List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
    }
    public UserDeck(List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cards)
    {
        this.cards = cards;
    }
    public List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> getCards()
    {
        return cards;
    }
    public void add(ClassLibraryCardInfo.CardInfo.sCARD_INFO card)
    {
        cards.Add(card);
    }

    public void remove(int index)
    {
        cards.RemoveAt(index);
    }
    public void removeByIndex(int index)
    {
        cards.RemoveAt(index);
    }

    public void setCards(List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cards)
    {
        this.cards = cards;
    }
    public ClassLibraryCardInfo.CardInfo.sCARD_INFO getCardByIndex(int index)
    {
        return this.cards[index];
    }

    public int size()
    {
        return cards.Count;
    }
    public ClassLibraryCardInfo.CardInfo.sCARD_INFO get(int index)
    {
        return cards[index];
    }

}

