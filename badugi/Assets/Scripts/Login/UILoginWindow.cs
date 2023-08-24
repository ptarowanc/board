using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using Unitycoding.UIWidgets;
using Unitycoding;

public class UILoginWindow : UIWidget
{
    [Header("Reference")]
    [SerializeField]
    private InputField username;
    [SerializeField]
    private InputField password;
    [SerializeField]
    private Toggle rememberMe;
    [SerializeField]
    private Button login;

    private MessageBox messageBox;
    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        messageBox = UIUtility.Find<MessageBox>("MessageBox");
        username.text = PlayerPrefs.GetString("username", string.Empty);
        password.text = PlayerPrefs.GetString("password", string.Empty);
        if (rememberMe != null)
        {
            rememberMe.isOn = string.IsNullOrEmpty(username.text) ? false : true;
        }

        EventHandler.Register("OnLogin", OnLogin);
        EventHandler.Register("OnFailedToLogin", OnFailedToLogin);
        EventHandler.Register("OnFailedToLogin1", OnFailedToLogin1);
        EventHandler.Register("OnFailedToLogin2", OnFailedToLogin2);
        EventHandler.Register("OnFailedToLogin3", OnFailedToLogin3);
        EventHandler.Register("OnFailedToLogin4", OnFailedToLogin4);
        EventHandler.Register("OnFailedToLogin5", OnFailedToLogin5);
        EventHandler.Register("OnFailedServerConnect", OnFailedServerConnect);
        
        login.onClick.AddListener(LoginUsingFields);
    }
    public void LoginUsingFields()
    {
        csAndroidManager.USERID = username.text;
        NetworkManager.Instance.LoginAccount(username.text, password.text);
    }
    private void OnLogin()
    {
        csAndroidManager.USERID = username.text;
        if (rememberMe != null && rememberMe.isOn)
        {
            PlayerPrefs.SetString("username", username.text);
            PlayerPrefs.SetString("password", password.text);
        }
        else
        {
            PlayerPrefs.DeleteKey("username");
            PlayerPrefs.DeleteKey("password");
        }
        Debug.Log("로그인");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
    private void OnFailedToLogin()
    {
        messageBox.Show(new MessageOptions()
        {
            text = "로그인에 실패했습니다.\n고객센터에 문의해주세요."
        }, delegate (string result) { Show(); }, "OK");
        Close();
    }
    private void OnFailedToLogin1()
    {
        messageBox.Show(new MessageOptions()
        {
            text = "아이디 또는 비밀번호를 다시 확인해주세요."
        }, delegate (string result) { Show(); }, "OK");
        Close();
    }
    private void OnFailedToLogin2()
    {
        messageBox.Show(new MessageOptions()
        {
            text = "이미 접속중인 아이디 입니다."
        }, delegate (string result) { Show(); }, "OK");
        Close();
    }
    private void OnFailedToLogin3()
    {
        messageBox.Show(new MessageOptions()
        {
            text = "이전 게임에서 퇴장하여 자동치기가 진행중입니다.\n잠시후 다시 시도하세요."
        }, delegate (string result) { Show(); }, "OK");
        Close();
    }
    private void OnFailedToLogin4()
    {
        messageBox.Show(new MessageOptions()
        {
            text = "차단된 접속 IP 입니다.\n고객센터에 문의해주세요."
        }, delegate (string result) { Show(); }, "OK");
        Close();
    }
    private void OnFailedToLogin5()
    {
        messageBox.Show(new MessageOptions()
        {
            text = "차단된 회원입니다.\n고객센터에 문의해주세요."
        }, delegate (string result) { Show(); }, "OK");
        Close();
    }
    private void OnFailedServerConnect()
    {
        messageBox.Show(new MessageOptions()
        {
            text = "서버 점검 중입니다."
        }, delegate (string result) { Show(); });
        Close();
    }
}
