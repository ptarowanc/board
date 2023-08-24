using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class csBtMyRoomTab : MonoBehaviour ,IPointerDownHandler{

	static csMyRoomManager.enMENU m_ActiveTab = csMyRoomManager.enMENU.AVATAR;

	public delegate void DelTabEvent(csMyRoomManager.enMENU kind);
	public static DelTabEvent TabEventChange;

	[SerializeField]
	csMyRoomManager.enMENU m_TabKind;



	public static csMyRoomManager.enMENU ACTIVE_TAB
	{
		get{ return m_ActiveTab; }
	}

	// Use this for initialization
	void Start () {

		OnTabEventChange (m_ActiveTab);

		TabEventChange += OnTabEventChange;
	}

	void OnTabEventChange(csMyRoomManager.enMENU SelKind)
	{
		GetComponent<Button> ().interactable = m_TabKind == SelKind ? false : true;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.pointerPressRaycast.gameObject == null)
		{
			return;
		}

		csBtMyRoomTab comSel = eventData.pointerPressRaycast.gameObject.GetComponent<csBtMyRoomTab> ();
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
            m_ActiveTab = csMyRoomManager.enMENU.AVATAR;
            TabEventChange -= OnTabEventChange;
            TabEventChange = null;

        }
	}
}
