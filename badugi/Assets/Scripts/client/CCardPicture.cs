using UnityEngine;
using ClassLibraryCardInfo;

public class CCardPicture : MonoBehaviour
{
    public CardInfo.sCARD_INFO card;
    public SpriteRenderer sprite_renderer { get; private set; }

    public byte slot { get; private set; }
    public byte server_slot { get; private set; }
    BoxCollider box_collider;
    public bool select { get; private set; }

    void Awake()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;

        sprite_renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        box_collider = gameObject.GetComponent<BoxCollider>();
        select = false;
        slot = byte.MaxValue;
    }

    public void set_slot_index(byte slot)
    {
        byte idx = slot++;
        this.slot = idx;
        sprite_renderer.sortingOrder = idx;
    }

    public void set_server_slot_index(byte slot)
    {
        this.server_slot = slot;
    }

    public void update_card(CardInfo.sCARD_INFO card, Sprite image)
    {
        this.card = card;
        sprite_renderer.sprite = image;
    }

    public void update_backcard(Sprite back_image)
    {
        card.m_btIsState = 1;
        update_image(back_image);
    }

    public void update_image(Sprite image)
    {
        sprite_renderer.sprite = image;
    }

    public bool is_same(byte number)
    {
        return true;//this.card.number == number;
    }

    public void SetSelect(bool setSelect)
    {
        select = setSelect;
    }

    public void SetAutoSelect()
    {
        if (select)
        {
            select = false;
        }
        else
        {
            select = true;
        }
        Vector3 localPosition = transform.localPosition;
        if (!select)
            localPosition.y -= 20f;
        else
            localPosition.y += 20f;
        transform.localPosition = localPosition;
    }

    public void SetSelectCard()
    {
        Vector3 localPosition = transform.localPosition;
        if (!select)
            localPosition.y += 20f;
        else
            localPosition.y -= 20f;
        transform.localPosition = localPosition;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public void on_touch()
    {
        if (select)
        {
            select = false;
        }
        else
        {
            select = true;
        }
    }

    public void enable_collider(bool flag)
    {
        box_collider.enabled = flag;
    }

    public bool is_back_card()
    {
        return card.m_btIsState == 1;
    }
}