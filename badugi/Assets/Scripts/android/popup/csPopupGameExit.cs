using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitycoding;

public class csPopupGameExit : csPopupBase {

    protected override string PopupEventName ()
	{
		return csFlagDef.POPUP_KIND_EXIT;
	}

	protected override void Start ()
	{
		base.Start();
		DontDestroyOnLoad (gameObject);
	}

    public void OnGameExit()
    {
        Application.Quit();
    }
}
