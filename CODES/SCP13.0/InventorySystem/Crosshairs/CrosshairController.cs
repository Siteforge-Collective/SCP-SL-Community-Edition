using InventorySystem.Items;
using PlayerRoles;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Crosshairs
{
    public class CrosshairController : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _defaultCrosshair;
        [SerializeField] private MonoBehaviour[] _customCrosshairs;
        [SerializeField] private GameObject _rootObject;
        [SerializeField] private CanvasScaler _canvasScaler;

        private static CrosshairController _singleton;

        private bool IsLowResolution => Screen.height < 720;

        private void Start()
        {
            _singleton = this;
            Inventory.OnCurrentItemChanged += OnItemChanged;
        }

        private void OnDestroy()
        {
            Inventory.OnCurrentItemChanged -= OnItemChanged;
        }

        private void Update()
        {
            bool showCrosshair = false;

            if (ReferenceHub.TryGetLocalHub(out ReferenceHub localHub))
            {
                PlayerRoleBase currentRole = localHub.roleManager?.CurrentRole;
                if (currentRole != null && PlayerRolesUtils.IsAlive(currentRole.RoleTypeId))
                    showCrosshair = !Cursor.visible;
            }

            if (_rootObject != null)
                _rootObject.SetActive(showCrosshair);

            if (_canvasScaler != null)
            {
                _canvasScaler.uiScaleMode = IsLowResolution
                    ? CanvasScaler.ScaleMode.ScaleWithScreenSize
                    : CanvasScaler.ScaleMode.ConstantPixelSize;
            }
        }

        private static void OnItemChanged(ReferenceHub ply, ItemIdentifier prevItem, ItemIdentifier newItem)
        {
            if (!ply.isLocalPlayer || _singleton == null) return;

            Inventory inventory = ply.inventory;
            if (inventory == null || inventory.UserInventory == null) return;

            ItemBase currentItem = null;
            bool hasItem = newItem.SerialNumber != 0 &&
                           inventory.UserInventory.Items.TryGetValue(newItem.SerialNumber, out currentItem) &&
                           currentItem != null;

            foreach (MonoBehaviour custom in _singleton._customCrosshairs)
            {
                if (custom != null)
                    custom.gameObject.SetActive(false);
            }

            if (!hasItem)
            {
                if (_singleton._defaultCrosshair != null)
                    _singleton._defaultCrosshair.gameObject.SetActive(true);
                return;
            }

            bool found = false;
            foreach (MonoBehaviour custom in _singleton._customCrosshairs)
            {
                if (custom == null) continue;

                if (custom.GetType().IsAssignableFrom(currentItem.GetType()) ||
                    currentItem.GetType().IsAssignableFrom(custom.GetType()))
                {
                    custom.gameObject.SetActive(true);
                    found = true;
                    break;
                }
            }

            if (!found && _singleton._defaultCrosshair != null)
                _singleton._defaultCrosshair.gameObject.SetActive(true);
        }
    }
}