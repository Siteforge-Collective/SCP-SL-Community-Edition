namespace PlayerRoles
{
	public abstract class PlayerRoleBase : global::GameObjectPools.PoolObject
	{
		private ReferenceHub _lastOwner;

		private global::PlayerRoles.RoleSpawnFlags _spawnFlags;

		private global::PlayerRoles.RoleChangeReason _spawnReason;

		private readonly global::System.Diagnostics.Stopwatch _activeTime = global::System.Diagnostics.Stopwatch.StartNew();

		public global::System.Action<global::PlayerRoles.RoleTypeId> OnRoleDisabled;

		public abstract global::PlayerRoles.RoleTypeId RoleTypeId { get; }

		public abstract global::PlayerRoles.Team Team { get; }

		public abstract global::UnityEngine.Color RoleColor { get; }

		[field: global::UnityEngine.SerializeField]
		public virtual global::UnityEngine.GameObject RoleHelpInfo { get; private set; }

		public string RoleName
		{
			get
			{
				if (!(this is global::PlayerRoles.ICustomNameRole customNameRole))
				{
					return global::PlayerRoles.RoleTranslations.GetRoleName(RoleTypeId);
				}
				return customNameRole.CustomRoleName;
			}
		}

		public float ActiveTime => (float)_activeTime.Elapsed.TotalSeconds;

		public bool IsLocalPlayer
		{
			get
			{
				if (TryGetOwner(out var hub))
				{
					return hub.isLocalPlayer;
				}
				return false;
			}
		}

		public global::PlayerRoles.RoleChangeReason ServerSpawnReason
		{
			get
			{
				if (!global::Mirror.NetworkServer.active)
				{
					global::UnityEngine.Debug.LogError("Server-only property ServerSpawnReason cannot be called on the client!");
				}
				return _spawnReason;
			}
			private set
			{
				_spawnReason = value;
			}
		}

		public global::PlayerRoles.RoleSpawnFlags ServerSpawnFlags
		{
			get
			{
				if (!global::Mirror.NetworkServer.active)
				{
					global::UnityEngine.Debug.LogError("Server-only property ServerSpawnFlags cannot be called on the client!");
				}
				return _spawnFlags;
			}
			internal set
			{
				_spawnFlags = value;
			}
		}

		internal virtual void Init(ReferenceHub hub, global::PlayerRoles.RoleChangeReason spawnReason, global::PlayerRoles.RoleSpawnFlags spawnFlags)
		{
			_lastOwner = hub;
			_spawnFlags = spawnFlags;
			_spawnReason = spawnReason;
			_activeTime.Restart();
		}

		public bool TryGetOwner(out ReferenceHub hub)
		{
			hub = _lastOwner;
			return !base.Pooled;
		}

		public virtual void DisableRole(global::PlayerRoles.RoleTypeId newRole)
		{
			try
			{
				OnRoleDisabled?.Invoke(newRole);
				if (!global::GameObjectPools.PoolManager.Singleton.TryReturnPoolObject(base.gameObject))
				{
					global::UnityEngine.Debug.LogError("Role " + RoleName + " could not be returned to pool.");
				}
			}
			catch (global::System.Exception exception)
			{
				global::UnityEngine.Debug.Log($"Disabling {RoleTypeId} role has thrown an exception while switching to {newRole}.");
				global::UnityEngine.Debug.LogException(exception);
			}
		}

		public override string ToString()
		{
			return string.Format("{0} (RoleTypeId = '{1}', Owner = '{2}', ActiveTime = '{3}')", "PlayerRoleBase", RoleTypeId, _lastOwner, ActiveTime);
		}
	}
}
