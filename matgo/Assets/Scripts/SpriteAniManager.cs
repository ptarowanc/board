using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAniManager : CSingletonMonobehaviour<SpriteAniManager>
{
    public GameObject[] SpriteAnimation;

    public enum SpriteAni
    {
        TK2D_MISSIONTITLE,
        TK2D_MISSIONSTART,
        TK2D_MISSIONSUCCESS,
        TK2D_MISSIONFAIL,

        Count
    }

    public void ResetAll()
    {
        for (int i = 0; i < SpriteAnimation.Length; i++)
        {
            SpriteAnimation[i].SetActive(false);
        }
    }

    public void PlaySpriteAni(SpriteAni name, bool clear)
    {
        SpriteAnimation[(int)name].SetActive(true);

        SpriteAnimation[(int)name].GetComponent<tk2dSpriteAnimator>().Play();

        if (clear) Clear(name);
    }

    public IEnumerator CoPlaySpriteAni(SpriteAni name, bool clear)
    {
        SpriteAnimation[(int)name].SetActive(true);

        SpriteAnimation[(int)name].GetComponent<tk2dSpriteAnimator>().Play();

        var temp = SpriteAnimation[(int)name].GetComponent<tk2dSpriteAnimator>();

        while (temp.Playing)
        {
            yield return null;
        }

        if (clear) Clear(name);
    }

    IEnumerator Clear(SpriteAni name)
    {
        var temp = SpriteAnimation[(int)name].GetComponent<tk2dSpriteAnimator>();

        while (temp.Playing)
        {
            yield return null;
        }

        SpriteAnimation[(int)name].SetActive(false);
    }
}