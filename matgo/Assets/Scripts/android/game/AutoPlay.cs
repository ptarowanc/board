using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlay : MonoBehaviour
{

    [Header("GameObject")]
    [SerializeField] GameObject button;
    [SerializeField] GameObject notice;

    [Header("Sprites")]
    [SerializeField] Sprite sprite_TurnOn;
    [SerializeField] Sprite sprite_TurnOff;

    public bool IsTurnOn { get; private set; }
    public float DelayTime { get; private set; }

    // Use this for initialization
    void Start()
    {
        DelayTime = 1.0f;
        HideButton();
    }

    public void StopWorking()
    {
        HideButton();
        TurnOff();
    }

    public void ShowButton()
    {
        if (button != null)
        {
            button.SetActive(true);
        }
    }

    public void HideButton()
    {
        if (button != null)
        {
            button.SetActive(false);
        }
    }

    public void TurnOn()
    {
        IsTurnOn = true;

        var image = button.GetComponent<Image>();
        image.sprite = sprite_TurnOn;
        notice.SetActive(true);
    }

    public void TurnOff()
    {
        IsTurnOn = false;

        var image = button.GetComponent<Image>();
        image.sprite = sprite_TurnOff;
        notice.SetActive(false);
    }

    public void Toggle()
    {
        if (IsTurnOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
}
