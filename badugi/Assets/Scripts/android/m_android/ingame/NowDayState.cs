using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NowDayState : MonoBehaviour {

    [Header("GameObject")]
    [SerializeField] SpriteRenderer bg;
    [SerializeField] SpriteRenderer morning;
    [SerializeField] SpriteRenderer noon;
    [SerializeField] SpriteRenderer night;

    [Header("Sprite Resource")]
    [SerializeField] Sprite[] sprite_morninng;
    [SerializeField] Sprite[] sprite_noon;
    [SerializeField] Sprite[] sprite_night;


    //public AnimationCurve ac;

    // Use this for initialization
    void Start () {
        TurnOff();
    }

    public void Play(GameRound round)
    {
        if(round == GameRound.END)
        {
            TurnOff();
        }
        else
        {
            TurnOn(round);
        }
    }

    void Show()
    {
        bg.enabled = true;

        morning.enabled = true;
        noon.enabled = true;
        night.enabled = true;
    }

    public void TurnOff()
    {
        Show();

        morning.sprite = sprite_morninng[0];
        noon.sprite = sprite_noon[0];
        night.sprite = sprite_night[0];
    }
    void TurnOn(GameRound round)
    {
        TurnOff();

        SpriteRenderer day = null;
        Sprite icon = null;
        Sprite active = null;
        switch (round)
        {
            case GameRound.MORNING:
                day = morning;
                active = sprite_morninng[2];
                break;
            case GameRound.AFTERNOON:
                day = noon;
                active = sprite_noon[2];
                break;
            case GameRound.EVENING:
                day = night;
                active = sprite_night[2];
                break;
        }
        if (day != null)
        {
            day.sprite = active;
        }
    }

    //IEnumerator CoAni(GameRound round)
    //{
    //    Image day = null;
    //    Sprite icon = null;
    //    Sprite active = null;
    //    switch (round)
    //    {
    //        case GameRound.MORNING:
    //            day = morning;
    //            icon = sprite_morninng[1];
    //            active = sprite_morninng[2];
    //            break;
    //        case GameRound.AFTERNOON:
    //            day = noon;
    //            icon = sprite_noon[1];
    //            active = sprite_noon[2];
    //            break;
    //        case GameRound.EVENING:
    //            day = night;
    //            icon = sprite_night[1];
    //            active = sprite_night[2];
    //            break;
    //    }
    //    if(day != null)
    //    {
    //        Vector3 StartScale = Vector3.one;
    //        Vector3 EndScale = new Vector3(1.5f, 1.5f, 1.5f);

    //        day.overrideSprite = icon;
    //        day.transform.localScale = StartScale;

    //        float PlayTime = 0.4f;
    //        float nowTime = 0.0f;
    //        while (nowTime <= PlayTime)
    //        {
    //            day.transform.localScale
    //                = Vector3.Lerp(StartScale, EndScale, ac.Evaluate(nowTime / PlayTime));
    //            nowTime += Time.deltaTime;
    //            yield return null;
    //        }

    //        StartScale = new Vector3(1.5f, 1.5f, 1.5f);
    //        EndScale = Vector3.one;
    //        nowTime = 0.0f;
    //        while (nowTime <= PlayTime)
    //        {
    //            day.transform.localScale
    //                = Vector3.Lerp(StartScale, EndScale, ac.Evaluate(nowTime / PlayTime));
    //            nowTime += Time.deltaTime;
    //            yield return null;
    //        }
    //        day.transform.localScale = EndScale;

    //        day.overrideSprite = active;
    //    }
    //    yield return null;
    //}

}
