namespace NonViolentFPS.Extension_Classes
{
	public static class PlayerInputExtensions
	{
		public static void SetPlayerActionsEnabled(this PlayerInput _playerInput, bool _enabled)
		{
			if(_enabled) _playerInput.Player.Enable();
			else _playerInput.Player.Disable();
		}
	}
}