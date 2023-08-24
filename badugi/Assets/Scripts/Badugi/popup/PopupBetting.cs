using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using ZNet;

public class PopupBetting : MonoBehaviour
{
    public Button die;
    public Button check;
    public Button ddadang;
    public Button bbing;
    public Button quater;
    public Button half;
    public Button call;
    void Awake()
    {
        die.onClick.AddListener(on_touch_die);
        check.onClick.AddListener(on_touch_check);
        ddadang.onClick.AddListener(on_touch_ddaddang);
        bbing.onClick.AddListener(on_touch_bbing);
        quater.onClick.AddListener(on_touch_quater);
        half.onClick.AddListener(on_touch_half);
        call.onClick.AddListener(on_touch_call);
    }

    public void DeactiveAllButton()
    {
        die.interactable = false;
        check.interactable = false;
        ddadang.interactable = false;
        bbing.interactable = false;
        quater.interactable = false;
        half.interactable = false;
        call.interactable = false;
    }

    public void SetButtonsActive(bool _call, bool _bbing, bool _quater, bool _half, bool _die, bool _check, bool _ddaddang)
    {
        call.interactable = _call;
        bbing.interactable = _bbing;
        quater.interactable = false;// _quater;
        half.interactable = _half;
        die.interactable = _die;
        check.interactable = _check;
        ddadang.interactable = false;//_ddaddang;
    }


    private void on_touch_die()
    {
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionBet;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)PlayGameUI.Instance.player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)BETTING.DIE);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        DeactiveAllButton();
        PlayGameUI.Instance.StopCountDownSound();
    }
    private void on_touch_check()
    {
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionBet;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)PlayGameUI.Instance.player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)BETTING.CHECK);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        DeactiveAllButton();
        PlayGameUI.Instance.StopCountDownSound();
    }
    private void on_touch_ddaddang()
    {
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionBet;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)PlayGameUI.Instance.player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)BETTING.DDADDANG);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        DeactiveAllButton();
        PlayGameUI.Instance.StopCountDownSound();
    }
    private void on_touch_bbing()
    {
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        //Invoke("DeactiveAllButton", 0.2f);
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionBet;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)PlayGameUI.Instance.player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)BETTING.BBING);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        DeactiveAllButton();
        PlayGameUI.Instance.StopCountDownSound();
    }
    private void on_touch_quater()
    {
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionBet;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)PlayGameUI.Instance.player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)BETTING.QUATER);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        DeactiveAllButton();
        PlayGameUI.Instance.StopCountDownSound();
    }
    private void on_touch_half()
    {
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionBet;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)PlayGameUI.Instance.player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)BETTING.HARF);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        DeactiveAllButton();
        PlayGameUI.Instance.StopCountDownSound();
    }
    private void on_touch_call()
    {
        PlayGameUI.Instance.keyState = PlayGameUI.UserInputState.NONE;
        CMessage newmsg = new CMessage();
        ZNet.PacketType msgID = (ZNet.PacketType)SS.Common.GameActionBet;
        newmsg.WriteStart(msgID, CPackOption.Basic, 0, true);
        Rmi.Marshaler.Write(newmsg, (byte)PlayGameUI.Instance.player_me_index);//player index
        Rmi.Marshaler.Write(newmsg, (byte)BETTING.CALL);
        NetworkManager.Instance.client.PacketSend(RemoteID.Remote_Server, CPackOption.Basic, newmsg);
        DeactiveAllButton();
        PlayGameUI.Instance.StopCountDownSound();
    }

    public void ClickButton(BETTING selected)
    {
        switch (selected)
        {
            case BETTING.CALL:
                PlayGameUI.Instance.StopCountDownSound();
                call.onClick.Invoke();
                break;
            case BETTING.BBING:
                PlayGameUI.Instance.StopCountDownSound();
                bbing.onClick.Invoke();
                break;
            case BETTING.QUATER:
                PlayGameUI.Instance.StopCountDownSound();
                quater.onClick.Invoke();
                break;
            case BETTING.HARF:
                PlayGameUI.Instance.StopCountDownSound();
                half.onClick.Invoke();
                break;
            case BETTING.DIE:
                PlayGameUI.Instance.StopCountDownSound();
                die.onClick.Invoke();
                break;
            case BETTING.CHECK:
                PlayGameUI.Instance.StopCountDownSound();
                check.onClick.Invoke();
                break;
            case BETTING.DDADDANG:
                PlayGameUI.Instance.StopCountDownSound();
                ddadang.onClick.Invoke();
                break;
        }
    }
}
