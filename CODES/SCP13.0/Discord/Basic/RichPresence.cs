using System;
using System.Collections.Generic;

namespace Discord.Basic
{
	public class RichPresence
	{
		private readonly List<IntPtr> _buffers;

		private RichPresenceStruct _presence;

		public string details;

		public long endTimestamp;

		public bool instance;

		public string joinSecret;

		public string largeImageKey;

		public string largeImageText;

		public string matchSecret;

		public string partyId;

		public int partyMax;

		public int partySize;

		public string smallImageKey;

		public string smallImageText;

		public string spectateSecret;

		public long startTimestamp;

		public string state;

		internal RichPresenceStruct GetStruct()
		{
			return default(RichPresenceStruct);
		}

		private IntPtr StrToPtr(string input)
		{
			return (IntPtr)0;
		}

		internal void FreeMem()
		{
		}
	}
}
