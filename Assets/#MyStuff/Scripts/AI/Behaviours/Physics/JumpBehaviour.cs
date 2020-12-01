using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/JumpBehaviour" )]
public class JumpBehaviour : AIBehaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        // RigidbodyStateMachine machine = _stateMachine as RigidbodyStateMachine;
        // machine.RigidbodyRef.AddForce( Vector3.up * machine.JumpForce, ForceMode.VelocityChange );
    }
}