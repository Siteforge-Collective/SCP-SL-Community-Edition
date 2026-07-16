namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049SenseAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049Role>
	{
		private const float BaseCooldown = 40f;

		private const float ReducedCooldown = 20f;

		private const float AttemptFailCooldown = 5f;

		private const float EffectDuration = 20f;

		private const float HeightDiffIgnoreY = 0.1f;

		private const float NearbyDistanceSqr = 4.5f;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Duration = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		public readonly global::System.Collections.Generic.HashSet<ReferenceHub> DeadTargets = new global::System.Collections.Generic.HashSet<ReferenceHub>();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _effectPrefab;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp049.Scp049AudioPlayer _audio;

		[global::UnityEngine.SerializeField]
		private float _dotThreshold = 0.88f;

		[global::UnityEngine.SerializeField]
		private float _distanceThreshold = 100f;

		private global::PlayerRoles.PlayableScps.Scp049.Scp049AttackAbility _attackAbility;

		private global::UnityEngine.Transform _pulseEffect;

		private bool _hasPulseUnsafe;

		public ReferenceHub Target { get; private set; }

		public bool HasTarget { get; private set; }

		public float DistanceFromTarget { get; private set; }

		protected override ActionName TargetKey => ActionName.ToggleFlashlight;

		public event global::System.Action OnFailed;

		public void ServerLoseTarget()
		{
			HasTarget = false;
			Cooldown.Trigger(20f);
			ServerSendRpc(toAll: true);
		}

		public void ServerProcessKilledPlayer(ReferenceHub hub)
		{
			if (HasTarget && !(Target != hub))
			{
				DeadTargets.Add(hub);
				Cooldown.Trigger(40f);
				HasTarget = false;
				ServerSendRpc(toAll: true);
			}
		}

		protected override void Update()
		{
			base.Update();
			if (HasTarget)
			{
				bool flag;
				if (Target.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole)
				{
					flag = true;
					global::UnityEngine.Vector3 position = humanRole.FpcModule.Position;
					global::UnityEngine.Vector3 position2 = base.ScpRole.FpcModule.Position;
					DistanceFromTarget = (position - position2).sqrMagnitude;
				}
				else
				{
					flag = false;
				}
				if (global::Mirror.NetworkServer.active && !(base.ScpRole.VisibilityController.ValidateVisibility(Target) && !Duration.IsReady && flag))
				{
					ServerLoseTarget();
				}
			}
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (Duration.IsReady && Cooldown.IsReady)
			{
				if (!CanFindTarget(out var bestTarget))
				{
					Target = null;
					this.OnFailed?.Invoke();
					ClientSendCmd();
				}
				else
				{
					Target = bestTarget;
					ClientSendCmd();
				}
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049AttackAbility>(out _attackAbility);
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
			_attackAbility.OnServerHit += OnServerHit;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			Cooldown.Clear();
			Duration.Clear();
			DeadTargets.Clear();
			HasTarget = false;
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Remove(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
			_attackAbility.OnServerHit -= OnServerHit;
		}

		private void OnServerHit(ReferenceHub hub)
		{
			if (HasTarget && !(hub == Target))
			{
				ServerLoseTarget();
			}
		}

		private void OnSpectatorTargetChanged()
		{
			if (_hasPulseUnsafe)
			{
				if (_pulseEffect != null)
				{
					global::UnityEngine.Object.Destroy(_pulseEffect.gameObject);
				}
				_hasPulseUnsafe = false;
			}
		}

		private void OnRoleChanged(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (global::Mirror.NetworkServer.active && (newRole is global::PlayerRoles.HumanRole || newRole is global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole))
			{
				DeadTargets.Remove(userHub);
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			if (!Cooldown.IsReady || !Duration.IsReady)
			{
				return;
			}
			HasTarget = false;
			Target = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
			if (Target == null)
			{
				Cooldown.Trigger(5f);
				ServerSendRpc(toAll: true);
			}
			else if (Target.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole)
			{
				float radius = humanRole.FpcModule.CharController.radius;
				global::UnityEngine.Vector3 cameraPosition = humanRole.CameraPosition;
				if (global::PlayerRoles.PlayableScps.VisionInformation.GetVisionInformation(base.Owner, base.Owner.PlayerCameraReference, cameraPosition, radius, _distanceThreshold).IsLooking)
				{
					Duration.Trigger(20f);
					HasTarget = true;
					ServerSendRpc(toAll: true);
				}
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, HasTarget ? Target : null);
			Cooldown.WriteCooldown(writer);
			Duration.WriteCooldown(writer);
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, Target);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			Target = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
			HasTarget = Target != null;
			if (_hasPulseUnsafe && _pulseEffect != null)
			{
				global::UnityEngine.Object.Destroy(_pulseEffect.gameObject);
				_hasPulseUnsafe = false;
			}
			if (HasTarget && (base.Owner.isLocalPlayer || global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner)))
			{
				_pulseEffect = global::UnityEngine.Object.Instantiate(_effectPrefab, Target.transform).transform;
				_hasPulseUnsafe = true;
				global::UnityEngine.Object.Destroy(_pulseEffect.gameObject, 3.5f);
			}
			Cooldown.ReadCooldown(reader);
			Duration.ReadCooldown(reader);
		}

		private bool CanFindTarget(out ReferenceHub bestTarget)
		{
			global::UnityEngine.Transform playerCameraReference = base.Owner.PlayerCameraReference;
			float num = _distanceThreshold * _distanceThreshold;
			float num2 = _dotThreshold;
			bool result = false;
			bestTarget = null;
			global::UnityEngine.Vector3 position = base.ScpRole.FpcModule.Position;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
				{
					continue;
				}
				global::UnityEngine.Vector3 position2 = humanRole.FpcModule.Position;
				global::UnityEngine.Vector3 vector = position2 - playerCameraReference.position;
				global::UnityEngine.Vector3 forward = playerCameraReference.forward;
				if (global::UnityEngine.Mathf.Abs((position2 - position).y) < 0.1f && vector.sqrMagnitude < 4.5f)
				{
					forward.y = 0f;
					forward.Normalize();
					vector.y = 0f;
				}
				float num3 = global::UnityEngine.Vector3.Dot(forward, vector.normalized);
				if (num3 < num2)
				{
					continue;
				}
				float sqrMagnitude = (position2 - position).sqrMagnitude;
				if (!(sqrMagnitude > num))
				{
					float radius = humanRole.FpcModule.CharacterControllerSettings.Radius;
					if (global::PlayerRoles.PlayableScps.VisionInformation.GetVisionInformation(base.Owner, playerCameraReference, humanRole.CameraPosition, radius, _distanceThreshold).IsLooking)
					{
						num = sqrMagnitude;
						bestTarget = allHub;
						num2 = num3;
						result = true;
					}
				}
			}
			return result;
		}
	}
}
