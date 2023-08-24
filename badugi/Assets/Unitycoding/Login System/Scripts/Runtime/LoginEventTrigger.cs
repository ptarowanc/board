using UnityEngine;
using System.Collections;

namespace Unitycoding.LoginSystem{
	public class LoginEventTrigger : CallbackHandler {
		public override string[] Callbacks {
			get {
				return new string[]{
					"OnLogin",
					"OnFailedToLogin",
					"OnAccountCreated",
					"OnFailedToCreateAccount",
					"OnPasswordRecovered",
					"OnFailedToRecoverPassword",
					"OnPassworedResetted",
					"OnFailedToResetPassword"
				};
			}
		}

		private void Awake(){
			for (int i=0; i< Callbacks.Length; i++) {
				string callback=Callbacks[i];
				EventHandler.Register (callback, delegate(){
					TriggerEvent(callback);
				});
			}
		}

		private void TriggerEvent(string eventName){
			Execute (eventName, new CallbackEventData ());
		}
	}
}