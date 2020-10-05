using UnityEngine;
using System.Linq;

namespace Bubblegum.FauxPhysics.Triggers
{

	/// <summary>
	/// Base functionality for both 2D and 3D triggers
	/// </summary>
	public abstract class TriggerBase : CacheBehaviour
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// If this trigger is 2D or 3D
		/// </summary>
		[SerializeField, Tooltip ("If this trigger is 2D or 3D")]
		protected bool is2D;

		/// <summary>
		/// The tag filter to use for the trigger
		/// </summary>
		[SerializeField, Tooltip ("The tag filter to use for the trigger")]
		protected string tagFilter;

		/// <summary>
		/// The layers that we want to check
		/// </summary>
		[SerializeField, Tooltip ("The layers that we want to check")]
		protected LayerMask layerFilter;

		#endregion // PUBLIC_VARIABLES

		#region PROTECTED_METHODS

		/// <summary>
		/// Checkt to see if the tag matches the tag filter
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		protected bool CheckFilters (GameObject collider)
		{
			if (debug)
				print ("Hit with collider " + collider.name + " and filtering = " + IsFiltered (collider));

			return IsFiltered (collider);
		}

		/// <summary>
		/// Get the first trigger component on this object
		/// </summary>
		/// <returns></returns>
		protected Component GetTrigger ()
		{
			Component trigger = null;

			if (is2D)
			{
				Collider2D[] colliders = GetComponents<Collider2D> ();
				trigger = colliders.FirstOrDefault (collider => collider.isTrigger);
			}
			else
			{
				Collider[] colliders = GetComponents<Collider> ();
				trigger = colliders.FirstOrDefault (collider => collider.isTrigger);
			}

			if (!trigger)
				Debug.LogError (name + " has no collider with isTrigger set to true");

			return trigger;
		}

		/// <summary>
		/// Check if we should filter
		/// </summary>
		/// <param name="collider"></param>
		/// <returns></returns>
		private bool IsFiltered (GameObject collider)
		{
			return layerFilter.ContainsLayer (collider.layer) && (string.IsNullOrEmpty (tagFilter) || string.Equals (collider.tag, tagFilter));
		}

		#endregion
	}
}