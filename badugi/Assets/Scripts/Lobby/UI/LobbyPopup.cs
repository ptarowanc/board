using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LOBBY_POPUP
{
    ROOM_MAKE,
    BANK,
    JOININFO,
    CONFIG
}
public class LobbyPopup : MonoBehaviour
{
    public static LobbyPopup Instance;

    public enum _eTab
    {
        ET_MILES = 0,
        ET_DEPOSIT,
        ET_WITHDRAWAL,

        ET_MAXCNT
    }

    [SerializeField]
    InputField m_inputMoney = null;

    [SerializeField]
    Text[] m_bankText = null;

    [SerializeField]
    Text[] m_bankText_paid = null;

    [SerializeField]
    Toggle[] m_tabButton = null;


    public bool isMakeRoom = false;
    public bool isJoinRoom = false;

    // 사운드설정
    public GameObject SoundON;
    public GameObject SoundOFF;

    // 입장옵션
    public InputField pw;
    public GameObject confirm;

    // 방만들기 옵션
    public GameObject MoneyTypeFree1;
    public GameObject[] MoneyTypeFreeBox1;
    public GameObject MoneyTypeFree2;
    public GameObject[] MoneyTypeFreeBox2;
    public GameObject MoneyTypeFree3;
    public GameObject[] MoneyTypeFreeBox3;
    public GameObject MoneyTypeFree4;
    public GameObject[] MoneyTypeFreeBox4;
    public GameObject PassWordMakeRoomLabel;
    public GameObject PassWordMakeRoom;

    // 금고 옵션
    //public GameObject DropdownType;
    public GameObject InputMoney;
    public GameObject TextMileage;
    public GameObject TextFreeMoney;
    public GameObject TextFreeBankMoney;
    public GameObject TextChargeMoney;
    public GameObject TextChargeBankMoney;

    public long Mileage;
    public long FreeMoney;
    public long ChargeMoney;
    public long FreeBankMoney;
    public long ChargeBankMoney;

    int m_iBankType = 0;

    Dictionary<LOBBY_POPUP, GameObject> ui_objects;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        m_inputMoney.onValueChanged.AddListener(delegate { BankMoneyEdit(); });

        m_iBankType = 0;

