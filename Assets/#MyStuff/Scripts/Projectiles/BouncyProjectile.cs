using UnityEngine;
using DG.Tweening;

public class BouncyProjectile : PhysicsProjectile
{
    [SerializeField] private Vector3 MaxSize;
    [SerializeField] private float ActiveWeight;
    [SerializeField] private float GrowthDuration;

    private Rigidbody RigidbodyRef;
    private FixedJoint Joint;
    private Collision Other;
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
            }
        }
    }

    protected override void ImpactAction(Collision other)
    {
        RigidbodyRef.isKinematic = true;
        Destroy( RigidbodyRef );
        transform.parent = other.transform.root;
        transform.DOScale( MaxSize, GrowthDuration ).SetEase( Ease.OutBounce );
        OtherRigidbody = other.transform.root.gameObject.GetComponent<Rigidbody>();
    }
}