namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class Scp079PingAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase
	{
		[global::UnityEngine.SerializeField]
		private int _cost;

		[global::UnityEngine.SerializeField]
		private float _instantCooldown;

		[global::UnityEngine.SerializeField]
		private float _groupCooldown;

		[global::UnityEngine.SerializeField]
		private int _groupSize;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Pinging.Scp079PingInstance _prefab;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite[] _icons;

		private string _abilityName;

		private string _cooldownMsg;

		private RateLimiter _rateLimiter;

		private byte _syncProcessorIndex;

		private global::RelativePositioning.RelativePosition _syncPos;

		private global::UnityEngine.Vector3 _syncNormal;

		private const float RaycastMaxDis = 130f;

		private static readonly global::PlayerRoles.PlayableScps.Scp079.Pinging.IPingProcessor[] PingProcessors = new global::PlayerRoles.PlayableScps.Scp079.Pinging.IPingProcessor[7]
		{
			new global::PlayerRoles.PlayableScps.Scp079.Pinging.GeneratorPingProcessor(),
			new global::PlayerRoles.PlayableScps.Scp079.Pinging.ProjectilePingProcessor(),
			new global::PlayerRoles.PlayableScps.Scp079.Pinging.MicroHidPingProcesssor(),
			new global::PlayerRoles.PlayableScps.Scp079.Pinging.HumanPingProcessor(),
			new global::PlayerRoles.PlayableScps.Scp079.Pinging.ElevatorPingProcessor(),
			new global::PlayerRoles.PlayableScps.Scp079.Pinging.DoorPingProcessor(),
			new global::PlayerRoles.PlayableScps.Scp079.Pinging.DefaultPingProcessor()
		};

		public override ActionName ActivationKey => ActionName.Scp079PingLocation;

		public override bool IsReady
		{
			get
			{
				if (base.AuxManager.CurrentAux >= (float)_cost)
				{
					return _rateLimiter.AllReady;
				}
				return false;
			}
		}

		public override bool IsVisible => !global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras;

		public override string AbilityName => string.Format(_abilityName, _cost);

		public override string FailMessage
		{
			get
			{
				if (!(base.AuxManager.CurrentAux < (float)_cost))
				{
					if (!_rateLimiter.RateReady)
					{
						return _cooldownMsg;
					}
					return null;
				}
				return GetNoAuxMessage(_cost);
			}
		}

		private void SpawnIndicator(int processorIndex, global::RelativePositioning.RelativePosition pos, global::UnityEngine.Vector3 normal)
		{
		}

		private void WriteSyncVars(global::Mirror.NetworkWriter writer)
		{
			writer.WriteByte(_syncProcessorIndex);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _syncPos);
			global::Mirror.NetworkWriterExtensions.WriteVector3(writer, _syncNormal);
		}

		private bool ServerCheckReceiver(ReferenceHub hub, global::UnityEngine.Vector3 point, int processorIndex)
		{
			global::PlayerRoles.PlayerRoleBase currentRole = hub.roleManager.CurrentRole;
			if (!(currentRole is global::PlayerRoles.PlayableScps.FpcStandardScp fpcStandardScp))
			{
				if (!hub.IsSCP())
				{
					return currentRole is global::PlayerRoles.Spectating.SpectatorRole;
				}
				return true;
			}
			float range = PingProcessors[processorIndex].Range;
			float num = range * range;
			global::UnityEngine.Vector3 position = fpcStandardScp.FpcModule.Position;
			global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(point);
			if (roomIdentifier == null)
			{
				return false;
			}
			global::UnityEngine.Vector3 gridScale = global::MapGeneration.RoomIdentifier.GridScale;
			global::UnityEngine.Vector3Int[] occupiedCoords = roomIdentifier.OccupiedCoords;
			for (int i = 0; i < occupiedCoords.Length; i++)
			{
				if (!(new global::UnityEngine.Bounds(global::UnityEngine.Vector3.Scale(occupiedCoords[i], gridScale), gridScale).SqrDistance(position) > num))
				{
					return true;
				}
			}
			return false;
		}

		protected override void Start()
		{
			base.Start();
			_rateLimiter = new RateLimiter(_instantCooldown, _groupSize, _groupCooldown);
			_abilityName = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.PingLocation);
			_cooldownMsg = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.PingRateLimited);
		}

		protected override void Trigger()
		{
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			WriteSyncVars(writer);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (!IsReady || !base.Role.TryGetOwner(out var _) || base.LostSignalHandler.Lost)
			{
				return;
			}
			_syncProcessorIndex = reader.ReadByte();
			if (_syncProcessorIndex < PingProcessors.Length)
			{
				_syncPos = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
				_syncNormal = global::Mirror.NetworkReaderExtensions.ReadVector3(reader);
				ServerSendRpc((ReferenceHub x) => ServerCheckReceiver(x, _syncPos.Position, _syncProcessorIndex));
				base.AuxManager.CurrentAux -= _cost;
				_rateLimiter.RegisterInput();
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			WriteSyncVars(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			SpawnIndicator(reader.ReadByte(), global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader), global::Mirror.NetworkReaderExtensions.ReadVector3(reader));
		}
	}
}
