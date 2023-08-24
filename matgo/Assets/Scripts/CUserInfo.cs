using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CUserInfo : MonoBehaviour
{
    public GameObject Name;
    public GameObject Rate;
    public GameObject Money;
    public GameObject VarMoney;
    public GameObject Avatar;

    public void DeleteUserInfo()
    {
        Name.GetComponent<Text>().text = "";
        Rate.GetComponent<Text>().text = "";
        Money.GetComponent<Text>().text = "";
        VarMoney.GetComponent<Text>().text = "";
        Avatar.SetActive(false);
    }

    public void SetUserInfo(Rmi.Marshaler.UserInfo userInfo)
    {
        Name.GetComponent<Text>().text = userInfo.nickName;
        Rate.GetComponent<Text>().text = userInfo.win.ToString() + "승   " + userInfo.lose.ToString() + "패";
        Money.GetComponent<Text>().text = MoneyConvertToUnit(userInfo.money_game);
        VarMoney.GetComponent<Text>().text = MoneyConvertToUnit(userInfo.money_var, true);

        Avatar.SetActive(true);
        if (csAndroidManager.AvatarAsset != null && Avatar.GetComponent<RawImage>().texture == null)
        {
            try
            {
                Avatar.GetComponent<RawImage>().texture = (Texture)csAndroidManager.AvatarAsset.LoadAsset(userInfo.avatar);
            }
            catch (System.Exception e)
            {
                Avatar.GetComponent<RawImage>().texture = (Texture)csAndroidManager.AvatarAsset.LoadAsset("avatar_honggildong_default");
            }
        }
    }

    public void ResetUserInfo()
    {
        Name.GetComponent<Text>().text = "";
        Rate.GetComponent<Text>().text = "";
        Money.GetComponent<Text>().text = "";
        VarMoney.GetComponent<Text>().text = "";

        Avatar.SetActive(false);
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

        if (moneyStr != "") moneyStr += CPlayGameUI.Instance.moneyType == 1 ? "냥" : "냥";
        else if (moneyStr == "") moneyStr += "0" + (CPlayGameUI.Instance.moneyType == 1 ? "냥" : "냥");

        return moneyStr;
    }
}