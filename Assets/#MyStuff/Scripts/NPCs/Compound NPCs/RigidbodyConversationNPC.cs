using MoreMountains.Feedbacks;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public class RigidbodyConversationNPC : NPC,
		IRigidbodyComponent,
		IDialogueComponent,
		IFeedbacksComponent,
		IRangeComponent,
		ITriggerComponent,
		ILookAtComponent,
		IHeadComponent,
		IInteractionComponent
	{
		[field: SerializeField] public Rigidbody RigidbodyRef { get; set; }
		[field: SerializeField] public Transform CanvasAttachmentPoint { get; set; }
		[field: SerializeField] public YarnProgram YarnDialogue { get; set; }
		[field: SerializeField] public string StartNode { get; set; }
		[field: SerializeField] public MMFeedbacks MMFeedbacks { get; set; }
		[field: SerializeField] public float Range { get; set; }
		public bool Triggered { get; set; }
		[field: SerializeField] public Transform LookAtTarget { get; set; }
		[field: SerializeField] public Transform Head { get; set; }
		[field: SerializeField] public GameObject InteractionPrompt { get; set; }
	}
}