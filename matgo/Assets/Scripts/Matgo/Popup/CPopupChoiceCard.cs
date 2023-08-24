using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ZNet;

public class CPopupChoiceCard : MonoBehaviour
{

    List<Image> slots;
    PLAYER_SELECT_CARD_RESULT result_type_from_server;

    void Awake()
    {
        this.slots = new List<Image>();
        for (int i = 0; i < 2; ++i)
        {
            Transform obj = transform.Find(string.Format("slot{0:D2}", (i + 1)));
            this.slots.Add(obj.GetComponent<Image>());
        }

        this.slots[0].GetComponent<Button>().onClick.AddListener(this.on_touch_01);
        this.slots[1].GetComponent<Button>().onClick.AddListener(this.on_touch_02);
    }


    public void refresh(Sprite card1, Sprite card2)
    {
        // 키보드 받음
        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.CHOICE;

        //Debug.Log(this.slots.Count);
        this.slots[0].sprite = card1;
        this.slots[1].sprite = card2;
    }


    void on_touch_01()
    {
        on_choice_card(0);
    }


    void on_touch_02()
    {
        on_choice_card(1);
    }


    public void on_choice_card(byte slot_index)
    {
        // 키보드 안받음
        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.NONE;

        gameObject.SetActive(false);

        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionChooseCard;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)slot_index);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
    }
}
