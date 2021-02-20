using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Enter Actions/ActivateRigidbodyEnterAction")]
	public class ActivateRigidbodyEnterAction : EnterAction
	{
		public override void Enter(NPC _npc)
		{
			var rigidbodyComponent = _npc as IRigidbodyComponent;
			if (rigidbodyComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
				return;
			}

			EnableRigidbody(rigidbodyComponent);
		}

		private void EnableRigidbody(IRigidbodyComponent _rigidbodyComponent)
		{
			_rigidbodyComponent.RigidbodyRef.isKinematic = false;
		}
	}
}
