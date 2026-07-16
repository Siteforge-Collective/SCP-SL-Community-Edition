using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class AttachmentSummaryPrinter : MonoBehaviour
	{
		[Serializable]
		private readonly struct ComparableConfiguration
		{
			public readonly string ConfigurationName;

			public readonly uint Code;

			public ComparableConfiguration(ItemType weapon, string label, uint code)
			{
				ConfigurationName = null;
				Code = 0u;
			}
		}

		[SerializeField]
		private AttachmentSelectorBase _selectorReference;

		[SerializeField]
		private AttachmentSummaryEntry _tableHeader;

		[SerializeField]
		private AttachmentSummaryEntry _entryTemplate;

		[SerializeField]
		private int _displayedPresets;

		[SerializeField]
		private string _goodColor;

		[SerializeField]
		private string _badColor;

		[SerializeField]
		private Color _oddEntryColor;

		private Firearm _prevFirearm;

		private readonly Queue<AttachmentSummaryEntry> _entryPool;

		private readonly HashSet<AttachmentSummaryEntry> _spawnedEntires;

		private readonly HashSet<uint> _displayedCodes;

		private readonly List<ComparableConfiguration> _displayedConfigurations;

		private Firearm Firearm => null;

		private void OnEnable()
		{
		}

		private void Update()
		{
		}

		private void Refresh()
		{
		}

		private KeyValuePair<string, string>[] GenerateConfigurationData(ComparableConfiguration conf)
		{
			return null;
		}

		private void ExtractNonparams(out float runningInaccuracy, out float length, out float weight, out float baseAdsRecoil)
		{
			runningInaccuracy = default(float);
			length = default(float);
			weight = default(float);
			baseAdsRecoil = default(float);
		}

		private KeyValuePair<string, string> GetStatValuePair(float defaultValue, AttachmentParam param, string unit = "", bool processValue = true, float rounding = 100f)
		{
			return default(KeyValuePair<string, string>);
		}

		private KeyValuePair<string, string> GetCustomValuePair(string label, float baseValue, float otherValue, string unit, float mainAccuracy, float diffAccuracy, bool asPercent, bool posBad, bool inverse = false)
		{
			return default(KeyValuePair<string, string>);
		}

		private bool TryGetFormatted(AttachmentParam param, out string str)
		{
			str = null;
			return false;
		}

		private void PrepareConfigurations(uint initial)
		{
		}

		private void AddConfiguration(uint attachmentCode, string label, bool forceAdd = false)
		{
		}
	}
}
