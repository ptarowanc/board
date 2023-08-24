using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using ZNet;
using Vector3 = UnityEngine.Vector3;

public class CPlayGameUI : CSingletonMonobehaviour<CPlayGameUI>
{
    public GameObject Notice;

    public GameObject counterBG;
    public GameObject counterNum;
    public GameObject EventBG;
    public GameObject PrefabFont;

    // 게임중인지 판별변수
    public bool isGaming = false;
    // 이벤트중인지 판별변수
    public bool isEventing = false;

    // 원본 이미지들.
    Sprite back_image;
    // 폭탄이미지
    Sprite bome_image;
    readonly string backImageDefault = "card_matgo_0001";
    // back 이미지 이름
    string backImage = "card_matgo_0001";
    // 폭탄 이미지 이름
    string bombImage = "bomb";

    // 각 슬롯의 좌표 객체.
    [SerializeField]
    Transform floor_slot_root;
    public List<Vector3> floor_slot_position;

    [SerializeField]
    Transform deck_slot;

    [SerializeField]
    CPlayerCardPosition[] player_card_positions;

    [SerializeField]
    GameObject m_objMissionSucces;

    [SerializeField]
    GameObject m_objDraw;


    // 카드 객체.
    List<CCardPicture> total_card_pictures;
    List<CCardPicture> bomb_card_pictures;

    CCardCollision card_collision_manager;

    // 자리별 카드 스케일.
    Vector3 SCALE_TO_FLOOR = new Vector3(0.6f, 0.6f, 1.0f);
    Vector3 SCALE_TO_OTHER_HAND = new Vector3(0.19f, 0.19f, 1.0f);
    Vector3 SCALE_TO_MY_HAND = new Vector3(1.0f, 1.0f, 1.0f);

    Vector3 SCALE_TO_OTHER_FLOOR = new Vector3(0.5f, 0.5f, 1.0f);
    Vector3 SCALE_TO_MY_FLOOR = new Vector3(0.5f, 0.5f, 1.0f);


    // 게임 플레이에 사용되는 객체들.
    public byte player_me_index;
    byte myPos = 0;
    byte enemyPos = 1;

    // 채널 게임머니 구분
    public int moneyType;

    //바닥에 깔린 슬롯
    List<CVisualFloorSlot> floor_cards_slots;
    // 가운데 쌓여있는 카드 객체.
    Stack<CCardPicture> deck_cards;
    // 플레이어 손에 있는 카드 객체
    List<CPlayerHandCardManager> player_hand_card_manager;
    // 플레이어가 먹은 카드 객체.
    List<CPlayerCardManager> player_floor_card_manager;
    List<CPlayerInfoSlot> player_info_slots;

    CCardManager card_manager;

    Queue<CRecvedMsg> waiting_packets;

    [HideInInspector]
    public RemoteID remote = RemoteID.Remote_Server;
    // 효과 관련 객체들.
    GameObject ef_focus;

    public int[] playerSoundIndex = new int[2] { 0, 0 };

    [SerializeField] AutoPlay autoPlay;

    [SerializeField]
    GameObject cardEventPos;

    int m_iPPUKCount = 0;
    #region 게임 준비 및 초기화
    void Awake()
    {
        if (NetworkManager.Instance.Screen == false)
        {
            SCALE_TO_FLOOR = new Vector3(0.7f, 0.7f, 1.0f);
            SCALE_TO_OTHER_HAND = new Vector3(0.21f, 0.21f, 1.0f);
            SCALE_TO_MY_HAND = new Vector3(0.7f, 0.7f, 1.0f);
            SCALE_TO_OTHER_FLOOR = new Vector3(0.5f, 0.5f, 1.0f);
            SCALE_TO_MY_FLOOR = new Vector3(0.5f, 0.5f, 1.0f);
        }

        m_iPPUKCount = 0;
        for (int i = 0; i < playerSoundIndex.Length; ++i)
            playerSoundIndex[i] = 0;

        waiting_packets = new Queue<CRecvedMsg>();
        card_collision_manager = GameObject.Find("GameManager").GetComponent<CCardCollision>();
        card_collision_manager.callback_on_touch = this.on_card_touch;

        player_me_index = 0;
        deck_cards = new Stack<CCardPicture>();
        card_manager = new CCardManager();
        card_manager.make_all_cards();
        floor_cards_slots = new List<CVisualFloorSlot>();
        for (byte i = 0; i < 12; ++i)
        {
            floor_cards_slots.Add(new CVisualFloorSlot(i, byte.MaxValue));
        }

        player_hand_card_manager = new List<CPlayerHandCardManager>();
        player_hand_card_manager.Add(new CPlayerHandCardManager());
        player_hand_card_manager.Add(new CPlayerHandCardManager());

        player_floor_card_manager = new List<CPlayerCardManager>();
        player_floor_card_manager.Add(new CPlayerCardManager());
        player_floor_card_manager.Add(new CPlayerCardManager());

        player_info_slots = new List<CPlayerInfoSlot>();
        player_info_slots.Add(transform.Find("Player_Info_01").GetComponent<CPlayerInfoSlot>());
        player_info_slots.Add(transform.Find("Player_Info_02").GetComponent<CPlayerInfoSlot>());

        back_image = CSpriteManager.Instance.get_sprite(backImage);
        bome_image = CSpriteManager.Instance.get_sprite(bombImage);
        floor_slot_position = new List<Vector3>();
        make_slot_positions(this.floor_slot_root, this.floor_slot_position);


        // 카드 만들어 놓기.
        this.total_card_pictures = new List<CCardPicture>();
        this.bomb_card_pictures = new List<CCardPicture>();

        GameObject original = Resources.Load("hwatoo") as GameObject;

        for (int i = 0; i < this.card_manager.cards.Count; ++i)
        {
            GameObject obj = GameObject.Instantiate(original);
            obj.transform.localScale = SCALE_TO_FLOOR;
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            CCardPicture card_pic = obj.AddComponent<CCardPicture>();
            this.total_card_pictures.Add(card_pic);

            //obj.GetComponent<Image>().color = back_red;
        }
        //폭탄 카드도 미리 만들어 놓는다.
        //※※※ i==5 -> 10
        for (int i = 0; i < 12; ++i)
        {
            GameObject obj = GameObject.Instantiate(original);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            CCardPicture card_pic = obj.AddComponent<CCardPicture>();
            this.bomb_card_pictures.Add(card_pic);
        }
        this.ef_focus = transform.Find("focus").gameObject;
        this.ef_focus.SetActive(false);

        // 키보드 입력기능 실행
        StartCoroutine(GetKeyboradKey());
        // 카운터 기능 실행
        StartCoroutine(Counter());
    }
    void Start()
    {
        enter();
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.GameWaitingPackets.Clear();
    }

    void reset()
    {
        this.card_manager.make_all_cards();

        for (int i = 0; i < this.floor_cards_slots.Count; ++i)
        {
            this.floor_cards_slots[i].reset();
        }

        make_deck_cards();

        for (int i = 0; i < this.player_hand_card_manager.Count; ++i)
        {
            this.player_hand_card_manager[i].reset();
        }

        for (int i = 0; i < this.player_floor_card_manager.Count; ++i)
        {
            this.player_floor_card_manager[i].reset();
        }

        clear_ui();
        AniManager.Instance.StopAllMovie();
        SpriteAniManager.Instance.ResetAll();

        bonusFlip = false;
        tempSlot = null;

        ef_focus.SetActive(false);

        // 게임시작시 초기화 해줘야하는것들
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().ResetColor();
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().HideCard();
        hide_hint_mark();
        hide_mission_mark();
        m_objMissionSucces.SetActive(false);
    }

