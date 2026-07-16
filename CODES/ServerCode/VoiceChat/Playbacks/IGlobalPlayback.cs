namespace VoiceChat.Playbacks
{
	public interface IGlobalPlayback
	{
		bool GlobalChatActive { get; }

		global::UnityEngine.Color GlobalChatColor { get; }

		string GlobalChatName { get; }

		float GlobalChatLoudness { get; }

		global::VoiceChat.Playbacks.GlobalChatIconType GlobalChatIcon { get; }
	}
}
