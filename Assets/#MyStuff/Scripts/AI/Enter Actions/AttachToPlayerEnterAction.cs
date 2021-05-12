using NonViolentFPS.NPCs;
using NonViolentFPS.Player;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = nameof(AttachToPlayerEnterAction), menuName = "AI Kit/Enter Actions/AttachToPlayerEnterAction")]
	public class AttachToPlayerEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			var attachToPlayerComponent = _npc as IAttachToPlayerComponent;
			if (attachToPlayerComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IAttachToPlayerComponent));
				return;
			}

			var player = _npc.Player;
			//TODO: This is pretty ugly but also only happens one time and not in update so...
			var attachmentPoints = player.GetComponent<EnemyAttachmentPoints>().AttachmentPoints;

			//attaches the prefab to the first empty attachment point in the player's attachment points
			foreach (var attachmentPoint in attachmentPoints)
			{
				if (attachmentPoint.childCount > 0) continue;
				Instantiate(attachToPlayerComponent.prefabToAttach, attachmentPoint);
				Destroy(_npc.gameObject); //TODO: this could be handled in it's own enter action?
				break;
			}
		}
	}
}