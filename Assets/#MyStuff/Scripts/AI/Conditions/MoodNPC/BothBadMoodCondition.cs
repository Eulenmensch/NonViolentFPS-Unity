using NonViolentFPS.NPCs;

namespace NonViolentFPS.AI
{
	public class BothBadMoodCondition : Condition
	{
		public override bool Evaluate(NPC _npc)
		{
			var otherNPCsComponent = _npc as IOtherNPCsComponent;
			if (otherNPCsComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IOtherNPCsComponent));
				return false;
			}

			foreach (var npc in otherNPCsComponent.OtherNPCs)
			{
				var moodNPC = npc as MoodNPC;
				if (moodNPC.Mood == Mood.Bad)
				{
					return true;
				}
			}
			return false;
		}
	}
}