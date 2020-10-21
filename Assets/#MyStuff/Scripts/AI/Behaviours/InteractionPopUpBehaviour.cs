using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/InteractionPopUpBehaviour" )]
public class InteractionPopUpBehaviour : Behaviour
{
    public InputActionAsset inputActions;
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        if ( Input.GetKeyDown( KeyCode.E ) )
        {
            _stateMachine.YarnUi.dialogueContainer = _stateMachine.DialogueContainer;
            if ( !_stateMachine.YarnRunner.yarnScripts.Contains( _stateMachine.YarnDialogue ) )
            {
                _stateMachine.YarnRunner.Add( _stateMachine.YarnDialogue );
            }
            _stateMachine.YarnRunner.StartDialogue( _stateMachine.StartNode );
        }
    }
}