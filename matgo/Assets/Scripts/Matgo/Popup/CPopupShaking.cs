using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CPopupShaking : MonoBehaviour
{

    CCard selected_card_info;
    byte card_slot;

    void Awake()
    {
        transform.Find("button_yes").GetComponent<Button>().onClick.AddListener(this.on_touch_01);
        transform.Find("button_no").GetComponent<Button>().onClick.AddListener(this.on_touch_02);
    }


    public void refresh(CCard selected_card_info, byte card_slot)
    {
        // 키보드 받음
        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.SHAKE;

        this.selected_card_info = selected_card_info;
        this.card_slot = card_slot;
    }


    void on_touch_01()
    {
        on_choice_shaking(1);
    }


    void on_touch_02()
    {
        on_choice_shaking(0);
    }


    public void on_choice_shaking(byte is_shaking)
    {
        // 키보드 안받음
        CPlayGameUI.Instance.keyState = CPlayGameUI.UserInputState.NONE;

        gameObject.SetActive(false);

        CPlayGameUI.send_select_card(this.selected_card_info, this.card_slot, is_shaking);
    }
}

