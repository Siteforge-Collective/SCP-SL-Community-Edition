namespace PlayerStatsSystem
{
	public class Scp018DamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		private readonly string _deathScreenText;

		private readonly string _serverLogsText;

		private readonly string _ragdollInspectText;

		private readonly global::UnityEngine.Vector3 _ballImpactVelocity;

		private const float ForceMultiplier = 0.5f;

		private const float HipMultiplier = 3f;

		public override float Damage { get; protected set; }

		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => true;

		public override string ServerLogsText => _serverLogsText;

		public override bool IgnoreFriendlyFireDetector => true;

		public override global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput result = base.ApplyDamage(ply);
			StartVelocity = _ballImpactVelocity * 0.5f;
			return result;
		}

		public override void ProcessRagdoll(BasicRagdoll ragdoll)
		{
			base.ProcessRagdoll(ragdoll);
			if (!(ragdoll is global::PlayerRoles.Ragdolls.DynamicRagdoll dynamicRagdoll))
			{
				return;
			}
			global::PlayerRoles.Ragdolls.HitboxData[] hitboxes = dynamicRagdoll.Hitboxes;
			for (int i = 0; i < hitboxes.Length; i++)
			{
				global::PlayerRoles.Ragdolls.HitboxData hitboxData = hitboxes[i];
				if (hitboxData.RelatedHitbox == HitboxType.Body)
				{
					hitboxData.Target.velocity *= 3f;
				}
			}
		}

		public Scp018DamageHandler(global::InventorySystem.Items.ThrowableProjectiles.Scp018Projectile ball, float dmg, bool ignoreFF)
		{
			if (dmg != 0f)
			{
				_ballImpactVelocity = (ball.TryGetComponent<global::UnityEngine.Rigidbody>(out var component) ? component.velocity : global::UnityEngine.Vector3.zero);
				_serverLogsText = "SCP-018 thrown by: " + ball.PreviousOwner.Nickname;
				Attacker = ball.PreviousOwner;
				Damage = dmg;
				ForceFullFriendlyFire = ignoreFF;
			}
		}
	}
}
