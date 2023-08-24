#if ICODE
using UnityEngine;
using System.Collections;
using Unitycoding.LoginSystem;

namespace ICode.Actions{
	[Category("Login System")]
	[Tooltip("Resets the password.")]
	[System.Serializable]
	public class ResetPassword : StateAction {
		[Tooltip("Username to use.")]
		public FsmString username;
		[Tooltip("Username to use.")]
		public FsmString password;

		public override void OnEnter ()
		{
			LoginSystem.ResetPassword (username.Value,password.Value);
			Finish ();
		}
	}
}
#endif