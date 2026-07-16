using System;

namespace Discord.Basic
{
	[Serializable]
	public struct DiscordUser : IEquatable<DiscordUser>
	{
		public string userId;

		public string username;

		public string discriminator;

		public string avatar;

		public bool Equals(DiscordUser other)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(DiscordUser left, DiscordUser right)
		{
			return false;
		}

		public static bool operator !=(DiscordUser left, DiscordUser right)
		{
			return false;
		}
	}
}
