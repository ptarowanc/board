using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserListManager : MonoBehaviour
{
    public static UserListManager Instance;

    List<Rmi.Marshaler.LobbyUserList> LobbyUserInfo = new List<Rmi.Marshaler.LobbyUserList>();

    public GameObject PrefabItem;

    public GameObject ViewPort;

    public List<GameObject> ListItem = new List<GameObject>();

    public int TabState = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetData(List<Rmi.Marshaler.LobbyUserList> lobbyUserInfo)
    {
        LobbyUserInfo = lobbyUserInfo; // 동기화 락 필요

        Refresh();
    }

    public void Refresh()
    {
        foreach (var g in ListItem) Destroy(g);
        ListItem.Clear();

        List<Rmi.Marshaler.LobbyUserList> tempData = new List<Rmi.Marshaler.LobbyUserList>();

        // 채널과 조건에 맞는 데이터를 찾아서 tempData에 넣는 작업
        if (TabState == 0)
        {
            for (int i = 0; i < LobbyUserInfo.Count; i++)
            {
                // 해당 채널의 전체 유저 찾는 과정
                if (LobbyUserInfo[i].chanID == ChannerManager.Instance.NowChannel)
                {
                    tempData.Add(LobbyUserInfo[i]);
                }
            }
        }
        else if (TabState == 1)
        {
            for (int i = 0; i < LobbyUserInfo.Count; i++)
            {
                // 해당 채널의 대기실 유저 찾는 과정
                if (LobbyUserInfo[i].chanID == ChannerManager.Instance.NowChannel && LobbyUserInfo[i].roomNumber == 0)
                {
                    tempData.Add(LobbyUserInfo[i]);
                }
            }
        }

        // 동적 추가 하는 부분
        int count = tempData.Count;
        int A = 200;
        float D = -33;
        float bottom = A + (count - 1) * D;

        ViewPort.GetComponent<RectTransform>().offsetMin = new Vector2(0, bottom); // new Vector2(left, bottom);
        ViewPort.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0); // new Vector2(-right, -top);

        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(PrefabItem);

            g.transform.SetParent(ViewPort.transform);

            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, ((-D / 2) * (count - 1)) + (i * D));

            ListItem.Add(g);

            g.transform.Find("NickName").gameObject.GetComponent<Text>().text = tempData[i].nickName;
            g.transform.Find("Money").gameObject.GetComponent<Text>().text = MoneyConvertToUnit(ChannerManager.Instance.NowChannel == 3 ? tempData[i].PayMoney : tempData[i].FreeMoney);
            if (tempData[i].roomNumber == 0) g.transform.Find("Location").gameObject.GetComponent<Text>().text = "대기실";
            else g.transform.Find("Location").gameObject.GetComponent<Text>().text = tempData[i].roomNumber.ToString() + "번방";
        }
    }

    public void SetTab0() { TabState = 0; Refresh(); }
    public void SetTab1() { TabState = 1; Refresh(); }
    public void SetTab2() { TabState = 2; Refresh(); }
    public void SetTab3() { TabState = 3; Refresh(); }
    public void SetTab4() { TabState = 4; Refresh(); }
    public void SetTab5() { TabState = 5; Refresh(); }
    public void SetTab6() { TabState = 6; Refresh(); }
    public void SetTab7() { TabState = 7; Refresh(); }

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

        if (moneyStr != "") moneyStr += (ChannerManager.Instance.NowChannel == 3 ? "냥" : "냥");

        return moneyStr;
    }
}