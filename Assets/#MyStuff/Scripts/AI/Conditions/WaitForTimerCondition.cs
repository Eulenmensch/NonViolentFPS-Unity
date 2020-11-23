using UnityEngine;

[CreateAssetMenu(menuName = "AI Kit/Conditions/WaitForTimerCondition")]
public class WaitForTimerCondition : Condition
{
	public override bool Evaluate(StateMachine _stateMachine)
	{
		if (_stateMachine.Waiting)
		{
			return false;
		}

		return true;
	}
}
