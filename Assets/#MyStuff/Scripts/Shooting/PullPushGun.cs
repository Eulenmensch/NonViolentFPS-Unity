using System;
using Ludiq.PeekCore;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float projectileWiggleTime;
    [SerializeField] private float projectileDeletionRadius;

    private RaycastHit hit;
    private float wiggleTimer;
    private Shooter shooter;

    private void Start()
    {
        shooter = GetComponent<Shooter>();
        pushParticles.SetActive(false);
        pullParticles.SetActive(false);
    }

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
        wiggleTimer = 0;
    }

    public void SecondaryMouseButtonAction()
    {
        if (CheckForObject())
        {
            SuckUpProjectiles();
            Pull(hit.rigidbody);
        }
        DeletePhysicsProjectiles();
    }

    public void ScrollWheelAction(InputAction.CallbackContext _context) { }

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
        if (_body == null) return;
        _body.AddForceAtPosition(-force, hit.point );
    }

    private Vector3 CalculateForce()
    {
        var force = Camera.main.transform.forward * (pushForce * (1 / (1 + hit.distance * rangeFallOffMultiplier)));
        return force;
    }

    private void SuckUpProjectiles()
    {
        if (hit.transform.GetComponent<PhysicsProjectile>() != null)
        {
            if (wiggleTimer <= projectileWiggleTime)
            {
                wiggleTimer += Time.deltaTime;
                return;
            }

            var body = hit.rigidbody;
            if (body == null)
            {
                body = hit.transform.AddComponent<Rigidbody>();
            }

            var wiggler = hit.transform.GetComponent<MMWiggle>();
            if (wiggler != null)
            {
                wiggler.PositionActive = true;
            }
        }
    }

    private void DeletePhysicsProjectiles()
    {
        var hitColliders = Physics.OverlapSphere(castOrigin.position, projectileDeletionRadius, interactibleMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<PhysicsProjectile>() != null)
            {
                Destroy(hitCollider.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.GetComponent<PhysicsProjectile>())
        {
            Destroy(_other.gameObject);
        }
    }
}
