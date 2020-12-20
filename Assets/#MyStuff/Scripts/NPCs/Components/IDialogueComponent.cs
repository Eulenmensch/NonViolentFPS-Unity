using UnityEngine;

namespace NonViolentFPS.Scripts.NPCs
{
	public interface IDialogueComponent
	{
		Transform CanvasAttachmentPoint {get; set;}
		YarnProgram YarnDialogue {get; set;}
		string StartNode {get; set;}
	}
}