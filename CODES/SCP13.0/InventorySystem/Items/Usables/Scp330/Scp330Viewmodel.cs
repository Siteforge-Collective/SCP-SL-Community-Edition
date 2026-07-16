using System;
using System.Collections.Generic;
using CursorManagement;
using CustomPlayerEffects;
using InventorySystem.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Usables.Scp330
{
    public class Scp330Viewmodel : UsableItemViewmodel, ICursorOverride
    {
        [Serializable]
        private struct CandyObject
        {
            public CandyKindID KindID;
            public GameObject HandObject;
            public Texture Icon;
            public AudioClip EatingSound;
        }

        [SerializeField] private RadialInventory _selector;
        [SerializeField] private RawImage[] _selectorSlots;
        [SerializeField] private CanvasGroup _selectorGroup;
        [SerializeField] private CanvasGroup _descriptionGroup;
        [SerializeField] private CandyObject[] _candies;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _effects;

        private Scp330Bag _bag;
        private bool _openDelay;
        private bool _cancelled;
        private CandyKindID _displayedCandy;
        private float _originalPitch = 1f;

        public CursorOverrideMode CursorOverride { get; private set; }
        public bool LockMovement => false;

        public override void InitLocal(ItemBase parent)
        {
            base.InitLocal(parent);
            _bag = parent as Scp330Bag;
        }

        public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
        {
            Scp330NetworkHandler.OnClientSelectMessageReceived += HandleSelectMessage;
            base.InitSpectator(ply, id, wasEquipped);
        }

        internal override void OnEquipped()
        {
            _descriptionGroup.alpha = 0f;
            _openDelay = true;
            _displayedCandy = CandyKindID.None;
            _selectorGroup.gameObject.SetActive(false);

            CursorOverride = CursorOverrideMode.NoOverride;
            CursorManager.Register(this);
            base.OnEquipped();
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || (InventoryGuiController.InventoryVisible && _selectorGroup.gameObject.activeSelf))
            {
                CancelSelector(true);
                return;
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (_bag == null || _bag.Candies == null) return;

            int selId = _bag.SelectedCandyId;
            if (selId >= 0 && selId < _bag.Candies.Count)
                SetCandyModel(_bag.Candies[selId]);
            else
                SetCandyModel(CandyKindID.None);

            for (int i = 0; i < _selector.OrganizedContent.Length; i++)
            {
                _selector.OrganizedContent[i] = (i < _bag.Candies.Count) ? (ushort)i : (ushort)0;
            }

            ushort selectedIdx;
            InventoryGuiAction action = _selector.DisplayAndSelectItems(null, out selectedIdx);

            if (action == InventoryGuiAction.Select)
            {
                _bag.SelectCandy(selectedIdx);
            }
            else if (action == InventoryGuiAction.Drop)
            {
                if (!Hub.playerEffectsController.GetEffect<AmnesiaItems>().IsEnabled)
                {
                    _bag.DropCandy(selectedIdx);
                }
            }

            int hovered = _selector.HighlightedSlot;
            if (hovered >= 0 && hovered < _bag.Candies.Count)
            {
                DisplayDescriptions(_bag.Candies[hovered]);
            }
            else
            {
                DisplayDescriptions(CandyKindID.None);
            }

            _selectorGroup.alpha = Mathf.MoveTowards(_selectorGroup.alpha, _selectorGroup.gameObject.activeSelf ? 1f : 0f, Time.deltaTime * 10f);

            if (_openDelay) _openDelay = false;
        }

        private void HandleSelectMessage(SelectScp330Message msg)
        {
            if (_bag == null || base.ItemId.SerialNumber != msg.Serial) return;
            SetCandyModel((CandyKindID)msg.CandyID);
            OnUsingStarted();
        }

        private void OnDisable()
        {
            Scp330NetworkHandler.OnClientSelectMessageReceived -= HandleSelectMessage;
            CursorManager.Unregister(this);
        }

        private void SetCandyModel(CandyKindID id)
        {
            foreach (var candy in _candies)
            {
                if (candy.HandObject != null)
                    candy.HandObject.SetActive(candy.KindID == id);
            }
        }

        private void DisplayDescriptions(CandyKindID candy)
        {
            if (candy == CandyKindID.None)
            {
                _descriptionGroup.alpha = Mathf.MoveTowards(_descriptionGroup.alpha, 0f, Time.deltaTime * 10f);
                return;
            }

            if (candy != _displayedCandy)
            {
                _title.text = TranslationReader.Get("SCP330", 0, candy.ToString());
                _description.text = TranslationReader.Get("SCP330", 1, "Description");
                _effects.text = TranslationReader.Get("SCP330", 2, "Effects");
                _displayedCandy = candy;
            }

            _descriptionGroup.alpha = Mathf.MoveTowards(_descriptionGroup.alpha, 1f, Time.deltaTime * 10f);
        }

        private bool TryGetCandyObject(CandyKindID id, out CandyObject val)
        {
            for (int i = 0; i < _candies.Length; i++)
            {
                if (_candies[i].KindID == id)
                {
                    val = _candies[i];
                    return true;
                }
            }
            val = default;
            return false;
        }

        private void CancelSelector(bool bringBackInventory = false)
        {
            _cancelled = true;
            if (_bag != null)
            {
                _bag.OwnerInventory.CmdSelectItem(0);
            }
            InventoryGuiController.InventoryVisible = bringBackInventory;
            _selectorGroup.gameObject.SetActive(false);
        }
    }
}