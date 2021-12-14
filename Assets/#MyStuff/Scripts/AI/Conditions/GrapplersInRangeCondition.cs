using NonViolentFPS.AI;
using NonViolentFPS.Manager;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Scripts.AI.Conditions
{
	[CreateAssetMenu(fileName = "GrapplersIn_UnitsCondition", menuName = "AI Kit/Conditions/GrapplersInRangeCondition")]
	public class GrapplersInRangeCondition : Condition
	{
		[SerializeField] private float range;

		public override UpdateType type => UpdateType.Regular;
		public override bool Evaluate(NPC _npc)
		{
			foreach (var npc in NpcManager.Instance.NPCs)
			{
				if (npc is GrapplerEnemyNPC)
				{
					var distanceToNPC = Vector3.Distance(npc.transform.position, _npc.transform.position);
					if (distanceToNPC <= range) return true;
				}
			}
			return false;
		}
	}
}