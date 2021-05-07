using System;
using System.Collections.Generic;
using NonViolentFPS.AI;
using NonViolentFPS.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public abstract class NPC : SerializedMonoBehaviour
	{
		[SerializeField] private State startState;
		[SerializeField] private State anyState;

		public StateMachine StateMachine { get; private set; }

		//Used by most NPCs and would not make sense to encapsulate in an interface
		public List<Collision> ActiveCollisions { get; private set; }
		public GameObject Player { get; private set; }

		protected virtual void Awake()
		{
			StateMachine = new StateMachine(this, anyState) {CurrentState = startState};
			StateMachine.TransitionToState(startState);
		}

		protected virtual void Start()
		{
			Player = GameManager.Instance.Player;
			ActiveCollisions = new List<Collision>();
		}

		private void Update()
		{
			StateMachine.Update();
		}

		private void FixedUpdate()
		{
			StateMachine.UpdatePhysics();
		}

		private void OnCollisionEnter(Collision _other)
		{
			ActiveCollisions.Add(_other);
		}

		private void OnCollisionExit(Collision _other)
		{
			ActiveCollisions.Remove(_other);
		}

		public static void ThrowComponentMissingError( Type _type)
		{
			Debug.LogError("The NPC does not implement " + _type.Name);
		}
	}
}