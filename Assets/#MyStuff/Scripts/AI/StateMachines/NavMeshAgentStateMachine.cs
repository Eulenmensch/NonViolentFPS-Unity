using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace NonViolentFPS.AI
{
    [RequireComponent( typeof( NavMeshAgent ) )]
    public class NavMeshAgentStateMachine : StateMachine
    {
        [SerializeField] private float wanderRadius;
        public float WanderRadius
        {
            get => wanderRadius;
            set => wanderRadius = value;
        }
        [SerializeField] private float pauseTime;
        public float PauseTime
        {
            get => pauseTime;
            set => pauseTime = value;
        }

        public NavMeshAgent Agent { get; private set; }

        public bool CoroutineRunning { get; private set; }

        protected override void Start()
        {
            base.Start();
            Agent = GetComponent<NavMeshAgent>();
        }

        public IEnumerator ContinueAfterSeconds(float _seconds)
        {
            CoroutineRunning = true;
            yield return new WaitForSeconds( _seconds );
            CoroutineRunning = false;
        }
    }
}