namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079NoiseController : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::System.Serializable]
		private class NoiseAnimation
		{
			[global::UnityEngine.SerializeField]
			private float _animationSpeed;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AnimationCurve _fadeCurve;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AnimationCurve _additiveCurve;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AnimationCurve _distortionCurve;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AnimationCurve _darkenCurve;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AnimationCurve _alphaCurve;

			public float PrevValue { get; set; }

			public void ApplyTowards(global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController inst, float target, bool force)
			{
				float num = global::UnityEngine.Mathf.MoveTowards(PrevValue, target, global::UnityEngine.Time.deltaTime * _animationSpeed);
				if (PrevValue != num || force)
				{
					ApplyAnimation(inst, num);
				}
			}

			public void ApplyAnimation(global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController inst, float f)
			{
				inst._static.fade.value = _fadeCurve.Evaluate(f);
				inst._static.fadeAdditive.value = _additiveCurve.Evaluate(f);
				inst._static.fadeDistortion.value = _distortionCurve.Evaluate(f);
				inst._darken.intensity.value = _darkenCurve.Evaluate(f);
				float num = _alphaCurve.Evaluate(f);
				inst._overconFader.SetColor(ColorHash, global::UnityEngine.Color.Lerp(TransparentWhite, global::UnityEngine.Color.white, num));
				inst._canvasFader.alpha = num;
				PrevValue = f;
			}
		}

		[global::System.Serializable]
		private struct ZoneSwitchSounds
		{
			public global::UnityEngine.AudioClip Clip;

			public global::MapGeneration.FacilityZone Zone;
		}

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _camSync;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler _lostSignalHandler;

		private global::PostProcessing.Static _static;

		private global::PostProcessing.Darken _darken;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState _prevSwitchState;

		private static readonly global::UnityEngine.Color TransparentWhite = new global::UnityEngine.Color(1f, 1f, 1f, 0f);

		private static readonly int ColorHash = global::UnityEngine.Shader.PropertyToID("_Color");

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _noiseClip;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController.ZoneSwitchSounds[] _zoneOverrides;

		[global::UnityEngine.SerializeField]
		private float _pitchVariation;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController.NoiseAnimation _lostSignalAnim;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController.NoiseAnimation _regularSwitchAnim;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController.NoiseAnimation _zoneSwitchAnim;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _canvasFader;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _zoneSwitchFader;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _overconFader;

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			base.Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _camSync);
			base.Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler>(out _lostSignalHandler);
			if (TryGetComponent<global::UnityEngine.Rendering.PostProcessing.PostProcessVolume>(out var component))
			{
				global::UnityEngine.Rendering.PostProcessing.PostProcessProfile profile = component.profile;
				_static = profile.GetSetting<global::PostProcessing.Static>();
				_darken = profile.GetSetting<global::PostProcessing.Darken>();
				_regularSwitchAnim.ApplyAnimation(this, 1f);
			}
		}

		private void Update()
		{
			if (_lostSignalHandler.Lost)
			{
				_regularSwitchAnim.ApplyAnimation(this, 0f);
				_zoneSwitchAnim.ApplyAnimation(this, 0f);
				_lostSignalAnim.ApplyTowards(this, 1f, force: true);
				_zoneSwitchFader.alpha = 0f;
			}
			else
			{
				UpdateSwitchState();
				_lostSignalAnim.ApplyTowards(this, 0f, force: false);
			}
		}

		private void UpdateSwitchState()
		{
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState curClientSwitchState = _camSync.CurClientSwitchState;
			bool flag = curClientSwitchState != _prevSwitchState;
			_prevSwitchState = curClientSwitchState;
			switch (curClientSwitchState)
			{
			case global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.SwitchingRoom:
				if (flag)
				{
					PlaySound(_noiseClip).pitch = global::UnityEngine.Random.Range(1f - _pitchVariation, 1f + _pitchVariation);
				}
				_regularSwitchAnim.ApplyTowards(this, 1f, force: true);
				break;
			case global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.SwitchingZone:
			{
				_regularSwitchAnim.ApplyTowards(this, 0f, force: true);
				_zoneSwitchAnim.ApplyTowards(this, 1f, force: true);
				_zoneSwitchFader.alpha = _darken.intensity.value;
				if (!flag)
				{
					break;
				}
				global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController.ZoneSwitchSounds[] zoneOverrides = _zoneOverrides;
				for (int i = 0; i < zoneOverrides.Length; i++)
				{
					global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NoiseController.ZoneSwitchSounds zoneSwitchSounds = zoneOverrides[i];
					if (zoneSwitchSounds.Zone == _camSync.CurClientTargetZone)
					{
						PlaySound(zoneSwitchSounds.Clip);
						return;
					}
				}
				PlaySound(_noiseClip);
				break;
			}
			case global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None:
				_regularSwitchAnim.ApplyTowards(this, 0f, force: false);
				_zoneSwitchAnim.ApplyTowards(this, 0f, force: false);
				_zoneSwitchFader.alpha = _zoneSwitchAnim.PrevValue;
				break;
			}
		}
	}
}
