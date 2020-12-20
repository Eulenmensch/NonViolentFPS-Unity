using NonViolentFPS.Scripts.NPCs;
using UnityEngine;
using UnityEngine.AI;

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

            var agent = navMeshAgentMoveComponent.Agent;
            agent.isStopped = true;
        }
    }
}