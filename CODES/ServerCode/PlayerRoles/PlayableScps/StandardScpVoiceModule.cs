namespace PlayerRoles.PlayableScps
{
	public class StandardScpVoiceModule : global::PlayerRoles.Voice.GlobalVoiceModuleBase
	{
		protected override global::VoiceChat.VoiceChatChannel PrimaryChannel => global::VoiceChat.VoiceChatChannel.ScpChat;
	}
}
