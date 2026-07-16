using System.Collections.Generic;
using System.Runtime.InteropServices;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Ammo
{
	public class AmmoPickup : ItemPickupBase
	{
		[SyncVar]
		public ushort SavedAmmo;

		[SerializeField]
		private int _minDisplayedValue;

		[SerializeField]
		private int _maxDisplayedValue;

		[SerializeField]
		private int _roundingValue;

		[SerializeField]
		private bool _hideFirstDigitBelow10;

		[SerializeField]
		private Material _targetDigitMaterial;

		[SerializeField]
		private Renderer[] _firstDigits;

		[SerializeField]
		private Renderer[] _secondDigits;

		private static readonly Dictionary<ItemType, Dictionary<int, Material>> DigitMaterials;

		private ushort _prevAmmo;

		public int MaxAmmo => 0;

		private Material GetDigitMaterial(int digit)
		{
			return null;
		}

		private void Update()
		{
		}
	}
}
