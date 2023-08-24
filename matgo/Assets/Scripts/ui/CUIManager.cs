using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using ZNet;

public enum UI_PAGE
{
    PLAY_ROOM,
    POPUP_PLAYER_ORDER,
    POPUP_CHOICE_CARD,
    POPUP_GO_STOP1,
    POPUP_GO_STOP2,
    POPUP_GO_STOP3,
    POPUP_GO_STOP4,
    POPUP_GO_STOP5,
    POPUP_GO_STOP6,
    POPUP_GO_STOP7,
    POPUP_ASK_SHAKING,
    POPUP_SHAKING_CARDS,
    POPUP_ASK_KOOKJIN,
    POPUP_GAME_RESULT,
    POPUP_GO_COUNT,
    POPUP_STOP,
    POPUP_FIRST_PLAYER,
    POPUP_MISSION,
    POPUP_CHONGTONG,
    BUTTON_PRACTICE,
    BUTTON_START,
    NOTICE_PRACTICE,
    GAME_OPTION,
    MAIN_MENU,
    CREDIT_BAR,
    STAGE_SELECT,
}

public class CUIManager : CSingletonMonobehaviour<CUIManager>
{
    public GameObject Exit;
    public GameObject ExitReserve;
    public GameObject Move;
    public GameObject MoveReserve;
    public GameObject RoomNumber;
    public GameObject RoomBet;

    public GameObject ppuk;
    public GameObject ppuk2;
    public GameObject ppuk3;

    public GameObject PrefabFont;

    bool isExit = false;
    bool isMove = false;

    Dictionary<UI_PAGE, GameObject> ui_objects;
    float m_fRoomMoveTime = 0.0f;

    void Awake()
    {
        this.ui_objects = new Dictionary<UI_PAGE, GameObject>();
        this.ui_objects.Add(UI_PAGE.POPUP_PLAYER_ORDER, transform.Find("popup_player_order").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_CHOICE_CARD, transform.Find("popup_choice_card").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP1, transform.Find("popup_gostop1").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP2, transform.Find("popup_gostop2").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP3, transform.Find("popup_gostop3").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP4, transform.Find("popup_gostop4").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP5, transform.Find("popup_gostop5").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP6, transform.Find("popup_gostop6").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP7, transform.Find("popup_gostop7").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_ASK_SHAKING, transform.Find("popup_shaking").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_SHAKING_CARDS, transform.Find("popup_shaking_cards").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_ASK_KOOKJIN, transform.Find("popup_kookjin").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_MISSION, transform.Find("popup_mission").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_CHONGTONG, transform.Find("popup_chongtong").gameObject);
        this.ui_objects.Add(UI_PAGE.POPUP_GAME_RESULT, transform.Find("popup_result").gameObject);

        this.ui_objects.Add(UI_PAGE.BUTTON_PRACTICE, transform.Find("button_practice").gameObject);
        this.ui_objects.Add(UI_PAGE.BUTTON_START, transform.Find("button_start").gameObject);

        this.ui_objects.Add(UI_PAGE.NOTICE_PRACTICE, transform.Find("notice_practice").gameObject);
    }


    public GameObject get_uipage(UI_PAGE page)
    {
        return this.ui_objects[page];
    }


    public void show(UI_PAGE page)
    {
        this.ui_objects[page].SetActive(true);
    }


    public void hide(UI_PAGE page)
    {
        this.ui_objects[page].SetActive(false);
    }

    public void ExitGame()
    {
        if (m_fRoomMoveTime > 1.0f)
        {
            m_fRoomMoveTime = 0.0f;
            // 나가기 예약으로 전환
            if (CPlayGameUI.Instance.isGaming)
            {
                Exit.SetActive(false);
                ExitReserve.SetActive(true);

                isExit = true;

                // 방이동 취소
                RoomMoveReserveCancel();
            }
            else
            {
                // 나가기
                if (isExit == false)
                {
                    isExit = true;
                    //NetworkManager.Instance.RoomOut();
                }
            }
        }
    }

    public void ExitGameReserveCancel()
    {
        isExit = false;
        Exit.SetActive(true);
        ExitReserve.SetActive(false);
    }

    public bool RoomMoveOutReserved()
    {
        return (isExit || isMove) == false;
    }

    public void RoomMove()
    {
        if (m_fRoomMoveTime > 1.0f)
        {
            m_fRoomMoveTime = 0.0f;
            // 나가기 예약으로 전환
            if (CPlayGameUI.Instance.isGaming)
            {
                //Move.SetActive(false);
                //MoveReserve.SetActive(true);
                isMove = true;

                // 나가기 취소
                ExitGameReserveCancel();
            }
            else
            {
                // 방이동
                if (isMove == false)
                {
                    isMove = true;
                    //NetworkManager.Instance.RoomMove();
                }
            }
        }
    }

