using UnityEngine;
using DG.Tweening;

public class BouncyProjectile : Projectile
{
    [SerializeField] Vector3 MaxSize;
    [SerializeField] float ActiveWeight;
    [SerializeField] float GrowthDuration;

    Rigidbody RigidbodyRef;
    FixedJoint Joint;

    private void Start()
    {
        RigidbodyRef = GetComponent<Rigidbody>();
    }

    public override void ImpactAction(Collision other)
    {
        transform.DOScale(MaxSize, GrowthDuration).SetEase(Ease.OutBounce);
        Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
        RigidbodyRef.mass = ActiveWeight;
        Joint = gameObject.AddComponent<FixedJoint>();
        Joint.connectedBody = body;
    }
}