using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Unitycoding.LoginSystem{
	public static class LoginSystemMenu {
		private const string componentToolbarMenu="Tools/Unitycoding/Login System/Components/";

		[MenuItem("Tools/Unitycoding/Login System/Editor",false, -1)]
		private static void OpenEditor(){
			LoginSystemEditor.ShowWindow ();
		}

		[MenuItem("Tools/Unitycoding/Login System/Create/Login System",false, 0)]
		private static void CreateLoginSystem(){
			GameObject go = new GameObject ("LoginSystem");
			go.AddComponent<LoginSystem> ();
			Selection.activeGameObject = go;
		}
		
		[MenuItem ("Tools/Unitycoding/Login System/Create/Login System", true)]
		private static bool ValidateCreateLoginSystem() {
			return GameObject.FindObjectOfType<LoginSystem> () == null;
		}

		[MenuItem("Tools/Unitycoding/Login System/Documentation",false, 2)]
		private static void OpenDocumentation(){
			Application.OpenURL ("http://unitycoding.com/login-system/");
		}

		[MenuItem (LoginSystemMenu.componentToolbarMenu+ "LoginEventTrigger")]
		static void AddLoginEventTrigger ()
		{
			Selection.activeGameObject.AddComponent<LoginEventTrigger> ();
		}


		[MenuItem (LoginSystemMenu.componentToolbarMenu+ "UILogin")]
		static void AddUILoginHandler ()
		{
			Selection.activeGameObject.AddComponent<UILogin> ();
		}
		
		[MenuItem (LoginSystemMenu.componentToolbarMenu+ "UILogin", true)]
		static bool ValidateAddUILoginHandler() {
			return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<UILogin>()== null;
		}

		[MenuItem (LoginSystemMenu.componentToolbarMenu+ "UIRegistration")]
		static void AddUIRegistrationHandler ()
		{
			Selection.activeGameObject.AddComponent<UIRegistration> ();
		}
		
		[MenuItem (LoginSystemMenu.componentToolbarMenu+ "UIRegistration", true)]
		static bool ValidateAddUIRegistrationHandler() {
			return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<UIRegistration>()== null;
		}

		[MenuItem (LoginSystemMenu.componentToolbarMenu+ "UIRecoverPassword")]
		static void AddUIRecoverPasswordHandler ()
		{
			Selection.activeGameObject.AddComponent<UIRecoverPassword> ();
		}
		
		[MenuItem (LoginSystemMenu.componentToolbarMenu+ "UIRecoverPassword", true)]
		static bool ValidateAddUIRecoverPasswordHandler() {
			return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<UIRecoverPassword>()== null;
		}

	}
}