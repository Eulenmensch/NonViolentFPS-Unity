using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu( menuName = "AI Kit/Enter Actions/StopNavigationEnterAction" )]
public class StopNavigationEnterAction : EnterAction
{
    public override void Enter(StateMachine _stateMachine)
    {
        NavMeshAgentStateMachine machine = _stateMachine as NavMeshAgentStateMachine;

        NavMeshAgent agent = machine.Agent;

        agent.isStopped = true;
    }
}