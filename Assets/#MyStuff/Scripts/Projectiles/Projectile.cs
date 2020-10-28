using UnityEngine;

[RequireComponent(typeof(QuadraticDrag))]
public abstract class PhysicsProjectile : MonoBehaviour
{
    [SerializeField] private float activeDrag;
    public float ActiveDrag
    {
        get { return activeDrag; }
        set { activeDrag = value; }
    }
    [SerializeField] private float activeAngularDrag;
    public float ActiveAngularDrag
    {
        get { return activeAngularDrag; }
        set { activeAngularDrag = value; }
    }

    public bool Activated { get; private set; }

    public QuadraticDrag Drag { get; set; }

    protected virtual void Start()
    {
        Drag = GetComponent<QuadraticDrag>();
    }

    protected abstract void ImpactAction(Collision other);

    private void OnCollisionEnter(Collision other)
    {
        if (Activated) { return; }
        if (other.gameObject.tag.Equals("Player")) { return; }
        ImpactAction(other);
        Drag.Drag = ActiveDrag;
        Drag.AngularDrag = ActiveAngularDrag;
        Activated = true;
    }
}