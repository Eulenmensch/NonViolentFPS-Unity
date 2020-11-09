using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] guns;

    public IGun ActiveGun { get; set; }
    private bool primaryActive;
    private bool secondaryActive;

    private void OnValidate()
    {
        foreach (var gun in guns)
        {
            Debug.Assert(gun is IGun, "The component you assigned does not implement IGun.");
        }
    }

    private void Start()
    {
        ActiveGun = guns[0] as IGun;
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

    public void GetMouseWheelInput(InputAction.CallbackContext _context)
    {
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
                        ActiveGun = guns[0] as IGun;
                    }
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    if (guns[1] != null)
                    {
                        ActiveGun = guns[1] as IGun;
                    }
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    if (guns[2] != null)
                    {
                        ActiveGun = guns[2] as IGun;
                    }
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    if (guns[3] != null)
                    {
                        ActiveGun = guns[3] as IGun;
                    }
                    break;
            }
        }
    }
}
