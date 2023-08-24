using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListManager : MonoBehaviour
{
    public LoopListView mLoopListView;
    public int ItemCount;
    public List<Rmi.Marshaler.RoomInfo> roomlist;
    public int roomCount;

	void Start ()
    {
        mLoopListView.InitListView(ItemCount, OnItemCreated, OnItemUpdated);
		//mLoopListView.SetListItemCount(0);
        //mLoopListView.SetListItemCount(15);
		//NetworkManager.Instance.client.requset_room_list(ZNet.RemoteID.Remote_Server, ZNet.CPackOption.Basic, 1);
    }
    void OnItemCreated(LoopListViewItem item)
    {
        RoomItem itemScript = item.GetComponent<RoomItem>();
        /*itemScript.mBtn.onClick.AddListener(delegate () {
            this.OnItemBtnClicked(item);
        });*/
    }

    void OnItemUpdated(LoopListViewItem item)
    {
        RoomItem itemScript = item.GetComponent<RoomItem>();

        for (int i = 0; i < roomlist.Count; i++)
        {
            if (roomlist[i].number < 10)
            {
                itemScript.RoomName1.SetActive(true);
                itemScript.RoomName2.SetActive(false);
                itemScript.RoomName1.transform.Find("TextRoomName").gameObject.GetComponent<tk2dSprite>().SetSprite("room_number_0" + roomlist[i].number.ToString());
            }
            else if (item.ItemIndex < 100)
            {
                itemScript.RoomName1.SetActive(false);
                itemScript.RoomName2.SetActive(true);
                int a = roomlist[i].number % 100 / 10;
                int b = roomlist[i].number % 100 % 10;
                itemScript.RoomName1.transform.Find("TextRoomName1").gameObject.GetComponent<tk2dSprite>().SetSprite("room_number_0" + a.ToString());
                itemScript.RoomName1.transform.Find("TextRoomName2").gameObject.GetComponent<tk2dSprite>().SetSprite("room_number_0" + b.ToString());
            }
        }

        if (roomlist != null)
        {
            if(roomlist.Count == roomCount)
            {
                if(roomlist[item.ItemIndex].restrict)
                {
                    itemScript.Close();
                }else
                {
                    if (roomlist[item.ItemIndex].userCount < 5)
                    {
                        itemScript.Open();
                    }
                    else
                    {
                        itemScript.Close();
                    }
                }
            }
        }
    }

    void OnItemBtnClicked(LoopListViewItem item)
    {

    }
}