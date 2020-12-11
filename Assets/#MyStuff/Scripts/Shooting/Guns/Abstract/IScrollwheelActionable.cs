using UnityEngine.InputSystem;

namespace NonViolentFPS.Shooting
{
	public interface IScrollwheelActionable
	{
		void ScrollWheelAction(InputAction.CallbackContext _context, bool _invertScrollDirection);
	}
}