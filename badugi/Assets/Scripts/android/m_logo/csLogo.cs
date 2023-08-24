using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csLogo : MonoBehaviour {

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(csFlagDef.SCREEN_WIDTH, csFlagDef.SCREEN_HEIGHT, true);
    }
    // Use this for initialization
    void Start ()
    {
        StartCoroutine("CoNextScene");
	}
    IEnumerator CoNextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("M_Android");
    }
	
}
