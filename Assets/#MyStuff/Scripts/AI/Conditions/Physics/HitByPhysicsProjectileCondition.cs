using UnityEngine;

[CreateAssetMenu(menuName = "AI Kit/Conditions/Physics/HitByPhysicsProjectileCondition")]
public class HitByPhysicsProjectileCondition : Condition
{
    public override bool Evaluate(StateMachine _stateMachine)
    {
        foreach (var collision in _stateMachine.ActiveCollisions)
        {
            if (collision.gameObject.GetComponent<PhysicsProjectile>() != null)
            {
                return true;
            }
        }
        return false;
    }
}