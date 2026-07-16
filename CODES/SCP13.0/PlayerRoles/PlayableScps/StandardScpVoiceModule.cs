using PlayerRoles.Voice;
using VoiceChat;

namespace PlayerRoles.PlayableScps
{
	public class StandardScpVoiceModule : GlobalVoiceModuleBase
	{
		protected override VoiceChatChannel PrimaryChannel => default(VoiceChatChannel);
	}
}
