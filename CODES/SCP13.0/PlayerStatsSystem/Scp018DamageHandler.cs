using Footprinting;
using InventorySystem.Items.ThrowableProjectiles;
using PlayerRoles.Ragdolls;
using UnityEngine;

namespace PlayerStatsSystem
{
	public class Scp018DamageHandler : AttackerDamageHandler
	{
		private readonly string _deathScreenText;

		private readonly string _serverLogsText;

		private readonly string _ragdollInspectText;

		private readonly Vector3 _ballImpactVelocity;

		private const float ForceMultiplier = 0.5f;

		private const float HipMultiplier = 3f;

		public override float Damage { get; internal set; }

		public override Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override string ServerLogsText => null;

		public override bool IgnoreFriendlyFireDetector => false;

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			return default(HandlerOutput);
		}

		public override void ProcessRagdoll(BasicRagdoll ragdoll)
		{
		}

		public Scp018DamageHandler(Scp018Projectile ball, float dmg, bool ignoreFF)
		{
		}
	}
}
