using Footprinting;
using UnityEngine;

namespace PlayerStatsSystem
{
	public class ExplosionDamageHandler : AttackerDamageHandler
	{
		private readonly string _deathScreenText;

		private readonly string _serverLogsText;

		private readonly string _ragdollInspectText;

		private readonly Vector3 _force;

		private const float ForceMultiplier = 1.3f;

		public override float Damage { get; internal set; }

		public override Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override string ServerLogsText => null;

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			return default(HandlerOutput);
		}

		public ExplosionDamageHandler(Footprint attacker, Vector3 force, float damage, int armorPenetration)
		{
		}
	}
}
