using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Unitycoding.LoginSystem
{
	public class LoginSystem : MonoBehaviour
	{

		private static LoginSystem m_Current;

		/// <summary>
		/// The LoginSystem singleton object. This object is set inside Awake()
		/// </summary>
		public static LoginSystem current {
			get {
				if (m_Current == null) {
					LoginSystem.logger.LogError ("Requires a LoginSystem. Create one from Tools > Unitycoding > Login System > Create > Login System!");
				}
				return m_Current;
			}
		}

		private static DebugFilter m_Logger;

		/// <summary>
		/// Gets the debug logger helper.
		/// </summary>
		/// <value>The logger.</value>
		public static DebugFilter logger {
			get {
				if (LoginSystem.m_Current != null && LoginSystem.m_Current.m_Settings != null) {
					m_Logger = new DebugFilter (LoginSystem.Settings.logLevel, "[Login System]");
				} else {
					m_Logger = new DebugFilter (DebugFilter.FilterLevel.Error, "[Login System]");
				}
				return m_Logger;
			}
		}

		[SerializeField]
		private LoginSettings m_Settings;

		/// <summary>
		/// Gets the settings LoginSystem settings. Configurate it inside the editor.
		/// </summary>
		/// <value>The settings.</value>
		public static LoginSettings Settings {
			get {
				if (LoginSystem.current != null && LoginSystem.current.m_Settings != null) {
					return LoginSystem.current.m_Settings;
				}
				LoginSystem.logger.LogError ("Please assign LoginSettings to the Login System!");
				return null;
			}
		}

		/// <summary>
		/// Don't destroy this object instance when loading new scenes.
		/// </summary>
		public bool dontDestroyOnLoad;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		private void Awake ()
		{
			if (LoginSystem.m_Current != null) {
				LoginSystem.logger.LogInfo ("Multiple LoginSystems in scene...this is not supported. Destroying instance!");
				Destroy (gameObject);
				return;
			} else {
				LoginSystem.m_Current = this;
				if (dontDestroyOnLoad) {
					DontDestroyOnLoad (gameObject);
				}
				LoginSystem.logger.LogInfo ("LoginSystem instance set.");
			}
		}

		/// <summary>
		/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		/// </summary>
		private void Start ()
		{
			if (Settings != null && Settings.loadLevelOnLogin) {
				if (string.IsNullOrEmpty (Settings.levelToLoad)) {
					LoginSystem.logger.LogWarning ("Load level on login is checked, but the level to load is empty. Please enter a level name in the editor.");
					return;
				}
				if (Settings.skipLogin) {
					#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
					Application.LoadLevel(Settings.levelToLoad);
					#else
					UnityEngine.SceneManagement.SceneManager.LoadScene (Settings.levelToLoad);
					#endif
				}
			}
		}

		/// <summary>
		/// Creates the account.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="email">Email.</param>
		public static void CreateAccount (string username, string password, string email)
		{
			if (LoginSystem.current != null) {
				LoginSystem.current.StartCoroutine (CreateAccountInternal (username, password, email));
			} 
		}

		private static IEnumerator CreateAccountInternal (string username, string password, string email)
		{
			if (LoginSystem.Settings == null) {
				EventHandler.Execute ("OnFailedToCreateAccount");
				yield break;		
			}

			LoginSystem.logger.LogInfo ("[CreateAccount]: Trying to register a new account with username: " + username + " and password: " + password + "!");

			WWWForm newForm = new WWWForm ();
			newForm.AddField ("name", username);
			newForm.AddField ("password", password);
			newForm.AddField ("email", email);
			
			WWW w = new WWW (LoginSystem.Settings.serverAddress + "/" + LoginSystem.Settings.createAccount, newForm);
			
			while (!w.isDone) {
				yield return new WaitForEndOfFrame ();
			}
			
			if (w.error != null) {
				Debug.LogError (w.error);
			}
			
			bool res = w.text.Trim ().Equals ("true");
			if (res) {
				LoginSystem.logger.LogInfo ("[CreateAccount] Account creation was successfull!");
				EventHandler.Execute ("OnAccountCreated");
			} else {
				LoginSystem.logger.LogInfo ("[CreateAccount] Failed to create account. Result: " + w.text);
				EventHandler.Execute ("OnFailedToCreateAccount");
			}
		}

		/// <summary>
		/// Logins the account.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public static void LoginAccount (string username, string password)
		{
			if (LoginSystem.current != null) {
				LoginSystem.current.StartCoroutine (LoginAccountInternal (username, password));
			} 
		}

		private static IEnumerator LoginAccountInternal (string username, string password)
		{
			if (LoginSystem.Settings == null) {
				EventHandler.Execute ("OnFailedToLogin");
				yield break;		
			}

			LoginSystem.logger.LogInfo ("[LoginAccount] Trying to login using username: " + username + " and password: " + password + "!");

			WWWForm newForm = new WWWForm ();
			newForm.AddField ("name", username);
			newForm.AddField ("password", password);
			
			WWW w = new WWW (LoginSystem.Settings.serverAddress + "/" + LoginSystem.Settings.login, newForm);
			
			while (!w.isDone) {
				yield return new WaitForEndOfFrame ();
			}
			
			if (w.error != null) {
				Debug.LogError (w.error);
			}
			
			bool res = w.text.Trim ().Equals ("true");
			if (res) {
				PlayerPrefs.SetString (LoginSystem.Settings.accountKey, username);
				LoginSystem.logger.LogInfo ("[LoginAccount] Login was successfull!");
				EventHandler.Execute ("OnLogin");
			} else {
				LoginSystem.logger.LogInfo ("[LoginAccount] Failed to login. Result: " + w.text);
				EventHandler.Execute ("OnFailedToLogin");
			}
		}

		/// <summary>
		/// Recovers the password.
		/// </summary>
		/// <param name="email">Email.</param>
		public static void RecoverPassword (string email)
		{
			if (LoginSystem.current != null) {
				LoginSystem.current.StartCoroutine (RecoverPasswordInternal (email));
			} 
		}

		private static IEnumerator RecoverPasswordInternal (string email)
		{
			if (LoginSystem.Settings == null) {
				EventHandler.Execute ("OnFailedToRecoverPassword");
				yield break;		
			}

			LoginSystem.logger.LogInfo ("[RecoverPassword] Trying to recover password using email: " + email + "!");

			WWWForm newForm = new WWWForm ();
			newForm.AddField ("email", email);
			
			WWW w = new WWW (LoginSystem.Settings.serverAddress + "/" + LoginSystem.Settings.recoverPassword, newForm);
			
			while (!w.isDone) {
				yield return new WaitForEndOfFrame ();
			}
			
			if (w.error != null) {
				Debug.LogError (w.error);
			}
			
			bool res = w.text.Trim ().Equals ("true");
			if (res) {
				EventHandler.Execute ("OnPasswordRecovered");
				LoginSystem.logger.LogInfo ("[RecoverPassword] Password recovered successfull!");
			} else {
				LoginSystem.logger.LogInfo ("[RecoverPassword] Failed to recover password. Result: " + w.text);
				EventHandler.Execute ("OnFailedToRecoverPassword");
			}
		}

		/// <summary>
		/// Resets the password.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public static void ResetPassword (string username, string password)
		{
			if (LoginSystem.current != null) {
				LoginSystem.current.StartCoroutine (ResetPasswordInternal (username, password));
			} 
		}

		private static IEnumerator ResetPasswordInternal (string username, string password)
		{
			if (LoginSystem.Settings == null) {
				EventHandler.Execute ("OnFailedToResetPassword");
				yield break;		
			}

			LoginSystem.logger.LogInfo ("[ResetPassword] Trying to reset password using username: " + username + " and password: " + password + "!");

			WWWForm newForm = new WWWForm ();
			newForm.AddField ("name", username);
			newForm.AddField ("password", password);
			
			WWW w = new WWW (LoginSystem.Settings.serverAddress + "/" + LoginSystem.Settings.resetPassword, newForm);
			
			while (!w.isDone) {
				yield return new WaitForEndOfFrame ();
			}
			
			if (w.error != null) {
				Debug.LogError (w.error);
			}
			
			bool res = w.text.Trim ().Equals ("true");
			if (res) {
				EventHandler.Execute ("OnPasswordResetted");
			} else {
				LoginSystem.logger.LogInfo ("Failed to reset password. Result: " + w.text);
				EventHandler.Execute ("OnFailedToResetPassword");
			}
		}

		/// <summary>
		/// Validates the email.
		/// </summary>
		/// <returns><c>true</c>, if email was validated, <c>false</c> otherwise.</returns>
		/// <param name="email">Email.</param>
		public static bool ValidateEmail (string email)
		{
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			System.Text.RegularExpressions.Match match = regex.Match (email);
			if (match.Success) {
				LoginSystem.logger.LogInfo ("Email validated was successfull for email: " + email + "!");
			} else {
				LoginSystem.logger.LogInfo ("Email validation failed for email: " + email + "!");
			}

			return match.Success;
		}
	}
}