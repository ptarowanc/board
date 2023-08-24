using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPopupChongtong : MonoBehaviour {

    List<Image> slots;

    void Awake()
    {
        this.slots = new List<Image>();
        for (int i = 0; i < 4; ++i)
        {
            Transform obj = transform.Find(string.Format("slot{0:D2}", (i + 1)));
            this.slots.Add(obj.GetComponent<Image>());
        }
    }


    public void refresh(List<Sprite> sprites)
    {
        for (int i = 0; i < this.slots.Count; ++i)
        {
            this.slots[i].sprite = sprites[i];
        }
    }
}
