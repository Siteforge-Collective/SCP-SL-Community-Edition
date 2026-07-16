using TMPro;
using UnityEngine;

namespace RemoteAdmin.Elements
{
	public class RaEffectButton : SendButton
	{
		private const string DefaultIntensity = "1";

		private const string DefaultDuration = "0";

		private const string ClearValue = "0";

		[SerializeField]
		private TMP_InputField _durationField;

		[SerializeField]
		private TMP_InputField _intensityField;

		[SerializeField]
		private SendButton _clearButton;

		public string EffectId { get; set; }

		public string EffectName { get; set; }

		public string Duration => null;

		public string Intensity => null;

		public void Setup()
		{
		}

		public void ResetFields()
		{
		}

		protected override void SendCommand(string command, string format)
		{
		}
	}
}
