using NonViolentFPS.Shooting;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/HitByHitscanEffectCondition")]
	public class HitByHitscanEffectCondition : Condition
	{
		public override bool Evaluate(StateMachine _stateMachine)
		{
			if (_stateMachine.gameObject.GetComponentInChildren<IHitscanEffect>() != null)
			{
				return true;
			}

			return false;
		}
	}
}
