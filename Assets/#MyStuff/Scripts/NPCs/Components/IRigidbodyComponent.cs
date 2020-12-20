using UnityEngine;

namespace NonViolentFPS.Scripts.NPCs
{
	public interface IRigidbodyComponent
	{
		Rigidbody RigidbodyRef { get; set; }
	}
}