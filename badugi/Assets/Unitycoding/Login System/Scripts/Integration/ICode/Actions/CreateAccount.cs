#if ICODE
using UnityEngine;
using System.Collections;
using Unitycoding.LoginSystem;

namespace ICode.Actions{
	[Category("Login System")]
	[Tooltip("Creates a new account.")]
	[System.Serializable]
	public class CreateAccount : StateAction {
		[Tooltip("Username to use.")]
		public FsmString username;
		[Tooltip("Password to use.")]
		public FsmString password;
		[Tooltip("Email to use.")]
		public FsmString email;

		public override void OnEnter ()
		{
			LoginSystem.CreateAccount (username.Value, password.Value, email.Value);
			Finish ();
		}
	}
}
#endif