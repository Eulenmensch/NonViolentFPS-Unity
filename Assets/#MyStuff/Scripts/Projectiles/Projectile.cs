using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public bool Activated { get; private set; }

    public abstract void ImpactAction(Collision other);

    private void OnCollisionEnter(Collision other)
    {
        if (Activated) { return; }
        if (other.gameObject.tag.Equals("Player")) { return; }
        ImpactAction(other);
        Activated = true;
        print("huh");
    }
}