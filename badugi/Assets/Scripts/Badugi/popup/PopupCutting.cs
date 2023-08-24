using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZNet;

public class PopupCutting : MonoBehaviour
{
    Button Change;
    Button Pass;
    void Awake()
    {
        Change = transform.Find("button_change").GetComponent<Button>();
        Pass = transform.Find("button_pass").GetComponent<Button>();
        Change.onClick.AddListener(this.on_touch_change);
        Pass.onClick.AddListener(this.on_touch_pass);
    }

    public void ClickButton(bool change)
    {
        if (change)
        {
            Change.onClick.Invoke();
        }
        else
        {
            Pass.onClick.Invoke();
        }
    }

    public void on_touch_change()
    {
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        List<CCardPicture> cards = new List<CCardPicture>();
        cards = PlayGameUI.Instance.player_hand_card_manager[0].getSelectCards();
        //Debug.Log(cards.Count);
        PlayGameUI.Instance.ChangeCard(cards);
        PlayGameUI.Instance.StopCountDownSound();
        if (cards.Count > 0)
        {
            UIManager.Instance.hide(UI_PAGE_BADUK.CHANGECARD);
            UIManager.Instance.show(UI_PAGE_BADUK.BETTING);
        }

    }
    public void on_touch_pass()
    {
        PlayGameUI.Instance.StopCountDownSound();
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionChangeCard;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)0);//player index
        Rmi.Marshaler.Write(newmsg, (byte)CHANGECARD.PASS);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        UIManager.Instance.hide(UI_PAGE_BADUK.CHANGECARD);
        UIManager.Instance.show(UI_PAGE_BADUK.BETTING);

    }
}
