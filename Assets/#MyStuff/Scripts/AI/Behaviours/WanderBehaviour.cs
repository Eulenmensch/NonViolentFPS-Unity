using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/WanderBehaviour" )]
public class WanderBehaviour : Behaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        NavMeshAgentStateMachine machine = _stateMachine as NavMeshAgentStateMachine;

        NavMeshAgent agent = machine.Agent;
        if ( !machine.CoroutineRunning )
            agent.SetDestination( agent.RandomPosition( machine.WanderRadius ) );
        if ( !machine.CoroutineRunning )
            machine.StartCoroutine( machine.ContinueAfterSeconds( machine.PauseTime ) );
    }
}