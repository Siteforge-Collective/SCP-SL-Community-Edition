namespace InventorySystem.Items.Usables.Scp1576
{
	public static class Scp1576SpectatorWarningHandler
	{
		public struct SpectatorWarningMessage : global::Mirror.NetworkMessage
		{
			public bool IsStop;
		}

		private static readonly global::System.Diagnostics.Stopwatch CooldownTimer = global::System.Diagnostics.Stopwatch.StartNew();

		private static readonly global::System.Collections.Generic.HashSet<ushort> CurrentlyUsed = new global::System.Collections.Generic.HashSet<ushort>();

		private static bool _stopSoundScheduled;

		public static event global::System.Action OnStart;

		public static event global::System.Action OnStop;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				CurrentlyUsed.Clear();
				_stopSoundScheduled = false;
				global::Mirror.NetworkClient.RegisterHandler<global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage>(HandleMessage);
			};
			StaticUnityMethods.OnUpdate += delegate
			{
				if (_stopSoundScheduled && global::Mirror.NetworkServer.active && !(CooldownTimer.Elapsed.TotalSeconds < 2.0))
				{
					SendMessage(isStop: true);
					_stopSoundScheduled = false;
				}
			};
		}

		private static void SendMessage(bool isStop)
		{
			global::Utils.Networking.NetworkUtils.SendToHubsConditionally(new global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage
			{
				IsStop = isStop
			}, (ReferenceHub x) => x.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole);
		}

		private static void HandleMessage(global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage msg)
		{
			if (msg.IsStop)
			{
				global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.OnStop?.Invoke();
			}
			else
			{
				global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.OnStart?.Invoke();
			}
		}

		public static void TriggerStart(global::InventorySystem.Items.Usables.Scp1576.Scp1576Item item)
		{
			_stopSoundScheduled = false;
			if (CurrentlyUsed.Count == 0)
			{
				CooldownTimer.Restart();
				SendMessage(isStop: false);
			}
			CurrentlyUsed.Add(item.ItemSerial);
		}

		public static void TriggerStop(global::InventorySystem.Items.Usables.Scp1576.Scp1576Item item)
		{
			if (CurrentlyUsed.Remove(item.ItemSerial))
			{
				_stopSoundScheduled = CurrentlyUsed.Count == 0;
			}
		}
	}
}
