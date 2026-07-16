namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079VoiceModule : global::PlayerRoles.PlayableScps.StandardScpVoiceModule
	{
		private global::PlayerRoles.PlayableScps.Scp079.Scp079SpeakerAbility _speakerAbility;

		public const global::VoiceChat.VoiceChatChannel SpeakerChannel = global::VoiceChat.VoiceChatChannel.Proximity;

		[field: global::UnityEngine.SerializeField]
		public global::VoiceChat.Playbacks.SingleBufferPlayback ProximityPlayback { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			(base.Role as global::PlayerRoles.PlayableScps.Scp079.Scp079Role).SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079SpeakerAbility>(out _speakerAbility);
		}

		protected override global::VoiceChat.VoiceChatChannel ProcessInputs(bool primary, bool alt)
		{
			if (alt && _speakerAbility.CanTransmit)
			{
				return global::VoiceChat.VoiceChatChannel.Proximity;
			}
			if (primary)
			{
				return PrimaryChannel;
			}
			return global::VoiceChat.VoiceChatChannel.None;
		}

		public override global::VoiceChat.VoiceChatChannel ValidateSend(global::VoiceChat.VoiceChatChannel channel)
		{
			if (channel != global::VoiceChat.VoiceChatChannel.Proximity || !_speakerAbility.CanTransmit)
			{
				return base.ValidateSend(channel);
			}
			return global::VoiceChat.VoiceChatChannel.Proximity;
		}

		protected override void ProcessSamples(float[] data, int len)
		{
			if (base.CurrentChannel == global::VoiceChat.VoiceChatChannel.Proximity)
			{
				ProximityPlayback.Buffer.Write(data, len);
			}
			else
			{
				base.ProcessSamples(data, len);
			}
		}
	}
}
