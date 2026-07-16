namespace PlayerStatsSystem
{
	public class StaminaStat : global::PlayerStatsSystem.SyncedStatBase
	{
		private const global::PlayerStatsSystem.SyncedStatBase.SyncMode DefaultSyncMode = global::PlayerStatsSystem.SyncedStatBase.SyncMode.PrivateAndSpectators;

		private global::PlayerStatsSystem.SyncedStatBase.SyncMode _syncMode = global::PlayerStatsSystem.SyncedStatBase.SyncMode.PrivateAndSpectators;

		private global::PlayerRoles.RoleTypeId _overrideRole = global::PlayerRoles.RoleTypeId.None;

		public override byte SyncId => 3;

		public override global::PlayerStatsSystem.SyncedStatBase.SyncMode Mode => _syncMode;

		public override float MinValue => 0f;

		public override float MaxValue => 1f;

		public void ModifyAmount(float f)
		{
			CurValue = global::UnityEngine.Mathf.Clamp01(CurValue + f);
		}

		public void ChangeSyncMode(global::PlayerStatsSystem.SyncedStatBase.SyncMode newMode)
		{
			_syncMode = newMode;
			_overrideRole = global::PlayerRoles.PlayerRolesUtils.GetRoleId(base.Hub);
		}

		private byte ToByte(float val)
		{
			return (byte)global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.Clamp01(val) * 255f);
		}

		public override float ReadValue(global::Mirror.NetworkReader reader)
		{
			return (float)(int)reader.ReadByte() / 255f;
		}

		public override void WriteValue(global::Mirror.NetworkWriter writer)
		{
			writer.WriteByte(ToByte(CurValue));
		}

		public override bool CheckDirty(float prevValue, float newValue)
		{
			return ToByte(prevValue) != ToByte(newValue);
		}

		internal override void ClassChanged()
		{
			CurValue = 1f;
			if (_overrideRole != global::PlayerRoles.RoleTypeId.None && global::PlayerRoles.PlayerRolesUtils.GetRoleId(base.Hub) != _overrideRole)
			{
				_syncMode = global::PlayerStatsSystem.SyncedStatBase.SyncMode.PrivateAndSpectators;
				_overrideRole = global::PlayerRoles.RoleTypeId.None;
			}
			base.ClassChanged();
		}
	}
}
