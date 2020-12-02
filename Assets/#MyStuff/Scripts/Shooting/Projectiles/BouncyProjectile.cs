using DG.Tweening;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
    public class BouncyProjectile : PhysicsProjectile
    {
        [SerializeField] private Vector3 maxSize;
        [SerializeField] private float activeWeight;
        [SerializeField] private float growthDuration;

        private Rigidbody rigidbodyRef;

        protected override void Start()
        {
            base.Start();
            rigidbodyRef = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if ( Activated )
            {
                if ( OtherRigidbody != null )
                {
                    OtherRigidbody.AddForceAtPosition( Vector3.down * activeWeight, transform.position, ForceMode.Acceleration );
                }
            }
        }

        protected override void ImpactAction(Collision _other)
        {
            rigidbodyRef.isKinematic = true;
            Destroy( rigidbodyRef );

            ChildToOtherRigidbody(_other);

            transform.DOScale( maxSize, growthDuration ).SetEase( Ease.OutBounce );
        }
    }
}