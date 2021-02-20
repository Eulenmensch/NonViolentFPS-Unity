using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/DisableNavmeshAgentEnterAction")]
	public class DisabeNavmeshAgentEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			var navmeshMoveComponent = _npc as INavMeshMoveComponent;
			if (navmeshMoveComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
				return;
			}

			DisableNavMeshAgent(navmeshMoveComponent);
		}

		private static void DisableNavMeshAgent(INavMeshMoveComponent _navMeshMoveComponent)
		{
			var navMeshAgent = _navMeshMoveComponent.Agent;
			if (navMeshAgent == null) return;
			navMeshAgent.enabled = false;
		}
	}
}
