using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLevel : MonoBehaviour
{
    public int MyPopupLevel;

	void Update ()
    {
        if (NetworkManager.Instance.PopupLevel < MyPopupLevel) gameObject.GetComponent<Button>().enabled = false;
        else if (NetworkManager.Instance.PopupLevel >= MyPopupLevel) gameObject.GetComponent<Button>().enabled = true;
    }
}