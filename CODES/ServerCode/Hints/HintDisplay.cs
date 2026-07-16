namespace Hints
{
	[global::UnityEngine.RequireComponent(typeof(global::InventorySystem.Searching.SearchCoordinator))]
	public class HintDisplay : global::Mirror.NetworkBehaviour
	{
		public void Show(global::Hints.Hint hint)
		{
			if (hint == null)
			{
				throw new global::System.ArgumentNullException("hint");
			}
			if (base.isLocalPlayer)
			{
				throw new global::System.InvalidOperationException("Cannot display a hint to the local player (headless server).");
			}
			if (global::Mirror.NetworkServer.active)
			{
				base.netIdentity.connectionToClient.Send(new global::Hints.HintMessage(hint));
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
