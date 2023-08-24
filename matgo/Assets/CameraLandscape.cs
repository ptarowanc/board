using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLandscape : MonoBehaviour
{
    void Awake()
    {
        //Screen.SetResolution(1920, 1080, true);
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Screen.SetResolution(1920, 1080, true);

    }

    // Update is called once per frame
    void Update()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
