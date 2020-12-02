using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/TurnBehaviour" )]
    public class TurnBehaviour : AIBehaviour
    {
        public override void DoBehaviour(StateMachine _stateMachine)
        {
            RigidbodyStateMachine machine = _stateMachine as RigidbodyStateMachine;
            machine.RigidbodyRef.AddTorque( machine.transform.up * machine.TurnTorque, ForceMode.Acceleration );
        }
    }
}