using Footprinting;
using Mirror;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using RelativePositioning;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939DamageHandler : AttackerDamageHandler
	{
		private string _ragdollInspect;

		private Scp939DamageType _damageType;

		private RagdollAnimationTemplate _lungeTemplate;

		private RelativePosition _hitPos;

		private bool _lungeTemplateSet;

		private const float LungeUpwardsSpeed = 3.5f;

		private const float LungeTotalSpeed = 5.5f;

		public override bool AllowSelfDamage => false;

		public override float Damage { get; internal set; }

		public override Footprint Attacker { get; protected set; }

		public override string ServerLogsText => null;

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		private RagdollAnimationTemplate LungeTemplate => null;

		public Scp939DamageType Scp939DamageType => default(Scp939DamageType);

		public Scp939DamageHandler(Scp939Role scp939, float damage, Scp939DamageType type = Scp939DamageType.None)
		{
		}

		protected override void ProcessDamage(ReferenceHub ply)
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
