using CustomPlayerEffects;
using UnityEngine;

namespace InventorySystem.Items.Usables
{
    public class UsableItemViewmodel : StandardAnimatedViemodel
    {
        private static readonly int UseAnimHash;
        private static readonly int SpeedModifierHash;

        [SerializeField]
        private AudioSource _equipSoundSource;

        private float _originalPitch = 1f;

        static UsableItemViewmodel()
        {
            UseAnimHash = Animator.StringToHash("IsUsing");
            SpeedModifierHash = Animator.StringToHash("SpeedModifier");
        }

        public override void InitAny()
        {
            base.InitAny();

            if (_equipSoundSource != null)
            {
                _originalPitch = _equipSoundSource.pitch;
            }
        }

        public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
        {
            base.InitSpectator(ply, id, wasEquipped);

            UsableItemsController.OnClientStatusReceived += HandleMessage;

            OnEquipped();

            if (wasEquipped)
            {
                if (_equipSoundSource != null)
                {
                    _equipSoundSource.Stop();
                }

                if (UsableItemsController.StartTimes.TryGetValue(id.SerialNumber, out float startTime))
                {
                    AnimatorSetBool(UseAnimHash, true);
                }
                else
                {
                    float skipTime = SkipEquipTime;
                    AnimatorForceUpdate(skipTime, true);
                }
            }
        }

        public virtual void OnUsingCancelled()
        {
            AnimatorSetBool(UseAnimHash, false);
        }

        public virtual void OnUsingStarted()
        {
            if (_equipSoundSource != null)
            {
                _equipSoundSource.Stop();
            }

            AnimatorSetBool(UseAnimHash, true);
        }

        internal override void OnEquipped()
        {
            base.OnEquipped();

            float speedMultiplier = UsableItemModifierEffectExtensions.GetSpeedMultiplier(ItemId.TypeId, Hub);

            AnimatorSetFloat(SpeedModifierHash, speedMultiplier);

            if (_equipSoundSource != null)
            {
                _equipSoundSource.pitch = speedMultiplier * _originalPitch;
            }
        }

        private void HandleMessage(StatusMessage msg)
        {
            if (msg.ItemSerial != ItemId.SerialNumber)
                return;

            bool isUsing = msg.Status == StatusMessage.StatusType.Start;

            AnimatorSetBool(UseAnimHash, isUsing);
        }

        private void OnDestroy()
        {
            UsableItemsController.OnClientStatusReceived -= HandleMessage;
        }
    }
}