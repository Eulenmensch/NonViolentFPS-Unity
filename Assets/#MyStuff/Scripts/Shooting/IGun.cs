using UnityEngine;

public interface IGun
{
    void PrimaryMouseButtonEnter();
    void PrimaryMouseButtonAction();
    void PrimaryMouseButtonExit();
    void SecondaryMouseButtonEnter();
    void SecondaryMouseButtonAction();
    void SecondaryMouseButtonExit();
    void ScrollWheelAction(Vector2 _direction);
}
