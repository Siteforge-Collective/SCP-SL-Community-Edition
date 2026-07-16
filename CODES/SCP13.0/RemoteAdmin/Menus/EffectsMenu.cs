using System.Collections.Generic;
using CustomPlayerEffects;
using RemoteAdmin.Elements;
using RemoteAdmin.Interfaces;
using UnityEngine;

namespace RemoteAdmin.Menus
{
	public class EffectsMenu : RaCommandMenu
	{
		private readonly List<RaEffectButton> _effectButtons;

		[SerializeField]
		private RaEffectButton _buttonTemplate;

		[SerializeField]
		private Transform _rootParent;

		[SerializeField]
		private Color _negativeColor;

		[SerializeField]
		private Color _positiveColor;

		[SerializeField]
		private Color _mixedColor;

		public void ResetOptions()
		{
		}

		protected override void OnStart()
		{
		}

		private bool IsCustomDisplay(StatusEffectBase statusEffect, out ICustomRADisplay display)
		{
			display = null;
			return false;
		}

		private string BuildName(string objectName)
		{
			return null;
		}
	}
}
