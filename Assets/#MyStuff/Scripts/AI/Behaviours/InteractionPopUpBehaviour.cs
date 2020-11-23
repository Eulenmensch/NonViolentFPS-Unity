using UnityEngine;

[CreateAssetMenu( menuName = "AI Kit/Behaviours/InteractionPopUpBehaviour" )]
public class InteractionPopUpBehaviour : Behaviour
{
    public override void DoBehaviour(StateMachine _stateMachine)
    {
        if ( Input.GetKeyDown( KeyCode.E ) )
        {
            _stateMachine.StartDialogue(_stateMachine.StartNode);
        }
    }
}