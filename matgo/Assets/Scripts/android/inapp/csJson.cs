using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class csJson {

    static public bool JsonDataContainsKey(LitJson.JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
        {
            return result;
        }
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key))
        {
            result = true;
        }
        return result;
    }

    public class js_serv_version
    {
        public string version;
        public string market;
        public string error;
    }

	public class marektstate
	{
		/// <summary>
		/// 결과 : 성공(true),실패(false)
		/// </summary>
		public string result;
		/// <summary>
		///상태: 	정지(stop),정상(work)
		/// </summary>
		public string state;
		/// <summary>
		/// 링크 주소
		/// </summary>
		public string link;
		/// <summary>
		/// 출력 메시지
		/// </summary>
		public string msg;
		/// <summary>
		/// 앱종료(close),진행(skip),링크이동(linkskip),링크이동 및 종료(linkclose)
		/// </summary>
		public string after;
	}

	public class js_inapp_product_list
	{
		public List<js_product_item> product;
	}

	public class js_product_list
	{
        public string error;
		public List<js_product_item> avatar;
		public List<js_product_item> card;
		public List<js_product_item> evt;
		public List<js_product_item> charge;
	}

	public class js_product_item
	{
		public string pid;
		public string pname;
        public int period;
		public string img;
		public string paymoney;
		public string freemoney;
		public string purchase_kind;
		public int price;
	}


	public class js_purchaseinfo
	{
		public string account;
		public string purchased;
	}

	public class js_buyproduct_result
	{
		public string account;
		public string th;
		public string purchased;
		public string msg;
	}

	public class js_inapp_enablepurchase
	{
		public string enable{get;set;}
		public string msg{get;set;}
	}

    public class js_myroom_list
    {
        public string error;
        public List<js_myroom_item> avatar;
        public List<js_myroom_item> card;
        public List<js_myroom_item> evt;

    }

    public class js_myroom_item
    {
        public string pid;
        public string pname;
        public string img;
        public string expire;
        public int count;
        public bool use;
    }
}
