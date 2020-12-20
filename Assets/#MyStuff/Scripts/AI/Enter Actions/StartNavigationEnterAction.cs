using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Enter Actions/StartNavigationEnterAction" )]
    public class StartNavigationEnterAction : EnterAction
    {
        public override void Enter(NPC _npc)
        {
            var navMeshMoveComponent = _npc as INavMeshMoveComponent;
            if (navMeshMoveComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
                return;
            }

            var agent = navMeshMoveComponent.Agent;
            agent.isStopped = false;
        }
    }
}