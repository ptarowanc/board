using System;
using System.Collections;
using System.Collections.Generic;

public class CCardManager
{
    public List<CCard> cards { get; private set; }

	public CCardManager()
	{
		this.cards = new List<CCard>();
	}


    public void make_all_cards()
    {
        // Generate cards.
        Queue<PAE_TYPE> total_pae_type = new Queue<PAE_TYPE>();
        // 1
        total_pae_type.Enqueue(PAE_TYPE.KWANG);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 2
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 3
        total_pae_type.Enqueue(PAE_TYPE.KWANG);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 4
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 5
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 6
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 7
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 8
        total_pae_type.Enqueue(PAE_TYPE.KWANG);
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 9
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 10
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 11
        total_pae_type.Enqueue(PAE_TYPE.KWANG);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        // 12
        total_pae_type.Enqueue(PAE_TYPE.KWANG);
        total_pae_type.Enqueue(PAE_TYPE.YEOL);
        total_pae_type.Enqueue(PAE_TYPE.TEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        this.cards.Clear();
        for (byte number = 0; number < 12; ++number)
        {
            for (byte pos = 0; pos < 4; ++pos)
            {
                this.cards.Add(new CCard(number, total_pae_type.Dequeue(), pos));
            }
        }
        total_pae_type.Enqueue(PAE_TYPE.PEE);
        total_pae_type.Enqueue(PAE_TYPE.PEE);

        this.cards.Add(new CCard(12, total_pae_type.Dequeue(), 0));
        this.cards.Add(new CCard(12, total_pae_type.Dequeue(), 1));
    }

	public CCard find_card(byte number, PAE_TYPE pae_type, byte position)
	{
		return this.cards.Find(obj => obj.is_same(number, pae_type, position));
	}
}
