using Mirror;
using RelativePositioning;

namespace InventorySystem.Items.Firearms
{
	public struct GunAudioMessage : NetworkMessage
	{
		public const float OutsideLoudnessMultiplier = 2.3f;

		public ItemType Weapon;

		public ReferenceHub ShooterHub;

		public RelativePosition ShooterPosition;

		public byte AudioClipId;

		public byte MaxDistance;

		public GunAudioMessage(ReferenceHub shooter, byte audioId, byte maxDistance, ReferenceHub target)
		{
			Weapon = default(ItemType);
			ShooterHub = null;
			ShooterPosition = default(RelativePosition);
			AudioClipId = 0;
			MaxDistance = 0;
		}

		public void Deserialize(NetworkReader reader)
		{
		}

		public void Serialize(NetworkWriter writer)
		{
		}
	}
}
