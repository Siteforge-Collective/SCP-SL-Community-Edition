using RemoteAdmin.Elements;
using RemoteAdmin.Presets;
using TMPro;
using UnityEngine;

namespace RemoteAdmin.Menus
{
	public class MapControlMenu : RaCommandMenu
	{
		private const string DefaultOverchargeValue = "3600";

		[SerializeField]
		internal ColorPreset Closed;

		[SerializeField]
		internal ColorPreset Opened;

		[SerializeField]
		internal ColorPreset Moving;

		[SerializeField]
		internal ColorPreset Unselected;

		[SerializeField]
		internal ColorPreset Selected;

		[SerializeField]
		internal ColorPreset LockedUnselected;

		[SerializeField]
		internal ColorPreset LockedSelected;

		[SerializeField]
		private RaDoorButton _doorButtonTemplate;

		[SerializeField]
		private RaElevatorButton _elevatorButtonTemplate;

		[SerializeField]
		private Transform _rootParent;

		[SerializeField]
		private Transform _elevatorRootParent;

		[SerializeField]
		private TMP_Dropdown _zoneDropdown;

		[SerializeField]
		private TMP_InputField _overchargeDuration;

		[SerializeField]
		private TMP_InputField _lockdownDuration;

		[SerializeField]
		private TMP_InputField _cleanupAmount;

		private float _updateTimer;

		public void RefreshButtons()
		{
		}

		public void FilterDoors()
		{
		}

		protected override string BuildCommand(string command, string format)
		{
			return null;
		}

		protected override void OnUpdate()
		{
		}

		protected override void OnStart()
		{
		}
	}
}
