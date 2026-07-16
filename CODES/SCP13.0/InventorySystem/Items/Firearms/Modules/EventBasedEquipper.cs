using System.Diagnostics;

namespace InventorySystem.Items.Firearms.Modules
{
	public class EventBasedEquipper : IEquipperModule, IFirearmModuleBase
	{
		private bool _ready;

		private const float ServerTolerance = 0.1f;

		private readonly Firearm _firearm;

		private readonly Stopwatch _stopwatch;

		public bool Standby => false;

		public EventBasedEquipper(Firearm firearm)
		{
		}

		public void OnEquipped()
		{
		}

		public void Equip()
		{
		}
	}
}
