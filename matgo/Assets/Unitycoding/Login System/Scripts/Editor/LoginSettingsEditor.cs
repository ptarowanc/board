using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Unitycoding.LoginSystem{
	[System.Serializable]
	public class LoginSettingsEditor : ICollectionEditor{
		[SerializeField]
		private LoginSettings settings;
		protected Editor editor;
		private Vector2 scrollPosition;
		
		public LoginSettingsEditor(LoginSettings database){
			settings=database;
		}
		
		public void OnGUI (Rect position)
		{
			GUILayout.BeginArea(position,"","ProgressBarBack");
			scrollPosition = GUILayout.BeginScrollView (scrollPosition);
			if (settings != null) {
				if (editor == null || editor.target != settings) {
					editor = Editor.CreateEditor (settings);
				}
				editor.OnInspectorGUI ();
			}
			GUILayout.EndScrollView ();
			GUILayout.EndArea ();
		}
	}
}