namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079CameraRotationSync : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::GameObjectPools.IPoolSpawnable
	{
		private global::PlayerRoles.PlayableScps.Scp079.Scp079Role _role;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler _lostSignalHandler;

		private ReferenceHub _owner;

		private readonly global::System.Diagnostics.Stopwatch _clientSendLimit = global::System.Diagnostics.Stopwatch.StartNew();

		private const float ClientSendRate = 15f;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera CurrentCam => _curCamSync.CurrentCamera;

		private void Update()
		{
			if (_owner.isLocalPlayer && !(_clientSendLimit.Elapsed.TotalSeconds < 0.06666667014360428))
			{
				ClientSendCmd();
				_clientSendLimit.Restart();
			}
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, CurrentCam.SyncId);
			CurrentCam.WriteAxes(writer);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (CurrentCam.SyncId == global::Mirror.NetworkReaderExtensions.ReadUInt16(reader) && !_lostSignalHandler.Lost)
			{
				CurrentCam.ApplyAxes(reader);
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, CurrentCam.SyncId);
			CurrentCam.WriteAxes(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.TryGetInteractable(global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera result))
			{
				result.ApplyAxes(reader);
			}
		}

		public void SpawnObject()
		{
			_role = base.Role as global::PlayerRoles.PlayableScps.Scp079.Scp079Role;
			_role.TryGetOwner(out _owner);
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler>(out _lostSignalHandler);
		}
	}
}
