namespace VoiceChat
{
	[global::System.Flags]
	public enum VcMuteFlags
	{
		None = 0,
		LocalRegular = 1,
		LocalIntercom = 2,
		GlobalRegular = 4,
		GlobalIntercom = 8
	}
}
