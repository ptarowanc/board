using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ZNet;

public class CPopupPlayerOrder : MonoBehaviour
{
    public GameObject[] slot;
    byte idx = 0;

    private void Awake()
    {
        slot[0].GetComponent<Button>().onClick.AddListener(set0);
        slot[1].GetComponent<Button>().onClick.AddListener(set1);
        slot[2].GetComponent<Button>().onClick.AddListener(set2);
        slot[3].GetComponent<Button>().onClick.AddListener(set3);
        slot[4].GetComponent<Button>().onClick.AddListener(set4);
        slot[5].GetComponent<Button>().onClick.AddListener(set5);
        slot[6].GetComponent<Button>().onClick.AddListener(set6);
        slot[7].GetComponent<Button>().onClick.AddListener(set7);
        //slot[8].GetComponent<Button>().onClick.AddListener(set8);
        //slot[9].GetComponent<Button>().onClick.AddListener(set9);
    }

    public void set0()
    {
        idx = 0;
        on_touch();
    }

    public void set1()
    {
        idx = 1;
        on_touch();
    }

    public void set2()
    {
        idx = 2;
        on_touch();
    }

    public void set3()
    {
        idx = 3;
        on_touch();
    }

    public void set4()
    {
        idx = 4;
        on_touch();
    }

    public void set5()
    {
        idx = 5;
        on_touch();
    }

    public void set6()
    {
        idx = 6;
        on_touch();
    }

    public void set7()
    {
        idx = 7;
        on_touch();
    }

    public void set8()
    {
        idx = 8;
        on_touch();
    }

    public void set9()
    {
        idx = 9;
        on_touch();
    }

    public void on_touch()
    {
        // 이제 선택불가
        for (int i = 0; i < slot.Length; i++) slot[i].GetComponent<Button>().enabled = false;

        // 카운터 종료
        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.NONE;

        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameSelectOrder;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, idx);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }

    public void reset(Sprite sprite)
	{
        for (int i = 0; i < slot.Length; i++)
        {
            slot[i].GetComponent<Image>().sprite = sprite;
            slot[i].transform.localPosition = CPlayGameUI.Instance.floor_slot_position[i];
            slot[i].transform.Find("me").gameObject.SetActive(false);
            slot[i].transform.Find("enemy").gameObject.SetActive(false);
            slot[i].GetComponent<Button>().enabled = true;
        }
    }

    public void update_slot_info(Sprite sprite, byte _idx)
    {
        slot[_idx].GetComponent<Image>().sprite = sprite;
    }

    public void choiceUpdate(byte playerIndex, int idx)
    {
        // 해당카드는 이제 고를수없음
        slot[idx].GetComponent<Button>().enabled = false;

        if (CPlayGameUI.Instance.is_me(playerIndex))
        {
            slot[idx].transform.Find("me").gameObject.SetActive(true);
            slot[idx].transform.Find("enemy").gameObject.SetActive(false);
        }
        else
        {
            slot[idx].transform.Find("me").gameObject.SetActive(false);
            slot[idx].transform.Find("enemy").gameObject.SetActive(true);
        }
    }
}