using UnityEngine;
using System.Collections;
using DG.Tweening;
using Obi;

public class BalloonProjectile : Projectile
{
    [SerializeField] Vector3 MaxSize;
    [SerializeField] float MaxRopeLength;
    [SerializeField] float GrowthDuration;
    [SerializeField] float RopeExtensionSpeed;
    [SerializeField] ObiSolver Solver;
    [SerializeField] ObiRopeCursor RopeCursor;
    [SerializeField] ObiRope Rope;
    [SerializeField] ObiParticleAttachment AttachmentBalloon;
    [SerializeField] ObiParticleAttachment AttachmentObject;

    float timer;

    private void Start()
    {
        Solver.enabled = false;
        AttachmentBalloon.attachmentType = ObiParticleAttachment.AttachmentType.Static;
    }

    public override void ImpactAction(Collision other)
    {
        transform.DOScale(MaxSize, GrowthDuration).SetEase(Ease.OutBounce);
        AttachmentBalloon.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        Solver.enabled = true;
        AttachmentObject.target = other.gameObject.transform;
        StartCoroutine(ExtendRope());
    }

    private IEnumerator ExtendRope()
    {
        while (timer <= GrowthDuration)
        {
            timer += Time.deltaTime;
            RopeCursor.ChangeLength(Rope.restLength + RopeExtensionSpeed * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
}