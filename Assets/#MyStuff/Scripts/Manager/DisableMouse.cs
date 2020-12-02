using UnityEngine;

namespace NonViolentFPS.Manager
{
    public class DisableMouse : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}