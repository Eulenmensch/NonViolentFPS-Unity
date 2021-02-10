using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NonViolentFPS.Events;
using NonViolentFPS.GameModes;
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

		[SerializeField] private GameMode startGameMode;
		[SerializeField] private SceneReference winScreen;
		[SerializeField] private SceneReference looseScreen;

		private GameMode currentGameMode;

		private void OnEnable()
		{
			GameEvents.Instance.OnGameWon += SetGameWon;
			GameEvents.Instance.OnGameLost += SetGameLost;
			GameEvents.Instance.OnGameRestarted += RestartCurrentGamemode;
			GameEvents.Instance.OnScoreChanged += ChangeCurrentGamemodeScore;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnGameWon -= SetGameWon;
			GameEvents.Instance.OnGameLost -= SetGameLost;
			GameEvents.Instance.OnGameRestarted -= RestartCurrentGamemode;
			GameEvents.Instance.OnScoreChanged -= ChangeCurrentGamemodeScore;
		}

		private void Start()
		{
			currentGameMode = startGameMode;
			currentGameMode.Load();
		}

		//FIXME: Only for testing
		private void Update()
		{
			currentGameMode.Evaluate();

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

		public void RestartCurrentGamemode()
		{
			currentGameMode.Unload();
			currentGameMode.Load();
		}

		private void ChangeCurrentGamemodeScore(int _scoreChange)
		{
			currentGameMode.ChangeScore(_scoreChange);
		}

		public void LoadNewGameMode(GameMode _gameMode)
		{
			currentGameMode.Unload();
			_gameMode.Load();
		}

		public void SetGameLost()
		{
			Time.timeScale = 0;
			SceneManager.LoadSceneAsync(looseScreen, LoadSceneMode.Additive);
		}

		public void SetGameWon()
		{
			Time.timeScale = 0;
			SceneManager.LoadSceneAsync(winScreen, LoadSceneMode.Additive);
		}
	}
}
