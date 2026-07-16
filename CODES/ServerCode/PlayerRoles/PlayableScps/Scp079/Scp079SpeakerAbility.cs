namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079SpeakerAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase
	{
		private string _abilityName;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079VoiceModule _voiceModule;

		public override float AuxRegenMultiplier
		{
			get
			{
				if (!_voiceModule.ServerIsSending || _voiceModule.CurrentChannel != global::VoiceChat.VoiceChatChannel.Proximity)
				{
					return 1f;
				}
				if ((global::VoiceChat.VoiceChatMutes.GetFlags(base.Owner) & (global::VoiceChat.VcMuteFlags.LocalRegular | global::VoiceChat.VcMuteFlags.GlobalRegular)) == 0)
				{
					return 0f;
				}
				return 1f;
			}
		}

		public bool CanTransmit => !base.LostSignalHandler.Lost;

		public override ActionName ActivationKey => ActionName.AltVoiceChat;

		public override bool IsReady => true;

		public override bool IsVisible
		{
			get
			{
				if (!global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapVisible)
				{
					return CanTransmit;
				}
				return false;
			}
		}

		public override string AbilityName => _abilityName;

		public override string FailMessage => null;

		protected override void Trigger()
		{
		}

		protected override void Awake()
		{
			base.Awake();
			_abilityName = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.UseSpeaker);
			base.CurrentCamSync.OnCameraChanged += RefreshNearestSpeaker;
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_voiceModule = base.ScpRole.VoiceModule as global::PlayerRoles.PlayableScps.Scp079.Scp079VoiceModule;
			RefreshNearestSpeaker();
		}

		private void RefreshNearestSpeaker()
		{
			if (base.CurrentCamSync.TryGetCurrentCamera(out var cam) && global::PlayerRoles.PlayableScps.Scp079.Scp079Speaker.TryGetSpeaker(cam, out var best))
			{
				_voiceModule.ProximityPlayback.transform.position = best.Position;
			}
		}
	}
}
