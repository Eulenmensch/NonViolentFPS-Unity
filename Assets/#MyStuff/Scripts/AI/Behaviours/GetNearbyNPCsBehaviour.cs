using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Behaviours/GetNearbyNPCsBehaviour")]
	public class GetNearbyNPCsBehaviour : AIBehaviour
	{
		public override void DoBehaviour(NPC _npc)
		{
			var otherNPCsComponent = _npc as IOtherNPCsComponent;
			if (otherNPCsComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IOtherNPCsComponent));
				return;
			}

			var rangeComponent = _npc as IRangeComponent;
			if (rangeComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IRangeComponent));
				return;
			}

			otherNPCsComponent.OtherNPCs.Clear();
			var overlaps = UnityEngine.Physics.OverlapSphere(_npc.transform.position, rangeComponent.Range);
			foreach (var overlap in overlaps)
			{
				var moodNPC = overlap.GetComponent<MoodNPC>();
				if (moodNPC != null)
				{
					otherNPCsComponent.OtherNPCs.Add(moodNPC);
				}
			}
		}
	}
}