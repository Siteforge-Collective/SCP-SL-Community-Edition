namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049ResurrectIndicators : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049Role>
	{
		private struct PotentialRagdoll
		{
			public BasicRagdoll Ragdoll;

			public global::System.Diagnostics.Stopwatch Stopwatch;
		}

		private readonly struct Indicator
		{
			public readonly global::UnityEngine.GameObject Instance;

			private readonly global::UnityEngine.CanvasGroup _group;

			private readonly global::UnityEngine.SpriteRenderer _sprite;

			private static readonly global::UnityEngine.Color Transparent = new global::UnityEngine.Color(1f, 1f, 1f, 0f);

			public void SetAlpha(float f)
			{
				f = global::UnityEngine.Mathf.Clamp01(f);
				_group.alpha = f;
				_sprite.color = global::UnityEngine.Color.Lerp(Transparent, global::UnityEngine.Color.white, f);
			}

			public Indicator(global::UnityEngine.GameObject inst)
			{
				Instance = inst;
				_group = inst.GetComponentInChildren<global::UnityEngine.CanvasGroup>();
				_sprite = inst.GetComponentInChildren<global::UnityEngine.SpriteRenderer>();
				SetAlpha(0f);
			}
		}

		private enum ListSyncRpcType
		{
			FullResync = 0,
			Add = 1,
			Remove = 2
		}

		[global::UnityEngine.SerializeField]
		private float _showDelay;

		[global::UnityEngine.SerializeField]
		private float _fullOpacityDistance;

		[global::UnityEngine.SerializeField]
		private float _visibleDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _indicatorTemplate;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _posOffset;

		private readonly global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.Indicator> _indicatorInstances = new global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.Indicator>();

		private readonly global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.PotentialRagdoll> _potentialRagdolls = new global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.PotentialRagdoll>();

		private readonly global::System.Collections.Generic.HashSet<BasicRagdoll> _availableRagdolls = new global::System.Collections.Generic.HashSet<BasicRagdoll>();

		private global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility _resurrectAbility;

		private global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType _rpcType;

		private uint _syncRagdoll;

		private void Update()
		{
			if (global::Mirror.NetworkServer.active && !ServerCheckNew())
			{
				ServerRevalidateOld();
			}
		}

		private bool ServerCheckNew()
		{
			if (_potentialRagdolls.Count == 0)
			{
				return false;
			}
			if (_potentialRagdolls.Peek().Stopwatch.Elapsed.TotalSeconds < (double)_showDelay)
			{
				return false;
			}
			BasicRagdoll ragdoll = _potentialRagdolls.Dequeue().Ragdoll;
			if (ragdoll == null || !_resurrectAbility.CheckRagdoll(ragdoll))
			{
				return false;
			}
			_availableRagdolls.Add(ragdoll);
			ServerSendRpc(global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType.Add, ragdoll);
			return true;
		}

		private bool ServerRevalidateOld()
		{
			if (!global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(_availableRagdolls, (BasicRagdoll x) => !_resurrectAbility.CheckRagdoll(x), out var first))
			{
				return false;
			}
			_availableRagdolls.Remove(first);
			ServerSendRpc(global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType.Remove, first);
			return true;
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility>(out _resurrectAbility);
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			global::PlayerRoles.Ragdolls.RagdollManager.OnRagdollRemoved += OnRagdollRemoved;
			global::PlayerRoles.Ragdolls.RagdollManager.OnRagdollSpawned += OnRagdollSpawned;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_availableRagdolls.Clear();
			_potentialRagdolls.Clear();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
			global::PlayerRoles.Ragdolls.RagdollManager.OnRagdollRemoved -= OnRagdollRemoved;
			global::PlayerRoles.Ragdolls.RagdollManager.OnRagdollSpawned -= OnRagdollSpawned;
			global::Utils.NonAllocLINQ.DictionaryExtensions.ForEachValue(_indicatorInstances, delegate(global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.Indicator x)
			{
				global::UnityEngine.Object.Destroy(x.Instance);
			});
			_indicatorInstances.Clear();
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)_rpcType);
			if (_rpcType != global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType.FullResync)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, _syncRagdoll);
				return;
			}
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(_availableRagdolls, delegate(BasicRagdoll x)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, x.netId);
			});
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_rpcType = (global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType)reader.ReadByte();
			if (_rpcType == global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType.FullResync || !global::Mirror.NetworkIdentity.spawned.TryGetValue(global::Mirror.NetworkReaderExtensions.ReadUInt32(reader), out var value) || !value.TryGetComponent<BasicRagdoll>(out var component))
			{
				return;
			}
			if (_rpcType == global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType.Add)
			{
				_availableRagdolls.Add(component);
				return;
			}
			if (_indicatorInstances.TryGetValue(component.netId, out var value2))
			{
				global::UnityEngine.Object.Destroy(value2.Instance);
			}
			_availableRagdolls.Remove(component);
		}

		private void OnRoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (global::Mirror.NetworkServer.active && newRole is global::PlayerRoles.Spectating.SpectatorRole)
			{
				_rpcType = global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType.FullResync;
				ServerSendRpc(hub);
			}
		}

		private void OnRagdollSpawned(BasicRagdoll ragdoll)
		{
			if (global::Mirror.NetworkServer.active)
			{
				_potentialRagdolls.Enqueue(new global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.PotentialRagdoll
				{
					Ragdoll = ragdoll,
					Stopwatch = global::System.Diagnostics.Stopwatch.StartNew()
				});
			}
		}

		private void ServerSendRpc(global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectIndicators.ListSyncRpcType rpcType, BasicRagdoll ragdoll)
		{
			_rpcType = rpcType;
			_syncRagdoll = ragdoll.netId;
			ServerSendRpc((ReferenceHub x) => x == base.Owner || x.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole);
		}

		private void OnRagdollRemoved(BasicRagdoll ragdoll)
		{
			_availableRagdolls.Remove(ragdoll);
			if (_indicatorInstances.TryGetValue(ragdoll.netId, out var value))
			{
				global::UnityEngine.Object.Destroy(value.Instance);
				_indicatorInstances.Remove(ragdoll.netId);
			}
		}
	}
}
