namespace Subtitles
{
	public struct SubtitlePart
	{
		public SubtitleType Subtitle;

		public string[] OptionalData;

		public SubtitlePart(SubtitleType subtitle, params string[] optionalData)
		{
			Subtitle = default(SubtitleType);
			OptionalData = null;
		}
	}
}
