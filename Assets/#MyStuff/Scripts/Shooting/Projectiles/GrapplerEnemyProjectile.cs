using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class GrapplerEnemyProjectile : PhysicsProjectile
	{
		[SerializeField] private GameObject grapplerPrefab;

		protected override void UnactivatedImpactAction(Collision _other)
		{
			if (_other.gameObject.Equals(GameManager.Instance.Player))
			{
				AttachToPlayer(_other);
				return;
			}
			SpawnGrappler();
		}

		private void AttachToPlayer(Collision _other)
		{
			var npc = grapplerPrefab.GetComponent<GrapplerEnemyNPC>();
			NPCEvents.Instance.AttachToPlayer(npc);
			Destroy(gameObject);
		}

		private void SpawnGrappler()
		{
			var grappler = Instantiate(grapplerPrefab, transform.position, Quaternion.identity);
			grappler.transform.SetParent(transform.parent);
			Destroy(gameObject);
		}
	}
}