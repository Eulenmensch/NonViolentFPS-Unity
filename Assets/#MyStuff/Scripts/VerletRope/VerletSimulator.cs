using System;
using System.Collections.Generic;
using UnityEngine;

namespace NonViolentFPS.Scripts.VerletRope
{
	public class VerletSimulator : MonoBehaviour
	{
		[SerializeField] private int simulationIterations;
		[SerializeField] private float gravity;
		[SerializeField] private float balloonGravity;

		[SerializeField] private VerletRope rope;
		[SerializeField] private LineRenderer lineRenderer;

		private void Start()
		{
			lineRenderer.positionCount = rope.resolution;
			lineRenderer.SetPosition(0, rope.Points[0].Position);
		}

		private void Update()
		{
			rope.Points[0].Position = rope.transform.position;
			lineRenderer.SetPosition(0, rope.Points[0].Position);

			rope.Points[rope.resolution-1].Position = rope.Balloon.transform.position;
			lineRenderer.SetPosition(rope.resolution-1, rope.Points[rope.resolution-1].Position);
		}

		private void FixedUpdate()
		{
			CalculatePointPositions();
			CalculateConnections();

			// rope.Balloon.transform.position = rope.Points[rope.resolution - 1].Position;
		}

		private void CalculatePointPositions()
		{
			foreach (var point in rope.Points)
			{
				if (!point.Locked)
				{
					Vector3 positionBeforeUpdate = point.Position;
					point.Position += point.Position - point.PrevPosition;
					point.Position += Vector3.down * gravity * Mathf.Pow(Time.fixedDeltaTime, 2);
					lineRenderer.SetPosition(rope.Points.IndexOf(point), point.Position);
					point.PrevPosition = positionBeforeUpdate;
				}
			}
		}

		private void CalculateConnections()
		{
			for (int i = 0; i < simulationIterations; i++)
			{
				foreach (Connection connection in rope.Connections)
				{
					Vector3 connectionCenter = (connection.PointA.Position + connection.PointB.Position) / 2;
					Vector3 connectionDirection = (connection.PointA.Position - connection.PointB.Position).normalized;
					if (!connection.PointA.Locked)
					{
						connection.PointA.Position = connectionCenter + connectionDirection * connection.Length / 2;
					}

					if (!connection.PointB.Locked)
					{
						connection.PointB.Position = connectionCenter - connectionDirection * connection.Length / 2;
					}
				}
			}
		}
	}
}