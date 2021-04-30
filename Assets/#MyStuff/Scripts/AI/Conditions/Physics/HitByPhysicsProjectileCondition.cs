using NonViolentFPS.NPCs;
using NonViolentFPS.Shooting;
using UnityEngine;

namespace NonViolentFPS.AI
{
    [CreateAssetMenu(menuName = "AI Kit/Conditions/Physics/HitByPhysicsProjectileCondition")]
    public class HitByPhysicsProjectileCondition : Condition
    {
        public override UpdateType type => UpdateType.Physics;

        public override bool Evaluate(NPC _npc)
        {
            foreach (var collision in _npc.ActiveCollisions)
            {
                if (collision.gameObject.GetComponent<PhysicsProjectile>() != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}