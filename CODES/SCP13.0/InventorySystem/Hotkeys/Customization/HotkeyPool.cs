using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Hotkeys.Customization
{
    public class HotkeyPool : MonoBehaviour
    {
        private static readonly float UpdateFitterLayoutThreshold = 0.05f;

        [SerializeField] private Rect _dragRect;
        [SerializeField] private Rect _deleteRect;
        [SerializeField] private float _dragLerp = 20f;
        [SerializeField] private float _heightLerp = 12f;
        [SerializeField] private PoolElementIcon _iconTemplate;
        [SerializeField] private RectTransform _dragIcon;
        [SerializeField] private RectTransform _deleteIcon;
        [SerializeField] private GameObject _addNewElementWindow;
        [SerializeField] private RectTransform _poolRt;
        [SerializeField] private ContentSizeFitter _fitter;
        [SerializeField] private VerticalLayoutGroup _layoutGroup;

        private List<PoolElementData> _elements;
        private List<PoolElementData> _prevElements = new List<PoolElementData>();
        private Dictionary<PoolElementData, Vector3> _prevPositions = new Dictionary<PoolElementData, Vector3>();
        private List<PoolElementIcon> _icons = new List<PoolElementIcon>();

        private PoolElementIcon _draggedIcon;
        private int _draggedIndex = -1;
        private Vector2 _dragOffset;
        private bool _isDragging;

        public event Action OnModified;

        public List<PoolElementData> Elements
        {
            get
            {
                _elements ??= new List<PoolElementData>();
                return _elements;
            }
        }

        private void Awake()
        {
            _dragIcon.gameObject.SetActive(false);
            _deleteIcon.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateLmbHolding();
            UpdateDragIcon();
            UpdateHeight(false);
        }

        private void LateUpdate()
        {
            if (_prevElements.Count == Elements.Count)
            {
                bool needsLerp = false;
                for (int i = 0; i < Elements.Count; i++)
                {
                    if (!Elements[i].Equals(_prevElements[i]))
                    {
                        needsLerp = true;
                        break;
                    }
                }

                if (needsLerp)
                {
                    for (int i = 0; i < _icons.Count && i < Elements.Count; i++)
                    {
                        PoolElementIcon icon = _icons[i];
                        Vector3 target = PositionAtIndex(i);
                        Vector3 current = icon.RectTransform.anchoredPosition3D;

                        if (Vector3.Distance(current, target) > 0.01f)
                        {
                            icon.RectTransform.anchoredPosition3D = Vector3.Lerp(current, target, Time.deltaTime * _dragLerp);
                        }
                        else
                        {
                            icon.RectTransform.anchoredPosition3D = target;
                        }
                    }
                }
            }
        }

        private void UpdateLmbHolding()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryStartDrag();
            }
            else if (Input.GetMouseButton(0) && _isDragging)
            {
                ContinueDrag();
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                EndDrag();
            }
        }

        private void TryStartDrag()
        {
            if (TryGetHighlightedElement(out int index, out Vector2 relativePos, true))
            {
                if (index < 0 || index >= _icons.Count) return;

                _draggedIndex = index;
                _draggedIcon = _icons[index];
                _dragOffset = relativePos;
                _isDragging = true;

                _dragIcon.gameObject.SetActive(true); 
                _dragIcon.GetComponent<RawImage>().texture = _draggedIcon.Icon.texture;
                _dragIcon.sizeDelta = _draggedIcon.RectTransform.sizeDelta;

                _draggedIcon.RectTransform.localScale = Vector3.zero;
            }
        }

        private void ContinueDrag()
        {
            Vector2 mousePos = Input.mousePosition;
            _dragIcon.position = mousePos - _dragOffset;

            bool overDelete = _deleteRect.Contains(mousePos);
            _deleteIcon.gameObject.SetActive(overDelete);

            if (TryGetHighlightedElement(out int hoverIndex, out _, false))
            {
                if (hoverIndex != _draggedIndex && hoverIndex >= 0 && hoverIndex < Elements.Count)
                {
                    PoolElementData draggedData = Elements[_draggedIndex];
                    Elements.RemoveAt(_draggedIndex);
                    Elements.Insert(hoverIndex > _draggedIndex ? hoverIndex - 1 : hoverIndex, draggedData);

                    _draggedIndex = hoverIndex > _draggedIndex ? hoverIndex - 1 : hoverIndex;

                    NotifyModified();
                    _prevElements.Clear();
                    _prevElements.AddRange(Elements);
                }
            }
        }

        private void EndDrag()
        {
            if (_isDragging)
            {
                Vector2 mousePos = Input.mousePosition;

                if (_deleteRect.Contains(mousePos) && _draggedIndex >= 0 && _draggedIndex < Elements.Count)
                {
                    RemoveElement(_draggedIndex);
                }
                else
                {
                    _draggedIcon.RectTransform.localScale = Vector3.one;
                    _prevElements.Clear();
                    _prevElements.AddRange(Elements);
                }

                _dragIcon.gameObject.SetActive(false);
                _deleteIcon.gameObject.SetActive(false);

                _isDragging = false;
                _draggedIndex = -1;
                _draggedIcon = null;
            }
        }

        private void UpdateDragIcon()
        {
            if (_isDragging && _dragIcon.gameObject.activeSelf)
            {
                // Дополнительная плавность для drag-иконки (опционально)
                // Если нужна более сложная анимация — добавьте здесь
            }
        }

        private void RemoveElement(int index)
        {
            if (index < 0 || index >= Elements.Count) return;

            Elements.RemoveAt(index);

            if (index < _icons.Count)
            {
                PoolElementIcon icon = _icons[index];
                _icons.RemoveAt(index);
                Destroy(icon.gameObject);
            }

            NotifyModified();
            UpdateLayout(true);
        }

        private void UpdateLayout(bool instant)
        {
            if (instant)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_poolRt);
            }
        }

        private void UpdateHeight(bool instant)
        {
            float targetHeight = _layoutGroup.preferredHeight + _layoutGroup.padding.vertical;
            float currentHeight = _poolRt.rect.height;

            if (Mathf.Abs(targetHeight - currentHeight) > UpdateFitterLayoutThreshold)
            {
                if (instant)
                {
                    _poolRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
                }
                else
                {
                    float lerped = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * _heightLerp);
                    _poolRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lerped);
                }
            }
        }

        private bool TryGetHighlightedElement(out int index, out Vector2 relativePos, bool clamp = false)
        {
            index = -1;
            relativePos = Vector2.zero;

            Vector2 mousePos = Input.mousePosition;

            if (!RectTransformUtility.RectangleContainsScreenPoint(_poolRt, mousePos))
                return false;

            Vector2 localPoint;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_poolRt, mousePos, null, out localPoint))
                return false;

            float elementHeight = _iconTemplate.RectTransform.rect.height + _layoutGroup.spacing;
            float y = -localPoint.y - _layoutGroup.padding.top;

            int calculatedIndex = Mathf.FloorToInt(y / elementHeight);

            if (clamp)
            {
                calculatedIndex = Mathf.Clamp(calculatedIndex, 0, Elements.Count);
            }

            if (calculatedIndex >= 0 && calculatedIndex <= Elements.Count)
            {
                index = calculatedIndex;
                float offsetY = y - calculatedIndex * elementHeight;
                relativePos = new Vector2(localPoint.x, -offsetY);
                return true;
            }

            return false;
        }

        private Vector3 PositionAtIndex(int index)
        {
            float elementHeight = _iconTemplate.RectTransform.rect.height + _layoutGroup.spacing;
            float y = -(index * elementHeight + _layoutGroup.padding.top);
            return new Vector3(0, y, 0);
        }

        public void NotifyModified()
        {
            OnModified?.Invoke();
        }
    }
}