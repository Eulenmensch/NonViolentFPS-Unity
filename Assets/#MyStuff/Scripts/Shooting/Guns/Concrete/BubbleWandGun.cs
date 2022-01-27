using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NonViolentFPS.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
	[CreateAssetMenu(menuName = "Guns/BubbleWandGun")]
	public class BubbleWandGun : Gun, IPrimaryFireable, ISecondaryFireable, IReloadable, IAmmoClipComponent
	{
		[BoxGroup("Visuals")]
		[SerializeField] private Vector3 adsPosition;
		[BoxGroup("Visuals")]
		[SerializeField] private Vector3 adsRotation;
		[BoxGroup("Visuals")]
		[SerializeField] private float adsDuration;

		[BoxGroup("Shooting")]
		[SerializeField] private GameObject bubblePrefab;
		[BoxGroup("Shooting")]
		[SerializeField] private float fullChargeTime;
		[BoxGroup("Shooting")]
		[SerializeField] private AnimationCurve fireForceCurve;
		[BoxGroup("Shooting")]
		[SerializeField] private float fireForceMultiplier;

		[BoxGroup("Reloading")]
		[SerializeField] private float reloadTime;
		[BoxGroup("Reloading")]
		[SerializeField] private float reloadTimeBuffer;
		[BoxGroup("Reloading")]
		[SerializeField] private float dunkTiming;

		[BoxGroup("Reloading")] [field: SerializeField] public int ClipSize { get; set; }

		[BoxGroup("Input")]
		[SerializeField] private InputActionAsset inputAsset;

		private int ammoInClip;
		public int AmmoInClip
		{
			get => ammoInClip;
			set
			{
				ammoInClip = value;
				if (value > ClipSize)
					ammoInClip = ClipSize;
			}
		}

		private Transform projectileContainer;
		private GameObject bubbleInstance;
		private float timer;
		private float fireForce;

		private Vector3 attachmentPointDefaultPosition;
		private Vector3 attachmentPointDefaultRotation;
		public Vector3 gunTargetDefaultPosition { get; private set; }
		public Vector3 gunTargetDefaultRotation { get; private set; }

		private LagBehindTarget[] lagBehindTargets;

		private BubbleWandVisuals bubbleWandVisuals;

		public override void SetUpGun(ShooterCopy _shooter)
		{
			base.SetUpGun(_shooter);
			projectileContainer = _shooter.ProjectileContainer;

			CacheDefaultAttachmentPointTransform();
			UpdateUIAmmoTypeCount(1);

			AmmoInClip = ClipSize;

			//this gun needs a non-default shooting origin
			if(Visuals == null)
			{
				Debug.LogError("There is no GunVisuals Component attached to the gun instance");
				return;
			}
			ShootingOrigin = Visuals.ShootingOriginOverride;

			bubbleWandVisuals = Visuals as BubbleWandVisuals;
			if (bubbleWandVisuals == null)
			{
				Debug.Log("The attached Visuals aren't of the type " + nameof(BubbleWandVisuals));
			}

			CacheDefaultGunTargetTransform();

			lagBehindTargets = GunInstance.GetComponentsInChildren<LagBehindTarget>();
			// foreach (var target in lagBehindTargets)
			// {
			// 	target.transform.parent = null;
			// }
		}

		public override void CleanUpGun()
		{
			base.CleanUpGun();
			foreach (var target in lagBehindTargets)
			{
				Destroy(target.gameObject);
			}
		}

		private void CacheDefaultAttachmentPointTransform()
		{
			attachmentPointDefaultPosition = AttachmentPoint.localPosition;
			attachmentPointDefaultRotation = AttachmentPoint.localRotation.eulerAngles;
		}

		private void CacheDefaultGunTargetTransform()
		{
			gunTargetDefaultPosition = bubbleWandVisuals.GunTarget.localPosition;
			gunTargetDefaultRotation = bubbleWandVisuals.GunTarget.localRotation.eulerAngles;
		}

		public void PrimaryFireEnter()
		{
			timer = 0;
			fireForce = 0;
			PlayFeedbacks(Visuals.ChargeFeedback);
		}

		public void PrimaryFireAction()
		{
			timer += Time.deltaTime;
			if (timer <= fullChargeTime)
			{
				var normalizedTimer = timer / fullChargeTime;
				fireForce = fireForceMultiplier * fireForceCurve.Evaluate(normalizedTimer);
			}
			else
			{
				StopFeedbacks(Visuals.ChargeFeedback);
			}
		}

		public void PrimaryFireExit()
		{
			StopFeedbacks(Visuals.ChargeFeedback);
			if (AmmoInClip <= 0) return;
			ShootBubble();
			UpdateAmmoInClipCount();
			PlayFeedbacks(Visuals.FireFeedback);
		}

		public void SecondaryFireEnter()
		{
			AnimateAttachmentPoint(adsPosition, adsRotation, adsDuration);
			PlayerEvents.Instance.AimDownSights(true);
		}
		public void SecondaryFireAction(){}

		public void SecondaryFireExit()
		{
			var targetPosition = attachmentPointDefaultPosition;
			var targetRotation = attachmentPointDefaultRotation;
			AnimateAttachmentPoint(targetPosition, targetRotation, adsDuration);
			PlayerEvents.Instance.AimDownSights(false);
		}

		public void Reload()
		{
			PlayerEvents.Instance.Reload(reloadTime);
			Shooter.StartCoroutine(ReloadTimer(reloadTime - reloadTimeBuffer));
			inputAsset.Disable();
		}

		private IEnumerator ReloadTimer(float _reloadTime)
		{
			var startTime = Time.time;
			while (Time.time - startTime <= _reloadTime)
			{
				if (Time.time - startTime >= dunkTiming)
				{
					AmmoInClip = ClipSize;
					UIEvents.Instance.UpdateAmmoText(AmmoInClip);
				}
				yield return null;
			}
			PlayerEvents.Instance.ReloadCompleted();
			inputAsset.Enable();
		}

		private void ShootBubble()
		{
			bubbleInstance = Instantiate(bubblePrefab, ShootingOrigin.position, Quaternion.identity, projectileContainer);
			var bubbleRigidbody = bubbleInstance.GetComponent<Rigidbody>();
			var force = Camera.main.transform.forward * fireForce + GetPlayerForwardVelocity();
			bubbleRigidbody.AddForce(force, ForceMode.VelocityChange);
		}

		private void UpdateAmmoInClipCount()
		{
			AmmoInClip--;
			UIEvents.Instance.UpdateAmmoText(AmmoInClip);
		}

		private void AnimateAttachmentPoint(Vector3 _targetPosition, Vector3 _targetRotation, float _animationDuration)
		{
			Shooter.GunAttachmentPoint.DOLocalMove(_targetPosition, _animationDuration).SetEase(Ease.InOutCirc);
			Shooter.GunAttachmentPoint.DOLocalRotate(_targetRotation, _animationDuration).SetEase(Ease.InOutCirc);
		}

		public void AnimateGunTarget(Vector3 _targetPosition, Vector3 _targetRotation, float _animationDuration)
		{
			bubbleWandVisuals.GunTarget.DOLocalMove(_targetPosition, _animationDuration).SetEase(Ease.InOutCirc);
			bubbleWandVisuals.GunTarget.DOLocalRotate(_targetRotation, _animationDuration).SetEase(Ease.InOutCirc);
		}

		private static Vector3 GetPlayerForwardVelocity()
        {
         	var playerRigidbody = Manager.GameManager.Instance.Player.GetComponent<Rigidbody>();
            if (Vector3.Dot(playerRigidbody.velocity, Camera.main.transform.forward) <= 0)
            {
	            return Vector3.zero;
            }
            return Vector3.Project(playerRigidbody.velocity, Camera.main.transform.forward);
        }
	}
}