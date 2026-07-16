namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079AbilityList : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079KeyAbilityGui> _instances = new global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079KeyAbilityGui>();

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _failMessageText;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _popupSound;

		private global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079FailMessageProvider _trackedMessage;

		private float _cachedAlpha = -1f;

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
				if (_cachedAlpha < 0f)
				{
					_cachedAlpha = _failMessageText.alpha;
				}
				return _cachedAlpha;
			}
			set
			{
				value = global::UnityEngine.Mathf.Clamp01(value);
				if (value != _cachedAlpha)
				{
					_failMessageText.alpha = value;
					_cachedAlpha = value;
				}
			}
		}

		private static float CurrentTime => global::UnityEngine.Time.timeSinceLevelLoad;

		public static global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079AbilityList Singleton { get; private set; }

		public global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079FailMessageProvider TrackedFailMessage
		{
			get
			{
				return _trackedMessage;
			}
			set
			{
				bool flag = value == null || (value is global::UnityEngine.Object obj && obj == null);
				if (!flag)
				{
					value.OnFailMessageAssigned();
					if (string.IsNullOrEmpty(value.FailMessage))
					{
						return;
					}
				}
				_trackedMessage = value;
				_failTextReady = false;
				if (!flag)
				{
					_fadeoutBeginTime = CurrentTime + 4f;
					_fadeoutEndTime = _fadeoutBeginTime + 1.8f;
					PlaySound(_popupSound);
				}
			}
		}

		private void Awake()
		{
			Singleton = this;
		}

		private void Update()
		{
			UpdateFailMessage();
			UpdateList();
		}

		private void UpdateFailMessage()
		{
			if (!_failTextReady || _trackedMessage == null || string.IsNullOrEmpty(_trackedMessage.FailMessage))
			{
				FailMessageAlpha -= global::UnityEngine.Time.deltaTime * 5.5f;
				if (FailMessageAlpha == 0f)
				{
					_failTextReady = true;
				}
			}
			else
			{
				float target = 1f - global::UnityEngine.Mathf.InverseLerp(_fadeoutBeginTime, _fadeoutEndTime, CurrentTime);
				FailMessageAlpha = global::UnityEngine.Mathf.MoveTowards(FailMessageAlpha, target, global::UnityEngine.Time.deltaTime * 5.5f);
				_failMessageText.text = _trackedMessage.FailMessage;
			}
		}

		private void UpdateList()
		{
			int num = 0;
			int num2 = -1;
			global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase[] allSubroutines = base.Role.SubroutineModule.AllSubroutines;
			foreach (global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase scpSubroutineBase in allSubroutines)
			{
				if (scpSubroutineBase is global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler scp079LostSignalHandler && scp079LostSignalHandler.Lost)
				{
					num = 0;
					break;
				}
				if (scpSubroutineBase is global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase scp079KeyAbilityBase && scp079KeyAbilityBase.IsVisible)
				{
					bool createSpace = false;
					if (scp079KeyAbilityBase.CategoryId != num2)
					{
						createSpace = num2 != -1;
						num2 = scp079KeyAbilityBase.CategoryId;
					}
					_instances[num++].Setup(scp079KeyAbilityBase.IsReady, scp079KeyAbilityBase.AbilityName, scp079KeyAbilityBase.ActivationKey, createSpace);
					if (num >= _instances.Count)
					{
						global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079KeyAbilityGui scp079KeyAbilityGui = _instances[0];
						_instances.Add(global::UnityEngine.Object.Instantiate(scp079KeyAbilityGui, scp079KeyAbilityGui.transform.parent));
					}
				}
			}
			if (!global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive)
			{
				num = 0;
			}
			for (int j = num; j < _instances.Count; j++)
			{
				_instances[j].gameObject.SetActive(value: false);
			}
		}
	}
}
