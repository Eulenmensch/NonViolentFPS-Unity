using System;
using CMF;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using NonViolentFPS.Manager;

namespace NonViolentFPS.Player
{
	//TODO: if this logic ever changes, check and consider the Gliding section of
	//TODO: DetermineControllerState() in the AdvancedWalkerController!
	public class GlidingBehaviour : MonoBehaviour
	{
		[SerializeField] private float glidingYMomentum;
		[Tooltip("Should be negative in most cases. " +
		         "This is the initial falling momentum when initiating a glide")]
		[SerializeField] private float glidingStartMomentum;
		[SerializeField] private float momentumTweenTime;
		[SerializeField] private InputActionAsset playerInput;

		private AdvancedWalkerController controller;
		private bool glideInputActive;
		private float finalYMomentum;

		private void Awake()
		{
			Debug.Log(playerInput["Glide"]);
			playerInput["Glide"].started += Glide;
			playerInput["Glide"].canceled += Glide;
		}

		private void OnEnable()
		{
			playerInput.Enable();
		}

		private void OnDisable()
		{
			playerInput.Disable();
		}

		private void Start()
		{
			controller = GameManager.Instance.Player.GetComponent<AdvancedWalkerController>();
		}

		private void FixedUpdate()
		{
			ApplyGlideMomentum();
		}

		private void SetGlideMomentum()
		{
			finalYMomentum = glidingStartMomentum;
			DOTween.To(()=> finalYMomentum, x=> finalYMomentum = x, glidingYMomentum, momentumTweenTime);
		}

		private void ApplyGlideMomentum()
		{
			if (glideInputActive && controller.currentControllerState == AdvancedWalkerController.ControllerState.Falling)
			{
				Vector3 momentum = controller.GetMomentum();
				var glidingMomentum = new Vector3(momentum.x, finalYMomentum, momentum.z);
				controller.SetMomentum(glidingMomentum);
			}
		}

		public void Glide(InputAction.CallbackContext _context)
		{
			if (_context.started)
			{
				if(controller.currentControllerState == AdvancedWalkerController.ControllerState.Falling ||
				   controller.currentControllerState == AdvancedWalkerController.ControllerState.Rising)
				{
					glideInputActive = true;
					SetGlideMomentum();
				}
			}

			if (_context.canceled)
			{
				glideInputActive = false;
			}
		}
	}
}