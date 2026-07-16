using System.Collections.Generic;
using System.Linq;
using AudioPooling;
using CustomPlayerEffects;
using Footprinting;
using Interactables.Verification;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Usables.Scp330;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Events;
using UnityEngine;

namespace Interactables.Interobjects
{
    public class Scp330Interobject : NetworkBehaviour, IServerInteractable, IInteractable
    {
        private readonly List<Footprint> _takenCandies = new List<Footprint>();

        [SerializeField]
        private AudioClip _takeSound;

        private const float TakeCooldown = 0.1f;

        private const int MaxAmountPerLife = 2;

        public IVerificationRule VerificationRule => StandardDistanceVerification.Default;

        private float _cooldown;

        public void ServerInteract(ReferenceHub ply, byte colliderId)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void Interactables.Interobjects.Scp330Interobject::ServerInteract(ReferenceHub,System.Byte)' called when server was not active");
                return;
            }

            if (_cooldown > 0f || !PlayerRolesUtils.IsHuman(ply))
                return;

            Footprint footprint = new Footprint(ply);
            int taken = _takenCandies.Count(f => f.SameLife(footprint));

            Player player = Player.Get(ply);

            PlayerInteractScp330Event ev = new PlayerInteractScp330Event(ply, taken)
            {
                PlaySound = true,
                AllowPunishment = true
            };

            if (!EventManager.ExecuteEvent(ev))
                return;

            bool success = taken < MaxAmountPerLife;

            if (!success)
            {
                if (ev.AllowPunishment)
                {
                    if (taken > MaxAmountPerLife)
                    {
                        if (ply.playerEffectsController.TryGetEffect<SeveredHands>(out var effect))
                            effect.ForceIntensity(1);
                    }
                    else
                    {
                        ply.playerStats.DealDamage(new UniversalDamageHandler(999f, DeathTranslations.SeveredHands));
                    }
                }
                return;
            }

            Scp330Bag bag = ply.inventory.ServerAddItem(ItemType.SCP330) as Scp330Bag;
            if (bag != null)
            {
                bag.ServerRefreshBag();
                bag.Candies.Add(CandyKindIDExtensions.RandomExcluding(CandyKindID.None));
            }

            _takenCandies.Add(footprint);

            _cooldown = TakeCooldown;

            if (ev.PlaySound)
                RpcMakeSound();
        }

        public bool AllowInteracting(ReferenceHub ply, byte colliderId)
        {
            return true;
        }

        [ClientRpc]
        private void RpcMakeSound()
        {
            AudioPooling.AudioSourcePoolManager.PlaySound(_takeSound, transform.position, 10f);
        }

        private void Update()
        {
            if (_cooldown > 0f)
                _cooldown -= Time.deltaTime;
        }
    }
}