using System;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939BreathController : StandardSubroutine<Scp939Role>
	{
		[Serializable]
		private class IdleLoop939
		{
			[SerializeField]
			private AudioSource _thirdperson;

			[SerializeField]
			[Range(0f, 1f)]
			private float _thirdpersonVolume;

			[SerializeField]
			private AudioSource _firstperson;

			[SerializeField]
			[Range(0f, 1f)]
			private float _firstPersonVolume;

			private bool _cacheSet;

			private bool _has3rd;

			private bool _has1st;

			private bool _local;

			public float CurVolume { get; private set; }

			public void SetVolume(bool isOn, float lerp)
			{
			}

			public void SetOwner(bool isLocalPlayer)
			{
			}

			public void SetVolume(float vol, float lerp = 1f)
			{
			}
		}

		[SerializeField]
		private float _exhaustionGainLerp;

		[SerializeField]
		private float _exhaustionDropLerp;

		[SerializeField]
		private float _exhaustionMuteLoopsThreshold;

		[SerializeField]
		private AnimationCurve _exhaustionVolume;

		[SerializeField]
		private float _breathLerp;

		[SerializeField]
		private float _focusGrowlGainLerp;

		[SerializeField]
		private float _focusGrowlDropLerp;

		private float _timeFromLastFocus;

		[SerializeField]
		private float _dropFocusAfter;

		[SerializeField]
		private IdleLoop939 _focusLoop;

		[SerializeField]
		private IdleLoop939 _breathLoop;

		[SerializeField]
		private IdleLoop939 _exhaustionLoop;

		[SerializeField]
		private IdleLoop939 _focusGrowlLoop;

		private float _curExhaustion;

		private StaminaStat _stamina;

		private Scp939FocusAbility _focus;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		protected override void Awake()
		{
		}

		private void RefreshPerspective()
		{
		}

		private void ForEachLoop(Action<IdleLoop939> action)
		{
		}

		private void Update()
		{
		}
	}
}
