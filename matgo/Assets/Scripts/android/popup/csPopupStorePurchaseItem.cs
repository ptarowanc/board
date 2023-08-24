using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitycoding;

public class csPopupStorePurchaseItem : csPopupBase {


	[Header ("ITEM INFO")]

    [SerializeField]
    Text m_Title;

    [SerializeField]
    Text m_Price;

    csJson.js_product_item m_jsItemInfo;

    protected override string PopupEventName ()
	{
		return csFlagDef.POPUP_KIND_STOREPURCHASE_ITEM;
	}

	public override void ShowPopup (object argMSG, System.Action argCloseCB)
	{
		base.ShowPopup (argMSG, argCloseCB);

		ResetInfo ();

		m_jsItemInfo = (csJson.js_product_item)argMSG;
		if (m_jsItemInfo == null)
		{
			return;
		}

        if (m_Title != null)
        {
            m_Title.text = string.Format("{0}({1})", m_jsItemInfo.pname != null ? m_jsItemInfo.pname : string.Empty, m_jsItemInfo.period == 0 ? "무제한" : string.Format("{0}일", m_jsItemInfo.period));
        }

        if (m_Price != null)
        {
            m_Price.text = string.Format("{0}실버", m_jsItemInfo.price.ToString("n0"));
        }
    }

    void ResetInfo()
	{
		m_jsItemInfo = null;
		m_Title.text = string.Empty;
		m_Price.text = string.Empty;

	}

    bool m_bReqPurchase = false;
    protected override void ClosePopup()
    {
        base.ClosePopup();
        m_bReqPurchase = false;
    }

    public void OnClickPurchase()
    {
        if (m_bReqPurchase)
        {
            return;
        }
        m_bReqPurchase = true;
        NetworkManager.Instance.PurchaseCash(m_jsItemInfo.pid, OnRcvPurchaseCash);
    }

    void OnRcvPurchaseCash(bool available, string reason)
    {
        System.Action empty = () => { };
        string msg = string.Format("{0}", !available ? "구매 실패했습니다.\n(" + reason + ")" : "성공적으로 구매가 이루어졌습니다.");
        EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)msg, empty);

        ClosePopup();

        NetworkManager.Instance.LobbyIn();
    }
}

