using System.Collections.Generic;

public class Dealer
{

    public static int DEFINED_CARD_SIZE_PER_USER = 4;
    private List<UserAgent> userList;
    private int startIndex;
    private List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> deck;
    private List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> exchangedDeck;

    public Dealer(List<UserAgent> userList, int startIndex)
    {
        this.userList = userList;
        this.startIndex = startIndex;
        this.deck = createCards();
        this.exchangedDeck = new List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
    }

    public List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> createCards()
    {
        List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> deck = new List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ClassLibraryCardInfo.CardInfo.sCARD_INFO card = new ClassLibraryCardInfo.CardInfo.sCARD_INFO();
                card.Init(j, i, 0);
                deck.Add(card);
            }
        }
        return deck;
    }

    public void suffleCards()
    {
        CHelper.Shuffle<ClassLibraryCardInfo.CardInfo.sCARD_INFO>(this.deck);
    }
    public void fill_to(Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO> target)
    {
        this.deck.ForEach(obj => target.Enqueue(obj));
    }
    public void shuffleExchangeCards()
    {
        CHelper.Shuffle<ClassLibraryCardInfo.CardInfo.sCARD_INFO>(this.exchangedDeck);
    }

    public void fill_from_exchangeDeck(Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO> target)
    {
        this.exchangedDeck.ForEach(obj => target.Enqueue(obj));
    }
    
    public List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> getDeck()
    {
        return deck;
    }

    // ?? ?? ??? ? ???? exchangedDeck?? ???? ??? ???? ?
    public void addExchangedDeck(ClassLibraryCardInfo.CardInfo.sCARD_INFO card)
    {
        this.exchangedDeck.Add(card);
    }

}