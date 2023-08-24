using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Collections;
using System.Collections;
using System.Collections.Generic;

public class CPlayerInfoSlot : MonoBehaviour
{
    public Text name_text;
    public Text money_text;

    public GameObject Die;
    public GameObject AllinAni;
    public GameObject Pass;
    public GameObject Winner;

    public GameObject m_objWinner;
    public GameObject m_objEventScoreSprite;
    public GameObject m_objReservationIcon;

    public GameObject IconDie;
    public GameObject IconBBing;
    public GameObject IconCheck;
    public GameObject IconCall;
    public GameObject IconQuater;
    public GameObject IconHalf;
    public GameObject IconDdadang;

    public PlayAnimation count1;
    public PlayAnimation count2;
    public PlayAnimation count3;
    public ChipAniManager chipAnimanager;

    public GameObject PrefabFont;
    public GameObject[] grade;

    public GameObject ResultBG;
    public GameObject ResultMoney;
    public GameObject ResultFullName;
    public GameObject ResultBox;

    public GameObject avatar;

    public GameObject changeObj;

    public GameObject Boss;

    public GameObject HandName; // only player 1

    public GameObject CountDown;

    void Awake()
    {
        Allreset();
        count1.gameObject.SetActive(false);
        count2.gameObject.SetActive(false);
        count3.gameObject.SetActive(false);
    }
    
    public void RoomMoveReset()
    {
        count1.gameObject.SetActive(false);
        count2.gameObject.SetActive(false);
        count3.gameObject.SetActive(false);
        m_objReservationIcon.SetActive(false);
    }

    public void Allreset(bool die = false)
    {
        CenterAniReset();

        IconResetAni();
        count1.pass = false;
        count2.pass = false;
        count3.pass = false;
        if (die == false)
        {
            count1.gameObject.SetActive(false);
            count2.gameObject.SetActive(false);
            count3.gameObject.SetActive(false);
            m_objReservationIcon.SetActive(false);
        }
        Boss.SetActive(false);

        CountDown.SetActive(false);
    }

    public void CenterAniReset()
    {
        Die.SetActive(false);
        AllinAni.SetActive(false);
        Pass.SetActive(false);
        Winner.SetActive(false);
        m_objWinner.SetActive(false);
        changeObj.SetActive(false);
    }
    public void MorningSet(int stat)
    {
        count1.gameObject.SetActive(true);
        count1.SetCurrentFrame(stat - 1);
        count2.gameObject.SetActive(false);
        count3.gameObject.SetActive(false);
    }
    public void MorningSet()
    {
        count1.gameObject.SetActive(true);
        count2.gameObject.SetActive(false);
        count3.gameObject.SetActive(false);
    }
    public void AfternoongSet(int stat)
    {
        count1.gameObject.SetActive(true);
        count2.gameObject.SetActive(true);
        count2.SetCurrentFrame(stat - 1);
        count3.gameObject.SetActive(false);
    }
    public void AfternoongSet()
    {
        count1.gameObject.SetActive(true);
        count2.gameObject.SetActive(true);
        count3.gameObject.SetActive(false);
    }
    public void EveningSet(int stat)
    {
        count1.gameObject.SetActive(true);
        count2.gameObject.SetActive(true);
        count3.gameObject.SetActive(true);
        count3.SetCurrentFrame(stat - 1);
    }
    public void EveningSet()
    {
        count1.gameObject.SetActive(true);
        count2.gameObject.SetActive(true);
        count3.gameObject.SetActive(true);
    }
    public void IconResetAni()
    {
        IconDie.SetActive(false);
        IconBBing.SetActive(false);
        IconCheck.SetActive(false);
        IconCall.SetActive(false);
        IconQuater.SetActive(false);
        IconHalf.SetActive(false);
        IconDdadang.SetActive(false);
    }
    public void DieAni()
    {
        Allreset(true);
        Die.SetActive(true);
        Die.GetComponent<tk2dSpriteAnimator>().Play();
        AllinAni.SetActive(false);
        Pass.SetActive(false);
        Winner.SetActive(false);
        m_objWinner.SetActive(false);
        m_objEventScoreSprite.SetActive(false);

        IconDie.SetActive(true);
        IconDie.GetComponent<tk2dSpriteAnimator>().Play();
        IconBBing.SetActive(false);
        IconCheck.SetActive(false);
        IconCall.SetActive(false);
        IconQuater.SetActive(false);
        IconHalf.SetActive(false);
        IconDdadang.SetActive(false);
    }

