using System;

namespace Bubblegum.Timing
{

	/// <summary>
	/// An object that keeps time for the application
	/// </summary>
	public interface ITimeKeeper
	{
		#region METHODS

		/// <summary>
		/// The current time in seconds
		/// </summary>
		float Time { get; }

		/// <summary>
		/// Invoke the given event at the given time
		/// </summary>
		/// <param name="action"></param>
		/// <param name="time"></param>
		void Invoke (Action action, float time);

		/// <summary>
		/// Cancel the given action
		/// </summary>
		void CancelInvoke (Action action);

		#endregion
	}
}