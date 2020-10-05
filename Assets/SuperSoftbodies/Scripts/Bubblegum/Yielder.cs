using System.Collections.Generic;
using UnityEngine;

namespace Bubblegum
{
	/// <summary>
	/// Contains a variety of yielding objects
	/// </summary>
	public static class Yielder
	{
		#region VARIABLES

		/// <summary>
		/// Get the wait for end of frame yielder
		/// </summary>
		public static WaitForEndOfFrame EndOfFrame { get { return endOfFrame; } }

		/// <summary>
		/// Get the fixed update yielder
		/// </summary>
		public static WaitForFixedUpdate FixedUpdate { get { return fixedUpdate; } }

		/// <summary>
		/// Get the null yielder
		/// </summary>
		public static YieldInstruction NullYield { get { return nullYield; } }

		/// <summary>
		/// All timed yields
		/// </summary>
		private static Dictionary<float, WaitForSeconds> timeYields = new Dictionary<float, WaitForSeconds> (100);

		/// <summary>
		/// End of frame yield
		/// </summary>
		private static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame ();

		/// <summary>
		/// Fixed update yielder
		/// </summary>
		private static WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate ();

		/// <summary>
		/// Null yield
		/// </summary>
		private static YieldInstruction nullYield = null;

		#endregion

		#region METHODS

		/// <summary>
		/// Get a yielder with the given wait for seconds
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		public static WaitForSeconds Get (float seconds)
		{
			if (!timeYields.ContainsKey (seconds))
				timeYields.Add (seconds, new WaitForSeconds (seconds));

			return timeYields[seconds];
		}

		#endregion
	}
}