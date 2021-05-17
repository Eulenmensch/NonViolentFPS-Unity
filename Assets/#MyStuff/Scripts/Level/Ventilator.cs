using System.Collections.Generic;
using CMF;
using NonViolentFPS.Manager;
using UnityEngine;

namespace NonViolentFPS.Level
{
    public class Ventilator : MonoBehaviour
    {
        [SerializeField] private float pushForce;
        [SerializeField] private float playerMultiplier;
        [SerializeField, Range(0.1f,10f)] private float forceDistanceFalloff;
        [SerializeField] private List<Transform> bladeTransforms;
        [SerializeField] private float rotationSpeedMultiplier;

        private HashSet<Rigidbody> pushableRigidbodies;
        private AdvancedWalkerController playerController;

        private void Start()
        {
            pushableRigidbodies = new HashSet<Rigidbody>();
        }

        private void FixedUpdate()
        {
            foreach (var pushableRigidbody in pushableRigidbodies)
            {
                var distance = Vector3.Distance(pushableRigidbody.position, transform.position);
                var distanceFalloff = 1 + forceDistanceFalloff * distance;
                //This makes for a Hyperbola where forceDistanceFalloff is the dilating factor
                var resultingForce = transform.up * (pushForce/distanceFalloff);
                //This force should depend on the mass of the pushed body
                pushableRigidbody.AddForce(resultingForce, ForceMode.Force);
            }

            if (playerController != null)
            {
                var distance = Vector3.Distance(playerController.transform.position, transform.position);
                var distanceFalloff = 1 + forceDistanceFalloff * distance;
                var resultingForce = transform.up * (pushForce * playerMultiplier / distanceFalloff);
                playerController.AddMomentum(resultingForce);
            }
        }

        private void Update()
        {
            foreach (var bladeTransform in bladeTransforms)
            {
                bladeTransform.Rotate(0,pushForce * rotationSpeedMultiplier * Time.deltaTime,0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.Equals(GameManager.Instance.Player))
            {
                playerController = other.GetComponent<AdvancedWalkerController>();
            }
            var otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody == null || otherRigidbody.isKinematic) {return;}

            pushableRigidbodies.Add(otherRigidbody);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.Equals(GameManager.Instance.Player))
            {
                playerController = null;
            }
            var otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody == null || otherRigidbody.isKinematic) {return;}

            pushableRigidbodies.Remove(otherRigidbody);
        }
    }
}
