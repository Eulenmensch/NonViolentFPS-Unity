using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IRigidbodyComponent
	{
		Rigidbody RigidbodyRef { get; set; }
	}
}