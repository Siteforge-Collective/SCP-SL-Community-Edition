using Decals;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public struct GunDecalMessage : NetworkMessage
	{
		public DecalPoolType Decal;

		public Vector3 BulletholeOrigin;

		public Vector3 BulletholeDirection;

		public GunDecalMessage(Vector3 origin, Vector3 direction, DecalPoolType decalType)
		{
			Decal = default(DecalPoolType);
			BulletholeOrigin = default(Vector3);
			BulletholeDirection = default(Vector3);
		}
	}
}
