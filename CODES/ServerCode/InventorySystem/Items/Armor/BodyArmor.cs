namespace InventorySystem.Items.Armor
{
	public class BodyArmor : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IEquipDequipModifier, global::InventorySystem.Items.IWearableItem, global::InventorySystem.Items.IItemNametag, global::InventorySystem.Items.ICustomSearchCompletorItem, global::PlayerRoles.FirstPersonControl.IMovementSpeedModifier, global::PlayerRoles.FirstPersonControl.IStaminaModifier
	{
		[global::System.Serializable]
		public struct ArmorAmmoLimit
		{
			public ItemType AmmoType;

			public ushort Limit;
		}

		[global::System.Serializable]
		public struct ArmorCategoryLimitModifier
		{
			public ItemCategory Category;

			public byte Limit;
		}

		[global::System.NonSerialized]
		public bool DontRemoveExcessOnDrop;

		[global::UnityEngine.Range(0f, 100f)]
		public int HelmetEfficacy;

		[global::UnityEngine.Range(0f, 100f)]
		public int VestEfficacy;

		public float CivilianClassDownsidesMultiplier = 1f;

		public global::InventorySystem.Items.Armor.BodyArmor.ArmorAmmoLimit[] AmmoLimits;

		public global::InventorySystem.Items.Armor.BodyArmor.ArmorCategoryLimitModifier[] CategoryLimits;

		[global::UnityEngine.SerializeField]
		[global::UnityEngine.Range(1f, 2f)]
		private float _staminaUseMultiplier = 1f;

		[global::UnityEngine.SerializeField]
		[global::UnityEngine.Range(0f, 1f)]
		private float _movementSpeedMultiplier = 1f;

		[global::UnityEngine.SerializeField]
		private float _weight;

		public override float Weight => _weight;

		public bool IsWorn => true;

		public bool AllowEquip => false;

		public bool AllowHolster => true;

		public bool MovementModifierActive => IsWorn;

		public float MovementSpeedMultiplier => ProcessMultiplier(_movementSpeedMultiplier);

		public float MovementSpeedLimit => float.MaxValue;

		public bool StaminaModifierActive => IsWorn;

		public float StaminaUsageMultiplier => ProcessMultiplier(_staminaUseMultiplier);

		public bool SprintingDisabled => false;

		public float StaminaRegenMultiplier => 1f;

		public override global::InventorySystem.Items.ItemDescriptionType DescriptionType => global::InventorySystem.Items.ItemDescriptionType.Armor;

		private float ProcessMultiplier(float f)
		{
			global::PlayerRoles.Team team = global::PlayerRoles.PlayerRolesUtils.GetTeam(base.Owner);
			if (team == global::PlayerRoles.Team.ClassD || team == global::PlayerRoles.Team.Scientists)
			{
				f = (f - 1f) * CivilianClassDownsidesMultiplier + 1f;
			}
			return f;
		}

		public global::InventorySystem.Searching.SearchCompletor GetCustomSearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase ipb, global::InventorySystem.Items.ItemBase ib, double disSqrt)
		{
			return new global::InventorySystem.Searching.ArmorSearchCompletor(hub, ipb, ib, disSqrt);
		}

		public override void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnRemoved(pickup);
			if (global::Mirror.NetworkServer.active && !DontRemoveExcessOnDrop)
			{
				global::InventorySystem.Items.Armor.BodyArmorUtils.RemoveEverythingExceedingLimits(base.OwnerInventory, null);
			}
		}
	}
}
