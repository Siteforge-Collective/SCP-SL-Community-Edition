using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class FirearmTemperatureTracker : MonoBehaviour
	{
		[Serializable]
		public struct TemperatureSettings
		{
			public AnimationCurve AdditionPerShotOverTemperature;

			public float HeatHalflife;
		}

		private class TemperatureRecord
		{
			public double Temperature;

			public Stopwatch LastRead;

			public TemperatureSettings Settings;
		}

		private static readonly Dictionary<ushort, TemperatureRecord> TemperatureOfFirearms;

		private static readonly Dictionary<ItemType, TemperatureSettings> SettingsOfFirearms;

		private static readonly TemperatureSettings DefaultSettings;

		[SerializeField]
		private TemperatureSettings _temperatureSettings;

		private void Start()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ProcessLocalFirearmShot(Firearm firearm)
		{
		}

		private static void ProcessReceivedAudio(ReferenceHub rh, ItemType it, FirearmAudioClip fac)
		{
		}

		private static void RegisterShot(ItemIdentifier shotFirearm)
		{
		}

		private static TemperatureSettings GetSettingsForFirearm(ItemType firearmType)
		{
			return default(TemperatureSettings);
		}

		private static TemperatureRecord CreateNewRecord(ItemIdentifier id)
		{
			return null;
		}

		private static TemperatureRecord GetRecord(ItemIdentifier firearm)
		{
			return null;
		}

		public static float GetTemperature(ItemIdentifier firearm)
		{
			return 0f;
		}
	}
}
