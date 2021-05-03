using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Enter Actions/JumpEnterAction" )]
    public class JumpEnterAction : EnterAction
    {
        public override void Enter(NPC _npc)
        {
            var groundCheckComponent = _npc as IGroundCheckComponent;
            if (groundCheckComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IGroundCheckComponent));
                return;
            }
            var rigidbodyComponent = _npc as IRigidbodyComponent;
            if (rigidbodyComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
                return;
            }
            var jumpComponent = _npc as IJumpComponent;
            if (jumpComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IJumpComponent));
                return;
            }

            groundCheckComponent.Grounded = false;
            rigidbodyComponent.RigidbodyRef.AddForce( jumpComponent.JumpDirection * jumpComponent.JumpForce, ForceMode.VelocityChange );
        }
    }
}