using System;

namespace Discord.Basic
{
	[Serializable]
	public struct RichPresenceStruct : IEquatable<RichPresenceStruct>
	{
		public IntPtr state;

		public IntPtr details;

		public long startTimestamp;

		public long endTimestamp;

		public IntPtr largeImageKey;

		public IntPtr largeImageText;

		public IntPtr smallImageKey;

		public IntPtr smallImageText;

		public IntPtr partyId;

		public int partySize;

		public int partyMax;

		public IntPtr matchSecret;

		public IntPtr joinSecret;

		public IntPtr spectateSecret;

		public bool instance;

		public bool Equals(RichPresenceStruct other)
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

		public static bool operator ==(RichPresenceStruct left, RichPresenceStruct right)
		{
			return false;
		}

		public static bool operator !=(RichPresenceStruct left, RichPresenceStruct right)
		{
			return false;
		}
	}
}
