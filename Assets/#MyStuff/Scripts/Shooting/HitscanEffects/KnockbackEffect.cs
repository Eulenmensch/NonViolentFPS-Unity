using UnityEngine;

public class KnockbackEffect : MonoBehaviour, IHitscanEffect
{
	[SerializeField] private float knockbackForce;

	public void Initialize(RaycastHit _hit)
	{
		KnockBack(_hit);
		Destroy(this);
	}

	private void KnockBack(RaycastHit _hit)
	{
		var body = GetComponentInParent<Rigidbody>();
		body.AddForceAtPosition(Camera.main.transform.forward * knockbackForce, _hit.point);
	}
}