#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Bubblegum
{

	/// <summary>
	/// Read only inspector attribute
	/// </summary>
	public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR

	/// <summary>
	/// Drawer for the read only attribute
	/// </summary>
	[CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer
	{
		/// <summary>
		/// Get the property height
		/// </summary>
		/// <param name="property"></param>
		/// <param name="label"></param>
		/// <returns></returns>
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight (property, label, true);
		}

		/// <summary>
		/// Draw the field
		/// </summary>
		/// <param name="position"></param>
		/// <param name="property"></param>
		/// <param name="label"></param>
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			bool enabled = GUI.enabled;
			GUI.enabled = false;
			EditorGUI.PropertyField (position, property, label, true);
			GUI.enabled = enabled;
		}
	}

#endif
}
