using System;
using System.Collections.Generic;
using CustomPlayerEffects.Danger;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Searching;
using PlayerRoles.Spectating;
using RemoteAdmin.Interfaces;
using UnityEngine;
using Mirror;

namespace CustomPlayerEffects
{
    public class Scp1853 : TickingEffectBase, ISpectatorDataPlayerEffect, IHealablePlayerEffect, IConflictableEffect, IWeaponModifierPlayerEffect, ISearchTimeModifier, ICustomRADisplay, IUsableItemModifierEffect, IHeavyItemPenaltyImmunity, ISoundtrackMutingEffect
    {
        [Serializable]
        private struct SerializedStat
        {
            public AttachmentParam Parameter;
            public float BoostPercentage;
            public float MaxBoost;
            public bool IsAdditive;

            public SerializedStat(AttachmentParam param, float boostPercentage, float maxBoost = 0f, bool isAdditive = true)
            {
                Parameter = param;
                BoostPercentage = boostPercentage;
                MaxBoost = maxBoost;
                IsAdditive = isAdditive;
            }
        }

        public const float DangerPerIntensity = 0.25f;
        private const float MinimumDanger = 1f;
        private const float MaximumDanger = 5f;
        private const float SearchSpeed = 1.5f;
        private const float EquipAndUse = 0.2f;
        private const float ItemUsageMaxMultiplier = 0.4f;
        private const float NoMaxBoost = 0f;
        private const float StatMultiplierPerDanger = 0.5f;

        private static readonly SerializedStat[] BaseWeaponModifiers = new SerializedStat[]
        {
            new SerializedStat(AttachmentParam.AdsSpeedMultiplier, 0.2f, 0.4f),
            new SerializedStat(AttachmentParam.OverallRecoilMultiplier, 0.2f, 0f, isAdditive: false),
            new SerializedStat(AttachmentParam.AdsInaccuracyMultiplier, 0.2f, 0f, isAdditive: false),
            new SerializedStat(AttachmentParam.HipInaccuracyMultiplier, 0.2f, 0f, isAdditive: false),
            new SerializedStat(AttachmentParam.ReloadSpeedMultiplier, 0.35f),
            new SerializedStat(AttachmentParam.DrawSpeedMultiplier, 0.2f)
        };

        private static readonly ItemType[] AffectedItems = new ItemType[]
        {
            ItemType.SCP018,
            ItemType.SCP500,
            ItemType.SCP330,
            ItemType.GrenadeFlash,
            ItemType.GrenadeHE,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.Adrenaline
        };

        public DangerStackBase[] Dangers = new DangerStackBase[8]
        {
            new WarheadDanger(),
            new CardiacArrestDanger(),
            new RageTargetDanger(),
            new CorrodingDanger(),
            new PlayerDamagedDanger(),
            new ScpEncounterDanger(),
            new ZombieEncounterDanger(),
            new ArmedEnemyDanger()
        };

        [SerializeField] private AudioSource _dangerChangedSource;
        [SerializeField] private AudioSource _heartbeatSource;
        [SerializeField] private AudioClip _dangerIncreasedClip;
        [SerializeField] private AudioClip _dangerDecreasedClip;

        private readonly Dictionary<AttachmentParam, float> _processedParams = new Dictionary<AttachmentParam, float>();
        private float _searchSpeedMultiplier;

        public override EffectClassification Classification => EffectClassification.Positive;
        public override byte MaxIntensity => 20;
        public bool ParamsActive => base.IsEnabled;
        public float ItemUsageSpeedMultiplier { get; private set; }
        public string DisplayName => "SCP-1853";
        public bool CanBeDisplayed => true;
        public bool SprintingDisabled => false;
        public bool MuteSoundtrack => base.IsEnabled;

        public float CurrentDanger => (float)Intensity * 0.25f;

        public float StatMultiplier => 1f + OffsetedDanger * 0.5f;
        private int OffsetedDanger => Mathf.Max(0, Mathf.FloorToInt(CurrentDanger - 1f));

        private VignetteSmoothnessPulse _vignetteSmoothnessPulse;
        private VignettePulse _vignettePulse;
        private float _cachedTickTime;

