using System;
using System.Collections.Generic;
using System.Diagnostics;
using AudioPooling;
using InventorySystem.Items.Autosync;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;
using PluginAPI.Events;

namespace InventorySystem.Items.Coin
{
    public class Coin : AutosyncItem, IItemDescription, IItemNametag
    {
        [SerializeField] private AudioClip _flipSound;

        private readonly Stopwatch _lastUseSw = Stopwatch.StartNew();
        private const float RateLimit = 0.6f;

        public static readonly Dictionary<ushort, double> FlipTimes = new Dictionary<ushort, double>();
        private static readonly int IdleHash = Animator.StringToHash("Idle");

        private static readonly ActionName[] ActivationKeys = { ActionName.InspectItem, ActionName.Zoom, ActionName.Noclip };

        private KeyCode[] _activationKeys;

        public override float Weight
        {
            get
            {
                return 0.0025f;
            }
        }

        public string Description => InventorySystem.Items.ItemTranslationReader.GetDescription(ItemTypeId);
        public string Name => InventorySystem.Items.ItemTranslationReader.GetName(ItemTypeId);

        public static event Action<ushort, bool> OnFlipped;

        public override void OnAdded(ItemPickupBase pickup)
        {
            base.OnAdded(pickup);
            if (IsLocalPlayer)
            {
                _activationKeys = new KeyCode[ActivationKeys.Length];
                for (int i = 0; i < ActivationKeys.Length; i++)
                {
                    if (NewInput.DefaultKeybinds.TryGetValue(ActivationKeys[i], out KeyCode key))
                    {
                        _activationKeys[i] = key;
                    }
                }
            }
        }

        public override void EquipUpdate()
        {
            if (this._lastUseSw.Elapsed.TotalSeconds < RateLimit)
                return;

            AnimatedViewmodelBase animatedViewmodelBase = this.ViewModel as AnimatedViewmodelBase;
            if (animatedViewmodelBase != null && animatedViewmodelBase.AnimatorStateInfo(0).tagHash != Coin.IdleHash)
                return;

            if (base.Owner.HasBlock(BlockedInteraction.ItemPrimaryAction))
                return;

            if (_activationKeys != null)
            {
                foreach (KeyCode key in _activationKeys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        this._lastUseSw.Restart();
                        ClientSendCmd();
                        return;
                    }
                }
            }
        }

        public override void ServerProcessCmd(NetworkReader reader)
        {
            base.ServerProcessCmd(reader);
            if ((!base.Owner.isLocalPlayer && _lastUseSw.Elapsed.TotalSeconds < 0.6000000238418579) || base.Owner.HasBlock(BlockedInteraction.ItemPrimaryAction))
            {
                return;
            }
            bool flag;
            switch (EventManager.ExecuteEvent<PlayerPreCoinFlipCancellationData>(new PlayerPreCoinFlipEvent(base.Owner)).Cancellation)
            {
                case PlayerPreCoinFlipCancellationData.CoinFlipCancellation.Heads:
                    flag = false;
                    break;
                case PlayerPreCoinFlipCancellationData.CoinFlipCancellation.Tails:
                    flag = true;
                    break;
                case PlayerPreCoinFlipCancellationData.CoinFlipCancellation.PreventFlip:
                    return;
                default:
                    flag = UnityEngine.Random.value >= 0.5f;
                    break;
            }
            if (!EventManager.ExecuteEvent(new PlayerCoinFlipEvent(base.Owner, flag)))
            {
                return;
            }
            _lastUseSw.Restart();

            using (new AutosyncRpc(this, toAll: true, out NetworkWriter writer))
            {
                writer.WriteBool(flag);
            }
        }

        public override void ClientProcessRpcTemplate(NetworkReader reader, ushort serial)
        {
            base.ClientProcessRpcTemplate(reader, serial);
            bool flag = reader.ReadBool();

            OnFlipped?.Invoke(serial, flag);

            FlipTimes[serial] = NetworkTime.time * (double)(flag ? -1 : 1);

            if (!InventoryExtensions.TryGetHubHoldingSerial(serial, out ReferenceHub referenceHub))
                return;

            if (referenceHub.isLocalPlayer)
            {
                AudioSourcePoolManager.PlaySound(_flipSound, Vector3.one, 5.5f, 1f, FalloffType.Exponential, AudioMixerChannelType.DefaultSfx, 1f);
                return;
            }
            AudioSourcePoolManager.PlaySound(_flipSound, referenceHub.transform, 5.5f, 1f, FalloffType.Exponential, AudioMixerChannelType.DefaultSfx, 1f);
        }

        internal override void OnTemplateReloaded(bool wasEverLoaded)
        {
            base.OnTemplateReloaded(wasEverLoaded);
            if (!wasEverLoaded)
            {
                FlipTimes.Clear();
            }
        }
    }
}