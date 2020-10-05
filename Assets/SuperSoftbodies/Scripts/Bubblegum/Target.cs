using UnityEngine;

namespace Bubblegum
{

	/// <summary>
	/// Stores information about a target transform that is of interest
	/// </summary>
	[System.Serializable]
	public struct Target : ITargetable
	{

		#region PUBLIC_VARIABLES

		/// <summary>
		/// Get the target point
		/// </summary>
		public Target MoveTarget
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Get the target point
		/// </summary>
		public Target LookTarget
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Gets or sets the target to move to
		/// </summary>
		/// <value>The target.</value>
		public Vector3 Position
		{
			get
			{
				if (_target)
					return _target.position;
				else
					return _position;
			}
		}

		/// <summary>
		/// Gets the target object
		/// </summary>
		/// <value>The target.</value>
		public Transform Transform { get { return _target; } }

		/// <summary>
		/// Return a forward vector of the target
		/// </summary>
		public Vector3 Forward
		{
			get
			{
				if (Transform)
					return Transform.forward;
				else
					return Position;
			}
		}

		/// <summary>
		/// Get a new empty target
		/// </summary>
		public static Target Empty
		{
			get
			{
				return new Target ();
			}
		}

		/// <summary>
		/// Check if the target is empty
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return !_notEmpty || _isTransform && !_target;
			}
		}

		#endregion // PUBLIC_VARIABLES

		#region PRIVATE_VARIABLES

		/// <summary>
		/// The target position.
		/// </summary>
		private Vector3 _position;

		/// <summary>
		/// The target.
		/// </summary>
		private Transform _target;

		/// <summary>
		/// If this target is empty
		/// </summary>
		private bool _notEmpty;

		/// <summary>
		/// If the target is a transform
		/// </summary>
		private bool _isTransform;

		#endregion // PRIVATE_VARIABLES

		#region CONSTRUCTORS

		/// <summary>
		/// Initializes a new instance of the <see cref="Bubblegum.TapTown.AIMovement+MovementTarget"/> class.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="dynamicTarget">If set to <c>true</c> dynamic target.</param>
		public Target (Transform target)
		{
			_isTransform = true;
			_notEmpty = true;
			_target = target;
			_position = target.position;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bubblegum.Behaviour.AIMovement+MovementTarget"/> class.
		/// </summary>
		/// <param name="targetPosition">Target position.</param>
		/// <param name="dynamicTarget">If set to <c>true</c> dynamic target.</param>
		public Target (Vector3 targetPosition)
		{
			_isTransform = false;
			_notEmpty = true;
			_target = null;
			_position = targetPosition;
		}

		#endregion // CONSTRUCTORS

		#region OPERATORS

		/// <summary>
		/// Opertator overload for ==
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator == (Target a, Target b)
		{
			if (System.Object.ReferenceEquals (a, b))
				return true;

			else if (a.IsEmpty && b.IsEmpty)
				return true;

			else if (a.Transform || b.Transform)
				return a.Transform == b.Transform;

			else
				return a.Position == b.Position;
		}

		/// <summary>
		/// Operator overload for !=
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator != (Target a, Target b)
		{
			return !(a == b);
		}

		#endregion

		#region PUBLIC_METHODS

		/// <summary>
		/// Check if two objects are the same
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals (object obj)
		{
			if (obj is Target)
				return ((Target) obj) == this;

			return false;
		}

		/// <summary>
		/// Get the hashcode for the object
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode ()
		{
			if (Transform)
				return Transform.GetHashCode ();
			else
				return Position.GetHashCode ();
		}

		/// <summary>
		/// Set the specified target, dynamicTarget and navigationUpdatePeriod.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="dynamicTarget">If set to <c>true</c> dynamic target.</param>
		/// <param name="navigationUpdatePeriod">Navigation update period.</param>
		public void Set (Transform target)
		{
			_target = target;
			_position = target.position;
		}

		/// <summary>
		/// Set the specified targetPosition, dynamicTarget and navigationUpdatePeriod.
		/// </summary>
		/// <param name="targetPosition">Target position.</param>
		/// <param name="dynamicTarget">If set to <c>true</c> dynamic target.</param>
		/// <param name="navigationUpdatePeriod">Navigation update period.</param>
		public void Set (Vector3 targetPosition)
		{
			_target = null;
			_position = targetPosition;
		}

		/// <summary>
		/// The desired velocity to reach the target
		/// </summary>
		/// <param name="transform">Transform.</param>
		public Vector3 DesiredVelocity (Transform transform)
		{
			return Position - transform.position;
		}

		#endregion // PUBLIC_METHODS
	}
}