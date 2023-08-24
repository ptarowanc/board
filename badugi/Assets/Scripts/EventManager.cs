using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public GameObject Pin;
    //[SerializeField]
    //Renderer m_Pin = null;

    private void Awake()
    {
        Instance = this;
        //m_Pin.material.renderQueue = 3100;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z)) StartCoroutine(StartEvent());
    //    if (Input.GetKeyDown(KeyCode.X)) StartCoroutine(StartTurtle());
    //    if (Input.GetKeyDown(KeyCode.C)) StartCoroutine(StartShark());
    //    if (Input.GetKeyDown(KeyCode.V)) StartCoroutine(StartWhale());
    //    if (Input.GetKeyDown(KeyCode.B)) StartCoroutine(EndEvent());
    //    if (Input.GetKeyDown(KeyCode.N)) ClockPinMove();
    //    if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Ceremony25());
    //    if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Ceremony50());
    //    if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(Ceremony100());
    //}

    //public IEnumerator StartEvent()
    //{
    //    AniManager.Instance.PlayMovie(AniManager._eType.CLOCK_INTRO);

    //    yield return new WaitForSeconds(1.2f);

    //    AniManager.Instance.PlayMovie(AniManager._eType.CLOCK_IN);

    //    yield return new WaitForSeconds(1.0f);

    //    AniManager.Instance.PlayMovie(AniManager._eType.CLOCK_LOOP, true);
    //    Pin.SetActive(true);
        
    //}

    public void ClockPinMove(byte jackPotPlayCount)
    {
        Pin.transform.localRotation = new Quaternion(0, 0, -72 * (jackPotPlayCount % 5), 1);
        //Pin.transform.localRotation(0, 0, -72 * (jackPotPlayCount % 5));
    }

    public IEnumerator CoStartEventIndex(int iIndeX)
    {
        int iStartIndex = (int)AniManager._eType.EVENT_INDEX1;

        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_SUDDEN, false);
        StartCoroutine(AniManager.Instance.CoPlayMovie((AniManager._eType)iStartIndex + iIndeX));

        yield return new WaitForSeconds(3.0f);

        int iEventScore = UIManager.Instance.GetRoomMoney() * 100;
        if (iEventScore > 200000)
            iEventScore = 200000;
        StartCoroutine(PlayGameUI.Instance.player_info_slots[iIndeX].CoPlayEventScore(iEventScore));
        //while (true)
        //{
        //    if (AniManager.Instance.GetCurrentFrame((AniManager._eType)iStartIndex + iIndeX) >= 100)
        //    {
        //        int iEventScore =  UIManager.Instance.GetRoomMoney() * 100;
        //        if (iEventScore > 200000)
        //            iEventScore = 200000;
        //        StartCoroutine(PlayGameUI.Instance.player_info_slots[iIndeX].CoPlayEventScore(iEventScore));
        //        break;
        //    }
        //    yield return null;
        //}
    }

    public IEnumerator StartTurtle()
    {
        // 이전단계 루프동영상 정지
        //Pin.SetActive(false);
        //AniManager.Instance.StopMovie(AniManager._eType.CLOCK_LOOP);
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_INTRO, false);
        yield return StartCoroutine(AniManager.Instance.CoPlayMovie(AniManager._eType.EVENT_INTRO));
        
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_PIG, false);
        yield return StartCoroutine(AniManager.Instance.CoPlayMovie(AniManager._eType.TURTLE_INTRO));
        PlayGameUI.Instance.SetEventMultipleVisible(0, true);        
        //AniManager.Instance.PlayMovie(AniManager._eType.CLOCK_YELLOW, true);
        //Pin.SetActive(true);
    }

    public IEnumerator StartShark()
    {
        // 이전단계 루프동영상 정지
        //Pin.SetActive(false);
        //AniManager.Instance.StopMovie(AniManager._eType.CLOCK_YELLOW);
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_INTRO, false);
        yield return StartCoroutine(AniManager.Instance.CoPlayMovie(AniManager._eType.EVENT_INTRO));

        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_HORSE, false);
        yield return StartCoroutine(AniManager.Instance.CoPlayMovie(AniManager._eType.SHARK_INTRO));
        PlayGameUI.Instance.SetEventMultipleVisible(1, true);
        //AniManager.Instance.PlayMovie(AniManager._eType.SHARK_INTRO);
        //SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_HORSE, false);
        //yield return new WaitForSeconds(4.0f);

        //AniManager.Instance.PlayMovie(AniManager._eType.CLOCK_BLUE, true);
        //Pin.SetActive(true);
    }

    public IEnumerator StartWhale()
    {
        // 이전단계 루프동영상 정지
        //Pin.SetActive(false);
        //AniManager.Instance.StopMovie(AniManager._eType.CLOCK_BLUE);

        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_INTRO, false);
        yield return StartCoroutine(AniManager.Instance.CoPlayMovie(AniManager._eType.EVENT_INTRO));

        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_DRAGON, false);
        yield return StartCoroutine(AniManager.Instance.CoPlayMovie(AniManager._eType.WHALE_INTRO));
        PlayGameUI.Instance.SetEventMultipleVisible(2, true);
        //AniManager.Instance.PlayMovie(AniManager._eType.WHALE_INTRO);
        //SoundManager.Instance.PlaySound(SoundManager._eSoundResource.EVENT_DRAGON, false);
        //yield return new WaitForSeconds(4.0f);

        //AniManager.Instance.PlayMovie(AniManager._eType.CLOCK_RED, true);
        //Pin.SetActive(true);
    }

    //public IEnumerator EndEvent()
    //{
    //    // 이전단계 루프동영상 정지
    //    AniManager.Instance.StopMovie(AniManager._eType.CLOCK_LOOP);
    //    AniManager.Instance.StopMovie(AniManager._eType.CLOCK_YELLOW);
    //    AniManager.Instance.StopMovie(AniManager._eType.CLOCK_BLUE);
    //    AniManager.Instance.StopMovie(AniManager._eType.CLOCK_RED);

    //    AniManager.Instance.PlayMovie(AniManager._eType.CLOCK_OUT);

    //    Pin.SetActive(false);
    //    Pin.transform.localRotation = new Quaternion(0, 0, 0, 1);

    //    yield return new WaitForSeconds(1.0f);
    //}

    public IEnumerator Ceremony50()
    {
        AniManager.Instance.PlayMovie(AniManager._eType.CEREMONY_X50);
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.X50, false);
        PlayGameUI.Instance.SetEventMultipleVisible(0, false);
        yield return null;
    }

    public IEnumerator Ceremony100()
    {
        AniManager.Instance.PlayMovie(AniManager._eType.CEREMONY_X100);
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.X100, false);
        PlayGameUI.Instance.SetEventMultipleVisible(1, false);
        yield return null;
    }

    public IEnumerator Ceremony200()
    {
        AniManager.Instance.PlayMovie(AniManager._eType.CEREMONY_X200);
        SoundManager.Instance.PlaySound(SoundManager._eSoundResource.X200, false);
        PlayGameUI.Instance.SetEventMultipleVisible(2, false);
        yield return null;
    }

}