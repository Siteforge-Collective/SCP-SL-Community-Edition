using PlayerRoles.PlayableScps.HUDs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049Hud : ScpHudBase
	{
		[Space(3f)]
		[SerializeField]
		[Header("--- Senses Ability")]
		private AbilityHud _senseElement;

		[SerializeField]
		private Image _senseIndicator;

		[SerializeField]
		private Vector3 _senseMinSize;

		[SerializeField]
		private Vector3 _senseMaxSize;

		[SerializeField]
		private Color _senseNearby;

		[SerializeField]
		private Color _senseFar;

		[SerializeField]
		private float _iconModifier;

		[SerializeField]
		private float _heartbeatModifier;

		[SerializeField]
		private float _heartbeatMin;

		[SerializeField]
		private float _heartbeatMax;

		[SerializeField]
		private AnimationCurve _heartbeatAnimation;

		[Header("--- Other Abilities")]
		[Space(3f)]
		[SerializeField]
		private LoadingCircleHud _resurrectBar;

		[SerializeField]
		private AbilityHud _callElement;

		[SerializeField]
		private AbilityHud _attackElement;

		[Header("--- HUD Elements")]
		[Space(3f)]
		[SerializeField]
		private GameObject _hudRoot;

		[SerializeField]
		private ScpWarningHud _warningHud;

		[SerializeField]
		private TMP_Text _zombieCounter;

		private Scp049ResurrectAbility _resurrectAbility;

		private Scp049AttackAbility _attackAbility;

		private Scp049SenseAbility _senseAbility;

		private Scp049CallAbility _callAbility;

		private Transform _senseTransform;

		private Transform _senseTransformParent;

		private GameObject _zombieCounterParent;

		private float _heartbeatTimer;

		internal override void Init(ReferenceHub hub)
		{
		}

		internal override void OnDied()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void Update()
		{
		}

		private void LateUpdate()
		{
		}

		private void ShowSenseError()
		{
		}

		private void ShowResurrectErrorCode(byte code)
		{
		}

		private void UpdateSenses(float distance)
		{
		}

		protected override void UpdateCounter()
		{
		}
	}
}
