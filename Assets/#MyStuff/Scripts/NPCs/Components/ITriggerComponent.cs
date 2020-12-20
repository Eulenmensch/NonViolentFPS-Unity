using UnityEngine;

namespace NonViolentFPS.Scripts.NPCs
{
	public interface ITriggerComponent
	{
		bool Triggered { get; set; }
	}
}