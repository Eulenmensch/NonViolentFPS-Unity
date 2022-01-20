using UnityEngine;

namespace NonViolentFPS.Scripts.VerletRope
{
	public class Connection
	{
		public Point PointA { get; set; }
		public Point PointB { get; set; }
		public float Length { get; set; }
	}
}