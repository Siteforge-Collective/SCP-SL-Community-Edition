namespace PlayerRoles.PlayableScps.HumeShield
{
	public class TestHsController : global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase
	{
		[global::UnityEngine.SerializeField]
		private float _regeneration;

		[global::UnityEngine.SerializeField]
		private float _maxAmount;

		[global::UnityEngine.Space]
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _color;

		[global::UnityEngine.SerializeField]
		private bool _colorActive;

		[global::UnityEngine.Space]
		[global::UnityEngine.SerializeField]
		private float _amountToModify;

		[global::UnityEngine.SerializeField]
		private bool _apply;

		public override float HsMax => _maxAmount;

		public override float HsRegeneration => _regeneration;

		public override global::UnityEngine.Color? HsWarningColor
		{
			get
			{
				if (!_colorActive)
				{
					return null;
				}
				return _color;
			}
		}

		private void Update()
		{
			if (_apply)
			{
				base.HsCurrent += _amountToModify;
				_apply = false;
			}
		}
	}
}
