using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using ClassLibraryCardInfo;
using System.Linq;
using Vector3 = UnityEngine.Vector3;
using ZNet;

public class PlayGameUI : MonoBehaviour
{
    static public PlayGameUI Instance;

    public enum _eDeckImageType
    {
        EDIT_NORMAL = 0,
        EDIT_EVENT,

        EDIT_MAXCNT
    }

    [SerializeField]
    Sprite[] m_sprNormalDeckCardImage;

    [SerializeField]
    Sprite[] m_sprEventDeckCardImage;

    [SerializeField]
    RawImage[] m_rawNormalDeckImage;

    [SerializeField]
    SpriteRenderer m_DeckSprite;

    [SerializeField]
    Sprite[] m_sprEventMultiple;

    [SerializeField]
    GameObject m_objEventMultiple;

    public GameObject Notice;

    // 181019 카드를 체인지 할때는 카드가 카드덱 아래로 들어가게 하려고 레이어 조절을 위해 만듦
    public GameObject DeckCardImage;

    // 원본 이미지들.
    Sprite back_image;
    Sprite[] m_sprCardBackImage;

    // 카드 이동속도
    const float cardMoveSpeed = 0.35f;

    //각 슬롯의 좌표 객체
    Vector3 DeckPos = new Vector3(0, 495, 0);

    [SerializeField]
    CPlayerCardPosition[] player_card_positions;

    [SerializeField]
    GameObject m_objHiddenCard;

    public GameObject MorningAni;
    public GameObject AfternoonAni;
    public GameObject EveningAni;

    public GameObject PrefabFont;

    //카드객체
    List<CCardPicture> total_card_pictures;
    CCardCollision card_collision_manager;

    //게임플레이에 사용되는 객체들
    [HideInInspector]
    public byte player_me_index;
    //카드 딜러 객체
    Stack<CCardPicture> deck_cards;
    //플레이어 패 카드 객체
    public List<CPlayerHandCardManager> player_hand_card_manager { get; private set; }
    bool[] badCards = new bool[4];

    //플레이어 정보 슬롯
    public List<CPlayerInfoSlot> player_info_slots;
    public CGameInfo gameInfo;

    // 플레이어 턴
    public GameObject[] playerTurn;

    // 채널 게임머니 구분
    public int moneyType;
    // 판돈
    public int RoomMoney;

    //network
    public Queue<CRecvedMsg> waiting_packets;

    CCardManager card_manager;

    PopupBetting bettingUI;
    PopupCutting cuttingUI;
    GameRound currentRound = GameRound.START;

    //순서
    private int userRoundIndex = 5;
    private byte currentBossIndex = 0;

    public bool isGaming = false;
    public bool isCounterOver = false;
    public bool isAFK = false;

    int[] playerSoundIndex = new int[5] { 1, 1, 1, 1, 1 };

    //===========================
    //START KWH
    [SerializeField] NowMyJockbo myjockbo;
    [SerializeField] NowDayState nowDayState;
    //END KWH
    //===========================

    int m_iHalfLevelCount = 0;

    public enum _eEnumeratorList
    {
        EET_on_server_deallcardtoAllUser = 0,
        EET_CoCountDown,
        EET_on_server_exchangeCard,
        EET_on_server_change_round,
        EET_on_server_req_open,
        EET_SERVER_GAME_RESULT,
        EET_SC_EVENT_JACKPOT_INFO,
        EET_StartTurtle,
        EET_StartShark,
        EET_StartWhale,
        EET_CoStartEventIndex,
        EET_Ceremony50,
        EET_Ceremony100,
        EET_Ceremony200,
        EET_CoPlayMovie,
        EET_exchangeCardAni,
        EET_on_server_card_open,
        EET_CountDownPlaySound,

        EET_MACNT
    }

    IEnumerator[] m_coEnumerator;

    #region 게임준비
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        m_coEnumerator = new IEnumerator[(int)_eEnumeratorList.EET_MACNT];
        //네트워크
        waiting_packets = new Queue<CRecvedMsg>();

        m_objEventMultiple.SetActive(false);

        //카드 콜라이더
        card_collision_manager = transform.GetComponent<CCardCollision>();
        card_collision_manager.callback_on_touch = on_card_touch;
        //플레이어 인덱스
        player_me_index = 0;
        //딜러 카드
        deck_cards = new Stack<CCardPicture>();
        card_manager = new CCardManager();
        card_manager.make_all_cards();

        //플레이어 카드
        player_hand_card_manager = new List<CPlayerHandCardManager>();

        //게임정보
        for (int i = 0; i < userRoundIndex; ++i)
        {
            player_hand_card_manager.Add(new CPlayerHandCardManager());
        }

        m_sprCardBackImage = new Sprite[2];
        back_image = CSpriteManager.Instance.get_sprite("card_badugi_0001");
        m_sprCardBackImage[0] = CSpriteManager.Instance.get_sprite("card_badugi_0001");
        m_sprCardBackImage[1] = CSpriteManager.Instance.get_sprite("back_event");

