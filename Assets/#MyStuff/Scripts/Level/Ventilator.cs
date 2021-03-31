using System;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    [SerializeField] private float pushForce;
    [SerializeField, Range(0.1f,10f)] private float forceDistanceFalloff;
    [SerializeField] private List<Transform> bladeTransforms;
    [SerializeField] private float rotationSpeedMultiplier;



    private HashSet<Rigidbody> pushableRigidbodies;

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
        var otherRigidbody = other.attachedRigidbody;
        if (otherRigidbody == null || otherRigidbody.isKinematic) {return;}

        pushableRigidbodies.Add(otherRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        var otherRigidbody = other.attachedRigidbody;
        if (otherRigidbody == null || otherRigidbody.isKinematic) {return;}

        pushableRigidbodies.Remove(otherRigidbody);
    }
}
