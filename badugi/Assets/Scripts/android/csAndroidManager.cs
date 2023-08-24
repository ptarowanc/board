using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unitycoding;
//using Firebase.Analytics;
using LitJson;
using UnityEngine.Events;


public class csAndroidManager : MonoBehaviour {

	#region Member Variables
	

    public static bool firebaseReady;
    static bool isCheckEndFireBase = false;
    #endregion

    #region Event & Action

    public delegate void DelGoScene();

	DelGoScene EvtGoLoginScene;
	public static System.Action EvtQuit;
	public static System.Action EvtGoMarket;

	Queue<System.Action<bool,object>> m_QProc = new Queue<System.Action<bool,object>>();
	System.Action<bool,string,object,System.Action> m_CBQProc;

	public void AddQ(string Func){m_QProc.Enqueue((bool bRet,object errMsg)=>{StartCoroutine (Func);});}
    #endregion

    #region Variable
    public static csAndroidManager Instance{get;private set;}
    csPopupGlobalMSG m_GlobalPopup;
    csPopupBase[] m_Popups;
    public static string USERID
    {
        get;
        set;
    }
    #endregion

    #region AssetBundle
    static AssetBundle m_StoreAssetBundle;
    static AssetBundle m_AvatarAssetBundle;
    static AssetBundle m_CardAssetBundle;
    public static AssetBundle StoreAsset
    {
        set { m_StoreAssetBundle = value; }
        get { return m_StoreAssetBundle; }
    }
    public static AssetBundle AvatarAsset
    {
        set { m_AvatarAssetBundle = value; }
        get { return m_AvatarAssetBundle; }
    }
    public static AssetBundle CardAsset
    {
        set { m_CardAssetBundle = value; }
        get { return m_CardAssetBundle; }
    }
    void UnLoadAssetBundles()
    {
        if (m_StoreAssetBundle)
        {
            m_StoreAssetBundle.Unload(true);
        }
        if (m_AvatarAssetBundle)
        {
            m_AvatarAssetBundle.Unload(true);
        }
        if (m_CardAssetBundle)
        {
            m_CardAssetBundle.Unload(true);
        }
    }
    #endregion

    #region Mono Function

    void Awake()
	{
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;

        //이벤트 등록
        RegistryEvent();

        //#if UNITY_ANDROID
		DontDestroyOnLoad(gameObject);


		//#else
		//EvtGoLoginScene();
		//DestroyImmediate(gameObject);
		//#endif
	}

	void Start()
	{
        //STEP-1
        //1단계: Google Play 서비스 버전 요건 확인
        //Google Play 서비스가 최신 상태여야 Android용 Firebase Unity SDK를 사용할 수 있습니다.
        //CheckFirebaseIfReady();

        //STEP-2
        //FireBase Cloud Message Initialize
        AddQ("CoInitFCM");

        //STEP-3
        //App Version Check
        AddQ("CoReqSerVersion");

        //STEP-4
        //Patch File Check
        AddQ("CoStartPatch");

        //STEP-5
        //Change NextScene
        AddQ("CoNextScene");

		m_CBQProc (true,string.Empty,string.Empty,null);
	}


    void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //Debug.Log("SceneLoaded 1-1");
        m_Popups = null;
        if (firebaseReady)
        {
            //FirebaseAnalytics.LogEvent(FirebaseAnalytics.ParameterLocation,
            //                                new Firebase.Analytics.Parameter[] {
            //                                new Firebase.Analytics.Parameter("user_id", SystemInfo.deviceUniqueIdentifier),
            //                                new Firebase.Analytics.Parameter("scene_name", arg0.name),
            //                                });
        }


