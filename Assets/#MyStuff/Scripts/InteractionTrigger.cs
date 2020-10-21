using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Collider ) )]
public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] StateMachine NPCStateMachine;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag( "Player" ) )
        {
            NPCStateMachine.PlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ( other.CompareTag( "Player" ) )
        {
            NPCStateMachine.PlayerInTrigger = false;
        }
    }
}
