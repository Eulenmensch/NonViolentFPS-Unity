using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(fileName = nameof(DeactivateRigidbodyExitAction), menuName = "AI Kit/Exit Actions/DeactivateRigidbodyExitAction")]
	public class DeactivateRigidbodyExitAction : ExitAction
	{
		public override void Exit(NPC _npc)
		{
			var rigidbodyComponent = _npc as IRigidbodyComponent;
			if (rigidbodyComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
				return;
			}

			rigidbodyComponent.RigidbodyRef.isKinematic = true;
		}
	}
}