namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939BreathController : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		[global::System.Serializable]
		private class IdleLoop939
		{
			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AudioSource _thirdperson;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AudioSource _firstperson;

			private bool _cacheSet;

			private bool _has3rd;

			private bool _has1st;

			private bool _local;

			public float CurVolume { get; private set; }

			public void SetVolume(bool isOn, float lerp)
			{
				SetVolume(isOn ? 1 : 0, lerp);
			}

			public void SetOwner(bool isLocalPlayer)
			{
				_local = isLocalPlayer;
				SetVolume(CurVolume);
			}

			public void SetVolume(float vol, float lerp = 1f)
			{
				if (!_cacheSet)
				{
					_has3rd = _thirdperson != null;
					_has1st = _firstperson != null;
					_cacheSet = true;
				}
				CurVolume = global::UnityEngine.Mathf.Lerp(CurVolume, vol, lerp);
				if (_has3rd)
				{
					_thirdperson.volume = (_local ? 0f : CurVolume);
				}
				if (_has1st)
				{
					_firstperson.volume = (_local ? CurVolume : 0f);
				}
			}
		}

		[global::UnityEngine.SerializeField]
		private float _exhaustionGainLerp;

		[global::UnityEngine.SerializeField]
		private float _exhaustionDropLerp;

		[global::UnityEngine.SerializeField]
		private float _exhaustionMuteLoopsThreshold;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _exhaustionVolume;

		[global::UnityEngine.SerializeField]
		private float _breathLerp;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Scp939BreathController.IdleLoop939 _focusLoop;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Scp939BreathController.IdleLoop939 _breathLoop;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Scp939BreathController.IdleLoop939 _exhaustionLoop;

		private float _curExhaustion;

		private global::PlayerStatsSystem.StaminaStat _stamina;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focus;

		public override void SpawnObject()
		{
			base.SpawnObject();
			RefreshPerspective();
			ForEachLoop(delegate(global::PlayerRoles.PlayableScps.Scp939.Scp939BreathController.IdleLoop939 x)
			{
				x.SetVolume(0f);
			});
			_stamina = base.Owner.playerStats.GetModule<global::PlayerStatsSystem.StaminaStat>();
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(RefreshPerspective));
			if (global::Mirror.NetworkServer.active)
			{
				_stamina.ChangeSyncMode(global::PlayerStatsSystem.SyncedStatBase.SyncMode.Public);
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_curExhaustion = 0f;
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Remove(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(RefreshPerspective));
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focus);
		}

		private void RefreshPerspective()
		{
			bool isLocal = global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner) || base.Owner.isLocalPlayer;
			ForEachLoop(delegate(global::PlayerRoles.PlayableScps.Scp939.Scp939BreathController.IdleLoop939 x)
			{
				x.SetOwner(isLocal);
			});
		}

		private void ForEachLoop(global::System.Action<global::PlayerRoles.PlayableScps.Scp939.Scp939BreathController.IdleLoop939> action)
		{
			action(_focusLoop);
			action(_breathLoop);
			action(_exhaustionLoop);
		}

		private void Update()
		{
			float num = global::UnityEngine.Mathf.Clamp01(1f - _stamina.CurValue);
			_curExhaustion = global::UnityEngine.Mathf.Lerp(_curExhaustion, num, global::UnityEngine.Time.deltaTime * ((num > _curExhaustion) ? _exhaustionGainLerp : _exhaustionDropLerp));
			_exhaustionLoop.SetVolume(_exhaustionVolume.Evaluate(_curExhaustion));
			bool flag = _curExhaustion > _exhaustionMuteLoopsThreshold;
			_focusLoop.SetVolume(!flag && _focus.TargetState, global::UnityEngine.Time.deltaTime * _breathLerp);
			_breathLoop.SetVolume(!flag && !_focus.TargetState, global::UnityEngine.Time.deltaTime * _breathLerp);
		}
	}
}
