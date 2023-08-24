using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;

namespace Unitycoding.LoginSystem{
	[System.Serializable]
	/// <summary>
	/// Login settings.
	/// </summary>
	public class LoginSettings : ScriptableObject {
		[HeaderLine("Default")]
		public DebugFilter.FilterLevel logLevel = DebugFilter.FilterLevel.Info;
		public bool loadLevelOnLogin=true;
		public string levelToLoad="SciFi Login Successful";
		public bool skipLogin = false;

		[HeaderLine("UI")]
		public string loginWindow="Login";
		public string registrationWindow="Registration";
		public string recoverPasswordWindow="RecoverPassword";
		public string messageBoxWindow="MessageBox";

		/// <summary>
		/// Settings to use when login fails.
		/// </summary>
		public MessageOptions loginFailed= new MessageOptions(){
			text = "Username or password wrong! You may need to activate your account."
		};
		/// <summary>
		/// Settings to use when the user not completed all required fields.
		/// </summary>
		public MessageOptions emptyField= new MessageOptions(){
			text = "You need to complete all fields!"
		};
		/// <summary>
		/// Settings to use in UIRegistration when the confirmation password does not match.
		/// </summary>
		public MessageOptions passwordMatch= new MessageOptions(){
			text = "Password does not match confirm password!"
		};
		/// <summary>
		/// Settings to use if the email is not valid.
		/// </summary>
		public MessageOptions invalidEmail= new MessageOptions(){
			text = "Please enter a valid email!"
		};
		/// <summary>
		/// Settings to use if the user does not accept terms of use.
		/// </summary>
		public MessageOptions termsOfUse = new MessageOptions(){
			text = "You need to accept the terms of use!"
		};
		/// <summary>
		/// Settings to use in UIRegistration if the user already exists with the same account/user name.
		/// </summary>
		public MessageOptions userExists= new MessageOptions(){
			text = "Username already exists!"
		};
		/// <summary>
		/// Settings to display when the account was created.
		/// </summary>
		public MessageOptions accountCreated= new MessageOptions(){
			text = "Your account was created and an activation link was sent to your email!"
		};
		/// <summary>
		/// Settings to use in UIRecoverPassword if recover failed.
		/// </summary>
		public MessageOptions accountNotFound= new MessageOptions(){
			text = "No corresponding account found!"
		};
		/// <summary>
		/// Settings to display when the password was recovered.
		/// </summary>
		public MessageOptions passwordRecovered= new MessageOptions(){
			text = "Your password has beed send to your email address. You may login now."
		};

		[HeaderLine("Saving")]
		public string serverAddress = "http://unitycoding.com/Unity/LoginSystem/PHP/";
		public string createAccount = "createAccount.php";
		public string login = "login.php";
		public string recoverPassword = "recoverPassword.php";
		public string resetPassword = "resetPassword.php";
		public string accountKey = "Account";
	}
}