using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitycoding;

public class csPopupStorePurchaseInApp : csPopupBase {


	[Header ("ITEM INFO")]

    [SerializeField]
    Text m_Title;

    [SerializeField]
	Text m_Price;

	csJson.js_product_item m_jsItemInfo;

    protected override string PopupEventName ()
	{
		return csFlagDef.POPUP_KIND_STOREPURCHASE_GOLD;
	}

    public override void ShowPopup(object argMSG, System.Action argCloseCB)
    {
        base.ShowPopup(argMSG, argCloseCB);

        ResetInfo();

        m_jsItemInfo = (csJson.js_product_item)argMSG;
        if (m_jsItemInfo == null)
        {
            return;
        }

        if(m_Title != null)
        {
            m_Title.text = string.Format("{0}({1})",m_jsItemInfo.pname != null ? m_jsItemInfo.pname : string.Empty , m_jsItemInfo.period == 0 ? "무제한" : string.Format("{0}일", m_jsItemInfo.period));
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
        if(m_bReqPurchase)
        {
            return;
        }
        m_bReqPurchase = true;
        NetworkManager.Instance.CheckEnablePurchase(m_jsItemInfo.pid, OnRcvCheckEnablePurchase);
    }
    void OnRcvCheckEnablePurchase(bool available, string reason)
    {
        System.Action empty = () => { };
        if (available)
        {
            if (m_jsItemInfo.purchase_kind == "inapp")
            {
                UnityEngine.Events.UnityAction<bool, string> CBResult = (bool bSuccess, string strMSG) =>
                {
                    string msg = string.Format("{0}", !bSuccess ? "결제에 실패했습니다.\n(" + strMSG + ")" : "성공적으로 결제가 이루어졌습니다.");
                    EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)msg, empty);
                    ClosePopup();
                    NetworkManager.Instance.LobbyIn();
                };
                csInAppPurchase com = GameObject.FindObjectOfType<csInAppPurchase>();
                if(com != null)
                {
                    com.BuyProductID(m_jsItemInfo.pid, CBResult);
                }
            }
            else
            {
            }
        }
        else
        {
            ClosePopup();
            EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)reason, empty);
        }
    }


}

