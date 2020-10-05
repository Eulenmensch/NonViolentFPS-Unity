using UnityEngine;

namespace Bubblegum.Serialization
{

	/// <summary>
	/// Stores two int parameters
	/// </summary>
	[System.Serializable]
	public struct Float2
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// Gets or sets the x.
		/// </summary>
		/// <value>The x.</value>
		public float x;

		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		/// <value>The y.</value>
		public float y;

		#endregion // PUBLIC_VARIABLES

		#region CONSTRUCTORS

		/// <summary>
		/// Initializes a new instance of the <see cref="Float3"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Float2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Float3"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Float2 (Vector3 v)
		{
			x = v.x;
			y = v.y;
		}

		#endregion // CONSTRUCTORS

		#region PUBLIC METHODS

		/// <summary>
		/// Return a new vector 3 from this object
		/// </summary>
		/// <returns></returns>
		public Vector2 ToVector2 ()
		{
			return new Vector2 (x, y);
		}

		#endregion
	}
}