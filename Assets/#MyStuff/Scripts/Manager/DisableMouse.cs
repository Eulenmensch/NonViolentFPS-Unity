using NonViolentFPS.Events;
using UnityEngine;

namespace NonViolentFPS.Manager
{
    public class DisableMouse : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEvents.Instance.OnGameLost += UnlockCursor;
            GameEvents.Instance.OnGameWon += UnlockCursor;
            GameEvents.Instance.OnGameRestarted += LockCursor;
        }

        private void OnDisable()
        {
            GameEvents.Instance.OnGameLost -= UnlockCursor;
            GameEvents.Instance.OnGameWon -= UnlockCursor;
            GameEvents.Instance.OnGameRestarted -= LockCursor;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}