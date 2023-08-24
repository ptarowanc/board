using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListManager : MonoBehaviour {
    public LoopListView mLoopListView;
    public int ItemCount;
    public List<Rmi.Marshaler.RoomInfo> roomlist;
    public int roomCount;
	// Use this for initialization
	void Start () {
        mLoopListView.InitListView(ItemCount, OnItemCreated, OnItemUpdated);
        //mLoopListView.SetListItemCount(0);
        //mLoopListView.SetListItemCount(60);
        //NetworkManager.Instance.client.requset_room_list(HostID.Remote_Server, ZNet.CPackOption.Basic, 1);
    }
    void OnItemCreated(LoopListViewItem item)
    {
        RoomItem itemScript = item.GetComponent<RoomItem>();
        itemScript.mBtn.onClick.AddListener(delegate () {
            this.OnItemBtnClicked(item);
        });

    }

    void OnItemUpdated(LoopListViewItem item)
    {
        RoomItem itemScript = item.GetComponent<RoomItem>();
        itemScript.RoomName.text = item.ItemIndex.ToString();
        if(roomlist != null)
        {
            if(roomlist.Count == roomCount)
            {
                if (roomlist[item.ItemIndex].userCount < 2)
                {
                    itemScript.Open.text = "Open";
                }
                else
                {
                    itemScript.Open.text = "Close";
                }
            }
            
        }
        
        /*
        */
        
    }

    void OnItemBtnClicked(LoopListViewItem item)
    {

    }
    // Update is called once per frame
    void Update () {
		
	}


}
