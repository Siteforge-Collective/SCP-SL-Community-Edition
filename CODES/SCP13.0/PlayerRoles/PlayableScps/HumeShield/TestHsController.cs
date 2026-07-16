using UnityEngine;

namespace PlayerRoles.PlayableScps.HumeShield
{
	public class TestHsController : HumeShieldModuleBase
	{
		[SerializeField]
		private float _regeneration;

		[SerializeField]
		private float _maxAmount;

		[SerializeField]
		[Space]
		private Color _color;

		[SerializeField]
		private bool _colorActive;

		[Space]
		[SerializeField]
		private float _amountToModify;

		[SerializeField]
		private bool _apply;

		public override float HsMax => 0f;

		public override float HsRegeneration => 0f;

		public override Color? HsWarningColor => null;

		private void Update()
		{
		}
	}
}
