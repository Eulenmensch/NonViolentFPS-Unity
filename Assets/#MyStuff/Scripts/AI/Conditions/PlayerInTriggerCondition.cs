using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Conditions/PlayerInTriggerCondition" )]
    public class PlayerInTriggerCondition : Condition
    {
        public override bool Evaluate(StateMachine _stateMachine)
        {
            if ( _stateMachine.PlayerInTrigger )
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