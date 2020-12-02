using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> guns;
        [SerializeField] private Transform shootingOrigin;
        [SerializeField] private Transform gunAttachmentPoint;
        [SerializeField] private Transform projectileContainer;
        [SerializeField] private ParticleSystem[] particles;

        [SerializeField] private bool canWeaponSwap = true;      //Solely for the shooting range test scene
        [SerializeField] private Transform pullOrigin;           //hotfix for thomas testing

        public Transform ShootingOrigin => shootingOrigin;
        public Transform GunAttachmentPoint => gunAttachmentPoint;
        public Transform ProjectileContainer => projectileContainer;
        public ParticleSystem[] Particles => particles;
        public Transform PullOrigin => pullOrigin;

        public IGun ActiveGun { get; set; }
        private bool primaryActive;
        private bool secondaryActive;

        private void OnValidate()
        {
            foreach (var gun in guns)
            {
                Debug.Assert(gun is IGun, "The object you assigned does not implement IGun.");
            }
        }

        private void Start()
        {
            ActivateGun(guns[0] as IGun);
        }

        private void Update()
        {
            if (primaryActive)
            {
                ActiveGun.PrimaryMouseButtonAction();
            }
            else if (secondaryActive)
            {
                ActiveGun.SecondaryMouseButtonAction();
            }
        }

        public void GetMouseButtonInput(InputAction.CallbackContext _context)
        {
            var mouseAxis = _context.ReadValue<float>();
            if (_context.started)
            {
                if (mouseAxis > 0)
                {
                    primaryActive = true;
                    ActiveGun.PrimaryMouseButtonEnter();
                }
                if (mouseAxis < 0)
                {
                    secondaryActive = true;
                    ActiveGun.SecondaryMouseButtonEnter();
                }
            }

            if (_context.canceled)
            {
                if (primaryActive)
                {
                    primaryActive = false;
                    ActiveGun.PrimaryMouseButtonExit();
                }

                if (secondaryActive)
                {
                    secondaryActive = false;
                    ActiveGun.SecondaryMouseButtonExit();
                }
            }
        }

        public void ActivateGun(IGun _gun)
        {
            ActiveGun?.CleanUpGun();

            ActiveGun = _gun;
            _gun.SetupGun(this);
        }

        public void GetMouseWheelInput(InputAction.CallbackContext _context)
        {
            if (!canWeaponSwap) { return; }
            ActiveGun.ScrollWheelAction(_context);
        }

        public void SelectGun(InputAction.CallbackContext _context)
        {
            var inputDirection = _context.ReadValue<Vector2>();

            if (_context.started)
            {
                switch (inputDirection)
                {
                    case Vector2 v when v.Equals(Vector2.up):
                        if (guns[0] != null)
                        {
                            ActivateGun(guns[0] as IGun);
                        }
                        break;
                    case Vector2 v when v.Equals(Vector2.left):
                        if (guns[1] != null)
                        {
                            ActivateGun(guns[1] as IGun);
                        }
                        break;
                    case Vector2 v when v.Equals(Vector2.right):
                        if (guns[2] != null)
                        {
                            ActivateGun(guns[2] as IGun);
                        }
                        break;
                    case Vector2 v when v.Equals(Vector2.down):
                        if (guns[3] != null)
                        {
                            ActivateGun(guns[3] as IGun);
                        }
                        break;
                }
            }
        }
    }
}
