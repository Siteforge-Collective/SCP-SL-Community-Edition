internal class LifeFriendlyFireDetector : FriendlyFireDetector
{
	private const string Detector = "Life";

	internal LifeFriendlyFireDetector(ReferenceHub hub)
		: base(null)
	{
	}

	public override bool RegisterDamage(float damage)
	{
		return false;
	}

	public override bool RegisterKill()
	{
		return false;
	}
}
