using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class csBtStoreTab : MonoBehaviour ,IPointerDownHandler{

	static csStoreManager.enMENU m_ActiveTab = csStoreManager.enMENU.AVATAR;

	public delegate void DelTabEvent(csStoreManager.enMENU kind);
	public static DelTabEvent TabEventChange;

	[SerializeField]
	csStoreManager.enMENU m_TabKind;



	public static csStoreManager.enMENU ACTIVE_TAB
	{
		get{ return m_ActiveTab; }
	}

	// Use this for initialization
	void Start () {

		OnTabEventChange (m_ActiveTab);

		TabEventChange += OnTabEventChange;
	}


    void OnTabEventChange(csStoreManager.enMENU SelKind)
	{
		GetComponent<Button> ().interactable = m_TabKind == SelKind ? false : true;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.pointerPressRaycast.gameObject == null)
		{
			return;
		}

		csBtStoreTab comSel = eventData.pointerPressRaycast.gameObject.GetComponent<csBtStoreTab> ();
		if (comSel == null)
		{
			return;
		}


		if (m_TabKind != m_ActiveTab)
		{
			m_ActiveTab = comSel.m_TabKind;

			if (TabEventChange != null)
			{
				TabEventChange (m_ActiveTab);
			}
		}
	}

	void OnDestroy()
	{
		if (TabEventChange != null)
		{
            m_ActiveTab = csStoreManager.enMENU.AVATAR;
            TabEventChange -= OnTabEventChange;
            TabEventChange = null;

        }
	}
}
