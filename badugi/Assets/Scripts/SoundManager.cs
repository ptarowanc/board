using UnityEngine;
using System.Collections;
using System.IO;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public bool isSound = true;

    public struct _stSoundInfo
    {
        public float fStartFadeVolum;
        public float fEndFadeVolum;
        public bool bFadeFlag;
        public bool bStartFlag;
        public float fVolume;
        public bool bBGM;
        public float fTime;
        public string strName;
    };

    public enum _eSoundResource
    {
        C_DEAL,
        C_HOPEN,
        C_LOSE,
        C_OPEN,
        C_SHUFFLE,
        C_WIN,
        //BBING0,
        //CALL0,
        //CHECK0,
        //DDADANG0,
        //DIE0,
        //EVENING0,
        //HALF0,
        //MORNING0,
        //NOON0,
        //PASS0,
        //QUATER0,
        BBING1,
        CALL1,
        CHECK1,
        DDADANG1,
        DIE1,
        EVENING1,
        HALF1_0,
        HALF1_1,
        HALF1_2,
        HALF1_3,
        HALF1_4,
        HALF1_5,
        MORNING1,
        NOON1,
        PASS1,
        QUATER1,

        C1_1,
        C2_1,
        C3_1,
        C4_1,
        //C1_0,
        //C2_0,
        //C3_0,
        //C4_0,

        MADE,

        NOBASE_1,
        NOBASE_2,
        NOBASE_3,
        NOBASE_4,
        NOBASE_5,
        NOBASE_6,
        NOBASE_7,
        NOBASE_8,
        NOBASE_9,
        NOBASE_10,
        NOBASE_11,
        NOBASE_12,
        NOBASE_13,

        MADE_5,
        MADE_6,
        MADE_7,
        MADE_8,
        MADE_9,
        MADE_10,
        MADE_11,
        MADE_12,
        MADE_13,

        BASE_3,
        BASE_4,
        BASE_5,
        BASE_6,
        BASE_7,
        BASE_8,
        BASE_9,
        BASE_10,
        BASE_11,
        BASE_12,
        BASE_13,

        TWOBASE_2,
        TWOBASE_3,
        TWOBASE_4,
        TWOBASE_5,
        TWOBASE_6,
        TWOBASE_7,
        TWOBASE_8,
        TWOBASE_9,
        TWOBASE_10,
        TWOBASE_11,
        TWOBASE_12,
        TWOBASE_13,

        EVENT_PIG,
        EVENT_HORSE,
        EVENT_DRAGON,

        GOLF,
        SECOND,
        THIRD,
        WINNERCEREMONY,

        GOLFCEREMONY,
        X50,
        X100,
        X200,

        EVENT_INTRO,
        EVENT_SUDDEN,

        //COUNT,

        Z_MAXCNT,
    };

    public AudioSource[] m_asSoundResource;
    _stSoundInfo[] m_stSoundInfo;

    public void SoundOn()
    {
        isSound = true;

        for (int i = 0; i < m_stSoundInfo.Length; ++i)
        {
            m_stSoundInfo[i].fEndFadeVolum = 1.0f;
            m_stSoundInfo[i].fStartFadeVolum = 1.0f;
            m_stSoundInfo[i].fVolume = 1.0f;
        }
    }

    public void SoundOff()
    {
        isSound = false;

        for (int i = 0; i < m_stSoundInfo.Length; ++i)
        {
            m_stSoundInfo[i].fEndFadeVolum = 0.0f;
            m_stSoundInfo[i].fStartFadeVolum = 0.0f;
            m_stSoundInfo[i].fVolume = 0.0f;
        }

        for (int i = 0; i < (int)_eSoundResource.Z_MAXCNT; i++) StopSound((_eSoundResource)i);
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
        LoadFirst();
    }

    public void LoadFirst()
    {
        string[] stFileName =
        {
            "c_deal", "c_hopen", "c_lose", "c_open","c_shuffle", "c_win",
            //"f_bbing", "f_call", "f_check", "f_ddadang", "f_die", "f_evening", "f_half", "f_morning", "f_noon", "f_pass", "f_quater",
            "m_bbing", "m_call", "m_check", "m_ddadang", "m_die", "m_evening", "m_half_0", "m_half_1", "m_half_2", "m_half_3", "m_half_4", "m_half_5", "m_morning", "m_noon", "m_pass", "m_quater",
            "m_c1","m_c2","m_c3","m_c4",/*"f_c1","f_c2","f_c3","f_c4",*/"made",
            "nobase_1","nobase_2","nobase_3","nobase_4","nobase_5","nobase_6","nobase_7","nobase_8","nobase_9","nobase_10","nobase_11","nobase_12","nobase_13",
            "made_5","made_6","made_7","made_8","made_9","made_10","made_11","made_12","made_13",
            "base_3","base_4","base_5","base_6","base_7","base_8","base_9","base_10","base_11","base_12","base_13",
            "twobase_2","twobase_3","twobase_4","twobase_5","twobase_6","twobase_7","twobase_8","twobase_9","twobase_10","twobase_11","twobase_12","twobase_13",
            "event_pig", "event_horse", "event_dragon",
            "m_golf", "m_second", "m_third", "winnerceremony", "golfceremony", "x50", "x100", "x200",
            "event_intro", "event_sudden"
        };

        for (int i = 0; i < stFileName.Length; ++i)
        {
            AudioClip acResource;// = new AudioClip();

            //            if( ITestManager.Instance.m_bTestCode && Directory.Exists(Application.dataPath + "/Sounds/") == true)
            //            {
            //                string url = "file://" + Application.dataPath + "/Sounds/" + stFileName[i] + ".wav";
            //                WWW audioLoader = new WWW(url);
            //                while (!audioLoader.isDone)
            //                {
            //                    //Debug.Log("uploading");
            //                }
            //                acResource = audioLoader.GetAudioClip(false, false, AudioType.WAV);
            //            }
            //            else
            {
                acResource = Resources.Load("sounds/" + stFileName[i], typeof(AudioClip)) as AudioClip;
            }

            m_asSoundResource[i].clip = acResource;
            m_asSoundResource[i].Stop();
        }

        m_stSoundInfo = new _stSoundInfo[m_asSoundResource.Length];
        for (int i = 0; i < stFileName.Length; ++i)
        {
            m_stSoundInfo[i].bFadeFlag = true;
            m_stSoundInfo[i].bStartFlag = false;
            m_stSoundInfo[i].fEndFadeVolum = 1.0f;
            m_stSoundInfo[i].fStartFadeVolum = 1.0f;
            m_stSoundInfo[i].fVolume = 1.0f;
            m_stSoundInfo[i].bBGM = false;
            m_stSoundInfo[i].fTime = 1.0f;
            m_stSoundInfo[i].strName = stFileName[i];
        }
    }
    // Use this for initialization
    void Start()
    {
        //Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //PlaySound(_eSoundResource.C1_0, false);
            //PlaySound(_eSoundResource.C_WIN, false);
            //PlaySound(_eSoundResource.CHECK0, false);
        }

        if (m_stSoundInfo == null)
            return;

        for (int i = 0; i < m_stSoundInfo.Length; ++i)
        {
            float fDeltaTime = Time.deltaTime / m_stSoundInfo[i].fTime;
            if (m_stSoundInfo[i].bStartFlag)
            {
                if (m_stSoundInfo[i].bFadeFlag)
                {
                    if (m_stSoundInfo[i].fStartFadeVolum < m_stSoundInfo[i].fEndFadeVolum)
                    {
                        m_stSoundInfo[i].fStartFadeVolum += fDeltaTime;
                    }
                    else
                    {
                        m_stSoundInfo[i].fStartFadeVolum = m_stSoundInfo[i].fEndFadeVolum;
                        m_stSoundInfo[i].bStartFlag = false;
                    }
                }
                else
                {
                    if (m_stSoundInfo[i].fStartFadeVolum > m_stSoundInfo[i].fEndFadeVolum)
                    {
                        m_stSoundInfo[i].fStartFadeVolum -= fDeltaTime;
                    }
                    else
                    {
                        m_stSoundInfo[i].fStartFadeVolum = m_stSoundInfo[i].fEndFadeVolum;
                        m_stSoundInfo[i].bStartFlag = false;
                        //if( m_stSoundInfo[i].fStartFadeVolum <= 0.0f )
                        if (m_asSoundResource[i] != null)
                        {
                            //Debug.Log("sound null : " + i);
                            m_asSoundResource[i].Stop();
                        }
                    }
                }
                m_asSoundResource[i].volume = m_stSoundInfo[i].fStartFadeVolum;
            }
        }
    }

    public bool IsPlaying(_eSoundResource eResource)
    {
        return m_asSoundResource[(int)eResource].isPlaying;
    }

    public void PlaySound(_eSoundResource eResource, bool bLoop, float volum = 1.0f)
    {
        if (m_asSoundResource[(int)eResource] == null)
        {
            Debug.LogError("sound null : " + (int)eResource);
            return;
        }

        m_asSoundResource[(int)eResource].volume = volum;
        m_asSoundResource[(int)eResource].loop = bLoop;
        m_asSoundResource[(int)eResource].Play();
        m_stSoundInfo[(int)eResource].fStartFadeVolum = 1.0f;
        m_stSoundInfo[(int)eResource].bStartFlag = false;
    }

    public void StopSound(_eSoundResource eResource)
    {
        m_asSoundResource[(int)eResource].Stop();
    }

    public void FadeVolum(_eSoundResource eResource, float fStartVolum, float fEndVolum, float fTime, bool bFade)
    {
        if (m_stSoundInfo == null)
            return;

        m_stSoundInfo[(int)eResource].fStartFadeVolum = m_stSoundInfo[(int)eResource].fVolume;
        m_stSoundInfo[(int)eResource].fEndFadeVolum = fEndVolum;
        m_stSoundInfo[(int)eResource].fTime = fTime;
        m_stSoundInfo[(int)eResource].bFadeFlag = bFade;
        m_stSoundInfo[(int)eResource].bStartFlag = true;
    }


    public _stSoundInfo[] GetSoundInfo()
    {
        return m_stSoundInfo;
    }

    public void SetSoundVolume(int iIndex, float fVolume)
    {
        m_stSoundInfo[iIndex].fVolume = fVolume;
    }

    public void SetSoundBGM(int iIndex, bool bBGM)
    {
        m_stSoundInfo[iIndex].bBGM = bBGM;
    }
}