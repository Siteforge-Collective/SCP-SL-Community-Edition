namespace PlayerRoles.RoleAssign
{
	public static class ScpSpawner
	{
		private static readonly global::System.Collections.Generic.Dictionary<ReferenceHub, float> SelectedSpawnChances = new global::System.Collections.Generic.Dictionary<ReferenceHub, float>();

		private static readonly global::System.Collections.Generic.Dictionary<ReferenceHub, float> ChancesBuffer = new global::System.Collections.Generic.Dictionary<ReferenceHub, float>();

		private static readonly global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> BackupScps = new global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId>(8);

		private static readonly global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> EnqueuedScps = new global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId>(8);

		private static global::PlayerRoles.PlayerRoleBase[] _cachedSpawnableScps;

		private static float[] _chancesArray;

		private static bool _cacheSet;

		private static global::PlayerRoles.PlayerRoleBase[] SpawnableScps
		{
			get
			{
				if (_cacheSet)
				{
					return _cachedSpawnableScps;
				}
				global::System.Collections.Generic.List<global::PlayerRoles.PlayerRoleBase> list = new global::System.Collections.Generic.List<global::PlayerRoles.PlayerRoleBase>();
				foreach (global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase> allRole in global::PlayerRoles.PlayerRoleLoader.AllRoles)
				{
					if (allRole.Value is global::PlayerRoles.PlayableScps.ISpawnableScp)
					{
						list.Add(allRole.Value);
					}
				}
				_cacheSet = true;
				_chancesArray = new float[list.Count];
				return _cachedSpawnableScps = list.ToArray();
			}
		}

		private static global::PlayerRoles.RoleTypeId NextScp
		{
			get
			{
				float num = 0f;
				int num2 = SpawnableScps.Length;
				for (int i = 0; i < num2; i++)
				{
					global::PlayerRoles.PlayerRoleBase playerRoleBase = SpawnableScps[i];
					if (EnqueuedScps.Contains(playerRoleBase.RoleTypeId))
					{
						_chancesArray[i] = 0f;
						continue;
					}
					float spawnChance = (SpawnableScps[i] as global::PlayerRoles.PlayableScps.ISpawnableScp).GetSpawnChance(EnqueuedScps);
					spawnChance = global::UnityEngine.Mathf.Max(spawnChance, 0f);
					num += spawnChance;
					_chancesArray[i] = spawnChance;
				}
				if (num == 0f)
				{
					return RandomLeastFrequentScp;
				}
				float num3 = global::UnityEngine.Random.Range(0f, num);
				for (int j = 0; j < num2; j++)
				{
					num3 -= _chancesArray[j];
					if (!(num3 >= 0f))
					{
						return SpawnableScps[j].RoleTypeId;
					}
				}
				return SpawnableScps[num2 - 1].RoleTypeId;
			}
		}

		private static global::PlayerRoles.RoleTypeId RandomLeastFrequentScp
		{
			get
			{
				int num = SpawnableScps.Length;
				int num2 = int.MaxValue;
				for (int i = 0; i < num; i++)
				{
					global::PlayerRoles.RoleTypeId roleTypeId = SpawnableScps[i].RoleTypeId;
					int num3 = 0;
					foreach (global::PlayerRoles.RoleTypeId enqueuedScp in EnqueuedScps)
					{
						if (enqueuedScp == roleTypeId)
						{
							num3++;
						}
					}
					if (num3 <= num2)
					{
						if (num3 < num2)
						{
							BackupScps.Clear();
						}
						BackupScps.Add(roleTypeId);
						num2 = num3;
					}
				}
				return BackupScps.RandomItem();
			}
		}

		public static void SpawnScps(int targetScpNumber)
		{
			EnqueuedScps.Clear();
			for (int i = 0; i < targetScpNumber; i++)
			{
				EnqueuedScps.Add(NextScp);
			}
			global::System.Collections.Generic.List<ReferenceHub> chosenPlayers = global::PlayerRoles.RoleAssign.ScpPlayerPicker.ChoosePlayers(targetScpNumber);
			while (EnqueuedScps.Count > 0)
			{
				global::PlayerRoles.RoleTypeId scp = EnqueuedScps[0];
				EnqueuedScps.RemoveAt(0);
				AssignScp(chosenPlayers, scp, EnqueuedScps);
			}
		}

		private static void AssignScp(global::System.Collections.Generic.List<ReferenceHub> chosenPlayers, global::PlayerRoles.RoleTypeId scp, global::System.Collections.Generic.List<global::PlayerRoles.RoleTypeId> otherScps)
		{
			ChancesBuffer.Clear();
			int num = 1;
			int num2 = 0;
			foreach (ReferenceHub chosenPlayer in chosenPlayers)
			{
				int num3 = GetPreferenceOfPlayer(chosenPlayer, scp);
				foreach (global::PlayerRoles.RoleTypeId otherScp in otherScps)
				{
					num3 -= GetPreferenceOfPlayer(chosenPlayer, otherScp);
				}
				num2++;
				ChancesBuffer[chosenPlayer] = num3;
				num = global::UnityEngine.Mathf.Min(num3, num);
			}
			float num4 = 0f;
			SelectedSpawnChances.Clear();
			foreach (global::System.Collections.Generic.KeyValuePair<ReferenceHub, float> item in ChancesBuffer)
			{
				float num5 = global::UnityEngine.Mathf.Pow(item.Value - (float)num + 1f, num2);
				SelectedSpawnChances[item.Key] = num5;
				num4 += num5;
			}
			float num6 = num4 * global::UnityEngine.Random.value;
			float num7 = 0f;
			foreach (global::System.Collections.Generic.KeyValuePair<ReferenceHub, float> selectedSpawnChance in SelectedSpawnChances)
			{
				num7 += selectedSpawnChance.Value;
				if (!(num7 < num6))
				{
					ReferenceHub key = selectedSpawnChance.Key;
					chosenPlayers.Remove(key);
					key.roleManager.ServerSetRole(scp, global::PlayerRoles.RoleChangeReason.RoundStart);
					break;
				}
			}
		}

		private static int GetPreferenceOfPlayer(ReferenceHub ply, global::PlayerRoles.RoleTypeId scp)
		{
			int connectionId = ply.connectionToClient.connectionId;
			if (!global::PlayerRoles.RoleAssign.ScpSpawnPreferences.Preferences.TryGetValue(connectionId, out var value))
			{
				return 0;
			}
			if (!value.Preferences.TryGetValue(scp, out var value2))
			{
				return 0;
			}
			return value2;
		}
	}
}
