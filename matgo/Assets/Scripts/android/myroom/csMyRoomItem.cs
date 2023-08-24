using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitycoding;
using System.Text;
using System;
using EnhancedUI.EnhancedScroller;

public class csMyRoomItem : EnhancedScrollerCellView
{

    [SerializeField]
    Text Title;
    [SerializeField]
    RawImage Image;
    [SerializeField]
    Text Expire;
    [SerializeField]
    Image UseState;
    [SerializeField]
    Button EquipButton;


    [Header("UseState Image")]
    [SerializeField]
    Sprite[] UseEnable;

    [SerializeField]
    Sprite[] UseIng;


    csJson.js_myroom_item m_jsInfo;

    //DateTime m_DtExpire = DateTime.MinValue;

    public void SetInfo(csJson.js_myroom_item info)
    {
        m_jsInfo = info;
    }

    csMyRoomList m_ParentList;
    // Use this for initialization
    void Start()
    {
        m_ParentList = GetComponentInParent<csMyRoomList>();
        m_ParentList.ItemUseChange += OnItemUseChange;
        UpdateUI();
    }
    private void Update()
    {
        if (m_jsInfo != null && Expire != null)
        {
            if (m_jsInfo.expire.Length == 0)
            {
                Expire.text = "무제한";
            }
            else
            {
                string strTime = string.Empty;
                DateTime dtExpire = Convert.ToDateTime(m_jsInfo.expire);
                TimeSpan ts = dtExpire - DateTime.Now;
                if (ts.Days > 365)
                {
                    strTime = "기한없음";
                }
                else if (ts.TotalSeconds > 0)
                {
                    strTime = ts.Days > 0 ? ts.Days + "일 " : string.Empty;
                    strTime += ts.Hours > 0 ? ts.Hours + "시간 " : string.Empty;
                    strTime += ts.Minutes > 0 ? ts.Minutes + "분 " : string.Empty;
                    if (ts.Days == 0 && ts.Hours == 0)
                    {
                        strTime += ts.Seconds > 0 ? ts.Seconds + "초" : string.Empty;
                    }
                }
                else
                {
                    strTime = "<color=red>기간 만료됨</color>";
                }

                if (Expire)
                {
                    Expire.text = strTime;
                }
            }
        }
    }
    public void UpdateUI()
    {

        if (Title != null && m_jsInfo.pname != null)
        {
            Title.text = m_jsInfo.pname;
        }

        if (Image != null && m_jsInfo.img != null && m_jsInfo.img.Length > 0)
        {
            if (csAndroidManager.StoreAsset != null)
            {
                Image.texture = (Texture)csAndroidManager.StoreAsset.LoadAsset(m_jsInfo.img);
                if (Image.texture != null)
                {
                    Image.rectTransform.sizeDelta = new Vector2(210, 310);
                }
            }
        }

        if (UseState != null)
        {
            //SpriteState ss = new SpriteState();
            //ss.highlightedSprite = m_jsInfo.use ? UseIng[0] : UseEnable[0];
            //ss.pressedSprite = m_jsInfo.use ? UseIng[1] : UseEnable[1];
            //ss.disabledSprite = m_jsInfo.use ? UseIng[0] : UseEnable[0];
            UseState.overrideSprite = m_jsInfo.use ? UseIng[0] : UseEnable[0];
        }
    }
    public override void RefreshCellView()
    {
        base.RefreshCellView();
        UpdateUI();
    }

    public void OnClickEquip()
    {
        m_ParentList.OnChangeEquip(m_jsInfo.pid);
    }

    private void OnDestroy()
    {
        if (m_ParentList != null)
        {
            m_ParentList.ItemUseChange -= OnItemUseChange;
        }
    }

    private void OnItemUseChange(string pid)
    {
        bool bSame = pid.Equals(m_jsInfo.pid);
        //SpriteState ss = new SpriteState();
        //ss.highlightedSprite = bSame ? UseIng[0] : UseEnable[0];
        //ss.pressedSprite = bSame ? UseIng[1] : UseEnable[1];
        //ss.disabledSprite = bSame ? UseIng[0] : UseEnable[0];
        //UseState.GetComponent<Button>().spriteState = ss;
        UseState.overrideSprite = bSame ? UseIng[0] : UseEnable[0];
    }

}
