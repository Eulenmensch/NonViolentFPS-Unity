using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/JumpBehaviour" )]
    public class JumpBehaviour : AIBehaviour
    {
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

            var force = Vector3.up * jumpComponent.JumpForce;
            rigidBodyComponent.RigidbodyRef.AddForce( force, ForceMode.VelocityChange );
        }
    }
}