    public void Allin()
    {
        CenterAniReset();
        Die.SetActive(false);
        AllinAni.SetActive(true);
        AllinAni.GetComponent<tk2dSpriteAnimator>().Play();
        Pass.SetActive(false);
        Winner.SetActive(false);
        m_objWinner.SetActive(false);
        m_objEventScoreSprite.SetActive(false);
    }

    public void PassAni()
    {
        CenterAniReset();
        Die.SetActive(false);
        AllinAni.SetActive(false);
        Pass.SetActive(true);
        Pass.GetComponent<tk2dSpriteAnimator>().Play();
        StartCoroutine(AutoOff());
        Winner.SetActive(false);
        m_objWinner.SetActive(false);
        m_objEventScoreSprite.SetActive(false);
    }

    IEnumerator AutoOff()
    {
        yield return new WaitForSeconds(1.5f);
        Pass.SetActive(false);
    }

    public void WinnerAni()
    {
        CenterAniReset();
        Die.SetActive(false);
        AllinAni.SetActive(false);
        Pass.SetActive(false);
        Winner.SetActive(true);
        Winner.GetComponent<tk2dSpriteAnimator>().Play();
    }

    public IEnumerator CoPlayWinnerEffect()
    {
        m_objWinner.SetActive(true);
        m_objWinner.GetComponent<tk2dSpriteAnimator>().Play();
        while (true)
        {
            if (!m_objWinner.GetComponent<tk2dSpriteAnimator>().IsPlaying("winner"))
                break;
            yield return null;
        }
        m_objWinner.SetActive(false);
        SetFullName("");
    }

    public IEnumerator CoPlayEventScore(int iScore)
    {
        m_objEventScoreSprite.SetActive(true);
        m_objEventScoreSprite.GetComponent<tk2dSprite>().SetSprite("" + iScore);

        Color col = Color.white;
        col.a = 1.0f;
        m_objEventScoreSprite.GetComponent<tk2dSprite>().color = col;
        LeanTween.value(m_objEventScoreSprite, SetSpriteScale_EventScore, 0.0f, 1.0f, 0.3f).setEase(LeanTweenType.easeOutBack);
        yield return new WaitForSeconds(0.5f);

        LeanTween.value(m_objEventScoreSprite, SetSpriteAlpha_EventScore, 1.0f, 0.0f, 1.0f);

        yield return new WaitForSeconds(1.0f);
        m_objEventScoreSprite.SetActive(false);
    }

    void SetSpriteAlpha_EventScore(float val)
    {
        m_objEventScoreSprite.GetComponent<tk2dSprite>().color = new Color(1f, 1f, 1f, val);
    }

    void SetSpriteScale_EventScore(float val)
    {
        m_objEventScoreSprite.GetComponent<tk2dSprite>().scale = new Vector3(118.0f * val, 108.0f * val, 1.0f);
    }

    public void LoserAni()
    {
        CenterAniReset();
        Winner.GetComponent<tk2dSpriteAnimator>().Stop();
    }

    public void BBingAni()
    {
        IconResetAni();
        IconDie.SetActive(false);
        IconBBing.SetActive(true);
        IconBBing.GetComponent<tk2dSpriteAnimator>().Play();
        IconCheck.SetActive(false);
        IconCall.SetActive(false);
        IconQuater.SetActive(false);
        IconHalf.SetActive(false);
        IconDdadang.SetActive(false);
    }

    public void CheckAni()
    {
        IconResetAni();
        IconDie.SetActive(false);
        IconBBing.SetActive(false);
        IconCheck.SetActive(true);
        IconCheck.GetComponent<tk2dSpriteAnimator>().Play();
        IconCall.SetActive(false);
        IconQuater.SetActive(false);
        IconHalf.SetActive(false);
        IconDdadang.SetActive(false);
    }

