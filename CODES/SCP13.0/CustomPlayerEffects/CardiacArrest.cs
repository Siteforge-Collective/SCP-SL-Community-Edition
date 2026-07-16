using AudioPooling;
using Footprinting;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Spectating;
using UnityEngine;
using Mirror;
using PlayerStatsSystem;

namespace CustomPlayerEffects
{
	public class CardiacArrest : ParentEffectBase<SubEffectBase>, IHealablePlayerEffect, IStaminaModifier
	{
		private const float SprintStaminaUsage = 3f;

		private const float DamagePerTick = 8f;

		private Footprint _attacker;

		[SerializeField]
		private AudioClip _dyingSoundEffect;

		[Tooltip("Used to track intervals/timers/etc without every effect needing to redefine a unique float.")]
		public float TimeBetweenTicks;

		private float _timeTillTick;

		private AudioSource _dyingSource;

		public bool StaminaModifierActive => false;

		public float StaminaUsageMultiplier => 0f;

		public float StaminaRegenMultiplier => 0f;

		public bool SprintingDisabled => false;

        public override bool AllowEnabling => !SpawnProtected.CheckPlayer(base.Hub);

        protected override void Enabled()
        {
            base.Enabled();
            if (base.Hub.isLocalPlayer || base.Hub.IsLocallySpectated())
            {
                _dyingSource = AudioSourcePoolManager.PlaySound(_dyingSoundEffect, base.Hub.transform.position, 5f, 1f, FalloffType.Exponential, AudioMixerChannelType.DefaultSfx, 0f, reserved: true);
            }
            if (NetworkServer.active)
            {
                _timeTillTick = 0f;
            }
        }

        protected override void Disabled()
        {
            base.Disabled();
            _attacker = default(Footprint);
        }

        public void SetAttacker(ReferenceHub ply)
        {
            _attacker = new Footprint(ply);
        }

        public bool IsHealable(ItemType it)
		{
			return it == ItemType.Adrenaline || it == ItemType.SCP500;
		}

        protected override void OnEffectUpdate()
        {
            if (NetworkServer.active)
            {
                ServerUpdate();
            }
            UpdateSubEffects();
        }

        private void ServerUpdate()
        {
            _timeTillTick -= Time.deltaTime;
            if (!(_timeTillTick > 0f))
            {
                _timeTillTick += TimeBetweenTicks;
                base.Hub.playerStats.DealDamage(new Scp049DamageHandler(_attacker, 8f, Scp049DamageHandler.AttackType.CardiacArrest));
            }
        }

        private void OnSpectate()
        {
            if (!(_dyingSource == null) && !(base.Hub == null))
            {
                if (SpectatorTargetTracker.CurrentTarget == null || SpectatorTargetTracker.CurrentTarget.TargetHub == null)
                {
                    _dyingSource.mute = true;
                }
                else
                {
                    _dyingSource.mute = SpectatorTargetTracker.CurrentTarget.TargetHub != base.Hub;
                }
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            SpectatorTargetTracker.OnTargetChanged += OnSpectate;
        }

        private void OnDestroy()
        {
            SpectatorTargetTracker.OnTargetChanged -= OnSpectate;
        }
    }
}
