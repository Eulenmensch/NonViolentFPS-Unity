using UnityEngine;
using UnityEngine.Events;

namespace Bubblegum.FauxPhysics.Triggers
{

	/// <summary>
	/// A unity event that also passes a collider parameter
	/// </summary>
	[System.Serializable]
	public class UnityComponentEvent : UnityEvent<Component> { }

	/// <summary>
	/// Triggers events when an object either enters the trigger or collides with this object
	/// </summary>
	public class TriggerStripped : TriggerBase
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// Methods to invoke when the trigger is entered
		/// </summary>
		[Tooltip ("Methods to invoke when the trigger is entered")]
		[SerializeField]
		public UnityComponentEvent OnTriggerEntered;

		/// <summary>
		/// Methods to invoke when the trigger is exited
		/// </summary>
		[Tooltip ("Methods to invoke when the trigger is exited")]
		[SerializeField]
		public UnityComponentEvent OnTriggerExited;

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_METHODS

		/// <summary>
		/// Collider that we are using as the trigger bounds
		/// </summary>
		protected Component trigger;

		#endregion

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Awaken this component
		/// </summary>
		void Awake ()
		{
			trigger = GetTrigger ();
		}

		/// <summary>
		/// Raises the trigger enter event.
		/// </summary>
		/// <param name="collider">Collider.</param>
		protected void OnTriggerEnter (Collider collider)
		{
			if (!is2D)
				OnEnter (collider);
		}

		/// <summary>
		/// Raises the trigger exit event
		/// </summary>
		/// <param name="collider"></param>
		protected void OnTriggerExit (Collider collider)
		{
			if (!is2D)
				OnExit (collider);
		}

		/// <summary>
		/// Raises the trigger enter event.
		/// </summary>
		/// <param name="collider">Collider.</param>
		protected void OnTriggerEnter2D (Collider2D collider)
		{
			if (is2D)
				OnEnter (collider);
		}

		/// <summary>
		/// Raises the trigger exit event
		/// </summary>
		/// <param name="collider"></param>
		protected void OnTriggerExit2D (Collider2D collider)
		{
			if (is2D)
				OnExit (collider);
		}

		#endregion // MONOBEHAVIOUR_METHODS

		#region PUBLIC_METHODS

		/// <summary>
		/// Send the trigger event
		/// </summary>
		public void InvokeEnter ()
		{
			OnTriggerEntered.Invoke (null);
		}

		/// <summary>
		/// Send the trigger event
		/// </summary>
		public void InvokeExit ()
		{
			OnTriggerExited.Invoke (null);
		}

		#endregion

		#region PROTECTED_METHODS

		/// <summary>
		/// Check filters then invoke event
		/// </summary>
		/// <param name="collider"></param>
		/// <returns></returns>
		protected virtual bool OnEnter (Component collider)
		{
			if (debug)
				print ("Trigger entered on " + name + " by " + collider.name + " and filtering = " + CheckFilters (collider.gameObject));

			if (!CheckFilters (collider.gameObject))
				return false;

			OnTriggerEntered.Invoke (collider);

			return true;
		}

		/// <summary>
		/// Check filters then invoke event
		/// </summary>
		/// <param name="collider"></param>
		/// <returns></returns>
		protected virtual bool OnExit (Component collider)
		{
			if (debug)
				print ("Trigger exited on " + name + " by " + collider.name + " and filtering = " + CheckFilters (collider.gameObject));

			if (!CheckFilters (collider.gameObject))
				return false;

			OnTriggerExited.Invoke (collider);

			return true;
		}

		/// <summary>
		/// If we are inside the given collider
		/// </summary>
		/// <param name="collider"></param>
		/// <returns></returns>
		protected bool IsInside (Component collider)
		{
			if (is2D)
				return ((Collider2D)collider).bounds.Intersects (((Collider2D)trigger).bounds);
			else
				return ((Collider)collider).bounds.Intersects (((Collider)trigger).bounds); ;
		}

		#endregion

	}
}