namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public static class TeammateProtectionRewards
	{
		private class TrackedTeammate
		{
			public readonly ReferenceHub Hub;

			public readonly global::PlayerRoles.FirstPersonControl.FpcStandardRoleBase Role;

			private readonly global::System.Collections.Generic.Dictionary<uint, double> _attackers;

			private const float MinDamage = 100f;

			private const float TimeTolerance = 6f;

			private const int AttackersLimit = 5;

			private double _lastDamageTime;

			private float _damageReceived;

			private static readonly global::UnityEngine.Vector3[] AttackersNonAlloc = new global::UnityEngine.Vector3[5];

			public TrackedTeammate(ReferenceHub ply)
			{
				Hub = ply;
				Role = ply.roleManager.CurrentRole as global::PlayerRoles.FirstPersonControl.FpcStandardRoleBase;
				_attackers = new global::System.Collections.Generic.Dictionary<uint, double>();
				Hub.playerStats.OnThisPlayerDamaged += OnDamaged;
			}

			public void Unsubscribe()
			{
				if (!(Hub == null))
				{
					Hub.playerStats.OnThisPlayerDamaged -= OnDamaged;
				}
			}

			public int GetAttackersNonAlloc(out global::UnityEngine.Vector3[] attackersPositions)
			{
				attackersPositions = AttackersNonAlloc;
				if (global::Mirror.NetworkTime.time > _lastDamageTime || _damageReceived < 100f)
				{
					return 0;
				}
				int num = 0;
				foreach (global::System.Collections.Generic.KeyValuePair<uint, double> attacker in _attackers)
				{
					if (!(attacker.Value > _lastDamageTime) && ReferenceHub.TryGetHubNetID(attacker.Key, out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
					{
						AttackersNonAlloc[num] = fpcRole.FpcModule.Position;
						if (++num >= 5)
						{
							break;
						}
					}
				}
				_attackers.Clear();
				_damageReceived = 0f;
				return num;
			}

			private void OnDamaged(global::PlayerStatsSystem.DamageHandlerBase dhb)
			{
				if (dhb is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler && !(dhb is global::PlayerStatsSystem.Scp018DamageHandler) && !(dhb is global::PlayerStatsSystem.ExplosionDamageHandler))
				{
					double time = global::Mirror.NetworkTime.time;
					if (time > _lastDamageTime)
					{
						_damageReceived = 0f;
					}
					_damageReceived += attackerDamageHandler.DealtHealthDamage;
					_lastDamageTime = time + 6.0;
					_attackers[attackerDamageHandler.Attacker.NetId] = _lastDamageTime;
				}
			}
		}

		private const float Cooldown = 10f;

		private static readonly int[] Rewards = new int[6] { 0, 10, 15, 25, 40, 60 };

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Rewards.TeammateProtectionRewards.TrackedTeammate> Teammates = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Rewards.TeammateProtectionRewards.TrackedTeammate>();

		private static double _grantTargetCooldown;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.OnServerAnyDoorInteraction += CheckBlock;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += delegate(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prev, global::PlayerRoles.PlayerRoleBase cur)
			{
				if (global::Mirror.NetworkServer.active)
				{
					if (ValidateRole(prev))
					{
						Teammates.RemoveWhere((global::PlayerRoles.PlayableScps.Scp079.Rewards.TeammateProtectionRewards.TrackedTeammate x) => x.Hub == hub);
					}
					if (ValidateRole(cur))
					{
						Teammates.Add(new global::PlayerRoles.PlayableScps.Scp079.Rewards.TeammateProtectionRewards.TrackedTeammate(hub));
					}
				}
			};
			ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, (global::System.Action<ReferenceHub>)delegate(ReferenceHub hub)
			{
				if (global::Mirror.NetworkServer.active && ValidateRole(hub.roleManager.CurrentRole))
				{
					Teammates.RemoveWhere((global::PlayerRoles.PlayableScps.Scp079.Rewards.TeammateProtectionRewards.TrackedTeammate x) => x.Hub == hub);
				}
			});
		}

		private static bool ValidateRole(global::PlayerRoles.PlayerRoleBase prb)
		{
			if (prb is global::PlayerRoles.FirstPersonControl.FpcStandardRoleBase)
			{
				return prb.Team == global::PlayerRoles.Team.SCPs;
			}
			return false;
		}

		private static void CheckBlock(global::PlayerRoles.PlayableScps.Scp079.Scp079Role scp079, global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
		{
			if (dv.TargetState)
			{
				return;
			}
			global::Interactables.Interobjects.DoorUtils.DoorLockReason activeLocks = (global::Interactables.Interobjects.DoorUtils.DoorLockReason)dv.ActiveLocks;
			if ((!global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(activeLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown079) && !global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(activeLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Regular079)) || _grantTargetCooldown > global::Mirror.NetworkTime.time)
			{
				return;
			}
			int num = 0;
			global::UnityEngine.Transform transform = dv.transform;
			foreach (global::PlayerRoles.PlayableScps.Scp079.Rewards.TeammateProtectionRewards.TrackedTeammate teammate in Teammates)
			{
				global::UnityEngine.Vector3[] attackersPositions;
				int attackersNonAlloc = teammate.GetAttackersNonAlloc(out attackersPositions);
				if (attackersNonAlloc == 0)
				{
					continue;
				}
				bool flag = transform.InverseTransformPoint(teammate.Role.FpcModule.Position).z > 0f;
				for (int i = 0; i < attackersNonAlloc; i++)
				{
					bool flag2 = transform.InverseTransformPoint(attackersPositions[i]).z > 0f;
					if (flag != flag2)
					{
						num++;
					}
				}
			}
			int num2 = global::UnityEngine.Mathf.Min(num, Rewards.Length - 1);
			global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager.GrantExp(scp079, Rewards[num2], global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTeammateProtection);
			_grantTargetCooldown = global::Mirror.NetworkTime.time + 10.0;
		}
	}
}
