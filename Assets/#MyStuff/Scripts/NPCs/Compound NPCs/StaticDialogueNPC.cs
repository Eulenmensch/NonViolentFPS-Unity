using MoreMountains.Feedbacks;
using NonViolentFPS.Manager;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public class StaticDialogueNPC: NPC,
		IDialogueComponent,
		IFeedbacksComponent,
		IRangeComponent,
		IInteractionComponent,
		ITriggerComponent,
		ILookAtComponent,
		IHeadComponent
	{
		[field: SerializeField] public Transform CanvasAttachmentPoint { get; set; }
		[field: SerializeField] public YarnProgram YarnDialogue { get; set; }
		[field: SerializeField] public string StartNode { get; set; }
		[field: SerializeField] public MMFeedbacks MMFeedbacks { get; set; }
		[field: SerializeField] public float Range { get; set; }
		[field: SerializeField] public GameObject InteractionPrompt { get; set; }
		public bool Triggered { get; set; }
		[field: SerializeField] public Transform Head { get; set; }
		[field: SerializeField] public Transform LookAtTarget { get; set; }

		public void StartDialogue()
		{
			DialogueManager.Instance.StartDialogue(YarnDialogue, StartNode, CanvasAttachmentPoint);
		}
	}
}