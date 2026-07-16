namespace PlayerRoles.RoleAssign
{
	public static class HumanSpawner
	{
		private class RoleHistory
		{
			private int _clock;

			private global::PlayerRoles.RoleTypeId[] _history;

			public global::PlayerRoles.RoleTypeId[] History
			{
				get
				{
					if (_history == null)
					{
						_history = new global::PlayerRoles.RoleTypeId[5];
						for (int i = 0; i < 5; i++)
						{
							_history[i] = global::PlayerRoles.RoleTypeId.None;
						}
					}
					return _history;
				}
			}

			public void RegisterRole(global::PlayerRoles.RoleTypeId role)
			{
				History[_clock++ % 5] = role;
			}
		}

		private const global::PlayerRoles.RoleTypeId DefaultHumanRole = global::PlayerRoles.RoleTypeId.ClassD;

		private const int HistorySize = 5;

		private static global::PlayerRoles.Team[] _humanQueue;

		private static int _queueClock;

		private static int _queueLength;

		private static readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.Team, global::PlayerRoles.RoleAssign.IHumanSpawnHandler> Handlers = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.Team, global::PlayerRoles.RoleAssign.IHumanSpawnHandler>
		{
			[global::PlayerRoles.Team.ClassD] = new global::PlayerRoles.RoleAssign.OneRoleHumanSpawner(global::PlayerRoles.RoleTypeId.ClassD),
			[global::PlayerRoles.Team.FoundationForces] = new global::PlayerRoles.RoleAssign.OneRoleHumanSpawner(global::PlayerRoles.RoleTypeId.FacilityGuard),
			[global::PlayerRoles.Team.Scientists] = new global::PlayerRoles.RoleAssign.OneRoleHumanSpawner(global::PlayerRoles.RoleTypeId.Scientist)
		};

		private static readonly global::System.Collections.Generic.Dictionary<string, global::PlayerRoles.RoleAssign.HumanSpawner.RoleHistory> History = new global::System.Collections.Generic.Dictionary<string, global::PlayerRoles.RoleAssign.HumanSpawner.RoleHistory>();

		private static readonly global::System.Collections.Generic.List<ReferenceHub> Candidates = new global::System.Collections.Generic.List<ReferenceHub>();

		private static global::PlayerRoles.RoleTypeId NextHumanRoleToSpawn
		{
			get
			{
				if (_queueLength == 0)
				{
					throw new global::System.InvalidOperationException("Failed to get next role to spawn, queue has no human roles.");
				}
				global::PlayerRoles.Team key = _humanQueue[_queueClock++ % _queueLength];
				if (!Handlers.TryGetValue(key, out var value))
				{
					return global::PlayerRoles.RoleTypeId.ClassD;
				}
				return value.NextRole;
			}
		}

		public static void SpawnHumans(global::PlayerRoles.Team[] queue, int queueLength)
		{
			_humanQueue = queue;
			_queueClock = 0;
			_queueLength = queueLength;
			int num = global::Utils.NonAllocLINQ.HashsetExtensions.Count(ReferenceHub.AllHubs, global::PlayerRoles.RoleAssign.RoleAssigner.CheckPlayer);
			global::PlayerRoles.RoleTypeId[] array = new global::PlayerRoles.RoleTypeId[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = NextHumanRoleToSpawn;
			}
			array.ShuffleList();
			for (int j = 0; j < num; j++)
			{
				AssignHumanRoleToRandomPlayer(array[j]);
			}
		}

		public static void SpawnLate(ReferenceHub ply)
		{
			ply.roleManager.ServerSetRole(NextHumanRoleToSpawn, global::PlayerRoles.RoleChangeReason.LateJoin);
		}

		private static void AssignHumanRoleToRandomPlayer(global::PlayerRoles.RoleTypeId role)
		{
			Candidates.Clear();
			int num = int.MaxValue;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!global::PlayerRoles.RoleAssign.RoleAssigner.CheckPlayer(allHub))
				{
					continue;
				}
				global::PlayerRoles.RoleAssign.HumanSpawner.RoleHistory orAdd = History.GetOrAdd(allHub.characterClassManager.UserId, () => new global::PlayerRoles.RoleAssign.HumanSpawner.RoleHistory());
				int num2 = 0;
				for (int num3 = 0; num3 < 5; num3++)
				{
					if (orAdd.History[num3] == role)
					{
						num2++;
					}
				}
				if (num2 <= num)
				{
					if (num2 < num)
					{
						Candidates.Clear();
					}
					Candidates.Add(allHub);
					num = num2;
				}
			}
			if (Candidates.Count != 0)
			{
				ReferenceHub referenceHub = Candidates.RandomItem();
				referenceHub.roleManager.ServerSetRole(role, global::PlayerRoles.RoleChangeReason.RoundStart);
				History[referenceHub.characterClassManager.UserId].RegisterRole(role);
			}
		}
	}
}
