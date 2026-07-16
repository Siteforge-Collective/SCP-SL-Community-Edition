namespace PlayerStatsSystem
{
	public class Scp096DamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		public enum AttackType
		{
			GateKill = 0,
			SlapLeft = 1,
			SlapRight = 2,
			Charge = 3
		}

		private static readonly global::System.Collections.Generic.Dictionary<global::PlayerStatsSystem.Scp096DamageHandler.AttackType, string> LogReasons = new global::System.Collections.Generic.Dictionary<global::PlayerStatsSystem.Scp096DamageHandler.AttackType, string>
		{
			[global::PlayerStatsSystem.Scp096DamageHandler.AttackType.SlapLeft] = "Got slapped by SCP-096's left hand",
			[global::PlayerStatsSystem.Scp096DamageHandler.AttackType.SlapRight] = "Got slapped by SCP-096's right hand",
			[global::PlayerStatsSystem.Scp096DamageHandler.AttackType.Charge] = "Stood in a line of SCP-096's charge",
			[global::PlayerStatsSystem.Scp096DamageHandler.AttackType.GateKill] = "Tried to pass through a gate being breached by SCP-096"
		};

		private readonly string _ragdollInspectText;

		private readonly global::PlayerStatsSystem.Scp096DamageHandler.AttackType _attackType;

		public override float Damage { get; protected set; }

		public override global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement CassieDeathAnnouncement => new global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement();

		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override string ServerLogsText => LogReasons[_attackType] + " (" + Attacker.Nickname + ").";

		public override bool AllowSelfDamage => false;

		public Scp096DamageHandler(global::PlayerRoles.PlayableScps.Scp096.Scp096Role attacker, float damage, global::PlayerStatsSystem.Scp096DamageHandler.AttackType attackType)
		{
			Damage = damage;
			if (!(attacker == null) && attacker.TryGetOwner(out var hub))
			{
				_attackType = attackType;
				Attacker = new global::Footprinting.Footprint(hub);
			}
		}

		public override global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput result = base.ApplyDamage(ply);
			global::UnityEngine.Vector3 normalized = (ply.transform.position - Attacker.Hub.transform.position).normalized;
			switch (_attackType)
			{
			case global::PlayerStatsSystem.Scp096DamageHandler.AttackType.Charge:
				StartVelocity = normalized * 8f;
				StartVelocity.y = 3.5f;
				break;
			case global::PlayerStatsSystem.Scp096DamageHandler.AttackType.SlapLeft:
			case global::PlayerStatsSystem.Scp096DamageHandler.AttackType.SlapRight:
			{
				global::UnityEngine.Vector3 right = Attacker.Hub.PlayerCameraReference.right;
				if (_attackType == global::PlayerStatsSystem.Scp096DamageHandler.AttackType.SlapLeft)
				{
					right *= -1f;
				}
				right += Attacker.Hub.transform.forward;
				right += global::UnityEngine.Vector3.up;
				StartVelocity = right * (global::UnityEngine.Random.value + 1.5f) * 3f;
				break;
			}
			case global::PlayerStatsSystem.Scp096DamageHandler.AttackType.GateKill:
				StartVelocity = normalized * 2f;
				StartVelocity.y = -10f;
				break;
			}
			return result;
		}

		public Scp096DamageHandler()
		{
		}
	}
}
