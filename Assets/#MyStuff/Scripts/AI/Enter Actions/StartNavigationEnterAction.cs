using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu( menuName = "AI Kit/Enter Actions/StartNavigationEnterAction" )]
public class StartNavigationEnterAction : EnterAction
{
    public override void Enter(StateMachine _stateMachine)
    {
        NavMeshAgentStateMachine machine = _stateMachine as NavMeshAgentStateMachine;

        NavMeshAgent agent = machine.Agent;

        agent.isStopped = false;
    }
}