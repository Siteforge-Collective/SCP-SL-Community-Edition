using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.ToggleableLights
{
	public static class FlashlightNetworkHandler
	{
		public readonly struct FlashlightMessage : NetworkMessage
		{
			public readonly ushort Serial;

			public readonly bool NewState;

			public FlashlightMessage(ushort flashlightSerial, bool newState)
			{
				Serial = 0;
				NewState = false;
			}
		}

		private static readonly HashSet<uint> AlreadyRequestedFirstimeSync;

		public static readonly Dictionary<ushort, bool> ReceivedStatuses;

		public static event Action<FlashlightMessage> OnStatusReceived
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterHandlers()
		{
		}

		private static void ClientProcessMessage(FlashlightMessage msg)
		{
		}

		private static void ServerProcessMessage(NetworkConnection conn, FlashlightMessage msg)
		{
		}

		private static void ServerProcessFirstTimeRequest(NetworkConnection conn)
		{
		}

		public static void Serialize(this NetworkWriter writer, FlashlightMessage value)
		{
		}

		public static FlashlightMessage Deserialize(this NetworkReader reader)
		{
			return default(FlashlightMessage);
		}
	}
}
