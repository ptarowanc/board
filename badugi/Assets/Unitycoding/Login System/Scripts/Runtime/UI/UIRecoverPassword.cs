using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace Unitycoding.LoginSystem{
	/// <summary>
	/// Recover password window.
	/// </summary>
	public class UIRecoverPassword : UIWidget {
		[Header("Reference")]
		[SerializeField]
		private InputField email;
		[SerializeField]
		private Button recover;

		private MessageBox messageBox;
		private UILogin loginWindow;

		protected override void OnStart ()
		{
			base.OnStart ();
			if (LoginSystem.Settings == null) {
				return;
			}

			if (email == null || recover == null) {
				LoginSystem.logger.LogError("[UIRecoverPassword] Please assign all fields in the inspector.");
				return;
			}

			messageBox = UIUtility.Find<MessageBox> (LoginSystem.Settings.messageBoxWindow);
			
			if (messageBox == null) {
				LoginSystem.logger.LogError("[UIRecoverPassword] No message box found with name " + LoginSystem.Settings.messageBoxWindow+"!");
				return;
			}

			loginWindow = UIUtility.Find<UILogin> (LoginSystem.Settings.loginWindow);

			if (loginWindow == null) {
				LoginSystem.logger.LogError("[UIRecoverPassword] No login window found with name " + LoginSystem.Settings.loginWindow+"!");
				return;
			}

			EventHandler.Register ("OnPasswordRecovered", OnPasswordRecovered);
			EventHandler.Register ("OnFailedToRecoverPassword", OnFailedToRecoverPassword);

			recover.onClick.AddListener (RecoverPasswordUsingFields);
		}

		public override void Show ()
		{
			base.Show ();
			recover.onClick.AddListener (RecoverPasswordUsingFields);
		}

		public override void Close ()
		{
			base.Close ();
			recover.onClick.RemoveListener (RecoverPasswordUsingFields);
		}

		/// <summary>
		/// Recovers the password using data from referenced ui fields
		/// </summary>
		public void RecoverPasswordUsingFields(){
			if (!LoginSystem.ValidateEmail (email.text)) {
				messageBox.Show(LoginSystem.Settings.invalidEmail, delegate(string result){Show(); },"OK");
				Close();
				return;
			}
			LoginSystem.RecoverPassword (email.text);
		}

		private void OnPasswordRecovered(){
			messageBox.Show(LoginSystem.Settings.passwordRecovered, delegate(string result){loginWindow.Show(); },"OK");
			Close ();
		}

		private void OnFailedToRecoverPassword(){
			messageBox.Show(LoginSystem.Settings.accountNotFound, delegate(string result){Show(); },"OK");
			Close();
		}
	}
}