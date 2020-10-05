using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum.Utility
{

	/// <summary>
	/// Contains various methods for destroying objects, automatically pools an object if a poolable component is attached
	/// </summary>
	public class Destroy : MonoBehaviour
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// If we should destroy the object after seconds
		/// </summary>
		[Tooltip ("If we should destroy the object after seconds")]
		[SerializeField]
		private bool destroyAfterSeconds;

		/// <summary>
		/// The seconds until destroying the object
		/// </summary>
		[Tooltip ("The seconds until destroying the object")]
		[SerializeField]
		private float seconds = 2f;

		/// <summary>
		/// If we should destroy a component rather than the entire object
		/// </summary>
		[Tooltip ("If we should destroy a component rather than the entire object")]
		[SerializeField]
		private bool destroyComponent;

		/// <summary>
		/// The components to destroy, if null we will destroy this object
		/// </summary>
		[Tooltip ("The components to destroy, if null we will destroy this object")]
		[SerializeField]
		private MonoBehaviour[] components;

		/// <summary>
		/// The effect to show when destroying this object
		/// </summary>
		[Tooltip ("The effect to show when destroying this object")]
		[SerializeField]
		protected GameObject destroyEffect;

		/// <summary>
		/// The offset for the position of the destroy effect
		/// </summary>
		[Tooltip ("The offset for the position of the destroy effect")]
		[SerializeField]
		protected Vector3 destroyEffectOffset;

		/// <summary>
		/// Methods to invoke when this object is destroyed
		/// </summary>
		[Tooltip ("Methods to invoke when this object is destroyed")]
		[SerializeField]
		private UnityEvent onDestroy;

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// If we are currently destroying the object
		/// </summary>
		private bool isDestroying;

		#endregion // PRIVATE_VARIABLES

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Called when all objects have been initialized regardless of whether the script is enabled
		/// </summary>
		void OnEnable ()
		{
			OnSpawn ();
		}

		/// <summary>
		/// When we spawn this object
		/// </summary>
		public void OnSpawn ()
		{
			if (destroyAfterSeconds)
				TriggerDestroyAfterSeconds (seconds);

			isDestroying = false;
		}

		#endregion // MONOBEHAVIOUR_METHODS

		#region PUBLIC_METHODS

		/// <summary>
		/// Destroy the selected gameobject/component
		/// </summary>
		public void TriggerDestroy ()
		{
			TriggerDestroy (true);
		}

		/// <summary>
		/// Destroy the object without playing an effect
		/// </summary>
		public void TriggerDestroyWithoutEffect ()
		{
			TriggerDestroy (false);
		}

		/// <summary>
		/// Destroy all child game objects but not this one
		/// </summary>
		public void TriggerDestroyChildren (bool playEffect)
		{
			//Destroy immediate children
			foreach (Transform t in transform)
			{
				Destroy destroy = t.GetComponent<Destroy> ();

				if (destroy)
					destroy.TriggerDestroy (playEffect);
				else
					Destroy (t.gameObject);
			}
		}

		/// <summary>
		/// Destroy the selected gameobject/component
		/// </summary>
		public void TriggerDestroyAfterSeconds (float seconds)
		{
			Invoke ("TriggerDestroy", seconds);
		}

		#endregion // PUBLIC_METHODS

		#region PRIVATE_METHODS

		/// <summary>
		/// Trigger the destruction of the object
		/// </summary>
		/// <param name="playEffect"></param>
		private void TriggerDestroy (bool playEffect)
		{
			if (isDestroying)
				return;

			//Only set destroying if destroying whole object
			isDestroying = !destroyComponent;

			//Destroy
			if (destroyComponent)
				for (int i = 0; i < components.Length; i++)
					Destroy (components[i]);

			else
				Destroy (gameObject);

			//Show effect
			if (playEffect && destroyEffect)
					Instantiate (destroyEffect, transform.position + destroyEffectOffset, destroyEffect.transform.rotation);

			onDestroy.Invoke ();
		}

		#endregion

		#region SUB_CLASSES

#if UNITY_EDITOR
		[CustomEditor (typeof (Destroy)), CanEditMultipleObjects]
		public class DestroyEditor : Bubblegum.EditorBase
		{

			//Declaring properties here means we can have multi object editing
			SerializedProperty destroyAfterSeconds;
			SerializedProperty seconds;
			SerializedProperty destroyComponent;
			SerializedProperty components;
			SerializedProperty destroyEffect;
			SerializedProperty destroyEffectOffset;
			SerializedProperty OnDestroy;

			/// <summary>
			/// Raises the enable event.
			/// </summary>
			private void OnEnable ()
			{
				destroyAfterSeconds = serializedObject.FindProperty ("destroyAfterSeconds");
				seconds = serializedObject.FindProperty ("seconds");
				destroyComponent = serializedObject.FindProperty ("destroyComponent");
				components = serializedObject.FindProperty ("components");
				destroyEffect = serializedObject.FindProperty ("destroyEffect");
				destroyEffectOffset = serializedObject.FindProperty ("destroyEffectOffset");
				OnDestroy = serializedObject.FindProperty ("onDestroy");
			}

			/// <summary>
			/// Raises the inspector GUI event.
			/// </summary>
			public override void OnInspectorGUI ()
			{
				serializedObject.Update ();

				DrawDefaultScriptField ();

				//Show the destroy after seconds tools?
				EditorGUILayout.PropertyField (destroyAfterSeconds);

				if (destroyAfterSeconds.boolValue)
					EditorGUILayout.PropertyField (seconds);

				//Show the destroy component tools?
				EditorGUILayout.PropertyField (destroyComponent);

				if (destroyComponent.boolValue)
					EditorGUILayout.PropertyField (components);

				//Should we show the destroy effect tools
				EditorGUILayout.PropertyField (destroyEffect);

				if (destroyEffect.objectReferenceValue != null)
					EditorGUILayout.PropertyField (destroyEffectOffset);

				EditorGUILayout.PropertyField (OnDestroy);

				serializedObject.ApplyModifiedProperties ();
			}
		}
#endif

		#endregion // SUS_CLASSES
	}
}