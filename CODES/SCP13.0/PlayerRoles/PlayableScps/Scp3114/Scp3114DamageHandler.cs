using Footprinting;
using Mirror;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114DamageHandler : AttackerDamageHandler, IRagdollInspectOverride
	{
		public enum HandlerType : byte
		{
			Slap = 0,
			Strangulation = 1,
			SkinSteal = 2
		}

		private DamageHandlerBase _replacedHandler;

		public override float Damage { get; internal set; }

		public override CassieAnnouncement CassieDeathAnnouncement => null;

		public override Footprint Attacker { get; protected set; }

		public override string ServerLogsText => null;

		public override bool AllowSelfDamage => false;

		public HandlerType Subtype { get; private set; }

		public bool StartingRagdoll { get; private set; }

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public string RagdollInspectFormatOverride => null;

		private string StartingDisguiseHint => null;

		private DeathTranslation DeathTranslation => default(DeathTranslation);

		public Scp3114DamageHandler(ReferenceHub attacker, float damage, HandlerType attackType)
		{
		}

		public Scp3114DamageHandler()
		{
		}

		public Scp3114DamageHandler(BasicRagdoll ragdoll, bool isStarting)
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
	}
}
