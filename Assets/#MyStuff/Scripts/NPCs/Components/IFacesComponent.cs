using System.Collections.Generic;
using UnityEngine;

namespace NonViolentFPS.NPCs
{
	public interface IFacesComponent
	{
		List<GameObject> Faces { get; set; }
	}
}