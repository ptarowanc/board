using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZNet;
using Vector3 = UnityEngine.Vector3;

public class CPopupGameResult : MonoBehaviour
{
    string reportStr = "";
    int reportCnt = 0;

    public GameObject finalBetPosObj;
    public GameObject finalScorePosObj;
    public GameObject finalMoneyPosObj;
    public GameObject eventFinalBetPosObj;
    public GameObject eventFinalScorePosObj;
    public GameObject eventFinalMoneyPosObj;
    public GameObject eventFinalAntePosObj;
    public GameObject eventFinalMultiPosObj;

    Vector3 finalBetPos;
    Vector3 finalScorePos;
    Vector3 finalMoneyPos;
    Vector3 eventFinalBetPos;
    Vector3 eventFinalScorePos;
    Vector3 eventFinalMoneyPos;
    Vector3 eventFinalAntePos;
    Vector3 eventFinalMultiPos;
    float finalBetPosD;
    float finalScorePosD;
    float finalMoneyPosD;
    float finalAntePosD;
    float finalMultiPosD;
    List<GameObject> finalBetObject = new List<GameObject>();
    List<GameObject> finalScoreObject = new List<GameObject>();
    List<GameObject> finalMoneyObject = new List<GameObject>();
    List<GameObject> finalAnteObject = new List<GameObject>();
    List<GameObject> finalMultiObject = new List<GameObject>();

    Vector3 jackpotPos;
    float jackpotPosD;
    List<GameObject> jackpotObject = new List<GameObject>();

    public GameObject FontPrefab;

    void Awake()
    {
        //transform.Find("button_play").GetComponent<Button>().onClick.AddListener(PlayTouch);
        transform.Find("button_push").GetComponent<Button>().onClick.AddListener(PushTouch);

        if(NetworkManager.Instance.Screen)
        {
            finalBetPos = new Vector3(-200, 220, 0);
            finalScorePos = new Vector3(-200, 105, 0);
            finalMoneyPos = new Vector3(-220, 0, 0);
            eventFinalBetPos = new Vector3(-240, 270, 0);
            eventFinalScorePos = new Vector3(-240, 155, 0);
            eventFinalMoneyPos = new Vector3(-280, 60, 0);
            eventFinalAntePos = new Vector3(-330, -45, 0);
            eventFinalMultiPos = new Vector3(-30, -45, 0);
        }
        else
        {
            finalBetPos = finalBetPosObj.transform.position;
            finalScorePos = finalScorePosObj.transform.position;
            finalMoneyPos = finalMoneyPosObj.transform.position;
            eventFinalBetPos = eventFinalBetPosObj.transform.position;
            eventFinalScorePos = eventFinalScorePosObj.transform.position;
            eventFinalMoneyPos = eventFinalMoneyPosObj.transform.position;
            eventFinalAntePos = eventFinalAntePosObj.transform.position;
            eventFinalMultiPos = eventFinalMultiPosObj.transform.position;
        }

        finalBetPosD = 25;
        finalScorePosD = 25;
        finalMoneyPosD = 25;
        finalAntePosD = 25;
        finalMultiPosD = 25;

        jackpotPos = new Vector3(-320, -50, 0);
        jackpotPosD = 40;
    }

    public void ResetAll()
    {
        foreach (var obj in finalBetObject) Destroy(obj);
        foreach (var obj in finalScoreObject) Destroy(obj);
        foreach (var obj in finalMoneyObject) Destroy(obj);
        foreach (var obj in finalAnteObject) Destroy(obj);
        foreach (var obj in finalMultiObject) Destroy(obj);
        foreach (var obj in jackpotObject) Destroy(obj);

        finalBetObject.Clear();
        finalScoreObject.Clear();
        finalMoneyObject.Clear();
        finalAnteObject.Clear();
        finalMultiObject.Clear();
        jackpotObject.Clear();

        transform.Find("button_play").gameObject.SetActive(false);
        gameObject.transform.Find("draw").gameObject.SetActive(false);
        gameObject.transform.Find("win").gameObject.SetActive(false);
        gameObject.transform.Find("lose").gameObject.SetActive(false);
        gameObject.transform.Find("jackpotWin").gameObject.SetActive(false);
        gameObject.transform.Find("reportInfo").gameObject.SetActive(false);
        gameObject.transform.Find("Text").gameObject.SetActive(false);

        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.NONE;
    }

