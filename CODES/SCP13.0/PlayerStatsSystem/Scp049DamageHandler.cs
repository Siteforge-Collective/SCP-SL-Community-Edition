using System.Collections.Generic;
using Footprinting;
using Mirror;

namespace PlayerStatsSystem
{
	public class Scp049DamageHandler : ScpDamageHandler
	{
		public enum AttackType : byte
		{
			Instakill = 0,
			CardiacArrest = 1,
			Scp0492 = 2
		}

		private static readonly Dictionary<AttackType, string> LogReasons;

		private readonly string _ragdollInspectText;

		public override float Damage { get; internal set; }

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override CassieAnnouncement CassieDeathAnnouncement => null;

		public override Footprint Attacker { get; protected set; }

		public override string ServerLogsText => null;

		public override bool AllowSelfDamage => false;

		public AttackType DamageSubType { get; private set; }

		public Scp049DamageHandler(ReferenceHub attacker, float damage, AttackType attackType)
		{
		}

		public Scp049DamageHandler(Footprint attacker, float damage, AttackType attackType)
		{
		}

		public Scp049DamageHandler()
		{
		}

		public override HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			return default(HandlerOutput);
		}

		public override void WriteDeathScreen(NetworkWriter writer)
		{
		}

		public override void WriteAdditionalData(NetworkWriter writer)
		{
		}

		public override void ReadAdditionalData(NetworkReader reader)
		{
		}
	}
}
