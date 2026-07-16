namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096ChargeAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>
	{
		private static readonly global::UnityEngine.Collider[] DoorDetections = new global::UnityEngine.Collider[8];

		private static readonly CachedLayerMask ClientsideDoorDetectorMask = new CachedLayerMask("Door");

		private static readonly global::System.Collections.Generic.HashSet<global::UnityEngine.Collider> DisabledColliders = new global::System.Collections.Generic.HashSet<global::UnityEngine.Collider>();

		public const float DefaultChargeCooldown = 5f;

		private const float DefaultChargeDuration = 1f;

		private const float DamageObjects = 750f;

		private const float DamageTarget = 90f;

		private const float DamageNonTarget = 45f;

		private const float ConcussionDurationTargets = 10f;

		private const float ConcussionDurationNonTargets = 4f;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler _hitHandler;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker _targetsTracker;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer _audioPlayer;

		private global::UnityEngine.Transform _tr;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _detectionOffset;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _detectionExtents;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _soundsLethal;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _soundsNonLethal;

		[global::UnityEngine.SerializeField]
		private float _soundDistance;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Duration = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		public bool CanCharge
		{
			get
			{
				if (base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged) && base.ScpRole.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.None))
				{
					return Cooldown.IsReady;
				}
				return false;
			}
		}

		protected override ActionName TargetKey => ActionName.Zoom;

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			Cooldown.ReadCooldown(reader);
			Duration.ReadCooldown(reader);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			Cooldown.WriteCooldown(writer);
			Duration.WriteCooldown(writer);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (CanCharge)
			{
				base.Role.TryGetOwner(out var hub);
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp096Charging, hub))
				{
					_hitHandler.Clear();
					Duration.Trigger(1f);
					base.ScpRole.StateController.SetAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.Charging);
					ServerSendRpc(toAll: true);
				}
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_hitHandler = new global::PlayerRoles.PlayableScps.Scp096.Scp096HitHandler(base.ScpRole, global::PlayerStatsSystem.Scp096DamageHandler.AttackType.Charge, 750f, 750f, 90f, 45f);
			_hitHandler.OnPlayerHit += delegate(ReferenceHub ply)
			{
				ply.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Concussed>(_targetsTracker.HasTarget(ply) ? 10f : 4f);
			};
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			ClientSendCmd();
		}

		protected override void Awake()
		{
			base.Awake();
			_tr = base.transform;
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096AudioPlayer>(out _audioPlayer);
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096TargetsTracker>(out _targetsTracker);
			base.ScpRole.StateController.OnAbilityUpdate += delegate
			{
				foreach (global::UnityEngine.Collider disabledCollider in DisabledColliders)
				{
					if (!(disabledCollider == null))
					{
						disabledCollider.enabled = true;
					}
				}
				DisabledColliders.Clear();
			};
		}

		protected override void Update()
		{
			base.Update();
			if (base.ScpRole.IsAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.Charging))
			{
				if (global::Mirror.NetworkServer.active)
				{
					UpdateServer();
				}
				if (base.Role.IsLocalPlayer)
				{
					UpdateLocalClient();
				}
			}
		}

		private void UpdateServer()
		{
			if (Duration.IsReady || !base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged))
			{
				base.ScpRole.ResetAbilityState();
				Cooldown.Trigger(5f);
				ServerSendRpc(toAll: true);
				return;
			}
			global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult scp096HitResult = _hitHandler.DamageBox(_tr.TransformPoint(_detectionOffset), _detectionExtents, _tr.rotation);
			if (scp096HitResult != global::PlayerRoles.PlayableScps.Scp096.Scp096HitResult.None)
			{
				Hitmarker.SendHitmarker(base.Owner, 1f);
				_audioPlayer.ServerPlayAttack(scp096HitResult);
			}
		}

		private void UpdateLocalClient()
		{
			int num = global::UnityEngine.Physics.OverlapBoxNonAlloc(_tr.TransformPoint(_detectionOffset), _detectionExtents, DoorDetections, _tr.rotation, ClientsideDoorDetectorMask);
			for (int i = 0; i < num; i++)
			{
				if (DoorDetections[i].TryGetComponent<global::Interactables.InteractableCollider>(out var component))
				{
					CheckDoor(component.Target as global::Interactables.IInteractable);
				}
			}
		}

		private void CheckDoor(global::Interactables.IInteractable inter)
		{
			if (inter == null)
			{
				return;
			}
			if (!(inter is global::Interactables.Interobjects.BreakableDoor breakableDoor))
			{
				if (inter is global::Interactables.Interobjects.PryableDoor pryableDoor)
				{
					global::Interactables.Interobjects.PryableDoor door = pryableDoor;
					GetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096PrygateAbility>(out var sr);
					sr.ClientTryPry(door);
				}
				return;
			}
			foreach (global::UnityEngine.Collider scp106Collider in breakableDoor.Scp106Colliders)
			{
				if (scp106Collider.enabled)
				{
					scp106Collider.enabled = false;
					DisabledColliders.Add(scp106Collider);
				}
			}
		}
	}
}
