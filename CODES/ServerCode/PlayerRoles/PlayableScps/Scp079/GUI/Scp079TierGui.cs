namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079TierGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _slider;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _textTier;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _textExpNormal;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _textExpInverted;

		private bool _uiDirty;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager _tierManager;

		private string _tierFormat;

		private string _expFormat;

		private string _maxTierText;

		private string _levelUpNotification;

		public const string NewLineFormat = "\n  - ";

		private string ExpText
		{
			set
			{
				_textExpNormal.text = value;
				_textExpInverted.text = value;
			}
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			_tierFormat = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.AccessTier);
			_expFormat = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Experience);
			_maxTierText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.MaxTierReached);
			_levelUpNotification = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.AccessTierUnlocked);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager>(out _tierManager);
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tierManager = _tierManager;
			tierManager.OnExpChanged = (global::System.Action)global::System.Delegate.Combine(tierManager.OnExpChanged, new global::System.Action(SetDirty));
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tierManager2 = _tierManager;
			tierManager2.OnLevelledUp = (global::System.Action)global::System.Delegate.Combine(tierManager2.OnLevelledUp, new global::System.Action(OnLevelledUp));
			_uiDirty = true;
		}

		private void OnDestroy()
		{
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tierManager = _tierManager;
			tierManager.OnExpChanged = (global::System.Action)global::System.Delegate.Remove(tierManager.OnExpChanged, new global::System.Action(SetDirty));
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tierManager2 = _tierManager;
			tierManager2.OnLevelledUp = (global::System.Action)global::System.Delegate.Remove(tierManager2.OnLevelledUp, new global::System.Action(OnLevelledUp));
		}

		private void Update()
		{
			if (_uiDirty)
			{
				_textTier.text = string.Format(_tierFormat, _tierManager.AccessTierLevel);
				if (_tierManager.NextLevelThreshold <= 0)
				{
					_slider.fillAmount = 1f;
					ExpText = _maxTierText;
				}
				else
				{
					_slider.fillAmount = (float)_tierManager.RelativeExp / (float)_tierManager.NextLevelThreshold;
					ExpText = string.Format(_expFormat, _tierManager.RelativeExp, _tierManager.NextLevelThreshold);
				}
				_uiDirty = false;
			}
		}

		private void SetDirty()
		{
			_uiDirty = true;
		}

		private void OnLevelledUp()
		{
			SetDirty();
			bool flag = false;
			global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
			stringBuilder.AppendFormat(_levelUpNotification, _tierManager.AccessTierLevel);
			global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase[] allSubroutines = base.Role.SubroutineModule.AllSubroutines;
			for (int i = 0; i < allSubroutines.Length; i++)
			{
				if (allSubroutines[i] is global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079LevelUpNotifier scp079LevelUpNotifier)
				{
					if (!flag)
					{
						stringBuilder.Append("\n  - ");
					}
					flag = !scp079LevelUpNotifier.WriteLevelUpNotification(stringBuilder, _tierManager.AccessTierIndex);
				}
			}
			string text = global::NorthwoodLib.Pools.StringBuilderPool.Shared.ToStringReturn(stringBuilder);
			if (flag)
			{
				text = text.Substring(0, text.Length - "\n  - ".Length);
			}
			global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationManager.AddNotification(new global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079AccentedNotification(text));
		}
	}
}
