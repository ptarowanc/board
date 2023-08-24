using EnhancedScrollerDemos.CellEvents;
using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class csMyRoomList : MonoBehaviour, IEnhancedScrollerDelegate
{

	[SerializeField]
	csMyRoomManager.enMENU m_ListKind;
    //[SerializeField]
    //GameObject m_Container;

    public delegate void ItemUseEventChange(string pid);
    public ItemUseEventChange ItemUseChange;


    /// <summary>
    /// This is our scroller we will be a delegate for
    /// </summary>
    public EnhancedScroller scroller;

    /// <summary>
    /// This will be the prefab of each cell in our scroller. Note that you can use more
    /// than one kind of cell, but this example just has the one type.
    /// </summary>
    public EnhancedScrollerCellView cellViewPrefab;


    Transform m_ChildOffSet;

    List<csJson.js_myroom_item> m_list = new List<csJson.js_myroom_item>();
    // Use this for initialization
    void Start () 
	{
		m_ChildOffSet = transform.Find ("offset");

        if (csMyRoomManager.m_jsMyRoomList != null)
		{
			m_list = csMyRoomManager.GetProductList (m_ListKind);
		}
		m_ChildOffSet.gameObject.SetActive (m_ListKind == csBtMyRoomTab.ACTIVE_TAB ? true : false);

		csBtMyRoomTab.TabEventChange += OnTabEventChange;

        scroller.Delegate = this;
    }

	void OnDestory()
	{
		if (csBtMyRoomTab.TabEventChange != null)
		{
			csBtMyRoomTab.TabEventChange -= OnTabEventChange;
		}
	}

	void OnTabEventChange(csMyRoomManager.enMENU SelKind)
	{
        if(m_ChildOffSet == null)
        {
            m_ChildOffSet = transform.Find("offset");
        }
        if(m_ChildOffSet != null)
        {
            m_ChildOffSet.gameObject.SetActive(m_ListKind == SelKind ? true : false);


            if (m_ListKind == SelKind)
            {
                m_list = csMyRoomManager.GetProductList(m_ListKind);

                scroller.ClearAll();
                scroller.ReloadData();
            }

        }
    }

    public void OnChangeEquip(string pid)
    {
        NetworkManager.Instance.ChangeMyRoomAction(pid, OnRcvChangeEquip);
    }

    private void OnRcvChangeEquip(string pid, bool result, string reason)
    {
        if (result)
        {
            ItemUseChange.Invoke(pid);

        }

        if (reason.Length > 0)
        {
            System.Action empty = () => { };
            Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)reason, empty);
        }
    }

    /*
    void CreateItems(List<csJson.js_myroom_item> list)
	{
        Object Obj = Resources.Load (string.Format ("android/pf_myroomitem")); 
		for (int i = 0; i < list.Count; i++)
		{
			csJson.js_myroom_item info = list [i];
			GameObject pfItem = (GameObject)Instantiate (Obj, Vector3.zero, Quaternion.identity);
			pfItem.transform.parent = m_Container.transform;
			pfItem.transform.localScale = Vector3.one;
			RectTransform rt = pfItem.GetComponent<RectTransform> ();
			pfItem.transform.localPosition = Vector3.zero;
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, rt.rect.width);
			rt.localPosition += new Vector3 (i * rt.rect.width, 0, 0);
			pfItem.name = info.pid;
            pfItem.GetComponent<csMyRoomItem> ().SetInfo (info);
        }
	}
    */
    #region EnhancedScroller Handlers

    /// <summary>
    /// This tells the scroller the number of cells that should have room allocated. This should be the length of your data array.
    /// </summary>
    /// <param name="scroller">The scroller that is requesting the data size</param>
    /// <returns>The number of cells</returns>
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        // in this example, we just pass the number of our data elements
        return m_list.Count;
    }

    /// <summary>
    /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
    /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
    /// cell size will be the width.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell size</param>
    /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
    /// <returns>The size of the cell</returns>
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        // in this example, even numbered cells are 30 pixels tall, odd numbered cells are 100 pixels tall
        return cellViewPrefab.GetComponent<RectTransform>().sizeDelta.x; //442f;// (dataIndex % 2 == 0 ? 30f : 100f);
    }

    /// <summary>
    /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
    /// Some examples of this would be headers, footers, and other grouping cells.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell</param>
    /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
    /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
    /// <returns>The cell for the scroller to use</returns>
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        csMyRoomItem cellView = scroller.GetCellView(cellViewPrefab) as csMyRoomItem;

        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
        cellView.name = "Cell " + dataIndex.ToString();

        // in this example, we just pass the data to our cell's view which will update its UI
        cellView.SetInfo(m_list[dataIndex]);
        cellView.UpdateUI();
        // return the cell to the scroller
        return cellView;
    }

    #endregion
}
