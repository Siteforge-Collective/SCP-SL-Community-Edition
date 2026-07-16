namespace InventorySystem.Configs
{
	public static class StartingInventories
	{
		public static readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::InventorySystem.InventoryRoleInfo> DefinedInventories = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::InventorySystem.InventoryRoleInfo>
		{
			[global::PlayerRoles.RoleTypeId.Scientist] = new global::InventorySystem.InventoryRoleInfo(new ItemType[2]
			{
				ItemType.KeycardScientist,
				ItemType.Medkit
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort>()),
			[global::PlayerRoles.RoleTypeId.FacilityGuard] = new global::InventorySystem.InventoryRoleInfo(new ItemType[6]
			{
				ItemType.KeycardGuard,
				ItemType.GunFSP9,
				ItemType.Medkit,
				ItemType.GrenadeFlash,
				ItemType.Radio,
				ItemType.ArmorLight
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort> { [ItemType.Ammo9x19] = 60 }),
			[global::PlayerRoles.RoleTypeId.NtfPrivate] = new global::InventorySystem.InventoryRoleInfo(new ItemType[5]
			{
				ItemType.KeycardNTFOfficer,
				ItemType.GunCrossvec,
				ItemType.Medkit,
				ItemType.Radio,
				ItemType.ArmorCombat
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort>
			{
				[ItemType.Ammo9x19] = 160,
				[ItemType.Ammo556x45] = 40
			}),
			[global::PlayerRoles.RoleTypeId.NtfSergeant] = new global::InventorySystem.InventoryRoleInfo(new ItemType[6]
			{
				ItemType.KeycardNTFLieutenant,
				ItemType.GunE11SR,
				ItemType.Medkit,
				ItemType.GrenadeHE,
				ItemType.Radio,
				ItemType.ArmorCombat
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort>
			{
				[ItemType.Ammo9x19] = 40,
				[ItemType.Ammo556x45] = 120
			}),
			[global::PlayerRoles.RoleTypeId.NtfSpecialist] = new global::InventorySystem.InventoryRoleInfo(new ItemType[6]
			{
				ItemType.KeycardNTFLieutenant,
				ItemType.GunE11SR,
				ItemType.Medkit,
				ItemType.GrenadeHE,
				ItemType.Radio,
				ItemType.ArmorCombat
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort>
			{
				[ItemType.Ammo9x19] = 40,
				[ItemType.Ammo556x45] = 120
			}),
			[global::PlayerRoles.RoleTypeId.NtfCaptain] = new global::InventorySystem.InventoryRoleInfo(new ItemType[7]
			{
				ItemType.KeycardNTFCommander,
				ItemType.GunE11SR,
				ItemType.Adrenaline,
				ItemType.Medkit,
				ItemType.GrenadeHE,
				ItemType.Radio,
				ItemType.ArmorHeavy
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort>
			{
				[ItemType.Ammo9x19] = 40,
				[ItemType.Ammo556x45] = 160
			}),
			[global::PlayerRoles.RoleTypeId.ChaosConscript] = new global::InventorySystem.InventoryRoleInfo(new ItemType[5]
			{
				ItemType.KeycardChaosInsurgency,
				ItemType.GunAK,
				ItemType.Medkit,
				ItemType.Painkillers,
				ItemType.ArmorCombat
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort> { [ItemType.Ammo762x39] = 120 }),
			[global::PlayerRoles.RoleTypeId.ChaosRifleman] = new global::InventorySystem.InventoryRoleInfo(new ItemType[5]
			{
				ItemType.KeycardChaosInsurgency,
				ItemType.GunAK,
				ItemType.Medkit,
				ItemType.Painkillers,
				ItemType.ArmorCombat
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort> { [ItemType.Ammo762x39] = 120 }),
			[global::PlayerRoles.RoleTypeId.ChaosRepressor] = new global::InventorySystem.InventoryRoleInfo(new ItemType[6]
			{
				ItemType.KeycardChaosInsurgency,
				ItemType.GunShotgun,
				ItemType.GunRevolver,
				ItemType.Medkit,
				ItemType.Painkillers,
				ItemType.ArmorCombat
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort>
			{
				[ItemType.Ammo44cal] = 24,
				[ItemType.Ammo12gauge] = 42
			}),
			[global::PlayerRoles.RoleTypeId.ChaosMarauder] = new global::InventorySystem.InventoryRoleInfo(new ItemType[5]
			{
				ItemType.KeycardChaosInsurgency,
				ItemType.GunLogicer,
				ItemType.Medkit,
				ItemType.Adrenaline,
				ItemType.ArmorHeavy
			}, new global::System.Collections.Generic.Dictionary<ItemType, ushort> { [ItemType.Ammo762x39] = 200 })
		};
	}
}
