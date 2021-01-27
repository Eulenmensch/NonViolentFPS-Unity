using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "DespawnParticlesExitAction", menuName = "AI Kit/Exit Actions/DespawnParticlesExitAction")]
	public class DespawnParticlesExitAction : ExitAction
	{
		public override void Exit(NPC _npc)
		{
			var particleSpawnComponent = _npc as IParticleSpawnComponent;
			if (particleSpawnComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IParticleSpawnComponent));
				return;
			}

			foreach (var particle in particleSpawnComponent.Particles)
			{
				Destroy(particle);
			}
		}
	}
}