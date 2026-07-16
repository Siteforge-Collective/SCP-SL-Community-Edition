using System.Diagnostics;
using GameObjectPools;
using Mirror;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079CameraRotationSync : SubroutineBase, IPoolSpawnable
	{
		private Scp079Role _role;

		private Scp079CurrentCameraSync _curCamSync;

		private Scp079LostSignalHandler _lostSignalHandler;

		private ReferenceHub _owner;

		private readonly Stopwatch _clientSendLimit;

		private const float ClientSendRate = 15f;

		private void Update()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public void SpawnObject()
		{
		}
	}
}
