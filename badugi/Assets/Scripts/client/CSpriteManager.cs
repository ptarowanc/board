using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSpriteManager : CSingletonMonobehaviour<CSpriteManager>
{
	Dictionary<string, Sprite> sprites_by_name;
	Sprite[] card;

    Dictionary<string, Sprite> spriteUI = new Dictionary<string, Sprite>();
    Sprite[] ui;

    void Awake()
	{
		card = Resources.LoadAll<Sprite>("Atlas/Poker");
		Sprite[] sprites = Resources.LoadAll<Sprite>("Atlas/Poker");

		sprites_by_name = new Dictionary<string, Sprite>();
		for (int i = 0; i < sprites.Length; ++i)
		{
			sprites_by_name.Add(sprites[i].name, sprites[i]);
		}

        // 180912 로비 UI 관련
        ui = Resources.LoadAll<Sprite>("Atlas/UI");
        for (int i = 0; i < ui.Length; ++i) spriteUI.Add(ui[i].name, ui[i]);
    }

    // 180912 로비 UI 관련
    public Sprite GetSpriteByName(string name)
    {
        return spriteUI[name];
    }

    public Sprite get_sprite(string name)
	{
		if (!sprites_by_name.ContainsKey(name))
		{
			return null;
		}

		return sprites_by_name[name];
	}

	public Sprite get_card_sprite(int index)
	{
		return card[index];
	}
}