using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupQuit : MonoBehaviour
{
    public void OnClick()
    {
        NetworkManager.Instance.PopupLevelDown();
        transform.parent.gameObject.SetActive(false);

#if TEST
        if (transform.parent.Find("Text").GetComponent<UnityEngine.UI.Text>().text.Contains("끊어"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("M_Logo");
        }
#else
        if (transform.parent.Find("Text").GetComponent<UnityEngine.UI.Text>().text.Contains("끊어"))
        {
            Application.Quit();
        }
#endif
    }
}