using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class csWWWiki : CSingletonMonobehaviour<csWWWiki> {

	#region Event Variables & Function
	event Action<bool,string> OnLoading;
	public void AddEventLoading(Action<bool,string> func)
	{
		if ( this.OnLoading != null )
		{
			foreach ( Delegate existingHandler in this.OnLoading.GetInvocationList() )
			{
				if (existingHandler.Equals(func))
				{
					return;
				}
			}
		}

		OnLoading += func;
	}
	public void RemoveEventAt(Action<bool,string> func)
	{
		if (OnLoading != null)
		{
			OnLoading -= func;
		}
	}
	public void RemoveRedundancies()
	{
		ArrayList ArrRemove = new ArrayList ();
		if ( this.OnLoading != null )
		{
			foreach ( Delegate existingHandler in this.OnLoading.GetInvocationList() )
			{
				if (existingHandler.Target.Equals (null))
				{
					ArrRemove.Add (existingHandler);
				}
			}
			for (int i = 0; i < ArrRemove.Count; i++)
			{
				OnLoading -= (Action<bool,string>)ArrRemove [i];
			}
			ArrRemove.Clear ();
		}

	}
	void OnSceneUnLoaded(Scene scene)
	{
		RemoveRedundancies ();
	}
	#endregion


	static float m_fTimeOut = 15;
	public static float TIME_OUT_LIMIT
	{
		get{ return m_fTimeOut; }
		set{ m_fTimeOut = value; }
	}
	private static int COUNT = 0;
	int index = 0;

	static bool m_bIsCript = false;
    static bool m_bIsHex = false;
	static bool m_bReq = false;
	static bool m_bRecv = false;
	static string m_StrRecvVal = "";
	static csFlagDef.NET_RECV_CODE m_enRecvCode = csFlagDef.NET_RECV_CODE.NONE;
	public static bool REQ
	{
		get{ return m_bReq;}
		set{ m_bReq = value;}
	}
	public static bool RECV
	{
		get{ return m_bRecv;}
		set{ m_bRecv = value;}
	}
	public static string RECV_VAL
	{
		get{ return m_StrRecvVal;}
	}
	public static csFlagDef.NET_RECV_CODE RECV_CODE
	{
		get{ return m_enRecvCode;}
	}
	public static bool IS_CRIPT
	{
		get{ return m_bIsCript;}
		set{ m_bIsCript = false;}
	}
    public static bool IS_HEX
    {
        get{ return m_bIsHex; }
        set{ m_bIsHex = value; }
    }

	public static void RESET_ARGS()
	{
		m_bIsCript = false;
        m_bIsHex = false;
		m_bReq = false;
		m_bRecv = false;
		m_StrRecvVal = "";
		m_enRecvCode = csFlagDef.NET_RECV_CODE.NONE;
	}

	void Awake()
	{
		SceneManager.sceneUnloaded += OnSceneUnLoaded;

		index = COUNT;
		COUNT++;

		if(index > 0)
		{
			DestroyImmediate(gameObject);
			return;
		}

		gameObject.name = "wwwmng";


		if (transform.parent == gameObject.transform)
		{
			DontDestroyOnLoad (transform.gameObject); 
		}
	}


	public WWW GET(string argURL,string arg = "") 
	{ 
		
		m_bReq = true;
		m_bRecv = false;
		m_StrRecvVal = "";

		string url = argURL;
		if(m_bIsCript)
		{
			if(arg.Length > 0)
			{
                arg = csCryptMng.EncryptString_AES256 (arg, csCryptMng.KEY_AES256,m_bIsHex);
			}
		}
		url += arg;
        //csLog.Show(url);


        WWW www = new WWW(url); 
		StartCoroutine(WaitForRequest(www)); 

		if (OnLoading != null)
		{
			OnLoading (true,csFlagDef.GetStringOfKindURL(argURL));
		}

		return www; 
	} 
	
	public WWW POST(string argURL, Dictionary<string, string> post) 
	{ 
		m_bReq = true;
		m_bRecv = false;
		m_StrRecvVal = "";

		string url = argURL;
		WWWForm form = new WWWForm(); 

		//csLog.Show("url:" + url);
		foreach (KeyValuePair<string, string> post_arg in post) 
		{ 
			string enc_val = post_arg.Value;
			//csLog.Show("arg:" + post_arg.Key + ",val:" + enc_val);
			if(enc_val == null)
			{
				enc_val = "";
			}
			if(post_arg.Value.Length > 0)
			{
				if(m_bIsCript)
				{
                    enc_val = csCryptMng.EncryptString_AES256(enc_val,csCryptMng.KEY_AES256,m_bIsHex);
				}
			}
			//csLog.Show("arg:" + post_arg.Key + ",val:" + enc_val);
			form.AddField(post_arg.Key, enc_val); 

		} 

        WWW www = new WWW(url, form); 
		StartCoroutine(WaitForRequest(www)); 
		if (OnLoading != null)
		{
			OnLoading (true,csFlagDef.GetStringOfKindURL(argURL));
		}

		return www; 
	} 
	
	
	private IEnumerator WaitForRequest(WWW www) 
	{ 
		float timer = 0;
		bool bFailed = false;
		while(!www.isDone)
		{
			if(timer > TIME_OUT_LIMIT){ bFailed = true; break; }
			timer += Time.deltaTime;
			yield return null;
		}

		if (!bFailed)
		{
			// check for errors 
			if (www.error == null)
			{ 
				m_StrRecvVal = "";
				if (www.text.Length > 0)
				{
					if (m_bIsCript)
					{
						m_StrRecvVal = csCryptMng.DecryptString_AES256 (www.text, csCryptMng.KEY_AES256, m_bIsHex);
					}
					else
					{
						m_StrRecvVal = www.text;
					}
					m_enRecvCode = csFlagDef.NET_RECV_CODE.S_OK;
				}
				else
				{
					m_enRecvCode = csFlagDef.NET_RECV_CODE.S_FALSE;
				}
			
			}
			else
			{ 
				//csLog.Show("WWW Error: " + www.error); 
				m_StrRecvVal = "";
				m_enRecvCode = csFlagDef.NET_RECV_CODE.S_FALSE;
			}
		}
		else
		{
			m_StrRecvVal = "";
			m_enRecvCode = csFlagDef.NET_RECV_CODE.S_FALSE;
		}

		m_bRecv = true;
		m_bReq = false;

		if (OnLoading != null)
		{
			OnLoading (false,string.Empty);
		}

		//csLog.Show("RECV Info : " + m_StrRecvVal + " / " + m_enRecvCode);

		www.Dispose ();
		www = null;
	} 

}
