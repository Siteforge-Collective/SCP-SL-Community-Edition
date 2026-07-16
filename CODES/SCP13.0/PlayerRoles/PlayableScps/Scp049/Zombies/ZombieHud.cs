using PlayerRoles.PlayableScps.HUDs;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieHud : ScpHudBase, IViewmodelRole
	{
		[SerializeField]
		private AbilityHud _attackCooldownIcon;

		[SerializeField]
		private LoadingCircleHud _consumeCircle;

		[SerializeField]
		private Image _bloodlustBackground;

		[SerializeField]
		private Image _bloodlustFill;

		[SerializeField]
		private float _bloodlustAlphaSpeed;

		[SerializeField]
		private float _bloodlustTickDelay;

		[SerializeField]
		private float _bloodlustThreshold;

		[SerializeField]
		private Sprite _eyeOpen;

		[SerializeField]
		private Sprite _eyeSemiOpen;

		[SerializeField]
		private Sprite _eyeClosed;

		[SerializeField]
		private Animator _hands;

		[SerializeField]
		private GameObject _uiRoot;

		[SerializeField]
		private ScpWarningHud _warningHud;

		private ZombieConsumeAbility _consumeAbility;

		private ZombieAttackAbility _attackAbility;

		private ZombieMovementModule _fpcModule;

		private float _bloodlustTimer;

		private float _bloodlustAlpha;

		private float _bloodlustLerpValue;

		private bool _handsDestroyed;

		private const float HandsFov = 70f;

		private static readonly int AttackHash;

		private static readonly int EatHash;

		protected override void Update()
		{
		}

		private void PlayAttack()
		{
		}

		private void ProcessError(byte err)
		{
		}

		private void UpdateBloodlust(Image fill, Image background, float fillAmount)
		{
		}

		private void SetBloodlustTransparency(Image fill, Image background, float fillAmount)
		{
		}

		private void SetTransparency(Image image, float alpha)
		{
		}

		private void DestroyHands()
		{
		}

		protected override void OnDestroy()
		{
		}

		internal override void OnDied()
		{
		}

		internal override void Init(ReferenceHub hub)
		{
		}

		public bool TryGetViewmodelFov(out float fov)
		{
			fov = default(float);
			return false;
		}
	}
}
