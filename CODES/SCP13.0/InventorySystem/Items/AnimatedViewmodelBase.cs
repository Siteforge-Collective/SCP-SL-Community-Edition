using InventorySystem.Items.SwayControllers;
using System;
using UnityEngine;

namespace InventorySystem.Items
{
    public abstract class AnimatedViewmodelBase : ItemViewmodelBase
    {
        [SerializeField] private Animator _animator;

        public bool DisableSharedHands;

        private const float MaxSkipEquipTime = 7.5f;

        public static event Action OnSwayUpdated;

        public Avatar AnimatorAvatar => _animator.avatar;
        public Animator Animator => _animator;

        public RuntimeAnimatorController AnimatorRuntimeController => _animator.runtimeAnimatorController;

        public Transform AnimatorTransform => _animator.transform;

        public bool IsFastForwarding { get; private set; }

        public abstract IItemSwayController SwayController { get; }

        public float SkipEquipTime => Mathf.Min(7.5f, base.Hub.inventory.LastItemSwitch);

        public virtual void LateUpdate()
        {
            SwayController?.UpdateSway();
            AnimatedViewmodelBase.OnSwayUpdated?.Invoke();
        }

        public override void InitAny()
        {
            base.InitAny();

            IsFastForwarding = true;

            float deltaTime = Time.deltaTime;
            _animator.Update(deltaTime);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.Update(deltaTime);
                }
            }

            IsFastForwarding = false;
        }

        public AnimatorStateInfo AnimatorStateInfo(int layer)
        {
            return _animator.GetCurrentAnimatorStateInfo(layer);
        }

        public void AnimatorForceUpdate()
        {
            IsFastForwarding = true;

            float deltaTime = Time.deltaTime;
            _animator.Update(deltaTime);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.Update(deltaTime);
                }
            }

            IsFastForwarding = false;
        }

        public void AnimatorForceUpdate(float deltaTime, bool fastMode = true)
        {
            if (!fastMode)
            {
                while (deltaTime > 0f)
                {
                    AnimatorForceUpdate(0.07f, true);
                    deltaTime -= 0.07f;
                }
                return;
            }

            IsFastForwarding = true;
            _animator.Update(deltaTime);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.Update(deltaTime);
                }
            }

            IsFastForwarding = false;
        }

        public void AnimatorSetBool(int hash, bool val)
        {
            _animator.SetBool(hash, val);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.SetBool(hash, val);
                }
            }
        }

        public void AnimatorSetFloat(int hash, float val)
        {
            _animator.SetFloat(hash, val);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.SetFloat(hash, val);
                }
            }
        }

        public void AnimatorSetInt(int hash, int val)
        {
            _animator.SetInteger(hash, val);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.SetInteger(hash, val);
                }
            }
        }

        public void AnimatorSetTrigger(int hash)
        {
            _animator.SetTrigger(hash);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.SetTrigger(hash);
                }
            }
        }

        public void AnimatorSetLayerWeight(int layer, float val)
        {
            _animator.SetLayerWeight(layer, val);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.SetLayerWeight(layer, val);
                }
            }
        }

        public void AnimatorPlay(int hash, int layer, float time)
        {
            _animator.Play(hash, layer, time);

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.Play(hash, layer, time);
                }
            }
        }

        public void AnimatorRebind()
        {
            _animator.Rebind();

            if (!DisableSharedHands)
            {
                var sharedHands = SharedHandsController.Singleton;
                if (sharedHands != null && sharedHands.Hands != null)
                {
                    sharedHands.Hands.Rebind();
                }
            }
        }
    }
}