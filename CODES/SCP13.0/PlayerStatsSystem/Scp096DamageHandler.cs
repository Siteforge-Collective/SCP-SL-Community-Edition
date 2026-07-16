using System.Collections.Generic;
using Footprinting;
using PlayerRoles.PlayableScps.Scp096;

namespace PlayerStatsSystem
{
	public class Scp096DamageHandler : ScpDamageHandler
	{
		public enum AttackType
		{
			GateKill = 0,
			SlapLeft = 1,
			SlapRight = 2,
			Charge = 3
		}

		private static readonly Dictionary<AttackType, string> LogReasons;

		private readonly string _ragdollInspectText;

		private readonly AttackType _attackType;

		public override float Damage { get; internal set; }

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override CassieAnnouncement CassieDeathAnnouncement => null;

		public override Footprint Attacker { get; protected set; }

		public override string ServerLogsText => null;

		public override bool AllowSelfDamage => false;

		public Scp096DamageHandler(Scp096Role attacker, float damage, AttackType attackType)
		{
		}

		public override HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			return default(HandlerOutput);
		}

		public Scp096DamageHandler()
		{
		}
	}
}
