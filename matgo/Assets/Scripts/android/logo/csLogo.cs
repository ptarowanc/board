using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csLogo : MonoBehaviour {

    bool isAuto;
    // Use this for initialization

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(csFlagDef.SCREEN_WIDTH, csFlagDef.SCREEN_HEIGHT, true);

        isAuto = false;
#if UNITY_ANDROID && !UNITY_EDITOR
        string arguments = "";

        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        bool hasExtra = intent.Call<bool>("hasExtra", "arguments");

        if (hasExtra)
        {
            AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
            arguments = extras.Call<string>("getString", "arguments");

            if (string.IsNullOrEmpty(arguments) == false)
            {
                string[] split = arguments.Split('\n');

                if (split.Count() > 0)
                {
                    string command = split.First();

                    if (command == "auto" && split.Count() == 3)
                    {
                        isAuto = true;
                    }
                }
            }
        }
#endif
    }

    void Start ()
    {
        if(isAuto)
        {
            SceneManager.LoadScene("M_Android");
        }
        else
        {
            StartCoroutine("CoNextScene");
        }
	}
    IEnumerator CoNextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("M_Android");
    }
	
}
