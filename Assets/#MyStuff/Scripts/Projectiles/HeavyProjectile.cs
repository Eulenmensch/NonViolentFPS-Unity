using UnityEngine;
using DG.Tweening;

public class HeavyProjectile : PhysicsProjectile
{
    [SerializeField] Vector3 MaxSize;
    [SerializeField] float ActiveWeight;
    [SerializeField] float GrowthDuration;

    Rigidbody RigidbodyRef;
    FixedJoint Joint;
    Rigidbody OtherRigidbody;

    protected override void Start()
    {
        base.Start();
        RigidbodyRef = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Activated)
        {
            if (OtherRigidbody != null)
            {
                OtherRigidbody.AddForceAtPosition(Vector3.down * ActiveWeight, transform.position, ForceMode.Acceleration);
                // OtherRigidbody.AddForce(Vector3.down * ActiveWeight, ForceMode.Acceleration);
            }
        }
    }

    protected override void ImpactAction(Collision other)
    {
        RigidbodyRef.isKinematic = true;
        Destroy(RigidbodyRef);
        transform.parent = other.transform.root;
        transform.DOScale(MaxSize, GrowthDuration).SetEase(Ease.OutBounce);
        OtherRigidbody = other.transform.root.gameObject.GetComponent<Rigidbody>();
        // RigidbodyRef.mass = ActiveWeight;
        // Joint = gameObject.AddComponent<FixedJoint>();
        // Joint.connectedBody = OtherRigidbody;
        // Joint.connectedAnchor = transform.InverseTransformPoint(other.contacts[0].point);
    }
}