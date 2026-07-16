using Discord.Modules;
using UnityEngine;

namespace Discord
{
	public class DiscordManager : MonoBehaviour
	{
		public const long ApplicationId = 420676877766623232L;

		public const string OptionalSteamId = "700330";

		private static bool _singletonSet;

		public static DiscordManager Singleton { get; private set; }

		[field: SerializeField]
		public RichPresenceModule RichPresence { get; private set; }

		[field: SerializeField]
		public RequestableJoinModule JoinModule { get; private set; }

		[field: SerializeField]
		public DebugModule DebugModule { get; private set; }

		[field: SerializeField]
		public CustomNetworkManager NetworkManager { get; set; }

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void SetEnabled(bool value)
		{
		}
	}
}
