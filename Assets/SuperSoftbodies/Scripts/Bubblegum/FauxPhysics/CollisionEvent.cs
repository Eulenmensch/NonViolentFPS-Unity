using UnityEngine;

namespace Bubblegum.FauxPhysics.Colliders
{

	/// <summary>
	/// A unity event that also passes a float parameter
	/// </summary>
	[System.Serializable]
	public class UnityFloatEvent : UnityEngine.Events.UnityEvent<float> { }

	/// <summary>
	/// An event that occurs when a collider hits a certain layer
	/// </summary>
	[System.Serializable]
	public class CollisionEvent
	{

		/// <summary>
		/// If we should debug this type of collision
		/// </summary>
		[SerializeField, Tooltip ("If we should debug this type of collision")]
		private bool debug;

		/// <summary>
		/// The force that the collision must occur at to trigger the effect
		/// </summary>
		[SerializeField, Tooltip ("The force that the collision must occur at to trigger the effect")]
		private float forceThreshold = 10f;

		/// <summary>
		/// What layers the event will occur when colliding with
		/// </summary>
		[SerializeField, Tooltip ("What layers the event will occur when colliding with")]
		private LayerMask layermask;

		/// <summary>
		/// Listeners interested in collision info
		/// </summary>
		[SerializeField, Tooltip ("Listeners interested in collision info")]
		private UnityFloatEvent OnCollide;

		/// <summary>
		/// When the collision is occuring
		/// </summary>
		/// <param name="collision"></param>
		/// <param name="rBody"></param>
		public void CollsionEnter (GameObject other, float force)
		{
			if (debug)
				Debug.Log ("Collision with " + other.name + " resulted in a force of " + force);

			if (layermask.ContainsLayer (other.layer) && force > forceThreshold)
				OnCollide.Invoke (force);
		}

	}
}