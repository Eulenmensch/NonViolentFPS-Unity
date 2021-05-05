using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/WaitForTimerCondition")]
	public class WaitForTimerCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		public override bool Evaluate(NPC _npc)
		{
			var timerComponent = _npc as ITimerComponent;
			if (timerComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(ITimerComponent));
				return false;
			}

			var time = Random.Range(timerComponent.MinTime, timerComponent.MaxTime);

			timerComponent.Timer += Time.deltaTime;

			if (timerComponent.Timer >= time)
			{
				timerComponent.Timer = 0;
				return true;
			}

			return false;
		}
	}
}
