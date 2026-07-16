namespace InventorySystem.Items.Firearms.Modules
{
	public class DisruptorAction : IActionModule, IFirearmModuleBase, IAmmoManagerModule
	{
		private const float StatusUpdateTime = 0.4f;

		private const float PostShotCooldown = 1.5f;

		private const float AdsCooldown = 0.1f;

		private const float DestroyTime = 3.1f;

		private const float ShotAnimTime = 2.2667f;

		private readonly Firearm _firearm;

		private readonly bool _isAmmoManager;

		private FirearmStatus _predictedStatus;

		private float _lastShotTime;

		private bool _allowLoadSound;

		public const int MaxShots = 5;

		public readonly float ShotDelay;

		private float TimeSinceLastShot => 0f;

		private float CurTime => 0f;

		public FirearmStatus PredictedStatus
		{
			get
			{
				return default(FirearmStatus);
			}
			private set
			{
			}
		}

		private bool IsReloading => false;

		private bool ModulesReady => false;

		private float ActualCooldown => 0f;

		public bool Standby => false;

		public float CyclicRate { get; private set; }

		public bool IsTriggerHeld { get; private set; }

		public byte MaxAmmo => 0;

		public bool ShotTriggered { get; private set; }

		public bool ClientCanReload => false;

		public bool ClientCanUnload => false;

		public bool AdsReady => false;

		public DisruptorAction(Firearm selfRef, float reloadTime, float chargeupTime, bool isAmmoManager)
		{
		}

		private void ClientModifyPredictedAmmo(int amount)
		{
		}

		public ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
		{
			return default(ActionModuleResponse);
		}

		public bool ServerAuthorizeShot()
		{
			return false;
		}

		public bool ServerTryReload()
		{
			return false;
		}

		public bool ServerTryStopReload()
		{
			return false;
		}

		public bool ServerAuthorizeDryFire()
		{
			return false;
		}

		public bool ServerTryUnload()
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
	}
}
