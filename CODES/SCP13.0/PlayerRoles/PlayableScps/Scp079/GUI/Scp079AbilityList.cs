using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079AbilityList : Scp079GuiElementBase
	{
		[SerializeField]
		private List<Scp079KeyAbilityGui> _mainGroupInstances;

		[SerializeField]
		private List<Scp079KeyAbilityGui> _leftGroupInstances;

		[SerializeField]
		private TextMeshProUGUI _failMessageText;

		[SerializeField]
		private AudioClip _popupSound;

		private IScp079FailMessageProvider _trackedMessage;

		private float _cachedAlpha;

		private bool _failTextReady;

		private float _fadeoutBeginTime;

		private float _fadeoutEndTime;

		private const float TransitionSpeed = 5.5f;

		private const float FadeoutDuration = 1.8f;

		private const float SustainDuration = 4f;

		private float FailMessageAlpha
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private static float CurrentTime => 0f;

		public static Scp079AbilityList Singleton { get; private set; }

		public IScp079FailMessageProvider TrackedFailMessage
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void UpdateFailMessage()
		{
		}

		private void UpdateList()
		{
		}

		private void UpdateGroup(bool isLeft)
		{
		}
	}
}
