using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = nameof(EnableNavMeshAgentExitAction), menuName = "AI Kit/Exit Actions/EnableNavMeshExitAction")]
	public class EnableNavMeshAgentExitAction : ExitAction
	{
		public override void Exit(NPC _npc)
		{
			var navMeshMoveComponent = _npc as INavMeshMoveComponent;
			if (navMeshMoveComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
				return;
			}

			var navMeshAgent = navMeshMoveComponent.Agent;
			if (navMeshAgent == null) return;
			navMeshAgent.enabled = true;
		}
	}
}