using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
    public class ShooterCopy : SerializedMonoBehaviour
    {
        [SerializeField] private List<Gun> guns;
        [SerializeField] private Transform shootingOrigin;
        [SerializeField] private Transform gunAttachmentPoint;
        [SerializeField] private Transform projectileContainer;
        [SerializeField] private bool invertScrollDirection;

        public Transform ShootingOrigin => shootingOrigin;
        public Transform GunAttachmentPoint => gunAttachmentPoint;
        public Transform ProjectileContainer => projectileContainer;

        private Gun activeGun;
        private bool primaryActive;
        private bool secondaryActive;

        private void Start()
        {
            ActivateGun(guns[0]);
        }

        private void Update()
        {
            if (primaryActive)
            {
                var primaryFireable = activeGun as IPrimaryFireable;
                primaryFireable?.PrimaryFireAction();
            }
            else if (secondaryActive)
            {
                var secondaryFireable = activeGun as ISecondaryFireable;
                secondaryFireable?.SecondaryFireAction();
            }
        }

        public void GetPrimaryFireInput(InputAction.CallbackContext _context)
        {
            if (_context.started)
            {
                var primaryFireable = activeGun as IPrimaryFireable;
                primaryFireable?.PrimaryFireEnter();
                primaryActive = true;
            }

            if (_context.canceled)
            {
                var primaryFireable = activeGun as IPrimaryFireable;
                primaryFireable?.PrimaryFireExit();
                primaryActive = false;
            }
        }

        public void GetSecondaryFireInput(InputAction.CallbackContext _context)
        {
            if (_context.started)
            {
                var secondaryFireable = activeGun as ISecondaryFireable;
                secondaryFireable?.SecondaryFireEnter();
                secondaryActive = true;
            }

            if (_context.canceled)
            {
                var secondaryFireable = activeGun as ISecondaryFireable;
                secondaryFireable?.SecondaryFireExit();
                secondaryActive = false;
            }
        }

        public void GetReloadInput(InputAction.CallbackContext _context)
        {
            if (_context.started)
            {
                var reloadable = activeGun as IReloadable;
                reloadable?.Reload();
            }
        }

        public void ActivateGun(Gun _gun)
        {
            if(activeGun != null)
            {
                activeGun.CleanUpGun();
            }
            activeGun = _gun;
            _gun.SetUpGun(this);
        }

        public void GetMouseWheelInput(InputAction.CallbackContext _context)
        {
            var scrollWheelAction = activeGun as IScrollwheelActionable;
            scrollWheelAction?.ScrollWheelAction(_context, invertScrollDirection);
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
                            ActivateGun(guns[0]);
                        }
                        break;
                    case Vector2 v when v.Equals(Vector2.left):
                        if (guns[1] != null)
                        {
                            ActivateGun(guns[1]);
                        }
                        break;
                    case Vector2 v when v.Equals(Vector2.right):
                        if (guns[2] != null)
                        {
                            ActivateGun(guns[2]);
                        }
                        break;
                    case Vector2 v when v.Equals(Vector2.down):
                        if (guns[3] != null)
                        {
                            ActivateGun(guns[3]);
                        }
                        break;
                }
            }
        }
    }
}
