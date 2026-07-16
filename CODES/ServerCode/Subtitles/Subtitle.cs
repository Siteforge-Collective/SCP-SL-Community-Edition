namespace Subtitles
{
	[global::System.Serializable]
	public class Subtitle
	{
		public global::Subtitles.SubtitleType SubtitleTypeValue = global::Subtitles.SubtitleType.None;

		public global::Subtitles.CassieAnnouncementType SubtitleCategory = global::Subtitles.CassieAnnouncementType.Normal;

		public string DefaultValue;

		public float Duration;

		public bool RequestSpace = true;

		public float Delay = 2.5f;

		public bool ConvertNumbers;
	}
}
