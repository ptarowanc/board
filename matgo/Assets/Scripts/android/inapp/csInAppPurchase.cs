using UnityEngine;
using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Security;


public class csInAppPurchase : MonoBehaviour, IStoreListener
{

	private static IStoreController storeController;
	private static IExtensionProvider extensionProvider;

	ConfigurationBuilder m_ConfBuilder;

    UnityEngine.Events.UnityAction<bool,string> m_Callback_BuyPID;

	List<csJson.js_product_item> m_ListProducts;
	public void SetProductList(List<csJson.js_product_item> arg)
	{
		m_ListProducts = arg;
	}

	void Start()
	{
		InitializePurchasing ();
	}

	void OnDestroy()
	{
		storeController = null;
		extensionProvider = null;
		m_Callback_BuyPID = null;
	}


	private bool IsInitialized()
	{
		return (storeController != null && extensionProvider != null);
	}

	public void InitializePurchasing()
	{
		if (IsInitialized())
        {
            return;
        }

        var module = StandardPurchasingModule.Instance();

		m_ConfBuilder = ConfigurationBuilder.Instance(module);

		if (m_ListProducts!= null)
		{
			for (int i = 0; i < m_ListProducts.Count; i++)
			{
				csJson.js_product_item item = (csJson.js_product_item)m_ListProducts [i];
				m_ConfBuilder.AddProduct (item.pid, ProductType.Consumable, new IDs {
					//{ item.pid, AppleAppStore.Name },
					{ item.pid, GooglePlay.Name },
				});
			}
		}

		UnityPurchasing.Initialize(this, m_ConfBuilder);
	}

	public void BuyProductID(string productId, UnityEngine.Events.UnityAction<bool,string> callback)
	{
		bool bSuccess = false;
		string strMSG = string.Empty;
		try
		{
			m_Callback_BuyPID = callback;
			if (IsInitialized())
			{
				Product p = storeController.products.WithID(productId);

				if (p != null && p.availableToPurchase)
				{
					//Debug.Log(string.Format("Purchasing product asychronously: '{0}'", p.definition.id));
					storeController.InitiatePurchase(p);
					bSuccess = true;
				}
				else
				{
					strMSG = "제품을 찾을 수 없거나 구매할 수 없습니다.";
				}
			}
			else
			{
				strMSG = "초기화되지 않음.";
			}
		}
		catch (Exception e)
		{
			strMSG = "구매시 예외상황 발생 " + e;
		}
		finally
		{
			if (!bSuccess)
			{
				if (m_Callback_BuyPID != null)
				{
					m_Callback_BuyPID (false, strMSG);
				}
			}
		}

	}

	public void RestorePurchase()
	{
		if (!IsInitialized())
		{
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("RestorePurchases started ...");

			var apple = extensionProvider.GetExtension<IAppleExtensions>();

			apple.RestoreTransactions
			(
				(result) => { Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); }
			);
		}
		else
		{
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController sc, IExtensionProvider ep)
	{
		Debug.Log("OnInitialized : PASS");

		storeController = sc;
		extensionProvider = ep;
	}

	public void OnInitializeFailed(InitializationFailureReason reason)
	{
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + reason);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		string LastTransationID = e.purchasedProduct.transactionID;
		string LastReceipt = e.purchasedProduct.receipt;

		//Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
		//Debug.Log("Receipt: " + e.purchasedProduct.receipt);
		//Debug.Log ("TransationID: " + e.purchasedProduct.transactionID);

        UnityEngine.Events.UnityAction<bool, Guid> callback = (bool bSuccess, Guid token) =>
		{
			if (bSuccess)
			{
				storeController.ConfirmPendingPurchase (e.purchasedProduct);
                NetworkManager.Instance.PurchaseResult(token, m_Callback_BuyPID);
			}
            else
            {
                if (m_Callback_BuyPID != null)
                {
                    m_Callback_BuyPID(bSuccess, "서버 영수증 유효성 검증 실패");
                }
            }
        };

        JsonData wrapper = JsonMapper.ToObject(e.purchasedProduct.receipt);
        if (wrapper != null)
        {
            // Corresponds to http://docs.unity3d.com/Manual/UnityIAPPurchaseReceipts.html
            var store = (string)wrapper["Store"];
            var payload = (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt


            // For GooglePlay payload contains more JSON
            //if (Application.platform == RuntimePlatform.Android)
            //{
            //	var details = JsonMapper.ToObject (payload);
            //	var gpJson = (string)details ["json"];
            //	var gpSig = (string)details ["signature"];
            //}
            NetworkManager.Instance.CheckPurchaseReceipt(payload, callback);
        }

        return PurchaseProcessingResult.Pending;
	}
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		//Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		if(m_Callback_BuyPID != null)
		{
			m_Callback_BuyPID(false,failureReason.ToString());
		}
	}
}	


#endif