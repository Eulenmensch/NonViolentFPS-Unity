using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Conditions/CollidedWithPlayerCondition")]
	public class CollidedWithPlayerCondition : Condition
	{
		public override UpdateType type => UpdateType.Physics;

		public override bool Evaluate(NPC _npc)
		{
			foreach (var collision in _npc.ActiveCollisions)
			{
				if (collision.gameObject == _npc.Player)
				{
					return true;
				}
			}
			return false;
		}
	}
}