using NonViolentFPS.AI;
using UnityEngine;

namespace NonViolentFPS.Interactions
{
    [RequireComponent( typeof( Collider ) )]
    public class InteractionTrigger : MonoBehaviour
    {
        [SerializeField] private StateMachine npcStateMachine;

        private void OnTriggerEnter(Collider other)
        {
            if ( other.CompareTag( "Player" ) )
            {
                npcStateMachine.PlayerInTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ( other.CompareTag( "Player" ) )
            {
                npcStateMachine.PlayerInTrigger = false;
            }
        }
    }
}
