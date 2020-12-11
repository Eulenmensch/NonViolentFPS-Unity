using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	[CreateAssetMenu(menuName = "Guns/BubbleWandGunCopy")]
	public class BubbleWandGunCopy : Gun, IPrimaryFireable, ISecondaryFireable
	{
		[BoxGroup("Visuals")]
		[SerializeField] private Vector3 adsPosition;
		[BoxGroup("Visuals")]
		[SerializeField] private Vector3 adsRotation;
		[BoxGroup("Visuals")]
		[SerializeField] private float adsDuration;

		[BoxGroup("Settings")]
		[SerializeField] private GameObject bubblePrefab;
		[BoxGroup("Settings")]
		[SerializeField] private float fullChargeTime;
		[BoxGroup("Settings")]
		[SerializeField] private AnimationCurve fireForceCurve;

		private Transform projectileContainer;
		private GameObject bubbleInstance;
		private float timer;
		private float fireForce;

		private Vector3 attachmentPointDefaultPosition;
		private Vector3 attachmentPointDefaultRotation;

		public override void SetUpGun(ShooterCopy _shooter)
		{
			base.SetUpGun(_shooter);
			projectileContainer = _shooter.ProjectileContainer;

			CacheDefaultAttachmentPointPosition();

			UpdateUIAmmoCount(1);

			//this gun needs a non-default shooting origin
			if(Visuals == null)
			{
				Debug.LogError("There is no GunVisuals Component attached to the gun instance");
				return;
			}
			ShootingOrigin = Visuals.ShootingOriginOverride;
		}

		private void CacheDefaultAttachmentPointPosition()
		{
			attachmentPointDefaultPosition = AttachmentPoint.localPosition;
			attachmentPointDefaultRotation = AttachmentPoint.localRotation.eulerAngles;
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
				fireForce = fireForceCurve.Evaluate(normalizedTimer);
			}
			else
			{
				StopFeedbacks(Visuals.ChargeFeedback);
			}
		}

		public void PrimaryFireExit()
		{
			ShootBubble();
			StopFeedbacks(Visuals.ChargeFeedback);
			PlayFeedbacks(Visuals.FireFeedback);
		}

		public void SecondaryFireEnter()
		{
			AnimateAttachmentPoint(adsPosition, adsRotation, adsDuration);
		}
		public void SecondaryFireAction(){}

		public void SecondaryFireExit()
		{
			var targetPosition = attachmentPointDefaultPosition;
			var targetRotation = attachmentPointDefaultRotation;
			AnimateAttachmentPoint(targetPosition, targetRotation, adsDuration);
		}

		private void ShootBubble()
		{
			bubbleInstance = Instantiate(bubblePrefab, ShootingOrigin.position, Quaternion.identity, projectileContainer);
			var bubbleRigidbody = bubbleInstance.GetComponent<Rigidbody>();
			var force = Camera.main.transform.forward * fireForce + GetPlayerForwardVelocity();
			bubbleRigidbody.AddForce(force, ForceMode.VelocityChange);
		}

		private void AnimateAttachmentPoint(Vector3 _targetPosition, Vector3 _targetRotation, float _animationDuration)
		{
			Shooter.GunAttachmentPoint.DOLocalMove(_targetPosition, _animationDuration).SetEase(Ease.InOutCirc);
			Shooter.GunAttachmentPoint.DOLocalRotate(_targetRotation, _animationDuration).SetEase(Ease.InOutCirc);
		}

		private Vector3 GetPlayerForwardVelocity()
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