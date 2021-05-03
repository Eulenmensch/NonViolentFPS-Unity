using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Behaviours/MoveToCachedPlayerLocationBehaviour")]
	public class MoveToCachedPlayerLocationBehaviour : AIBehaviour
	{
		public override UpdateType type => UpdateType.Regular;

		public override void DoBehaviour(NPC _npc)
		{
			var chaseComponent = _npc as IChaseComponent;
			if (chaseComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IChaseComponent));
				return;
			}

			var navMeshComponent = _npc as INavMeshMoveComponent;
			if (navMeshComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
				return;
			}

			navMeshComponent.Agent.SetDestination(chaseComponent.LastKnownPlayerLocation);
		}
	}
}