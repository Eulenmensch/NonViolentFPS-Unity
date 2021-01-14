using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.NPCs
{
	public class MoodNPC : NPC, INavMeshMoveComponent ,IRangeComponent, ILookAtComponent, IHeadComponent, IOtherNPCsComponent
	{
		[SerializeField] private float startMood;
		[field: SerializeField] public float Mood { get; set; }
		[field: SerializeField] public float MoodWorseningTime { get; private set; }

		public HashSet<NPC> OtherNPCs { get; set; }
		[field: SerializeField] public float Range { get; set; }
		[field: SerializeField] public float WanderRadius { get; set; }
		[field: SerializeField] public float PauseTime { get; set; }
		[field: SerializeField] public NavMeshAgent Agent { get; set; }
		[field: SerializeField] public Transform LookAtTarget { get; set; }
		[field: SerializeField] public Transform Head { get; set; }

		protected override void Awake()
		{
			base.Awake();
			OtherNPCs = new HashSet<NPC>();
			Mood = startMood;
		}
	}
}