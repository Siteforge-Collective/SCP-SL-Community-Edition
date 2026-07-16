using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class GeneratorPingProcessor : IPingProcessor
	{
		public float Range => 0f;

		public LayerMask Mask => default(LayerMask);

		public int IconId => 0;

		public bool TryProcess(RaycastHit hit)
		{
			return false;
		}
	}
}
