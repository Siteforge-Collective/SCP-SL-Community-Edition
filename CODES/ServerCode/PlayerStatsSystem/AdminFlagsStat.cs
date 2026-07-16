namespace PlayerStatsSystem
{
	public class AdminFlagsStat : global::PlayerStatsSystem.SyncedStatBase
	{
		public override byte SyncId => 4;

		public override global::PlayerStatsSystem.SyncedStatBase.SyncMode Mode => global::PlayerStatsSystem.SyncedStatBase.SyncMode.Public;

		public override float MinValue => 0f;

		public override float MaxValue => float.MaxValue;

		public global::PlayerStatsSystem.AdminFlags Flags
		{
			get
			{
				return (global::PlayerStatsSystem.AdminFlags)global::UnityEngine.Mathf.RoundToInt(CurValue);
			}
			set
			{
				CurValue = (float)value;
			}
		}

		public bool HasFlag(global::PlayerStatsSystem.AdminFlags flag)
		{
			return (flag & Flags) == flag;
		}

		public void InvertFlag(global::PlayerStatsSystem.AdminFlags flag)
		{
			global::PlayerStatsSystem.AdminFlags flags = Flags;
			Flags = (((flag & flags) != flag) ? (flags | flag) : (flags & ~flag));
		}

		public void SetFlag(global::PlayerStatsSystem.AdminFlags flag, bool status)
		{
			Flags = (status ? (Flags | flag) : (Flags & ~flag));
		}

		public override float ReadValue(global::Mirror.NetworkReader reader)
		{
			return (int)reader.ReadByte();
		}

		public override void WriteValue(global::Mirror.NetworkWriter writer)
		{
			writer.WriteByte((byte)Flags);
		}

		public override bool CheckDirty(float prevValue, float newValue)
		{
			return (int)prevValue != (int)newValue;
		}
	}
}
