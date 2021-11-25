using System.Collections.Generic;
using NonViolentFPS.Shooting;
using NonViolentFPS.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace NonViolentFPS.NPCs
{
	public class GrapplerEnemyNPC : NPC,
		IChaseComponent,IDefaultLocationComponent,IRangeComponent,INavMeshMoveComponent,IRigidbodyComponent,IJumpComponent,
		IGroundCheckComponent,IHeadComponent,ITimerComponent,IParticleSpawnComponent,IAttachToPlayerComponent,IGroundRayComponent,
		IAnimatorComponent, IBubbleComponent
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
		public float Timer { get; set; }
		[field: SerializeField] public float MinTime { get; set; }
		[field: SerializeField] public float MaxTime { get; set; }
		[field: SerializeField] public Transform SpawnPoint { get; set; }
		public HashSet<GameObject> Particles { get; set; } = new HashSet<GameObject>();
		[field: SerializeField] public GameObject PrefabToAttach { get; set; }
		[field: SerializeField] public PrefabWrapper SelfPrefab { get; set; }
		[field: SerializeField] public float GroundRayLength { get; set; }
		[field: SerializeField] public Animator Animator { get; set; }
		public PhysicsProjectile AttachedBubble { get; set; }

		protected override void Start()
		{
			base.Start();
			DefaultLocation = transform.position;
		}

		protected override void Update()
		{
			base.Update();
			Animator.SetFloat("Velocity", Agent.velocity.magnitude);
		}
		#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (Camera.current != Camera.main && Camera.current != SceneView.lastActiveSceneView.camera) return;

			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(DefaultLocation, 0.3f);
		}
		#endif
	}
}