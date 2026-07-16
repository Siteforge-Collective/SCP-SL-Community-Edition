namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Attack : global::PlayerRoles.PlayableScps.Scp106.Scp106VigorAbilityBase
	{
		private const float TargetTraceTime = 0.35f;

		private const float VigorCaptureReward = 0.3f;

		private const float CooldownReductionReward = 5f;

		private const float TraumatizedDuration = 180f;

		private ReferenceHub _targetHub;

		private global::UnityEngine.Quaternion _ownerCamRotation;

		private global::UnityEngine.Vector3 _ownerPosition;

		private global::UnityEngine.Vector3 _targetPosition;

		private double _nextAttack;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _dotOverDistance;

		[global::UnityEngine.SerializeField]
		private float _maxRangeSqr;

		[global::UnityEngine.SerializeField]
		private float _hitCooldown;

		[global::UnityEngine.SerializeField]
		private float _missCooldown;

		[global::UnityEngine.SerializeField]
		private int _damage;

		private global::UnityEngine.Transform OwnerCam => base.Owner.PlayerCameraReference;

		protected override ActionName TargetKey => ActionName.Shoot;

		public static event global::System.Action<ReferenceHub> OnPlayerTeleported;

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			float num = -1f;
			_ownerPosition = base.ScpRole.FpcModule.Position;
			_ownerCamRotation = OwnerCam.rotation;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
				{
					continue;
				}
				global::UnityEngine.Vector3 position = humanRole.FpcModule.Position;
				global::UnityEngine.Vector3 vector = position - _ownerPosition;
				if (!(vector.sqrMagnitude > _maxRangeSqr))
				{
					float num2 = global::UnityEngine.Vector3.Dot(vector.normalized, OwnerCam.forward);
					if (!(num2 < num))
					{
						_targetPosition = position;
						_targetHub = allHub;
						num = num2;
					}
				}
			}
			if (num != -1f)
			{
				ClientSendCmd();
			}
		}

		private void SendCooldown(float cooldown)
		{
			if (!(cooldown <= 0f))
			{
				_nextAttack = global::Mirror.NetworkTime.time + (double)cooldown;
				ServerSendRpc((ReferenceHub x) => x == base.Owner || global::PlayerRoles.Spectating.SpectatorNetworking.IsSpectatedBy(base.Owner, x));
			}
		}

		private void ReduceSinkholeCooldown()
		{
			base.ScpRole.Sinkhole.Cooldown.NextUse -= 5.0;
		}

		private void ServerShoot()
		{
			using (new global::PlayerRoles.FirstPersonControl.FpcBacktracker(_targetHub, _targetPosition, 0.35f))
			{
				global::UnityEngine.Vector3 vector = _targetPosition - _ownerPosition;
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude > _maxRangeSqr)
				{
					return;
				}
				global::UnityEngine.Vector3 forward = OwnerCam.forward;
				forward.y = 0f;
				vector.y = 0f;
				if (global::UnityEngine.Physics.Linecast(_ownerPosition, _targetPosition, global::InventorySystem.Items.MicroHID.MicroHIDItem.WallMask))
				{
					return;
				}
				if (!(_dotOverDistance.Evaluate(sqrMagnitude) <= global::UnityEngine.Vector3.Dot(vector.normalized, forward.normalized)))
				{
					SendCooldown(_missCooldown);
					return;
				}
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp106TeleportPlayer, base.Owner, _targetHub))
				{
					return;
				}
				global::PlayerStatsSystem.DamageHandlerBase handler = new global::PlayerStatsSystem.ScpDamageHandler(base.Owner, _damage, global::PlayerStatsSystem.DeathTranslations.PocketDecay);
				if (!_targetHub.playerStats.DealDamage(handler))
				{
					return;
				}
			}
			SendCooldown(_hitCooldown);
			base.Vigor.VigorAmount += 0.3f;
			ReduceSinkholeCooldown();
			Hitmarker.SendHitmarker(base.Owner, 1f);
			global::PlayerRoles.PlayableScps.Scp106.Scp106Attack.OnPlayerTeleported?.Invoke(_targetHub);
			PlayerEffectsController playerEffectsController = _targetHub.playerEffectsController;
			playerEffectsController.EnableEffect<global::CustomPlayerEffects.Traumatized>(180f);
			playerEffectsController.EnableEffect<global::CustomPlayerEffects.Corroding>();
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, _targetHub);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(_targetPosition));
			global::Mirror.NetworkWriterExtensions.WriteQuaternion(writer, _ownerCamRotation);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(_ownerPosition));
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (_nextAttack > global::Mirror.NetworkTime.time || base.ScpRole.Sinkhole.NormalizedState > 0f)
			{
				return;
			}
			_targetHub = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
			_targetPosition = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position;
			_ownerCamRotation = global::Mirror.NetworkReaderExtensions.ReadQuaternion(reader);
			_ownerPosition = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position;
			if (_targetHub == null || !_targetHub.IsHuman())
			{
				return;
			}
			using (new global::PlayerRoles.FirstPersonControl.FpcBacktracker(base.Owner, _ownerPosition, _ownerCamRotation))
			{
				ServerShoot();
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _nextAttack);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			ReduceSinkholeCooldown();
			global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.PlayCooldownAnimation(global::Mirror.NetworkReaderExtensions.ReadDouble(reader));
		}
	}
}
