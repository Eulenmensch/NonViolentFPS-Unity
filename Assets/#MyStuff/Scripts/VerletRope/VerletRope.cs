using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NonViolentFPS.Scripts.VerletRope
{
	public class VerletRope : MonoBehaviour
	{
		[field: SerializeField] public float length { get; set; }
		[field: SerializeField] public int resolution { get; set; }
		[field: SerializeField] public GameObject Balloon { get; set; }

		public List<Point> Points { get; } = new List<Point>();
		public List<Connection> Connections { get; } = new List<Connection>();

		private void Awake()
		{
			Points.Clear();
			BuildRope();
			Points[0].Locked = true;
			Points[resolution-1].Locked = true;
		}

		private void BuildRope()
		{
			CreatePoints();
			CreateConnections();
		}

		private void CreatePoints()
		{
			for (int i = 0; i < resolution; i++)
			{
				var point = new Point();
				var startPosition = transform.position;
				var endPosition = startPosition + Vector3.up * length;
				point.Position = Vector3.Lerp(startPosition, endPosition, (float)(i + 1) / resolution);
				Points.Add(point);
			}
		}

		private void CreateConnections()
		{
			for (int i = 0; i < resolution - 1; i++)
			{
				var connection = new Connection();
				connection.PointA = Points[i];
				connection.PointB = Points[i + 1];
				connection.Length = Vector3.Distance(connection.PointA.Position, connection.PointB.Position);
				Connections.Add(connection);
				Debug.Log("length = " + connection.Length + "\n PointA = " + connection.PointA.Position + "\n PointB = " + connection.PointB.Position);
			}
		}
	}
}