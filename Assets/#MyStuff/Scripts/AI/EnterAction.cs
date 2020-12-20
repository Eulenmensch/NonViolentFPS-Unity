using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    public abstract class EnterAction : ScriptableObject
    {
        public abstract void Enter(NPC _npc);
    }
}