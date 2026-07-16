public struct TeslaHitMsg : global::Mirror.NetworkMessage
{
	public readonly sbyte TeslaGateId;

	public TeslaHitMsg(sbyte teslaGateId)
	{
		TeslaGateId = teslaGateId;
	}
}
