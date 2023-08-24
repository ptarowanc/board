using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitycoding;

public class csPopupManager : MonoBehaviour {

	#region CallBack Event 

	protected System.Action<string,object,System.Action> m_OpenCB;

	protected System.Action<csPopupBase,string> m_RegPopupCB;
	protected System.Action<string> m_UnRegPopupCB;

	#endregion

	#region Member Variable
	Hashtable m_HtRegPopupObjs = new Hashtable();

	string REG_POPUP_NAME = "REG_{0}";
	#endregion

	#region Mono Functions
	void Awake()
	{
		DontDestroyOnLoad (gameObject);
		RegEvent ();
	}

	void OnDestroy()
	{
		UnRegEvent ();
	}

	void OnApplicationQuit()
	{
		UnRegEvent ();
	}

	#endregion

	#region Custom Functions
	void RegEvent()
	{

		m_RegPopupCB = (csPopupBase argPopupObj, string argPopupName) =>
		{
			string key = string.Format(REG_POPUP_NAME,argPopupName);
			if(!m_HtRegPopupObjs.ContainsKey(key))
			{
				m_HtRegPopupObjs.Add(key,argPopupObj);
			}
		};
		EventHandler.Register (csFlagDef.EVTNAME_REGISTRY_POPUP_INFO, m_RegPopupCB);

		m_UnRegPopupCB = (string argPopupName) =>
		{
			string key = string.Format(REG_POPUP_NAME,argPopupName);
			if(m_HtRegPopupObjs.ContainsKey(key))
			{
				m_HtRegPopupObjs.Remove(key);
			}
		};
		EventHandler.Register (csFlagDef.EVTNAME_UNREGISTRY_POPUP_INFO, m_UnRegPopupCB);


		m_OpenCB = (string argPopupName, object argMSG,System.Action argCloseCB) =>
		{
			string key = string.Format(REG_POPUP_NAME,argPopupName);
			if(m_HtRegPopupObjs.ContainsKey(key))
			{
				((csPopupBase)(m_HtRegPopupObjs[key])).ShowPopup(argMSG,argCloseCB);
			}
		};
		EventHandler.Register (csFlagDef.EVTNAME_REQ_POPUP_OPEN, m_OpenCB);

	}
	void UnRegEvent()
	{
		if (m_RegPopupCB != null)
		{
			EventHandler.Unregister (csFlagDef.EVTNAME_REGISTRY_POPUP_INFO, m_RegPopupCB);
			m_RegPopupCB = null;
		}

		if (m_UnRegPopupCB != null)
		{
			EventHandler.Unregister (csFlagDef.EVTNAME_UNREGISTRY_POPUP_INFO, m_UnRegPopupCB);
			m_UnRegPopupCB = null;
		}


		if (m_OpenCB != null)
		{
			EventHandler.Unregister (csFlagDef.EVTNAME_REQ_POPUP_OPEN, m_OpenCB);
			m_OpenCB = null;
		}
	}
	#endregion
}
