using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IDialogueComponent
	{
		Transform CanvasAttachmentPoint {get; set;}
		YarnProgram YarnDialogue {get; set;}
		string StartNode {get; set;}
	}
}