namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public static class HumanBlockingRewards
	{
		private const float MinDot = 0.5f;

		private const float Cooldown = 5f;

		private const int Reward = 5;

		private const int SqrDistanceCutoff = 400;

		private static double _lastBlockTime;

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule> RoomScps = new global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule>();

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule> RoomHumans = new global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.OnServerAnyDoorInteraction += ProcessBlockage;
		}

		private static void ProcessBlockage(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
		{
			if (dv.TargetState || _lastBlockTime + 5.0 > global::Mirror.NetworkTime.time)
			{
				return;
			}
			global::UnityEngine.Vector3 position = dv.transform.position;
			global::MapGeneration.RoomIdentifier[] rooms = dv.Rooms;
			for (int i = 0; i < rooms.Length; i++)
			{
				if (CheckRoom(rooms[i], position))
				{
					global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager.GrantExp(role, 5, global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainBlockingHuman);
					_lastBlockTime = global::Mirror.NetworkTime.time;
					break;
				}
			}
		}

		private static bool CheckRoom(global::MapGeneration.RoomIdentifier room, global::UnityEngine.Vector3 doorPos)
		{
			RoomScps.Clear();
			RoomHumans.Clear();
			bool flag = false;
			bool flag2 = false;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
				{
					continue;
				}
				global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = fpcRole.FpcModule;
				if (!(global::MapGeneration.RoomIdUtils.RoomAtPosition(fpcModule.Position) != room))
				{
					if (allHub.IsSCP())
					{
						RoomScps.Add(fpcModule);
						flag = true;
					}
					else
					{
						RoomHumans.Add(fpcModule);
						flag2 = true;
					}
				}
			}
			if (!flag2 || !flag)
			{
				return false;
			}
			foreach (global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule roomScp in RoomScps)
			{
				global::UnityEngine.Vector3 lhs = NormalizeIgnoreY(roomScp.Motor.MoveDirection.normalized);
				global::UnityEngine.Vector3 rhs = NormalizeIgnoreY(doorPos - roomScp.Position);
				if (global::UnityEngine.Vector3.Dot(lhs, rhs) < 0.5f)
				{
					continue;
				}
				foreach (global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule roomHuman in RoomHumans)
				{
					global::UnityEngine.Vector3 direction = roomHuman.Position - roomScp.Position;
					if (!(direction.sqrMagnitude > 400f) && global::UnityEngine.Vector3.Dot(lhs, NormalizeIgnoreY(direction)) >= 0.5f)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static global::UnityEngine.Vector3 NormalizeIgnoreY(global::UnityEngine.Vector3 direction)
		{
			direction.y = 0f;
			return direction.normalized;
		}
	}
}
