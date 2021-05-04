using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.NPCs
{
	public class GrapplerEnemyNPC : NPC,
		IChaseComponent,IDefaultLocationComponent,IRangeComponent,INavMeshMoveComponent,IRigidbodyComponent,IJumpComponent,IGroundCheckComponent,IHeadComponent
	{
		public Vector3 LastKnownPlayerLocation { get; set; }
		public Vector3 DefaultLocation { get; set; }
		[field: SerializeField] public float BufferRadiusMin { get; set; }
		[field: SerializeField] public float BufferRadiusMax { get; set; }
		[field: SerializeField] public float Range { get; set; }
		[field: SerializeField] public float WanderRadius { get; set; }
		[field: SerializeField] public float PauseTime { get; set; }
		[field: SerializeField] public NavMeshAgent Agent { get; set; }
		[field: SerializeField] public Rigidbody RigidbodyRef { get; set; }
		[field: SerializeField] public float JumpForce { get; set; }
		[field: SerializeField] public Vector3 JumpDirection { get; set; }
		public bool Grounded { get; set; }
		[field: SerializeField] public Transform Head { get; set; }

		protected override void Start()
		{
			base.Start();
			DefaultLocation = transform.position;
		}
	}
}