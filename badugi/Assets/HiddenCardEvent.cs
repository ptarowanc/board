using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(BoxCollider2D))]

public class HiddenCardEvent : MonoBehaviour
{
    //private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        PlayGameUI.Instance.HiddenCardStop = true;
        offset = gameObject.transform.position -
            Camera.main.ScreenToWorldPoint(new Vector3(gameObject.transform.position.x, Input.mousePosition.y, 10.0f));
    }

    void OnMouseUp()
    {
        PlayGameUI.Instance.HiddenCardSkip = true;
    }

    void OnMouseDrag()
    {
        if (PlayGameUI.Instance.HiddenCardSkip == true) return;
        Vector3 newPosition = new Vector3(gameObject.transform.position.x, Input.mousePosition.y, 10.0f);
        Vector3 newpos = Camera.main.ScreenToWorldPoint(newPosition) + offset;
        newpos.y = Mathf.Min(-345, newpos.y);
        transform.position = newpos;

        if (transform.position.y < -455)
        {
            PlayGameUI.Instance.HiddenCardSkip = true;
        }
    }
}