using UnityEngine;

[CreateAssetMenu(menuName = "AI Kit/Conditions/WaitForTimerCondition")]
public class WaitForTimerCondition : Condition
{
	public override bool Evaluate(StateMachine _stateMachine)
	{
		if (_stateMachine.Waiting)
		{
			Debug.Log("waiting for coroutine");
			return false;
		}

		Debug.Log("transition to next state");
		return true;
	}
}
