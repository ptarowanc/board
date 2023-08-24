using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using Unitycoding.UIWidgets;

namespace Unitycoding.LoginSystem{
	/// <summary>
	/// Registration window.
	/// </summary>
	public class UIRegistration : UIWidget {
		[Header("Reference")]
		[SerializeField]
		private InputField username;
		[SerializeField]
		private InputField password;
		[SerializeField]
		private InputField confirmPassword;
		[SerializeField]
		private InputField email;
		[SerializeField]
		private Toggle acceptTermsOfUse;
		[SerializeField]
		private Button register;

		private UILogin loginWindow;
		private MessageBox messageBox;

		protected override void OnStart ()
		{
			base.OnStart ();
			if (LoginSystem.Settings == null) {
				return;
			}

			if (username == null ||
			    password == null || 
			    confirmPassword == null ||
			    acceptTermsOfUse == null || 
			    register == null ||
			    email == null) {
				LoginSystem.logger.LogError("[UIRegistration] Please assign all fields in the inspector.");
				return;
			}

			messageBox = UIUtility.Find<MessageBox> (LoginSystem.Settings.messageBoxWindow);
			
			if (messageBox == null) {
				LoginSystem.logger.LogError("[UIRegistration] No message box found with name " + LoginSystem.Settings.messageBoxWindow+"!");
				return;
			}

			loginWindow = UIUtility.Find<UILogin> (LoginSystem.Settings.loginWindow);
			if (loginWindow == null) {
				LoginSystem.logger.LogError("[UIRegistration] No login window found with name " + LoginSystem.Settings.loginWindow+"!");
				return;
			}

			EventHandler.Register("OnAccountCreated", OnAccountCreated);
			EventHandler.Register ("OnFailedToCreateAccount", OnFailedToCreateAccount);
		}

		/// <summary>
		/// Show this widget.
		/// </summary>
		public override void Show ()
		{
			base.Show ();
			register.onClick.AddListener (CreateAccountUsingFields);
		}

		/// <summary>
		/// Close this widget.
		/// </summary>
		public override void Close ()
		{
			base.Close ();
			register.onClick.RemoveListener (CreateAccountUsingFields);
		}

		/// <summary>
		/// Creates the account using data from referenced fields.
		/// </summary>
		public void CreateAccountUsingFields(){
			if (string.IsNullOrEmpty (email.text) || 
			    string.IsNullOrEmpty (password.text) ||
			    string.IsNullOrEmpty (confirmPassword.text) ||
			    string.IsNullOrEmpty (email.text)) {
				messageBox.Show (LoginSystem.Settings.emptyField, delegate(string result){Show(); },"OK");
				Close();
				return;
			}
			
			if (password.text != confirmPassword.text) {
				messageBox.Show (LoginSystem.Settings.passwordMatch, delegate(string result){Show(); },"OK");
				Close();
				return;
			}
			
			if (!LoginSystem.ValidateEmail (email.text)) {
				messageBox.Show (LoginSystem.Settings.invalidEmail, delegate(string result){Show(); },"OK");
				Close();
				return;
			}
			
			if (!acceptTermsOfUse.isOn) {
				messageBox.Show (LoginSystem.Settings.termsOfUse, delegate(string result){Show(); },"OK");
				Close();
				return;
			}
			LoginSystem.CreateAccount (username.text, password.text, email.text);
		}

		private void OnAccountCreated(){
			messageBox.Show (LoginSystem.Settings.accountCreated, delegate(string result){loginWindow.Show(); },"OK");
			Close ();
		}

		private void OnFailedToCreateAccount(){
			messageBox.Show (LoginSystem.Settings.userExists, delegate(string result){Show(); },"OK");
			Close ();
		}
	}
}