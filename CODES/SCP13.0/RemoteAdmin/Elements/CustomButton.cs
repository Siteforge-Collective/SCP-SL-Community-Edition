using RemoteAdmin.Interfaces;
using RemoteAdmin.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Elements
{
	public abstract class CustomButton : MonoBehaviour, ISelectableElement
	{
		[Header("-- Tools --")]
		[SerializeField]
		[Tooltip("When toggled, it'll search for any Outline, Builder and more components to initialize the properties shown above.")]
		private bool _magicButton;

		[SerializeField]
		[Header("-- Properties --")]
		private string _selectedText;

		[SerializeField]
		private string _unselectedText;

		[SerializeField]
		private bool _modifyText;

		private bool _isSelected;

		private string _oldText;

		public bool IsSelected
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		[field: Tooltip("If set, it will change the button's outline color to the pre-defined one.")]
		[field: SerializeField]
		public TMP_Text Text { get; set; }

		[field: SerializeField]
		[field: Tooltip("If set, it will change the component's text to whatever \"SelectedText\" is set to.")]
		public Outline Outline { get; set; }

		[field: SerializeField]
		[field: Header("-- References --")]
		[field: Tooltip("If left empty, it will look for the component inside the GameObject.")]
		public RaCommandMenu CommandMenu { get; set; }

		public virtual void Select()
		{
		}

		public virtual void SetState(bool isSelected)
		{
		}

		protected virtual void OnInitialize()
		{
		}
	}
}
