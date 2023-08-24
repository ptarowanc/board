#if ICODE
using UnityEngine;
using System.Collections;
using Unitycoding.LoginSystem;

namespace ICode.Actions{
	[Category("Login System")]
	[Tooltip("Login with an account")]
	[System.Serializable]
	public class LoginAccount : StateAction {
		[Tooltip("Username to use.")]
		public FsmString username;
		[Tooltip("Password to use.")]
		public FsmString password;
		
		public override void OnEnter ()
		{
			LoginSystem.LoginAccount (username.Value, password.Value);
			Finish ();
		}
	}
}
#endif