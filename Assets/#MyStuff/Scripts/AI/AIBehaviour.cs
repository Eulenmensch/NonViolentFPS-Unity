using NonViolentFPS.Scripts.NPCs;
using Sirenix.OdinInspector;

namespace NonViolentFPS.AI
{
    public abstract class AIBehaviour : SerializedScriptableObject
    {
        public abstract void DoBehaviour(NPC _npc);
    }
}
