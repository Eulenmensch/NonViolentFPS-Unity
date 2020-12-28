using NonViolentFPS.NPCs;
using NonViolentFPS.Shooting;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/HitByHitscanEffectCondition")]
	public class HitByHitscanEffectCondition : Condition
	{
		public override bool Evaluate(NPC _npc)
		{
			if (_npc.gameObject.GetComponentInChildren<IHitscanEffect>() != null)
			{
				return true;
			}

			return false;
		}
	}
}
