using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CPlayerInfoSlot : MonoBehaviour
{
    public GameObject shakeObj;
    public GameObject pukObj;
    public GameObject peebakObj;
    public GameObject gwangbakObj;
    public GameObject goObj;
    public GameObject first;

    // 피카운트는 UI로 써야하기때문에 하이어라키의 캔버스 아래에 있다.
    public GameObject peeCount;

    public GameObject[] PrefabTypeAFont;
    public GameObject[] PrefabTypeBFont;

    Vector3 PukPos;
    Vector3 ShakePos;
    Vector3 GoPos;
    float scoreD = -30f;        // score 공차

    public GameObject scoreObj;
    float scoreX;
    float scoreY;

    List<GameObject> listPuk = new List<GameObject>();
    List<GameObject> listShake = new List<GameObject>();
    List<GameObject> listGo = new List<GameObject>();
    List<GameObject> listScore = new List<GameObject>();

    private void Awake()
    {
        if (NetworkManager.Instance.Screen)
        {
            if (gameObject.name == "Player_Info_01")
            {
                PukPos = new Vector3(390, -235, 0);
                ShakePos = new Vector3(465, -235, 0);
                GoPos = new Vector3(310, -235, 0);
                scoreX = 425;
                scoreY = -155;
            }
            else
            {
                PukPos = new Vector3(390, 420, 0);
                ShakePos = new Vector3(465, 420, 0);
                GoPos = new Vector3(310, 420, 0);
                scoreX = 425;
                scoreY = 354.5f;
            }
        }
        else
        {
            PukPos = pukObj.transform.position + new Vector3(-20, 0, 0); // 35
            ShakePos = shakeObj.transform.position + new Vector3(-20, 0, 0); // 35
            GoPos = goObj.transform.position + new Vector3(-20, 0, 0); // 30
            scoreX = scoreObj.transform.position.x;
            scoreY = scoreObj.transform.position.y;
        }
    }

    public void resetAll()
    {
        shakeObj.SetActive(false);
        pukObj.SetActive(false);
        peebakObj.SetActive(false);
        gwangbakObj.SetActive(false);
        goObj.SetActive(false);

        foreach (var g in listScore) Destroy(g);
        listScore.Clear();

        foreach (var g in listGo) Destroy(g);
        listGo.Clear();

        foreach (var g in listShake) Destroy(g);
        listShake.Clear();

        foreach (var g in listPuk) Destroy(g);
        listPuk.Clear();

        peeCount.SetActive(false);
    }

    public void update_score(short score)
    {
        foreach (var g in listScore) Destroy(g);
        listScore.Clear();

        float numberX = scoreX;
        float numberY = scoreY;
        float numberZ = 0;
        float numberD = 30;
        if (NetworkManager.Instance.Screen == false)
        {
            numberD = 15;
        }

        if (score >= 1)
        {
            string str = score.ToString();
            char[] str_a = new char[str.ToCharArray().Length];
            str_a = str.ToCharArray();

            for (int i = 0; i < str_a.Length; i++)
            {
                GameObject g = Instantiate(PrefabTypeBFont[int.Parse(str_a[i].ToString())]);
                if (NetworkManager.Instance.Screen == false)
                {
                    g.transform.localScale = new Vector3(60f, 60f, 0);
                }

                g.transform.localPosition = new Vector3((numberX + (i * numberD)) + ((numberD / 2 * -1) * (str_a.Length)), numberY, numberZ);

                listScore.Add(g);

                if (i == str_a.Length - 1)
                {
                    GameObject g2 = Instantiate(PrefabTypeBFont[PrefabTypeBFont.Length - 1]);
                    if (NetworkManager.Instance.Screen == false)
                    {
                        g2.transform.localScale = new Vector3(50f, 50f, 0);
                        g2.transform.localPosition = new Vector3((numberX + (i * numberD)) + ((numberD / 2 * -1) * (str_a.Length)) + 20, numberY, numberZ);
                    }
                    else
                    {
                        g2.transform.localPosition = new Vector3((numberX + (i * numberD)) + ((numberD / 2 * -1) * (str_a.Length)) + 40, numberY, numberZ);
                    }

                    listScore.Add(g2);
                }
            }
        }
    }

    public void update_go(short go)
    {
        foreach (var g in listGo) Destroy(g);
        listGo.Clear();
        goObj.SetActive(false);

        if (go >= 1)
        {
            goObj.SetActive(true);

            GameObject g = Instantiate(PrefabTypeAFont[go - 1]);
            if (NetworkManager.Instance.Screen == false)
                g.transform.localScale = new Vector3(70, 70, 0);

            g.transform.localPosition = GoPos;

            listGo.Add(g);
        }
    }

    public void update_shake(short shake)
    {
        foreach (var g in listShake) Destroy(g);
        listShake.Clear();
        shakeObj.SetActive(false);

        if (shake >= 1)
        {
            shakeObj.SetActive(true);

            GameObject g = Instantiate(PrefabTypeAFont[shake - 1]);
            if (NetworkManager.Instance.Screen == false)
                g.transform.localScale = new Vector3(70, 70, 0);

            g.transform.localPosition = ShakePos;

            listShake.Add(g);
        }
    }

    public void update_ppuk(short ppuk)
    {
        foreach (var g in listPuk) Destroy(g);
        listPuk.Clear();
        pukObj.SetActive(false);

        if (ppuk >= 1)
        {
            pukObj.SetActive(true);

            GameObject g = Instantiate(PrefabTypeAFont[ppuk - 1]);

            g.transform.localPosition = PukPos;

            if (NetworkManager.Instance.Screen == false)
                g.transform.localScale = new Vector3(70,70,0);

            listPuk.Add(g);
        }
    }

    public void update_peecount(byte count, bool gwangbak, bool peebak, CPlayerCardManager player_info_slots = null)
    {
        if (count >= 1 && player_info_slots != null)
        {
            peeCount.SetActive(true);
            peeCount.transform.Find("Text").GetComponent<Text>().text = count.ToString();
            peeCount.transform.localPosition = player_info_slots.GetLastPeePos() + new Vector3(15, -42.5f, 0);
        }
        else
        {
            peeCount.SetActive(false);
        }

        if (gwangbak) gwangbakObj.SetActive(true);
        else gwangbakObj.SetActive(false);

        if (peebak) peebakObj.SetActive(true);
        else peebakObj.SetActive(false);
    }
}