using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csDataManager {

	static csJson.js_inapp_product_list m_jsInAppProductList;
	public static csJson.js_inapp_product_list INAPP_PRODUCT_LIST
	{
		get{ return m_jsInAppProductList; }
		set{ m_jsInAppProductList = value; }
	}
}
