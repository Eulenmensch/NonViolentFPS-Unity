using System;
using System.Collections.Generic;
using NonViolentFPS.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonViolentFPS.GameModes
{
	public abstract class GameMode : SerializedScriptableObject
	{
		[SerializeField] private SceneReference[] scenes;

		[ShowInInspector] protected int Score { get; private set; }

		private HashSet<SceneReference> loadedScenes = new HashSet<SceneReference>();

		public virtual void Load()
		{
			foreach (var scene in scenes)
			{
				SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
				loadedScenes.Add(scene);
			}
		}

		public void Unload()
		{
			foreach (var scene in loadedScenes)
			{
				SceneManager.UnloadSceneAsync(scene);
			}
		}

		public abstract void Evaluate();

		public virtual void ChangeScore(int _scoreChange)
		{
			Score += _scoreChange;
		}
	}
}