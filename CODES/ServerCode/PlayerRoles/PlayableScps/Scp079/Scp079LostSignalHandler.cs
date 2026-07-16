namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079LostSignalHandler : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::GameObjectPools.IPoolSpawnable
	{
		[global::UnityEngine.SerializeField]
		private float _ghostlightLockoutDuration;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager _auxManager;

		private double _recoveryTime;

		private bool _prevLost;

		public bool Lost => _recoveryTime > global::Mirror.NetworkTime.time;

		public float RemainingTime => global::UnityEngine.Mathf.Max(0f, (float)(_recoveryTime - global::Mirror.NetworkTime.time));

		public event global::System.Action OnStatusChanged;

		private void Update()
		{
			bool lost = Lost;
			if (global::Mirror.NetworkServer.active && lost)
			{
				_auxManager.CurrentAux = 0f;
			}
			if (_prevLost != lost)
			{
				_prevLost = lost;
				this.OnStatusChanged?.Invoke();
				if (_curCamSync.TryGetCurrentCamera(out var cam))
				{
					cam.IsActive = !lost;
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule subroutineModule = (base.Role as global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole).SubroutineModule;
			subroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
			subroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager>(out _auxManager);
			global::InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile.OnServerShattered += delegate(global::MapGeneration.RoomIdentifier rid)
			{
				if (global::Mirror.NetworkServer.active && !base.Role.Pooled && !(rid != _curCamSync.CurrentCamera.Room) && base.Role.TryGetOwner(out var hub) && !hub.characterClassManager.GodMode)
				{
					ServerLoseSignal(Lost ? 0f : _ghostlightLockoutDuration);
				}
			};
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _recoveryTime);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_recoveryTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		}

		public void ServerLoseSignal(float duration)
		{
			_recoveryTime = global::Mirror.NetworkTime.time + (double)duration;
			ServerSendRpc(toAll: true);
		}

		public void SpawnObject()
		{
			_recoveryTime = 0.0;
			_prevLost = false;
		}
	}
}
