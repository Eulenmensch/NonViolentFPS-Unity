using System;
using System.Threading.Tasks;
using DG.Tweening;
using MoreMountains.Feedbacks;
using NonViolentFPS.Manager;
using NonViolentFPS.Physics;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class EnclosingProjectile : PhysicsProjectile
	{
		[SerializeField] private MMFeedbacks burstFeedbacks;
		[SerializeField] private float growthDuration;
		[SerializeField] private float collisionGraceTime;
		[SerializeField] private float yOffset;
		[SerializeField] private float riseForce;
		[SerializeField] private float maxHeight;
		[SerializeField] private float travelGrowthDuration;
		[SerializeField] private float maxTravelScale;
		[SerializeField] private Material frozenMaterial;

		public Transform AttachedTarget { get; private set; }

		private Material defaultMaterial;
		private Renderer rendererRef;
		private Rigidbody rigidbodyRef;
		private SphereCollider sphereCollider;
		private bool unfreezing;

		protected override void Start()
		{
			base.Start();
			rigidbodyRef = GetComponent<Rigidbody>();
			rendererRef = GetComponent<Renderer>();
			defaultMaterial = rendererRef.material;
			sphereCollider = GetComponent<SphereCollider>();
			sphereCollider.enabled = false;
			AttachedTarget = null;
			EnableCollisionAfterSeconds(sphereCollider, collisionGraceTime);
			transform.DOScale(Vector3.one *maxTravelScale, travelGrowthDuration).SetEase(Ease.OutExpo);
		}

		private void FixedUpdate()
		{
			if (!Activated) return;

			if ( OtherRigidbody != null )
			{
				if(transform.position.y >= maxHeight) {return;}
				OtherRigidbody.AddForceAtPosition( Vector3.up * riseForce, transform.position, ForceMode.Acceleration );
			}
		}

		protected override void ImpactAction(Collision _other)
		{
			DOTween.Kill(transform);
			if (_other.rigidbody == null)
			{
				Burst();
				return;
			}
			var enclosingProjectile = _other.collider.GetComponentInChildren<EnclosingProjectile>();
			if (enclosingProjectile != null)
			{
				Burst();
				return;
			}
			if (_other.gameObject == GameManager.Instance.Player)
			{
				Burst();
				return;
			}

			rigidbodyRef.isKinematic = true;
			Destroy(GetComponent<CustomGravity>());
			Destroy( rigidbodyRef );
			AttachedTarget = _other.transform;
			ChildToOtherRigidbody(_other);
			transform.DOLocalMove(Vector3.up * yOffset, growthDuration).SetEase(Ease.OutCirc);
		}

		private async void EnableCollisionAfterSeconds(SphereCollider _collider, float _seconds)
		{
			var timer = 0f;
			while (timer <= _seconds)
			{
				timer += Time.deltaTime;
				await Task.Yield();
			}

			_collider.enabled = true;
		}

		private void Burst()
		{
			sphereCollider.enabled = false;
			burstFeedbacks.PlayFeedbacks();
		}

		public void Freeze()
		{
			if(!Activated)
			{
				DOTween.Kill(transform);
				PlayMMFeedbacks();
				Activate();
			}
			doesImpactWithPlayer = false;
			rendererRef.material = frozenMaterial;
		}

		public async void Unfreeze(float _unfreezeTime)
		{
			var timer = 0f;
			while (timer <= _unfreezeTime)
			{
				timer += Time.deltaTime;
				await Task.Yield();
			}

			doesImpactWithPlayer = true;
			//TODO: Material Flicker
			if(rendererRef != null)
			{
				rendererRef.material = defaultMaterial;
			}
		}
	}
}
