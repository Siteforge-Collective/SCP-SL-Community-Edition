namespace PlayerRoles.PlayableScps
{
	public abstract class ScpAttackAbilityBase<T> : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<T> where T : global::PlayerRoles.PlayerRoleBase, global::PlayerRoles.FirstPersonControl.IFpcRole
	{
		[global::UnityEngine.SerializeField]
		private float _detectionRadius;

		[global::UnityEngine.SerializeField]
		private float _detectionOffset;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _killSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _hitClipsHuman;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _hitClipsObjects;

		private bool _attackTriggered;

		private global::PlayerRoles.PlayableScps.AttackResult _syncAttack;

		private readonly global::System.Diagnostics.Stopwatch _delaySw = new global::System.Diagnostics.Stopwatch();

		private readonly global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown _clientCooldown = new global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown();

		private readonly global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown _serverCooldown = new global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown();

		private static readonly global::System.Collections.Generic.HashSet<ReferenceHub> TargettedPlayers = new global::System.Collections.Generic.HashSet<ReferenceHub>();

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.FpcBacktracker> BacktrackedPlayers = new global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.FpcBacktracker>();

		private static readonly global::UnityEngine.Collider[] DetectionsNonAlloc = new global::UnityEngine.Collider[128];

		private static readonly CachedLayerMask DetectionMask = new CachedLayerMask("Hitbox", "Glass");

		private static readonly CachedLayerMask BlockerMask = new CachedLayerMask("Locker", "Default", "Door");

		private const int DetectionsNumber = 128;

		public global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown Cooldown
		{
			get
			{
				if (!base.Owner.isLocalPlayer && global::Mirror.NetworkServer.active)
				{
					return _serverCooldown;
				}
				return _clientCooldown;
			}
		}

		public abstract float DamageAmount { get; }

		protected abstract global::PlayerStatsSystem.DamageHandlerBase DamageHandler { get; }

		protected virtual float SoundRange => 13f;

		protected virtual float AttackDelay => 0f;

		protected virtual float BaseCooldown => 1f;

		protected virtual bool SelfRepeating => true;

		protected virtual bool CanTriggerAbility => _clientCooldown.IsReady;

		protected override ActionName TargetKey => ActionName.Shoot;

		private global::UnityEngine.Transform PlyCam => base.Owner.PlayerCameraReference;

		private global::UnityEngine.Vector3 OverlapSphereOrigin => PlyCam.position + PlyCam.forward * _detectionOffset;

		public event global::System.Action<global::PlayerRoles.PlayableScps.AttackResult> OnAttacked;

		public event global::System.Action OnTriggered;

		private void ServerPerformAttack()
		{
			int num = global::UnityEngine.Physics.OverlapSphereNonAlloc(OverlapSphereOrigin, _detectionRadius, DetectionsNonAlloc, DetectionMask);
			_syncAttack = global::PlayerRoles.PlayableScps.AttackResult.None;
			for (int i = 0; i < num; i++)
			{
				if (!DetectionsNonAlloc[i].TryGetComponent<IDestructible>(out var component) || global::UnityEngine.Physics.Linecast(PlyCam.position, component.CenterOfMass, BlockerMask) || (component is HitboxIdentity hitboxIdentity && !TargettedPlayers.Remove(hitboxIdentity.TargetHub)) || !component.Damage(DamageAmount, DamageHandler, component.CenterOfMass))
				{
					continue;
				}
				OnDestructibleDamaged(component);
				_syncAttack |= global::PlayerRoles.PlayableScps.AttackResult.AttackedObject;
				if (component is HitboxIdentity hitboxIdentity2)
				{
					_syncAttack |= global::PlayerRoles.PlayableScps.AttackResult.AttackedHuman;
					if (!(hitboxIdentity2.TargetHub.playerStats.GetModule<global::PlayerStatsSystem.HealthStat>().CurValue > 0f))
					{
						_syncAttack |= global::PlayerRoles.PlayableScps.AttackResult.KilledHuman;
					}
				}
			}
			ServerSendRpc(toAll: true);
		}

		protected virtual void OnDestructibleDamaged(IDestructible dest)
		{
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			if (_attackTriggered)
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, default(global::RelativePositioning.RelativePosition));
				return;
			}
			global::UnityEngine.Vector3 position = base.ScpRole.FpcModule.Position;
			float num = _detectionOffset + _detectionRadius;
			float num2 = num * num;
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(position));
			writer.WriteLowPrecisionQuaternion(new LowPrecisionQuaternion(PlyCam.rotation));
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole)
				{
					global::UnityEngine.Vector3 position2 = humanRole.FpcModule.Position;
					if (!((position2 - position).sqrMagnitude > num2))
					{
						global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, allHub);
						global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(position2));
					}
				}
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			global::RelativePositioning.RelativePosition relativePosition = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
			if (relativePosition.WaypointId == 0)
			{
				_attackTriggered = true;
				ServerSendRpc(toAll: true);
			}
			else
			{
				if (!_serverCooldown.TolerantIsReady && !base.Owner.isLocalPlayer)
				{
					return;
				}
				_attackTriggered = false;
				global::UnityEngine.Vector3 position = relativePosition.Position;
				global::UnityEngine.Quaternion value = reader.ReadLowPrecisionQuaternion().Value;
				BacktrackedPlayers.Add(new global::PlayerRoles.FirstPersonControl.FpcBacktracker(base.Owner, position, value));
				while (reader.Position < reader.Length)
				{
					ReferenceHub referenceHub = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
					global::RelativePositioning.RelativePosition relativePosition2 = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
					if (!(referenceHub == null) && referenceHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole)
					{
						BacktrackedPlayers.Add(new global::PlayerRoles.FirstPersonControl.FpcBacktracker(referenceHub, relativePosition2.Position));
						TargettedPlayers.Add(referenceHub);
					}
				}
				ServerPerformAttack();
				global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(BacktrackedPlayers, delegate(global::PlayerRoles.FirstPersonControl.FpcBacktracker x)
				{
					x.RestorePosition();
				});
				_serverCooldown.Trigger(BaseCooldown);
				BacktrackedPlayers.Clear();
				TargettedPlayers.Clear();
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			if (!_attackTriggered)
			{
				writer.WriteByte((byte)_syncAttack);
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (reader.Position >= reader.Length)
			{
				if (!base.Owner.isLocalPlayer)
				{
					_clientCooldown.Trigger(BaseCooldown);
					this.OnTriggered?.Invoke();
				}
				return;
			}
			_syncAttack = (global::PlayerRoles.PlayableScps.AttackResult)reader.ReadByte();
			this.OnAttacked?.Invoke(_syncAttack);
			if (_syncAttack != global::PlayerRoles.PlayableScps.AttackResult.None && (base.Owner.isLocalPlayer || global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner)))
			{
				Hitmarker.PlayHitmarker(1f);
			}
			if (HasFlagFast(global::PlayerRoles.PlayableScps.AttackResult.KilledHuman) && _killSound != null)
			{
				global::AudioPooling.AudioSourcePoolManager.PlaySound(_killSound, base.transform, SoundRange);
			}
			else if (HasFlagFast(global::PlayerRoles.PlayableScps.AttackResult.AttackedHuman) && _hitClipsHuman.Length != 0)
			{
				global::AudioPooling.AudioSourcePoolManager.PlaySound(_hitClipsHuman.RandomItem(), base.transform, SoundRange);
			}
			else if (HasFlagFast(global::PlayerRoles.PlayableScps.AttackResult.AttackedObject) && _hitClipsObjects.Length != 0)
			{
				global::AudioPooling.AudioSourcePoolManager.PlaySound(_hitClipsObjects.RandomItem(), base.transform, SoundRange);
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_attackTriggered = false;
			_delaySw.Reset();
			_clientCooldown.Clear();
			_serverCooldown.Clear();
			TargettedPlayers.Clear();
			BacktrackedPlayers.Clear();
		}

		protected override void Update()
		{
			base.Update();
		}

		protected virtual void OnClientUpdate()
		{
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (!_attackTriggered && !SelfRepeating && CanTriggerAbility)
			{
				ClientPerformAttack();
			}
		}

		protected virtual void ClientPerformAttack(bool attackTriggered = true)
		{
		}

		private bool HasFlagFast(global::PlayerRoles.PlayableScps.AttackResult flag)
		{
			return (_syncAttack & flag) == flag;
		}

		private void OnDrawGizmosSelected()
		{
			if (!(base.Owner == null))
			{
				global::UnityEngine.Gizmos.color = global::UnityEngine.Color.green;
				global::UnityEngine.Gizmos.DrawWireSphere(OverlapSphereOrigin, _detectionRadius);
			}
		}
	}
}
