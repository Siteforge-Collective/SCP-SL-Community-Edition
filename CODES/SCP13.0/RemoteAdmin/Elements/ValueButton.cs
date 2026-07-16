using RemoteAdmin.Interfaces;
using UnityEngine;

namespace RemoteAdmin.Elements
{
	public class ValueButton : CustomButton, IValueHolder<string>
	{
		[SerializeField]
		protected bool AllowMultipleSelection;

		[SerializeField]
		[Tooltip("If any other buttons have the same Choice ID as this one, they will be toggled off when this button is selected.")]
		private int _choiceIdentifier;

		[SerializeField]
		private string _value;

		public string Value
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int ChoiceId
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public override void SetState(bool isSelected)
		{
		}

		protected override void OnInitialize()
		{
		}

		private bool CanSelectMultiple()
		{
			return false;
		}
	}
}
