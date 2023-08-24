using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyJackpotManager : MonoBehaviour
{
    public static LobbyJackpotManager Instance;

    public Canvas childCavas;

    public GameObject PrefabFont;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    long BeforeJackpotMoney = 0;
    long NowJackpotMoney = 0;
    [HideInInspector]
    public long JackpotMoney = 0;
    private void Update()
    {
        if (JackpotMoney != NowJackpotMoney)
        {
            long unitMoney = (JackpotMoney - BeforeJackpotMoney) / 300;

            if (JackpotMoney - BeforeJackpotMoney < 300) unitMoney = JackpotMoney - BeforeJackpotMoney;

            if (unitMoney > 0)
            {
                if (NowJackpotMoney < JackpotMoney)
                {
                    NowJackpotMoney += unitMoney;

                    JackpotRefresh(NowJackpotMoney);

                    if (NowJackpotMoney > JackpotMoney)
                    {
                        BeforeJackpotMoney = NowJackpotMoney = JackpotMoney;
                        JackpotRefresh(JackpotMoney);
                    }
                }
            }
            else if (unitMoney < 0)
            {
                if (NowJackpotMoney > JackpotMoney)
                {
                    NowJackpotMoney += unitMoney;

                    JackpotRefresh(NowJackpotMoney);

                    if (NowJackpotMoney < JackpotMoney)
                    {
                        BeforeJackpotMoney = NowJackpotMoney = JackpotMoney;
                        JackpotRefresh(JackpotMoney);
                    }
                }
            }
        }
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
        float numberX = 680;
        float numberY = 495;
        float numberZ = 0;
        float numberD = -20;

        float dotY = 495;
        float dotZ = 0;
        float dotD = -15;

        numberX += ((num.Length - 1) * 10);

        int dotCount = num.Length % 3 > 0 ? num.Length / 3 : num.Length / 3 - 1;

        numberX += dotCount * -10;

        for (int i = 0; i < _num.Length; i++)
        {
            // 3자리가 넘어가면 dot 생성
            if (i % 3 == 0 && i != 0)
            {
                GameObject g = Instantiate(PrefabFont);

                if (childCavas != null)
                {
                    g.transform.SetParent(childCavas.transform);
                    g.transform.localScale = Vector3.one;
                }

                g.GetComponent<tk2dSprite>().SetSprite("jackpot_dot");

                numberX += dotD;

                float posX = 0;

                posX = numberX + (i - 1) * numberD;

                g.transform.localPosition = new Vector3(posX, dotY, dotZ);

                ListJackpotNumberObject.Add(g);
            }

            {
                GameObject g = Instantiate(PrefabFont);
                if (childCavas != null)
                {
                    g.transform.SetParent(childCavas.transform);
                    g.transform.localScale = Vector3.one;
                }

                g.GetComponent<tk2dSprite>().SetSprite("jackpot_" + _num[i].ToString());

                float posX = 0;

                if (i % 3 == 0 && i != 0) numberX -= numberD - dotD;

                posX = numberX + i * numberD;

                g.transform.localPosition = new Vector3(posX, numberY, numberZ);

                ListJackpotNumberObject.Add(g);
            }
        }
    }
}