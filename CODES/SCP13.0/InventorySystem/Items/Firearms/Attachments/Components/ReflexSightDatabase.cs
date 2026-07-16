using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public static class ReflexSightDatabase
	{
		public static Dictionary<ushort, Dictionary<int, ReflexSightSyncMessage>> Database;

		private static readonly HashSet<int> DirtyValues;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void HandleNewClient(ReferenceHub hub)
		{
		}

		private static void ClientReceiveMessage(ReflexSightSyncMessage msg)
		{
		}

		private static void ServerReceiveMessage(NetworkConnection conn, ReflexSightSyncMessage msg)
		{
		}

		private static void AddMessage(ReflexSightSyncMessage msg)
		{
		}

		private static void ServerUpdate()
		{
		}
	}
}
