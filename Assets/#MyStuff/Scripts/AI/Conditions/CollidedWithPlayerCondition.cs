using NonViolentFPS.NPCs;

namespace NonViolentFPS.AI
{
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