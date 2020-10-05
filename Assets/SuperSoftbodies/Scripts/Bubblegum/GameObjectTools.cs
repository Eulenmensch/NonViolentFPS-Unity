using UnityEngine;

namespace Bubblegum 
{

	/// <summary>
	/// Common functions for GameObjects that can be called from scripts
	/// </summary>
	[CreateAssetMenu (menuName = "Scriptable Object/GameObject Tools")]
	public class GameObjectTools : ScriptableObject 
	{
		
		#region PUBLIC_METHODS
		
		/// <summary>
		/// Set the given gameobject to not be destroyed between scenes
		/// </summary>
		public void SetAsDontDestroyOnLoad (GameObject target)
		{
			DontDestroyOnLoad (target);
		}

		/// <summary>
		/// Destroy the given gameobject
		/// </summary>
		/// <param name="target"></param>
		public void DestroyObject (GameObject target)
		{
			Destroy (target);
		}

		/// <summary>
		/// Destroy the given component
		/// </summary>
		/// <param name="component"></param>
		public void DestroyComponent (Component component)
		{
			if (component)
				Destroy (component);
		}

		/// <summary>
		/// Destroy the root of the given game object
		/// </summary>
		/// <param name="component"></param>
		public void DestroyRootGameObject (Component component)
		{
			if (component)
				Destroy (component.transform.root.gameObject);
		}
		
		/// <summary>
		/// Disable the root gameobject
		/// </summary>
		/// <param name="component"></param>
		public void DisableRoot (Component component)
		{
			component.transform.root.gameObject.SetActive (false);
		}

		/// <summary>
		/// Toggle the given gameobject state
		/// </summary>
		/// <param name="target"></param>
		public void ToggleActive (GameObject target)
		{
			target.SetActive (!target.activeSelf);
		}

		#endregion // PUBLIC_METHODS
	}
}