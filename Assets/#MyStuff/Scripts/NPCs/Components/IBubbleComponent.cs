using NonViolentFPS.Shooting;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IBubbleComponent
	{
		PhysicsProjectile AttachedBubble { get; set; }
	}
}