using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/InteractionPopUpBehaviour" )]
public class InteractionPopUpBehaviour : AIBehaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        if ( Input.GetKeyDown( KeyCode.E ) )
        {
            _stateMachine.StartDialogue(_stateMachine.StartNode);
            if (_stateMachine.InteractionPrompt != null)
            {
                _stateMachine.InteractionPrompt.SetActive(false);
            }
        }
    }
}