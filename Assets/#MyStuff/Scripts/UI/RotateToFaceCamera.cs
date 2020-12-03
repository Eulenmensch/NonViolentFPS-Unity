using UnityEngine;

namespace NonViolentFPS.UI
{
    public class RotateToFaceCamera : MonoBehaviour
    {
        private Transform mainCameraTransform;

        private void Start()
        {
            mainCameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            var position = transform.position;
            if(mainCameraTransform != null)
            {
                var look = 2 * transform.position - mainCameraTransform.position;
                transform.LookAt(new Vector3(look.x, position.y, look.z), Vector3.up);
            }
        }
    }
}