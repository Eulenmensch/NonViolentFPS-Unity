using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

namespace Bubblegum.SceneUtility
{

	/// <summary>
	/// Used to control the current scene, since it is a scriptable object it can be accessed from anywhere
	/// </summary>
	[CreateAssetMenu (menuName = "Scriptable Object/Scene/Scene Controls")]
	public class SceneControls : ScriptableObject
	{
		#region PUBLIC_VARIABLES

		/// <summary>
		/// If the last scene load was a restart
		/// </summary>
		public static bool LastSceneRestarted { get; private set; }

		/// <summary>
		/// If we should debug this object
		/// </summary>
		[SerializeField, Tooltip ("If we should debug this object")]
		private bool debug;

		#endregion

		#region EVENTS/DELEGATES

		/// <summary>
		/// When a new scene starts
		/// </summary>
		public static StateChanged onSceneStart;

		/// <summary>
		/// When a scene restarts
		/// </summary>
		public static StateChanged onSceneRestart;

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Load the given scene
		/// </summary>
		public void LoadScene (string name)
		{
#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene (name);
#else
			Application.LoadLevel (name);
#endif

			SceneStart ();
		}

		/// <summary>
		/// Load the next scene from the build settings
		/// </summary>
		public void LoadNextScene ()
		{
#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
#else
			Application.LoadLevel (Application.loadedLevel + 1);
#endif

			SceneStart ();
		}

		/// <summary>
		/// Load the next scene from the build settings
		/// </summary>
		public void LoadNextSceneAdditive ()
		{
#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1, LoadSceneMode.Additive);
#else
			Application.LoadLevelAdditive (Application.loadedLevel + 1);
#endif
		}

		/// <summary>
		/// Load the previous scene
		/// </summary>
		public void LoadPreviousScene ()
		{
#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
#else
			Application.LoadLevel (Application.loadedLevel - 1);
#endif

			SceneStart ();
		}

		/// <summary>
		/// Load the given scene
		/// </summary>
		public void LoadSceneAdditive (string name)
		{
#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene (name, LoadSceneMode.Additive);
#else
			Application.LoadLevelAdditive (name);
#endif
		}

		/// <summary>
		/// Restart the current scene
		/// </summary>
		public void RestartScene ()
		{
#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
#else
			Application.LoadLevel (Application.loadedLevel);
#endif

			SceneRestart ();
		}

		/// <summary>
		/// Remove the scene with the given name
		/// </summary>
		public void RemoveScene (string name)
		{
			SceneManager.UnloadSceneAsync (name);
		}

		#endregion // PUBLIC_METHODS

		#region PRIVATE_METHODS

		/// <summary>
		/// Scene start
		/// </summary>
		/// <returns></returns>
		private void SceneStart ()
		{
			LastSceneRestarted = false;

			if (onSceneStart != null)
				onSceneStart ();
		}

		/// <summary>
		/// Scene restart
		/// </summary>
		/// <returns></returns>
		private void SceneRestart ()
		{
			LastSceneRestarted = true;

			if (onSceneRestart != null)
				onSceneRestart ();
		}

        #endregion
    }
}