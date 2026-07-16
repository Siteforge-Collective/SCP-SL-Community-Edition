namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class FirearmBasicMessagesHandler
	{
		public static readonly global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.Firearms.FirearmStatus> ReceivedStatuses = new global::System.Collections.Generic.Dictionary<ushort, global::InventorySystem.Items.Firearms.FirearmStatus>();

		private static readonly global::System.Collections.Generic.HashSet<int> AlreadyRequestedStatuses = new global::System.Collections.Generic.HashSet<int>();

		public static event global::System.Action<global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage> OnStatusMessageReceived;

		public static event global::System.Action<ReferenceHub> OnStatusRequested;

		public static event global::System.Action<global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage> OnClientConfirmationReceived;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += RegisterHandlers;
			global::InventorySystem.Inventory.OnLocalClientStarted += delegate
			{
				global::Mirror.NetworkClient.Send(new global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage(0, global::InventorySystem.Items.Firearms.BasicMessages.RequestType.RequestStatuses));
			};
		}

		private static void RegisterHandlers()
		{
			ReceivedStatuses.Clear();
			AlreadyRequestedStatuses.Clear();
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage>(ClientStatusMessageReceived);
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage>(ClientConfirmationReceived);
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage>(ServerShotReceived);
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage>(ServerRequestReceived);
		}

		private static void ClientConfirmationReceived(global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage msg)
		{
		}

		private static void ServerShotReceived(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage msg)
		{
			if (ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) && msg.ShooterWeaponSerial == hub.inventory.CurItem.SerialNumber && hub.inventory.CurInstance is global::InventorySystem.Items.Firearms.Firearm firearm && firearm.ActionModule.ServerAuthorizeShot())
			{
				if (global::CustomPlayerEffects.SpawnProtected.CheckPlayer(hub) && !global::CustomPlayerEffects.SpawnProtected.CanShoot)
				{
					hub.playerEffectsController.DisableEffect<global::CustomPlayerEffects.SpawnProtected>();
				}
				firearm.HitregModule.ServerProcessShot(msg);
				firearm.OnWeaponShot();
			}
		}

		private static void ServerRequestReceived(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage msg)
		{
			if (!ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub))
			{
				return;
			}
			if (msg.Request == global::InventorySystem.Items.Firearms.BasicMessages.RequestType.RequestStatuses && AlreadyRequestedStatuses.Add(hub.PlayerId))
			{
				foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
				{
					global::InventorySystem.Inventory inventory = allHub.inventory;
					if (inventory.CurInstance is global::InventorySystem.Items.Firearms.Firearm firearm && !(inventory.CurInstance == null))
					{
						conn.Send(new global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage(inventory.CurItem.SerialNumber, firearm.Status));
					}
				}
				global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.OnStatusRequested?.Invoke(hub);
			}
			if (msg.Serial != hub.inventory.CurItem.SerialNumber || !(hub.inventory.CurInstance is global::InventorySystem.Items.Firearms.Firearm firearm2))
			{
				return;
			}
			switch (msg.Request)
			{
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Reload:
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerReloadWeapon, hub, firearm2) && !(global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(firearm2, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.PreventReload) > 0f) && firearm2.AmmoManagerModule.ServerTryReload())
				{
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(msg);
				}
				break;
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Unload:
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerUnloadWeapon, hub, firearm2) && !(global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(firearm2, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.PreventReload) > 0f) && firearm2.AmmoManagerModule.ServerTryUnload())
				{
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(msg);
				}
				break;
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Dryfire:
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDryfireWeapon, hub, firearm2) && firearm2.ActionModule.ServerAuthorizeDryFire())
				{
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(msg);
					firearm2.OnWeaponDryfired();
				}
				break;
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.AdsIn:
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerAimWeapon, hub, firearm2, true))
				{
					firearm2.AdsModule.ServerAds = true;
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(msg);
				}
				break;
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.AdsOut:
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerAimWeapon, hub, firearm2, false))
				{
					firearm2.AdsModule.ServerAds = false;
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(msg);
				}
				break;
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ToggleFlashlight:
			{
				bool flag = firearm2.Status.Flags.HasFlagFast(global::InventorySystem.Items.Firearms.FirearmStatusFlags.FlashlightEnabled);
				if (global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.HasAdvantageFlag(firearm2, global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveAdvantages.Flashlight) && flag != !flag && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerToggleFlashlight, hub, firearm2, !flag))
				{
					firearm2.Status = new global::InventorySystem.Items.Firearms.FirearmStatus(firearm2.Status.Ammo, global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.OverrideFlashlightFlags(firearm2, !flag), firearm2.Status.Attachments);
				}
				break;
			}
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Inspect:
				global::Utils.Networking.NetworkUtils.SendToHubsConditionally(msg, (ReferenceHub x) => x.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole);
				break;
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ReloadStop:
			case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.RequestStatuses:
				break;
			}
		}

		private static void ClientStatusMessageReceived(global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage msg)
		{
			ReceivedStatuses[msg.Serial] = msg.Status;
			global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.OnStatusMessageReceived?.Invoke(msg);
		}

		public static bool HasFlagFast(this global::InventorySystem.Items.Firearms.FirearmStatusFlags flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags flag)
		{
			return (flags & flag) == flag;
		}
	}
}
