using DG.Tweening;
using Ludiq.PeekCore;
using NonViolentFPS.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
	[CreateAssetMenu(menuName = "Guns/SoapWandGun")]
	public class SoapWandGun : ScriptableObject, IGun
	{
		[Header("Visuals")]
		[SerializeField] private GameObject gunPrefab;
		[SerializeField] private Vector3 adsPosition;
		[SerializeField] private Vector3 adsRotation;
		[SerializeField] private float adsDuration;

		[Header("Gun Settings")]
		[SerializeField] private GameObject bubblePrefab;
		[SerializeField] private float fullChargeTime;
		[SerializeField] private AnimationCurve fireForceCurve;

		private Transform projectileSpawnPoint;
		private Transform projectileContainer;
		private float timer;
		private GameObject gunInstance;
		private GameObject bubbleInstance;
		private float fireForce;

		private Shooter shooter;
		private Vector3 attachmentPointDefaultPosition;
		private Vector3 attachmentPointDefaultRotation;

		public void SetupGun(Shooter _shooter)
		{
			shooter = _shooter;
			projectileContainer = _shooter.ProjectileContainer;

			var attachmentPoint = _shooter.GunAttachmentPoint;
			gunInstance = Instantiate(gunPrefab, attachmentPoint.position, Quaternion.identity, attachmentPoint);
			gunInstance.transform.localRotation = Quaternion.identity;
			attachmentPointDefaultPosition = attachmentPoint.localPosition;
			attachmentPointDefaultRotation = attachmentPoint.localRotation.eulerAngles;

			var visuals = gunInstance.GetComponent<GunVisuals>();
			projectileSpawnPoint = visuals.ShootingOrigin;

			PlayerEvents.Instance.UpdateGunStats(1);
		}

		public void CleanUpGun()
		{
			Destroy(gunInstance);
		}

		public void PrimaryMouseButtonEnter()
		{
			timer = 0;
			fireForce = 0;
		}

		public void PrimaryMouseButtonAction()
		{
			timer += Time.deltaTime;
			if (timer <= fullChargeTime)
			{
				var normalizedTimer = timer / fullChargeTime;
				fireForce = fireForceCurve.Evaluate(normalizedTimer);
			}
		}

		public void PrimaryMouseButtonExit()
		{
			bubbleInstance = Instantiate(bubblePrefab, projectileSpawnPoint.position, Quaternion.identity, projectileContainer);
			var bubbleRigidbody = bubbleInstance.GetComponent<Rigidbody>();
			var force = Camera.main.transform.forward * fireForce;
			bubbleRigidbody.AddForce(force, ForceMode.VelocityChange);
		}

		public void SecondaryMouseButtonEnter()
		{
			AnimateAttachmentPoint(adsPosition, adsRotation, adsDuration);
		}
		public void SecondaryMouseButtonAction(){}

		public void SecondaryMouseButtonExit()
		{
			var targetPosition = attachmentPointDefaultPosition;
			var targetRotation = attachmentPointDefaultRotation;
			AnimateAttachmentPoint(targetPosition, targetRotation, adsDuration);
		}
		public void ScrollWheelAction(InputAction.CallbackContext _context){}

		private void AnimateAttachmentPoint(Vector3 _targetPosition, Vector3 _targetRotation, float _animationDuration)
		{
			shooter.GunAttachmentPoint.DOLocalMove(_targetPosition, _animationDuration).SetEase(Ease.InOutCirc);
			shooter.GunAttachmentPoint.DOLocalRotate(_targetRotation, _animationDuration).SetEase(Ease.InOutCirc);
		}
	}
}