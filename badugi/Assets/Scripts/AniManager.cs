using System.Collections;
using UnityEngine;

public class AniManager : CSingletonMonobehaviour<AniManager>
{
    public enum _eType
    {
        TURTLE_INTRO,
        SHARK_INTRO,
        WHALE_INTRO,
        CEREMONY_X50,
        CEREMONY_X100,
        CEREMONY_X200,
        MADECEREMONY,
        GOLF_CEREMONY,
        EVENT_INTRO,
        EVENT_INDEX1,
        EVENT_INDEX2,
        EVENT_INDEX3,
        EVENT_INDEX4,
        EVENT_INDEX5,

        MAXCOUNT
    };

    public BinkUnity[] m_binkMovie;

    IEnumerator movieLoad()
    {
        for (int i = 0; i < m_binkMovie.Length; i++)
        {
            StartCoroutine(m_binkMovie[i].CoStart());
            m_binkMovie[i].SetEnable2(false, 1, 30);
            yield return null;
        }

        //for (int i = 0; i < m_binkMovie.Length; i++)
        //    m_binkMovie[i].m_MeshRender.enabled = false;

        //for (int i = 0; i < m_binkMovie.Length; i++)
        //{
        //    m_binkMovie[i].gameObject.GetComponent<MeshRenderer>().sortingLayerName = "High2";
        //}

        for (int i = 0; i < m_binkMovie.Length; i++)
        {
            m_binkMovie[i].gameObject.GetComponent<MeshRenderer>().sortingLayerName = "High2";
        }

        m_binkMovie[(int)_eType.GOLF_CEREMONY].gameObject.GetComponent<MeshRenderer>().sortingOrder = 501;
        m_binkMovie[(int)_eType.CEREMONY_X50].gameObject.GetComponent<MeshRenderer>().sortingOrder = 501;
        m_binkMovie[(int)_eType.CEREMONY_X100].gameObject.GetComponent<MeshRenderer>().sortingOrder = 501;
        m_binkMovie[(int)_eType.CEREMONY_X200].gameObject.GetComponent<MeshRenderer>().sortingOrder = 501;
        yield return null;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(movieLoad());
    }

    public void PlayMovie(_eType eType, Vector3 vPos, bool loop = false)
    {
        if (loop) m_binkMovie[(int)eType].Loop(0);

        m_binkMovie[(int)eType].gameObject.transform.localPosition = vPos;
        m_binkMovie[(int)eType].SetEnable(true, 1, 30);
    }

    public void PlayMovie(_eType eType, bool loop = false)
    {
        if (loop) m_binkMovie[(int)eType].Loop(0);

        m_binkMovie[(int)eType].SetEnable(true, 1, 30);
    }

    public void StopMovie(_eType eType)
    {
        m_binkMovie[(int)eType].SetEnable(false, 1, 30);
    }

    public void StopAllMovie()
    {
        for (int i = 0; i < m_binkMovie.Length; i++)
        {
            m_binkMovie[i].SetEnable(false, 1, 30);
        }
    }

    public IEnumerator CoPlayMovie(_eType eType, bool loop = false)
    {
        if (loop) m_binkMovie[(int)eType].Loop(0);

        m_binkMovie[(int)eType].SetEnable(true, 1, 30);
        while (true)
        {
            if (m_binkMovie[(int)eType].IsEnd())
            {
                StopMovie(eType);
                break;
            }
            yield return null;
        }
    }

    public int GetCurrentFrame(_eType eType)
    {
        return m_binkMovie[(int)eType].GetCurrentFrame();
    }
}