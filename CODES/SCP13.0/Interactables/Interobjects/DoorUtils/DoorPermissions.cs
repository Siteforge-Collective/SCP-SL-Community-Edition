using System;
using InventorySystem.Items;
using InventorySystem.Items.Keycards;
using PlayerRoles;

namespace Interactables.Interobjects.DoorUtils
{
    [Serializable]
    public class DoorPermissions
    {
        public KeycardPermissions RequiredPermissions;

        public bool RequireAll;

        public bool Bypass2176;

        public bool CheckPermissions(ItemBase item, ReferenceHub ply)
        {
            if (RequiredPermissions == KeycardPermissions.None)
            {
                return true;
            }
            if (ply != null)
            {
                if (ply.serverRoles.BypassMode)
                {
                    return true;
                }
                if (item == null)
                {
                    if (ply.IsSCP())
                    {
                        return RequiredPermissions.HasFlagFast(KeycardPermissions.ScpOverride);
                    }
                    return false;
                }
            }
            if (item is KeycardItem keycardItem)
            {
                if (!RequireAll)
                {
                    return (keycardItem.Permissions & RequiredPermissions) != 0;
                }
                return (keycardItem.Permissions & RequiredPermissions) == RequiredPermissions;
            }
            return false;
        }
    }
}
