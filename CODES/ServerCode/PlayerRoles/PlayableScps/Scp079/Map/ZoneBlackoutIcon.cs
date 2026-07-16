namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class ZoneBlackoutIcon : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Map.ZoneBlackoutIcon> Instances = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Map.ZoneBlackoutIcon>();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _root;

		[global::UnityEngine.SerializeField]
		private float _triggerDis;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.FacilityZone _zone;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Graphic _recolorable;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _defaultColor;

		private global::UnityEngine.RectTransform _rt;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager _tier;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079BlackoutZoneAbility _ability;

		public static global::MapGeneration.FacilityZone HighlightedZone
		{
			get
			{
				foreach (global::PlayerRoles.PlayableScps.Scp079.Map.ZoneBlackoutIcon instance in Instances)
				{
					if (instance.Highlighted)
					{
						return instance._zone;
					}
				}
				return global::MapGeneration.FacilityZone.None;
			}
		}

		private bool Highlighted
		{
			get
			{
				bool flag = global::UnityEngine.Vector3.Distance(_rt.position, _root.position) < _triggerDis * _root.localScale.x;
				_recolorable.color = (flag ? global::UnityEngine.Color.white : _defaultColor);
				return flag;
			}
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			base.Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager>(out _tier);
			base.Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079BlackoutZoneAbility>(out _ability);
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tier = _tier;
			tier.OnLevelledUp = (global::System.Action)global::System.Delegate.Combine(tier.OnLevelledUp, new global::System.Action(RefreshVisibiltiy));
			Instances.Add(this);
			_rt = GetComponent<global::UnityEngine.RectTransform>();
			RefreshVisibiltiy();
		}

		private void OnDestroy()
		{
			Instances.Remove(this);
			global::PlayerRoles.PlayableScps.Scp079.Scp079TierManager tier = _tier;
			tier.OnLevelledUp = (global::System.Action)global::System.Delegate.Remove(tier.OnLevelledUp, new global::System.Action(RefreshVisibiltiy));
		}

		private void RefreshVisibiltiy()
		{
			base.gameObject.SetActive(_ability.Unlocked);
		}
	}
}
