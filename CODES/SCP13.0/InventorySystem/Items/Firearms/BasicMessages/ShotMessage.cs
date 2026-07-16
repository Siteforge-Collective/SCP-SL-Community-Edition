using Mirror;
using RelativePositioning;
using UnityEngine;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public struct ShotMessage : NetworkMessage
	{
		public uint TargetNetId;

		public RelativePosition TargetPosition;

		public Quaternion TargetRotation;

		public ushort ShooterWeaponSerial;

		public RelativePosition ShooterPosition;

		public Quaternion ShooterCameraRotation;

		public void Deserialize(NetworkReader reader)
		{
		}

		public void Serialize(NetworkWriter writer)
		{
		}
	}
}
