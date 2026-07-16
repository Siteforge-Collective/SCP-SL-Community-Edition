namespace InventorySystem.Items.Flashlight
{
	public class FlashlightItem : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IItemDescription, global::InventorySystem.Items.IItemNametag, global::InventorySystem.Items.ILightEmittingItem
	{
		private const float ToggleCooldownTime = 0.13f;

		private const float EquipCooldownTime = 0.6f;

		private float _nextAllowedTime;

		private bool _isEmitting;

		public override float Weight => 0.7f;

		public bool IsEmittingLight
		{
			get
			{
				return _isEmitting;
			}
			set
			{
				if (_isEmitting != value)
				{
					_isEmitting = value;
				}
			}
		}

		public override void OnEquipped()
		{
			_nextAllowedTime = global::UnityEngine.Time.timeSinceLevelLoad + 0.6f;
			IsEmittingLight = true;
			if (global::Mirror.NetworkServer.active && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerToggleFlashlight, base.Owner, this, IsEmittingLight))
			{
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage(base.OwnerInventory.CurItem.SerialNumber, IsEmittingLight));
			}
		}

		public void ClientSendRequest(bool value)
		{
		}
	}
}
