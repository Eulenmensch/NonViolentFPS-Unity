using Ludiq.PeekCore;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
    [CreateAssetMenu(menuName = "Guns/PullPushGun")]
    public class PullPushGun : ScriptableObject, IGun
    {
        [SerializeField] private LayerMask interactibleMask;
        [SerializeField] private Transform castOrigin;
        [SerializeField] private float castRadius;
        [SerializeField] private float castRange;
        [SerializeField] private float pushForce;
        [SerializeField] private float rangeFallOffMultiplier;
        [SerializeField] private ParticleSystem pushParticles;
        [SerializeField] private ParticleSystem pullParticles;
        [SerializeField] private float projectileWiggleTime;
        [SerializeField] private float projectileDeletionRadius;

        private RaycastHit hit;
        private float wiggleTimer;

        public void SetupGun(Shooter _shooter)
        {
            castOrigin = _shooter.PullOrigin;
            pushParticles = _shooter.Particles[0];
            pullParticles = _shooter.Particles[1];
            pushParticles.gameObject.SetActive(true);
            pullParticles.gameObject.SetActive(true);
        }

        public void CleanUpGun()
        {

        }

        public void PrimaryMouseButtonEnter()
        {
            pushParticles.gameObject.SetActive(true);
            pushParticles.Play();
        }

        public void PrimaryMouseButtonExit()
        {
            pushParticles.Clear();
            pushParticles.Stop();
            pushParticles.gameObject.SetActive(false);
        }

        public void PrimaryMouseButtonAction()
        {
            if (CheckForObject())
            {
                Push(hit.rigidbody);
            }
        }

        public void SecondaryMouseButtonEnter()
        {
            pullParticles.gameObject.SetActive(true);
            pullParticles.Play();
        }

        public void SecondaryMouseButtonExit()
        {
            pullParticles.Clear();
            pullParticles.Stop();
            pullParticles.gameObject.SetActive(false);

            wiggleTimer = 0;
        }

        public void SecondaryMouseButtonAction()
        {
            if (CheckForObject())
            {
                SuckUpProjectiles();
                Pull(hit.rigidbody);
            }
            DeletePhysicsProjectiles();
        }

        public void ScrollWheelAction(InputAction.CallbackContext _context) { }

        private bool CheckForObject()
        {
            if (UnityEngine.Physics.SphereCast(castOrigin.position, castRadius, Camera.main.transform.forward, out hit, castRange, interactibleMask))
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
            var force = Camera.main.transform.forward * (pushForce * (1 / (1 + hit.distance * rangeFallOffMultiplier)));
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
                    body = hit.collider.AddComponent<Rigidbody>();
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
            var hitColliders = UnityEngine.Physics.OverlapSphere(castOrigin.position, projectileDeletionRadius, interactibleMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<PhysicsProjectile>() != null)
                {
                    Destroy(hitCollider.gameObject);
                }
            }
        }
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.GetComponent<PhysicsProjectile>())
            {
                Destroy(_other.gameObject);
            }
        }
    }
}
