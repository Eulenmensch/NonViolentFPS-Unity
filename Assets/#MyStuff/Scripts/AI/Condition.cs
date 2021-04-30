using NonViolentFPS.NPCs;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NonViolentFPS.AI
{
    public abstract class Condition : ScriptableObject
    {
        public abstract UpdateType type { get; }
        public abstract bool Evaluate(NPC _npc);
    }
}