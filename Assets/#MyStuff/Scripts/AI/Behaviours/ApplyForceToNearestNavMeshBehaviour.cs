using NonViolentFPS.NPCs;
using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.AI
{
	[CreateAssetMenu(menuName = "AI Kit/Behaviours/ApplyForceToNearestNavMeshBehaviour")]
	public class ApplyForceToNearestNavMeshBehaviour : AIBehaviour
	{
		[SerializeField] private string areaName;
		[SerializeField] private float forceAmount;

		public override UpdateType type => UpdateType.Physics;

		public override void DoBehaviour(NPC _npc)
		{
			var rigidbodyComponent = _npc as IRigidbodyComponent;
			if (rigidbodyComponent == null)
			{
				NPC.ThrowComponentMissingError(typeof(IRigidbodyComponent));
				return;
			}

			var npcPosition = _npc.transform.position;
			NavMesh.FindClosestEdge(npcPosition, out var hit, NavMesh.AllAreas);
			var directionToNavMesh = (hit.position - npcPosition).normalized;
			var horizontalDirectionToNavMesh = new Vector3(directionToNavMesh.x, 0, directionToNavMesh.z);

			if(horizontalDirectionToNavMesh.magnitude > 0)
			{
				var npcRigidbody = rigidbodyComponent.RigidbodyRef;
				npcRigidbody.AddForce(horizontalDirectionToNavMesh * forceAmount, ForceMode.Acceleration);
			}
		}
	}
}