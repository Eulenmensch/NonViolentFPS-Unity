using System.Collections;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class KnockbackEffect : MonoBehaviour, IHitscanEffect
	{
		[SerializeField] private float knockbackForce;
		[SerializeField] private float upMultiplier;
		public void Initialize(RaycastHit _hit)
		{
			KnockBack(_hit);
			StartCoroutine(DestroyAfterSeconds(0.1f));
		}

		private void KnockBack(RaycastHit _hit)
		{
			var body = GetComponentInParent<Rigidbody>();
			var force = Camera.main.transform.forward + Vector3.up * upMultiplier;
			body.AddForceAtPosition( force * knockbackForce, _hit.point);
		}

		IEnumerator DestroyAfterSeconds(float _seconds)
		{
			yield return new WaitForSeconds(_seconds);
			Destroy(gameObject);
		}
	}
}