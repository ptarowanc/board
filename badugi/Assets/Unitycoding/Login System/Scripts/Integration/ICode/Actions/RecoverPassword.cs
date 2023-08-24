#if ICODE
using UnityEngine;
using System.Collections;
using Unitycoding.LoginSystem;

namespace ICode.Actions{
	[Category("Login System")]
	[Tooltip("Recover password. New password will be sended to this email.")]
	[System.Serializable]
	public class RecoverPassword : StateAction {
		[Tooltip("Email to use.")]
		public FsmString email;
		
		public override void OnEnter ()
		{
			LoginSystem.RecoverPassword (email.Value);
			Finish ();
		}
	}
}
#endif