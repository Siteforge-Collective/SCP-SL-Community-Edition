using InventorySystem.Items.Firearms.Modules;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class ShotgunViewmodel : AnimatedFirearmViewmodel
	{
		[SerializeField]
		private GameObject[] _loadedShells;

		[SerializeField]
		private float _recoilPerShot;

		[SerializeField]
		private float _recoilOffset;

		private Shotgun _shotgun;

		private float _triggerSmooth;

		private const int ShootLayer = 1;

		private const int TriggerLayer = 4;

		private PumpAction PumpAction => null;

		public override void InitAny()
		{
		}

		public override void LateUpdate()
		{
		}

		private void OnFire()
		{
		}
	}
}
