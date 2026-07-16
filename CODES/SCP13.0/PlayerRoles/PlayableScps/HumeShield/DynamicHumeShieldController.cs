using System.Collections.Generic;
using AudioPooling;
using GameObjectPools;
using Mirror;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.HumeShield
{
	public class DynamicHumeShieldController : HumeShieldModuleBase, IPoolSpawnable, IPoolResettable
	{
		public struct ShieldBreakMessage : NetworkMessage
		{
			public ReferenceHub Target;
		}

		private HealthStat _hp;

		private double _nextRegenTime;

		private int _blockersCount;

		private const float ShieldBreakSoundRange = 37f;

		private const AudioMixerChannelType ShieldBreakSoundChannel = AudioMixerChannelType.NoDucking;

		private readonly HashSet<IHumeShieldBlocker> _blockers;

		[SerializeField]
		private AudioClip _shieldBreakSound;

		public AnimationCurve ShieldOverHealth;

		public float RegenerationRate;

		public float RegenerationCooldown;

		private bool IsBlocked => false;

		public virtual AudioClip ShieldBreakSound => null;

		public override float HsMax => 0f;

		public override float HsRegeneration => 0f;

		public override Color? HsWarningColor => null;

		public void AddBlocker(IHumeShieldBlocker blocker)
		{
		}

		public void ResumeRegen()
		{
		}

		public override void OnHsValueChanged(float prevValue, float newValue)
		{
		}

		public override void SpawnObject()
		{
		}

		public void ResetObject()
		{
		}

		private void OnDamaged(DamageHandlerBase dhb)
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ProcessBreakMessage(ShieldBreakMessage msg)
		{
		}
	}
}
