using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI.Physics
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/JumpBehaviour" )]
    public class JumpBehaviour : AIBehaviour
    {
        public override UpdateType type => UpdateType.Physics;

        public override void DoBehaviour(NPC _npc)
        {
            var jumpComponent = _npc as IJumpComponent;
            if (jumpComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IJumpComponent));
                return;
            }
            var rigidBodyComponent = _npc as IRigidbodyComponent;
            if (rigidBodyComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
                return;
            }

            var force = jumpComponent.JumpDirection * jumpComponent.JumpForce;
            rigidBodyComponent.RigidbodyRef.AddForce( force, ForceMode.VelocityChange );
        }
    }
}