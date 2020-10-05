using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Bubblegum {

	/// <summary>
	/// Extension for the default unity editor class
	/// </summary>
	public class EditorBase : UnityEditor.Editor {

		#region PROTECTED_METHODS

		/// <summary>
		/// Draws the default script field.
		/// </summary>
		protected void DrawDefaultScriptField() {
			serializedObject.Update();
			SerializedProperty prop = serializedObject.FindProperty("m_Script");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Draw a dropdown for the selected object that lets the user select a component
		/// </summary>
		/// <param name="gameobject"></param>
		/// <param name="currentIndex"></param>
		/// <returns></returns>
		protected int DrawDropdown (string[] objectNames, int currentIndex, bool includeEmpty)
		{
			if (includeEmpty)
				objectNames = new string[] { "None" }.Concat (objectNames);

			return EditorGUILayout.Popup (currentIndex, objectNames, EditorStyles.popup, GUILayout.Width (EditorGUIUtility.currentViewWidth / 2.5f));

		}

		#endregion // PROTECTED_METHODS
	}
}

#endif