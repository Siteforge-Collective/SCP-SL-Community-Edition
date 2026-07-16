using System.Diagnostics;

namespace InventorySystem.Items.Firearms.Modules
{
	public class AutomaticAmmoManager : IAmmoManagerModule, IFirearmModuleBase
	{
		private const float MinimalBusyTime = 0.3f;

		private readonly Firearm _firearm;

		private readonly int _reloadTriggerHash;

		private readonly int _unloadTriggerHash;

		private readonly int _defaultAnimHash;

		private readonly int _idleTagHash;

		private readonly Stopwatch _busyStopwatch;

		private readonly int _reloadAnimsLayer;

		private readonly int _chamberSize;

		private bool _isBusy;

		private byte _defaultMaxAmmo;

		public int ChamberedAmount => 0;

		public byte MaxAmmo
		{
			get
			{
				return 0;
			}
			private set
			{
			}
		}

		public bool Standby => false;

		private ushort UserAmmo => 0;

		public bool ClientCanUnload => false;

		public bool ClientCanReload => false;

		public AutomaticAmmoManager(Firearm selfRef, byte maxAmmo, int reloadAnimsLayer, int chamberSize)
		{
		}

		public bool ServerTryReload()
		{
			return false;
		}

		public void ClientReload()
		{
		}

		public void ClientUnload()
		{
		}

		public void ClientStopReload()
		{
		}

		public bool ServerTryUnload()
		{
			return false;
		}

		public bool ServerTryStopReload()
		{
			return false;
		}

		private void EquipUpdate()
		{
		}

		private void CancelReload()
		{
		}
	}
}
