﻿using UnityEngine;
using DG.Tweening;

namespace NonViolentFPS.IK
{
	public class FootIKTarget : MonoBehaviour
	{
		public float MaxFootHeight;
		public float TweenTime;
		public float MaxDist;
		public Transform FootPos;

		void Start()
		{
			MaxDist *= 1.5f;
		}

		void FixedUpdate()
		{
			var distanceRatio = Mathf.Clamp( ( ( FootPos.position - transform.position ).magnitude / MaxDist ), 0, 1 );

			var height = Mathf.Lerp( 0, MaxFootHeight, distanceRatio );

			if ( ( FootPos.position - transform.position ).magnitude >= MaxDist )
			{
				transform.DOMove( FootPos.position, TweenTime ).SetEase( Ease.InOutQuad );
				// transform.DOPunchPosition( MaxFootHeight * Vector3.up, TweenTime, 0, 0, false );
			}
		}
	}
}