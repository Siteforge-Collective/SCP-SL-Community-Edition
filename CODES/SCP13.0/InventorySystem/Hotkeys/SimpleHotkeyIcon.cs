using InventorySystem.Hotkeys.Customization;
using InventorySystem.Items;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Hotkeys
{
    public class SimpleHotkeyIcon : HotkeyIconBase
    {
        [SerializeField]
        private RawImage _icon;

        [SerializeField]
        private HotkeyApperance _size;

        public override bool CheckCompatibility(ItemBase item, HotkeyApperance apperance)
        {
            return apperance == this._size;
        }

        public override void SetItem(ItemBase item)
        {
            if (item == null)
            {
                if (_icon != null) _icon.enabled = false;
                return;
            }

            if (_icon != null)
            {
                _icon.enabled = true;
                _icon.texture = item.Icon;
                _icon.color = Color.white;
            }
        }
    }
}