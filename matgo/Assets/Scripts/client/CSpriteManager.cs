using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CSpriteManager : CSingletonMonobehaviour<CSpriteManager>
{
    Dictionary<string, Sprite> sprites_by_name;
	Sprite[] hwatoo;

    Dictionary<string, Sprite> spriteUI = new Dictionary<string, Sprite>();
    Sprite[] ui;

    void Awake()
	{
        if(csAndroidManager.CardAsset != null)
        {
            hwatoo = csAndroidManager.CardAsset.LoadAllAssets<Sprite>();
            Sprite[] sprites = csAndroidManager.CardAsset.LoadAllAssets<Sprite>();

            sprites_by_name = new Dictionary<string, Sprite>();
            for (int i = 0; i < sprites.Length; ++i)
            {
                sprites_by_name.Add(sprites[i].name, sprites[i]);
            }
        }

#if UNITY_EDITOR
        hwatoo = Resources.LoadAll<Sprite>("Atlas/Cards");
        Sprite[] sprites2 = Resources.LoadAll<Sprite>("Atlas/Cards");

        sprites_by_name = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites2.Length; ++i)
        {
            sprites_by_name.Add(sprites2[i].name, sprites2[i]);
        }
#endif

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
        
		return hwatoo[index];
	}
}