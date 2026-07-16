namespace PlayerRoles.Spectating
{
	public class SpectatorVoiceModule : global::PlayerRoles.Voice.GlobalVoiceModuleBase
	{
		protected override global::VoiceChat.VoiceChatChannel PrimaryChannel => global::VoiceChat.VoiceChatChannel.Spectator;

		public override global::VoiceChat.Playbacks.GlobalChatIconType GlobalChatIcon => global::VoiceChat.Playbacks.GlobalChatIconType.None;

		public override global::VoiceChat.VoiceChatChannel ValidateReceive(ReferenceHub speaker, global::VoiceChat.VoiceChatChannel channel)
		{
			if (channel != global::VoiceChat.VoiceChatChannel.Scp1576)
			{
				channel = base.ValidateReceive(speaker, channel);
			}
			switch ((int)channel)
			{
			case 1:
			case 2:
			case 6:
			case 8:
				if ((base.ReceiveFlags & global::PlayerRoles.Voice.GroupMuteFlags.Alive) != global::PlayerRoles.Voice.GroupMuteFlags.None)
				{
					return global::VoiceChat.VoiceChatChannel.None;
				}
				break;
			case 4:
				if ((base.ReceiveFlags & global::PlayerRoles.Voice.GroupMuteFlags.Spectators) != global::PlayerRoles.Voice.GroupMuteFlags.None)
				{
					return global::VoiceChat.VoiceChatChannel.None;
				}
				break;
			}
			return channel;
		}

		protected override void ProcessSamples(float[] data, int len)
		{
			if (base.CurrentChannel == global::VoiceChat.VoiceChatChannel.Scp1576)
			{
				global::InventorySystem.Items.Usables.Scp1576.Scp1576Playback.DistributeSamples(base.Owner, data, len);
			}
			else
			{
				base.ProcessSamples(data, len);
			}
		}
	}
}
