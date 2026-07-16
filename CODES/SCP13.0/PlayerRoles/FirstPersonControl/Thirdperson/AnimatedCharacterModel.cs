using System;
using System.Diagnostics;
using AudioPooling;
using CustomPlayerEffects;
using GameObjectPools;
using PlayerRoles.Spectating;
using UnityEngine;

namespace PlayerRoles.FirstPersonControl.Thirdperson
{
    public class AnimatedCharacterModel : CharacterModel, IPoolResettable, IPoolSpawnable
    {
        private enum FootstepLoudness
        {
            Civilian = 8,
            FoundationForces = 12,
            Chaos = 30,
            Scp = 35
        }

        public static Action<AnimatedCharacterModel, float> OnFootstepPlayed;

        private static readonly int HashForward = Animator.StringToHash("Forward");

        private static readonly int HashStrafe = Animator.StringToHash("Strafe");

        private static readonly int HashSpeed = Animator.StringToHash("Speed");

        private readonly Stopwatch _lastTouchdownSw = Stopwatch.StartNew();

        private int _lastFootstep;

        private bool _forceUpdate;

        private const float SilentVelocityMultiplier = 0.7f;

        private const float SprintingLoudnessMultiplier = 2f;

        private const float MinimalFootstepSoundCooldown = 0.2f;

        private const float SpawnGroundedSuppression = 0.3f;

        [Header("Animation settings")]
        [SerializeField]
        private float _firstpersonDampTime;

        [SerializeField]
        private float _thirdpersonDampTime;

        [SerializeField]
        private AnimationCurve _walkVelocityScale;

        [Header("Footsteps")]
        [SerializeField]
        private AudioClip[] _footstepClips;

        [Range(0f, 1f)]
        [SerializeField]
        private float[] _footstepTimes;

        [SerializeField]
        private FootstepLoudness _footstepLoudness;

        public ModelSharedSettings SharedSettings;

        public AudioClip RandomFootstep => _footstepClips.RandomItem();

        public Vector3 HeadBobPosition { get; protected set; }

        public bool IsTracked
        {
            get
            {
                if (!base.Pooled)
                {
                    if (!base.OwnerHub.isLocalPlayer)
                    {
                        return base.OwnerHub.IsLocallySpectated();
                    }
                    return true;
                }
                return false;
            }
        }

        internal Animator Animator { get; private set; }

        internal AnimatorOverrideController AnimatorOverride { get; private set; }

        protected FirstPersonMovementModule FpcModule { get; private set; }

        protected PlayerRoleBase Role { get; private set; }

        protected virtual float FootstepLoudnessDistance
        {
            get
            {
                float num = (float)_footstepLoudness;
                if (FpcModule.CurrentMovementState == PlayerMovementState.Sprinting)
                {
                    num *= 2f;
                }
                return num;
            }
        }

        protected virtual bool FootstepPlayable
        {
            get
            {
                if (!FpcModule.IsGrounded || !FpcModule.Motor.MovementDetected)
                {
                    return false;
                }
                float num = FpcModule.VelocityForState(PlayerMovementState.Sneaking, applyCrouch: false);
                if (FpcModule.MaxMovementSpeed <= num)
                {
                    return false;
                }
                num *= 0.7f;
                return FpcModule.Motor.Velocity.SqrMagnitudeIgnoreY() >= num * num;
            }
        }

        public float WalkCycle
        {
            get
            {
                float normalizedTime = Animator.GetCurrentAnimatorStateInfo(WalkLayer).normalizedTime;
                if (!float.IsNaN(normalizedTime))
                {
                    return normalizedTime - (float)(int)normalizedTime;
                }
                return 0f;
            }
        }

        protected virtual int WalkLayer => 0;

        protected virtual bool LandingFootstepPlayable => true;

        public event Action<AudioSource> OnFootstepAudioSpawned;

        protected override void Awake()
        {
            base.Awake();
            Animator = SetupAnimator();
        }

        protected virtual void Update()
        {
            if (!base.Pooled)
            {
                float dampTime = (base.OwnerHub.isLocalPlayer ? _firstpersonDampTime : _thirdpersonDampTime);
                float num = Animator.GetCurrentAnimatorStateInfo(WalkLayer).normalizedTime;
                if (float.IsNaN(num))
                {
                    num = 0f;
                }
                Vector2 movementDirection;
                float normalizedVelocity;
                if (!FpcModule.IsGrounded)
                {
                    movementDirection = Vector2.zero;
                    normalizedVelocity = 0f;
                }
                else
                {
                    Vector3 vector = base.CachedTransform.InverseTransformDirection(FpcModule.Motor.Velocity);
                    Vector2 vector2 = new Vector2(vector.x, vector.z);
                    float magnitude = vector2.magnitude;
                    movementDirection = ((magnitude <= float.Epsilon) ? Vector2.zero : (vector2 / magnitude));
                    float walkSpeed = FpcModule.WalkSpeed;
                    normalizedVelocity = ((walkSpeed == 0f) ? 1f : (magnitude / walkSpeed));
                }
                UpdateHeadBob(num);
                UpdateFootsteps(num);
                UpdateAnimatorParameters(movementDirection, normalizedVelocity, dampTime);
            }
        }

