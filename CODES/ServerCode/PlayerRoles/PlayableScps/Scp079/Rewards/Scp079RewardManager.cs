namespace PlayerRoles.PlayableScps.Scp079.Rewards
{
	public class Scp079RewardManager : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::GameObjectPools.IPoolResettable
	{
		private const float MarkDuration = 12f;

		private readonly global::System.Collections.Generic.Dictionary<global::MapGeneration.RoomIdentifier, double> _markedRooms = new global::System.Collections.Generic.Dictionary<global::MapGeneration.RoomIdentifier, double>();

		private static int _cachedRewardSubrtId;

		private static int _cachedTierSubrtId;

		private static bool _cacheSet;

		private static double CurTime => global::Mirror.NetworkTime.time;

		private static void RefreshCache()
		{
			if (_cacheSet)
			{
				return;
			}
			global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayableScps.Scp079.Scp079Role>(global::PlayerRoles.RoleTypeId.Scp079, out var result);
			global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase[] allSubroutines = result.SubroutineModule.AllSubroutines;
			for (int i = 0; i < allSubroutines.Length; i++)
			{
				if (allSubroutines[i] is global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager)
				{
					_cachedRewardSubrtId = i;
				}
				else if (allSubroutines[i] is global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager)
				{
					_cachedTierSubrtId = i;
				}
			}
			_cacheSet = true;
		}

		public void MarkRoom(global::MapGeneration.RoomIdentifier room)
		{
			_markedRooms[room] = CurTime;
		}

		public void MarkRooms(global::MapGeneration.RoomIdentifier[] rooms)
		{
			foreach (global::MapGeneration.RoomIdentifier room in rooms)
			{
				MarkRoom(room);
			}
		}

		public void ResetObject()
		{
			_markedRooms.Clear();
		}

		public static bool GrantExpForRoom(global::MapGeneration.RoomIdentifier room, int reward, global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation gainReason)
		{
			RefreshCache();
			bool result = false;
			foreach (global::PlayerRoles.PlayableScps.Scp079.Scp079Role activeInstance in global::PlayerRoles.PlayableScps.Scp079.Scp079Role.ActiveInstances)
			{
				if ((activeInstance.SubroutineModule.AllSubroutines[_cachedRewardSubrtId] as global::PlayerRoles.PlayableScps.Scp079.Rewards.Scp079RewardManager)._markedRooms.TryGetValue(room, out var value) && !(CurTime - value > 12.0))
				{
					result = true;
					GrantExp(activeInstance, reward, gainReason);
				}
			}
			return result;
		}

		public static void GrantExp(global::PlayerRoles.PlayableScps.Scp079.Scp079Role instance, int reward, global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation gainReason)
		{
			RefreshCache();
			if ((!instance.TryGetOwner(out var hub) || global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079GainExperience, hub, reward, gainReason)) && instance.SubroutineModule.AllSubroutines[_cachedTierSubrtId] is global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager scp079TierManager)
			{
				scp079TierManager.ServerGrantExperience(reward, gainReason);
			}
		}
	}
}
