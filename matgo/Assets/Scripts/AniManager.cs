using System.Collections;
using UnityEngine;

public class AniManager : CSingletonMonobehaviour<AniManager>
{
    bool m_bWaitChoice = false;
    bool m_bWaitPaeChoice = false;
    bool m_bWaitGoStopChoice = false;

    public enum _eType
    {
        ET_1GO__0 = 0,
        ET_2GO__0,
        ET_3GO__0,
        ET_4GO__0,
        ET_5GO__0,
        ET_6GO__0,
        ET_7GO__0,
        ET_8GO,
        ET_9GO,
        ET_MISSION_COMPLETE,
        ET_MISSION_FAIL,
        ET_MISSION_START,
        ET_MISSION_TITLE,
        ET_GAJUWA__0,
        ET_GODORI__0,
        ET_CHUNGDAN__0,
        ET_CHODAN__0,
        ET_HONGDAN__0,
        ET_GOBAK,
        ET_KWANGBAK,
        ET_PIBAK,
        ET_DDADAK__0,
        ET_MUNGTTA__0,
        ET_BBUCK__0,
        ET_5KWANG__0,
        ET_4KWANG__0,
        ET_3KWANG__0,
        ET_BISAMKWANG__0,
        ET_SUN,
        ET_KISS__0,
        ET_BOMB,
        ET_CLEAR,
        //ET_GOORSTOP,
        //ET_WAITGOSTOP__0,
        //ET_WAITPAECHOICE__0,
        //ET_WAITCHOICE__0,
        //ET_WIN__0,
        //ET_LOSE__0,
        ES_SHAKING__0,

        EVENTINTRO,

        ET_WAITCHOICE, // wait

        //ET_1GO__1,
        //ET_2GO__1,
        //ET_3GO__1,
        //ET_4GO__1,
        //ET_5GO__1,
        //ET_6GO__1,
        //ET_7GO__1,
        //ET_GAJUWA__1,
        //ET_GODORI__1,
        //ET_CHUNGDAN__1,
        //ET_CHODAN__1,
        //ET_HONGDAN__1,
        //ET_DDADAK__1,
        //ET_MUNGTTA__1,
        //ET_BBUCK__1,
        //ET_5KWANG__1,
        //ET_4KWANG__1,
        //ET_3KWANG__1,
        //ET_BISAMKWANG__1,
        //ET_KISS__1,
        //ET_WAITGOSTOP__1,
        //ET_WAITPAECHOICE__1,
        //ET_WAITCHOICE__1,
        //ET_WIN__1,
        //ET_LOSE__1,
        //ES_SHAKING__1,

        ET_MAXCNT,
    };

    public BinkUnity[] m_binkMovie;

    IEnumerator CoLoadBink()
    {
        for (int i = 0; i < m_binkMovie.Length; ++i)
        {
            StartCoroutine(m_binkMovie[i].CoStart());
            m_binkMovie[i].SetEnable2(false, 1, 30);
            yield return null;
            yield return null;
        }

        for (int i = 0; i < m_binkMovie.Length; i++)
        {
            m_binkMovie[i].gameObject.GetComponent<MeshRenderer>().sortingLayerName = "popup";
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(CoLoadBink());
    }

    public void PlayMovie(_eType eType, int iFrame, int ms_per_process, Vector3 vPos)
    {
        m_binkMovie[(int)eType].gameObject.transform.localPosition = vPos;
        m_binkMovie[(int)eType].SetEnable(true, 1, 30);
    }

    public void PlayMovie(_eType eType, int iFrame, int ms_per_process, bool loop = false)
    {
        if (loop) m_binkMovie[(int)eType].Loop(0);

        m_binkMovie[(int)eType].SetEnable(true, 1, 30);
    }

    public bool IsEnd(_eType eType)
    {

        return m_binkMovie[(int)eType].IsEnd();
    }
    public void StopMovie(_eType eType, int iFrame, int ms_per_process)
    {
        m_binkMovie[(int)eType].SetEnable(false, 1, 30);
    }

    public void StopAllMovie()
    {
        m_bWaitChoice = false;
        m_bWaitPaeChoice = false;
        m_bWaitGoStopChoice = false;

        for (int i = 0; i < m_binkMovie.Length; i++)
        {
            m_binkMovie[i].SetEnable(false, 1, 30);
        }
    }

    public void SetWaitChoice(bool bFlag)
    {
        m_bWaitChoice = bFlag;
    }

    public void SetWaitPaeChoice(bool bFlag)
    {
        m_bWaitPaeChoice = bFlag;
    }

    public void SetWaitGoStopChoice(bool bFlag)
    {
        m_bWaitGoStopChoice = bFlag;
    }

    public IEnumerator CoWaitChoice(byte playerIndex)
    {
        float fTime = 0.0f;
        m_bWaitChoice = true;
        while (m_bWaitChoice && CPlayGameUI.Instance.isGaming)
        {
            fTime += Time.deltaTime;
            if (fTime > 3.0f)
            {
                m_binkMovie[(int)_eType.ET_WAITCHOICE].Loop(0);
                PlayMovie(_eType.ET_WAITCHOICE, 1, 30);
                m_bWaitChoice = false;
            }
            yield return null;
        }
    }

    public IEnumerator CoWaitPaeChoice(byte playerIndex)
    {
        float fTime = 0.0f;
        m_bWaitPaeChoice = true;
        while (m_bWaitPaeChoice && CPlayGameUI.Instance.isGaming)
        {
            fTime += Time.deltaTime;
            if (fTime > 4.0f)
            {
                //string tstr = "ET_WAITPAECHOICE__" + CPlayGameUI.Instance.playerSoundIndex[playerIndex].ToString();
                //m_binkMovie[(int)(AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr)].Loop(0);
                //PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);
                m_bWaitPaeChoice = false;
            }
            yield return null;
        }
    }

    public IEnumerator CoWaitGoTopChoice(byte playerIndex)
    {
        float fTime = 0.0f;
        m_bWaitGoStopChoice = true;
        while (m_bWaitGoStopChoice && CPlayGameUI.Instance.isGaming)
        {
            fTime += Time.deltaTime;
            if (fTime > 4.0f)
            {
                //string tstr = "ET_WAITGOSTOP__" + CPlayGameUI.Instance.playerSoundIndex[playerIndex].ToString();
                //m_binkMovie[(int)(AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr)].Loop(0);
                //PlayMovie((AniManager._eType)System.Enum.Parse(typeof(AniManager._eType), tstr), 1, 30);
                m_bWaitGoStopChoice = false;
            }
            yield return null;
        }
    }
}