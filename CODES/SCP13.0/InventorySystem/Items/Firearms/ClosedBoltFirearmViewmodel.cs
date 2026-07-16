using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class ClosedBoltFirearmViewmodel : AnimatedFirearmViewmodel
	{
		private enum ClosedBoltLayer
		{
			Grips = 0,
			MainAnims = 1,
			Ads = 2,
			BoltCatch = 3,
			NoMag = 4,
			Trigger = 5,
			FullAuto = 6
		}

		[SerializeField]
		private float _fullAutoKickSpeed;

		[SerializeField]
		private bool _isOpenBolt;

		[SerializeField]
		private bool _boltLockNeedsMag;

		[SerializeField]
		private float _fullAutoReturnSmoothness;

		[SerializeField]
		private float _fullAutoMultiplier;

		private AutomaticFirearm _firearm;

		private float _fullAutoState;

		private float _triggerSmooth;

		private static int ReloadTagHash => 0;

		internal override void OnEquipped()
		{
		}

		public override void LateUpdate()
		{
		}

		private void SetLayerWeight(ClosedBoltLayer layer, float val)
		{
		}
	}
}
