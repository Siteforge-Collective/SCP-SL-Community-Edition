using UnityEngine;
using VoiceChat;
using VoiceChat.Playbacks;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079VoiceModule : StandardScpVoiceModule
	{
		private Scp079SpeakerAbility _speakerAbility;

		public const VoiceChatChannel SpeakerChannel = VoiceChatChannel.Proximity;

		[field: SerializeField]
		public SingleBufferPlayback ProximityPlayback { get; private set; }

		protected override void Awake()
		{
		}

		protected override VoiceChatChannel ProcessInputs(bool primary, bool alt)
		{
			return default(VoiceChatChannel);
		}

		public override VoiceChatChannel ValidateSend(VoiceChatChannel channel)
		{
			return default(VoiceChatChannel);
		}

		protected override void ProcessSamples(float[] data, int len)
		{
		}
	}
}
