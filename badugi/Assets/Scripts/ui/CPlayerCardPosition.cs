using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CPlayerCardPosition : MonoBehaviour
{
	// 플레이어가 손에 들고 있는 패의 위치.
	public List<GameObject> hands;
    List<Vector3> hand_positions;


	void Awake()
	{
		this.hand_positions = new List<Vector3>();
		//make_slot_positions(transform.Find("Hand"), this.hand_positions);
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

	public Vector3 get_hand_position(int slot_index)
	{
        return this.hands[slot_index].transform.position;

        //return this.hand_positions[slot_index];
    }


}