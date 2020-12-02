using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/StartTimerEnterAction")]
	public class StartTimerEnterAction : EnterAction
	{
		[SerializeField] private float seconds;
		public override void Enter(StateMachine _stateMachine)
		{
			_stateMachine.StartCoroutine(_stateMachine.WaitForSeconds(seconds));
		}
	}
}
