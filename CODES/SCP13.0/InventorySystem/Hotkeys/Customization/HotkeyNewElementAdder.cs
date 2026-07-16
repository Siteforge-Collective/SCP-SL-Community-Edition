using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Hotkeys.Customization
{
	public class HotkeyNewElementAdder : MonoBehaviour
	{
		private struct ItemElement
		{
			private readonly GameObject _go;

			private readonly TMP_Text _text;

			private readonly RawImage _icon;

			private readonly Button _button;

			private readonly Texture _defIcon;

			public PoolElementData Data { get; private set; }

			public ItemElement(GameObject newInst, Texture defaultIcon)
			{
				_go = null;
				_text = null;
				_icon = null;
				_button = null;
				_defIcon = null;
				Data = default(PoolElementData);
			}

			public readonly void Disable()
			{
			}

			public bool TryEnable(PoolElementData data, TMP_InputField filter, Action<PoolElementData> callback)
			{
				return false;
			}

			private readonly bool CheckFilter(string filter)
			{
				return false;
			}
		}

		[SerializeField]
		private RectTransform _elementTemplate;

		[SerializeField]
		private Texture _defaultIcon;

		[SerializeField]
		private TMP_InputField _searchBar;

		[SerializeField]
		private PoolElementData.ElementType[] _types;

		[SerializeField]
		private HotkeyPool _pool;

		private int _elementClock;

		private Canvas _canvas;

		private readonly List<ItemElement> _elementPool;

		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Update()
		{
		}

		private void OnOptionSelected(PoolElementData data)
		{
		}

		private void UpdateList()
		{
		}

		private void AddElementsOfType(PoolElementData.ElementType type)
		{
		}

		private void AddAllGroups()
		{
		}

		private void AddAllSpecificItems()
		{
		}

		private void AddAllOrders()
		{
		}

		private void AddElement(PoolElementData data)
		{
		}
	}
}
