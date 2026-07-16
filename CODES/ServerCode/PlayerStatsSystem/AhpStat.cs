namespace PlayerStatsSystem
{
	public class AhpStat : global::PlayerStatsSystem.SyncedStatBase
	{
		public class AhpProcess
		{
			public float CurrentAmount;

			public float Limit;

			public float DecayRate;

			public float Efficacy;

			public float SustainTime;

			public readonly bool Persistant;

			public readonly int KillCode;

			private static int _killCodeAI;

			public AhpProcess(float startAmount, float limit, float decay, float efficacy, float sustain, bool persistant)
			{
				_killCodeAI++;
				CurrentAmount = startAmount;
				Limit = limit;
				DecayRate = decay;
				Efficacy = efficacy;
				SustainTime = sustain;
				Persistant = persistant;
				KillCode = _killCodeAI;
			}
		}

		public const float DefaultMax = 75f;

		public const float DefaultEfficacy = 0.7f;

		public const float DefaultDecay = 1.2f;

		private readonly global::System.Collections.Generic.List<global::PlayerStatsSystem.AhpStat.AhpProcess> _activeProcesses = new global::System.Collections.Generic.List<global::PlayerStatsSystem.AhpStat.AhpProcess>();

		private float _maxSoFar;

		public override byte SyncId => 1;

		public override global::PlayerStatsSystem.SyncedStatBase.SyncMode Mode => global::PlayerStatsSystem.SyncedStatBase.SyncMode.PrivateAndSpectators;

		public override float MinValue => 0f;

		public override float MaxValue => _maxSoFar;

		internal override void ClassChanged()
		{
			_maxSoFar = 75f;
			if (global::Mirror.NetworkServer.active)
			{
				_activeProcesses.Clear();
			}
		}

		internal override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active)
			{
				ServerUpdateProcesses();
			}
		}

		protected override void OnValueChanged(float prevValue, float newValue)
		{
			_maxSoFar = global::UnityEngine.Mathf.Max(_maxSoFar, newValue);
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

		public global::PlayerStatsSystem.AhpStat.AhpProcess ServerAddProcess(float amount, float limit, float decay, float efficacy, float sustain, bool persistant)
		{
			float num = 0f;
			float num2 = limit;
			foreach (global::PlayerStatsSystem.AhpStat.AhpProcess activeProcess in _activeProcesses)
			{
				num += activeProcess.CurrentAmount;
				num2 = global::UnityEngine.Mathf.Min(num2, activeProcess.Limit);
			}
			float num3 = num + amount - num2;
			if (num3 > 0f)
			{
				amount = global::UnityEngine.Mathf.Max(0f, amount - num3);
			}
			global::PlayerStatsSystem.AhpStat.AhpProcess ahpProcess = new global::PlayerStatsSystem.AhpStat.AhpProcess(amount, limit, decay, efficacy, sustain, persistant);
			for (int i = 0; i < _activeProcesses.Count; i++)
			{
				if (!(efficacy < _activeProcesses[i].Efficacy))
				{
					_activeProcesses.Insert(i, ahpProcess);
					return ahpProcess;
				}
			}
			_activeProcesses.Add(ahpProcess);
			return ahpProcess;
		}

		public global::PlayerStatsSystem.AhpStat.AhpProcess ServerAddProcess(float amount)
		{
			return ServerAddProcess(amount, 75f, 1.2f, 0.7f, 0f, persistant: false);
		}

		public bool ServerTryGetProcess(int killcode, out global::PlayerStatsSystem.AhpStat.AhpProcess process)
		{
			foreach (global::PlayerStatsSystem.AhpStat.AhpProcess activeProcess in _activeProcesses)
			{
				if (activeProcess.KillCode == killcode)
				{
					process = activeProcess;
					return true;
				}
			}
			process = null;
			return false;
		}

		public bool ServerKillProcess(int killcode)
		{
			if (ServerTryGetProcess(killcode, out var process))
			{
				return _activeProcesses.Remove(process);
			}
			return false;
		}

		public float ServerProcessDamage(float damage)
		{
			if (damage <= 0f)
			{
				return damage;
			}
			foreach (global::PlayerStatsSystem.AhpStat.AhpProcess activeProcess in _activeProcesses)
			{
				float num = damage * activeProcess.Efficacy;
				if (num >= activeProcess.CurrentAmount)
				{
					damage -= activeProcess.CurrentAmount;
					activeProcess.CurrentAmount = 0f;
					continue;
				}
				activeProcess.CurrentAmount -= num;
				return damage - num;
			}
			return damage;
		}

		private void ServerUpdateProcesses()
		{
			float num = 0f;
			global::System.Collections.Generic.List<int> list = global::NorthwoodLib.Pools.ListPool<int>.Shared.Rent();
			for (int i = 0; i < _activeProcesses.Count; i++)
			{
				global::PlayerStatsSystem.AhpStat.AhpProcess ahpProcess = _activeProcesses[i];
				num += ahpProcess.CurrentAmount;
				if (ahpProcess.SustainTime > 0f)
				{
					ahpProcess.SustainTime -= global::UnityEngine.Time.deltaTime;
					continue;
				}
				ahpProcess.CurrentAmount = global::UnityEngine.Mathf.Clamp(ahpProcess.CurrentAmount - ahpProcess.DecayRate * global::UnityEngine.Time.deltaTime, 0f, ahpProcess.Limit);
				if (ahpProcess.CurrentAmount == 0f && !ahpProcess.Persistant)
				{
					list.Add(i - list.Count);
				}
			}
			foreach (int item in list)
			{
				_activeProcesses.RemoveAt(item);
			}
			global::NorthwoodLib.Pools.ListPool<int>.Shared.Return(list);
			CurValue = global::UnityEngine.Mathf.Max(MinValue, num);
		}
	}
}
