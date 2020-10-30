using UnityEngine;
using UnityEngine.InputSystem;

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
        activeGun.ScrollWheelAction(_context);
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
                        activeGun = guns[0] as IGun;
                    }
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    if (guns[1] != null)
                    {
                        activeGun = guns[1] as IGun;
                    }
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    if (guns[2] != null)
                    {
                        activeGun = guns[2] as IGun;
                    }
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    if (guns[3] != null)
                    {
                        activeGun = guns[3] as IGun;
                    }
                    break;
            }
        }
    }
}
