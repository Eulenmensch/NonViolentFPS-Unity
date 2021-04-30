using NonViolentFPS.NPCs;

namespace NonViolentFPS.AI
{
	public class TrackPlayerLocationBehaviour : AIBehaviour
	{
		public override UpdateType type => UpdateType.Regular;

		public override void DoBehaviour(NPC _npc)
		{
			var chaseComponent = _npc as IChaseComponent;
			if (chaseComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IChaseComponent));
				return;
			}

			chaseComponent.LastKnownPlayerLocation = _npc.Player.transform.position;
		}
	}
}