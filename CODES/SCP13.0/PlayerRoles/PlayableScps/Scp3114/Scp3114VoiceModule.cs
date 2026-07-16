using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using VoiceChat;
using VoiceChat.Playbacks;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114VoiceModule : StandardScpVoiceModule
	{
		[SerializeField]
		private SingleBufferPlayback _proximityPlayback;

		private bool _hintAlreadyDisplayed;

		private const VoiceChatChannel ProximityChannel = VoiceChatChannel.Proximity;

		private const float HumanRangeSqr = 320f;

		public override bool IsSpeaking => false;

		private bool IsDisguised => false;

		private Scp3114Role ScpRole => null;

		private bool AnyHumansNearby => false;

		public event Action OnHintTriggered
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void UpdateHint(bool primary, bool alt)
		{
		}

		protected override VoiceChatChannel ProcessInputs(bool primary, bool alt)
		{
			return default(VoiceChatChannel);
		}

		protected override void ProcessSamples(float[] data, int len)
		{
		}

		public override VoiceChatChannel ValidateSend(VoiceChatChannel channel)
		{
			return default(VoiceChatChannel);
		}

		public override void ResetObject()
		{
		}
	}
}
