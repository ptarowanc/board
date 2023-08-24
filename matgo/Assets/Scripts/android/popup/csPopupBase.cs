using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unitycoding;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class csPopupBase : MonoBehaviour {

	#region SerializeField
	[Header("BASE FIELD")]
	[SerializeField]
	Image m_BG;

	[SerializeField]
	Image m_Popup;



	[SerializeField]
	Button m_CloseButton;
	#endregion

	#region abstract funcion
	protected abstract string PopupEventName ();
	#endregion

	#region CallBack Action
	protected System.Action<string,System.Action> m_ShowCB;
	protected System.Action m_CloseCB;
	#endregion

	#region Member Variable
	bool m_bIsOpen = false;
	public bool ISOPEN {
		get { return m_bIsOpen; }
	}

	#endregion

	#region MonoBase Function
	protected virtual void Awake()
	{
		RegEvent ();
		m_BG.gameObject.SetActive (false);
	}
	protected virtual void OnDestroy()
	{
		EventHandler.Execute (csFlagDef.EVTNAME_UNREGISTRY_POPUP_INFO, PopupEventName ());
	}
	#endregion

	#region Custom Function
	protected virtual void RegEvent()
	{
		EventHandler.Execute(csFlagDef.EVTNAME_REGISTRY_POPUP_INFO,GetComponent<csPopupBase>(), PopupEventName());
		m_CloseButton.onClick.AddListener (ClosePopup);

		SceneManager.sceneLoaded += (arg0, arg1) => 
		{
			if(this != null)
			{
				if(GetComponent<Canvas>().worldCamera == null)
				{
					GetComponent<Canvas>().worldCamera = Camera.main;
				}
			}
		};

	}

	public virtual void ShowPopup (object argMSG,System.Action argCloseCB)
	{
		if (m_bIsOpen)
		{
			return;
		}
		m_CloseCB = argCloseCB;

		m_bIsOpen = true;
		StopCoroutine ("CoAniOpen");
		StartCoroutine ("CoAniOpen");
	}
    

    protected virtual void ClosePopup()
	{
		m_BG.gameObject.SetActive (false);
		if (m_CloseCB != null)
		{
			m_CloseCB ();
		}
		m_CloseCB = null;

		m_bIsOpen = false;
	}

	IEnumerator CoAniOpen()
	{
		m_BG.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
		m_BG.gameObject.SetActive (true);

		bool bScaleUp = true;
		bool bMoveUp = true;
		float fSpeed = 5.0f;
		float fMoveSpeed = 0.5f;
		Vector3 vtOriPos = m_BG.transform.localPosition;
		while(true)
		{
			float fTime = Time.deltaTime;
			if(bScaleUp)
			{
				m_BG.transform.Translate(Vector3.up * fTime * fMoveSpeed);
				m_BG.transform.localScale += new Vector3(fSpeed * fTime,fSpeed * fTime,fSpeed * fTime);
				if(m_BG.transform.localScale.x >= 1.0f)
				{
					bScaleUp =false;
					m_BG.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
					bMoveUp = false;
					fMoveSpeed = 1.8f;
				}
			}
			else
			{
				if(!bMoveUp)
				{
					m_BG.transform.Translate(Vector3.down * fTime * fMoveSpeed);
					if(m_BG.transform.localPosition.y < vtOriPos.y - 20)
					{
						bMoveUp = true;
						m_BG.transform.localPosition = vtOriPos - new Vector3(0,20,0);
					}

				}
				else
				{
					m_BG.transform.Translate(Vector3.up * fTime * fMoveSpeed);
					if(m_BG.transform.localPosition.y >= vtOriPos.y)
					{
						m_BG.transform.localPosition = vtOriPos;
						break;
					}
				}

			}
			yield return null;
		}

	}

	#endregion
}
