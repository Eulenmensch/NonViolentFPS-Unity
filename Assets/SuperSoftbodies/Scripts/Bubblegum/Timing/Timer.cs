using System;
using UnityEngine;

namespace Bubblegum.Timing
{

	/// <summary>
	/// Global timer with start/stop functions
	/// </summary>
	[CreateAssetMenu (menuName = "Scriptable Object/Timing/Timer")]
	public class Timer : ScriptableObject, ITimer
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// The value of the timer
		/// </summary>
		public float TimeValue
		{
			get
			{
				if (timing)
					return Time.time - startTime;
				else
					return stopTime - startTime;
			}
		}

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The time that the timer started
		/// </summary>
		private float startTime, stopTime;

		/// <summary>
		/// If we are currently timing
		/// </summary>
		private bool timing;

		#endregion // PRIVATE_VARIABLES

		#region PUBLIC_METHODS

		/// <summary>
		/// Start the timer
		/// </summary>
		public void StartTimer ()
		{
			timing = true;
			startTime = Time.time;
			stopTime = 0f;
		}

		/// <summary>
		/// Pause the timer
		/// </summary>
		public void PauseTimer ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Stop the timer
		/// </summary>
		public void StopTimer ()
		{
			timing = false;
			stopTime = Time.time;
		}

		/// <summary>
		/// Add the given time to the timer
		/// </summary>
		/// <param name="amount"></param>
		public void PunchTimer (float amount)
		{
			startTime -= amount;
		}

		/// <summary>
		/// Reset the timer
		/// </summary>
		public void ResetTimer ()
		{
			startTime = Time.time;
		}

		#endregion // PUBLIC_METHODS
	}
}