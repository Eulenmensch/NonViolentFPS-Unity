using UnityEngine;

namespace Bubblegum.Serialization
{

	/// <summary>
	/// Stores two int parameters
	/// </summary>
	[System.Serializable]
	public struct Float4
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
		/// Gets or sets the z.
		/// </summary>
		/// <value>The z.</value>
		public float z;

		/// <summary>
		/// Gets or sets the w.
		/// </summary>
		/// <value>The w.</value>
		public float w;

		#endregion

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The type that the object was before converting to a float4
		/// </summary>
		[SerializeField, HideInInspector]
		private string storedType;

		#endregion

		#region CONSTRUCTORS

		/// <summary>
		/// Initializes a new instance of the <see cref="Float4"/> class.
		/// </summary>
		public Float4 (float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
			storedType = typeof (Vector4).AssemblyQualifiedName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Float4"/> class.
		/// </summary>
		public Float4 (Vector4 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
			storedType = typeof (Vector4).AssemblyQualifiedName;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Float4"/> class.
		/// </summary>
		public Float4 (Quaternion q)
		{
			x = q.x;
			y = q.y;
			z = q.z;
			w = q.w;
			storedType = typeof (Quaternion).AssemblyQualifiedName;

		}

		#endregion // CONSTRUCTORS

		#region PUBLIC_METHODS

		/// <summary>
		/// Return a quaternion with this objects values
		/// </summary>
		/// <returns></returns>
		public Vector4 ToVector4 ()
		{
			return new Vector4 (x, y, z, w);
		}

		/// <summary>
		/// Return a quaternion with this objects values
		/// </summary>
		/// <returns></returns>
		public Quaternion ToQuaternion ()
		{
			return new Quaternion (x, y, z, w);
		}

		/// <summary>
		/// Return an object of type that this Float4 was set as
		/// </summary>
		/// <returns></returns>
		public object ToSavedType ()
		{
			System.Type type = System.Type.GetType (storedType);

			if (type == typeof (Vector4))
				return ToVector4 ();
			else
				return ToQuaternion ();
		}

		#endregion
	}
}