using UnityEngine;

namespace Bubblegum.Serialization
{

	/// <summary>
	/// Used to serialize unity colors
	/// </summary>
	[System.Serializable]
	public struct ColorSerializable
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// All of our color values to serialize
		/// </summary>
		public float r, g, b, a;

		#endregion // PUBLIC_VARIABLES

		#region CONSTRUCTORS

		/// <summary>
		/// Create a new color object
		/// </summary>
		public ColorSerializable (float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		/// <summary>
		/// Create a new serializable color with the one given
		/// </summary>
		/// <param name="color"></param>
		public ColorSerializable (Color color)
		{
			r = color.r;
			g = color.g;
			b = color.b;
			a = color.a;
		}

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Get the color value of this object
		/// </summary>
		/// <returns></returns>
		public Color ToColor ()
		{
			return new Color (r, g, b, a);
		} 

		#endregion // PUBLIC_METHODS
	}
}