using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unitycoding;

public class csPatchManager : MonoBehaviour {

	csPatchFileDownLoader m_DownLoader;
	void Awake()
	{
		
		EventHandler.Register (csFlagDef.EVTNAME_PATCH_START, CheckPatch);

		m_DownLoader = new csPatchFileDownLoader();
	}

	void OnDownLoadState(csPatchFileDownLoader.enSTATE state,float progress,string msg)
	{
		if (state == csPatchFileDownLoader.enSTATE.NET_ERROR)
		{
			EventHandler.Execute (csFlagDef.POPUP_KIND_GLOBALMSG, msg ,csAndroidManager.EvtQuit);
		}
	}

	public void CheckPatch()
	{
		csPatchFileDownLoader.OnStateChange -= OnDownLoadState;
		csPatchFileDownLoader.OnStateChange += OnDownLoadState;
		m_DownLoader.StartDownLoad (csFlagDef.URL_PATCH);
	}

	void OnDestroy()
	{
		csPatchFileDownLoader.OnStateChange -= OnDownLoadState;
	}
}
