namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096TargetsTracker : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096Role>
	{
		private const float Vision096InnerAngle = 0.1f;

		private const float VisionTriggerDistance = 60f;

		private const float HeadSize = 0.12f;

		private const float PostRageCooldownDuration = 10f;

		public global::UnityEngine.GameObject TargetMarker;

		public readonly global::System.Collections.Generic.HashSet<ReferenceHub> Targets = new global::System.Collections.Generic.HashSet<ReferenceHub>();

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _postRageCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private readonly global::System.Collections.Generic.Dictionary<ReferenceHub, global::UnityEngine.GameObject> _markers = new global::System.Collections.Generic.Dictionary<ReferenceHub, global::UnityEngine.GameObject>();

		private readonly global::System.Collections.Generic.HashSet<ReferenceHub> _unvalidatedTargets = new global::System.Collections.Generic.HashSet<ReferenceHub>();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _targetSound;

		private bool _sendTargetsNextFrame;

		private bool _eventsAssigned;

		public bool CanReceiveTargets => base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Docile);

		public event global::System.Action<ReferenceHub> OnTargetAdded;

		public event global::System.Action<ReferenceHub> OnTargetAttacked;

		public event global::System.Action<ReferenceHub> OnTargetRemoved;

		public bool AddTarget(ReferenceHub target, bool isForLook)
		{
			if (target == null || Targets.Contains(target))
			{
				return false;
			}
			base.Role.TryGetOwner(out var hub);
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp096AddingTarget, hub, target, isForLook))
			{
				return false;
			}
			Targets.Add(target);
			if (!global::Mirror.NetworkServer.active && !_markers.ContainsKey(target))
			{
				_markers.Add(target, global::UnityEngine.Object.Instantiate(TargetMarker, target.transform));
			}
			_sendTargetsNextFrame = true;
			this.OnTargetAdded?.Invoke(target);
			return true;
		}

		public bool RemoveTarget(ReferenceHub target)
		{
			if (target == null || !Targets.Remove(target))
			{
				return false;
			}
			if (_markers.TryGetValue(target, out var value))
			{
				_markers.Remove(target);
				global::UnityEngine.Object.Destroy(value);
			}
			_sendTargetsNextFrame = true;
			this.OnTargetRemoved?.Invoke(target);
			return true;
		}

		public void ClearAllTargets()
		{
			foreach (ReferenceHub target in Targets)
			{
				if (_markers.TryGetValue(target, out var value))
				{
					_markers.Remove(target);
					global::UnityEngine.Object.Destroy(value);
				}
				this.OnTargetRemoved?.Invoke(target);
			}
			_sendTargetsNextFrame = true;
			Targets.Clear();
		}

		public bool IsObservedBy(ReferenceHub target)
		{
			global::UnityEngine.Vector3 position = (base.ScpRole.FpcModule.CharacterModelInstance as global::PlayerRoles.PlayableScps.Scp096.Scp096CharacterModel).Head.position;
			if (global::UnityEngine.Vector3.Dot((target.PlayerCameraReference.position - position).normalized, base.Owner.PlayerCameraReference.forward) < 0.1f)
			{
				return false;
			}
			return global::PlayerRoles.PlayableScps.VisionInformation.GetVisionInformation(target, target.PlayerCameraReference, position, 0.12f, 60f).IsLooking;
		}

		public bool HasTarget(ReferenceHub target)
		{
			return Targets.Contains(target);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(Targets, writer.WriteReferenceHub);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_unvalidatedTargets.UnionWith(Targets);
			while (reader.Position < reader.Length)
			{
				if (global::Utils.Networking.ReferenceHubReaderWriter.TryReadReferenceHub(reader, out var hub) && !_unvalidatedTargets.Remove(hub))
				{
					AddTarget(hub, isForLook: false);
				}
			}
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(_unvalidatedTargets, delegate(ReferenceHub x)
			{
				RemoveTarget(x);
			});
			_unvalidatedTargets.Clear();
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_eventsAssigned = true;
			base.Owner.playerStats.OnThisPlayerDamaged += AddTargetOnDamage;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			ClearAllTargets();
			if (_eventsAssigned)
			{
				_eventsAssigned = false;
				base.Owner.playerStats.OnThisPlayerDamaged -= AddTargetOnDamage;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, new global::System.Action<ReferenceHub>(CheckRemovedPlayer));
			base.ScpRole.StateController.OnRageUpdate += delegate(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState state)
			{
				if (state == global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming)
				{
					_postRageCooldown.Trigger(10f);
					ClearAllTargets();
				}
			};
		}

		private void AddTargetOnDamage(global::PlayerStatsSystem.DamageHandlerBase obj)
		{
			if (obj is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler)
			{
				ReferenceHub hub = attackerDamageHandler.Attacker.Hub;
				if (CanReceiveTargets && !(hub == null))
				{
					AddTarget(hub, isForLook: false);
					this.OnTargetAttacked?.Invoke(hub);
				}
			}
		}

		private void OnDestroy()
		{
			ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Remove(ReferenceHub.OnPlayerRemoved, new global::System.Action<ReferenceHub>(CheckRemovedPlayer));
		}

		private void Update()
		{
			bool visible = base.Owner.isLocalPlayer || global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner);
			global::Utils.NonAllocLINQ.DictionaryExtensions.ForEachValue(_markers, delegate(global::UnityEngine.GameObject x)
			{
				x.SetActive(visible);
			});
			if (global::Mirror.NetworkServer.active)
			{
				ServerCheckTargets();
			}
		}

		private void CheckRemovedPlayer(ReferenceHub ply)
		{
			RemoveTarget(ply);
		}

		private void ServerCheckTargets()
		{
			if (base.ScpRole.IsRageState(global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming) || !_postRageCooldown.IsReady)
			{
				return;
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				UpdateTarget(allHub);
			}
			if (_sendTargetsNextFrame)
			{
				_sendTargetsNextFrame = false;
				ServerSendRpc(toAll: true);
			}
		}

		private void UpdateTarget(ReferenceHub target)
		{
			if (!target.IsHuman())
			{
				RemoveTarget(target);
			}
			else if (IsObservedBy(target))
			{
				AddTarget(target, isForLook: true);
			}
		}
	}
}
