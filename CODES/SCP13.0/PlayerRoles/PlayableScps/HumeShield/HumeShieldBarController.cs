using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.HumeShield
{
	public class HumeShieldBarController : MonoBehaviour
	{
		[SerializeField]
		private StatusBar _targetBar;

		[SerializeField]
		private Image _hsWarning;

		[SerializeField]
		private Color _hsColor;

		private float _colorTimer;

		private bool _prevVisible;

		private bool _firstFrame;

		private const float FadeSpeed = 8f;

		private const float BlinkSpeed = 35f;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void GetValues(out bool barVisible, out Color? warningColor)
		{
			barVisible = default(bool);
			warningColor = null;
		}
	}
}
