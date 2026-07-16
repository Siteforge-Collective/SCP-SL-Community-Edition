using System;
using System.Runtime.CompilerServices;
using RemoteAdmin.Settings;
using TMPro;
using UnityEngine;

namespace RemoteAdmin.Menus
{
	public class RaSettings : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _display;

		public static RaSettings Singleton { get; private set; }

		[field: SerializeField]
		public ToggleMovementSetting ToggleMovement { get; set; }

		[field: SerializeField]
		public ToggleItemOrderSetting ToggleItemOrder { get; set; }

		[field: SerializeField]
		public ToggleListOrderSetting ToggleListOrder { get; set; }

		[field: SerializeField]
		public ToggleSuggestionsSetting ToggleSuggestions { get; set; }

		[field: SerializeField]
		public ToggleTimestampsSetting ToggleTimestamps { get; set; }

		[field: SerializeField]
		public BandwidthCooldownSetting BandwidthCooldown { get; set; }

		[field: SerializeField]
		public PlayerListSortingSetting PlayerListSorting { get; set; }

		[field: SerializeField]
		public ToggleSpawnpointSetting ToggleSpawnpoint { get; set; }

		[field: SerializeField]
		public ToggleResetInventorySetting ToggleResetInventory { get; set; }

		[field: SerializeField]
		public ToggleTooltipSetting ToggleTooltip { get; set; }

		[field: SerializeField]
		public TooltipHideDelaySetting HideTooltipDelay { get; set; }

		[field: SerializeField]
		public TooltipShowDelaySetting ShowTooltipDelay { get; set; }

		[field: SerializeField]
		public RedWindowSetting WindowRed { get; set; }

		[field: SerializeField]
		public GreenWindowSetting WindowGreen { get; set; }

		[field: SerializeField]
		public BlueWindowSetting WindowBlue { get; set; }

		[field: SerializeField]
		public AlphaWindowSetting WindowAlpha { get; set; }

		public static event Action OnLoad
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public static event Action OnReset
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public static event Action OnSave
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Save(bool updateDisplay = false)
		{
		}

		public void Load(bool updateDisplay = false)
		{
		}

		public void ForceReset(bool updateDisplay = false)
		{
		}

		public void ToggleListOrdering()
		{
		}

		public void ToggleItemOrdering()
		{
		}

		public void RefreshTooltipShowDelay()
		{
		}

		public void RefreshTooltipHideDelay()
		{
		}

		public void RefreshTimestampsToggle()
		{
		}

		public void RefreshBandwithCooldown()
		{
		}

		public void RefreshMovementToggle()
		{
		}

		public void RefreshTooltipsToggle()
		{
		}

		public void RefreshSuggestionsToggle()
		{
		}

		public void RefreshSpawnpointToggle()
		{
		}

		public void RefreshResetInventoryToggle()
		{
		}

		private void Awake()
		{
		}

		private void Start()
		{
		}
	}
}
