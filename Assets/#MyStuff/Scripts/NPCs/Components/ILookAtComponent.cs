using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface ILookAtComponent
	{
		Transform LookAtTarget { get; set; }
	}
}