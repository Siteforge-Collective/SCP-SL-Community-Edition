namespace PlayerRoles.Voice
{
	public abstract class StandardVoiceModule : global::PlayerRoles.Voice.VoiceModuleBase, global::VoiceChat.Playbacks.IGlobalPlayback
	{
		private const float SustainTime = 0.2f;

		private bool _primHeld;

		private bool _altHeld;

		private readonly global::System.Diagnostics.Stopwatch _primSw = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Diagnostics.Stopwatch _altSw = new global::System.Diagnostics.Stopwatch();

		[global::UnityEngine.SerializeField]
		protected global::VoiceChat.Playbacks.SingleBufferPlayback GlobalPlayback;

		public virtual bool GlobalChatActive => GlobalPlayback.MaxSamples > 0;

		public virtual global::UnityEngine.Color GlobalChatColor => base.Owner.serverRoles.GetVoiceColor();

		public virtual string GlobalChatName => base.Owner.nicknameSync.DisplayName;

		public virtual float GlobalChatLoudness => GlobalPlayback.Loudness;

		public virtual global::VoiceChat.Playbacks.GlobalChatIconType GlobalChatIcon
		{
			get
			{
				if (!IsRoundSummary)
				{
					return global::VoiceChat.Playbacks.GlobalChatIconType.Avatar;
				}
				return global::VoiceChat.Playbacks.GlobalChatIconType.None;
			}
		}

		protected bool IsRoundSummary => RoundSummary.SummaryActive;

		public override global::VoiceChat.VoiceChatChannel GetUserInput()
		{
			global::UnityEngine.KeyCode key = NewInput.GetKey(ActionName.VoiceChat);
			global::UnityEngine.KeyCode key2 = NewInput.GetKey(ActionName.AltVoiceChat);
			return ProcessInputs(ProcessKey(key, ref _primHeld, _primSw), ProcessKey(key2, ref _altHeld, _altSw));
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::VoiceChat.GlobalChatIndicatorManager.Subscribe(this, base.Owner);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_primHeld = false;
			_altHeld = false;
			_primSw.Stop();
			_altSw.Stop();
			global::VoiceChat.GlobalChatIndicatorManager.Unsubscribe(this);
		}

		public override global::VoiceChat.VoiceChatChannel ValidateReceive(ReferenceHub speaker, global::VoiceChat.VoiceChatChannel channel)
		{
			if (speaker == base.Owner)
			{
				return global::VoiceChat.VoiceChatChannel.None;
			}
			if (channel == global::VoiceChat.VoiceChatChannel.Mimicry)
			{
				return channel;
			}
			if (IsRoundSummary)
			{
				if ((base.ReceiveFlags & global::PlayerRoles.Voice.GroupMuteFlags.Summary) == 0)
				{
					return global::VoiceChat.VoiceChatChannel.RoundSummary;
				}
				if (!(speaker.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole))
				{
					return global::VoiceChat.VoiceChatChannel.Spectator;
				}
				return global::VoiceChat.VoiceChatChannel.Proximity;
			}
			if (global::PlayerRoles.Voice.Intercom.CheckPerms(speaker) && channel != global::VoiceChat.VoiceChatChannel.Scp1576)
			{
				return global::VoiceChat.VoiceChatChannel.Intercom;
			}
			return channel;
		}

		protected override void ProcessSamples(float[] data, int len)
		{
			switch (base.CurrentChannel)
			{
			case global::VoiceChat.VoiceChatChannel.RoundSummary:
				GlobalPlayback.Buffer.Write(data, len);
				break;
			case global::VoiceChat.VoiceChatChannel.Intercom:
				global::VoiceChat.Playbacks.IntercomPlayback.ProcessSamples(base.Owner, data, len);
				break;
			}
		}

		protected abstract global::VoiceChat.VoiceChatChannel ProcessInputs(bool primary, bool alt);

		private bool ProcessKey(global::UnityEngine.KeyCode kc, ref bool prev, global::System.Diagnostics.Stopwatch sw)
		{
			if (global::UnityEngine.Input.GetKeyDown(kc))
			{
				prev = true;
			}
			if (!global::UnityEngine.Input.GetKey(kc))
			{
				prev = false;
			}
			if (prev)
			{
				sw.Restart();
			}
			if (sw.IsRunning)
			{
				return sw.Elapsed.TotalSeconds < 0.20000000298023224;
			}
			return false;
		}
	}
}
