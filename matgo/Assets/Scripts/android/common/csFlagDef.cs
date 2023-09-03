using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csFlagDef {

	/// <summary>
	/// http통신 응답 결과
	/// </summary>
	/// <remarks>
	/// S_OK = 성공 / S_FALSE = 실패
	/// </remarks>
	public enum NET_RECV_CODE { NONE , S_OK , S_FALSE}

    #region Screen Resolution
    public readonly static int SCREEN_WIDTH = 1920;
    public readonly static int SCREEN_HEIGHT = 1080;
    #endregion

#if TEST
    #region URL's
    /// <summary>
    /// 앱 버전 정보 요청 url
    /// </summary>
    public static string URL_VERSION_CHECK = "http://192.168.0.134/GameApi/VersionCheck";

    /// <summary>
    /// 안드로이드 앱 마켓 url
    /// </summary>
    public static string URL_MARKET = "market://details?id=com.nsoft.jangbimatgo";

    /// <summary>
    /// 패치 파일 url
    /// </summary>
    //public static string URL_PATCH = "http://192.168.0.134:2100/mobilepatch/matgojangbi/update_version.unity3d";
    public static string URL_PATCH = "http://192.168.0.134:2100/mobilepatch/matgojangbi/update_version.xml";

    public static string URL_SIGNUP = "http://192.168.0.134/MobileWebView/RegisterForm";

    public static string URL_STORELIST = "http://192.168.0.134/GameApi/CashShopItemList";

    public static string URL_MYROOMLIST = "http://192.168.0.134/GameApi/MyRoomItemList?UserId={0}";
    #endregion
#else
    #region URL's
    /// <summary>
    /// 앱 버전 정보 요청 url
    /// </summary>
    public static string URL_VERSION_CHECK = "http://xzygames.com/GameApi/VersionCheck";

	/// <summary>
	/// 안드로이드 앱 마켓 url
	/// </summary>
	public static string URL_MARKET = "market://details?id=com.nsoft.jangbimatgo";

    /// <summary>
    /// 패치 파일 url
    /// </summary>
    //public static string URL_PATCH = "http://xzygames.com:2100/mobilepatch/matgojangbi/update_version.unity3d";
    public static string URL_PATCH = "http://xzygames.com:2100/mobilepatch/matgojangbi/update_version.xml";

    public static string URL_SIGNUP = "http://xzygames.com/MobileWebView/RegisterForm";

    public static string URL_STORELIST = "http://xzygames.com/GameApi/CashShopItemList";

    public static string URL_MYROOMLIST = "http://xzygames.com/GameApi/MyRoomItemList?UserId={0}";
    #endregion
#endif

    #region EVENT NAME Define - POPUP

    /// <summary>
    /// 팝업 객체(csBasePopup)는 csPopupManager에 팝업 정보 등록을 하기 위한 이벤트
    /// 전용 : csBasePopup , csPopupManager
    /// </summary>
    public static string EVTNAME_REGISTRY_POPUP_INFO = "REGISTRY_POPUP_INFO";

	/// <summary>
	/// 팝업 객체(csBasePopup)는 소멸시 csPopupManager에 알리기 위한 이벤트
	/// 전용 : csBasePopup , csPopupManager
	/// </summary>
	public static string EVTNAME_UNREGISTRY_POPUP_INFO = "UNREGISTRY_POPUP_INFO";

	/// <summary>
	/// 팝업 열기 요청
	/// 범용
	/// </summary>
	public static string EVTNAME_REQ_POPUP_OPEN = "REQ_POPUP_OPEN";
#endregion

#region POPUP KIND NAME Define
	/// <summary>
	/// 글로벌 팝업 이벤트
	/// 모든 팝업 중 최상단에 위치함.
	/// 범용
	/// </summary>
	public static string POPUP_KIND_GLOBALMSG = "POPUP_GLOBALMSG";
    
    /// <summary>
    /// 종료 여부 팝업 이벤트
    /// 안드로이드 Back 버튼 눌렀을시 팝업 이벤트(게임을 종료하시겠습니까?)
    /// 범용
    /// </summary>
    public static string POPUP_KIND_EXIT = "POPUP_EXIT";

    /// <summary>
    /// 상품 구매 팝업
    /// 전용 : csBasePopup , csPopupManager
    /// </summary>
    public static string POPUP_KIND_STOREPURCHASE_ITEM = "POPUP_STOREPURCHASE_ITEM";
    public static string POPUP_KIND_STOREPURCHASE_GOLD = "POPUP_STOREPURCHASE_GOLD";
#endregion

#region EVENT NAME Define
    /// <summary>
    /// 패치 파일 체크 / 다운로드 및 업데이트 
    /// </summary>
    public static string EVTNAME_PATCH_START = "PATCH_START";

	public static string EVTNAME_INAPP_ADDPRODUCTLIST = "INAPP_ADDPRODUCTLIST";

#endregion

	/// <summary>
	/// url에 따른 설명 제공
	/// </summary>
	/// <returns>url 설명 메시지</returns>
	/// <param name="StrArgURL">String argument UR.</param>
	public static string GetStringOfKindURL(string StrArgURL)
	{
		string[] Split = StrArgURL.Split ('?');
		string StrURL = Split [0];
		if(URL_VERSION_CHECK.Contains(StrURL))
		{
			return "버전 체크";
		}

		return string.Empty;
	}
}
