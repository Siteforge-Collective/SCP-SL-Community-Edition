namespace InventorySystem.Items.Firearms.BasicMessages
{
	public struct StatusMessage : global::Mirror.NetworkMessage, global::System.IEquatable<global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage>
	{
		public ushort Serial;

		public global::InventorySystem.Items.Firearms.FirearmStatus Status;

		public StatusMessage(ushort serial, global::InventorySystem.Items.Firearms.FirearmStatus status)
		{
			Serial = serial;
			Status = status;
		}

		public void Deserialize(global::Mirror.NetworkReader reader)
		{
			byte ammo = reader.ReadByte();
			byte flags = reader.ReadByte();
			uint attachments = global::Mirror.NetworkReaderExtensions.ReadUInt32(reader);
			Serial = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			Status = new global::InventorySystem.Items.Firearms.FirearmStatus(ammo, (global::InventorySystem.Items.Firearms.FirearmStatusFlags)flags, attachments);
		}

		public void Serialize(global::Mirror.NetworkWriter writer)
		{
			writer.WriteByte(Status.Ammo);
			writer.WriteByte((byte)Status.Flags);
			global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, Status.Attachments);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, Serial);
		}

		public override int GetHashCode()
		{
			return (Status.GetHashCode() * 397) ^ Serial;
		}

		public static bool operator ==(global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage left, global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage left, global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage right)
		{
			return !left.Equals(right);
		}

		public bool Equals(global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage other)
		{
			if (Status == other.Status)
			{
				return Serial == other.Serial;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage other)
			{
				return Equals(other);
			}
			return false;
		}
	}
}
