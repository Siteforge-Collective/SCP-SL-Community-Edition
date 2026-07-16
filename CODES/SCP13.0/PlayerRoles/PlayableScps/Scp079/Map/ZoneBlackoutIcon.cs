using System.Collections.Generic;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.GUI;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class ZoneBlackoutIcon : Scp079GuiElementBase
	{
		private static readonly HashSet<ZoneBlackoutIcon> Instances;

		[SerializeField]
		private RectTransform _root;

		[SerializeField]
		private float _triggerDis;

		[SerializeField]
		private FacilityZone _zone;

		[SerializeField]
		private Graphic _recolorable;

		[SerializeField]
		private Color _defaultColor;

		private RectTransform _rt;

		private Scp079TierManager _tier;

		private Scp079BlackoutZoneAbility _ability;

		public static FacilityZone HighlightedZone => default(FacilityZone);

		private bool Highlighted => false;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}

		private void RefreshVisibiltiy()
		{
		}
	}
}
