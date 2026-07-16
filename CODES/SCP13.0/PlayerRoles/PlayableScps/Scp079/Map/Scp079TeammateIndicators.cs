using System.Collections.Generic;
using PlayerRoles.PlayableScps.Scp079.GUI;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079TeammateIndicators : Scp079GuiElementBase
	{
		private enum IndicatorType
		{
			Low = 0,
			Medium = 1,
			High = 2
		}

		private readonly Dictionary<ReferenceHub, RectTransform> _instances;

		private Scp079TierManager _tierManager;

		private IZoneMap[] _maps;

		[SerializeField]
		private RectTransform _templateLow;

		[SerializeField]
		private RectTransform _templateMid;

		[SerializeField]
		private RectTransform _templateHigh;

		private IndicatorType CurType => default(IndicatorType);

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}

		private void OnRoleChanged(ReferenceHub userHub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void SetupPlayer(ReferenceHub hub)
		{
		}

		private void Rebuild()
		{
		}

		private void Update()
		{
		}
	}
}
