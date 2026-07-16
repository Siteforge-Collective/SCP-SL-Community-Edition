namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079TeammateIndicators : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		private enum IndicatorType
		{
			Low = 0,
			Medium = 1,
			High = 2
		}

		private readonly global::System.Collections.Generic.Dictionary<ReferenceHub, global::UnityEngine.RectTransform> _instances = new global::System.Collections.Generic.Dictionary<ReferenceHub, global::UnityEngine.RectTransform>();

		private global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager _tierManager;

		private global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap[] _maps;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _templateLow;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _templateMid;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _templateHigh;

		private global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType CurType
		{
			get
			{
				int accessTierLevel = _tierManager.AccessTierLevel;
				if (accessTierLevel <= 2)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.Low;
				}
				if (accessTierLevel == 3)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.Medium;
				}
				return global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.High;
			}
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager>(out _tierManager);
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tierManager = _tierManager;
			tierManager.OnLevelledUp = (global::System.Action)global::System.Delegate.Combine(tierManager.OnLevelledUp, new global::System.Action(Rebuild));
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			_maps = GetComponentsInChildren<global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap>(includeInactive: true);
			Rebuild();
		}

		private void OnDestroy()
		{
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tierManager = _tierManager;
			tierManager.OnLevelledUp = (global::System.Action)global::System.Delegate.Remove(tierManager.OnLevelledUp, new global::System.Action(Rebuild));
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
		}

		private void OnRoleChanged(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (_instances.TryGetValue(userHub, out var value))
			{
				global::UnityEngine.Object.Destroy(value.gameObject);
				_instances.Remove(userHub);
			}
			SetupPlayer(userHub);
		}

		private void SetupPlayer(ReferenceHub hub)
		{
			if (!hub.IsSCP(includeZombies: false))
			{
				return;
			}
			global::UnityEngine.RectTransform rectTransform;
			switch (CurType)
			{
			default:
				return;
			case global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.Low:
				rectTransform = global::UnityEngine.Object.Instantiate(_templateLow);
				break;
			case global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.Medium:
				rectTransform = global::UnityEngine.Object.Instantiate(_templateMid);
				break;
			case global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.High:
			{
				rectTransform = global::UnityEngine.Object.Instantiate(_templateHigh);
				string text = hub.roleManager.CurrentRole.RoleTypeId.ToString();
				if (text.Length > 3)
				{
					rectTransform.GetComponentInChildren<global::TMPro.TextMeshProUGUI>().text = text.Substring(3);
				}
				break;
			}
			}
			rectTransform.localScale = global::UnityEngine.Vector3.one;
			_instances[hub] = rectTransform;
		}

		private void Rebuild()
		{
			global::Utils.NonAllocLINQ.DictionaryExtensions.ForEachValue(_instances, delegate(global::UnityEngine.RectTransform x)
			{
				global::UnityEngine.Object.Destroy(x.gameObject);
			});
			_instances.Clear();
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(ReferenceHub.AllHubs, SetupPlayer);
		}

		private void Update()
		{
			if (!global::PlayerRoles.PlayableScps.Scp079.Map.Scp079ToggleMapAbility.MapState)
			{
				return;
			}
			global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType curType = CurType;
			foreach (global::System.Collections.Generic.KeyValuePair<ReferenceHub, global::UnityEngine.RectTransform> instance in _instances)
			{
				bool flag = false;
				global::PlayerRoles.PlayableScps.Scp079.Map.IZoneMap[] maps = _maps;
				for (int i = 0; i < maps.Length; i++)
				{
					if (maps[i].TrySetTeammateIndicator(instance.Key, instance.Value, curType != global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.Low))
					{
						flag = true;
					}
				}
				instance.Value.gameObject.SetActive(flag);
				if (flag && curType == global::PlayerRoles.PlayableScps.Scp079.Map.Scp079TeammateIndicators.IndicatorType.High)
				{
					instance.Value.GetComponentInChildren<global::TMPro.TextMeshProUGUI>().rectTransform.rotation = _templateHigh.rotation;
				}
			}
		}
	}
}