    public void CallAni()
    {
        IconResetAni();
        IconDie.SetActive(false);
        IconBBing.SetActive(false);
        IconCheck.SetActive(false);
        IconCall.SetActive(true);
        IconCall.GetComponent<tk2dSpriteAnimator>().Play();
        IconQuater.SetActive(false);
        IconHalf.SetActive(false);
        IconDdadang.SetActive(false);
    }

    public void QuaterAni()
    {
        IconResetAni();
        IconDie.SetActive(false);
        IconBBing.SetActive(false);
        IconCheck.SetActive(false);
        IconCall.SetActive(false);
        IconQuater.SetActive(true);
        IconQuater.GetComponent<tk2dSpriteAnimator>().Play();
        IconHalf.SetActive(false);
        IconDdadang.SetActive(false);
    }
    public void HarfAni()
    {
        IconResetAni();
        IconDie.SetActive(false);
        IconBBing.SetActive(false);
        IconCheck.SetActive(false);
        IconCall.SetActive(false);
        IconQuater.SetActive(false);
        IconHalf.SetActive(true);
        IconHalf.GetComponent<tk2dSpriteAnimator>().Play();
        IconDdadang.SetActive(false);
    }
    public void DdadangAni()
    {
        IconResetAni();
        IconDie.SetActive(false);
        IconBBing.SetActive(false);
        IconCheck.SetActive(false);
        IconCall.SetActive(false);
        IconQuater.SetActive(false);
        IconHalf.SetActive(false);
        IconDdadang.SetActive(true);
        IconDdadang.GetComponent<tk2dSpriteAnimator>().Play();
    }

    public void update_name(string name)
    {
        name_text.text = name;
    }

    public void ShowCard(string fullName, bool made)
    {
        if (HandName != null)
            HandName.GetComponent<Text>().text = "";

        ResultBG.SetActive(true);
        //ResultMoney.SetActive(true);
        if (made)
            ResultFullName.GetComponent<Text>().color = Color.yellow;
        else
            ResultFullName.GetComponent<Text>().color = Color.white;
        ResultFullName.GetComponent<Text>().text = fullName;
    }

    public IEnumerator ShowResult(string _fullName, byte _grade, byte _number, long _money = 50000000000, byte win = 2, int count = 1)
    {
        if (HandName != null)
            HandName.GetComponent<Text>().text = "";
        //grade[0].GetComponent<tk2dSprite>().SetSprite("empty");
        //grade[1].GetComponent<tk2dSprite>().SetSprite("empty");

        if (count != 1)
        {
            ResultBG.SetActive(true);
            //ResultMoney.SetActive(true);
        }

        //if (win == 0) ResultBG.GetComponent<tk2dSpriteAnimator>().Play("win");
        //else if (win == 1) ResultBG.GetComponent<tk2dSpriteAnimator>().Play("win");
        //else ResultBG.GetComponent<tk2dSpriteAnimator>().Play("lose");

        //ResultMoney.GetComponent<Text>().text = MoneyConvertToUnit(_money, true);

        if (count != 1)
        {
            if (win == 3)
            {
                ResultFullName.GetComponent<Text>().text = "기 권 승";
            }
            else
            {
                if (_grade >= 4)
                    ResultFullName.GetComponent<Text>().color = Color.yellow;
                else
                    ResultFullName.GetComponent<Text>().color = Color.white;
                ResultFullName.GetComponent<Text>().text = _fullName;
            }
        }

        yield return null;
    }


    public void HideResult()
    {
        ResultBG.SetActive(false);
        //ResultMoney.SetActive(false);
    }

