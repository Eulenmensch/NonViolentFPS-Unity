using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bubblegum.SceneUtility
{
	/// <summary>
	/// Used for testing and showcase
	/// </summary>
	public class SceneQuickload : MonoBehaviour
	{
		#region METHODS
		/// <summary>
		/// The number keyodes
		/// </summary>
		private KeyCode[] numberKeys =
		{
			 KeyCode.Alpha0,
			 KeyCode.Alpha1,
			 KeyCode.Alpha2,
			 KeyCode.Alpha3,
			 KeyCode.Alpha4,
			 KeyCode.Alpha5,
			 KeyCode.Alpha6,
			 KeyCode.Alpha7,
			 KeyCode.Alpha8,
			 KeyCode.Alpha9
		};

		#endregion

		#region METHODS

		/// <summary>
		/// Update this object
		/// </summary>
		private void Update ()
		{
			for (int i = 0; i < SceneManager.sceneCountInBuildSettings && i < numberKeys.Length; i++)
				if (Input.GetKeyDown (numberKeys[i]))
					SceneManager.LoadScene (i);
		}

		#endregion
	}
}