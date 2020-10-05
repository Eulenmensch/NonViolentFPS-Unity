namespace Bubblegum
{

	/// <summary>
	/// An object that will target other objects
	/// </summary>
	public interface ITargetable
	{

		#region PUBLIC_METHODS

		/// <summary>
		/// Target we want to look at
		/// </summary>
		Target LookTarget { get; }

		/// <summary>
		/// Target we want to move to
		/// </summary>
		Target MoveTarget { get; }

		#endregion

	}
}
