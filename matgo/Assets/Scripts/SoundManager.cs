using UnityEngine;
using System.Collections;
using System.IO;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public bool isSound = true;

    public struct _stSoundInfo
    {
        public float    fStartFadeVolum;
        public float    fEndFadeVolum;
        public bool     bFadeFlag;
        public bool     bStartFlag;
        public float    fVolume;
        public bool     bBGM;
        public float    fTime;
        public string   strName;
    };

    public enum _eSoundResource
    {
        ESR_1GO__0 = 0,
        ESR_2GO__0,
        ESR_3GO__0,
        ESR_4GO__0,
        ESR_5GO__0,
        ESR_6GO__0,
        ESR_7GO__0,
        ESR_8GO,
        ESR_9GO,
        ESR_BOMB1__0,
        ESR_BOMB2__0,
        ESR_BOMB3__0,
        ESR_CLEAR1__0,
        ESR_CLEAR2__0,
        ESR_CLEAR3__0,
        ESR_KISS__0,
        ESR_PPUCK1__0,
        ESR_PPUCK2__0,
        ESR_PPUCK3__0,
        ESR_SHAKING1__0,
        ESR_SHAKING2__0,
        ESR_SHAKING3__0,
        ESR_DDADAK1__0,
        ESR_DDADAK2__0,
        ESR_DDADAK3__0,
        ESR_DEAL1,
        ESR_DEAL2,
        ESR_DEAL3,
        ESR_DEAL4,
        ESR_PUT1,
        ESR_PUT2,
        ESR_PUT3,
        ESR_PUT4,
        ESR_GAJUWA1__0,
        ESR_GAJUWA2__0,

        ESR_1GO2__0,
        ESR_1GO3__0,
        ESR_2GO2__0,
        ESR_2GO3__0,
        ESR_3GO2__0,
        ESR_3GO3__0,
        ESR_4GO2__0,
        ESR_4GO3__0,
        ESR_5GO2__0,
        ESR_5GO3__0,
        ESR_6GO2__0,
        ESR_6GO3__0,
        ESR_7GO2__0,
        ESR_7GO3__0,
        ESR_3KWANG0__0,
        ESR_3KWANG1__0,
        ESR_3KWANG2__0,
        ESR_4KWANG0__0,
        ESR_4KWANG1__0,
        ESR_4KWANG2__0,
        ESR_5KWANG0__0,
        ESR_5KWANG1__0,
        ESR_5KWANG2__0,
        ESR_B3KWANG0__0,
        ESR_B3KWANG1__0,
        ESR_B3KWANG2__0,
        ESR_CHEONGDAN0__0,
        ESR_CHEONGDAN1__0,
        ESR_CHEONGDAN2__0,
        ESR_CHODAN0__0,
        ESR_CHODAN1__0,
        ESR_CHODAN2__0,
        ESR_HONGDAN0__0,
        ESR_HONGDAN1__0,
        ESR_HONGDAN2__0,
        ESR_GODORI0__0,
        ESR_GODORI1__0,
        ESR_GODORI2__0,
        ESR_CHONGTONG__0,
        ESR_GAMELOSE,
        ESR_GAMESTART,
        ESR_GAMEWIN,
        ESR_JOIN,
        ESR_FIRST,
        ESR_EVENTINTRO,

        ESR_1GO__1,
        ESR_2GO__1,
        ESR_3GO__1,
        ESR_4GO__1,
        ESR_5GO__1,
        ESR_6GO__1,
        ESR_7GO__1,
        ESR_BOMB1__1,
        ESR_BOMB2__1,
        ESR_BOMB3__1,
        ESR_CLEAR1__1,
        ESR_CLEAR2__1,
        ESR_CLEAR3__1,
        ESR_KISS__1,
        ESR_PPUCK1__1,
        ESR_PPUCK2__1,
        ESR_PPUCK3__1,
        ESR_SHAKING1__1,
        ESR_SHAKING2__1,
        ESR_SHAKING3__1,
        ESR_DDADAK1__1,
        ESR_DDADAK2__1,
        ESR_DDADAK3__1,
        ESR_GAJUWA1__1,
        ESR_GAJUWA2__1,
        ESR_1GO2__1,
        ESR_1GO3__1,
        ESR_2GO2__1,
        ESR_2GO3__1,
        ESR_3GO2__1,
        ESR_3GO3__1,
        ESR_4GO2__1,
        ESR_4GO3__1,
        ESR_5GO2__1,
        ESR_5GO3__1,
        ESR_6GO2__1,
        ESR_6GO3__1,
        ESR_7GO2__1,
        ESR_7GO3__1,
        ESR_3KWANG0__1,
        ESR_3KWANG1__1,
        ESR_3KWANG2__1,
        ESR_4KWANG0__1,
        ESR_4KWANG1__1,
        ESR_4KWANG2__1,
        ESR_5KWANG0__1,
        ESR_5KWANG1__1,
        ESR_5KWANG2__1,
        ESR_B3KWANG0__1,
        ESR_B3KWANG1__1,
        ESR_B3KWANG2__1,
        ESR_CHEONGDAN0__1,
        ESR_CHEONGDAN1__1,
        ESR_CHEONGDAN2__1,
        ESR_CHODAN0__1,
        ESR_CHODAN1__1,
        ESR_CHODAN2__1,
        ESR_HONGDAN0__1,
        ESR_HONGDAN1__1,
        ESR_HONGDAN2__1,
        ESR_GODORI0__1,
        ESR_GODORI1__1,
        ESR_GODORI2__1,
        ESR_CHONGTONG__1,

        ESR_MAXCNT,
    };

    public AudioSource[] m_asSoundResource;
    _stSoundInfo [] m_stSoundInfo;

    public void SoundOn()
    {
        isSound = true;
        
        PlayerPrefs.SetString("sound", "on");

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

        PlayerPrefs.SetString("sound", "off");

        for (int i = 0; i < m_stSoundInfo.Length; ++i)
        {
            m_stSoundInfo[i].fEndFadeVolum = 0.0f;
            m_stSoundInfo[i].fStartFadeVolum = 0.0f;
            m_stSoundInfo[i].fVolume = 0.0f;
        }

        for (int i = 0; i < (int)_eSoundResource.ESR_MAXCNT; i++) StopSound((_eSoundResource)i);
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
        LoadFirst();

        var sound = PlayerPrefs.GetString("sound", string.Empty);

        if(sound == "on")
        {
            if(isSound == false)
            {
                SoundOn();
            }
        }
        else
        {
            if (isSound == true)
            {
                SoundOff();
            }
        }
    }

    public void LoadFirst()
    {
        string[] stFileName =
        {
            "1go", "2go", "3go", "4go", "5go", "6go", "7go", "8go", "9go",
            "bomb1", "bomb2", "bomb3", "clear1", "clear2", "clear3", "kiss", "ppuck1", "ppuck2", "ppuck3", "shaking1", "shaking2", "shaking3",
            "ddadak1", "ddadak2", "ddadak3", "deal", "deal", "deal", "deal", "put1", "put2", "put3", "put4", "gajuwa1", "gajuwa2",
            "1go_2", "1go_3", "2go_2", "2go_3", "3go_2", "3go_3", "4go_2", "4go_3", "5go_2", "5go_3", "6go_2", "6go_3", "7go_2", "7go_3",
            "3kwang0", "3kwang1", "3kwang2", "4kwang0", "4kwang1", "4kwang2", "5kwang0", "5kwang1", "5kwang2", "b3kwang0", "b3kwang1", "b3kwang2",
            "cheongdan0", "cheongdan1", "cheongdan2", "chodan0", "chodan1", "chodan2", "hongdan0", "hongdan1", "hongdan2", "godori0", "godori1", "godori2",
            "chongtong", "gamelose", "gamestart", "gamewin", "join", "first","eventintro",
        };

        for (int i = 0; i < stFileName.Length; ++i)
        {
            AudioClip acResource;// = AudioClip.Create(stFileName[i], 0, 1, 440, true);

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
                acResource = Resources.Load("Sounds/" + stFileName[i] , typeof(AudioClip)) as AudioClip;            
            }           

            m_asSoundResource[i].clip = acResource;
            m_asSoundResource[i].Stop();
        }

        string[] stFileName2 =
        {
            "1go", "2go", "3go", "4go", "5go", "6go", "7go",
            "bomb1", "bomb2", "bomb3", "clear1", "clear2", "clear3", "kiss", "ppuck1", "ppuck2", "ppuck3", "shaking1", "shaking2", "shaking3",
            "ddadak1", "ddadak2", "ddadak3","gajuwa1", "gajuwa2",
            "1go_2", "1go_3", "2go_2", "2go_3", "3go_2", "3go_3", "4go_2", "4go_3", "5go_2", "5go_3", "6go_2", "6go_3", "7go_2", "7go_3",
            "3kwang0", "3kwang1", "3kwang2", "4kwang0", "4kwang1", "4kwang2", "5kwang0", "5kwang1", "5kwang2", "b3kwang0", "b3kwang1", "b3kwang2",
            "cheongdan0", "cheongdan1", "cheongdan2", "chodan0", "chodan1", "chodan2", "hongdan0", "hongdan1", "hongdan2", "godori0", "godori1", "godori2",
            "chongtong"
        };

        for (int i = stFileName.Length, j = 0; i < stFileName2.Length + stFileName.Length; ++i, j++)
        {
            AudioClip acResource;// = AudioClip.Create(stFileName2[i], 0, 1, 440, true);

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
                acResource = Resources.Load("Sounds/female/" + stFileName2[j], typeof(AudioClip)) as AudioClip;
            }

            m_asSoundResource[i].clip = acResource;
            m_asSoundResource[i].Stop();
        }

        m_stSoundInfo = new _stSoundInfo[m_asSoundResource.Length];

        for( int i = 0; i < stFileName.Length; ++i )
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

        for (int i = stFileName.Length, j = 0; i < stFileName2.Length + stFileName.Length; ++i, j++)
        {
            m_stSoundInfo[i].bFadeFlag = true;
            m_stSoundInfo[i].bStartFlag = false;
            m_stSoundInfo[i].fEndFadeVolum = 1.0f;
            m_stSoundInfo[i].fStartFadeVolum = 1.0f;
            m_stSoundInfo[i].fVolume = 1.0f;
            m_stSoundInfo[i].bBGM = false;
            m_stSoundInfo[i].fTime = 1.0f;
            m_stSoundInfo[i].strName = stFileName2[j];
        }
    }
    // Use this for initialization
    void Start () {
        //Load();
    }

    // Update is called once per frame
    void Update ()
    {
        if ( m_stSoundInfo == null )
            return;

        for( int i = 0; i < m_stSoundInfo.Length; ++i )
        {
            float fDeltaTime = Time.deltaTime / m_stSoundInfo[i].fTime;
            if( m_stSoundInfo[i].bStartFlag )
            {
                if( m_stSoundInfo[i].bFadeFlag )
                {
                    if( m_stSoundInfo[i].fStartFadeVolum < m_stSoundInfo[i].fEndFadeVolum )
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
                    if( m_stSoundInfo[i].fStartFadeVolum > m_stSoundInfo[i].fEndFadeVolum )
                    {
                        m_stSoundInfo[i].fStartFadeVolum -= fDeltaTime;
                    }
                    else
                    {
                        m_stSoundInfo[i].fStartFadeVolum = m_stSoundInfo[i].fEndFadeVolum;
                        m_stSoundInfo[i].bStartFlag = false;
                        //if( m_stSoundInfo[i].fStartFadeVolum <= 0.0f )
                        if( m_asSoundResource[i] != null )
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
        return m_asSoundResource [(int)eResource].isPlaying;
    }

    public void PlaySound(_eSoundResource eResource, bool bLoop)
    {
        if( m_asSoundResource [(int)eResource] == null ) 
        {
            Debug.Log("sound null : " + (int)eResource);
            return;
        }
        //m_asSoundResource [(int)eResource].Stop();

        m_asSoundResource[(int)eResource].volume = m_stSoundInfo [(int)eResource].fVolume;     
        m_asSoundResource [(int)eResource].loop = bLoop;
        m_asSoundResource [(int)eResource].Play ();
        m_stSoundInfo [(int)eResource].fStartFadeVolum = 1.0f;      
        m_stSoundInfo [(int)eResource].bStartFlag       = false;
    }

    public void StopSound(_eSoundResource eResource)
    {
        m_asSoundResource [(int)eResource].Stop ();
    }

    public void FadeVolum(_eSoundResource eResource, float fStartVolum, float fEndVolum, float fTime, bool bFade)
    {
        if( m_stSoundInfo == null )
            return;

        m_stSoundInfo [(int)eResource].fStartFadeVolum  = m_stSoundInfo [(int)eResource].fVolume;
        m_stSoundInfo [(int)eResource].fEndFadeVolum    = fEndVolum;
        m_stSoundInfo [(int)eResource].fTime            = fTime;
        m_stSoundInfo [(int)eResource].bFadeFlag        = bFade;
        m_stSoundInfo [(int)eResource].bStartFlag       = true;
    }

 
    public _stSoundInfo [] GetSoundInfo()
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