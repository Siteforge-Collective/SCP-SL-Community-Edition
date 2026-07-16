namespace InventorySystem.Items.Firearms.Modules
{
	public class EventBasedEquipper : global::InventorySystem.Items.Firearms.Modules.IEquipperModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private bool _ready;

		private const float ServerTolerance = 0.1f;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly global::System.Diagnostics.Stopwatch _stopwatch;

		public bool Standby
		{
			get
			{
				if (_ready || _firearm.IsSpectated)
				{
					return true;
				}
				if (!_stopwatch.IsRunning || _stopwatch.Elapsed.TotalSeconds < 0.10000000149011612)
				{
					return false;
				}
				_stopwatch.Stop();
				_ready = true;
				return true;
			}
		}

		public EventBasedEquipper(global::InventorySystem.Items.Firearms.Firearm firearm)
		{
			_ready = true;
			_firearm = firearm;
			_stopwatch = new global::System.Diagnostics.Stopwatch();
		}

		public void OnEquipped()
		{
			_ready = false;
			_stopwatch.Stop();
		}

		public void Equip()
		{
			if (_firearm.IsLocalPlayer)
			{
				_stopwatch.Restart();
			}
			else
			{
				_ready = true;
			}
		}
	}
}