    //public void PlayTouch()
    //{
    //    ResetAll();

    //    CUIManager.Instance.hide(UI_PAGE.POPUP_GAME_RESULT);
    //    CMessage newmsg = new CMessage();
    //    PacketType msgID = (PacketType)Rmi.Common.CS_READY_TO_START;
    //    CPackOption pkOption = CPackOption.Basic;
    //    newmsg.WriteStart(msgID, pkOption, 0, true);
    //    NetworkManager.Instance.client.PacketSend(CPlayGameUI.Instance.remote, pkOption, newmsg);
    //}

    void PushTouch()
    {
        ResetAll();

        CUIManager.Instance.hide(UI_PAGE.POPUP_GAME_RESULT);
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameSelectPush;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }

    private void SetReportString()
    {
        if (reportStr.Split('\n').Length > 100)
        {
            reportStr = reportStr.Split(System.Environment.NewLine.ToCharArray(), 100 + 1).Skip(100).FirstOrDefault();
        }

        transform.parent.transform.Find("gameReportRoot").GetChild(0).GetComponent<Text>().text = reportStr;
    }

    public IEnumerator PlayerAction(string NickName, int Action)
    {
        yield return null;

        switch (Action)
        {
            case 1:
                {
                    reportStr += NickName + " 님이 입장했습니다.\n";
                }
                break;
            case 2:
                {
                    reportStr += NickName + " 님이 퇴장했습니다.\n";
                }
                break;
        }

        //SetReportString();
    }

    public IEnumerator refresh(byte is_win, int startMoney, int finalScore, int score, int goBak, int peeBak, int gwangBak, int meongTta, int shake, int goMulti, int missionMulti, int drawMulti, int pushMulti, bool is_push, int chongtongNumber, long winnerMoney, long loserMoney, long dealerMoney, byte playerIndex, int jackpotMoney, int jackpotReward, short score1, short score2, short score3, short score4, short score5, bool canGameStart)
    {
        yield return null;

        // 총통이면
        if (chongtongNumber >= 0 && chongtongNumber <= 11)
        {
            string tstr = "ESR_CHONGTONG" + "__" + CPlayGameUI.Instance.playerSoundIndex[playerIndex];
            SoundManager.Instance.PlaySound((SoundManager._eSoundResource)System.Enum.Parse(typeof(SoundManager._eSoundResource), tstr), false);

            // 총통이미지 가져오기
            List<Sprite> l_sprite = new List<Sprite>();
            string _str = "";
            if (chongtongNumber < 9) _str += "0";
            _str += (chongtongNumber + 1).ToString();
            for (int i = 0; i < 4; i++) l_sprite.Add(CSpriteManager.Instance.get_sprite(_str + "-" + (i + 1).ToString()));
            // 총통표시
            CUIManager.Instance.show(UI_PAGE.POPUP_CHONGTONG);
            CUIManager.Instance.get_uipage(UI_PAGE.POPUP_CHONGTONG).GetComponent<CPopupChongtong>().refresh(l_sprite);

            yield return new WaitForSeconds(3.0f);

            CUIManager.Instance.hide(UI_PAGE.POPUP_CHONGTONG);

            CPlayGameUI.Instance.hide_hint_mark();
            CPlayGameUI.Instance.hide_mission_mark();
        }

        CPlayGameUI.Instance.SetVisible_Draw(false);

        if (is_win == 0)
        {   // 무
            gameObject.transform.Find("draw").gameObject.SetActive(true);
            gameObject.transform.Find("win").gameObject.SetActive(false);
            gameObject.transform.Find("lose").gameObject.SetActive(false);
            gameObject.transform.Find("jackpotWin").gameObject.SetActive(false);
            CPlayGameUI.Instance.SetVisible_Draw(true);
        }
        else if (is_win == 1)
        {   // 승
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_GAMEWIN, false);
            gameObject.transform.Find("draw").gameObject.SetActive(false);
            gameObject.transform.Find("lose").gameObject.SetActive(false);
            gameObject.transform.Find("reportInfo").gameObject.SetActive(true);
            gameObject.transform.Find("Text").gameObject.SetActive(true);

            if (CPlayGameUI.Instance.isEventing)
            {
                gameObject.transform.Find("win").gameObject.SetActive(false);
                gameObject.transform.Find("jackpotWin").gameObject.SetActive(true);
            }
            else
            {
                gameObject.transform.Find("win").gameObject.SetActive(true);
                gameObject.transform.Find("jackpotWin").gameObject.SetActive(false);
            }

            string tstr = "ET_WIN__" + CPlayGameUI.Instance.playerSoundIndex[playerIndex];
        }
        else if (is_win == 2)
        {   // 패
            SoundManager.Instance.PlaySound(SoundManager._eSoundResource.ESR_GAMELOSE, false);
            gameObject.transform.Find("draw").gameObject.SetActive(false);
            gameObject.transform.Find("win").gameObject.SetActive(false);
            gameObject.transform.Find("lose").gameObject.SetActive(true);
            gameObject.transform.Find("jackpotWin").gameObject.SetActive(false);
            gameObject.transform.Find("reportInfo").gameObject.SetActive(true);
            gameObject.transform.Find("Text").gameObject.SetActive(true);

            string tstr = "ET_LOSE__" + CPlayGameUI.Instance.playerSoundIndex[playerIndex];
        }

