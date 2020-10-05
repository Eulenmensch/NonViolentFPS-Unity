using UnityEngine;

namespace Bubblegum.FauxPhysics.Colliders
{

	/// <summary>
	/// Universal collision events
	/// </summary>
	public class CollisionEvents : CacheBehaviour, IForceReceiver
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// If the collisions to monitor are 2D
		/// </summary>
		[SerializeField, Tooltip ("If the collisions to monitor are 2D")]
		private bool is2D;

		/// <summary>
		/// All of the events to trigger
		/// </summary>
		[SerializeField, Tooltip ("All of the events to trigger")]
		private CollisionEvent[] collisionEvents;

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The connected rigidbody
		/// </summary>
		private Rigidbody2D rBody2D;

		#endregion

		#region METHODS

		/// <summary>
		/// Awaken this instance
		/// </summary>
		void Awake ()
		{
			rBody2D = GetComponent<Rigidbody2D> ();
		}

		/// <summary>
		/// When a collision starts
		/// </summary>
		/// <param name="collision"></param>
		void OnCollisionEnter (Collision collision)
		{
			if (!is2D)
			{
				float force = collision.GetForce ();

				for (int i = 0; i < collisionEvents.Length; i++)
					collisionEvents[i].CollsionEnter (collision.gameObject, force);
			}
		}

		/// <summary>
		/// When a collision starts
		/// </summary>
		/// <param name="collision"></param>
		void OnCollisionEnter2D (Collision2D collision)
		{
			if (is2D)
			{
				Rigidbody2D otherRigidBody = collision.collider.GetComponent<Rigidbody2D> ();

				float force = collision.GetForce (rBody2D, otherRigidBody);

				for (int i = 0; i < collisionEvents.Length; i++)
					collisionEvents[i].CollsionEnter (collision.gameObject, force);
			}
		}

		/// <summary>
		/// Apply a custom force
		/// </summary>
		/// <param name="position"></param>
		/// <param name="force"></param>
		/// <param name="radius"></param>
		public void ApplyForce (Vector3 position, float force, float radius)
		{
			for (int i = 0; i < collisionEvents.Length; i++)
				collisionEvents[i].CollsionEnter (gameObject, force);
		}

		#endregion
	}
}