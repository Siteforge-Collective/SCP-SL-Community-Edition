using System;
using Mirror;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	[Serializable]
	public struct ReflexSightSyncMessage : NetworkMessage
	{
		public readonly ushort WeaponSerial;

		public readonly int AttachmentId;

		public readonly int TextureId;

		public readonly int ColorId;

		public readonly int SizeId;

		public ReflexSightSyncMessage(ReflexSightAttachment att)
		{
			WeaponSerial = 0;
			AttachmentId = 0;
			TextureId = 0;
			ColorId = 0;
			SizeId = 0;
		}

		public ReflexSightSyncMessage(NetworkReader reader)
		{
			WeaponSerial = 0;
			AttachmentId = 0;
			TextureId = 0;
			ColorId = 0;
			SizeId = 0;
		}

		public void Write(NetworkWriter writer)
		{
		}

		public override string ToString()
		{
			return null;
		}
	}
}
