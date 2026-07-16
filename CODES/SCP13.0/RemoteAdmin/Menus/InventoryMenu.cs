using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using InventorySystem.Items;
using MEC;
using TMPro;
using UnityEngine;

namespace RemoteAdmin.Menus
{
	public class InventoryMenu : RaCommandMenu
	{
		public struct PrintedItem
		{
			public GameObject GameObject { get; internal set; }

			public ItemCategory Category { get; internal set; }

			public ItemType Type { get; internal set; }

			public string Name { get; internal set; }

			public string DisplayName { get; internal set; }

			public string SerializedString { get; private set; }

			public PrintedItem(GameObject obj, ItemType itemType, ItemCategory itemCategory, string name)
			{
				GameObject = null;
				Category = default(ItemCategory);
				Type = default(ItemType);
				Name = null;
				DisplayName = null;
				SerializedString = null;
			}

			public override string ToString()
			{
				return null;
			}
		}

		[CompilerGenerated]
		private sealed class _003CSearchAnimation_003Ed__12 : IEnumerator<float>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private float _003C_003E2__current;

			public string query;

			public InventoryMenu _003C_003E4__this;

			private bool _003CisEmpty_003E5__2;

			private int _003Cindex_003E5__3;

			private List<PrintedItem>.Enumerator _003C_003E7__wrap3;

			float IEnumerator<float>.Current
			{
				[DebuggerHidden]
				get
				{
					return 0f;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			[DebuggerHidden]
			public _003CSearchAnimation_003Ed__12(int _003C_003E1__state)
			{
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void _003C_003Em__Finally1()
			{
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}
		}

		[SerializeField]
		private Transform _parent;

		[SerializeField]
		private GameObject _template;

		[SerializeField]
		private TMP_Dropdown _dropdown;

		[SerializeField]
		private TMP_InputField _itemSearchInput;

		[SerializeField]
		private float _searchAnimSpeed;

		private CoroutineHandle _searchAnimHandle;

		protected List<PrintedItem> PrintedItems { get; }

		public void ForceReorder()
		{
		}

		public void AnimatedSearch(string query)
		{
		}

		[IteratorStateMachine(typeof(_003CSearchAnimation_003Ed__12))]
		private IEnumerator<float> SearchAnimation(string query)
		{
			return null;
		}

		protected override void OnStart()
		{
		}

		private void RegisterItem(ItemType type, ItemBase itemBase, GameObject go)
		{
		}
	}
}
