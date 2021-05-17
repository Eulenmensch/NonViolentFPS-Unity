using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class GrapplerEnemyProjectile : PhysicsProjectile
	{
		[SerializeField] private GameObject grapplerPrefab;

		protected override void ImpactAction(Collision _other)
		{
			SpawnGrappler();
		}

		private void SpawnGrappler()
		{
			Instantiate(grapplerPrefab, transform.position, Quaternion.identity);
		}
	}
}