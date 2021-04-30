using NonViolentFPS.Manager;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu( menuName = "AI Kit/Behaviours/InteractionPopUpBehaviour" )]
    public class InteractionPopUpBehaviour : AIBehaviour
    {
        public override UpdateType type => UpdateType.Regular;

        public override void DoBehaviour(NPC _npc)
        {
            if ( Input.GetKeyDown( KeyCode.E ) )
            {
                var dialogueComponent = _npc as IDialogueComponent;
                if (dialogueComponent == null)
                {
                    NPC.ThrowComponentMissingError(typeof(IDialogueComponent));
                    return;
                }
                var interactionComponent = _npc as IInteractionComponent;
                if (interactionComponent == null)
                {
                    NPC.ThrowComponentMissingError(typeof(IInteractionComponent));
                    return;
                }

                DialogueManager.Instance.StartDialogue(dialogueComponent.YarnDialogue ,dialogueComponent.StartNode, dialogueComponent.CanvasAttachmentPoint);

                if (interactionComponent.InteractionPrompt != null)
                {
                    interactionComponent.InteractionPrompt.SetActive(false);
                }
            }
        }
    }
}