using System.Threading.Tasks;
using NonViolentFPS.Extension_Classes;
using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/WanderBehaviour" )]
    public class WanderBehaviour : AIBehaviour
    {
        public override async void DoBehaviour(NPC _npc)
        {
            var navMeshMoveComponent = _npc as INavMeshMoveComponent;
            if (navMeshMoveComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
                return;
            }

            var agent = navMeshMoveComponent?.Agent;
            agent.SetDestination( agent.RandomPosition( navMeshMoveComponent.WanderRadius ) );
            var timer = 0f;
            while (timer <= navMeshMoveComponent?.PauseTime)
            {
                timer += Time.deltaTime;
                await Task.Yield();
            }
        }
    }
}