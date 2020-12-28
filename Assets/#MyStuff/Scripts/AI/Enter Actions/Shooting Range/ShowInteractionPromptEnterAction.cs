using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/ShowInteractionPromptEnterAction")]
	public class ShowInteractionPromptEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			var interactionComponent = _npc as IInteractionComponent;
			if (interactionComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IInteractionComponent));
				return;
			}

			interactionComponent.InteractionPrompt.SetActive(true);
		}
	}
}