using System.Diagnostics;

namespace InventorySystem.Items.Firearms.Modules
{
	public class ChambercheckEquipper : IEquipperModule, IFirearmModuleBase
	{
		private int _equips;

		private bool _allowInteraction;

		private float _targetTime;

		private readonly int _pickupParamHash;

		private readonly float _normalTime;

		private readonly float _pickupTime;

		private readonly Firearm _firearm;

		private readonly Stopwatch _stopwatch;

		private readonly int _randomParamHash;

		public bool Standby => false;

		public ChambercheckEquipper(Firearm firearm, string pickupParamName, float normalAnimationTime, float pickupAnimationTime)
		{
		}

		public void OnEquipped()
		{
		}
	}
}
