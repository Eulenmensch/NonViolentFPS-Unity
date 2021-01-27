using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "SpawnParticlesEnterAction", menuName = "AI Kit/Enter Actions/SpawnParticlesEnterAction")]
	public class SpawnParticlesEnterAction : EnterAction
	{
		[SerializeField] private GameObject ParticlePrefab;

		public override void Enter(NPC _npc)
		{
			var particleSpawnComponent = _npc as IParticleSpawnComponent;
			if (particleSpawnComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IParticleSpawnComponent));
				return;
			}

			var spawnPoint = particleSpawnComponent.SpawnPoint;
			var particles = Instantiate(ParticlePrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
			particleSpawnComponent.Particles.Add(particles);
		}
	}
}