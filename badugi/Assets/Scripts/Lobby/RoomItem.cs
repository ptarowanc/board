using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour {
    public GameObject RoomName1;
    public GameObject RoomName2;
    public GameObject mOpen;
    public GameObject mClose;

    public void Open()
    {
        mOpen.SetActive(true);
        mClose.SetActive(false);
    }
    public void Close()
    {
        mOpen.SetActive(false);
        mClose.SetActive(true);
    }
}