    public void RoomMoveReserveCancel()
    {
        isMove = false;
        //Move.SetActive(true);
        //MoveReserve.SetActive(false);
    }

    private void Update()
    {
        if (isExit)
        {
            if (CPlayGameUI.Instance.isGaming)
            {
                if (NetworkManager.Instance.Channel == 8 || NetworkManager.Instance.Channel == 7)
                {
                    isExit = false;
                    NetworkManager.Instance.RoomOut();
                }
            }
            else
            {
                isExit = false;
                NetworkManager.Instance.RoomOut();
            }
        }
        //if (isExit && !CPlayGameUI.Instance.isGaming)
        //{
        //    isExit = false;
        //    NetworkManager.Instance.RoomOut();
        //}

        if (isMove && !CPlayGameUI.Instance.isGaming)
        {
            isMove = false;
            NetworkManager.Instance.RoomMove();
        }
        m_fRoomMoveTime += Time.deltaTime;
    }

    public void SetRoomInfo(int channel, int roomNumber, int money)
    {
        string _roomNum = "";
        string _roomBet = "";

        switch (channel)
        {
            case 1: _roomNum += "[초보채널 "; break;
            case 2: _roomNum += "[중수채널 "; break;
            case 3: _roomNum += "[고수채널 "; break;
            case 4: _roomNum += "[자유채널 "; break;
            case 5: _roomNum += "[일반1 "; break;
            case 6: _roomNum += "[일반2 "; break;
            case 7: _roomNum += "[자유채널 "; break;
            case 8: _roomNum += "[자유2채널 "; break;
            default: _roomNum += "[임시채널 "; break;
        }

        _roomNum += roomNumber.ToString() + "번방]";
        _roomBet += "점 " + MoneyConvertToUnit(money) + " 경기장";

        RoomNumber.GetComponent<Text>().text = _roomNum;
        RoomBet.GetComponent<Text>().text = _roomBet;
    }

    // 연습하기 버튼
    public void ReqPracticeGame()
    {
        get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
        CUIManager.Instance.ResetGoNStopAll();
        hide(UI_PAGE.BUTTON_PRACTICE);

        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GamePractice;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }
    // 시작하기 버튼
    public void ReadyToStartGame()
    {
        get_uipage(UI_PAGE.POPUP_GAME_RESULT).GetComponent<CPopupGameResult>().ResetAll();
        CUIManager.Instance.ResetGoNStopAll();
        hide(UI_PAGE.BUTTON_START);

        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameReady;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }

    string MoneyConvertToUnit(long money, bool isVar = false)
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

        if (money < 0) moneyStr += "-";
        else if (isVar && money > 0) moneyStr += "+";

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
            if (!last) moneyStr += " ";
        }

        if (moneyStr != "") moneyStr += (CPlayGameUI.Instance.moneyType == 1 ? "냥" : "냥");

        return moneyStr;
    }

    public IEnumerator ppukShow(byte num, long money)
    {
        if (num == 1) ppuk.SetActive(true);
        else if (num == 2) ppuk2.SetActive(true);
        else if (num == 3) ppuk3.SetActive(true);

        List<GameObject> listTemp = new List<GameObject>();

        yield return new WaitForSeconds(2.5f);

        foreach (var obj in listTemp) Destroy(obj);
        listTemp.Clear();

        if (num == 1) ppuk.SetActive(false);
        else if (num == 2) ppuk2.SetActive(false);
        else if (num == 3) ppuk3.SetActive(false);
    }

    public void ResetGoNStopAll()
    {
        this.ui_objects[UI_PAGE.POPUP_GO_STOP1].GetComponent<CPopupGoStop>().ResetAll();
        this.ui_objects[UI_PAGE.POPUP_GO_STOP2].GetComponent<CPopupGoStop>().ResetAll();
        this.ui_objects[UI_PAGE.POPUP_GO_STOP3].GetComponent<CPopupGoStop>().ResetAll();
        this.ui_objects[UI_PAGE.POPUP_GO_STOP4].GetComponent<CPopupGoStop>().ResetAll();
        this.ui_objects[UI_PAGE.POPUP_GO_STOP5].GetComponent<CPopupGoStop>().ResetAll();
        this.ui_objects[UI_PAGE.POPUP_GO_STOP6].GetComponent<CPopupGoStop>().ResetAll();
        this.ui_objects[UI_PAGE.POPUP_GO_STOP7].GetComponent<CPopupGoStop>().ResetAll();
    }

    public void SetRoomMoveTime(float fTime)
    {
        m_fRoomMoveTime = fTime;
    }
}