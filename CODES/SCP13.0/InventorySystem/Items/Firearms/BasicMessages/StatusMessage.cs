using System;
using Mirror;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public struct StatusMessage : NetworkMessage, IEquatable<StatusMessage>
	{
		public ushort Serial;

		public FirearmStatus Status;

		public StatusMessage(ushort serial, FirearmStatus status)
		{
			Serial = 0;
			Status = default(FirearmStatus);
		}

		public void Deserialize(NetworkReader reader)
		{
		}

		public void Serialize(NetworkWriter writer)
		{
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(StatusMessage left, StatusMessage right)
		{
			return false;
		}

		public static bool operator !=(StatusMessage left, StatusMessage right)
		{
			return false;
		}

		public bool Equals(StatusMessage other)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}
	}
}
