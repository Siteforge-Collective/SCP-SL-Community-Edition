namespace PlayerRoles.Voice
{
	public class LobbyVoiceModule : global::PlayerRoles.Spectating.SpectatorVoiceModule
	{
		private global::VoiceChat.VoiceChatChannel ValidateAuth(global::VoiceChat.VoiceChatChannel channelToValidate)
		{
			if (base.Owner.characterClassManager.InstanceMode != ClientInstanceMode.Unverified)
			{
				return channelToValidate;
			}
			return global::VoiceChat.VoiceChatChannel.None;
		}

		public override global::VoiceChat.VoiceChatChannel ValidateSend(global::VoiceChat.VoiceChatChannel channel)
		{
			return ValidateAuth(base.ValidateSend(channel));
		}

		public override global::VoiceChat.VoiceChatChannel ValidateReceive(ReferenceHub sp, global::VoiceChat.VoiceChatChannel ch)
		{
			if (base.Owner.isLocalPlayer)
			{
				return global::VoiceChat.VoiceChatChannel.None;
			}
			if ((base.ReceiveFlags & global::PlayerRoles.Voice.GroupMuteFlags.Lobby) != global::PlayerRoles.Voice.GroupMuteFlags.None)
			{
				return global::VoiceChat.VoiceChatChannel.None;
			}
			return ValidateAuth(base.ValidateReceive(sp, ch));
		}
	}
}
