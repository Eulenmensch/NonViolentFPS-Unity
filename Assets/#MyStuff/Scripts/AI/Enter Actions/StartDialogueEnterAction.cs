using NonViolentFPS.Manager;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = "Start_DialogueEnterAction", menuName = "AI Kit/Enter Actions/StartDialogueEnterAction")]
	public class StartDialogueEnterAction : EnterAction
	{
		[SerializeField] private YarnProgram yarnDialogue;
		[SerializeField] private string startNode;

		public override void Enter(NPC _npc)
		{
			var dialogueComponent = _npc as IDialogueComponent;
			if (dialogueComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IDialogueComponent));
				return;
			}

			DialogueManager.Instance.StartDialogue(yarnDialogue, startNode, dialogueComponent.CanvasAttachmentPoint);
		}
	}
}