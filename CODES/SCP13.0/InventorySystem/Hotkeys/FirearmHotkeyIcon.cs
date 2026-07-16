using InventorySystem.Hotkeys.Customization;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Hotkeys
{
    public class FirearmHotkeyIcon : HotkeyIconBase
    {
        [SerializeField] private RawImage _rootIcon;
        [SerializeField] private RawImage[] _attachmentsPool;
        [SerializeField] private RectTransform _transformToFit;
        [SerializeField] private float _horizontalPadding = 10f;
        [SerializeField] private Vector2 _maxSize = new Vector2(200f, 100f);

        private uint _prevAttachments;
        private ushort _prevSerial;
        private bool _refresh;

        public override bool CheckCompatibility(ItemBase item, HotkeyApperance apperance)
        {
            return item is Firearm && (int)apperance == 0;
        }

        public override void SetItem(ItemBase item)
        {
            Firearm firearm = item as Firearm;
            if (firearm == null)
            {
                if (_rootIcon != null) _rootIcon.enabled = false;
                return;
            }

            uint currentAttachments = AttachmentsUtils.GetCurrentAttachmentsCode(firearm);

            if (!_refresh && currentAttachments == _prevAttachments && _prevSerial == firearm.ItemSerial)
            {
                return;
            }

            _refresh = false;
            UpdateIcon(firearm);
        }

        public override void SetColors(Color color)
        {
            base.SetColors(color);
            _refresh = true;
        }

        private void UpdateIcon(Firearm firearm)
        {
            if (_rootIcon == null) return;

            _rootIcon.enabled = true;
            Vector2 generatedSize = FirearmIconGenerator.GenerateIcon(
                firearm,
                _rootIcon,
                _attachmentsPool,
                _maxSize,
                (int x) => Outline != null ? Outline.color : Color.white
            );

            if (_transformToFit != null)
            {
                float newWidth = generatedSize.x + _horizontalPadding * 2f;
                _transformToFit.sizeDelta = new Vector2(newWidth, _transformToFit.sizeDelta.y);
            }

            _prevSerial = firearm.ItemSerial;
            _prevAttachments = AttachmentsUtils.GetCurrentAttachmentsCode(firearm);
        }
    }
}