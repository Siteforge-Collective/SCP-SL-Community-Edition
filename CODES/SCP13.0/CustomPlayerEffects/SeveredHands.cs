using System.Collections.Generic;
using Footprinting;
using InventorySystem;
using InventorySystem.Items;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerStatsSystem;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class SeveredHands : TickingEffectBase, IInteractionBlocker
    {
        private static readonly BlockedInteraction Interactions = BlockedInteraction.All;

        private static readonly int HashSeveredHands = Animator.StringToHash("SeveredHands");

        public static readonly List<Footprint> AllSeveredHands = new List<Footprint>();

        [SerializeField]
        private AudioClip _severClip;

        [SerializeField]
        private float _severSoundDistance;

        [SerializeField]
        private GameObject _severedHandsPrefab;

        [SerializeField]
        private int _overrideLayerIndex = -1;

        [SerializeField]
        private float _tickDamage = 5f;

        public override bool AllowEnabling => true;

        public bool CanBeCleared => !IsEnabled;

        public BlockedInteraction BlockedInteractions => Interactions;

        protected override void Enabled()
        {
            base.Enabled();

            Hub.interCoordinator.AddBlocker(this);

            AllSeveredHands.Add(new Footprint(Hub));

            GameObject handsObject = Object.Instantiate(
                _severedHandsPrefab,
                Hub.transform.position,
                Hub.transform.rotation);

            if (_overrideLayerIndex >= 0 && handsObject != null)
            {
                handsObject.layer = _overrideLayerIndex;
            }

            AudioPooling.AudioSourcePoolManager.PlaySound(
                _severClip,
                Hub.transform.position,
                _severSoundDistance);

            if (Hub.TryGetComponent<BloodDrawer>(out var bloodDrawer))
            {
                bloodDrawer.PlaceUnderneath(Hub.transform.position);
            }

            ChangeHandsState(true);
        }

        protected override void Disabled()
        {
            base.Disabled();

            ChangeHandsState(false);
        }

        protected override void OnTick()
        {
            if (Hub.inventory.CurItem != null)
            {
                ushort serial = Hub.inventory.CurItem.SerialNumber;
                InventoryExtensions.ServerDropItem(Hub.inventory, serial);
            }

            UniversalDamageHandler damageHandler = new UniversalDamageHandler(
                _tickDamage,
                DeathTranslations.SeveredHands);

            Hub.playerStats.DealDamage(damageHandler);
        }

        private void ChangeHandsState(bool handsCut)
        {
            if (Hub.roleManager.CurrentRole is HumanRole humanRole)
            {
                if (humanRole.FpcModule.CharacterModelInstance is HumanCharacterModel humanModel)
                {
                    humanModel.Animator.SetBool(HashSeveredHands, handsCut);
                }
            }
        }
    }
}