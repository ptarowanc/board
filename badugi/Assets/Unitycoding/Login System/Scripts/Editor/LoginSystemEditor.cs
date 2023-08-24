using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Unitycoding.LoginSystem{
	public class LoginSystemEditor : BaseSystemEditor<LoginSettings> {
		protected override void AddChildEditor (List<ICollectionEditor> editors){
			editors.Add(new LoginSettingsEditor(base.database));
		}
	}
}
