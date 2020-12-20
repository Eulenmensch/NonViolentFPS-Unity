using UnityEngine.AI;

namespace NonViolentFPS.Scripts.NPCs
{
	public interface INavMeshMoveComponent
	{
		float WanderRadius { get; set; }
		float PauseTime { get; set; }
		NavMeshAgent Agent { get; set; }
	}
}