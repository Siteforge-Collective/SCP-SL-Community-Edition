public class PlayerIpOverride : global::Mirror.NetworkBehaviour
{
	private void Start()
	{
		if (!CustomLiteNetLib4MirrorTransport.IpPassthroughEnabled || !global::Mirror.NetworkServer.active || base.isLocalPlayer || !(base.connectionToClient is global::Mirror.NetworkConnectionToClient networkConnectionToClient))
		{
			return;
		}
		try
		{
			int id = global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorServer.Peers[networkConnectionToClient.connectionId].Id;
			if (CustomLiteNetLib4MirrorTransport.RealIpAddresses.ContainsKey(id))
			{
				networkConnectionToClient.IpOverride = CustomLiteNetLib4MirrorTransport.RealIpAddresses[id];
			}
		}
		catch (global::System.Exception ex)
		{
			ServerConsole.AddLog("Error during IP passthrough processing: " + ex.Message);
		}
	}

	private void MirrorProcessed()
	{
	}
}
