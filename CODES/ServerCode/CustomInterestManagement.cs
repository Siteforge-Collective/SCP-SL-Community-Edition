public class CustomInterestManagement : global::Mirror.InterestManagement
{
	public override bool OnCheckObserver(global::Mirror.NetworkIdentity identity, global::Mirror.NetworkConnection newObserver)
	{
		return true;
	}

	public override void OnRebuildObservers(global::Mirror.NetworkIdentity identity, global::System.Collections.Generic.HashSet<global::Mirror.NetworkConnection> newObservers, bool initialize)
	{
		if (!initialize || identity.visible == global::Mirror.Visibility.ForceHidden)
		{
			return;
		}
		foreach (global::Mirror.NetworkConnectionToClient value in global::Mirror.NetworkServer.connections.Values)
		{
			if (value.isReady)
			{
				newObservers.Add(value);
			}
		}
		if (global::Mirror.NetworkServer.localConnection != null && global::Mirror.NetworkServer.localConnection.isReady)
		{
			newObservers.Add(global::Mirror.NetworkServer.localConnection);
		}
	}
}
