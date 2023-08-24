using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitycoding;

public class csPopupGlobalMSG : csPopupBase {

	[Header ("MSG INFO")]
	[SerializeField]
	Text m_MSG;

	protected override string PopupEventName ()
	{
		return csFlagDef.POPUP_KIND_GLOBALMSG;
	}

	protected override void Awake ()
	{
		base.Awake ();
		DontDestroyOnLoad (gameObject);
	}

	public override void ShowPopup (object argMSG, System.Action argCloseCB)
	{
		base.ShowPopup (argMSG, argCloseCB);
		m_MSG.text = argMSG.ToString ();

	}

}
