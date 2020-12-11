using System;
using MoreMountains.Feedbacks;
using NonViolentFPS.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
    [CreateAssetMenu(menuName = "Guns/ProjectileGun")]
    public class ProjectileGunCopy : Gun, IPrimaryFireable, IScrollwheelActionable
    {
        [BoxGroup("Settings")]
        [SerializeField] private float fireRate;
        [BoxGroup("Settings")]
        [SerializeField] private float fireForce;
        [BoxGroup("Settings")]
        [SerializeField] private GameObject[] projectileTypes;

        private Transform projectileContainer;
        private float timer;
        private GameObject activeProjectile;

        protected override void OnValidate()
        {
            base.OnValidate();
            foreach (var type in projectileTypes)
            {
                Debug.Assert(type.GetComponent<PhysicsProjectile>() != null,"The prefab you assigned has no PhysicsProjectile component." );
            }
        }

        public override void SetUpGun(ShooterCopy _shooter)
        {
            activeProjectile = projectileTypes[0];
            projectileContainer = _shooter.ProjectileContainer;

            UpdateUIAmmoCount(projectileTypes.Length);
        }

        public void PrimaryFireEnter()
        {
            timer = fireRate;
        }
        public void PrimaryFireExit() { }
        public void PrimaryFireAction()
        {
            timer += Time.deltaTime;
            if (!(timer >= fireRate)) return;
            timer = 0;
            Shoot();
            PlayFeedback();
        }

        public void ScrollWheelAction(InputAction.CallbackContext _context, bool _invertScrollDirection)
        {
            var input = _context.ReadValue<Vector2>();
            var projectileCount = projectileTypes.Length - 1;
            var currentIndex = Array.IndexOf(projectileTypes, activeProjectile);

            if(_context.started)
            {
                int direction = Mathf.RoundToInt(input.y);
                direction = _invertScrollDirection ? -direction : direction;

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
            var projectileSpace = Instantiate(activeProjectile, ShootingOrigin.position, Quaternion.identity, projectileContainer);
            var projectile = projectileSpace.GetComponentInChildren<PhysicsProjectile>();
            var rigidBody = projectile.GetComponent<Rigidbody>();

            rigidBody.AddForce(Camera.main.transform.forward * fireForce, ForceMode.VelocityChange);
        }

        private void PlayFeedback()
        {
            var feedbacks = GunInstance.GetComponentInChildren<MMFeedbacks>();
            feedbacks.PlayFeedbacks();
        }
    }
}
