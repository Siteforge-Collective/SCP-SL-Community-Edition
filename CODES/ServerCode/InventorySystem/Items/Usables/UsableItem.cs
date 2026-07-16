namespace InventorySystem.Items.Usables
{
	public abstract class UsableItem : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IEquipDequipModifier, global::InventorySystem.Drawers.IItemAlertDrawer, global::InventorySystem.Drawers.IItemDrawer, global::InventorySystem.Items.IItemDescription, global::InventorySystem.Items.IItemNametag
	{
		[global::System.NonSerialized]
		public float RemainingCooldown;

		[global::System.NonSerialized]
		public bool IsUsing;

		public float UseTime;

		public float MaxCancellableTime;

		[global::UnityEngine.SerializeField]
		private float _weight = 1f;

		private static global::UnityEngine.KeyCode _useKey;

		private static global::UnityEngine.KeyCode _cancelKey;

		private static string _cooldownFormat;

		public const float AudibleSfxRange = 15f;

		public virtual bool CanStartUsing { get; protected set; } = true;

		public override float Weight => _weight;

		public virtual bool AllowHolster => !IsUsing;

		public virtual bool AllowEquip => true;

		public abstract void ServerOnUsingCompleted();

		public virtual void OnUsingStarted()
		{
			IsUsing = true;
		}

		public virtual void OnUsingCancelled()
		{
			IsUsing = false;
		}

		protected void ServerRemoveSelf()
		{
			base.OwnerInventory.ServerRemoveItem(base.ItemSerial, null);
		}

		protected void ServerSetPersonalCooldown(float timeSeconds)
		{
			global::InventorySystem.Items.Usables.UsableItemsController.GetHandler(base.Owner).PersonalCooldowns[ItemTypeId] = global::UnityEngine.Time.timeSinceLevelLoad + timeSeconds;
		}

		protected void ServerSetGlobalItemCooldown(float timeSeconds)
		{
			global::InventorySystem.Items.Usables.UsableItemsController.GlobalItemCooldowns[base.ItemSerial] = global::UnityEngine.Time.timeSinceLevelLoad + timeSeconds;
		}

		protected void ServerAddRegeneration(global::UnityEngine.AnimationCurve regenCurve, float speedMultiplier = 1f, float hpMultiplier = 1f)
		{
			global::InventorySystem.Items.Usables.UsableItemsController.GetHandler(base.Owner).ActiveRegenerations.Add(new global::InventorySystem.Items.Usables.RegenerationProcess(regenCurve, speedMultiplier, hpMultiplier));
		}

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
		}

		public override void OnEquipped()
		{
			if (global::Mirror.NetworkServer.active)
			{
				float cooldown = global::InventorySystem.Items.Usables.UsableItemsController.GetCooldown(base.ItemSerial, this, global::InventorySystem.Items.Usables.UsableItemsController.GetHandler(base.Owner));
				base.OwnerInventory.connectionToClient.Send(new global::InventorySystem.Items.Usables.ItemCooldownMessage(base.ItemSerial, cooldown));
			}
		}

		public override void EquipUpdate()
		{
		}

		public virtual bool ServerValidateCancelRequest(global::InventorySystem.Items.Usables.PlayerHandler handler)
		{
			return true;
		}

		public virtual bool ServerValidateStartRequest(global::InventorySystem.Items.Usables.PlayerHandler handler)
		{
			return true;
		}
	}
}
