namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939DamagedEffect : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		private bool _eventAssigned;

		private global::PlayerStatsSystem.HealthStat _hpStat;

		private global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController _hume;

		private readonly global::System.Diagnostics.Stopwatch _lastTriggered = new global::System.Diagnostics.Stopwatch();

		private float _totalDamageReceived;

		private const float AbsoluteCooldown = 3f;

		private const float HighDamageCooldown = 10f;

		private const float HighDamageThreshold = 90f;

		private const float HighDamageDecay = 80f;

		public override void SpawnObject()
		{
			base.SpawnObject();
			if (global::Mirror.NetworkServer.active)
			{
				_lastTriggered.Restart();
				_hpStat = base.Owner.playerStats.GetModule<global::PlayerStatsSystem.HealthStat>();
				_hume = base.ScpRole.HumeShieldModule as global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController;
				_eventAssigned = true;
				base.Owner.playerStats.OnThisPlayerDamaged += OnDamaged;
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			if (_eventAssigned)
			{
				_eventAssigned = false;
				base.Owner.playerStats.OnThisPlayerDamaged -= OnDamaged;
			}
		}

		private void Update()
		{
			if (!(_totalDamageReceived <= 0f))
			{
				_totalDamageReceived = global::UnityEngine.Mathf.Clamp(_totalDamageReceived - global::UnityEngine.Time.deltaTime * 80f, 0f, 90f);
			}
		}

		private void OnDamaged(global::PlayerStatsSystem.DamageHandlerBase dhb)
		{
			if (dhb is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler && !(_hpStat.CurValue <= 0f))
			{
				_totalDamageReceived += attackerDamageHandler.AbsorbedHumeDamage + attackerDamageHandler.DealtHealthDamage;
				if (!(_lastTriggered.Elapsed.TotalSeconds < 3.0) && CheckDamagedConditions(attackerDamageHandler))
				{
					ServerSendRpc(toAll: true);
					_lastTriggered.Restart();
				}
			}
		}

		private bool CheckDamagedConditions(global::PlayerStatsSystem.AttackerDamageHandler adh)
		{
			float time = _hpStat.CurValue + adh.DealtHealthDamage;
			float a = _hume.HsCurrent + adh.AbsorbedHumeDamage;
			float b = _hume.ShieldOverHealth.Evaluate(time);
			if (global::UnityEngine.Mathf.Approximately(a, b))
			{
				return true;
			}
			if (adh.AbsorbedHumeDamage > 0f && _hume.HsCurrent == 0f)
			{
				return true;
			}
			if (_lastTriggered.Elapsed.TotalSeconds > 10.0 && _totalDamageReceived >= 90f)
			{
				return true;
			}
			return false;
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)global::UnityEngine.Random.Range(0, 255));
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (base.ScpRole.FpcModule.CharacterModelInstance is global::PlayerRoles.PlayableScps.Scp939.Scp939Model scp939Model)
			{
				scp939Model.PlayDamagedEffect(reader.ReadByte());
			}
		}
	}
}
