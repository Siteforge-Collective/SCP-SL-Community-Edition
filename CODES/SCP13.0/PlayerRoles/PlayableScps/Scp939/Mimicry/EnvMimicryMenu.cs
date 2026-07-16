using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class EnvMimicryMenu : MimicryMenuBase
	{
		[SerializeField]
		private CanvasGroup _fader;

		[SerializeField]
		private float _fadedAlpha;

		[SerializeField]
		private float _fadeSpeed;

		[SerializeField]
		private TMP_Text _cooldownText;

		private EnvironmentalMimicry _envMimicry;

		protected override void Setup(Scp939Role role)
		{
		}

		private void UpdateFade(bool instant)
		{
		}

		private void OnEnable()
		{
		}

		private void Update()
		{
		}
	}
}
