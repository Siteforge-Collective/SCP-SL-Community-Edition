using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class HumanPingProcessor : IPingProcessor
	{
		private const float HumanRangeSqr = 0.64f;

		private const float HumanHeight = 1.5f;

		private const int HitboxLayer = 8192;

		public float Range => 0f;

		public LayerMask Mask => default(LayerMask);

		public int IconId => 0;

		public bool TryProcess(RaycastHit hit)
		{
			return false;
		}

		public static bool TryGetHuman(Vector3 hitPos, out ReferenceHub best)
		{
			best = null;
			return false;
		}
	}
}
