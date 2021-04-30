using NonViolentFPS.NPCs;
using Sirenix.OdinInspector;

namespace NonViolentFPS.AI
{
    public abstract class AIBehaviour : SerializedScriptableObject
    {
        public abstract UpdateType type { get; }
        public abstract void DoBehaviour(NPC _npc);
    }
}
