using DG.Tweening;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
    public class BalloonProjectile : PhysicsProjectile
    {
        [SerializeField] private float RiseForce;
        [SerializeField] private float maxHeight;
        [SerializeField] private Vector3 MaxSize;
        [SerializeField] private float GrowthDuration;
        [SerializeField] private float ActiveWeight;

        private bool Ground;
        private Rigidbody RigidbodyRef;
        private SpringJoint Joint;
        private LineRenderer Line;
        private float DefaultSpringForce;
        private GameObject BalloonStringStart;

        protected override void Start()
        {
            base.Start();

            RigidbodyRef = GetComponent<Rigidbody>();
            Line = GetComponent<LineRenderer>();

            Joint = GetComponent<SpringJoint>();
            DefaultSpringForce = Joint.spring;
            Joint.spring = 0;
        }

        private void FixedUpdate()
        {
            if (Activated)
            {
                Rise();
                UpdateStringLine();
            }
        }

        private void Rise()
        {
            if (transform.position.y >= maxHeight) { return; }
            RigidbodyRef.AddForce(Vector3.up * RiseForce, ForceMode.Acceleration);
        }

        private void UpdateStringLine()
        {
            Line.SetPosition(0, transform.position);
            if (Ground) { return; }
            Line.SetPosition(1, BalloonStringStart.transform.position);
        }

        protected override void ImpactAction(Collision _other)
        {
            transform.DOScale(MaxSize, GrowthDuration).SetEase(Ease.OutBounce);

            Rigidbody body = _other.gameObject.GetComponent<Rigidbody>();
            RigidbodyRef.mass = ActiveWeight;

            Joint.spring = DefaultSpringForce;
            Joint.connectedBody = body;
            Joint.connectedAnchor = transform.InverseTransformPoint(_other.contacts[0].point);

            BalloonStringStart = new GameObject("BalloonStringStart");
            BalloonStringStart.transform.position = _other.contacts[0].point;
            BalloonStringStart.transform.parent = _other.gameObject.transform;
            Line.SetPosition(1, _other.contacts[0].point);
            if (_other.gameObject.tag.Equals("Ground"))
            {
                Ground = true;
            }
        }
    }
}