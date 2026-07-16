namespace PlayerStatsSystem
{
	public class HealthStat : global::PlayerStatsSystem.SyncedStatBase
	{
		public override byte SyncId => 2;

		public override global::PlayerStatsSystem.SyncedStatBase.SyncMode Mode => global::PlayerStatsSystem.SyncedStatBase.SyncMode.PrivateAndSpectators;

		public override float MinValue => 0f;

		public override float MaxValue
		{
			get
			{
				if (!(base.Hub.roleManager.CurrentRole is global::PlayerRoles.IHealthbarRole healthbarRole))
				{
					return 0f;
				}
				return healthbarRole.MaxHealth;
			}
		}

		public override float ReadValue(global::Mirror.NetworkReader reader)
		{
			return (int)global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
		}

		public override void WriteValue(global::Mirror.NetworkWriter writer)
		{
			int num = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.CeilToInt(CurValue), 0, 65535);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, (ushort)num);
		}

		public override bool CheckDirty(float prevValue, float newValue)
		{
			return global::UnityEngine.Mathf.CeilToInt(prevValue) != global::UnityEngine.Mathf.CeilToInt(newValue);
		}

		internal override void ClassChanged()
		{
			base.ClassChanged();
			if (global::Mirror.NetworkServer.active)
			{
				CurValue = MaxValue;
			}
		}

		public void ServerHeal(float healAmount)
		{
			CurValue = global::UnityEngine.Mathf.Min(CurValue + global::UnityEngine.Mathf.Abs(healAmount), MaxValue);
		}
	}
}
