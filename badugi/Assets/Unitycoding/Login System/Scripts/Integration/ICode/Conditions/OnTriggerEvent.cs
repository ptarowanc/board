#if ICODE
using UnityEngine;
using System.Collections;
using Unitycoding.LoginSystem;

namespace ICode.Conditions.LoginSystem{
	[Category("Login System")]  
	[Tooltip("Login System messages that are sended.")]
	[System.Serializable]
	public class OnTriggerEvent : Condition {

		public EventType type;

		private bool isTrigger;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			Unitycoding.EventHandler.Register(type.ToString (), OnTrigger);
		}
		
		public override void OnExit ()
		{
		
			isTrigger = false;
			Unitycoding.EventHandler.Unregister(type.ToString (), OnTrigger);
		}
		
		private void OnTrigger(){
			isTrigger = true;
		}
		
		public override bool Validate ()
		{
			if (isTrigger) {
				isTrigger=false;	
				return true;
			}
			return isTrigger;
		}

		public enum EventType{
			OnLogin,
			OnFailedToLogin,
			OnAccountCreated,
			OnFailedToCreateAccount,
			OnPasswordRecovered,
			OnFailedToRecoverPassword,
			OnPassworedResetted,
			OnFailedToResetPassword
		}
	}
}
#endif