using MoreMountains.Feedbacks;
using NonViolentFPS.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public abstract class Gun : SerializedScriptableObject
	{
		[SerializeField, Required]
		[BoxGroup("Visuals")]
		private GameObject gunPrefab;

		protected ShooterCopy Shooter;
		protected GunVisuals Visuals;
		protected Transform ShootingOrigin;
		protected Transform AttachmentPoint;
		protected GameObject GunInstance;

		protected virtual void OnValidate()
		{
			Debug.Assert(gunPrefab.GetComponentInChildren<GunVisuals>(), "The object you assigned has no GunVisuals component on any of its children.");
		}

		public virtual void SetUpGun(ShooterCopy _shooter)
		{
			Shooter = _shooter;
			ShootingOrigin = Shooter.ShootingOrigin;

			SpawnGun(_shooter);
			Visuals = GunInstance.GetComponentInChildren<GunVisuals>();
			UpdateAmmoUI();
		}

		public void CleanUpGun()
		{
			Destroy(GunInstance);
		}

		private void SpawnGun(ShooterCopy _shooter)
		{
			AttachmentPoint = _shooter.GunAttachmentPoint;
			GunInstance = Instantiate(gunPrefab, AttachmentPoint.position, Quaternion.identity, AttachmentPoint);
			GunInstance.transform.localRotation = Quaternion.identity;
		}

		protected void PlayFeedbacks(MMFeedbacks _feedbacks)
		{
			if (_feedbacks == null) return;
			_feedbacks.PlayFeedbacks();
		}

		protected void StopFeedbacks(MMFeedbacks _feedbacks)
		{
			if (_feedbacks == null) return;
			_feedbacks.StopFeedbacks();
		}

		protected void UpdateUIAmmoTypeCount(int _ammoCount)
		{
			PlayerEvents.Instance.UpdateGunStats(_ammoCount);
		}

		private void UpdateAmmoUI()
		{

		}
	}
}