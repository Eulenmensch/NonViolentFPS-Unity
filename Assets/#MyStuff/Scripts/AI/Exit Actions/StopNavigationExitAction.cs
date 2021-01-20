using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Exit Actions/StopNavigationExitAction")]
	public class StopNavigationExitAction : ExitAction
	{
		public override void Exit(NPC _npc)
		{
			var navMeshAgentMoveComponent = _npc as INavMeshMoveComponent;
			if (navMeshAgentMoveComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
				return;
			}

			var moodNPC = _npc as MoodNPC;
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return;
			}

			var agent = navMeshAgentMoveComponent.Agent;
			agent.isStopped = true;

			if(moodNPC.SetDestinationRoutine != null)
			{
				moodNPC.StopCoroutine(moodNPC.SetDestinationRoutine);
			}
		}
	}
}