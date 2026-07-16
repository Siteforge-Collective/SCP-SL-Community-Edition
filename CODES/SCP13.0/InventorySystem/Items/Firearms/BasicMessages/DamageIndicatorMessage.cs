using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.BasicMessages
{
	public struct DamageIndicatorMessage : NetworkMessage
	{
		public byte ReceivedDamage;

		public Vector3 DamageDirection;

		public DamageIndicatorMessage(float damage, Vector3 attackerDirection)
		{
			ReceivedDamage = 0;
			DamageDirection = default(Vector3);
		}
	}
}
