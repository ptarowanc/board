using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {
    Rect moveWindowRect;
    Rect SizeBeforeMaximize;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {

        moveWindowRect = new Rect(0, 0, Screen.width, 31);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    public void MinButton()
    {
        Window.Minimize();
    }
    public void FullButton()
    {
        if (Screen.width == UnityEngine.Screen.currentResolution.width && Screen.height == UnityEngine.Screen.currentResolution.height)
        {
            Window.SetRect(SizeBeforeMaximize);
        }
        else
        {
            SizeBeforeMaximize = Window.GetRect();
            Window.Maximize();
        }
    }

    public void ExitButton()
    {
        Window.Exit();
    }
    void OnGUI()
    {
       
        if (Screen.width != UnityEngine.Screen.currentResolution.width && Screen.height != UnityEngine.Screen.currentResolution.height)
        {
            if (moveWindowRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    Window.GrabStart();
                }
                if ((Event.current.type == EventType.MouseUp && Event.current.button == 0) || (Window.MoveWindow && !Input.GetKey(KeyCode.Mouse0)))
                {
                    Window.GrabEnd();
                }
            }
        }
    }

}
