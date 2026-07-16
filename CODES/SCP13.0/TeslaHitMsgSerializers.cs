using Mirror;

public static class TeslaHitMsgSerializers
{
	public static void Serialize(this NetworkWriter writer, TeslaHitMsg value)
	{
	}

	public static TeslaHitMsg Deserialize(this NetworkReader reader)
	{
		return default(TeslaHitMsg);
	}
}