        if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Free)
        {
            TextFreeMoney.SetActive(true);
            TextFreeBankMoney.SetActive(true);
            TextChargeMoney.SetActive(false);
            TextChargeBankMoney.SetActive(false);

            //DropdownType.GetComponent<Dropdown>().options.Clear();
            //DropdownType.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("적립금 교환"));
            //DropdownType.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("보관하기"));
            //DropdownType.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("꺼내오기"));
        }
        else if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Paid)
        {
            TextFreeMoney.SetActive(false);
            TextFreeBankMoney.SetActive(false);
            TextChargeMoney.SetActive(true);
            TextChargeBankMoney.SetActive(true);

            //DropdownType.GetComponent<Dropdown>().options.Clear();
            //DropdownType.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("보관하기"));
            //DropdownType.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("꺼내오기"));
            //var t = DropdownType.GetComponent<Dropdown>();
            //DropdownType.GetComponent<Dropdown>().captionText.text = "보관하기";
        }

        ui_objects = new Dictionary<LOBBY_POPUP, GameObject>();

        ui_objects.Add(LOBBY_POPUP.ROOM_MAKE, transform.parent.Find("CanvasForTK2D").transform.Find("popup_room_make").gameObject);
        ui_objects.Add(LOBBY_POPUP.BANK, transform.parent.Find("CanvasForTK2D").transform.Find("popup_bank").gameObject);
        ui_objects.Add(LOBBY_POPUP.JOININFO, transform.parent.Find("CanvasForTK2D").transform.Find("popup_joinInfo").gameObject);
        ui_objects.Add(LOBBY_POPUP.CONFIG, transform.parent.Find("CanvasForTK2D").transform.Find("popup_config").gameObject);
    }

    private void Start()
    {
    }

    void Update()
    {
    }

    public GameObject get_uipage(LOBBY_POPUP page)
    {
        return ui_objects[page];
    }

    public void show(LOBBY_POPUP page)
    {
        ui_objects[page].SetActive(true);
    }

    public void hide(LOBBY_POPUP page)
    {
        ui_objects[page].SetActive(false);
    }
    public void ShowConfig()
    {
        NetworkManager.Instance.PopupLevelUp();
        show(LOBBY_POPUP.CONFIG);

        // 사운드
        if (SoundManager.Instance.isSound)
        {
            SoundON.SetActive(true);
            SoundOFF.SetActive(false);
        }
        else
        {
            SoundON.SetActive(false);
            SoundOFF.SetActive(true);
        }
    }

    public void ClickSoundON() { SoundON.SetActive(true); SoundOFF.SetActive(false); SoundManager.Instance.SoundOn(); }
    public void ClickSoundOFF() { SoundON.SetActive(false); SoundOFF.SetActive(true); SoundManager.Instance.SoundOff(); }

    public void HideConfig()
    {
        hide(LOBBY_POPUP.CONFIG);
        NetworkManager.Instance.PopupLevelDown();
    }

    public void ShowMakeRoomPopup()
    {
        NetworkManager.Instance.PopupLevelUp();
        show(LOBBY_POPUP.ROOM_MAKE);
        hide(LOBBY_POPUP.BANK);

        MoneyTypeFree1.SetActive(false);
        MoneyTypeFree2.SetActive(false);
        MoneyTypeFree3.SetActive(false);
        MoneyTypeFree4.SetActive(false);

        switch (ChannerManager.Instance.NowChannel)
        {
            case 5: MoneyTypeFree1.SetActive(true); break;
            case 6: MoneyTypeFree2.SetActive(true); break;
            case 7: MoneyTypeFree3.SetActive(true); break;
            case 8: MoneyTypeFree4.SetActive(true); break;
        }

        if (ChannerManager.Instance.NowChannel == 3 || ChannerManager.Instance.NowChannel == 4 || ChannerManager.Instance.NowChannel == 7 || ChannerManager.Instance.NowChannel == 8)
        {
            PassWordMakeRoomLabel.SetActive(true);
            PassWordMakeRoom.SetActive(true);
        }
        else
        {
            PassWordMakeRoomLabel.SetActive(false);
            PassWordMakeRoom.SetActive(false);
        }
    }
    public void MakeRoom()
    {
        if (!isMakeRoom)
        {
            NetworkManager.Instance.PopupLevelDown();
            hide(LOBBY_POPUP.ROOM_MAKE);
            hide(LOBBY_POPUP.BANK);

            isMakeRoom = true;

            int idx = 1;
            switch (ChannerManager.Instance.NowChannel)
            {
                case 5: for (int i = 0; i < MoneyTypeFreeBox1.Length; ++i) { if (MoneyTypeFreeBox1[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;
                case 6: for (int i = 0; i < MoneyTypeFreeBox2.Length; ++i) { if (MoneyTypeFreeBox2[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;
                case 7: for (int i = 0; i < MoneyTypeFreeBox3.Length; ++i) { if (MoneyTypeFreeBox3[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;
                case 8: for (int i = 0; i < MoneyTypeFreeBox4.Length; ++i) { if (MoneyTypeFreeBox4[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;

            }

            NetworkManager.Instance.MakeOption = idx;

            if (ChannerManager.Instance.NowChannel == 3 || ChannerManager.Instance.NowChannel == 4 || ChannerManager.Instance.NowChannel == 7 || ChannerManager.Instance.NowChannel == 8) NetworkManager.Instance.MakeRoom(PassWordMakeRoom.GetComponent<InputField>().text);
            else NetworkManager.Instance.MakeRoom();
        }
    }
    public void JoinRoom(int option)
    {
        if (!isJoinRoom)
        {
            isJoinRoom = true;
            NetworkManager.Instance.JoinRoom(option);
        }
    }

    public void GameOut()
    {
        Application.Quit();
    }

    public void CancleMakeRoom()
    {
        NetworkManager.Instance.PopupLevelDown();
        hide(LOBBY_POPUP.ROOM_MAKE);
        hide(LOBBY_POPUP.BANK);
    }

    public void StartSetting()
    {
        NetworkManager.Instance.PopupLevelUp();
        hide(LOBBY_POPUP.ROOM_MAKE);
        hide(LOBBY_POPUP.BANK);

        MoneyTypeFree1.SetActive(false);
        MoneyTypeFree2.SetActive(false);
        MoneyTypeFree3.SetActive(false);
        MoneyTypeFree4.SetActive(false);

        switch (ChannerManager.Instance.NowChannel)
        {
            case 5: MoneyTypeFree1.SetActive(true); break;
            case 6: MoneyTypeFree2.SetActive(true); break;
            case 7: MoneyTypeFree3.SetActive(true); break;
            case 8: MoneyTypeFree4.SetActive(true); break;
        }
    }

    public void EndSetting()
    {
        NetworkManager.Instance.PopupLevelDown();
        hide(LOBBY_POPUP.ROOM_MAKE);
        hide(LOBBY_POPUP.BANK);

        int idx = 1;
        switch (ChannerManager.Instance.NowChannel)
        {
            case 5: for (int i = 0; i < MoneyTypeFreeBox1.Length; ++i) { if (MoneyTypeFreeBox1[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;
            case 6: for (int i = 0; i < MoneyTypeFreeBox2.Length; ++i) { if (MoneyTypeFreeBox2[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;
            case 7: for (int i = 0; i < MoneyTypeFreeBox3.Length; ++i) { if (MoneyTypeFreeBox3[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;
            case 8: for (int i = 0; i < MoneyTypeFreeBox4.Length; ++i) { if (MoneyTypeFreeBox4[i].GetComponent<Toggle>().isOn) idx = i + 1; } break;

        }

        NetworkManager.Instance.JoinOption = idx;
    }

    public void StartBank()
    {
        NetworkManager.Instance.PopupLevelUp();
        hide(LOBBY_POPUP.ROOM_MAKE);
        show(LOBBY_POPUP.BANK);

        m_inputMoney.text = "";

        SelectTab(0);
        m_tabButton[0].isOn = true;

        InputMoney.GetComponent<Text>().text = "";
        TextMileage.GetComponent<Text>().text = "적립금 : " + MoneyConvertToUnit(Mileage) + "점";
        TextFreeMoney.GetComponent<Text>().text = "보유중 : " + MoneyConvertToUnit(FreeMoney) + "냥";
        TextFreeBankMoney.GetComponent<Text>().text = "보관중 : " + MoneyConvertToUnit(FreeBankMoney) + "냥";
        //TextChargeMoney.GetComponent<Text>().text = MoneyConvertToUnit(ChargeMoney) + " 원 보유중";
        //TextChargeBankMoney.GetComponent<Text>().text = MoneyConvertToUnit(ChargeBankMoney) + " 원 보관중
    }

    public void UpdateInput()
    {
        InputMoney.GetComponent<Text>().text = "";
    }

    public void UpdateBank()
    {
        //InputMoney.GetComponent<Text>().text = "";
        TextMileage.GetComponent<Text>().text = "적립금 : " + MoneyConvertToUnit(Mileage) + "점";
        TextFreeMoney.GetComponent<Text>().text = "보유중 : " + MoneyConvertToUnit(FreeMoney) + "냥";
        TextFreeBankMoney.GetComponent<Text>().text = "보관중 : " + MoneyConvertToUnit(FreeBankMoney) + "냥";
        //TextChargeMoney.GetComponent<Text>().text = MoneyConvertToUnit(ChargeMoney) + " 원 보유중";
        //TextChargeBankMoney.GetComponent<Text>().text = MoneyConvertToUnit(ChargeBankMoney) + " 원 보관중
    }

    public void EndBank()
    {
        if (InputMoney.GetComponent<Text>().text == "") return;
        //NetworkManager.Instance.Bank(DropdownType.GetComponent<Dropdown>().value + 1, long.Parse(InputMoney.GetComponent<Text>().text.ToString()), "");
        NetworkManager.Instance.Bank(m_iBankType + 1, long.Parse(InputMoney.GetComponent<Text>().text.ToString()), "");
    }

    public void HideBank()
    {
        NetworkManager.Instance.PopupLevelDown();
        hide(LOBBY_POPUP.ROOM_MAKE);
        hide(LOBBY_POPUP.BANK);
    }

    public void CancelBank()
    {
        NetworkManager.Instance.PopupLevelDown();
        hide(LOBBY_POPUP.ROOM_MAKE);
        hide(LOBBY_POPUP.BANK);
    }

    // 17.11.30
    public void StartJoinInfo(bool isClose, bool isPW, int ChannelID, int RoomNumber)
    {
        if (isClose)
        {
            NetworkManager.Instance.PopupLevelUp();
            NetworkManager.Instance.PopupMessage.SetActive(true);
            NetworkManager.Instance.PopupMessage.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "방에 입장할 수 없습니다.";
            return;
        }

        if (!isPW)
        {
            NetworkManager.Instance.JoinRoom2(ChannelID, RoomNumber, "");
            return;
        }

        NetworkManager.Instance.PopupLevelUp();
        hide(LOBBY_POPUP.ROOM_MAKE);
        hide(LOBBY_POPUP.BANK);
        show(LOBBY_POPUP.JOININFO);

        confirm.GetComponent<Confim>().roomNum = RoomNumber;
    }
    public void ConfirmJoinInfo(int ChannelID, int RoomNumber, string PassWord)
    {
        NetworkManager.Instance.PopupLevelDown();
        hide(LOBBY_POPUP.JOININFO);

        NetworkManager.Instance.JoinRoom2(ChannelID, RoomNumber, PassWord);
    }
    public void CancelJoinInfo()
    {
        NetworkManager.Instance.PopupLevelDown();
        hide(LOBBY_POPUP.JOININFO);
    }

    public void ShopButton()
    {
#if UNITY_ANDROID
        csAndroidManager.Instance.GoToStore();
#else
        Application.OpenURL("http://www.vongvong.co.kr/");
#endif

    }
    public void MyRoomButton()
    {
        csAndroidManager.Instance.GoToMyRoom();
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
            if (!last) moneyStr += " ";
        }

        if (moneyStr == "") moneyStr = "0";

        return moneyStr;
    }


    public void BankMoneyEdit()
    {
        long iMaxCheckMoney = 0;
        if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Free)
        {
            if (m_iBankType == 0)
                iMaxCheckMoney = Mileage;
            else if (m_iBankType == 1)
                iMaxCheckMoney = FreeMoney;
            else if (m_iBankType == 2)
                iMaxCheckMoney = FreeBankMoney;
        }
        else if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Paid)
        {
            if (m_iBankType == 0)
                iMaxCheckMoney = Mileage;
            else if (m_iBankType == 1)
                iMaxCheckMoney = ChargeMoney;
            else if (m_iBankType == 2)
                iMaxCheckMoney = ChargeBankMoney;
        }

        long iCurrentMoney = 0;

        if (m_inputMoney.text != "")
            iCurrentMoney = long.Parse(m_inputMoney.text.ToString());

        if (iCurrentMoney > iMaxCheckMoney)
            iCurrentMoney = iMaxCheckMoney;

        m_inputMoney.text = iCurrentMoney.ToString();
    }

    public void SelectTab(int iTabIIndex)
    {
        m_iBankType = iTabIIndex;
        m_inputMoney.text = "";
        if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Free)
        {
            for (int i = 0; i < m_bankText.Length; ++i)
                m_bankText[i].color = Color.white;

            m_bankText[iTabIIndex].color = Color.green;
        }
        else if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Paid)
        {
            for (int i = 0; i < m_bankText_paid.Length; ++i)
                m_bankText_paid[i].color = Color.white;

            m_bankText_paid[iTabIIndex].color = Color.green;
        }
    }

    public void AddMoney(int iMoney)
    {
        long iMaxCheckMoney = 0;
        if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Free)
        {
            if (m_iBankType == 0)
                iMaxCheckMoney = Mileage;
            else if (m_iBankType == 1)
                iMaxCheckMoney = FreeMoney;
            else if (m_iBankType == 2)
                iMaxCheckMoney = FreeBankMoney;
        }
        else if (NetworkManager.Instance.mStartType == CommonBadugi.StartType.Paid)
        {
            if (m_iBankType == 0)
                iMaxCheckMoney = Mileage;
            else if (m_iBankType == 1)
                iMaxCheckMoney = ChargeMoney;
            else if (m_iBankType == 2)
                iMaxCheckMoney = ChargeBankMoney;
        }

        long iCurrentMoney = 0;

        if (m_inputMoney.text != "")
            iCurrentMoney = long.Parse(m_inputMoney.text.ToString());

        iCurrentMoney += iMoney;

        if (iCurrentMoney > iMaxCheckMoney)
            iCurrentMoney = iMaxCheckMoney;

        if (iMoney == -1)
            iCurrentMoney = iMaxCheckMoney;

        m_inputMoney.text = iCurrentMoney.ToString();
    }
}