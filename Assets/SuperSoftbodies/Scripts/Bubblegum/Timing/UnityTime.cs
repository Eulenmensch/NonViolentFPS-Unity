using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubblegum.Timing
{

	/// <summary>
	/// Basic time object with Unity time
	/// </summary>
	[CreateAssetMenu (menuName = "Scriptable Object/Timing/Unity Time")]
	public class UnityTime : ScriptableObject, ITimeKeeper
	{
		#region PUBLIC_VARIABLES

		/// <summary>
		/// Get the time
		/// </summary>
		public float Time
		{
			get
			{
				return UnityEngine.Time.time;
			}
		}

		/// <summary>
		/// Set the time scale
		/// </summary>
		public float TimeScale
		{
			set
			{
				UnityEngine.Time.timeScale = value;
				UnityEngine.Time.fixedDeltaTime = value * defaultDeltaTime;
			}
		}

		/// <summary>
		/// Speed to change timescale
		/// </summary>
		[SerializeField, Tooltip ("Speed to change timescale")]
		private float scaleChangeDelay = 0.5f;

		#endregion

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The default delta time scale
		/// </summary>
		private static float defaultDeltaTime;

		/// <summary>
		/// The current timescale routine
		/// </summary>
		private Coroutine timescaleRoutine;

		/// <summary>
		/// All of the actions
		/// </summary>
		private Dictionary<Action, System.Threading.Timer> actions = new Dictionary<Action, System.Threading.Timer> ();

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Initialize this object
		/// </summary>
		[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Initialize ()
		{
			defaultDeltaTime = UnityEngine.Time.fixedDeltaTime;
		}

		/// <summary>
		/// Animate to the given timescale
		/// </summary>
		/// <param name="timescale"></param>
		public void AnimateTimeScale (float timescale)
		{
			if (timescaleRoutine != null)
				ApplicationManager.Instance.StopCoroutine (timescaleRoutine);

			timescaleRoutine = ApplicationManager.Instance.StartCoroutine (AnimateTime (timescale));
		}

		/// <summary>
		/// Invoke the action at the given time
		/// </summary>
		/// <param name="action"></param>
		/// <param name="time"></param>
		public void Invoke (Action action, float time)
		{
			System.Threading.Timer timer = null;
			timer = new System.Threading.Timer ((obj) =>
			{
				Dispatcher.Instance.InvokeAsync (action);
				actions.Remove (action);
				timer.Dispose ();
			},
			null, (int) (time * 1000f), System.Threading.Timeout.Infinite);
			actions[action] = timer;
		}

		/// <summary>
		/// Cancel the invoke for the given action
		/// </summary>
		/// <param name="action"></param>
		public void CancelInvoke (Action action)
		{
			if (actions.ContainsKey (action))
			{
				actions[action].Dispose ();
				actions.Remove (action);
			}
		}

		/// <summary>
		/// Animate to the given timescale
		/// </summary>
		/// <param name="newScale"></param>
		/// <returns></returns>
		IEnumerator AnimateTime (float newScale)
		{
			float fromScale = UnityEngine.Time.timeScale;
			float time = 0f;

			while (time < scaleChangeDelay)
			{
				time += UnityEngine.Time.deltaTime;
				TimeScale = Mathf.Lerp (fromScale, newScale, time / scaleChangeDelay);

				yield return Yielder.NullYield;
			}
		}

		#endregion
	}
}
