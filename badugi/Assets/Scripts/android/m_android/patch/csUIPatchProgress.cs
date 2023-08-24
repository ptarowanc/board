using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csUIPatchProgress : MonoBehaviour {

	[SerializeField]
	Image m_ProgressBar;

	[SerializeField]
	Text m_StateText;

	// Use this for initialization
	void Start () {
		csPatchFileDownLoader.OnStateChange += OnStateChangeUI;
	}

	void OnStateChangeUI(csPatchFileDownLoader.enSTATE state,float progress,string msg)
	{
		if (m_ProgressBar != null)
		{
			m_ProgressBar.fillAmount = progress;
		}
		if (m_StateText != null)
		{
			m_StateText.text = state != csPatchFileDownLoader.enSTATE.NET_ERROR ? msg : string.Empty;
		}
	}
}
