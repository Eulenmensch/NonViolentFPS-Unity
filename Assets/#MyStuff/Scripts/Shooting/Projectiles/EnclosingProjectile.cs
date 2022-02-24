using System.Threading.Tasks;
using DG.Tweening;
using MoreMountains.Feedbacks;
using NonViolentFPS.Extension_Classes;
using NonViolentFPS.Level;
using NonViolentFPS.Manager;
using NonViolentFPS.NPCs;
using Obi;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class EnclosingProjectile : PhysicsProjectile
	{
		[SerializeField] private LayerMask attachableMask;
		[SerializeField] private MMFeedbacks burstFeedbacks;
		[SerializeField] private MMFeedbacks unfreezeFeedbacks;
		[SerializeField] private float growthDuration;
		[SerializeField] private float collisionGraceTime;
		[SerializeField] private float yOffset;
		[SerializeField] private float riseForce;
		[SerializeField] private float maxHeight;
		[SerializeField] private float travelGrowthDuration;
		[SerializeField] private float maxTravelScale;
		[SerializeField] private Material frozenMaterial;
		[SerializeField] private bool burstAfterEnclosing;
		[SerializeField] private float burstAfterEnclosingTime;

		public Transform AttachedTarget { get; private set; }

		private Material defaultMaterial;
		private Renderer rendererRef;
		private Rigidbody rigidbodyRef;
		private SphereCollider sphereCollider;
		private bool unfreezing;
		private bool hasBurst;
		private Collider[] otherColliders;

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
				ApplyUpwardsForce();
			}
		}

		private void Update()
		{
			if (AttachedTarget != null)
			{
				MoveWithEnclosedObject();
			}
		}

		protected override void UnactivatedImpactAction(Collider _other)
		{
			DOTween.Kill(transform);
			//if the impacted thing is a windzone, ignore the impact
			if (_other.GetComponent<Ventilator>() != null)
			{
				return;
			}
			//if the gameObject is NOT on the attachable layer, burst
			if (!attachableMask.IsGameObjectInMask(_other.gameObject))
			{
				Burst();
				return;
			}
			//burst if hitting another bubble
			var enclosingProjectile = _other.GetComponentInChildren<EnclosingProjectile>();
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
			AttachedTarget = _other.transform;
			//HACK: the inability of scriptable objects to subscribe to c# events forces me to
			//		use the spaggethiness of the IBubbleComponent and manually setting its variable.
			if (AttachedTarget.gameObject.GetComponent<NPC>() is IBubbleComponent bubbleComponent)
			{
				bubbleComponent.AttachedBubble = this;
			}
			OtherRigidbody = AttachedTarget.gameObject.GetComponent<Rigidbody>();

			//disable the enclosed collider to avoid physics going haywire due to multiple colliders
			otherColliders = AttachedTarget.gameObject.GetComponentsInChildren<Collider>();
			// foreach (var otherCollider in otherColliders)
			// {
			// 	otherCollider.enabled = false;
			// }

			// transform.DOLocalMove(Vector3.up * yOffset, growthDuration).SetEase(Ease.OutCirc);
			if (burstAfterEnclosing)
			{
				BurstAfterSeconds(burstAfterEnclosingTime);
			}
		}

		protected override void ActivatedImpactAction(Collision _other)
		{
			print(_other.gameObject.name);
			if (_other.gameObject.GetComponent<ObiCollider>() != null)
			{
				return;
			}
			if (_other.gameObject != AttachedTarget.gameObject)
			{
				print(_other.gameObject.name);
				Burst();
			}
		}

		private void ApplyUpwardsForce()
		{
			if (transform.position.y >= maxHeight) return;
			if (hasBurst) return;
			OtherRigidbody.AddForceAtPosition(Vector3.up * riseForce, transform.position, ForceMode.Acceleration);
		}

		private void MoveWithEnclosedObject()
		{
			transform.position = AttachedTarget.position;
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

		private async void BurstAfterSeconds(float _seconds)
		{
			var timer = 0f;
			while (timer <= _seconds)
			{
				timer += Time.deltaTime;
				await Task.Yield();
			}

			Burst();
		}

		private void Burst()
		{
			sphereCollider.enabled = false;
			hasBurst = true;
			if (AttachedTarget != null)
			{
				foreach (var otherCollider in otherColliders)
				{
					otherCollider.enabled = true;
				}
			}
			burstFeedbacks.PlayFeedbacks();
			if(AttachedTarget == null) return;
			if (AttachedTarget.gameObject.GetComponent<NPC>() is IBubbleComponent bubbleComponent)
			{
				bubbleComponent.AttachedBubble = null;
			}
		}

		public void Freeze()
		{
			if (unfreezing) return;
			if(!Activated)
			{
				DOTween.Kill(transform);
				PlayMMFeedbacks();
				Activate();
			}
			doesImpactWithPlayer = false;
			rendererRef.material = frozenMaterial;
		}

		// private async void DestroySelf()
		// {
		// 	while (burstFeedbacks.IsPlaying)
		// 	{
		// 		await Task.Yield();
		// 	}
		//
		// 	Destroy(gameObject);
		// }

		public async void Unfreeze(float _unfreezeTime)
		{
			unfreezing = true;
			unfreezeFeedbacks.PlayFeedbacks();
			var timer = 0f;
			while (timer <= _unfreezeTime)
			{
				timer += Time.deltaTime;
				await Task.Yield();
			}

			Burst();
		}
	}
}
