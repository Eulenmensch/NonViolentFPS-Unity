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
            DialogueManager.Instance.YarnUI.dialogueContainer = _stateMachine.DialogueContainer;
            if ( !DialogueManager.Instance.YarnRunner.yarnScripts.Contains( _stateMachine.YarnDialogue ) )
            {
                DialogueManager.Instance.YarnRunner.Add( _stateMachine.YarnDialogue );
            }
            DialogueManager.Instance.YarnRunner.StartDialogue( _stateMachine.StartNode );
        }
    }
}