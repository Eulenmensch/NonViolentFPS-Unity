using System.Threading.Tasks;
using NonViolentFPS.Extension_Classes;
using NonViolentFPS.NPCs;
using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/WanderBehaviour" )]
    public class WanderBehaviour : AIBehaviour
    {
        private System.Threading.Timer timer;

        public override UpdateType type => UpdateType.Regular;

        public override async void DoBehaviour(NPC _npc)
        {
            var navMeshMoveComponent = _npc as INavMeshMoveComponent;
            if (navMeshMoveComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(INavMeshMoveComponent));
                return;
            }


            // var timer = 0f;
            // while (timer <= navMeshMoveComponent.PauseTime)
            // {
            //     timer += Time.deltaTime;
            //     await Task.Yield();
            // }
            // timer = 0;
        }
    }
}