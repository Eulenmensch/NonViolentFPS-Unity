using NonViolentFPS.NPCs;
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

            var moodNPC = _npc as MoodNPC;
            if (moodNPC == null)
            {
                NPC.ThrowComponentMissingError(typeof(MoodNPC));
                return;
            }

            var agent = navMeshMoveComponent.Agent;
            agent.isStopped = false;

            moodNPC.SetDestinationRoutine = moodNPC.StartCoroutine(moodNPC.SetDestinationRepeated());
        }
    }
}