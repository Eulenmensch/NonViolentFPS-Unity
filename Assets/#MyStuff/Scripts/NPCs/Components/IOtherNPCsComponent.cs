using System.Collections.Generic;

namespace NonViolentFPS.NPCs
{
	public interface IOtherNPCsComponent
	{
		HashSet<NPC> OtherNPCs { get; set; }
	}
}