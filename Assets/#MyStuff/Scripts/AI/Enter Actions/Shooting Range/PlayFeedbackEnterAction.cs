using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/PlayFeedbackEnterAction")]
	public class PlayFeedbackEnterAction : EnterAction
	{
		public override void Enter(StateMachine _stateMachine)
		{
			PlayFeedback(_stateMachine);
		}

		private void PlayFeedback(StateMachine _stateMachine)
		{
			_stateMachine.MMFeedbacks.PlayFeedbacks();
		}
	}
}
