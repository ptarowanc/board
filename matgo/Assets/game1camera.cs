using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class game1camera : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    void Start()
    {

        //Camera camera = GetComponent<Camera>();
        //Rect rect = camera.rect;

        ////float scaleheight = ((float)1080 / 1920) / ((float)9 / 16);
        //float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        //float scalewidth = 1f / scaleheight;

        //if (scaleheight < 1)
        //{
        //    rect.height = scaleheight;
        //    rect.y = (1f - scaleheight) / 2f;
        //}
        //else
        //{
        //    rect.width = scalewidth;
        //    rect.x = (1f - scalewidth) / 2f;
        //}

        //camera.rect = rect;
    }

    //void OnPreCull()
    //{
    //    GL.Clear(true, true, Color.black);
    //}

    private void OnDestroy()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        Screen.SetResolution(1080, 1920, true);
    }
}
