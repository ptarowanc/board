using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ZNet;

public class CPopupKookjin : MonoBehaviour {

	void Awake()
	{
		transform.Find("button_yes").GetComponent<Button>().onClick.AddListener(this.on_touch_01);
		transform.Find("button_no").GetComponent<Button>().onClick.AddListener(this.on_touch_02);
	}


	void on_touch_01()
	{
		on_choice_kookjin(1);
	}


	void on_touch_02()
	{
		on_choice_kookjin(0);
	}


	public void on_choice_kookjin(byte use_kookjin)
	{
        // 키보드 안받음
        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.NONE;

        gameObject.SetActive(false);

        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameSelectKookjin;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, use_kookjin);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }
}