        //transform.Find("button_play").gameObject.SetActive(true);
        if (canGameStart)
        {
            // 게임시작
            CUIManager.Instance.show(UI_PAGE.BUTTON_START);
        }
        else
        {
            // 연습시작
            CUIManager.Instance.show(UI_PAGE.BUTTON_PRACTICE);
        }

        reportStr += "\n<color=#000000ff>최종 점수 : " + finalScore + "점</color>\n";

        if (is_win > 0)
        {
            string winnerNickName;
            string loserNickName;
            if (is_win == 1) // 승
            {
                winnerNickName = CUIManager.Instance.transform.Find("Player01").transform.GetComponent<CUserInfo>().Name.GetComponent<Text>().text;
                loserNickName = CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().Name.GetComponent<Text>().text;
            }
            else // 패
            {
                winnerNickName = CUIManager.Instance.transform.Find("Player02").transform.GetComponent<CUserInfo>().Name.GetComponent<Text>().text;
                loserNickName = CUIManager.Instance.transform.Find("Player01").transform.GetComponent<CUserInfo>().Name.GetComponent<Text>().text;
            }

            if (winnerMoney != 0) reportStr += ("<color=#0000ffff>" + winnerNickName + " : +" + GetMoneyConvert(winnerMoney) + "</color>\n");
            if (loserMoney != 0) reportStr += ("<color=#000000ff>" + loserNickName + " : -" + GetMoneyConvert(loserMoney) + "</color>\n");
        }

        // 게임 리포트
        //SetReportString(); // PC

        // 추가된 게임 결과 정보
        string reportInfo = "";
        if (score1 > 1) reportInfo += ("<color=#00ff00ff>광 " + score1 + "점</color>\n");
        if (score2 > 1) reportInfo += ("<color=#00ff00ff>열 " + score2 + "점</color>\n");
        if (score3 > 1) reportInfo += ("<color=#00ff00ff>띠 " + score3 + "점</color>\n");
        if (score4 > 1) reportInfo += ("<color=#00ff00ff>피 " + score4 + "점</color>\n");
        if (score5 > 1) reportInfo += ("<color=#00ff00ff>고 " + score5 + "점</color>\n");
        if (goMulti > 1) reportInfo += ("<color=#ffff00ff>고 [" + goMulti + "배]</color>\n");
        if (goBak > 1) reportInfo += ("<color=#ffff00ff>고박 [" + goBak + "배]</color>\n");
        if (peeBak > 1) reportInfo += ("<color=#ffff00ff>피박 [" + peeBak + "배]</color>\n");
        if (gwangBak > 1) reportInfo += ("<color=#ffff00ff>광박 [" + gwangBak + "배]</color>\n");
        if (meongTta > 1) reportInfo += ("<color=#ffff00ff>멍따 [" + meongTta + "배]</color>\n");
        if (missionMulti > 1) reportInfo += ("<color=#ffff00ff>미션 [" + missionMulti + "배]</color>\n");
        if (shake > 1) reportInfo += ("<color=#ffff00ff>흔들기 [" + shake + "배]</color>\n");
        if (drawMulti > 1) reportInfo += ("<color=#ffff00ff>나가리 [" + drawMulti + "배]</color>\n");
        if (pushMulti > 1) reportInfo += ("<color=#ffff00ff>밀기 [" + pushMulti + "배]</color>\n");
        reportInfo += "<color=#ffa500ff>점수 합계 : " + finalScore + "점</color>\n";
        gameObject.transform.Find("Text").GetComponent<Text>().text = reportInfo;

