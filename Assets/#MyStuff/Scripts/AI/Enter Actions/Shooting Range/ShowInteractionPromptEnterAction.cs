using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/ShowInteractionPromptEnterAction")]
	public class ShowInteractionPromptEnterAction : EnterAction
	{
		public override void Enter(StateMachine _stateMachine)
		{
			_stateMachine.InteractionPrompt.SetActive(true);
		}
	}
}