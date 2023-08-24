using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CPlayerCardPosition : MonoBehaviour
{
    // 플레이어가 먹은 바닥패의 위치.
    Dictionary<PAE_TYPE, Vector3> floor_positions;

    // 플레이어가 손에 들고 있는 패의 위치.
    List<Vector3> hand_positions;


    void Awake()
    {
        List<Vector3> targets = new List<Vector3>();
        make_slot_positions(transform.Find("Floor"), targets);
        this.floor_positions = new Dictionary<PAE_TYPE, Vector3>();
        this.floor_positions.Add(PAE_TYPE.KWANG, targets[0]);
        this.floor_positions.Add(PAE_TYPE.YEOL, targets[1]);
        this.floor_positions.Add(PAE_TYPE.TEE, targets[2]);
        this.floor_positions.Add(PAE_TYPE.PEE, targets[3]);

        this.hand_positions = new List<Vector3>();
        make_slot_positions(transform.Find("Hand"), this.hand_positions);
    }


    void make_slot_positions(Transform root, List<Vector3> targets)
    {
        Transform[] slots = root.GetComponentsInChildren<Transform>();
        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i] == root)
            {
                continue;
            }

            targets.Add(slots[i].localPosition);
        }
    }


    public Vector3 get_floor_position(byte playerIndex, int card_count, PAE_TYPE pae_type, bool isMe, int card_max)
    {
        int cardCountPos = 0;
        int cardYpos = 0;

        if(NetworkManager.Instance.Screen)
        {
            if (pae_type == PAE_TYPE.PEE)
            {
                cardCountPos = (card_count % 10) * 24;
                cardYpos = (card_count / 10) * 30;
                if (isMe) cardYpos = (card_count / 10) * -30;
            }
            else if (pae_type == PAE_TYPE.YEOL)
            {
                if (card_max > 5)
                {
                    cardCountPos = card_count * (30 - ((20 / 9) * (9 - (9 - card_max))));
                }
                else
                {
                    cardCountPos = card_count * 30;
                }
            }
            else if (pae_type == PAE_TYPE.TEE)
            {
                if (card_max > 5)
                {
                    cardCountPos = card_count * (30 - ((20 / 10) * (10 - (10 - card_max))));
                }
                else
                {
                    cardCountPos = card_count * 30;
                }
            }
            else if (pae_type == PAE_TYPE.KWANG)
            {
                if (card_max >= 4)
                {
                    cardCountPos = card_count * 25;
                }
                else
                {
                    cardCountPos = card_count * 30;
                }
            }
            else
            {
                cardCountPos = card_count * 30;
            }
        }
        else
        {
            if (pae_type == PAE_TYPE.PEE)
            {
                //cardCountPos = card_count * (30 - ((20 / 9) * (9 - (9 - card_max))));
                //cardCountPos = (card_count % 10) * 24;
                //cardYpos = (card_count / 10) * 30;
                //if (isMe) cardYpos = (card_count / 10) * -30;
                cardCountPos = card_count * 20;
            }
            else if (pae_type == PAE_TYPE.YEOL)
            {
                //if (card_max > 5)
                //{
                //    cardCountPos = card_count * (30 - ((20 / 9) * (9 - (9 - card_max))));
                //}
                //else
                //{
                //    cardCountPos = card_count * 30;
                //}
                cardCountPos = card_count * 20;
            }
            else if (pae_type == PAE_TYPE.TEE)
            {
                //if (card_max > 5)
                //{
                //    cardCountPos = card_count * (30 - ((20 / 10) * (10 - (10 - card_max))));
                //}
                //else
                //{
                //    cardCountPos = card_count * 30;
                //}
                cardCountPos = card_count * 20;
            }
            else if (pae_type == PAE_TYPE.KWANG)
            {
                //if (card_max >= 4)
                //{
                //    cardCountPos = card_count * 25;
                //}
                //else
                //{
                //    cardCountPos = card_count * 30;
                //}
                cardCountPos = card_count * 25;
            }
            else
            {
                cardCountPos = card_count * 25;
            }
        }

        Vector3 newpos = new Vector3(cardCountPos, cardYpos, 0);

        return this.floor_positions[pae_type] + newpos;
    }


    public Vector3 get_hand_position(int slot_index)
    {
        return this.hand_positions[slot_index];
    }
}