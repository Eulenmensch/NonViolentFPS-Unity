using System;
using UnityEngine;

namespace NonViolentFPS.Events
{
	public class GameEvents : MonoBehaviour
	{
		#region Singleton
		public static GameEvents Instance { get; private set; }

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

		public event Action OnGameWon;
		public void GameWon(){OnGameWon?.Invoke();}

		public event Action OnGameLost;
		public void GameLost(){OnGameLost?.Invoke();}

		public event Action OnGameRestarted;
		public void RestartGame(){OnGameRestarted?.Invoke();}

		public event Action<int> OnScoreChanged;
		public void ChangeScore(int _scoreChange){OnScoreChanged?.Invoke(_scoreChange);}
	}
}