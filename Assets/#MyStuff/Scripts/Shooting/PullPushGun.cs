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
    [SerializeField] private float rangeFallOffMultiplier;
    [SerializeField] private GameObject pushParticles;
    [SerializeField] private GameObject pullParticles;
    
    private RaycastHit hit;
    private bool active;
    private float pushPullAxis;

    private void Start()
    {
        pushParticles.SetActive(false);
        pullParticles.SetActive(false);
    }

    /*private void Update()
    {
        Shoot();
    }*/

    public void PrimaryMouseButtonEnter()
    {
        pushParticles.SetActive(true);
    }

    public void PrimaryMouseButtonExit()
    {
        pushParticles.SetActive(false);
    }

    public void PrimaryMouseButtonAction()
    {
        if (CheckForObject())
        {
            Push(hit.rigidbody);
        }
    }
    
    public void SecondaryMouseButtonEnter()
    {
        pullParticles.SetActive(true);
    }

    public void SecondaryMouseButtonExit()
    {
        pullParticles.SetActive(false);
    }
    
    public void SecondaryMouseButtonAction()
    {
        if (CheckForObject())
        {
            Pull(hit.rigidbody);
        }
    }

    public void ScrollWheelAction(Vector2 _direction)
    {
    }

    /*public void Shoot()
    {
        if (active)
        {
            if (CheckForObject())
            {
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
    }*/

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
        var force = CalculateForce();
        _body.AddForceAtPosition(force, hit.point);
    }

    private void Pull(Rigidbody _body)
    {
        var force = CalculateForce();
        _body.AddForceAtPosition(-force, hit.point );
    }

    private Vector3 CalculateForce()
    {
        var force = Camera.main.transform.forward * (pushForce * (1 / (1 + hit.distance * rangeFallOffMultiplier)));
        return force;
    }

    /*public void GetPushPullInput(InputAction.CallbackContext _context)
    {
        pushPullAxis = _context.ReadValue<float>();
        if (_context.started)
        {
            active = true;
            if (pushPullAxis > 0)
            {
                pushParticles.SetActive(true);
            }
            else if (pushPullAxis < 0)
            {
                pullParticles.SetActive(true);
            }
        }

        if (_context.canceled)
        {
            active = false;
            pushParticles.SetActive(false);
            pullParticles.SetActive(false);
        }
    }*/
}
