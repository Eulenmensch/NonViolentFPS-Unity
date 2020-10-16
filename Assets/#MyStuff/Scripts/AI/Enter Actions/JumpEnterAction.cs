using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/Enter Actions/JumpEnterAction" )]
public class JumpEnterAction : EnterAction
{
    public override void Enter(StateMachine _stateMachine)
    {
        RigidbodyStateMachine machine = _stateMachine as RigidbodyStateMachine;

        machine.Grounded = false;

        machine.RigidbodyRef.AddForce( Vector3.up * machine.JumpForce, ForceMode.VelocityChange );
    }
}