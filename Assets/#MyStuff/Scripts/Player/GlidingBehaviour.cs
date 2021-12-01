using System;
using CMF;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Player
{
	//TODO: if this logic ever changes, check and consider the Gliding section of
	//TODO: DetermineControllerState() in the AdvancedWalkerController!
	public class GlidingBehaviour : MonoBehaviour
	{
		[SerializeField] private float glidingGravity;

		private AdvancedWalkerController controller;
		private float defaultGravity;
		private bool glideInputActive;

		private void OnEnable()
		{
			controller.OnLand += ResetGravity;
		}

		private void OnDisable()
		{
			controller.OnLand -= ResetGravity;
		}

		private void Awake()
		{
			controller = GetComponent<AdvancedWalkerController>();
			defaultGravity = controller.gravity;
		}

		private void Update()
		{
			if (glideInputActive)
			{
				if (controller.currentControllerState == AdvancedWalkerController.ControllerState.Falling && !controller.isInCoyoteTime)
				{
					if (controller.gravity == glidingGravity) return;
					controller.gravity = glidingGravity;
				}
			}
		}

		public void Glide(InputAction.CallbackContext _context)
		{
			if (_context.started)
			{
				glideInputActive = true;
			}

			if (_context.canceled)
			{
				glideInputActive = false;
				controller.gravity = defaultGravity;
			}
		}

		private void ResetGravity(Vector3 _vector3)
		{
			controller.gravity = defaultGravity;
		}
	}
}