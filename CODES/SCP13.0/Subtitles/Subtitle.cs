using System;

namespace Subtitles
{
	[Serializable]
	public class Subtitle
	{
		public SubtitleType SubtitleTypeValue;

		public CassieAnnouncementType SubtitleCategory;

		public string DefaultValue;

		public float Duration;

		public bool RequestSpace;

		public float Delay;

		public bool ConvertNumbers;
	}
}
