using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.MicroHID
{
	public static class MicroHIDHandler
	{
		public static readonly Dictionary<ushort, float> SyncEnergy;

		public static readonly Dictionary<ushort, HidStatusMessage> SyncStates;

		private static readonly Dictionary<int, AudioSource> Sources;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterServerHandlers()
		{
		}

		private static void ServerReceiveKey(NetworkConnection conn, HidStatusMessage msg)
		{
		}

		private static void RegisterClientHandlers()
		{
		}

		private static void ClientReceiveStatus(HidStatusMessage msg)
		{
		}

		private static void ClientProcessStateSync(ReferenceHub targetPlayer, MicroHIDItem hidReference, HidStatusMessage msg)
		{
		}

		private static void ClientProcessEnergySync(ushort msgSerial, float msgEnergy)
		{
		}
	}
}
