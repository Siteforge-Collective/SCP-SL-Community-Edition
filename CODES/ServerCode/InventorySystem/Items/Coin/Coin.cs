namespace InventorySystem.Items.Coin
{
	public class Coin : global::InventorySystem.Items.Autosync.AutosyncItem, global::InventorySystem.Items.IItemDescription, global::InventorySystem.Items.IItemNametag
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _flipSound;

		private readonly global::System.Diagnostics.Stopwatch _lastUseSw = global::System.Diagnostics.Stopwatch.StartNew();

		private const float RateLimit = 0.6f;

		public override float Weight => 0.0025f;

		internal override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (!base.Owner.isLocalPlayer && _lastUseSw.Elapsed.TotalSeconds < 0.6000000238418579)
			{
				return;
			}
			bool flag;
			switch (global::PluginAPI.Events.EventManager.ExecuteEvent<global::PluginAPI.Events.PlayerPreCoinFlipCancellationData>(global::PluginAPI.Enums.ServerEventType.PlayerPreCoinFlip, new object[1] { base.Owner }).Cancellation)
			{
			case global::PluginAPI.Events.PlayerPreCoinFlipCancellationData.CoinFlipCancellation.Heads:
				flag = false;
				break;
			case global::PluginAPI.Events.PlayerPreCoinFlipCancellationData.CoinFlipCancellation.Tails:
				flag = true;
				break;
			case global::PluginAPI.Events.PlayerPreCoinFlipCancellationData.CoinFlipCancellation.PreventFlip:
				return;
			default:
				flag = global::UnityEngine.Random.value >= 0.5f;
				break;
			}
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerCoinFlip, base.Owner, flag))
			{
				return;
			}
			_lastUseSw.Restart();
			global::Mirror.NetworkWriter writer;
			using (new global::InventorySystem.Items.Autosync.AutosyncRpc(this, toAll: true, out writer))
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, flag);
			}
		}
	}
}
