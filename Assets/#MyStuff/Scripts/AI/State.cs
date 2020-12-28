using System.Collections.Generic;
using NonViolentFPS.NPCs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/State" )]
    public class State : SerializedScriptableObject
    {
        [SerializeField] private List<AIBehaviour> behaviours;
        [SerializeField] private EnterAction[] enterActions;
        [SerializeField] private ExitAction[] exitActions;
        [SerializeField] private Transition[] transitions;

        public void UpdateState(StateMachine _stateMachine)
        {
            DoBehaviours( _stateMachine.npc );
            EvaluateConditions( _stateMachine.npc, _stateMachine );
        }

        private void DoBehaviours(NPC _npc)
        {
            foreach ( var behaviour in behaviours )
            {
                if (behaviour == null) return;
                behaviour.DoBehaviour( _npc );
            }
        }

        private void EvaluateConditions(NPC _npc, StateMachine _stateMachine)
        {
            foreach ( var transition in transitions )
            {
                var conditionTrue = transition.condition.Evaluate( _npc );

                _stateMachine.TransitionToState( conditionTrue ? transition.trueState : transition.falseState );
            }
        }

        public void Enter(NPC _npc)
        {
            foreach ( var enterAction in enterActions )
            {
                enterAction.Enter( _npc );
            }
        }

        public void Exit(NPC _npc)
        {
            foreach ( var exitAction in exitActions )
            {
                exitAction.Exit( _npc );
            }
        }
    }
}