using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Exit Actions/HideInteractionPromptExitAction")]
	public class HideInteractionPromptExitAction : ExitAction
	{
		public override void Exit(StateMachine _stateMachine)
		{
			_stateMachine.InteractionPrompt.SetActive(false);
		}
	}
}