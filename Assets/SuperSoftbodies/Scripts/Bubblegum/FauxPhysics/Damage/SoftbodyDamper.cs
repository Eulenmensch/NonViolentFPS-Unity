using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Bubblegum.FauxPhysics.Damage
{

	/// <summary>
	/// Sends collision events with trigger information
	/// </summary>
	public class SoftbodyDamper : MonoBehaviour
	{
		#region PUBLIC_VARIABLES

		/// <summary>
		/// If we should debug this object
		/// </summary>
		[SerializeField, Tooltip ("If we should debug this object")]
		private bool debug;

		/// <summary>
		/// If we should collide with triggers
		/// </summary>
		[SerializeField, Tooltip ("If we should collide with triggers")]
		private bool collideWithTriggers;

		/// <summary>
		/// Overlap we don't care about
		/// </summary>
		[SerializeField, Tooltip ("Overlap we don't care about")]
		private float sleepOverlap = 0.1f;

		/// <summary>
		/// Speed we need to be above to warrant changes (overlap must also pass check)
		/// </summary>
		[SerializeField, Tooltip ("Speed we need to be above to warrant changes (overlap must also pass check)")]
		private float sleepVelocity = 0.5f;

		/// <summary>
		/// The collision layers we are interested in
		/// </summary>
		[SerializeField, Tooltip ("The collision layers we are interested in")]
		private LayerMask collisionLayers = -1;

		[Header ("Core")]

		/// <summary>
		/// The core collider to measure movement from
		/// </summary>
		[SerializeField, Tooltip ("The core collider to measure movement from")]
		private Collider core;

		/// <summary>
		/// The speed the core will change size
		/// </summary>
		[SerializeField, Tooltip ("The speed the core will change size"), Range (1f, 10f)]
		private float coreTension = 10f;

		/// <summary>
		/// How soft our collisions will be based on our speed (elasticity)
		/// </summary>
		[SerializeField, Tooltip ("How soft our collisions will be based on our speed (elasticity)")]
		private AnimationCurve coreElasticity = new AnimationCurve (new Keyframe (0f, 0.475f), new Keyframe (10f, 0.2f));

		/// <summary>
		/// The slow down we add to the rigidbody
		/// </summary>
		[SerializeField, Tooltip ("The slow down we add to the rigidbody")]
		private AnimationCurve dampingForce = new AnimationCurve (new Keyframe (0f, 0f), new Keyframe (1f, 100f));

		/// <summary>
		/// Object to take force events
		/// </summary>
		private Softbody softbody;

		/// <summary>
		/// The rigidbody component
		/// </summary>
		private Rigidbody rbody;

		/// <summary>
		/// The trigger material
		/// </summary>
		private Collider trigger;

		/// <summary>
		/// Core sphere collider
		/// </summary>
		private SphereCollider coreSphere;

		/// <summary>
		/// Core box collider
		/// </summary>
		private BoxCollider coreBox;

		/// <summary>
		/// All of the current collisions
		/// </summary>
		private List<Collider> collisions = new List<Collider> ();

		#endregion

		#region METHODS

		/// <summary>
		/// Start this object
		/// </summary>
		void Start ()
		{
			softbody = transform.GetComponentInParent<Softbody> ();
			rbody = transform.GetComponentInParent<Rigidbody> ();
			trigger = GetComponents<Collider> ().FirstOrDefault (collider => collider.isTrigger);

			softbody.ReactToCollisions = false;

			coreSphere = core as SphereCollider;
			coreBox = core as BoxCollider;
		}

		/// <summary>
		/// Update this object
		/// </summary>
		public void UpdateDamper ()
		{
			if (!rbody)
				return;

			//Core scaling
			float speed = rbody.velocity.magnitude;

			if (coreSphere)
				coreSphere.radius = Mathf.Lerp (coreSphere.radius, coreElasticity.Evaluate (speed), Time.deltaTime * coreTension);
			else if (coreBox)
				coreBox.size = Vector3.Lerp (coreBox.size, Vector3.one * coreElasticity.Evaluate (speed), Time.deltaTime * coreTension);

			//Collision data
			Vector3 direction;
			float distance;
			Collider other;

			//Apply softbody movement
			for (int i = 0; i < collisions.Count; i++)
			{
				other = collisions[i];

				if (Physics.ComputePenetration (trigger, transform.position, transform.rotation, other, other.transform.position, other.transform.rotation, out direction, out distance))
				{
					if (distance < sleepOverlap && speed < sleepVelocity)
						continue;

					softbody.ApplySoftForce (direction, other, distance);
					rbody.AddForce (direction * dampingForce.Evaluate (distance) * Time.deltaTime);
				}
			}
		}

		/// <summary>
		/// When we enter another object
		/// </summary>
		/// <param name="other"></param>
		public void OnTriggerEnter (Collider other)
		{
			if (collisionLayers.ContainsLayer (other.gameObject.layer) && (collideWithTriggers || !other.isTrigger) && softbody != null)
			{
				collisions.Add (other);

				float maxSpeed = coreElasticity.keys[coreElasticity.keys.Length - 1].time;
				softbody.IncrementPriority ((int)(rbody.velocity.magnitude / maxSpeed * SoftbodyJobManager.Instance.ExecutePriority));

				if (debug)
					print ("Collision started on " + name + " with " + name + " with priority " + softbody.Priority);
			}
		}

		/// <summary>
		/// When we exit a trigger
		/// </summary>
		/// <param name="collider"></param>
		/// <returns></returns>
		public void OnTriggerExit (Collider other)
		{
			if (collisionLayers.ContainsLayer (other.gameObject.layer) && (collideWithTriggers || !other.isTrigger) && softbody != null)
			{
				collisions.Remove (other);
				
				if (debug)
					print ("Collision ended on " + name + " with " + name);
			}
		}

		#endregion
	}
}