using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPopupMission : MonoBehaviour
{
    public List<GameObject> slots;
    public GameObject all;
    public GameObject[] baesu;

    public GameObject whalbinBG;
    public GameObject whalbinMe;
    public GameObject whalbinEnemy;

    public void ResetColor()
    {
        foreach (var obj in slots) obj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void HideCard()
    {
        foreach (var obj in slots) obj.SetActive(false);
        foreach (var obj in baesu) obj.SetActive(false);
        all.SetActive(false);
        whalbinBG.SetActive(false);
        whalbinMe.SetActive(false);
        whalbinEnemy.SetActive(false);
    }

    public void RefreshMission(MISSION mission, List<CCard> cards)
    {
        // 알파값 초기화
        ResetColor();

        // 전부 숨기기
        HideCard();

        // 2장일때 스트링 만들기
        string[] str = new string[cards.Count];
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].number <= 8) str[i] += "0";
            str[i] += (cards[i].number + 1).ToString();
            str[i] += "-";
            str[i] += (cards[i].position + 1).ToString();
        }

        // 활성화
        switch (mission)
        {
            case MISSION.FIVEKWANG:
                for (int i = 0; i < 5; i++) slots[i].SetActive(true);
                baesu[3].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("01-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("03-1");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("08-1");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("11-1");
                slots[4].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("12-1");
                break;
            case MISSION.KWANGTTANG:
                for (int i = 0; i < 3; i++) slots[i].SetActive(true);
                baesu[2].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("01-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("03-1");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("08-1");
                break;
            case MISSION.GODORI:
                for (int i = 0; i < 3; i++) slots[i].SetActive(true);
                baesu[2].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("02-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("04-1");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("08-2");
                break;
            case MISSION.HONGDAN:
                for (int i = 0; i < 3; i++) slots[i].SetActive(true);
                baesu[2].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("01-2");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("02-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("03-2");
                break;
            case MISSION.CHODAN:
                for (int i = 0; i < 3; i++) slots[i].SetActive(true);
                baesu[2].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("04-2");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("05-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("07-2");
                break;
            case MISSION.CHUNGDAN:
                for (int i = 0; i < 3; i++) slots[i].SetActive(true);
                baesu[2].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("06-2");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("09-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("10-2");
                break;
            case MISSION.WOL1_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("01-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("01-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("01-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("01-4");
                break;
            case MISSION.WOL2_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("02-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("02-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("02-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("02-4");
                break;
            case MISSION.WOL3_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("03-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("03-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("03-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("03-4");
                break;
            case MISSION.WOL4_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("04-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("04-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("04-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("04-4");
                break;
            case MISSION.WOL5_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("05-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("05-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("05-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("05-4");
                break;
            case MISSION.WOL6_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("06-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("06-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("06-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("06-4");
                break;
            case MISSION.WOL7_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("07-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("07-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("07-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("07-4");
                break;
            case MISSION.WOL8_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("08-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("08-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("08-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("08-4");
                break;
            case MISSION.WOL9_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("09-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("09-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("09-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("09-4");
                break;
            case MISSION.WOL10_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("10-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("10-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("10-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("10-4");
                break;
            case MISSION.WOL11_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("11-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("11-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("11-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("11-4");
                break;
            case MISSION.WOL12_4:
                for (int i = 0; i < 4; i++) slots[i].SetActive(true);
                baesu[1].SetActive(true);
                slots[0].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("12-1");
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("12-2");
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("12-3");
                slots[3].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite("12-4");
                break;
            case MISSION.WOL1_2:
            case MISSION.WOL2_2:
            case MISSION.WOL3_2:
            case MISSION.WOL4_2:
            case MISSION.WOL5_2:
            case MISSION.WOL6_2:
            case MISSION.WOL7_2:
            case MISSION.WOL8_2:
            case MISSION.WOL9_2:
            case MISSION.WOL10_2:
            case MISSION.WOL11_2:
            case MISSION.WOL12_2:
                slots[1].SetActive(true);
                slots[1].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite(str[0]);
                slots[2].SetActive(true);
                slots[2].GetComponent<Image>().sprite = CSpriteManager.Instance.get_sprite(str[1]);
                baesu[0].SetActive(true);
                break;
            case MISSION.BAE2:
                all.SetActive(true);
                baesu[0].SetActive(true);
                break;
            case MISSION.BAE3:
                all.SetActive(true);
                baesu[1].SetActive(true);
                break;
            case MISSION.BAE4:
                all.SetActive(true);
                baesu[2].SetActive(true);
                break;
            case MISSION.WHALBIN:
                whalbinBG.SetActive(true);
                break;
        }
    }
}