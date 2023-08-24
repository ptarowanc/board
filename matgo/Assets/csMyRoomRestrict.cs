using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csMyRoomRestrict : MonoBehaviour
{
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        input.text = "";
    }

    public void OnSetRestrict()
    {
        string pid = "restrict,";
        int day = 0;
        if(int.TryParse(input.text,out day))
        {
            pid += day.ToString();
            NetworkManager.Instance.ChangeMyRoomAction(pid, OnRcvSetRestrict);
        }
    }
    private void OnRcvSetRestrict(string pid, bool result, string reason)
    {
        if (pid.Contains("restrict"))
        {
            if (result)
            {
                System.Action goToLogin = () => { Application.Quit(); };
                Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)reason, goToLogin);
            }
            else
            {
                System.Action empty = () => { };
                Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)reason, empty);
            }
        }
        else
        {
            System.Action empty = () => { };
            Unitycoding.EventHandler.Execute(csFlagDef.EVTNAME_REQ_POPUP_OPEN, csFlagDef.POPUP_KIND_GLOBALMSG, (object)reason, empty);
        }
    }
}
