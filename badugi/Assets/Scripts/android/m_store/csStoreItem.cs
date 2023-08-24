using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitycoding;
using System.Text;
using EnhancedUI.EnhancedScroller;

public class csStoreItem : EnhancedScrollerCellView
{

	[SerializeField]
	Text Title;
	[SerializeField]
	RawImage Image;
	[SerializeField]
	Text PayMoney;
	[SerializeField]
	Text FreeMoney;
    [SerializeField]
    Text Period;
    [SerializeField]
	Text Price;
	[SerializeField]
	Button BtPurchase;


	csJson.js_product_item m_jsInfo;

	public void SetInfo(csJson.js_product_item info)
	{
		m_jsInfo = info;
	}

	// Use this for initialization
	void Start () 
	{
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (Title != null)
        {

            Title.text = m_jsInfo.pname;// m_jsInfo.purchase_kind.ToLower() == "inapp" ? m_jsInfo.pname : string.Format("{0}", m_jsInfo.pname);
        }

        if (Image != null && m_jsInfo.img != null && m_jsInfo.img.Length > 0)
        {
            Image.texture = (Texture)csAndroidManager.StoreAsset.LoadAsset(m_jsInfo.img);
            if (Image.texture != null)
            {
                Image.SetNativeSize();
                float h = 300;
                Vector2 vtSize = Image.rectTransform.sizeDelta;
                float fRatio = h / vtSize.y;
                Image.rectTransform.sizeDelta = new Vector2(fRatio * vtSize.x, h);
            }
        }
        if (PayMoney != null && m_jsInfo.paymoney != null)
        {
            PayMoney.text = m_jsInfo.paymoney;
        }
        if (FreeMoney != null && m_jsInfo.freemoney != null)
        {
            FreeMoney.text = m_jsInfo.freemoney;
        }
        if (Price != null)
        {
            Price.text = string.Format("{0}원", m_jsInfo.price.ToString("n0"));
        }
        if(Period != null)
        {
            Period.text = m_jsInfo.period == 0 ? "무제한" : string.Format("{0}일", m_jsInfo.period);
        }

    }
    public override void RefreshCellView()
    {
        base.RefreshCellView();
        UpdateUI();
    }
    public void OnClickPurchase()
	{
        System.Action cb = () => { };
        EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, m_jsInfo.purchase_kind.ToLower() == "inapp" ? csFlagDef.POPUP_KIND_STOREPURCHASE_GOLD : csFlagDef.POPUP_KIND_STOREPURCHASE_ITEM, (object)m_jsInfo, cb);
    }

    void OnRcvCheckEnablePurchase(bool available,string reason)
    {
        System.Action empty = () => { };
        if (available)
        {
            EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, m_jsInfo.purchase_kind.ToLower() == "inapp" ? csFlagDef.POPUP_KIND_STOREPURCHASE_GOLD : csFlagDef.POPUP_KIND_STOREPURCHASE_ITEM, (object)m_jsInfo, empty);
        }
        else
        {
            EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)reason, empty);
        }
    }
}
