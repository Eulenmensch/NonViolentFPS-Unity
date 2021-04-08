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

		[SerializeField] private GameMode startGameMode;
		[SerializeField] private SceneReference winScreen;
		[SerializeField] private SceneReference loseScreen;

		public GameMetaState MetaState { get; private set; }
		public GameObject Player { get; set; }
		public GameMode CurrentGameMode { get; private set; }

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
			MetaState = GameMetaState.Playing;
			CurrentGameMode = startGameMode;
			if(CurrentGameMode != null)
			{
				CurrentGameMode.Load();
			}
		}

		private void Update()
		{
			if(CurrentGameMode != null)
			{
				CurrentGameMode.Evaluate();
			}

			#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.R))
			{
				RestartCurrentGamemode();
			}
			#endif

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}

		private void RestartCurrentGamemode()
		{
			CurrentGameMode.Unload();
			CurrentGameMode.Load();
			MetaState = GameMetaState.Playing;
		}

		private void ChangeCurrentGamemodeScore(int _scoreChange)
		{
			CurrentGameMode.ChangeScore(_scoreChange);
		}

		public void LoadNewGameMode(GameMode _gameMode)
		{
			CurrentGameMode.Unload();
			_gameMode.Load();
			MetaState = GameMetaState.Playing;
		}

		private void SetGameLost()
		{
			MetaState = GameMetaState.Lost;
			Time.timeScale = 0;
			SceneManager.LoadSceneAsync(loseScreen, LoadSceneMode.Additive);
		}

		private void SetGameWon()
		{
			MetaState = GameMetaState.Won;
			Time.timeScale = 0;
			SceneManager.LoadSceneAsync(winScreen, LoadSceneMode.Additive);
		}
	}
}