        protected virtual Animator SetupAnimator()
        {
            Animator component = GetComponent<Animator>();
            AnimatorOverride = new AnimatorOverrideController(component.runtimeAnimatorController);
            component.runtimeAnimatorController = AnimatorOverride;
            return component;
        }

        protected virtual AudioSource PlayFootstepAudioClip(AudioClip clip, float dis, float vol)
        {
            return AudioSourcePoolManager.PlaySound(RandomFootstep, base.transform, dis, vol, FalloffType.Footstep);
        }

        private void UpdateHeadBob(float time)
        {
            if (IsTracked)
            {
                float strafe = Animator.GetFloat(HashStrafe);
                float forward = Animator.GetFloat(HashForward);
                Vector2 animDirection = new(strafe, forward);
                HeadBobPosition = SharedSettings.GetHeadBob(time, animDirection);
            }
        }

        private void UpdateFootsteps(float time)
        {
            time -= (float)(int)time;
            int num = _footstepTimes.Length;
            if (_lastFootstep < num)
            {
                if (!(time < _footstepTimes[_lastFootstep]))
                {
                    _lastFootstep++;
                    if (FootstepPlayable)
                    {
                        PlayFootstep();
                    }
                }
            }
            else if (num > 0 && time < _footstepTimes[0])
            {
                _lastFootstep = 0;
            }
        }

        private void OnGrounded()
        {
            if (!(base.OwnerHub.roleManager.CurrentRole.ActiveTime < 0.3f) && LandingFootstepPlayable)
            {
                PlayFootstep();
                _lastTouchdownSw.Restart();
                if (IsTracked)
                {
                    SharedSettings.PlayLandingAnimation();
                }
            }
        }

        private void PlayFootstep()
        {
            float footstepLoudnessDistance = FootstepLoudnessDistance;
            float num = 1f;
            bool flag = true;
            PlayerEffectsController playerEffectsController = base.OwnerHub.playerEffectsController;
            int num2 = playerEffectsController.AllEffects.Length;
            for (int i = 0; i < num2; i++)
            {
                if (playerEffectsController.AllEffects[i].IsEnabled && playerEffectsController.AllEffects[i] is IFootstepEffect footstepEffect)
                {
                    float num3 = footstepEffect.ProcessFootstepOverrides(footstepLoudnessDistance);
                    if (num3 >= 0f)
                    {
                        flag = false;
                    }
                    num = Mathf.Min(num, num3);
                }
            }
            if (!flag || num >= 0f)
            {
                OnFootstepPlayed?.Invoke(this, footstepLoudnessDistance);
            }
        }

        public virtual void UpdateAnimatorParameters(Vector2 movementDirection, float normalizedVelocity, float dampTime)
        {
            float value = _walkVelocityScale.Evaluate(normalizedVelocity);
            movementDirection *= normalizedVelocity;
            if (_forceUpdate)
            {
                Animator.SetFloat(HashForward, movementDirection.y);
                Animator.SetFloat(HashStrafe, movementDirection.x);
                Animator.SetFloat(HashSpeed, value);
            }
            else
            {
                Animator.SetFloat(HashForward, movementDirection.y, dampTime, Time.deltaTime);
                Animator.SetFloat(HashStrafe, movementDirection.x, dampTime, Time.deltaTime);
                Animator.SetFloat(HashSpeed, value, dampTime, Time.deltaTime);
            }
        }

        public virtual void ForceUpdate()
        {
            _forceUpdate = true;
            Update();
            _forceUpdate = false;
        }

        public override void ResetObject()
        {
            base.ResetObject();
            FirstPersonMovementModule fpcModule = FpcModule;
            fpcModule.OnGrounded = (Action)Delegate.Remove(fpcModule.OnGrounded, new Action(OnGrounded));
        }

        public override void SpawnObject()
        {
            base.SpawnObject();
            Role = base.OwnerHub.roleManager.CurrentRole;
            FpcModule = (Role as IFpcRole).FpcModule;
            FirstPersonMovementModule fpcModule = FpcModule;
            fpcModule.OnGrounded = (Action)Delegate.Combine(fpcModule.OnGrounded, new Action(OnGrounded));
            Animator.Rebind();
            HitboxIdentity[] hitboxes = Hitboxes;
            foreach (HitboxIdentity hitboxIdentity in hitboxes)
            {
                HitboxIdentity.Instances.Add(hitboxIdentity);
                hitboxIdentity.SetColliders(!base.OwnerHub.isLocalPlayer);
            }
        }

        public override void OnTreadmillInitialized()
        {
            base.OnTreadmillInitialized();
            Animator = SetupAnimator();
        }
    }
}
