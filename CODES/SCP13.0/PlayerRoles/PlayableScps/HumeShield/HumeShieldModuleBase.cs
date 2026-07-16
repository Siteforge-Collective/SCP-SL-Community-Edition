using GameObjectPools;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.HumeShield
{
	public abstract class HumeShieldModuleBase : MonoBehaviour, IPoolSpawnable
	{
		[SerializeField]
		private PlayerRoleBase _role;

		protected HumeShieldStat HsStat { get; private set; }

		protected ReferenceHub Owner { get; private set; }

		public PlayerRoleBase Role => null;

		public float HsCurrent
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public abstract float HsMax { get; }

		public abstract float HsRegeneration { get; }

		public abstract Color? HsWarningColor { get; }

		public virtual void OnHsValueChanged(float prevValue, float newValue)
		{
		}

		public virtual void SpawnObject()
		{
		}
	}
}
