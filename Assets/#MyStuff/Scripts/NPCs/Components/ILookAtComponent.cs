using UnityEngine;

namespace NonViolentFPS.Scripts.NPCs
{
	public interface ILookAtComponent
	{
		Transform LookAtTarget { get; set; }
	}
}