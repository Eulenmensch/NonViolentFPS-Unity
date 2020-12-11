using Ludiq.PeekCore;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
    [CreateAssetMenu(menuName = "Guns/PullPushGun")]
    public class PullPushGunCopy : Gun, IPrimaryFireable, ISecondaryFireable
    {
        [SerializeField] private LayerMask interactibleMask;
        [SerializeField] private float castRadius;
        [SerializeField] private float castRange;
        [SerializeField] private float pushForce;
        [SerializeField] private float rangeFallOffMultiplier;
        [SerializeField] private float projectileWiggleTime;
        [SerializeField] private float projectileDeletionRadius;

        private RaycastHit hit;
        private float wiggleTimer;

        public override void SetUpGun(ShooterCopy _shooter)
        {
            base.SetUpGun(_shooter);
            UpdateUIAmmoCount(1);

            //this gun needs a custom shooting origin
            ShootingOrigin = Visuals.ShootingOriginOverride;
        }

        public void PrimaryFireEnter()
        {
            PlayFeedbacks(Visuals.FireFeedback);
        }

        public void PrimaryFireExit()
        {
            StopFeedbacks(Visuals.FireFeedback);
        }

        public void PrimaryFireAction()
        {
            if (CheckForObject())
            {
                Push(hit.rigidbody);
            }
        }

        public void SecondaryFireEnter()
        {
            PlayFeedbacks(Visuals.SecondaryFireFeedback);
        }

        public void SecondaryFireExit()
        {
            StopFeedbacks(Visuals.SecondaryFireFeedback);
            wiggleTimer = 0;
        }

        public void SecondaryFireAction()
        {
            if (CheckForObject())
            {
                SuckUpProjectiles();
                Pull(hit.rigidbody);
            }
            DeletePhysicsProjectiles();
        }

        private bool CheckForObject()
        {
            if (UnityEngine.Physics.SphereCast(ShootingOrigin.position, castRadius, Camera.main.transform.forward, out hit, castRange, interactibleMask))
            {
                return true;
            }

            return false;
        }

        private void Push(Rigidbody _body)
        {
            var force = CalculateForce();
            _body.AddForceAtPosition(force, hit.point);
        }

        private void Pull(Rigidbody _body)
        {
            var force = CalculateForce();
            if (_body == null) return;
            _body.AddForceAtPosition(-force, hit.point );
        }

        private Vector3 CalculateForce()
        {
            var fallOffMultiplier = 1 / (1 + hit.distance * rangeFallOffMultiplier);
            var force = Camera.main.transform.forward * (pushForce * fallOffMultiplier);
            return force;
        }

        private void SuckUpProjectiles()
        {
            if (hit.collider.GetComponent<PhysicsProjectile>() != null)
            {
                if (wiggleTimer <= projectileWiggleTime)
                {
                    wiggleTimer += Time.deltaTime;
                    return;
                }
                var body = hit.collider.GetComponent<Rigidbody>();
                if (body == null)
                {
                    hit.collider.AddComponent<Rigidbody>();
                }

                var wiggler = hit.collider.GetComponent<MMWiggle>();
                if (wiggler != null)
                {
                    wiggler.PositionActive = true;
                }
            }
        }

        private void DeletePhysicsProjectiles()
        {
            var hitColliders = UnityEngine.Physics.OverlapSphere(ShootingOrigin.position, projectileDeletionRadius, interactibleMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<PhysicsProjectile>() != null)
                {
                    Destroy(hitCollider.gameObject);
                }
            }
        }
    }
}
