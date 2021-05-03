using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IDefaultLocationComponent
	{
		Vector3 DefaultLocation { get; set; }
		float BufferRadiusMin { get; set; }
		float BufferRadiusMax { get; set; }
	}
}