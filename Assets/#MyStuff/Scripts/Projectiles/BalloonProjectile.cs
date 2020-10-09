using UnityEngine;
using System.Collections;
using DG.Tweening;
using Obi;

public class BalloonProjectile : Projectile
{
    [SerializeField] float RiseForce;
    [SerializeField] Vector3 MaxSize;
    [SerializeField] float GrowthDuration;
    [SerializeField] float ActiveWeight;

    float timer;
    bool Rising;
    bool Ground;
    Rigidbody RigidbodyRef;
    SpringJoint Joint;
    LineRenderer Line;
    float DefaultSpringForce;
    GameObject BalloonStringStart;

    private void Start()
    {
        RigidbodyRef = GetComponent<Rigidbody>();
        Joint = GetComponent<SpringJoint>();
        Line = GetComponent<LineRenderer>();
        DefaultSpringForce = Joint.spring;
        Joint.spring = 0;
    }

    private void FixedUpdate()
    {
        if (Rising)
        {
            RigidbodyRef.AddForce(Vector3.up * RiseForce, ForceMode.Acceleration);
            Line.SetPosition(0, transform.position);
            if (Ground) { return; }
            Line.SetPosition(1, BalloonStringStart.transform.position);
        }
    }

    public override void ImpactAction(Collision other)
    {
        transform.DOScale(MaxSize, GrowthDuration).SetEase(Ease.OutBounce);
        Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
        RigidbodyRef.mass = ActiveWeight;
        Joint.spring = DefaultSpringForce;
        Joint.connectedBody = body;
        Joint.connectedAnchor = transform.InverseTransformPoint(other.contacts[0].point);
        BalloonStringStart = new GameObject("BalloonStringStart");
        BalloonStringStart.transform.position = other.contacts[0].point;
        BalloonStringStart.transform.parent = other.gameObject.transform;
        Line.SetPosition(1, other.contacts[0].point);
        if (other.gameObject.tag.Equals("Ground"))
        {
            Ground = true;
        }
        Rising = true;
    }
}