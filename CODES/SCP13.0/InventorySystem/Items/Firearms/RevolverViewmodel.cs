using System;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class RevolverViewmodel : AnimatedFirearmViewmodel
	{
		private enum RevolverLayer
		{
			MainAnims = 0,
			Ads = 1,
			Unloaded = 2,
			Cylinder = 3,
			DryFire = 4,
			Cocked = 5,
			Idle = 6
		}

		[Serializable]
		private struct RevolverPrimer
		{
			[SerializeField]
			private GameObject _rootObject;

			[SerializeField]
			private int _bulletsAmount;

			[SerializeField]
			private GameObject[] _primers;

			[SerializeField]
			private int _absoluteOffset;

			public void Refresh(int targetCylinder, int remainingBullets, int offset)
			{
			}
		}

		[SerializeField]
		private Transform _swayPivot;

		[SerializeField]
		private RevolverPrimer[] _firedPrimers;

		private Revolver _revolver;

		private byte _unfiredBulletsRemaining;

		private bool _wasCocked;

		private static int ReloadTagHash => 0;

		internal override void OnEquipped()
		{
		}

		private void RefreshPrimers(int offset)
		{
		}

		public override void LateUpdate()
		{
		}

		private void SetLayerWeight(RevolverLayer layer, float val)
		{
		}
	}
}