        // 카드 만들어 놓기.
        total_card_pictures = new List<CCardPicture>();
        GameObject original = Resources.Load("card") as GameObject;
        for (int i = 0; i < card_manager.cards.Count; ++i)
        {
            GameObject obj = Instantiate(original);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            obj.transform.localPosition = DeckPos;
            obj.transform.transform.localRotation = Quaternion.identity;
            obj.transform.transform.Rotate(new Vector3(0, 0, 90));
            obj.AddComponent<CMovingObject>();
            CCardPicture card_pic = obj.AddComponent<CCardPicture>();
            total_card_pictures.Add(card_pic);
        }
    }

    void Start()
    {
        cuttingUI = UIManager.Instance.get_uipage(UI_PAGE_BADUK.CHANGECARD).GetComponent<PopupCutting>();
        bettingUI = UIManager.Instance.get_uipage(UI_PAGE_BADUK.BETTING).GetComponent<PopupBetting>();
        bettingUI.DeactiveAllButton();
        enter();

        for (int i = 0; i < player_info_slots.Count; ++i)
        {
            player_info_slots[i].update_name("");
            player_info_slots[i].update_money(0);
            player_info_slots[i].Allreset();
            player_info_slots[i].name_text.gameObject.SetActive(false);
            player_info_slots[i].money_text.gameObject.SetActive(false);
        }
        currentRound = GameRound.START;

        // 키입력
        StartCoroutine(GetKeyboradKey());

        // 카운터 기능 실행
        StartCoroutine(Counter());
    }
    void clear_infoUI()
    {
        gameInfo.set_totalmymoney(0);
        gameInfo.update_totalmoney(0);
        gameInfo.update_call(0);
        for (int i = 0; i < player_info_slots.Count; ++i)
        {
            //this.player_info_slots[i].update_name("");
            //this.player_info_slots[i].update_money(0);
            player_info_slots[i].Allreset();
            if (player_info_slots[i].HandName != null)
                player_info_slots[i].HandName.GetComponent<Text>().text = "";
            //player_info_slots[i].grade[0].GetComponent<tk2dSprite>().SetSprite("empty");
            //player_info_slots[i].grade[1].GetComponent<tk2dSprite>().SetSprite("empty");
        }

        //===========================
        //START KWH
        if (myjockbo != null)
        {
            myjockbo.TurnOff();
        }

        if (nowDayState != null)
        {
            nowDayState.TurnOff();
        }
        //END KWH
        //===========================

    }
    void reset()
    {
        currentRound = GameRound.START;
        card_manager.make_all_cards();

        bettingUI.DeactiveAllButton();

        make_deck_cards();

        for (int i = 0; i < player_hand_card_manager.Count; ++i)
        {
            player_hand_card_manager[i].reset();
        }

        foreach (var g in playerTurn) g.SetActive(false);

        clear_infoUI();
    }
    public void enter()
    {
        clear_infoUI();
        StartCoroutine(sequential_packet_handler());
    }
    #endregion

    #region 네트워크
    public void OnReceive(ZNet.CRecvedMsg msg)
    {
        ZNet.CRecvedMsg clone = new ZNet.CRecvedMsg();
        clone.remote = msg.remote;
        clone.pkop = msg.pkop;
        clone.msg = msg.msg;
        clone.pkID = msg.pkID;

        this.waiting_packets.Enqueue(clone);
    }

    //패킷처리
    IEnumerator sequential_packet_handler()
    {
        yield return null;
        while (true)
        {
            if (this.waiting_packets.Count <= 0)
            {
                yield return null;
                continue;
            }

            var msg_ = this.waiting_packets.Dequeue();
            RemoteID remote = msg_.remote;
            CPackOption pkOption = msg_.pkop;
            CMessage msg = msg_.msg;
            PacketType packetType = msg_.pkID;

            switch (packetType)
            {
                // 로비->클라 입장시 자동으로 cs_roomin을 보내면 받음
                case SS.Common.GameUserIn:
                    {
                        //reset();

                        //UIManager.Instance.hide(UI_PAGE_BADUK.BETTING);
                        UIManager.Instance.hide(UI_PAGE_BADUK.CHANGECARD);
                        byte GameRuleType;
                        Rmi.Marshaler.Read(msg, out GameRuleType);
                        byte playerindex;
                        Rmi.Marshaler.Read(msg, out playerindex);
                        player_me_index = playerindex;
                        byte bossIndex;
                        Rmi.Marshaler.Read(msg, out bossIndex);
                        currentBossIndex = bossIndex;
                        // 관전기능
                        bool isObserver;
                        Rmi.Marshaler.Read(msg, out isObserver);
                        if (isObserver)
                        {
                            // 관전초기화
                            EntrySpectator(msg);
                        }
                        else
                        {
                            if (player_me_index == currentBossIndex)
                            {
                            }
                            else
                            {
                                UIManager.Instance.show(UI_PAGE_BADUK.WAITGAMESTART);
                            }
                        }
                        on_server_start();
                    }
                    break;
                case SS.Common.GameSetBoss:
                    {
                        //현재 보스
                        byte bossIndex;
                        Rmi.Marshaler.Read(msg, out bossIndex);
                        currentBossIndex = bossIndex;

                        foreach (var p in player_info_slots)
                        {
                            p.SetBoss(false);
                        }
                        int roundIndex = ((userRoundIndex - player_me_index) + currentBossIndex) % userRoundIndex;
                        player_info_slots[roundIndex].SetBoss(true);

                        //Debug.Log("보스 : " + currentBossIndex);

                    }
                    break;
                case SS.Common.GameStart:
                    {
                        isGaming = true;
                        isCounterOver = false;
                        UIManager.Instance.hide(UI_PAGE_BADUK.WAITGAMESTART);
                    }
                    break;
                // 라운드 시작
                case SS.Common.GameRoundStart:
                    {
                        // 판돈
                        int roomMoney;
                        Rmi.Marshaler.Read(msg, out roomMoney);
                        // 참여유저수
                        int playerCount;
                        Rmi.Marshaler.Read(msg, out playerCount);
                        // 내베팅턴
                        int myBettingTurn;
                        Rmi.Marshaler.Read(msg, out myBettingTurn);
                        RoomMoney = roomMoney;
                        // 라운드 시작 베팅 머니 설정
                        on_server_startbetting(msg);
                    }
                    break;
                case SS.Common.GameNotifyStat:
                    {
                        on_server_statics(msg);
                    }
                    break;
                case SS.Common.GameDealCards:
                    {
                        //첫 카드 나눠주기
                        m_coEnumerator[(int)_eEnumeratorList.EET_on_server_deallcardtoAllUser] = on_server_deallcardtoAllUser(msg);
                        yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_on_server_deallcardtoAllUser]);
                    }
                    break;
                case SS.Common.GameChangeTurn:
                    {
                        byte currentTurn;
                        Rmi.Marshaler.Read(msg, out currentTurn);
                        int roundIndex = ((userRoundIndex - player_me_index) + currentTurn) % userRoundIndex;

                        yield return new WaitForSeconds(0.2f);

                        // 플레이어 턴 시작
                        foreach (var g in playerTurn) g.SetActive(false);
                        playerTurn[roundIndex].SetActive(true);

                        // 카운트다운 타이머 시작
                        m_coEnumerator[(int)_eEnumeratorList.EET_CoCountDown] = CoCountDown(roundIndex);
                        StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_CoCountDown]);
                    }
                    break;
                case SS.Common.GameRequestBet:
                    {
                        UIManager.Instance.show(UI_PAGE_BADUK.BETTING);
                        UIManager.Instance.hide(UI_PAGE_BADUK.CHANGECARD);
                        on_server_req_betting(msg);
                    }
                    break;
                case SS.Common.GameResponseBet:
                    {
                        on_server_res_betting(msg);
                    }
                    break;

                case SS.Common.GameRequestChangeCard:
                    {
                        //카드 바꾸기 패스 활성화
                        UIManager.Instance.hide(UI_PAGE_BADUK.BETTING);
                        UIManager.Instance.show(UI_PAGE_BADUK.CHANGECARD);
                        keyState = UserInputState.CHANGE;
                    }
                    break;
                // 카드체인지
                case SS.Common.GameResponseChangeCard:
                    {
                        UIManager.Instance.hide(UI_PAGE_BADUK.WAITGAMESTART);
                        //바꾸는 카드 결과 전송
                        //패스 했을 때 패스 표시
                        m_coEnumerator[(int)_eEnumeratorList.EET_on_server_exchangeCard] = on_server_exchangeCard(msg);
                        yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_on_server_exchangeCard]);
                    }
                    break;

                case SS.Common.GameChangeRound:
                    {
                        m_coEnumerator[(int)_eEnumeratorList.EET_on_server_change_round] = on_server_change_round(msg);
                        yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_on_server_change_round]);
                    }
                    break;
                // 게임 마무리 카드오픈
                case SS.Common.GameCardOpen:
                    {
                        bool isLast;
                        Rmi.Marshaler.Read(msg, out isLast);
                        m_coEnumerator[(int)_eEnumeratorList.EET_on_server_req_open] = on_server_req_open(msg);
                        if (isLast) yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_on_server_req_open]);
                        else yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_on_server_req_open]);
                    }
                    break;
                // 게임결과
                case SS.Common.GameOver:
                    {
                        m_coEnumerator[(int)_eEnumeratorList.EET_SERVER_GAME_RESULT] = SERVER_GAME_RESULT(msg);
                        yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_SERVER_GAME_RESULT]);
                        //yield return StartCoroutine(SERVER_GAME_RESULT(msg));
                    }
                    break;
                // 방정보 받기
                case SS.Common.GameRoomInfo:
                    {
                        int channel;
                        Rmi.Marshaler.Read(msg, out channel);
                        int roomNumber;
                        Rmi.Marshaler.Read(msg, out roomNumber);
                        int money;
                        Rmi.Marshaler.Read(msg, out money);
                        Rmi.Marshaler.Read(msg, out moneyType);
                        UIManager.Instance.SetRoomInfo(channel, roomNumber, money);
                    }
                    break;
                // 올인시
                case SS.Common.GameKickUser:
                    {
                        //yield return StartCoroutine(RoomOut());
                        UIManager.Instance.ExitGame();
                    }
                    break;
                // 잭팟 새로고침
                case SS.Common.GameEventInfo:
                    {
                        StartCoroutine(SC_EVENT_JACKPOT_INFO(msg));
                    }
                    break;
                // 유저정보
                case SS.Common.GameUserInfo:
                    {
                        UserInfoRefresh(msg);
                    }
                    break;
                // 유저 나갔을때
                case SS.Common.GameUserOut:
                    {
                        UserRoomOut(msg);
                    }
                    break;
                // 잭팟시작
                case SS.Common.GameEventStart:
                    {
                        byte type;
                        Rmi.Marshaler.Read(msg, out type);

                        SetDeckImage(_eDeckImageType.EDIT_EVENT);
                        if (type == 1)
                        {
                            m_coEnumerator[(int)_eEnumeratorList.EET_StartTurtle] = EventManager.Instance.StartTurtle();
                            yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_StartTurtle]);
                        }
                        else if (type == 2)
                        {
                            m_coEnumerator[(int)_eEnumeratorList.EET_StartShark] = EventManager.Instance.StartShark();
                            yield return StartCoroutine(EventManager.Instance.StartShark());
                        }
                        else if (type == 3)
                        {
                            m_coEnumerator[(int)_eEnumeratorList.EET_StartWhale] = EventManager.Instance.StartWhale();
                            yield return StartCoroutine(EventManager.Instance.StartWhale());
                        }
                        else if (type == 4)
                        {
                            byte player_index;
                            Rmi.Marshaler.Read(msg, out player_index);

                            int roundIndex = ((userRoundIndex - player_me_index) + player_index) % userRoundIndex;
                            // 해당하는 라운드 인덱스에 맞는 연출
                            // roundIndex => 0 = 6시 방향, 1 = 9시 방향, 2 = 11시 방향, 3 = 1시 방향, 4 = 3시 방향

                            //yield return StartCoroutine(EventManager.Instance.StartWhale()); // 돌발 이벤트 연출
                        }
                    }
                    break;
                case SS.Common.GameEvent2Start: // 돌발이벤트
                    {
                        byte player_index;
                        Rmi.Marshaler.Read(msg, out player_index);

                        // roundIndex => 0 = 6시 방향, 1 = 9시 방향, 2 = 11시 방향, 3 = 1시 방향, 4 = 3시 방향
                        int roundIndex = ((userRoundIndex - player_me_index) + player_index) % userRoundIndex;

                        m_coEnumerator[(int)_eEnumeratorList.EET_CoStartEventIndex] = EventManager.Instance.CoStartEventIndex(roundIndex);
                        yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_CoStartEventIndex]);
                    }
                    break;
                // 잭팟초침이동
                case SS.Common.GameEventRefresh:
                    {
                        byte jackPotPlayCount;
                        Rmi.Marshaler.Read(msg, out jackPotPlayCount);

                        EventManager.Instance.ClockPinMove(jackPotPlayCount);
                    }
                    break;
                // 시계사라짐
                case SS.Common.GameEventEnd:
                    {
                        byte type;
                        Rmi.Marshaler.Read(msg, out type);

                        SetDeckImage(_eDeckImageType.EDIT_NORMAL);

                        //yield return StartCoroutine(EventManager.Instance.EndEvent());

                        if (type == 1)
                        {
                            m_coEnumerator[(int)_eEnumeratorList.EET_Ceremony50] = EventManager.Instance.Ceremony50();
                            yield return StartCoroutine(EventManager.Instance.Ceremony50());
                        }
                        else if (type == 2)
                        {
                            m_coEnumerator[(int)_eEnumeratorList.EET_Ceremony100] = EventManager.Instance.Ceremony100();
                            yield return StartCoroutine(EventManager.Instance.Ceremony100());
                        }
                        else if (type == 3)
                        {
                            m_coEnumerator[(int)_eEnumeratorList.EET_Ceremony200] = EventManager.Instance.Ceremony200();
                            yield return StartCoroutine(EventManager.Instance.Ceremony200());
                        }
                    }
                    break;
                // 방장의 게임시작 버튼
                case SS.Common.GameRoomReady:
                    {
                        bool gameStart;
                        Rmi.Marshaler.Read(msg, out gameStart);

                        if (gameStart)
                        {
                            UIManager.Instance.hide(UI_PAGE_BADUK.WAITGAMESTART);
                        }
                        else
                        {
                        }
                    }
                    break;
                // 마일리지
                case SS.Common.GameMileageRefresh:
                    {
                        int per;
                        Rmi.Marshaler.Read(msg, out per);
                        int cnt;
                        Rmi.Marshaler.Read(msg, out cnt);
                    }
                    break;
                case SS.Common.GameEventNotify:
                    {
                        int eventmoney;
                        Rmi.Marshaler.Read(msg, out eventmoney);
                        string nickname;
                        Rmi.Marshaler.Read(msg, out nickname);
                        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.GOLFCEREMONY, false);

                        m_coEnumerator[(int)_eEnumeratorList.EET_CoPlayMovie] = AniManager.Instance.CoPlayMovie(AniManager._eType.GOLF_CEREMONY, false);
                        yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_CoPlayMovie]);
                        //UIManager.Instance.ReportText("▶ [" + nickname + "] 님 골프 잭팟 당첨! 축하금 " + eventmoney + "냥");
                    }
                    break;
                // 나가기 예약 표시
                case SS.Common.GameResponseRoomOutRsvp:
                    {
                        byte player_index;
                        Rmi.Marshaler.Read(msg, out player_index);
                        bool isRsvp;
                        Rmi.Marshaler.Read(msg, out isRsvp);

                        int roundIndex = ((userRoundIndex - player_me_index) + player_index) % userRoundIndex;

                        // 해당 인덱스 플레이어 UI에 나가기예약 표시 (isRsvp => true = 나가기예약, false = 나가기예약 취소)
                        // roundIndex

                        PlayGameUI.Instance.player_info_slots[roundIndex].SetReservationIcon(isRsvp);
                    }
                    break;
                // 나가기
                case SS.Common.GameResponseRoomOut:
                    {
                        bool isSuccess;
                        Rmi.Marshaler.Read(msg, out isSuccess);
                        if (isSuccess == false)
                        {
                            UIManager.Instance.ExitGameReset();
                        }
                    }
                    break;
                case SS.Common.GameRoomIn:
                    {
                        bool isEnter;
                        Rmi.Marshaler.Read(msg, out isEnter);
                        if (isEnter == true)
                        {
                            reset();
                            NetworkManager.Instance.RoomIn();
                        }
                    }
                    break;
                case SS.Common.GameNotifyMessage:
                    {
                        int messageType; Rmi.Marshaler.Read(msg, out messageType); // 공지 타입 (기본값:0)
                        string message; Rmi.Marshaler.Read(msg, out message); // 공지 문자열
                        int sec; Rmi.Marshaler.Read(msg, out sec); // 시간
                        StartCoroutine(NoticeRefresh(message, sec));
                    }
                    break;
                // 방이동
                case SS.Common.GameResponseRoomMove:
                    {
                        bool isSuccess;
                        Rmi.Marshaler.Read(msg, out isSuccess);
                        string errMsg;
                        Rmi.Marshaler.Read(msg, out errMsg);

                        if (isSuccess == true)
                        {
                            for (int i = 0; i < m_coEnumerator.Length; ++i)
                            {
                                if (m_coEnumerator[i] != null)
                                {
                                    StopCoroutine(m_coEnumerator[i]);
                                }
                            }

                            // 리셋
                            reset();

                            // 유저정보 리셋
                            for (int j = 0; j < player_info_slots.Count; j++)
                            {
                                player_info_slots[j].name_text.gameObject.SetActive(false);
                                player_info_slots[j].money_text.gameObject.SetActive(false);
                                //player_info_slots[j].grade[0].gameObject.SetActive(false);
                                //player_info_slots[j].grade[1].gameObject.SetActive(false);
                                player_info_slots[j].avatar.SetActive(false);
                                player_info_slots[j].Allreset();
                                player_info_slots[j].RoomMoveReset();
                            }

                            PlayGameUI.Instance.SetEventMultipleVisible(1, false);
                            IBettingChipsManager.Instance.Clear();
                            AniManager.Instance.StopAllMovie();
                            UIManager.Instance.hide(UI_PAGE_BADUK.WAITGAMESTART);

                            //// 이벤트 리셋
                            //AniManager.Instance.StopMovie(AniManager._eType.CLOCK_LOOP);
                            //AniManager.Instance.StopMovie(AniManager._eType.CLOCK_YELLOW);
                            //AniManager.Instance.StopMovie(AniManager._eType.CLOCK_BLUE);
                            //AniManager.Instance.StopMovie(AniManager._eType.CLOCK_RED);

                            //EventManager.Instance.Pin.SetActive(false);
                            //EventManager.Instance.Pin.transform.localRotation = new Quaternion(0, 0, 0, 1);

                            // 리포트창 리셋
                            UIManager.Instance.ReportAllReset();

                            // 방이동 및 나가기 버튼 리셋
                            UIManager.Instance.ExitGameReset();
                            UIManager.Instance.RoomMoveReserveReset();

                            UIManager.Instance.SetRoomMoveTime(0.0f);
                            //NetworkManager.Instance.server_tag = CommonBadugi.Server.Room;
                        }
                        else
                        {
                            UIManager.Instance.RoomMoveReserveReset();
                        }
                    }
                    break;
            }
            yield return 0;
        }
    }

    IEnumerator NoticeRefresh(string message, int sec)
    {
        INoticeManager.Instance.AddNotice(message, 10.0f, 0.0f);
        //NetworkManager.Instance.Notice.SetActive(true);

        //NetworkManager.Instance.Notice.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = message;

        yield return new WaitForSeconds(sec);

        NetworkManager.Instance.Notice.SetActive(false);
    }

    public void on_server_start()
    {
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameRequestReady;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }

    public IEnumerator on_server_deallcardtoAllUser(CMessage msg)
    {
        reset();
        // 초기화 
        byte bossIndex;
        Rmi.Marshaler.Read(msg, out bossIndex);
        currentBossIndex = bossIndex;
        //Debug.Log("보스 : " + currentBossIndex);
        foreach (var p in player_info_slots)
        {
            p.SetBoss(false);
        }
        int roundIndex = ((userRoundIndex - player_me_index) + currentBossIndex) % userRoundIndex;
        player_info_slots[roundIndex].SetBoss(true);

        byte playerIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);
        player_me_index = playerIndex;
        byte playerCount;
        Rmi.Marshaler.Read(msg, out playerCount);
        Dictionary<byte, Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO>> player_cards =
            new Dictionary<byte, Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO>>();

        byte grade = 0;
        byte number = 0;

        int Position = 0;

        for (byte player = 0; player < playerCount; ++player, ++Position)
        {
            Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cards = new Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index);
            byte player_card_count;
            Rmi.Marshaler.Read(msg, out player_card_count);
            byte num = byte.MaxValue;
            for (byte i = 0; i < player_card_count; ++i)
            {
                Rmi.Marshaler.Read(msg, out num);
                if (num != byte.MaxValue)
                {
                    ClassLibraryCardInfo.CardInfo.sCARD_INFO card = new ClassLibraryCardInfo.CardInfo.sCARD_INFO();
                    card.m_nCardNum = num;
                    byte shape; Rmi.Marshaler.Read(msg, out shape); card.m_nShape = shape;
                    byte isState; Rmi.Marshaler.Read(msg, out isState); card.m_btIsState = isState;
                    cards.Enqueue(card);
                }
            }
            if (num != byte.MaxValue)
            {
                Rmi.Marshaler.Read(msg, out grade);
                Rmi.Marshaler.Read(msg, out number);
            }
            player_cards.Add(player_index, cards);
            //this.player_me_index = player_index;
        }

        //카드 배포하는 애니메이션
        yield return StartCoroutine(distribute_cards(bossIndex, playerIndex, player_cards, grade, number));

        // 리포트창 초기화
        UIManager.Instance.ReportAllReset();
    }
    private IEnumerator distribute_cards(byte bossIndex, byte playerIndex, Dictionary<byte, Queue<CardInfo.sCARD_INFO>> player_cards, byte grade, byte number)
    {
        //yield return new WaitForSeconds(0.5f);

        // 플레이어의 카드를 분배한다.
        for (int card_index = 0; card_index < 4; ++card_index)
        {
            //SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_DEAL, false);

            // 플레이어에게 1장씩 4번 분배한다.
            for (byte i = bossIndex; i < userRoundIndex + bossIndex; ++i)
            {
                SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_OPEN, false);
                byte player_index = (byte)(i % userRoundIndex);
                Queue<CardInfo.sCARD_INFO> cards = null;
                if (player_cards.TryGetValue(player_index, out cards) == false) continue;

                int roundIndex = ((userRoundIndex - playerIndex) + player_index) % userRoundIndex;
                // 본인 카드는 해당 이미지를 보여주고,
                // 상대방 카드(is_nullcard)는 back_image로 처리한다.
                if (playerIndex == player_index)
                {
                    CCardPicture card_picture = deck_cards.Pop();
                    card_picture.sprite_renderer.enabled = true;
                    card_picture.set_slot_index((byte)card_index);
                    card_picture.set_server_slot_index((byte)card_index);

                    player_hand_card_manager[roundIndex].add(card_picture);

                    CardInfo.sCARD_INFO card = cards.Dequeue();
                    card_picture.update_card(card, get_card_sprite(card));
                    card_picture.sprite_renderer.sortingOrder = card_index;
                    StartCoroutine(move_card(card_picture, player_card_positions[roundIndex].get_hand_position(card_index), 0.1f, false, 0, false));
                }
                else
                {
                    CCardPicture card_picture = deck_cards.Pop();
                    card_picture.sprite_renderer.enabled = true;
                    card_picture.set_slot_index((byte)card_index);
                    card_picture.set_server_slot_index((byte)card_index);

                    player_hand_card_manager[roundIndex].add(card_picture);

                    card_picture.update_backcard(back_image);
                    card_picture.sprite_renderer.sortingOrder = card_index;
                    StartCoroutine(move_card(card_picture, player_card_positions[roundIndex].get_hand_position(card_index), 0.15f, false, 0, false));
                }
                yield return new WaitForSeconds(0.15f);

            }
        }
        yield return new WaitForSeconds(0.1f);

        CardInfo(grade, number, 0);

        //카드 입력 활성화
        card_collision_manager.enabled = true;
        player_hand_card_manager[0].enable_all_colliders(true);

        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameDealCardsEnd;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)0);//player index
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }
    public void on_server_startbetting(CMessage msg)
    {
        byte count;
        Rmi.Marshaler.Read(msg, out count);

        for (int i = 0; i < count; i++)
        {
            byte playerIndex;
            Rmi.Marshaler.Read(msg, out playerIndex);
            byte bettingtype;
            Rmi.Marshaler.Read(msg, out bettingtype);
            long currentmoney;
            Rmi.Marshaler.Read(msg, out currentmoney);

            int roundIndex = ((userRoundIndex - player_me_index) + playerIndex) % userRoundIndex;
            player_info_slots[roundIndex].update_money(currentmoney);

            player_info_slots[roundIndex].chipAnimanager.ChipCreate(RoomMoney);
            if (player_me_index == playerIndex)
            {
                gameInfo.add_totalmymoney(RoomMoney);
            }
        }

        long totoalgamemoney;
        Rmi.Marshaler.Read(msg, out totoalgamemoney);
        long callmoney;
        Rmi.Marshaler.Read(msg, out callmoney);

        gameInfo.update_totalmoney(totoalgamemoney);
        gameInfo.update_call(callmoney);

    }
    public void on_server_req_betting(CMessage msg)
    {
        //내 턴에서 베팅 가능한 버튼 활성화
        byte[] Buttons = new byte[7];
        byte call;
        Rmi.Marshaler.Read(msg, out call);
        Buttons[(int)BETTING.CALL] = call; //콜
        byte bbing;
        Rmi.Marshaler.Read(msg, out bbing);
        Buttons[(int)BETTING.BBING] = bbing; //삥
        byte quater;
        Rmi.Marshaler.Read(msg, out quater);
        Buttons[(int)BETTING.QUATER] = quater; //쿼터
        byte half;
        Rmi.Marshaler.Read(msg, out half);
        Buttons[(int)BETTING.HARF] = half; //하프
        byte die;
        Rmi.Marshaler.Read(msg, out die);
        Buttons[(int)BETTING.DIE] = die; //다이
        byte check;
        Rmi.Marshaler.Read(msg, out check);
        Buttons[(int)BETTING.CHECK] = check; //체크
        byte ddaddang;
        Rmi.Marshaler.Read(msg, out ddaddang);
        Buttons[(int)BETTING.DDADDANG] = ddaddang; //따당
        bettingUI.SetButtonsActive(Convert.ToBoolean(Buttons[0]), Convert.ToBoolean(Buttons[1]), Convert.ToBoolean(Buttons[2]), Convert.ToBoolean(Buttons[3]), Convert.ToBoolean(Buttons[4]), Convert.ToBoolean(Buttons[5]), Convert.ToBoolean(Buttons[6]));

        keyState = UserInputState.BET;

    }

    public void on_server_res_betting(CMessage msg)
    {
        UIManager.Instance.hide(UI_PAGE_BADUK.WAITGAMESTART);
        bCountDown = false;
        keyState = UserInputState.NONE;

        //베팅후 결과 갱신
        byte _player_index;
        Rmi.Marshaler.Read(msg, out _player_index);
        byte betting;
        Rmi.Marshaler.Read(msg, out betting);
        BETTING bet = (BETTING)betting;
        long paidMoney;
        Rmi.Marshaler.Read(msg, out paidMoney);

        int roundIndex = ((userRoundIndex - player_me_index) + _player_index) % userRoundIndex;

        StopCountDownSound();
        player_info_slots[roundIndex].CountDown.SetActive(false);
        player_info_slots[roundIndex].chipAnimanager.ChipCreate(paidMoney);

        if (player_me_index == _player_index)
        {
            gameInfo.add_totalmymoney(paidMoney);
        }

        long player_money;
        Rmi.Marshaler.Read(msg, out player_money);
        player_info_slots[roundIndex].update_money(player_money);
        long totalGameMoney;
        Rmi.Marshaler.Read(msg, out totalGameMoney);
        gameInfo.update_totalmoney(totalGameMoney);
        long callMoney;
        Rmi.Marshaler.Read(msg, out callMoney);
        gameInfo.update_call(callMoney);

        switch (bet)
        {
            case BETTING.CALL:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("CALL" + playerSoundIndex[roundIndex].ToString())), false);

                player_info_slots[roundIndex].CallAni();
                break;
            case BETTING.BBING:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("BBING" + playerSoundIndex[roundIndex].ToString())), false);

                player_info_slots[roundIndex].BBingAni();
                break;
            case BETTING.QUATER:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("QUATER" + playerSoundIndex[roundIndex].ToString())), false);

                player_info_slots[roundIndex].QuaterAni();
                break;
            case BETTING.HARF:
                //저녁때만 하프 사운드 적용
                if (currentRound != GameRound.EVENING)
                    m_iHalfLevelCount = 0;

                string strHalfSoundName = string.Format("HALF{0}_{1}", playerSoundIndex[roundIndex].ToString(), m_iHalfLevelCount);
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), (strHalfSoundName)), false);
                m_iHalfLevelCount++;
                if (m_iHalfLevelCount > 5)
                    m_iHalfLevelCount = 5;

                player_info_slots[roundIndex].HarfAni();
                break;
            case BETTING.DIE:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("DIE" + playerSoundIndex[roundIndex].ToString())), false);

                bettingUI = UIManager.Instance.get_uipage(UI_PAGE_BADUK.BETTING).GetComponent<PopupBetting>();
                bettingUI.DeactiveAllButton();
                StartCoroutine(dieCardAni((byte)roundIndex));
                player_info_slots[roundIndex].DieAni();

                //다이 했을때 방나가기 가능하게(취소)
                if (_player_index == player_me_index) isGaming = false;
                break;
            case BETTING.CHECK:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("CHECK" + playerSoundIndex[roundIndex].ToString())), false);

                player_info_slots[roundIndex].CheckAni();
                break;
            case BETTING.DDADDANG:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("DDADANG" + playerSoundIndex[roundIndex].ToString())), false);

                player_info_slots[roundIndex].DdadangAni();
                break;
        }
    }
    public void on_server_statics(CMessage msg)
    {
        byte playerIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);
        byte bettingtype;
        Rmi.Marshaler.Read(msg, out bettingtype);
        long currentmoney;
        Rmi.Marshaler.Read(msg, out currentmoney);
        long totoalgamemoney;
        Rmi.Marshaler.Read(msg, out totoalgamemoney);
        long callmoney;
        Rmi.Marshaler.Read(msg, out callmoney);
        int roundIndex = ((userRoundIndex - player_me_index) + playerIndex) % userRoundIndex;
        this.player_info_slots[roundIndex].update_money(currentmoney);
        gameInfo.update_totalmoney(totoalgamemoney);
        gameInfo.update_call(callmoney);
    }

    IEnumerator on_server_exchangeCard(CMessage msg)
    {
        bCountDown = false;
        keyState = UserInputState.NONE;

        byte players_count;
        Rmi.Marshaler.Read(msg, out players_count);
        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);

        int roundIndex = ((userRoundIndex - player_me_index) + player_index) % userRoundIndex;

        byte cardchange;
        Rmi.Marshaler.Read(msg, out cardchange);
        CHANGECARD changeCard = (CHANGECARD)cardchange;
        byte cardCount = 0;

        StopCountDownSound();
        player_info_slots[roundIndex].CountDown.SetActive(false);
        //★★★★★★★★★
        if (changeCard == CHANGECARD.PASS)
        {
            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("PASS" + playerSoundIndex[roundIndex].ToString())), false);

            player_info_slots[roundIndex].PassAni();
        }
        else
        {
            Dictionary<byte, CardInfo.sCARD_INFO> changeCards =
            new Dictionary<byte, CardInfo.sCARD_INFO>();
            Rmi.Marshaler.Read(msg, out cardCount);

            for (int i = 0; i < cardCount; ++i)
            {
                byte index;
                Rmi.Marshaler.Read(msg, out index);
                if (index != byte.MaxValue)
                {
                    ClassLibraryCardInfo.CardInfo.sCARD_INFO card =
                              new ClassLibraryCardInfo.CardInfo.sCARD_INFO();
                    byte cardNum;
                    Rmi.Marshaler.Read(msg, out cardNum);
                    card.m_nCardNum = cardNum;
                    byte shape;
                    Rmi.Marshaler.Read(msg, out shape);
                    card.m_nShape = shape;
                    changeCards.Add(index, card);
                }
            }

            byte grade;
            Rmi.Marshaler.Read(msg, out grade);
            byte number;
            Rmi.Marshaler.Read(msg, out number);

            m_coEnumerator[(int)_eEnumeratorList.EET_exchangeCardAni] = exchangeCardAni(grade, number, (byte)roundIndex, player_index, changeCards);
            yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_exchangeCardAni]);
            //카드 배포하는 애니메이션 
            StartCoroutine(player_info_slots[roundIndex].changeAni(cardCount));

            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("C" + cardCount.ToString() + "_" + playerSoundIndex[roundIndex].ToString())), false);
        }
        switch (currentRound)
        {
            case GameRound.MORNING:

                if (changeCard == CHANGECARD.PASS)
                {
                    player_info_slots[roundIndex].MorningSet(5);
                }
                else
                {
                    player_info_slots[roundIndex].MorningSet(cardCount);
                }
                break;
            case GameRound.AFTERNOON:
                if (changeCard == CHANGECARD.PASS)
                {
                    player_info_slots[roundIndex].AfternoongSet(5);
                }
                else
                {
                    player_info_slots[roundIndex].AfternoongSet(cardCount);
                }
                break;
            case GameRound.EVENING:
                if (changeCard == CHANGECARD.PASS)
                {
                    player_info_slots[roundIndex].EveningSet(5);
                }
                else
                {
                    player_info_slots[roundIndex].EveningSet(cardCount);
                }
                break;
        }
    }
    IEnumerator on_server_change_round(CMessage msg)
    {
        int user;
        Rmi.Marshaler.Read(msg, out user);
        int myturn;
        Rmi.Marshaler.Read(msg, out myturn);

        yield return new WaitForSeconds(0.5f);

        byte bround;
        Rmi.Marshaler.Read(msg, out bround);
        GameRound round = (GameRound)bround;

        m_iHalfLevelCount = 0;

        switch (round)
        {
            case GameRound.MORNING:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("MORNING" + playerSoundIndex[0].ToString())), false);
                currentRound = GameRound.MORNING;

                MorningAni.GetComponent<tk2dSpriteAnimator>().Play();

                break;
            case GameRound.AFTERNOON:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("NOON" + playerSoundIndex[0].ToString())), false);
                currentRound = GameRound.AFTERNOON;

                AfternoonAni.GetComponent<tk2dSpriteAnimator>().Play();

                break;
            case GameRound.EVENING:
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), ("EVENING" + playerSoundIndex[0].ToString())), false);
                currentRound = GameRound.EVENING;

                EveningAni.GetComponent<tk2dSpriteAnimator>().Play();

                break;
        }

        //===========================
        //START KWH
        if (nowDayState != null)
        {
            nowDayState.Play(round);
        }
        //END KWH
        //===========================

        long callmoney;
        Rmi.Marshaler.Read(msg, out callmoney);
        gameInfo.update_call(callmoney);
        for (int i = 0; i < this.player_info_slots.Count; ++i)
        {
            this.player_info_slots[i].IconResetAni();
        }
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.WAIT_CHANGE;
        //아침, 점심, 저녘
    }
    IEnumerator on_server_req_open(CMessage msg)
    {
        yield return new WaitForSeconds(0.2f);

        bettingUI = UIManager.Instance.get_uipage(UI_PAGE_BADUK.BETTING).GetComponent<PopupBetting>();
        bettingUI.DeactiveAllButton();
        for (int i = 0; i < player_info_slots.Count; ++i)
        {
            player_info_slots[i].IconResetAni();
        }
        m_coEnumerator[(int)_eEnumeratorList.EET_on_server_card_open] = on_server_card_open(msg);
        yield return StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_on_server_card_open]);
    }
    #endregion

    #region 게임준비
    void make_deck_cards()
    {
        CSpriteLayerOrderManager.Instance.reset();
        Vector3 pos = DeckPos;
        for (int i = 0; i < card_manager.cards.Count; ++i)
        {
            total_card_pictures[i].gameObject.SetActive(true);
            //obj.GetComponent<Image>().color = back_red;
        }
        deck_cards.Clear();
        for (int i = 0; i < total_card_pictures.Count; ++i)
        {
            Animator ani = total_card_pictures[i].GetComponentInChildren<Animator>();
            //ani.Play("card_idle");

            total_card_pictures[i].update_backcard(back_image);
            total_card_pictures[i].enable_collider(false);
            total_card_pictures[i].SetSelect(false);
            total_card_pictures[i].set_slot_index(byte.MaxValue);
            total_card_pictures[i].set_server_slot_index(byte.MaxValue);

            deck_cards.Push(total_card_pictures[i]);

            total_card_pictures[i].transform.localPosition = pos;
            total_card_pictures[i].transform.localScale = Vector3.one;
            total_card_pictures[i].transform.transform.localRotation = Quaternion.identity;
            total_card_pictures[i].transform.transform.Rotate(new Vector3(0, 0, 90));

            // this.total_card_pictures[i].sprite_renderer.sortingOrder =
            //   CSpriteLayerOrderManager.Instance.Order;
            total_card_pictures[i].sprite_renderer.enabled = false;
        }
    }

    IEnumerator move_card(CCardPicture card_picture, Vector3 to, float speed, bool isStartCardOpen, int rotateType, bool isCardHide)
    {
        // 카드 그림장 바꾸기
        if (isStartCardOpen)
        {
            // 보여주기
            if (card_picture != null)
                card_picture.update_image(get_card_sprite(card_picture.card));
        }
        else
        {
            // 안보여주기
            if (card_picture != null)
                card_picture.update_image(back_image);
        }

        // 이동
        if (card_picture != null)
            LeanTween.move(card_picture.gameObject, to, speed).setEase((LeanTweenType)9);

        // 0 == 덱->유저 // 1 == 유저->덱 // 2 == 덱->덱
        // 회전 180=반바퀴, 720=한바퀴
        // LeanTween라이브러리가 좆같은 버그때문에 로테이션 제대로 안먹혀서 위->아래랑 아래->위랑 로테이션값을 하나씩 대입하면서 찾았다
        if (card_picture != null)
        {
            if (rotateType == 0) LeanTween.rotateZ(card_picture.gameObject, -720, speed).setEase((LeanTweenType)9);
            else if (rotateType == 1) LeanTween.rotateZ(card_picture.gameObject, 180, speed).setEase((LeanTweenType)9);
        }
        // 181019 유저->덱 일때 카드가 카드덱 아래로 들어가게 하기위해 만듦
        if (rotateType == 1) DeckCardImage.GetComponent<SpriteRenderer>().sortingOrder = 255;

        // 대기
        yield return new WaitForSeconds(speed);
        while (true)
        {
            if (card_picture == null)
                break;

            if (card_picture.gameObject.transform.localPosition == to)
                break;
            yield return null;
        }

        if (card_picture != null)
            card_picture.gameObject.transform.localRotation = Quaternion.identity;

        // 유저->덱으로 가는 카드의 경우 다시 옆으로 뉘여줌
        if (rotateType == 1 && card_picture != null) card_picture.gameObject.transform.Rotate(0, 0, 90);

        // 카드 그림장 바꾸기
        if (card_picture != null && card_picture.card.m_btIsState == 0)
        {
            // 보여주기
            card_picture.update_image(get_card_sprite(card_picture.card));
        }
        else if (card_picture != null)
        {
            // 안보여주기
            card_picture.update_image(back_image);
        }

        // 딜러에게 카드를 다시 줬을때
        if (isCardHide && card_picture != null)
        {
            card_picture.sprite_renderer.enabled = false;
        }

        // 181019 유저->덱 일때 카드가 카드덱 아래로 들어가게 하기위해 만듦
        if (rotateType == 1) DeckCardImage.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }
    //---------------------------------------------------------------------------------------------------------------
    public Sprite get_card_sprite(CardInfo.sCARD_INFO card)
    {
        int sprite_index = card.m_nCardNum * 4 + card.m_nShape;
        return CSpriteManager.Instance.get_card_sprite(sprite_index);

    }
    #endregion

    #region 카드교환
    public void ChangeCard(List<CCardPicture> cards)
    {
        if (cards.Count == 0)
        {
            //Debug.Log("카드를 선택해주세요");
        }
        else
        {
            card_collision_manager.enabled = false;
            player_hand_card_manager[0].enable_all_colliders(false);
            ChangeCardAnimation(cards);
        }
    }

    void ChangeCardAnimation(List<CCardPicture> cards)
    {
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionChangeCard;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)CHANGECARD.CHANGE);
        Rmi.Marshaler.Write(newmsg, (byte)cards.Count);
        for (int i = 0; i < cards.Count; ++i)
        {
            //sungwon 05-20 slot -> server_slot
            Rmi.Marshaler.Write(newmsg, (byte)cards[i].server_slot);
            Rmi.Marshaler.Write(newmsg, (byte)cards[i].card.m_nCardNum);
            Rmi.Marshaler.Write(newmsg, (byte)cards[i].card.m_nShape);
        }
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }

    public bool HiddenCardStop;
    public bool HiddenCardSkip;
    IEnumerator PlayHiddenCard(byte grade, byte number, byte roundIndex)
    {
        HiddenCardStop = false;
        HiddenCardSkip = false;

        m_objHiddenCard.SetActive(true);
        m_objHiddenCard.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        LeanTween.move(m_objHiddenCard, new Vector3(200.0f, -255.0f, 0.0f), 1.5f).setEase((LeanTweenType)14);
        for (int i = 0; i < 15; ++i)
        {
            if (HiddenCardStop == true)
            {
                LeanTween.cancel(m_objHiddenCard);
                HiddenCardStop = false;
            }

            if (HiddenCardSkip == true)
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
        HiddenCardSkip = true;
        LeanTween.move(m_objHiddenCard, new Vector3(200.0f, -640.0f, 0.0f), 0.2f).setEase((LeanTweenType)14);
        yield return new WaitForSeconds(0.2f);
        m_objHiddenCard.SetActive(false);

        //yield return new WaitForSeconds(0.1f);

        // 카드 힌트. 저녁에는 교체 없으니 힌트도 없음
        bool cardHint;
        if (currentRound == GameRound.EVENING)
            cardHint = false;
        else
            cardHint = true;
        CardInfo(grade, number, 0, cardHint);

        //카드 입력 활성화
        this.card_collision_manager.enabled = true;
        this.player_hand_card_manager[roundIndex].enable_all_colliders(true);
    }

    private IEnumerator exchangeCardAni(byte grade, byte number, byte roundIndex, byte _playerIndex, Dictionary<byte, CardInfo.sCARD_INFO> player_cards)
    {
        for (int card_index = 0; card_index < player_cards.Count; ++card_index)
        {
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_OPEN, false);

            var item = player_cards.ElementAt(card_index);
            byte handIndex = item.Key;
            CCardPicture card;

            //2019-05-17 sungwon 
            if (_playerIndex == this.player_me_index)
                card = player_hand_card_manager[roundIndex].get_card_from_slot_playercard(handIndex);
            else
                card = player_hand_card_manager[roundIndex].get_card_from_slot(handIndex);
            if (card == null)
            {
                StopCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_exchangeCardAni]);
            }
            StartCoroutine(move_card(card, DeckPos, 0.1f, false, 1, true));

            //yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds(0.08f);

            byte byCheckSlotIndex = card.slot;
            card.set_slot_index(byte.MaxValue);
            card.set_server_slot_index(byte.MaxValue);

            card.SetSelect(false);
            deck_cards.Push(card);

            //2019-05-17 sungwon 
            if (_playerIndex == this.player_me_index)
                player_hand_card_manager[roundIndex].remove_playercard(byCheckSlotIndex);
            else
                player_hand_card_manager[roundIndex].remove(byCheckSlotIndex);
        }

        //----------------
        // 2019-0614 sungwon 카드 교환할때 정렬 추가
        player_hand_card_manager[roundIndex].Sorting();
        byte iSortIndex = 0;
        bool bOpenCard = false;
        if (_playerIndex == this.player_me_index)
        {
            bOpenCard = true;
        }

        for (int i = 0; i < player_hand_card_manager[roundIndex].get_card_count(); ++i)
        {
            CCardPicture card2 = (CCardPicture)player_hand_card_manager[roundIndex].getCardPictures()[i];
            if (card2 != null && card2.slot < player_card_positions[roundIndex].hands.Count && !card2.select)
            {
                StartCoroutine(move_card(card2, player_card_positions[roundIndex].get_hand_position(card2.slot), 0.15f, bOpenCard, 2, false));
                iSortIndex++;
            }
        }
        //----------------

        yield return new WaitForSeconds(0.1f);

        bool isHiddenCard = false;
        // 플레이어에게 카드를 분배한다.
        for (int card_index = 0; card_index < player_cards.Count; ++card_index)
        {
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_HOPEN, false);

            // 본인 카드는 해당 이미지를 보여주고,
            // 상대방 카드(is_nullcard)는 back_image로 처리한다.
            if (_playerIndex == this.player_me_index)
            {
                var item = player_cards.ElementAt(card_index);
                // 2019-0614 sungwon 카드 교환할때 정렬 수정
                byte handIndex = iSortIndex++;// item.Key;
                CardInfo.sCARD_INFO card = item.Value;
                CCardPicture card_picture = this.deck_cards.Pop();
                card_picture.sprite_renderer.enabled = true;
                card_picture.set_slot_index((byte)handIndex);
                card_picture.set_server_slot_index((byte)item.Key);

                card_picture.SetSelect(false);
                card_picture.update_card(card, get_card_sprite(card));
                card_picture.sprite_renderer.sortingOrder = handIndex;
                card_picture.set_slot_index(handIndex);
                card_picture.set_server_slot_index((byte)item.Key);

                StartCoroutine(move_card(card_picture, player_card_positions[roundIndex].get_hand_position(handIndex), 0.1f, false, 0, false));
                //2019-05-17 sungwon 히든카드 추가
                if (currentRound == GameRound.EVENING && iSortIndex == 4)
                {
                    //yield return StartCoroutine(PlayHiddenCard());
                    isHiddenCard = true;
                    StartCoroutine(PlayHiddenCard(grade, number, roundIndex));
                }
                player_hand_card_manager[roundIndex].set_card(handIndex, card_picture);
            }
            else
            {
                var item = player_cards.ElementAt(card_index);
                // 2019-0614 sungwon 카드 교환할때 정렬 수정
                byte handIndex = iSortIndex++;// item.Key;

                CardInfo.sCARD_INFO card = item.Value;
                CCardPicture card_picture = this.deck_cards.Pop();
                card_picture.sprite_renderer.enabled = true;
                card_picture.set_slot_index((byte)handIndex);
                card_picture.set_server_slot_index((byte)item.Key);

                card_picture.SetSelect(false);
                card_picture.update_backcard(this.back_image);
                card_picture.sprite_renderer.sortingOrder = handIndex;
                card_picture.set_slot_index(handIndex);
                card_picture.set_server_slot_index((byte)item.Key);

                StartCoroutine(move_card(card_picture, player_card_positions[roundIndex].get_hand_position(handIndex), 0.15f, false, 0, false));
                player_hand_card_manager[roundIndex].set_card(handIndex, card_picture);
            }

            yield return new WaitForSeconds(0.15f);
        }

        if (_playerIndex == this.player_me_index)
        {
            // 히든 연출중이면 처리 안함
            if (isHiddenCard == false)
            {
                yield return new WaitForSeconds(0.1f);

                // 카드 힌트. 저녁에는 교체 없으니 힌트도 없음
                bool cardHint;
                if (currentRound == GameRound.EVENING)
                    cardHint = false;
                else
                    cardHint = true;
                CardInfo(grade, number, 0, cardHint);

                //카드 입력 활성화
                this.card_collision_manager.enabled = true;
                this.player_hand_card_manager[roundIndex].enable_all_colliders(true);
            }
        }

    }

    private void CardInfo(byte grade, byte number, int playerIndex, bool select = true)
    {
        //player_hand_card_manager[playerIndex].UnselectAllCards();
        List<CardInfo.sCARD_INFO> cardinfos = new List<CardInfo.sCARD_INFO>();
        badCards = new bool[4];
        cardinfos = player_hand_card_manager[playerIndex].getCards();
        CardInfo info = new CardInfo();
        info.SetCard(cardinfos.ToArray());
        info.MakeResult();
        info.AutoSelect(badCards);

        if (select)
        {
            for (int i = 0; i < badCards.Length; i++)
            {
                if (badCards[i] == true)
                {
                    player_hand_card_manager[playerIndex].get_card(i).SetAutoSelect();
                }
            }
        }

        // 카드 족보 이름
        string HandName = info.GetHandNumber();

        if (playerIndex == 0)
        {
            if (myjockbo != null)
            {
                myjockbo.UpdateCardInfo(grade);
            }
        }

        //player_info_slots[playerIndex].update_cardinfo(grade, number);
        player_info_slots[playerIndex].update_cardinfo(grade, HandName);
    }
    #endregion

    #region 다이
    private IEnumerator dieCardAni(byte _playerIndex)
    {
        for (byte i = 0; i < 4; ++i)
        {
            //---------------
            //2019-05-14 sungwon 카드정렬 추가
            if (player_hand_card_manager[_playerIndex].get_card_from_slot(i) != null)
            //---------------
            {
                player_hand_card_manager[_playerIndex].get_card_from_slot(i).SetSelect(false);
                player_hand_card_manager[_playerIndex].get_card_from_slot(i).SetPosition(player_card_positions[_playerIndex].get_hand_position(i));
                player_hand_card_manager[_playerIndex].get_card_from_slot(i).update_backcard(back_image);
            }
        }

        Vector3 pos = player_card_positions[_playerIndex].get_hand_position(0);

        StartCoroutine(move_card(player_hand_card_manager[_playerIndex].get_card_from_slot(1), new Vector3(pos.x + 5, pos.y, pos.z), 0.01f, false, 2, false));

        yield return new WaitForSeconds(0.01f);

        StartCoroutine(move_card(player_hand_card_manager[_playerIndex].get_card_from_slot(2), new Vector3(pos.x + 10, pos.y, pos.z), 0.03f, false, 2, false));

        yield return new WaitForSeconds(0.03f);

        StartCoroutine(move_card(player_hand_card_manager[_playerIndex].get_card_from_slot(3), new Vector3(pos.x + 15, pos.y, pos.z), 0.05f, false, 2, false));

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator dieCardAniInsta(byte _playerIndex) // 관전시
    {
        for (byte i = 0; i < 4; ++i)
        {
            player_hand_card_manager[_playerIndex].get_card_from_slot(i).SetSelect(false);
            player_hand_card_manager[_playerIndex].get_card_from_slot(i).SetPosition(player_card_positions[_playerIndex].get_hand_position(i));
            player_hand_card_manager[_playerIndex].get_card_from_slot(i).update_backcard(back_image);
        }

        Vector3 pos = player_card_positions[_playerIndex].get_hand_position(0);
        StartCoroutine(move_card(player_hand_card_manager[_playerIndex].get_card_from_slot(1), new Vector3(pos.x + 5, pos.y, pos.z), 0.01f, false, 2, false));
        StartCoroutine(move_card(player_hand_card_manager[_playerIndex].get_card_from_slot(2), new Vector3(pos.x + 10, pos.y, pos.z), 0.03f, false, 2, false));
        StartCoroutine(move_card(player_hand_card_manager[_playerIndex].get_card_from_slot(3), new Vector3(pos.x + 15, pos.y, pos.z), 0.05f, false, 2, false));

        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region 카드 오픈
    IEnumerator on_server_card_open(CMessage msg)
    {
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_OPEN, false);

        byte _playerIndex;
        Rmi.Marshaler.Read(msg, out _playerIndex);
        string playerHand;
        Rmi.Marshaler.Read(msg, out playerHand);
        byte cardCount;
        Rmi.Marshaler.Read(msg, out cardCount);
        int roundIndex = ((userRoundIndex - player_me_index) + _playerIndex) % userRoundIndex;

        for (byte i = 0; i < 4; ++i)
        {
            if (player_hand_card_manager[roundIndex].get_card_from_slot(i) == null)
                StopCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_on_server_card_open]);
            if (player_hand_card_manager[roundIndex].get_card_from_slot(i) != null)
            {
                player_hand_card_manager[roundIndex].get_card_from_slot(i).SetSelect(false);
                player_hand_card_manager[roundIndex].get_card_from_slot(i).SetPosition(player_card_positions[roundIndex].get_hand_position(i));
                player_hand_card_manager[roundIndex].get_card_from_slot(i).card.m_btIsState = 0;
                if (_playerIndex != player_me_index)
                {
                    player_hand_card_manager[roundIndex].get_card_from_slot(i).update_backcard(back_image);
                }
            }
        }

        if (player_card_positions[roundIndex].hands.Count > 3)
        {
            Vector3 pos = player_card_positions[roundIndex].get_hand_position(0);
            StartCoroutine(move_card(player_hand_card_manager[roundIndex].get_card_from_slot(1), new Vector3(pos.x + 5, pos.y, pos.z), 0.01f, false, 2, false));
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(move_card(player_hand_card_manager[roundIndex].get_card_from_slot(2), new Vector3(pos.x + 10, pos.y, pos.z), 0.03f, false, 2, false));
            yield return new WaitForSeconds(0.03f);
            StartCoroutine(move_card(player_hand_card_manager[roundIndex].get_card_from_slot(3), new Vector3(pos.x + 15, pos.y, pos.z), 0.05f, false, 2, false));
            yield return new WaitForSeconds(0.5f);
        }
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_HOPEN, false);

        // 패 결과 보여주기
        string gradeName = "";
        Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cardsq = new Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
        for (int i = 0; i < cardCount; i++)
        {
            byte cardNum;
            Rmi.Marshaler.Read(msg, out cardNum);
            byte cardShape;
            Rmi.Marshaler.Read(msg, out cardShape);
            byte isState;
            Rmi.Marshaler.Read(msg, out isState);
            ClassLibraryCardInfo.CardInfo.sCARD_INFO card = new ClassLibraryCardInfo.CardInfo.sCARD_INFO();
            card.m_nCardNum = cardNum;
            card.m_nShape = cardShape;
            card.m_btIsState = isState;
            player_hand_card_manager[roundIndex].set_cardinfo((byte)i, card);
            cardsq.Enqueue(card);
        }

        ClassLibraryCardInfo.CardInfo cardinfo = new ClassLibraryCardInfo.CardInfo();
        cardinfo.SetCard(cardsq.ToArray());
        cardinfo.MakeResult();
        cardinfo.GetTotalScore();
        gradeName = cardinfo.GetCardName2();

        player_info_slots[roundIndex].ShowCard(gradeName, cardinfo.m_nResult >= 4);
        PlayResultSound(cardinfo.m_nResult, cardinfo.GetTopNumber());

        if (player_card_positions[roundIndex].hands.Count > 3)
        {
            Vector3 pos = player_card_positions[roundIndex].get_hand_position(0);
            StartCoroutine(move_card(player_hand_card_manager[roundIndex].get_card_from_slot(3), player_card_positions[roundIndex].get_hand_position(3), 0.05f, true, 2, false));
            StartCoroutine(move_card(player_hand_card_manager[roundIndex].get_card_from_slot(2), player_card_positions[roundIndex].get_hand_position(2), 0.03f, true, 2, false));
            StartCoroutine(move_card(player_hand_card_manager[roundIndex].get_card_from_slot(1), player_card_positions[roundIndex].get_hand_position(1), 0.01f, true, 2, false));
        }
        yield return new WaitForSeconds(0.5f);
    }
    #endregion

    void PlaySound(SoundManager._eSoundResource eSound, int iNumber, int iIndex)
    {
        // 메이드
        int iStartSoundIndex = (int)eSound;
        int iSoundIndex = iStartSoundIndex + iNumber;

        //if (iNumber >= 2 && iNumber <= 12)
        {
            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)iSoundIndex - iIndex, false);
        }
    }

    public void PlayResultSound(int _grade, int _number)
    {
        if (_grade == 7)
        {
            //골프
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.GOLF, false);
        }
        else if (_grade == 6)
        {
            //세컨
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.SECOND, false);
        }
        else if (_grade == 5)
        {
            //써드
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.THIRD, false);
        }
        else if (_grade == 4)
        {
            // 메이드
            PlaySound(SoundManager._eSoundResource.MADE_5, _number, 4);
        }
        else if (_grade == 3)
        {
            // 베이스
            PlaySound(SoundManager._eSoundResource.BASE_3, _number, 2);
        }
        else if (_grade == 2)
        {
            // 투베이스
            PlaySound(SoundManager._eSoundResource.TWOBASE_2, _number, 1);
        }
        else
        {
            // 노베이스
            PlaySound(SoundManager._eSoundResource.NOBASE_1, _number, 0);
        }
    }

    #region 카드 갱신
    void refresh_player_hand_slots(byte player_index)
    {
        int roundIndex = ((userRoundIndex - player_me_index) + player_index) % userRoundIndex;
        CPlayerHandCardManager hand_card_manager = this.player_hand_card_manager[roundIndex];
        byte count = (byte)hand_card_manager.get_card_count();
        for (byte card_index = 0; card_index < count; ++card_index)
        {
            CCardPicture card = hand_card_manager.get_card(card_index);
            // 슬롯 인덱스를 재설정 한다.
            card.set_slot_index(card_index);
            card.set_server_slot_index(card_index);
            // 화면 위치를 재설정 한다.
            card.transform.position = this.player_card_positions[roundIndex].get_hand_position(card_index);
        }
    }
    #endregion

    #region 카드입력이벤트
    //카드 콜라이더 이벤트
    private void on_card_touch(CCardPicture card_picture)
    {
        card_picture.SetSelectCard();
    }
    #endregion

    #region 내인덱스
    bool is_me(byte player_index)
    {
        return this.player_me_index == player_index;
    }
    #endregion

    #region 잭팟
    bool overlapPrevent = true;
    long NowJackpotMoney = 0;
    IEnumerator SC_EVENT_JACKPOT_INFO(CMessage msg)
    {
        long jackpotMoney;
        Rmi.Marshaler.Read(msg, out jackpotMoney);

        JackpotManager.Instance.JackpotMoney = jackpotMoney;

        yield return null;
    }

    List<GameObject> ListJackpotNumberObject = new List<GameObject>();
    void JackpotRefresh(long number)
    {
        // 초기화
        foreach (var obj in ListJackpotNumberObject) Destroy(obj);
        ListJackpotNumberObject.Clear();

        // 숫자 저장
        string num = number.ToString();
        char[] _num = new char[num.Length];
        _num = num.ToCharArray();
        System.Array.Reverse(_num);

        // 등차수열에 사용될 위치값 및 공차
        float numberX = 490;
        float numberY = 330;
        float numberZ = 0;
        float numberD = -12;

        float dotY = 330;
        float dotZ = 0;
        float dotD = -9;

        numberX += ((num.Length - 1) * 6);

        int dotCount = num.Length % 3 > 0 ? num.Length / 3 : num.Length / 3 - 1;

        numberX += dotCount * 4.5f;

        for (int i = 0; i < _num.Length; i++)
        {
            // 3자리가 넘어가면 dot 생성
            if (i % 3 == 0 && i != 0)
            {
                GameObject g = Instantiate(PrefabFont);

                g.GetComponent<tk2dSprite>().SetSprite("jackpot_dot");

                numberX += dotD;

                float posX = 0;

                posX = numberX + (i - 1) * numberD;

                g.transform.position = new Vector3(posX, dotY, dotZ);

                ListJackpotNumberObject.Add(g);
            }

            {
                GameObject g = Instantiate(PrefabFont);

                g.GetComponent<tk2dSprite>().SetSprite("jackpot_" + _num[i].ToString());

                float posX = 0;

                if (i % 3 == 0 && i != 0) numberX -= numberD - dotD;

                posX = numberX + i * numberD;

                g.transform.position = new Vector3(posX, numberY, numberZ);

                ListJackpotNumberObject.Add(g);
            }
        }
    }
    #endregion

    #region 유저정보
    void UserInfoRefresh(CMessage msg)
    {
        byte playerIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);

        Rmi.Marshaler.UserInfo userInfo = new Rmi.Marshaler.UserInfo();
        Rmi.Marshaler.Read(msg, out userInfo);

        bool join;
        Rmi.Marshaler.Read(msg, out join);

        if (join)
        {
            //UIManager.Instance.ReportText(userInfo.nickName + "님 입장");
        }

        int roundIndex = ((userRoundIndex - player_me_index) + playerIndex) % userRoundIndex;

        playerSoundIndex[playerIndex] = 1;// userInfo.voice;

        player_info_slots[roundIndex].Allreset(true);
        player_info_slots[roundIndex].name_text.gameObject.SetActive(true);
        player_info_slots[roundIndex].money_text.gameObject.SetActive(true);
        //player_info_slots[roundIndex].grade[0].gameObject.SetActive(true);
        //player_info_slots[roundIndex].grade[1].gameObject.SetActive(true);
        player_info_slots[roundIndex].avatar.SetActive(true);

        if (csAndroidManager.StoreAsset != null && player_info_slots[roundIndex].avatar.GetComponent<RawImage>().texture == null)
        {
            try
            {
                player_info_slots[roundIndex].avatar.GetComponent<RawImage>().texture = (Texture)csAndroidManager.StoreAsset.LoadAsset(userInfo.avatar);
            }
            catch (Exception e)
            {
                player_info_slots[roundIndex].avatar.GetComponent<RawImage>().texture = (Texture)csAndroidManager.StoreAsset.LoadAsset("avatar_honggildong_default");
            }
        }
        player_info_slots[roundIndex].update_name(userInfo.nickName);
        player_info_slots[roundIndex].update_money(userInfo.money_game);

    }
    #endregion

    #region 유저 나갔을때
    void UserRoomOut(CMessage msg)
    {
        byte playerIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);
        bool init;
        Rmi.Marshaler.Read(msg, out init);

        if (init)
        {
            int roundIndex = ((userRoundIndex - player_me_index) + playerIndex) % userRoundIndex;

            //UIManager.Instance.ReportText(player_info_slots[roundIndex].name_text.text + "님 퇴장");

            player_info_slots[roundIndex].name_text.gameObject.SetActive(false);
            player_info_slots[roundIndex].money_text.gameObject.SetActive(false);
            //player_info_slots[roundIndex].grade[0].gameObject.SetActive(false);
            //player_info_slots[roundIndex].grade[1].gameObject.SetActive(false);
            player_info_slots[roundIndex].avatar.SetActive(false);
            player_info_slots[roundIndex].Allreset();
            player_info_slots[roundIndex].SetReservationIcon(false);

            isGaming = false;
            isCounterOver = false;

            // 초기화
            // 게임결과 연출 끝
            for (int i = 0; i < player_info_slots.Count; ++i)
            {
                player_info_slots[i].ResultBG.SetActive(false);
                //player_info_slots[i].ResultMoney.SetActive(false);
            }

            // 게임 끝나면 리셋
            reset();
            if (UIManager.Instance.RoomMoveOutReserved())
            {
                // 자동레디
                on_server_start();
            }
        }
        else
        {
            int roundIndex = ((userRoundIndex - player_me_index) + playerIndex) % userRoundIndex;

            //UIManager.Instance.ReportText(player_info_slots[roundIndex].name_text.text + "님 퇴장");

            player_info_slots[roundIndex].name_text.gameObject.SetActive(false);
            player_info_slots[roundIndex].money_text.gameObject.SetActive(false);
            //player_info_slots[roundIndex].grade[0].gameObject.SetActive(false);
            //player_info_slots[roundIndex].grade[1].gameObject.SetActive(false);
            player_info_slots[roundIndex].avatar.SetActive(false);
            player_info_slots[roundIndex].Allreset();
            player_info_slots[roundIndex].SetReservationIcon(false);
            /* NULL 참조 에러
            player_hand_card_manager[roundIndex].getCardPictures()[0].gameObject.SetActive(false);
            player_hand_card_manager[roundIndex].getCardPictures()[1].gameObject.SetActive(false);
            player_hand_card_manager[roundIndex].getCardPictures()[2].gameObject.SetActive(false);
            player_hand_card_manager[roundIndex].getCardPictures()[3].gameObject.SetActive(false);
            */
        }
    }
    #endregion

    #region 게임결과
    IEnumerator SERVER_GAME_RESULT(CMessage msg)
    {
        // 플레이어 턴 종료
        foreach (var g in playerTurn) g.SetActive(false);

        currentRound = GameRound.END;

        //===========================
        //START KWH
        if (nowDayState != null)
        {
            nowDayState.TurnOff();
        }
        //END KWH
        //===========================

        byte myResult = 0;
        long myVarMoney = 0;

        byte count;
        byte playerIndex;
        byte grade;
        byte number;
        byte win;       // 0==승리, 1==무승부, 2==패배
        long varMoney;
        bool deadPlayer;

        Rmi.Marshaler.Read(msg, out count);

        byte[,,] userCards = new byte[count, 4, 2];

        for (int i = 0; i < count; i++)
        {
            Rmi.Marshaler.Read(msg, out playerIndex);
            Rmi.Marshaler.Read(msg, out grade);
            Rmi.Marshaler.Read(msg, out number);

            Rmi.Marshaler.Read(msg, out userCards[i, 0, 0]);
            Rmi.Marshaler.Read(msg, out userCards[i, 0, 1]);
            Rmi.Marshaler.Read(msg, out userCards[i, 1, 0]);
            Rmi.Marshaler.Read(msg, out userCards[i, 1, 1]);
            Rmi.Marshaler.Read(msg, out userCards[i, 2, 0]);
            Rmi.Marshaler.Read(msg, out userCards[i, 2, 1]);
            Rmi.Marshaler.Read(msg, out userCards[i, 3, 0]);
            Rmi.Marshaler.Read(msg, out userCards[i, 3, 1]);

            //
            string gradeName = "";
            Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cardsq = new Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
            ClassLibraryCardInfo.CardInfo cardinfo = new ClassLibraryCardInfo.CardInfo();
            for (byte j = 0; j < 4; ++j)
            {
                ClassLibraryCardInfo.CardInfo.sCARD_INFO card = new ClassLibraryCardInfo.CardInfo.sCARD_INFO();
                card.m_nCardNum = userCards[i, j, 0];
                card.m_nShape = userCards[i, j, 1];
                card.m_btIsState = 1;
                cardsq.Enqueue(card);
            }
            cardinfo.SetCard(cardsq.ToArray());
            cardinfo.MakeResult();
            cardinfo.GetTotalScore();
            gradeName = cardinfo.GetCardName2();
            //

            Rmi.Marshaler.Read(msg, out win);
            Rmi.Marshaler.Read(msg, out varMoney);
            Rmi.Marshaler.Read(msg, out deadPlayer);

            //if (playerIndex == player_me_index)
            //{
            //    if (win == 0 || win == 1) SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_WIN, false, 0.5f);
            //    else SoundManager.Instance.PlaySound(SoundManager._eSoundResource.C_LOSE, false, 0.5f);
            //}

            if (deadPlayer == true) continue;

            int roundIndex = ((userRoundIndex - player_me_index) + playerIndex) % userRoundIndex;
            StartCoroutine(player_info_slots[roundIndex].ShowResult(gradeName, grade, number, varMoney, win, count));

            if (win == 0)
            {
                player_info_slots[roundIndex].WinnerAni();
                IBettingChipsManager.Instance.MoveChip(roundIndex);
                //yield return StartCoroutine(IBettingChipsManager.Instance.CoMoveChipToWinner(roundIndex));
            }
            else if (win == 1)
            {
                player_info_slots[roundIndex].WinnerAni();
                IBettingChipsManager.Instance.MoveChip(roundIndex);
                //yield return StartCoroutine(IBettingChipsManager.Instance.CoMoveChipToWinner(roundIndex));
            }
            else if (win == 3)
            {
                IBettingChipsManager.Instance.MoveChip(roundIndex);
                //yield return StartCoroutine(IBettingChipsManager.Instance.CoMoveChipToWinner(roundIndex));
            }

            //else player_info_slots[roundIndex].LoserAni();

        }

        // 리포트 관련 정보
        byte reportCount;
        Rmi.Marshaler.Read(msg, out reportCount);
        string[] nick = new string[reportCount];
        byte[,,] cards = new byte[reportCount, 4, 2];
        long[] changeMoney = new long[reportCount];
        for (int i = 0; i < reportCount; ++i)
        {
            Rmi.Marshaler.Read(msg, out nick[i]);
            Rmi.Marshaler.Read(msg, out cards[i, 0, 0]);
            Rmi.Marshaler.Read(msg, out cards[i, 0, 1]);
            Rmi.Marshaler.Read(msg, out cards[i, 1, 0]);
            Rmi.Marshaler.Read(msg, out cards[i, 1, 1]);
            Rmi.Marshaler.Read(msg, out cards[i, 2, 0]);
            Rmi.Marshaler.Read(msg, out cards[i, 2, 1]);
            Rmi.Marshaler.Read(msg, out cards[i, 3, 0]);
            Rmi.Marshaler.Read(msg, out cards[i, 3, 1]);

            Rmi.Marshaler.Read(msg, out changeMoney[i]);
        }
        long dealerMoney;
        Rmi.Marshaler.Read(msg, out dealerMoney);

        string[] strCardInfo = new string[reportCount + 1];
        int[] iCardScores = new int[reportCount + 1];
        for (byte i = 0; i < reportCount; ++i)
        {
            Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO> cardsq = new Queue<ClassLibraryCardInfo.CardInfo.sCARD_INFO>();
            ClassLibraryCardInfo.CardInfo cardinfo = new ClassLibraryCardInfo.CardInfo();
            for (byte j = 0; j < 4; ++j)
            {
                ClassLibraryCardInfo.CardInfo.sCARD_INFO card = new ClassLibraryCardInfo.CardInfo.sCARD_INFO();
                card.m_nCardNum = cards[i, j, 0];
                card.m_nShape = cards[i, j, 1];
                card.m_btIsState = 1;
                cardsq.Enqueue(card);
            }
            cardinfo.SetCard(cardsq.ToArray());
            cardinfo.MakeResult();
            iCardScores[i] = cardinfo.GetTotalScore();
            strCardInfo[i] = cardinfo.GetCardName2();
            //if (changeMoney[i] > 0)
            //    strCardInfo[i] = strCardInfo[i] + "[+" + MoneyConvertToUnit(changeMoney[i]) + "]";
            //else
            //    strCardInfo[i] = strCardInfo[i] + "[-" + MoneyConvertToUnit(changeMoney[i]) + "]";
        }
        // 순위별로 정렬
        if (reportCount > 1)
        {
            for (int i = 0; i < reportCount; ++i)
            {
                for (int j = 0; j < reportCount - 1; ++j)
                {
                    if (iCardScores[j] < iCardScores[j + 1])
                    {
                        var temp1 = iCardScores[j];
                        iCardScores[j] = iCardScores[j + 1];
                        iCardScores[j + 1] = temp1;

                        var temp2 = strCardInfo[j];
                        strCardInfo[j] = strCardInfo[j + 1];
                        strCardInfo[j + 1] = temp2;

                        var temp3 = nick[j];
                        nick[j] = nick[j + 1];
                        nick[j + 1] = temp3;

                        var temp4 = changeMoney[j];
                        changeMoney[j] = changeMoney[j + 1];
                        changeMoney[j + 1] = temp4;
                    }
                }
            }
        }

        // 딜러비
        string strDealerMoney = "▶ 딜러비 : " + MoneyConvertToUnit(dealerMoney);

        // 리포트
        UIManager.Instance.ReportReset(reportCount, nick, strCardInfo, changeMoney);

        // 올인관련 정보
        bool is_noMoney;        // 판돈 부족여부
        Rmi.Marshaler.Read(msg, out is_noMoney);

        // 나가기예약 대기시간
        yield return new WaitForSeconds(1.5f);

        isGaming = false;
        isCounterOver = false;

        yield return new WaitForSeconds(1.0f);

        // 게임결과 연출 끝
        for (int i = 0; i < player_info_slots.Count; ++i)
        {
            player_info_slots[i].ResultBG.SetActive(false);
            //player_info_slots[i].ResultMoney.SetActive(false);
        }

        // 게임 끝나면 리셋
        reset();

        yield return new WaitForSeconds(0.5f);

        // 자동레디
        if (is_noMoney)
        {
            // 돈 없으면 퇴장
            UIManager.Instance.ExitGame();
        }

        if (UIManager.Instance.RoomMoveOutReserved())
        {
            // 자동레디
            on_server_start();
        }


        yield return null;
    }
    #endregion

    public UserInputState keyState = UserInputState.NONE;
    public enum UserInputState { NONE, BET, CHANGE, WAIT_CHANGE }
    IEnumerator GetKeyboradKey()
    {
        while (true)
        {
            yield return null;

            if (Input.anyKey && isAFK == true)
            {
                // 잠수 후 입력 다시 받으면 나가기 예약 취소
                UIManager.Instance.ExitGameReserveCancel();
                isAFK = false;
                isCounterOver = false;
                //UIManager.Instance.ReportText("※ 나가기 예약 취소");
            }

            if (keyState == UserInputState.NONE)
            {

            }
            else if (keyState == UserInputState.BET)
            {
                // 0==콜, 1==삥, 2==쿼터, 3==하프, 4==다이, 5==체크, 6==따당
                int GetKeyValue = -1;

                if (Input.GetKeyDown(KeyCode.Alpha0)) GetKeyValue = 0;
                else if (Input.GetKeyDown(KeyCode.Alpha1)) GetKeyValue = 1;
                else if (Input.GetKeyDown(KeyCode.Alpha2)) GetKeyValue = 2;
                else if (Input.GetKeyDown(KeyCode.Alpha3)) GetKeyValue = 3;
                else if (Input.GetKeyDown(KeyCode.Alpha4)) GetKeyValue = 4;
                else if (Input.GetKeyDown(KeyCode.Alpha5)) GetKeyValue = 5;
                else if (Input.GetKeyDown(KeyCode.Alpha6)) GetKeyValue = 6;
                else if (Input.GetKeyDown(KeyCode.Keypad0)) GetKeyValue = 0;
                else if (Input.GetKeyDown(KeyCode.Keypad1)) GetKeyValue = 1;
                else if (Input.GetKeyDown(KeyCode.Keypad2)) GetKeyValue = 2;
                else if (Input.GetKeyDown(KeyCode.Keypad3)) GetKeyValue = 3;
                else if (Input.GetKeyDown(KeyCode.Keypad4)) GetKeyValue = 4;
                else if (Input.GetKeyDown(KeyCode.Keypad5)) GetKeyValue = 5;
                else if (Input.GetKeyDown(KeyCode.Keypad6)) GetKeyValue = 6;

                if (GetKeyValue == 0 && bettingUI.call.interactable)
                {
                    keyState = UserInputState.NONE;
                    bettingUI.ClickButton((BETTING)GetKeyValue);
                }
                else if (GetKeyValue == 1 && bettingUI.bbing.interactable)
                {
                    keyState = UserInputState.NONE;
                    bettingUI.ClickButton((BETTING)GetKeyValue);
                }
                else if (GetKeyValue == 2 && bettingUI.quater.interactable)
                {
                    keyState = UserInputState.NONE;
                    bettingUI.ClickButton((BETTING)GetKeyValue);
                }
                else if (GetKeyValue == 3 && bettingUI.half.interactable)
                {
                    keyState = UserInputState.NONE;
                    bettingUI.ClickButton((BETTING)GetKeyValue);
                }
                else if (GetKeyValue == 4 && bettingUI.die.interactable)
                {
                    keyState = UserInputState.NONE;
                    bettingUI.ClickButton((BETTING)GetKeyValue);
                }
                else if (GetKeyValue == 5 && bettingUI.check.interactable)
                {
                    keyState = UserInputState.NONE;
                    bettingUI.ClickButton((BETTING)GetKeyValue);
                }
                else if (GetKeyValue == 6 && bettingUI.ddadang.interactable)
                {
                    keyState = UserInputState.NONE;
                    bettingUI.ClickButton((BETTING)GetKeyValue);
                }
            }
            else if (keyState == UserInputState.CHANGE)
            {
                // 0==카드1, 1==카드2, 2==카드3, 3==카드4, 5==바꾸기, 6==패스
                int GetKeyValue = -1;

                if (Input.GetKeyDown(KeyCode.Alpha1)) GetKeyValue = 1;
                else if (Input.GetKeyDown(KeyCode.Alpha2)) GetKeyValue = 2;
                else if (Input.GetKeyDown(KeyCode.Alpha3)) GetKeyValue = 3;
                else if (Input.GetKeyDown(KeyCode.Alpha4)) GetKeyValue = 4;
                else if (Input.GetKey(KeyCode.Alpha5)) GetKeyValue = 5;
                else if (Input.GetKey(KeyCode.Alpha6)) GetKeyValue = 6;
                else if (Input.GetKeyDown(KeyCode.Keypad1)) GetKeyValue = 1;
                else if (Input.GetKeyDown(KeyCode.Keypad2)) GetKeyValue = 2;
                else if (Input.GetKeyDown(KeyCode.Keypad3)) GetKeyValue = 3;
                else if (Input.GetKeyDown(KeyCode.Keypad4)) GetKeyValue = 4;
                else if (Input.GetKey(KeyCode.Keypad5)) GetKeyValue = 5;
                else if (Input.GetKey(KeyCode.Keypad6)) GetKeyValue = 6;
                else if (Input.GetKey(KeyCode.Home)) GetKeyValue = 5;
                else if (Input.GetKey(KeyCode.PageUp)) GetKeyValue = 6;

                if (GetKeyValue == 1)
                {
                    player_hand_card_manager[0].get_card(0).SetAutoSelect();
                }
                else if (GetKeyValue == 2)
                {
                    player_hand_card_manager[0].get_card(1).SetAutoSelect();
                }
                else if (GetKeyValue == 3)
                {
                    player_hand_card_manager[0].get_card(2).SetAutoSelect();
                }
                else if (GetKeyValue == 4)
                {
                    player_hand_card_manager[0].get_card(3).SetAutoSelect();
                }
                else if (GetKeyValue == 5)
                {
                    keyState = UserInputState.NONE;
                    cuttingUI.on_touch_change();
                }
                else if (GetKeyValue == 6)
                {
                    keyState = UserInputState.NONE;
                    cuttingUI.on_touch_pass();
                }
            }
        }
    }

    IEnumerator Counter()
    {
        float overTime = 7.0f;

        float time = 0;
        while (true)
        {
            yield return null;
            time += Time.deltaTime;

            switch (keyState)
            {
                // 아무차례도 아닐때
                case UserInputState.NONE:
                case UserInputState.WAIT_CHANGE:
                    // 시간초기화
                    time = 0;
                    break;
                case UserInputState.BET:
                    if (time >= overTime)
                    {
                        try
                        {
                            // 콜머니가 0원이면 콜 or 체크
                            if (gameInfo.callMoney == 0)
                            {
                                if (bettingUI.call.interactable)
                                {
                                    bettingUI.ClickButton(BETTING.CALL);
                                }
                                else if (bettingUI.check.interactable)
                                {
                                    bettingUI.ClickButton(BETTING.CHECK);
                                }
                                else if (bettingUI.bbing.interactable)
                                {
                                    bettingUI.ClickButton(BETTING.BBING);
                                }
                                else
                                {
                                    bettingUI.ClickButton(BETTING.DIE);
                                }
                            }
                            else
                            {
                                if (bettingUI.die.interactable)
                                {
                                    bettingUI.ClickButton(BETTING.DIE);
                                }
                                else if (bettingUI.call.interactable)
                                {
                                    bettingUI.ClickButton(BETTING.CALL);
                                }
                                else if (bettingUI.check.interactable)
                                {
                                    bettingUI.ClickButton(BETTING.CHECK);
                                }
                                else if (bettingUI.bbing.interactable)
                                {
                                    bettingUI.ClickButton(BETTING.BBING);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            bettingUI.ClickButton(BETTING.DIE);
                        }
                        if (isCounterOver == false)
                        {
                            //UIManager.Instance.ReportText("※ 베팅 시간 초과 - 나가기 예약되었습니다.");
                            isCounterOver = true;
                            isAFK = true;
                            UIManager.Instance.ExitGame();
                        }
                    }
                    break;
                case UserInputState.CHANGE:
                    if (time >= overTime)
                    {
                        cuttingUI.ClickButton(false);
                        if (isCounterOver == false)
                        {
                            //UIManager.Instance.ReportText("※ 카드교체 시간 초과 - 나가기 예약되었습니다.");
                            isCounterOver = true;
                            isAFK = true;
                            UIManager.Instance.ExitGame();
                        }
                    }
                    break;
            }
        }
    }

    void EntrySpectator(CMessage msg)
    {
        byte currentRound;
        Rmi.Marshaler.Read(msg, out currentRound);    // 현재 라운드. 0:패트, 1:아침, 2:점심, 3:저녁
        long totalMoney;
        Rmi.Marshaler.Read(msg, out totalMoney);      // 콜머니 합
        long callMoney;
        Rmi.Marshaler.Read(msg, out callMoney);       // 현재 콜머니

        this.currentRound = (GameRound)currentRound;
        //SetRoundUI((_eROUND)currentRound);
        // 플레이어 라운드 정보
        byte playerCount;
        Rmi.Marshaler.Read(msg, out playerCount);
        for (int i = 0; i < playerCount; ++i)
        {
            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index);

            int roundIndex = ((userRoundIndex - player_me_index) + player_index) % userRoundIndex;

            if (player_index == currentBossIndex)
            {
                player_info_slots[roundIndex].SetBoss(true);
            }

            for (int j = 0; j < 3; ++j)
            {
                byte round_info;
                Rmi.Marshaler.Read(msg, out round_info);

                if (j == 0 && round_info != 0) player_info_slots[roundIndex].MorningSet(round_info);
                else if (j == 1 && round_info != 0) player_info_slots[roundIndex].AfternoongSet(round_info);
                else if (j == 2 && round_info != 0) player_info_slots[roundIndex].EveningSet(round_info);
            }

            for (int card_index = 0; card_index < 4; ++card_index)
            {
                CCardPicture card_picture = deck_cards.Pop();
                card_picture.sprite_renderer.enabled = true;
                card_picture.set_slot_index((byte)card_index);
                card_picture.set_server_slot_index((byte)card_index);
                player_hand_card_manager[roundIndex].add(card_picture);

                card_picture.update_backcard(back_image);
                card_picture.sprite_renderer.sortingOrder = card_index;
                CardShow(card_picture, player_card_positions[roundIndex].get_hand_position(card_index));
            }
            bool die;
            Rmi.Marshaler.Read(msg, out die);
            if (die)
            {
                StartCoroutine(dieCardAniInsta((byte)roundIndex));
                player_info_slots[roundIndex].DieAni();
            }
        }
    }

    void CardShow(CCardPicture card_picture, Vector3 to, bool rotate = true, float duration = 0.15f, bool enable = false, bool start = false)
    {
        if (start)
        {
            card_picture.update_image(get_card_sprite(card_picture.card));
        }
        else
        {
            card_picture.update_image(back_image);
        }

        if (card_picture.card.m_btIsState == 0)
        {
            card_picture.update_image(get_card_sprite(card_picture.card));
        }
        else
        {
            card_picture.update_image(back_image);
        }

        if (!rotate && !enable)
        {
            card_picture.sprite_renderer.enabled = false;
        }

        card_picture.gameObject.transform.localPosition = to;

        card_picture.transform.localRotation = Quaternion.identity;
    }

    string MoneyConvertToUnit(long money)
    {
        long _tMoney = System.Math.Abs(money);

        string moneyStr = "";
        char[] moneyArray = new char[_tMoney.ToString().Length];
        moneyArray = _tMoney.ToString().ToCharArray();
        System.Array.Reverse(moneyArray);
        int unitCount = _tMoney.ToString().Length % 4 == 0 ? _tMoney.ToString().Length / 4 : _tMoney.ToString().Length / 4 + 1;
        long[] unitMoney = new long[unitCount];
        for (int i = 0; i < unitCount; i++)
        {
            string temp = "";
            for (int j = i * 4; j < i * 4 + 4; j++)
            {
                if (i == unitCount - 1 && j >= _tMoney.ToString().Length) break;
                temp += moneyArray[j];
            }

            char[] _temp = new char[temp.Length];
            _temp = temp.ToCharArray();
            System.Array.Reverse(_temp);

            if (new string(_temp) == "0000") unitMoney[i] = 0;
            else unitMoney[i] = long.Parse(new string(_temp));
        }

        string[] __temp = new string[unitMoney.Length];
        for (int i = 0; i < unitMoney.Length; i++)
        {
            __temp[i] = "";
            if (i == 0 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString());
            else if (i == 1 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "만");
            else if (i == 2 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "억");
            else if (i == 3 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "조");
            else if (i == 4 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "경");
            else if (i == 5 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "해");
            else if (i == 6 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "자");
            else if (i == 7 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "양");
            else if (i == 8 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "구");
            else if (i == 9 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "간");
            else if (i == 10 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "정");
            else if (i == 11 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "재");
            else if (i == 12 && unitMoney[i] > 0) __temp[i] = (unitMoney[i].ToString() + "극");
        }

        for (int i = __temp.Length - 1; i >= 0; i--)
        {
            moneyStr += __temp[i];
            bool last = true;
            if (i > 0)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (__temp[j] != "") last = false;
                }
            }
            if (!last) moneyStr += "";
        }

        if (moneyStr != "") moneyStr += (PlayGameUI.Instance.moneyType == 1 ? "냥" : "냥");
        else if (moneyStr == "") moneyStr = (PlayGameUI.Instance.moneyType == 1 ? "냥" : "냥");

        return moneyStr;
    }

    public IEnumerator CoPlayCountDownSound()
    {
        int iCount = 5;
        while (true)
        {
            //SoundManager.Instance.PlaySound(SoundManager._eSoundResource.COUNT, false);
            yield return new WaitForSeconds(1.0f);
            iCount--;
            if (iCount == 0)
                break;
        }
    }

    public Vector3 GetPlayCardPos(int iRound, int iSlotIndex)
    {
        return player_card_positions[iRound].get_hand_position(iSlotIndex);
    }

    bool bCountDown = false;
    public IEnumerator CoCountDown(int playerIndex)
    {
        float fTime = 0.0f;
        bCountDown = true;
        while (bCountDown && PlayGameUI.Instance.isGaming)
        {
            fTime += Time.deltaTime;
            if (fTime > 2.0f)
            {
                player_info_slots[playerIndex].CountDown.SetActive(true);
                player_info_slots[playerIndex].CountDown.GetComponent<tk2dSpriteAnimator>().PlayFromFrame(0);


                if (playerIndex == 0)
                {
                    m_coEnumerator[(int)_eEnumeratorList.EET_CountDownPlaySound] = CoPlayCountDownSound();
                    StartCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_CountDownPlaySound]);
                }
                bCountDown = false;
            }
            yield return null;
        }
    }

    public void SetDeckImage(_eDeckImageType eType)
    {
        if (eType == _eDeckImageType.EDIT_NORMAL)
        {
            m_rawNormalDeckImage[0].texture = m_sprNormalDeckCardImage[0].texture;
            m_rawNormalDeckImage[1].texture = m_sprNormalDeckCardImage[2].texture;
            m_DeckSprite.sprite = m_sprNormalDeckCardImage[1];
            back_image = m_sprCardBackImage[0];
        }
        else
        {
            m_rawNormalDeckImage[0].texture = m_sprEventDeckCardImage[0].texture;
            m_rawNormalDeckImage[1].texture = m_sprEventDeckCardImage[2].texture;
            m_DeckSprite.sprite = m_sprEventDeckCardImage[1];
            back_image = m_sprCardBackImage[1];
        }
    }

    public void SetEventMultipleVisible(int iIndex, bool bFlag)
    {
        if (bFlag)
            m_objEventMultiple.GetComponent<SpriteRenderer>().sprite = m_sprEventMultiple[iIndex];
        m_objEventMultiple.SetActive(bFlag);
    }

    public void StopCountDownSound()
    {
        if (m_coEnumerator[(int)_eEnumeratorList.EET_CountDownPlaySound] != null)
        {
            StopCoroutine(m_coEnumerator[(int)_eEnumeratorList.EET_CountDownPlaySound]);
        }
    }
}