using MoreMountains.Feedbacks;
using NonViolentFPS.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
	[CreateAssetMenu(menuName = "Guns/HitscanGun")]
	public class HitscanGun : ScriptableObject, IGun
	{
		[Header("Visuals")]
		[SerializeField] private GameObject gun;

		[Header("Gun Settings")]
		[SerializeField] private GameObject[] effects;
		[SerializeField] private float fireRate;
		[SerializeField] private float sphereCastRadius;
		[SerializeField] private LayerMask interactibleMask;
		[SerializeField] private bool invertScrollDirection;

		private Transform sphereCastOrigin;
		private float timer;
		private int activeEffectIndex;
		private GameObject gunObject;


		private void OnValidate()
		{
			foreach (var effect in effects)
			{
				Debug.Assert(effect.GetComponent<IHitscanEffect>() != null,"The prefab you assigned has no component that implements IHitscanEffect." );
			}
		}

		public void SetupGun(Shooter _shooter)
		{
			sphereCastOrigin = _shooter.ShootingOrigin;
			activeEffectIndex = 0;

			var attachmentPoint = _shooter.GunAttachmentPoint;
			gunObject = Instantiate(gun, attachmentPoint.position, Quaternion.identity, attachmentPoint);
			gunObject.transform.localRotation = Quaternion.identity;

			PlayerEvents.Instance.UpdateGunStats(effects.Length);
		}

		public void CleanUpGun()
		{
			Destroy(gunObject);
		}

		public void PrimaryMouseButtonEnter()
		{
			timer = fireRate;
		}
		public void PrimaryMouseButtonAction()
		{
			timer += Time.deltaTime;
			if (!(timer >= fireRate)) return;
			timer = 0;
			Shoot();
			PlayFeedback();
		}
		public void PrimaryMouseButtonExit() { }

		public void SecondaryMouseButtonEnter() { }
		public void SecondaryMouseButtonAction() { }
		public void SecondaryMouseButtonExit() { }

		public void ScrollWheelAction(InputAction.CallbackContext _context)
		{
			Vector2 input = _context.ReadValue<Vector2>();
			int projectileCount = effects.Length - 1;

			if(_context.started)
			{
				int direction = Mathf.RoundToInt(input.y);
				direction = invertScrollDirection ? -direction : direction;

				if (activeEffectIndex < projectileCount && activeEffectIndex > 0)
				{
					activeEffectIndex += direction;
				}
				else if (activeEffectIndex == projectileCount)
				{
					if (direction > 0)
					{
						activeEffectIndex = 0;
					}
					else if (direction < 0)
					{
						activeEffectIndex += direction;
					}
				}
				else if (activeEffectIndex == 0)
				{
					if (direction < 0)
					{
						activeEffectIndex = projectileCount;
					}
					else if (direction > 0)
					{
						activeEffectIndex = 1;
					}
				}
			}

			PlayerEvents.Instance.ChangeAmmo(activeEffectIndex);
		}

		private void Shoot()
		{
			if (UnityEngine.Physics.SphereCast(sphereCastOrigin.position, sphereCastRadius, Camera.main.transform.forward, out var hit,
				Mathf.Infinity, interactibleMask, QueryTriggerInteraction.Ignore))
			{
				var hitTransform = hit.rigidbody.transform; // Because only one rigidbody per object hierarchy
				var effect = Instantiate(effects[activeEffectIndex], hitTransform.position, Quaternion.identity, hitTransform).GetComponent<IHitscanEffect>();
				effect.Initialize(hit);
			}
		}

		private void PlayFeedback()
		{
			var feedbacks = gunObject.GetComponentInChildren<MMFeedbacks>();
			feedbacks.PlayFeedbacks();
		}
	}
}
