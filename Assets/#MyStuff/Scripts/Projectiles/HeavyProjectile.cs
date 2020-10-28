using UnityEngine;
using DG.Tweening;

public class HeavyProjectile : PhysicsProjectile
{
    [SerializeField] private Vector3 MaxSize;
    [SerializeField] private float ActiveWeight;
    [SerializeField] private float GrowthDuration;

    private Rigidbody RigidbodyRef;
    private FixedJoint Joint;
    private Rigidbody OtherRigidbody;

    protected override void Start()
    {
        base.Start();
        RigidbodyRef = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if ( Activated )
        {
            if ( OtherRigidbody != null )
            {
                OtherRigidbody.AddForceAtPosition( Vector3.down * ActiveWeight, transform.position, ForceMode.Acceleration );
                // OtherRigidbody.AddForce(Vector3.down * ActiveWeight, ForceMode.Acceleration);
            }
        }
    }

    protected override void ImpactAction(Collision other)
    {
        RigidbodyRef.isKinematic = true;
        Destroy( RigidbodyRef );
        transform.parent = other.transform.root; //FIXME: this has to be fixed so that physics objects can exist as children of other game objects!
        transform.DOScale( MaxSize, GrowthDuration ).SetEase( Ease.OutBounce );
        OtherRigidbody = other.transform.root.gameObject.GetComponent<Rigidbody>();
        // RigidbodyRef.mass = ActiveWeight;
        // Joint = gameObject.AddComponent<FixedJoint>();
        // Joint.connectedBody = OtherRigidbody;
        // Joint.connectedAnchor = transform.InverseTransformPoint(other.contacts[0].point);
    }
}