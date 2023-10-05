using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannerManager : MonoBehaviour
{
    public static ChannerManager Instance;

    List<Rmi.Marshaler.RoomInfo> RoomList = new List<Rmi.Marshaler.RoomInfo>();

    public GameObject PrefabOpen;
    public GameObject PrefabClose;
    public GameObject PrefabItem;

    public GameObject ChannerMove;

    public GameObject ParentObject;

    List<GameObject> Room = new List<GameObject>();

    public Button[] ChannelTab;

    public int NowChannel = 5;

    //public GameObject MoneyType1Enter;
    //public GameObject MoneyType2Enter;
    //public GameObject MoneyType3Enter;
    //public GameObject MoneyType4Enter;
    public GameObject MoneyTypeFree1Enter;
    public GameObject MoneyTypeFree2Enter;
    public GameObject MoneyTypeFree3Enter;
    public GameObject MoneyTypeFree4Enter;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 게임 채널 조정
        // 무료, 유료 분리

        //if (NetworkManager.Instance.mStartType == CommonMatgo.StartType.Free)
        //{
        //    ChannelTab[0].gameObject.SetActive(false);
        //    ChannelTab[1].gameObject.SetActive(false);
        //    ChannelTab[2].gameObject.SetActive(false);
        //    ChannelTab[3].gameObject.SetActive(false);
        //    ChannelTab[4].gameObject.SetActive(true);
        //    ChannelTab[5].gameObject.SetActive(true);
        //    ChannelTab[6].gameObject.SetActive(true);
        //    ChannelTab[7].gameObject.SetActive(true);
        //}
        //else if (NetworkManager.Instance.mStartType == CommonMatgo.StartType.Paid)
        //{
        //    ChannelTab[0].gameObject.SetActive(true);
        //    ChannelTab[1].gameObject.SetActive(true);
        //    ChannelTab[2].gameObject.SetActive(true);
        //    ChannelTab[3].gameObject.SetActive(true);
        //    ChannelTab[4].gameObject.SetActive(false);
        //    ChannelTab[5].gameObject.SetActive(false);
        //    ChannelTab[6].gameObject.SetActive(false);
        //    ChannelTab[7].gameObject.SetActive(false);
        //}
    }

    public void SetData(List<Rmi.Marshaler.RoomInfo> roomList, int channel)
    {
        // 데이터 저장
        RoomList = roomList;
        NowChannel = channel;

        // 갱신
        Refresh();

        // 바로입장 버튼 갱신
        JoinSet();
    }

    void JoinSet()
    {
        MoneyTypeFree1Enter.SetActive(false);
        MoneyTypeFree2Enter.SetActive(false);
        MoneyTypeFree3Enter.SetActive(false);
        MoneyTypeFree4Enter.SetActive(false);
        switch (NowChannel)
        {
            case 5: MoneyTypeFree1Enter.SetActive(true); break;
            case 6: MoneyTypeFree2Enter.SetActive(true); break;
            case 7: MoneyTypeFree3Enter.SetActive(true); break;
            case 8: MoneyTypeFree4Enter.SetActive(true); break;
        }
    }

    void Refresh()
    {
        // 로딩 완료
        ChannerMove.SetActive(false);

        // 초기화
        foreach (var g in Room) DestroyObject(g);
        Room.Clear();

        // 현재 생성된 방
        int roomIndex = 0;

        // 생성하여야 할 방 개수
        int roomCount = 0;
        for (int i = 0; i < RoomList.Count; i++) if (RoomList[i].chanID == NowChannel) roomCount++;
        roomCount--;

        // 채널 탭 활성화 및 비활성화
        if (NowChannel > 0)
        {
            foreach (var chan in ChannelTab) { chan.interactable = true; }
            if (NowChannel - 5 > 0 && NowChannel - 5 < ChannelTab.Length)
                ChannelTab[NowChannel - 5].interactable = false;
        }

        // 생성
        for (int i = 0; i < RoomList.Count; i++)
        {
            // 현재 채널만 표시
            if (RoomList[i].chanID == NowChannel)
            {
                // Open 또는 Close 방 생성
                GameObject room;
                if (RoomList[i].restrict) room = Instantiate(PrefabClose);
                else room = Instantiate(PrefabOpen);
                room.SetActive(true);

                // 무료자유채널이면 버튼 활성화
                if (NowChannel == 4 || NowChannel == 8 || NowChannel == 7) room.GetComponent<Button>().interactable = true;
                else room.GetComponent<Button>().interactable = false;

                // 위치 조정
                ParentObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                ParentObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, -100 * roomCount);

                room.transform.SetParent(ParentObject.transform);
                room.transform.localScale = Vector3.one;

                room.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-100 * roomCount) + (roomIndex * -100));
                roomIndex++;

                // 방번호 생성
                RoomNumberCreate(room, RoomList[i].number);

                // 판돈 생성
                //var _bet = room.transform.Find("Bet").gameObject;
                //if (NowChannel > 4) _bet.GetComponent<Image>().sprite = CSpriteManager.Instance.GetSpriteByName("f_" + RoomList[i].stakeType);
                //else _bet.GetComponent<Image>().sprite = CSpriteManager.Instance.GetSpriteByName("c_1");

                // 자물쇠 생성
                var _lock = room.transform.Find("Lock").gameObject;
                if (RoomList[i].needPassword) _lock.GetComponent<Image>().sprite = CSpriteManager.Instance.GetSpriteByName("icon_lock");
                else _lock.GetComponent<Image>().sprite = CSpriteManager.Instance.GetSpriteByName("icon_unlock");

                // 방 정보 저장
                room.transform.GetComponent<RoomInfo>().roomNum = RoomList[i].number;
                room.transform.GetComponent<RoomInfo>().channel = RoomList[i].chanID;
                room.transform.GetComponent<RoomInfo>().bet = RoomList[i].stakeType;
                room.transform.GetComponent<RoomInfo>().isClose = RoomList[i].restrict;
                room.transform.GetComponent<RoomInfo>().isPW = RoomList[i].needPassword;

                // 판돈 생성
                room.transform.Find("BetText").GetComponent<Text>().text = room.transform.GetComponent<RoomInfo>().GetRoomBet(RoomList[i].chanID, RoomList[i].stakeType);

                // 리스트에 추가
                Room.Add(room);
            }
            else
            {
                continue;
            }
        }

        //switch (UserListManager.Instance.TabState)
        //{
        //    case 0: UserListManager.Instance.SetTab0(); break;
        //    case 1: UserListManager.Instance.SetTab1(); break;
        //    case 2: UserListManager.Instance.SetTab2(); break;
        //    case 3: UserListManager.Instance.SetTab3(); break;
        //    case 4: UserListManager.Instance.SetTab4(); break;
        //    case 5: UserListManager.Instance.SetTab5(); break;
        //    case 6: UserListManager.Instance.SetTab6(); break;
        //    case 7: UserListManager.Instance.SetTab7(); break;
        //}
    }

    public void SetChannel1()
    {
        NowChannel = NetworkManager.Instance.Channel = 1;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }

    public void SetChannel2()
    {
        NowChannel = NetworkManager.Instance.Channel = 2;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }

    public void SetChannel3()
    {
        NowChannel = NetworkManager.Instance.Channel = 3;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }

    public void SetChannel4()
    {
        NowChannel = NetworkManager.Instance.Channel = 4;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }
    public void SetChannel5()
    {
        NowChannel = NetworkManager.Instance.Channel = 5;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }
    public void SetChannel6()
    {
        NowChannel = NetworkManager.Instance.Channel = 6;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }
    public void SetChannel7()
    {
        NowChannel = NetworkManager.Instance.Channel = 7;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }
    public void SetChannel8()
    {
        NowChannel = NetworkManager.Instance.Channel = 8;
        NetworkManager.Instance.request_channel_move(NowChannel);
        Refresh();
        JoinSet();
    }

    Vector3 NumberScale = new Vector3(2, 2, 1);
    void RoomNumberCreate(GameObject parent, int number)
    {
        parent.transform.Find("NumberText").GetComponent<Text>().text = number.ToString();
    }
}