using Mirror;
using UnityEngine;

namespace Subtitles
{
	public static class SubtitleMessageExtensions
	{
		public static void Serialize(this NetworkWriter writer, SubtitleMessage value)
		{
		}

		public static SubtitleMessage Deserialize(this NetworkReader reader)
		{
			return default(SubtitleMessage);
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterHandlers()
		{
		}

		private static void ClientMessageReceived(SubtitleMessage msg)
		{
		}
	}
}
