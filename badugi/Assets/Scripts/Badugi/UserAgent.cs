using System;
using System.Collections;
using System.Collections.Generic;
public class UserHand
{
    public int index;
    public ClassLibraryCardInfo.CardInfo.sCARD_INFO card;
    public UserHand(int _index, ClassLibraryCardInfo.CardInfo.sCARD_INFO _card)
    {
        this.index = _index;
        this.card = _card;
    }
}
public class UserAgent {
    byte player_index;

    public int money { get; private set; }
    public byte possibleRaceCount { get; private set; }
    public int paidMoney;
    public int totalpaideMoney;
    public bool isDeadPlayer { get; private set; }
    public bool isBoss { get; private set; }
    public bool isOperator { get; private set; }
    public bool bCalled = false;
    public bool bAllin = false;
    private String name;

    public List<UserHand> userHands { get; private set; }
    private GameRound currentRound = GameRound.START;

    public UserAgent(byte player_index, bool isBoss)
    {
        userHands = new List<UserHand>(4);
        this.player_index = player_index;
        this.money = 1000000;
        this.possibleRaceCount = 1;
        
        this.paidMoney = 0;
        
        this.isBoss = isBoss;
        
        this.isDeadPlayer = false;
        this.bCalled = false;
        this.bAllin = false;
    }

    public void reset()
    {
        this.isDeadPlayer = false;
        this.bCalled = false;
        this.bAllin = false;
        userHands.Clear();
    }

    public void add_card_to_hand(int index, ClassLibraryCardInfo.CardInfo.sCARD_INFO card)
    {
        UserHand handcard = new UserHand(index, card);
        userHands.Add(handcard);
    }

    public void remove_card_to_hand(byte _index)
    {
        userHands.Remove(userHands.Find(c => c.index == _index));
    }
    public List<UserHand> GetUserHands()
    {
        return userHands;
    }
    public int GetUserHandScore()
    {
        ClassLibraryCardInfo.CardInfo cardinfo= new ClassLibraryCardInfo.CardInfo();
        cardinfo.Clear();
        List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cards = new List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
        for(int i = 0; i < userHands.Count; ++i)
        {
            cards.Add(userHands[i].card);
        }
        cardinfo.SetCard(cards.ToArray());
        cardinfo.MakeResult();
        //string cardName = cardinfo.GetCardName();
        int cardRes = cardinfo.GetTotalScore();// + "  점수 : " + cardinfo.GetTotalScore();
        return cardRes;
    }

    public string GetUserHandName()
    {
        ClassLibraryCardInfo.CardInfo cardinfo = new ClassLibraryCardInfo.CardInfo();
        cardinfo.Clear();
        List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cards = new List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
        for (int i = 0; i < userHands.Count; ++i)
        {
            cards.Add(userHands[i].card);
        }
        cardinfo.SetCard(cards.ToArray());
        cardinfo.MakeResult();

        string cardName = cardinfo.GetCardName();
        string cardRes = cardName + " " + cardinfo.GetVoiceFileName();
        return cardRes;
    }

    public void SetBoss(bool _isBoss)
    {
        this.isBoss = _isBoss;
    }
    public String getName()
    {
        return this.name;
    }
    public long getMoney()
    {
        return this.money;
    }

    public void subtractMoney(int money)
    {
        this.money -= money;
        if(this.money < 0)
        {
            this.money = 0;
            this.bAllin = true;
        }
    }

    

    public void addMoney(int money)
    {
        this.money += money;
    }
    public void addPaidMoney(int money)
    {
        paidMoney += money;
    }
    public int getPaidMoney()
    {
        return paidMoney;
    }
    public void SetCurrentRound(GameRound _round)
    {
        this.currentRound = _round;
    }
  


    public byte getPossibleRaceCount()
    {
        return possibleRaceCount;
    }
    public void setPossibleRaceCount(byte possibleRaceCount)
    {
        this.possibleRaceCount = possibleRaceCount;
    }
    public void initPossibleRaceCount(GameRound round)
    {
        switch (round)
        {
            case GameRound.START:
                this.possibleRaceCount = 1;
                break;
            case GameRound.MORNING:
                this.possibleRaceCount = 2;
                break;
            case GameRound.AFTERNOON:
                this.possibleRaceCount = 3;
                break;
            case GameRound.EVENING:
                this.possibleRaceCount = 3;
                break;
        }
    }


    public bool getIsDeadPlayer()
    {
        return this.isDeadPlayer;
    }
    public void setIsDeadPlayer(bool isDeadPlayer)
    {
        this.isDeadPlayer = isDeadPlayer;
    }

    public bool GetisAllin()
    {
        return bAllin;
    }
}
