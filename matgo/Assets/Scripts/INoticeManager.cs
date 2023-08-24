using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class INoticeManager : MonoBehaviour {

    public static INoticeManager Instance;

    public enum _eNoticeType
    {
        ENT_CENTER = 0,
        ENT_TOP,

        ENT_MAXCNT
    }

    public struct _stNoticeInfo
    {
        public string strMsg;
        public float fSpeed;
        public float fMoveDistance;
    }

    [SerializeField]
    GameObject[] m_objNoticeFrameImage;

    [SerializeField]
    GameObject[] m_objNoticeText;

    [SerializeField]
    RectTransform [] m_rtTextTransfrom;
        
    ArrayList m_aryCenterNoticeList = new ArrayList();
    ArrayList m_aryTopNoticeList = new ArrayList();

    //GameObject m_objBetMoney = null;
    void Awake()
    {
        Instance = this;        
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(coCenterNoticeUpdate());
        StartCoroutine(coTopNoticeUpdate());
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    if (Random.Range(0, 100) < 50)
        //        AddNotice("잠시후 5시 30분에 서버 다운이 있을 예정입니다. 게임을 종료해 주시기 바랍니다.", 10.0f, 0.0f);
        //    else
        //        AddNotice("일반1 5000점 방에서 장비가 용 잭팟당첨 축하합니다. test1님 ㅁ넝레ㅐ벚레ㅐㅓㅂ제ㅐㅓ레ㅐㅓㅈ데래ㅓㅈ베ㅐㅓ레ㅐㅂ절", 10.0f, 0.0f);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    if (Random.Range(0, 100) < 50)
        //        AddNotice("잠시후 5시 30분에 서버 다운이 있을 예정입니다. 게임을 종료해 주시기 바랍니다.", 10.0f, 0.0f);
        //    else
        //        AddNotice("일반1 5000점 방에서 장비가 용 잭팟당첨 축하합니다. test1님 ㅁ넝레ㅐ벚레ㅐㅓㅂ제ㅐㅓ레ㅐㅓㅈ데래ㅓㅈ베ㅐㅓ레ㅐㅂ절", 10.0f, 0.0f);
        //}
    }
    IEnumerator coCenterNoticeUpdate()
    {
        while (true)
        {
            if (m_aryCenterNoticeList.Count > 0)
            {
                _stNoticeInfo stInfo = (_stNoticeInfo)m_aryCenterNoticeList[0];

                _eNoticeType eType = _eNoticeType.ENT_CENTER;

                float[] fWidth = { 1280.0f, 677.0f };
                yield return StartCoroutine(coScrollingText(eType, stInfo, fWidth[(int)eType]));

                if (m_aryCenterNoticeList.Count > 0 )
                    m_aryCenterNoticeList.RemoveAt(0);
            }
            yield return null;
        }
    }

    IEnumerator coTopNoticeUpdate()
    {
        while (true)
        {
            if (m_aryTopNoticeList.Count > 0)
            {
                _stNoticeInfo stInfo = (_stNoticeInfo)m_aryTopNoticeList[0];

                _eNoticeType eType = _eNoticeType.ENT_TOP;
                //if (SceneManager.GetActiveScene().name == "M_Game")
                //{
                //    m_objBetMoney = GameObject.Find("Text_Bet");
                //}
                float[] fWidth = { 1280.0f, 677.0f };
                yield return StartCoroutine(coScrollingText(eType, stInfo, fWidth[(int)eType]));

                if(m_aryTopNoticeList.Count > 0 )
                    m_aryTopNoticeList.RemoveAt(0);
            }
            yield return null;
        }
    }

    //IEnumerator coCenterScrollingText(_stNoticeInfo stInfo)
    //{
    //    int iType = (int)_eNoticeType.ENT_CENTER;
                
    //    m_objNoticeText[iType].SetActive(true);
    //    m_objNoticeFrameImage[iType].SetActive(true);

    //    Text txNotice = m_objNoticeText[iType].GetComponent<Text>();
    //    txNotice.text = stInfo.strMsg;

    //    float fWidth = txNotice.preferredWidth;
    //    Vector3 vStartPos = m_rtTextTransfrom[iType].position;
    //    float fScrollPosition = 0.0f;

    //    while (true)
    //    {
    //        m_rtTextTransfrom[iType].position = new Vector3(-fScrollPosition % fWidth, vStartPos.y, vStartPos.z);
    //        fScrollPosition += stInfo.fSpeed * 20 * Time.deltaTime;

    //        if (fScrollPosition > fWidth + 1280.0f)
    //            break;
    //        yield return null;
    //    }

    //    m_objNoticeText[iType].SetActive(false);
    //    m_objNoticeFrameImage[iType].SetActive(false);
    //}

    void MoveListCenterToTop()
    {
        for( int i = 0; i < m_aryCenterNoticeList.Count; ++i )
        {
            _stNoticeInfo stInfo = (_stNoticeInfo)m_aryCenterNoticeList[i];
            AddNotice(stInfo.strMsg, stInfo.fSpeed, stInfo.fMoveDistance);
        }
        m_aryCenterNoticeList.Clear();
    }

    void MoveListTopToCenter()
    {
        for (int i = 0; i < m_aryTopNoticeList.Count; ++i)
        {
            _stNoticeInfo stInfo = (_stNoticeInfo)m_aryTopNoticeList[i];
            AddNotice(stInfo.strMsg, stInfo.fSpeed, stInfo.fMoveDistance);
        }
        m_aryTopNoticeList.Clear();
    }

    IEnumerator coScrollingText(_eNoticeType eType, _stNoticeInfo stInfo, float fSizeX)
    {
        int iType = (int)eType;

        //if (SceneManager.GetActiveScene().name == "M_Game" &&
        //    eType == _eNoticeType.ENT_TOP &&
        //    m_objBetMoney != null)
        //{
        //    m_objBetMoney.SetActive(false);
        //}

        m_objNoticeText[iType].SetActive(true);
        m_objNoticeFrameImage[iType].SetActive(true);

        Text txNotice = m_objNoticeText[iType].GetComponent<Text>();
        txNotice.text = stInfo.strMsg;

        float [] fFontSize = { 49.0f, 26.0f };
        float fWidth = txNotice.text.Length * fFontSize[iType];

        Vector3 vStartPos = m_rtTextTransfrom[iType].position;
        float[] fStartPosX = { 2033.0f, 1724.0f };
        float fScrollPosition = fStartPosX[iType];
        float fScrollingPosX = stInfo.fMoveDistance;

        fScrollPosition -= stInfo.fMoveDistance;

        while (true)
        {
            m_rtTextTransfrom[iType].position = new Vector3(fScrollPosition, vStartPos.y, vStartPos.z);
            float fMoveDistance = stInfo.fSpeed * 20 * Time.deltaTime; ;
            fScrollPosition -= fMoveDistance;
            fScrollingPosX += fMoveDistance;
            
            if (fScrollingPosX > fWidth + fSizeX)
                break;

            if (eType == _eNoticeType.ENT_CENTER &&
                SceneManager.GetActiveScene().name.Contains("M_Game"))
            {
                AddNotice(stInfo.strMsg, stInfo.fSpeed, fScrollingPosX);
                if (m_aryCenterNoticeList.Count > 0)
                {
                    m_aryCenterNoticeList.RemoveAt(0);
                    MoveListCenterToTop();
                }
                break;
            }
            else if (eType == _eNoticeType.ENT_TOP &&
                SceneManager.GetActiveScene().name.Contains("M_Game") == false)
            {
                AddNotice(stInfo.strMsg, stInfo.fSpeed, fScrollingPosX);
                if (m_aryTopNoticeList.Count > 0)
                {
                    m_aryTopNoticeList.RemoveAt(0);
                    MoveListTopToCenter();
                }
                break;
            }

            yield return null;
        }

        m_objNoticeText[iType].SetActive(false);
        m_objNoticeFrameImage[iType].SetActive(false);

        //if (eType == _eNoticeType.ENT_TOP &&
        //    m_objBetMoney != null)
        //    m_objBetMoney.SetActive(true);

        yield return new WaitForSeconds(1.0f);
    }

    public void AddNotice(string strMsg, float fSpeed, float fMoveDistance)
    {
        _stNoticeInfo stInfo = new _stNoticeInfo();

        stInfo.strMsg = strMsg;
        stInfo.fSpeed = fSpeed;
        stInfo.fMoveDistance = fMoveDistance;

        if(SceneManager.GetActiveScene().name.Contains("M_Game"))
        {
            m_aryTopNoticeList.Add(stInfo);
        }
        else
        {
            m_aryCenterNoticeList.Add(stInfo);
        }        
    }
}
