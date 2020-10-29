using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Shooter : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] guns;

    private IGun activeGun;
    private bool primaryActive;
    private bool secondaryActive;

    private void Start()
    {
        activeGun = guns[0] as IGun;
    }

    private void Update()
    {
        if (primaryActive)
        {
            activeGun.PrimaryMouseButtonAction();
        }
        else if (secondaryActive)
        {
            activeGun.SecondaryMouseButtonAction();
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
                activeGun.PrimaryMouseButtonEnter();
            }
            if (mouseAxis < 0)
            {
                secondaryActive = true;
                activeGun.SecondaryMouseButtonEnter();
            }
        }

        if (_context.canceled)
        {
            if (primaryActive)
            {
                primaryActive = false;
                activeGun.PrimaryMouseButtonExit();
            }

            if (secondaryActive)
            {
                secondaryActive = false;
                activeGun.SecondaryMouseButtonExit();
            }
        }
    }

    public void GetMouseWheelInput(InputAction.CallbackContext _context)
    {
        
    }
}
