using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/State" )]
public class State : SerializedScriptableObject
{
    [SerializeField] private List<AIBehaviour> behaviours;
    [SerializeField] private EnterAction[] enterActions;
    [SerializeField] private ExitAction[] exitActions;
    [SerializeField] private Transition[] transitions;

    public void UpdateState(StateMachine _stateMachine)
    {
        DoBehaviours( _stateMachine );
        EvaluateConditions( _stateMachine );
    }

    private void DoBehaviours(StateMachine _stateMachine)
    {
        foreach ( var behaviour in behaviours )
        {
            behaviour.DoBehaviour( _stateMachine );
        }
    }

    private void EvaluateConditions(StateMachine _stateMachine)
    {
        foreach ( var transition in transitions )
        {
            var conditionTrue = transition.condition.Evaluate( _stateMachine );

            _stateMachine.TransitionToState( conditionTrue ? transition.trueState : transition.falseState );
        }
    }

    public void Enter(StateMachine _stateMachine)
    {
        foreach ( var enterAction in enterActions )
        {
            enterAction.Enter( _stateMachine );
        }
    }

    public void Exit(StateMachine _stateMachine)
    {
        foreach ( var exitAction in exitActions )
        {
            exitAction.Exit( _stateMachine );
        }
    }
}