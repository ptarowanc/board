using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class csBtStoreSort : MonoBehaviour {


    #region Field
    [Header("SortKind")]
    [SerializeField]
    Sprite[] m_DAY_Total;
    [SerializeField]
    Sprite[] m_DAY_1;
    [SerializeField]
    Sprite[] m_DAY_3;
    [SerializeField]
    Sprite[] m_DAY_7;
    [SerializeField]
    Sprite[] m_DAY_15;
    [SerializeField]
    Sprite[] m_DAY_30;
    [SerializeField]
    Sprite[] m_DAY_60;
    [SerializeField]
    Sprite[] m_DAY_90;
    #endregion

    #region enum
    public enum enStoreSort { TOTAL = 0, DAY1, DAY3, DAY7, DAY15, DAY30, DAY60, DAY90 }
    #endregion

    #region Variable
    static enStoreSort m_SortKind = enStoreSort.TOTAL;
    public static enStoreSort SORTKIND { get { return m_SortKind; } }
    public static int GetSortKindToDays(enStoreSort enKind)
    {
        switch(enKind)
        {
            case enStoreSort.TOTAL: return -1;
            case enStoreSort.DAY1:  return 1;
            case enStoreSort.DAY3:  return 3;
            case enStoreSort.DAY7:  return 7;
            case enStoreSort.DAY15: return 15;
            case enStoreSort.DAY30: return 30;
            case enStoreSort.DAY60: return 60;
            case enStoreSort.DAY90: return 90;
        }
        return -1;
    }

    Image m_SortIMG;
    #endregion

    #region Event
    class EVENT<T0> : UnityEvent<T0> { }
    static EVENT<enStoreSort> EVENT_SortChange = new EVENT<enStoreSort>();
    public static void AddListener(UnityAction<enStoreSort> listener)
    {
        EVENT_SortChange.AddListener(listener);
    }
    public static void DelListener(UnityAction<enStoreSort> listener)
    {
        EVENT_SortChange.RemoveListener(listener);
    }
    public void OnClickSort()
    {
        m_SortKind = (enStoreSort)(((int)m_SortKind + 1) % 8);
        ChangeSprite(m_SortKind);

        EVENT_SortChange.Invoke(m_SortKind);
    }
    #endregion

    #region Mono Function
    private void Start()
    {
        m_SortIMG = GetComponent<Image>();
        ChangeSprite(m_SortKind);
    }
    private void OnDestroy()
    {
        EVENT_SortChange.RemoveAllListeners();
    }

    #endregion

    #region Custom Function
    void ChangeSprite(enStoreSort kind)
    {
        Sprite[] Sel = null;
        switch(kind)
        {
            case enStoreSort.TOTAL:
                Sel = m_DAY_Total;
                break;
            case enStoreSort.DAY1:
                Sel = m_DAY_1;
                break;
            case enStoreSort.DAY3:
                Sel = m_DAY_3;
                break;
            case enStoreSort.DAY7:
                Sel = m_DAY_7;
                break;
            case enStoreSort.DAY15:
                Sel = m_DAY_15;
                break;
            case enStoreSort.DAY30:
                Sel = m_DAY_30;
                break;
            case enStoreSort.DAY60:
                Sel = m_DAY_60;
                break;
            case enStoreSort.DAY90:
                Sel = m_DAY_90;
                break;
            default:
                return;
        }
        m_SortIMG.overrideSprite = Sel[0];

        SpriteState ss = new SpriteState();
        ss.highlightedSprite = Sel[0];
        ss.pressedSprite = Sel[1];
        ss.disabledSprite = Sel[0];
        m_SortIMG.GetComponent<Button>().spriteState = ss;
        m_SortIMG.GetComponent<Image>().sprite = ss.highlightedSprite;
    }
    #endregion



}
