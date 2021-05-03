using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IJumpComponent
	{
		float JumpForce { get; set; }
		Vector3 JumpDirection { get; set; }
	}
}