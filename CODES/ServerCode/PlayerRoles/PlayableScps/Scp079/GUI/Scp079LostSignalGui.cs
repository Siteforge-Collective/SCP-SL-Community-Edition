namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079LostSignalGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::System.Serializable]
		private struct ZoneClip
		{
			public global::MapGeneration.FacilityZone Zone;

			public global::UnityEngine.AudioClip Clip;
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _rootObj;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _loopSource;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _etaText;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079LostSignalGui.ZoneClip[] _zoneStarts;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _fallbackStart;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079LostSignalGui.ZoneClip[] _zoneLoops;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _fallbackLoop;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler _handler;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		private string _textFormat;

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler>(out _handler);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
			_handler.OnStatusChanged += UpdateScreen;
			_textFormat = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ReconnectingEta);
		}

		private void OnDestroy()
		{
			_handler.OnStatusChanged -= UpdateScreen;
		}

		private void UpdateScreen()
		{
			if (!_handler.Lost)
			{
				_rootObj.SetActive(value: false);
				return;
			}
			_rootObj.SetActive(value: true);
			global::MapGeneration.FacilityZone zone = _curCamSync.CurrentCamera.Room.Zone;
			PlayStart(zone);
			PlayLoop(zone);
		}

		private void Update()
		{
			_etaText.text = string.Format(_textFormat, global::UnityEngine.Mathf.CeilToInt(_handler.RemainingTime));
		}

		private void PlayStart(global::MapGeneration.FacilityZone zone)
		{
			global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079LostSignalGui.ZoneClip[] zoneStarts = _zoneStarts;
			for (int i = 0; i < zoneStarts.Length; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079LostSignalGui.ZoneClip zoneClip = zoneStarts[i];
				if (zoneClip.Zone == zone)
				{
					PlayClip(zoneClip.Clip);
					return;
				}
			}
			PlayClip(_fallbackStart);
		}

		private void PlayLoop(global::MapGeneration.FacilityZone zone)
		{
			_loopSource.clip = _fallbackLoop;
			global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079LostSignalGui.ZoneClip[] zoneLoops = _zoneLoops;
			for (int i = 0; i < zoneLoops.Length; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079LostSignalGui.ZoneClip zoneClip = zoneLoops[i];
				if (zoneClip.Zone == zone)
				{
					_loopSource.clip = zoneClip.Clip;
					break;
				}
			}
			_loopSource.Play();
		}

		private void PlayClip(global::UnityEngine.AudioClip clip)
		{
			global::AudioPooling.AudioSourcePoolManager.PlaySound(clip, null, 1f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.DefaultSfx, 0f);
		}
	}
}
