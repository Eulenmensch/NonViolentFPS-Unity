using UnityEngine;

namespace Bubblegum.Serialization
{

	/// <summary>
	/// Stores two int parameters
	/// </summary>
	[System.Serializable]
	public struct Float3
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

		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		/// <value>The y.</value>
		public float z;

		#endregion // PUBLIC_VARIABLES

		#region CONSTRUCTORS

		/// <summary>
		/// Initializes a new instance of the <see cref="Float3"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Float3 (float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Float3"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Float3 (Vector3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}

		#endregion // CONSTRUCTORS

		#region PUBLIC METHODS

		/// <summary>
		/// Return a new vector 3 from this object
		/// </summary>
		/// <returns></returns>
		public Vector3 ToVector3 ()
		{
			return new Vector3 (x, y, z);
		}

		#endregion
	}
}