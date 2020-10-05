using UnityEngine;

namespace Bubblegum.FauxPhysics
{
	/// <summary>
	/// An object that will react to forces
	/// </summary>
	public interface IForceReceiver
	{
		#region METHODS

		/// <summary>
		/// Apply the force to the object
		/// </summary>
		void ApplyForce (Vector3 position, float force, float radius);
		
		#endregion
	}
}