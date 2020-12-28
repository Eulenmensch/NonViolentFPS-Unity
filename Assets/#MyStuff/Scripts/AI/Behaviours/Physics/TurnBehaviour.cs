using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI.Physics
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/TurnBehaviour" )]
    public class TurnBehaviour : AIBehaviour
    {
        [SerializeField] private float turnTorque;
        public override void DoBehaviour(NPC _npc)
        {
            var rigidbodyComponent = _npc as IRigidbodyComponent;
            if (rigidbodyComponent == null)
            {
                NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
                return;
            }

            rigidbodyComponent.RigidbodyRef.AddTorque( _npc.transform.up * turnTorque, ForceMode.Acceleration );
        }
    }
}