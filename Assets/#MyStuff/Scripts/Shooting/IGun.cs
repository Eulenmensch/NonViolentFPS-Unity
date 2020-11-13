using UnityEngine.InputSystem;

public interface IGun
{
    void SetupGun(Shooter _shooter);
    void CleanUpGun();
    void PrimaryMouseButtonEnter();
    void PrimaryMouseButtonAction();
    void PrimaryMouseButtonExit();
    void SecondaryMouseButtonEnter();
    void SecondaryMouseButtonAction();
    void SecondaryMouseButtonExit();
    void ScrollWheelAction(InputAction.CallbackContext _context);
}
