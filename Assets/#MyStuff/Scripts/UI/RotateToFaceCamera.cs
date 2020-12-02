using UnityEngine;

namespace NonViolentFPS.UI
{
    public class RotateToFaceCamera : MonoBehaviour
    {
        private Camera MainCamera;

        private void Start()
        {
            MainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            var look = 2 * transform.position - MainCamera.transform.position;
            // transform.LookAt(transform.position + MainCamera.transform.rotation * Vector3.forward, MainCamera.transform.rotation * Vector3.up);
            transform.LookAt( new Vector3( look.x, transform.position.y, look.z ), Vector3.up );
        }
    }
}