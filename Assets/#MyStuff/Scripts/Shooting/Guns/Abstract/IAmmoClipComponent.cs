namespace NonViolentFPS.Shooting
{
	public interface IAmmoClipComponent
	{
		int ClipSize { get; set; }
		int AmmoInClip { get; set; }
	}
}