    //덱카드 만들어 두기
    void make_deck_cards()
    {
        CSpriteLayerOrderManager.Instance.reset();
        Vector3 pos = deck_slot.localPosition;

        for (int i = 0; i < this.card_manager.cards.Count; ++i)
        {
            total_card_pictures[i].gameObject.SetActive(true);
        }

        this.deck_cards.Clear();
        for (int i = 0; i < this.total_card_pictures.Count; ++i)
        {
            Animator ani = total_card_pictures[i].GetComponentInChildren<Animator>();
            ani.Play("card_idle");

            total_card_pictures[i].update_backcard(this.back_image);
            total_card_pictures[i].enable_collider(false);
            deck_cards.Push(total_card_pictures[i]);

            total_card_pictures[i].transform.localPosition = pos;
            pos.x -= 0.6f;
            pos.y += 0.6f;
            total_card_pictures[i].transform.localScale = SCALE_TO_FLOOR;
            total_card_pictures[i].transform.rotation = Quaternion.identity;
            total_card_pictures[i].sprite_renderer.sortingOrder = CSpriteLayerOrderManager.Instance.Order;
            total_card_pictures[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < bomb_card_pictures.Count; ++i)
        {
            bomb_card_pictures[i].transform.localPosition = pos;
            bomb_card_pictures[i].transform.localScale = SCALE_TO_FLOOR;
            bomb_card_pictures[i].transform.rotation = Quaternion.identity;

            bomb_card_pictures[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < card_manager.cards.Count; ++i)
        {
            total_card_pictures[i].gameObject.SetActive(false);
        }
    }

    //각 슬롯 위치 미리 만들어두기
    void make_slot_positions(Transform root, List<Vector3> targets)
    {
        Transform[] slots = root.GetComponentsInChildren<Transform>();
        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i] == root)
            {
                continue;
            }

            targets.Add(slots[i].localPosition);
        }
    }

    void clear_ui()
    {
        for (int i = 0; i < this.player_info_slots.Count; ++i)
        {
            this.player_info_slots[i].update_score(0);
            this.player_info_slots[i].update_go(0);
            this.player_info_slots[i].update_shake(0);
            this.player_info_slots[i].update_ppuk(0);
            this.player_info_slots[i].update_peecount(0, false, false);
        }
    }

    //게임 시작
    public void enter()
    {
        clear_ui();
        StartCoroutine(sequential_packet_handler());
    }
    #endregion

    #region ON RECEIVE
    public void OnReceive(CRecvedMsg msg)
    {
        //CRecvedMsg clone = new CRecvedMsg();
        //clone.Msg = msg;
        //clone.mRmiID = msg.mRmiID;

        //this.waiting_packets.Enqueue(clone);
        this.waiting_packets.Enqueue(msg);
    }

    /// <summary>
    /// 패킷을 순차적으로 처리하기 위한 루프.
    /// 카드 움직이는 연출 장면을 순서대로 처리하기 위해 구현한 매소드 이다.
    /// 코루틴에 의한 카드 이동 연출이 진행중일때도 서버로부터의 패킷은 수신될 수 있으므로
    /// 연출 도중에 다른 연출이 수행되는 경우가 생겨 버린다.
    /// 이런 경우를 방지하려면 두가지 방법이 있다.
    /// 첫번째. 각 연출 단계마다 다른 클라이언트들과 동기화를 수행한다.
    /// 두번째. 들어오는 패킷을 큐잉처리 하여 하나의 연출 장면이 끝난 뒤에 다음 패킷을 꺼내어 처리한다.
    /// 여기서는 두번째 방법으로 구현하였다.
    /// 첫번째 방법의 경우 동기화 패킷을 수시로 교환해야 하기 때문에 구현하기가 번거롭고
    /// 상대방의 네트워크 상태가 좋지 않을 경우 게임 진행이 매끄럽지 못하게 된다.
    /// </summary>
    /// <returns></returns>
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
                //1. 게임시작
                case SS.Common.GameRequestReady:
                    {
                        SC_ROOM_GAME_STARTED(msg);
                    }
                    break;
                case SS.Common.GameObserveInfo:
                    {
                        yield return StartCoroutine(SC_OBSERVE_INFO(msg));
                    }
                    break;
                //2. 선잡기에 대한 결과
                case SS.Common.GameOrderEnd:
                    {
                        yield return StartCoroutine(SC_PLAYER_ORDER_RESULT(msg));
                    }
                    break;
                //3. 카드 나눠주기
                case SS.Common.GameDistributedStart:
                    {
                        yield return StartCoroutine(SC_DISTRIBUTE_ALL_CARDS(msg));
                    }
                    break;
                //4. 턴 시작
                case SS.Common.GameTurnStart:
                    {
                        SC_START_TURN(msg);
                    }
                    break;
                //5. 내 차례에 낸 카드에 대한 응답
                case SS.Common.GameSelectCardResult:
                    {
                        yield return StartCoroutine(SC_SELECT_CARD_ACK(msg));
                    }
                    break;
                //6.바닥에 보너스카드가 있을때
                case SS.Common.GameFloorHasBonus:
                    {
                        yield return StartCoroutine(SC_FLOOR_HAS_BONUS(msg));
                    }
                    break;
                //7. 덱 카드 열기 묻기
                case SS.Common.GameFlipDeckResult:
                    {
                        yield return StartCoroutine(SC_FLIP_DECK_CARD_ACK(msg));
                    }
                    break;
                //8. 턴 결과 
                case SS.Common.GameTurnResult:
                    {
                        yield return StartCoroutine(SC_TURN_RESULT(msg));
                    }
                    break;
                //9. 고혹은 스톱 묻기
                case SS.Common.GameRequestGoStop:
                    {
                        StartCoroutine(SC_ASK_GO_OR_STOP(msg));
                    }
                    break;
                //10. 고 카운트 알림
                case SS.Common.GameNotifyGoStop:
                    {
                        yield return StartCoroutine(SC_NOTIFY_GO_COUNT(msg));
                    }
                    break;
                //11. 플레이어 상태 업데이트
                case SS.Common.GameNotifyStat:
                    {
                        SC_UPDATE_PLAYER_STATISTICS(msg);
                    }
                    break;
                //12. 국진을 피로 묻기
                case SS.Common.GameRequestKookjin:
                    {
                        SC_ASK_KOOKJIN_TO_PEE(msg);
                    }
                    break;
                //13. 국진을 피로 이동
                case SS.Common.GameMoveKookjin:
                    {
                        yield return StartCoroutine(SC_MOVE_KOOKJIN_TO_PEE(msg));
                    }
                    break;
                //14. 게임 결과
                case SS.Common.GameOver:
                    {
                        // 게임결과 시 자동치기 해제
                        autoPlay.StopWorking();
                        yield return StartCoroutine(SC_GAME_RESULT(msg));
                    }
                    break;
                case SS.Common.GamePracticeEnd: // 연습게임 종료
                    {
                        SC_PRACTICE_GAME_END(msg);
                    }
                    break;
                //15.유저가 밀었을때 민것에 대한 대답
                case SS.Common.GameRequestPush:
                    {
                        yield return StartCoroutine(SC_PUSH(msg));
                    }
                    break;
                //16. 준비가 완료되었으니 선잡기 시작해라
                case SS.Common.GameStart:
                    {
                        yield return StartCoroutine(SC_READY_TO_START(msg));
                    }
                    break;
                //17. 선잡기 선택에 대한 응답
                case SS.Common.GameRequestSelectOrder:
                    {
                        SC_PLAYER_ORDER_START(msg);
                    }
                    break;
                //18. 이벤트시작
                case SS.Common.GameEventStart:
                    {
                        yield return StartCoroutine(EventStart(msg));
                    }
                    break;
                //19. 잭팟 새로고침
                case SS.Common.GameEventInfo:
                    {
                        SC_EVENT_JACKPOT_INFO(msg, false);
                    }
                    break;
                //20. 탑쌓기 정보
                //case SS.Common.SC_EVENT_TAPSSAHGI_INFO:
                //    {

                //    }
                //    break;
                //21. 유저정보
                case SS.Common.GameUserInfo:
                    {
                        SC_USER_INFO(msg);
                    }
                    break;
                //22. 올인시
                case SS.Common.GameKickUser:
                    {
                        CUIManager.Instance.ExitGame();
                    }
                    break;
                //23. 나의 인덱스 받기
                case SS.Common.GameNotifyIndex:
                    {
                        Rmi.Marshaler.Read(msg, out player_me_index);
                    }
                    break;
                //24. 방정보
                case SS.Common.GameRoomInfo:
                    {
                        int channel;
                        Rmi.Marshaler.Read(msg, out channel);
                        int roomNumber;
                        Rmi.Marshaler.Read(msg, out roomNumber);
                        int money;
                        Rmi.Marshaler.Read(msg, out money);
                        Rmi.Marshaler.Read(msg, out moneyType);

                        CUIManager.Instance.SetRoomInfo(channel, roomNumber, money);

                    }
                    break;
                //25. 상대방이 나갔을때
                case SS.Common.GameUserOut:
                    {
                        UserRoomOut(msg);
                    }
                    break;
                //26. 뻑
                //case SS.Common.SC_RULEMONEY_INFO:
                //    {
                //        byte __plyaerIndex;
                //        Rmi.Marshaler.Read(msg, out __plyaerIndex);
                //        byte type;
                //        Rmi.Marshaler.Read(msg, out type);
                //        long money;
                //        Rmi.Marshaler.Read(msg, out money);

                //        yield return StartCoroutine(CUIManager.Instance.ppukShow(type, money));
                //    }
                //    break;
                // 방이동
                case SS.Common.GameResponseRoomMove:
                    {
                        bool isSuccess;
                        Rmi.Marshaler.Read(msg, out isSuccess);
                        string errMsg;
                        Rmi.Marshaler.Read(msg, out errMsg);

                        if (isSuccess == true)
                        {
                            SetVisible_Draw(false);
                            // 방이동 시 자동치기 해제
                            autoPlay.StopWorking();
                            // 초기화
                            isGaming = false;
                            meObserver = false;
                            EventEnd();
                            reset();
                            CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
                            CUIManager.Instance.ResetGoNStopAll();
                            CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().ResetUserInfo();
                            for (int i = 0; i < player_info_slots.Count; i++) player_info_slots[i].resetAll();
                            //CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ReportReset();

                            // 게임시작 버튼 리셋
                            CUIManager.Instance.hide(UI_PAGE.BUTTON_START);
                            CUIManager.Instance.hide(UI_PAGE.BUTTON_PRACTICE);
                            CUIManager.Instance.hide(UI_PAGE.NOTICE_PRACTICE);


                            // 방이동 및 나가기 버튼 리셋
                            CUIManager.Instance.ExitGameReserveCancel();
                            CUIManager.Instance.RoomMoveReserveCancel();
                            m_iPPUKCount = 0;

                            yield return new WaitForSeconds(1.0f);
                            CUIManager.Instance.SetRoomMoveTime(0.0f);
                        }
                        else
                        {
                            CUIManager.Instance.RoomMoveReserveCancel();
                        }
                    }
                    break;
                // 나가기
                case SS.Common.GameResponseRoomOut:
                    {
                        bool isSuccess;
                        Rmi.Marshaler.Read(msg, out isSuccess);
                        if (isSuccess == false)
                        {
                            CUIManager.Instance.ExitGameReserveCancel();
                            //NetworkManager.Instance.server_tag = CommonMatgo.Server.Room;
                        }
                        else
                        {
                            SetVisible_Draw(false);
                            //NetworkManager.Instance.server_tag = CommonMatgo.Server.Lobby;
                        }
                    }
                    break;
                case SS.Common.GameRoomIn:
                    {
                        bool isEnter;
                        Rmi.Marshaler.Read(msg, out isEnter);
                        if (isEnter == true)
                        {
                            SetVisible_Draw(false);
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
            }

            yield return 0;
        }
    }

    IEnumerator NoticeRefresh(string message, int sec)
    {
        Notice.SetActive(true);

        Notice.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = message;

        yield return new WaitForSeconds(sec);

        Notice.SetActive(false);
    }
    #endregion ON RECEIVE

    #region SC_ROOM_GAME_STARTED
    void SC_ROOM_GAME_STARTED(CMessage msg)
    {
        // 준비 요청 여부
        bool meReady;
        Rmi.Marshaler.Read(msg, out meReady);

        if (meReady == true)
        {
            CUIManager.Instance.ReadyToStartGame();
        }

        // 연습게임 가능 여부
        bool canGamePractice;
        Rmi.Marshaler.Read(msg, out canGamePractice);

        if (canGamePractice == true)
        {
            CUIManager.Instance.show(UI_PAGE.BUTTON_PRACTICE);
        }
        else
        {
            CUIManager.Instance.hide(UI_PAGE.BUTTON_PRACTICE);
        }

        // 게임시작 가능 여부
        bool canGameStart;
        Rmi.Marshaler.Read(msg, out canGameStart);

        if (canGameStart == true)
        {
            // 연습치기 중이 아니면 바로 시작
            if (CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().Avatar.activeSelf == true)
            {
                CUIManager.Instance.ReadyToStartGame();
            }
            else
            {
                CUIManager.Instance.show(UI_PAGE.BUTTON_START);
            }
        }
        else
        {
            CUIManager.Instance.hide(UI_PAGE.BUTTON_START);
        }
    }
    #endregion SC_ROOM_GAME_STARTED

    bool meObserver;
    IEnumerator SC_OBSERVE_INFO(CMessage msg)
    {
        meObserver = true;
        CUIManager.Instance.show(UI_PAGE.NOTICE_PRACTICE);

        keyState = UserInputState.NONE;
        // 리셋 후 세팅
        reset();
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
        for (int i = 0; i < player_info_slots.Count; i++) player_info_slots[i].resetAll();

        Queue<CCard> floor_cards = new Queue<CCard>();
        Dictionary<int, byte> player_hand = new Dictionary<int, byte>();
        Dictionary<int, List<CCard>> player_floor = new Dictionary<int, List<CCard>>();

        // 바닥패 채우기
        byte floor_count;
        Rmi.Marshaler.Read(msg, out floor_count);
        for (byte i = 0; i < floor_count; ++i)
        {
            byte number;
            Rmi.Marshaler.Read(msg, out number);
            byte paetype;
            Rmi.Marshaler.Read(msg, out paetype);
            PAE_TYPE pae_type = (PAE_TYPE)paetype;
            byte position;
            Rmi.Marshaler.Read(msg, out position);

            CCard card = this.card_manager.find_card(number, pae_type, position);
            floor_cards.Enqueue(card);
        }

        byte PlayerCount;
        Rmi.Marshaler.Read(msg, out PlayerCount);
        for (int i = 0; i < PlayerCount; ++i)
        {
            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index);

            byte handCount;
            Rmi.Marshaler.Read(msg, out handCount);

            // 손패 채우기
            player_hand.Add(player_index, handCount);

            // 플레이어 바닥패 채우기
            byte playerFloorCount;
            Rmi.Marshaler.Read(msg, out playerFloorCount);
            player_floor[player_index] = new List<CCard>();
            for (int j = 0; j < playerFloorCount; ++j)
            {
                byte number;
                Rmi.Marshaler.Read(msg, out number);
                byte pae_type;
                Rmi.Marshaler.Read(msg, out pae_type);
                byte position;
                Rmi.Marshaler.Read(msg, out position);

                CCard card = new CCard(number, (PAE_TYPE)pae_type, position);
                player_floor[player_index].Add(card);
            }

            short score;
            Rmi.Marshaler.Read(msg, out score);
            byte go_count;
            Rmi.Marshaler.Read(msg, out go_count);
            byte shaking_count;
            Rmi.Marshaler.Read(msg, out shaking_count);
            byte ppuk_count;
            Rmi.Marshaler.Read(msg, out ppuk_count);
            byte pee_count;
            Rmi.Marshaler.Read(msg, out pee_count);
            bool peebak;
            Rmi.Marshaler.Read(msg, out peebak);
            bool gwangbak;
            Rmi.Marshaler.Read(msg, out gwangbak);
            short multiple;
            Rmi.Marshaler.Read(msg, out multiple);

            //// 판돈*최종점수=승리금액 ==> startMoney*finalScore=승리금액
            //byte is_win;            // 0==무, 1==승, 2==패
            //int startMoney;         // 판돈
            //int finalScore;         // 최종점수
            ////int score;              // 패점수
            //int goBak;              // 고박 2배
            //int peeBak;             // 피박 2배
            //int gwangBak;           // 광박 2배
            //int meongTta;           // 멍따 2배
            //int shake;              // 흔들기 8배
            //int goMulti;            // 고횟수 32배
            //int missionMulti;       // 미션 10배
            //int drawMulti;          // 무승부 8배
            //int pushMulti;          // 밀었을때 8배
            //bool is_push;           // 밀기여부 가능 true 불가능 false
            //int chongtongNumber;    // 총통숫자(12 == 없음, 0~11 == 넘버)
            //bool is_threePpuck;     // 3뻑인지
            //long winnerMoney;       // 승리플레이어가 획득한 머니
            //long loserMoney;       // 패배플레이어가 잃은 머니
            //long dealerMoney;       // 딜러비
            //bool is_noMoney;        // 판돈 부족여부
            //int jackpotMoney;       // 잭팟 머니(판돈)
            //int jackpotReward;      // 잭팟 배율(200배)
            //short score1, score2, score3, score4, score5;           // 광 열 띠 피 고

            //bool canGameStart;

            //Rmi.Marshaler.Read(msg, out is_win);
            //Rmi.Marshaler.Read(msg, out startMoney);
            //Rmi.Marshaler.Read(msg, out finalScore);
            ////Rmi.Marshaler.Read(msg, out score);
            //Rmi.Marshaler.Read(msg, out goBak);
            //Rmi.Marshaler.Read(msg, out peeBak);
            //Rmi.Marshaler.Read(msg, out gwangBak);
            //Rmi.Marshaler.Read(msg, out meongTta);
            //Rmi.Marshaler.Read(msg, out shake);
            //Rmi.Marshaler.Read(msg, out goMulti);
            //Rmi.Marshaler.Read(msg, out missionMulti);
            //Rmi.Marshaler.Read(msg, out drawMulti);
            //Rmi.Marshaler.Read(msg, out pushMulti);
            //Rmi.Marshaler.Read(msg, out is_push);
            //Rmi.Marshaler.Read(msg, out chongtongNumber);
            //Rmi.Marshaler.Read(msg, out is_threePpuck);
            //Rmi.Marshaler.Read(msg, out winnerMoney);
            //Rmi.Marshaler.Read(msg, out loserMoney);
            //Rmi.Marshaler.Read(msg, out dealerMoney);
            //Rmi.Marshaler.Read(msg, out is_noMoney);
            //Rmi.Marshaler.Read(msg, out jackpotMoney);
            //Rmi.Marshaler.Read(msg, out jackpotReward);
            //Rmi.Marshaler.Read(msg, out score1);
            //Rmi.Marshaler.Read(msg, out score2);
            //Rmi.Marshaler.Read(msg, out score3);
            //Rmi.Marshaler.Read(msg, out score4);
            //Rmi.Marshaler.Read(msg, out score5);
            //Rmi.Marshaler.Read(msg, out canGameStart);

            //short[] goMultiple = { 0, 0, 2, 4, 8, 16, 32, 64, 128, 256, 512 };
            //short[] shakingMultiple = { 0, 2, 4, 8, 16 };
            //short peebakMultiple = 0;
            //short gwangbakMultiple = 0;

            //if (peebak)
            //    peebakMultiple = 2;
            //if (gwangbak)
            //    gwangbakMultiple = 2;
            //short multiple = (short)(goMultiple[go_count] + shakingMultiple[shaking_count] + peebakMultiple + gwangbakMultiple);

            if (multiple <= 0)
                multiple = 1;

            this.player_info_slots[player_index].update_score(score);
            this.player_info_slots[player_index].update_go(go_count);
            this.player_info_slots[player_index].update_shake(shaking_count);
            this.player_info_slots[player_index].update_ppuk(ppuk_count);
            this.player_info_slots[player_index].update_peecount(pee_count, gwangbak, peebak);
        }


        // 카드더미 보여주기
        for (int i = 0; i < card_manager.cards.Count; ++i)
        {
            total_card_pictures[i].gameObject.SetActive(true);
        }

        yield return StartCoroutine(observe_distribute_cards(floor_cards, player_hand, player_floor));

    }

    IEnumerator observe_distribute_cards(Queue<CCard> floor_cards, Dictionary<int, byte> player_hand, Dictionary<int, List<CCard>> player_floor)
    {
        //yield return new WaitForSeconds(1.0f);

        List<CCardPicture> begin_cards_picture = new List<CCardPicture>();
        int iDealCount = 0;
        int iDealIndex = (int)SoundManager._eSoundResource.ESR_DEAL1;
        // 플레이어 손패 분배
        for (int i = 0; i < 2; ++i)
        {
            byte player_index = (byte)i;

            byte ui_slot_index = 0;
            for (int card_index = 0; card_index < player_hand[player_index]; ++card_index)
            {
                if (is_me(player_index))
                {
                    CCardPicture card_picture = this.deck_cards.Pop();
                    card_picture.set_slot_index(ui_slot_index);
                    this.player_hand_card_manager[myPos].add(card_picture);
                    card_picture.update_backcard(this.back_image);
                    card_picture.transform.localScale = SCALE_TO_MY_HAND;
                    move_card(card_picture, card_picture.transform.localPosition,
                        this.player_card_positions[myPos].get_hand_position(ui_slot_index));
                }
                else
                {
                    CCardPicture card_picture = this.deck_cards.Pop();
                    card_picture.set_slot_index(ui_slot_index);
                    this.player_hand_card_manager[enemyPos].add(card_picture);
                    card_picture.update_backcard(this.back_image);
                    card_picture.transform.localScale = SCALE_TO_OTHER_HAND;
                    move_card(card_picture, card_picture.transform.localPosition,
                        this.player_card_positions[enemyPos].get_hand_position(ui_slot_index));
                }

                ++ui_slot_index;
                int iSoundIndex = iDealIndex + iDealCount;
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)iSoundIndex, false);
                iDealCount++;
                if (iDealCount > 3)
                    iDealCount = 0;
                yield return new WaitForSeconds(0.01f);
            }

            // 플레이어 바닥
            for (int f = 0; f < player_floor[player_index].Count; ++f)
            {
                CCardPicture card_picture = this.deck_cards.Pop();
                card_picture.update_card(player_floor[player_index][f], get_hwatoo_sprite(player_floor[player_index][f]));

                if (is_me(player_index))
                {
                    Vector3 begin = card_picture.transform.localPosition;
                    Vector3 to = get_playerfloor_card_position(myPos, card_picture.card.pae_type);
                    card_picture.transform.localScale = SCALE_TO_MY_FLOOR;
                    move_card(card_picture, begin, to);
                    this.player_floor_card_manager[myPos].add(card_picture);
                }
                else
                {
                    Vector3 begin = card_picture.transform.localPosition;
                    Vector3 to = get_playerfloor_card_position(enemyPos, card_picture.card.pae_type);
                    card_picture.transform.localScale = SCALE_TO_OTHER_FLOOR;
                    move_card(card_picture, begin, to);
                    this.player_floor_card_manager[enemyPos].add(card_picture);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        // 바닥에 분배
        int floorCount = floor_cards.Count;
        for (int i = 0; i < floorCount; ++i)
        {
            CCard card = floor_cards.Dequeue();
            CCardPicture card_picture = this.deck_cards.Pop();
            card_picture.update_card(card, get_hwatoo_sprite(card));
            begin_cards_picture.Add(card_picture);

            card_picture.transform.localScale = SCALE_TO_FLOOR;
            move_card(card_picture, card_picture.transform.localPosition, this.floor_slot_position[i]);

            int iSoundIndex = iDealIndex + iDealCount;
            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)iSoundIndex, false);
            iDealCount++;
            if (iDealCount > 3)
                iDealCount = 0;
            yield return new WaitForSeconds(0.01f);
        }
        sort_floor_cards_after_distributed(begin_cards_picture);
    }

    #region SC_READY_TO_START
    IEnumerator SC_READY_TO_START(CMessage msg)
    {
        m_iPPUKCount = 0;
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_GAMESTART, false);
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
        CUIManager.Instance.ResetGoNStopAll();
        // 팝업 초기화
        {
            CUIManager.Instance.hide(UI_PAGE.POPUP_CHOICE_CARD);
            CUIManager.Instance.hide(UI_PAGE.POPUP_GO_STOP1);
            CUIManager.Instance.hide(UI_PAGE.POPUP_GO_STOP2);
            CUIManager.Instance.hide(UI_PAGE.POPUP_GO_STOP3);
            CUIManager.Instance.hide(UI_PAGE.POPUP_GO_STOP4);
            CUIManager.Instance.hide(UI_PAGE.POPUP_GO_STOP5);
            CUIManager.Instance.hide(UI_PAGE.POPUP_GO_STOP6);
            CUIManager.Instance.hide(UI_PAGE.POPUP_GO_STOP7);
            CUIManager.Instance.hide(UI_PAGE.POPUP_ASK_SHAKING);
            CUIManager.Instance.hide(UI_PAGE.POPUP_SHAKING_CARDS);
            CUIManager.Instance.hide(UI_PAGE.POPUP_ASK_KOOKJIN);
            CUIManager.Instance.hide(UI_PAGE.POPUP_CHONGTONG);
            CUIManager.Instance.hide(UI_PAGE.BUTTON_PRACTICE);
            CUIManager.Instance.hide(UI_PAGE.BUTTON_START);
            CUIManager.Instance.hide(UI_PAGE.NOTICE_PRACTICE);
        }

        isGaming = true;
        meObserver = false;

        // 이미지 및 애니메이션 초기화
        reset();
        // 뻑,흔들기,광박,피박,고,점수,피갯수 초기화
        for (int i = 0; i < player_info_slots.Count; i++) player_info_slots[i].resetAll();

        // 화투장 보여주기
        CUIManager.Instance.show(UI_PAGE.POPUP_PLAYER_ORDER);
        CPopupPlayerOrder popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>();
        // 화투장 그림 초기화
        popup.reset(back_image);

        back_image = CSpriteManager.Instance.get_sprite(backImageDefault);

        keyState = UserInputState.SUN;

        yield return null;
    }
    #endregion

    #region  SC_PLAYER_ORDER_START
    void SC_PLAYER_ORDER_START(CMessage msg)
    {
        byte playerIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);
        byte choiceIndex;
        Rmi.Marshaler.Read(msg, out choiceIndex);

        // 카드 선택한것에 대한 응답과 연출
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().choiceUpdate(playerIndex, choiceIndex);
    }
    #endregion SC_PLAYER_ORDER_START

    #region SC_PLAYER_ORDER_RESULT
    IEnumerator SC_PLAYER_ORDER_RESULT(CMessage msg)
    {
        byte playerIndex;
        byte headIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);
        Rmi.Marshaler.Read(msg, out headIndex);

        byte paetype;
        byte number;
        byte position;
        byte choiceposition;
        PAE_TYPE pae_type;
        CCard card;

        byte paetype2;
        byte number2;
        byte position2;
        byte choiceposition2;
        PAE_TYPE pae_type2;
        CCard card2;

        Rmi.Marshaler.Read(msg, out number);
        Rmi.Marshaler.Read(msg, out paetype);
        pae_type = (PAE_TYPE)paetype;
        Rmi.Marshaler.Read(msg, out position);
        Rmi.Marshaler.Read(msg, out choiceposition);
        card = new CCard(number, pae_type, position);

        Rmi.Marshaler.Read(msg, out number2);
        Rmi.Marshaler.Read(msg, out paetype2);
        pae_type2 = (PAE_TYPE)paetype2;
        Rmi.Marshaler.Read(msg, out position2);
        Rmi.Marshaler.Read(msg, out choiceposition2);
        card2 = new CCard(number2, pae_type2, position2);

        Rmi.Marshaler.Read(msg, out backImage);
        back_image = CSpriteManager.Instance.get_sprite(backImage);

        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().update_slot_info(get_hwatoo_sprite(card), choiceposition);
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().update_slot_info(get_hwatoo_sprite(card2), choiceposition2);

        //int iSoundIndex = (int)SoundManager._eSoundResource.ESR_DEAL1;
        //SoundManager.Instance.PlaySound((SoundManager._eSoundResource)(iSoundIndex + ), false);

        yield return new WaitForSeconds(1.0f);

        float fY = 230.0f; ;
        if (playerIndex == headIndex)
        {
            fY = -230.0f;
            player_info_slots[0].first.SetActive(true);
            player_info_slots[1].first.SetActive(false);
        }
        else
        {
            fY = 230.0f;
            player_info_slots[0].first.SetActive(false);
            player_info_slots[1].first.SetActive(true);
        }

        Vector3 vPos = new Vector3(220f, fY, -0.1f);
        // 선 동영상 재생
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_FIRST, false);
        AniManager.Instance.PlayMovie(AniManager._eType.ET_SUN, 1, 30, vPos);
        yield return new WaitForSeconds(1.5f);
        CUIManager.Instance.hide(UI_PAGE.POPUP_PLAYER_ORDER);
    }
    #endregion SC_PLAYER_ORDER_RESULT

    #region SC_DISTRIBUTE_ALL_CARDS
    IEnumerator SC_DISTRIBUTE_ALL_CARDS(CMessage msg)
    {
        // 연습치기 일경우 알림창 표시
        if (CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().Avatar.activeSelf == false)
        {
            CUIManager.Instance.show(UI_PAGE.NOTICE_PRACTICE);
        }

        keyState = UserInputState.NONE;

        // 이미지 및 애니메이션 초기화
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
        CUIManager.Instance.ResetGoNStopAll();
        reset();

        // 카드더미 보여주기
        for (int i = 0; i < card_manager.cards.Count; ++i)
        {
            total_card_pictures[i].transform.Find("mark_first").gameObject.SetActive(false);
            total_card_pictures[i].transform.Find("mark_die").gameObject.SetActive(false);
            total_card_pictures[i].transform.Find("mark_bomb").gameObject.SetActive(false);
            total_card_pictures[i].transform.Find("mark_mission").gameObject.SetActive(false);
            total_card_pictures[i].transform.Find("mark_shake").gameObject.SetActive(false);
            total_card_pictures[i].gameObject.SetActive(true);
        }

        Queue<CCard> floor_cards = new Queue<CCard>();

        // 바닥패 세팅
        byte floor_count;
        Rmi.Marshaler.Read(msg, out floor_count);
        for (byte i = 0; i < floor_count; ++i)
        {
            byte number;
            Rmi.Marshaler.Read(msg, out number);
            byte paetype;
            Rmi.Marshaler.Read(msg, out paetype);
            PAE_TYPE pae_type = (PAE_TYPE)paetype;
            byte position;
            Rmi.Marshaler.Read(msg, out position);

            CCard card = this.card_manager.find_card(number, pae_type, position);
            if (card == null)
            {
                Debug.LogError(string.Format("1Cannot find the card. {0}, {1}, {2}", number, pae_type, position));
                //break;
            }
            floor_cards.Enqueue(card);
        }

        // 플레이어 손 패 세팅
        Dictionary<byte, Queue<CCard>> player_cards = new Dictionary<byte, Queue<CCard>>();
        byte player_count;
        Rmi.Marshaler.Read(msg, out player_count);
        for (byte player = 0; player < player_count; ++player)
        {
            Queue<CCard> cards = new Queue<CCard>();
            byte player_index;
            Rmi.Marshaler.Read(msg, out player_index);
            byte card_count;
            Rmi.Marshaler.Read(msg, out card_count);
            for (byte i = 0; i < card_count; ++i)
            {
                byte number;
                Rmi.Marshaler.Read(msg, out number);
                if (number != byte.MaxValue && meObserver == false)
                {
                    byte paetype;
                    Rmi.Marshaler.Read(msg, out paetype);
                    PAE_TYPE pae_type = (PAE_TYPE)paetype;
                    byte position;
                    Rmi.Marshaler.Read(msg, out position);
                    CCard card = this.card_manager.find_card(number, pae_type, position);
                    cards.Enqueue(card);
                }
                else
                {
                }
            }

            // 카드정보를 저장
            player_cards.Add(player_index, cards);
        }
        yield return StartCoroutine(distribute_cards(floor_cards, player_cards));
        //미션 시작
        byte mission;
        Rmi.Marshaler.Read(msg, out mission);
        MISSION currentMission = (MISSION)mission;
        byte missioncardsCount;
        Rmi.Marshaler.Read(msg, out missioncardsCount);
        List<CCard> missionCards = new List<CCard>();
        if (missioncardsCount > 0)
        {
            for (int i = 0; i < missioncardsCount; ++i)
            {
                byte number;
                Rmi.Marshaler.Read(msg, out number);
                byte paetype;
                Rmi.Marshaler.Read(msg, out paetype);
                PAE_TYPE pae_type = (PAE_TYPE)paetype;
                byte position;
                Rmi.Marshaler.Read(msg, out position);
                CCard card = this.card_manager.find_card(number, pae_type, position);
                missionCards.Add(card);
                //Debug.LogError(number + " :  " + pae_type + " " + currentMission.ToString());
            }
        }

        if (currentMission != MISSION.NONE) // 연습게임은 미션 연출 없음
            yield return StartCoroutine(MissionStartAni(currentMission, missionCards));

        if (!meObserver)
        {
            CMessage newmsg = new CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameDistributedEnd;
            newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
            NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        }
    }
    //카드 분배
    IEnumerator distribute_cards(Queue<CCard> floor_cards, Dictionary<byte, Queue<CCard>> player_cards)
    {
        yield return new WaitForSeconds(1.0f);

        List<CCardPicture> begin_cards_picture = new List<CCardPicture>();
        player_cards = player_cards.Reverse().ToDictionary(x => x.Key, x => x.Value);
        // [2P -> 1P -> 바닥 나눠주기] 순서
        int iDealCount = 0;
        int iDealIndex = (int)SoundManager._eSoundResource.ESR_DEAL1;
        for (int looping = 0; looping < 2; ++looping)
        {
            // 플레이어 카드를 분배
            for (int i = player_cards.Count - 1; i >= 0; i--)
            {
                byte player_index = (byte)i;
                Queue<CCard> cards = player_cards[(byte)i];

                byte ui_slot_index = (byte)(looping * 5);
                // 플레이어에게는 한번에 5장씩 분배한다.
                for (int card_index = 0; card_index < 5; ++card_index)
                {

                    // 본인 카드는 해당 이미지를 보여주고,
                    // 상대방 카드(is_nullcard)는 back_image로 처리한다.
                    if (is_me(player_index) && meObserver == false)
                    {
                        CCardPicture card_picture = this.deck_cards.Pop();
                        card_picture.set_slot_index(ui_slot_index);
                        this.player_hand_card_manager[myPos].add(card_picture);
                        CCard card = cards.Dequeue();
                        card_picture.update_card(card, get_hwatoo_sprite(card));
                        card_picture.transform.localScale = SCALE_TO_MY_HAND;
                        move_card(card_picture, card_picture.transform.position,
                            this.player_card_positions[myPos].get_hand_position(ui_slot_index));
                    }
                    else
                    {
                        CCardPicture card_picture = this.deck_cards.Pop();
                        card_picture.set_slot_index(ui_slot_index);
                        this.player_hand_card_manager[enemyPos].add(card_picture);
                        card_picture.update_backcard(this.back_image);
                        card_picture.transform.localScale = SCALE_TO_OTHER_HAND;
                        move_card(card_picture, card_picture.transform.position,
                            this.player_card_positions[enemyPos].get_hand_position(ui_slot_index));
                    }

                    ++ui_slot_index;
                    int iSoundIndex = iDealIndex + iDealCount;
                    SoundManager.Instance.PlaySound((SoundManager._eSoundResource)iSoundIndex, false);
                    iDealCount++;
                    if (iDealCount > 3)
                        iDealCount = 0;
                    yield return new WaitForSeconds(0.02f);
                }
            }

            // 바닥에는 4장씩 분배한다.
            for (int i = 0; i < 4; ++i)
            {
                CCard card = floor_cards.Dequeue();
                CCardPicture card_picture = this.deck_cards.Pop();
                card_picture.update_card(card, get_hwatoo_sprite(card));
                begin_cards_picture.Add(card_picture);

                card_picture.transform.localScale = SCALE_TO_FLOOR;
                move_card(card_picture, card_picture.transform.position, this.floor_slot_position[i + looping * 4]);

                int iSoundIndex = iDealIndex + iDealCount;
                SoundManager.Instance.PlaySound((SoundManager._eSoundResource)iSoundIndex, false);
                iDealCount++;
                if (iDealCount > 3)
                    iDealCount = 0;
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        sort_floor_cards_after_distributed(begin_cards_picture);
        sort_playerhand_slots(player_me_index);
    }

    // ★★★ 미션카드 리스트
    List<CCard> ListMissionCard = new List<CCard>();
    IEnumerator MissionStartAni(MISSION curmission, List<CCard> cards)
    {
        //yield return StartCoroutine(SpriteAniManager.Instance.CoPlaySpriteAni(SpriteAniManager.SpriteAni.TK2D_MISSIONTITLE, false));
        //yield return StartCoroutine(SpriteAniManager.Instance.CoPlaySpriteAni(SpriteAniManager.SpriteAni.TK2D_MISSIONSTART, false));

        // 초기화
        ListMissionCard.Clear();

        // 미션 보여주기
        // 이렇게까지 복잡해진 이유는 기존 개발자의 프레임워크를 크게 벗어나지 않게 하기위함이다.
        CUIManager.Instance.show(UI_PAGE.POPUP_MISSION);
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().RefreshMission(curmission, cards);

        switch (curmission)
        {
            case MISSION.FIVEKWANG:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(0, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(2, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(7, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(10, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(11, PAE_TYPE.KWANG, 0));
                break;
            case MISSION.KWANGTTANG:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(0, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(2, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(7, PAE_TYPE.KWANG, 0));
                break;
            case MISSION.GODORI:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(1, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(3, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(7, PAE_TYPE.YEOL, 1));
                break;
            case MISSION.HONGDAN:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(0, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(1, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(2, PAE_TYPE.TEE, 1));
                break;
            case MISSION.CHODAN:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(3, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(4, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(6, PAE_TYPE.TEE, 1));
                break;
            case MISSION.CHUNGDAN:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(5, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(8, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(9, PAE_TYPE.TEE, 1));
                break;
            case MISSION.WOL1_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(0, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(0, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(0, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(0, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL2_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(1, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(1, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(1, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(1, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL3_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(2, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(2, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(2, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(2, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL4_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(3, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(3, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(3, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(3, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL5_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(4, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(4, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(4, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(4, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL6_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(5, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(5, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(5, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(5, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL7_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(6, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(6, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(6, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(6, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL8_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(7, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(7, PAE_TYPE.YEOL, 1));
                ListMissionCard.Add(new CCard(7, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(7, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL9_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(8, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(8, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(8, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(8, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL10_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(9, PAE_TYPE.YEOL, 0));
                ListMissionCard.Add(new CCard(9, PAE_TYPE.TEE, 1));
                ListMissionCard.Add(new CCard(9, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(9, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL11_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(10, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(10, PAE_TYPE.PEE, 1));
                ListMissionCard.Add(new CCard(10, PAE_TYPE.PEE, 2));
                ListMissionCard.Add(new CCard(10, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL12_4:
                // 미션카드 추가
                ListMissionCard.Add(new CCard(11, PAE_TYPE.KWANG, 0));
                ListMissionCard.Add(new CCard(11, PAE_TYPE.YEOL, 1));
                ListMissionCard.Add(new CCard(11, PAE_TYPE.TEE, 2));
                ListMissionCard.Add(new CCard(11, PAE_TYPE.PEE, 3));
                break;
            case MISSION.WOL1_2:
            case MISSION.WOL2_2:
            case MISSION.WOL3_2:
            case MISSION.WOL4_2:
            case MISSION.WOL5_2:
            case MISSION.WOL6_2:
            case MISSION.WOL7_2:
            case MISSION.WOL8_2:
            case MISSION.WOL9_2:
            case MISSION.WOL10_2:
            case MISSION.WOL11_2:
            case MISSION.WOL12_2:
                // 미션카드 추가
                ListMissionCard.Add(cards[0]);
                ListMissionCard.Add(cards[1]);
                break;
            case MISSION.BAE2:
                break;
            case MISSION.BAE3:
                break;
            case MISSION.BAE4:
                break;
            case MISSION.WHALBIN:
                {
                    GameObject g = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().whalbinMe;
                    g.SetActive(true);
                    g.GetComponent<tk2dSpriteAnimator>().Play("Mission_Whalbin_Me_1");
                }
                {
                    GameObject g = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().whalbinEnemy;
                    g.SetActive(true);
                    g.GetComponent<tk2dSpriteAnimator>().Play("Mission_Whalbin_Enemy_1");
                }
                break;
        }

        yield return null;

        //Debug.LogError("미션:" + curmission.ToString());
        //foreach (var i in ListMissionCard) Debug.LogError("숫자:" + i.number + ",패타입:" + i.pae_type.ToString() + ",포지션:" + i.position);
    }
    #endregion SC_DISTRIBUTE_ALL_CARDS

    #region SC_START_TURN
    void SC_START_TURN(CMessage msg)
    {
        AniManager.Instance.StopAllMovie();

        byte currentTurn;
        Rmi.Marshaler.Read(msg, out currentTurn);
        byte remain_bomb_card_count;
        Rmi.Marshaler.Read(msg, out remain_bomb_card_count);

        if (currentTurn == player_me_index && meObserver == false)
        {

            refresh_hint_mark();

            // 내 차례가 되었을 때 카드 선택 기능을 활성화 시켜준다.
            this.ef_focus.SetActive(true);
            this.card_collision_manager.enabled = true;
            this.player_hand_card_manager[myPos].enable_all_colliders(true);

            // 내 차례가 되었을 때 키보드 입력 기능을 활성화 시켜준다.
            keyState = UserInputState.NORMAL;
        }
        else
        {
            StartCoroutine(AniManager.Instance.CoWaitChoice(player_me_index));
            //StartCoroutine(AniManager.Instance.CoWaitPaeChoice(player_me_index));

            // 내턴이 아니면 카드 선택 기능을 비활성화
            this.ef_focus.SetActive(false);
            this.card_collision_manager.enabled = false;
            this.player_hand_card_manager[myPos].enable_all_colliders(false);
        }
    }
    #endregion SC_START_TURN

    #region SC_SELECT_CARD_ACK
    IEnumerator SC_SELECT_CARD_ACK(CMessage msg)
    {
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_DEAL1, false);
        AniManager.Instance.StopAllMovie();
        yield return StartCoroutine(on_select_card_ack(msg));
        AniManager.Instance.StopAllMovie();
    }
    //플레이어 자기 패 카드선택해서 빼기 결과
    IEnumerator on_select_card_ack(CMessage msg)
    {
        // 데이터 파싱 시작 ----------------------------------------
        byte delay;
        Rmi.Marshaler.Read(msg, out delay);
        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);

        // 플레이어가 낸 카드 정보.
        byte player_card_number;
        Rmi.Marshaler.Read(msg, out player_card_number);
        byte paetype;
        Rmi.Marshaler.Read(msg, out paetype);
        PAE_TYPE player_card_pae_type = PAE_TYPE.PEE;
        player_card_pae_type = (PAE_TYPE)paetype;
        byte player_card_position;
        Rmi.Marshaler.Read(msg, out player_card_position);
        byte same_count_with_player;
        Rmi.Marshaler.Read(msg, out same_count_with_player);
        byte slot_index;
        Rmi.Marshaler.Read(msg, out slot_index);
        byte cardevent;
        Rmi.Marshaler.Read(msg, out cardevent);
        CARD_EVENT_TYPE card_event = CARD_EVENT_TYPE.NONE;
        card_event = (CARD_EVENT_TYPE)cardevent;
        //Debug.Log("-------------------- event " + card_event);
        List<CCard> bomb_cards_info = new List<CCard>();
        List<CCard> shaking_cards_info = new List<CCard>();
        int iBombCount = 0;
        switch (card_event)
        {
            case CARD_EVENT_TYPE.BOMB:
                {
                    byte bomb_card_count;
                    Rmi.Marshaler.Read(msg, out bomb_card_count);
                    iBombCount = bomb_card_count;
                    for (byte i = 0; i < bomb_card_count; ++i)
                    {
                        byte number;
                        Rmi.Marshaler.Read(msg, out number);

                        byte cardtype;
                        Rmi.Marshaler.Read(msg, out cardtype);
                        PAE_TYPE pae_type = (PAE_TYPE)cardtype;
                        byte position;
                        Rmi.Marshaler.Read(msg, out position);
                        CCard card = this.card_manager.find_card(number, pae_type, position);
                        bomb_cards_info.Add(card);

                        CVisualFloorSlot slot = this.floor_cards_slots.Find(obj => obj.is_same_card(card));
                        Vector3 pos = get_floorcards_slot_position(slot);
                    }
                }
                break;
            case CARD_EVENT_TYPE.SHAKING:
                {
                    byte shaking_card_count;
                    Rmi.Marshaler.Read(msg, out shaking_card_count);
                    for (byte i = 0; i < shaking_card_count; ++i)
                    {
                        byte number;
                        Rmi.Marshaler.Read(msg, out number);
                        byte paetype1;
                        Rmi.Marshaler.Read(msg, out paetype1);
                        PAE_TYPE pae_type = (PAE_TYPE)paetype1;
                        byte position;
                        Rmi.Marshaler.Read(msg, out position);
                        CCard card = this.card_manager.find_card(number, pae_type, position);
                        shaking_cards_info.Add(card);
                    }
                }
                break;
        }

        List<Sprite> target_to_choice = new List<Sprite>();
        byte result;
        Rmi.Marshaler.Read(msg, out result);
        PLAYER_SELECT_CARD_RESULT select_result = (PLAYER_SELECT_CARD_RESULT)result;
        if (select_result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)
        {
            byte count;
            Rmi.Marshaler.Read(msg, out count);
            for (byte i = 0; i < count; ++i)
            {
                byte number;
                Rmi.Marshaler.Read(msg, out number);
                byte paetype1;
                Rmi.Marshaler.Read(msg, out paetype1);
                PAE_TYPE pae_type = (PAE_TYPE)paetype1;
                byte position;
                Rmi.Marshaler.Read(msg, out position);
                CCard card = this.card_manager.find_card(number, pae_type, position);
                target_to_choice.Add(get_hwatoo_sprite(card));
            }
        }
        // 파싱 끝 ------------------------------------------------
        yield return StartCoroutine(delay_if_exist(delay));

        hide_hint_mark();

        // 화면 연출 진행.
        // 흔들었을 경우 흔든 카드의 정보를 출력해 준다.
        if (card_event == CARD_EVENT_TYPE.SHAKING)
        {
            CUIManager.Instance.show(UI_PAGE.POPUP_SHAKING_CARDS);
            CPopupShakingCards popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_SHAKING_CARDS).GetComponent<CPopupShakingCards>();
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < shaking_cards_info.Count; ++i)
            {
                sprites.Add(get_hwatoo_sprite(shaking_cards_info[i]));
            }
            popup.refresh(sprites);

            float fYPos = -230.0f;
            if (!is_me(player_index))
            {
                fYPos = 246.0f;
            }
            Vector3 vPos = new Vector3(-72.0f, fYPos, 0.0f);
            string tstr = "ES_SHAKING__" + playerSoundIndex[player_index];
            //AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

            tstr = "ESR_SHAKING" + Random.Range(1, 4).ToString() + "__" + playerSoundIndex[player_index];
            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);

            yield return new WaitForSeconds(1.5f);
            CUIManager.Instance.hide(UI_PAGE.POPUP_SHAKING_CARDS);
        }

        // 플레이어가 낸 카드 움직이기.
        yield return StartCoroutine(move_player_cards_to_floor(player_index, card_event, bomb_cards_info, slot_index, player_card_number, player_card_pae_type, player_card_position));

        yield return new WaitForSeconds(0.15f);

        if (card_event != CARD_EVENT_TYPE.NONE)
        {
            // 흔들기는 위에서 팝업으로 보여줬기 때문에 별도의 이펙트는 필요 없다.
            if (card_event != CARD_EVENT_TYPE.SHAKING)
            {
                CCard card = this.card_manager.find_card(player_card_number, player_card_pae_type, player_card_position);
                CVisualFloorSlot slot = floor_cards_slots.Find(obj => obj.is_same_card(card));
                Vector3 pos;
                if (NetworkManager.Instance.Screen)
                    pos = get_floorcards_slot_position(slot);
                else
                    pos = cardEventPos.transform.position;

                // 게임 규칙상 첫 카드를 냈을때 가능한 행동은 뻑을 먹거나 폭탄을 내는것외엔 나올 수 없다.
                switch (card_event)
                {
                    case CARD_EVENT_TYPE.SELF_EAT_PPUK:
                    case CARD_EVENT_TYPE.EAT_PPUK:
                        string tstr = "ET_GAJUWA__" + playerSoundIndex[player_index];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        tstr = "ESR_GAJUWA" + Random.Range(1, 3).ToString() + "__" + playerSoundIndex[player_index];
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
                        break;
                    case CARD_EVENT_TYPE.BOMB:
                        AniManager.Instance.PlayMovie(AniManager._eType.ET_BOMB, 1, 30, pos);

                        //if (iBombCount == 2)
                        //    tstr = "ESR_BOMB" + "1" + "__" + playerSoundIndex[player_index];
                        //else
                        //    tstr = "ESR_BOMB" + "2" + "__" + playerSoundIndex[player_index];
                        tstr = "ESR_BOMB" + Random.Range(1, 3).ToString() + "__" + playerSoundIndex[player_index];
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
                        break;
                }
                yield return new WaitForSeconds(1.5f);
            }
        }

        // 보너스카드일때
        if (select_result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
        {
            List<CCard> bonusCards = new List<CCard>();
            CCard card = this.card_manager.find_card(player_card_number, player_card_pae_type, player_card_position);
            bonusCards.Add(card);
            byte number;
            Rmi.Marshaler.Read(msg, out number);
            byte paetype1;
            Rmi.Marshaler.Read(msg, out paetype1);
            PAE_TYPE pae_type = (PAE_TYPE)paetype1;
            byte position;
            Rmi.Marshaler.Read(msg, out position);
            CCard deckcard = this.card_manager.find_card(number, pae_type, position);
            List<CCard> deckopenCards = new List<CCard>();
            deckopenCards.Add(deckcard);
            yield return StartCoroutine(move_bonus_card(player_index, bonusCards, deckopenCards, slot_index, false));
            yield break;
        }
        if (is_me(player_index) && meObserver == false)
        {
            // 바닥에 깔린 카드가 두장일 때 둘중 하나를 선택하는 팝업을 출력한다.
            if (select_result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)
            {
                CUIManager.Instance.show(UI_PAGE.POPUP_CHOICE_CARD);
                CPopupChoiceCard popup =
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_CHOICE_CARD).GetComponent<CPopupChoiceCard>();
                popup.refresh(target_to_choice[0], target_to_choice[1]); // on_select_card_ack
            }
        }
    }

    // 플레이어가 선택한 카드를 바닥에 내는 장면 구현.
    // 폭탄 이벤트가 존재할 경우 같은 번호의 카드 세장을 한꺼번에 내도록 구현한다.
    IEnumerator move_player_cards_to_floor(byte player_index, CARD_EVENT_TYPE event_type, List<CCard> bomb_cards_info, byte slot_index, byte player_card_number, PAE_TYPE player_card_pae_type, byte player_card_position)
    {
        float card_moving_delay = 0.1f;

        List<CCardPicture> targets = new List<CCardPicture>();
        if (event_type == CARD_EVENT_TYPE.BOMB)
        {
            card_moving_delay = 0.15f;

            // 폭탄인 경우에는 폭탄 카드 수 만큼 낸다.
            if (is_me(player_index))
            {
                if (meObserver == false)
                {
                    for (int i = 0; i < bomb_cards_info.Count; ++i)
                    {
                        CCardPicture card_picture = this.player_hand_card_manager[myPos].find_card(
                            bomb_cards_info[i].number, bomb_cards_info[i].pae_type, bomb_cards_info[i].position);
                        targets.Add(card_picture);
                    }
                }
                else // 관전
                {
                    for (int i = 0; i < bomb_cards_info.Count; ++i)
                    {
                        CCardPicture card_picture = this.player_hand_card_manager[myPos].get_card(i);
                        CCard card = this.card_manager.find_card(bomb_cards_info[i].number,
                            bomb_cards_info[i].pae_type, bomb_cards_info[i].position);
                        card_picture.update_card(card, get_hwatoo_sprite(card));
                        targets.Add(card_picture);
                    }
                }
            }
            else
            {
                for (int i = 0; i < bomb_cards_info.Count; ++i)
                {
                    CCardPicture card_picture = this.player_hand_card_manager[enemyPos].get_card(i);
                    CCard card = this.card_manager.find_card(bomb_cards_info[i].number,
                        bomb_cards_info[i].pae_type, bomb_cards_info[i].position);
                    card_picture.update_card(card, get_hwatoo_sprite(card));
                    targets.Add(card_picture);
                }
            }
        }
        else
        {
            // 폭탄이 아닌 경우에는 한장의 카드만 낸다.
            if (is_me(player_index))
            {
                if (meObserver == false)
                {
                    CCardPicture card_picture = this.player_hand_card_manager[myPos].get_card(slot_index);
                    targets.Add(card_picture);
                }
                else
                {
                    CCardPicture card_picture = this.player_hand_card_manager[myPos].get_card(0);
                    targets.Add(card_picture);
                    CCard card = this.card_manager.find_card(player_card_number,
                                            player_card_pae_type, player_card_position);
                    card_picture.update_card(card, get_hwatoo_sprite(card));
                }
            }
            else
            {
                CCardPicture card_picture = this.player_hand_card_manager[enemyPos].get_card(0);
                targets.Add(card_picture);
                CCard card = this.card_manager.find_card(player_card_number,
                                        player_card_pae_type, player_card_position);
                card_picture.update_card(card, get_hwatoo_sprite(card));
            }

        }

        if (event_type == CARD_EVENT_TYPE.BOMB)
        {
            CCard card = this.card_manager.find_card(player_card_number, player_card_pae_type, player_card_position);
            CVisualFloorSlot slot =
                this.floor_cards_slots.Find(obj => obj.is_same_card(card));
            Vector3 to = get_floorcards_slot_position(slot);
            CEffectManager.Instance.play_dust(to, 0.3f, true);
        }

        // 카드 움직이기.
        if (is_me(player_index))
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                // 손에 들고 있는 패에서 제거한다.
                CCardPicture player_card = targets[i];
                this.player_hand_card_manager[myPos].remove(player_card);

                // 스케일 장면.
                yield return StartCoroutine(scale_to(
                    player_card,
                    2.5f,
                    0.05f));

                yield return new WaitForSeconds(card_moving_delay);

                // 이동 장면.
                player_card.transform.localScale = SCALE_TO_FLOOR;
                move_playercard_to_floor(player_card, event_type);
            }
            if (event_type == CARD_EVENT_TYPE.BOMB)
            {
                for (int i = 0; i < targets.Count - 1; ++i)
                {
                    int bombCardIndex = 0;
                    for (int j = 0; j < bomb_card_pictures.Count; j++)
                    {
                        if (!bomb_card_pictures[j].gameObject.activeSelf)
                        {
                            bombCardIndex = j;
                        }
                    }

                    bomb_card_pictures[bombCardIndex].gameObject.SetActive(true);
                    bomb_card_pictures[bombCardIndex].gameObject.transform.localScale = new Vector3(1, 1, 1);
                    CCardPicture bomb_card = bomb_card_pictures[bombCardIndex];
                    player_hand_card_manager[myPos].add(bomb_card);
                    bomb_card.update_image(bome_image);
                    bomb_card.card = new CCard(13, PAE_TYPE.BOMB, 0);
                    bomb_card.isBomb = true;
                    bomb_card.transform.localScale = SCALE_TO_MY_HAND;

                }
            }
        }
        else
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                // 손에 들고 있는 패에서 제거한다.
                CCardPicture player_card = targets[i];
                this.player_hand_card_manager[enemyPos].remove(player_card);

                // 스케일 장면.
                yield return StartCoroutine(scale_to(
                    player_card,
                    2.5f,
                    0.05f));

                yield return new WaitForSeconds(card_moving_delay);

                // 이동 장면.
                player_card.transform.localScale = SCALE_TO_FLOOR;
                move_playercard_to_floor(player_card, event_type);
            }
            if (event_type == CARD_EVENT_TYPE.BOMB)
            {
                for (int i = 0; i < targets.Count - 1; ++i)
                {
                    int bombCardIndex = 0;
                    for (int j = 0; j < bomb_card_pictures.Count; j++)
                    {
                        if (!bomb_card_pictures[j].gameObject.activeSelf)
                        {
                            bombCardIndex = j;
                        }
                    }

                    bomb_card_pictures[bombCardIndex].gameObject.SetActive(true);
                    bomb_card_pictures[bombCardIndex].gameObject.transform.localScale = new Vector3(1, 1, 1);
                    CCardPicture bomb_card = bomb_card_pictures[bombCardIndex];
                    player_hand_card_manager[enemyPos].add(bomb_card);
                    bomb_card.update_image(bome_image);
                    bomb_card.isBomb = true;
                    bomb_card.transform.localScale = SCALE_TO_OTHER_HAND;
                }
            }
        }

        sort_playerhand_slots(player_index);
    }
    #endregion SC_SELECT_CARD_ACK

    #region BONUS CARD EVENT
    //보너스 카드 이동
    IEnumerator move_bonus_card(byte player_index, List<CCard> bonus_card, List<CCard> deck_flip_card, byte slotindex = 0, bool first = true)
    {
        // 카드 가져오기.
        for (int i = 0; i < bonus_card.Count; ++i)
        {

            //보너스 카드는 내 바닥으로 이동
            CVisualFloorSlot floorposition = this.floor_cards_slots.Find(obj => obj.is_same_card(bonus_card[i]));
            if (floorposition == null)
            {
                UnityEngine.Debug.LogError(string.Format("1 Cannot find floor slot. {0}, {1}, {2}",
                    bonus_card[i].number, bonus_card[i].pae_type, bonus_card[i].position));
                //break;
            }
            CCardPicture card_picture = floorposition.find_card(bonus_card[i]);
            if (card_picture == null)
            {
                UnityEngine.Debug.LogError(string.Format("2 Cannot find the card. {0}, {1}, {2}",
                    bonus_card[i].number, bonus_card[i].pae_type, bonus_card[i].position));
                //break;
            }

            Vector3 begin = card_picture.transform.localPosition;
            floorposition.remove_card(card_picture);
            Vector3 to;
            //바닥에 있는 보너스카드를 따 먹은 플레이어바닥으로 이동
            if (is_me(player_index)) to = get_playerfloor_card_position(myPos, card_picture.card.pae_type);
            else to = get_playerfloor_card_position(enemyPos, card_picture.card.pae_type);

            card_picture.transform.localScale = SCALE_TO_OTHER_FLOOR;
            move_card(card_picture, begin, to);

            if (is_me(player_index))
            {
                card_picture.transform.localScale = SCALE_TO_MY_FLOOR;
                player_floor_card_manager[myPos].add(card_picture);
            }
            else
            {
                card_picture.transform.localScale = SCALE_TO_OTHER_FLOOR;
                player_floor_card_manager[enemyPos].add(card_picture);
            }

            yield return new WaitForSeconds(0.4f);

            if (first)
            {
                //새로 깐 카드를 바닥에 등록
                yield return StartCoroutine(bonus_move_toFloor_flip_card(deck_flip_card[i].number, deck_flip_card[i].pae_type, deck_flip_card[i].position));
            }
            else
            {
                // 더미에서 새로 뽑은 카드는 내 핸드패에 이동
                CCardPicture cardpicture = this.deck_cards.Pop();
                CCard card = deck_flip_card[0];
                //Debug.Log(card.number + " : " + card.pae_type + " : " + card.position);
                if (is_me(player_index))
                {
                    if (meObserver == false)
                    {
                        // 본인 카드는 해당 이미지를 보여주고,
                        cardpicture.set_slot_index(slotindex);
                        cardpicture.update_card(card, get_hwatoo_sprite(card));
                        this.player_hand_card_manager[myPos].add(cardpicture);
                        cardpicture.transform.localScale = SCALE_TO_MY_HAND;
                        move_card(cardpicture, cardpicture.transform.position,
                            this.player_card_positions[myPos].get_hand_position(slotindex));
                    }
                    else // 관전시
                    {
                        cardpicture.card = null;
                        cardpicture.set_slot_index(slotindex);
                        cardpicture.update_backcard(this.back_image);
                        this.player_hand_card_manager[myPos].add(cardpicture);
                        cardpicture.transform.localScale = SCALE_TO_MY_HAND;
                        move_card(cardpicture, cardpicture.transform.position,
                            this.player_card_positions[myPos].get_hand_position(slotindex));
                    }
                }
                else
                {
                    // 상대방 카드(is_nullcard)는 back_image로 처리한다.
                    cardpicture.card = null;
                    cardpicture.set_slot_index(slotindex);
                    cardpicture.update_backcard(this.back_image);
                    this.player_hand_card_manager[enemyPos].add(cardpicture);
                    cardpicture.transform.localScale = SCALE_TO_OTHER_HAND;
                    move_card(cardpicture, cardpicture.transform.position,
                        this.player_card_positions[enemyPos].get_hand_position(slotindex));
                }
                sort_playerhand_slots(player_index);
                //refresh_playerhand_slots(player_index);
                SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_DEAL1, false);
                yield return new WaitForSeconds(0.02f);
            }
        }
        sort_floor_cards_when_finished_turn();
    }

    //더미에서 뒤집은 보너스 카드를 바닥으로이동
    IEnumerator bonus_move_toFloor_flip_card(byte number, PAE_TYPE pae_type, byte position)
    {
        // 뒤집은 카드 움직이기.
        CCardPicture deck_card_picture = this.deck_cards.Pop();
        CCard flipped_card = this.card_manager.find_card(number, pae_type, position);
        deck_card_picture.update_card(flipped_card, get_hwatoo_sprite(flipped_card));
        yield return StartCoroutine(flip_deck_card(deck_card_picture));

        yield return new WaitForSeconds(0.2f);

        deck_card_picture.transform.localScale = SCALE_TO_FLOOR;
        move_playercard_to_floor(deck_card_picture, CARD_EVENT_TYPE.NONE);

        yield return new WaitForSeconds(0.2f);
    }
    #endregion BONUS CARD EVENT

    #region SC_FLOOR_HAS_BONUS
    IEnumerator SC_FLOOR_HAS_BONUS(CMessage msg)
    {
        byte bonuseatplayer;
        Rmi.Marshaler.Read(msg, out bonuseatplayer);
        int bonusCardCount;
        Rmi.Marshaler.Read(msg, out bonusCardCount);
        List<CCard> bonusCards = new List<CCard>();
        for (int i = 0; i < bonusCardCount; i++)
        {
            byte number;
            Rmi.Marshaler.Read(msg, out number);
            byte paetype;
            Rmi.Marshaler.Read(msg, out paetype);
            PAE_TYPE pae_type = (PAE_TYPE)paetype;
            byte position;
            Rmi.Marshaler.Read(msg, out position);
            CCard card = this.card_manager.find_card(number, pae_type, position);
            if (card == null)
            {
                Debug.LogError(string.Format("3 Cannot find the card. {0}, {1}, {2}", number, pae_type, position));
                //break;
            }
            bonusCards.Add(card);
        }
        int floorcardcount;
        Rmi.Marshaler.Read(msg, out floorcardcount);
        List<CCard> deckopenCards = new List<CCard>();
        for (int i = 0; i < floorcardcount; i++)
        {
            byte number;
            Rmi.Marshaler.Read(msg, out number);
            byte paetype;
            Rmi.Marshaler.Read(msg, out paetype);
            PAE_TYPE pae_type = (PAE_TYPE)paetype;
            byte position;
            Rmi.Marshaler.Read(msg, out position);
            CCard card = this.card_manager.find_card(number, pae_type, position);
            if (card == null)
            {
                Debug.LogError(string.Format("4 Cannot find the card. {0}, {1}, {2}", number, pae_type, position));
                //break;
            }
            deckopenCards.Add(card);
        }
        yield return StartCoroutine(move_bonus_card(bonuseatplayer, bonusCards, deckopenCards));
    }
    #endregion SC_FLOOR_HAS_BONUS

    #region SC_FLIP_DECK_CARD_ACK
    IEnumerator SC_FLIP_DECK_CARD_ACK(CMessage msg)
    {
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_DEAL1, false);
        yield return StartCoroutine(on_flip_deck_card_ack(msg));
        AniManager.Instance.StopAllMovie();
    }
    bool bonusFlip = false;
    CVisualFloorSlot tempSlot;
    //더미에서 카드 뒤집기 하기
    IEnumerator on_flip_deck_card_ack(CMessage msg)
    {
        hide_hint_mark();

        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);
        // 덱에서 뒤집은 카드 정보.
        byte deck_card_number;
        Rmi.Marshaler.Read(msg, out deck_card_number);
        byte paetype;
        Rmi.Marshaler.Read(msg, out paetype);
        PAE_TYPE deck_card_pae_type = (PAE_TYPE)paetype;
        byte deck_card_position;
        Rmi.Marshaler.Read(msg, out deck_card_position);
        byte same_count_with_deck;
        Rmi.Marshaler.Read(msg, out same_count_with_deck);

        //Debug.Log("더미에서 카드 뒤집기 : " + player_index + "   내 인덱스 : " + player_me_index);

        List<Sprite> target_to_choice = new List<Sprite>();
        byte result1;
        Rmi.Marshaler.Read(msg, out result1);
        bool choose;
        Rmi.Marshaler.Read(msg, out choose);
        byte flip_type_;
        Rmi.Marshaler.Read(msg, out flip_type_);
        FLIP_TYPE flip_type = (FLIP_TYPE)flip_type_;

        if (flip_type == FLIP_TYPE.FLIP_BOOM)
        {
            // 상대가 폭탄카드를 써서 뒤집은거라면 폭탄카드 삭제
            if (!is_me(player_index))
            {
                CCardPicture card = player_hand_card_manager[enemyPos].get_card(player_hand_card_manager[enemyPos].get_card_count() - 1);
                if (card.isBomb != true)
                {
                    card = player_hand_card_manager[enemyPos].find_bomb_card();
                }
                card.gameObject.SetActive(false);
                player_hand_card_manager[enemyPos].remove(card);
            }
        }

        PLAYER_SELECT_CARD_RESULT result = (PLAYER_SELECT_CARD_RESULT)result1;
        if (result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
        {
            // for AI
            List<CCard> floor_cards_to_player = parse_cards_to_get(msg);
            List<CCardPicture> other_cards_to_player = parse_cards_to_take_from_others(get_other_player_index(player_index), msg, false);

            byte count;
            Rmi.Marshaler.Read(msg, out count);
            for (byte i = 0; i < count; ++i)
            {
                byte number;
                Rmi.Marshaler.Read(msg, out number);
                byte paetype1;
                Rmi.Marshaler.Read(msg, out paetype1);
                PAE_TYPE pae_type = (PAE_TYPE)paetype1;
                byte position;
                Rmi.Marshaler.Read(msg, out position);
                CCard card = this.card_manager.find_card(number, pae_type, position);
                target_to_choice.Add(get_hwatoo_sprite(card));
            }

            // ★★★ 뒤집은카드를 해당카드가 있는곳으로 보냄
            yield return StartCoroutine(move_toFloor_flip_card(deck_card_number, deck_card_pae_type, deck_card_position));

            // ★★★ 카드를 둘중하나 선택해서 먹어야함
            if (is_me(player_index) && meObserver == false)
            {
                CUIManager.Instance.show(UI_PAGE.POPUP_CHOICE_CARD);
                CPopupChoiceCard popup =
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_CHOICE_CARD).GetComponent<CPopupChoiceCard>();
                popup.refresh(target_to_choice[0], target_to_choice[1]); // on_flip_deck_card_ack
            }
        }
        else
        {
            if (result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
            {
                bonusFlip = true;
            }

            List<CCard> floor_cards_to_player = parse_cards_to_get(msg);
            List<CCardPicture> other_cards_to_player = parse_cards_to_take_from_others(player_index, msg);
            List<PLAYER_FLOOR_CHECK> playerFloorCheck = parse_playerfloor_events(msg);
            List<CARD_EVENT_TYPE> events = parse_flip_card_events(msg);

            refresh_playerfloor_slots_all(myPos);
            refresh_playerfloor_slots_all(enemyPos);

            // 화면 연출 진행.
            yield return StartCoroutine(move_toFloor_flip_card(deck_card_number, deck_card_pae_type, deck_card_position));

            if (events.Count > 0)
            {
                string tstr = "";
                CCard card = card_manager.find_card(deck_card_number, deck_card_pae_type, deck_card_position);
                CVisualFloorSlot slot = floor_cards_slots.Find(obj => obj.is_same_card(card));
                Vector3 pos;
                if (NetworkManager.Instance.Screen)
                    pos = get_floorcards_slot_position(slot);
                else
                    pos = cardEventPos.transform.position;
                for (int i = 0; i < events.Count; ++i)
                {
                    switch (events[i])
                    {
                        case CARD_EVENT_TYPE.KISS:
                            tstr = "ET_KISS__" + playerSoundIndex[player_index];
                            AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                            tstr = "ESR_KISS" + "__" + playerSoundIndex[player_index];
                            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
                            break;

                        case CARD_EVENT_TYPE.SELF_EAT_PPUK:
                        case CARD_EVENT_TYPE.EAT_PPUK:
                            tstr = "ET_GAJUWA__" + playerSoundIndex[player_index];
                            AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                            tstr = "ESR_GAJUWA" + Random.Range(1, 3).ToString() + "__" + playerSoundIndex[player_index];
                            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
                            break;

                        case CARD_EVENT_TYPE.DDADAK:
                            tstr = "ET_DDADAK__" + playerSoundIndex[player_index];
                            AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                            tstr = "ESR_DDADAK" + Random.Range(1, 4).ToString() + "__" + playerSoundIndex[player_index];
                            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
                            break;

                        case CARD_EVENT_TYPE.CLEAN:
                            AniManager.Instance.PlayMovie(AniManager._eType.ET_CLEAR, 1, 30);

                            tstr = "ESR_CLEAR" + Random.Range(1, 4).ToString() + "__" + playerSoundIndex[player_index];
                            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
                            break;

                        case CARD_EVENT_TYPE.PPUK:
                            tstr = "ET_BBUCK__" + playerSoundIndex[player_index];
                            AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                            //if (m_iPPUKCount > 0 && m_iPPUKCount < 4)
                            //    tstr = "ESR_PPUCK" + m_iPPUKCount.ToString() + "__" + playerSoundIndex[player_index];
                            //else
                            //{
                            //    m_iPPUKCount = 1;
                            //    tstr = "ESR_PPUCK" + m_iPPUKCount.ToString() + "__" + playerSoundIndex[player_index];
                            //}
                            //m_iPPUKCount++;
                            tstr = "ESR_PPUCK" + Random.Range(1, 4).ToString() + "__" + playerSoundIndex[player_index];
                            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
                            break;
                    }
                    yield return new WaitForSeconds(1.5f);
                }
            }

            // 나
            if (is_me(player_index))
            {
                if (result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
                {
                    yield break;
                }

                yield return StartCoroutine(Move_after_flip_card(myPos, other_cards_to_player, floor_cards_to_player, playerFloorCheck, msg, player_index, 1));
            }
            // 상대방
            else
            {
                if (result == PLAYER_SELECT_CARD_RESULT.BONUS_CARD)
                {
                    yield break;
                }

                yield return StartCoroutine(Move_after_flip_card(enemyPos, other_cards_to_player, floor_cards_to_player, playerFloorCheck, msg, player_index, 2));
            }
        }
    }
    //뒤집기 카드 이벤트 분석
    List<CARD_EVENT_TYPE> parse_flip_card_events(CMessage msg)
    {
        List<CARD_EVENT_TYPE> events = new List<CARD_EVENT_TYPE>();
        byte count;
        Rmi.Marshaler.Read(msg, out count);
        for (byte i = 0; i < count; ++i)
        {
            byte eventtype;
            Rmi.Marshaler.Read(msg, out eventtype);
            CARD_EVENT_TYPE type = (CARD_EVENT_TYPE)eventtype;
            events.Add(type);
        }

        return events;
    }

    List<PLAYER_FLOOR_CHECK> parse_playerfloor_events(CMessage msg)
    {
        List<PLAYER_FLOOR_CHECK> events = new List<PLAYER_FLOOR_CHECK>();
        byte count;
        Rmi.Marshaler.Read(msg, out count);
        for (byte i = 0; i < count; ++i)
        {
            byte eventtype;
            Rmi.Marshaler.Read(msg, out eventtype);
            PLAYER_FLOOR_CHECK type = (PLAYER_FLOOR_CHECK)eventtype;
            events.Add(type);
        }
        return events;
    }

    //더미에서 뒤집은 카드를 바닥에 이동
    IEnumerator move_toFloor_flip_card(byte number, PAE_TYPE pae_type, byte position)
    {
        // 뒤집은 카드 움직이기.
        CCardPicture deck_card_picture = deck_cards.Pop();
        CCard flipped_card = card_manager.find_card(number, pae_type, position);
        deck_card_picture.update_card(flipped_card, get_hwatoo_sprite(flipped_card));
        yield return StartCoroutine(flip_deck_card(deck_card_picture));

        yield return new WaitForSeconds(0.3f);

        deck_card_picture.transform.localScale = SCALE_TO_FLOOR;
        move_playercard_to_floor(deck_card_picture, CARD_EVENT_TYPE.NONE);

        yield return new WaitForSeconds(0.2f);
    }

    //더미에서 카드 뒤집기
    IEnumerator flip_deck_card(CCardPicture deck_card_picture)
    {
        Animator ani = deck_card_picture.GetComponentInChildren<Animator>();
        ani.enabled = true;
        ani.Play("rotation");

        yield return StartCoroutine(scale_to(
            deck_card_picture,
            2.0f,
            0.1f));
    }

    #endregion SC_FLIP_DECK_CARD_ACK

    #region SC_TURN_RESULT
    IEnumerator SC_TURN_RESULT(CMessage msg)
    {
        // 데이터 파싱 시작 ----------------------------------------
        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);
        if (is_me(player_index))
        {
            yield return StartCoroutine(on_turn_result(myPos, msg, player_index));
        }
        else
        {
            yield return StartCoroutine(on_turn_result(enemyPos, msg, player_index));
        }
        refresh_playerfloor_slots_all(myPos);
        refresh_playerfloor_slots_all(enemyPos);
    }
    //턴 결과
    IEnumerator on_turn_result(byte player_index, CMessage msg, byte player_index2)
    {
        bool bonus;
        Rmi.Marshaler.Read(msg, out bonus);
        List<CCard> floor_cards_to_player = parse_cards_to_get(msg);
        List<CCardPicture> other_cards_to_player = parse_cards_to_take_from_others(player_index, msg);
        List<PLAYER_FLOOR_CHECK> playerFloorCheck = parse_playerfloor_events(msg);
        List<CARD_EVENT_TYPE> events = parse_flip_card_events(msg);

        yield return StartCoroutine(Move_after_flip_card(player_index, other_cards_to_player, floor_cards_to_player, playerFloorCheck, msg, player_index2, 3));

    }
    //카드정보로 카드 값
    List<CCard> parse_cards_to_get(CMessage msg)
    {
        List<CCard> cards_to_give = new List<CCard>();
        byte count_to_give;
        Rmi.Marshaler.Read(msg, out count_to_give);

        for (int i = 0; i < count_to_give; ++i)
        {
            byte card_number;
            Rmi.Marshaler.Read(msg, out card_number);
            byte paetype;
            Rmi.Marshaler.Read(msg, out paetype);
            PAE_TYPE pae_type = (PAE_TYPE)paetype;
            byte position;
            Rmi.Marshaler.Read(msg, out position);
            CCard card = this.card_manager.find_card(card_number, pae_type, position);
            cards_to_give.Add(card);
        }

        return cards_to_give;
    }
    //다른유저에게 받는 카드 값
    List<CCardPicture> parse_cards_to_take_from_others(byte player_index, CMessage msg, bool is_move = true)
    {
        // 뺏어올 카드.
        List<CCardPicture> take_cards_from_others = new List<CCardPicture>();
        byte victim_count;
        Rmi.Marshaler.Read(msg, out victim_count);
        //Debug.Log(string.Format("================== 상대피 뺏어올 갯수. {0}", victim_count));
        for (byte victim = 0; victim < victim_count; ++victim)
        {
            byte victim_index;
            Rmi.Marshaler.Read(msg, out victim_index);
            byte count_to_take;
            Rmi.Marshaler.Read(msg, out count_to_take);
            for (byte i = 0; i < count_to_take; ++i)
            {
                byte card_number;
                Rmi.Marshaler.Read(msg, out card_number);
                byte paetype;
                Rmi.Marshaler.Read(msg, out paetype);
                PAE_TYPE pae_type = (PAE_TYPE)paetype;
                byte position;
                Rmi.Marshaler.Read(msg, out position);

                //Debug.LogError(string.Format("{0}, {1}, {2}", card_number, pae_type, position));

                if (is_me(victim_index))
                {
                    CCardPicture card_pic = player_floor_card_manager[myPos].get_card(card_number, pae_type, position);
                    if (card_pic == null)
                    {
                        Debug.LogError(string.Format("피뺏기 카드 NULL my->enemy : number:{0}, pae_type:{1}, position:{2}", card_number, pae_type, position));
                    }
                    else
                    {
                        take_cards_from_others.Add(card_pic);
                        if (is_move)
                        {
                            player_floor_card_manager[myPos].remove(card_pic);
                        }
                    }
                }
                else
                {
                    CCardPicture card_pic = player_floor_card_manager[enemyPos].get_card(card_number, pae_type, position);
                    if (card_pic == null)
                    {
                        Debug.LogError(string.Format("피뺏기 카드 NULL enemy->my : number:{0}, pae_type:{1}, position:{2}", card_number, pae_type, position));
                    }
                    else
                    {
                        take_cards_from_others.Add(card_pic);
                        if (is_move)
                        {
                            player_floor_card_manager[enemyPos].remove(card_pic);
                        }
                    }
                }
            }
        }

        return take_cards_from_others;
    }

    //뒤집은 카드 이동
    IEnumerator Move_after_flip_card(byte player_index, List<CCardPicture> other_cards_to_player, List<CCard> floor_cards_to_player, List<PLAYER_FLOOR_CHECK> playerFloorCheck, CMessage msg, byte player_index2, int local)
    {
        // 바닥에서 가져올 카드 가져오기.
        if (floor_cards_to_player.Count > 0)
        {
            //int i = floor_cards_to_player.Count; i-- > 0;
            for (int i = 0; i < floor_cards_to_player.Count; ++i)
            {
                CVisualFloorSlot slot = floor_cards_slots.Find(obj => obj.is_same_card(floor_cards_to_player[i], true));
                if (slot == null)
                {
                    UnityEngine.Debug.LogError(string.Format("2 Cannot find floor slot. {0}, {1}, {2}",
                        floor_cards_to_player[i].number, floor_cards_to_player[i].pae_type, floor_cards_to_player[i].position));
                    break;
                }
                CCardPicture card_picture = slot.find_card(floor_cards_to_player[i]);

                if (card_picture == null)
                {
                    Debug.LogError(floor_cards_to_player[i].number + "/" + floor_cards_to_player[i].pae_type.ToString() + "/" + floor_cards_to_player[i].position);
                    for (int j = 0; j < slot.get_cards().Count; j++) Debug.LogError(slot.get_cards()[j].card.number + "/" + slot.get_cards()[j].card.pae_type.ToString() + "/" + slot.get_cards()[j].card.position);

                    UnityEngine.Debug.LogError(string.Format("5 Cannot find the card. {0}, {1}, {2}",
                        floor_cards_to_player[i].number, floor_cards_to_player[i].pae_type, floor_cards_to_player[i].position));
                    //break;
                }

                slot.remove_card(card_picture);

                Vector3 begin = card_picture.transform.localPosition;
                Vector3 to = get_playerfloor_card_position(player_index, card_picture.card.pae_type);

                if (is_me(player_index2))
                {
                    card_picture.transform.localScale = SCALE_TO_MY_FLOOR;
                }
                else
                {
                    card_picture.transform.localScale = SCALE_TO_OTHER_FLOOR;
                }

                move_card(card_picture, begin, to);

                this.player_floor_card_manager[player_index].add(card_picture);

                yield return new WaitForSeconds(0.1f);
            }
        }
        // 상대방에게 뺏어올 카드 움직이기.
        for (int i = 0; i < other_cards_to_player.Count; ++i)
        {
            string tstr = "ESR_GAJUWA" + Random.Range(1, 3).ToString() + "__" + playerSoundIndex[player_index == 0 ? 1 : 0];

            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);
            if (is_me(player_index2))
            {
                other_cards_to_player[i].transform.localScale = SCALE_TO_MY_FLOOR;
            }
            else
            {
                other_cards_to_player[i].transform.localScale = SCALE_TO_OTHER_FLOOR;
            }
            Vector3 pos = get_playerfloor_card_position(player_index, PAE_TYPE.PEE);
            move_card(other_cards_to_player[i], other_cards_to_player[i].transform.localPosition, pos);
            this.player_floor_card_manager[player_index].add(other_cards_to_player[i]);

            yield return new WaitForSeconds(0.1f);
        }

        sort_floor_cards_when_finished_turn();
        refresh_playerhand_slots(player_index);
        // ★★★ 미션카드 먹은거 색깔변경
        int _cnt = 0;
        for (int i = 0; i < 5; i++) if (CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i].activeSelf) _cnt++;
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().ResetColor();
        hide_mission_mark();
        for (int i = 0; i < ListMissionCard.Count; i++)
        {
            // 내바닥
            if (player_floor_card_manager[myPos].get_card(ListMissionCard[i].number, ListMissionCard[i].pae_type, ListMissionCard[i].position) != null)
            {
                if (_cnt == 2)
                {
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i + 1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i + 1].transform.Find("mark_mission_1").gameObject.SetActive(true);
                }
                else
                {
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i].transform.Find("mark_mission_1").gameObject.SetActive(true);
                }
            }

            // 상대바닥
            if (player_floor_card_manager[enemyPos].get_card(ListMissionCard[i].number, ListMissionCard[i].pae_type, ListMissionCard[i].position) != null)
            {
                if (_cnt == 2)
                {
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i + 1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i + 1].transform.Find("mark_mission_2").gameObject.SetActive(true);
                }
                else
                {
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i].transform.Find("mark_mission_2").gameObject.SetActive(true);
                }
            }
        }

        // 미션처리
        if (msg != null)
        {
            byte type;          // 0==아무것도아님, 1==미션성공, 2==미션실패, 3==활빈당일때내뒷패붙음, 4==활빈당일때상대방뒷패붙음
            byte multiple;      // 0==활빈당아님, 2~11==현재배수

            Rmi.Marshaler.Read(msg, out type);
            Rmi.Marshaler.Read(msg, out multiple);

            if (type == 1)
            {
                m_objMissionSucces.SetActive(true);
                if (is_me(player_index2))
                    m_objMissionSucces.GetComponent<tk2dSprite>().SetSprite("mission_success02");
                else
                    m_objMissionSucces.GetComponent<tk2dSprite>().SetSprite("mission_success01");
                //SpriteAniManager.Instance.PlaySpriteAni(SpriteAniManager.SpriteAni.TK2D_MISSIONSUCCESS, false);
            }
            else if (type == 2)
            {
                SpriteAniManager.Instance.PlaySpriteAni(SpriteAniManager.SpriteAni.TK2D_MISSIONFAIL, false);
            }
            else if (type == 3)
            {
                GameObject g = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().whalbinMe;
                g.GetComponent<tk2dSpriteAnimator>().Play("Mission_Whalbin_Me_" + multiple);
            }
            else if (type == 4)
            {
                GameObject g = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().whalbinEnemy;
                g.GetComponent<tk2dSpriteAnimator>().Play("Mission_Whalbin_Enemy_" + multiple);
            }
        }

        int sndRandom = Random.Range(0, 3);
        sndRandom = 0;
        string str = "";

        if (playerFloorCheck.Count > 0)
        {
            string tstr = "";
            for (int i = 0; i < playerFloorCheck.Count; ++i)
            {
                Vector3 pos = new Vector3();
                switch (playerFloorCheck[i])
                {
                    case PLAYER_FLOOR_CHECK.FIVE_KWANG:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.KWANG);

                        tstr = "ET_5KWANG__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_5KWANG" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.FORE_KWANG:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.KWANG);

                        tstr = "ET_4KWANG__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_4KWANG" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.THREE_KWANG:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.KWANG);

                        tstr = "ET_3KWANG__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_3KWANG" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.BI_THREE_KWANG:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.KWANG);

                        tstr = "ET_BISAMKWANG__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_B3KWANG" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.GODORI:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.YEOL);

                        tstr = "ET_GODORI__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_GODORI" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.HONGDAN:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.TEE);

                        tstr = "ET_HONGDAN__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_HONGDAN" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.CHODAN:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.TEE);

                        tstr = "ET_CHODAN__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_CHODAN" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.CHUNGDAN:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.TEE);

                        tstr = "ET_CHUNGDAN__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

                        str = "ESR_CHEONGDAN" + sndRandom.ToString() + "__" + playerSoundIndex[player_index2].ToString();
                        SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
                        break;
                    case PLAYER_FLOOR_CHECK.MUNGTTA:
                        pos = get_playerfloor_card_position(player_index, PAE_TYPE.YEOL);

                        tstr = "ET_MUNGTTA__" + playerSoundIndex[player_index2];
                        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);
                        break;
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    #endregion SC_TURN_RESULT

    #region SC_ASK_GO_OR_STOP
    IEnumerator SC_ASK_GO_OR_STOP(CMessage msg)
    {
        byte playerIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);

        byte gocout;
        Rmi.Marshaler.Read(msg, out gocout);
        gocout = (byte)(gocout + 1);

        long stopMoney;
        Rmi.Marshaler.Read(msg, out stopMoney);

        AniManager.Instance.StopAllMovie();

        refresh_playerfloor_slots_all(myPos);
        refresh_playerfloor_slots_all(enemyPos);

        yield return new WaitForSeconds(0.2f);

        if (!is_me(playerIndex) || meObserver == true)
        {
            // 상대방 고스톱 결정 동영상대기
            StartCoroutine(AniManager.Instance.CoWaitChoice(player_me_index));
            //StartCoroutine(AniManager.Instance.CoWaitGoTopChoice(player_me_index));
            yield break;
        }

        switch (gocout)
        {
            case 1:
                CUIManager.Instance.show(UI_PAGE.POPUP_GO_STOP1);
                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GO_STOP1).GetComponent<CPopupGoStop>().showMoney(stopMoney, PrefabFont);
                break;
            case 2:
                CUIManager.Instance.show(UI_PAGE.POPUP_GO_STOP2);
                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GO_STOP2).GetComponent<CPopupGoStop>().showMoney(stopMoney, PrefabFont);
                break;
            case 3:
                CUIManager.Instance.show(UI_PAGE.POPUP_GO_STOP3);
                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GO_STOP3).GetComponent<CPopupGoStop>().showMoney(stopMoney, PrefabFont);
                break;
            case 4:
                CUIManager.Instance.show(UI_PAGE.POPUP_GO_STOP4);
                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GO_STOP4).GetComponent<CPopupGoStop>().showMoney(stopMoney, PrefabFont);
                break;
            case 5:
                CUIManager.Instance.show(UI_PAGE.POPUP_GO_STOP5);
                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GO_STOP5).GetComponent<CPopupGoStop>().showMoney(stopMoney, PrefabFont);
                break;
            case 6:
                CUIManager.Instance.show(UI_PAGE.POPUP_GO_STOP6);
                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GO_STOP6).GetComponent<CPopupGoStop>().showMoney(stopMoney, PrefabFont);
                break;
            case 7:
                CUIManager.Instance.show(UI_PAGE.POPUP_GO_STOP7);
                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GO_STOP7).GetComponent<CPopupGoStop>().showMoney(stopMoney, PrefabFont);
                break;
        }

        // 키보드 받음
        keyState = UserInputState.GOSTOP;
    }
    #endregion SC_ASK_GO_OR_STOP

    #region SC_NOTIFY_GO_COUNT
    IEnumerator SC_NOTIFY_GO_COUNT(CMessage msg)
    {
        AniManager.Instance.StopAllMovie();

        refresh_playerfloor_slots_all(myPos);
        refresh_playerfloor_slots_all(enemyPos);

        yield return new WaitForSeconds(0.5f);

        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);
        byte delay;
        Rmi.Marshaler.Read(msg, out delay);
        byte go_count;
        Rmi.Marshaler.Read(msg, out go_count);
        yield return StartCoroutine(show_go_count(go_count, player_index));
    }
    //고 횟수 보이기
    IEnumerator show_go_count(byte count, byte player_index)
    {
        string tstr = "ET_" + count.ToString() + "GO__" + playerSoundIndex[player_index].ToString();
        AniManager.Instance.PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);

        int sndRandom = Random.Range(0, 3);
        sndRandom = 0;
        string str = "";
        if (sndRandom == 0)
        {
            str += "ESR_" + count.ToString() + "GO__" + playerSoundIndex[player_index].ToString();
            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
        }
        else
        {
            str += "ESR_" + count.ToString() + "GO" + (sndRandom + 1).ToString() + "__" + playerSoundIndex[player_index].ToString();
            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), str), false);
        }

        while (true)
        {
            if (AniManager.Instance.IsEnd((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr)))
                break;

            yield return null;
        }
    }
    #endregion SC_NOTIFY_GO_COUNT

    #region SC_UPDATE_PLAYER_STATISTICS
    //플레이어 상태 갱신
    void SC_UPDATE_PLAYER_STATISTICS(CMessage msg)
    {
        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);

        short score;
        Rmi.Marshaler.Read(msg, out score);
        byte go_count;
        Rmi.Marshaler.Read(msg, out go_count);
        byte shaking_count;
        Rmi.Marshaler.Read(msg, out shaking_count);
        byte ppuk_count;
        Rmi.Marshaler.Read(msg, out ppuk_count);
        byte pee_count;
        Rmi.Marshaler.Read(msg, out pee_count);
        bool peebak;
        Rmi.Marshaler.Read(msg, out peebak);
        bool gwangbak;
        Rmi.Marshaler.Read(msg, out gwangbak);
        short multiple;
        Rmi.Marshaler.Read(msg, out multiple);

        // 판돈*최종점수=승리금액 ==> startMoney*finalScore=승리금액
        //byte is_win;            // 0==무, 1==승, 2==패
        //int startMoney;         // 판돈
        //int finalScore;         // 최종점수
        //                        //int score;              // 패점수
        //int goBak;              // 고박 2배
        //int peeBak;             // 피박 2배
        //int gwangBak;           // 광박 2배
        //int meongTta;           // 멍따 2배
        //int shake;              // 흔들기 8배
        //int goMulti;            // 고횟수 32배
        //int missionMulti;       // 미션 10배
        //int drawMulti;          // 무승부 8배
        //int pushMulti;          // 밀었을때 8배
        //bool is_push;           // 밀기여부 가능 true 불가능 false
        //int chongtongNumber;    // 총통숫자(12 == 없음, 0~11 == 넘버)
        //bool is_threePpuck;     // 3뻑인지
        //long winnerMoney;       // 승리플레이어가 획득한 머니
        //long loserMoney;       // 패배플레이어가 잃은 머니
        //long dealerMoney;       // 딜러비
        //bool is_noMoney;        // 판돈 부족여부
        //int jackpotMoney;       // 잭팟 머니(판돈)
        //int jackpotReward;      // 잭팟 배율(200배)
        //short score1, score2, score3, score4, score5;           // 광 열 띠 피 고

        //bool canGameStart;

        //Rmi.Marshaler.Read(msg, out is_win);
        //Rmi.Marshaler.Read(msg, out startMoney);
        //Rmi.Marshaler.Read(msg, out finalScore);
        ////Rmi.Marshaler.Read(msg, out score);
        //Rmi.Marshaler.Read(msg, out goBak);
        //Rmi.Marshaler.Read(msg, out peeBak);
        //Rmi.Marshaler.Read(msg, out gwangBak);
        //Rmi.Marshaler.Read(msg, out meongTta);
        //Rmi.Marshaler.Read(msg, out shake);
        //Rmi.Marshaler.Read(msg, out goMulti);
        //Rmi.Marshaler.Read(msg, out missionMulti);
        //Rmi.Marshaler.Read(msg, out drawMulti);
        //Rmi.Marshaler.Read(msg, out pushMulti);
        //Rmi.Marshaler.Read(msg, out is_push);
        //Rmi.Marshaler.Read(msg, out chongtongNumber);
        //Rmi.Marshaler.Read(msg, out is_threePpuck);
        //Rmi.Marshaler.Read(msg, out winnerMoney);
        //Rmi.Marshaler.Read(msg, out loserMoney);
        //Rmi.Marshaler.Read(msg, out dealerMoney);
        //Rmi.Marshaler.Read(msg, out is_noMoney);
        //Rmi.Marshaler.Read(msg, out jackpotMoney);
        //Rmi.Marshaler.Read(msg, out jackpotReward);
        //Rmi.Marshaler.Read(msg, out score1);
        //Rmi.Marshaler.Read(msg, out score2);
        //Rmi.Marshaler.Read(msg, out score3);
        //Rmi.Marshaler.Read(msg, out score4);
        //Rmi.Marshaler.Read(msg, out score5);
        //Rmi.Marshaler.Read(msg, out canGameStart);

        //short[] goMultiple = { 0, 0, 2, 4, 8, 16, 32, 64, 128, 256, 512 };
        //short[] shakingMultiple = { 0, 2, 4, 8, 16 };
        //short peebakMultiple = 0;
        //short gwangbakMultiple = 0;

        //if (peebak)
        //    peebakMultiple = 2;
        //if (gwangbak)
        //    gwangbakMultiple = 2;
        //short multiple = (short)(goMultiple[go_count] + shakingMultiple[shaking_count] + peebakMultiple + gwangbakMultiple);

        if (multiple <= 0)
            multiple = 1;

        if (is_me(player_index))
        {
            this.player_info_slots[myPos].update_score(score);
            this.player_info_slots[myPos].update_go(go_count);
            this.player_info_slots[myPos].update_shake(shaking_count);
            this.player_info_slots[myPos].update_ppuk(ppuk_count);
            this.player_info_slots[myPos].update_peecount(pee_count, gwangbak, peebak, player_floor_card_manager[myPos]);
        }
        else
        {
            this.player_info_slots[enemyPos].update_score(score);
            this.player_info_slots[enemyPos].update_go(go_count);
            this.player_info_slots[enemyPos].update_shake(shaking_count);
            this.player_info_slots[enemyPos].update_ppuk(ppuk_count);
            this.player_info_slots[enemyPos].update_peecount(pee_count, gwangbak, peebak, player_floor_card_manager[enemyPos]);
        }
    }
    #endregion SC_UPDATE_PLAYER_STATISTICS

    #region SC_ASK_KOOKJIN_TO_PEE
    void SC_ASK_KOOKJIN_TO_PEE(CMessage msg)
    {
        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);

        refresh_playerfloor_slots_all(myPos);
        refresh_playerfloor_slots_all(enemyPos);

        if (is_me(player_index) && meObserver == false)
        {
            CUIManager.Instance.show(UI_PAGE.POPUP_ASK_KOOKJIN);

            // 키보드 받음
            keyState = UserInputState.KOOGJIN;
        }
        else
        {
            StartCoroutine(AniManager.Instance.CoWaitPaeChoice(player_me_index));
        }
    }
    #endregion SC_ASK_KOOKJIN_TO_PEE

    #region SC_MOVE_KOOKJIN_TO_PEE
    IEnumerator SC_MOVE_KOOKJIN_TO_PEE(CMessage msg)
    {
        byte player_index;
        Rmi.Marshaler.Read(msg, out player_index);
        if (is_me(player_index))
        {
            yield return StartCoroutine(move_kookjin_to_pee(myPos, player_index));
        }
        else
        {
            yield return StartCoroutine(move_kookjin_to_pee(enemyPos, player_index));
        }
    }
    //국진을 피로 이동
    IEnumerator move_kookjin_to_pee(byte player_index, byte player_index2)
    {
        CCardPicture card_picture =
            this.player_floor_card_manager[player_index].get_card(8, PAE_TYPE.YEOL, 0);

        // 카드 자리 움직이기.
        move_card(card_picture, card_picture.transform.localPosition,
            get_playerfloor_card_position(player_index, PAE_TYPE.PEE));

        // 열끗에서 지우고 피로 넣는다.
        this.player_floor_card_manager[player_index].remove(card_picture);

        card_picture.card.change_pae_type(PAE_TYPE.PEE);
        card_picture.card.set_card_status(CARD_STATUS.TWO_PEE);

        this.player_floor_card_manager[player_index].add(card_picture);

        yield return new WaitForSeconds(1.0f);

        // 바닥 패 정렬.
        refresh_playerfloor_slots_all(player_index);

    }
    #endregion SC_MOVE_KOOKJIN_TO_PEE

    #region 유저 나갔을때
    void UserRoomOut(CMessage msg)
    {
        bool init;
        Rmi.Marshaler.Read(msg, out init);

        bool canGamePractice;
        Rmi.Marshaler.Read(msg, out canGamePractice);

        if (init)
        {
            // 게임결과 시 자동치기 해제
            autoPlay.StopWorking();
            // 초기화
            reset();
            CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
            CUIManager.Instance.ResetGoNStopAll();
            for (int i = 0; i < player_info_slots.Count; i++) player_info_slots[i].resetAll();
            CPopupGameResult popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>();
            StartCoroutine(popup.PlayerAction(CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().Name.GetComponent<Text>().text, 2));
            CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().ResetUserInfo();
            isGaming = false;
            meObserver = false;
            EventEnd();
        }
        else
        {
            CPopupGameResult popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>();
            StartCoroutine(popup.PlayerAction(CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().Name.GetComponent<Text>().text, 2));

            CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().ResetUserInfo();
        }

        // 연습게임 
        if (canGamePractice == true)
        {
            CUIManager.Instance.show(UI_PAGE.BUTTON_PRACTICE);
        }
        else
        {
            CUIManager.Instance.hide(UI_PAGE.BUTTON_PRACTICE);
        }

        CUIManager.Instance.hide(UI_PAGE.BUTTON_START);
    }
    #endregion

    #region SC_GAME_RESULT
    //게임결과 화면
    IEnumerator SC_GAME_RESULT(CMessage msg)
    {
        CUIManager.Instance.hide(UI_PAGE.NOTICE_PRACTICE);
        AniManager.Instance.StopAllMovie();

        refresh_playerfloor_slots_all(myPos);
        refresh_playerfloor_slots_all(enemyPos);

        yield return new WaitForSeconds(0.5f);

        hide_hint_mark();

        // 판돈*최종점수=승리금액 ==> startMoney*finalScore=승리금액
        byte is_win;            // 0==무, 1==승, 2==패
        int startMoney;         // 판돈
        int finalScore;         // 최종점수
        int score;              // 패점수
        int goBak;              // 고박 2배
        int peeBak;             // 피박 2배
        int gwangBak;           // 광박 2배
        int meongTta;           // 멍따 2배
        int shake;              // 흔들기 8배
        int goMulti;            // 고횟수 32배
        int missionMulti;       // 미션 10배
        int drawMulti;          // 무승부 8배
        int pushMulti;          // 밀었을때 8배
        bool is_push;           // 밀기여부 가능 true 불가능 false
        int chongtongNumber;    // 총통숫자(12 == 없음, 0~11 == 넘버)
        bool is_threePpuck;     // 3뻑인지
        long winnerMoney;       // 승리플레이어가 획득한 머니
        long loserMoney;       // 패배플레이어가 잃은 머니
        long dealerMoney;       // 딜러비
        bool is_noMoney;        // 판돈 부족여부
        int jackpotMoney;       // 잭팟 머니(판돈)
        int jackpotReward;      // 잭팟 배율(200배)
        short score1, score2, score3, score4, score5;           // 광 열 띠 피 고

        bool canGameStart;

        Rmi.Marshaler.Read(msg, out is_win);
        Rmi.Marshaler.Read(msg, out startMoney);
        Rmi.Marshaler.Read(msg, out finalScore);
        Rmi.Marshaler.Read(msg, out score);
        Rmi.Marshaler.Read(msg, out goBak);
        Rmi.Marshaler.Read(msg, out peeBak);
        Rmi.Marshaler.Read(msg, out gwangBak);
        Rmi.Marshaler.Read(msg, out meongTta);
        Rmi.Marshaler.Read(msg, out shake);
        Rmi.Marshaler.Read(msg, out goMulti);
        Rmi.Marshaler.Read(msg, out missionMulti);
        Rmi.Marshaler.Read(msg, out drawMulti);
        Rmi.Marshaler.Read(msg, out pushMulti);
        Rmi.Marshaler.Read(msg, out is_push);
        Rmi.Marshaler.Read(msg, out chongtongNumber);
        Rmi.Marshaler.Read(msg, out is_threePpuck);
        Rmi.Marshaler.Read(msg, out winnerMoney);
        Rmi.Marshaler.Read(msg, out loserMoney);
        Rmi.Marshaler.Read(msg, out dealerMoney);
        Rmi.Marshaler.Read(msg, out is_noMoney);
        Rmi.Marshaler.Read(msg, out jackpotMoney);
        Rmi.Marshaler.Read(msg, out jackpotReward);
        Rmi.Marshaler.Read(msg, out score1);
        Rmi.Marshaler.Read(msg, out score2);
        Rmi.Marshaler.Read(msg, out score3);
        Rmi.Marshaler.Read(msg, out score4);
        Rmi.Marshaler.Read(msg, out score5);
        Rmi.Marshaler.Read(msg, out canGameStart);

        if (is_threePpuck) yield return StartCoroutine(CUIManager.Instance.ppukShow(3, 0));

        if (chongtongNumber >= 0 && chongtongNumber <= 11) keyState = UserInputState.CHONGTONG;
        else if (is_threePpuck) keyState = UserInputState.THREEPPUK;
        else keyState = UserInputState.RESULT;

        CUIManager.Instance.show(UI_PAGE.POPUP_GAME_RESULT);
        CPopupGameResult popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>();
        yield return StartCoroutine(popup.refresh(is_win, startMoney, finalScore, score, goBak, peeBak, gwangBak, meongTta, shake, goMulti, missionMulti, drawMulti, pushMulti, is_push, chongtongNumber, winnerMoney, loserMoney, dealerMoney, player_me_index, jackpotMoney, jackpotReward, score1, score2, score3, score4, score5, canGameStart));

        yield return new WaitForSeconds(1.5f);

        isGaming = false;
        meObserver = false;

        //AniManager.Instance.StopAllMovie();

        // 돈 없으면 퇴장
        if (is_noMoney) CUIManager.Instance.ExitGame();

        yield return new WaitForSeconds(2.5f);

        EventEnd();

    }
    #endregion SC_GAME_RESULT

    void SC_PRACTICE_GAME_END(CMessage msg)
    {
        CUIManager.Instance.hide(UI_PAGE.NOTICE_PRACTICE);
        // 연습게임 종료 시 자동치기 해제
        autoPlay.StopWorking();
        AniManager.Instance.StopAllMovie();
        isGaming = false;
        meObserver = false;
        EventEnd();
        reset();
        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
        CUIManager.Instance.ResetGoNStopAll();
        for (int i = 0; i < player_info_slots.Count; i++) player_info_slots[i].resetAll();
    }

    #region SC_PUSH
    IEnumerator SC_PUSH(CMessage msg)
    {
        yield return null;
    }
    #endregion SC_CHONGTONG

    #region SC_EVENT_START
    IEnumerator EventStart(CMessage msg)
    {
        //// 이미지 및 애니메이션 초기화
        //reset();
        //// 뻑,흔들기,광박,피박,고,점수,피갯수 초기화
        //for (int i = 0; i < player_info_slots.Count; i++) player_info_slots[i].resetAll();

        yield return new WaitForSeconds(0.2f);

        // 이벤트 시작
        isEventing = true;
        back_image = CSpriteManager.Instance.get_sprite("event_card");

        AniManager.Instance.PlayMovie(AniManager._eType.EVENTINTRO, 1, 30);
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_EVENTINTRO, false);

        yield return new WaitForSeconds(3.5f);

        EventBG.SetActive(true);
    }

    void EventEnd()
    {
        // 이벤트종료
        isEventing = false;
        back_image = CSpriteManager.Instance.get_sprite(backImage);
        EventBG.SetActive(false);
    }
    #endregion

    #region SC_EVENT_JACKPOT_INFO
    bool overlapPrevent = true;
    long NowJackpotMoney = 0;

    void SC_EVENT_JACKPOT_INFO(CMessage msg, bool tmp = false)
    {
        long jackpotMoney;
        Rmi.Marshaler.Read(msg, out jackpotMoney);
        JackpotManager.Instance.JackpotMoney = jackpotMoney;
    }

    IEnumerator SC_EVENT_JACKPOT_INFO(CMessage msg)
    {
        long jackpotMoney;
        Rmi.Marshaler.Read(msg, out jackpotMoney);

        if (overlapPrevent)
        {
            overlapPrevent = false;

            long unitMoney = (jackpotMoney - NowJackpotMoney) / 300;

            if (unitMoney > 0)
            {
                while (NowJackpotMoney < jackpotMoney)
                {
                    NowJackpotMoney += unitMoney;

                    JackpotRefresh(NowJackpotMoney);

                    yield return null;
                }
            }
            else if (unitMoney < 0)
            {
                while (NowJackpotMoney > jackpotMoney)
                {
                    NowJackpotMoney += unitMoney;

                    JackpotRefresh(NowJackpotMoney);

                    yield return null;
                }
            }
            overlapPrevent = true;
            NowJackpotMoney = jackpotMoney;
            JackpotRefresh(jackpotMoney);
        }

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
        float numberX = 47;
        float numberY = 370;
        float numberZ = 0;
        float numberD = -12;

        float dotY = 364;
        float dotZ = 0;
        float dotD = -9;

        for (int i = 0; i < _num.Length; i++)
        {
            // 3자리가 넘어가면 dot 생성
            if (i % 3 == 0 && i != 0)
            {
                GameObject g = Instantiate(PrefabFont);

                g.GetComponent<tk2dSprite>().SetSprite("j_D");

                numberX += dotD;

                float posX = 0;

                posX = numberX + (i - 1) * numberD;

                g.transform.position = new Vector3(posX, dotY, dotZ);

                ListJackpotNumberObject.Add(g);
            }

            {
                GameObject g = Instantiate(PrefabFont);

                g.GetComponent<tk2dSprite>().SetSprite("j_0" + _num[i].ToString());

                float posX = 0;

                if (i % 3 == 0 && i != 0) numberX -= numberD - dotD;

                posX = numberX + i * numberD;

                g.transform.position = new Vector3(posX, numberY, numberZ);

                ListJackpotNumberObject.Add(g);
            }
        }
    }
    #endregion

    #region SC_USER_INFO
    void SC_USER_INFO(CMessage msg)
    {
        byte playerIndex;
        Rmi.Marshaler.Read(msg, out playerIndex);

        Rmi.Marshaler.UserInfo userInfo = new Rmi.Marshaler.UserInfo();
        Rmi.Marshaler.Read(msg, out userInfo);

        bool join;
        Rmi.Marshaler.Read(msg, out join);

        // 남자 보이스만 사용
        //playerSoundIndex[playerIndex] = userInfo.voice;

        if (is_me(playerIndex))
        {
            CUIManager.Instance.transform.Find("Player01").transform.GetComponent<CUserInfo>().SetUserInfo(userInfo);
        }
        else
        {
            CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().SetUserInfo(userInfo);

            if (join)
            {
                CPopupGameResult popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>();
                StartCoroutine(popup.PlayerAction(CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().Name.GetComponent<Text>().text, 1));
                SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_JOIN, false);
            }
        }

    }
    #endregion

    #region Util
    public bool is_me(byte player_index)
    {
        return player_me_index == player_index;
    }
    public byte get_other_player_index(byte player_index)
    {
        if (player_index == 0) return 1;
        return 0;
    }
    //카드 이동
    void move_card(CCardPicture card_picture, Vector3 begin, Vector3 to, float duration = 0.1f)
    {
        if (card_picture.card != null)
        {
            card_picture.update_image(get_hwatoo_sprite(card_picture.card));
        }
        else
        {
            card_picture.update_image(back_image);
        }
        card_picture.sprite_renderer.sortingOrder = CSpriteLayerOrderManager.Instance.Order;
        LeanTween.moveLocal(card_picture.gameObject, to, duration).setEase(LeanTweenType.linear);

        // 180918 레이어 정리 추가
        StartCoroutine(delaySet(card_picture, duration));
        // -----------------------
    }

    // 180918 레이어 정리 추가
    IEnumerator delaySet(CCardPicture card_picture, float sec)
    {
        yield return new WaitForSeconds(sec);
        card_picture.sprite_renderer.sortingLayerName = "ingame_object";
    }
    // -----------------------

    //카드 스케일변화
    IEnumerator scale_to(CCardPicture card_picture, float ratio, float duration)
    {
        // 180918 레이어 정리 추가
        card_picture.sprite_renderer.sortingLayerName = "Ani";
        // -----------------------
        card_picture.sprite_renderer.sortingOrder = CSpriteLayerOrderManager.Instance.Order;
        LeanTween.scale(card_picture.gameObject, card_picture.transform.localScale * ratio, duration);
        yield return null;
    }
    //카드 그림 가져오기
    Sprite get_hwatoo_sprite(CCard card)
    {
        int sprite_index = card.number * 4 + card.position;
        return CSpriteManager.Instance.get_card_sprite(sprite_index);
    }

    IEnumerator delay_if_exist(byte delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
    }

    #endregion Util

    #region PLAYER HAND
    //플레이어의 패를 번호 순서에 따라 오름차순 정렬 한다.
    void sort_playerhand_slots(byte player_index)
    {
        if (meObserver == true) return;
        if (is_me(player_index))
        {
            this.player_hand_card_manager[myPos].sort_by_number();
            refresh_playerhand_slots(myPos);
        }
        else
        {
            this.player_hand_card_manager[enemyPos].sort_by_bomb();
            refresh_playerhand_slots(enemyPos);
        }
    }

    // 플레이어의 패의 위치를 갱신한다.
    // 패를 내면 중간중간 빠진 자리가 생기는데 그 자리를 처음부터 다시 채워준다.
    void refresh_playerhand_slots(byte player_index)
    {
        CPlayerHandCardManager hand_card_manager = this.player_hand_card_manager[player_index];
        byte count = (byte)hand_card_manager.get_card_count();
        for (byte card_index = 0; card_index < count; ++card_index)
        {
            CCardPicture card = hand_card_manager.get_card(card_index);
            // 슬롯 인덱스를 재설정 한다.
            card.set_slot_index(card_index);

            // 화면 위치를 재설정 한다.
            card.transform.localPosition = this.player_card_positions[player_index].get_hand_position(card_index);
        }

        // 자동치기 버튼 활성화
        if (is_me(player_index) && meObserver == false)
        {
            int cardCnt = player_hand_card_manager[myPos].get_card_count();
            if (cardCnt <= 9)
            {
                autoPlay.ShowButton();
            }
            else if (cardCnt == 0 || cardCnt == 10)
            {
                autoPlay.HideButton();
            }
        }
    }

    #endregion PLAYER HAND

    #region FLOOR EVENT
    //바닥패 위치를 구한다
    Vector3 get_floorcards_slot_position(CVisualFloorSlot slot)
    {
        Vector3 position = this.floor_slot_position[slot.ui_slot_position];
        int stacked_count = slot.get_card_count();
        position.x += (stacked_count * 24.0f);
        position.y -= (stacked_count * 3.0f);
        return position;
    }
    //카드 분배후 바닥패 정렬
    void sort_floor_cards_after_distributed(List<CCardPicture> begin_cards_picture)
    {
        Dictionary<byte, byte> slots = new Dictionary<byte, byte>();

        for (byte i = 0; i < begin_cards_picture.Count; ++i)
        {
            byte number = begin_cards_picture[i].card.number;
            CVisualFloorSlot slot = this.floor_cards_slots.Find(obj => obj.is_same_card(begin_cards_picture[i].card));
            Vector3 to = Vector3.zero;
            if (slot == null)
            {
                to = this.floor_slot_position[i];

                slot = this.floor_cards_slots[i];
                slot.add_card(begin_cards_picture[i]);
            }
            else
            {
                to = get_floorcards_slot_position(slot);

                slot.add_card(begin_cards_picture[i]);
            }


            Vector3 begin = this.floor_slot_position[i];
            move_card(begin_cards_picture[i], begin, to);
        }
    }
    //턴이 끝나면 바닥패 정렬
    void sort_floor_cards_when_finished_turn()
    {
        for (int i = 0; i < this.floor_cards_slots.Count; ++i)
        {
            CVisualFloorSlot slot = this.floor_cards_slots[i];
            if (slot.get_card_count() != 1)
            {
                continue;
            }

            CCardPicture card_pic = slot.get_first_card();
            move_card(card_pic,
                card_pic.transform.localPosition,
                this.floor_slot_position[slot.ui_slot_position]);
        }
    }
    //카드를 바닥으로 이동
    void move_playercard_to_floor(CCardPicture card_picture, CARD_EVENT_TYPE event_type)
    {
        byte slot_index = 0;
        Vector3 begin = card_picture.transform.localPosition;
        Vector3 to = Vector3.zero;

        CVisualFloorSlot slot;

        if (bonusFlip)
        {
            slot = tempSlot;
            bonusFlip = false;
        }
        else
        {
            slot = this.floor_cards_slots.Find(obj => obj.is_same_card(card_picture.card));
        }
        tempSlot = slot;
        SoundManager._eSoundResource eType = SoundManager._eSoundResource.ESR_PUT1;
        if (slot == null)
        {
            byte empty_slot = find_empty_floorslot();
            //Debug.Log(string.Format("empty slot pos " + empty_slot));
            to = this.floor_slot_position[empty_slot];
            slot_index = empty_slot;
        }
        else
        {
            eType = SoundManager._eSoundResource.ESR_PUT2;
            to = get_floorcards_slot_position(slot);
            List<CCardPicture> floor_card_pictures = slot.get_cards();
            for (int i = 0; i < floor_card_pictures.Count; ++i)
            {
                Animator ani = floor_card_pictures[i].GetComponentInChildren<Animator>();
                ani.enabled = true;
                ani.Play("card_hit_under");
            }

            slot_index = slot.ui_slot_position;

            if (event_type != CARD_EVENT_TYPE.BOMB)
            {
                CEffectManager.Instance.play_dust(to, 0.1f, false);
            }

            Animator card_ani = card_picture.GetComponentInChildren<Animator>();
            card_ani.enabled = true;
            card_ani.Play("card_hit");
        }

        // 바닥 카드로 등록.
        this.floor_cards_slots[slot_index].add_card(card_picture);

        move_card(card_picture, begin, to, 0.01f);
        SoundManager.Instance.PlaySound(eType, false);
    }
    //바닥슬롯중 빈 슬롯 찾기
    byte find_empty_floorslot()
    {
        CVisualFloorSlot slot = this.floor_cards_slots.Find(obj => obj.get_card_count() == 0);
        if (slot == null)
        {
            return byte.MaxValue;
        }

        return slot.ui_slot_position;
    }
    #endregion FLOOR EVENT

    #region PLAYERFLOOR
    // 플레이어의 바닥 카드 위치를 갱신한다.
    // 피를 뺏기거나 옮기거나 했을 때 생기는 빈자리를 채워준다.
    void refresh_playerfloor_slots(PAE_TYPE pae_type, byte player_index)
    {
        //Debug.Log(pae_type + "    " + player_index);
        int count = this.player_floor_card_manager[player_index].get_card_count(pae_type);
        //if (player_index == myPos) Debug.LogError("내" + count);
        //if (player_index == enemyPos) Debug.LogError("적" + count);
        for (int i = 0; i < count; ++i)
        {
            Vector3 pos = this.player_card_positions[player_index].get_floor_position(player_index, i, pae_type, is_me(player_index), count);
            CCardPicture card_pic = this.player_floor_card_manager[player_index].get_card_at(pae_type, i);
            pos.z = card_pic.transform.position.z;
            card_pic.transform.position = pos;
        }
    }
    void refresh_playerfloor_slots_all(byte player_index)
    {
        if (NetworkManager.Instance.Screen == false)
            return;

        for (PAE_TYPE pae_type = PAE_TYPE.PEE; pae_type <= PAE_TYPE.YEOL; ++pae_type)
        {
            //Debug.Log(pae_type + "    " + player_index);
            int count = this.player_floor_card_manager[player_index].get_card_count(pae_type);
            //if (player_index == myPos) Debug.LogError("내" + count);
            //if (player_index == enemyPos) Debug.LogError("적" + count);
            for (int i = 0; i < count; ++i)
            {
                Vector3 pos = this.player_card_positions[player_index].get_floor_position(player_index, i, pae_type, is_me(player_index), count);
                CCardPicture card_pic = this.player_floor_card_manager[player_index].get_card_at(pae_type, i);
                pos.z = card_pic.transform.position.z;
                card_pic.transform.position = pos;
            }
        }
    }

    //플레이어 카드 위치 찾기
    Vector3 get_playerfloor_card_position(byte player_index, PAE_TYPE pae_type)
    {
        int count = this.player_floor_card_manager[player_index].get_card_count(pae_type);
        return this.player_card_positions[player_index].get_floor_position(player_index, count, pae_type, is_me(player_index), count);
    }

    #endregion PLAYERFLOOR

    #region CARD TOUCH EVENT
    //유저 카드 터치
    void on_card_touch(CCardPicture card_picture)
    {
        if (meObserver) return;
        // 카드 연속 터치등을 막기 위한 처리.
        this.card_collision_manager.enabled = false;
        this.ef_focus.SetActive(false);

        // 키보드 안받음
        keyState = UserInputState.NONE;

        int count = this.player_hand_card_manager.Count;
        for (int i = 0; i < count; ++i)
        {
            this.player_hand_card_manager[i].enable_all_colliders(false);
        }


        // 일반 카드, 폭탄 카드에 따라 다르게 처리한다.
        if (card_picture.card.number == 13)
        {
            //폭탄카드를 낸경우
            CMessage newmsg = new CMessage();
            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionFlipBomb;
            newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
            NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);

            card_picture.gameObject.SetActive(false);
            this.player_hand_card_manager[myPos].remove(card_picture);
        }
        else
        {
            // 손에 같은 카드 3장이 있고 바닥에 같은카드가 없을 때 흔들기 팝업을 출력한다.
            int same_on_hand =
                this.player_hand_card_manager[myPos].get_same_number_count(card_picture.card.number);
            int same_on_floor = get_samenumbercount_on_floor(card_picture.card);
            // 이게 흔들때
            if (same_on_hand == 3 && same_on_floor == 0)
            {
                //흔듦 팝업
                CUIManager.Instance.show(UI_PAGE.POPUP_ASK_SHAKING);
                CPopupShaking popup =
                    CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_SHAKING).GetComponent<CPopupShaking>();
                popup.refresh(card_picture.card, card_picture.slot);
            }
            // 이게 일반카드
            else
            {
                CPlayGameUI.send_select_card(card_picture.card, card_picture.slot, 0);
            }
        }
    }
    //바닥에 같은 숫자 카드 찾기
    int get_samenumbercount_on_floor(CCard card)
    {
        List<CVisualFloorSlot> slots =
            this.floor_cards_slots.FindAll(obj => obj.is_same_card(card));
        return slots.Count;
    }
    #endregion CARD TOUCH EVENT

    #region Hint
    //------------------------------------------------------------------------------
    //힌트 감추기
    public void hide_hint_mark()
    {
        for (int i = 0; i < player_hand_card_manager[myPos].get_card_count(); i++)
        {
            player_hand_card_manager[myPos].get_card(i).transform.Find("mark_first").gameObject.SetActive(false);
            player_hand_card_manager[myPos].get_card(i).transform.Find("mark_die").gameObject.SetActive(false);
            player_hand_card_manager[myPos].get_card(i).transform.Find("mark_bomb").gameObject.SetActive(false);
            player_hand_card_manager[myPos].get_card(i).transform.Find("mark_mission").gameObject.SetActive(false);
            player_hand_card_manager[myPos].get_card(i).transform.Find("mark_shake").gameObject.SetActive(false);
        }
        for (int i = 0; i < player_hand_card_manager[enemyPos].get_card_count(); i++)
        {
            player_hand_card_manager[enemyPos].get_card(i).transform.Find("mark_first").gameObject.SetActive(false);
            player_hand_card_manager[enemyPos].get_card(i).transform.Find("mark_die").gameObject.SetActive(false);
            player_hand_card_manager[enemyPos].get_card(i).transform.Find("mark_bomb").gameObject.SetActive(false);
            player_hand_card_manager[enemyPos].get_card(i).transform.Find("mark_mission").gameObject.SetActive(false);
            player_hand_card_manager[enemyPos].get_card(i).transform.Find("mark_shake").gameObject.SetActive(false);
        }
    }

    public void hide_mission_mark()
    {
        for (int i = 0; i < CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots.Count; i++)
        {
            CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i].transform.Find("mark_mission_1").gameObject.SetActive(false);
            CUIManager.Instance.get_uipage(UI_PAGE.POPUP_MISSION).GetComponent<CPopupMission>().slots[i].transform.Find("mark_mission_2").gameObject.SetActive(false);
        }
    }

    //힌트 갱신
    public void refresh_hint_mark()
    {
        hide_hint_mark();

        for (int i = 0; i < player_hand_card_manager[myPos].get_card_count(); i++)
        {
            CCardPicture card_picture = player_hand_card_manager[myPos].get_card(i);

            // 조커힌트 보여주기
            if (card_picture.card.number == 12)
            {
                card_picture.transform.Find("mark_first").gameObject.SetActive(true);
            }

            // 힌트 보여주기
            CVisualFloorSlot slot = floor_cards_slots.Find(obj => obj.is_same_card(card_picture.card));
            if (slot != null)
            {
                card_picture.transform.Find("mark_first").gameObject.SetActive(true);
            }

            // 굳은자 보여주기
            int _cnt = 0;
            // 내 카드 hand에서 같은카드 찾기
            for (int j = 0; j < player_hand_card_manager[myPos].get_card_count(); j++) if (card_picture.card.number == player_hand_card_manager[myPos].get_card(j).card.number) _cnt++;
            // 바닥 카드 floor에서 같은카드 찾기
            foreach (var obj in floor_cards_slots) if (card_picture.card.number == obj.card_number) _cnt += obj.get_card_count();
            // 내 카드 floor에서 같은카드 찾기
            _cnt += player_floor_card_manager[myPos].get_same_card_count(card_picture.card.number);
            // 상대방 카드 floor에서 같은카드 찾기
            _cnt += player_floor_card_manager[enemyPos].get_same_card_count(card_picture.card.number);
            // 4장이상 있으면 굳은자
            if (_cnt >= 4)
            {
                card_picture.transform.Find("mark_first").gameObject.SetActive(false);
                card_picture.transform.Find("mark_die").gameObject.SetActive(true);
            }

            // 폭탄 보여주기
            if (player_hand_card_manager[myPos].get_same_number_count(card_picture.card.number) == 3)
            {
                if (slot != null)
                {
                    card_picture.transform.Find("mark_bomb").gameObject.SetActive(true);
                }
                else
                {
                    // 1~12의 화투만 흔들 수 있다.
                    if (card_picture.card.number < 12)
                    {
                        card_picture.transform.Find("mark_first").gameObject.SetActive(false);
                        card_picture.transform.Find("mark_die").gameObject.SetActive(false);
                        card_picture.transform.Find("mark_shake").gameObject.SetActive(true);
                    }
                }
            }
            // 2장폭탄 보여주기
            else if (player_hand_card_manager[myPos].get_same_number_count(card_picture.card.number) == 2)
            {
                int tCnt = 0;
                foreach (var obj in floor_cards_slots) if (card_picture.card.number == obj.card_number) tCnt += obj.get_card_count();
                if (tCnt == 2)
                {
                    card_picture.transform.Find("mark_bomb").gameObject.SetActive(true);
                    card_picture.transform.Find("mark_first").gameObject.SetActive(false);
                    card_picture.transform.Find("mark_die").gameObject.SetActive(false);
                    card_picture.transform.Find("mark_shake").gameObject.SetActive(false);
                }
            }

            // 미션 보여주기
            for (int j = 0; j < ListMissionCard.Count; j++)
            {
                //Debug.LogError(card_picture.card.number+" "+ ListMissionCard[j].number+"/"+ card_picture.card.pae_type.ToString()+" "+ ListMissionCard[j].pae_type.ToString()+"/" + card_picture.card.position+" " + ListMissionCard[j].position);
                if (card_picture.card.number == ListMissionCard[j].number && card_picture.card.position == ListMissionCard[j].position)
                {
                    card_picture.transform.Find("mark_mission").gameObject.SetActive(true);
                }
            }
        }
    }
    #endregion Hint

    #region Keyboard
    public UserInputState keyState = UserInputState.NONE;

    public enum UserInputState { NONE, NORMAL, CHOICE, SHAKE, KOOGJIN, GOSTOP, PUSH, SUN, RESULT, CHONGTONG, THREEPPUK }
    IEnumerator GetKeyboradKey()
    {
        while (true)
        {
            yield return null;

            switch (keyState)
            {
                // 평범한 내턴의 상황
                case UserInputState.NORMAL:
                    // 유저가 누른 키보드의 번호를 저장할 변수
                    int GetKeyValue = -1;

                    // 유저가 선택한 인덱스를 저장할 변수
                    int GetKeyIndex = -1;

                    // 유저가 누른 키값
                    if (Input.GetKeyDown(KeyCode.Alpha1)) GetKeyValue = 1;
                    else if (Input.GetKeyDown(KeyCode.Alpha2)) GetKeyValue = 2;
                    else if (Input.GetKeyDown(KeyCode.Alpha3)) GetKeyValue = 3;
                    else if (Input.GetKeyDown(KeyCode.Alpha4)) GetKeyValue = 4;
                    else if (Input.GetKeyDown(KeyCode.Alpha5)) GetKeyValue = 5;
                    else if (Input.GetKeyDown(KeyCode.Alpha6)) GetKeyValue = 6;
                    else if (Input.GetKeyDown(KeyCode.Alpha7)) GetKeyValue = 7;
                    else if (Input.GetKeyDown(KeyCode.Alpha8)) GetKeyValue = 8;
                    else if (Input.GetKeyDown(KeyCode.Alpha9)) GetKeyValue = 9;
                    else if (Input.GetKeyDown(KeyCode.Alpha0)) GetKeyValue = 10;
                    else if (Input.GetKeyDown(KeyCode.Keypad1)) GetKeyValue = 1;
                    else if (Input.GetKeyDown(KeyCode.Keypad2)) GetKeyValue = 2;
                    else if (Input.GetKeyDown(KeyCode.Keypad3)) GetKeyValue = 3;
                    else if (Input.GetKeyDown(KeyCode.Keypad4)) GetKeyValue = 4;
                    else if (Input.GetKeyDown(KeyCode.Keypad5)) GetKeyValue = 5;
                    else if (Input.GetKeyDown(KeyCode.Keypad6)) GetKeyValue = 6;
                    else if (Input.GetKeyDown(KeyCode.Keypad7)) GetKeyValue = 7;
                    else if (Input.GetKeyDown(KeyCode.Keypad8)) GetKeyValue = 8;
                    else if (Input.GetKeyDown(KeyCode.Keypad9)) GetKeyValue = 9;
                    else if (Input.GetKeyDown(KeyCode.Keypad0)) GetKeyValue = 10;

                    // 유저가 가진 카드 갯수를 초과하는 번호를 누르면 무시해야한다
                    if (player_hand_card_manager[myPos].get_card_count() >= GetKeyValue && GetKeyValue != -1)
                    {
                        // 인덱스를 저장할거니까 -1을 해주어서 저장한다.
                        GetKeyIndex = GetKeyValue - 1;

                        // 카드 연속 터치등을 막기 위한 처리
                        card_collision_manager.enabled = false;
                        ef_focus.SetActive(false);

                        // 키보드 안받음
                        keyState = UserInputState.NONE;
                    }
                    // 아니면 처음으로 되돌아가기
                    else continue;

                    // 일단 카드를 받아온다.
                    CCardPicture card = player_hand_card_manager[myPos].get_card(GetKeyIndex);
                    if (card == null) Debug.LogError("GetKeyboardKey Error for card is null");

                    // 폭탄카드일 경우
                    if (card.card.number == 13)
                    {
                        //폭탄카드를 낸경우
                        CMessage newmsg = new CMessage();
                        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionFlipBomb;
                        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
                        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);

                        card.gameObject.SetActive(false);
                        player_hand_card_manager[myPos].remove(card);
                    }
                    // 일반카드일 경우
                    else
                    {
                        // 손에 같은 카드 3장이 있고 바닥에 같은카드가 없을 때 흔들기 팝업을 출력한다
                        int same_on_hand = player_hand_card_manager[myPos].get_same_number_count(card.card.number);
                        int same_on_floor = get_samenumbercount_on_floor(card.card);

                        // 같은카드가 3장이여서 흔들기를 진행하여야 할 경우
                        if (same_on_hand == 3 && same_on_floor == 0)
                        {
                            //흔듦 팝업
                            CUIManager.Instance.show(UI_PAGE.POPUP_ASK_SHAKING);
                            CPopupShaking popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_SHAKING).GetComponent<CPopupShaking>();
                            popup.refresh(card.card, card.slot);
                        }
                        // 흔들지 않고 바로 제출 가능할 경우
                        else
                        {
                            CPlayGameUI.send_select_card(card.card, card.slot, 0);
                        }
                    }
                    break;
                // 패를 2장중 한장을 선택해야 할때
                case UserInputState.CHOICE:
                    // 유저가 입력한 인덱스
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) CUIManager.Instance.get_uipage(UI_PAGE.POPUP_CHOICE_CARD).GetComponent<CPopupChoiceCard>().on_choice_card(0);
                    else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) CUIManager.Instance.get_uipage(UI_PAGE.POPUP_CHOICE_CARD).GetComponent<CPopupChoiceCard>().on_choice_card(1);
                    break;
                // 흔들때
                case UserInputState.SHAKE:
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_SHAKING).GetComponent<CPopupShaking>().on_choice_shaking(1);
                    else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_SHAKING).GetComponent<CPopupShaking>().on_choice_shaking(0);
                    break;
                // 국진일때
                case UserInputState.KOOGJIN:
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_KOOKJIN).GetComponent<CPopupKookjin>().on_choice_kookjin(1);
                    else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_KOOKJIN).GetComponent<CPopupKookjin>().on_choice_kookjin(0);
                    break;
                // 고스톱일때
                case UserInputState.GOSTOP:
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (CUIManager.Instance.get_uipage((UI_PAGE)(i + 3)).activeSelf)
                            {
                                CUIManager.Instance.get_uipage((UI_PAGE)(i + 3)).GetComponent<CPopupGoStop>().on_choice_go_or_stop(1);
                            }
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (CUIManager.Instance.get_uipage((UI_PAGE)(i + 3)).activeSelf)
                            {
                                CUIManager.Instance.get_uipage((UI_PAGE)(i + 3)).GetComponent<CPopupGoStop>().on_choice_go_or_stop(0);
                            }
                        }
                    }
                    break;
                // 게임이 끝난뒤 돈안받고 2배로 밀었을때
                case UserInputState.PUSH:
                    break;
            }
        }
    }
    #endregion

    #region Counter
    IEnumerator Counter()
    {
        float overTime = 11.0f;
        float time = 0;
        while (true)
        {
            yield return null;
            time += Time.deltaTime;

            switch (keyState)
            {
                // 아무차례도 아닐때
                case UserInputState.NONE:
                    // 시간초기화
                    time = 0;
                    CounterAnimOff();
                    break;
                // 평범한 내턴의 상황
                case UserInputState.NORMAL:
                    CounterAnim(time);
                    if (time >= overTime || (autoPlay.IsTurnOn && autoPlay.DelayTime <= time))
                    {
                        // 카드 연속 터치등을 막기 위한 처리
                        card_collision_manager.enabled = false;
                        ef_focus.SetActive(false);

                        // 키보드 안받음
                        keyState = UserInputState.NONE;

                        byte Choice = 0;
                        Choice = (byte)AutoPlayCard();
                        CCardPicture card = player_hand_card_manager[myPos].get_card(Choice);

                        if (card == null) Debug.LogError("GetKeyboardKey Error for card is null");

                        // 폭탄카드일 경우
                        if (card.card.number == 13)
                        {
                            //폭탄카드를 낸경우
                            CMessage newmsg = new CMessage();
                            ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionFlipBomb;
                            newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
                            NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);

                            card.gameObject.SetActive(false);
                            player_hand_card_manager[myPos].remove(card);
                        }
                        // 일반카드일 경우
                        else
                        {
                            // 손에 같은 카드 3장이 있고 바닥에 같은카드가 없을 때 흔들기 팝업을 출력한다
                            int same_on_hand = player_hand_card_manager[myPos].get_same_number_count(card.card.number);
                            int same_on_floor = get_samenumbercount_on_floor(card.card);

                            // 같은카드가 3장이여서 흔들기를 진행하여야 할 경우
                            if (same_on_hand == 3 && same_on_floor == 0)
                            {
                                //흔듦 팝업
                                CUIManager.Instance.show(UI_PAGE.POPUP_ASK_SHAKING);
                                CPopupShaking popup = CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_SHAKING).GetComponent<CPopupShaking>();
                                popup.refresh(card.card, card.slot);
                            }
                            // 흔들지 않고 바로 제출 가능할 경우
                            else
                            {
                                CPlayGameUI.send_select_card(card.card, card.slot, 0);
                            }
                        }

                        // 카드 낼때 시간 초과하면 자동치기, 나가기 예약
                        if (autoPlay.IsTurnOn == false)
                        {
                            autoPlay.TurnOn();

                            // 자동치기중이면 나가지 않음
                            if (CUIManager.Instance.get_uipage(UI_PAGE.NOTICE_PRACTICE).activeSelf == false)
                            {
                                CUIManager.Instance.ExitGame();
                            }
                        }
                    }
                    break;
                // 패를 2장중 한장을 선택해야 할때 (기본적으로 왼쪽 선택)
                case UserInputState.CHOICE:
                    CounterAnim(time);
                    if (time >= overTime || (autoPlay.IsTurnOn && autoPlay.DelayTime <= time))
                    {
                        byte Choice;
                        Choice = 0;

                        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_CHOICE_CARD).GetComponent<CPopupChoiceCard>().on_choice_card(Choice);
                    }
                    break;
                // 흔들때
                case UserInputState.SHAKE:
                    CounterAnim(time);
                    if (time >= overTime || (autoPlay.IsTurnOn && autoPlay.DelayTime <= time))
                    {
                        // 기본적으로 흔듦
                        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_SHAKING).GetComponent<CPopupShaking>().on_choice_shaking(1);
                    }
                    break;
                // 국진일때 (기본적으로 옮김)
                case UserInputState.KOOGJIN:
                    CounterAnim(time);
                    if (time >= overTime || (autoPlay.IsTurnOn && autoPlay.DelayTime <= time))
                    {
                        byte Choice;
                        Choice = 1;

                        CUIManager.Instance.get_uipage(UI_PAGE.POPUP_ASK_KOOKJIN).GetComponent<CPopupKookjin>().on_choice_kookjin(Choice);
                    }
                    break;
                // 고스톱일때 (기본적으로 스톱)
                case UserInputState.GOSTOP:
                    CounterAnim(time);
                    if (time >= overTime || (autoPlay.IsTurnOn && autoPlay.DelayTime <= time))
                    {
                        byte Choice;
                        Choice = 0;
                        for (int i = 0; i < 7; i++)
                        {
                            if (CUIManager.Instance.get_uipage((UI_PAGE)(i + 3)).activeSelf)
                            {
                                CUIManager.Instance.get_uipage((UI_PAGE)(i + 3)).GetComponent<CPopupGoStop>().on_choice_go_or_stop(Choice);
                            }
                        }
                    }
                    break;
                // 선결정할 패 고를때 ★ 카운터기능만 넣음
                case UserInputState.SUN:
                    if (time >= 7.0f)
                    {
                        while (true)
                        {
                            int randomIdx = Random.Range(0, 8);

                            if (player_me_index == 0) randomIdx = Random.Range(0, 4);
                            else if (player_me_index == 1) randomIdx = Random.Range(4, 8);

                            if (randomIdx == 0 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[0].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set0();
                                break;
                            }
                            else if (randomIdx == 1 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[1].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set1();
                                break;
                            }
                            else if (randomIdx == 2 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[2].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set2();
                                break;
                            }
                            else if (randomIdx == 3 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[3].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set3();
                                break;
                            }
                            else if (randomIdx == 4 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[4].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set4();
                                break;
                            }
                            else if (randomIdx == 5 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[5].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set5();
                                break;
                            }
                            else if (randomIdx == 6 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[6].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set6();
                                break;
                            }
                            else if (randomIdx == 7 && CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().slot[7].GetComponent<Button>().enabled)
                            {
                                CUIManager.Instance.get_uipage(UI_PAGE.POPUP_PLAYER_ORDER).GetComponent<CPopupPlayerOrder>().set7();
                                break;
                            }
                            yield return null;
                        }
                    }
                    break;

                // 나가기 예약했으면 레디 안함
                // 결과창 확인버튼 ★ 카운터기능만 넣음
                case UserInputState.RESULT:
                    if (time >= 7.0f)
                    {
                        keyState = UserInputState.NONE;
                        if (CUIManager.Instance.RoomMoveOutReserved() && isGaming == false)
                        {
                            CUIManager.Instance.ReadyToStartGame();
                        }
                    }
                    break;
                case UserInputState.CHONGTONG:
                    if (time >= 10.0f)
                    {
                        keyState = UserInputState.NONE;
                        if (CUIManager.Instance.RoomMoveOutReserved() && isGaming == false)
                        {
                            CUIManager.Instance.ReadyToStartGame();
                        }
                    }
                    break;
                case UserInputState.THREEPPUK:
                    if (time >= 10.0f)
                    {
                        keyState = UserInputState.NONE;
                        if (CUIManager.Instance.RoomMoveOutReserved() && isGaming == false)
                        {
                            CUIManager.Instance.ReadyToStartGame();
                        }
                    }
                    break;
                // 게임이 끝난뒤 돈안받고 2배로 밀었을때
                case UserInputState.PUSH:
                    break;
            }
        }
    }

    void CounterAnim(float time)
    {
        if (time >= 10)
        {
            counterBG.SetActive(true);
            counterBG.GetComponent<tk2dSpriteAnimator>().Play();
            counterNum.SetActive(true);
            counterNum.GetComponent<tk2dSprite>().SetSprite("01");
        }
        else if (time >= 9)
        {
            counterBG.SetActive(true);
            counterBG.GetComponent<tk2dSpriteAnimator>().Play();
            counterNum.SetActive(true);
            counterNum.GetComponent<tk2dSprite>().SetSprite("02");
        }
        else if (time >= 8)
        {
            counterBG.SetActive(true);
            counterBG.GetComponent<tk2dSpriteAnimator>().Play();
            counterNum.SetActive(true);
            counterNum.GetComponent<tk2dSprite>().SetSprite("03");
        }
        else if (time >= 7)
        {
            counterBG.SetActive(true);
            counterBG.GetComponent<tk2dSpriteAnimator>().Play();
            counterNum.SetActive(true);
            counterNum.GetComponent<tk2dSprite>().SetSprite("04");
        }
        else if (time >= 6)
        {
            counterBG.SetActive(true);
            counterBG.GetComponent<tk2dSpriteAnimator>().Play();
            counterNum.SetActive(true);
            counterNum.GetComponent<tk2dSprite>().SetSprite("05");
        }
    }

    void CounterAnimOff()
    {
        counterBG.SetActive(false);
        counterNum.SetActive(false);
    }

    /* 1. 보너스패 있으면 냄
     * 2. 맞는 바닥패 있으면 냄
     * 3. 굳은자 있으면 냄
     * 4. 폭탄패 있으면 냄
     * 5. 첫 손패부터 버림
     */
    int AutoPlayCard()
    {
        // 보너스패 확인
        var bonus2 = player_hand_card_manager[myPos].find_card(12, PAE_TYPE.PEE, 0);
        if (bonus2 != null)
        {
            return player_hand_card_manager[myPos].get_card_index(12, 0);
        }
        var bonus3 = player_hand_card_manager[myPos].find_card(12, PAE_TYPE.PEE, 1);
        if (bonus3 != null)
        {
            return player_hand_card_manager[myPos].get_card_index(12, 1);
        }

        // 맞는 바닥패 확인
        for (int i = 0; i < floor_cards_slots.Count; ++i)
        {
            var slots = floor_cards_slots[i].get_cards();

            for (int j = 0; j < slots.Count; ++j)
            {
                var result = player_hand_card_manager[myPos].find_card(slots[j].card.number);
                if (result != null)
                {
                    int _cnt = 0;
                    for (int k = 0; k < player_hand_card_manager[myPos].get_card_count(); k++) if (result.card.number == player_hand_card_manager[myPos].get_card(k).card.number) _cnt++;
                    foreach (var obj in floor_cards_slots) if (result.card.number == obj.card_number) _cnt += obj.get_card_count();
                    _cnt += player_floor_card_manager[myPos].get_same_card_count(result.card.number);
                    _cnt += player_floor_card_manager[enemyPos].get_same_card_count(result.card.number);
                    if (_cnt < 4)
                    {
                        return player_hand_card_manager[myPos].get_card_index(result.card.number, result.card.position);
                    }
                }
            }
        }

        // 굳은자 확인
        for (int i = 0; i < floor_cards_slots.Count; ++i)
        {
            var slots = floor_cards_slots[i].get_cards();

            for (int j = 0; j < slots.Count; ++j)
            {
                var result = player_hand_card_manager[myPos].find_card(slots[j].card.number);
                if (result != null)
                {
                    int _cnt = 0;
                    for (int k = 0; k < player_hand_card_manager[myPos].get_card_count(); k++) if (result.card.number == player_hand_card_manager[myPos].get_card(k).card.number) _cnt++;
                    foreach (var obj in floor_cards_slots) if (result.card.number == obj.card_number) _cnt += obj.get_card_count();
                    _cnt += player_floor_card_manager[myPos].get_same_card_count(result.card.number);
                    _cnt += player_floor_card_manager[enemyPos].get_same_card_count(result.card.number);
                    if (_cnt >= 4)
                    {
                        return player_hand_card_manager[myPos].get_card_index(result.card.number, result.card.position);
                    }
                }
            }
        }

        // 폭탄패 확인
        var bomb = player_hand_card_manager[myPos].find_card(13);
        if (bomb != null)
        {
            return player_hand_card_manager[myPos].get_card_index(bomb.card.number);
        }

        // 손에 들고 있는 첫패부터 버림
        return 0;
    }

    #endregion

    //------------------------------------------------------------------------------
    // static 매소드.
    public static void send_select_card(CCard card, byte slot, byte is_shaking)
    {
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionPutCard;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, card.number);
        Rmi.Marshaler.Write(newmsg, (byte)card.pae_type);
        Rmi.Marshaler.Write(newmsg, card.position);
        Rmi.Marshaler.Write(newmsg, slot);
        Rmi.Marshaler.Write(newmsg, is_shaking);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }
    //------------------------------------------------------------------------------

    public void SetVisible_Draw(bool bFlag)
    {
        m_objDraw.SetActive(bFlag);
    }
}