using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/Mood/MoodLessThanCondition")]
	public class MoodLessThanCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		[SerializeField] private float lessThanValue;
		public override bool Evaluate(NPC _npc)
		{
			var moodNPC = _npc as MoodNPC;
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return false;
			}

			return moodNPC.Mood < lessThanValue;
		}
	}
}