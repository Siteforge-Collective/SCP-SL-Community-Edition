namespace PlayerStatsSystem
{
	public abstract class SyncedStatBase : global::PlayerStatsSystem.StatBase
	{
		public enum SyncMode
		{
			Private = 0,
			PrivateAndSpectators = 1,
			Public = 2
		}

		private float _lastValue;

		private float _lastSent;

		private bool _valueDirty;

		private static readonly global::System.Collections.Generic.Dictionary<uint, global::System.Collections.Generic.Dictionary<byte, global::PlayerStatsSystem.SyncedStatBase>> AllSyncedStats = new global::System.Collections.Generic.Dictionary<uint, global::System.Collections.Generic.Dictionary<byte, global::PlayerStatsSystem.SyncedStatBase>>();

		public override float CurValue
		{
			get
			{
				return _lastValue;
			}
			set
			{
				float lastValue = _lastValue;
				_lastValue = value;
				if (CheckDirty(_lastSent, value))
				{
					_valueDirty = true;
				}
				if (lastValue != value)
				{
					OnValueChanged(lastValue, value);
				}
			}
		}

		public abstract byte SyncId { get; }

		public abstract global::PlayerStatsSystem.SyncedStatBase.SyncMode Mode { get; }

		public abstract float ReadValue(global::Mirror.NetworkReader reader);

		public abstract void WriteValue(global::Mirror.NetworkWriter writer);

		public abstract bool CheckDirty(float prevValue, float newValue);

		protected virtual void OnValueChanged(float prevValue, float newValue)
		{
		}

		public static global::PlayerStatsSystem.SyncedStatBase GetStatOfUser(uint netId, byte syncId)
		{
			if (!AllSyncedStats.TryGetValue(netId, out var value))
			{
				if (!ReferenceHub.TryGetHubNetID(netId, out var hub))
				{
					throw new global::System.InvalidOperationException($"Cannot generate stats for non-existing user of NetId={netId}");
				}
				value = new global::System.Collections.Generic.Dictionary<byte, global::PlayerStatsSystem.SyncedStatBase>();
				global::PlayerStatsSystem.StatBase[] statModules = hub.playerStats.StatModules;
				for (int i = 0; i < statModules.Length; i++)
				{
					if (statModules[i] is global::PlayerStatsSystem.SyncedStatBase syncedStatBase)
					{
						value.Add(syncedStatBase.SyncId, syncedStatBase);
					}
				}
				AllSyncedStats[netId] = value;
			}
			if (!value.TryGetValue(syncId, out var value2))
			{
				throw new global::System.InvalidOperationException($"Stat of SyncId={syncId} does not exist.");
			}
			return value2;
		}

		internal override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active && _valueDirty)
			{
				global::Utils.Networking.NetworkUtils.SendToHubsConditionally(new global::PlayerStatsSystem.SyncedStatMessages.StatMessage
				{
					Stat = this,
					SyncedValue = CurValue
				}, CanReceive);
				_lastSent = CurValue;
				_valueDirty = false;
			}
		}

		internal override void ClassChanged()
		{
			base.ClassChanged();
			if (global::Mirror.NetworkServer.active)
			{
				_valueDirty = true;
			}
		}

		private bool CanReceive(ReferenceHub hub)
		{
			if (hub.isLocalPlayer)
			{
				return false;
			}
			switch (Mode)
			{
			case global::PlayerStatsSystem.SyncedStatBase.SyncMode.Private:
				return hub == base.Hub;
			case global::PlayerStatsSystem.SyncedStatBase.SyncMode.PrivateAndSpectators:
				if (global::PlayerRoles.PlayerRolesUtils.IsAlive(hub))
				{
					return hub == base.Hub;
				}
				return true;
			case global::PlayerStatsSystem.SyncedStatBase.SyncMode.Public:
				return true;
			default:
				return false;
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void InitOnLoad()
		{
			ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, (global::System.Action<ReferenceHub>)delegate(ReferenceHub x)
			{
				AllSyncedStats.Remove(x.netId);
			});
		}
	}
}
