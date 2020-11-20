
using UnityEngine;

[CreateAssetMenu(menuName = "AI Kit/Conditions/HitByHitscanEffectCondition")]
public class HitByHitscanEffectCondition : Condition
{
	public override bool Evaluate(StateMachine _stateMachine)
	{
		if (_stateMachine.gameObject.GetComponentInChildren<IHitscanEffect>() != null)
		{
			Debug.Log("hit by hitscan effect");
			return true;
		}

		return false;
	}
}
