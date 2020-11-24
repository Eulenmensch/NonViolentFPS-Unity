using DG.Tweening;
using UnityEngine;

public class EnclosingProjectile : PhysicsProjectile
{
	[SerializeField] private float maxSize;
	[SerializeField] private float growthDuration;
	[SerializeField] private float yOffset;
	[SerializeField] private float riseForce;
	[SerializeField] private float maxHeight;

	private Rigidbody rigidbodyRef;

	protected override void Start()
	{
		base.Start();
		rigidbodyRef = GetComponent<Rigidbody>();
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
		var enclosingProjectile = _other.collider.GetComponentInChildren<EnclosingProjectile>();
		if(enclosingProjectile != null) {return;}

		rigidbodyRef.isKinematic = true;
		Destroy( rigidbodyRef );

		ChildToOtherRigidbody(_other);
		transform.DOLocalMove(Vector3.up * yOffset, growthDuration).SetEase(Ease.InOutCirc);

		transform.DOScale( maxSize, growthDuration ).SetEase( Ease.InOutElastic );
	}
}
