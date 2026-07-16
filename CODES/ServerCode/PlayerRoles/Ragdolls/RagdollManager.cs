namespace PlayerRoles.Ragdolls
{
	public static class RagdollManager
	{
		private static readonly global::System.Collections.Generic.HashSet<global::Mirror.NetworkIdentity> CachedRagdollPrefabs = new global::System.Collections.Generic.HashSet<global::Mirror.NetworkIdentity>();

		private static bool _prefabsCacheSet;

		public static int CleanupTime { get; set; }

		private static global::System.Collections.Generic.HashSet<global::Mirror.NetworkIdentity> AllRagdollPrefabs
		{
			get
			{
				if (_prefabsCacheSet)
				{
					return CachedRagdollPrefabs;
				}
				foreach (global::System.Collections.Generic.KeyValuePair<global::PlayerRoles.RoleTypeId, global::PlayerRoles.PlayerRoleBase> allRole in global::PlayerRoles.PlayerRoleLoader.AllRoles)
				{
					if (allRole.Value is global::PlayerRoles.IRagdollRole ragdollRole)
					{
						CachedRagdollPrefabs.Add(ragdollRole.Ragdoll.netIdentity);
					}
				}
				_prefabsCacheSet = true;
				return CachedRagdollPrefabs;
			}
		}

		public static event global::System.Action<BasicRagdoll> OnRagdollSpawned;

		public static event global::System.Action<BasicRagdoll> OnRagdollRemoved;

		internal static void OnSpawnedRagdoll(BasicRagdoll ragdoll)
		{
			global::PlayerRoles.Ragdolls.RagdollManager.OnRagdollSpawned?.Invoke(ragdoll);
		}

		internal static void OnRemovedRagdoll(BasicRagdoll ragdoll)
		{
			global::PlayerRoles.Ragdolls.RagdollManager.OnRagdollRemoved?.Invoke(ragdoll);
		}

		public static BasicRagdoll ServerSpawnRagdoll(ReferenceHub owner, global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			if (!global::Mirror.NetworkServer.active || owner == null)
			{
				return null;
			}
			if (!(owner.roleManager.CurrentRole is global::PlayerRoles.IRagdollRole ragdollRole))
			{
				return null;
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(ragdollRole.Ragdoll.gameObject);
			if (gameObject.TryGetComponent<BasicRagdoll>(out var component))
			{
				global::UnityEngine.Transform transform = ragdollRole.Ragdoll.transform;
				component.NetworkInfo = new RagdollData(owner, handler, transform.localPosition, transform.localRotation);
			}
			else
			{
				component = null;
			}
			global::Mirror.NetworkServer.Spawn(gameObject);
			return component;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(AllRagdollPrefabs, delegate(global::Mirror.NetworkIdentity x)
				{
					global::Mirror.NetworkClient.prefabs[x.assetId] = x.gameObject;
				});
			};
			global::InventorySystem.Inventory.OnLocalClientStarted += delegate
			{
				CleanupTime = PlayerPrefsSl.Get("ragdoll_cleanup", 0);
			};
		}
	}
}
