using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using Unitycoding.UIWidgets;

namespace Unitycoding.LoginSystem
{
    /// <summary>
    /// Login window
    /// </summary>
    public class UILogin : UIWidget
    {
        [Header("Reference")]
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        private InputField username;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        private InputField password;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        private Toggle rememberMe;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        private Button login;

        private MessageBox messageBox;

        protected override void OnStart()
        {
            base.OnStart();
            if (LoginSystem.Settings == null)
            {
                return;
            }

            if (username == null || password == null || login == null)
            {
                LoginSystem.logger.LogError("[UILogin] Please assign all fields in the inspector.(Required: Username, Password, Login)");
                return;
            }

            messageBox = UIUtility.Find<MessageBox>(LoginSystem.Settings.messageBoxWindow);

            if (messageBox == null)
            {
                LoginSystem.logger.LogError("[UILogin] No message box found with name " + LoginSystem.Settings.messageBoxWindow + "!");
                return;
            }

            username.text = PlayerPrefs.GetString("username", string.Empty);
            password.text = PlayerPrefs.GetString("password", string.Empty);

            if (rememberMe != null)
            {
                rememberMe.isOn = string.IsNullOrEmpty(username.text) ? false : true;
            }

            EventHandler.Register("OnLogin", OnLogin);
            EventHandler.Register("OnFailedToLogin", OnFailedToLogin);

            login.onClick.AddListener(LoginUsingFields);
        }

        /// <summary>
        /// Logins using data from referenced fields.
        /// </summary>
        public void LoginUsingFields()
        {
            csAndroidManager.USERID = username.text;
            LoginSystem.LoginAccount(username.text, password.text);
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

            if (LoginSystem.Settings.loadLevelOnLogin)
            {
#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				Application.LoadLevel(LoginSystem.Settings.levelToLoad);
#else
                UnityEngine.SceneManagement.SceneManager.LoadScene(LoginSystem.Settings.levelToLoad);
#endif
            }
        }

        private void OnFailedToLogin()
        {
            messageBox.Show(LoginSystem.Settings.loginFailed, delegate (string result) { Show(); }, "OK");
            Close();
        }
    }
}