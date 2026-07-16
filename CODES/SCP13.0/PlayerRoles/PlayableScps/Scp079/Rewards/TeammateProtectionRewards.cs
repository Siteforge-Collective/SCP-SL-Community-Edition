using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public static class TeammateProtectionRewards
	{
		private class TrackedTeammate
		{
			public readonly ReferenceHub Hub;

			public readonly FpcStandardRoleBase Role;

			private readonly Dictionary<uint, double> _attackers;

			private const float MinDamage = 100f;

			private const float TimeTolerance = 6f;

			private const int AttackersLimit = 5;

			private double _lastDamageTime;

			private float _damageReceived;

			private static readonly Vector3[] AttackersNonAlloc;

			public TrackedTeammate(ReferenceHub ply)
			{
			}

			public void Unsubscribe()
			{
			}

			public int GetAttackersNonAlloc(out Vector3[] attackersPositions)
			{
				attackersPositions = null;
				return 0;
			}

			private void OnDamaged(DamageHandlerBase dhb)
			{
			}
		}

		private const float Cooldown = 10f;

		private static readonly int[] Rewards;

		private static readonly HashSet<TrackedTeammate> Teammates;

		private static double _grantTargetCooldown;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static bool ValidateRole(PlayerRoleBase prb)
		{
			return false;
		}

		private static void CheckBlock(Scp079Role scp079, DoorVariant dv)
		{
		}
	}
}
