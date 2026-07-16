using System.Diagnostics;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public class TubularMagazineAmmoManager : IAmmoManagerModule, IFirearmModuleBase
	{
		private enum CurrentAction
		{
			Idle = 0,
			Reloading = 1,
			Unloading = 2
		}

		private readonly Firearm _firearm;

		private readonly byte _numberOfChambers;

		private readonly Stopwatch _cooldownStopwatch;

		private readonly float _cooldownTime;

		private readonly int _bulletsToLoadAnimHash;

		private readonly int _animLoopLayer;

		private readonly ushort _serial;

		private readonly KeyCode[] _cancelReloadKeys;

		private byte _defaultMaxAmmo;

		private CurrentAction _currentAction;

		private CurrentAction CurAction
		{
			get
			{
				return default(CurrentAction);
			}
			set
			{
			}
		}

		private int ChamberedRounds => 0;

		private bool ClientIsReloading => false;

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

		public bool ClientCanUnload => false;

		public bool ClientCanReload => false;

		private bool ClientModulesReady => false;

		public TubularMagazineAmmoManager(Firearm selfRef, ushort serial, byte maxAmmo, byte numberOfChambers, float cooldownTime, int reloadAnimatorLayer, string bulletsToLoadParamName, params ActionName[] cancelReloadActions)
		{
		}

		public bool ServerTryReload()
		{
			return false;
		}

		public bool ServerTryUnload()
		{
			return false;
		}

		public bool ServerTryStopReload()
		{
			return false;
		}

		private bool ServerHandleRequest(CurrentAction action)
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

		private void ClientUpdateAction(CurrentAction newAction)
		{
		}

		private void EquipUpdate()
		{
		}

		private void UpdateLocalPlayerOnly()
		{
		}

		private void UpdateReload()
		{
		}

		private void UpdateUnload()
		{
		}

		private void Holstered()
		{
		}
	}
}
