using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 바두기 룰을 구현한 클래스
/// </summary>
public class CBadugiEngine {


    List<UserAgent> player_agents;
    Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO> deck;


    private Dealer dealer;//카드 메니져
    public byte bossIndex { get; private set; }
    public byte currentTurnUserIndex { get; set; }
    public int baseMoney { get; set; }
    public int callMoney { get; private set; }
    public int totalMoney { get; private set; }
    public int callDieCount { get; private set; }
    public int round { get; private set; }
    public int deadPersonCount { get; private set; }

    public List<List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>> distributed_players_cards { get; private set; }
    public List<ClassLibraryCardInfo.CardInfo.sCARD_INFO> changeCardDeck { get; private set; }
    public CBadugiEngine()
    {
        this.player_agents = new List<UserAgent>();
        this.distributed_players_cards = new List<List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>>();
        this.changeCardDeck = new List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
        this.dealer = new Dealer(player_agents, 0);
        this.deck = new Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
    }

    public void reset()
    {
        this.player_agents.ForEach(obj => obj.reset());

        this.distributed_players_cards.Clear();
        this.changeCardDeck.Clear();
        dealer.createCards();
    }
    public void clear_turn_data()
    { }
    public void start(List<User> players)
    {
        this.player_agents.Clear();

        for (int i = 0; i < players.Count; ++i)
        {
            this.player_agents.Add(players[i].agent);
            this.player_agents[i].reset();
            this.distributed_players_cards.Add(new List<ClassLibraryCardInfo.CardInfo.sCARD_INFO>());
        }

        shuffle();

        distribute_cards();
    }
    private void shuffle()
    {
        this.dealer.suffleCards();
        this.deck.Clear();
        this.dealer.fill_to(deck);
    }

    private void shuffleExcahgne()
    {
        this.dealer.shuffleExchangeCards();
        this.deck.Clear();
        this.dealer.fill_from_exchangeDeck(this.deck);
    }
    private void distribute_cards()
    {
        //보스 확인
        //보스로직 다음판일 경우 보스는 시계방향으로 바뀐다.
        int bossIndex = player_agents.IndexOf(player_agents.Find(p => p.isBoss == true));
        if (bossIndex == -1)
        {
            bossIndex = player_agents.IndexOf(player_agents.Find(p => p.isOperator == true));
            player_agents[bossIndex].SetBoss(true);
        }

        //카드 네장을 분배한다.

        for (int j = bossIndex; j < player_agents.Count + bossIndex; ++j)
        {
            for (int i = 0; i < 4; ++i)
            {
                int nIndex = j % player_agents.Count;
                ClassLibraryCardInfo.CardInfo.sCARD_INFO card = pop_deck_card();
                this.distributed_players_cards[nIndex].Add(card);
                this.player_agents[nIndex].add_card_to_hand(i, card);
            }
        }
    }

    private int takeBaseMoneyFromUsers()
    {
        int sumOfMoney = 0;
        foreach(UserAgent user in this.player_agents)
        {
            user.subtractMoney(this.baseMoney);
            sumOfMoney = sumOfMoney + this.baseMoney;
        }
        return sumOfMoney;
    }
   
    ClassLibraryCardInfo.CardInfo.sCARD_INFO pop_deck_card()
    {
        if(deck.Count == 0)
        {
            shuffleExcahgne();
        }
        return this.deck.Dequeue();
    }

    public void AddExcahngeCard(ClassLibraryCardInfo.CardInfo.sCARD_INFO card)
    {
        this.dealer.addExchangedDeck(card);
    }

    public ClassLibraryCardInfo.CardInfo.sCARD_INFO getExchangeCard()
    {
        return pop_deck_card();
    }
    #region 턴
    public byte get_next_player_index()
    {
        byte nextTurnUserIndex = (byte)((this.currentTurnUserIndex + 1) % player_agents.Count);
        UserAgent nextTurnUser = this.player_agents[nextTurnUserIndex];
        //다이 했거나 다음 유저가 없을떄 그 다음으로 건너 뛴다.
        while(nextTurnUser == null || nextTurnUser.getIsDeadPlayer())
        {
            currentTurnUserIndex = nextTurnUserIndex;
            nextTurnUserIndex = (byte)((this.currentTurnUserIndex + 1) % player_agents.Count);
            nextTurnUser = this.player_agents[nextTurnUserIndex];
        }
        return nextTurnUserIndex;
    }

    public void move_to_next_player()
    {
        this.currentTurnUserIndex = get_next_player_index();
    }
    
    private byte find_next_player_index_of(byte prev_player_index)
    { return 0; }
    #endregion
}
