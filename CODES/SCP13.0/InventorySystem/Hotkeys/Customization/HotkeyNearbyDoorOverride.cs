using System.Collections.Generic;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Keycards;
using MapGeneration;
using UnityEngine;

namespace InventorySystem.Hotkeys.Customization
{
    public class HotkeyNearbyDoorOverride : HotkeyOverrideBase
    {
        private static readonly List<DoorVariant> NearbyDoors = new List<DoorVariant>();

        private static bool _anyNearbyDoors;

        private static Vector3Int? _prevCoords;

        public override HotkeyOverrideOption OptionType => HotkeyOverrideOption.PrioritizeNearbyDoors; 

        public override HotkeysTranslation OptionName => HotkeysTranslation.OverrideNearbyDoorHint;

        public override HotkeysTranslation? Description => HotkeysTranslation.OverrideNearbyDoorHint;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Inventory.OnLocalClientStarted += () => _prevCoords = null;
            HotkeyStorage.OnHotkeysSaved += () => _prevCoords = null;
        }

        private static void UpdateNearbyDoors(Vector3 position)
        {
            NearbyDoors.Clear();
            _anyNearbyDoors = false;

            RoomIdentifier room = MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(position);

            if (room == null) return;

            if (DoorVariant.DoorsByRoom.TryGetValue(room, out var doorsInRoom))
            {
                foreach (var door in doorsInRoom)
                {
                    if (door != null)
                    {
                        NearbyDoors.Add(door);
                    }
                }
            }

            _anyNearbyDoors = NearbyDoors.Count > 0;
        }

        public override bool CheckAvailability(List<PoolElementData> elements)
        {
            ItemType[] keycardItems = HotkeyItemGroupDefinitions.GetItems(HotkeyItemGroup.AnyKeycard);
            return HotkeyUtils.CountMatches(elements, keycardItems) > 0;
        }

        public override bool TryGetOverride(ReferenceHub player, int hotkeyId, List<ItemBase> matches, out ItemBase result)
        {
            result = null;

            if (player == null || matches == null || matches.Count == 0) return false;

            PlayerRoles.PlayerRoleBase role = player.roleManager.CurrentRole;

            if (role == null) return false;

            Vector3Int coords = MapGeneration.RoomIdUtils.PositionToCoords(player.transform.position);

            if (_prevCoords == null || _prevCoords.Value.z != coords.z)
            {
                UpdateNearbyDoors(player.transform.position);
                _prevCoords = new Vector3Int(0, 0, coords.z);
            }

            if (!_anyNearbyDoors) return false;

            foreach (var door in NearbyDoors)
            {
                foreach (var item in matches)
                {
                    if (door.RequiredPermissions.CheckPermissions(item, player))
                    {
                        result = item;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}