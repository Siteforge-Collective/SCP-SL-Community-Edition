namespace PlayerRoles.PlayableScps.HumeShield
{
	public class DynamicHumeShieldController : global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase, global::GameObjectPools.IPoolSpawnable, global::GameObjectPools.IPoolResettable
	{
		public struct ShieldBreakMessage : global::Mirror.NetworkMessage
		{
			public ReferenceHub Target;
		}

		private global::PlayerStatsSystem.HealthStat _hp;

		private double _nextRegenTime;

		private int _blockersCount;

		private const float ShieldBreakSoundRange = 37f;

		private const global::AudioPooling.AudioMixerChannelType ShieldBreakSoundChannel = global::AudioPooling.AudioMixerChannelType.NoDucking;

		private readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldBlocker> _blockers = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldBlocker>();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _shieldBreakSound;

		public global::UnityEngine.AnimationCurve ShieldOverHealth;

		public float RegenerationRate;

		public float RegenerationCooldown;

		private bool IsBlocked
		{
			get
			{
				_blockersCount -= _blockers.RemoveWhere((global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldBlocker x) => (x is global::UnityEngine.Object obj && obj == null) || x == null || !x.HumeShieldBlocked);
				return _blockersCount > 0;
			}
		}

		public override float HsMax => ShieldOverHealth.Evaluate(_hp.NormalizedValue);

		public override float HsRegeneration
		{
			get
			{
				if (!(_nextRegenTime < global::Mirror.NetworkTime.time) || IsBlocked)
				{
					return 0f;
				}
				return RegenerationRate;
			}
		}

		public override global::UnityEngine.Color? HsWarningColor
		{
			get
			{
				if (!IsBlocked)
				{
					return null;
				}
				return new global::UnityEngine.Color(1f, 0f, 0f, 0.3f);
			}
		}

		public void AddBlocker(global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldBlocker blocker)
		{
			if (_blockers.Add(blocker))
			{
				_blockersCount++;
			}
		}

		public void ResumeRegen()
		{
			_nextRegenTime = 0.0;
		}

		public override void OnHsValueChanged(float prevValue, float newValue)
		{
			if (global::Mirror.NetworkServer.active && !(newValue > 0f) && !(prevValue <= 0f) && !(_shieldBreakSound == null))
			{
				global::Mirror.NetworkServer.SendToReady(new global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController.ShieldBreakMessage
				{
					Target = base.Owner
				});
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerStatsSystem.PlayerStats playerStats = base.Owner.playerStats;
			_hp = playerStats.GetModule<global::PlayerStatsSystem.HealthStat>();
			playerStats.OnThisPlayerDamaged += OnDamaged;
			if (global::Mirror.NetworkServer.active)
			{
				base.HsCurrent = ShieldOverHealth.Evaluate(1f);
			}
		}

		public void ResetObject()
		{
			if (base.Owner != null)
			{
				base.Owner.playerStats.OnThisPlayerDamaged -= OnDamaged;
			}
			_nextRegenTime = 0.0;
			_blockersCount = 0;
			_blockers.Clear();
		}

		private void OnDamaged(global::PlayerStatsSystem.DamageHandlerBase dhb)
		{
			if (global::Mirror.NetworkServer.active)
			{
				_nextRegenTime = global::Mirror.NetworkTime.time + (double)RegenerationCooldown;
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkClient.ReplaceHandler(delegate(global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController.ShieldBreakMessage msg)
				{
					if (!(msg.Target == null) && msg.Target.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole humeShieldedRole && humeShieldedRole.HumeShieldModule is global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController dynamicHumeShieldController && !(dynamicHumeShieldController._shieldBreakSound == null))
					{
						global::AudioPooling.AudioSourcePoolManager.PlaySound(dynamicHumeShieldController._shieldBreakSound, dynamicHumeShieldController.transform, 37f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.NoDucking);
					}
				});
			};
		}
	}
}
