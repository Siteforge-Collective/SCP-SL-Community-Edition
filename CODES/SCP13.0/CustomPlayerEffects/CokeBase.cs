using System.Collections.Generic;
using CustomRendering;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;

namespace CustomPlayerEffects
{
    public abstract class CokeBase : TickingEffectBase
    {
    }

    public abstract class CokeBase<TStack> : CokeBase, IHealablePlayerEffect, IMovementSpeedModifier, IConflictableEffect
        where TStack : ICokeStack
    {
        private const float ConflictExplosionDelay = 1.05f;

        private bool _goingToExplode;

        private float _explosionTimer;

        private SpeedLines _speedLines;

        public abstract Dictionary<PlayerMovementState, float> StateMultipliers { get; }

        public abstract float MovementSpeedMultiplier { get; }

        public bool MovementModifierActive => IsEnabled;

        public float MovementSpeedLimit => float.MaxValue;

        protected TStack[] StackMultipliers { get; private set; }

        protected TStack CurrentStack { get; private set; }

        protected bool GoingToExplode
        {
            get => _goingToExplode;
            private set
            {
                _goingToExplode = value;
                _explosionTimer = 0f;
            }
        }

        public bool IsHealable(ItemType it) => it == ItemType.SCP500;

        protected float GetMovementStateMultiplier()
        {
            if (Hub.roleManager.CurrentRole is IFpcRole fpcRole)
            {
                PlayerMovementState state = fpcRole.FpcModule.CurrentMovementState;

                Vector3 velocity = FpcExtensionMethods.GetVelocity(Hub);
                float horizontalSqr = Misc.SqrMagnitudeIgnoreY(velocity);

                if (state == PlayerMovementState.Sprinting && horizontalSqr > 0f)
                {
                    if (StateMultipliers.TryGetValue(PlayerMovementState.Sprinting, out float sprintMult))
                    {
                        return sprintMult;
                    }
                }

                if (StateMultipliers.TryGetValue(state, out float mult))
                {
                    return mult;
                }
            }

            return 1f;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            Volume volume = GetComponent<Volume>();
            if (volume != null && volume.profile != null && volume.profile.TryGet<SpeedLines>(out SpeedLines speedLines))
            {
                _speedLines = speedLines;
            }
        }

        protected override void IntensityChanged(byte prevState, byte newState)
        {
            base.IntensityChanged(prevState, newState);

            if (newState == 0)
            {
                CurrentStack = default;
            }
            else
            {
                int index = Mathf.Min(newState - 1, StackMultipliers.Length - 1);
                CurrentStack = StackMultipliers[index];
            }

            StackChanged(prevState, newState);
        }

        protected virtual void StackChanged(byte oldState, byte newState)
        {
        }

        protected override void Enabled()
        {
            base.Enabled();

            byte intensity = Intensity;
            if (intensity > 0)
            {
                int index = Mathf.Min(intensity - 1, StackMultipliers.Length - 1);
                CurrentStack = StackMultipliers[index];
            }

            if (IsLocalPlayer && _speedLines != null)
            {
                _speedLines.Intensity.value = CurrentStack != null ? CurrentStack.SpeedMultiplier - 1f : 0f;
            }
        }

        protected override void Disabled()
        {
            base.Disabled();

            if (IsLocalPlayer && _speedLines != null)
            {
                _speedLines.Intensity.value = 0f;
            }
        }

        protected override void OnEffectUpdate()
        {
            base.OnEffectUpdate();

            if (IsLocalPlayer && CurrentStack != null && _speedLines != null)
            {
                _speedLines.Intensity.value = CurrentStack.SpeedMultiplier - 1f;
            }
        }

        protected override void OnTick()
        {
            if (GoingToExplode)
            {
                _explosionTimer += Time.deltaTime;

                if (_explosionTimer >= ConflictExplosionDelay)
                {
                    ExplosionUtils.ServerExplode(Hub);
                    GoingToExplode = false;
                }
            }
        }

        public virtual bool CheckConflicts(StatusEffectBase other)
        {
            if (other is Poisoned)
            {
                GoingToExplode = true;

                if (Hub.playerEffectsController.TryGetEffect<Poisoned>(out Poisoned poisoned))
                {
                    if (!poisoned.IsEnabled)
                    {
                        poisoned.ForceIntensity(1);
                    }
                }

                other.ServerDisable();
                return true;
            }

            return false;
        }
    }
}