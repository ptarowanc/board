using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class csMyRoomManager : MonoBehaviour {

    public enum enMENU { AVATAR, CARD, EVT }

    #region Variable
    public static csJson.js_myroom_list m_jsMyRoomList;
    //static AssetBundle m_AssetBundle;
    //public static AssetBundle StoreAsset
    //{
    //    set { m_AssetBundle = value; }
    //    get { return m_AssetBundle; }
    //}
    #endregion

    #region Mono Function
    void Awake()
    {
        //LoadMyRoomAssetBundle();
    }

    private void OnDestroy()
    {
        //UnLoadMyRoomAssetBundle();
    }
    #endregion

    #region Custom Function
    
    //void LoadMyRoomAssetBundle()
    //{
    //    StringBuilder sbPath = new StringBuilder();
    //    sbPath.AppendFormat("{0}/{1}", csBundleLoader.GetRelativePath(), "img_store.unity3d");
    //    m_AssetBundle = AssetBundle.LoadFromFile(sbPath.ToString());
    //}
    //void UnLoadMyRoomAssetBundle()
    //{
    //    if (m_AssetBundle != null)
    //    {
    //        m_AssetBundle.Unload(true);
    //    }
    //}

    public static void SetProductList(string packet)
    {
        //TEST
        //packet = "{\"avatar\" : [ { \"pid\" : \"10\",\"pname\" : \"홍길동\",\"img\" : \"a-1\",\"expire\" : \"2018-01-30 00:00:00\",\"count\" : 1,\"use\" : true},{ \"pid\" : \"10\",\"pname\" : \"홍길동\",\"img\" : \"avatar_honggildong_default\",\"expire\" : \"2018-01-30 00:00:00\",\"count\" : 1,\"use\" : true},{ \"pid\" : \"10\",\"pname\" : \"홍길동\",\"img\" : \"avatar_honggildong_default\",\"expire\" : \"2018-01-30 00:00:00\",\"count\" : 1,\"use\" : true},{ \"pid\" : \"10\",\"pname\" : \"홍길동\",\"img\" : \"avatar_honggildong_default\",\"expire\" : \"2018-01-30 00:00:00\",\"count\" : 1,\"use\" : true},{ \"pid\" : \"10\",\"pname\" : \"홍길동\",\"img\" : \"avatar_honggildong_default\",\"expire\" : \"2018-01-30 00:00:00\",\"count\" : 1,\"use\" : true},{ \"pid\" : \"11\",\"pname\" : \"어우동\",\"img\" : \"avatar_eoudong_default\",\"expire\" : \"\",\"count\" : 1,\"use\" : false}],\"card\" : [ { \"pid\" : \"12\",\"pname\" : \"맞고 화투패\",\"img\" : \"card_matgo_default\",\"expire\" : \"\",\"count\" : 1,\"use\" : true}],\"evt\" : []}";
        m_jsMyRoomList = JsonMapper.ToObject<csJson.js_myroom_list>(packet);
    }
    public static List<csJson.js_myroom_item> GetProductList(enMENU kind)
    {
        if (m_jsMyRoomList == null)
        {
            return null;
        }

        switch (kind)
        {
            case enMENU.AVATAR:
                return m_jsMyRoomList.avatar;
            case enMENU.CARD:
                return m_jsMyRoomList.card;
            case enMENU.EVT:
                return m_jsMyRoomList.evt;
        }
        return null;
    }

    public void OnMyRoomClose()
    {
        csAndroidManager.Instance.GoToLobby();
    }
    public void OnStore()
    {
        csAndroidManager.Instance.GoToStore();
    }
    #endregion


}
