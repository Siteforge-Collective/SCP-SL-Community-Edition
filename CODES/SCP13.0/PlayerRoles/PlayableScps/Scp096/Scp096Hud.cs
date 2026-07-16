using PlayerRoles.PlayableScps.HUDs;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096Hud : ViewmodelScpHud
	{
		[SerializeField]
		private AbilityHud _rageDuration;

		[SerializeField]
		private AbilityHud _chargeCooldown;

		[SerializeField]
		private Image[] _keyCircles;

		[SerializeField]
		private Image _rageEnterSustainCircle;

		[SerializeField]
		private GameObject _docileCircles;

		[SerializeField]
		private ScpWarningHud _rageInfo;

		[SerializeField]
		private Volume _rageVolume;

		[SerializeField]
		private float _rageVolumeDelta;

		private Scp096Role _scp096;

		private Scp096RageCycleAbility _rageCycle;

		private Scp096RageManager _rageManager;

		private Scp096ChargeAbility _chargeAbility;

		internal override void Init(ReferenceHub hub)
		{
		}

		protected override void Update()
		{
		}

		private void UpdateColorGrading(float maxDelta)
		{
		}

		private void UpdateRageInfo()
		{
		}

		private void SetWarning(Scp096HudTranslation key, ActionName action, float duration = 3.8f)
		{
		}
	}
}
