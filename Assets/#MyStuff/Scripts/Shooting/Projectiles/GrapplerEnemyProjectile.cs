using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class GrapplerEnemyProjectile : PhysicsProjectile
	{
		[SerializeField] private GameObject grapplerPrefab;

		protected override void ImpactAction(Collision _other)
		{
			AttachToPlayer(_other);
			SpawnGrappler();
		}

		private void AttachToPlayer(Collision _other)
		{
			var npc = grapplerPrefab.GetComponent<GrapplerEnemyNPC>();
			if (_other.gameObject.Equals(GameManager.Instance.Player))
			{
				NPCEvents.Instance.AttachToPlayer(npc);
				Destroy(gameObject);
			}
		}

		private void SpawnGrappler()
		{
			Instantiate(grapplerPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}