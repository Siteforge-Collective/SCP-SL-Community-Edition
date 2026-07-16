using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.HUDs
{
	public class ScpWarningHud : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _text;

		private const float FadeSpeed = 8f;

		private const float DefaultTime = 3.8f;

		private float _duration;

		private string _targetText;

		private float _alpha;

		private bool _dirty;

		private readonly Stopwatch _elapsed;

		public float Alpha
		{
			get
			{
				return 0f;
			}
			private set
			{
			}
		}

		private void Awake()
		{
		}

		private void Update()
		{
		}

		public void SetText(string text, float duration = 3.8f)
		{
		}
	}
}
