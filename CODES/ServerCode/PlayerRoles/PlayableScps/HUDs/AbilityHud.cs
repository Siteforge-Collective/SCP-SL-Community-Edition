namespace PlayerRoles.PlayableScps.HUDs
{
	[global::System.Serializable]
	public class AbilityHud
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _parent;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.CanvasGroup _fader;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _durationCircle;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _cooldownCircle;

		[global::UnityEngine.SerializeField]
		private bool _inverseDuration;

		[global::UnityEngine.SerializeField]
		private bool _inverseCooldown;

		[global::UnityEngine.SerializeField]
		private bool _showDurationAtCooldown;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _startScale = global::UnityEngine.Vector3.one;

		private global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _cooldown;

		private global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _duration;

		private global::UnityEngine.RectTransform _rt;

		private bool _hasDuration;

		private bool _hasFader;

		private readonly global::System.Diagnostics.Stopwatch _fullStopwatch = new global::System.Diagnostics.Stopwatch();

		private const float MinFullTime = 0.3f;

		private const float FadeSpeed = 8.5f;

		public void Setup(global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown cd, global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown duration)
		{
			_cooldown = cd;
			_duration = duration;
			_rt = _cooldownCircle.rectTransform;
			_hasDuration = _duration != null;
			_hasFader = _fader != null;
			bool flag = UpdateVisibility();
			if (_hasFader)
			{
				_fader.alpha = (flag ? 1 : 0);
			}
			else
			{
				_parent.SetActive(flag);
			}
		}

		public void Update(bool forceHidden = false)
		{
			bool flag = !forceHidden && UpdateVisibility();
			if (_hasFader)
			{
				_fader.alpha = global::UnityEngine.Mathf.Clamp01(_fader.alpha + global::UnityEngine.Time.deltaTime * (flag ? 8.5f : (-8.5f)));
			}
			else
			{
				_parent.SetActive(flag);
			}
		}

		private bool UpdateVisibility()
		{
			bool result = UpdateCooldown();
			if (_hasDuration && UpdateDuration())
			{
				result = true;
			}
			return result;
		}

		private bool UpdateCooldown()
		{
			float t = FillCircle(_cooldown, _cooldownCircle, _inverseCooldown);
			_rt.localScale = global::UnityEngine.Vector3.Lerp(_startScale, global::UnityEngine.Vector3.one, t);
			return !_cooldown.IsReady;
		}

		private bool UpdateDuration()
		{
			if (_duration.IsReady)
			{
				_durationCircle.enabled = false;
				if (_fullStopwatch.IsRunning)
				{
					return _fullStopwatch.Elapsed.TotalSeconds < 0.30000001192092896;
				}
				return false;
			}
			_durationCircle.enabled = _showDurationAtCooldown || _cooldown.IsReady;
			FillCircle(_duration, _durationCircle, !_inverseDuration);
			_fullStopwatch.Restart();
			return true;
		}

		private float FillCircle(global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown cd, global::UnityEngine.UI.Image circle, bool inverse)
		{
			float num = cd.Readiness;
			if (inverse)
			{
				num = 1f - num;
			}
			circle.fillAmount = num;
			return num;
		}
	}
}
