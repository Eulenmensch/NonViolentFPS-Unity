using NonViolentFPS.AI;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class ShootingRangeTestEffect : MonoBehaviour, IHitscanEffect
	{
		public void Initialize(RaycastHit _hit) { }

		private void Start()
		{
			var machine = GetComponentInParent<StateMachine>();
			machine.hit = true;
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}
