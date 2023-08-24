using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
using Unitycoding;
using System.Text;

public class csStoreManager : MonoBehaviour {

    #region Enum
    public enum enMENU{ AVATAR,CARD,EVT,CHARGE}
    #endregion

    #region Variable
    public static csJson.js_product_list m_jsStoreList;
	public static void SetProductList(string packet)
	{
        //TEST
        /*
		packet = 
			"{" +
			"\"avatar\" : " +
			"[" +
				"{" +
					"\"pid\" : \"item_avatar_1\"" +
					"," +
					"\"pname\" : \"홍길동 분신술\"" +
                    "," +
                    "\"period\" : 1" +
                    "," +
					"\"img\" : \"a-1\"" +
					"," +
					"\"money\" : \"1만원\"" +
					"," +
					"\"freemoney\" : \"100만원\"" +
					"," +
					"\"purchase_kind\" : \"gold\"" +
					"," +
					"\"price\" : 5" +
				"}" +
			"," +
				"{" +
				"\"pid\" : \"item_avatar_2\"" +
				"," +
				"\"pname\" : \"홍길동 분신술2\"" +
                "," +
                "\"period\" : 3" +
                "," +
                "\"img\" : \"a-2\"" +
				"," +
				"\"money\" : \"2만원\"" +
				"," +
				"\"freemoney\" : \"200만원\"" +
				"," +
				"\"purchase_kind\" : \"gold\"" +
				"," +
				"\"price\" : 10" +
				"}" +
            "]" +
			"," +
			"\"card\" : " +
			"[" +
				"{" +
					"\"pid\" : \"item_card_1\"" +
					"," +
					"\"pname\" : \"홍길동 화투패1\"" +
                    "," +
                    "\"period\" : 7" +
                    "," +
                    "\"img\" : \"back\"" +
					"," +
					"\"money\" : \"1만원\"" +
					"," +
					"\"freemoney\" : \"100만원\"" +
					"," +
					"\"purchase_kind\" : \"gold\"" +
					"," +
					"\"price\" : 5" +
				"}" +
			"," +
				"{" +
					"\"pid\" : \"item_card_2\"" +
					"," +
					"\"pname\" : \"홍길동 화투패2\"" +
                    "," +
                    "\"period\" : 15" +
                    "," +
                    "\"img\" : \"back02\"" +
					"," +
					"\"money\" : \"2만원\"" +
					"," +
					"\"freemoney\" : \"200만원\"" +
					"," +
					"\"purchase_kind\" : \"gold\"" +
					"," +
					"\"price\" : 10" +
				"}" +

			"]" +
			"," +
			"\"evt\" : " +
			"[" +
				"{" +
					"\"pid\" : \"item_evt_1\"" +
					"," +
					"\"pname\" : \"홍길동 이벤트1\"" +
                    "," +
                    "\"period\" : 30" +
                    "," +
					"\"img\" : \"a-5\"" +
					"," +
					"\"money\" : \"1만원\"" +
					"," +
					"\"freemoney\" : \"100만원\"" +
					"," +
					"\"purchase_kind\" : \"gold\"" +
					"," +
					"\"price\" : 5" +
				"}" +
				"," +
				"{" +
					"\"pid\" : \"item_evt_2\"" +
					"," +
					"\"pname\" : \"홍길동 이벤트2\"" +
                    "," +
                    "\"period\" : 60" +
                    "," +
					"\"img\" : \"back05\"" +
					"," +
					"\"money\" : \"2만원\"" +
					"," +
					"\"freemoney\" : \"200만원\"" +
					"," +
					"\"purchase_kind\" : \"gold\"" +
					"," +
					"\"price\" : 10" +
				"}" +
			"]" +
			"," +
			"\"charge\" : " +
			"[" +

				"{" +
					"\"pid\" : \"item_charge_1\"" +
					"," +
					"\"pname\" : \"금괴 1개\"" +
					"," +
                    "\"img\" : \"img-gold1\"" +
					"," +
					"\"purchase_kind\" : \"inapp\"" +
					"," +
					"\"price\" : 5000" +
				"}" +
				"," +
				"{" +
					"\"pid\" : \"item_charge_2\"" +
					"," +
					"\"pname\" : \"금괴 10개\"" +
					"," +
                    "\"img\" : \"img-gold10\"" +
					"," +
					"\"purchase_kind\" : \"inapp\"" +
					"," +
					"\"price\" : 10000" +
				"}" +
                "," +
                "{" +
                    "\"pid\" : \"item_charge_3\"" +
                    "," +
                    "\"pname\" : \"금괴 50개\"" +
                    "," +
                    "\"img\" : \"img-gold50\"" +
                    "," +
                    "\"purchase_kind\" : \"inapp\"" +
                    "," +
                    "\"price\" : 20000" +
                "}" +

            "]" +
			"}";
            */
		m_jsStoreList = JsonMapper.ToObject<csJson.js_product_list> (packet);


	}
	public static List<csJson.js_product_item> GetProductList(enMENU kind)
	{
		if (m_jsStoreList == null)
		{
			return null;
		}

		switch (kind)
		{
		case enMENU.AVATAR:
			return m_jsStoreList.avatar;
		case enMENU.CARD:
			return m_jsStoreList.card;
		case enMENU.EVT:
			return m_jsStoreList.evt;
		case enMENU.CHARGE:
			return m_jsStoreList.charge;
		}
		return null;
	}

    //static AssetBundle m_AssetBundle;
    //public static AssetBundle StoreAsset
    //{
    //    set { m_AssetBundle = value; }
    //    get { return m_AssetBundle; }
    //}

    #endregion

    #region Mono Function
    // Use this for initialization
    void Awake () 
	{
        //LoadStoreAssetBundle();
    }
    
	void Start()
	{
#if UNITY_ANDROID && UNITY_PURCHASING
        InitInAppPurchase();
#endif
    }
    private void OnDestroy()
    {
        //UnLoadStoreAssetBundle();
    }
    #endregion

#region Custom Function
    #if UNITY_PURCHASING
    void InitInAppPurchase()
    {
        csInAppPurchase com = GameObject.FindObjectOfType<csInAppPurchase>();
        if (com != null)
        {
            DestroyImmediate(com.gameObject);
        }

        ((new GameObject("InAppPurchaseModule", typeof(csInAppPurchase)))).
                                GetComponent<csInAppPurchase>().SetProductList(GetProductList(enMENU.CHARGE));
    }
    #endif
    //void LoadStoreAssetBundle()
    //{
    //    StringBuilder sbPath = new StringBuilder();
    //    sbPath.AppendFormat("{0}/{1}", csBundleLoader.GetRelativePath(), "img_store.unity3d");
    //    m_AssetBundle = AssetBundle.LoadFromFile(sbPath.ToString());
    //}
    //void UnLoadStoreAssetBundle()
    //{
    //    if (m_AssetBundle != null)
    //    {
    //        m_AssetBundle.Unload(true);
    //    }
    //}

    public void OnStoreClose()
	{
        csAndroidManager.Instance.GoToLobby();
	}

    public void OnMyRoom()
    {
        csAndroidManager.Instance.GoToMyRoom();
    }
#endregion
}
