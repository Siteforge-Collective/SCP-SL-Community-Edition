namespace PlayerRoles.PlayableScps.Subroutines
{
	public class AbilityCooldown
	{
		public double InitialTime { get; set; }

		public double NextUse { get; set; }

		public virtual bool IsReady => global::Mirror.NetworkTime.time >= NextUse;

		public float Remaining
		{
			get
			{
				return global::UnityEngine.Mathf.Max(0f, (float)(NextUse - global::Mirror.NetworkTime.time));
			}
			set
			{
				NextUse = global::Mirror.NetworkTime.time + (double)value;
			}
		}

		public float Readiness => global::UnityEngine.Mathf.Clamp01((float)((global::Mirror.NetworkTime.time - InitialTime) / (NextUse - InitialTime)));

		public void WriteCooldown(global::Mirror.NetworkWriter writer)
		{
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, NextUse);
		}

		public void ReadCooldown(global::Mirror.NetworkReader reader)
		{
			InitialTime = global::Mirror.NetworkTime.time;
			NextUse = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		}

		public void Clear()
		{
			InitialTime = 0.0;
			NextUse = 1.0;
		}

		public virtual void Trigger(float cooldown)
		{
			InitialTime = global::Mirror.NetworkTime.time;
			NextUse = InitialTime + (double)cooldown;
		}
	}
}
