using NonViolentFPS.AI;
using NonViolentFPS.NPCs;
using UnityEngine;

namespace NonViolentFPS.Interactions
{
    [RequireComponent( typeof( Collider ) )]
    public class InteractionTrigger : MonoBehaviour
    {
        [SerializeField] private NPC npc;

        private void OnTriggerEnter(Collider other)
        {
            if ( other.CompareTag( "Player" ) )
            {
                var triggerComponent = npc as ITriggerComponent;
                triggerComponent.Triggered = true;                      //FIXME: ok so this obviously can be nulled
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ( other.CompareTag( "Player" ) )
            {
                var triggerComponent = npc as ITriggerComponent;
                triggerComponent.Triggered = false;
            }
        }
    }
}
