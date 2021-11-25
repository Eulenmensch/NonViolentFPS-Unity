using NonViolentFPS.Shooting;

namespace NonViolentFPS.NPCs
{
	public interface IBubbleComponent
	{
		PhysicsProjectile AttachedBubble { get; set; }
	}
}