using UnityEngine;
using UnityEngine.Events;

namespace Bubblegum.Events

{

	/// <summary>
	/// View type class for making one object look at a position or object
	/// </summary>
	public class TimerEvent : MonoBehaviour
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// The default delay to use when invoking events
		/// </summary>
		[SerializeField, Tooltip ("The default delay to use when invoking events")]
		private float delay = 1f;

		/// <summary>
		/// If we should start the timer in the enable method
		/// </summary>
		[SerializeField, Tooltip ("If we should start the timer in the enable method")]
		private bool startOnEnable;

		/// <summary>
		/// Methods to invoke every time the timer completes
		/// </summary>
		[SerializeField, Tooltip ("Methods to invoke every time the timer completes")]
		private UnityEvent onComplete;

		#endregion // PUBLIC_VARIABLES

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Awaken this instance
		/// </summary>
		private void OnEnable ()
		{
			if (startOnEnable)
				InvokeAfter (delay);
		}

		#endregion // MONOBEHAVIOUR_METHODS

		#region PUBLIC_METHODS

		/// <summary>
		/// Start a timer with the selected amount of seconds
		/// </summary>
		/// <param name="time"></param>
		public void InvokeAfter (float seconds)
		{
			Invoke ("TriggerEvent", seconds);
		}

		#endregion // PUBLIC_METHODS

		#region PRIVATE_METHODS

		/// <summary>
		/// Trigger the events listeners
		/// </summary>
		private void TriggerEvent ()
		{
			onComplete.Invoke ();
		}

		#endregion
	}
}