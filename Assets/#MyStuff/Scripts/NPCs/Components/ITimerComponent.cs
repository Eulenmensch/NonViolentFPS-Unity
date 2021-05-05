namespace NonViolentFPS.NPCs
{
	public interface ITimerComponent
	{
		float Timer { get; set; }
		float MinTime { get; set; }
		float MaxTime { get; set; }
	}
}