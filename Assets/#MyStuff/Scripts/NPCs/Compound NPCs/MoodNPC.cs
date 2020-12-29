using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.NPCs
{
	public enum Mood{Good, Neutral, Bad}

	public class MoodNPC : NPC, INavMeshMoveComponent ,IRangeComponent, ILookAtComponent, IHeadComponent, IOtherNPCsComponent
	{
		public Mood Mood { get; private set; }
		public float MoodWorseningTimer { get; set; }
		[ShowInInspector] public float MoodWorseningTime { get; private set; }

		public HashSet<NPC> OtherNPCs { get; set; }
		[ShowInInspector] public float Range { get; set; }
		[ShowInInspector] public float WanderRadius { get; set; }
		[ShowInInspector] public float PauseTime { get; set; }
		[ShowInInspector] public NavMeshAgent Agent { get; set; }
		[ShowInInspector] public Transform LookAtTarget { get; set; }
		[ShowInInspector] public Transform Head { get; set; }

		protected override void Awake()
		{
			base.Awake();
			Mood = Mood.Good;
			OtherNPCs = new HashSet<NPC>();
		}
	}
}