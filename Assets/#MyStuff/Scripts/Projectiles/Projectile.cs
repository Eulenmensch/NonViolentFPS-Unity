using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public abstract void ImpactAction(Collision other);

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            return;
        }
        ImpactAction(other);
    }
}