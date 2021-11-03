using System;
using System.Collections.Generic;
using NonViolentFPS.AI;
using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public abstract class NPC : SerializedMonoBehaviour
	{
		[ShowInInspector] private State currentState;
		[SerializeField] private State startState;
		[SerializeField] private State anyState;

		public StateMachine StateMachine { get; private set; }

		//Used by most NPCs and would not make sense to encapsulate in an interface
		public List<Collision> ActiveCollisions { get; private set; }
		public GameObject Player { get; private set; }

		private void OnEnable()
		{
			GameEvents.Instance.OnPlayerLoaded += SetPlayer;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnPlayerLoaded -= SetPlayer;
		}

		protected virtual void Awake()
		{
			StateMachine = new StateMachine(this, anyState) {CurrentState = startState};
			StateMachine.TransitionToState(startState);
		}

		protected virtual void Start()
		{
			ActiveCollisions = new List<Collision>();
		}

		protected virtual void Update()
		{
			StateMachine.Update();
			currentState = StateMachine.CurrentState;
		}

		private void FixedUpdate()
		{
			StateMachine.UpdatePhysics();
		}

		private void OnCollisionEnter(Collision _other)
		{
			if (ActiveCollisions.Contains(_other)) return;
			ActiveCollisions.Add(_other);
		}

		private void OnCollisionExit(Collision _other)
		{
			if (!ActiveCollisions.Contains(_other)) return;
			ActiveCollisions.Remove(_other);
		}

		private void SetPlayer(Transform _player)
		{
			Player = _player.gameObject;
		}

		public static void ThrowComponentMissingError( Type _type)
		{
			Debug.LogError("The NPC does not implement " + _type.Name);
		}
	}
}