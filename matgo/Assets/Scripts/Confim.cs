using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confim : MonoBehaviour
{
    public int roomNum;
    public GameObject pw;

    public void OnClick()
    {
        LobbyPopup.Instance.ConfirmJoinInfo(ChannerManager.Instance.NowChannel, roomNum, pw.GetComponent<InputField>().text);
    }
}