using System;
using System.Threading.Tasks;
using DG.Tweening;
using MoreMountains.Feedbacks;
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

		private Material defaultMaterial;
		private Renderer renderer;
		private Rigidbody rigidbodyRef;

		protected override void Start()
		{
			base.Start();
			rigidbodyRef = GetComponent<Rigidbody>();
			renderer = GetComponent<Renderer>();
			defaultMaterial = renderer.material;
			var sphereCollider = GetComponent<SphereCollider>();
			sphereCollider.enabled = false;
			EnableCollisionAfterSeconds(sphereCollider, collisionGraceTime);
			transform.DOScale(Vector3.one *maxTravelScale, travelGrowthDuration).SetEase(Ease.InOutCirc);
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

			rigidbodyRef.isKinematic = true;
			Destroy(GetComponent<CustomGravity>());
			Destroy( rigidbodyRef );

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
			burstFeedbacks.PlayFeedbacks();
		}

		public void Freeze()
		{
			PlayMMFeedbacks();
			Activate();
			doesImpactWithPlayer = false;
			renderer.material = frozenMaterial;
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
			renderer.material = defaultMaterial;
		}
	}
}
