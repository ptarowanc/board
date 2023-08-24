using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

public class ChipAniManager : MonoBehaviour
{
    public GameObject[] Chips;
    List<GameObject> moveChips;

    [SerializeField]
    Sprite[] m_sprChip1;
    [SerializeField]
    Sprite[] m_sprChip2;
    [SerializeField]
    Sprite[] m_sprChip3;
    [SerializeField]
    Sprite[] m_sprChip4;
    [SerializeField]
    Sprite[] m_sprChip5;
    [SerializeField]
    Sprite[] m_sprChip6;
    [SerializeField]
    Sprite[] m_sprChip7;
    [SerializeField]
    Sprite[] m_sprChip8;
    [SerializeField]
    Sprite[] m_sprChip9;
    [SerializeField]
    Sprite[] m_sprChip10;
    [SerializeField]
    Sprite[] m_sprChip11;
    [SerializeField]
    Sprite[] m_sprChip12;
    [SerializeField]
    Sprite[] m_sprChip13;

    float radiusx = 150f;
    float radiusy = 70f;

    Vector3 originPoint;

    ArrayList m_aryChipList = new ArrayList();
    ArrayList m_aryChipList2 = new ArrayList();

    private void Awake()
    {
        m_aryChipList.Add(m_sprChip1);
        //m_aryChipList.Add(m_sprChip2);
        m_aryChipList.Add(m_sprChip3);
        m_aryChipList.Add(m_sprChip4);
        //m_aryChipList.Add(m_sprChip5);
        m_aryChipList.Add(m_sprChip6);
        m_aryChipList.Add(m_sprChip7);
        //m_aryChipList.Add(m_sprChip8);
        m_aryChipList.Add(m_sprChip9);
        //m_aryChipList.Add(m_sprChip10);
        m_aryChipList.Add(m_sprChip11);
        m_aryChipList.Add(m_sprChip12);
        m_aryChipList.Add(m_sprChip13);

        m_aryChipList2.Add(m_sprChip2);
        m_aryChipList2.Add(m_sprChip2);
        m_aryChipList2.Add(m_sprChip5);
        m_aryChipList2.Add(m_sprChip5);
        m_aryChipList2.Add(m_sprChip8);
        m_aryChipList2.Add(m_sprChip10);
    }

    void Start ()
    {
        originPoint = new Vector3(0, 300, 0);
    }
	
    public void ChipCreate(long money)
    {
        StartCoroutine(CreatChipMoney(money));
    }

    //IEnumerator CreatChipMoney(long money)
    //{
    //    string MoneyString = new string(money.ToString().ToCharArray().Reverse().ToArray());
    //    moveChips = new List<GameObject>();
    //    for (int i = 0; i < MoneyString.Length; ++i)
    //    {
    //        int count = Int32.Parse(MoneyString[i].ToString());

    //        for (int c = 0; c < count; ++c)
    //        {
    //            GameObject chip = Instantiate(Chips[i >= Chips.Length ? Chips.Length - 1 : i], transform.position, Quaternion.identity, this.transform) as GameObject;
    //            chip.SetActive(true);
    //            moveChips.Add(chip);
    //        }
    //    }

    //    for (int i = 0; i < moveChips.Count; ++i)
    //    {
    //        Vector3 target = new Vector3();
    //        target.x = originPoint.x + UnityEngine.Random.Range(-radius, radius);
    //        target.y = originPoint.y + UnityEngine.Random.Range(-radius, radius);
    //        LeanTween.move(moveChips[i], target, 0.2f).setEase(LeanTweenType.easeInOutSine);
    //    }

    //    yield return new WaitForSeconds(0.3f);
    //    //LeanTween.value(gameObject, SetSpriteAlpha, 1f, 0f, 0.3f);
    //    //yield return new WaitForSeconds(0.32f);
    //    for (int i = 0; i < moveChips.Count; ++i)
    //    {
    //        DestroyImmediate(moveChips[i]);
    //    }
    //    moveChips.Clear();
    //}

    IEnumerator CreatChipMoney(long money)
    {
        string MoneyString = new string(money.ToString().ToCharArray().Reverse().ToArray());
        //List<GameObject> moveChips = new List<GameObject>();
        for (int i = 0; i < MoneyString.Length; ++i)
        {
            int count = Int32.Parse(MoneyString[i].ToString());

            if (i == 0 || i == 2 || i == 4 || i == 5)
            {
                int ivalue = count / 5;
                if (ivalue > 0)
                {
                    for (int j = 0; j < ivalue; ++j)
                    {
                        int iChipIndex = i >= Chips.Length ? Chips.Length - 1 : i;
                        GameObject chip = Instantiate(Chips[iChipIndex], transform.position, Quaternion.identity, this.transform) as GameObject;
                        Sprite[] sprChip = (Sprite[])m_aryChipList2[iChipIndex];

                        int iRand = UnityEngine.Random.Range(0, 5);

                        chip.GetComponent<SpriteRenderer>().sprite = sprChip[iRand];
                        chip.transform.localPosition = new Vector3(chip.transform.localPosition.x, chip.transform.localPosition.y, 0.0f);
                        chip.transform.SetParent(IInstantiateParent.Instance.m_objWinnerChip.transform);
                        chip.SetActive(true);

                        Vector3 target = new Vector3();
                        target.x = originPoint.x + UnityEngine.Random.Range(-radiusx, radiusx);
                        target.y = originPoint.y + UnityEngine.Random.Range(-radiusy, radiusy);
                        target.z = 0.0f;
                        LeanTween.move(chip, target, 0.2f).setEase(LeanTweenType.easeInOutSine);

                        IBettingChipsManager.Instance.AddChip(chip);
                    }
                    count -= (ivalue * 5);
                }
            }

            for (int c = 0; c < count; ++c)
            {
                int iChipIndex = i >= Chips.Length ? Chips.Length - 1 : i;
                GameObject chip = Instantiate(Chips[iChipIndex], transform.position, Quaternion.identity, this.transform) as GameObject;
                Sprite[] sprChip = (Sprite[])m_aryChipList[iChipIndex];

                int iRand = UnityEngine.Random.Range(0, 5);

                chip.GetComponent<SpriteRenderer>().sprite = sprChip[iRand];
                chip.transform.localPosition = new Vector3(chip.transform.localPosition.x, chip.transform.localPosition.y, 0.0f);
                chip.transform.SetParent(IInstantiateParent.Instance.m_objWinnerChip.transform);
                chip.SetActive(true);

                Vector3 target = new Vector3();
                target.x = originPoint.x + UnityEngine.Random.Range(-radiusx, radiusx);
                target.y = originPoint.y + UnityEngine.Random.Range(-radiusy, radiusy);
                target.z = 0.0f;
                LeanTween.move(chip, target, 0.2f).setEase(LeanTweenType.easeInOutSine);

                IBettingChipsManager.Instance.AddChip(chip);     
            }
        }

        yield return new WaitForSeconds(0.3f);
        IBettingChipsManager.Instance.SortingLayer("Low1");
    }

    void SetSpriteAlpha(float val)
    {
        for (int i = 0; i < moveChips.Count; ++i)
        {
            SpriteRenderer sprite = moveChips[i].GetComponent<SpriteRenderer>();
            sprite.color = new Color(1f, 1f, 1f, val);
        }
    }
}