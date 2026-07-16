using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class MicroHidPingProcesssor : IPingProcessor
	{
		private const int PickupLayerMask = 512;

		private const int HitboxLayer = 8192;

		public float Range => 0f;

		public LayerMask Mask => default(LayerMask);

		public int IconId => 0;

		public bool TryProcess(RaycastHit hit)
		{
			return false;
		}

		private bool IsHoldingMicro(ReferenceHub hub)
		{
			return false;
		}
	}
}
