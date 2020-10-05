

namespace Bubblegum.Timing
{

	/// <summary>
	/// Object that will handle timing
	/// </summary>
	public interface ITimer 
	{		
		#region PUBLIC_VARIABLES

		/// <summary>
		/// The value of the timer
		/// </summary>
		float TimeValue { get; }

		#endregion // PUBLIC_VARIABLES

		#region PUBLIC_METHODS

		/// <summary>
		/// Start the timer
		/// </summary>
		void StartTimer ();

		/// <summary>
		/// Pause the timer
		/// </summary>
		void PauseTimer ();

		/// <summary>
		/// Stop the timer
		/// </summary>
		void StopTimer ();
		
		#endregion // PUBLIC_METHODS
	}
}