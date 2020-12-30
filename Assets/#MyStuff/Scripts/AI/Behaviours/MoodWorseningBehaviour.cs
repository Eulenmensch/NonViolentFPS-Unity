using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Behaviours/Mood/MoodWorseningBehaviour")]
	public class MoodWorseningBehaviour : AIBehaviour
	{
		public override void DoBehaviour(NPC _npc)
		{
			var moodNPC = _npc as MoodNPC;
			if (moodNPC == null)
			{
				NPC.ThrowComponentMissingError(typeof(MoodNPC));
				return;
			}

			// moodNPC.MoodWorseningTimer += Time.deltaTime;
			// if (moodNPC.MoodWorseningTimer >= moodNPC.MoodWorseningTime && moodNPC.Mood != Mood.Bad)
			// {
			// 	moodNPC.Mood.Next();
			// }

			//decreases the mood by one every [MoodWorseningTime] seconds
			if(moodNPC.Mood >= 0)
			{
				moodNPC.Mood -= Time.deltaTime / moodNPC.MoodWorseningTime;
			}
		}
	}
}