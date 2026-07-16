namespace PlayerRoles.PlayableScps.HumeShield
{
	public abstract class HumeShieldModuleBase : global::UnityEngine.MonoBehaviour, global::GameObjectPools.IPoolSpawnable
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayerRoleBase _role;

		protected global::PlayerStatsSystem.HumeShieldStat HsStat { get; private set; }

		protected ReferenceHub Owner { get; private set; }

		public global::PlayerRoles.PlayerRoleBase Role => _role;

		public float HsCurrent
		{
			get
			{
				return HsStat.CurValue;
			}
			set
			{
				if (!global::Mirror.NetworkServer.active)
				{
					throw new global::System.InvalidOperationException("Hume Shield cannot be assigned by a client!");
				}
				HsStat.CurValue = value;
			}
		}

		public abstract float HsMax { get; }

		public abstract float HsRegeneration { get; }

		public abstract global::UnityEngine.Color? HsWarningColor { get; }

		public virtual void OnHsValueChanged(float prevValue, float newValue)
		{
		}

		public virtual void SpawnObject()
		{
			if (!Role.TryGetOwner(out var hub))
			{
				throw new global::System.InvalidOperationException("'" + base.name + "' Hume Shield Controller spawned without a role!");
			}
			Owner = hub;
			HsStat = hub.playerStats.GetModule<global::PlayerStatsSystem.HumeShieldStat>();
		}
	}
}
