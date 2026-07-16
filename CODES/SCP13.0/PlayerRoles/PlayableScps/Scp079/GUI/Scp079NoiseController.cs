using System;
using CustomRendering;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079NoiseController : Scp079GuiElementBase
	{
		[Serializable]
		private class NoiseAnimation
		{
			[SerializeField]
			private float _animationSpeed;

			[SerializeField]
			private AnimationCurve _fadeCurve;

			[SerializeField]
			private AnimationCurve _additiveCurve;

			[SerializeField]
			private AnimationCurve _distortionCurve;

			[SerializeField]
			private AnimationCurve _darkenCurve;

			[SerializeField]
			private AnimationCurve _alphaCurve;

			public float PrevValue { get; set; }

			public void ApplyTowards(Scp079NoiseController inst, float target, bool force)
			{
			}

			public void ApplyAnimation(Scp079NoiseController inst, float f)
			{
			}
		}

		[Serializable]
		private struct ZoneSwitchSounds
		{
			public AudioClip Clip;

			public FacilityZone Zone;
		}

		private Scp079CurrentCameraSync _camSync;

		private Scp079LostSignalHandler _lostSignalHandler;

		private Static _static;

		private Scp079CurrentCameraSync.ClientSwitchState _prevSwitchState;

		private static readonly Color TransparentWhite;

		private static readonly int ColorHash;

		[SerializeField]
		private AudioClip _noiseClip;

		[SerializeField]
		private Image _darkenImage;

		[SerializeField]
		private ZoneSwitchSounds[] _zoneOverrides;

		[SerializeField]
		private float _pitchVariation;

		[SerializeField]
		private NoiseAnimation _lostSignalAnim;

		[SerializeField]
		private NoiseAnimation _startupAnim;

		[SerializeField]
		private NoiseAnimation _regularSwitchAnim;

		[SerializeField]
		private NoiseAnimation _zoneSwitchAnim;

		[SerializeField]
		private CanvasGroup _canvasFader;

		[SerializeField]
		private CanvasGroup _zoneSwitchFader;

		[SerializeField]
		private Material _overconFader;

		public bool FullyDarkened => false;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void Update()
		{
		}

		private void UpdateSwitchState()
		{
		}
	}
}
