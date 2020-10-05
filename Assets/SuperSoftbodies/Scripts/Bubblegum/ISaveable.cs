namespace Bubblegum
{ 
	/// <summary>
	/// An object whos state can be saved into a key
	/// </summary>
	public interface ISaveable
	{
		#region METHODS

		/// <summary>
		/// Save our state into the key
		/// </summary>
		/// <param name="id"></param>
		void Save (Key id);

		/// <summary>
		/// Load our state using the key
		/// </summary>
		/// <param name="id"></param>
		void Load (Key id);

		/// <summary>
		/// Clear state from the key
		/// </summary>
		/// <param name="id"></param>
		void Clear (Key id);

		#endregion
	}
}