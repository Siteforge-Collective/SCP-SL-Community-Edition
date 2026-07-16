namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173TeleportIndicator : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private float _volumeAdjustmentSpeed;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _soundSource;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _normalIndicator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _killIndicator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _neutralIndicator;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule _subroutineManager;

		private float _targetVolume;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility _teleportAbility;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private void Awake()
		{
			_subroutineManager.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility>(out _teleportAbility);
			_subroutineManager.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility>(out _breakneckSpeedsAbility);
		}

		private void Update()
		{
			_soundSource.volume = global::UnityEngine.Mathf.MoveTowards(_soundSource.volume, _targetVolume, _volumeAdjustmentSpeed * global::UnityEngine.Time.deltaTime);
		}

		private void SetupVisiblity(bool normal = false, bool kill = false, bool neutral = false)
		{
			_normalIndicator.SetActive(normal);
			_killIndicator.SetActive(kill);
			_neutralIndicator.SetActive(neutral);
		}

		public void UpdateVisibility(bool isVisible)
		{
			_targetVolume = (isVisible ? 1 : 0);
			if (!isVisible)
			{
				SetupVisiblity();
				return;
			}
			if (_breakneckSpeedsAbility.IsActive)
			{
				SetupVisiblity(normal: false, kill: false, neutral: true);
				return;
			}
			bool flag = _teleportAbility.BestTarget != null;
			SetupVisiblity(!flag, flag);
		}
	}
}
