using MoreMountains.Feedbacks;
using NonViolentFPS.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
	[CreateAssetMenu(menuName = "Guns/HitscanGun")]
	public class HitscanGun : Gun, IPrimaryFireable, IScrollwheelActionable
	{
		[BoxGroup("Settings")]
		[SerializeField] private GameObject[] effects;
		[BoxGroup("Settings")]
		[SerializeField] private float fireRate;
		[BoxGroup("Settings")]
		[SerializeField] private float sphereCastRadius;
		[BoxGroup("Settings")]
		[SerializeField] private LayerMask interactibleMask;

		private float timer;
		private int activeEffectIndex;

		protected override void OnValidate()
		{
			base.OnValidate();
			foreach (var effect in effects)
			{
				Debug.Assert(effect.GetComponent<IHitscanEffect>() != null,"The prefab you assigned has no component that implements IHitscanEffect." );
			}
		}

		public override void SetUpGun(ShooterCopy _shooter)
		{
			base.SetUpGun(_shooter);
			activeEffectIndex = 0;
			UpdateUIAmmoCount(effects.Length);
		}

		public void PrimaryFireEnter()
		{
			timer = fireRate;
		}
		public void PrimaryFireAction()
		{
			timer += Time.deltaTime;
			if (!(timer >= fireRate)) return;
			timer = 0;
			Shoot();
			PlayFeedbacks(Visuals.FireFeedback);
		}
		public void PrimaryFireExit() { }

		public void ScrollWheelAction(InputAction.CallbackContext _context, bool _invertScrollDirection)
		{
			Vector2 input = _context.ReadValue<Vector2>();
			int projectileCount = effects.Length - 1;

			if(_context.started)
			{
				int direction = Mathf.RoundToInt(input.y);
				direction = _invertScrollDirection ? -direction : direction;

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
			if (UnityEngine.Physics.SphereCast(ShootingOrigin.position, sphereCastRadius, Camera.main.transform.forward, out var hit,
				Mathf.Infinity, interactibleMask, QueryTriggerInteraction.Ignore))
			{
				var hitTransform = hit.rigidbody.transform; // Because only one rigidbody per object hierarchy
				var effect = Instantiate(effects[activeEffectIndex], hitTransform.position, Quaternion.identity, hitTransform).GetComponent<IHitscanEffect>();
				effect.Initialize(hit);
			}
		}
	}
}
