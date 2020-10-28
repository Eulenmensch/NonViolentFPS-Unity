using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent( typeof( Collider ) )]
public class InteractionTrigger : MonoBehaviour
{
    [FormerlySerializedAs("NPCStateMachine")] [SerializeField] private StateMachine npcStateMachine;

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
