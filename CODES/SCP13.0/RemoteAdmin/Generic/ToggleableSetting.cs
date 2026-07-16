using System;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Generic
{
	[Serializable]
	public abstract class ToggleableSetting : RaSetting<bool>
	{
		public override bool Value
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		[field: SerializeField]
		public Button RepresentingButton { get; set; }

		public void Toggle()
		{
		}

		protected override void OnSave()
		{
		}

		protected override void OnLoad()
		{
		}

		protected virtual void SetButton(bool value)
		{
		}
	}
}
