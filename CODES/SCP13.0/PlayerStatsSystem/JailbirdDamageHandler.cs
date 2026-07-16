using Footprinting;
using UnityEngine;

namespace PlayerStatsSystem
{
	public class JailbirdDamageHandler : AttackerDamageHandler
	{
		private readonly Vector3 _moveDirection;

		private const float ZombieDamageMultiplier = 4f;

		private const float UpwardsForce = 0.02f;

		private const float HorizontalForce = 0.1f;

		public override float Damage { get; internal set; }

		public override Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override string ServerLogsText => null;

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public JailbirdDamageHandler()
		{
		}

		public JailbirdDamageHandler(ReferenceHub attacker, float damage, Vector3 moveDirection)
		{
		}

		public override HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			return default(HandlerOutput);
		}
	}
}
