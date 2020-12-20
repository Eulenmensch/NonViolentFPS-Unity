using NonViolentFPS.Scripts.NPCs;
using UnityEngine;

namespace NonViolentFPS.AI
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool Evaluate(NPC _npc);
    }
}