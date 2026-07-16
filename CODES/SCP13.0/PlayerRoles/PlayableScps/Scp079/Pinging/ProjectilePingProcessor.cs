using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class ProjectilePingProcessor : IPingProcessor
	{
		private const float Radius = 0.9f;

		private const int Detections = 8;

		private static readonly Collider[] Hit;

		public float Range => 0f;

		public LayerMask Mask => default(LayerMask);

		public int IconId { get; private set; }

		public bool TryProcess(RaycastHit hit)
		{
			return false;
		}
	}
}
