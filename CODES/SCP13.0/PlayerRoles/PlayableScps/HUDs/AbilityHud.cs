using System;
using System.Diagnostics;
using PlayerRoles.Subroutines;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.HUDs
{
	[Serializable]
	public class AbilityHud
	{
		[Serializable]
		private class GraphicColor
		{
			public Color StartingColor;

			public Graphic Graphic;

			private GraphicColor(Graphic graphic)
			{
			}
		}

		[SerializeField]
		private GameObject _parent;

		[SerializeField]
		private CanvasGroup _fader;

		[SerializeField]
		private Image _durationCircle;

		[SerializeField]
		private Image _cooldownCircle;

		[SerializeField]
		private bool _inverseDuration;

		[SerializeField]
		private bool _inverseCooldown;

		[SerializeField]
		private bool _showDurationAtCooldown;

		[SerializeField]
		private Vector3 _startScale;

		[SerializeField]
		private float _flashDuration;

		[SerializeField]
		private float _flashSpeed;

		[SerializeField]
		private GraphicColor[] _graphics;

		private Stopwatch _flashStopwatch;

		private float _flashIdleTime;

		private AbilityCooldown _cooldown;

		private AbilityCooldown _duration;

		private RectTransform _rt;

		private bool _hasDuration;

		private bool _hasFader;

		private readonly Stopwatch _fullStopwatch;

		private const float MinFullTime = 0.3f;

		private const float FadeSpeed = 8.5f;

		public void Setup(AbilityCooldown cd, AbilityCooldown duration)
		{
		}

		public void Update(bool forceHidden = false)
		{
		}

		private bool UpdateVisibility()
		{
			return false;
		}

		private bool UpdateCooldown()
		{
			return false;
		}

		private bool UpdateDuration()
		{
			return false;
		}

		private float FillCircle(AbilityCooldown cd, Image circle, bool inverse)
		{
			return 0f;
		}

		public void LateUpdate()
		{
		}

		public void FlashAbility()
		{
		}

		private void UpdateFlash(Graphic targetGraphic, Stopwatch sw, Color normalColor, ref float idleTime)
		{
		}
	}
}
