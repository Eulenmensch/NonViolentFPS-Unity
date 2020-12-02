using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Conditions/Physics/GroundedCondition" )]
    public class GroundedCondition : Condition
    {
        public override bool Evaluate(StateMachine _stateMachine)
        {
            RigidbodyStateMachine machine = _stateMachine as RigidbodyStateMachine;
            if ( machine.Grounded )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}