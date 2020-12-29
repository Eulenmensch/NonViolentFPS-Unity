using NonViolentFPS.Extension_Classes;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	public class MoodBehaviour : AIBehaviour
	{
		public override void DoBehaviour(NPC _npc)
		{
			var moodNPC = _npc as MoodNPC;
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return;
			}

			moodNPC.MoodWorseningTimer += Time.deltaTime;
			if (moodNPC.MoodWorseningTimer >= moodNPC.MoodWorseningTime && moodNPC.Mood != Mood.Bad)
			{
				moodNPC.Mood.Next();
			}
		}
	}
}