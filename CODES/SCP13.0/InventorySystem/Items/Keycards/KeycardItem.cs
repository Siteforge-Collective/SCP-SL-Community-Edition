using System.Runtime.InteropServices;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles.Spectating;
using UnityEngine;
using System;

namespace InventorySystem.Items.Keycards
{
    public class KeycardItem : ItemBase, IItemNametag
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct UseMessage : NetworkMessage
        {
        }

        public KeycardPermissions Permissions;
        public override float Weight => 0.01f;
        public override ItemDescriptionType DescriptionType => ItemDescriptionType.Keycard;
        public string Name => ItemTranslationReader.GetName(ItemTypeId);

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += () =>
            {
                NetworkServer.ReplaceHandler<UseMessage>((conn, msg) =>
                {
                    if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub))
                    {
                        if (hub.inventory.CurInstance is KeycardItem && hub != null)
                        {
                            SpectatorNetworking.SendToSpectatorsOf(default(UseMessage), hub, false);
                        }
                    }
                }, true);
            };
        }
    }
}