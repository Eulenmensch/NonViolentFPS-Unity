using UnityEngine;

namespace NonViolentFPS.Scripts.VerletRope
{
	public class Point
	{
		public Vector3 Position { get; set; }
		public Vector3 PrevPosition { get; set; }
		public bool Locked { get; set; }
	}
}