using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Conditions/Physics/GroundedCondition" )]
    public class GroundedCondition : Condition
    {
        public override bool Evaluate(NPC _npc)
        {
            var groundCheckComponent = _npc as IGroundCheckComponent;
            if (groundCheckComponent == null) return false;
            if ( groundCheckComponent.Grounded )
            {
                return true;
            }

            return false;
        }
    }
}