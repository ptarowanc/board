using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class TotalMoneyManager : MonoBehaviour
{
    public GameObject[] Chips;
    public Transform[] Chippos;
    SpriteRenderer[,] chips2DArray;

    int maxMoneyUnit = 16;
    int numberUnit = 9;

    void Awake()
    {
        chips2DArray = new SpriteRenderer[maxMoneyUnit, numberUnit];

        // 조의 단위인 최대 16자리까지 표현 가능하다. maxMoneyUnit에 영향을 주며, 씬에서도 Chippos의 갯수를 늘려줘야함
        long money = 9999999999999999;
        string MoneyString = new string(money.ToString().ToCharArray().Reverse().ToArray());
        for (int i = 0; i < MoneyString.Length; ++i)
        {
            int count = Int32.Parse(MoneyString[i].ToString());
            for (int c = 0; c < count; ++c)
            {
                Vector3 pos = Chippos[i].position;
                pos.y = pos.y + c * 5;

                GameObject chip = Instantiate(Chips[i >= Chips.Length ? Chips.Length - 1 : i], pos, Quaternion.identity, this.transform) as GameObject;
                chip.GetComponent<SpriteRenderer>().sortingOrder = c - 10;
                chip.SetActive(false);
                chips2DArray[i, c] = chip.GetComponent<SpriteRenderer>();
            }
        }
    }

    public void SetTotalMoney(long money)
    {
        return;
        if (money == 0)
        {
            for (int i = 0; i < maxMoneyUnit; ++i) for (int j = 0; j < numberUnit; ++j)
                    chips2DArray[i, j].
                        gameObject.
                        SetActive(false);
            return;
        }

        string MoneyString = new string(money.ToString().ToCharArray().Reverse().ToArray());
        for (int i = 0; i < maxMoneyUnit; ++i)
        {
            for (int j = 0; j < numberUnit; ++j)
            {
                chips2DArray[i, j].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < MoneyString.Length; ++i)
        {
            int count = Int32.Parse(MoneyString[i].ToString());

            for (int c = 0; c < count; ++c)
            {
                chips2DArray[i, c].gameObject.SetActive(true);
            }
        }
    }
}