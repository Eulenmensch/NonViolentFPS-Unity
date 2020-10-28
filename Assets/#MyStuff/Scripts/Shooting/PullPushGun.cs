using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PullPushGun : MonoBehaviour, IGun
{
    [SerializeField] private LayerMask interactibleMask;
    [SerializeField] private Transform castOrigin;
    [SerializeField] private float castRadius;
    [SerializeField] private float castRange;
    [SerializeField] private float pushForce;
    
    private RaycastHit hit;
    private bool active;
    private float pushPullAxis;

    private void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (active)
        {
            if (CheckForObject())
            {
                print(hit.rigidbody.name);
                if (pushPullAxis > 0)
                {
                    Push(hit.rigidbody);
                }
                else if (pushPullAxis < 0)
                {
                    Pull(hit.rigidbody);
                }
            }
        }
    }

    private bool CheckForObject()
    {
        if (Physics.SphereCast(castOrigin.position, castRadius, Camera.main.transform.forward, out hit, castRange, interactibleMask))
        {
            return true;
        }

        return false;
    }
    
    private void Push(Rigidbody _body)
    {
        _body.AddForceAtPosition(Camera.main.transform.forward * pushForce, hit.point );
    }

    private void Pull(Rigidbody _body)
    {
        _body.AddForceAtPosition(-Camera.main.transform.forward * pushForce, hit.point );
    }

    public void GetPushPullInput(InputAction.CallbackContext _context)
    {
        pushPullAxis = _context.ReadValue<float>();
        if (_context.started)
        {
            active = true;
        }

        if (_context.canceled)
        {
            active = false;
        }
    }
}