    public void update_cardinfo(byte _grade, byte _number)
    {
        if (_grade == 7)
        {
            // 골프
            grade[0].GetComponent<tk2dSprite>().SetSprite("grade_golf");
            grade[1].GetComponent<tk2dSprite>().SetSprite("empty");
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
            return;
        }
        else if (_grade == 6)
        {
            // 세컨
            grade[0].GetComponent<tk2dSprite>().SetSprite("grade_second");
            grade[1].GetComponent<tk2dSprite>().SetSprite("empty");
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
            return;
        }
        else if (_grade == 5)
        {
            // 써드
            grade[0].GetComponent<tk2dSprite>().SetSprite("grade_third");
            grade[1].GetComponent<tk2dSprite>().SetSprite("empty");
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
            return;
        }
        else if (_grade == 4)
        {
            // 메이드
            grade[0].GetComponent<tk2dSprite>().SetSprite("grade_made");
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
        }
        else if (_grade == 3)
        {
            // 베이스
            grade[0].GetComponent<tk2dSprite>().SetSprite("grade_base");
        }
        else if (_grade == 2)
        {
            // 투베이스
            HandName.GetComponent<Text>().color = Color.white;
            grade[0].GetComponent<tk2dSprite>().SetSprite("grade_twobase");
        }
        else
        {
            // 노베이스
            grade[0].GetComponent<tk2dSprite>().SetSprite("grade_nobase");
            grade[1].GetComponent<tk2dSprite>().SetSprite("empty");
            return;
        }

        if (_number == 0) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_a");
        else if (_number == 1) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_02");
        else if (_number == 2) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_03");
        else if (_number == 3) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_04");
        else if (_number == 4) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_05");
        else if (_number == 5) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_06");
        else if (_number == 6) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_07");
        else if (_number == 7) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_08");
        else if (_number == 8) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_09");
        else if (_number == 9) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_10");
        else if (_number == 10) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_j");
        else if (_number == 11) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_q");
        else if (_number == 12) grade[1].GetComponent<tk2dSprite>().SetSprite("grade_k");
    }

    public void update_cardinfo(byte _grade, string _number)
    {
        if (_grade == 7)
        {
            // 골프
            HandName.GetComponent<Text>().color = Color.yellow;
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
        }
        else if (_grade == 6)
        {
            // 세컨
            HandName.GetComponent<Text>().color = Color.yellow;
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
        }
        else if (_grade == 5)
        {
            // 써드
            HandName.GetComponent<Text>().color = Color.yellow;
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
        }
        else if (_grade == 4)
        {
            // 메이드
            HandName.GetComponent<Text>().color = Color.yellow;
            AniManager.Instance.PlayMovie(AniManager._eType.MADECEREMONY);
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.MADE, false);
        }
        else if (_grade == 3)
        {
            // 베이스
            HandName.GetComponent<Text>().color = Color.white;
        }
        else if (_grade == 2)
        {
            // 투베이스
            HandName.GetComponent<Text>().color = Color.white;
        }
        else
        {
            // 노베이스
            HandName.GetComponent<Text>().color = Color.white;
        }

        HandName.GetComponent<Text>().text = _number;
    }

    public void update_money(long money)
    {
        money_text.text = MoneyConvertToUnit(money);
    }

    string MoneyConvertToUnit(long money, bool isVar = false)
    {
        if (money == 0) return "0" + (PlayGameUI.Instance.moneyType == 1 ? "냥" : "냥");

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

        if (isVar && money < 0) moneyStr += "- ";
        else if (isVar && money > 0) moneyStr += "+ ";

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

        if (moneyStr != "") moneyStr += PlayGameUI.Instance.moneyType == 1 ? "냥" : "냥";

        return moneyStr;
    }

    public IEnumerator changeAni(int cnt)
    {
        changeObj.SetActive(true);
        changeObj.GetComponent<tk2dSpriteAnimator>().Play("c" + cnt.ToString());
        yield return new WaitForSeconds(1.5f);
        changeObj.SetActive(false);
    }

    public void SetBoss(bool isBoss)
    {
        if (isBoss == true)
        {
            Boss.SetActive(true);
        }
        else
        {
            Boss.SetActive(false);
        }
    }

    public void SetFullName(string strText)
    {
        ResultFullName.GetComponent<Text>().text = "";
    }

    public void SetReservationIcon(bool bFlag)
    {
        m_objReservationIcon.SetActive(bFlag);
    }
}