using System;
using MoreMountains.Feedbacks;
using NonViolentFPS.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
    [CreateAssetMenu(menuName = "Guns/ProjectileGun")]
    public class ProjectileGun : ScriptableObject, IGun
    {
        [Header("Visuals")]
        [SerializeField] private GameObject gun;

        [Header("Gun Settings")]
        [SerializeField] private float fireRate;
        [SerializeField] private float fireForce;
        [SerializeField] private GameObject[] projectileTypes;
        [SerializeField] private bool invertScrollDirection;

        private Transform projectileSpawnPoint;
        private Transform projectileContainer;
        private float timer;
        private GameObject activeProjectile;
        private GameObject gunObject;

        private void OnValidate()
        {
            Debug.Assert(gun.GetComponentInChildren<MMFeedbacks>(), "The object you assigned has no MMFeedbacks component on any of its children.");
        }

        public void SetupGun(Shooter _shooter)
        {
            activeProjectile = projectileTypes[0];
            projectileSpawnPoint = _shooter.ShootingOrigin;
            projectileContainer = _shooter.ProjectileContainer;

            var attachmentPoint = _shooter.GunAttachmentPoint;
            gunObject = Instantiate(gun, attachmentPoint.position, Quaternion.identity, attachmentPoint);
            gunObject.transform.localRotation = Quaternion.identity;

            PlayerEvents.Instance.UpdateGunStats(projectileTypes.Length);
        }

        public void CleanUpGun()
        {
            Destroy(gunObject);
        }

        public void PrimaryMouseButtonEnter()
        {
            timer = fireRate;
        }
        public void PrimaryMouseButtonExit() { }
        public void PrimaryMouseButtonAction()
        {
            timer += Time.deltaTime;
            if (!(timer >= fireRate)) return;
            timer = 0;
            Shoot();
            PlayFeedback();
        }

        public void SecondaryMouseButtonEnter() { }
        public void SecondaryMouseButtonExit() { }
        public void SecondaryMouseButtonAction() { }

        public void ScrollWheelAction(InputAction.CallbackContext _context)
        {
            var input = _context.ReadValue<Vector2>();
            var projectileCount = projectileTypes.Length - 1;
            var currentIndex = Array.IndexOf(projectileTypes, activeProjectile);

            if(_context.started)
            {
                int direction = Mathf.RoundToInt(input.y);
                direction = invertScrollDirection ? -direction : direction;

                if (currentIndex < projectileCount && currentIndex > 0)
                {
                    activeProjectile = projectileTypes[currentIndex + direction];
                }
                else if (currentIndex == projectileCount)
                {
                    if (direction > 0)
                    {
                        activeProjectile = projectileTypes[0];
                    }
                    else if (direction < 0)
                    {
                        activeProjectile = projectileTypes[currentIndex + direction];
                    }
                }
                else if (currentIndex == 0)
                {
                    if (direction < 0)
                    {
                        activeProjectile = projectileTypes[projectileCount];
                    }
                    else if (direction > 0)
                    {
                        activeProjectile = projectileTypes[1];
                    }
                }
            }

            PlayerEvents.Instance.ChangeAmmo(currentIndex);
        }

        private void Shoot()
        {
            var projectileSpace = Instantiate(activeProjectile, projectileSpawnPoint.position, Quaternion.identity, projectileContainer);
            var projectile = projectileSpace.GetComponentInChildren<PhysicsProjectile>();
            var rigidBody = projectile.GetComponent<Rigidbody>();

            rigidBody.AddForce(Camera.main.transform.forward * fireForce, ForceMode.VelocityChange);
        }

        private void PlayFeedback()
        {
            var feedbacks = gunObject.GetComponentInChildren<MMFeedbacks>();
            feedbacks.PlayFeedbacks();
        }
    }
}
