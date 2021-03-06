using MoreMountains.Feedbacks;
using NonViolentFPS.Physics;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
    [RequireComponent(typeof(QuadraticDrag))]
    public abstract class PhysicsProjectile : MonoBehaviour
    {
        [SerializeField] private float activeDrag;
        public float ActiveDrag
        {
            get => activeDrag;
            set => activeDrag = value;
        }
        [SerializeField] private float activeAngularDrag;
        public float ActiveAngularDrag
        {
            get => activeAngularDrag;
            set => activeAngularDrag = value;
        }

        [SerializeField] private MMFeedbacks mMFeedbacks;
        public MMFeedbacks MMFeedbacks
        {
            get => mMFeedbacks;
            set => mMFeedbacks = value;
        }

        [SerializeField] protected bool doesImpactWithPlayer;


        protected bool Activated { get; set; }

        private QuadraticDrag Drag { get; set; }
        protected Rigidbody OtherRigidbody { get; set; }

        protected virtual void Start()
        {
            Drag = GetComponent<QuadraticDrag>();
        }

        protected abstract void ImpactAction(Collision _other);

        protected void ChildToOtherRigidbody(Collision _other)
        {
            OtherRigidbody = _other.gameObject.GetComponentInParent<Rigidbody>();
            if(OtherRigidbody != null)
            {
                transform.parent = OtherRigidbody.transform;
            }
        }

        protected void Activate()
        {
            Drag.Drag = ActiveDrag;
            Drag.AngularDrag = ActiveAngularDrag;
            Activated = true;
        }

        protected void PlayMMFeedbacks()
        {
            if (MMFeedbacks != null)
            {
                MMFeedbacks.PlayFeedbacks();
            }
        }

        private void OnCollisionEnter(Collision _other)
        {
            if (Activated) { return; }
            if (!doesImpactWithPlayer && _other.gameObject.tag.Equals("Player")) { return; }
            PlayMMFeedbacks();
            ImpactAction(_other);
            Activate();
        }
    }
}