        protected override void IntensityChanged(byte prevState, byte newState)
        {
            float statMultiplier = StatMultiplier;

            float prevDanger = Mathf.Floor(prevState * 0.25f);
            float newDanger = Mathf.Floor(newState * 0.25f);

            foreach (SerializedStat stat in BaseWeaponModifiers)
            {
                float boostAmount = stat.BoostPercentage * statMultiplier;
                _processedParams[stat.Parameter] = CalculateStat(boostAmount, stat.MaxBoost, stat.IsAdditive);
            }

            _searchSpeedMultiplier = SearchSpeed;
            ItemUsageSpeedMultiplier = CalculateStat(EquipAndUse * statMultiplier, ItemUsageMaxMultiplier);

            if (Hub.isLocalPlayer || SpectatorNetworking.IsLocallySpectated(Hub))
            {
                UpdateHeartbeat();

                if (prevDanger != newDanger && prevDanger != 0f && newDanger != 0f)
                {
                    bool increased = newDanger > prevDanger;
                    _dangerChangedSource.PlayOneShot(increased ? _dangerIncreasedClip : _dangerDecreasedClip, 1f);
                }
            }
        }

        public override void OnStopSpectating()
        {
            base.OnStopSpectating();
            UpdateHeartbeat();
        }

        protected override void OnTick()
        {
            if (!Hub.isLocalPlayer && !SpectatorNetworking.IsLocallySpectated(Hub))
                return;

            float speedMultiplier = _cachedTickTime * 0.35f;
            float pulseIntensity = OffsetedDanger * 0.015f;

            _vignetteSmoothnessPulse.PulseIntensity = pulseIntensity;
            _vignetteSmoothnessPulse.animationSpeedMultiplier = speedMultiplier;
            _vignetteSmoothnessPulse.enabled = true;

            _vignettePulse.PulseIntensity = pulseIntensity * 0.0015f;
            _vignettePulse.animationSpeedMultiplier = speedMultiplier;
            _vignettePulse.enabled = true;
        }

        protected override void Enabled()
        {
            base.Enabled();
            UpdateHeartbeat();

            if (NetworkServer.active)
            {
                foreach (DangerStackBase danger in Dangers)
                {
                    danger.Initialize(Hub);
                }
            }
        }

        protected override void Disabled()
        {
            base.Disabled();
            UpdateHeartbeat();

            if (NetworkServer.active)
            {
                foreach (DangerStackBase danger in Dangers)
                {
                    danger.Dispose();
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if (!IsEnabled || !NetworkServer.active)
                return;

            float totalDanger = 1f;
            foreach (DangerStackBase danger in Dangers)
            {
                if (danger.IsActive)
                    totalDanger += danger.DangerValue;
            }

            totalDanger = Mathf.Clamp(totalDanger, MinimumDanger, MaximumDanger);
            byte newIntensity = (byte)(totalDanger / DangerPerIntensity);

            if (newIntensity != Intensity)
                Intensity = newIntensity;
        }

        protected override void OnAwake()
        {
            _vignettePulse = GetComponent<VignettePulse>();
            _vignetteSmoothnessPulse = GetComponent<VignetteSmoothnessPulse>();
            _cachedTickTime = TimeBetweenTicks;
        }

        private void UpdateHeartbeat()
        {
            if (!Hub.isLocalPlayer && !SpectatorNetworking.IsLocallySpectated(Hub))
            {
                if (_heartbeatSource.isPlaying)
                    _heartbeatSource.Stop();
                return;
            }

            if (OffsetedDanger < 1)
            {
                if (_heartbeatSource.isPlaying)
                    _heartbeatSource.Stop();
                return;
            }

            if (!_heartbeatSource.isPlaying)
                _heartbeatSource.Play();

            _heartbeatSource.volume = OffsetedDanger * 0.2f;
            _heartbeatSource.pitch = 1f + OffsetedDanger * 0.1f;
        }

        private float CalculateStat(float boostAmount, float max = 0f, bool isAdditive = true)
        {
            if (max > 0f)
                boostAmount = Mathf.Clamp(boostAmount, 0f, max);

            return isAdditive ? 1f + boostAmount : 1f - boostAmount;
        }

        public bool IsHealable(ItemType it) => it == ItemType.SCP500;

        public bool GetSpectatorText(out string s)
        {
            s = $"SCP-1853 ({CurrentDanger} danger)";
            return true;
        }

        public bool TryGetSpeed(ItemType item, out float speed)
        {
            speed = ItemUsageSpeedMultiplier;
            return IsEnabled && System.Array.IndexOf(AffectedItems, item) != -1;
        }

        public float ProcessSearchTime(float val) => val / Mathf.Max(_searchSpeedMultiplier, 1f);

        public bool TryGetWeaponParam(AttachmentParam param, out float val) =>
            _processedParams.TryGetValue(param, out val);

        public bool CheckConflicts(StatusEffectBase other)
        {
            if (other is CokeBase && Hub.playerEffectsController.TryGetEffect<Poisoned>(out var poisoned))
            {
                if (!poisoned.IsEnabled)
                    poisoned.ForceIntensity(1);
                return true;
            }
            return false;
        }
    }
}