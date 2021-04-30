using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/Mood/MoodGreaterThanCondition")]
	public class MoodGreaterThanCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		[SerializeField] private float greaterThanValue;

		public override bool Evaluate(NPC _npc)
		{
			var moodNPC = _npc as MoodNPC;
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return false;
			}

			return moodNPC.Mood > greaterThanValue;
		}
	}
}