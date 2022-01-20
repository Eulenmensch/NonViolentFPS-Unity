using NonViolentFPS.Scripts.VerletRope;
using UnityEngine;

namespace NonViolentFPS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class Balloon : MonoBehaviour
	{
		[SerializeField] private float upForce;
		[SerializeField] private Transform origin;
		[SerializeField] private VerletRope rope;

		[SerializeField] private float proportional, integral, derivative;

		private Rigidbody rigidbodyRef;
		private PIDController pidController;

		private void Awake()
		{
			rigidbodyRef = GetComponent<Rigidbody>();
			pidController = new PIDController(proportional, integral, derivative);
		}

		private void FixedUpdate()
		{
			pidController.Kp = proportional;
			pidController.Ki = integral;
			pidController.Kd = derivative;

			Vector3 restingPosition = origin.position + Vector3.up * rope.length;
			var pidControlValue = pidController.Control(restingPosition, transform.position);
			rigidbodyRef.AddForce(pidControlValue * upForce, ForceMode.Acceleration);
		}
	}
}