        switch (arg0.name.ToLower())
        {
            case "m_lobby":
                NetworkManager.Instance.SetLobbyLocation(NetworkManager.LOBBYLOCATION.LOBBY);
                break;
            case "m_myroom":
                NetworkManager.Instance.SetLobbyLocation(NetworkManager.LOBBYLOCATION.MYROOM);
                break;
            case "m_store":
                NetworkManager.Instance.SetLobbyLocation(NetworkManager.LOBBYLOCATION.SHOP);
                break;
            default:
                Debug.Log(arg0.name);
                return;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch(SceneManager.GetActiveScene().name.ToLower())
            {
                case "m_Android":
                case "m_login":
                case "m_lobby":
                    ShowPopupGameExit();
                    break;
                case "m_myroom":
                case "m_store":
                    BackKeyToLobby();
                    break;

            }
        }
    }

    void OnApplicationQuit()
	{
		UnRegistryEvent ();
	}

	void OnDestroy()
	{
		UnRegistryEvent ();
        UnLoadAssetBundles();
    }

    #endregion

    #region AddQ Function

    void CheckFirebaseIfReady()
    {
        //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        //    Firebase.DependencyStatus dependencyStatus = task.Result;
        //    if (dependencyStatus == Firebase.DependencyStatus.Available)
        //    {
        //        firebaseReady = true;
        //        Debug.Log("Firebase is ready for use.");
        //        // Create and hold a reference to your FirebaseApp, i.e.
        //        //   app = Firebase.FirebaseApp.DefaultInstance;
        //        // where app is a Firebase.FirebaseApp property of your application class.

        //        // Set a flag here indicating that Firebase is ready to use by your
        //        // application.
        //    }
        //    else
        //    {
        //        firebaseReady = false;
        //        UnityEngine.Debug.Log(System.String.Format(
        //          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //        // Firebase Unity SDK is not safe to use here.
        //    }

        //    isCheckEndFireBase = true;
        //});
    }

    IEnumerator CoInitFCM() 
	{
        yield return null;


        #region Analyitcs
        //Analytics
        //FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //// Set the user's sign up method.
        //FirebaseAnalytics.SetUserProperty(
        //	FirebaseAnalytics.UserPropertySignUpMethod,
        //	Application.platform.ToString());
        //// Set the user ID.
        //FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
        #endregion

        #region Firebase Cloude Message
        //      Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        #endregion

        if (m_CBQProc != null)
        {

            m_CBQProc(true, string.Empty, string.Empty, null);
        }
        //yield return null;

        //     while(!isCheckEndFireBase)
        //      {
        //          yield return null;
        //      }
        //      if (firebaseReady)
        //      {
        //          #region Analyitcs
        //          //Analytics
        //          //FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //          //// Set the user's sign up method.
        //          //FirebaseAnalytics.SetUserProperty(
        //          //    FirebaseAnalytics.UserPropertySignUpMethod,
        //          //    Application.platform.ToString());
        //          ////Set the user ID.
        //          //FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);

        //          #endregion

        //          #region Firebase Cloude Message
        //          //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //          //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        //          #endregion
        //      }
        ////      if (m_CBQProc != null)
        ////{

        ////          m_CBQProc (true,string.Empty,string.Empty,null);
        ////}
    }

	IEnumerator CoReqSerVersion()
	{
		yield return null;

		csWWWiki.RESET_ARGS ();
		csWWWiki.Instance.GET (csFlagDef.URL_VERSION_CHECK);
		while (!csWWWiki.RECV) 
		{
			yield return null;
		}

        System.Action ErrCallBack = EvtQuit;
        bool bSuccess = true;
		string ErrMSG = string.Empty;
		if (csWWWiki.RECV_CODE == csFlagDef.NET_RECV_CODE.S_OK) 
		{
			if(csWWWiki.RECV_VAL.Length == 0)
			{
				bSuccess = false;
				ErrMSG = "서버의 앱버전 정보가 올바르지 않습니다.";
            }
			else
			{
                csJson.js_serv_version jsVer = JsonMapper.ToObject<csJson.js_serv_version>(csWWWiki.RECV_VAL);
                if(jsVer.error != null && jsVer.error.Length > 0)
                {
                    bSuccess = false;
                    ErrMSG = jsVer.error;
                }
                else
                {
                    string[] app_vers = Application.version.Split('.');
                    string[] svr_vers = jsVer.version.Split('.');

                    if (app_vers.Length != 3 || svr_vers.Length != 3)
                    {
                        bSuccess = false;
                        ErrMSG = "버전 정보 형식이 올바르지 않습니다.";
                    }
                    else
                    {
                        
                        bool bMissMatch =
                            (int.Parse(svr_vers[0]) != int.Parse(app_vers[0]) ||
                                int.Parse(svr_vers[1]) != int.Parse(app_vers[1]) ||
                                int.Parse(svr_vers[2]) != int.Parse(app_vers[2])) ? true : false;

                        if (bMissMatch)
                        {
                            if(int.Parse(Application.version.Replace(".","")) < int.Parse(jsVer.version.Replace(".","")))
                            {
                                //bSuccess = false;
                                //ErrMSG = "최신버전이 업데이트 되었습니다.\n새로 다운로드, 설치 후 이용해주세요.";
                                //ErrCallBack = EvtQuit;

                                bSuccess = false;
                                ErrMSG = "최신버전이 업데이트 되었습니다.\n마켓에서 업데이트 후 이용해 주세요.";
                                if (jsVer.market.Length > 0)
                                {
                                    csFlagDef.URL_MARKET = jsVer.market;
                                }
                                ErrCallBack = EvtGoMarket;
                            }
                        }
                    }
                }
			}
		}
		else 
		{
			bSuccess = false;
			ErrMSG = "네트워크 상태가 원활하지 않습니다.\n잠시 후 다시 이용해 주세요.";

		}

        EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, ErrMSG, ErrCallBack);
        if (m_CBQProc != null)
        {
            m_CBQProc(bSuccess, csFlagDef.POPUP_KIND_GLOBALMSG, ErrMSG, ErrCallBack);
        }
    }

	IEnumerator CoStartPatch()
	{
		yield return null;

		csPatchFileDownLoader comPatch = GameObject.FindObjectOfType<csPatchFileDownLoader>();
		if (comPatch == null)
		{
			GameObject patch = new GameObject ("PatchFileDownLoader");
			comPatch = patch.AddComponent<csPatchFileDownLoader> ();
		}
		comPatch.StartDownLoad (csFlagDef.URL_PATCH);
	}

	IEnumerator CoNextScene()
	{
		yield return null;
		EvtGoLoginScene();
	}

	#endregion

	#region Custom Functions

	void RegistryEvent()
	{
		SceneManager.sceneLoaded += SceneLoaded;

		csPatchFileDownLoader.OnStateChange += OnPatchState;

		EvtGoLoginScene = 	()	=>	{SceneManager.LoadScene("M_Login");};
		EvtQuit 		= 	()	=>	{Application.Quit();};
		EvtGoMarket 	= 	() 	=>	{Application.OpenURL (csFlagDef.URL_MARKET);};

        m_CBQProc = (bool bRet, string PopupKind, object errMSG, System.Action cb) =>
        {
            if (bRet)
            {
                if (m_QProc.Count > 0)
                {
                    m_QProc.Dequeue()(bRet, errMSG);
                }
            }
            else
            {
                EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, PopupKind, errMSG, cb);
            }
        };
    }
	void UnRegistryEvent()
	{
		SceneManager.sceneLoaded -= SceneLoaded;

		csPatchFileDownLoader.OnStateChange -= OnPatchState;

        //Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
        //Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;

        EvtGoLoginScene = null;
		EvtQuit = null;
		EvtGoMarket = null;
		m_CBQProc = null;
	}

	void OnPatchState(csPatchFileDownLoader.enSTATE state,float progress,string msg)
	{
		switch (state)
		{
		case csPatchFileDownLoader.enSTATE.FINISH:
			m_CBQProc (true, string.Empty, string.Empty, null);
			break;
		case csPatchFileDownLoader.enSTATE.NET_ERROR:
                //EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, msg, EvtQuit);
            m_CBQProc (false, csFlagDef.POPUP_KIND_GLOBALMSG,msg,EvtQuit);
            break;
		}
	}

    void ShowPopupGameExit()
    {
        if (m_GlobalPopup == null)
        {
            m_GlobalPopup = GameObject.FindObjectOfType<csPopupGlobalMSG>();
        }
        if (m_GlobalPopup != null)
        {
            if (!m_GlobalPopup.ISOPEN)
            {
                System.Action empty = () => { };
                Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_EXIT, (object)string.Empty, empty);
            }
        }
    }
    void BackKeyToLobby()
    {
        if (m_Popups == null)
        {
            m_Popups = GameObject.FindObjectsOfType<csPopupBase>();
        }

        if (m_Popups != null)
        {
            bool bOpen = false;
            for (int i = 0; i < m_Popups.Length; i++)
            {
                if (m_Popups[i].ISOPEN)
                {
                    bOpen = true;
                    break;
                }
            }
            if (!bOpen)
            {
                GoToLobby();
            }
        }
    }

    public void GoToLobby()
    {
        NetworkManager.Instance.SetLobbyLocation(NetworkManager.LOBBYLOCATION.LOBBY);
//#if UNITY_ANDROID
        SceneManager.LoadScene("M_Lobby");
//#else
//        SceneManager.LoadScene("Lobby");
//#endif
    }
    public void GoToMyRoom()
    {
        NetworkManager.Instance.GetMyRoomList(OnRcvMyRoomList);
        if (CoWork != null)
        {
            StopCoroutine(CoWork);
            CoWork = null;
        }
        CoWork = StartCoroutine(CoReqStoreOrMyRoomList(string.Format(csFlagDef.URL_MYROOMLIST, csAndroidManager.USERID), OnRcvMyRoomList));
    }
    Coroutine CoWork = null;
    public void GoToStore()
    {
        NetworkManager.Instance.GetStoreList(OnRcvStoreList);
        if (CoWork != null)
        {
            StopCoroutine(CoWork);
            CoWork = null;
        }
        CoWork = StartCoroutine(CoReqStoreOrMyRoomList(csFlagDef.URL_STORELIST,OnRcvStoreList));
    }
    static void OnRcvStoreList(string argJson)
    {
        csStoreManager.SetProductList(argJson);
     
        if(csStoreManager.m_jsStoreList.error != null && csStoreManager.m_jsStoreList.error.Length > 0)
        {
            System.Action empty = () => { };
            Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)csStoreManager.m_jsStoreList.error, empty);
        }
        else
        {
            SceneManager.LoadScene("M_Store");
        }
        
        
    }
    static void OnRcvMyRoomList(string argJson)
    {
        csMyRoomManager.SetProductList(argJson);

        if(csMyRoomManager.m_jsMyRoomList.error != null && csMyRoomManager.m_jsMyRoomList.error.Length > 0)
        {
            System.Action empty = () => { };
            Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)csMyRoomManager.m_jsMyRoomList.error, empty);
        }
        else
        {
            SceneManager.LoadScene("M_MyRoom");
        }
    }


    IEnumerator CoReqStoreOrMyRoomList(string StrURL, UnityAction<string> callback)
    {
        yield return null;

        csWWWiki.RESET_ARGS();
        csWWWiki.Instance.GET(StrURL);
        while (!csWWWiki.RECV)
        {
            yield return null;
        }

        bool bSuccess = false;
        string ErrMSG = string.Empty;
        if (csWWWiki.RECV_CODE == csFlagDef.NET_RECV_CODE.S_OK)
        {
            if (csWWWiki.RECV_VAL.Length == 0)
            {
                bSuccess = false;
                ErrMSG = "네트워크 상태가 원활하지 않습니다.\n잠시 후 다시 시도해 주세요.";
            }
            else
            {
                bSuccess = true;
                if (callback != null)
                {
                    callback(csWWWiki.RECV_VAL);
                }

            }
        }
        else
        {
            bSuccess = false;
            ErrMSG = "네트워크 상태가 원활하지 않습니다.\n잠시 후 다시 시도해 주세요.";

        }
        if(!bSuccess)
        {
            System.Action empty = () => { };
            Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)ErrMSG, empty);

        }
    }

    #endregion

    #region FCM Receiver / Event Functions

 //   public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) 
	//{
	//	//UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
	//}

	//public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) 
	//{
	//	//UnityEngine.Debug.Log ("Received a new message from: " + e.Message.From);
	//}

	//public static void FBEVT_CLICK(string StrEvtKind,string StrClickName)
	//{
 //       if (firebaseReady)
 //       {

 //           FirebaseAnalytics.LogEvent(StrEvtKind,
 //           new Firebase.Analytics.Parameter[] {
 //           new Firebase.Analytics.Parameter("user_id", SystemInfo.deviceUniqueIdentifier),
 //           new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterItemName, StrClickName),
 //           });
 //       }
	//}
	//public static void FBEVT_CLICK(string StrEvtKind,Firebase.Analytics.Parameter[] ArgParams)
	//{
 //       if (firebaseReady)
 //       {

 //           FirebaseAnalytics.LogEvent(StrEvtKind, ArgParams);
 //       }
	//}
    #endregion

}
