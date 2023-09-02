using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Engine
{
    public class CDealer
    {
        public static int DEFINED_CARD_SIZE_PER_USER = 4;
        public List<CardInfo.sCARD_INFO> deck;
        public List<CardInfo.sCARD_INFO> exchangedDeck;

        public CDealer()
        {
            this.deck = new List<CardInfo.sCARD_INFO>();
            this.exchangedDeck = new List<CardInfo.sCARD_INFO>();
        }

        public void reset()
        {
            exchangedDeck.Clear();
        }

        public void createCards()
        {
            deck.Clear();
#if DEBUG2
            //setDeckInit(deck);
            //setDeckDraw_golf(deck);
            //setDeckDraw_테스트1(deck);
#endif
            // 1~13  x 1
            //for (int x = 0; x < 4; x++)
            for (int i = 0; i < 13; i++)
                for (int j = 0; j < 4; j++)
                {
                    CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
                    card.Init(j, i, 0);
                    deck.Add(card);
                }

            // 1~10 x1
            //for (int i = 0; i < 10; i++)
            //    for (int j = 0; j < 4; j++)
            //    {
            //        CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
            //        card.Init(j, i, 0);
            //        deck.Add(card);
            //    }

            //// 1~7 x1
            //for (int i = 0; i < 7; i++)
            //    for (int j = 0; j < 4; j++)
            //    {
            //        CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
            //        card.Init(j, i, 0);
            //        deck.Add(card);
            //    }
        }

        private void setDeckDraw_golf(List<CardInfo.sCARD_INFO> deck)
        {
            deck.Add(GetCard(0, 0));
            deck.Add(GetCard(1, 1));
            deck.Add(GetCard(2, 2));
            deck.Add(GetCard(3, 3));

            //deck.Add(GetCard(0, 0));
            //deck.Add(GetCard(1, 1));
            //deck.Add(GetCard(2, 2));
            //deck.Add(GetCard(3, 3));
        }
        private void setDeckDraw_테스트1(List<CardInfo.sCARD_INFO> deck)
        {
            // 0스페 1다야 2하트 3클롭

            // 클10 다큐 하제 하4
            deck.Add(GetCard(3, 9));
            deck.Add(GetCard(1, 11));
            deck.Add(GetCard(2, 10));
            deck.Add(GetCard(2, 3));
            // 스7 스큐 다카 다2
            deck.Add(GetCard(0, 6));
            deck.Add(GetCard(0, 11));
            deck.Add(GetCard(1, 12));
            deck.Add(GetCard(1, 1));
            // 하6 클제 스카 스9
            deck.Add(GetCard(2, 5));
            deck.Add(GetCard(3, 10));
            deck.Add(GetCard(0, 12));
            deck.Add(GetCard(0, 8));
            // 하9 다1 클2 하5
            deck.Add(GetCard(2, 8));
            deck.Add(GetCard(1, 0));
            deck.Add(GetCard(3, 1));
            deck.Add(GetCard(2, 4));

            // 클7
            deck.Add(GetCard(3, 6));
            // 다9
            deck.Add(GetCard(1, 8));

            // 하2
            deck.Add(GetCard(2, 1));
            // 클큐
            deck.Add(GetCard(3, 11));

            // 클카
            deck.Add(GetCard(3, 12));
            // 스5
            deck.Add(GetCard(0, 4));

        }

        private CardInfo.sCARD_INFO GetCard(int m_nMark, int m_nCardNum)
        {
            CardInfo.sCARD_INFO card = new CardInfo.sCARD_INFO();
            card.Init(m_nMark, m_nCardNum, 0);
            return card;
        }

        public void suffleCards()
        {
            Shuffle<CardInfo.sCARD_INFO>(this.deck);
        }
        public void fill_to(Queue<CardInfo.sCARD_INFO> target)
        {
            this.deck.ForEach(obj => target.Enqueue(obj));
        }
        public void shuffleExchangeCards()
        {
            Shuffle<CardInfo.sCARD_INFO>(this.exchangedDeck);
        }

        public void fill_from_exchangeDeck(Queue<CardInfo.sCARD_INFO> target)
        {
            this.exchangedDeck.ForEach(obj => target.Enqueue(obj));
        }

        //public List<CardInfo.sCARD_INFO> getDeck()
        //{
        //    return deck;
        //}

        // ?? ?? ??? ? ???? exchangedDeck?? ???? ??? ???? ?
        public void addExchangedDeck(CardInfo.sCARD_INFO card)
        {
            this.exchangedDeck.Add(card);
        }

        Random rng = new Random((int)DateTime.UtcNow.Ticks);
        public void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
