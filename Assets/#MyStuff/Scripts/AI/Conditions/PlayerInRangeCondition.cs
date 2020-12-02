using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Conditions/PlayerInRangeCondition" )]
    public class PlayerInRangeCondition : Condition
    {
        public override bool Evaluate(StateMachine _stateMachine)
        {
            Vector3 playerPosition = _stateMachine.Player.transform.position;
            Vector3 selfPosition = _stateMachine.transform.position;

            if ( Vector3.Distance( playerPosition, selfPosition ) <= _stateMachine.Range )
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