using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonViolentFPS.GameModes
{
	public abstract class GameMode : SerializedScriptableObject
	{
		[SerializeField] private SceneReference[] scenes;

		public int Score { get; private set; }

		protected virtual void Initialize()
		{
			foreach (var scene in scenes)
			{
				SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
			}
		}

		protected abstract void Evaluate();

		protected virtual void ChangeScore(int _scoreChange)
		{
			Score += _scoreChange;
		}
	}
}