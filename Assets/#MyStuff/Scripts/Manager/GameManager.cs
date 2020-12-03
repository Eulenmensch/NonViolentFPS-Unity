using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonViolentFPS.Manager
{
	public class GameManager : MonoBehaviour
	{
		#region Singleton
		public static GameManager Instance { get; private set; }

		private void Awake()
		{
			if ( Instance != null && Instance != this )
			{
				Destroy( this );
			}
			else
			{
				Instance = this;
			}
		}
		#endregion

		public GameObject Player { get; set; }

		//FIXME: Only for testing
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				ReloadAllScenes();
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}

		private async void ReloadAllScenes()
		{
			var sceneNames = new List<string>();
			var sceneCount = SceneManager.sceneCount;
			for (int i = 0; i < sceneCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);
				sceneNames.Add(scene.name);
			}

			foreach (var sceneName in sceneNames)
			{
				var unloading = SceneManager.UnloadSceneAsync(sceneName);
				while(!unloading.isDone){ await Task.Yield(); }

				var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				while(!loading.isDone){ await Task.Yield(); }
			}

		}
	}
}
