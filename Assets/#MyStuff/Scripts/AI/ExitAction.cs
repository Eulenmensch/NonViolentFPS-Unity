using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    public abstract class ExitAction : ScriptableObject
    {
        public abstract void Exit(NPC _npc);
    }
}