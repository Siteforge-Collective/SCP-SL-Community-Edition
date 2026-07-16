namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173TeleportAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173Role>
	{
		[global::System.Flags]
		private enum CmdTeleportData
		{
			Aiming = 1,
			WantsToTeleport = 2
		}

		private const float BlinkDistance = 8f;

		private const float BreakneckDistanceMultiplier = 1.8f;

		private const float KillRadiusSqr = 1.66f;

		private const float KillHeight = 2.2f;

		private const float KillBacktracking = 0.4f;

		private const float ClientDistanceAddition = 0.1f;

		private const int GlassLayerMask = 16384;

		private const float GlassDestroyRadius = 0.8f;

		private static readonly global::UnityEngine.Collider[] DetectedColliders = new global::UnityEngine.Collider[8];

		private global::PlayerRoles.PlayableScps.Scp173.Scp173MovementModule _fpcModule;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observersTracker;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173BlinkTimer _blinkTimer;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer _audioSubroutine;

		private bool _isAiming;

		private float _targetDis;

		private global::UnityEngine.Vector3 _tpPosition;

		private float _lastBlink;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData _cmdData;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportIndicator _tpIndicator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _blinkIntensity;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Rendering.PostProcessing.PostProcessVolume _blinkEffect;

		private float EffectiveBlinkDistance => 8f * (_breakneckSpeedsAbility.IsActive ? 1.8f : 1f);

		protected override ActionName TargetKey => ActionName.Zoom;

		public ReferenceHub BestTarget
		{
			get
			{
				ReferenceHub result = null;
				float num = float.MaxValue;
				foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
				{
					if (allHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole)
					{
						global::UnityEngine.Vector3 position = humanRole.FpcModule.Position;
						global::UnityEngine.Vector3 tpPosition = _tpPosition;
						if ((position - tpPosition).MagnitudeOnlyY() < 2.2f)
						{
							position.y = 0f;
							tpPosition.y = 0f;
						}
						float sqrMagnitude = (position - tpPosition).sqrMagnitude;
						if (!(sqrMagnitude > num))
						{
							result = allHub;
							num = sqrMagnitude;
						}
					}
				}
				if (!(num > 1.66f))
				{
					return result;
				}
				return null;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_fpcModule = base.ScpRole.FpcModule as global::PlayerRoles.PlayableScps.Scp173.Scp173MovementModule;
			global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule subroutineModule = base.ScpRole.SubroutineModule;
			subroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observersTracker);
			subroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility>(out _breakneckSpeedsAbility);
			subroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer>(out _audioSubroutine);
			subroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BlinkTimer>(out _blinkTimer);
		}

		protected override void Update()
		{
			base.Update();
			if (!base.Owner.isLocalPlayer && !global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner))
			{
				if (_isAiming)
				{
					_isAiming = false;
					_tpIndicator.UpdateVisibility(isVisible: false);
				}
				return;
			}
			bool flag = ((!base.Owner.isLocalPlayer) ? HasDataFlag(global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.Aiming) : (IsKeyHeld && !global::UnityEngine.Cursor.visible));
			if (_isAiming)
			{
				UpdateAiming(!flag);
			}
			else if (flag)
			{
				_isAiming = true;
			}
		}

		private void UpdateAiming(bool wantsToTeleport)
		{
			bool flag = _fpcModule.TryGetTeleportPos(EffectiveBlinkDistance, out _tpPosition, out _targetDis);
			if (!wantsToTeleport)
			{
				_tpIndicator.UpdateVisibility(flag && _blinkTimer.AbilityReady);
				_tpIndicator.transform.position = _tpPosition;
				if (!HasDataFlag(global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.Aiming))
				{
					_cmdData = global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.Aiming;
					ClientSendCmd();
				}
			}
			else
			{
				if (base.Owner.isLocalPlayer)
				{
					_cmdData = ((!flag) ? global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.Aiming : global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.WantsToTeleport);
					ClientSendCmd();
				}
				_isAiming = false;
				_tpIndicator.UpdateVisibility(isVisible: false);
			}
		}

		private bool TryBlink(float maxDis)
		{
			maxDis = global::UnityEngine.Mathf.Clamp(maxDis, 0f, EffectiveBlinkDistance);
			if (!_blinkTimer.AbilityReady)
			{
				return false;
			}
			if (!_fpcModule.TryGetTeleportPos(maxDis, out _tpPosition, out var _))
			{
				return false;
			}
			float num = _fpcModule.CharController.height / 2f;
			_blinkTimer.ServerBlink(_tpPosition + global::UnityEngine.Vector3.up * num);
			return true;
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			writer.WriteByte((byte)_cmdData);
			if (HasDataFlag(global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.WantsToTeleport))
			{
				global::Mirror.NetworkWriterExtensions.WriteQuaternion(writer, base.Owner.PlayerCameraReference.rotation);
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _targetDis + 0.1f);
				global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, BestTarget);
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			_cmdData = (global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData)reader.ReadByte();
			if (!HasDataFlag(global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.WantsToTeleport))
			{
				ServerSendRpc(toAll: true);
			}
			else
			{
				if (!_blinkTimer.AbilityReady)
				{
					return;
				}
				global::UnityEngine.Transform playerCameraReference = base.Owner.PlayerCameraReference;
				global::System.Collections.Generic.HashSet<ReferenceHub> prevObservers = new global::System.Collections.Generic.HashSet<ReferenceHub>(_observersTracker.Observers);
				global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData cmdData = _cmdData;
				_cmdData = (global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData)0;
				ServerSendRpc(toAll: true);
				_cmdData = cmdData;
				global::UnityEngine.Quaternion rotation = playerCameraReference.rotation;
				playerCameraReference.rotation = global::Mirror.NetworkReaderExtensions.ReadQuaternion(reader);
				bool num = TryBlink(global::Mirror.NetworkReaderExtensions.ReadSingle(reader));
				playerCameraReference.rotation = rotation;
				if (!num)
				{
					return;
				}
				prevObservers.UnionWith(_observersTracker.Observers);
				ServerSendRpc((ReferenceHub x) => prevObservers.Contains(x));
				_audioSubroutine.ServerSendSound(global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173SoundId.Teleport);
				if (_breakneckSpeedsAbility.IsActive)
				{
					return;
				}
				int num2 = global::UnityEngine.Physics.OverlapSphereNonAlloc(_fpcModule.Position, 0.8f, DetectedColliders, 16384);
				for (int num3 = 0; num3 < num2; num3++)
				{
					if (DetectedColliders[num3].TryGetComponent<BreakableWindow>(out var component))
					{
						component.Damage(component.health, base.ScpRole.DamageHandler, global::UnityEngine.Vector3.zero);
					}
				}
				ReferenceHub referenceHub = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
				if (!(referenceHub == null) && referenceHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp173SnapPlayer, base.Owner, referenceHub))
				{
					global::UnityEngine.Bounds bounds = humanRole.FpcModule.Tracer.GenerateBounds(0.4f, ignoreTeleports: true);
					bounds.Encapsulate(new global::UnityEngine.Bounds(humanRole.FpcModule.Position, global::UnityEngine.Vector3.up * 2.2f));
					if (!(bounds.SqrDistance(_fpcModule.Position) > 1.66f) && referenceHub.playerStats.DealDamage(base.ScpRole.DamageHandler) && base.ScpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer>(out var subroutine))
					{
						Hitmarker.SendHitmarker(base.Owner, 1f);
						subroutine.ServerSendSound(global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173SoundId.Snap);
					}
				}
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)_cmdData);
			if (HasDataFlag(global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.WantsToTeleport))
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(_fpcModule.Position));
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_cmdData = (global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData)reader.ReadByte();
			if (HasDataFlag(global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData.WantsToTeleport))
			{
				global::RelativePositioning.RelativePosition receivedPosition = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
				_fpcModule.Motor.ReceivedPosition = receivedPosition;
				_fpcModule.Position = receivedPosition.Position;
				_lastBlink = global::UnityEngine.Time.timeSinceLevelLoad;
				_blinkEffect.weight = 1f;
				(_fpcModule.CharacterModelInstance as global::PlayerRoles.PlayableScps.Scp173.Scp173CharacterModel).Frozen = false;
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_lastBlink = 0f;
		}

		private bool HasDataFlag(global::PlayerRoles.PlayableScps.Scp173.Scp173TeleportAbility.CmdTeleportData ctd)
		{
			return (_cmdData & ctd) == ctd;
		}
	}
}
