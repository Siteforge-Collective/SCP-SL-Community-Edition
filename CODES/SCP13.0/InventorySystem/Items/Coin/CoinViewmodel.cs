using UnityEngine;
using Mirror;

namespace InventorySystem.Items.Coin
{
    public class CoinViewmodel : StandardAnimatedViemodel
    {
        private static readonly int TriggerHash = Animator.StringToHash("Flip");
        private static readonly int TailsHash = Animator.StringToHash("IsTails");

        private const float MessageVitality = 3.9f;

        public override void InitAny()
        {
            base.InitAny();
            Coin.OnFlipped += ProcessCoinflip;
        }

        private void OnDestroy()
        {
            Coin.OnFlipped -= ProcessCoinflip;
        }

        public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
        {
            base.InitSpectator(ply, id, wasEquipped);

            if (wasEquipped)
            {
                if (TryGetComponent(out AudioSource component))
                {
                    component.Stop();
                }

                if (TryGetMessage(id.SerialNumber, out bool isTails))
                {
                    base.AnimatorSetBool(TailsHash, isTails);
                    base.AnimatorSetTrigger(TriggerHash);
                    base.AnimatorForceUpdate(base.SkipEquipTime, false);
                }
            }
        }

        private bool TryGetMessage(ushort serial, out bool isTails)
        {
            isTails = false;
            if (Coin.FlipTimes.TryGetValue(serial, out double time))
            {
                isTails = time < 0;
                return NetworkTime.time - Mathf.Abs((float)time) < MessageVitality;
            }
            return false;
        }

        private void ProcessCoinflip(ushort serial, bool isTails)
        {
            if (serial == base.ItemId.SerialNumber)
            {
                base.AnimatorSetBool(TailsHash, isTails);
                base.AnimatorSetTrigger(TriggerHash);
            }
        }
    }
}