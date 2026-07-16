namespace PlayerRoles.RoleAssign
{
	public static class ScpPlayerPicker
	{
		private class PotentialScp
		{
			public global::PlayerRoles.RoleAssign.TicketEntry Entry;

			public long Weight;
		}

		private const int DefaultTickets = 10;

		private const int HumanTicketBonus = 2;

		private static readonly global::System.Collections.Generic.Dictionary<string, global::PlayerRoles.RoleAssign.TicketEntry> LoadedTickets = new global::System.Collections.Generic.Dictionary<string, global::PlayerRoles.RoleAssign.TicketEntry>();

		private static readonly global::System.Collections.Generic.List<global::PlayerRoles.RoleAssign.TicketEntry> ScpsToSpawn = new global::System.Collections.Generic.List<global::PlayerRoles.RoleAssign.TicketEntry>();

		private static readonly global::System.Collections.Generic.List<ReferenceHub> ChoosenPlayers = new global::System.Collections.Generic.List<ReferenceHub>();

		public static global::System.Collections.Generic.List<ReferenceHub> ChoosePlayers(int targetScps)
		{
			ChoosenPlayers.Clear();
			foreach (global::PlayerRoles.RoleAssign.TicketEntry item in GenerateList(targetScps))
			{
				item.Tickets = 10;
				if (global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(ReferenceHub.AllHubs, (ReferenceHub x) => x.characterClassManager.UserId == item.UserId, out var first))
				{
					ChoosenPlayers.Add(first);
				}
			}
			if (targetScps != ChoosenPlayers.Count)
			{
				throw new global::System.InvalidOperationException("Failed to meet target number of SCPs.");
			}
			return ChoosenPlayers;
		}

		private static global::System.Collections.Generic.List<global::PlayerRoles.RoleAssign.TicketEntry> GenerateList(int scpsToAssign)
		{
			ScpsToSpawn.Clear();
			if (scpsToAssign <= 0)
			{
				return ScpsToSpawn;
			}
			LoadedTickets.Clear();
			global::System.Collections.Generic.HashSet<string> hashSet = global::NorthwoodLib.Pools.HashSetPool<string>.Shared.Rent();
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (global::PlayerRoles.RoleAssign.RoleAssigner.CheckPlayer(allHub))
				{
					hashSet.Add(allHub.characterClassManager.UserId);
				}
			}
			foreach (global::PlayerRoles.RoleAssign.TicketEntry allEntry in global::PlayerRoles.RoleAssign.ScpTicketsLoader.AllEntries)
			{
				if (hashSet.Remove(allEntry.UserId))
				{
					LoadedTickets[allEntry.UserId] = allEntry;
					allEntry.Tickets += 2;
				}
			}
			foreach (string item2 in hashSet)
			{
				global::PlayerRoles.RoleAssign.TicketEntry ticketEntry = new global::PlayerRoles.RoleAssign.TicketEntry
				{
					UserId = item2,
					Tickets = 12
				};
				LoadedTickets[item2] = ticketEntry;
				global::PlayerRoles.RoleAssign.ScpTicketsLoader.AllEntries.Add(ticketEntry);
			}
			global::NorthwoodLib.Pools.HashSetPool<string>.Shared.Return(hashSet);
			int num = 0;
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::PlayerRoles.RoleAssign.TicketEntry> loadedTicket in LoadedTickets)
			{
				int tickets = loadedTicket.Value.Tickets;
				if (tickets >= num)
				{
					if (tickets > num)
					{
						ScpsToSpawn.Clear();
					}
					num = tickets;
					ScpsToSpawn.Add(loadedTicket.Value);
				}
			}
			if (ScpsToSpawn.Count > 1)
			{
				global::PlayerRoles.RoleAssign.TicketEntry item = ScpsToSpawn.RandomItem();
				ScpsToSpawn.Clear();
				ScpsToSpawn.Add(item);
			}
			scpsToAssign -= ScpsToSpawn.Count;
			if (scpsToAssign <= 0)
			{
				return ScpsToSpawn;
			}
			global::System.Collections.Generic.List<global::PlayerRoles.RoleAssign.ScpPlayerPicker.PotentialScp> list = global::NorthwoodLib.Pools.ListPool<global::PlayerRoles.RoleAssign.ScpPlayerPicker.PotentialScp>.Shared.Rent();
			long num2 = 0L;
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::PlayerRoles.RoleAssign.TicketEntry> loadedTicket2 in LoadedTickets)
			{
				if (!ScpsToSpawn.Contains(loadedTicket2.Value))
				{
					long num3 = 1L;
					int tickets2 = loadedTicket2.Value.Tickets;
					for (int i = 0; i < scpsToAssign; i++)
					{
						num3 *= tickets2;
					}
					list.Add(new global::PlayerRoles.RoleAssign.ScpPlayerPicker.PotentialScp
					{
						Entry = loadedTicket2.Value,
						Weight = num3
					});
					num2 += num3;
				}
			}
			while (scpsToAssign > 0)
			{
				double num4 = (double)global::UnityEngine.Random.value * (double)num2;
				for (int j = 0; j < list.Count; j++)
				{
					global::PlayerRoles.RoleAssign.ScpPlayerPicker.PotentialScp potentialScp = list[j];
					num4 -= (double)potentialScp.Weight;
					if (!(num4 > 0.0))
					{
						scpsToAssign--;
						ScpsToSpawn.Add(potentialScp.Entry);
						list.RemoveAt(j);
						num2 -= potentialScp.Weight;
						break;
					}
				}
			}
			global::NorthwoodLib.Pools.ListPool<global::PlayerRoles.RoleAssign.ScpPlayerPicker.PotentialScp>.Shared.Return(list);
			return ScpsToSpawn;
		}
	}
}
