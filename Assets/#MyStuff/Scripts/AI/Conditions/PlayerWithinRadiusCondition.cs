using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu( menuName = "AI Kit/Conditions/PlayerWithinRadiusCondition" )]
	public class PlayerWithinRadiusCondition : Condition
	{
		public override UpdateType type => UpdateType.Regular;

		[SerializeField] private float range;

		public override bool Evaluate(NPC _npc)
		{
			var playerPosition = _npc.Player.transform.position;
			var selfPosition = _npc.transform.position;

			return Vector3.Distance(playerPosition, selfPosition) <= range;
		}
	}
}