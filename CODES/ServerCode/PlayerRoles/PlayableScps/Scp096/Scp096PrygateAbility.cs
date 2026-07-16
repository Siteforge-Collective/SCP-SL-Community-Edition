namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096PrygateAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>
	{
		private global::Interactables.Interobjects.PryableDoor _syncDoor;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler _hitHandler;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer _audioPlayer;

		private const float DoorKillerHeight = 1.5f;

		private const float DoorKillerRadius = 0.2f;

		private const float MaxDisSqr = 8.12f;

		private const float HumanDamage = 200f;

		public void ClientTryPry(global::Interactables.Interobjects.PryableDoor door)
		{
			_syncDoor = door;
			ClientSendCmd();
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_hitHandler = new global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler(base.ScpRole, global::PlayerStatsSystem.Scp096DamageHandler.AttackType.GateKill, 0f, 0f, 200f, 200f);
		}

		public override void ResetObject()
		{
			base.ResetObject();
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::Mirror.NetworkWriterExtensions.WriteNetworkBehaviour(writer, _syncDoor);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (base.ScpRole.StateController.RageState != global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged || base.ScpRole.StateController.AbilityState == global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.PryingGate)
			{
				return;
			}
			_syncDoor = global::Mirror.NetworkReaderExtensions.ReadNetworkBehaviour<global::Interactables.Interobjects.PryableDoor>(reader);
			if (!(_syncDoor == null) && !_syncDoor.TargetState && !(_syncDoor.GetExactState() > 0f) && !((_syncDoor.transform.position - base.ScpRole.FpcModule.Position).sqrMagnitude > 8.12f))
			{
				base.Role.TryGetOwner(out var hub);
				if (_syncDoor.TryPryGate(hub))
				{
					_hitHandler.Clear();
					ServerSendRpc(toAll: true);
				}
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteNetworkBehaviour(writer, _syncDoor);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_syncDoor = global::Mirror.NetworkReaderExtensions.ReadNetworkBehaviour<global::Interactables.Interobjects.PryableDoor>(reader);
			if (global::Mirror.NetworkServer.active || base.Owner.isLocalPlayer)
			{
				(base.ScpRole.FpcModule as global::PlayerRoles.PlayableScps.Scp096.Scp096MovementModule).SetTargetGate(_syncDoor);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer>(out _audioPlayer);
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active && base.ScpRole.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.PryingGate) && !(_syncDoor == null))
			{
				global::UnityEngine.Vector3 position = _syncDoor.transform.position + global::UnityEngine.Vector3.up * 1.5f;
				global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult scp096HitResult = _hitHandler.DamageSphere(position, 0.2f);
				if (scp096HitResult != global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.None)
				{
					_audioPlayer.ServerPlayAttack(scp096HitResult);
					Hitmarker.SendHitmarker(base.Owner, 1f);
				}
			}
		}
	}
}
