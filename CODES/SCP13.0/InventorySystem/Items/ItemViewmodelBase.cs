using System;
using UnityEngine;

namespace InventorySystem.Items
{
    public class ItemViewmodelBase : MonoBehaviour
    {
        private bool _idSet;

        private ItemIdentifier _itemId;

        public virtual float ViewmodelCameraFOV => 60f;

        public ItemBase ParentItem { get; protected set; }

        public ItemIdentifier ItemId
        {
            get
            {
                if (!_idSet)
                {
                    if (IsLocal)
                    {
                        if (ParentItem != null && ParentItem.ItemSerial != 0)
                        {
                            _idSet = true;
                            _itemId = new ItemIdentifier(ParentItem.ItemTypeId, ParentItem.ItemSerial);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("ItemId could not be set.");
                    }
                }
                return _itemId;
            }
        }

        public ReferenceHub Hub { get; private set; }

        public bool IsLocal { get; private set; }

        public bool IsSpectator { get; private set; }

        public static event Action<ItemViewmodelBase> OnLocallyInitialized;

        public static event Action<ItemViewmodelBase> OnSpectatorInitialized;

        public static event Action<ItemViewmodelBase> OnAnyInitialized;

        public virtual void InitLocal(ItemBase ib)
        {
            Hub = ib.Owner;
            ParentItem = ib;
            IsLocal = true;
            IsSpectator = false;

            OnLocallyInitialized?.Invoke(this);
            InitAny();
        }

        public virtual void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
        {
            Hub = ply;
            IsLocal = false;
            IsSpectator = true;

            _itemId = id;
            _idSet = true;

            OnSpectatorInitialized?.Invoke(this);
            InitAny();
        }

        public virtual void InitAny()
        {
            OnAnyInitialized?.Invoke(this);
        }

        internal virtual void OnEquipped()
        {
        }
    }
}