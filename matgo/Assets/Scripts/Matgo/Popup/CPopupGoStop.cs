using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ZNet;
using Vector3 = UnityEngine.Vector3;

public class CPopupGoStop : MonoBehaviour
{

    List<GameObject> listMoney = new List<GameObject>();

    void Awake()
    {
        transform.Find("button_go").GetComponent<Button>().onClick.AddListener(this.on_touch_01);
        transform.Find("button_stop").GetComponent<Button>().onClick.AddListener(this.on_touch_02);
    }

    void on_touch_01()
    {
        on_choice_go_or_stop(1);
    }

    void on_touch_02()
    {
        on_choice_go_or_stop(0);
    }

    public void on_choice_go_or_stop(byte is_go)
    {
        deleteMoney();

        // 키보드 안받음
        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.NONE;

        gameObject.SetActive(false);

        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameSelectGoStop;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, is_go);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }

    public void deleteMoney()
    {
        foreach (var obj in listMoney) Destroy(obj);
        listMoney.Clear();
    }

    public void ResetAll()
    {
        deleteMoney();
        gameObject.SetActive(false);
    }

    public void showMoney(long money, GameObject fontPrefab)
    {
        foreach (var obj in listMoney) Destroy(obj);
        listMoney.Clear();

        if(NetworkManager.Instance.Screen)
            MoneyConvertToUnit(money, fontPrefab, listMoney, 30, 50, 0, 25);
        else
            MoneyConvertToUnit(money, fontPrefab, listMoney, 30, 30, 0, 25);
    }

    void MoneyConvertToUnit(long money, GameObject fontPrefab, List<GameObject> listObject, float posX, float posY, float posZ, float posD)
    {
        string moneyStr = "";
        char[] moneyArray = new char[money.ToString().Length];
        moneyArray = money.ToString().ToCharArray();
        System.Array.Reverse(moneyArray);
        int unitCount = money.ToString().Length % 4 == 0 ? money.ToString().Length / 4 : money.ToString().Length / 4 + 1;
        long[] unitMoney = new long[unitCount];

        for (int i = 0; i < unitCount; i++)
        {
            string temp = "";
            for (int j = i * 4; j < i * 4 + 4; j++)
            {
                if (i == unitCount - 1 && j >= money.ToString().Length) break;
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
        }
        
        //if (CPlayGameUI.Instance.moneyType == 1) moneyStr += "냥";
        //else moneyStr += "냥";
        //if (CPlayGameUI.Instance.moneyType == 1) moneyStr += "s";
        //else moneyStr += "g";

        char[] c_str = new char[moneyStr.Length];
        c_str = moneyStr.ToCharArray();

        for (int i = 0; i < c_str.Length; i++)
        {
            GameObject g = Instantiate(fontPrefab);

            string BasicStr = "";

            if (c_str[i] == '만') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "1u");
            else if (c_str[i] == '억') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "2u");
            else if (c_str[i] == '조') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "3u");
            else if (c_str[i] == '0') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "00");
            else if (c_str[i] == '1') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "01");
            else if (c_str[i] == '2') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "02");
            else if (c_str[i] == '3') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "03");
            else if (c_str[i] == '4') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "04");
            else if (c_str[i] == '5') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "05");
            else if (c_str[i] == '6') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "06");
            else if (c_str[i] == '7') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "07");
            else if (c_str[i] == '8') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "08");
            else if (c_str[i] == '9') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "09");
            //else if (c_str[i] == '냥') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "nyang");
            //else if (c_str[i] == 's') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "silver");
            //else if (c_str[i] == 'g') g.GetComponent<tk2dSprite>().SetSprite(BasicStr + "gold");

            g.GetComponent<Renderer>().sortingLayerName = "popup";

            float addD = 0;
            //if (isJackpot) addD = 10;
            //else addD = 5;
            addD = 5;

            if (c_str[i] == '만' || c_str[i] == '억' || c_str[i] == '조' || c_str[i] == '냥') posX += addD;
            else if (i > 0) if (c_str[i - 1] == '만' || c_str[i - 1] == '억' || c_str[i - 1] == '조' || c_str[i - 1] == '냥') posX += addD;

            g.transform.position = new Vector3(posX + (i * posD) + ((posD / -2) * (c_str.Length - 1)), posY, posZ);

            listObject.Add(g);
        }
    }
}