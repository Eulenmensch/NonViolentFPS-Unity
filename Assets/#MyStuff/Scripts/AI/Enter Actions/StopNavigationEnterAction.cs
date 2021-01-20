using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Enter Actions/StopNavigationEnterAction" )]
    public class StopNavigationEnterAction : EnterAction
    {
        public override void Enter(NPC _npc)
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

            moodNPC.StopCoroutine(moodNPC.SetDestinationRoutine);
        }
    }
}