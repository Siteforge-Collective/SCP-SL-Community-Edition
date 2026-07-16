using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public static class PumpMessageHandler
	{
		public static void Serialize(this NetworkWriter writer, ShotgunResyncMessage value)
		{
		}

		public static ShotgunResyncMessage Deserialize(this NetworkReader reader)
		{
			return default(ShotgunResyncMessage);
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ClientMsgReceived(ShotgunResyncMessage msg)
		{
		}
	}
}
