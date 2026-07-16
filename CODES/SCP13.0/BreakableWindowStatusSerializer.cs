using Mirror;

public static class BreakableWindowStatusSerializer
{
	public static void WriteBreakableWindowStatus(this NetworkWriter writer, BreakableWindow.BreakableWindowStatus value)
	{
	}

	public static BreakableWindow.BreakableWindowStatus ReadBreakableWindowStatus(this NetworkReader reader)
	{
		return default(BreakableWindow.BreakableWindowStatus);
	}
}
