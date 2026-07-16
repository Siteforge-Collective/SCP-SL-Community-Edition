namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106StalkAbility : global::PlayerRoles.PlayableScps.Scp106.Scp106VigorAbilityBase, global::GameObjectPools.IPoolResettable, global::InventorySystem.Items.IInteractionBlocker
	{
		private const float VigorRegeneration = 0.03f;

		private const float VigorStalkCostStationary = 0.06f;

		private const float VigorStalkCostMoving = 0.09f;

		private const float MinVigorToSubmerge = 0.25f;

		private const float SubmergeTime = 2.5f;

		private const global::InventorySystem.Items.BlockedInteraction Interactions = global::InventorySystem.Items.BlockedInteraction.All;

		private bool _isActive;

		private bool _valueDirty;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106SinkholeController _sinkhole;

		protected override ActionName TargetKey => ActionName.Run;

		public override bool IsSubmerged => IsActive;

		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			private set
			{
				if (_isActive != value && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp106Stalking, base.Owner, value))
				{
					_isActive = value;
					_valueDirty = true;
					base.Owner.interCoordinator.AddBlocker(this);
					if (value)
					{
						base.ScpRole.Sinkhole.TargetDuration = 2.5f;
					}
				}
			}
		}

		public global::InventorySystem.Items.BlockedInteraction BlockedInteractions => global::InventorySystem.Items.BlockedInteraction.All;

		public bool CanBeCleared => !IsActive;

		private void UpdateServerside()
		{
			if (_valueDirty)
			{
				_valueDirty = false;
				ServerSendRpc(toAll: true);
			}
			if (_sinkhole.IsDuringAnimation)
			{
				return;
			}
			if (IsActive)
			{
				float num = (base.ScpRole.FpcModule.Motor.MovementDetected ? 0.09f : 0.06f);
				base.Vigor.VigorAmount -= global::UnityEngine.Time.deltaTime * num;
				if (base.Vigor.VigorAmount == 0f && base.ScpRole.FpcModule.IsGrounded)
				{
					IsActive = false;
				}
			}
			else if (_sinkhole.Cooldown.IsReady && base.ScpRole.FpcModule.Motor.MovementDetected)
			{
				base.Vigor.VigorAmount += 0.03f * global::UnityEngine.Time.deltaTime;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_sinkhole = base.ScpRole.Sinkhole;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += delegate(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase old, global::PlayerRoles.PlayerRoleBase cur)
			{
				if (global::Mirror.NetworkServer.active && cur is global::PlayerRoles.Spectating.SpectatorRole)
				{
					ServerSendRpc(hub);
				}
			};
		}

		protected override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServerside();
			}
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (!_sinkhole.Cooldown.IsReady)
			{
				global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.PlayFlash(vigor: false);
			}
			ClientSendCmd();
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (_sinkhole.IsDuringAnimation || !_sinkhole.Cooldown.IsReady || !base.ScpRole.FpcModule.IsGrounded)
			{
				return;
			}
			if (IsActive)
			{
				IsActive = false;
			}
			else if (base.Vigor.VigorAmount < 0.25f)
			{
				if (base.Role.IsLocalPlayer)
				{
					global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.PlayFlash(vigor: true);
				}
				ServerSendRpc(toAll: false);
			}
			else
			{
				IsActive = true;
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, IsActive);
			_sinkhole.Cooldown.WriteCooldown(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			bool flag = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			_sinkhole.Cooldown.ReadCooldown(reader);
			if (!global::Mirror.NetworkServer.active)
			{
				if (flag == IsActive)
				{
					global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.PlayFlash(vigor: true);
				}
				else
				{
					IsActive = flag;
				}
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			IsActive = false;
		}
	}
}
