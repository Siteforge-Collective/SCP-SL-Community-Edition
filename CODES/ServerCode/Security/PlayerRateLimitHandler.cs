namespace Security
{
	public class PlayerRateLimitHandler : global::Mirror.NetworkBehaviour
	{
		public global::Security.RateLimit[] RateLimits;

		private void Awake()
		{
			RateLimits = global::Security.RateLimitCreator.CreateRateLimit(base.connectionToClient, base.isServer && base.isLocalPlayer);
		}

		private void MirrorProcessed()
		{
		}
	}
}
