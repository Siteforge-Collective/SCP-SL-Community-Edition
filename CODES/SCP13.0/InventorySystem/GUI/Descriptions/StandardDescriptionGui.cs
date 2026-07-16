using InventorySystem.Items;
using TMPro;
using UnityEngine;

namespace InventorySystem.GUI.Descriptions
{
    public class StandardDescriptionGui : RadialDescriptionBase
    {
        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _desc;

        public override void UpdateInfo(ItemBase targetItem, Color roleColor)
        {
            if (targetItem == null)
                return;

            _title.text = targetItem.ToString();

            string[] info = new string[7];
            info[0] = "Item '";
            info[1] = targetItem.ItemTypeId.ToString();
            info[2] = "' of the class '";
            info[3] = targetItem.GetType().FullName;
            info[4] = "' does have an implementation of the 'IItemDescription' interface, which is required by items of the '";
            info[5] = targetItem.Category.ToString();
            info[6] = "' category.";

            _desc.text = string.Concat(info);
        }
    }
}