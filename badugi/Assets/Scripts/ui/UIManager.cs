using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public enum UI_PAGE_BADUK
{
    BOSSGAMESTART,
    WAITGAMESTART,
    BETTING,
    CHANGECARD
}
public class UIManager : CSingletonMonobehaviour<UIManager>
{
    public GameObject RoomNumber;
    public GameObject RoomBet;
    public GameObject Move;
    public GameObject MoveReserve;
    public GameObject Exit;
    public GameObject ExitReserve;

    public GameObject Button_Start;
    public GameObject Animation_Wait;
    public GameObject Button_Betting;
    public GameObject Button_Cutting;

    [SerializeField]
    Image m_imgRoomMove;

    [SerializeField]
    Image m_imgRoomExit;

    [SerializeField]
    Button m_btnRoomMove;

    [SerializeField]
    Button m_btnRoomExit;


    public Text report;

    bool isExit = false;
    bool isMove = false;

    int m_iRoomMoney = 100;
    float m_fRoomMoveTime = 0.0f;

    Dictionary<UI_PAGE_BADUK, GameObject> ui_objects;
    void Awake()
    {
        ui_objects = new Dictionary<UI_PAGE_BADUK, GameObject>();
        ui_objects.Add(UI_PAGE_BADUK.BOSSGAMESTART, Button_Start);
        ui_objects.Add(UI_PAGE_BADUK.WAITGAMESTART, Animation_Wait);
        ui_objects.Add(UI_PAGE_BADUK.BETTING, Button_Betting);
        ui_objects.Add(UI_PAGE_BADUK.CHANGECARD, Button_Cutting);
    }

    public GameObject get_uipage(UI_PAGE_BADUK page)
    {
        return ui_objects[page];
    }


    public void show(UI_PAGE_BADUK page)
    {
        ui_objects[page].SetActive(true);
    }


    public void hide(UI_PAGE_BADUK page)
    {
        ui_objects[page].SetActive(false);
    }

    public IEnumerator delay_if_exist(byte delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
    }

    public void ButtonExitGame()
    {
        PlayGameUI.Instance.isCounterOver = false;
        ExitGame();

    }

    public void ExitGame()
    {
        // 나가기 예약으로 전환
        if (PlayGameUI.Instance.isGaming)
        {
            Exit.SetActive(false);
            ExitReserve.SetActive(true);
            PlayGameUI.Instance.player_info_slots[0].SetReservationIcon(true);
            NetworkManager.Instance.RoomOutRsvn(true);
            isExit = true;

            // 방이동 취소
            RoomMoveReserveReset();
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

    public void ExitGameReserveCancel()
    {
        NetworkManager.Instance.RoomOutRsvn(false);
        isExit = false;
        Exit.SetActive(true);
        ExitReserve.SetActive(false);
    }
    public void ExitGameReset()
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
            if (PlayGameUI.Instance.isGaming)
            {
                //m_btnRoomMove.enabled = false;
                Move.SetActive(false);
                MoveReserve.SetActive(true);

                NetworkManager.Instance.RoomOutRsvn(true);
                isMove = true;

                // 나가기 취소
                ExitGameReset();
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
        NetworkManager.Instance.RoomOutRsvn(false);
        isMove = false;
        Move.SetActive(true);
        MoveReserve.SetActive(false);
    }
    public void RoomMoveReserveReset()
    {
        isMove = false;
        Move.SetActive(true);
        MoveReserve.SetActive(false);
    }

    private void Update()
    {
        if (isExit && !PlayGameUI.Instance.isGaming && !PlayGameUI.Instance.isCounterOver)
        {
            isExit = false;
            NetworkManager.Instance.RoomOut();
        }

        if (isMove && !PlayGameUI.Instance.isGaming)
        {
            isMove = false;
            NetworkManager.Instance.RoomMove();
        }
        m_fRoomMoveTime += Time.deltaTime;
        if (m_fRoomMoveTime < 1.0f)
        {
            Color col = Color.white;
            col.a = 0.5f;
            m_imgRoomMove.color = col;
            m_btnRoomMove.enabled = false;
        }
        else
        {
            Color col = Color.white;
            col.a = 1.0f;
            m_imgRoomMove.color = col;
            m_btnRoomMove.enabled = true;
        }
    }

    public void SetRoomInfo(int channel, int roomNumber, int money)
    {
        string _roomNum = "";
        string _roomBet = "";

        m_iRoomMoney = money;

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

        if (moneyStr != "") moneyStr += (PlayGameUI.Instance.moneyType == 1 ? "냥" : "냥");

        return moneyStr;
    }

    public void ReportReset(byte count, string[] nick, string[] cards, long[] changeMoney)
    {
        //char[] temp = new char[report.text.Length];
        //temp = report.text.ToCharArray();

        //string strTemp = "";

        //for (int i = 0, nullCount = 0; i < temp.Length; i++)
        //{
        //    if (temp[i] == '\n') nullCount++;
        //    if (nullCount >= 30) break;

        //    strTemp += temp[i].ToString();
        //}

        if (report.text.Split('\n').Length >= 100) report.text = report.text.Split(System.Environment.NewLine.ToCharArray(), 100 + 1).Skip(100).FirstOrDefault();

        //report.text += "------------------------------------\n";

        for (byte i = 0; i < count; i++)
        {
            if (changeMoney[i] > 0)
            {
                report.text += "<color=yellow>▶ " + nick[i] + " : " + cards[i] + "</color>\n";
                //report.text += "<color=yellow>▶ " + nick[i] + " : " + cards[i] + "\n</color>";
            }
            else
                report.text += "▶ " + nick[i] + " : " + cards[i] + "\n";
        }
        //report.text += dealMoney + "\n";
    }

    public void ReportText(string Text)
    {
        report.text += Text + "\n";
    }

    public void ReportAllReset()
    {
        report.text = "";
    }

    public int GetRoomMoney()
    {
        return m_iRoomMoney;
    }

    public void SetRoomMoveTime(float fTime)
    {
        m_fRoomMoveTime = fTime;
    }
}