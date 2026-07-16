using System.Collections.Generic;
using Footprinting;
using InventorySystem.Items.Firearms;
using Mirror;
using PlayerRoles.Ragdolls;

namespace PlayerStatsSystem
{
	public class FirearmDamageHandler : AttackerDamageHandler
	{
		public ItemType WeaponType;

		private string _ammoName;

		private ItemType _ammoType;

		private sbyte _hitDirectionX;

		private sbyte _hitDirectionZ;

		private readonly float _penetration;

		private readonly string _deathReasonFormat;

		private readonly bool _useHumanHitboxes;

		private static readonly Dictionary<HitboxType, float> HitboxToForce;

		private static readonly Dictionary<HitboxType, float> HitboxDamageMultipliers;

		private static readonly Dictionary<ItemType, float> AmmoToForce;

		private const float UpwardVelocityFactor = 0.1f;

		public override float Damage { get; internal set; }

		public override Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override string ServerLogsText => null;

		public FirearmDamageHandler()
		{
		}

		public FirearmDamageHandler(Firearm firearm, float damage, bool useHumanMutlipliers = true)
		{
		}

		public override void WriteAdditionalData(NetworkWriter writer)
		{
		}

		public override void ReadAdditionalData(NetworkReader reader)
		{
		}

		public override void ProcessRagdoll(BasicRagdoll ragdoll)
		{
		}

		private void SetWeapon(ItemType weapon)
		{
		}

		protected override void ProcessDamage(ReferenceHub ply)
		{
		}
	}
}
