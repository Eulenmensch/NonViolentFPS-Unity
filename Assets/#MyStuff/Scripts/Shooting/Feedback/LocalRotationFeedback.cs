using UnityEngine;

namespace NonViolentFPS.Shooting
{
    public class LocalRotationFeedback : MonoBehaviour
    {
        public float xLocalRotation;

        private Vector3 defaultLocalRotation;

        private void Start()
        {
            defaultLocalRotation = transform.localRotation.eulerAngles;
        }

        void Update()
        {
            transform.localRotation = Quaternion.Euler(xLocalRotation, defaultLocalRotation.y, defaultLocalRotation.z);
        }
    }
}
