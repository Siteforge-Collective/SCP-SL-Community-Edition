using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Radio
{
	public static class RadioMessages
	{
		public enum RadioCommand : byte
		{
			Enable = 0,
			Disable = 1,
			ChangeRange = 2
		}

		public enum RadioRangeLevel : sbyte
		{
			RadioDisabled = -1,
			LowRange = 0,
			MediumRange = 1,
			HighRange = 2,
			UltraRange = 3
		}

		public static readonly Dictionary<uint, RadioStatusMessage> SyncedRangeLevels;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ServerCommandReceived(NetworkConnection conn, ClientRadioCommandMessage msg)
		{
		}

		private static void ClientStatusReceived(RadioStatusMessage msg)
		{
		}

		private static bool GetRadio(ReferenceHub ply, out RadioItem radio)
		{
			radio = null;
			return false;
		}

		public static void WriteRadioStatusMessage(this NetworkWriter writer, RadioStatusMessage msg)
		{
		}

		public static RadioStatusMessage ReadRadioStatusMessage(this NetworkReader reader)
		{
			return default(RadioStatusMessage);
		}

		public static void WriteClientRadioCommandMessage(this NetworkWriter writer, ClientRadioCommandMessage msg)
		{
		}

		public static ClientRadioCommandMessage ReadClientRadioCommandMessage(this NetworkReader reader)
		{
			return default(ClientRadioCommandMessage);
		}
	}
}
