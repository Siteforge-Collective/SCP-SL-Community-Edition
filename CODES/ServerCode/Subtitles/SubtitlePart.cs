namespace Subtitles
{
	public struct SubtitlePart
	{
		public global::Subtitles.SubtitleType Subtitle;

		public string[] OptionalData;

		public SubtitlePart(global::Subtitles.SubtitleType subtitle, params string[] optionalData)
		{
			Subtitle = subtitle;
			OptionalData = optionalData;
		}
	}
}
