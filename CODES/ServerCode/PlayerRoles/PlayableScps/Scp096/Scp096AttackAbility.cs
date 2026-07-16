namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096AttackAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>, global::GameObjectPools.IPoolResettable
	{
		public const float DefaultAttackCooldown = 0.5f;

		private const float HumanDamage = 85f;

		private const float DoorDamage = 250f;

		private const int WindowDamage = 500;

		private const float BacktrackingDisSqr = 9f;

		private const byte LeftAttackSyncCode = 64;

		[global::UnityEngine.SerializeField]
		private float _sphereHitboxRadius;

		[global::UnityEngine.SerializeField]
		private float _sphereHitboxOffset;

		private static readonly global::System.Collections.Generic.List<global::PlayerRoles.FirstPersonControl.FpcBacktracker> BacktrackedPlayers = new global::System.Collections.Generic.List<global::PlayerRoles.FirstPersonControl.FpcBacktracker>();

		private static readonly global::System.Collections.Generic.List<ReferenceHub> PlayersToSend = new global::System.Collections.Generic.List<ReferenceHub>();

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _clientAttackCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private readonly global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown _serverAttackCooldown = new global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown();

		private global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler _leftHitHandler;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler _rightHitHandler;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer _audioPlayer;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult _hitResult;

		private bool AttackPossible
		{
			get
			{
				if (base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged))
				{
					return base.ScpRole.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.None);
				}
				return false;
			}
		}

		protected override ActionName TargetKey => ActionName.Shoot;

		public bool LeftAttack { get; private set; }

		public event global::System.Action<global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult> OnHitReceived;

		public event global::System.Action OnAttackTriggered;

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			byte b = (byte)_hitResult;
			if (LeftAttack)
			{
				b |= 0x40;
			}
			writer.WriteByte(b);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			byte b = reader.ReadByte();
			LeftAttack = (b & 0x40) != 0;
			global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult scp096HitResult = (global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult)(b & -65);
			this.OnHitReceived?.Invoke(scp096HitResult);
			if (scp096HitResult != global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.None && (base.Owner.isLocalPlayer || global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner)))
			{
				Hitmarker.PlayHitmarker(1f);
			}
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(base.ScpRole.FpcModule.Position));
			global::Mirror.NetworkWriterExtensions.WriteQuaternion(writer, base.Owner.PlayerCameraReference.rotation);
			foreach (ReferenceHub item in PlayersToSend)
			{
				global::UnityEngine.Vector3 position = (item.roleManager.CurrentRole as global::PlayerRoles.HumanRole).FpcModule.Position;
				global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, item);
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(position));
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (!_serverAttackCooldown.TolerantIsReady || !AttackPossible)
			{
				return;
			}
			BacktrackedPlayers.Clear();
			global::RelativePositioning.RelativePosition relativePosition = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
			BacktrackedPlayers.Add(new global::PlayerRoles.FirstPersonControl.FpcBacktracker(base.Owner, relativePosition.Position, global::Mirror.NetworkReaderExtensions.ReadQuaternion(reader)));
			while (reader.Position < reader.Length)
			{
				ReferenceHub hub;
				bool num = global::Utils.Networking.ReferenceHubReaderWriter.TryReadReferenceHub(reader, out hub);
				global::UnityEngine.Vector3 position = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position;
				if (num)
				{
					BacktrackedPlayers.Add(new global::PlayerRoles.FirstPersonControl.FpcBacktracker(hub, position));
				}
			}
			ServerAttack();
			BacktrackedPlayers.ForEach(delegate(global::PlayerRoles.FirstPersonControl.FpcBacktracker x)
			{
				x.RestorePosition();
			});
			_serverAttackCooldown.Trigger(0.5f);
		}

		private void ServerAttack()
		{
			if (global::Mirror.NetworkServer.active)
			{
				LeftAttack = !LeftAttack;
				base.ScpRole.StateController.SetAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.Attacking);
				global::UnityEngine.Transform playerCameraReference = base.Owner.PlayerCameraReference;
				global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler scp096HitHandler = (LeftAttack ? _leftHitHandler : _rightHitHandler);
				scp096HitHandler.Clear();
				_hitResult = scp096HitHandler.DamageSphere(playerCameraReference.position + playerCameraReference.forward * _sphereHitboxOffset, _sphereHitboxRadius);
				_audioPlayer.ServerPlayAttack(_hitResult);
				ServerSendRpc(toAll: true);
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_clientAttackCooldown.Clear();
			_serverAttackCooldown.Clear();
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_leftHitHandler = new global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler(base.ScpRole, global::PlayerStatsSystem.Scp096DamageHandler.AttackType.SlapLeft, 500f, 250f, 85f, 0f);
			_rightHitHandler = new global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler(base.ScpRole, global::PlayerStatsSystem.Scp096DamageHandler.AttackType.SlapRight, 500f, 250f, 85f, 0f);
		}

		protected override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active && _serverAttackCooldown.IsReady && base.ScpRole.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.Attacking))
			{
				base.ScpRole.ResetAbilityState();
			}
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (!AttackPossible || !_clientAttackCooldown.IsReady)
			{
				return;
			}
			PlayersToSend.Clear();
			global::UnityEngine.Vector3 position = base.ScpRole.FpcModule.Position;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole && !((humanRole.FpcModule.Position - position).sqrMagnitude > 9f))
				{
					PlayersToSend.Add(allHub);
				}
			}
			ClientSendCmd();
			_clientAttackCooldown.Trigger(0.5f);
			this.OnAttackTriggered?.Invoke();
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer>(out _audioPlayer);
		}
	}
}
