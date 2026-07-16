using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public static class HumanBlockingRewards
	{
		private const float MinDot = 0.5f;

		private const float Cooldown = 5f;

		private const int Reward = 5;

		private const int SqrDistanceCutoff = 400;

		private static double _lastBlockTime;

		private static readonly HashSet<FirstPersonMovementModule> RoomScps;

		private static readonly HashSet<FirstPersonMovementModule> RoomHumans;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ProcessBlockage(Scp079Role role, DoorVariant dv)
		{
		}

		private static bool CheckRoom(RoomIdentifier room, Vector3 doorPos)
		{
			return false;
		}

		private static Vector3 NormalizeIgnoreY(Vector3 direction)
		{
			return default(Vector3);
		}
	}
}