        // 이벤트 승리로 게임종료
        if (is_win == 1 && CPlayGameUI.Instance.isEventing)
        {
            // 판돈표시
            MoneyConvertToUnit(System.Convert.ToInt64(startMoney), FontPrefab, finalBetObject, eventFinalBetPos.x, eventFinalBetPos.y, eventFinalBetPos.z, finalBetPosD, false);
            // 점수표시
            MoneyConvertToUnit(System.Convert.ToInt64(finalScore), FontPrefab, finalScoreObject, eventFinalScorePos.x, eventFinalScorePos.y, eventFinalScorePos.z, finalScorePosD, true);
            // 금액표시
            MoneyConvertToUnit(System.Convert.ToInt64(startMoney * finalScore), FontPrefab, finalMoneyObject, eventFinalMoneyPos.x, eventFinalMoneyPos.y, eventFinalMoneyPos.z, finalMoneyPosD, false);
            // 잭팟 판돈 표시
            MoneyConvertToUnit(System.Convert.ToInt64(jackpotMoney), FontPrefab, finalAnteObject, eventFinalAntePos.x, eventFinalAntePos.y, eventFinalAntePos.z, finalAntePosD, false);
            // 잭팟 배율 표시
            MoneyConvertToUnit(System.Convert.ToInt64(jackpotReward), FontPrefab, finalMultiObject, eventFinalMultiPos.x, eventFinalMultiPos.y, eventFinalMultiPos.z, finalMultiPosD, false);
        }
        // 평범한 게임종료
        else if (is_win != 0)
        {
            // 판돈표시
            MoneyConvertToUnit(System.Convert.ToInt64(startMoney), FontPrefab, finalBetObject, finalBetPos.x, finalBetPos.y, finalBetPos.z, finalBetPosD, false);
            // 점수표시
            MoneyConvertToUnit(System.Convert.ToInt64(finalScore), FontPrefab, finalScoreObject, finalScorePos.x, finalScorePos.y, finalScorePos.z, finalScorePosD, true);
            // 금액표시
            MoneyConvertToUnit(System.Convert.ToInt64(startMoney * finalScore), FontPrefab, finalMoneyObject, finalMoneyPos.x, finalMoneyPos.y, finalMoneyPos.z, finalMoneyPosD, false);
        }

        if(finalScore == 0)
        {
            // 서버와의 접속이 끊어졌습니다.
            NetworkManager.Instance.PopupServerDisconnect("※안내※\n서버와 연결을 시도중입니다.");
        }
    }

    string GetMoneyConvert(long money)
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

        if (moneyStr != "") moneyStr += CPlayGameUI.Instance.moneyType == 1 ? "냥" : "냥";
        else if (moneyStr == "") moneyStr += "0" + (CPlayGameUI.Instance.moneyType == 1 ? "냥" : "냥");

        return moneyStr;
    }

    void MoneyConvertToUnit(long money, GameObject fontPrefab, List<GameObject> listObject, float posX, float posY, float posZ, float posD, bool iScore)
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

        if (iScore == false)
        {
            //if (CPlayGameUI.Instance.moneyType == 1) moneyStr += "s";
            //else moneyStr += "g";
            
            //if (CPlayGameUI.Instance.moneyType == 1) moneyStr += "s";
            //else moneyStr += "g";
        }

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
            //if (isJackpot) addD = 0.05f;
            //else addD = 5;
            addD = 5;

            if (c_str[i] == '만' || c_str[i] == '억' || c_str[i] == '조' || c_str[i] == '냥' || c_str[i] == '점') posX += addD;
            else if (i > 0) if (c_str[i - 1] == '만' || c_str[i - 1] == '억' || c_str[i - 1] == '조' || c_str[i - 1] == '냥' || c_str[i - 1] == '점') posX += addD;

            g.transform.position = new Vector3(posX + (i * posD) + ((posD / -2) * (c_str.Length - 1)), posY, posZ);

            listObject.Add(g);
        }
    }
}